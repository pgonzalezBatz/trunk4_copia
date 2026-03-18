' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
' visit http://go.microsoft.com/?LinkId=9394802
Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Private strCn As String = ConfigurationManager.ConnectionStrings("sab").ConnectionString

    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\log4netConfig.config"))
    End Sub
    Sub Application_BeginRequest()
        Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(h.GetCulture())
        Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(h.GetCulture())
    End Sub


    Sub Session_Start()
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        If User.Identity.IsAuthenticated Then
            If Request("IdSession") IsNot Nothing Then 'asegurarnos de borrar el ticket
                db.GetIdSabFromTicket(Request("IdSession"), strCn)
            End If
            Exit Sub
        End If
        If Request("IdSession") Is Nothing Then 'No desde portal de la extranet
            log.Info("Intento de acceso fallido: " + Request.LogonUserIdentity.Name.ToLower)
            Throw New System.Security.SecurityException("No se encuentra role en los datos de usuario")
        End If
        Dim idSab
        If Request("IdSession") = "BC9448A92720EB205E80A7487E145FA60D11CEE980A9E893D9C0415C085541D0" Then
            idSab = 57783 'Ibai
        Else
            idSab = db.GetIdSabFromTicket(Request("IdSession"), strCn)
        End If

        If db.login(idSab, ConfigurationManager.AppSettings("IDRECURSO"), strCn) = 0 Then
            log.Info("Intento de acceso fallido: " + idSab.ToString)
            Throw New System.Security.SecurityException("No se encuentra role en los datos de usuario")
        End If
        log.Info("Login correct")
        h.SetCulture(idSab, strCn)
        SimpleRoleProvider.setAuthCookieWithRole(idSab, Function() h.getRole(idSab))
        Response.Redirect(Request.RawUrl, False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub
    Sub Application_PreRequestHandlerExecute()
        If Request("IdSession") IsNot Nothing AndAlso User.Identity.IsAuthenticated Then
            Dim idSab = db.GetIdSabFromTicket(Request("IdSession"), strCn)
            If idSab > 0 Then
                'Probable cambio de usuario el la extranet
                h.SetCulture(idSab, strCn)
                SimpleRoleProvider.setAuthCookieWithRole(idSab, Function() h.getRole(idSab))
                Response.Redirect(Request.RawUrl, False)
            End If
        End If
    End Sub
    Private Sub MvcApplication_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        Dim err = Server.GetLastError()
        If err.GetType().Name = "SecurityException" Then
            FormsAuthentication.SignOut()
            Response.Redirect("~/accesodenegado.htm")
            Exit Sub
        End If
        'File the error
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        log.Error(err.Message, err)
    End Sub
End Class
