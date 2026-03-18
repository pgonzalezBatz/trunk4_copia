Public Class ListadoLiq
    Inherits UserControl

#Region "Propiedades"

    Public Event Advertencia(ByVal mensaje As String)
    Public Event ErrorGenerado(ByVal mensaje As String)
    Public Event Finalizado(ByVal lHGLiq As List(Of ELL.HojaGastos.Liquidacion))
    Private itzultzaileWeb As New itzultzaile

    ''' <summary>
    ''' Id de la planta actual
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property IdPlanta As Integer
        Get
            Return CInt(Session("IdPlanta"))
        End Get
        Set(ByVal value As Integer)
            Session("IdPlanta") = value
        End Set
    End Property

#End Region

#Region "Carga del control"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
        Dim script As New StringBuilder
        script.AppendLine("try{$('#dtFechaVal').datetimepicker({showClear:true,locale:'" & ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});}catch(err) {}")
        AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(Me, Me.GetType, "DatePicker", script.ToString, True)
    End Sub

    ''' <summary>
    ''' Se le indica lo que tiene que traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo)
            itzultzaileWeb.Itzuli(labelFiltrar) : itzultzaileWeb.Itzuli(chbHGEntregada)
            itzultzaileWeb.Itzuli(btnFiltrar)
            itzultzaileWeb.Itzuli(labelProcesando) : itzultzaileWeb.Itzuli(btnContinuar)
        End If
    End Sub

    ''' <summary>
    ''' Proceso para que se inicie el control
    ''' </summary>
    Public Sub Iniciar()
        chbHGEntregada.Checked = True
        txtFechaVal.Text = String.Empty
        pnlInfo.Visible = False
    End Sub

#End Region

#Region "Acciones"

    ''' <summary>
    ''' Se pinta el gridview de las liquidaciones a procesar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        Try
            BuscarLiquidaciones()
        Catch batzEx As BatzException
            RaiseEvent ErrorGenerado(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Busca las hojas de gastos que se encuentren entre las fechas señaladas, y calcula el importe a pagar o a ingresar
    ''' </summary>    
    Private Sub BuscarLiquidaciones()
        Try
            Dim hubContext As Microsoft.AspNet.SignalR.IHubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext(Of SignalR.SignalRHub)()
            Dim hubClientContext = hubContext.Clients.Client(hfConnectionId.Value)
            Dim hojasGastosBLL As New BLL.HojasGastosBLL
            pnlInfo.Visible = True
            Dim fLimite As Date = If(txtFechaVal.Text <> String.Empty, CDate(txtFechaVal.Text), CDate(Now.ToShortDateString))
            Dim lLiquidaciones As List(Of ELL.HojaGastos.Liquidacion) = hojasGastosBLL.loadLiquidaciones(IdPlanta, ELL.HojaGastos.Liquidacion.TipoLiq.Metalico, Integer.MinValue, fLimite, chbHGEntregada.Checked, hubClientContext)
            If (lLiquidaciones IsNot Nothing) Then lLiquidaciones = lLiquidaciones.OrderBy(Function(o) o.Usuario.NombreCompleto).ToList
            gvLiquidaciones.DataSource = lLiquidaciones
            gvLiquidaciones.DataBind()
            btnContinuar.Visible = (lLiquidaciones IsNot Nothing AndAlso lLiquidaciones.Count > 0)
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al buscar las hojas de gastos a liquidar", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se excluye la hoja para que no aparezca
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub imgExcluir_Click(sender As Object, e As ImageClickEventArgs)
        Try
            Dim idHoja As Integer = CInt(CType(sender, ImageButton).CommandArgument)
            Dim hojBLL As New BLL.HojasGastosBLL
            hojBLL.ExcluirHG(idHoja)
            PageBase.log.Info("Se ha excluido la hoja de gastos " & idHoja & " de la liquidacion")
            BuscarLiquidaciones()
        Catch batzEx As BatzException
            RaiseEvent ErrorGenerado(batzEx.Termino)
        End Try
    End Sub

    ''' <summary>
    ''' Enlaza los datos con el gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvLiquidaciones_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvLiquidaciones.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            Dim chbSelectAll As CheckBox = CType(e.Row.FindControl("chbSelectAll"), CheckBox)
            chbSelectAll.Attributes("onclick") = "ChangeAllCheckBoxStates(this.checked);"
            CheckBoxIDsArray.Text = chbSelectAll.ClientID  'Se guarda el id en esta variable para que luego en el footer, sepa cual es
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oLiq As ELL.HojaGastos.Liquidacion = e.Row.DataItem
            Dim lblPersona As Label = CType(e.Row.FindControl("lblPersona"), Label)
            Dim hlViajeHoja As HyperLink = CType(e.Row.FindControl("hlViajeHoja"), HyperLink)
            Dim imgExcluir As ImageButton = CType(e.Row.FindControl("imgExcluir"), ImageButton)
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim epsilonBLL As New BLL.Epsilon(IdPlanta)
            lblPersona.Text = oLiq.Usuario.NombreCompleto & " (" & oLiq.Usuario.CodPersona & ")"
            If (oLiq.Usuario.DadoBaja) Then e.Row.CssClass = "danger"
            CType(e.Row.FindControl("lblIdUser"), Label).Text = oLiq.Usuario.Id
            Dim myHoja As ELL.HojaGastos.Liquidacion.Hoja = oLiq.Hojas.First
            hlViajeHoja.NavigateUrl = "~/Viaje/HojaGastos.aspx?id=" & myHoja.IdHoja & "&orig=LIQ"
            If (myHoja.IdHojaLibre <> Integer.MinValue) Then
                hlViajeHoja.Text = "H" & myHoja.IdHojaLibre
            ElseIf (myHoja.IdViaje <> Integer.MinValue) Then
                hlViajeHoja.Text = "V" & myHoja.IdViaje
            End If
            CType(e.Row.FindControl("lblId"), Label).Text = myHoja.IdHoja
            CType(e.Row.FindControl("lblImportes"), Label).Text = myHoja.ImporteEuros
            CType(e.Row.FindControl("lblLiquidacion"), Label).Text = myHoja.ImporteEuros & " €"
            CType(e.Row.FindControl("lblFVal"), Label).Text = myHoja.FechaValidacion.ToShortDateString
            CType(e.Row.FindControl("imgHGEntreg"), Image).Visible = myHoja.Entregada
            imgExcluir.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("Si excluye esta hoja desparecera del listado de liquidaciones") & "');"
            imgExcluir.ToolTip = itzultzaileWeb.Itzuli("Excluir hoja")
            imgExcluir.CommandArgument = myHoja.IdHoja
        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
            Dim ArrayValues As New List(Of String)
            Dim lblTotal As Label = CType(e.Row.FindControl("lblTotal"), Label)
            Dim labelTotal As Label = CType(e.Row.FindControl("labelTotal"), Label)
            'Se añade el primero el de la cabecera
            ArrayValues.Add(String.Concat("'", CheckBoxIDsArray.Text, "'"))  'En la cabecera, se ha guardado el nombre del check de la cabecera
            Dim cont As Integer = 0
            Dim total As Decimal = 0
            For Each gvr As GridViewRow In gvLiquidaciones.Rows
                Dim cb As CheckBox = CType(gvr.FindControl("chbMarcar"), CheckBox)
                Dim lblImp As Label = CType(gvr.FindControl("lblImportes"), Label)
                cb.Attributes("onclick") = "ChangeHeaderAsNeeded();"
                ArrayValues.Add(String.Concat("'", cb.ClientID, "'"))
                If (cb.Enabled) Then cont += 1
                total += PageBase.DecimalValue(lblImp.Text.Trim)
            Next
            If (labelTotal IsNot Nothing) Then itzultzaileWeb.Itzuli(labelTotal)
            lblTotal.Text = total & "€"
            CheckBoxIDsArray.Text = "<script type=""text/javascript"">" & vbCrLf &
                    "<!--" & vbCrLf &
                    String.Concat("var CheckBoxIDs = new Array(", String.Join(",", ArrayValues.ToArray()), ");") & vbCrLf &
                    "// -->" & vbCrLf &
                    "</script>"
        End If
    End Sub

    ''' <summary>
    ''' Una vez seleccionadas las lineas, se intenta continuar al paso de resumen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        Try
            Dim hojasGastosBLL As New BLL.HojasGastosBLL
            Dim hoja As ELL.HojaGastos
            Dim lLiquidaciones As New List(Of ELL.HojaGastos.Liquidacion)
            Dim lLineas As New List(Of ELL.HojaGastos.Liquidacion)
            Dim linea As ELL.HojaGastos.Liquidacion
            Dim lHojas As List(Of ELL.HojaGastos.Liquidacion.Hoja)
            Dim idUser, idHoja, numSel As Integer
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim importeEuros As Decimal
            numSel = 0
            For Each row As GridViewRow In gvLiquidaciones.Rows
                If (CType(row.Cells(2).Controls(0), CheckBox).Checked) Then
                    idHoja = CInt(CType(row.Cells(0).Controls(0), Label).Text)
                    idUser = CInt(CType(row.Cells(6).Controls(0), Label).Text)
                    importeEuros = PageBase.DecimalValue(CType(row.Cells(1).Controls(0), Label).Text)
                    lHojas = New List(Of ELL.HojaGastos.Liquidacion.Hoja)
                    hoja = hojasGastosBLL.loadHoja(idHoja, False)
                    lHojas.Add(New ELL.HojaGastos.Liquidacion.Hoja With {.IdHoja = hoja.Id, .IdViaje = hoja.IdViaje, .IdHojaLibre = hoja.IdSinViaje, .FechaValidacion = hoja.GetFechaEstado(ELL.HojaGastos.eEstado.Validada), .ImporteEuros = importeEuros})
                    linea = New ELL.HojaGastos.Liquidacion With {.TipoLiquidacion = ELL.HojaGastos.Liquidacion.TipoLiq.Metalico, .Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idUser}, False),
                                                                 .Hojas = lHojas, .ImporteTotalEuros = importeEuros}
                    lLineas.Add(linea)
                    numSel += 1
                End If
            Next
            If (numSel > 0) Then
                RaiseEvent Finalizado(lLineas)
            Else
                RaiseEvent Advertencia(itzultzaileWeb.Itzuli("Seleccione alguna linea"))
            End If
        Catch batzEx As BatzException
            RaiseEvent ErrorGenerado(batzEx.Termino)
        Catch ex As Exception
            Dim sms As String = "Error al recoger las hojas de gastos seleccionadas"
            PageBase.log.Error(sms, ex)
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli(sms))
        End Try
    End Sub

#End Region

End Class