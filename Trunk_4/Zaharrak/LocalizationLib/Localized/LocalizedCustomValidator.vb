Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Globalization.CultureInfo

<Obsolete("No utilizar")> _
Public Class LocalizedCustomValidator
	Inherits CustomValidator

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

	Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
		Dim accesoGenerico As New AccesoGenerico()
		MyBase.OnLoad(e)
		MyBase.Text = "*"
		MyBase.ErrorMessage = accesoGenerico.GetTermino(Me.Key, CurrentCulture.Name)
	End Sub
End Class
