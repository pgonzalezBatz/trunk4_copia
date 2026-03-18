

Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

Public Class USUARIOSROL 
	Inherits _USUARIOSROL
	Private Log As ILog = LogManager.GetLogger("root.Gertakariak")
	Public Sub New()
		'Decide connection string depending on state
		Try
			If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
				Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("GERTAKARIAKLIVE").ConnectionString
			Else
				Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("GERTAKARIAKTEST").ConnectionString
			End If
		Catch ex As Exception
			Log.Error("Error al inicializar el connection string the Gertakariak.", ex)
		End Try
	End Sub

End Class

End NameSpace
