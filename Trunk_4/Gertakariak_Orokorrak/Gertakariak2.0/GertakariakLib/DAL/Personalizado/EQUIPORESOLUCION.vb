Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized
Imports AccesoAutomaticoBD

Imports log4net
NameSpace DAL

Public Class EQUIPORESOLUCION 
	Inherits _EQUIPORESOLUCION
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

        Public Function FiltrarPor(ByVal Filtro As ELL.Filtrado) As IDataReader
            Try
                Return MyBase.LoadFromSqlReader(Filtro.SelectFromWhere("EQUIPORESOLUCION"), Nothing, CommandType.Text)
            Catch ex As Exception
                Throw (ex)
            End Try
        End Function
End Class

End NameSpace
