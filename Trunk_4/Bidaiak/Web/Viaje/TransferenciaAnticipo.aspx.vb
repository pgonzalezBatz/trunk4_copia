Partial Public Class TransferenciaAnticipo
    Inherits PageBase

#Region "Variables y properties"

    ''' <summary>
    ''' Id de la hoja de la que viene
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property IdHoja As Integer
        Get
            Return CInt(ViewState("IdHoja"))
        End Get
        Set(ByVal value As Integer)
            ViewState("IdHoja") = value
        End Set
    End Property

    ''' <summary>
    ''' Id del viaje destino. Si se informa en el request, habra que editar, sino nuevo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property IdViajeDestino As Integer
        Get
            If (ViewState("IdViajeDest") Is Nothing) Then
                Return 0
            Else
                Return CInt(ViewState("IdViajeDest"))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("IdViajeDest") = value
        End Set
    End Property

    ''' <summary>
    ''' Id del viaje origen
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property IdViajeOrigen As Integer
        Get
            Return CInt(ViewState("IdViajeOrigen"))
        End Get
        Set(ByVal value As Integer)
            ViewState("IdViajeOrigen") = value
        End Set
    End Property

    ''' <summary>
    ''' Id del viaje destino seleccionado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property Edicion As Boolean
        Get
            If (ViewState("Edicion") Is Nothing) Then
                Return False
            Else
                Return CBool(ViewState("Edicion"))
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("Edicion") = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para guardar y leer el origen de donde viene
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property Origen As String
        Get
            If (btnVolver.CommandName = String.Empty) Then
                Return String.Empty
            Else
                Return btnVolver.CommandName
            End If
        End Get
        Set(ByVal value As String)
            btnVolver.CommandName = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si al finalizar la transferencia, se tendra que dar la opcion a reimprimir la hoja de gastos origen
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property ReImprimirHGOrigen As Boolean
        Get
            If (ViewState("ImpHGOrig") Is Nothing) Then
                Return False
            Else
                Return CBool(ViewState("ImpHGOrig"))
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("ImpHGOrig") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si al finalizar la transferencia, se tendra que dar la opcion a reimprimir la hoja de gastos destino
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property ReImprimirHGDestino As Boolean
        Get
            If (ViewState("ImpHGDest") Is Nothing) Then
                Return False
            Else
                Return CBool(ViewState("ImpHGDest"))
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("ImpHGDest") = value
        End Set
    End Property

#End Region

