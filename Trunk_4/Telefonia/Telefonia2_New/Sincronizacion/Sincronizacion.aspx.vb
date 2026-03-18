Imports TelefoniaLib

Partial Public Class Sincronizacion
    Inherits PageBase

    ''' <summary>
    ''' Realiza la sincronizacion: Muestra las bajas realizadas de personas que tengan extensiones asignadas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                Master.SetTitle = "Sincronizar Geminix"
                Dim extComp As New BLL.ExtensionComponent
                Dim dtSincro As DataTable = extComp.SincronizacionGeminix(Master.Ticket.IdPlantaActual)
                gvSincro.DataSource = dtSincro
                gvSincro.DataBind()
            Catch ex As Exception
                Dim batzEx As New BatzException("errSincronizacion", ex)
                Master.MensajeError = batzEx.Termino
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Informa la accion a realizar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvSincro_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSincro.RowDataBound
        Try
            If (e.Row.RowType = DataControlRowType.Header) Then
                itzultzaileWeb.TraducirWebControls(e.Row.Controls)
            ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim row As DataRowView = CType(e.Row.DataItem, DataRowView)
                Dim lblAccion As Label = CType(e.Row.FindControl("lblAccion"), Label)
                Dim lblExtenTlfno As Label = CType(e.Row.FindControl("lblExtenTlfno"), Label)
                'Accion a realizar
                If ((row("Extensiones") IsNot DBNull.Value AndAlso row("Extensiones").ToString() <> String.Empty) And (row("Numero") IsNot DBNull.Value AndAlso row("Numero").ToString() <> String.Empty)) Then
                    lblAccion.Text = "libereExtensionesYTelefonos"
                    lblExtenTlfno.Text = row("Extensiones") & "," & row("Numero")
                ElseIf (row("Extensiones") IsNot DBNull.Value AndAlso row("Extensiones").ToString <> String.Empty) Then
                    lblAccion.Text = "libereExtensiones"
                    lblExtenTlfno.Text = row("Extensiones")
                ElseIf (row("Numero") IsNot DBNull.Value AndAlso row("Numero").String <> String.Empty) Then
                    lblAccion.Text = "libereTelefonos"
                    lblExtenTlfno.Text = row("Numero")
                End If
                itzultzaileWeb.Itzuli(lblAccion)
            End If
        Catch
        End Try
    End Sub
End Class