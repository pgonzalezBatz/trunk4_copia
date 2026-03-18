'===============================================================================
'BATZ, Koop. - 02/04/2008 12:10:34
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_View.vbgen
' El soporte de la clase  esta en el directorio Architecture  en "dOOdads".
'===============================================================================


Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

    Public Class W_PROCESOS
        Inherits _W_PROCESOS
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
		''' Realiza un filtrado o busqueda para la consulta indicada.
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
