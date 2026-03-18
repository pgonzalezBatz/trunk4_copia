'Imports System.Data.OracleClient

Namespace DAL

    Public Class DALBase

#Region "Variables"

        Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.KaplanNew")

        'Private parameter As OracleParameter

        Protected Shared ReadOnly Property Conexion As String
            Get
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
                    Return Configuration.ConfigurationManager.ConnectionStrings.Item("KAPLANLIVE").ConnectionString
                Else
                    Return Configuration.ConfigurationManager.ConnectionStrings.Item("KAPLANTEST").ConnectionString
                End If


            End Get
        End Property



        'Private parameter As OracleParameter

        ''' <summary>
        ''' Obtiene la cadena de conexión de Referencias de venta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected ReadOnly Property CadenaConexionSQL As String
            Get
                Dim status As String = "KAPLANSQL"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "KAPLANSQL"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

        Protected ReadOnly Property CadenaConexion As String
            Get
                Dim status As String = "KAPLANTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "KAPLANLIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property
        ''' <summary>
        ''' Obtiene la cadena de conexión de Referencias de venta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected ReadOnly Property CadenaConexionXBAT As String
            Get
                Dim status As String = "EMPTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "EMPLIVE"
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
        Protected ReadOnly Property CadenaConexionIZARO As String
            Get
                Dim status As String = "IZAROTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "IZAROLIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

        ''' <summary>
        ''' Obtiene la cadena de conexión de IZARO
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Protected ReadOnly Property CadenaConexionSAS As String
            Get
                Dim status As String = "IZAROTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "SASLIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

#End Region

    End Class

End Namespace
