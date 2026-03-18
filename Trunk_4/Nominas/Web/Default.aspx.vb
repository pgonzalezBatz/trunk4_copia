Public Partial Class _Default
    Inherits PageBase

    ''' <summary>
    ''' Redirige a la pagina de Consultar nominas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Response.Redirect("ConsultarNominas.aspx")
        End If
    End Sub
End Class