Imports System.Security.Permissions
Namespace web
    Public Class agrupacionController
        Inherits System.Web.Mvc.Controller
        Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function Pesar(ByVal agrupar As List(Of Integer)) As ActionResult
            If agrupar IsNot Nothing AndAlso agrupar.Count > 0 Then
                If DBAccess.UnicoProveedorEnMovimientos(agrupar, strCn) AndAlso DBAccess.UnicoNegocioEnMovimientos(agrupar, strCn) Then
                    Dim lstMovimientos = DBAccess.GetListMovimientos(strCn)
                    lstMovimientos = lstMovimientos.FindAll(Function(m) agrupar.Exists(Function(e) e = m.Id))
                    ViewData("total") = lstMovimientos.Sum(Function(m) m.Peso)
                    Return View(viewName:="pesar", model:=lstMovimientos)
                Else
                    Return RedirectToAction("errordirecciones")
                End If
            End If
            Return Redirect(Request.UrlReferrer.AbsolutePath)
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function RemoveMovimientos(ByVal agrupar As Integer) As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function Create(ByVal agrupar As List(Of Integer), ByVal pesoBulto As Nullable(Of Decimal)) As ActionResult
            If agrupar IsNot Nothing AndAlso agrupar.Count > 0 Then
                If DBAccess.UnicoProveedorEnMovimientos(agrupar, strCn) Then
                    If pesoBulto.HasValue Then
                        DBAccess.SaveAgrupacion(agrupar, pesoBulto, strCn)
                        Return RedirectToAction("list")
                    Else
                        ModelState.AddModelError("peso", "Es obligatorio introducir el peso del bulto")
                        Return Pesar(agrupar)
                    End If
                Else
                    Return RedirectToAction("errordirecciones")
                End If
            End If
            Return Redirect(Request.UrlReferrer.AbsolutePath)
        End Function
        <SimpleRoleProvider(Role.creacion)>
        Function ErrorDirecciones() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(Role.creacion)>
        Function List() As ActionResult
            Dim l = DBAccess.GetListOfAgrupaciones(strCn).OrderBy(Function(ag)
                                                                      If ag.ListOfMovimiento.Count = 0 Then
                                                                          Return ""
                                                                      Else
                                                                          Return ag.ListOfMovimiento(0).CodPro
                                                                      End If
                                                                  End Function).ToList
            ViewData("listOfAgrupacion") = l
            Return View("list")
        End Function
        <SimpleRoleProvider(Role.creacion)>
        Function Edit(ByVal id As Integer) As ActionResult
            Return View(DBAccess.GetAgrupacion(id, strCn))
        End Function
        <SimpleRoleProvider(Role.creacion)>
        Function editPeso(idBulto As Integer) As ActionResult
            Return View(DBAccess.GetAgrupacion(idBulto, strCn))
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        <SimpleRoleProvider(Role.creacion)>
        Function editPeso(idBulto As Integer, peso As Decimal?) As ActionResult
            If peso.HasValue Then
                DBAccess.updatePesoBulto(idBulto, peso.Value, strCn)
                Return RedirectToAction("list", h.ToRouteValuesDelete(Request.QueryString, "idbulto"))
            End If
            Return View(DBAccess.GetAgrupacion(idBulto, strCn))
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function Remove(ByVal agrupacion As Integer, ByVal movimiento As Integer) As ActionResult
            DBAccess.RemoveMovimientoFromGrupo(movimiento, strCn)
            If DBAccess.GetAgrupacion(agrupacion, strCn).ListOfMovimiento.Count > 0 Then
                Return RedirectToAction("edit", New With {.id = agrupacion})
            Else
                Return RedirectToAction("list")
            End If
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function RemoveAll(ByVal agrupacion As Integer) As ActionResult
            DBAccess.RemoveGrupo(agrupacion, strCn)
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.creacion)>
        Function AddMovimiento(ByVal id As Integer) As ActionResult
            Dim listaFinal As New List(Of Movimiento)

            ''''OPCION A:
            'Dim hayError As Boolean = False
            ''''' buscar agrupacion con ese id y ver si tiene algún movimiento
            'Dim listaMovimientos As List(Of Movimiento) = DBAccess.GetListMovimientosForId(id, strCn)
            'If listaMovimientos.Count > 0 Then
            '    '''' si tiene movimientos, comprobar que todos pertenecen al mismo negocio
            '    Dim negocio = listaMovimientos.FirstOrDefault().IdNegocio
            '    For Each mov In listaMovimientos
            '        If mov.IdNegocio <> negocio Then
            '            ''''    si no, error
            '            hayError = True
            '            'listaFinal = New List(Of Movimiento)
            '            Exit For
            '        End If
            '    Next
            '    If Not hayError Then
            '        ''''    si ok, getlistmovimientos filtrado por negocio
            '        listaFinal = DBAccess.GetListMovimientosSinAgrupacionForNegocio(negocio, strCn)
            '    End If
            'Else
            '    '''' si no tiene movimientos, mostrar todos pero con el negocio
            '    listaFinal = DBAccess.GetListMovimientosSinAgrupacion(strCn)
            'End If

            ''''OPCION B:
            Dim negocioAgrupacion As List(Of Integer) = DBAccess.GetNegocioFromAgrupacion(id, strCn)
            If negocioAgrupacion.Count > 1 Then
                '''' hay error, dentro de una agrupación sólo debería ir un negocio --> devolveremos la listaFinal tal cual, vacía
            ElseIf negocioAgrupacion.Count < 1 Then
                '''' la agrupacion no tiene movimientos --> devolvemos la listaFinal total sin filtrar
                listaFinal = DBAccess.GetListMovimientosSinAgrupacion(strCn)
            ElseIf negocioAgrupacion.Count = 1 Then
                '''' la agrupacion tiene movimientos de un negocio concreto -> devolvemos la listaFinal filtrada por negocio
                listaFinal = DBAccess.GetListMovimientosSinAgrupacionForNegocio(negocioAgrupacion.FirstOrDefault(), strCn)
            End If
            Return View(listaFinal)
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function AddMovimiento(ByVal id As Integer, ByVal movimiento As Integer) As ActionResult
            DBAccess.AddMovimiento(id, movimiento, strCn)
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.creacion)>
        Function pdf(id As Integer) As ActionResult
            Dim blt = DBAccess.GetAgrupacion(id, strCn)
            Dim p = DBAccess.GetProveedorConDireccion(blt.ListOfMovimiento.First.CodPro, strCn)
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=viaje_" + id.ToString + ".pdf")
            h.printLabel(New With {.proveedor = blt.ListOfMovimiento.First.NombreProveedor, .calle = p.calle, .codigoPostal = p.codigopostal,
                                               .poblacion = p.poblacion, .provincia = p.provincia, .telefono = p.telefono, .peso = blt.Peso, .idBulto = id})
            Return New FileStreamResult(h.bultoPdf(p, id, blt.ListOfMovimiento, blt.Peso), "application/pdf")
        End Function
        <SimpleRoleProvider(Role.creacion)>
        Function EmbedBulto(id As Integer) As ActionResult
            Dim negocioId = DBAccess.GetNegocioFromAgrupacion(id, strCn)
            If negocioId.Count > 1 Then
                ModelState.AddModelError("negocio", "el bulto no es válido, tiene varios negocios")
                ViewData("listOfAgrupacion") = Nothing
            Else
                ViewData("listOfAgrupacion") = DBAccess.GetListOfAgrupaciones(strCn).Where(Function(ag) ag.Id <> id AndAlso ag.ListOfMovimiento.All(Function(m) m.IdNegocio = negocioId.FirstOrDefault())).OrderBy(Function(ag)
                                                                                                                                                                                                                       If ag.ListOfMovimiento.Count = 0 Then
                                                                                                                                                                                                                           Return ""
                                                                                                                                                                                                                       Else
                                                                                                                                                                                                                           Return ag.ListOfMovimiento(0).CodPro
                                                                                                                                                                                                                       End If
                                                                                                                                                                                                                   End Function).ToList
            End If

            Return View()
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function EmbedBulto(id As Integer, idParent As Integer) As ActionResult
            DBAccess.updateBultoEmbed(id, idParent, strCn)
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.creacion)>
        Function UnEmbedBulto(id As Integer) As ActionResult
            DBAccess.updateBultoUnEmbed(id, strCn)
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function createEmptyBulto(peso As Decimal?) As ActionResult
            If Not peso.HasValue Then
                ModelState.AddModelError("peso", "Necesario introducir peso del bulto sin marcas")
            End If
            If ModelState.IsValid Then
                DBAccess.SaveEmptyAgrupacion(peso.Value, strCn)
                Return RedirectToAction("list")
            End If
            Return View(List)
        End Function
    End Class
End Namespace