#Region "Page Load e inicializaciones"

    ''' <summary>
    ''' Carga la pagina para hacer una transferencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                Master.SetTitle = "Transferencia de anticipos"
                If (Request.QueryString("idHoja") IsNot Nothing) Then
                    Dim hojasBLL As New BLL.HojasGastosBLL
                    IdHoja = CInt(Request.QueryString("idHoja"))  'Para los casos que viene del detalle de una hoja de gastos
                    Dim oHoja As ELL.HojaGastos = hojasBLL.loadHoja(IdHoja)
                    IdViajeOrigen = oHoja.IdViaje
                ElseIf (Request.QueryString("idViaje") IsNot Nothing) Then 'Para los casos que no tienen hoja de gastos
                    IdViajeOrigen = CInt(Request.QueryString("idViaje"))
                End If
                btnVolver.CommandArgument = IdViajeOrigen  'Guardamos en el boton guardar el idViaje de la hoja origen. Porque si es una edicion, el idViajeOrigen puede cambiar
                If (Request.QueryString("orig") IsNot Nothing) Then Origen = Request.QueryString("orig")
                If (Request.QueryString("AB") IsNot Nothing) Then
                    IdViajeDestino = CInt(Request.QueryString("AB"))
                    Edicion = True
                ElseIf (Request.QueryString("BA") IsNot Nothing) Then
                    IdViajeOrigen = CInt(Request.QueryString("BA"))
                    IdViajeDestino = btnVolver.CommandArgument  'Es el idViaje del origen sin sobreescribir
                    Edicion = True
                End If
                mostrarDetalle()
            Catch batzEx As BatzException
                Master.MensajeError = batzEx.Termino
            End Try
        End If
        If (Not Edicion OrElse (Edicion AndAlso Request.QueryString("AB") IsNot Nothing)) Then
            labelInfo.Text = "Ha entregado parte del anticipo a otra persona de otro viaje"
        Else
            labelInfo.Text = "Ha recibido parte del anticipo de otra persona de otro viaje"
        End If
        Dim script As New StringBuilder
        script.AppendLine("$('#" & dtFechaTransf.ClientID & "').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
        AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(Me, Me.GetType, "DatePicker", script.ToString, True)
    End Sub

    ''' <summary>
    ''' Inicializa la pagina
    ''' </summary>	
    Private Sub inicializar()
        CargarMesesYAños(Now.Month, Now.Year)
        pnlDatosTransf.Visible = False : divSelViajes.Visible = False
        pnlLiquidadorDist.Visible = False : pnlSinAcabar.Visible = False
        txtIdViajeF.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Nº de viaje"))
        searchUserF.PlaceHolder = itzultzaileWeb.Itzuli("Nombre, apellidos, nombre de usuario o numero de trabajador")
        txtIdViajeF.Text = String.Empty : searchUserF.Limpiar()
        pnlFiltro.Visible = True
        divUserOmitidos.Visible = False
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(btnSearchF) : itzultzaileWeb.Itzuli(btnResetF)
            itzultzaileWeb.Itzuli(labelSelViaje) : itzultzaileWeb.Itzuli(labelDivViajeOrig)
            itzultzaileWeb.Itzuli(labelViajeO) : itzultzaileWeb.Itzuli(labelFechasO) : itzultzaileWeb.Itzuli(labelLiqO)
            itzultzaileWeb.Itzuli(labelDivViajeDest) : itzultzaileWeb.Itzuli(labelViajeD) : itzultzaileWeb.Itzuli(labelFechasD)
            itzultzaileWeb.Itzuli(labelLiqD) : itzultzaileWeb.Itzuli(btnTransferir) : itzultzaileWeb.Itzuli(labelDivImp)
            itzultzaileWeb.Itzuli(labelEurosPend) : itzultzaileWeb.Itzuli(labelObserv) : itzultzaileWeb.Itzuli(btnTransferirM)
            itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(labelFecha) : itzultzaileWeb.Itzuli(labelSelViaje)
            itzultzaileWeb.Itzuli(btnVolverF) : itzultzaileWeb.Itzuli(labelSelLiq) : itzultzaileWeb.Itzuli(labelTitleModalReimp)
            itzultzaileWeb.Itzuli(labelReimp) : itzultzaileWeb.Itzuli(labelTitle) : itzultzaileWeb.Itzuli(labelTitleModalTransf)
            itzultzaileWeb.Itzuli(btnCancelM) : itzultzaileWeb.Itzuli(btnVolverReimp)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa el detalle
    ''' </summary>    
    Private Sub inicializarDetalle()
        lblSinAcabar.Text = String.Empty
        lblViajeO.Text = String.Empty : lblFechasO.Text = String.Empty : lblViajeD.Text = String.Empty
        lblLiqO.Text = String.Empty : lblFechasD.Text = String.Empty
        lblLiqD.Text = String.Empty : lblEurosPend.Text = String.Empty : txtObservaciones.Text = String.Empty
        txtFechaTransf.Text = Now.ToShortDateString : hfUsuarioTransf.Value = String.Empty
        labelEurosPend.Visible = True : lblEurosPend.Visible = True
        ddlLiqDest.Items.Clear()
        divCollapse.Attributes.Add("class", "Panel-collapse collapse") 'Se le quita el in para que se pliegue
    End Sub

    ''' <summary>
    ''' <para>Dado un mes y un año, carga los desplegable solo con los valores que podria elegir</para>
    ''' <para>Para el año actual, solo se podra elegir hasta el mes actual</para>
    ''' </summary>
    ''' <param name="month">Mes</param>
    ''' <param name="year">Año</param>
    Private Sub CargarMesesYAños(ByVal month As Integer, ByVal year As Integer)
        Try
            Dim i, año, mesesPintar As Integer
            año = CInt(Now.Date.Year)
            mesesPintar = month
            ddlMes.Items.Clear() : ddlAño.Items.Clear()
            'Si no es el año actual, pintara todos los meses del año
            mesesPintar = If(Now.Date.Year <> year, 12, Now.Date.Month)
            ddlMes.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Mes"), -1))
            For i = 1 To mesesPintar
                ddlMes.Items.Add(New ListItem(MonthName(i).ToUpper, i))
            Next i
            ddlMes.SelectedIndex = -1
            ddlAño.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Año"), -1))
            ddlAño.Items.Add(New ListItem(año - 1, año - 1))
            ddlAño.Items.Add(New ListItem(año, año))
            ddlAño.SelectedIndex = -1
        Catch ex As Exception
            Throw New BatzException("errIKScargandoFechas", ex)
        End Try
    End Sub

#End Region

#Region "Mostrar detalle"

    ''' <summary>
    ''' Se carga el detalle de una transferencia nuevo o ya existente
    ''' </summary>    
    Private Sub mostrarDetalle()
        Try
            inicializar()
            If (Edicion) Then
                Dim viajeBLL As New BLL.ViajesBLL
                Dim oViajeDest As ELL.Viaje = viajeBLL.loadInfo(IdViajeDestino)
                searchUserF.Texto = oViajeDest.ResponsableLiquidacion.NombreCompleto
                searchUserF.SelectedId = oViajeDest.ResponsableLiquidacion.Id
                mostrarDetalleAvanzado()
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("TRANSFERENCIA: Error al mostrar el detalle inicial", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el detalle con la informacion del viaje origen y viaje destino
    ''' </summary>    
    Private Sub mostrarDetalleAvanzado()
        Try
            inicializarDetalle()
            Dim viajesBLL As New BLL.ViajesBLL
            Dim hojasBLL As New BLL.HojasGastosBLL
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim lMovs As List(Of ELL.Anticipo.Movimiento) = Nothing
            Dim oVOrig As ELL.Viaje = viajesBLL.loadInfo(IdViajeOrigen, True, True) 'Los dos parametros a true es para que pueda calcular el importe que le queda por justificar
            Dim oVDest As ELL.Viaje = viajesBLL.loadInfo(IdViajeDestino)
            Dim bViajeDestConAnticipo = (oVDest.Anticipo IsNot Nothing)
            'No se podran pasar anticipos a viajes mas antiguos que el origen
            If (oVOrig.FechaIda > oVDest.FechaIda) Then
                log.Warn("TRANSFERENCIA: No se puede realizar la transferencia a un viaje mas antiguo que el origen.Viaje Origen:" & oVOrig.IdViaje & " | Viaje destino:" & oVDest.IdViaje)
                pnlDatosTransf.Visible = False : pnlSinAcabar.Visible = True
                lblSinAcabar.Text = itzultzaileWeb.Itzuli("No se puede realizar la transferencia a un viaje mas antiguo que el origen")
                upDatosTransf.Update()
                Exit Sub
            End If
            If (Edicion) Then
                'Se obtienen los movimientos de tipo transferencia cuyo idViajeDestino sea el parametrizado
                lMovs = oVOrig.Anticipo.Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) If(o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia, o.IdViajeOrigen = oVOrig.IdViaje And o.IdViajeDestino = IdViajeDestino, False))
            End If
            divSelViajes.Visible = False
            btnTransferir.Visible = (Not Edicion) : pnlFiltro.Visible = (Not Edicion)
            ddlLiqDest.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
            For Each oInt As ELL.Viaje.Integrante In oVDest.ListaIntegrantes
                'Solo se añaden los integrantes que puedan recibir anticipos
                If (bidaiakBLL.PuedeRecibirVisasAnticipos(oInt.Usuario, Master.IdPlantaGestion)) Then
                    ddlLiqDest.Items.Add(New ListItem(oInt.Usuario.NombreCompleto, oInt.Usuario.Id))
                ElseIf (Not Edicion) Then
                    divUserOmitidos.Visible = True : itzultzaileWeb.Itzuli(labelUserOmitidos)
                End If
            Next
            If (searchUserF.SelectedId <> String.Empty) Then ddlLiqDest.SelectedIndex = ddlLiqDest.Items.IndexOf(ddlLiqDest.Items.FindByValue(CInt(searchUserF.SelectedId)))
            'Se comprueba que si el viaje destino, tiene un anticipo no entregado, no dejara continuar
            If (bViajeDestConAnticipo AndAlso (oVDest.Anticipo.Estado <> ELL.Anticipo.EstadoAnticipo.Entregado AndAlso oVDest.Anticipo.Estado <> ELL.Anticipo.EstadoAnticipo.cerrado AndAlso oVDest.Anticipo.Estado <> ELL.Anticipo.EstadoAnticipo.cancelada)) Then
                pnlDatosTransf.Visible = False : pnlSinAcabar.Visible = True
                lblSinAcabar.Text = itzultzaileWeb.Itzuli("No se puede realizar la transferencia porque el viaje destino tiene un anticipo pendiente de recibir. Contacte con el departamento financiero")
                log.Warn("TRANSFERENCIA: No se puede realizar la transferencia porque el anticipo del viaje destino " & oVDest.IdViaje & " no ha sido entregado")
                upDatosTransf.Update()
                Exit Sub
            End If
            'Se comprueba el estado de la HG del liquidador del anticipo de origen
            Dim hojaOrig As ELL.HojaGastos = oVOrig.HojasGastos.Find(Function(o As ELL.HojaGastos) o.Usuario.Id = oVOrig.ResponsableLiquidacion.Id)
            ReImprimirHGOrigen = (hojaOrig IsNot Nothing AndAlso hojaOrig.Estado <> ELL.HojaGastos.eEstado.Rellenada)
            If (Not Edicion AndAlso ReImprimirHGOrigen AndAlso Not (hasProfile(BLL.BidaiakBLL.Profiles.Financiero, BLL.BidaiakBLL.Profiles.Administrador))) Then
                'Si hay que reimprimir y no es ni admin ni financiero, no se permitira
                pnlDatosTransf.Visible = False : pnlSinAcabar.Visible = True
                lblSinAcabar.Text = itzultzaileWeb.Itzuli("No se puede realizar la transferencia porque la hoja de gastos del liquidador [LIQUIDADOR] del viaje [VIAJE] esta en estado [ESTADO]. Contacte con el para que su validador desvalide su hoja")
                lblSinAcabar.Text = lblSinAcabar.Text.Replace("[LIQUIDADOR]", oVOrig.ResponsableLiquidacion.NombreCompleto)
                lblSinAcabar.Text = lblSinAcabar.Text.Replace("[VIAJE]", oVOrig.Destino & " (" & oVOrig.IdViaje & ")")
                lblSinAcabar.Text = lblSinAcabar.Text.Replace("[ESTADO]", [Enum].GetName(GetType(ELL.HojaGastos.eEstado), hojaOrig.Estado))
                log.Warn("TRANSFERENCIA: No se puede realizar la transferencia porque la hoja de gastos origen del liquidador " & oVOrig.ResponsableLiquidacion.NombreCompleto & " del viaje " & oVOrig.Destino & " (" & oVOrig.IdViaje & ")" & " esta en estado " & [Enum].GetName(GetType(ELL.HojaGastos.eEstado), hojaOrig.Estado) & ". Contacte con el para que su validador desvalide su hoja")
                upDatosTransf.Update()
                Exit Sub
            End If
            'Se comprueba el estado de la HG del liquidador del viaje destino
            Dim lHojas As List(Of ELL.HojaGastos) = Nothing
            If (oVDest.ResponsableLiquidacion IsNot Nothing) Then lHojas = hojasBLL.loadHojas(Master.IdPlantaGestion, oVDest.ResponsableLiquidacion.Id, IdViajeDestino)
            Dim oHoja As ELL.HojaGastos = Nothing
            If (Not Edicion AndAlso lHojas IsNot Nothing AndAlso lHojas.Count = 1) Then oHoja = lHojas.FirstOrDefault 'Cuando se esta en modo edicion da igual el estado porque es solo de consulta
            ReImprimirHGDestino = (oHoja IsNot Nothing AndAlso oHoja.Estado <> ELL.HojaGastos.eEstado.Rellenada)
            If (Not Edicion AndAlso ReImprimirHGDestino AndAlso Not (hasProfile(BLL.BidaiakBLL.Profiles.Financiero, BLL.BidaiakBLL.Profiles.Administrador))) Then
                'Si hay que reimprimir y no es ni admin ni financiero, no se permitira
                pnlDatosTransf.Visible = False : pnlSinAcabar.Visible = True
                lblSinAcabar.Text = itzultzaileWeb.Itzuli("No se puede realizar la transferencia porque la hoja de gastos del liquidador [LIQUIDADOR] del viaje [VIAJE] esta en estado [ESTADO]. Contacte con el para que su validador desvalide su hoja")
                lblSinAcabar.Text = lblSinAcabar.Text.Replace("[LIQUIDADOR]", oVDest.ResponsableLiquidacion.NombreCompleto)
                lblSinAcabar.Text = lblSinAcabar.Text.Replace("[VIAJE]", oVDest.Destino & " (" & oVDest.IdViaje & ")")
                lblSinAcabar.Text = lblSinAcabar.Text.Replace("[ESTADO]", [Enum].GetName(GetType(ELL.HojaGastos.eEstado), oHoja.Estado))
                log.Warn("TRANSFERENCIA: No se puede realizar la transferencia porque la hoja de gastos destino del liquidador " & oVDest.ResponsableLiquidacion.NombreCompleto & " del viaje " & oVDest.Destino & " (" & oVDest.IdViaje & ")" & " esta en estado " & [Enum].GetName(GetType(ELL.HojaGastos.eEstado), oHoja.Estado) & ". Contacte con el para que su validador desvalide su hoja")
                upDatosTransf.Update()
                Exit Sub
            End If
            'El viaje destino, no tiene asignado ningun anticipo
            pnlDatosTransf.Visible = True : pnlSinAcabar.Visible = False
            labelEurosPend.Visible = (Not Edicion) : lblEurosPend.Visible = labelEurosPend.Visible
            ddlLiqDest.Enabled = (Not Edicion)
            If (bViajeDestConAnticipo) Then
                pnlLiquidadorDist.Visible = True : divUserOmitidos.Visible = False
                lblLiqD.Text = oVDest.ResponsableLiquidacion.NombreCompleto
                ddlLiqDest.SelectedValue = oVDest.ResponsableLiquidacion.Id
                ddlLiqDest.Enabled = False : ddlLiqDest.ToolTip = itzultzaileWeb.Itzuli("Existe un anticipo contra este viaje y solamente se le puede entregar el dinero al actual liquidador")
            End If
            If (pnlLiquidadorDist.Visible) Then pnlLiquidadorDist.Visible = (Not Edicion)
            'Informacion del viaje origen                        
            lblViajeO.Text = oVOrig.Destino & " (" & oVOrig.IdViaje & ")"
            lblFechasO.Text = oVOrig.FechaIda.ToShortDateString & " - " & oVOrig.FechaVuelta.ToShortDateString
            lblLiqO.Text = oVOrig.ResponsableLiquidacion.NombreCompleto
            hfUsuarioTransf.Value = oVOrig.ResponsableLiquidacion.Id
            'Informacion del viaje destino            
            lblViajeD.Text = oVDest.Destino & " (" & oVDest.IdViaje & ")"
            lblFechasD.Text = oVDest.FechaIda.ToShortDateString & " - " & oVDest.FechaVuelta.ToShortDateString
            lblEurosPend.Text = Math.Round(oVOrig.Anticipo.EurosPendientes, 2) - Math.Round(oVOrig.Anticipo.EurosPendientesHojaGastosLiq, 2)
            dtFechaTransf.Visible = (Not Edicion) : lblFechaTransf.Visible = Edicion
            If (Not Edicion) Then
                txtFechaTransf.Text = Now.ToShortDateString
                selImportes.Importes = Nothing
            Else
                If (lMovs IsNot Nothing) Then ddlLiqDest.SelectedValue = lMovs.First.UserDestino.Id
                lblFechaTransf.Text = lMovs.First.Fecha
                selImportes.Importes = lMovs
                selImportes.Modificable = False
                txtObservaciones.Text = lMovs.First.Comentarios
            End If
            selImportes.Inicializar()
            upDatosTransf.Update()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("TRANSFERENCIA: Error al mostrar el detalle avanzado", ex)
        End Try
    End Sub

