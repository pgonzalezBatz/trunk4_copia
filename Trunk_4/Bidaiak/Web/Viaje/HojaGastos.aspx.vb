Public Class HojaGastos
    Inherits PageBase

#Region "Properties"

    Private hojasBLL As New BLL.HojasGastosBLL
    Private hImportaciones As Hashtable

    ''' <summary>
    ''' Propiedad para guardar y leer el origen de donde viene
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property Origen As String
        Get
            If (ViewState("Origen") Is Nothing) Then
                Return String.Empty
            Else
                Return CStr(ViewState("Origen"))
            End If
        End Get
        Set(ByVal value As String)
            ViewState("Origen") = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para guardar y leer el id del boton volver
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
    ''' Propiedad para guardar y leer el id del viaje
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property IdViaje As Integer
        Get
            Return CInt(ViewState("IdViaje"))
        End Get
        Set(ByVal value As Integer)
            ViewState("IdViaje") = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para guardar y leer el id del viaje
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property IdUser As Integer
        Get
            If (ViewState("IdUser") Is Nothing) Then
                Return Integer.MinValue
            Else
                Return CInt(ViewState("IdUser"))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("IdUser") = value
        End Set
    End Property

    ''' <summary>
    ''' Es para saber si viene el de financiero viene de las hg o de los anticipos
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property OrigenHG_Financiero As Boolean
        Get
            If (ViewState("origHG") Is Nothing) Then ViewState("origHG") = True
            Return CType(ViewState("origHG"), Boolean)
        End Get
        Set(value As Boolean)
            ViewState("origHG") = value
        End Set
    End Property

    Private precioKm As Decimal = 0

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Muestra la hoja de gastos del usuario que entra
    ''' Si fuera el organizador del viaje, mostrara el de todos los integrantes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Hoja de gastos"
                IdHoja = Integer.MinValue : IdViaje = Integer.MinValue
                If (Request.QueryString("Id") IsNot Nothing) Then IdHoja = CInt(Request.QueryString("Id"))
                If (Request.QueryString("IdViaje") IsNot Nothing) Then IdViaje = Request.QueryString("IdViaje")
                If (Request.QueryString("orig") IsNot Nothing) Then Origen = Request.QueryString("orig")
                If (Request.QueryString("IdUser") IsNot Nothing) Then IdUser = Request.QueryString("IdUser")
                If (hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) Then
                    Try
                        Dim pagReferer As String = Page.Request.UrlReferrer.Segments(Page.Request.UrlReferrer.Segments.GetUpperBound(0)).ToString.ToLower
                        OrigenHG_Financiero = (pagReferer = "viajes.aspx" OrElse pagReferer = "hojasgastossinviaje.aspx" OrElse pagReferer = "infohg.aspx")
                    Catch ex As Exception
                        log.Error("Error en el page load de solicitud de viaje al comprobar si viene del listado de viajes", ex)
                    End Try
                End If
                mostrarDetalle()
            End If
            Page.MaintainScrollPositionOnPostBack = True
            Dim script As New StringBuilder
            script.AppendLine("$('#dtFechaMet').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
            script.AppendLine("$('#dtFechaKm').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(Me, Me.GetType, "DatePicker", script.ToString, True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub HojaGastos_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelViaje) : itzultzaileWeb.Itzuli(labelFechas) : itzultzaileWeb.Itzuli(btnValidarHoja) : itzultzaileWeb.Itzuli(btnRechazarHoja)
            itzultzaileWeb.Itzuli(labelSelUsu) : itzultzaileWeb.Itzuli(labelDivCabMetalico) : itzultzaileWeb.Itzuli(labelFMet)
            itzultzaileWeb.Itzuli(rfvFechaMet) : itzultzaileWeb.Itzuli(labelConceptoConReciboMet) : itzultzaileWeb.Itzuli(txtConceptoConReciboMet) : itzultzaileWeb.Itzuli(rfvConceptoConReciboMet) : itzultzaileWeb.Itzuli(btnAddLineaMetalico)
            itzultzaileWeb.Itzuli(labelImporteMet) : itzultzaileWeb.Itzuli(rfvImporteMet) : itzultzaileWeb.Itzuli(labelMonedaMet) : itzultzaileWeb.Itzuli(labelTitleKm) : itzultzaileWeb.Itzuli(labelCancelarModal)
            itzultzaileWeb.Itzuli(labelDivCabKilometraje) : itzultzaileWeb.Itzuli(labelFechaKm) : itzultzaileWeb.Itzuli(rfvFechaKm) : itzultzaileWeb.Itzuli(labelKm) : itzultzaileWeb.Itzuli(rfvKm)
            itzultzaileWeb.Itzuli(labelOrigen) : itzultzaileWeb.Itzuli(rfvOrigenKm) : itzultzaileWeb.Itzuli(labelDestino) : itzultzaileWeb.Itzuli(rfvDestinoKm)
            itzultzaileWeb.Itzuli(labelSelFechaTray) : itzultzaileWeb.Itzuli(labelDivTotal) : itzultzaileWeb.Itzuli(btnVerAnticipo) : itzultzaileWeb.Itzuli(revJustifi) : itzultzaileWeb.Itzuli(lnkVerEstados1)
            itzultzaileWeb.Itzuli(lblLabelAntRec) : itzultzaileWeb.Itzuli(labelTotGastosMet) : itzultzaileWeb.Itzuli(labelDiferencia) : itzultzaileWeb.Itzuli(btnEnviar) : itzultzaileWeb.Itzuli(btnImprimir)
            itzultzaileWeb.Itzuli(btnEliminar) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(labelInfo1) : itzultzaileWeb.Itzuli(labelInfo2) : itzultzaileWeb.Itzuli(btnAceptarModal)
            itzultzaileWeb.Itzuli(labelDivCabVisa) : itzultzaileWeb.Itzuli(btnTransferir) : itzultzaileWeb.Itzuli(labelCancelarModalJusfit)
            itzultzaileWeb.Itzuli(labelDivCabDocumentacion) : itzultzaileWeb.Itzuli(labelDocInfo1) : itzultzaileWeb.Itzuli(btnSubirDoc) : itzultzaileWeb.Itzuli(labelDocTitulo) : itzultzaileWeb.Itzuli(labelDocAdj)
            itzultzaileWeb.Itzuli(labelMensaVisa2) : itzultzaileWeb.Itzuli(labelMensaVisa3) : itzultzaileWeb.Itzuli(labelTitleModalJustificar) : itzultzaileWeb.Itzuli(labelInfoJust) : itzultzaileWeb.Itzuli(btnSaveModalJust)
            itzultzaileWeb.Itzuli(labelInfoEstados) : itzultzaileWeb.Itzuli(lnkVerEstados2) : itzultzaileWeb.Itzuli(labelInfo3) : itzultzaileWeb.Itzuli(labelAntDevuelto) : itzultzaileWeb.Itzuli(labelMensaVisa4)
            itzultzaileWeb.Itzuli(labelImpAbonado) : itzultzaileWeb.Itzuli(labelDivCabInfo) : itzultzaileWeb.Itzuli(labelFechas2) : itzultzaileWeb.Itzuli(labelUsuario)
            itzultzaileWeb.Itzuli(labelTitleModalEstados) : itzultzaileWeb.Itzuli(labelCancelarModalEstados) : itzultzaileWeb.Itzuli(labelInfo31) : itzultzaileWeb.Itzuli(labelInfo32)
            itzultzaileWeb.Itzuli(labelInfo33) : itzultzaileWeb.Itzuli(labelConceptoSinReciboMet) : itzultzaileWeb.Itzuli(txtConceptoSinReciboMet) : itzultzaileWeb.Itzuli(rfvConceptoSinReciboMet)
            itzultzaileWeb.Itzuli(labelMovAjuste) : itzultzaileWeb.Itzuli(btnAddLineaKm) : itzultzaileWeb.Itzuli(labelInfo4)
        End If
        'If (gvGastosMet.Rows.Count = 0 AndAlso gvGastosKM.Rows.Count = 0) Then 'La primera vez se tiene que forzar un postback para que actualice el idHoja 
        '    upMetalico.Triggers.Add(New PostBackTrigger With {.ControlID = btnAddLineaMetalico.ID})
        '    upKilometraje.Triggers.Add(New PostBackTrigger With {.ControlID = btnAddLineaKm.ID})
        'Else
        '    upMetalico.Triggers.Clear() : upKilometraje.Triggers.Clear() : upModales.Triggers.Clear()
        'End If
    End Sub

#End Region

#Region "Mostrar detalle"

    ''' <summary>
    ''' Muestra el detalle
    ''' El origen es para saber de donde viene la pagina. Si no trae nada, se tratara normalmente.    
    ''' Si trae ant, vendra de anticipos y tendra que mostrar solo las hojas de gastos validas    
    ''' </summary>        
    ''' <param name="bCambioUser">Es para saber si se ha cambiado de usuario o no. Es porque cuando se quiere seleccionar un usuario, no sabiamos antes si venia del cambio de un usuario o porque acababa de entrar</param>
    Private Sub mostrarDetalle(Optional ByVal bCambioUser As Boolean = False)
        Try
            Dim viajesBLL As New BLL.ViajesBLL
            Dim lLineasMet As New List(Of ELL.HojaGastos.Linea)
            Dim lLineasVisa As New List(Of ELL.Visa.Movimiento)
            Dim lLineasKM As New List(Of ELL.HojaGastos.Linea)
            Dim organizador As Boolean = False
            Dim bExisteAnticipo As Boolean = False
            Dim bInformadoViaje As Boolean = (IdViaje <> Integer.MinValue)  'Si se ha informado el id, el idviaje no estara informado aunque despues se calculara. En estos casos, el desplegable, solo mostrara el nombre de la persona y no el de todos como cuando se informa el idViaje
            Dim totalMet, totalVisa, totalKm, totalTransferencia, totalLiquidador, totalDevuelto, totalMovAjuste As Decimal
            Dim idUserInfo, idLiquidador As Integer
            Dim myHoja As ELL.HojaGastos = Nothing
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim actividadExenta As Boolean = False
            Dim importeLiquidacion As Decimal = 0
            idUserInfo = Integer.MinValue : idLiquidador = Integer.MinValue
            inicializar()
            'En los accesos a traves del email, se suele pasar el id de la hoja. Pero pasaba que no se mostraba lo correspondiente a un viaje como los anticipos porque no entraba por el if del id viaje
            'De esta forma, se consulta la hoja y si existe y tiene idViaje, entrara por este punto
            If (IdHoja <> Integer.MinValue) Then
                myHoja = hojasBLL.loadHoja(IdHoja)
                If (myHoja IsNot Nothing) Then
                    If (myHoja.IdViaje <> Integer.MinValue) Then IdViaje = myHoja.IdViaje
                    If (hojasBLL.loadStates(myHoja.Id).Exists(Function(o) o(0) = ELL.HojaGastos.eEstado.Liquidada)) Then
                        importeLiquidacion = hojasBLL.loadImporteLiquidacion(myHoja.Id)
                    End If
                End If
            End If
            Dim oViaje As ELL.Viaje = Nothing
            Dim lHojas As New List(Of ELL.HojaGastos)
            If (IdViaje <> Integer.MinValue) Then
                oViaje = viajesBLL.loadInfo(IdViaje)
                organizador = (oViaje.IdUserSolicitador = Master.Ticket.IdUser)
                bExisteAnticipo = ((oViaje.Anticipo IsNot Nothing AndAlso oViaje.Anticipo.Transferencias IsNot Nothing) OrElse (oViaje.Anticipo IsNot Nothing AndAlso (oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.Entregado Or oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.cerrado Or oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.cancelada)))
                'Para ser si es organizador a parte de lo anterior, habra que mirar si alguno de los integrantes del viaje, tiene a validador al usuario
                organizador = organizador OrElse (oViaje.ListaIntegrantes.Exists(Function(o As ELL.Viaje.Integrante) bidaiakBLL.GetResponsable(o.Usuario.IdDepartamento, o.Usuario.IdResponsable, Master.IdPlantaGestion) = Master.Ticket.IdUser))
                If (Not organizador And Not hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) Then 'Entra un integrante de un viaje a meter sus gastos
                    idUserInfo = Master.Ticket.IdUser
                ElseIf (hasProfile(BLL.BidaiakBLL.Profiles.Financiero) AndAlso OrigenHG_Financiero) Then 'Entra como financiero pero para rellenar su hoja
                    idUserInfo = Master.Ticket.IdUser
                ElseIf (Not organizador) Then  'Es financiero desde las HG de financiero
                    If (myHoja IsNot Nothing) Then
                        idUserInfo = myHoja.Usuario.Id
                    Else
                        idUserInfo = Integer.MinValue
                    End If
                Else  'Entra el organizador del viaje
                    If (ddlUsuarios.Items.Count > 0) Then
                        idUserInfo = ddlUsuarios.SelectedValue  'Seleccionado
                    Else
                        idUserInfo = Integer.MinValue  'Todos
                    End If
                End If
                '****Antes estaba aqui el codigo de comprobacion del integrante y la actividad exenta
                pnlInfoViaje.Visible = True
                lblDestino.Text = oViaje.Destino
                lblIdViaje.Text = "V" & oViaje.IdViaje
                lblFechaDesde.Text = oViaje.FechaIda.ToShortDateString
                lblFechaHasta.Text = oViaje.FechaVuelta.ToShortDateString
                If (oViaje.Anticipo IsNot Nothing AndAlso oViaje.ResponsableLiquidacion IsNot Nothing) Then idLiquidador = oViaje.ResponsableLiquidacion.Id
                'Se obtienen las hojas de gastos
                '16/07: Cuando uno de financiero veia la HG de un usuario de un viaje que a su vez fuera validador de una persona que tambien iba en ese viaje, veia los movimientos juntos
                'Como no se si al poner true en el parametro bSoloHojaDeUsuario puede influir en otros perfiles, por si acaso , se cambia solo para el de financiero
                Dim bSoloHojaDeUsuario As Boolean = False
                If (hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) Then bSoloHojaDeUsuario = True
                lHojas = hojasBLL.loadHojas(Master.IdPlantaGestion, idUserInfo, IdViaje, Date.MinValue, Date.MinValue, False, True, bSoloHojaDeUsuario)
                'Si estan asociadas a un viaje y es el organizador,nos quedamos solo con las que ya estan enviadas
                If (organizador OrElse (hasProfile(BLL.BidaiakBLL.Profiles.Financiero) AndAlso OrigenHG_Financiero)) Then
                    lHojas = lHojas.FindAll(Function(o As ELL.HojaGastos) o.Usuario.Id = Master.Ticket.IdUser OrElse (o.Validador.Id = Master.Ticket.IdUser AndAlso o.Estado <> ELL.HojaGastos.eEstado.Rellenada))
                    'ElseIf (hasProfile(BLL.BidaiakBLL.Profiles.Financiero) AndAlso Not OrigenHG_Financiero) Then 'Solo nos quedamos con las validadas
                    '    lHojas = lHojas.FindAll(Function(o As ELL.HojaGastos) o.Estado = ELL.HojaGastos.eEstado.Validada OrElse o.Estado = ELL.HojaGastos.eEstado.Liquidada)
                End If
                Dim bExisteComoIntegrante As Boolean = oViaje.ListaIntegrantes.Exists(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = Master.Ticket.IdUser)
                If (organizador) Then
                    cargarUsuarios(lHojas, bExisteComoIntegrante, bInformadoViaje, organizador)
                    If (bExisteComoIntegrante And Not Page.IsPostBack) Then  'Si es organizador, existe como integrante y es la primera vez que se carga la pagina, se mostrara la hoja del organizador
                        lHojas = lHojas.FindAll(Function(o As ELL.HojaGastos) o.Usuario.Id = Master.Ticket.IdUser)
                        ddlUsuarios.SelectedValue = Master.Ticket.IdUser
                        idUserInfo = Master.Ticket.IdUser
                        If (lHojas.Count > 0) Then IdHoja = lHojas.First.Id
                    Else  'Es la segunda vez o no es integrante
                        If (ddlUsuarios.SelectedValue <> Integer.MinValue) Then
                            lHojas = lHojas.FindAll(Function(o As ELL.HojaGastos) o.Usuario.Id = ddlUsuarios.SelectedValue)
                            idUserInfo = ddlUsuarios.SelectedValue
                            If (lHojas.Count > 0) Then IdHoja = lHojas.First.Id
                        Else
                            idUserInfo = Integer.MinValue
                            IdHoja = Integer.MinValue
                        End If
                    End If
                ElseIf ((lHojas.Count > 0 AndAlso Not hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) OrElse (hasProfile(BLL.BidaiakBLL.Profiles.Financiero) AndAlso OrigenHG_Financiero)) Then
                    'Si no es organizador y no es financiero o es financiero pero entrando a rellenar su hoja, el usuario tendra una hoja en caso de que la haya rellenado
                    '01/04/14: Me he encontrado con un caso de un financiero queria meter su hoja de gastos pero no tenia hojas de gastos y fallaba al acceder a la hoja 1
                    If (lHojas.Count > 0) Then IdHoja = lHojas.First.Id
                End If
                Dim oInteg As ELL.Viaje.Integrante = oViaje.ListaIntegrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = idUserInfo)
                If (oInteg IsNot Nothing) Then
                    Dim activBLL As New BLL.ActividadesBLL
                    Dim oActiv As ELL.Actividad = activBLL.loadInfo(oInteg.IdActividad)
                    actividadExenta = (oActiv IsNot Nothing AndAlso oActiv.ExentaIRPF AndAlso oViaje.Nivel <> ELL.Viaje.eNivel.Nacional)
                    lblFechaDesde.Text = oInteg.FechaIda.ToShortDateString
                    lblFechaHasta.Text = oInteg.FechaVuelta.ToShortDateString
                End If
                'Si no se ha encontrado la hoja y se ha informado en la url la hoja
                If (IdHoja = Integer.MinValue AndAlso IdUser <> Integer.MinValue And Not bCambioUser) Then 'Si se ha seleccionado TODOS, no se debera reasignar el usuario
                    lHojas = lHojas.FindAll(Function(o As ELL.HojaGastos) o.Usuario.Id = IdUser)
                    If (lHojas.Count > 0) Then
                        IdHoja = lHojas.First.Id
                        idUserInfo = IdUser
                        ddlUsuarios.SelectedValue = IdUser
                    End If
                End If
                txtFechaMet.Text = oViaje.FechaIda.ToShortDateString
                txtFechaKm.Text = oViaje.FechaIda.ToShortDateString
            Else 'Hoja sin viaje asociado. Si no tiene viaje, el IdHoja estara informado. Si es nuevo minValue pero sino, habra venido informado en la url                
                idUserInfo = Master.Ticket.IdUser
                If (IdHoja <> Integer.MinValue) Then  'Modificacion                    
                    idUserInfo = myHoja.Usuario.Id
                    Dim myList As New List(Of ELL.HojaGastos)
                    myList.Add(myHoja)
                    If (myHoja.IdViaje <> Integer.MinValue) Then
                        oViaje = viajesBLL.loadInfo(myHoja.IdViaje)
                        lblDestino.Text = oViaje.Destino & " ( V" & oViaje.IdViaje & " )"
                        lblFechaDesde.Text = oViaje.FechaIda.ToShortDateString
                        lblFechaHasta.Text = oViaje.FechaVuelta.ToShortDateString
                        'Añadido para que cuando se pase en la url el id de una hoja, si tiene un viaje asociado, se informe el idViaje y el idLiquidador
                        IdViaje = oViaje.IdViaje
                        If (oViaje.Anticipo IsNot Nothing) Then idLiquidador = oViaje.ResponsableLiquidacion.Id
                        'Puede pasar que un viaje lo solicite la de financiero. Pero realmente, esta no va a ser su validador.
                        'Asi que se coge siempre el validador de cualquier hoja. Se ha supuesto que todos los integrantes de un viaje, tienen el mismo validador
                        organizador = (Master.Ticket.IdUser = myHoja.Validador.Id)
                        pnlInfoViaje.Visible = True
                    Else
                        lblSinIdViaje.Text = "H" & myHoja.IdSinViaje
                        pnlSinIdViaje.Visible = True
                        lblFechasDesde2.Text = myHoja.FechaDesde
                        lblFechasHasta2.Text = myHoja.FechaHasta
                        Dim sablibBLL As New SabLib.BLL.UsuariosComponent
                        Dim oUser As SabLib.ELL.Usuario = sablibBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = myHoja.Usuario.Id}, False)
                        organizador = (bidaiakBLL.GetResponsable(oUser.IdDepartamento, oUser.IdResponsable, Master.IdPlantaGestion) = Master.Ticket.IdUser)  'Sera el organizador si es su responsable
                    End If
                    lHojas.Add(myHoja)
                    cargarUsuarios(myList, False, bInformadoViaje, organizador)  'Para que se cargue el usuario en el desplegable (el unico de la hoja de gastos)
                    ddlUsuarios.SelectedValue = myHoja.Usuario.Id
                    txtFechaMet.Text = myHoja.FechaDesde.ToShortDateString
                    txtFechaKm.Text = myHoja.FechaDesde.ToShortDateString
                Else
                    pnlSinIdViaje.Visible = True
                    Dim ticks As Decimal = Request.QueryString("fIni")
                    lblFechasDesde2.Text = New Date(ticks)
                    ticks = Request.QueryString("fFin")
                    lblFechasHasta2.Text = New Date(ticks)
                    txtFechaMet.Text = lblFechasDesde2.Text
                    txtFechaKm.Text = lblFechasDesde2.Text
                End If
            End If
            btnValidarHoja.CommandArgument = idUserInfo
            'Calculo de totales
            totalMet = 0 : totalVisa = 0 : totalKm = 0 : totalTransferencia = 0 : totalLiquidador = 0 : totalDevuelto = 0 : totalMovAjuste = 0
            If (lHojas.Count > 0) Then
                'Si tiene hojas de gastos, se mostraran las lineas de las hojas
                For Each oHoja As ELL.HojaGastos In lHojas
                    For Each linea As ELL.HojaGastos.Linea In oHoja.Lineas
                        If (linea.TipoGasto = ELL.HojaGastos.Linea.eTipoGasto.Metalico) Then
                            totalMet += linea.ImporteEuros
                            If (linea.Usuario.Id = idLiquidador) Then totalLiquidador += linea.ImporteEuros
                            lLineasMet.Add(linea)
                        ElseIf (linea.TipoGasto = ELL.HojaGastos.Linea.eTipoGasto.Kilometraje) Then
                            totalKm += linea.ImporteEuros
                            lLineasKM.Add(linea)
                        End If
                    Next
                    If (oHoja.MovimientosVisa IsNot Nothing AndAlso oHoja.MovimientosVisa.Count > 0) Then
                        lLineasVisa.AddRange(oHoja.MovimientosVisa)
                        totalVisa += oHoja.MovimientosVisa.Sum(Function(o As ELL.Visa.Movimiento) o.ImporteEuros)
                    End If
                Next
            Else
                'Sino tiene hojas, es una hoja de viaje y el usuario propietario tiene visa, se intentara consultar los gastos de visa
                '240912:Si no tiene viaje asociado, hasta que no se meta un gastos, no se chequearan los gastos de visa
                '300413:Se van a chequear los gastos de visa tenga o no hoja creada ya que se van a poder generar hojas de gastos sin lineas
                Dim visaBLL As New BLL.VisasBLL
                Dim mov As List(Of ELL.Visa.Movimiento) = Nothing
                If (IdViaje <> Integer.MinValue) Then
                    mov = visaBLL.loadMovimientos(Master.Ticket.IdUser, IdViaje, Master.IdPlantaGestion, DateTime.MinValue, DateTime.MinValue, (idUserInfo = Integer.MinValue))
                Else
                    mov = visaBLL.loadMovimientos(Master.Ticket.IdUser, Nothing, Master.IdPlantaGestion, CDate(lblFechasDesde2.Text), CDate(lblFechasHasta2.Text), False, idHojaLibre:=IdHoja)
                End If
                If (mov IsNot Nothing AndAlso mov.Count > 0) Then
                    lLineasVisa.AddRange(mov)
                    totalVisa += mov.Sum(Function(o As ELL.Visa.Movimiento) o.ImporteEuros)
                End If
            End If
            Dim comentario As String = String.Empty
            Dim lTransf As New List(Of String())
            If (oViaje IsNot Nothing AndAlso oViaje.Anticipo IsNot Nothing) Then
                If (oViaje.Anticipo.Devoluciones IsNot Nothing AndAlso oViaje.Anticipo.Devoluciones.Count > 0) Then
                    For Each oDev As ELL.Anticipo.Movimiento In oViaje.Anticipo.Devoluciones
                        totalDevuelto += oDev.ImporteEuros
                    Next
                End If
                If (oViaje.Anticipo.Transferencias IsNot Nothing AndAlso oViaje.Anticipo.Transferencias.Count > 0) Then
                    Dim idViajeOrigTrans, idViajeDestTrans As Integer
                    Dim sTrans As String() = Nothing
                    For Each lineaTrans As ELL.Anticipo.Movimiento In oViaje.Anticipo.Transferencias
                        idViajeOrigTrans = lineaTrans.IdViajeOrigen : idViajeDestTrans = lineaTrans.IdViajeDestino
                        If (IdViaje = lineaTrans.IdViajeOrigen) Then
                            totalTransferencia -= lineaTrans.ImporteEuros
                        Else
                            totalTransferencia += lineaTrans.ImporteEuros
                        End If
                        sTrans = lTransf.Find(Function(o As String()) CInt(o(0)) = idViajeOrigTrans And CInt(o(1)) = idViajeDestTrans)
                        If (sTrans IsNot Nothing AndAlso sTrans.Length > 0) Then
                            sTrans(2) = CDec(sTrans(2)) + lineaTrans.ImporteEuros
                        Else
                            lTransf.Add(New String() {lineaTrans.IdViajeOrigen, lineaTrans.IdViajeDestino, lineaTrans.ImporteEuros})
                        End If
                    Next
                    If (lblLabelAntRec.Text.IndexOf("3") = -1) Then lblLabelAntRec.Text = itzultzaileWeb.Itzuli("Anticipo recibido") & " (4)"
                    comentario = "4)&nbsp;" & itzultzaileWeb.Itzuli("Al existir movimientos de traspaso entre viajes, se le sumara o restara dependiendo cual sea el viaje destino")
                End If
                If (oViaje.Anticipo.Movimientos.Exists(Function(o) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Diferencia_Cambio OrElse o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Manual)) Then
                    Dim lMovAjuste As List(Of ELL.Anticipo.Movimiento) = oViaje.Anticipo.Movimientos.FindAll(Function(o) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Diferencia_Cambio OrElse o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Manual)
                    For Each mov In lMovAjuste
                        totalMovAjuste += mov.ImporteEuros 'Esto puede ser negativo o positivo
                    Next
                End If
            End If
            'Se ordena ascendente por usuario y ascendente por fecha            
            lLineasMet = (From linea As ELL.HojaGastos.Linea In lLineasMet Select linea Order By linea.Usuario.NombreCompleto Ascending, linea.Fecha Ascending, If(linea.ConceptoBatz IsNot Nothing, linea.ConceptoBatz.Nombre, linea.Concepto) Ascending).ToList
            lLineasVisa = (From linea As ELL.Visa.Movimiento In lLineasVisa Select linea Order By linea.NombreUsuario Ascending, linea.Fecha Ascending, linea.Sector Ascending).ToList
            lLineasKM = (From linea As ELL.HojaGastos.Linea In lLineasKM Select linea Order By linea.Usuario.NombreCompleto Ascending, linea.Fecha Ascending).ToList
            Dim myVisas As String = String.Empty
            If (organizador) Then
                If (IdViaje = Integer.MinValue) Then  'Es una hoja sin viaje asociado de solo una persona                   
                    myVisas = getVisasUser(idUserInfo)
                    If (myVisas <> String.Empty) Then myVisas = itzultzaileWeb.Itzuli("Su numero de visa") & ":" & myVisas
                Else
                    Dim idUserToFind As Integer = ddlUsuarios.SelectedValue  'Si se elige seleccioneUno o Todos, mostrara si alguno tiene visas. Si se selecciona uno, se mirara solo ese
                    Dim visaIntegr As String = String.Empty
                    For Each Integr As SabLib.ELL.Usuario In oViaje.getUsuariosIntegrantes
                        If (idUserToFind = Integer.MinValue OrElse idUserToFind = Integr.Id) Then
                            visaIntegr = getVisasUser(Integr.Id)
                            If (visaIntegr <> String.Empty) Then
                                If (myVisas <> String.Empty) Then myVisas &= ","
                                myVisas &= Integr.NombreCompleto & "(" & visaIntegr & ")"
                            End If
                        End If
                    Next
                End If
            Else
                If (idUserInfo <> Integer.MinValue) Then
                    myVisas = getVisasUser(idUserInfo)
                    If (myVisas <> String.Empty) Then myVisas = itzultzaileWeb.Itzuli("Su numero de visa") & ":" & myVisas
                Else
                    myVisas = " "  'Se le pone algo para que aparezca el panel
                End If
            End If
            Dim lTrayectosKm As List(Of ELL.HojaGastos.Linea) = Nothing
            If (Master.Ticket.IdUser = idUserInfo) Then  'El usuario es el propietario de l ahoja
                lTrayectosKm = hojasBLL.loadTrayectosKilometraje(Master.Ticket.IdUser, IdHoja)
                If (lTrayectosKm IsNot Nothing) Then lTrayectosKm = lTrayectosKm.OrderBy(Of String)(Function(o) o.LugarOrigen.ToUpper).ToList
            End If
            pnlTrayectos.Visible = (lTrayectosKm IsNot Nothing AndAlso lTrayectosKm.Count > 0)
            gvTrayectosKM.DataSource = lTrayectosKm
            gvTrayectosKM.DataBind()
            'Visas
            '************************
            If (myVisas <> String.Empty) Then
                pnlDivVisas.Visible = True : pnlDescripVisas.Visible = True : divGastosVisaCarga.Visible = True
                lblDescripVisas.Text = myVisas
                'Se mira a ver si se ha cargado el fichero de visas del mes
                Dim visasBLL As New BLL.VisasBLL
                Dim fVuelta As Date = If(lblFechaHasta.Text <> String.Empty, CDate(lblFechaHasta.Text), CDate(lblFechasHasta2.Text))
                Dim bFicheroVisasCargado As Boolean = visasBLL.FicheroVisasCargado(fVuelta.Month, fVuelta.Year, Master.IdPlantaGestion)
                If (bFicheroVisasCargado) Then
                    If Not (lLineasVisa IsNot Nothing AndAlso lLineasVisa.Count > 0) Then
                        labelMensaVisa.Text = itzultzaileWeb.Itzuli("El fichero de visas ya se ha cargado pero no se ha encontrado ningun gasto")
                        divGastosVisaCarga.Attributes.Add("class", "alert alert-success")
                    Else 'Hay movimientos
                        divGastosVisaCarga.Visible = False
                        divGastosVisaSinCom.Visible = (lLineasVisa.Exists(Function(o As ELL.Visa.Movimiento) o.Estado = ELL.Visa.Movimiento.eEstado.Cargado))
                    End If
                Else
                    labelMensaVisa.Text = itzultzaileWeb.Itzuli("Los gastos de Visa se cargan automaticamente a partir del extracto de Caja Laboral")
                    divGastosVisaCarga.Attributes.Add("class", "alert alert-info")
                    If Not (lLineasMet IsNot Nothing AndAlso lLineasMet.Count > 0 AndAlso lLineasKM IsNot Nothing AndAlso lLineasKM.Count > 0) Then 'Solo se muestra este div si no hay ningun movimiento en metalico ni km
                        divSoloGastosVisa.Visible = True
                    End If
                End If
            End If
            gvGastosV.DataSource = lLineasVisa
            gvGastosV.DataBind()
            '************************
            'Documentos
            '************************
            If (Not Page.IsPostBack) Then  'Si se crea una linea luego da error porque no se puede actualizar
                pnlDivDocumentos.Visible = (IdHoja <> Integer.MinValue AndAlso actividadExenta)
                If (IdHoja <> Integer.MinValue AndAlso actividadExenta) Then
                    Dim lDocs As List(Of ELL.Viaje.DocumentoIntegrante) = viajesBLL.loadDocumentosIntegrante(IdViaje, idUserInfo) 'Antes master.ticket.iduser
                    If (lDocs IsNot Nothing) Then
                        lDocs = lDocs.OrderBy(Of String)(Function(o) o.Titulo).ToList
                        divDocsNoSubidos.Visible = lDocs.Count = 0
                    Else
                        divDocsNoSubidos.Visible = True
                    End If
                    rptDocumentos.DataSource = lDocs
                    rptDocumentos.DataBind()
                End If
            End If
            '************************
            gvGastosMet.DataSource = lLineasMet
            gvGastosMet.DataBind()
            If (gvGastosMet.FooterRow IsNot Nothing) Then
                Dim lblTotalMet As Label = CType(gvGastosMet.FooterRow.Cells(5).FindControl("lblImporteTotalMet"), Label)
                lblTotalMet.Text = totalMet & " EUR"
            End If
            'Se comprueba cual es la ultima fecha de validacion            
            Dim lastHGStateDate As DateTime = DateTime.MinValue
            hImportaciones = New Hashtable 'Se inicializa ya que se va a usar en gv.Databind
            If (IdHoja > 0) Then
                Dim myHojaEst As New ELL.HojaGastos
                myHojaEst.Estados = hojasBLL.loadStates(IdHoja)
                lastHGStateDate = myHojaEst.GetFechaEstado(ELL.HojaGastos.eEstado.Validada) 'Se inicializa ya que se va a usar en gv.Databind
            End If
            hfLastHGStateDate.Value = If(lastHGStateDate <> DateTime.MinValue, lastHGStateDate.ToShortDateString, String.Empty)
            If (gvGastosV.FooterRow IsNot Nothing) Then
                Dim lblTotalVisa As Label = CType(gvGastosV.FooterRow.Cells(5).FindControl("lblImporteTotalVisa"), Label)
                lblTotalVisa.Text = totalVisa & " EUR"
            End If
            If Not (divGastosVisaDespues.Visible) Then gvGastosV.Columns(0).Visible = False
            Dim params As ELL.Parametro = getParametrosKm()
            precioKm = params.PrecioKm  'Se deja en esta variable global ya que en el databind de gvGastosKm, se va a utilizar
            gvGastosKM.DataSource = lLineasKM
            gvGastosKM.DataBind()
            If (gvGastosKM.FooterRow IsNot Nothing) Then
                Dim lblTotalKm As Label = CType(gvGastosKM.FooterRow.Cells(7).FindControl("lblImporteTotalKM"), Label)
                lblTotalKm.Text = totalKm & " EUR"
            End If
            'Panel resumen
            If (IdViaje <> Integer.MinValue) Then
                'If (oViaje.Anticipo IsNot Nothing AndAlso (oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.Entregado Or oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.cerrado) AndAlso (idUserInfo = idLiquidador OrElse (organizador AndAlso ddlUsuarios.SelectedValue = Integer.MinValue) OrElse Master.Perfil = BLL.IBidaiak.Profiles.Financiero OrElse Master.Perfil = BLL.IBidaiak.Profiles.Administrador)) Then
                If ((oViaje.Anticipo IsNot Nothing AndAlso oViaje.Anticipo.Transferencias IsNot Nothing) OrElse (oViaje.Anticipo IsNot Nothing AndAlso (oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.Entregado Or oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.cerrado Or oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.cancelada) AndAlso (idUserInfo = idLiquidador OrElse (organizador AndAlso ddlUsuarios.SelectedValue = Integer.MinValue)))) Then
                    If (idUserInfo = idLiquidador) Then
                        lblTotalUsuario.Text = itzultzaileWeb.Itzuli("El total de gastos del viaje de esta persona asciende a") & " " & (totalMet + totalKm + totalVisa) & " €"
                    Else
                        lblTotalUsuario.Text = itzultzaileWeb.Itzuli("El total de gastos del viaje de todos los integrantes asciende a") & " " & (totalMet + totalKm + totalVisa) & " €"
                    End If
                    totalTransferencia = Math.Round(totalTransferencia, 2)
                    totalLiquidador = Math.Round(totalLiquidador, 2)
                    'Solo son cantidades del liquidador
                    Dim diferencia As Decimal
                    lblAnticipo.Text = Math.Round(oViaje.Anticipo.EurosEntregados, 2)
                    divAntEntregado.Visible = False
                    If (totalDevuelto > 0) Then
                        divAntEntregado.Visible = True
                        lblAntDevuelto.Text = Math.Round(totalDevuelto, 2)
                    End If
                    If (lTransf.Count > 0) Then
                        rptTransf.DataSource = lTransf
                        rptTransf.DataBind()
                    End If
                    'Se miran los movimientos manuales
                    divMovAjuste.Visible = False
                    If (totalMovAjuste <> 0) Then
                        divMovAjuste.Visible = True
                        lblMovAjuste.Text = Math.Round(totalMovAjuste, 2)
                    End If
                    If (importeLiquidacion > 0 And totalKm = 0) Then  'Si hay km es muy complicado ya que puede deber dinero del anticipo pero habersele pagado el km
                        divImpAbonado.Visible = True
                        lblImpAbonado.Text = Math.Round(importeLiquidacion, 2)
                    End If
                    lblTotalGastos.Text = totalLiquidador  'Solo cuentan los gastos con/sin recibo                    
                    diferencia = Math.Round(oViaje.Anticipo.EurosEntregados, 2) + totalTransferencia + totalMovAjuste - totalLiquidador - totalDevuelto
                    If (totalKm = 0) Then diferencia += importeLiquidacion
                    lblDiferencia.Text = diferencia
                    If (diferencia > 0) Then
                        lblDiferenciaTexto.Text = "<span class='text-danger' style='font-size:16px'>" & itzultzaileWeb.Itzuli("El trabajador debe devolver") & " " & Math.Abs(diferencia) & " €" & "[NOTA]</span>"
                        lblDiferencia.CssClass = "text-danger"
                        If (totalKm) Then
                            lblDiferenciaTexto.Text &= "<br /><span class='text-success' style='font-size:16px'>" & itzultzaileWeb.Itzuli("Total a recibir por el trabajador por kilometraje") & " " & totalKm & " €</span>"
                        End If
                        'Si le sale a devolver, tiene anticipo y el anticipo o la HG tiene distintas monedas o solo una pero no es el euro, saldra una nota explicando que el resultado en euros es aproximado                        
                        Dim bNotaLiq As Boolean = False
                        'Puede que no tenga anticipo pero si una transferencia
                        If (oViaje.Anticipo.AnticiposSolicitados.Count > 0) Then
                            bNotaLiq = True
                        ElseIf (oViaje.Anticipo.Transferencias.Count > 0) Then
                            bNotaLiq = True
                        End If
                        Dim numNota As String = "4"
                        If (Not bNotaLiq) Then
                            'Se busca si la moneda del anticipo es el Euro. Si es distinta, se mostrara la nota
                            If (oViaje.Anticipo.AnticiposSolicitados.Count > 0) Then
                                bNotaLiq = (oViaje.Anticipo.AnticiposSolicitados.First.Moneda.Abreviatura.ToLower <> "eur")
                            ElseIf (oViaje.Anticipo.Transferencias.Count > 0) Then
                                bNotaLiq = (oViaje.Anticipo.Transferencias.First.Moneda.Abreviatura.ToLower <> "eur")
                            Else
                                bNotaLiq = False
                            End If
                            If (Not bNotaLiq) Then
                                'Se busca en la HG si hay mas de una moneda. Si solo hay una, se mirara si es en euros
                                If (IdHoja <> Integer.MinValue) Then
                                    Dim myHojaG As ELL.HojaGastos = lHojas.Find(Function(o As ELL.HojaGastos) o.Id = IdHoja)
                                    Dim monedasDistintasAntic = From moneda In myHojaG.Lineas Select moneda.Moneda.Id Distinct

                                    If (monedasDistintasAntic IsNot Nothing AndAlso monedasDistintasAntic.Count > 1) Then
                                        bNotaLiq = True
                                    ElseIf (monedasDistintasAntic IsNot Nothing AndAlso monedasDistintasAntic.Count = 1) Then
                                        bNotaLiq = (monedasDistintasAntic.First <> 90) 'Id del euro
                                    End If
                                End If
                            End If
                        End If
                        If (bNotaLiq) Then
                            If (comentario <> String.Empty) Then
                                numNota = "5" 'Si esta visible es porque se ha metido una nota antes
                                comentario &= "<br />"
                            End If
                            comentario &= numNota & ")&nbsp;" & itzultzaileWeb.Itzuli("El resultado de la liquidacion siempre se aproxima a euros. Por tanto, podra haber diferencias entre el resultado de la liquidacion y la cantidad en metalico a devolver")
                            lblDiferenciaTexto.Text = lblDiferenciaTexto.Text.Replace("[NOTA]", " <span>(" & numNota & ")</span>")
                        Else
                            lblDiferenciaTexto.Text = lblDiferenciaTexto.Text.Replace("[NOTA]", String.Empty)
                        End If
                        lblDiferenciaTexto.Text &= "<br /><span class='text-danger'>" & itzultzaileWeb.Itzuli("El anticipo se debera entregar en mano en administracion").ToString.ToUpper & "</span>"
                    ElseIf (diferencia = 0) Then
                        If (totalKm > 0) Then
                            lblDiferenciaTexto.Text = "<span class='text-success' style='font-size:16px'>" & itzultzaileWeb.Itzuli("Total a recibir por el trabajador") & " " & totalKm & " €</span>"
                            lblDiferencia.CssClass = "text-success"
                        Else
                            lblDiferenciaTexto.Text = "<span class='text-primary'>" & itzultzaileWeb.Itzuli("Hoja de gastos liquidada") & "</span>"
                            lblDiferencia.CssClass = "text-primary"
                        End If
                    Else
                        If (totalKm > 0) Then diferencia = Math.Abs(diferencia) + totalKm
                        lblDiferenciaTexto.Text = "<span class='text-success' style='font-size:16px'>" & itzultzaileWeb.Itzuli("Total a recibir por el trabajador") & " " & Math.Abs(diferencia) & " €</span>"
                        lblDiferencia.CssClass = "text-success"
                    End If
                Else 'Sin anticipo, habrá que pagarle todo
                    If (idUserInfo <> Integer.MinValue) Then
                        lblTotalUsuario.Text = itzultzaileWeb.Itzuli("El total de gastos del viaje de esta persona asciende a") & " " & (totalMet + totalKm + totalVisa) & " €"
                    Else
                        lblTotalUsuario.Text = itzultzaileWeb.Itzuli("El total de gastos del viaje asciende a") & " " & (totalMet + totalKm + totalVisa) & " €"
                    End If
                    'Si no es el liquidador o si es el organizador y ha seleccionado algun usuario                    
                    If (importeLiquidacion > 0) Then
                        lblResumenUser.Text = itzultzaileWeb.Itzuli("Hoja de gastos liquidada")
                        lblResumenUser.CssClass = "text-primary"
                    Else
                        lblResumenUser.Text = itzultzaileWeb.Itzuli("Total a recibir por el trabajador") & " " & (totalMet + totalKm) & " €"
                        lblResumenUser.CssClass = "text-success"
                    End If
                End If
            Else 'Se consulta por una hoja de gastos sin viaje asociado                
                lblTotalUsuario.Text = itzultzaileWeb.Itzuli("El total de gastos de esta persona asciende a") & " " & (totalMet + totalKm + totalVisa) & " €"
                'lblResumenUser.Text = itzultzaileWeb.Itzuli("Total a recibir por el trabajador") & " " & (totalMet + totalKm) & " €"
                If (importeLiquidacion > 0) Then
                    lblResumenUser.Text = itzultzaileWeb.Itzuli("Hoja de gastos liquidada")
                    lblResumenUser.CssClass = "text-primary"
                Else
                    lblResumenUser.Text = itzultzaileWeb.Itzuli("Total a recibir por el trabajador") & " " & (totalMet + totalKm) & " €"
                    lblResumenUser.CssClass = "text-success"
                End If
            End If
            'If (oViaje IsNot Nothing AndAlso oViaje.Anticipo IsNot Nothing AndAlso (oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.Entregado Or oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.cerrado Or oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.cancelada)) Then hlVerAnticipo.NavigateUrl = "~/Financiero/Anticipos/GestionAnticipos.aspx?idViaje=" & oViaje.Anticipo.IdViaje
            lblComentario.Text = comentario
            lblComentario.Visible = (comentario <> String.Empty)
            'If (lLineasMet.Count = 0 AndAlso lLineasKM.Count = 0) Then 'La primera vez se tiene que forzar un postback para que actualice el idHoja 
            '    upMetalico.Triggers.Add(New PostBackTrigger With {.ControlID = btnAddLineaMetalico.ID})
            '    upKilometraje.Triggers.Add(New PostBackTrigger With {.ControlID = btnAddLineaKm.ID})
            'Else
            '    upMetalico.Triggers.Clear() : upKilometraje.Triggers.Clear() : upModales.Triggers.Clear()
            'End If
            gestionPaneles(lHojas, organizador, Master.Ticket.IdUser, idUserInfo, idLiquidador, bExisteAnticipo)
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar el detalle", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene las visas de un usuario y lo devuelve en un string
    ''' </summary>
    ''' <param name="idUser">Id usuario</param>
    ''' <returns></returns>    
    Private Function getVisasUser(ByVal idUser As Integer) As String
        Dim visasBLL As New BLL.VisasBLL
        Dim lVisas As List(Of ELL.Visa)
        Dim myVisas As String = String.Empty
        lVisas = visasBLL.loadList(New ELL.Visa With {.Propietario = New SabLib.ELL.Usuario With {.Id = idUser}}, Master.IdPlantaGestion, False)
        If (lVisas IsNot Nothing AndAlso lVisas.Count > 0) Then
            For Each myVisa As ELL.Visa In lVisas
                If (myVisas <> String.Empty) Then myVisas &= ","
                myVisas &= myVisa.NumTarjeta
            Next
        End If
        Return myVisas
    End Function

    ''' <summary>
    ''' Cuando se selecciona otro usuario, se muestra el detalle
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlUsuarios_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlUsuarios.SelectedIndexChanged
        Try
            mostrarDetalle(True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el historico de estados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkVerEstados_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkVerEstados1.Click, lnkVerEstados2.Click
        Try
            Dim hojasBLL As New BLL.HojasGastosBLL
            Dim lEstados As List(Of String()) = hojasBLL.loadStates(IdHoja)
            lEstados = lEstados.OrderBy(Of Date)(Function(o) CDate(o(1))).ToList
            gvEstados.DataSource = lEstados
            gvEstados.DataBind()
            ShowModal(True, "divModalEstados")
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los estados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvEstados_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvEstados.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim estado As String() = DirectCast(e.Row.DataItem, String())
            Dim fecha As Date = CType(estado(1), Date)
            DirectCast(e.Row.FindControl("lblEstado"), Label).Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.HojaGastos.eEstado), CInt(estado(0))))
            DirectCast(e.Row.FindControl("lblFecha"), Label).Text = fecha.ToShortDateString & " " & fecha.ToShortTimeString
        End If
    End Sub

#End Region

#Region "Inicializaciones y gestion de permisos"

    ''' <summary>
    ''' Inicializa los controles
    ''' </summary>    
    Private Sub inicializar()
        lblEstado.Text = itzultzaileWeb.Itzuli("La hoja de gastos todavia no ha sido rellenada/enviada")
        divEstado.Attributes.Add("class", "alert alert-warning")
        cargarMonedas()
        txtFechaMet.Text = Now.ToShortDateString : txtConceptoConReciboMet.Text = String.Empty : txtImporteMet.Text = String.Empty : txtDocTitulo.Text = String.Empty : txtConceptoSinReciboMet.Text = String.Empty
        txtConceptoConReciboMet.Style("display") = "none" : txtConceptoSinReciboMet.Style("display") = "none"
        Dim escribaComentario As String = itzultzaileWeb.Itzuli("Escriba un comentario")
        txtConceptoConReciboMet.Attributes.Add("placeholder", escribaComentario)
        txtConceptoSinReciboMet.Attributes.Add("placeholder", escribaComentario)
        txtFechaKm.Text = Now.ToShortDateString : txtOrigenKm.Text = String.Empty : txtDestinoKm.Text = String.Empty : txtKm.Text = String.Empty
        lblAnticipo.Text = String.Empty : lblDiferencia.Text = String.Empty : lblTotalGastos.Text = String.Empty
        lblDiferenciaTexto.Text = String.Empty : lblTotalUsuario.Text = String.Empty : lblDestino.Text = String.Empty
        lblFechaDesde.Text = String.Empty : lblFechaHasta.Text = String.Empty : lblNombreUsuario.Text = String.Empty
        lblResumenUser.Text = String.Empty : lblSinIdViaje.Text = String.Empty : lblDescripVisas.Text = String.Empty
        lblFechasDesde2.Text = String.Empty : lblFechasHasta2.Text = String.Empty : hfGastosVSinJustif.Value = String.Empty
        lnkVerEstados1.Visible = False : lnkVerEstados2.Visible = False : hfLastHGStateDate.Value = String.Empty
        'Por defecto, ningun panel sera visible
        pnlEstado.Visible = False : pnlCambioEstado.Visible = False : pnlMultiUser.Visible = False
        pnlFiltroMet.Visible = False : pnlFiltroKm.Visible = False : pnlTrayectos.Visible = False : pnlDivVisas.Visible = False
        divGastosVisaCarga.Visible = True : divSoloGastosVisa.Visible = False : divGastosVisaSinCom.Visible = False : divGastosVisaDespues.Visible = False
        labelMensaVisa.Text = String.Empty : divGastosVisaCarga.Attributes.Remove("class") : divDiferencia.Attributes.Remove("class")
        pnlDivDocumentos.Visible = False : pnlTotalTodos.Visible = False
        pnlInfoUsuario.Visible = False : pnlInfoViaje.Visible = False : pnlSinIdViaje.Visible = False
        pnlDescripVisas.Visible = False : pnlInfoUploadDoc.Visible = False : lblIdViaje.Text = String.Empty : lblSinIdViaje.Text = itzultzaileWeb.Itzuli("Sin crear")
        cargarDesplegablesConceptos()
        ddlConceptosConReciboMet.SelectedIndex = 0 : ddlConceptosSinReciboMet.SelectedIndex = 0
        rptDocumentos.DataSource = Nothing : rptDocumentos.DataBind() : divDocsNoSubidos.Visible = False
        btnImprimir.Visible = False : btnEliminar.Visible = False : btnEnviar.Visible = False
        lblComentario.Visible = False
        btnValidarHoja.Visible = True : btnRechazarHoja.Visible = False
        btnRechazarHoja.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmacion de rechazo"), itzultzaileWeb.Itzuli("¿Esta seguro de que desea rechazar la hoja de gastos? Si continua, la persona podra volver a editarla"), String.Empty, "RejHG")
        btnValidarHoja.CommandArgument = String.Empty
        divImpAbonado.Visible = False
    End Sub

    ''' <summary>
    ''' Obtiene los parametros del kilometraje
    ''' </summary>    
    Private Function getParametrosKm() As ELL.Parametro
        Dim bidaiakBLL As New BLL.BidaiakBLL
        Dim params As ELL.Parametro = bidaiakBLL.loadParameters(Master.IdPlantaGestion)
        params.IdConceptoKm = If(params.IdConceptoKm > 0, params.IdConceptoKm, 0)
        Return params
    End Function

    ''' <summary>
    ''' Carga los desplegables de conceptos de gastos en metalico y visas con los conceptos de Batz
    ''' </summary>    
    Private Sub cargarDesplegablesConceptos()
        If (ddlConceptosConReciboMet.Items.Count = 0) Then
            Try
                Dim conceptosBLL As New BLL.ConceptosBLL
                Dim lConceptos As List(Of ELL.Concepto) = conceptosBLL.loadList(Master.IdPlantaGestion, True)
                lConceptos = lConceptos.OrderBy(Of String)(Function(o) o.Nombre).ToList
                Dim lConceptosAux As List(Of ELL.Concepto) = lConceptos.FindAll(Function(f) f.MostrarHojaGastosRecibo)
                ddlConceptosConReciboMet.Attributes.Add("onChange", "ChequearSeleccionadoConRecibo();")
                ddlConceptosConReciboMet.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
                For Each oConcept As ELL.Concepto In lConceptosAux
                    ddlConceptosConReciboMet.Items.Add(New ListItem(oConcept.Nombre, oConcept.Id & "|" & oConcept.RequiereDetalle.ToString))
                Next
                lConceptosAux = lConceptos.FindAll(Function(f) f.MostrarHojaGastosSinRecibo)
                ddlConceptosSinReciboMet.Attributes.Add("onChange", "ChequearSeleccionadoSinRecibo();")
                ddlConceptosSinReciboMet.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
                For Each oConcept As ELL.Concepto In lConceptosAux
                    ddlConceptosSinReciboMet.Items.Add(New ListItem(oConcept.Nombre, oConcept.Id & "|" & oConcept.RequiereDetalle.ToString))
                Next
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al cargar los desplegables de conceptos", ex)
            End Try
        End If
        ddlConceptosConReciboMet.SelectedIndex = 0 : ddlConceptosSinReciboMet.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Carga los usuarios de la hojas de gastos.
    ''' Si el organizador tambien es integrante y todavia no la ha rellenado, se le añade
    ''' </summary>
    ''' <param name="lHojas">Hojas del viaje</param>
    ''' <param name="bEsIntegrante">Indica si el organizador, tambien es integrante del viaje</param>
    ''' <param name="bInformadoViaje">Si se habia informado el viaje desde la url, se meteran todos los integrantes con hojas. Sino, de momento no se mete ninguno</param>
    ''' <param name="bOrganizador">Si es organizador, solo se mostraran a sus pupilos</param>
    Private Sub cargarUsuarios(ByVal lHojas As List(Of ELL.HojaGastos), ByVal bEsIntegrante As Boolean, ByVal bInformadoViaje As Boolean, ByVal bOrganizador As Boolean)
        'Obtenemos los integrantes con hoja de gastos para un viaje
        Dim hojaBLL As New BLL.HojasGastosBLL
        Dim lUsuarios As List(Of SabLib.ELL.Usuario) = Nothing
        Dim idOrganiz As Integer = Integer.MinValue
        If (bOrganizador) Then idOrganiz = Master.Ticket.IdUser
        lUsuarios = hojaBLL.getIntegrantesConHojaGastos(IdViaje, 3, idOrganiz)
        Dim selectedValue As Integer = Integer.MinValue
        If (ddlUsuarios.Items.Count > 0) Then selectedValue = ddlUsuarios.SelectedValue
        ddlUsuarios.Items.Clear()
        Dim idUser As Integer
        For Each oHoja As ELL.HojaGastos In lHojas
            idUser = oHoja.Usuario.Id
            If (Not lUsuarios.Exists(Function(o As SabLib.ELL.Usuario) o.Id = idUser)) Then
                lUsuarios.Add(oHoja.Usuario)
            End If
        Next
        If (bInformadoViaje) Then
            If (bEsIntegrante) Then
                'Se comprueba si ya esta metido el organizador. Sino estuviera significara que todavia no ha rellenado su hoja de gastos
                If (Not lUsuarios.Exists(Function(o As SabLib.ELL.Usuario) o.Id = Master.Ticket.IdUser)) Then
                    lUsuarios.Add(New SabLib.ELL.Usuario With {.Id = Master.Ticket.IdUser, .Nombre = Master.Ticket.NombrePersona, .Apellido1 = Master.Ticket.Apellido1, .Apellido2 = Master.Ticket.Apellido2})
                End If
            End If
        End If
        lUsuarios = lUsuarios.OrderBy(Of String)(Function(o) o.NombreCompleto).ToList
        If (bInformadoViaje) Then
            ddlUsuarios.Items.Add(New ListItem(itzultzaileWeb.Itzuli("todos"), Integer.MinValue))
        Else
            ddlUsuarios.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
        End If
        For Each oUser As SabLib.ELL.Usuario In lUsuarios
            If (ddlUsuarios.Items.FindByValue(oUser.Id) Is Nothing) Then ddlUsuarios.Items.Add(New ListItem(oUser.NombreCompleto, oUser.Id))
        Next
        ddlUsuarios.SelectedValue = selectedValue
    End Sub

    ''' <summary>
    ''' Muestra los botones para el cambio de estado    
    ''' </summary>
    ''' <param name="estado">Estado</param>        
    Private Sub mostrarBotonesCambioEstado(ByVal estado As ELL.HojaGastos.eEstado)
        btnValidarHoja.Visible = False : btnRechazarHoja.Visible = True
        Select Case estado
            Case ELL.HojaGastos.eEstado.Enviada
                btnValidarHoja.Visible = True
                btnRechazarHoja.Visible = True
            Case ELL.HojaGastos.eEstado.Validada
                btnRechazarHoja.Visible = True
        End Select
    End Sub

    ''' <summary>
    ''' Gestiona la visualizacion de los paneles
    ''' </summary>
    ''' <param name="lHojas">Lista de hojas de gastos</param>
    ''' <param name="bOrganizador">Indica si lo ve el organizador</param>      
    ''' <param name="idUserActual">Id del usuario actual, el que ha iniciado session</param>
    ''' <param name="idUserVerInfo">Id del usuario del que se vera informacion. Si es integer.MinValue, significara que se vera la informacion de todos</param>
    ''' <param name="idLiquidador">Indica el id del liquidador</param>      
    ''' <param name="existeAnticipo">Indica si existe un anticipo</param>
    Private Sub gestionPaneles(ByVal lHojas As List(Of ELL.HojaGastos), ByVal bOrganizador As Boolean, ByVal idUserActual As Integer, ByVal idUserVerInfo As Integer, ByVal idLiquidador As Integer, ByVal existeAnticipo As Boolean)
        Dim modificable As Boolean = False
        Dim lControles As List(Of Object) = Nothing
        Dim myHoja As ELL.HojaGastos = lHojas.Find(Function(o As ELL.HojaGastos) o.Usuario.Id = idUserVerInfo)
        pnlMultiUser.Visible = bOrganizador  'La seleccion de usuarios solo sera visible si es organizador
        labelInfo3.Visible = False
        pnlInfoUsuario.Visible = (Not bOrganizador And idUserVerInfo <> Integer.MinValue)
        If (pnlInfoUsuario.Visible) Then
            Dim sablibBLL As New SabLib.BLL.UsuariosComponent
            lblNombreUsuario.Text = sablibBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idUserVerInfo}, False).NombreCompleto
        End If
        pnlTotalTodos.Visible = existeAnticipo AndAlso (idUserVerInfo <> Integer.MinValue AndAlso (bOrganizador AndAlso ddlUsuarios.SelectedValue = Integer.MinValue) OrElse (idLiquidador = idUserVerInfo))  'Si selecciona "TODOS", no se vera la tabla de la diferencia del dinero                
        lblResumenUser.Visible = ((myHoja IsNot Nothing AndAlso (myHoja.Estado = ELL.HojaGastos.eEstado.Validada OrElse myHoja.Estado = ELL.HojaGastos.eEstado.Liquidada OrElse myHoja.Estado = ELL.HojaGastos.eEstado.Entregada_Administracion)) OrElse hasProfile(BLL.BidaiakBLL.Profiles.Financiero))  'Solo se mostrara lo que le tienen que pagar una vez este validada        
        btnVerAnticipo.Visible = (pnlTotalTodos.Visible AndAlso hasProfile(BLL.BidaiakBLL.Profiles.Financiero))
        If (pnlFiltroKm.Visible) Then pnlFiltroKm.Visible = (myHoja Is Nothing OrElse (myHoja IsNot Nothing AndAlso (myHoja.Estado = ELL.HojaGastos.eEstado.Rellenada Or myHoja.Estado = ELL.HojaGastos.eEstado.NoValidada)))
        'El estado solo se mostrara cuando se seleccione a un usuario en concreto
        If (idUserVerInfo <> Integer.MinValue) Then
            pnlEstado.Visible = True
            If (myHoja IsNot Nothing) Then
                Select Case myHoja.Estado
                    Case ELL.HojaGastos.eEstado.Rellenada
                        lblEstado.Text = itzultzaileWeb.Itzuli("La hoja de gastos todavia no ha sido rellenada/enviada")
                        divEstado.Attributes.Add("class", "alert alert-warning")
                    Case ELL.HojaGastos.eEstado.Enviada
                        lblEstado.Text = itzultzaileWeb.Itzuli("La hoja de gastos ha sido enviada al responsable. Ya no se puede modificar ningun dato")
                        divEstado.Attributes.Add("class", "alert alert-info")
                    Case ELL.HojaGastos.eEstado.Validada
                        lblEstado.Text = itzultzaileWeb.Itzuli("Validada por el responsable")
                        divEstado.Attributes.Add("class", "alert alert-success")
                        btnImprimir.Visible = True
                    Case ELL.HojaGastos.eEstado.NoValidada
                        lblEstado.Text = itzultzaileWeb.Itzuli("El responsable no ha dado el ok a la hoja de gastos. Pongase en contacto con el")
                        divEstado.Attributes.Add("class", "alert alert-danger")
                    Case ELL.HojaGastos.eEstado.Liquidada
                        lblEstado.Text = itzultzaileWeb.Itzuli("La hoja de gastos ha sido integrada en el sistema")
                        divEstado.Attributes.Add("class", "alert alert-info")
                        btnImprimir.Visible = True
                End Select
                pnlCambioEstado.Visible = ((idUserVerInfo <> idUserActual) AndAlso (myHoja.Estado = ELL.HojaGastos.eEstado.Enviada Or myHoja.Estado = ELL.HojaGastos.eEstado.Validada)) AndAlso bOrganizador
                If (pnlCambioEstado.Visible) Then mostrarBotonesCambioEstado(myHoja.Estado)
            End If
        End If
        'Los documentos solo los podran subir y eliminar el usuario
        pnlInfoUploadDoc.Visible = (idUserActual = idUserVerInfo)
        If (rptDocumentos IsNot Nothing AndAlso (idUserActual <> idUserVerInfo)) Then
            If (idUserActual <> idUserVerInfo) Then
                lControles = New List(Of Object)
                lControles.Add(New LinkButton With {.ID = "lnkEliminar"})
                DisableControlsRepeater(rptDocumentos, lControles)
            End If
        End If
        'El link de transferencias solo sera visible si es el mismo usuario, es el liquidador, hay anticipo y la hoja no esta ni validada ni enviada ni liquidada
        btnTransferir.Visible = (idUserActual = idUserVerInfo AndAlso existeAnticipo AndAlso idLiquidador = idUserVerInfo AndAlso myHoja IsNot Nothing AndAlso
                                 (myHoja.Estado = ELL.HojaGastos.eEstado.Rellenada Or myHoja.Estado = ELL.HojaGastos.eEstado.NoValidada))
        'El boton de enviar y eliminar solo sera visible si el usuario actual es el mismo que el actual y si el estado de la hoja es igual a rellenado o no validada                
        If (idUserActual = idUserVerInfo) Then
            'Si no ha rellenado todavia la hoja o si la ha rellenado y su estado es Rellenado, podra modificarla
            Dim modificableEnvEli As Boolean = (myHoja IsNot Nothing AndAlso (myHoja.Estado = ELL.HojaGastos.eEstado.Rellenada Or myHoja.Estado = ELL.HojaGastos.eEstado.NoValidada))
            modificable = (myHoja Is Nothing OrElse (myHoja IsNot Nothing AndAlso (myHoja.Estado = ELL.HojaGastos.eEstado.Rellenada Or myHoja.Estado = ELL.HojaGastos.eEstado.NoValidada)))
            'Si la HG es sin viaje, habra que hacer unas comprobaciones dependiendo de si tiene visa o no
            Dim bEnviarHG As Boolean = modificableEnvEli
            If (IdViaje = Integer.MinValue) Then
                Dim mes, ano As Integer
                If (myHoja IsNot Nothing) Then
                    mes = myHoja.FechaHasta.Month : ano = myHoja.FechaHasta.Year
                Else
                    Dim fecha As Date = CDate(lblFechasHasta2.Text)
                    mes = fecha.Month : ano = fecha.Year
                End If
                If (Session("ConVisa") IsNot Nothing AndAlso CType(Session("ConVisa"), Boolean) = True) Then
                    'Si tiene una visa asociada, solo la podra enviar cuando se haya cargado el fichero de visas del mes
                    Dim visasBLL As New BLL.VisasBLL
                    Dim bPendientesVisa As Boolean = Not (visasBLL.FicheroVisasCargado(mes, ano, Master.IdPlantaGestion))
                    If (myHoja IsNot Nothing) Then 'Si la hoja no es nothing, se hara un and ya que habra que mirar el estado de la hoja
                        bEnviarHG = bEnviarHG And (Not bPendientesVisa)
                    Else
                        bEnviarHG = Not bPendientesVisa AndAlso (gvGastosV.Rows.Count > 0)
                    End If
                    divSoloGastosVisa.Visible = bPendientesVisa
                Else 'Sino tiene una visa asociada, solo se podra enviar a partir del ultimo dia del mes
                    If (myHoja IsNot Nothing) Then 'Si la hoja no es nothing, se hara un and ya que habra que mirar el estado de la hoja
                        bEnviarHG = bEnviarHG And (CDate(Date.Now) >= New Date(ano, mes, Date.DaysInMonth(ano, mes)))
                    Else
                        bEnviarHG = (CDate(Date.Now) >= New Date(ano, mes, Date.DaysInMonth(ano, mes)))
                    End If
                End If
            Else 'Tiene un viaje
                If (Session("ConVisa") IsNot Nothing AndAlso CType(Session("ConVisa"), Boolean) = True) Then
                    Dim visasBLL As New BLL.VisasBLL
                    Dim fFinViaje As Date = CType(lblFechaHasta.Text, Date)
                    Dim bPendientesVisa As Boolean = Not (visasBLL.FicheroVisasCargado(fFinViaje.Month, fFinViaje.Year, Master.IdPlantaGestion))
                    If (myHoja IsNot Nothing) Then 'Si la hoja no es nothing, se hara un and ya que habra que mirar el estado de la hoja
                        bEnviarHG = bEnviarHG And (Not bPendientesVisa)
                    Else
                        bEnviarHG = Not bPendientesVisa AndAlso (gvGastosV.Rows.Count > 0)
                    End If
                    divSoloGastosVisa.Visible = bPendientesVisa
                    'If (myHoja Is Nothing AndAlso Not bPendientesVisa) Then bEnviarHG = (gvGastosV.Rows.Count > 0)
                End If
            End If
            divSoloGastosVisa.Visible = (divSoloGastosVisa.Visible AndAlso (myHoja Is Nothing OrElse (myHoja IsNot Nothing AndAlso myHoja.Lineas.Count = 0)))  'Solo sera visible cuando no tenga lineas en metalico o kilometraje y no se haya cargado el fichero de visas
            '---------------------------------------------------------------
            btnEnviar.Visible = bEnviarHG
            labelInfo3.Visible = Not bEnviarHG
            btnEliminar.Visible = modificableEnvEli
            If (btnEnviar.Visible) Then btnEnviar.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Enviar HG"), itzultzaileWeb.Itzuli("¿Desea enviar la hoja de gastos a su responsable para que la valide? Una vez enviada, no podra realizar ningun cambio"), String.Empty, "SendHG")
            If (btnEliminar.Visible) Then btnEliminar.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmacion de eliminacion"), itzultzaileWeb.Itzuli("¿Desea eliminar la hoja de gastos?"), String.Empty, "DelHG")
        End If
        'Los filtros para añadir lineas, solo seran visibles si la hoja de gastos es modificable
        pnlFiltroMet.Visible = modificable
        pnlFiltroKm.Visible = modificable
        Dim vLinkEstados As Boolean = (idUserVerInfo > 0 AndAlso IdHoja > 0)  'El link de ver los estados solo sera visible cuando la hoja se haya creado y cuando se vaya a ver la hg de un usuario en concreto        
        lnkVerEstados1.Visible = vLinkEstados
        lnkVerEstados2.Visible = vLinkEstados
        'Si no es modificable, se evita poder quitar lineas de gastos
        If (Not modificable) Then
            lControles = New List(Of Object)
            lControles.Add(New LinkButton With {.ID = "lnkElim"})
            lControles.Add(New ImageButton With {.ID = "imgInvertir"})
            DisableControlsGridview(gvGastosMet, lControles)
            DisableControlsGridview(gvGastosV, lControles)
            DisableControlsGridview(gvGastosKM, lControles)
        End If
        gvGastosMet.Columns(1).Visible = UsuarioVisible()
        gvGastosV.Columns(2).Visible = UsuarioVisible()
        gvGastosKM.Columns(1).Visible = UsuarioVisible()
        gvGastosMet.Columns(6).Visible = (hasProfile(BLL.BidaiakBLL.Profiles.Financiero) OrElse hasProfile(BLL.BidaiakBLL.Profiles.Administrador)) 'Si es financiero o admin, verá la columna cambio moneda
    End Sub

    ''' <summary>
    ''' Carga las monedas existentes
    ''' </summary>	
    Private Sub cargarMonedas()
        If (ddlMonedaMet.Items.Count = 0) Then
            Dim xbatComp As New BLL.XbatBLL
            Dim lMonedas As List(Of ELL.Moneda) = xbatComp.GetMonedas()
            For Each oMon As ELL.Moneda In lMonedas
                ddlMonedaMet.Items.Add(New ListItem(oMon.Nombre.ToUpper, oMon.Id))
                If (oMon.Abreviatura.ToLower = "eur") Then
                    ddlMonedaMet.SelectedValue = oMon.Id
                    hfEuroId.Value = oMon.Id
                End If
            Next
        Else
            ddlMonedaMet.SelectedValue = CInt(hfEuroId.Value)
        End If
    End Sub

    ''' <summary>
    ''' Deshabilita los controles del gridview de forma recursiva
    ''' </summary>
    ''' <param name="gv">Gridview donde operar</param>
    ''' <param name="controles">Lista de controles con los ids a invisibilizar</param>
    ''' <param name="makeInvisibleIfVisible">Indicara si se quiere que se hagan invisibles si son visibles</param>
    Private Sub DisableControlsGridview(ByVal gv As GridView, ByVal controles As List(Of Object), Optional ByVal makeInvisibleIfVisible As Boolean = True)
        For Each row As GridViewRow In gv.Rows
            DisableControlsGridview_Items(row, controles, makeInvisibleIfVisible)
        Next
    End Sub

    ''' <summary>
    ''' Deshabilita los controles del gridview de forma recursiva
    ''' </summary>
    ''' <param name="row">Control a comprobar</param>
    ''' <param name="controles">Lista de controles con los ids a invisibilizar</param>
    ''' <param name="makeInvisibleIfVisible">Indicara si se quiere que se hagan invisibles si son visibles</param>
    Private Sub DisableControlsGridview_Items(ByVal row As Control, ByVal controles As List(Of Object), ByVal makeInvisibleIfVisible As Boolean)
        Dim nombreTipo As String
        Dim lObjetos As List(Of Object)
        For Each rContr As Control In row.Controls
            nombreTipo = rContr.GetType().Name
            lObjetos = controles.FindAll(Function(o As Object) o.GetType.Name = nombreTipo)
            If (lObjetos IsNot Nothing AndAlso lObjetos.Count > 0) Then
                For Each obj As Object In lObjetos
                    If (obj.Id = rContr.ID) Then
                        If (makeInvisibleIfVisible) Then rContr.Visible = False
                    End If
                Next
            End If
            If (rContr.HasControls) Then DisableControlsGridview_Items(rContr, controles, makeInvisibleIfVisible)
        Next
    End Sub

    ''' <summary>
    ''' Deshabilita los controles del repeater de forma recursiva
    ''' </summary>
    Private Sub DisableControlsRepeater(ByVal rpt As Repeater, ByVal controles As List(Of Object))
        For Each item As RepeaterItem In rpt.Items
            DisableControlsRepeater_Items(item, controles)
        Next
    End Sub

    ''' <summary>
    ''' Deshabilita los controles del repeater de forma recursiva
    ''' </summary>
    ''' <param name="item">Control a comprobar</param>
    Private Sub DisableControlsRepeater_Items(ByVal item As Control, ByVal controles As List(Of Object))
        Dim nombreTipo As String
        Dim lObjetos As List(Of Object)
        For Each rContr As Control In item.Controls
            nombreTipo = rContr.GetType().Name
            lObjetos = controles.FindAll(Function(o As Object) o.GetType.Name = nombreTipo)
            If (lObjetos IsNot Nothing AndAlso lObjetos.Count > 0) Then
                For Each obj As Object In lObjetos
                    If (obj.Id = rContr.ID) Then
                        rContr.Visible = False
                    End If
                Next
            End If
            If (rContr.HasControls) Then DisableControlsRepeater_Items(rContr, controles)
        Next
    End Sub

#End Region

#Region "Eventos Gridviews/Repeaters"

    ''' <summary>
    ''' Configura la pantalla modal
    ''' </summary>    
    ''' <param name="titleMessage">Titulo del pop up</param>    
    ''' <param name="message">Mensaje a mostrar</param>
    ''' <param name="action">Tipo de accion</param>
    ''' <param name="param">Parametro</param>    
    Private Function ConfigureModal(ByVal titleMessage As String, ByVal message As String, ByVal param As String, ByVal action As String) As String
        Dim script As New StringBuilder
        script.AppendLine("$('#" & labelTitleModal.ClientID & "').text('" & titleMessage & "');")
        script.AppendLine("$('#" & labelMessageModal.ClientID & "').text('" & message.Replace("'", "") & "');")
        script.AppendLine("$('#" & hfModalParam.ClientID & "').val('" & param & "');")
        script.AppendLine("$('#" & hfModalAction.ClientID & "').val('" & action & "');")
        script.AppendLine("$('#divModal').modal('show'); return false;")
        Return script.ToString
    End Function

    ''' <summary>
    ''' Muestra el panel modal generico para todas las confirmaciones
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    ''' <param name="divId">Id del div a mostrar</param>
    Private Sub ShowModal(ByVal action As Boolean, ByVal divId As String)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#" & divId & "').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#" & divId & "').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True)
        End If
    End Sub

    ''' <summary>
    ''' Pulsa el boton guardar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAceptarModal_Click(sender As Object, e As EventArgs) Handles btnAceptarModal.Click
        Try
            Select Case hfModalAction.Value
                Case "DelGastoMet"
                    QuitarLinea(CInt(hfModalParam.Value), "M")
                Case "DelGastoKm"
                    QuitarLinea(CInt(hfModalParam.Value), "KM")
                Case "DelDocInteg"
                    DeleteDocIntegrante(CInt(hfModalParam.Value))
                Case "ValHG"
                    CambiarEstado("V")  'Esta funciona no llama a cambiarEstadoHG porque ya viene de ella y entra en un bucle
                Case "RejHG"
                    CambiarEstadoHG("R")
                Case "SendHG"
                    EnviarHG()
                Case "DelHG"
                    EliminarHG()
            End Select
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al realizar la accion")
        End Try
        ShowModal(False, "divModal")
    End Sub

    ''' <summary>
    ''' Enlaza las lineas de gastos con y sin recibos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvGastosMet_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvGastosMet.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oLinea As ELL.HojaGastos.Linea = e.Row.DataItem
            Dim lblConcepto As Label = CType(e.Row.FindControl("lblConcepto"), Label)
            Dim chRecibo As CheckBox = CType(e.Row.FindControl("chRecibo"), CheckBox)
            Dim lnkElim As LinkButton = CType(e.Row.FindControl("lnkElim"), LinkButton)
            CType(e.Row.FindControl("lblUsuario"), Label).Text = oLinea.Usuario.NombreCompleto
            CType(e.Row.FindControl("lblFecha"), Label).Text = oLinea.Fecha.ToShortDateString
            CType(e.Row.FindControl("lblImporteMon"), Label).Text = oLinea.Cantidad & " " & oLinea.Moneda.Abreviatura
            CType(e.Row.FindControl("lblImporteEur"), Label).Text = oLinea.ImporteEuros & " EUR"
            CType(e.Row.FindControl("lblCambioMoneda"), Label).Text = If(oLinea.Moneda.Id = 90, "-", oLinea.CambioMonedaEUR)
            lblConcepto.Text = oLinea.ConceptoBatz.Nombre
            If (oLinea.Concepto <> String.Empty) Then lblConcepto.Text &= " (" & oLinea.Concepto & ")"
            chRecibo.Checked = oLinea.Recibo
            If (lnkElim IsNot Nothing) Then
                lnkElim.ToolTip = itzultzaileWeb.Itzuli("Quita la linea de la hoja de gastos")
                lnkElim.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmacion de eliminacion"), itzultzaileWeb.Itzuli("ConfirmarEliminar"), oLinea.Id, "DelGastoMet")
                itzultzaileWeb.Itzuli(lnkElim)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Evento al dar a justificar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvGastosV_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvGastosV.RowCommand
        Try
            If e.CommandName = "Justificar" Then
                Dim movBLL As New BLL.VisasBLL
                Dim idLinea As Integer = CInt(e.CommandArgument)
                Dim oMov As ELL.Visa.Movimiento = movBLL.loadMovimiento(idLinea)
                btnSaveModalJust.CommandName = oMov.Estado
                btnSaveModalJust.CommandArgument = idLinea
                txtJustificacion.Text = oMov.Comentarios
                ShowModal(True, "divModalJustificar")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se guarda el comentario asignado a la visa y se marca como justificada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSaveModalJust_Click(sender As Object, e As EventArgs) Handles btnSaveModalJust.Click
        Try
            If (txtJustificacion.Text.Length > 300) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("300 caracteres maximo")
                ShowModal(True, "divModalJustificar")
            Else
                Dim visasBLL As New BLL.VisasBLL
                Dim idLinea As Integer = CInt(btnSaveModalJust.CommandArgument)
                Dim lMovs As New List(Of ELL.Visa.Movimiento)
                Dim oldState As ELL.Visa.Movimiento.eEstado = CType(btnSaveModalJust.CommandName, ELL.Visa.Movimiento.eEstado)
                Dim newState As ELL.Visa.Movimiento.eEstado = CType(btnSaveModalJust.CommandName, ELL.Visa.Movimiento.eEstado)
                'Si el estado actual es cargado, se le pone justificado, sino el que tenia antes
                If (CType(btnSaveModalJust.CommandName, ELL.Visa.Movimiento.eEstado) = ELL.Visa.Movimiento.eEstado.Cargado) Then newState = ELL.Visa.Movimiento.eEstado.Justificado
                lMovs.Add(New ELL.Visa.Movimiento With {.Id = idLinea, .Estado = newState, .Comentarios = txtJustificacion.Text.Trim})
                visasBLL.CambiarEstadoMovimientos(lMovs)
                If (oldState = ELL.Visa.Movimiento.eEstado.Cargado) Then
                    log.Info("Se ha justificado el gasto de visa (" & idLinea & ") de la HG " & IdHoja)
                Else
                    log.Info("Se ha modificado el comentario del gasto de visa (" & idLinea & ") de la HG " & IdHoja)
                End If
                Try
                    Dim visaBLL As New BLL.VisasBLL
                    Dim mov As List(Of ELL.Visa.Movimiento) = Nothing
                    If (IdViaje <> Integer.MinValue) Then
                        mov = visaBLL.loadMovimientos(Master.Ticket.IdUser, IdViaje, Master.IdPlantaGestion, DateTime.MinValue, DateTime.MinValue, True)
                    Else
                        mov = visaBLL.loadMovimientos(Master.Ticket.IdUser, Nothing, Master.IdPlantaGestion, CDate(lblFechasDesde2.Text), CDate(lblFechasHasta2.Text), False, idHojaLibre:=IdHoja)
                    End If
                    mov = (From linea As ELL.Visa.Movimiento In mov Select linea Order By linea.NombreUsuario Ascending, linea.Fecha Ascending, linea.Sector Ascending).ToList
                    gvGastosV.DataSource = mov
                    gvGastosV.DataBind()
                    divGastosVisaSinCom.Visible = mov.Exists(Function(o) o.Estado = ELL.Visa.Movimiento.eEstado.Cargado)
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("Gasto de visa justificado")
                    ShowModal(False, "divModalJustificar")
                Catch batzEx As BatzException
                    Master.MensajeError = itzultzaileWeb.Itzuli("Se ha guardado el comentario pero ha dado error al listar los gastos de visa")
                    log.Error("Se ha guardado el comentario pero ha dado error al listar los gastos de visa", batzEx.Excepcion)
                    ShowModal(True, "divModalJustificar")
                Catch ex As Exception
                    Master.MensajeError = itzultzaileWeb.Itzuli("Se ha guardado el comentario pero ha dado error al listar los gastos de visa")
                    log.Error("Se ha guardado el comentario pero ha dado error al listar los gastos de visa", ex)
                    ShowModal(True, "divModalJustificar")
                End Try
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
            ShowModal(True, "divModalJustificar")
        End Try
    End Sub

    ''' <summary>
    ''' Enlaza las lineas de gastos de visa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvGastosVisas_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvGastosV.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oLinea As ELL.Visa.Movimiento = e.Row.DataItem
            Dim imgWarning As Image = CType(e.Row.FindControl("imgWarning"), Image)
            Dim imgEstado As ImageButton = CType(e.Row.FindControl("imgEstado"), ImageButton)
            Dim lblComentario As Label = CType(e.Row.FindControl("lblComentario"), Label)
            Dim imgMasComentarios As Image = CType(e.Row.FindControl("imgMasComentarios"), Image)
            Dim lnkJustificar As LinkButton = CType(e.Row.FindControl("lnkJustificar"), LinkButton)
            CType(e.Row.FindControl("lblUsuario"), Label).Text = oLinea.NombreUsuario
            CType(e.Row.FindControl("lblFecha"), Label).Text = oLinea.Fecha.ToShortDateString
            Dim fDesde, fHasta As DateTime
            If (pnlSinIdViaje.Visible) Then 'Sin viaje
                fDesde = CDate(lblFechasDesde2.Text)
                fHasta = CDate(lblFechasHasta2.Text)
            Else 'Con viaje
                fDesde = CDate(lblFechaDesde.Text)
                fHasta = CDate(lblFechaHasta.Text)
            End If
            'Si esta fuera de las fechas, se indica            
            If Not (fDesde <= oLinea.Fecha AndAlso oLinea.Fecha <= fHasta) Then
                divGastosVisaDespues.Visible = True : imgWarning.Visible = True
                imgWarning.ToolTip = itzultzaileWeb.Itzuli("Este gasto pertenece a otro mes pero no se ha podido meter en dicha hoja ya que estaba enviada o validada")
            Else
                imgWarning.Visible = False
            End If
            If (IdViaje > 0 And oLinea.IdImportacion > 0) Then 'Cuando se trata de un viaje, los movimientos de visa siempre estaran dentro de las fechas. Habra que ver si la fecha de insercion, es mayor que la fecha de validacion
                Dim loadMovDate As DateTime = DateTime.MinValue
                If (hImportaciones IsNot Nothing AndAlso hImportaciones.ContainsKey(oLinea.IdImportacion)) Then
                    loadMovDate = CDate(hImportaciones.Item(oLinea.IdImportacion))
                Else
                    Dim bidaiBLL As New BLL.BidaiakBLL
                    Dim import As BLL.BidaiakBLL.Importacion = bidaiBLL.loadImportacionDoc(oLinea.IdImportacion)
                    loadMovDate = import.Fecha
                    If (hImportaciones Is Nothing) Then hImportaciones = New Hashtable
                    hImportaciones.Add(oLinea.IdImportacion, import.Fecha)
                End If
                Dim lastHGStateDate As DateTime = If(hfLastHGStateDate.Value <> String.Empty, CDate(hfLastHGStateDate.Value), DateTime.MinValue)
                If (lastHGStateDate <> DateTime.MinValue AndAlso loadMovDate > lastHGStateDate) Then
                    divGastosVisaDespues.Visible = True : imgWarning.Visible = True
                    imgWarning.ToolTip = itzultzaileWeb.Itzuli("Este movimiento de visa ha llegado despues de que la hoja se validara")
                End If
            End If
            CType(e.Row.FindControl("lblImporteEur"), Label).Text = oLinea.ImporteEuros & " " & oLinea.Moneda.Abreviatura
            CType(e.Row.FindControl("lblImporteMonedaGasto"), Label).Text = oLinea.ImporteMonedaGasto & " " & oLinea.MonedaGasto.Abreviatura
            lblComentario.Visible = True : imgMasComentarios.Visible = False
            If (oLinea.Comentarios <> String.Empty) Then
                If (oLinea.Comentarios.Length > 70) Then
                    imgMasComentarios.Visible = True
                    lblComentario.Text = oLinea.Comentarios.Substring(0, 70).Replace(vbCrLf, "<br />") & "..."
                    imgMasComentarios.ToolTip = oLinea.Comentarios
                Else
                    lblComentario.Text = oLinea.Comentarios.Replace(vbCrLf, "<br />")
                End If
            End If
            If (oLinea.Estado = ELL.Visa.Movimiento.eEstado.Cargado) Then
                lblComentario.Text = itzultzaileWeb.Itzuli("Gasto sin comentar")
                lblComentario.CssClass = "danger"
            End If
            Dim bPropietarioHoja As Boolean = (Master.Ticket.IdUser = oLinea.IdUsuario)
            lnkJustificar.Visible = (oLinea.Estado = ELL.Visa.Movimiento.eEstado.Cargado AndAlso bPropietarioHoja)  'El usuario siempre podra justificar el gasto pero solo se visualizara el link cuando este cargado            
            lnkJustificar.CommandArgument = oLinea.Id
            imgEstado.CommandArgument = oLinea.Id
            Select Case oLinea.Estado
                Case ELL.Visa.Movimiento.eEstado.Cargado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Sin_Validar.png"
                    If (bPropietarioHoja) Then
                        imgEstado.ToolTip = itzultzaileWeb.Itzuli("Justificar gasto")
                    Else
                        imgEstado.ToolTip = itzultzaileWeb.Itzuli("Cargado")
                    End If
                    hfGastosVSinJustif.Value = True : divGastosVisaSinCom.Visible = True  'Con que haya un mov en este estado, se visualizara
                Case ELL.Visa.Movimiento.eEstado.Conforme
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Validado.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Conforme") & If(bPropietarioHoja, "(" & itzultzaileWeb.Itzuli("Editar comentarios") & ")", "")
                Case ELL.Visa.Movimiento.eEstado.No_Conforme
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Rechazada.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("No conforme") & If(bPropietarioHoja, "(" & itzultzaileWeb.Itzuli("Editar comentarios") & ")", "")
                Case ELL.Visa.Movimiento.eEstado.Liquidado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Integrado.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Integrado") & If(bPropietarioHoja, "(" & itzultzaileWeb.Itzuli("Editar comentarios") & ")", "")
                Case ELL.Visa.Movimiento.eEstado.Justificado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Justificado.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Justicado/Comentado") & If(bPropietarioHoja, "(" & itzultzaileWeb.Itzuli("Editar comentarios") & ")", "")
            End Select
            imgEstado.Enabled = bPropietarioHoja
        End If
    End Sub

    ''' <summary>
    ''' Enlaza las lineas de gastos de los kilometrajes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvGastosKM_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvGastosKM.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oLinea As ELL.HojaGastos.Linea = e.Row.DataItem
            Dim imgInvertir As ImageButton = CType(e.Row.FindControl("imgInvertir"), ImageButton)
            Dim lnkElim As LinkButton = CType(e.Row.FindControl("lnkElim"), LinkButton)
            CType(e.Row.FindControl("lblUsuario"), Label).Text = oLinea.Usuario.NombreCompleto
            CType(e.Row.FindControl("lblFecha"), Label).Text = oLinea.Fecha.ToShortDateString
            CType(e.Row.FindControl("lblImporteKM"), Label).Text = precioKm & " EUR" 'Variable global calculada en el procedimiento mostrarDetalle
            CType(e.Row.FindControl("lblImporteEur"), Label).Text = oLinea.Cantidad & " EUR"
            imgInvertir.CommandArgument = oLinea.Id
            imgInvertir.ToolTip = itzultzaileWeb.Itzuli("Crea una nueva linea igual pero con los lugares invertidos. La fecha la coge de la caja de texto")
            If (lnkElim IsNot Nothing) Then
                lnkElim.ToolTip = itzultzaileWeb.Itzuli("Quita la linea de la hoja de gastos")
                lnkElim.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmacion de eliminacion"), itzultzaileWeb.Itzuli("ConfirmarEliminar"), oLinea.Id, "DelGastoKm")
                itzultzaileWeb.Itzuli(lnkElim)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Añade la linea a la hoja de gastos
    ''' Si es un movimiento de una HG libre, comprobar que ese movimiento no este dentro de la HG de un viaje
    ''' Ademas, si todavia no existe la HG y es libre, comprobar que no tiene HG libres en el rango y que no existe otra HG libre con el estado rellenada
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAddLinea_Click(sender As Object, e As EventArgs) Handles btnAddLineaMetalico.Click, btnAddLineaKm.Click
        Try
            Dim btnEvent As Button = CType(sender, Button)
            Dim bSave, bFechaError, bCantKmMayorCero As Boolean
            Dim cambioMoneda As Decimal = 0
            Dim anticipBLL As New BLL.AnticiposBLL
            Dim xbatBLL As New BLL.XbatBLL
            Dim myDate As Date
            Dim oLinea As New ELL.HojaGastos.Linea With {.IdHoja = IdHoja, .Usuario = New SabLib.ELL.Usuario With {.Id = Master.Ticket.IdUser}}
            bSave = True : bFechaError = False : bCantKmMayorCero = False
            Select Case btnEvent.CommandName
                Case "M"
                    If (txtFechaMet.Text = String.Empty OrElse txtImporteMet.Text.Trim = String.Empty OrElse (ddlConceptosConReciboMet.SelectedIndex = 0 AndAlso ddlConceptosSinReciboMet.SelectedIndex = 0) OrElse
                        (ddlConceptosConReciboMet.SelectedIndex > 0 AndAlso CType(ddlConceptosConReciboMet.SelectedValue.Split("|")(1), Boolean) AndAlso txtConceptoConReciboMet.Text.Trim = String.Empty) OrElse
                        (ddlConceptosSinReciboMet.SelectedIndex > 0 AndAlso CType(ddlConceptosSinReciboMet.SelectedValue.Split("|")(1), Boolean) AndAlso txtConceptoSinReciboMet.Text.Trim = String.Empty)) Then
                        bSave = False
                        Exit Select
                    ElseIf (Not Date.TryParse(txtFechaMet.Text, myDate)) Then
                        bFechaError = True
                        Exit Select
                    End If
                    oLinea.Fecha = CDate(txtFechaMet.Text)
                    Dim fechaCambio As DateTime = oLinea.Fecha
                    If (IdViaje > 0) Then 'Si tiene anticipo, la fecha de cambio se coge de la fecha de entrega
                        fechaCambio = anticipBLL.loadAnticipoFechaEntrega(IdViaje)
                        If (fechaCambio = DateTime.MinValue) Then fechaCambio = oLinea.Fecha 'Se asigna la fecha de la linea para que no falle                    
                    End If
                    oLinea.TipoGasto = ELL.HojaGastos.Linea.eTipoGasto.Metalico
                    If (ddlConceptosConReciboMet.SelectedIndex > 0) Then
                        oLinea.Recibo = True
                        oLinea.Concepto = Request.Form.GetValues(txtConceptoConReciboMet.ClientID.Replace("_", "$"))(0)
                        oLinea.ConceptoBatz = New ELL.Concepto With {.Id = CInt(ddlConceptosConReciboMet.SelectedValue.Split("|")(0))}  'Se hace un split porque en el value viene Id|RequiereDetalle
                    Else
                        oLinea.Recibo = False
                        oLinea.Concepto = Request.Form.GetValues(txtConceptoSinReciboMet.ClientID.Replace("_", "$"))(0)
                        oLinea.ConceptoBatz = New ELL.Concepto With {.Id = CInt(ddlConceptosSinReciboMet.SelectedValue.Split("|")(0))}  'Se hace un split porque en el value viene Id|RequiereDetalle
                    End If
                    oLinea.Moneda = xbatBLL.GetMoneda(CInt(ddlMonedaMet.SelectedValue))
                    oLinea.Cantidad = DecimalValue(txtImporteMet.Text)
                    oLinea.ImporteEuros = xbatBLL.ObtenerRateEuros(oLinea.Moneda.Id, oLinea.Cantidad, fechaCambio, cambioMoneda)
                    oLinea.CambioMonedaEUR = cambioMoneda
                    bCantKmMayorCero = (oLinea.Cantidad > 0)
                Case "KM"
                    If (txtFechaKm.Text = String.Empty OrElse txtOrigenKm.Text.Trim = String.Empty Or txtDestinoKm.Text.Trim = String.Empty Or txtKm.Text.Trim = String.Empty) Then
                        bSave = False
                        Exit Select
                    ElseIf (Not Date.TryParse(txtFechaKm.Text, myDate)) Then
                        bFechaError = True
                        Exit Select
                    End If
                    Dim params As ELL.Parametro = getParametrosKm()
                    oLinea.Fecha = CDate(txtFechaKm.Text)
                    oLinea.TipoGasto = ELL.HojaGastos.Linea.eTipoGasto.Kilometraje
                    oLinea.LugarOrigen = txtOrigenKm.Text.Trim
                    oLinea.LugarDestino = txtDestinoKm.Text.Trim
                    oLinea.Kilometros = DecimalValue(txtKm.Text)
                    oLinea.Cantidad = oLinea.Kilometros * params.PrecioKm
                    oLinea.Moneda = xbatBLL.GetMoneda("EUR")
                    oLinea.ConceptoBatz = New ELL.Concepto With {.Id = params.IdConceptoKm}
                    oLinea.ImporteEuros = oLinea.Cantidad  'Porque es en euros
                    oLinea.CambioMonedaEUR = 1
                    bCantKmMayorCero = (oLinea.Kilometros > 0)
            End Select
            Dim bConMensaje As Boolean = False
            If (oLinea.Fecha > CType(Now.ToShortDateString, Date)) Then
                bConMensaje = True
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La fecha de la linea no puede ser superior al dia de hoy")
            Else
                If (bFechaError) Then
                    bConMensaje = True
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El formato de la fecha no es correcto")
                ElseIf (Not bCantKmMayorCero) Then
                    bConMensaje = True
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Tiene que introducir un kilometraje o cantidad mayor que cero")
                ElseIf (bSave) Then
                    If (IdViaje = Integer.MinValue) Then
                        'Se chequea que si es una hoja de gastos sin viaje asociado, este dentro de las fechas
                        Dim fDesde, fHasta As Date
                        fDesde = CDate(lblFechasDesde2.Text) : fHasta = CDate(lblFechasHasta2.Text)
                        If (oLinea.Fecha < fDesde Or oLinea.Fecha > fHasta) Then
                            bConMensaje = True
                            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No puede añadir la linea porque esta fuera de las fechas de la hoja")
                            Exit Sub
                        End If
                    End If
                    Dim fInicio As Date = Date.MaxValue
                    If (lblFechasDesde2.Text <> String.Empty) Then fInicio = CDate(lblFechasDesde2.Text)
                    IdHoja = hojasBLL.AddLinea(oLinea, IdViaje, CInt(Session("IdResponsable")), Master.IdPlantaGestion)
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("Linea añadida a la hoja de gastos")
                    mostrarDetalle()
                Else
                    bConMensaje = True
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("debeRellenarDatos")
                End If
            End If
            If (bConMensaje) Then
                If (ddlConceptosConReciboMet.SelectedIndex > 0) Then
                    If (CType(ddlConceptosConReciboMet.SelectedValue.Split("|")(1), Boolean)) Then txtConceptoConReciboMet.Style("display") = "inline"
                Else
                    If (CType(ddlConceptosSinReciboMet.SelectedValue.Split("|")(1), Boolean)) Then txtConceptoSinReciboMet.Style("display") = "inline"
                End If
            End If
        Catch batzEx As BatzException
            If (batzEx.Excepcion IsNot Nothing AndAlso batzEx.Excepcion.Message = "adv") Then  'Es una forma de indicar que es una advertencia y no un error
                Master.MensajeAdvertencia = batzEx.Termino
            Else
                Master.MensajeError = batzEx.Termino
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al añadir la linea")
        End Try
    End Sub

    ''' <summary>
    ''' Se añde una linea nueva con los valores invertidos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub InvertirLinea(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Try
            Dim img As ImageButton = CType(sender, ImageButton)
            Dim hojaGastoBLL As New BLL.HojasGastosBLL
            Dim oLinea As ELL.HojaGastos.Linea = hojaGastoBLL.loadLineaHojaGastos(CInt(img.CommandArgument))
            Dim lugarOrigen As String = oLinea.LugarOrigen
            Dim params As ELL.Parametro = getParametrosKm()
            oLinea.Id = Integer.MinValue
            oLinea.LugarOrigen = oLinea.LugarDestino
            oLinea.LugarDestino = lugarOrigen
            oLinea.Fecha = CDate(txtFechaKm.Text)
            oLinea.ConceptoBatz = New ELL.Concepto With {.Id = params.IdConceptoKm}
            IdHoja = hojasBLL.AddLinea(oLinea, IdViaje, CInt(Session("IdResponsable")), Master.IdPlantaGestion)
            log.Info("Se ha añadido una linea de kilometraje usando el boton de invertir linea, en la hoja " & IdHoja)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Linea añadida a la hoja de gastos")
            mostrarDetalle()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al invertir la linea de kilometraje")
        End Try
    End Sub

    ''' <summary>
    ''' Se quita la linea con seleccionada
    ''' </summary>
    ''' <param name="idLinea">Id de la linea</param>
    ''' <param name="origen">M:metalico,K:Km</param>
    Protected Sub QuitarLinea(ByVal idLinea As Integer, ByVal origen As String)
        Try
            hojasBLL.DeleteLinea(idLinea)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Linea eliminada de la hoja de gastos")
            Dim mensa As String = "HOJA_GASTOS:El usuario " & Master.Ticket.NombreCompleto & " (" & Master.Ticket.IdUser & ") ha borrado una linea del bloque "
            Select Case origen
                Case "M" : mensa &= "GASTOS EN METALICO"
                Case "KM" : mensa &= "GASTOS DE KILOMETRAJE"
            End Select
            mensa &= " de su hoja de gastos"
            log.Info(mensa)
            mostrarDetalle()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Visualiza u oculta la columna de usuario
    ''' Solo sera visible cuando entre el organizador y seleccione todos
    ''' </summary>
    ''' <returns></returns>    
    Protected Function UsuarioVisible() As Boolean
        Return ((pnlMultiUser.Visible AndAlso ddlUsuarios.SelectedValue = Integer.MinValue) Or Origen = "ant")
    End Function

    ''' <summary>
    ''' Se enlazan los trayectos de kilometraje
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvTrayectosKM_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvTrayectosKM.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvTrayectosKM, "Select$" & CStr(e.Row.RowIndex))
        End If
    End Sub

    ''' <summary>
    ''' Se añade a la hoja de gastos el trayecto de kilometraje
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvTrayectosKM_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvTrayectosKM.RowCommand
        Try
            If (e.CommandName = "Select") Then
                txtOrigenKm.Text = gvTrayectosKM.Rows(CInt(e.CommandArgument)).Cells(0).Text
                txtDestinoKm.Text = gvTrayectosKM.Rows(CInt(e.CommandArgument)).Cells(1).Text
                txtKm.Text = gvTrayectosKM.Rows(CInt(e.CommandArgument)).Cells(2).Text
                btnAddLinea_Click(New Button With {.CommandName = "KM"}, Nothing)
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al insertar el trayecto de kilometraje")
        End Try
    End Sub

    ''' <summary>
    ''' Se redirige al detalle de una transferencia
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>    
    Private Sub rptTransf_ItemCommand(ByVal source As Object, ByVal e As RepeaterCommandEventArgs) Handles rptTransf.ItemCommand
        If (IdHoja > 0) Then Response.Redirect("TransferenciaAnticipo.aspx?idHoja=" & IdHoja & "&orig=HG&" & e.CommandArgument)
    End Sub

    ''' <summary>
    ''' Se enlazan las posibles transferencias entre viajes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rptTransf_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptTransf.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim transf As String() = e.Item.DataItem
            Dim lnkTransfEdit As LinkButton = CType(e.Item.FindControl("lnkTransfEdit"), LinkButton)
            Dim lblTransf As Label = CType(e.Item.FindControl("lblTransf"), Label)
            If (CInt(transf(0)) = IdViaje) Then  'Transferencia desde este viaje a otro
                lnkTransfEdit.Text = itzultzaileWeb.Itzuli("Transferencia a") & " V" & transf(1) 'IdViajeDestino
                lnkTransfEdit.CommandArgument = "AB=" & transf(1)
                lblTransf.Text = "-" & Math.Round(CDec(transf(2)), 2)
            Else  'Transferencia desde otro viaje
                lnkTransfEdit.Text &= itzultzaileWeb.Itzuli("Transferencia desde") & " V" & transf(0) 'IdViajeOrigen
                lnkTransfEdit.CommandArgument = "BA=" & transf(0)
                lblTransf.Text = "+" & Math.Round(CDec(transf(2)), 2)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Elimina un documento de un integrante
    ''' </summary>    
    Private Sub DeleteDocIntegrante(ByVal idDoc As Integer)
        Try
            Dim viajesBLL As New BLL.ViajesBLL
            viajesBLL.DeleteDocumentoIntegrante(idDoc)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Documento borrado")
            log.Info("Se ha borrado el documento " & idDoc & " de la HG " & IdHoja)
            mostrarDetalle()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se cargan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rptDocumentos_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptDocumentos.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oDoc As ELL.Viaje.DocumentoIntegrante = e.Item.DataItem
            Dim hkTitulo As HyperLink = CType(e.Item.FindControl("hkTitulo"), HyperLink)
            Dim lnkEliminar As LinkButton = CType(e.Item.FindControl("lnkEliminar"), LinkButton)
            hkTitulo.Text = oDoc.Titulo
            hkTitulo.NavigateUrl = "~/Publico/ViewDocument.aspx?id=" & oDoc.Id & "&tipo=docintegrviaje"
            If (oDoc.FechaSubida <> DateTime.MinValue) Then
                hkTitulo.ToolTip = itzultzaileWeb.Itzuli("Subido el") & ": " & oDoc.FechaSubida.ToShortDateString & " - " & oDoc.FechaSubida.ToShortTimeString
            Else '031213: Todos los documentos antiguos, no tienen informado este campo
                hkTitulo.ToolTip = itzultzaileWeb.Itzuli("Abrir documento")
            End If
            lnkEliminar.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmacion de eliminacion"), itzultzaileWeb.Itzuli("ConfirmarEliminar"), oDoc.Id, "DelDocInteg")
        End If
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Cambia el estado de una hoja de gastos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnCambiarEstado_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnValidarHoja.Click
        Try
            CambiarEstadoHG("V")
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al intentar cambiar el estado")
        End Try
    End Sub

    ''' <summary>
    ''' Accion paraver si se puede cambiar de estado una hoja de gastos
    ''' </summary>
    ''' <param name="action">V:Validar y R:Rechazar</param>
    Private Sub CambiarEstadoHG(ByVal action As String)
        If (ddlUsuarios.Items.Count = 0 OrElse (ddlUsuarios.Items.Count > 0 AndAlso ddlUsuarios.SelectedValue <> Integer.MinValue)) Then
            If (action = "V" AndAlso hfGastosVSinJustif.Value <> String.Empty) Then 'Existen gastos de visa sin validar. Se le avisa con un popup
                'labelTitleModal.Text = itzultzaileWeb.Itzuli("Validacion hoja de gastos")
                'labelMessageModal.Text = itzultzaileWeb.Itzuli("Existen gastos de visa que todavia no se han justificado. Es necesario que la persona los justifique para poder validar la hoja de gastos")
                'hfModalAction.Value = "ValHG"
                'ShowModal(True, "divModal")
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Existen gastos de visa que todavia no se han justificado. Es necesario que la persona los justifique para poder validar la hoja de gastos")
            Else
                CambiarEstado(action)
            End If
        Else
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar un usuario")
        End If
    End Sub

    ''' <summary>
    ''' Cambia el estado de la hoja
    ''' </summary>    
    ''' <param name="action">Accion V:Validar y R:Rechazar</param>
    Private Sub CambiarEstado(ByVal action As String)
        Dim estado As ELL.HojaGastos.eEstado
        Dim sEstado As String = String.Empty
        Dim emailUser, nombreUser As String
        If (action = "V") Then
            estado = ELL.HojaGastos.eEstado.Validada
            sEstado = "validada"
        ElseIf (action = "R") Then  'Devolver: La vuelve a dejar modificable por el usuario
            estado = ELL.HojaGastos.eEstado.NoValidada
            sEstado = "No validada"
        End If
        hojasBLL.ChangeState(IdHoja, estado)
        Master.MensajeInfo = itzultzaileWeb.Itzuli("Cambio de estado realizado")
        Dim infoHoja As String
        If (pnlInfoViaje.Visible) Then
            infoHoja = lblIdViaje.Text
        Else
            infoHoja = lblSinIdViaje.Text
        End If
        Dim userBLL As New SabLib.BLL.UsuariosComponent
        Dim oUser As SabLib.ELL.Usuario = Nothing
        Dim idUser As Integer
        Dim mensa As String = "HOJA_GASTOS:El usuario " & Master.Ticket.NombreCompleto & " (" & Master.Ticket.IdUser & ") ha cambiado el estado de la hoja de gastos [" & infoHoja & "] de "
        If (ddlUsuarios.Items.Count > 0) Then
            idUser = CInt(ddlUsuarios.SelectedValue)
        Else
            idUser = CInt(btnValidarHoja.CommandArgument)
        End If
        oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idUser}, False)
        mensa &= oUser.NombreCompleto & " (" & oUser.Id & ") "
        emailUser = oUser.Email
        nombreUser = oUser.NombreCompleto
        mensa &= "al estado [" & sEstado & "]"
        log.Info(mensa)
        Try
            'Hay que quitar el usuario del desplegable si se le ha devuelto
            If (estado = ELL.HojaGastos.eEstado.Rellenada) Then ddlUsuarios.Items.Remove(ddlUsuarios.SelectedItem)
        Catch
        End Try
        If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
            Try
                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                Dim profileBLL As New BLL.BidaiakBLL
                Dim subject As String = "Hoja de gastos" & " (" & infoHoja & ")"
                Dim linkUrl As String = String.Empty
                Dim sPerfil As String() = profileBLL.loadProfile(Master.IdPlantaGestion, oUser.Id, CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon")))
                linkUrl = If(sPerfil(1) = "0", "index.aspx", "index_Directo.aspx")
                linkUrl &= "?hojaGasto=" & IdHoja
                Dim body As String = "Su responsable " & Master.Ticket.NombreCompleto & " "
                If (estado = ELL.HojaGastos.eEstado.Validada) Then
                    body &= "ha validado su hoja de gastos (" & infoHoja & ")<br />No olvide que tiene que imprimirla y adjuntarla en el sobre a entregar en administracion"
                ElseIf (estado = ELL.HojaGastos.eEstado.NoValidada) Then
                    body &= "no ha validado su hoja de gastos (" & infoHoja & ")"
                Else
                    body &= "ha modificado el estado de su hoja de gastos (" & infoHoja & ") a " & sEstado
                End If
                body = PageBase.getBodyHmtl("Hoja de gastos", infoHoja, body, linkUrl, (sPerfil(1) = "0"))
                SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), oUser.Email, subject, body, serverEmail)
                log.Info("HOJA DE GASTOS:Se ha enviado un email para indicar que el responsable " & Master.Ticket.NombreCompleto & " ha cambiado el estado de la hoja de gastos a " & oUser.NombreCompleto)
            Catch ex As Exception
                log.Error("HOJA DE GASTOS: No se ha podido avisar al usuario (" & oUser.NombreCompleto & ") del cambio de estado de su hoja de gastos", ex)
            End Try
        End If
        If (action = "V") Then  'La ha validado
            Response.Redirect("InfoHG.aspx?idH=" & IdHoja & "&Origen=" & Origen, False)
        Else
            mostrarDetalle()
        End If
    End Sub

    ''' <summary>
    ''' Envia la hoja de gastos al responsable, le cambia de estado y se actualizan las fechas de la hoja de gastos
    ''' </summary>    
    Private Sub EnviarHG()
        Try
            If (gvGastosMet.Rows.Count + gvGastosKM.Rows.Count + gvGastosV.Rows.Count > 0) Then
                'Se comprueba que todos los gastos de visa esten justificados                
                Dim visasBLL As New BLL.VisasBLL
                Dim fechaDesde, fechaHasta As DateTime
                If (IdViaje <> Integer.MinValue) Then
                    fechaDesde = CDate(lblFechaDesde.Text)
                    fechaHasta = CDate(lblFechaHasta.Text)
                Else
                    fechaDesde = CDate(lblFechasDesde2.Text)
                    fechaHasta = CDate(lblFechasHasta2.Text)
                End If
                Dim lMovVisa As List(Of ELL.Visa.Movimiento) = visasBLL.loadMovimientos(Master.Ticket.IdUser, IdViaje, Master.IdPlantaGestion, fechaDesde, fechaHasta, bUserYPupilos:=False, bSinJustificar:=True, idHojaLibre:=IdHoja)
                If (lMovVisa IsNot Nothing AndAlso lMovVisa.Count > 0) Then
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No se puede enviar la hoja de gastos porque existen gastos de visa sin justificar")
                Else
                    Dim bValidada As Boolean = False
                    Dim estadoHoja As ELL.HojaGastos.eEstado = ELL.HojaGastos.eEstado.Enviada
                    If (Master.Ticket.IdUser = CInt(Session("IdResponsable"))) Then
                        estadoHoja = ELL.HojaGastos.eEstado.Validada
                        bValidada = True
                        log.Info("Al coincidir el usuario de la hoja y su validador, se va a marcara como validada automaticamente")
                    End If
                    If (IdHoja > 0) Then
                        If (estadoHoja = ELL.HojaGastos.eEstado.Validada) Then hojasBLL.ChangeState(IdHoja, ELL.HojaGastos.eEstado.Enviada) 'Hay que informar la fecha de envio aunque se valide automaticamente
                        hojasBLL.ChangeState(IdHoja, estadoHoja)
                        If (estadoHoja = ELL.HojaGastos.eEstado.Enviada) Then 'Cuando se envia una hoja de gastos, se actualiza el id_validador por si ya no fuera el mismo
                            hojasBLL.UpdateHGValidator(IdHoja, CInt(Session("IdResponsable")))
                        End If
                        'Si solo tiene gastos de visas y estan todos confirmados, se valida automaticamente
                        If (gvGastosMet.Rows.Count = 0 AndAlso gvGastosKM.Rows.Count = 0 AndAlso gvGastosV.Rows.Count > 0) Then
                            lMovVisa = visasBLL.loadMovimientos(Master.Ticket.IdUser, IdViaje, Master.IdPlantaGestion, fechaDesde, fechaHasta, idHojaLibre:=IdHoja)
                            If (lMovVisa IsNot Nothing AndAlso lMovVisa.FindAll(Function(o As ELL.Visa.Movimiento) o.Estado = ELL.Visa.Movimiento.eEstado.Conforme).Count = lMovVisa.Count) Then
                                bValidada = True
                                hojasBLL.ChangeState(IdHoja, ELL.HojaGastos.eEstado.Validada)
                                log.Info("Al enviar la hoja de gastos solo con gastos de visa y estar ya todos validados, se ha marcado como validada la hoja automaticamente")
                            End If
                        End If
                    Else
                        Dim myHojaCab As New ELL.HojaGastos With {.Usuario = New SabLib.ELL.Usuario With {.Id = Master.Ticket.IdUser}, .Estado = estadoHoja,
                                                                  .IdViaje = IdViaje, .Validador = New SabLib.ELL.Usuario With {.Id = CInt(Session("IdResponsable"))},
                                                                  .FechaDesde = fechaDesde, .FechaHasta = fechaHasta}
                        lMovVisa = visasBLL.loadMovimientos(myHojaCab.Usuario.Id, IdViaje, Master.IdPlantaGestion, fechaDesde, fechaHasta, False)
                        If (lMovVisa IsNot Nothing AndAlso lMovVisa.FindAll(Function(o As ELL.Visa.Movimiento) o.Estado = ELL.Visa.Movimiento.eEstado.Conforme).Count = lMovVisa.Count) Then
                            bValidada = True
                            myHojaCab.Estado = ELL.HojaGastos.eEstado.Validada
                            log.Info("Al enviar la hoja de gastos solo con gastos de visa y estar ya todos validados, se ha marcado como validada la hoja automaticamente")
                        End If
                        IdHoja = hojasBLL.CreateCabecera(myHojaCab)
                    End If
                    Dim mensaje As String = String.Empty
                    If (bValidada) Then
                        mensaje = "HOJA_GASTOS:La hoja de gastos del usuario " & Master.Ticket.NombreCompleto & " (" & Master.Ticket.IdUser & ") ha sido validada automaticamente"
                    Else
                        mensaje = "HOJA_GASTOS:El usuario " & Master.Ticket.NombreCompleto & " (" & Master.Ticket.IdUser & ") ha enviado su hoja de gastos a su validador"
                    End If
                    Dim infoHoja As String = String.Empty
                    If (IdViaje <> Integer.MinValue) Then
                        mensaje &= " [idViaje=" & IdViaje & "]"
                        infoHoja = "V" & IdViaje
                    Else
                        Dim myHoja As ELL.HojaGastos = hojasBLL.loadHoja(IdHoja)
                        mensaje &= " [idHoja=" & myHoja.IdSinViaje & "]"
                        infoHoja = "H" & myHoja.IdSinViaje
                    End If
                    log.Info(mensaje)
                    'Si no se ha validado automaticamente, se envia el email
                    If (Not bValidada AndAlso ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
                        Try
                            Dim paramBLL As New SabLib.BLL.ParametrosBLL
                            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                            Dim bidaiakBLL As New BLL.BidaiakBLL
                            Dim userBLL As New SabLib.BLL.UsuariosComponent
                            Dim idResp As Integer = CInt(Session("IdResponsable"))
                            Dim oResp As New SabLib.ELL.Usuario With {.Id = idResp}
                            oResp = userBLL.GetUsuario(oResp)
                            Dim subject As String = "Hoja de gastos" & " (" & infoHoja & ")"
                            Dim sPerfil As String() = bidaiakBLL.loadProfile(Master.IdPlantaGestion, idResp, CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon")))
                            Dim linkUrl As String = If(sPerfil(1) = "0", "index.aspx", "index_Directo.aspx")
                            linkUrl &= "?hojaGasto=" & IdHoja
                            Dim body As String = PageBase.getBodyHmtl("Hoja de gastos", infoHoja, Master.Ticket.NombreCompleto & " ha enviado su hoja de gastos (" & infoHoja & ") para ser validada. Acceda a bidaiak para consultarla", linkUrl, (sPerfil(1) = "0"))
                            SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), oResp.Email, subject, body, serverEmail)
                            log.Info("HOJA DE GASTOS:Se ha enviado un email al responsable " & oResp.NombreCompleto & " para avisarle del envio de la hoja de gastos (" & infoHoja & ")")
                        Catch ex As Exception
                            log.Error("HOJA DE GASTOS: No se ha podido avisar al responsable del envio de la hoja de gastos", ex)
                        End Try
                    End If
                    Response.Redirect("InfoHG.aspx?idH=" & IdHoja & "&origen=" & Origen, False)
                End If
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe introducir alguna linea antes de enviarla")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Genera el pdf y lo visualiza
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImprimir.Click
        Dim fs As IO.FileStream = Nothing
        Try
            Dim ruta As String = BLL.GenerarPDF.HojaGastos(IdHoja, Master.IdPlantaGestion, Master.Ticket.Culture, Session.SessionID, Session("Perfil"))
            fs = IO.File.OpenRead(ruta)
            Dim bytes(fs.Length) As Byte
            fs.Read(bytes, 0, CInt(fs.Length))
            fs.Close()
            If (Origen = "ALER") Then
                'En caso de que venga la pagina de alertas para la reimpresion, se elimina dicho registro
                Dim bidaiBLL As New BLL.BidaiakBLL
                bidaiBLL.DeleteHGReimprimir(Master.Ticket.IdUser, IdViaje)
                log.Info("Se ha borrado el registro de aviso de reimpresion del usuario " & Master.Ticket.IdUser & " y viaje " & IdViaje)
            End If
            log.Info("HOJA DE GASTOS: Se a generado el pdf para imprimir la HG")
            Response.Clear()
            Response.AppendHeader("Content-Disposition", "attachment; filename=HojaGastos.pdf")
            Response.OutputStream.Write(bytes, 0, bytes.Length)
            Response.OutputStream.Flush()
            Response.OutputStream.Close()
            Response.End()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Finally
            If (fs IsNot Nothing) Then fs.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Elimina la HG de la base de datos
    ''' </summary>    
    Private Sub EliminarHG()
        Try
            hojasBLL.Delete(IdHoja)
            Dim myIdViaje As String = If(IdViaje <> Integer.MinValue, "V" & IdViaje, lblSinIdViaje.Text)
            Dim mensaje As String = "HOJA_GASTOS:El usuario " & Master.Ticket.NombreCompleto & " (" & Master.Ticket.IdUser & ") ha eliminado su hoja de gastos (" & myIdViaje & ")"
            log.Info(mensaje)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Hoja de gastos eliminada")
            Volver()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Subi el documento a la hoja de gastos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSubirDoc_Click(sender As Object, e As EventArgs) Handles btnSubirDoc.Click
        Try
            If (txtDocTitulo.Text = String.Empty Or Not fuDocumento.HasFile) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar todos los datos")
            ElseIf (fuDocumento.FileName.Length > 75) Then
                log.Warn("Hoja gastos documento: Se ha excedido el maximo numero de caracteres del fichero a subir (" & fuDocumento.FileName.Length & ")")
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La longitud maxima del nombre del fichero son 75 caracteres")
            Else
                Dim oDoc As New ELL.Viaje.DocumentoIntegrante With {.Documento = fuDocumento.FileBytes, .ContentType = fuDocumento.PostedFile.ContentType, .NombreFichero = fuDocumento.FileName, .IdViaje = IdViaje, .IdIntegrante = Master.Ticket.IdUser, .Titulo = txtDocTitulo.Text.Trim}
                Dim viajesBLL As New BLL.ViajesBLL
                Dim idDoc As Integer = viajesBLL.AddDocumentoIntegrante(oDoc)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Documento añadido")
                log.Info("Se ha subido el documento (" & idDoc & ") " & oDoc.Titulo & " a la HG " & IdHoja)
                mostrarDetalle()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado de viajes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Volver()
    End Sub

    ''' <summary>
    ''' Vuelve
    ''' </summary>    
    Private Sub Volver()
        If (Origen = "ValHG") Then
            Response.Redirect("..\Validacion\ValHojasGastos.aspx", False)
        ElseIf (Origen = "HGS") Then
            Response.Redirect("..\Financiero\HojasGastos.aspx?return=1", False)  'Es para que en la pagina de vuelta sepa que es un retorno
        ElseIf (Origen = "LIQ") Then
            Response.Redirect("..\Financiero\Liquidaciones\Liquidacion.aspx", False)
        ElseIf (Origen = "LIQ_FAC") Then
            Response.Redirect("..\Financiero\Liquidaciones\VerLiquidacionesFacturas.aspx", False)
        ElseIf (IdViaje And hasProfile(BLL.BidaiakBLL.Profiles.Financiero) And Not OrigenHG_Financiero) Then
            Response.Redirect("..\Financiero\Anticipos\DetalleAnticipo.aspx?idViaje=" & IdViaje, False)
        ElseIf (Origen = "VIA") Then
            Response.Redirect("Viajes.aspx", False)
        ElseIf (Origen = "ALER") Then
            Response.Redirect("..\Publico\Alertas.aspx", False)
        Else
            If (Request.QueryString("fFin") IsNot Nothing) Then
                Dim fecha As Date = New Date(CDec(Request.QueryString("fFin")))
                Response.Redirect("HojasGastosSinViaje.aspx?anno=" & fecha.Year, False)
            Else
                Response.Redirect("HojasGastosSinViaje.aspx", False)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Redirige a una pagina donde se realizan las transferencias a otro viaje
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnTransferir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTransferir.Click
        Response.Redirect("TransferenciaAnticipo.aspx?idHoja=" & IdHoja & "&orig=HG", False)
    End Sub

    ''' <summary>
    ''' Redirige al anticipo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVerAnticipo_Click(sender As Object, e As EventArgs) Handles btnVerAnticipo.Click
        Response.Redirect("~/Financiero/Anticipos/GestionAnticipos.aspx?idViaje=" & IdViaje, False)
    End Sub

#End Region

End Class