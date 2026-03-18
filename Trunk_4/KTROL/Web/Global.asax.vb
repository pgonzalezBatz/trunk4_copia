Imports System.Web.SessionState
Imports KtrolLib

Public Class Global_asax
	Inherits System.Web.HttpApplication
	Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Ktrol")

	Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
		'log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.xml"))
		log4net.Config.XmlConfigurator.Configure()
	End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        Dim myTicket As New Sablib.ELL.Ticket
        Dim lg As New Sablib.BLL.LoginComponent
        Dim Recurso As String = System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb")

        myTicket = lg.Login(User.Identity.Name.ToLower)

#If DEBUG Then
        ''Try
        ''    'myTicket = lg.Login("BATZNT\talsis".ToLower) '58724
        ''myTicket = lg.Login("batznt\jonathanrodriguez".ToLower)
        'myTicket = lg.Login("batznt\diglesias")

        ''Usuario
        ''--------------------------------------------------------------------------------------------------------------
        'Dim CodPersona As Integer = myTicket.IdTrabajador
        ''Dim CodPersona As Integer = 990177 'Asier Arrondo
        '''Dim CodPersona As Integer = 3069 'Jon juaristi 
        'Dim IdPlanta As Integer = myTicket.IdPlanta '1

        'Dim oUsuarioIntranet As New SabLib.BLL.UsuariosComponent
        'Dim usuarioIntranet As SabLib.ELL.Usuario
        'Dim oUsuarios As New BLL.UsuariosBLL
        'usuarioIntranet = oUsuarioIntranet.GetUsuario(New SabLib.ELL.Usuario With {.CodPersona = CodPersona, .IdPlanta = IdPlanta}, False)
        'Dim usuario As ELL.Usuarios = oUsuarios.ObtenerUsuario(usuarioIntranet.Id)
        'Session("PerfilUsuario") = New ELL.PerfilUsuario With {.IdUsuario = usuarioIntranet.Id, .IdDepartamento = usuarioIntranet.IdDepartamento, .IdTipoTrabajador = usuario.IdRol}
        ''--------------------------------------------------------------------------------------------------------------

        ''Operacion
        ''--------------------------------------------------------------------------------------------------------------
        'Session("CodOperacion") = "8103962" '"8102526"
        ''Session("Registros") = "400834"
        'Dim consultasBLL As New BLL.ConsultasBLL
        'Dim regOperacion As String() = consultasBLL.consultarCodigoOperacion(Session("CodOperacion"))
        'If (regOperacion IsNot Nothing) Then
        '    Session("DescripcionOpe") = regOperacion(1) & " / " & regOperacion(2)
        '    Session("Info") = "KAIXO"
        'End If
        ''--------------------------------------------------------------------------------------------------------------
        ''Catch ex As Exception
        ''    log.Error(ex)
        ''End Try
#End If

        If Not myTicket Is Nothing Then
            If lg.AccesoRecursoValido(myTicket, Recurso) Then
                Session("Ticket") = myTicket

                Dim sPlanta As String()

                If (System.Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
                    sPlanta = {"1", "BATZ_IGORRE", System.Configuration.ConfigurationManager.ConnectionStrings("BATZ_IGORRE").ConnectionString().ToString}
                Else
                    sPlanta = {"1", "BATZ_IGORRE_TEST", System.Configuration.ConfigurationManager.ConnectionStrings("BATZ_IGORRE_TEST").ConnectionString().ToString}
                End If

                Session("Planta") = sPlanta
            Else
                Response.Redirect(PageBase.PAG_PERMISODENEGADO)
            End If
        End If
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
		' Se desencadena al comienzo de cada solicitud
#If Debug And Trace Then
		'Habilitamos el seguimiento para la aplicacion
		Context.Trace.IsEnabled = True
#End If
	End Sub

	Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
		' Se desencadena al intentar autenticar el uso
	End Sub

	Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando se produce un error
        'If Server.GetLastError IsNot Nothing Then log.Error(DirectCast(sender, System.Web.HttpApplication).Request.Url.ToString, Server.GetLastError)
        If Server.GetLastError IsNot Nothing Then
            Dim Request_Browser As String = ""
            With Request.Browser
                Request_Browser &= "Browser Capabilities" & vbCrLf
                Request_Browser &= "Type = " & .Type & vbCrLf
                Request_Browser &= "Name = " & .Browser & vbCrLf
                Request_Browser &= "Version = " & .Version & vbCrLf
                Request_Browser &= "Major Version = " & .MajorVersion & vbCrLf
                Request_Browser &= "Minor Version = " & .MinorVersion & vbCrLf
                Request_Browser &= "Platform = " & .Platform & vbCrLf
                Request_Browser &= "Is Beta = " & .Beta & vbCrLf
                Request_Browser &= "Is Crawler = " & .Crawler & vbCrLf
                Request_Browser &= "Is AOL = " & .AOL & vbCrLf
                Request_Browser &= "Is Win16 = " & .Win16 & vbCrLf
                Request_Browser &= "Is Win32 = " & .Win32 & vbCrLf
                Request_Browser &= "Supports Frames = " & .Frames & vbCrLf
                Request_Browser &= "Supports Tables = " & .Tables & vbCrLf
                Request_Browser &= "Supports Cookies = " & .Cookies & vbCrLf
                Request_Browser &= "Supports VBScript = " & .VBScript & vbCrLf
                Request_Browser &= "Supports JavaScript = " & .EcmaScriptVersion.ToString() & vbCrLf
                Request_Browser &= "Supports Java Applets = " & .JavaApplets & vbCrLf
                Request_Browser &= "Supports ActiveX Controls = " & .ActiveXControls &
            vbCrLf
            End With

            log.Error(DirectCast(sender, System.Web.HttpApplication).Request.Url.ToString & vbCrLf & Request_Browser, Server.GetLastError)
        End If
    End Sub

	Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la sesión           
        Session("PerfilUsuario") = Nothing
        Session("IdReferencia") = Nothing
        Session("CodOperacion") = Nothing
    End Sub

	Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
		' Se desencadena cuando finaliza la aplicación
	End Sub

#Region "Pooling"

	'''' <summary>
	'''' Realiza unas consultas a la base de datos ya que el pooling se quedaba colgado y la primera vez del dia, daba fallo.
	'''' Se recomienda uncluir esta funcion en las librerias de Acceso a Base de Datos de Oracle.
	'''' </summary>
	'''' <remarks></remarks>
	'Public Shared Sub ConsultaPooling()
	'	Try
	'		Dim connection As String
	'		If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
	'			connection = Configuration.ConfigurationManager.ConnectionStrings("SABLIVE").ConnectionString
	'		Else
	'			connection = Configuration.ConfigurationManager.ConnectionStrings("SABTEST").ConnectionString
	'		End If
	'		Memcached.OracleDirectAccess.Seleccionar("select sysdate from dual", connection)
	'	Catch
	'	End Try
	'End Sub

#End Region

End Class