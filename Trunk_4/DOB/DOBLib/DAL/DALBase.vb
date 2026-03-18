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
                Dim status As String = "DOBTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "DOBLIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

        ''' <summary>
        ''' Obtiene la cadena de conexión
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected Shared ReadOnly Property CadenaConexionGestIKS As String
            Get
                Dim status As String = "GESTIONIKSTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "GESTIONIKSLIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

#End Region

    End Class

End Namespace
