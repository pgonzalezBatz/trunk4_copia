Namespace web
    Public Class busquedaController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString

        <SimpleRoleProvider(Role.busquedas)> _
        Function Index() As ActionResult
            ViewData("ejercicio") = Now.Year
            ViewData("transportista") = DBAccess.GetListOfTransportista(strCn).Select(Function(o) New Mvc.SelectListItem() With {.Value = o.codProv.trim(), .Text = o.nomProv, .Selected = Regex.IsMatch(o.nomProv, "JOVEN")})
            ViewData("negocios") = DBAccess.GetNegocios(strCn)
            Return View()
        End Function
        <SimpleRoleProvider(Role.busquedas)> _
        Function Productivas() As ActionResult
            If Not IsNumeric(Request("idfrom")) And Not Request("idfrom") Is Nothing Then
                ModelState.AddModelError("idfrom", "El valor debe ser numérico indicando el numero de viaje")
            End If
            If Not IsNumeric(Request("idto")) And Not Request("idto") Is Nothing Then
                ModelState.AddModelError("idto", "El valor debe ser numérico indicando el numero de viaje")
            End If
            Dim lst As New List(Of Viaje)
            If ModelState.IsValid Then
                lst.AddRange(DBAccess.GetListOfBusquedasProductivas(Request("idfrom"), Request("idto"), strCn))
            End If
            Return View(lst)
        End Function
        <SimpleRoleProvider(Role.busquedas)> _
        Function NoProductivas() As ActionResult
            If Not IsNumeric(Request("idfrom")) And Request("idfrom") IsNot Nothing Then
                ModelState.AddModelError("idfrom", "El valor debe ser numérico indicando el numero de viaje")
            End If
            If Not IsNumeric(Request("idto")) And Request("idto") IsNot Nothing Then
                ModelState.AddModelError("idto", "El valor debe ser numérico indicando el numero de viaje")
            End If
            Dim lst As New List(Of Object)
            If ModelState.IsValid Then
                lst.AddRange(DBAccess.GetListOfBusquedasNoProductivas(Request("idfrom"), Request("idto"), strCn))
            End If
            Return View(lst)
        End Function
        <SimpleRoleProvider(Role.busquedas)>
        Function ListadoMensual(ejercicio As Integer, mes As Nullable(Of Integer), negocios As Nullable(Of Integer), transportista As String, taxista As String) As ActionResult
            If Not mes.HasValue Then
                'Return RedirectToAction("index")
                ModelState.AddModelError("mes", "El mes es obligatorio")
            End If
            If Not negocios.HasValue OrElse negocios < 1 Then
                ModelState.AddModelError("negocios", "El negocio es obligatorio")
            End If
            Dim lstViajes As New List(Of Object)
            If ModelState.IsValid Then
                Dim negocio = DBAccess.GetNegocioFromId(negocios, strCn)
                ViewData("transportista") = DBAccess.GetTransportista(transportista, strCn)
                lstViajes = DBAccess.GetListOfViajesMesTransportista(ejercicio, mes, transportista, taxista IsNot Nothing, negocios, strCn)
                For Each l In lstViajes
                    l.Negocio = negocio
                Next
                ViewData("importexbat") = lstViajes.Sum(Function(v) CDec(v.importexbat))
                If Not String.IsNullOrEmpty(taxista) Then
                    Dim lst2 = DBAccess.GetListOfViajesMesTaxiSubcontratado(ejercicio, mes, transportista, negocios, strCn)
                    For Each l In lst2
                        l.Negocio = negocio
                    Next
                    ViewData("taxiSubcontratado") = lst2
                    ViewData("importexbatSubconstratado") = lst2.Sum(Function(v) CDec(v.importexbat))
                End If
                Return View(lstViajes)
            Else
                ViewData("ejercicio") = Now.Year
                ViewData("transportista") = DBAccess.GetListOfTransportista(strCn).Select(Function(o) New Mvc.SelectListItem() With {.Value = o.codProv.trim(), .Text = o.nomProv, .Selected = Regex.IsMatch(o.nomProv, "JOVEN")})
                ViewData("negocios") = DBAccess.GetNegocios(strCn)
                Return View("index")
            End If
        End Function
        <SimpleRoleProvider(Role.busquedas)>
        Function albaran() As ActionResult
            Return View()
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        <SimpleRoleProvider(Role.busquedas)>
        Function albaran(id As Integer) As ActionResult
            Return View(DBAccess.GetAlbaranViaje(id, strCn))
        End Function
    End Class
End Namespace