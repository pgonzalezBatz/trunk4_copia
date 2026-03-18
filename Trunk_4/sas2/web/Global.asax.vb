' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
' visit http://go.microsoft.com/?LinkId=9394802

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Private strCn As String = ConfigurationManager.ConnectionStrings("sas").ConnectionString
    Private idGrupo As String = ConfigurationManager.AppSettings("IDGRUPO")

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

        If User.Identity.IsAuthenticated AndAlso Request.Cookies(FormsAuthentication.FormsCookieName) Is Nothing Then
            log.Info("Session starting. We have windows loggin for " + User.Identity.Name.ToLower + " grupo " + idGrupo.ToString)
            Dim idSab = DBAccess.login(User.Identity.Name.ToLower, ConfigurationManager.AppSettings("IDGRUPO"), strCn)
            If idSab.Length = 0 Then
                Throw New System.Security.SecurityException("No se encuentra role en los datos de usuario")
            End If
            If User.Identity.Name.ToLower.Contains("diglesias") Then
                'idSab = 58706 'Almacen mat
                'idSab = 57834 'jjvitoria
                'idSab = 57066 'ralonso
                'idSab = 61340 'andoni atutxa
                ' idSab = 64152
                'idSab = 64153
                'idSab = 56950 'gorka egileor
            End If
            h.SetCulture(idSab, strCn)
            SimpleRoleProvider.setAuthCookieWithRole(idSab, Function() DBAccess.GetRoles(idSab, strCn))
        End If
    End Sub
    Private Sub MvcApplication_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        Dim err = Server.GetLastError()
        If err.GetType().Name = "SecurityException" Then
            FormsAuthentication.SignOut()
            Session.Abandon()
            Response.Redirect("~/accesodenegado.htm")
            Exit Sub
        End If
        'File the error
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        If err.GetType().Name = "HttpException " Then
            FormsAuthentication.SignOut()
            log.Info(err.Message, err)
            Response.Redirect("~/accesodenegado.htm")
            Exit Sub
        End If
        log.Error(err.Message, err)
#If Not Debug Then
        Response.Redirect("~/error.html")
#End If
    End Sub
End Class