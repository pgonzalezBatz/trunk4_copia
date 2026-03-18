Imports System.Security.Permissions
Namespace web
    Public Class TransporteController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString
        <SimpleRoleProvider(Role.compras)> _
        Function List() As ActionResult
            Dim l = DBAccess.GetListEnvio(True, GetStringTransportistas(strCn), strCn)
            Select Case Request("f")
                Case "transportista"
                    l = l.FindAll(Function(e) e.IdTransportista.ToString = Request("v"))
            End Select
            l = l.FindAll(Function(o) o.fechaCreacion > Date.Today.AddMonths(-3))
            Return View(l)
        End Function


        <SimpleRoleProvider(Role.compras)> _
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function List(ByVal id As Integer, ByVal precio As Nullable(Of Decimal)) As ActionResult
            If precio.HasValue Then
                If DBAccess.HasPedido(id, strCn) Then
                    Return Redirect(Url.Action("list") + "?" + Request.QueryString.ToString)
                End If
                DBAccess.InsertOUpdatePedido(id, precio, SimpleRoleProvider.GetId(), False, strCn)
                Return Redirect(Url.Action("list") + "?" + Request.QueryString.ToString)
            End If
            Return View(DBAccess.GetListEnvio(True, GetStringTransportistas(strCn), strCn))
        End Function
        <SimpleRoleProvider(Role.compras)> _
        Function Imprimir(ByVal id As Integer) As FileStreamResult
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=viaje_" + id.ToString + ".pdf")
            Return New FileStreamResult(h.viajePdf(id), "application/pdf")
        End Function
        <SimpleRoleProvider(Role.compras)> _
        Function filter(ByVal f As String) As ActionResult
            Select Case f
                Case "transportista"
                    ViewData("list") = DBAccess.GetTransportistasConViajeSinPrecio(strCn)
            End Select
            Return View("filter")
        End Function
        Private Function GetStringTransportistas(ByVal strCn As String) As String
            Dim s As New Text.StringBuilder
            For Each o In h.GetListOfDefaultEmpresaFromStrCn(strCn)
                s.Append(o.id.ToString)
            Next
            s.Remove(s.Length - 1, 1)
            Return s.ToString
        End Function
        <SimpleRoleProvider(Role.envio)> _
        Function decamino(idViaje As Integer) As ActionResult
            Dim viaje = DBAccess.GetEnvio(idViaje, strCn)
            For Each a In viaje.ListOfAlbaran
                Dim idempresa = a.ListOfAgrupacion.First.ListOfMovimiento.First.CodPro
                Dim email = DBAccess.GetEmailempresa(idempresa, strCn)
                h.SendEmail(email, "Batz salida viaje Nº " + idViaje.ToString, "El material adjuntado en el albaran esta de camino.", New Net.Mail.Attachment(h.albaranPdf(idViaje, a.Id, 0), "Viaje_" + idViaje.ToString, "application/pdf"))
            Next
            DBAccess.updateViajeSetDeCamino(idViaje, strCn)
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.envio)> _
        Function listhorasextradiario() As ActionResult
            Return View(DBAccess.GetViajesHorasExtra(strCn))
        End Function
        <SimpleRoleProvider(Role.envio)> _
        Function edithorasextradiario(nPedido As Integer, ticksFecha As Int64, horasExtra As Integer) As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(Role.envio)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function edithorasextradiario(nPedido As Integer, ticksFecha As Int64, horasExtra As Integer, confirm As String) As ActionResult
            If ModelState.IsValid Then
                DBAccess.updatePedidoHorasExtras(nPedido, New Date(ticksFecha), horasExtra, ConfigurationManager.AppSettings("precioHoraExtra"), ConfigurationManager.AppSettings("precio_trp_dia"), strCn)
                Return RedirectToAction("listhorasextradiario")
            End If
            Return View()
        End Function
    End Class
End Namespace