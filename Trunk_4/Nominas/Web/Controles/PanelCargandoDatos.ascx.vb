Public Class PanelCargandoDatos
    Inherits UserControl

    Private itzultzaileWeb As New LocalizationLib.Itzultzaile

    ''' <summary>
    ''' Se asigna un texto al texto
    ''' </summary>
    Public WriteOnly Property Text() As String
        Set(ByVal value As String)
            Dim lbl As Label = CType(UpdateProg1.FindControl("lblCargandoDatos"), Label)
            lbl.Text = value
        End Set
    End Property

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