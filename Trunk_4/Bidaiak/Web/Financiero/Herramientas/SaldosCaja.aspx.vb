Public Class SaldosCaja
    Inherits PageBase

    Private currencyName As String

#Region "Page Load"

    ''' <summary>
    ''' Carga de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Saldo en caja"
                loadTabOperations()
            End If
            Dim script As New StringBuilder
            script.AppendLine("$('#dtDateIni').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
            script.AppendLine("$('#dtDateFin').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(Me, Me.GetType, "DatePicker", script.ToString, True) 'Tiene que ser ToolkitScript para que funcione cuando hay un evento ajax        
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar la pagina")
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(btnGuardar)
            itzultzaileWeb.Itzuli(labelOpe) : itzultzaileWeb.Itzuli(labelCant) : itzultzaileWeb.Itzuli(labelComen)
            itzultzaileWeb.Itzuli(btnBuscar) : itzultzaileWeb.Itzuli(labelFIni) : itzultzaileWeb.Itzuli(labelFIni)
            itzultzaileWeb.Itzuli(labelOperationCons) : itzultzaileWeb.Itzuli(labelInfo2) : itzultzaileWeb.Itzuli(labelCurrencyCons)
            itzultzaileWeb.Itzuli(labelOpTitle) : itzultzaileWeb.Itzuli(labelMovTitle)
        End If
    End Sub

    ''' <summary>
    ''' Eventos al cambiar de pestaña
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub tabSaldos_ActiveTabChanged(sender As Object, e As EventArgs) Handles tabSaldos.ActiveTabChanged
        Try
            If (tabSaldos.ActiveTabIndex = 0) Then
                loadTabOperations()
            Else
                loadTabConsultas()
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
        End Try
    End Sub

#End Region

#Region "Tab Operaciones"

    ''' <summary>
    ''' Carga el tab de saldos
    ''' </summary>
    Private Sub loadTabOperations()
        loadSaldos()
    End Sub

    ''' <summary>
    ''' Carga la tabla con los saldos de cada moneda
    ''' </summary>
    Private Sub loadSaldos()
        inicializarTabOperations()
        Dim bidaiBLL As New BLL.BidaiakBLL
        Dim lSaldos As List(Of String()) = bidaiBLL.loadSaldosCaja(Master.IdPlantaGestion)
        lSaldos = lSaldos.OrderBy(Of String)(Function(o) o(2)).ToList
        gvSaldos.DataSource = lSaldos
        gvSaldos.DataBind()
    End Sub

    ''' <summary>
    ''' Inicializa los controles
    ''' </summary>
    Private Sub inicializarTabOperations()
        tabSaldos.ActiveTabIndex = 0
        cargarDropMonedas()
        cargarDropOperaciones()
        txtCantidad.Text = String.Empty : txtComen.Text = String.Empty
        gvSaldos.DataSource = Nothing : gvSaldos.DataBind()
    End Sub

    ''' <summary>
    ''' Carga el desplegable con todas las monedas. Antes salian solamente las de los anticipos pero así se puede gestionar la caja de todas las monedas
    ''' </summary>
    Private Sub cargarDropMonedas()
        If (ddlMonedas.Items.Count = 0) Then
            Dim xbatBLL As New BLL.XbatBLL
            Dim lMonedasAnt As List(Of ELL.Moneda) = xbatBLL.GetMonedas(True, 0) 'Master.IdPlantaGestion
            ddlMonedas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), -1))
            ddlMonedaCons.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), -1))
            For Each oMon As ELL.Moneda In lMonedasAnt
                ddlMonedas.Items.Add(New ListItem(oMon.Nombre, oMon.Id))
                ddlMonedaCons.Items.Add(New ListItem(oMon.Nombre, oMon.Id))
            Next
        End If
        ddlMonedas.SelectedValue = -1
        ddlMonedaCons.SelectedValue = -1
    End Sub

    ''' <summary>
    ''' Carga el desplegable con las operaciones que se pueden realizar
    ''' </summary>
    Private Sub cargarDropOperaciones()
        If (ddlOperacion.Items.Count = 0) Then
            ddlOperacion.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), -1))
            ddlOperacion.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Ingresar"), 0))
            ddlOperacion.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Actualizar"), 1))
            ddlOperacion.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Sacar"), 2))

            ddlOperationCons.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Todas"), -1))
            ddlOperationCons.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Ingreso"), 0))
            ddlOperationCons.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Actualizacion"), 1))
            ddlOperationCons.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Extraccion"), 2))
            ddlOperationCons.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Anticipo entregado"), 3))
            ddlOperationCons.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Devuelto"), 4))
        End If
        ddlOperacion.SelectedValue = -1
        ddlOperationCons.SelectedValue = -1
    End Sub

    ''' <summary>
    ''' Se linkan los datos al grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvSaldos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvSaldos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim sSaldo As String() = e.Row.DataItem
            Dim lnkMoneda As LinkButton = CType(e.Row.FindControl("lnkMoneda"), LinkButton)
            lnkMoneda.Text = sSaldo(2) : lnkMoneda.CommandArgument = sSaldo(1)
            CType(e.Row.FindControl("lblSaldo"), Label).Text = sSaldo(0)
        End If
    End Sub

    ''' <summary>
    ''' Accede a buscar los movimientos de una moneda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub lnkMoneda_Click(sender As Object, e As EventArgs)
        Try
            Dim lnk As LinkButton = CType(sender, LinkButton)
            loadTabConsultas(lnk.CommandArgument)
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar")
        End Try
    End Sub

    ''' <summary>
    ''' Se intenta registrar una operacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            If (ddlOperacion.SelectedValue = -1 OrElse ddlMonedas.SelectedValue = -1 OrElse txtCantidad.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Rellene todos los datos")
            Else
                Dim bidaiBLL As New BLL.BidaiakBLL
                Dim oSaldo As New ELL.SaldoCaja With {.Fecha = Now, .IdUsuario = Master.Ticket.IdUser, .IdPlanta = Master.IdPlantaGestion, .IdMoneda = CInt(ddlMonedas.SelectedValue),
                                                       .Operacion = CInt(ddlOperacion.SelectedValue), .Cantidad = DecimalValue(txtCantidad.Text), .Comentario = txtComen.Text.Trim}
                bidaiBLL.saveSaldoCaja(oSaldo)
                loadTabOperations()
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
                log.Info("Saldo de caja registrado =>" & txtCantidad.Text & " " & ddlMonedas.SelectedItem.Text & " - " & ddlOperacion.SelectedItem.Text)
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar")
        End Try
    End Sub

#End Region

#Region "Tab Consultas"

    ''' <summary>
    ''' Carga el tab de consultas
    ''' </summary>
    ''' <param name="idCurrency">Id de la moneda.Si viene informado, cargara los movimientos directamente</param>
    Private Sub loadTabConsultas(Optional ByVal idCurrency As Integer = 0)
        inicializarTabConsultas()
        If (idCurrency > 0) Then
            ddlMonedaCons.SelectedIndex = ddlMonedaCons.Items.IndexOf(ddlMonedaCons.Items.FindByValue(idCurrency))
            If (CInt(ddlMonedaCons.SelectedValue) > 0) Then loadMovimientosCaja() 'Puede que la moneda no exista para consultar
        End If
    End Sub

    ''' <summary>
    ''' Carga la tabla con los movimientos de cada moneda
    ''' </summary>
    Private Sub loadMovimientosCaja()
        Dim bidaiBLL As New BLL.BidaiakBLL
        Dim xbatBLL As New BLL.XbatBLL
        Dim oCurrency As ELL.Moneda = xbatBLL.GetMoneda(CInt(ddlMonedaCons.SelectedValue))
        currencyName = oCurrency.Abreviatura
        Dim lMovs As List(Of ELL.SaldoCaja) = bidaiBLL.loadMovimientosCaja(Master.IdPlantaGestion, CInt(ddlMonedaCons.SelectedValue), CDate(txtFechaInicioCons.Text), CDate(txtFechaFinCons.Text), CInt(ddlOperationCons.SelectedValue))
        lMovs = lMovs.OrderByDescending(Of DateTime)(Function(o) o.Fecha).ToList
        gvMovimientos.DataSource = lMovs
        gvMovimientos.DataBind()
    End Sub

    ''' <summary>
    ''' Inicializa los controles
    ''' </summary>
    Private Sub inicializarTabConsultas()
        tabSaldos.ActiveTabIndex = 1
        cargarDropMonedas()
        cargarDropOperaciones()
        txtFechaInicioCons.Text = New Date(Now.Year, Now.Month, 1).ToShortDateString
        txtFechaFinCons.Text = New Date(Now.Year, Now.Month, Date.DaysInMonth(Now.Year, Now.Month)).ToShortDateString
        gvMovimientos.DataSource = Nothing : gvMovimientos.DataBind()
    End Sub

    ''' <summary>
    ''' Busca los movimientos segun el filtro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            If (CInt(ddlMonedaCons.SelectedValue) = -1 OrElse txtFechaInicioCons.Text = String.Empty OrElse txtFechaFinCons.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione la moneda y las fechas")
            Else
                loadMovimientosCaja()
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar")
        End Try
    End Sub

    ''' <summary>
    ''' Se linkan los datos al grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvMovimientos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMovimientos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oMov As ELL.SaldoCaja = e.Row.DataItem
            CType(e.Row.FindControl("lblFecha"), Label).Text = oMov.Fecha.ToShortDateString & " - " & oMov.Fecha.ToShortTimeString
            CType(e.Row.FindControl("lblOperacion"), Label).Text = [Enum].GetName(GetType(ELL.SaldoCaja.EOperacion), oMov.Operacion).Replace("_", " ")
            Dim numDecimales As Integer = If(Math.Floor(oMov.Cantidad) = Math.Ceiling(oMov.Cantidad), 0, 2)
            CType(e.Row.FindControl("lblCant"), Label).Text = FormatearNumero(oMov.Cantidad, numDecimales) & " " & currencyName
            numDecimales = If(Math.Floor(oMov.SaldoRestante) = Math.Ceiling(oMov.SaldoRestante), 0, 2)
            CType(e.Row.FindControl("lblSaldo"), Label).Text = If(oMov.SaldoRestante > 0, FormatearNumero(oMov.SaldoRestante, numDecimales), oMov.SaldoRestante) & " " & currencyName  'Se tiene que mostrar negativo
            CType(e.Row.FindControl("lblComentario"), Label).Text = oMov.Comentario
        End If
    End Sub

#End Region

End Class