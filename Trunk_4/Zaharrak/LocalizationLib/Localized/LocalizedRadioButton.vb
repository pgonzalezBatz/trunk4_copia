Imports System.Web.UI.WebControls
Imports System.Globalization.CultureInfo

<Obsolete("No utilizar")> _
Public Class LocalizedRadioButton
	Inherits RadioButton

	Private _key As String
	Public Property Key() As String
		Get
			Return _key
		End Get
		Set(ByVal value As String)
			_key = value
		End Set
	End Property

	Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
		Dim accesoGenerico As New AccesoGenerico()
		MyBase.Text = accesoGenerico.GetTermino(Me.Key, CurrentCulture.Name)
		MyBase.Render(writer)
	End Sub
End Class
