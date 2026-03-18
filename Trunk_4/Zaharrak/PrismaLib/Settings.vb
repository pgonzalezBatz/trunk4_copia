
Imports Oracle.ManagedDataAccess.Client

Namespace My

    'Esta clase le permite controlar eventos específicos en la clase de configuración:
    ' El evento SettingChanging se desencadena antes de cambiar un valor de configuración.
    ' El evento PropertyChanged se desencadena después de cambiar el valor de configuración.
    ' El evento SettingsLoaded se desencadena después de cargar los valores de configuración.
    ' El evento SettingsSaving se desencadena antes de guardar los valores de configuración.
    Partial Friend NotInheritable Class MySettings
        Property Conexion_BBDD As String
        Property ConexionOracle As New OracleConnection

        Public Sub New()
            Dim CadenaConexion As New ConnectionStringSettings
#If DEBUG Then
            CadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings.Item("PrismaTest")
            Conexion_BBDD = If(CadenaConexion Is Nothing, My.Settings.Conexion_Test, CadenaConexion.ConnectionString)
#Else
			CadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings.Item("PrismaLive")
			Conexion_BBDD = If(CadenaConexion Is Nothing, My.Settings.Conexion_Real, CadenaConexion.ConnectionString)
#End If
            ConexionOracle.ConnectionString = Conexion_BBDD
        End Sub
    End Class
End Namespace
