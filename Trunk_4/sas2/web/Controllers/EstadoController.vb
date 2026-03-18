Imports System.Web.Mvc

Namespace Controllers
    Public Class EstadoController
        Inherits Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString
        <SimpleRoleProvider(Role.creacion)>
        Public Function ListMovimientosSinAsignarAgrupado(fecha As Long?) As ActionResult
            Dim lkExpressionMovimiento = LinqKit.PredicateBuilder.[New](Of VMMovimientoFinal)(True)
            Dim lkExpressionRecogida = LinqKit.PredicateBuilder.[New](Of VMRecogidaFinal)(True)

            Dim predicate As Func(Of VMMovimientoFinal, Boolean) = Function(mf) True
            If fecha.HasValue Then
                lkExpressionMovimiento = lkExpressionMovimiento.And(Function(mf) mf.Fecha.Ticks = fecha)
                lkExpressionRecogida = lkExpressionRecogida.And(Function(rf) rf.Fecha.Ticks = fecha)
            End If

            Dim mr As New VMMovimientoSinAsignar
            mr.LstMovimientoProveedorFecha = DBAccess.GetListMovimientosSinAgrupacion(strCn).OrderByDescending(Function(o) o.Fecha).Where(lkExpressionMovimiento)

            Return View(mr)
        End Function

        <SimpleRoleProvider(Role.creacion)>
        Public Function ListMovimientosSinAsignar(fecha As Long?) As ActionResult
            Dim lkExpressionMovimiento = LinqKit.PredicateBuilder.[New](Of VMMovimientoFinal)(True)
            Dim lkExpressionRecogida = LinqKit.PredicateBuilder.[New](Of VMRecogidaFinal)(True)

            Dim predicate As Func(Of VMMovimientoFinal, Boolean) = Function(mf) True
            If fecha.HasValue Then
                lkExpressionMovimiento = lkExpressionMovimiento.And(Function(mf) mf.Fecha.Ticks = fecha)
                lkExpressionRecogida = lkExpressionRecogida.And(Function(rf) rf.Fecha.Ticks = fecha)
            End If
            Dim mr As New VMMovimientoSinAsignar
            mr.LstMovimientoProveedorFecha = DBAccess.GetListMovimientosSinAgrupacion(strCn).OrderByDescending(Function(o) o.Fecha).Where(lkExpressionMovimiento)
            Return View(mr)
            'Return View(l.Where(h.searchFilter(Of Movimiento)(filterValue, Function(m) m.)))
        End Function

        <SimpleRoleProvider(Role.creacion)>
        Function ListMovimientoIndividuales(ovmMovimiento As VMMovimientoFinal, IdEmpresaOrigen As Integer?, IdEmpresaDestino As Integer?, IdHelbideOrigen As Integer?, IdHelbideDestino As Integer?, fechaTicks As Long?)
            ovmMovimiento.VectorMovimiento = New VMVectorViaje With {.PuntoOrigen = New VMPuntoViaje With {.IdEmpresa = IdEmpresaOrigen, .IdHelbide = IdHelbideOrigen},
                .PuntoDestino = New VMPuntoViaje With {.IdEmpresa = IdEmpresaDestino, .IdHelbide = IdHelbideDestino}}

            Dim lkExpressionMovimiento = LinqKit.PredicateBuilder.[New](Of VMMovimientoFinal)(True)

            Dim predicate As Func(Of VMMovimientoFinal, Boolean) = Function(mf) True
            lkExpressionMovimiento = lkExpressionMovimiento.And(Function(mf) mf.Numope = ovmMovimiento.Numope And mf.Numord = ovmMovimiento.Numord And mf.Fecha.Ticks = fechaTicks)




            Return View(DBAccess.GetListMovimientosSinAgrupacion(strCn).OrderByDescending(Function(o) o.Fecha).Where(lkExpressionMovimiento).First)
        End Function
        <SimpleRoleProvider(Role.creacion)>
        Function ListBultosSinAsignar() As ActionResult
            Return View(New VMAgrupacionSinAsignar With {.ListOfAgrupacion = DBAccess.GetListOfAgrupacionSinAsignar(strCn)})
        End Function

        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function AgruparMovimiento(ovmMovimientoRecogida As VMMovimientoRecogida)
            Dim lstMarcaSeleccionada = ovmMovimientoRecogida.LstMovimientoProveedorFecha.SelectMany(Function(m) m.ListOfMarca.Select(Function(m2) m2)).Where(Function(m) m.Seleccionado).Select(Function(m) m.Id)
            Dim MovimientosSeleccionados = DBAccess.GetListMovimientosSinAgrupacion(strCn).OrderByDescending(Function(o) o.Fecha).Where(Function(m) m.ListOfMarca.Any(Function(m2) lstMarcaSeleccionada.Contains(m2.Id)))

            For Each mv In ModelState.Values
                mv.Errors.Clear()
            Next
            'Dim MovimientosSeleccionados = ovmMovimientoRecogida.LstMovimientoProveedorFecha.Where(Function(m) m.ListOfMarca.Any(Function(m2) m2.Seleccionado))
            'Asegurarse de que el origen, destino y fecha coinciden en todas las seleccionadas
            Dim grupoOrigenDestinoFecha = MovimientosSeleccionados.GroupBy(Function(m) New With {Key .fecha = m.Fecha, Key .idEmpresaOrigen = m.VectorMovimiento.PuntoOrigen.IdEmpresa, Key .idHelbideOrigen = m.VectorMovimiento.PuntoOrigen.IdHelbide,
                Key .idEmpresaDestino = m.VectorMovimiento.PuntoDestino.IdEmpresa, Key .idHelbideDestino = m.VectorMovimiento.PuntoDestino.IdHelbide})
            If grupoOrigenDestinoFecha.Count <> 1 Then
                ModelState.AddModelError("", h.traducir("No coinciden las fechas o direcciones de envio"))
            End If

            'Dim marcasSeleccionadas = ovmMovimientoRecogida.LstMovimientoProveedorFecha.SelectMany(Function(m) m.ListOfMarca).Where(Function(m) m.Seleccionado)
            Dim oAgruparMovimiento As New VMAgruparMovimiento With {.LstMovimientoProveedorFecha = MovimientosSeleccionados.Select(Function(m)
                                                                                                                                       m.ListOfMarca = m.ListOfMarca.Where(Function(m2) lstMarcaSeleccionada.Contains(m2.Id)).ToList
                                                                                                                                       Return m
                                                                                                                                   End Function)}
            If ModelState.IsValid Then
                Return View(oAgruparMovimiento)
            End If
            Return RedirectToAction("ListMovimientosRecogidasSinAsignar", h.ToRouteValues(Request.QueryString, Nothing))
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function AgruparMovimientoSave(ovmAgruparMovimiento As VMAgruparMovimiento)
            If ModelState.IsValid Then
                DBAccess.SaveAgrupacion(ovmAgruparMovimiento.LstMovimientoProveedorFecha.SelectMany(Function(m) m.ListOfMarca.Select(Function(m2) m2.Id)).ToList, ovmAgruparMovimiento.Peso, strCn)
                Return RedirectToAction("ListBultosSinAsignar")
            End If
            Return View("AgruparMovimiento", ovmAgruparMovimiento)
        End Function


        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function CrearAlbaran(ovmAgrupacionSinAsignar As VMAgrupacionSinAsignar)
            'TODO:
            'Ensure same source and destination
            ' DBAccess.GetMovimiento(ovmAgrupacionSinAsignar.ListOfAgrupacion.First)
            ' DBAccess.getli
            Dim v = ovmAgrupacionSinAsignar.ListOfAgrupacion.First.ListOfMovimiento.First
            Return View(New VMCreateAlbaran With {.LstBulto = ovmAgrupacionSinAsignar})
        End Function

        <SimpleRoleProvider(Role.creacion)>
        Public Function AddMovimientoStep1() As ActionResult
            Dim empresaOrigen = DBAccess.GetProveedorConDireccion(1, strCn)
            Dim vMovimiento = New VMVectorViaje With {.PuntoOrigen = New VMPuntoViaje With {.IdEmpresa = CType(empresaOrigen.id, Integer?), .TxtIdEmpresa = empresaOrigen.nombre}}
            Return View(New VMMovimientoStep1 With {.Fecha = Now.Date,
                .VectorMovimiento = vMovimiento,
                         .ListOfNumope = New List(Of SelectListItem) From {}})
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Public Function AddMovimientoStep1(oVMMovimientoStep1 As VMMovimientoStep1) As ActionResult
            If ModelState.IsValid Then
                Dim vmFinal = New VMMovimientoFinal(oVMMovimientoStep1)
                vmFinal.ListOfMarca = DBAccess.GetListVMMarcas(vmFinal.Numord, vmFinal.Numope, strCn)
                vmFinal.ListOfMarca.Add(New VMMarca With {.Marca = "ZZZZ", .Peso = 1})
                Return View("AddMovimientoStep2", vmFinal)
            End If
            If oVMMovimientoStep1.Numord.HasValue Then
                oVMMovimientoStep1.ListOfNumope = DBAccess.GetListNumOpActivas(oVMMovimientoStep1.Numord, strCn).Select(Function(op) New SelectListItem With {.Value = op.numope, .Text = op.numope.ToString + " - " + op.Descripcion})
            Else
                oVMMovimientoStep1.ListOfNumope = New List(Of SelectListItem) From {}
            End If
            Return View(oVMMovimientoStep1)
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Public Function AddMovimientoStep2(oVMMovimientoFinal As VMMovimientoFinal, trick As String) As ActionResult
            If ModelState.IsValid Then
                DBAccess.SaveVMMovimientosMaterial(oVMMovimientoFinal, SimpleRoleProvider.GetId(), strCn)
                Return RedirectToAction("ListMovimientosRecogidasSinAsignar")
            End If
            Return View(oVMMovimientoFinal)
        End Function

        <SimpleRoleProvider(Role.creacion)>
        Public Function AddRecogidaCabecera() As ActionResult
            Dim empresaDestino = DBAccess.GetProveedorConDireccion(1, strCn)
            Dim vRecogida = New VMVectorViaje With {.PuntoDestino = New VMPuntoViaje With {.IdEmpresa = CType(empresaDestino.id, Integer?), .TxtIdEmpresa = empresaDestino.nombre}}
            Return View(New VMRecogidaCabecera With {.Fecha = Now.Date, .VectorRecogida = vRecogida})
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Public Function AddRecogidaCabecera(oVMRecogidaCabecera As VMRecogidaCabecera) As ActionResult
            If ModelState.IsValid Then
                oVMRecogidaCabecera.idSab = SimpleRoleProvider.GetId()
                DBAccess.SaveCabeceraRecogida(oVMRecogidaCabecera, strCn)
                Return RedirectToAction("AddRecogidaLinea", oVMRecogidaCabecera)
            End If
            Return View(oVMRecogidaCabecera)
        End Function

        <SimpleRoleProvider(Role.creacion)>
        Public Function AddRecogidaLinea(oVMRecogidaFinal As VMRecogidaFinal) As ActionResult
            oVMRecogidaFinal = DBAccess.GetRecogidaFinal(oVMRecogidaFinal.id, strCn)
            Return View("AddRecogidaLinea", oVMRecogidaFinal)
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Public Function InsertRecogidaLinea(oVMRecogidaFinal As VMRecogidaFinal) As ActionResult
            If ModelState.IsValid Then
                DBAccess.saveLineaRecogida(oVMRecogidaFinal, strCn)
                Return RedirectToAction("AddRecogidaLinea", New With {.id = oVMRecogidaFinal.id})
            End If
            Return View(oVMRecogidaFinal)
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Public Function DeleteRecogidaLinea(id As Integer, l As VMRecogidaLinea) As ActionResult
            If ModelState.IsValid Then
                DBAccess.DeleteLineaRecogida(id, l, strCn)
                Return RedirectToAction("AddRecogidaLinea", New VMRecogidaFinal With {.id = id})
            End If
            Return AddRecogidaLinea(New VMRecogidaFinal With {.id = id})
        End Function

        <SimpleRoleProvider(Role.creacion)>
        Public Function EditCabeceraRecogida(id As Integer)
            Dim oVMRecogidaCabecera = CType(DBAccess.GetRecogidaFinal(id, strCn), VMRecogidaCabecera)
            Return View("AddRecogidaCabecera", oVMRecogidaCabecera)
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Public Function EditCabeceraRecogida(oVMRecogidaCabecera As VMRecogidaFinal)
            If ModelState.IsValid Then
                DBAccess.UpdateCabeceraRecogida(oVMRecogidaCabecera, strCn)
                Return RedirectToAction("ListMovimientosRecogidasSinAsignar")
            End If
            Return View("AddRecogidaCabecera", oVMRecogidaCabecera)
        End Function

        <SimpleRoleProvider(Role.creacion)>
        Public Function CrearViajeRecogida(ovmMovimientoRecogida As VMMovimientoRecogida) As ActionResult
            Dim lstRecogidaSeleccionada = ovmMovimientoRecogida.LstRecogida.Where(Function(m) m.Seleccionado).Select(Function(r) r.id)
            Dim oVmRecogidaViaje As New VMRecogidaViaje With {
                .LstRecogida = DBAccess.GetListOfRecogidaFinalSinViaje(strCn).Where(Function(r) lstRecogidaSeleccionada.Contains(r.id)),
                .LstTransportista = DBAccess.GetListOfTransportista(strCn).Select(Function(m) New SelectListItem With {.Value = m.codProv, .Text = m.nomProv})}
            Return View(oVmRecogidaViaje)
        End Function

        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Public Function RecogidaViajeSave(ovmRecogidaViaje As VMRecogidaViaje)
            If ModelState.IsValid Then
                DBAccess.SaveEnvio(ovmRecogidaViaje.Transportista, ovmRecogidaViaje.Matricula1, ovmRecogidaViaje.Matricula2, Nothing, ovmRecogidaViaje.LstRecogida.Select(Function(r) r.id), strCn)
            End If
        End Function
    End Class
End Namespace