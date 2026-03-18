Partial Public Class _Default
    Inherits Page

    ''' <summary>
    ''' Dependiendo el perfil, redirige a una pagina u otra
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim url As String = "AMFE/ListadoAMFE.aspx"
        If Request.QueryString("tipo") IsNot Nothing AndAlso Request.QueryString("prod") IsNot Nothing Then
            url &= "?tipo=" & Request.QueryString("tipo") & "&prod=" & Request.QueryString("prod")
        End If
        Response.Redirect(url)
    End Sub

End Class