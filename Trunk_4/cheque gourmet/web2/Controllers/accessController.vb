Imports System.Security.Permissions
Namespace web
    Public Class accessController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAB").ConnectionString
        Function Index() As ActionResult
            'Tiene cookie de autenticacion?
            If User.Identity.IsAuthenticated Then
                Return RedirectToAction("index", "historico")
            End If
            If Request.LogonUserIdentity.Name.ToLower = "batznt\diglesias" Then
                FormsAuthentication.SetAuthCookie(57552, False)
                Return RedirectToAction("index", "historico")
            End If
            If Request("id") Is Nothing Then
                Return Redirect("~/accesodenegado.html")
            End If
            Dim idSab = db.GetIdSabFromTicket(Request("id"), strCn)
            'Comprobar accesso a recurso
            Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
            If db.login(idSab, ConfigurationManager.AppSettings("IDRECURSO"), strCn) = 0 Then
                log.Info("Intento de acceso fallido: " + Request.LogonUserIdentity.Name.ToLower)
                Return Redirect("~/accesodenegado.html")
            End If
            log.Info("login " + Request.LogonUserIdentity.Name.ToLower + "," + Server.MachineName)
            FormsAuthentication.SetAuthCookie(idSab, False)

            Return RedirectToAction("index", "historico")
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function CerrarSession() As ActionResult
            FormsAuthentication.SignOut()
            Return Redirect("//intranet2.batz.es")
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function volverportal() As ActionResult
            Dim sessionId = Session.SessionID
            db.setTicket(sessionId, SimpleRoleProvider.GetId(), strCn)
            FormsAuthentication.SignOut()
            Return Redirect("//intranet2.batz.es/langileen_txokoa/menuempleado.aspx?id=" + sessionId)
        End Function
    End Class
End Namespace