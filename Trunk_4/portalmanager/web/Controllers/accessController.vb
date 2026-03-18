Public Class accessController
    Inherits System.Web.Mvc.Controller
    Private strCn As String = ConfigurationManager.ConnectionStrings("oracle").ConnectionString

    Function Index(id As String)
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        If Not String.IsNullOrEmpty(id) Then
            Dim idsab = db.GetIdSabFromTicket(Request("id"), ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
            log.Info("Recuperado id desde ticket: " + idsab.ToString)
            If idsab = 0 Then
                log.Info("Intento de acceso fallido: " + User.Identity.Name)
                Throw New System.Security.SecurityException("No se encuentra role en los datos de usuario")
            End If
            h.SetCulture(idsab, ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
            log.Info("IdSab para cookie: " + idsab.ToString)
            SimpleRoleProvider.setAuthCookieWithRole(idsab, Function() db.GetRole(idsab, ConfigurationManager.ConnectionStrings("oracle").ConnectionString))
        Else
            h.SetCulture(SimpleRoleProvider.GetId(), ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
        End If
        Return DecidirRedireccion()
    End Function
    <AcceptVerbs(HttpVerbs.Post)>
    Function closesession() As ActionResult
        db.SetTIcket(Session.SessionID, SimpleRoleProvider.GetId(), strCn)
        Session.Abandon()
        FormsAuthentication.SignOut()
        Return Redirect("https://intranet2.batz.es/langileentxokoa/menuEmpleado.aspx?id=" + Session.SessionID)
    End Function
    <SimpleRoleProvider(Role.rrhh)>
    Function suplantarUsuario(ac As String, co As String) As ActionResult
        Return View()
    End Function
    <AcceptVerbs(HttpVerbs.Post)>
    <SimpleRoleProvider(Role.rrhh)>
    Function suplantarUsuario(newIdSab As Integer, ac As String, co As String) As ActionResult
        Dim fixedRole = SimpleRoleProvider.GetRole()
        SimpleRoleProvider.setAuthCookieWithRole(newIdSab, Function() fixedRole)
        Return RedirectToAction(If(ac, "index"), If(co, "listado"))
    End Function

    Private Function DecidirRedireccion() As ActionResult
        Select Case Request("controller")
            Case "evaluacion"
                Return RedirectToAction("index", "evaluacion")
            Case "solicitud"
                Return RedirectToAction("index", "solicitud")
            Case Else
                Return RedirectToAction("index", "listado")
        End Select
    End Function
End Class