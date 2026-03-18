Imports TelefoniaLib

Public Class Prefijo
    Inherits PageBase

    Private tlfnoBLL As New BLL.TelefonoComponent

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(labelPrefijo)
            itzultzaileWeb.Itzuli(labelCom) : itzultzaileWeb.Itzuli(btnGuardar)
        End If
    End Sub

    ''' <summary>
    ''' Carga el prefijo de la planta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Prefijo"
                txtPrefijo.Text = tlfnoBLL.getPrefijo(Master.Ticket.IdPlantaActual)
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se guarda el prefijo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            tlfnoBLL.savePrefijo(Master.Ticket.IdPlantaActual, txtPrefijo.Text.Trim)
            If (txtPrefijo.Text.Trim <> String.Empty) Then
                log.Info("Se ha guardado el prefijo " & txtPrefijo.Text.Trim & " de la planta " & Master.Ticket.IdPlantaActual)
            Else
                log.Info("Se ha borrado el prefijo de la planta " & Master.Ticket.IdPlantaActual)
            End If
            Master.MensajeInfo = "datosGuardados"
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

End Class