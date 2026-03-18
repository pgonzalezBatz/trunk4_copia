Imports System.Data.OracleClient

Namespace DAL

    Public Class DALBase

        Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Ktrol")

#Region "Variables"

        Private parameter As OracleParameter

        ''' <summary>
        ''' Obtiene la cadena de conexión
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected ReadOnly Property CadenaConexion As String
            Get
                Dim status As String = "KTROLTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "KTROLLIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

        ''' <summary>
        ''' Obtiene la cadena de conexión de Kaplan de Igorre
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected ReadOnly Property CnKaplanIgorre As String
            Get
                'Dim status As String = "BATZ_IGORRE_TEST"
                'If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BATZ_IGORRE"
                'Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
                Return Configuration.ConfigurationManager.ConnectionStrings("BATZ_IGORRE").ConnectionString
            End Get
        End Property

#End Region
	End Class

End Namespace
