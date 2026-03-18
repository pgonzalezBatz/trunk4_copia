Namespace web
    Public Class accessController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAB").ConnectionString


        Function Index() As ActionResult
            'Tiene cookie de autenticacion?
            Return RedirectToAction("index", "distribucion")
            If User.Identity.IsAuthenticated Then
                Return RedirectToAction("index", "distribucion")
            End If
            Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
            Dim idSab = db.login(Request.LogonUserIdentity.Name.ToLower, ConfigurationManager.AppSettings("IDRECURSO"), strCn)
            If idSab.Length = 0 Then
                log.Info("Intento de acceso fallido: " + Request.LogonUserIdentity.Name.ToLower)
                Return Redirect("~/accesodenegado.htm")
            End If
            log.Info("login " + Request.LogonUserIdentity.Name.ToLower + "," + Server.MachineName)
            FormsAuthentication.SetAuthCookie(idSab, False)

            Return RedirectToAction("index", "distribucion")
        End Function
    End Class
End Namespace