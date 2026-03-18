' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
' visit http://go.microsoft.com/?LinkId=9394802
Public Class MvcApplication
    Inherits System.Web.HttpApplication
    Private strCn = ConfigurationManager.ConnectionStrings("salmentafakturazioa").ConnectionString
    Private idRecurso = ConfigurationManager.AppSettings.Get("IDRECURSO")
    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\log4netConfig.config"))
    End Sub
    Sub Application_BeginRequest()
        Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(h.GetCulture())
        Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(h.GetCulture())
    End Sub
    Private Sub MvcApplication_AuthorizeRequest(sender As Object, e As EventArgs) Handles Me.AuthorizeRequest
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")

        If User.Identity.IsAuthenticated AndAlso Request.Cookies(FormsAuthentication.FormsCookieName) Is Nothing Then
            log.Info("Session starting. We have windows loggin for " + User.Identity.Name.ToLower + " recurso " + idRecurso.ToString)
            Dim lst = DB.GetUsuario("batznt\slanbarri", idRecurso, strCn)
            If lst.Count = 0 Then
                Throw New System.Security.SecurityException("No se encuentra role en los datos de usuario")
            End If
            If User.Identity.Name.ToLower.Contains("aazkuenaga") Then
                'lst(0) = 60169
                'lst(0) = 57544
                'lst(0) = 57526 'Isaac Olabarri
            End If
            h.SetCulture(lst.First, strCn)
            SimpleRoleProvider.SetAuthCookieWithRole(lst.First, Function() Roles.normal)
        End If
    End Sub
    Private Sub MvcApplication_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        Dim err = Server.GetLastError()
        Select Case err.GetType().Name
            Case "SecurityException"
                Response.Redirect("~/accesodenegado.htm")
                Exit Sub
        End Select
        'File the error
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        log.Error(err.Message, err)
#If Not DEBUG Then
        Response.Redirect("~/error.html")
#End If
    End Sub
End Class
