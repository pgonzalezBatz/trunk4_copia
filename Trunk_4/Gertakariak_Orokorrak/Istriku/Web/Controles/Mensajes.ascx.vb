Public Class Mensajes
    Inherits System.Web.UI.UserControl

#Region "Propiedades"
#End Region
#Region "Eventos de Pagina"
    'Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
    '    Dim Explorador As HttpBrowserCapabilities = Request.Browser
    '    If Explorador.Browser = "InternetExplorer" AndAlso CInt(Explorador.Version) < 11 Then
    '        rce_pnlMensaje.Enabled = False
    '    End If
    'End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    'End Sub

    'Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

    'End Sub
#End Region

#Region "Funciones y Procesos"
    Public Sub MensajeError(ex As Exception)
        imgVentana.ImageUrl = "~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/warning-icon24.png"
        imgVentana.ToolTip = "Error critico"
        Mensaje(ex.Message)
    End Sub
    Public Sub MensajeError(ex As ApplicationException)
        imgVentana.ImageUrl = "~/App_Themes/Batz/IconosAjaxControl/VentanaMensajes/info-icon24.png"
        imgVentana.ToolTip = "Advertencia para el usuario"
        Mensaje(ex.Message)
    End Sub

    Private Sub Mensaje(Texto As String)
		lblMensaje.Text = Texto.Replace(vbCrLf, "<br/>")
        mpe_pnlMensaje.Show()
    End Sub
#End Region
End Class