Public Partial Class DefaultAdm
    Inherits PageBase

    ''' <summary>
    ''' Se comprueba si es administrador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not (Master.Ticket.EsAdministrador OrElse Master.Ticket.EsAdministradorPlanta) Then
                Response.Redirect(PageBase.PAG_PERMISODENEGADO, False)
            End If
        End If
    End Sub

End Class