Imports System.Security.Permissions
Namespace ExtranetMVC
    Public Class accessController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("sab").ConnectionString
        Function Index() As ActionResult
            Return RedirectToAction("nuevoscabecera", "pedidos")
        End Function
        <SimpleRoleProvider(1)>
        Function CerrarSession() As ActionResult
            FormsAuthentication.SignOut()
            Return Redirect("//extranet.batz.es")
        End Function
        <SimpleRoleProvider(1)>
        Function volverportal() As ActionResult
            Dim sessionId = Session.SessionID
            db.setTicket(sessionId, SimpleRoleProvider.GetId(), strCn)
            FormsAuthentication.SignOut()
            Return Redirect("/extranetlogin/?IdSession=" + sessionId)
        End Function

    End Class
End Namespace