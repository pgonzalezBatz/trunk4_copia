Imports System.Data.OracleClient

Namespace DAL

    Public Class DALBase

#Region "Variables"

        Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.ReferenciasVenta")

        'Private parameter As OracleParameter

        ''' <summary>
        ''' Obtiene la cadena de conexión de Referencias de venta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected ReadOnly Property CadenaConexionReferenciasVenta As String
            Get
                Dim status As String = "SOLSISTEMASTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "SOLSISTEMASLIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

        ''' <summary>
        ''' Obtiene la cadena de conexión de SAB
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected ReadOnly Property CadenaConexionSAB As String
            Get
                Dim status As String = "SABTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "SABLIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

        ''' <summary>
        ''' Obtiene la cadena de conexión de SAB
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected ReadOnly Property CadenaConexionBonoSis As String
            Get
                Dim status As String = "BONOSISTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BONOSISLIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

#End Region

    End Class

End Namespace
