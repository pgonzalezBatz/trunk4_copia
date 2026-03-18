Public Class MonedasAnticipo
    Inherits PageBase

    ''' <summary>
    ''' Muestra todos los estados de los proyectos existentes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Monedas anticipo"
            loadMonedas()
        End If
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Parametros_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(labelDivCabMonSel) : itzultzaileWeb.Itzuli(labelDivCabMonNoSel)
        End If
    End Sub

    ''' <summary>
    ''' Carga las monedas seleccionables como las no seleccionables por los anticipos
    ''' </summary>
    Private Sub loadMonedas()
        Try
            Dim xbatBLL As New BLL.XbatBLL
            Dim lMonedas As List(Of ELL.Moneda) = xbatBLL.GetMonedas()
            Dim lMonedasAnt As List(Of ELL.Moneda) = xbatBLL.GetMonedas(True, Master.IdPlantaGestion)
            Dim idMon As Integer
            For indexMon As Integer = lMonedas.Count - 1 To 0 Step -1
                idMon = lMonedas(indexMon).Id
                If (lMonedasAnt.Exists(Function(o As ELL.Moneda) o.Id = idMon)) Then
                    lMonedas.RemoveAt(indexMon)
                End If
            Next
            gvMonSel.DataSource = lMonedasAnt : gvMonNoSel.DataSource = lMonedas
            gvMonSel.DataBind() : gvMonNoSel.DataBind()
        Catch batz As BatzException
            Master.MensajeError = batz.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvMonSel_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMonSel.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oMon As ELL.Moneda = e.Row.DataItem
            Dim lnkElim As LinkButton = DirectCast(e.Row.FindControl("lnkElim"), LinkButton)
            If (oMon.Id = 90) Then
                lnkElim.Visible = False
            Else
                lnkElim.CommandArgument = oMon.Id
            End If
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvMonNoSel_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMonNoSel.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oMon As ELL.Moneda = e.Row.DataItem
            DirectCast(e.Row.FindControl("lnkAdd"), LinkButton).CommandArgument = oMon.Id
        End If
    End Sub

    ''' <summary>
    ''' Se añade la moneda
    ''' </summary>
    Protected Sub AddMoneda(ByVal sender As Object, e As EventArgs)
        Try
            Dim antBLL As New BLL.AnticiposBLL
            Dim idMoneda As Integer = CType(sender, LinkButton).CommandArgument
            antBLL.DeleteMoneda(idMoneda, Master.IdPlantaGestion)
            log.Info("Se ha quitado la moneda " & idMoneda & " de los anticipos seleccionables")
            loadMonedas()
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
        Catch ex As Exception
            log.Error("Error al añadir la moneda a los anticipos", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar")
        End Try
    End Sub

    ''' <summary>
    ''' Se quita la moneda
    ''' </summary>
    Protected Sub DeleteMoneda(ByVal sender As Object, e As EventArgs)
        Try
            Dim antBLL As New BLL.AnticiposBLL
            Dim idMoneda As Integer = CType(sender, LinkButton).CommandArgument
            antBLL.AddMoneda(idMoneda, Master.IdPlantaGestion)
            log.Info("Se ha añadido la moneda " & idMoneda & " a los anticipos seleccionables")
            loadMonedas()
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
        Catch ex As Exception
            log.Error("Error al quitar la moneda de los anticipos", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar")
        End Try
    End Sub

End Class