Public Partial Class PermisoDenegado
	Inherits Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim Mensaje As String = "permisoDenegado".Itzuli
		Label1.Text = Mensaje
		Page.Title = Mensaje
	End Sub

End Class