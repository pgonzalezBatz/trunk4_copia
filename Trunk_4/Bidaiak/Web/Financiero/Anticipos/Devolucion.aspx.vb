Partial Public Class Devolucion
    Inherits PageBase

#Region "Page Load e inicializaciones"

    ''' <summary>
    ''' Carga la pagina de devoluciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Devoluciones"
            Try
                btnDevolver.CommandArgument = Request.QueryString("idViaje")
                inicializar()
            Catch batzEx As BatzException
                Master.MensajeError = batzEx.Termino
            Catch ex As Exception
                Master.MensajeError = itzultzaileWeb.Itzuli("errCargarPagina")
            End Try
        End If
        Dim script As New StringBuilder
        script.AppendLine("$('#dtFechaDev').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
        AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(Me, Me.GetType, "DatePicker", script.ToString, True) 'Tiene que ser ToolkitScript para que funcione cuando hay un evento ajax
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Devolucion_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelUsuario) : itzultzaileWeb.Itzuli(lblEtiEurosPend) : itzultzaileWeb.Itzuli(labelDivDev)
            itzultzaileWeb.Itzuli(labelFecha) : itzultzaileWeb.Itzuli(btnDevolver) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(labelDivHistorico) : itzultzaileWeb.Itzuli(btnPrint)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa los controles de la pagina
    ''' </summary>	
    Private Sub inicializar()
        txtCantidad.Text = String.Empty : txtCantidad.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Cantidad a devolver"))
        txtFechaDev.Text = Now.ToShortDateString
        cargarUsuariosViaje()
        cargarMonedas()
        Dim viajesBLL As New BLL.ViajesBLL
        Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(CInt(btnDevolver.CommandArgument), True, True)  'El tercer parametro es true para que calcule tambien los pendientes de la hoja de gastos del liquidador
        Dim eurosPend As Decimal = Math.Round(oViaje.Anticipo.EurosPendientes, 2) - Math.Round(oViaje.Anticipo.EurosPendientesHojaGastosLiq, 2)
        lblEurosPend.Text = eurosPend
        lblEurosPend.CssClass = If(eurosPend >= 0, "text-primary", "text-danger")
        loadDevolucionesRealizadas()
    End Sub

#End Region

#Region "Carga de datos"

    ''' <summary>
    ''' Carga las monedas existentes
    ''' </summary>	
    Private Sub cargarMonedas()
        If (ddlMonedas.Items.Count = 0) Then
            Dim lMonedas As List(Of ELL.Moneda)
            Dim xbatComp As New BLL.XbatBLL
            lMonedas = xbatComp.GetMonedas()
            ddlMonedas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
            For Each oMon As ELL.Moneda In lMonedas
                ddlMonedas.Items.Add(New ListItem(oMon.Nombre.ToUpper, oMon.Id))
            Next
        End If
        ddlMonedas.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Carga los usuarios del viaje del parametro
    ''' </summary>	
    Private Sub cargarUsuariosViaje()
        If (ddlUsuarios.Items.Count = 0) Then
            Try
                ddlUsuarios.Items.Clear()
                ddlUsuarios.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
                Dim viajeBLL As New BLL.ViajesBLL
                Dim integrantes As List(Of ELL.Viaje.Integrante) = viajeBLL.loadIntegrantes(CInt(btnDevolver.CommandArgument))
                For Each item As ELL.Viaje.Integrante In integrantes
                    ddlUsuarios.Items.Add(New ListItem(item.Usuario.NombreCompleto, item.Usuario.Id))
                Next
            Catch ex As Exception
                Throw New BatzException("Error al cargar los integrantes del viaje", ex)
            End Try
        End If
        ddlUsuarios.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Muestra las devoluciones que se han realizado
    ''' </summary>
    Private Sub loadDevolucionesRealizadas()
        Try
            Dim anticipoBLL As New BLL.AnticiposBLL
            Dim lMovs As List(Of ELL.Anticipo.Movimiento) = anticipoBLL.loadMovimientos(CInt(Request.QueryString("idViaje")), ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion)
            If (lMovs IsNot Nothing AndAlso lMovs.Count > 0) Then lMovs = lMovs.OrderByDescending(Of Date)(Function(o) o.Fecha).ToList
            gvDevoluciones.DataSource = lMovs
            gvDevoluciones.DataBind()
        Catch ex As Exception
            Throw New BatzException("Error al cargar los movimientos de devolucion", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los estados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvDevoluciones_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvDevoluciones.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            Dim chbSelectAll As CheckBox = CType(e.Row.FindControl("chbSelectAll"), CheckBox)
            If (chbSelectAll IsNot Nothing) Then
                chbSelectAll.Attributes("onclick") = "ChangeAllCheckBoxStates(this.checked);"
                CheckBoxIDsArray.Text = chbSelectAll.ClientID  'Se guarda el id en esta variable para que luego en el footer, sepa cual es
                itzultzaileWeb.TraducirWebControls(e.Row.Controls)
            End If
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim mov As ELL.Anticipo.Movimiento = DirectCast(e.Row.DataItem, ELL.Anticipo.Movimiento)
            DirectCast(e.Row.FindControl("lblId"), Label).Text = mov.Id
            DirectCast(e.Row.FindControl("lblFechaDev"), Label).Text = mov.Fecha.ToShortDateString
            DirectCast(e.Row.FindControl("lblCantidad"), Label).Text = mov.Cantidad
            DirectCast(e.Row.FindControl("lblMoneda"), Label).Text = mov.Moneda.Abreviatura
        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
            Dim ArrayValues As New List(Of String)
            'Se añade el primero el de la cabecera
            ArrayValues.Add(String.Concat("'", CheckBoxIDsArray.Text, "'"))  'En la cabecera, se ha guardado el nombre del check de la cabecera
            Dim cont As Integer = 0
            Dim total As Decimal = 0
            For Each gvr As GridViewRow In gvDevoluciones.Rows
                Dim cb As CheckBox = CType(gvr.FindControl("chbMarcar"), CheckBox)
                'If the checkbox is unchecked, ensure that the Header CheckBox is unchecked
                cb.Attributes("onclick") = "ChangeHeaderAsNeeded();"
                'Add the CheckBox's ID to the client-side CheckBoxIDs array
                ArrayValues.Add(String.Concat("'", cb.ClientID, "'"))
                If (cb.Enabled) Then cont += 1
            Next
            btnPrint.Enabled = (cont > 0)
            CheckBoxIDsArray.Text = "<script type=""text/javascript"">" & vbCrLf &
                    "<!--" & vbCrLf &
                    String.Concat("var CheckBoxIDs = new Array(", String.Join(",", ArrayValues.ToArray()), ");") & vbCrLf &
                    "// -->" & vbCrLf &
                    "</script>"
        End If
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Devuelve un anticipo de un viaje
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnDevolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDevolver.Click
        Try
            If (ddlMonedas.SelectedValue = Integer.MinValue Or ddlUsuarios.SelectedValue = Integer.MinValue Or txtCantidad.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("debeRellenarDatos")
            Else
                Dim xbatBLL As New BLL.XbatBLL
                Dim anticBLL As New BLL.AnticiposBLL
                Dim bidaiBLL As New BLL.BidaiakBLL
                Dim cantidad As Decimal = CDec(txtCantidad.Text)
                Dim fechaDev As DateTime = CDate(txtFechaDev.Text)
                fechaDev = New DateTime(fechaDev.Year, fechaDev.Month, fechaDev.Day, Now.Hour, Now.Minute, Now.Second)   'Para que no le meta hora 00:00:00 
                Dim idAnticipo As Integer = CInt(btnDevolver.CommandArgument)
                Dim fechaEntregAnt As DateTime = anticBLL.loadAnticipoFechaEntrega(idAnticipo)
                If (fechaEntregAnt = DateTime.MinValue) Then fechaEntregAnt = New DateTime(fechaDev.Year, fechaDev.Month, fechaDev.Day, Now.Hour, Now.Minute, Now.Second)   'Para que no le meta hora 00:00:00 
                Dim idMoneda As Integer = CInt(ddlMonedas.SelectedValue)
                Dim cambioMoneda As Decimal = 0
                Dim importeEuros As Decimal = xbatBLL.ObtenerRateEuros(idMoneda, cantidad, fechaEntregAnt, cambioMoneda) 'Se consulta en la fecha de entrega del anticipo

                Dim oMov As New ELL.Anticipo.Movimiento With {.IdAnticipo = idAnticipo, .Cantidad = cantidad, .Fecha = fechaDev, .Moneda = New ELL.Moneda With {.Id = idMoneda}, .TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion,
                    .ImporteEuros = importeEuros, .UserOrigen = New SabLib.ELL.Usuario With {.Id = CInt(ddlUsuarios.SelectedValue)}, .CambioMonedaEUR = cambioMoneda}
                anticBLL.SaveMovimiento(oMov)
                bidaiBLL.saveSaldoCaja(New ELL.SaldoCaja With {.Cantidad = cantidad, .Fecha = Now, .IdUsuario = Master.Ticket.IdUser, .IdPlanta = Master.IdPlantaGestion, .Operacion = ELL.SaldoCaja.EOperacion.Devolucion_Anticipo,
                                                               .IdMoneda = idMoneda, .Comentario = "Devolucion en metalico del usuario " & ddlUsuarios.SelectedItem.Text & " del viaje " & oMov.IdAnticipo})
                log.Info("HOJA_GASTOS:El usuario " & ddlUsuarios.SelectedItem.Text & " (" & ddlUsuarios.SelectedItem.Value & ") ha realizado una devolucion de " & oMov.Cantidad & " " & ddlMonedas.SelectedItem.Text & " del viaje (" & idAnticipo & ")")
                inicializar()
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Operacion realizada")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Info("Ha ocurrido un error al realizar una devolucion del usuario (" & ddlUsuarios.SelectedItem.Text & ") del viaje (" & btnDevolver.CommandArgument & ")", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al realizar la operacion")
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve a la pagina de anticipos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Response.Redirect("GestionAnticipos.aspx?idViaje=" & btnDevolver.CommandArgument)
    End Sub

    ''' <summary>
    ''' Se imprime un documento para justificar que se ha devuelto dinero
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Try
            Dim numSel As Integer = 0
            Dim lineas As New StringBuilder
            For Each row As GridViewRow In gvDevoluciones.Rows
                If (CType(row.Cells(1).Controls(0), CheckBox).Checked) Then
                    numSel += 1
                    If (lineas.ToString <> String.Empty) Then lineas.Append("|")
                    lineas.Append(CType(row.Cells(0).Controls(0), Label).Text)
                End If
            Next
            If (numSel = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione algun movimiento")
            Else
                log.Info("Se va a generar el pdf de la devolucion del anticipo")
                Dim sb As String = "window.open('../../Publico/ViewDocument.aspx?tipo=devolucionAnt&id=" & lineas.ToString & "&idViaje=" & Request.QueryString("IdViaje") & "','Devolucion');"
                ScriptManager.RegisterStartupScript(Page, Me.GetType, "Recibo", sb.ToString, True)
                'Page.ClientScript.RegisterStartupScript(Me.GetType, "Recibo", sb, True)
            End If
        Catch ex As Exception
            log.Error("Error al imprimir los movimientos de devolucion", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al imprimir")
        End Try
    End Sub

#End Region

End Class