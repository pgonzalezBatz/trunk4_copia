Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class DALBase

#Region "Variables"

        Private parameter As OracleParameter

        ''' <summary>
        ''' Obtiene la cadena de conexión
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected Shared ReadOnly Property CadenaConexion As String
            Get
                Dim status As String = "COSTCARRIERSTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
                    status = "COSTCARRIERSLIVE"
                End If
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

        ''' <summary>
        ''' Obtiene la cadena de conexión de BRAIN
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected Shared ReadOnly Property CadenaConexionBRAIN As String
            Get
                Return Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
            End Get
        End Property

        ''' <summary>
        ''' Obtiene la cadena de conexión de Navision
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected Shared ReadOnly Property CadenaConexionNavisionZamudio As String
            Get
                Return Configuration.ConfigurationManager.ConnectionStrings("NavisionZamudio").ConnectionString
            End Get
        End Property

#End Region

    End Class

End Namespace
