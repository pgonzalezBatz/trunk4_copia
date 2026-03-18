Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Private log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private mySession As SabLib.ELL.Ticket

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al iniciar la aplicación
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al iniciar la sesión
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))
        Try
            Dim lg As New SabLib.BLL.LoginComponent
            Dim sablibComp As New SabLib.BLL.RecursosComponent
            Dim ticket As SabLib.ELL.Ticket = lg.Login(User.Identity.Name.ToLower)
            Session(PageBase.STICKET) = ticket
            Session("usuario") = ticket.NombreCompleto
            mySession = ticket
            'Comprueba si se tiene que mostrar el icono de emails o no. De momento, se mostrará cuando no tenga ninguna limitacion de recursos el equipo.  
            If (ticket IsNot Nothing) Then
                log.Info(" * session start ticket isnot nothing getlistado:")
                Dim lRecursos = sablibComp.GetRecursosCulturaAll(ticket.IdUser, ticket.Culture)
                'Dim idRecurso = ConfigurationManager.AppSettings("idRecurso")
                'Dim exists = lRecursos.Where(Function(f) f.Id = CInt(idRecurso)).Count > 0
                'If Not exists Then
                '    Response.Redirect(PageBase.PAG_PERMISODENEGADO)
                'End If
                Session("MostrarEmail") = (lRecursos.Count = 0)
                Response.Redirect(PageBase.PAG_PERIODOCIERRE)
            Else
                log.Warn(User.Identity.Name & " no tiene acceso a la intranet")
                Response.Redirect(PageBase.PAG_PERMISODENEGADO)
            End If
        Catch batzEx As SabLib.BatzException
            log.Error("Error en en el session start de la home: " & batzEx.Termino, batzEx.Excepcion)
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        Catch ex As Exception
            log.Error("Error en el session start de la home", ex)
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End Try
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