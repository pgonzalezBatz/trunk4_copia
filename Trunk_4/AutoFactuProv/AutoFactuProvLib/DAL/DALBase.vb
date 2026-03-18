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
                Dim status As String = "EROSKETAKTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "EROSKETAKLIVE"
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
        ''' Obtiene la cadena de conexión
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected Shared ReadOnly Property CadenaConexionIntegraFactu As String
            Get
                Return Configuration.ConfigurationManager.ConnectionStrings("INTEGRAFACTULIVE").ConnectionString
            End Get
        End Property

        ''' <summary>
        ''' Obtiene la cadena de conexión
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected Shared ReadOnly Property CadenaConexionNavisionIgorre As String
            Get
                Return Configuration.ConfigurationManager.ConnectionStrings("NAVBATZIGORREREAD").ConnectionString
            End Get
        End Property

#End Region

    End Class

End Namespace
