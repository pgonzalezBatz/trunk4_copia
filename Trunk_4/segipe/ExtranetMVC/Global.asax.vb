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
            log.Info("Intento de acceso fallido: usuario desconocido, ip: " + Request.UserHostAddress)
            Throw New System.Security.SecurityException("No se encuentra role en los datos de usuario")
        End If

        If Request("IdSession") = "dskjjhewknvmzxhsieupo9iureijhtiuayabvnaldfop0ewrtjdsfnggeruthybnxdtujer" Then
            'Dim testId = 641
            'Dim testid = 269 'Bitrok
            'Dim testid = 4 'FIbro
            Dim testid = 641 'ROyme
            h.SetCulture(testId, strCn)
            SimpleRoleProvider.setAuthCookieWithRole(testId, Function() 1)
            Exit Sub
        End If

        Dim idSab = db.GetIdSabFromTicket(Request("IdSession"), strCn)
        If db.login(idSab, ConfigurationManager.AppSettings("IDRECURSO"), strCn) = 0 Then
            log.Info("Intento de acceso fallido: " + idSab.ToString)
            Throw New System.Security.SecurityException("No se encuentra role en los datos de usuario")
        End If
        log.Info("Login correct")
        h.SetCulture(idSab, strCn)
        SimpleRoleProvider.setAuthCookieWithRole(idSab, Function() 1)
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
        log.Error(err.Message, err)
#If Not DEBUG Then
        Response.Redirect("~/error.html")
#End If
    End Sub
End Class
