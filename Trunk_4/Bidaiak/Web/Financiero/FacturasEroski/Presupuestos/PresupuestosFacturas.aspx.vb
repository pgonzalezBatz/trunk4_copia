Public Class PresupuestosFacturas
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Carga la pagina de remesas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Presupuestos - Facturas"
            txtFechaInicio.Text = New Date(Now.Year, Now.Month, 1).ToShortDateString
            txtFechaFin.Text = New Date(Now.Year, Now.Month, Date.DaysInMonth(Now.Year, Now.Month)).ToShortDateString
            pnlResultado.Visible = False
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
            itzultzaileWeb.Itzuli(labelSel)
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
            Dim fechaIni As Date = CType(txtFechaInicio.Text, Date)
            Dim fechaFin As Date = CType(txtFechaFin.Text, Date)
            If (fechaIni > fechaFin) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La fecha de inicio tiene que ser menor que la fecha de fin")
            ElseIf (fechaIni < New Date(2019, 9, 1)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Solo estan disponibles los datos a partir de septiembre del 2019")
            Else
                pnlResultado.Visible = True
                Dim presupBLL As New BLL.PresupuestosBLL
                Dim lPresupFact As List(Of Object) = presupBLL.loadPresupuestosFacturas(Master.IdPlantaGestion, fechaIni, fechaFin, chbNoCoincidentes.Checked)
                lblReg.Text = lPresupFact.Count
                lPresupFact = lPresupFact.OrderBy(Function(o) o.IdViaje).ToList
                gvPresupFact.DataSource = lPresupFact
                gvPresupFact.DataBind()
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
    ''' Al enlazarse los datos del listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvPresupFact_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPresupFact.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oPresup = e.Row.DataItem
            Dim hlViaje As HyperLink = DirectCast(e.Row.FindControl("hlViaje"), HyperLink)
            Dim lblPresupuestado As Label = DirectCast(e.Row.FindControl("lblPresupuestado"), Label)
            Dim lblFacturado As Label = DirectCast(e.Row.FindControl("lblFacturado"), Label)
            Dim hlView As HyperLink = DirectCast(e.Row.FindControl("hlView"), HyperLink)
            If (oPresup.EstadoPresupuesto = ELL.Presupuesto.EstadoPresup.Validado) Then
                lblPresupuestado.Text = oPresup.Presupuestado & " €"
            Else
                If (oPresup.EstadoPresupuesto = -1) Then
                    lblPresupuestado.Text = itzultzaileWeb.Itzuli("No encontrado")
                    lblPresupuestado.CssClass = "label label-danger"
                Else
                    lblPresupuestado.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), oPresup.EstadoPresupuesto))
                    Select Case oPresup.EstadoPresupuesto
                        Case ELL.Presupuesto.EstadoPresup.Creado : lblPresupuestado.CssClass = "label label-default"
                        Case ELL.Presupuesto.EstadoPresup.Enviado : lblPresupuestado.CssClass = "label label label-info"
                        Case ELL.Presupuesto.EstadoPresup.Generado : lblPresupuestado.CssClass = "label label-default"
                        Case ELL.Presupuesto.EstadoPresup.Rechazado : lblPresupuestado.CssClass = "label label label-danger"
                    End Select
                End If
            End If
            If (oPresup.EstadoFactura = "F") Then
                lblFacturado.Text = oPresup.Facturado & " €"
            Else 'SF: Sin factura
                lblFacturado.Text = itzultzaileWeb.Itzuli("No encontrado")
                lblFacturado.CssClass = "label label-danger"
            End If
            hlViaje.Text = "V" & oPresup.IdViaje
            hlViaje.NavigateUrl = "../../Viaje/SolicitudViaje.aspx?id=" & oPresup.IdViaje
            hlView.NavigateUrl = "DetPresupuestoFactura.aspx?id=" & oPresup.IdViaje
            If (oPresup.Warning <> String.Empty) Then
                e.Row.CssClass = "warning"
                DirectCast(e.Row.FindControl("lblFaltaFacturar"), Label).Text = oPresup.Warning
            Else
                If (oPresup.Presupuestado = oPresup.Facturado) Then e.Row.CssClass = "success"
            End If
            itzultzaileWeb.Itzuli(hlViaje)
        End If
    End Sub

#End Region

End Class