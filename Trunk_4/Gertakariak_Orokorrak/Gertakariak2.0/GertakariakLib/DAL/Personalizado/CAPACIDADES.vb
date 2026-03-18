'===============================================================================
'BATZ, Koop. - 01/12/2008 8:44:56
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_View.vbgen
' El soporte de la clase  esta en el directorio Architecture  en "dOOdads".
'===============================================================================


Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

Public Class CAPACIDADES 
	Inherits _CAPACIDADES
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


		Public Function Consultar(ByVal CAPID As String) As ELL.gtkCapacidad
			Dim tCapacidades As New DAL.CAPACIDADES
			Dim gtkCapacidad As New ELL.gtkCapacidad

			tCapacidades.LoadByPrimaryKey(CAPID.Trim)

			If tCapacidades.EOF Then
				gtkCapacidad = Nothing
			Else
				gtkCapacidad.CAPID = tCapacidades.CAPID
				gtkCapacidad.NOMBRE = tCapacidades.NOMBRE
				gtkCapacidad.OBSOLETO = tCapacidades.OBSOLETO
			End If

			Return gtkCapacidad
		End Function

End Class

End NameSpace
