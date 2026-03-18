Public Partial Class _Default
    Inherits PageBase

    ''' <summary>
    ''' Redirecciona al listado de usuarios
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Response.Redirect("~\Usuarios\Usuarios.aspx")
    End Sub

End Class