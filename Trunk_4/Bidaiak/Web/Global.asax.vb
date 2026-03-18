Public Class Global_asax
    Inherits HttpApplication

    ''' <summary>
    ''' Al iniciar la aplicacion, se configura el lognet
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ScriptManager.ScriptResourceMapping.AddDefinition("jquery", New ScriptResourceDefinition With {.Path = "~/Scripts/jquery-1.6.4.js"}) 'Es para evitar un error de unobstrusive de jquery que da al poner framework 4.5
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.config"))
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)       
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)        
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)        
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)        
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)        
    End Sub

End Class