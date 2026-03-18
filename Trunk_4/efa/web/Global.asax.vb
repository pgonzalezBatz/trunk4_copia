' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
' visit http://go.microsoft.com/?LinkId=9394802
Imports System.Web.Http

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Private IdGrupo As Integer = ConfigurationManager.AppSettings.Get("GrupoAdmin")
    Private strCnSab As String = ConfigurationManager.ConnectionStrings("SAB").ConnectionString

    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()

        RouteConfig.RegisterRoutes(RouteTable.Routes)
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\log4netConfig.config"))
    End Sub
    Sub Session_Start()
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        If User.Identity.IsAuthenticated AndAlso Request.Cookies(FormsAuthentication.FormsCookieName) Is Nothing Then
            Dim idsab = 0
            Dim lst = DB.GetLoginDominio(User.Identity.Name.ToLower, strCnSab)
            If lst.Count = 1 Then
                idsab = lst.First
            End If
            'Superuser
            If User.Identity.Name.ToLower.Contains("aazkuenaga") Then
                idsab = 59568 'touch
            End If

            If idsab = 0 Then
                log.Info("Intento de acceso fallido: " + User.Identity.Name)
                Throw New System.Security.SecurityException("No se encuentra role en los datos de usuario")
            End If

            h.SetCulture(idsab, strCnSab)
            SimpleRoleProvider.setAuthCookieWithRole(idsab, Function() DB.GetRole(idsab, strCnSab))
            'Response.Redirect(Request.RawUrl)
        End If
    End Sub
    Sub Application_AcquireRequestState()
        If Request.Cookies("cultura") IsNot Nothing Then
            Threading.Thread.CurrentThread.CurrentCulture = Globalization.CultureInfo.CreateSpecificCulture(h.GetCulture())
        End If
    End Sub
    Private Sub MvcApplication_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        Dim err = Server.GetLastError()
        If err.GetType().Name = "SecurityException" Then
            FormsAuthentication.SignOut()
            Response.Redirect("~/accessodenegado.html")
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
