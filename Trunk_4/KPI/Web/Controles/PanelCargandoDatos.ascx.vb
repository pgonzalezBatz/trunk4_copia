Partial Public Class PanelCargandoDatos
	Inherits System.Web.UI.UserControl

    Private itzultzaileWeb As New itzultzaile

    ''' <summary>
    ''' Traduce el pop up de cargando datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param> 
    Private Sub UpdateProg1_PreRender(sender As Object, e As EventArgs) Handles UpdateProg1.PreRender
        If Not Page.IsPostBack Then
            Dim up As UpdateProgress = CType(sender, UpdateProgress)
            Dim lbl As Label = CType(up.FindControl("lblCargandoDatos"), Label)
            itzultzaileWeb.Itzuli(lbl)
        End If
    End Sub
End Class