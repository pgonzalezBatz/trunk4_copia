Public Class ResultadoLiq
    Inherits UserControl

#Region "Propiedades"

    Private itzultzaileWeb As New itzultzaile

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    ''' <summary>
    ''' Proceso para que se inicie el control
    ''' </summary>
    ''' <param name="resul">Si ha ido OK o no</param>
    ''' <param name="mensa">Mensaje del resultado</param>
    ''' <param name="idLiq">Id de la liquidacion</param>
    Public Sub Iniciar(ByVal resul As Boolean, ByVal mensa As String, Optional ByVal idLiq As Integer = 0)
        pnlResul.CssClass = If(resul, "alert alert-success", "alert alert-danger")
        lblMensaje.Text = mensa
        divRedirigirLiq.Visible = resul
        btnRedirigir.CommandArgument = idLiq
    End Sub

    ''' <summary>
    ''' Se redirige al resultado de la liquidacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRedirigir_Click(sender As Object, e As EventArgs) Handles btnRedirigir.Click
        Response.Redirect("../VerLiquidacionesMetalico.aspx?idLiq=" & btnRedirigir.CommandArgument, False)
    End Sub

End Class