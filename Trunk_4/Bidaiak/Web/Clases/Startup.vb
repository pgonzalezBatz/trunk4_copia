Imports Microsoft.Owin
Imports Owin

<Assembly: OwinStartup(GetType(SignalR.Startup))>
Namespace SignalR
    Public Class Startup
        Public Sub Configuration(ByVal app As IAppBuilder)
            'Any connection or hub wire up and configuration should go here
            app.MapSignalR()
        End Sub

    End Class

End Namespace