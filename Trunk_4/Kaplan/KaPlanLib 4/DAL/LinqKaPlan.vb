Imports System.Web

Namespace DAL

	Public Class ELL
		Inherits System.Data.Linq.DataContext

		'Si se produce algun tipo de error en la declaracion de "Sub New" quitar la declaracion autogenerada.
		Public Sub New()
			'#If DEBUG Then
			'MyBase.New(Global.KaPlanLib.My.MySettings.Default.PAC_IGO_TESTConnectionString, mappingSource)
			'#Else
			'------------------------------------------------------------------------------------------------------------------------------------------------------
			'MyBase.New(CStr(IIf(HttpContext.Current.Session("Planta") Is Nothing, String.Empty, HttpContext.Current.Session("Planta")(2).ToString)), mappingSource)
			'------------------------------------------------------------------------------------------------------------------------------------------------------
			'FROGA:2012-10-07:
			'------------------------------------------------------------------------------------------------------------------------------------------------------
			MyBase.New(If(HttpContext.Current.Session("Planta") Is Nothing, String.Empty, HttpContext.Current.Session("Planta")(2).ToString), mappingSource)
			'------------------------------------------------------------------------------------------------------------------------------------------------------
			'#End If
			OnCreated()
		End Sub


	End Class

End Namespace