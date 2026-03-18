Partial Public Class Remesas
    Inherits PageBase

    Private lAnticiposRetenidos As List(Of ELL.Anticipo.Movimiento)

#Region "Page Load"

    ''' <summary>
    ''' Carga la pagina de remesas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Remesas"
            txtFechaInicio.Text = New Date(Now.Year, Now.Month, 1).ToShortDateString
            txtFechaFin.Text = New Date(Now.Year, Now.Month, Date.DaysInMonth(Now.Year, Now.Month)).ToShortDateString
            pnlSinResultados.Visible = False : pnlDineroRetenido.Visible = False
        End If
        Dim script As New StringBuilder
        script.AppendLine("$('#dtFechaIni').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
        script.AppendLine("$('#dtFechaFin').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
        AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(Me, Me.GetType, "DatePicker", script.ToString, True)
    End Sub

    ''' <summary>
    ''' Se le indica lo que tiene que traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Remesas_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelFFin) : itzultzaileWeb.Itzuli(labelFIni) : itzultzaileWeb.Itzuli(btnSearchF)
            itzultzaileWeb.Itzuli(labelSinResul) : itzultzaileWeb.Itzuli(labelSel) : itzultzaileWeb.Itzuli(labelDivCabecera)
        End If
    End Sub

#End Region

#Region "Buscar"

    ''' <summary>
    ''' Busca las remesas entra las fechas seleccionadas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnSearchF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchF.Click
        Try
            Dim remesas As New List(Of String())
            Dim fechaIni As Date = CType(txtFechaInicio.Text, Date)
            Dim fechaFin As Date = CType(txtFechaFin.Text, Date)
            pnlDineroRetenido.Visible = False
            If (fechaIni > fechaFin) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La fecha de inicio tiene que ser menor que la fecha de fin")
            Else
                Dim anticBLL As New BLL.AnticiposBLL
                lAnticiposRetenidos = anticBLL.loadLinesAnticiposPreparados(Master.IdPlantaGestion, fechaIni, fechaFin)
                If (lAnticiposRetenidos IsNot Nothing AndAlso lAnticiposRetenidos.Count > 0) Then
                    pnlDineroRetenido.Visible = True
                    gvDineroRet.DataSource = lAnticiposRetenidos
                    gvDineroRet.DataBind()
                End If
                Dim lRemesas As List(Of String()) = anticBLL.loadRemesas(Master.IdPlantaGestion, fechaIni, fechaFin)
                rptRemesas.Visible = (lRemesas.Count > 0)
                pnlSinResultados.Visible = (lRemesas.Count = 0)
                rptRemesas.DataSource = lRemesas
                rptRemesas.DataBind()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errBuscar")
        End Try
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Se enlazan los movimientos de anticipos retenidos (en estado preparado)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvDineroRet_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDineroRet.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim data As ELL.Anticipo.Movimiento = e.Row.DataItem
            Dim xbatBLL As New BLL.XbatBLL
            DirectCast(e.Row.FindControl("lblMoneda"), Label).Text = xbatBLL.GetMoneda(data.Moneda.Id).Nombre
            DirectCast(e.Row.FindControl("lblCantidad"), Label).Text = data.Cantidad
        End If
    End Sub

    ''' <summary>
    ''' Al enlazarse los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptRemesas_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptRemesas.ItemDataBound
        If (e.Item.ItemType = ListItemType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Item.Controls)
        ElseIf (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim sRemesa As String() = e.Item.DataItem
            Dim lblSaldoCaja As Label = DirectCast(e.Item.FindControl("lblSaldoCaja"), Label)
            Dim lblSaldoCajaRetenido As Label = DirectCast(e.Item.FindControl("lblSaldoCajaRetenido"), Label)
            Dim hlIdViaje As HyperLink = DirectCast(e.Item.FindControl("hlIdViaje"), HyperLink)
            Dim pnlDetalle As Panel = DirectCast(e.Item.FindControl("pnlDetalle"), Panel)
            Dim idMoneda As Integer = CInt(sRemesa(0))
            Dim xbatBLL As New BLL.XbatBLL
            DirectCast(e.Item.FindControl("lblIdMoneda"), Label).Text = idMoneda
            DirectCast(e.Item.FindControl("lblMoneda"), Label).Text = xbatBLL.GetMoneda(CInt(sRemesa(0))).Nombre
            DirectCast(e.Item.FindControl("lblCantidad"), Label).Text = CDec(sRemesa(1))
            DirectCast(e.Item.FindControl("lblFechaReq"), Label).Text = CType(sRemesa(2), Date).ToShortDateString
            pnlDetalle.Visible = False
            'Se comprueba si se tiene que mostrar el mas o no
            Dim imgDetalle As ImageButton = DirectCast(e.Item.FindControl("imgDetalle"), ImageButton)
            Dim fechaIni As Date = CType(txtFechaInicio.Text, Date)
            Dim fechaFin As Date = CType(txtFechaFin.Text, Date)
            'Se obtienen las remesas de ese tipo de moneda
            Dim lRemesas As List(Of String()) = Nothing
            Dim anticBLL As New BLL.AnticiposBLL
            lRemesas = anticBLL.loadRemesas(Master.IdPlantaGestion, fechaIni, fechaFin, idMoneda)
            imgDetalle.Visible = (lRemesas IsNot Nothing AndAlso lRemesas.Count > 1)
            imgDetalle.ToolTip = itzultzaileWeb.Itzuli("Desplegar para ver los distintos anticipos solicitados de esta moneda")
            If (lRemesas.Count > 1) Then
                'Si no tiene imagen de detalle, significa que la cantidad de dinero solo pertenece a un viaje, asi que se consulta su idViaje                
                Dim myRemesa As List(Of String()) = anticBLL.loadRemesas(Master.IdPlantaGestion, fechaIni, fechaFin, Integer.MinValue, True)
                If (myRemesa IsNot Nothing AndAlso myRemesa.Count = 1) Then
                    hlIdViaje.Text = "V" & myRemesa(0)(3)
                    hlIdViaje.NavigateUrl = "../../Viaje/SolicitudViaje.aspx?id=" & myRemesa(0)(3)
                End If
            Else
                hlIdViaje.Text = "V" & lRemesas.First()(3)
                hlIdViaje.NavigateUrl = "../../Viaje/SolicitudViaje.aspx?id=" & lRemesas.First()(3)
                If (CInt(lRemesas.First()(4)) = ELL.Anticipo.EstadoAnticipo.Preparado) Then
                    CType(e.Item.Controls(1), HtmlTableRow).Attributes.Add("class", "warning")
                    CType(e.Item.Controls(1), HtmlTableRow).Attributes.Add("title", itzultzaileWeb.Itzuli("Anticipo retenido por estar en estado preparado"))
                End If
            End If
            Dim saldoSinRetencion, saldoConRetencion As Decimal
            Dim bidaiBLL As New BLL.BidaiakBLL
            Dim saldo As String() = bidaiBLL.loadSaldoCaja(idMoneda, Master.IdPlantaGestion)
            saldoSinRetencion = If(saldo Is Nothing, 0, DecimalValue(saldo(0)))
            lblSaldoCaja.Text = saldoSinRetencion
            saldoConRetencion = saldoSinRetencion
            If (lAnticiposRetenidos IsNot Nothing AndAlso lAnticiposRetenidos.Count > 0) Then
                Dim saldoAnticipo As ELL.Anticipo.Movimiento = lAnticiposRetenidos.Find(Function(f) f.Moneda.Id = idMoneda)
                If (saldoAnticipo IsNot Nothing) Then saldoConRetencion = saldoSinRetencion - saldoAnticipo.Cantidad
            End If
            lblSaldoCajaRetenido.Text = saldoConRetencion
            If (saldoConRetencion <= 0) Then
                lblSaldoCajaRetenido.CssClass = "text-danger"
            Else
                lblSaldoCajaRetenido.CssClass = "text-info"
            End If
        End If
    End Sub

    ''' <summary>
    ''' Al enlazarse los datos del detalle
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub rptRemesasDet_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim sRemesaDet As String() = e.Item.DataItem
            Dim xbatBLL As New BLL.XbatBLL
            DirectCast(e.Item.FindControl("lblMonedaDet"), Label).Text = xbatBLL.GetMoneda(CInt(sRemesaDet(0))).Nombre
            DirectCast(e.Item.FindControl("lblCantidadDet"), Label).Text = CDec(sRemesaDet(1))
            DirectCast(e.Item.FindControl("lblFechaReqDet"), Label).Text = CType(sRemesaDet(2), Date).ToShortDateString
            Dim hlIdViajeDet As HyperLink = DirectCast(e.Item.FindControl("hlIdViajeDet"), HyperLink)
            hlIdViajeDet.Text = "V" & sRemesaDet(3)
            hlIdViajeDet.NavigateUrl = "../../Viaje/SolicitudViaje.aspx?id=" & sRemesaDet(3)
            itzultzaileWeb.Itzuli(hlIdViajeDet)
            If (CInt(sRemesaDet(4)) = ELL.Anticipo.EstadoAnticipo.Preparado) Then
                CType(e.Item.Controls(1), HtmlTableRow).Attributes.Add("class", "warning")
                CType(e.Item.Controls(1), HtmlTableRow).Attributes.Add("title", itzultzaileWeb.Itzuli("Anticipo retenido por estar en estado preparado"))
            End If
        End If
    End Sub

    ''' <summary>
    ''' Al producirse el evento de pulsar el boton mas o menos del repeater
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>    
    Private Sub rptRemesas_ItemCommand(ByVal source As Object, ByVal e As RepeaterCommandEventArgs) Handles rptRemesas.ItemCommand
        Try
            If (e.CommandName = "Detalle") Then
                Dim index As Integer = CInt(e.Item.ItemIndex)
                Dim imgDetalle As ImageButton = CType(rptRemesas.Items(index).FindControl("imgDetalle"), ImageButton)
                Dim visibleDetalle As Boolean = rptRemesas.Items(index).FindControl("pnlDetalle").Visible
                If (visibleDetalle) Then
                    imgDetalle.ImageUrl = "~/App_Themes/Tema1/images/signo_mas.jpg"
                    imgDetalle.ToolTip = itzultzaileWeb.Itzuli("Desplegar para ver los distintos anticipos solicitados de esta moneda")
                Else
                    imgDetalle.ImageUrl = "~/App_Themes/Tema1/images/signo_menos.jpg"
                    imgDetalle.ToolTip = itzultzaileWeb.Itzuli("Contraer")
                End If
                rptRemesas.Items(index).FindControl("pnlDetalle").Visible = Not visibleDetalle
                If (Not visibleDetalle) Then 'Si antes no estaba visible pero ahora se va a visualizar, hay que mostrar los detalles
                    Dim rptRemesasDet As Repeater = CType(rptRemesas.Items(index).FindControl("rptRemesasDet"), Repeater)
                    Dim anticBLL As New BLL.AnticiposBLL
                    Dim fechaIni As Date = CType(txtFechaInicio.Text, Date)
                    Dim fechaFin As Date = CType(txtFechaFin.Text, Date)
                    Dim idMoneda As Integer = CInt(CType(rptRemesas.Items(index).FindControl("lblIdMoneda"), Label).Text)
                    Dim lRemesas As List(Of String()) = anticBLL.loadRemesas(Master.IdPlantaGestion, fechaIni, fechaFin, idMoneda)
                    lRemesas = lRemesas.OrderBy(Of Date)(Function(o) CDate(o(2))).ToList 'Se ordena por fecha
                    rptRemesasDet.DataSource = lRemesas
                    rptRemesasDet.DataBind()
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al realizar la operacion")
        End Try
    End Sub

#End Region

End Class