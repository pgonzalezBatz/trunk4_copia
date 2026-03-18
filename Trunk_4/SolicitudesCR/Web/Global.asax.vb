Imports log4net

Public Class Global_asax
    Inherits HttpApplication

    Private log As ILog = LogManager.GetLogger("root.SolicitudesCR")

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al comienzo de cada solicitud
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        '    log.Error("Empieza2")
        ' Se desencadena al intentar autenticar el uso
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        log.Error("Empieza")
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la sesión
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la aplicación
    End Sub

End Class