' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
' visit http://go.microsoft.com/?LinkId=9394802

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Private strCn As String = ConfigurationManager.ConnectionStrings("SAB").ConnectionString

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
        'If Request("id") IsNot Nothing Then
        If User.Identity.IsAuthenticated AndAlso Request.Cookies(FormsAuthentication.FormsCookieName) Is Nothing Then
            log.Info("Session starting. We have windows loggin for " + User.Identity.Name.ToLower + " recurso " + ConfigurationManager.AppSettings("IDRECURSO"))
            Dim idSab = db.GetIdSabFromTicket(Request("id"), strCn)
            If User.Identity.Name.ToLower.Contains("diglesias") Then
                'lst(0) = 60169
                'idSab = 57544
                'idSab = 57528 'Amaia Fernandez
                'idSab = 57526 'Isaac Olabarri
            End If
            If db.login(idSab, ConfigurationManager.AppSettings("IDRECURSO"), strCn) = 0 Then
                Throw New System.Security.SecurityException("No se encuentra role en los datos de usuario")
            End If

            H.SetCulture(idSab, strCn)
            SimpleRoleProvider.SetAuthCookieWithRole(idSab, Function() 1)
        End If
    End Sub
    Private Sub MvcApplication_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        Dim err = Server.GetLastError()
        If err.GetType().Name = "SecurityException" Then
            FormsAuthentication.SignOut()
            Response.Redirect("~/accesodenegado.html")
            Exit Sub
        End If
        'File the error
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        log.Error(err.Message, err)
#If Not Debug Then
        Response.Redirect("~/error.html")
#End If
    End Sub
End Class
