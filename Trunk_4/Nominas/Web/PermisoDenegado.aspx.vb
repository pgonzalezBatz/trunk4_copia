Public Class PermisoDenegado
    Inherits Page

    ''' <summary>
    ''' Se carga la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim itzultzaileWeb As New LocalizationLib.Itzultzaile
        Dim mensaje As String = itzultzaileWeb.Itzuli("permisoDenegado")
        itzultzaileWeb.Itzuli(btnVolver)
        Master.NotShowHeader()
        Master.SetTitle = "Permiso denegado"
        lblMensaje.Text = mensaje
    End Sub

    ''' <summary>
    ''' Vuelve al portal del empleado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Response.Redirect("/LangileenTxokoa")
    End Sub

End Class