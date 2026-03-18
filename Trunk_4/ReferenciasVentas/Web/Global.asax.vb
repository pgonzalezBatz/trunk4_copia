Imports System.Web.SessionState

Public Class Global_asax
	Inherits System.Web.HttpApplication

	Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al iniciar la aplicación
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))

    End Sub

	Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al iniciar la sesión
        Dim myTicket As New SabLib.ELL.Ticket
        Dim lg As New SabLib.BLL.LoginComponent
        Dim Recurso As String = System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb")

#If DEBUG Then
        'myTicket = lg.Login(User.Identity.Name.ToLower)
        myTicket = lg.Login("batznt\jmsanchez")
#Else
        myTicket = lg.Login(User.Identity.Name.ToLower)
#End If

        Session("Ticket") = Nothing
        If myTicket IsNot Nothing Then
            If lg.AccesoRecursoValido(myTicket, Recurso) Then
                myTicket.Culture = "en-GB"
                Dim perfilUsuarioBLL As New BLL.PerfilUsuariosBLL()
                Session("PerfilUsuario") = perfilUsuarioBLL.CargarPerfilUsuario(myTicket.IdUser)
                Session("Ticket") = myTicket
            Else
                Response.Redirect(PageBase.PAG_PERMISODENEGADO)
            End If
        End If
	End Sub

	Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al comienzo de cada solicitud
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
