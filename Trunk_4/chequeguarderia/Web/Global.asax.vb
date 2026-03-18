' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
' visit http://go.microsoft.com/?LinkId=9394802
Imports System.Web.Http

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Private ReadOnly strCn As String = ConfigurationManager.ConnectionStrings("SAB").ConnectionString
    Private ReadOnly idRecurso = ConfigurationManager.AppSettings("IDRECURSO")

    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        WebApiConfig.Register(GlobalConfiguration.Configuration)
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
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
            Dim idSab As Integer?
            If Request("id") Is Nothing Then
                'Directo desde la intranet
                idSab = DB.GetIdSabFromDomainUser(User.Identity.Name.ToLower, strCn)
            Else
                'Desde el portal del empleado
                idSab = DB.GetIdSabFromTicket(Request("id"), strCn)
            End If
            If idSab Is Nothing Then
                Throw New System.Security.SecurityException("No se encuentra el usuario")
            End If
            If User.Identity.Name.ToLower = "batznt\diglesias" Then
                'idSab = 57552 'Yuste
                'idSab = 34 'Bilbo
                'idSab = 57528 'Amaia
                'idSab = 58817
                'idSab = 68658 ' Ane uriona
                'idSab = 63073 ' Abel
                'idSab = 57552 ' Alberto
                'idSab = 61409 ' Batirtze
                'idSab = 60283 ' Alaitz madinabeitia
            End If
            If DB.HasRecurso(idSab.Value, ConfigurationManager.AppSettings("IDRECURSOADMIN"), strCn) Then
                SimpleRoleProvider.setAuthCookieWithRole(idSab, Function() Role.administracion, True)
            ElseIf DB.HasRecurso(idSab.Value, ConfigurationManager.AppSettings("IDRECURSO"), strCn) Then
                SimpleRoleProvider.setAuthCookieWithRole(idsab, Function() Role.normal, True)
                End If
                H.SetCulture(idsab, strCn)
                Response.Redirect(Request.RawUrl)
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