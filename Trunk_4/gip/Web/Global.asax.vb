Imports System.Net

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()

        RouteConfig.RegisterRoutes(RouteTable.Routes)
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\log4net.config"))
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        log.Info("Application starting...")

        ServicePointManager.Expect100Continue = False
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
    End Sub

    Sub Application_BeginRequest()
        Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(h.GetCulture())
        Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(h.GetCulture())
    End Sub

    Private Sub MvcApplication_AuthorizeRequest(sender As Object, e As EventArgs) Handles Me.AuthorizeRequest
        'If User.Identity.IsAuthenticated AndAlso Request.Cookies(FormsAuthentication.FormsCookieName) Is Nothing Then
        Dim lst = Db.GetIdSab(User.Identity.Name.ToLower, ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
            If Not lst.Any Then
                SimpleRoleProvider.SetAuthCookieWithRole(0, Function() Db.GetRole(0, ConfigurationManager.AppSettings.Get("recurso").Split(";").ToList, ConfigurationManager.ConnectionStrings("oracle").ConnectionString))
                h.SetCultureDirectly("es-ES")
            Else
                Dim idSab As Integer = lst(0).idSab
                idSab = 58553
                If User.Identity.Name.ToLower.Contains("abelgarcia") Then
                    'idSab = 57550 ' lpacin
                    'idSab = 62956 'Xie caigan
                    'idSab = 61340 'Andoni Atutxa                
                    'idSab = 57233 ' Unai Belaustegigoitia
                    'idSab = 57038 ' enrique bernaola
                End If
                SimpleRoleProvider.SetAuthCookieWithRole(idSab, Function() Db.GetRole(idSab, ConfigurationManager.AppSettings.Get("recurso").Split(";").ToList, ConfigurationManager.ConnectionStrings("oracle").ConnectionString))
                h.SetCulture(idSab, ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
            End If
        'End If
    End Sub

    Private Sub MvcApplication_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        Dim err = Server.GetLastError()
        If err.GetType().Name = "SecurityException" Then
            FormsAuthentication.SignOut()
            Response.Redirect("~/accesodenegado.html")
        Else
            Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
            log.Error(err.Message, err)
#If Not DEBUG Then
        Response.Redirect("~/error.html")
#End If
        End If
    End Sub

End Class