#End Region

#Region "Busqueda y seleccion de viajes"

    ''' <summary>
    ''' Busca en que viajes estaba esa persona
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSearchF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchF.Click
        Try
            If (ddlAño.SelectedValue = -1 And ddlMes.SelectedValue = -1 And searchUserF.SelectedId = String.Empty And txtIdViajeF.Text.Trim = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca todos los datos")
            ElseIf (ddlAño.SelectedValue = -1 And ddlMes.SelectedValue <> -1) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Si introduce el mes, el año es obligatorio")
            Else
                If (searchUserF.Texto = String.Empty) Then searchUserF.SelectedId = String.Empty
                divSelViajes.Visible = True : pnlDatosTransf.Visible = False
                pnlSinAcabar.Visible = False
                lblPersonaTransf.Text = searchUserF.Texto
                'Hay que pasarle los parametros de busqueda
                selViajes.IdViajeOmitir = IdViajeOrigen
                selViajes.Año = If(ddlAño.SelectedValue > -1, CInt(ddlAño.SelectedValue), 0)
                selViajes.Mes = If(ddlMes.SelectedValue > -1, CInt(ddlMes.SelectedValue), 0)
                selViajes.IdUsuario = If(searchUserF.SelectedId <> String.Empty, CInt(searchUserF.SelectedId), 0)
                selViajes.IdViaje = If(txtIdViajeF.Text <> String.Empty, CInt(txtIdViajeF.Text), 0)
                selViajes.ConAntipo = False
                selViajes.Inicializar()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Resetea los valores del filtro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnResetF_Click(sender As Object, e As EventArgs) Handles btnResetF.Click
        Try
            txtIdViajeF.Text = String.Empty
            searchUserF.Limpiar()
            CargarMesesYAños(Now.Month, Now.Year)
            selViajes.Inicializar()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub
    ''' <summary>
    ''' Se ha seleccionado un viaje
    ''' </summary>
    ''' <param name="idViaje">Viaje seleccionado</param>	
    Private Sub selViajes_ViajeSeleccionado(ByVal idViaje As Integer) Handles selViajes.ViajeSeleccionado
        Try
            IdViajeDestino = idViaje
            mostrarDetalleAvanzado()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar la informacion del viaje")
        End Try
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Cuando se cambia la fecha, se alimenta la propiedad del control importe con la fecha cambiada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub txtFechaTransf_TextChanged(sender As Object, e As EventArgs) Handles txtFechaTransf.TextChanged
        Dim anticipBLL As New BLL.AnticiposBLL
        Dim fechaEntreg As DateTime = anticipBLL.loadAnticipoFechaEntrega(IdViajeOrigen)
        If (fechaEntreg = DateTime.MinValue) Then fechaEntreg = CDate(txtFechaTransf.Text)
        selImportes.FechaCalculoConversionEuros = fechaEntreg
    End Sub

    ''' <summary>
    ''' Se comprobara que el importe a transferir, no se mayor al pendiente
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnTransferir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTransferir.Click
        Dim eurosPend As Decimal = CDec(lblEurosPend.Text)
        If (CInt(ddlLiqDest.SelectedValue) = Integer.MinValue) Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar un destinatario")
        ElseIf (selImportes.Importes.Count = 0) Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe introducir el importe")
        ElseIf (eurosPend >= 0) Then
            Dim liquidador As String = ddlLiqDest.SelectedItem.Text
            lblTextoTransf.Text = itzultzaileWeb.Itzuli("Se van a transferir [IMPORTE] euros a [LIQUIDADOR] y se le va a avisar por email.") & "<br />"
            lblTextoTransf.Text &= itzultzaileWeb.Itzuli("Al finalizar, consulte de nuevo su hoja de gastos para ver el nuevo movimiento de transferencia")
            lblTextoTransf.Text = lblTextoTransf.Text.Replace("[IMPORTE]", selImportes.TotalImporte)
            lblTextoTransf.Text = lblTextoTransf.Text.Replace("[LIQUIDADOR]", liquidador)
            ShowModalTransf(True)
        Else
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El importe de la transferencia no puede superar el importe de euros pendientes")
        End If
    End Sub

    ''' <summary>
    ''' Se acepta la transferencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnTransferirM_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTransferirM.Click
        Try
            Transferir()
            ShowModalTransf(False)
        Catch batzEx As BatzException
            ShowModalTransf(True)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el panel modal de transferencia
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalTransf(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModalTransf').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModalTransf').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True)
        End If
    End Sub

    ''' <summary>
    ''' Muestra el panel modal de reimpresion
    ''' </summary>
    Private Sub ShowModalReimpresion()
        ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModalReimp').modal('show');", True)
    End Sub

    ''' <summary>
    ''' Transfiere los importes especificados
    ''' </summary>    
    Private Sub Transferir()
        Try
            If (IdViajeDestino <= 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar el viaje destino de la transferencia")
            ElseIf (selImportes.Importes.Count = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe introducir el importe")
            Else
                Dim anticBLL As New BLL.AnticiposBLL
                Dim hojasBLL As New BLL.HojasGastosBLL
                Dim viajesBLL As New BLL.ViajesBLL
                Dim oViajeDest As ELL.Viaje = viajesBLL.loadInfo(IdViajeDestino)
                Dim lMovimientos As New List(Of ELL.Anticipo.Movimiento)
                Dim oMov As ELL.Anticipo.Movimiento
                Dim importeTotal As Decimal = 0
                Dim cantidad, body, liquidadorDest As String
                cantidad = String.Empty : body = String.Empty
                Dim bLiquidadorDistintoRec As Boolean = (oViajeDest.ResponsableLiquidacion IsNot Nothing AndAlso oViajeDest.ResponsableLiquidacion.Id <> CInt(ddlLiqDest.SelectedValue))
                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                liquidadorDest = ddlLiqDest.SelectedItem.Text
                'Se comprueba que contra ese viaje, no se haya realizado ya ninguna otra transferencia
                If (oViajeDest.Anticipo IsNot Nothing) Then
                    Dim lMovExistentes As List(Of ELL.Anticipo.Movimiento) = anticBLL.loadMovimientos(oViajeDest.Anticipo.IdViaje, ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia, False)
                    If (lMovExistentes IsNot Nothing AndAlso lMovExistentes.Count > 0) Then
                        If (lMovExistentes.Exists(Function(o As ELL.Anticipo.Movimiento) o.IdViajeOrigen = IdViajeOrigen And o.IdViajeDestino = IdViajeDestino)) Then
                            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No se puede realizar mas de una transferencia contra el mismo viaje")
                            Exit Sub
                        End If
                    End If
                End If
                'Se comprueba que la hoja de gasto destino, no este validada si reimprimir es false. Si es true ya se habra comprobado
                '------------------------------------------------------------------------------------------------------------------------
                Dim idLiquidador As Integer = If(bLiquidadorDistintoRec, oViajeDest.ResponsableLiquidacion.Id, CInt(ddlLiqDest.SelectedValue))
                Dim lHojas As List(Of ELL.HojaGastos) = hojasBLL.loadHojas(Master.IdPlantaGestion, idLiquidador, IdViajeDestino, Date.MinValue, Date.MinValue, False, True, True)
                Dim oHoja As ELL.HojaGastos = Nothing
                Dim xbatComp As New BLL.XbatBLL
                If (lHojas IsNot Nothing AndAlso lHojas.Count = 1) Then oHoja = lHojas.FirstOrDefault 'Cuando se esta en modo edicion da igual el estado porque es solo de consulta
                ReImprimirHGDestino = (oHoja IsNot Nothing AndAlso (oHoja.Estado <> ELL.HojaGastos.eEstado.Rellenada AndAlso oHoja.Estado <> ELL.HojaGastos.eEstado.NoValidada))  'Se añade no validada porque cuando se desvalida una hoja se queda en este estado
                If (ReImprimirHGDestino AndAlso Not (hasProfile(BLL.BidaiakBLL.Profiles.Financiero, Master.Perfil = BLL.BidaiakBLL.Profiles.Administrador))) Then
                    Dim oVDest As ELL.Viaje = viajesBLL.loadInfo(IdViajeDestino)
                    'Si hay que reimprimir y no es ni admin ni financiero, no se permitira
                    Dim texto As String = String.Empty
                    If (oVDest.Anticipo IsNot Nothing) Then
                        texto = itzultzaileWeb.Itzuli("No se puede realizar la transferencia porque la hoja de gastos del liquidador [LIQUIDADOR] del viaje [VIAJE] esta en estado [ESTADO]. Contacte con el para que su validador desvalide su hoja")
                        texto = texto.Replace("[LIQUIDADOR]", oVDest.ResponsableLiquidacion.NombreCompleto)
                        log.Warn("TRANSFERENCIA: No se puede realizar la transferencia porque la hoja de gastos destino del liquidador " & oVDest.ResponsableLiquidacion.NombreCompleto & " del viaje " & oVDest.Destino & " (" & oVDest.IdViaje & ")" & " esta en estado " & [Enum].GetName(GetType(ELL.HojaGastos.eEstado), oHoja.Estado) & ". Contacte con el para que su validador desvalide su hoja")
                    Else
                        texto = itzultzaileWeb.Itzuli("No se puede realizar la transferencia porque la hoja de gastos de [PERSONA] del viaje [VIAJE] esta en estado [ESTADO]. Contacte con el para que su validador desvalide su hoja")
                        texto = texto.Replace("[PERSONA]", liquidadorDest)
                        log.Warn("TRANSFERENCIA: No se puede realizar la transferencia porque la hoja de gastos destino de " & liquidadorDest & " del viaje " & oVDest.Destino & " (" & oVDest.IdViaje & ")" & " esta en estado " & [Enum].GetName(GetType(ELL.HojaGastos.eEstado), oHoja.Estado) & ". Contacte con el para que su validador desvalide su hoja")
                    End If
                    texto = texto.Replace("[VIAJE]", oVDest.Destino & " (" & oVDest.IdViaje & ")")
                    texto = texto.Replace("[ESTADO]", [Enum].GetName(GetType(ELL.HojaGastos.eEstado), oHoja.Estado))
                    Master.MensajeAdvertencia = texto
                    upDatosTransf.Update()
                    Exit Sub
                End If
                Dim fechaEntregAntic, fechaAuxEntregAnticipo As DateTime
                Dim cambioMoneda As Decimal = 0
                fechaEntregAntic = anticBLL.loadAnticipoFechaEntrega(IdViajeOrigen) 'Se intenta coger la fecha de entrega del anticipo de origen
                For Each import As ELL.Anticipo.Movimiento In selImportes.Importes
                    oMov = New ELL.Anticipo.Movimiento With {.IdAnticipo = IdViajeOrigen, .Cantidad = CDec(import.Cantidad), .IdViajeOrigen = IdViajeOrigen, .IdViajeDestino = IdViajeDestino,
                        .Moneda = New ELL.Moneda With {.Id = CInt(import.Moneda.Id)}, .TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia}
                    oMov.UserOrigen = New SabLib.ELL.Usuario With {.Id = CInt(hfUsuarioTransf.Value)}  'usuario liquidador del viaje de origen                    
                    If (bLiquidadorDistintoRec) Then
                        'Si se le asigna a uno distinto al responsable de liquidacion
                        oMov.UserDestino = oViajeDest.ResponsableLiquidacion
                    Else
                        oMov.UserDestino = New SabLib.ELL.Usuario With {.Id = CInt(ddlLiqDest.SelectedValue)}
                    End If
                    oMov.Fecha = CDate(txtFechaTransf.Text)
                    oMov.Fecha = New DateTime(oMov.Fecha.Year, oMov.Fecha.Month, oMov.Fecha.Day, Now.Hour, Now.Minute, Now.Second) 'Para que no le meta hora 00:00:00                    
                    If (bLiquidadorDistintoRec) Then oMov.Comentarios = "El dinero se entrego a " & ddlLiqDest.SelectedItem.Text & "." & vbCrLf
                    oMov.Comentarios &= txtObservaciones.Text.Trim
                    fechaAuxEntregAnticipo = If(fechaEntregAntic <> DateTime.MinValue, fechaEntregAntic, oMov.Fecha)
                    cambioMoneda = 0
                    oMov.ImporteEuros = xbatComp.ObtenerRateEuros(import.Moneda.Id, import.Cantidad, fechaAuxEntregAnticipo, cambioMoneda)
                    oMov.CambioMonedaEUR = cambioMoneda
                    If (cantidad <> String.Empty) Then cantidad &= ","
                    cantidad &= import.Cantidad & " " & import.Moneda.Abreviatura
                    importeTotal += import.Cantidad
                    lMovimientos.Add(oMov)
                Next
                If (CDec(lblEurosPend.Text) < 0) Then
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El importe de la transferencia no puede superar el importe de euros pendientes")
                Else
                    anticBLL.Transferencia(lMovimientos)
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("Cantidad transferida")
                    If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
                        'Se envia un email al que se le dio el dinero para que este al corriente                        
                        Dim oUser As SabLib.ELL.Usuario = Nothing
                        Dim userBLL As New SabLib.BLL.UsuariosComponent
                        Try
                            oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(ddlLiqDest.SelectedValue)}, False)
                            body = lblLiqO.Text & " del viaje " & lblViajeO.Text & " le ha transferido los siguientes importes: <br />" & cantidad
                            If (bLiquidadorDistintoRec) Then body &= "<br /><br />Estos importes los tiene que justificar el liquidador del viaje " & lblLiqD.Text
                            If (txtObservaciones.Text.Trim <> String.Empty) Then body &= "<br /><br />Observaciones:<br />" & txtObservaciones.Text.Trim
                            body = PageBase.getBodyHmtl("Transferencia de importes", "V" & IdViajeDestino, body)
                            SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), oUser.Email, "Transferencia de anticipo", body, serverEmail)
                            log.Info("TRANSFERENCIA: Se ha enviado un email a " & oUser.NombreCompleto & " para avisarle de que " & lblLiqO.Text & " le dio el dinero del anticipo")
                        Catch ex As Exception
                            log.Error("TRANSFERENCIA: No se le ha podido avisar a " & oUser.NombreCompleto & " de que el usuario " & lblLiqO.Text & " le ha realizado una transferencia entre viajes", ex)
                        End Try
                        'Si se le ha entregado a uno que no es el liquidador, se le avisa tambien al liquidador
                        If (bLiquidadorDistintoRec) Then
                            Try
                                oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViajeDest.ResponsableLiquidacion.Id}, False)
                                body = lblLiqO.Text & " del viaje " & lblViajeO.Text & " le ha transferido  los siguientes importes: <br />" & cantidad
                                body &= "<br /><br />Estos importes le fueron entregados a " & ddlLiqDest.SelectedItem.Text & " pero es usted el que debe justificarlos por ser el liquidador del viaje"
                                If (txtObservaciones.Text.Trim <> String.Empty) Then body &= "<br /><br />Observaciones:<br />" & txtObservaciones.Text.Trim
                                body = PageBase.getBodyHmtl("Transferencia de importes", "V" & IdViajeDestino, body)
                                SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), oUser.Email, "Transferencia de anticipo", body, serverEmail)
                                log.Info("TRANSFERENCIA: Se ha enviado un email a " & oUser.NombreCompleto & " para avisarle de que " & lblLiqO.Text & " le ha realizado una tranferencia de anticipo")
                            Catch ex As Exception
                                log.Error("TRANSFERENCIA: No se le ha podido avisar a " & oUser.NombreCompleto & " de que el usuario " & lblLiqO.Text & " le ha realizado una tranferencia de anticipo", ex)
                            End Try
                        End If
                    End If
                    Dim mensa As String = "TRANSFERENCIA:Se ha realizado una transferencia del viaje " & lblViajeO.Text & " al viaje " & lblViajeD.Text &
                        " del usuario " & lblLiqO.Text & " al usuario " & liquidadorDest &
                        " de " & cantidad
                    If (bLiquidadorDistintoRec) Then mensa &= vbCrLf & "La transferencia le fue entregada pero el no es el liquidador del viaje"
                    log.Info(mensa)
                    'Si tiene el perfil de financiero y se ha marcado para reimprimir unas hojas de gasto, se mostrara un popup para informar e imprimir las hojas
                    If ((ReImprimirHGOrigen OrElse ReImprimirHGDestino) AndAlso (hasProfile(BLL.BidaiakBLL.Profiles.Administrador, BLL.BidaiakBLL.Profiles.Financiero))) Then
                        Dim textoReimp, textoAux, idHojas As String
                        Dim lDatos As New List(Of Object)
                        textoReimp = String.Empty : idHojas = String.Empty : textoAux = String.Empty
                        If (ReImprimirHGOrigen) Then
                            Dim oVOrig As ELL.Viaje = viajesBLL.loadInfo(IdViajeOrigen, True)
                            Dim hojaOrig As ELL.HojaGastos = oVOrig.HojasGastos.Find(Function(o As ELL.HojaGastos) o.Usuario.Id = oVOrig.ResponsableLiquidacion.Id)
                            textoReimp = "H" & IdViajeOrigen & " [" & itzultzaileWeb.Itzuli("Origen") & "] " & hojaOrig.Usuario.NombreCompleto
                            idHojas = hojaOrig.Id
                            textoAux = "origen"
                            lDatos.Add(New With {.IdHoja = hojaOrig.Id, .Texto = textoReimp})
                        End If
                        If (ReImprimirHGDestino) Then
                            textoReimp = "H" & IdViajeDestino & " [" & itzultzaileWeb.Itzuli("Destino") & "] " & oHoja.Usuario.NombreCompleto
                            idHojas &= If(idHojas <> String.Empty, ",", "") & oHoja.Id
                            textoAux &= If(textoAux <> String.Empty, " y ", "") & "destino"
                            lDatos.Add(New With {.IdHoja = oHoja.Id, .Texto = textoReimp})
                        End If
                        rptHojas.DataSource = lDatos
                        rptHojas.DataBind()
                        log.Info("TRANSFERENCIA: Las hojas (" & idHojas & " - " & textoAux & ") se van a tener que reimprimir ya que estaban validadas y se ha realizado un nuevo movimiento de transferencia")
                        ShowModalReimpresion()
                    Else
                        Volver()
                    End If
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("TRANSFERENCIA:Ha ocurrido un error al realizar la transferencia del viaje " & lblViajeO.Text & "(" & lblLiqO.Text & ") al viaje " & lblViajeD.Text & "(" & lblLiqD.Text & ")", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al realizar la operacion")
        End Try
    End Sub

    ''' <summary>
    ''' Se enlaza con las hojas de gastos a reimprimir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rptHojas_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptHojas.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item = e.Item.DataItem
            Dim hlHG As HyperLink = CType(e.Item.FindControl("hlHG"), HyperLink)
            hlHG.Text = item.Texto
            hlHG.NavigateUrl = "~\Publico\ViewDocument.aspx?id=" & item.IdHoja & "&tipo=hg"
            itzultzaileWeb.Itzuli(hlHG)
        End If
    End Sub

    ''' <summary>
    ''' Vuelve a la pagina de anticipos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click, btnVolverF.Click
        Volver()
    End Sub

    ''' <summary>
    ''' Se vuelve a la hoja
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolverReimp_Click(sender As Object, e As EventArgs) Handles btnVolverReimp.Click
        log.Info("Se cancela la reimpresion de las hojas de gastos despues de realizar la transferencia")
        Volver()
    End Sub

    ''' <summary>
    ''' Vuelve a la pagina de gestion de anticipos
    ''' </summary>    
    Private Sub Volver()
        If (Origen = "HG") Then 'Hoja de gastos
            Response.Redirect("HojaGastos.aspx?id=" & IdHoja, False)
        ElseIf (Origen = "ANT" And (hasProfile(BLL.BidaiakBLL.Profiles.Financiero, BLL.BidaiakBLL.Profiles.Administrador))) Then 'Anticipo
            Response.Redirect("..\Financiero\Anticipos\GestionAnticipos.aspx?idViaje=" & btnVolver.CommandArgument, False)
        End If
    End Sub

#End Region

#Region "Control importes"

    ''' <summary>
    ''' Obtiene el importe en euros modificado y su operacion
    ''' IMPORTANTE: En este caso, cuando se ha añadido un importe se tiene que deducir del importe a justificar. Cuando se quita, se añade
    ''' </summary>
    ''' <param name="importe">Importe a añadir o quitar</param>
    ''' <param name="operacion">0:Añadir,1:Quitar</param>    
    Private Sub selImportes_ImporteModificado(ByVal importe As Decimal, ByVal operacion As Integer) Handles selImportes.ImporteModificado
        Dim eurosPend As Decimal = If(operacion = 1, CDec(lblEurosPend.Text) + importe, CDec(lblEurosPend.Text) - importe)
        lblEurosPend.Text = eurosPend
        If (eurosPend >= 0) Then
            lblEurosPend.CssClass = "text-info"
        Else
            lblEurosPend.CssClass = "text-danger"
        End If
    End Sub

#End Region

End Class