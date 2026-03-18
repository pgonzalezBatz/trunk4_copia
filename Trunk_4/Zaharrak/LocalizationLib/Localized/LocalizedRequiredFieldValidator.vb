Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Globalization.CultureInfo

<Obsolete("No utilizar", False)> _
Public Class LocalizedRequiredFieldValidator
	Inherits RequiredFieldValidator

#Region "Fields and Properties"
	Private _key As String

	Public Property Key() As String
		Get
			Return _key
		End Get
		Set(ByVal Value As String)
			_key = Value
		End Set
	End Property

#End Region

	Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
		Dim accesoGenerico As New AccesoGenerico()
		MyBase.Text = "*"
		MyBase.ErrorMessage = accesoGenerico.GetTermino(Me.Key, CurrentCulture.Name)
		MyBase.Render(writer)
	End Sub
End Class