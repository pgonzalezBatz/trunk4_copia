Namespace DAL

    Public Class DALBase

#Region "Variables"

        ''' <summary>
        ''' Obtiene la cadena de conexión
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected Shared ReadOnly Property CadenaConexion As String
            Get
                Dim status As String = "TARJETASTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
                    status = "TARJETASLIVE"
                End If
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

#End Region

    End Class

End Namespace
