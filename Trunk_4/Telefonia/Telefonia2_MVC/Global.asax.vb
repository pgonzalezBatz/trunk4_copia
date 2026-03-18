Imports System.Web.Mvc
Imports System.Web.Routing

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Private strCn = ConfigurationManager.ConnectionStrings("oracle").ConnectionString

    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\log4net.config"))
    End Sub

    Sub Application_BeginRequest()
        Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(h.GetCulture())
        Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(h.GetCulture())
    End Sub

    Private Sub MvcApplication_AuthorizeRequest(sender As Object, e As EventArgs) Handles Me.AuthorizeRequest
        If User.Identity.IsAuthenticated AndAlso Request.Cookies(FormsAuthentication.FormsCookieName) Is Nothing Then
            Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
            log.Info("Session starting. We have windows loggin for " + User.Identity.Name.ToLower)
            Dim lst = db.GetLogin(User.Identity.Name.ToLower, strCn)
            'Dim lst = db.GetLogin("MBTOOLING\SRodriguez".ToLower, strCn)

            If lst.Count = 0 OrElse lst.First = 0 Then
                Throw New System.Security.SecurityException("No se encuentra role en los datos de usuario")
            End If
            If User.Identity.Name.ToLower.Contains("aazkuenaga") Then
                'lst(0) = 60169
                'lst(0) = 57544
                ' lst(0) = 57526 'Isaac Olabarri
                'lst(0) = 62931
            End If
            h.SetCulture(lst.First, ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
            SimpleRoleProvider.SetAuthCookieWithRole(lst.First, Function() db.getRole(lst(0)))
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
#If Not DEBUG Then
        Response.Redirect("~/error.html")
#End If
    End Sub
End Class
