Namespace web
    Public Class taxiController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString
        <SimpleRoleProvider(Role.taxi)> _
        Function list() As ActionResult
            Return View(DBAccess.getListOfVijeTaxi(strCn))
        End Function
        <SimpleRoleProvider(Role.taxi)> _
        Function create() As ActionResult
            ViewData("proveedor") = h.MySelectList(DBAccess.GetListOfTaxista(strCn), Function(o) New Mvc.SelectListItem With {.Value = o.id, .Text = o.nombre})
            ViewData("negocios") = DBAccess.GetNegocios(strCn)
            Return View()
        End Function
        <SimpleRoleProvider(Role.taxi)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function create(ByVal proveedor As Nullable(Of Integer), ByVal origen As String, ByVal destino As String, ByVal fecha As Nullable(Of DateTime), ByVal observacion As String, ByVal negocios As Nullable(Of Integer)) As ActionResult

            validateViajeTaxiForm(proveedor, fecha, negocios)
            If ModelState.IsValid Then
                Dim id = DBAccess.addViajeTaxi(proveedor, origen, destino, fecha, observacion, negocios, strCn)

                Dim strBody As New Text.StringBuilder("* Origen:")
                strBody.Append(Environment.NewLine) : strBody.Append(origen)
                strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine) : strBody.Append("* Destino:")
                strBody.Append(Environment.NewLine) : strBody.Append(destino)
                strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine) : strBody.Append("* Fecha:")
                strBody.Append(Environment.NewLine) : strBody.Append(fecha.Value.ToShortDateString)
                strBody.Append(Environment.NewLine) : strBody.Append(Environment.NewLine) : strBody.Append("* Observaciones:")
                strBody.Append(Environment.NewLine) : strBody.Append(observacion)
                h.SendEmail(DBAccess.GetEmailempresa(proveedor, strCn), "Viaje taxi Nº " + id.ToString, strBody.ToString)
                Return RedirectToAction("list")
            End If
            ViewData("proveedor") = h.MySelectList(DBAccess.GetListOfTaxista(strCn), Function(o) New Mvc.SelectListItem With {.Value = o.id, .Text = o.nombre})
            ViewData("negocios") = DBAccess.GetNegocios(strCn)
            Return View()
        End Function
        <SimpleRoleProvider(Role.taxi)> _
        Function Edit(ByVal id As Integer) As ActionResult
            Dim v = DBAccess.GetVijeTaxi(id, strCn)
            ViewData("proveedor") = h.MySelectList(DBAccess.GetListOfTaxista(strCn), Function(o) New Mvc.SelectListItem With {.Value = o.id, .Text = o.nombre, .Selected = o.id = v.idProveedor})
            ViewData("negocios") = DBAccess.GetNegocios(strCn)
            Return View("create", v)
        End Function
        <SimpleRoleProvider(Role.taxi)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function Edit(ByVal id As Integer, ByVal proveedor As Nullable(Of Integer), ByVal origen As String, ByVal destino As String, ByVal fecha As Nullable(Of DateTime), ByVal observacion As String, ByVal negocio As Nullable(Of Integer)) As ActionResult
            validateViajeTaxiForm(proveedor, fecha, negocio)
            If ModelState.IsValid Then
                DBAccess.updateViajeTaxi(id, proveedor, origen, destino, fecha, observacion, negocio, strCn)
                Return RedirectToAction("list")
            End If
            Dim v = DBAccess.GetVijeTaxi(id, strCn)
            ViewData("proveedor") = h.MySelectList(DBAccess.GetListOfTaxista(strCn), Function(o) New Mvc.SelectListItem With {.Value = o.id, .Text = o.nombre, .Selected = o.id = proveedor})
            Return View("create", v)
        End Function


        <SimpleRoleProvider(Role.taxi)>
        Function EditNegocio(ByVal id As Integer) As ActionResult
            ViewData("Id") = id
            ViewData("negocios") = DBAccess.GetNegocios(strCn)
            Return View("editnegocio")
        End Function

        <SimpleRoleProvider(Role.taxi)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function EditNegocio(ByVal id As Integer, ByVal negocios As Nullable(Of Integer)) As ActionResult
            If Not negocios.HasValue Then
                ModelState.AddModelError("negocios", "Es obligatorio introducir un negocio")
            End If
            If ModelState.IsValid Then
                DBAccess.updateNegocioTaxi(id, negocios, strCn)
                Return RedirectToAction("list")
            End If
            ViewData("Id") = id
            ViewData("negocios") = DBAccess.GetNegocios(strCn)
            Return View("editnegocio")
        End Function

        <SimpleRoleProvider(Role.taxi)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function UpdatePrecio(ByVal id As Integer, ByVal precio As Nullable(Of Decimal)) As ActionResult
            If Not precio.HasValue Then
                ModelState.AddModelError("precio", "Es obligatorio introducir precio para crear pedido")
            End If
            If ModelState.IsValid Then
                'Tambien crea pedido
                DBAccess.UpdatePrecioTaxi(id, precio, SimpleRoleProvider.GetId(), strCn)
                Return RedirectToAction("list")
            End If
            Return RedirectToAction("list")
        End Function
        Private Sub validateViajeTaxiForm(ByVal proveedor As Nullable(Of Integer), ByVal fecha As Nullable(Of DateTime), ByVal negocio As Nullable(Of Integer))
            If Not proveedor.HasValue Then
                ModelState.AddModelError("proveedor", "Es obligatorio introducir un taxista")
            End If
            If Not fecha.HasValue Then
                ModelState.AddModelError("fecha", "Es obligatorio introducir fecha")
            End If
            If Not negocio.HasValue Then
                ModelState.AddModelError("negocios", "Es obligatorio introducir un negocio")
            End If
        End Sub
    End Class
End Namespace