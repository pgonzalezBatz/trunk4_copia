Public Partial Class PermisoDenegado
    Inherits Page

    Private itzultzaileWeb As New TraduccionesLib.itzultzaile

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelSinAcceso)
        End If
    End Sub

End Class