Imports System.Security.Permissions
Namespace web
    Public Class movimientosmaterialController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString
        <SimpleRoleProvider(Role.creacion)> _
        Function list() As ActionResult
            If Request.QueryString.Count = 0 Then
                ViewData("listOfMovimientos") = DBAccess.GetListMovimientosGroupes(strCn)
                Return View("listgrouped")
            End If
            'Dim l = DBAccess.GetListMovimientos(strCn)
            Dim l = DBAccess.GetListMovimientosConNegocio(strCn)
            Dim lstFiltrosApplicados As New List(Of Object)

            If Request("proveedor") IsNot Nothing Then
                l = l.FindAll(Function(e) e.NombreProveedor = Request("proveedor"))
                lstFiltrosApplicados.Add(New With {.type = "proveedor", .nombre = Request("proveedor")})
            End If
            If Request("empresasalida") IsNot Nothing Then
                l = l.FindAll(Function(e) e.NombreEmpresaSalida = Request("empresasalida"))
                lstFiltrosApplicados.Add(New With {.type = "empresasalida", .nombre = Request("empresasalida")})
            End If
            If Request("of") IsNot Nothing Then
                l = l.FindAll(Function(e) e.Numord.ToString = Request("of"))
                lstFiltrosApplicados.Add(New With {.type = "of", .nombre = Request("of")})
            End If
            If Request("fecha") IsNot Nothing Then
                l = l.FindAll(Function(e) e.FechaEntrega.Value.ToShortDateString = Request("fecha"))
                lstFiltrosApplicados.Add(New With {.type = "fecha", .nombre = Request("fecha")})
            End If


            Select Case Request("o")
                Case "of"
                    If Request("d") = "up" Then
                        l.Sort(Function(s1, s2) s1.Numord.CompareTo(s2.Numord))
                    Else
                        l.Sort(Function(s1, s2) s2.Numord.CompareTo(s1.Numord))
                    End If
                Case "fecha"
                    If Request("d") = "up" Then
                        l.Sort(Function(s1, s2) s1.FechaEntrega < s2.FechaEntrega)
                    Else
                        l.Sort(Function(s1, s2) s1.FechaEntrega > s2.FechaEntrega)
                    End If
                Case "peso"
                    If Request("d") = "up" Then
                        l.Sort(Function(s1, s2) s1.Peso < s2.Peso)
                    Else
                        l.Sort(Function(s1, s2) s1.Peso > s2.Peso)
                    End If
                Case "proveedor"
                    If Request("d") = "up" Then
                        l.Sort(Function(s1, s2) s1.NombreProveedor.CompareTo(s2.NombreProveedor))
                    Else
                        l.Sort(Function(s1, s2) s2.NombreProveedor.CompareTo(s1.NombreProveedor))
                    End If
                Case "observacion"
                    If Request("d") = "up" Then
                        l.Sort(Function(s1, s2) s1.Observacion.CompareTo(s2.Observacion))
                    Else
                        l.Sort(Function(s1, s2) s2.Observacion.CompareTo(s1.Observacion))
                    End If
                Case "empresasalida"
                    If Request("d") = "up" Then
                        l.Sort(Function(s1, s2) s1.NombreEmpresaSalida.CompareTo(s2.NombreEmpresaSalida))
                    Else
                        l.Sort(Function(s1, s2) s2.NombreEmpresaSalida.CompareTo(s1.NombreEmpresaSalida))
                    End If
            End Select
            ViewData("listOfMovimientos") = l
            ViewData("lstFiltrosAplicados") = lstFiltrosApplicados
            Return View()
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        Function filter(ByVal f As String) As ActionResult
            Select Case f
                Case "of"
                    ViewData("list") = DBAccess.GetListMovimientos(strCn).GroupBy(Function(g) g.Numord)
                Case "peso"
                    ViewData("list") = DBAccess.GetListMovimientos(strCn).GroupBy(Function(g) g.Peso)
                Case "proveedor"
                    ViewData("list") = DBAccess.GetListMovimientos(strCn).GroupBy(Function(g) g.NombreProveedor)
                Case "observacion"
                    ViewData("list") = DBAccess.GetListMovimientos(strCn).GroupBy(Function(g) g.Observacion)
                Case "empresasalida"
                    ViewData("list") = DBAccess.GetListMovimientos(strCn).GroupBy(Function(g) g.NombreEmpresaSalida)
                Case "fecha"
                    ViewData("list") = DBAccess.GetListMovimientos(strCn).GroupBy(Function(g) g.FechaEntrega.Value.ToShortDateString)
            End Select
            Return View("filter")
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        Function Create0() As ActionResult
            ViewData("operaciones") = New List(Of Mvc.SelectListItem)
            ViewData("negocios") = DBAccess.GetNegocios(strCn)
            Return View(New MovimientoBase With {.EmpresaSalida = 1, .Numord = Nothing})
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Create0(ByVal mb As MovimientoBase) As ActionResult
            If ModelState.IsValid AndAlso mb.IdNegocio > 0 Then
                mb.Negocio = DBAccess.GetNegocioFromId(mb.IdNegocio, strCn)
                ViewData("marcas") = DBAccess.GetListMarcas(mb.Numord, mb.Numope, strCn)
                ViewData("marcas").add(New Movimiento With {.Marca = "ZZZZ", .Otros = If(ViewData("marcas").count > 0, ViewData("marcas")(0).otros, New With {.salida = "", .empresaDestino = "", .EmpresaSalida = ""}), .Peso = 1})
                Return View("create1", mb)
            End If
            ViewData("operaciones") = h.MySelectList(DBAccess.GetListNumOpActivas(mb.Numord, strCn),
                                                     Function(o) New Mvc.SelectListItem With {.Value = o.Numope, .Text = o.Numope.ToString + " - " + o.Descripcion, .Selected = o.Numope = mb.Numope})
            ViewData("negocios") = DBAccess.GetNegocios(strCn)
            Return View()
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Create1(ByVal formValues As FormCollection) As ActionResult
            Dim listOfMovimiento = New List(Of Movimiento)
            If ModelState.IsValid Then
                Dim result = Array.FindAll(Of String)(formValues.AllKeys, Function(str) Regex.IsMatch(str, "_.*\s*$"))
                Dim grouped = result.ToList().GroupBy(Function(str) Regex.Split(str, "^[a-z]+_")(1))

                For Each g In grouped
                    Dim g2 = g.ToList
                    Dim m As New Movimiento
                    For Each e In g2
                        Dim a = e.Split("_")
                        Select Case a(0)
                            Case "marca"
                                m.Marca = formValues(e)
                            Case "cantidad"
                                m.Cantidad = If(IsNumeric(formValues(e)), CInt(formValues(e)), New Nullable(Of Integer))
                                If formValues(e).Length > 0 And Not IsNumeric(formValues(e)) Then
                                    ModelState.AddModelError(e, "La cantidad es erronea")
                                    ViewData(e) = formValues(e)
                                End If
                            Case "peso"
                                m.Peso = If(IsNumeric(formValues(e)), CDec(formValues(e)), New Nullable(Of Decimal))
                            Case "ancho"
                                m.Ancho = If(IsNumeric(formValues(e)), CInt(formValues(e)), New Nullable(Of Integer))
                            Case "largo"
                                m.Largo = If(IsNumeric(formValues(e)), CInt(formValues(e)), New Nullable(Of Integer))
                            Case "alto"
                                m.Alto = If(IsNumeric(formValues(e)), CInt(formValues(e)), New Nullable(Of Integer))
                            Case "diametro"
                                m.Diametro = If(IsNumeric(formValues(e)), CInt(formValues(e)), New Nullable(Of Integer))
                            Case "observacion"
                                If formValues(e).Length = 0 AndAlso formValues("descripciongeneral").Length > 0 Then
                                    m.Observacion = formValues("descripciongeneral")
                                Else
                                    m.Observacion = formValues(e)
                                End If
                        End Select
                    Next
                    If m.Cantidad.HasValue AndAlso m.Peso.HasValue AndAlso m.Peso > 0 Then
                        m.IdNegocio = formValues("IdNegocio")
                        m.Numope = formValues("Numope") : m.Numord = formValues("Numord") : m.CodPro = formValues("CodPro") : m.FechaEntrega = formValues("FechaEntrega")
                        m.EmpresaSalida = formValues("EmpresaSalida")
                        m.IdSab = SimpleRoleProvider.GetId()
                        listOfMovimiento.Add(m)
                    ElseIf m.Cantidad.HasValue Then
                        ModelState.AddModelError(g2.First, h.traducir("Es obligatorio introducir peso mayor que 0"))
                    End If
                Next
                If Not IsNumeric(formValues("Numope")) AndAlso Not IsNumeric(formValues("Numord")) AndAlso Not IsNumeric(formValues("CodPro")) AndAlso Not IsNumeric(formValues("EmpresaSalida")) Then
                    ModelState.AddModelError("", "Algo raro a ocurrido. Puede que se haya perdido la session")
                End If
                If ModelState.IsValid Then
                    DBAccess.SaveMovimientosMaterial(listOfMovimiento, strCn)
                    Return RedirectToAction("list")
                End If
            End If
            ViewData("marcas") = h.map(Of Movimiento, Movimiento)(DBAccess.GetListMarcas(formValues("Numord"), formValues("Numope"), strCn), Function(m As Movimiento) RestoreMarcas(m, formValues))
            Return View(New MovimientoBase With {.Numope = formValues("Numope"), .Numord = formValues("Numord"), .CodPro = formValues("CodPro"), .FechaEntrega = formValues("FechaEntrega"), .EmpresaSalida = formValues("EmpresaSalida")})
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        Function Edit(ByVal id As Integer) As ActionResult
            Dim m = DBAccess.GetMovimiento(id, strCn)
            DBAccess.GetListNumOpActivas(m.Numord, strCn)
            ViewData("operaciones") = Operaciones(m.Numord, id, strCn)
            Return View(m)
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Edit(ByVal m As Movimiento) As ActionResult
            'Mirar si la marca tiene peso en cplismat
            If Not DBAccess.IsMarcaValida(m.Numord, m.Numope, m.Marca.Trim(" "), strCn) Then
                ModelState.AddModelError("nummar", "La marca introducida no existe")
            End If
            If ModelState.IsValid Then
                DBAccess.UpdateMovimiento(m, strCn)
                Return RedirectToAction("list")
            End If
            ViewData("operaciones") = Operaciones(m.Numord, m.Id, strCn)
            Return View(m)
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        Function Delete(ByVal id As Integer) As ActionResult
            Dim m = DBAccess.GetMovimiento(id, strCn)
            Return View(m)
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Delete(ByVal m As Movimiento) As ActionResult
            DBAccess.RemoveMovimientoMaterial(m.Id, strCn)
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function DeleteList(ByVal agrupar As List(Of Integer)) As ActionResult
            DBAccess.RemoveListOfMovimientoMaterial(agrupar, strCn)
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function EditList(ByVal agrupar As List(Of Integer), ByVal proveedor As Nullable(Of Integer), ByVal empresaSalida As Nullable(Of Integer)) As ActionResult
            If proveedor.HasValue Or empresaSalida.HasValue And agrupar IsNot Nothing AndAlso agrupar.Count > 0 Then
                DBAccess.updateListOfMovimiento(agrupar, proveedor, empresaSalida, strCn)
            End If
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.creacion)>
        Function CreateDesdePedido() As ActionResult
            ViewData("negocios") = DBAccess.GetNegocios(strCn)
            Return View("CreateDesdePedido")
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function CreateDesdePedido(npedido As Nullable(Of Integer), codpro As Nullable(Of Integer), FechaEntrega As Nullable(Of DateTime), idNegocio As Nullable(Of Integer)) As ActionResult
            If Not npedido.HasValue Then
                ModelState.AddModelError("npedido", "Es necesario introducir el numero de pedido")
            End If
            If Not codpro.HasValue Then
                ModelState.AddModelError("codpro", "Es necesario introducir el numero de proveedor")
            End If
            If Not FechaEntrega.HasValue Then
                ModelState.AddModelError("FechaEntrega", "Es necesario introducir la fecha de salida")
            End If
            If Not idNegocio.HasValue OrElse idNegocio.Value < 1 Then
                ModelState.AddModelError("Negocio", "Es necesario introducir un negocio")
            End If
            If ModelState.IsValid Then
                ViewData("marcas") = DBAccess.GetListMarcasDesdePedido(npedido.Value, strCn)
                If ViewData("marcas").count = 0 Then
                    ModelState.AddModelError("npedido", "El pedido seleccionado no tiene marcas")
                    Return View("CreateDesdePedido")
                Else
                    ViewData("idnegocio") = idNegocio.Value
                    Return View("CreateDesdePedido1")
                End If

            Else
                Return CreateDesdePedido()
            End If
        End Function
        <SimpleRoleProvider(Role.creacion)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function CreateDesdePedido1(ByVal formValues As FormCollection) As ActionResult
            Dim listOfMovimiento = New List(Of Movimiento)
            If ModelState.IsValid Then
                Dim result = Array.FindAll(Of String)(formValues.AllKeys, Function(str) Regex.IsMatch(str, "_.*\s*$"))
                Dim grouped = result.ToList().GroupBy(Function(str) Regex.Split(str, "^[a-z]+_")(1))

                For Each g In grouped
                    Dim g2 = g.ToList
                    Dim m As New Movimiento
                    For Each e In g2
                        Dim a = e.Split("_")
                        Select Case a(0)
                            Case "marca"
                                m.Marca = formValues(e)
                            Case "cantidad"
                                m.Cantidad = If(IsNumeric(formValues(e)), CInt(formValues(e)), New Nullable(Of Integer))
                                If formValues(e).Length > 0 And Not IsNumeric(formValues(e)) Then
                                    ModelState.AddModelError(e, "La cantidad es erronea")
                                    ViewData(e) = formValues(e)
                                End If
                            Case "peso"
                                m.Peso = If(IsNumeric(formValues(e)), CDec(formValues(e)), New Nullable(Of Decimal))
                            Case "ancho"
                                m.Ancho = If(IsNumeric(formValues(e)), CInt(formValues(e)), New Nullable(Of Integer))
                            Case "largo"
                                m.Largo = If(IsNumeric(formValues(e)), CInt(formValues(e)), New Nullable(Of Integer))
                            Case "alto"
                                m.Alto = If(IsNumeric(formValues(e)), CInt(formValues(e)), New Nullable(Of Integer))
                            Case "diametro"
                                m.Diametro = If(IsNumeric(formValues(e)), CInt(formValues(e)), New Nullable(Of Integer))
                            Case "observacion"
                                If formValues(e).Length = 0 AndAlso formValues("descripciongeneral").Length > 0 Then
                                    m.Observacion = formValues("descripciongeneral")
                                Else
                                    m.Observacion = formValues(e)
                                End If
                        End Select
                    Next
                    If m.Cantidad.HasValue AndAlso m.Peso.HasValue Then
                        m.IdNegocio = formValues("idnegocio")
                        m.Numope = formValues("Numope") : m.Numord = formValues("Numord") : m.CodPro = formValues("CodPro") : m.FechaEntrega = formValues("FechaEntrega")
                        m.EmpresaSalida = formValues("EmpresaSalida")
                        m.IdSab = SimpleRoleProvider.GetId()
                        listOfMovimiento.Add(m)
                    ElseIf m.Cantidad.HasValue Then
                        ModelState.AddModelError(g2.First, "Es obligatorio introducir peso")
                    End If
                Next
                If Not IsNumeric(formValues("Numope")) AndAlso Not IsNumeric(formValues("Numord")) AndAlso Not IsNumeric(formValues("CodPro")) AndAlso Not IsNumeric(formValues("EmpresaSalida")) Then
                    ModelState.AddModelError("", "Algo raro a ocurrido. Puede que se haya perdido la session")
                End If
                If ModelState.IsValid Then
                    DBAccess.SaveMovimientosMaterial(listOfMovimiento, strCn)
                    Return RedirectToAction("list")
                End If
            End If
            ViewData("marcas") = h.map(Of Movimiento, Movimiento)(DBAccess.GetListMarcas(formValues("Numord"), formValues("Numope"), strCn), Function(m As Movimiento) RestoreMarcas(m, formValues))
            Return View("CreateDesdePedido")
        End Function
      
        Private Function Operaciones(ByVal numord As Integer, ByVal id As Integer, ByVal strCn As String) As List(Of Mvc.SelectListItem)
            Return h.MySelectList(Of Object)(DBAccess.GetListNumOpActivas(numord, strCn), _
                                                      Function(o) New Mvc.SelectListItem With {.Value = o.numope.ToString, .Text = o.numope.ToString + " - " + o.descripcion, .Selected = id = o.numope})
        End Function
        Private Function RestoreMarcas(ByVal m As Movimiento, ByVal fm As FormCollection) As Movimiento
            If fm("peso_" + m.Marca).Length > 0 Then
                m.Peso = fm("peso_" + m.Marca)
            End If
            If fm("observacion_" + m.Marca).Length > 0 Then
                m.Observacion = fm("observacion_" + m.Marca)
            End If
            Return m
        End Function
    End Class
End Namespace