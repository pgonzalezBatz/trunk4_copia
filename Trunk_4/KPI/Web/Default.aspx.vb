Public Class _Default
    Inherits Page

    ''' <summary>
    ''' Redirige a la pagina de modificacion de KPI
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Redirect("ModificarKPI.aspx")
    End Sub

End Class