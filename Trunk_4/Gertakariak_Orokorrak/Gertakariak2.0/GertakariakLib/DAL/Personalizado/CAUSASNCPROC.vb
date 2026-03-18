

Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

Public Class CAUSASNCPROC 
	Inherits _CAUSASNCPROC
        Private Log As ILog = LogManager.GetLogger("root.GertakariakLib")
	Public Sub New()
		'Decide connection string depending on state
		Try
			If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
					Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("ConexionWeb_LIVE").ConnectionString
			Else
					Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("ConexionWeb_TEST").ConnectionString
			End If
		Catch ex As Exception
			Log.Error("Error al inicializar el connection string the Gertakariak.", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Realiza un filtrado o busqueda de las incidencias dependiendo de los parametros introducidos
        ''' </summary>
        ''' <param name="SQL">Consulta</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FiltrarPor(ByVal SQL As String) As IDataReader
            Try
                Return MyBase.LoadFromSqlReader(SQL, Nothing, CommandType.Text)
            Catch ex As Exception
                Throw (ex)
            End Try
        End Function

End Class

End NameSpace
