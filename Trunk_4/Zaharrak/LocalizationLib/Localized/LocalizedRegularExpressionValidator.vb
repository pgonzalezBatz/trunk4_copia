Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Globalization.CultureInfo

<Obsolete("No utilizar", False)> _
Public Class LocalizedRegularExpressionValidator
	Inherits RegularExpressionValidator

#Region "Fields and Properties"
	Private _key As String
	Private _validationKey As String

	Public Property Key() As String
		Get
			Return _key
		End Get
		Set(ByVal Value As String)
			_key = Value
		End Set
	End Property
	Public Property ValidationKey() As String
		Get
			Return _validationKey
		End Get
		Set(ByVal value As String)
			_validationKey = value
		End Set
	End Property
#End Region

	Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
		Dim accesoGenerico As New AccesoGenerico()
		Dim regularExpression As String = ""
		If Not Me.ValidationKey Is Nothing Then
			regularExpression = accesoGenerico.GetRegex(Me.ValidationKey, CurrentCulture.Name)
		End If
		If regularExpression.Length > 0 Then
			MyBase.ValidationExpression = regularExpression
		End If
		MyBase.OnInit(e)
	End Sub

	Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
		Dim accesoGenerico As New AccesoGenerico()
		MyBase.Text = "*"
		MyBase.ErrorMessage = accesoGenerico.GetTermino(Me.Key, CurrentCulture.Name)
		MyBase.Render(writer)
	End Sub
End Class
