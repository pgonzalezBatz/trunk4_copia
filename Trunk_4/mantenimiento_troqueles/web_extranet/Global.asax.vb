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
        Dim sessionId = Session.SessionID
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        If User.Identity.IsAuthenticated Then
            Exit Sub
        End If
        If Request("IdSession") Is Nothing Then 'No desde portal de la extranet
            log.Info("Intento de acceso fallido: " + Request.LogonUserIdentity.Name.ToLower)
            Throw New System.Security.SecurityException("No se encuentra role en los datos de usuario")
        End If
        Dim idSab
        If Request("IdSession") = "BC9448A92720EB205E80A7487E145FA60D11CEE980A9E893D9C0415C085541D0" Then
            'idSab = 57745 'Arrondo            
            'idSab = 381 'Eretza
            'idSab = 386 'erlas
            'idSab = 63259 'TTN
            'idSab = 699 'TT Goiko
            'idSab = 559 'Metaltermica
            'idSab = 64227 'Josdan
            'idSab = 274 'Oerlikon spain
            'idSab = 715 ' Tecnocrom
            'idSab = 400 ' Funsan
            'idSab = 414 'Funbarri
            'idSab = 66267 'hirucoat
            'idSab = 549 'Arratxa
            'idSab = 412 'FAE DSL
            'idSab = 62949 ' ormaurre
            'idSab = 69167 'Lasertool
            'idSab = 274 'Oerlikon
        Else
            idSab = db.GetIdSabFromTicket(Request("IdSession"), strCn)
        End If
        If db.login(idSab, ConfigurationManager.AppSettings("IDRECURSO"), strCn) = 0 Then
            log.Info("Intento de acceso fallido: " + idSab.ToString)
            Throw New System.Security.SecurityException(String.Format("No se encuentra role en los datos de usuario idSab: {0}", idSab))
        End If
        h.SetCulture(idSab, ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
        SimpleRoleProvider.SetAuthCookieWithRole(idSab, Function() Roles.externo)
        Response.Redirect("~/")
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
