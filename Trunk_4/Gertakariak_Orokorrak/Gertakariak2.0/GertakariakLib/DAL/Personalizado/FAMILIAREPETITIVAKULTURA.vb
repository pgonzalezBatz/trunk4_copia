'===============================================================================
'BATZ, Koop. - 28/02/2008 9:41:44
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_View.vbgen
' El soporte de la clase  esta en el directorio Architecture  en "dOOdads".
'===============================================================================


Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

Public Class FAMILIAREPETITIVAKULTURA 
	Inherits _FAMILIAREPETITIVAKULTURA
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

End Class

End NameSpace
