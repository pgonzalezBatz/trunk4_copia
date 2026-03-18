' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
' visit http://go.microsoft.com/?LinkId=9394802

Imports System.Net

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()

        RouteConfig.RegisterRoutes(RouteTable.Routes)
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\log4net.config"))
    End Sub
    Sub Application_BeginRequest()
        Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(H.GetCulture())
        Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(H.GetCulture())
    End Sub
    Sub Session_Start()
        ServicePointManager.Expect100Continue = False
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        log.Info("Session starting...")
        If User.Identity.IsAuthenticated AndAlso Request.Cookies(FormsAuthentication.FormsCookieName) Is Nothing Then
            Dim idsab = 0
            If Request("id") Is Nothing Then
                'Directo desde la intranet
                idsab = db.getIdSabFromADName(User.Identity.Name.ToLower, ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
            Else
                'Desde portal del empleado
                Exit Sub
                'idsab = db.GetIdSabFromTicket(Request("id"), ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
                'log.Info("Recuperado id desde ticket: " + idsab.ToString)
            End If
            'Superuser
            If User.Identity.Name.ToLower.Contains("diglesias") Then
                'idsab = 56964 'mardaraz
                'idsab = 57028 'jose bustinza
                'idsab = 57194 'sgoikouria
                'idsab = 60831 'lapresa
                'idsab = 58118 ' ander bilbao
                'idsab = 57205 'Aitor Lekue
                'idsab = 62541
                'idsab = 58436
                'idsab = 57532 'Ortuzar
                'idsab = 57526 'Issac Olabarri
                'idsab = 59578 'Edurne
                'idsab = 57580  'Gizane
                'idsab = 57236 'jaime
                'idsab = 57527 'Olivares
                'idsab = 58118 'Ander bilbao
                'idsab = 57545
                'idsab = 57155 'Barroeta
                'idsab = 61432 'Olatz
                'idsab = 57197 'Loli
                'idsab = 57535 'Jorge castillo
                'idsab = 57077 'Petra
                'idsab = 57154 'Torres
                'idsab = 56944 'Larri
                'idsab = 60735
                'idsab = 57545 'Jonathan
                'idsab = 57953
                'idsab = 57028
                'idsab = 61512 'Laura bolinaga
                'idsab = 65852 'Asier Azkuenaga
                'idsab = 60277 'camacho
                'idsab = 67799 'Judith Manzanos
                'idsab = 67366 'Iñaki diez
                'idsab = 67146 'Olaskoaga
                'idsab = 57200
                'idsab = 69086 'Gonzalo Villarroya
                'idsab = 57042 'Carlos grande
                'dsab = 57247 'Egoitz anzola
                'idsab = 58445 'Carmelo Muga
                'idsab = 59336 'marta amo
            End If
            If idsab = 0 Then
                log.Info("Intento de acceso fallido: " + User.Identity.Name)
                Throw New System.Security.SecurityException("No se encuentra role en los datos de usuario")
            End If

            H.SetCulture(idsab, ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
            log.Info("IdSab para cookie: " + idsab.ToString)
            SimpleRoleProvider.SetAuthCookieWithRole(idsab, Function() db.GetRole(idsab, ConfigurationManager.ConnectionStrings("oracle").ConnectionString))
        ElseIf User.Identity.IsAuthenticated Then
            H.SetCulture(SimpleRoleProvider.GetId(), ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
        End If
    End Sub
    Private Sub MvcApplication_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        Dim err = Server.GetLastError()
        If err.GetType().Name = "SecurityException" Then
            FormsAuthentication.SignOut()
            Session.Abandon()
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
