Imports System.Security.Permissions
Namespace extranet
    Public Class accessController
        Inherits System.Web.Mvc.Controller


        Private strCn As String = ConfigurationManager.ConnectionStrings("sab").ConnectionString

        <SimpleRoleProvider(Role.productiva, Role.inproductiva)>
        Function CerrarSession() As ActionResult
            FormsAuthentication.SignOut()
            Return Redirect("https://extranet.batz.es")
        End Function
        <SimpleRoleProvider(Role.productiva, Role.inproductiva)>
        Function volverportal() As ActionResult
            Dim sessionId = Session.SessionID
            db.setTicket(sessionId, User.Identity.Name, strCn)
            FormsAuthentication.SignOut()
            Session.Abandon()
            Return Redirect("/extranetlogin/?IdSession=" + sessionId)
        End Function

    End Class
End Namespace