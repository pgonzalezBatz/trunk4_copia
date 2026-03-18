Imports System.Net
Imports System.Web.Mvc
Imports System.Web.Routing

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Private idRecurso = ConfigurationManager.AppSettings.Get("IDRECURSO")
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
    Sub Session_Start()
        ServicePointManager.Expect100Continue = False
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
        If User.Identity.IsAuthenticated AndAlso Request.Cookies(FormsAuthentication.FormsCookieName) Is Nothing Then
            Dim idsab = 0
            idsab = db.GetUsuario(User.Identity.Name.ToLower, idRecurso, strCn)
            If User.Identity.Name.ToLower.Contains("aazkuenaga") Then
                'Superuser
            End If
            If idsab = 0 Then
                Throw New System.Security.SecurityException("No se encuentra role en los datos de usuario")
            End If
            h.SetCulture(idsab, ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
            SimpleRoleProvider.SetAuthCookieWithRole(idsab, Function() If(ConfigurationManager.AppSettings("Validadores").Split(";").Contains(idsab), Roles.validador + Roles.interno, Roles.interno))
            Response.Redirect("~/")
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
