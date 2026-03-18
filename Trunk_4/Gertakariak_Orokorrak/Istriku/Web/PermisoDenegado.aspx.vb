Public Partial Class PermisoDenegado
	Inherits PageBase

	Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
		Dim msg As String = Request.QueryString("msg")
		If Not String.IsNullOrWhiteSpace(msg) Then lblMensaje.Text = msg
	End Sub
	'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
	'	Dim Mensaje As String = "permisoDenegado".Itzuli
	'	Label1.Text = Mensaje
	'	Page.Title = Mensaje
	'End Sub

End Class