Imports System.Globalization

Public Class Global_asax
    Inherits HttpApplication

    ''' <summary>
    ''' Al iniciar la aplicacion, se configura el lognet
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim myTicket As SabLib.ELL.Ticket
            Dim lg As New SabLib.BLL.LoginComponent
            Dim Recurso As String = ConfigurationManager.AppSettings.Get("RecursoWeb")
            log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))
            Session("Ticket") = Nothing
            myTicket = lg.Login(User.Identity.Name.ToLower)
            Threading.Thread.CurrentThread.CurrentCulture = New Globalization.CultureInfo(myTicket.Culture)
            If Not myTicket Is Nothing Then
                If Not lg.AccesoRecursoValido(myTicket, Recurso) Then
                    Try
                        PageBase.WriteLog("El usuario no tiene acceso al recurso", PageBase.TipoLog.Warn)
                        Response.Redirect("~/PermisoDenegado.aspx?mensa=2", True)
                    Catch ex As Exception 'Para que no de el error de Subproceso anulado
                    End Try
                End If
            Else
                Try
                    PageBase.WriteLog("La persona no puede navegar por la aplicacion. Su ticket es nothing", PageBase.TipoLog.Warn)
                    Response.Redirect("~/PermisoDenegado.aspx?mensa=1", True)
                Catch ex As Exception 'Para que no de el error de Subproceso anulado                    
                End Try
            End If
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Session("Ticket") = myTicket
            Session("IdPlanta") = myTicket.IdPlanta
            Dim idAdmins As String() = ConfigurationManager.AppSettings("Administradores").Split(",")
            For Each admin As String In idAdmins
                If (CInt(admin) = myTicket.IdUser) Then
                    Session("Admin") = True
                    Exit For
                End If
            Next
            PageBase.log.Info("Login - " & User.Identity.Name.ToLower & " (" & Server.MachineName & ")")
            Response.Redirect(Request.Url.AbsoluteUri)
        Catch ex As Exception
            PageBase.WriteLog("Error en Index.aspx", PageBase.TipoLog.Err, ex)
            Response.Redirect("~/PermisoDenegado.aspx?mensa=3", False)
        End Try
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al intentar autenticar el uso
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando se produce un error
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la sesión
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la aplicación
    End Sub

End Class