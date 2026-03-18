Imports BidaiakLib.ELL
Imports BidaiakLib.ELL.Viaje
Imports SabLib.ELL

Partial Public Class SolicitudViaje
    Inherits PageBase

#Region "Properties"

    Private bNotifInteg, bNotifAgencia, bNotifAdmin, bNotifGerente, liquidadorModificable As Boolean
    Private lCondicionesEspeciales As List(Of String()) 'Se utilizara para cargar de bbdd las condiciones. Si en una misma carga de pagina se tuvieran que hacer 3 consultas, solo se haría una y se guardaria en esta lista
    Public Const DIAS_ANTELACION_EUROPA_Y_NORTE_AFRICA As Integer = 7 'Dias minimos con lo que sería optimo planificar un viaje europeo
    Public Const DIAS_ANTELACION_RESTO_MUNDO As Integer = 15 'Dias minimos con lo que sería optimo planificar un viaje fuera de Europa

    ''' <summary>
    ''' Obtiene o establece la lista de integrantes de la solicitud
    ''' </summary>
    ''' Posiciones:0->IdSab,1->Nombre persona
    ''' <value></value>
    ''' <returns></returns>	
    Private Property Integrantes() As List(Of ELL.Viaje.Integrante)
        Get
            If (ViewState("Integrantes") Is Nothing) Then
                ViewState("Integrantes") = New List(Of ELL.Viaje.Integrante)
            End If
            Return CType(ViewState("Integrantes"), List(Of ELL.Viaje.Integrante))
        End Get
        Set(ByVal value As List(Of ELL.Viaje.Integrante))
            ViewState("Integrantes") = value
        End Set
    End Property

    ''' <summary>
    ''' Es para saber si viene del listado de viajes o no
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property OrigenViajes As Boolean
        Get
            If (ViewState("origV") Is Nothing) Then ViewState("origV") = True
            Return CType(ViewState("origV"), Boolean)
        End Get
        Set(value As Boolean)
            ViewState("origV") = value
        End Set
    End Property

#End Region

#Region "Page load e inicializaciones"

    ''' <summary>
    ''' Carga o inicializa una solicitud de viaje
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Planificacion de viaje"
                Dim bContinuar As Boolean = True
                Dim idViaje As Integer = Integer.MinValue
                If (Request.QueryString("Id") IsNot Nothing) Then
                    idViaje = (CInt(Request.QueryString("Id")))
                Else
                    If Not (hasProfile(BLL.BidaiakBLL.Profiles.Planificador, BLL.BidaiakBLL.Profiles.Financiero, BLL.BidaiakBLL.Profiles.Agencia, BLL.BidaiakBLL.Profiles.Administrador)) Then
                        bContinuar = False
                        log.Error("No tiene permisos para crear un nuevo viaje")
                    End If
                End If
                If (bContinuar) Then
                    mostrarDetalle(idViaje)
                    If (hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) Then
                        Try
                            OrigenViajes = (Page.Request.UrlReferrer.Segments(Page.Request.UrlReferrer.Segments.GetUpperBound(0)).ToString.ToLower = "viajes.aspx")
                        Catch ex As Exception
                            log.Warn("Error al averiguar en la solicitud de un viaje entrando desde financiero, si viene de la pagina de viajes o no")
                        End Try
                    End If
                Else
                    Response.Redirect("~/PermisoDenegado.aspx?mensa=3", False)
                End If
            End If
            If (btnGuardar.CommandArgument = String.Empty) Then
                btnGuardar.Text = itzultzaileWeb.Itzuli("Crear viaje")
            Else
                btnGuardar.Text = itzultzaileWeb.Itzuli("Modificar")
            End If
            searchUser.PlaceHolder = itzultzaileWeb.Itzuli("Nombre, apellidos, nombre de usuario o numero de trabajador")
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error en el page load de la solicitud de viajes", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("errCargarPagina")
        End Try
    End Sub

    ''' <summary>
    ''' En todos postback, hay que volver a seleccionar los valores del optionDropdown
    ''' Por alguna razon, pierde el elemento seleccionado en cada postback
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        Try
            If (Not Page.IsPostBack) Then
                itzultzaileWeb.Itzuli(labelDivCabDatosIni) : itzultzaileWeb.Itzuli(labelIdViaje) : itzultzaileWeb.Itzuli(labelFechSol)
                itzultzaileWeb.Itzuli(labelProp) : itzultzaileWeb.Itzuli(labelDestino)
                itzultzaileWeb.Itzuli(labelTipViaje) : itzultzaileWeb.Itzuli(cvTipoViaje) : itzultzaileWeb.Itzuli(labelDivCabInteg)
                itzultzaileWeb.Itzuli(labelFIda) : itzultzaileWeb.Itzuli(labelFV) : itzultzaileWeb.Itzuli(labelDescr)
                itzultzaileWeb.Itzuli(labelDivCabInteg) : itzultzaileWeb.Itzuli(labelEstadoViaje)
                itzultzaileWeb.Itzuli(labelDivCabAgencia) : itzultzaileWeb.Itzuli(labelInfoAgen) : itzultzaileWeb.Itzuli(labelEstado)
                itzultzaileWeb.Itzuli(labelConductor) : itzultzaileWeb.Itzuli(labelComen)
                itzultzaileWeb.Itzuli(labelDivCabProyecto) : itzultzaileWeb.Itzuli(labelUO) : itzultzaileWeb.Itzuli(cvUnidadOrg)
                itzultzaileWeb.Itzuli(rblConSinProyecto) : itzultzaileWeb.Itzuli(labelCliente) : itzultzaileWeb.Itzuli(labelProyecto)
                itzultzaileWeb.Itzuli(txtPorcentajeProyCli) : itzultzaileWeb.Itzuli(btnAddProyCli) : itzultzaileWeb.Itzuli(labelOF)
                itzultzaileWeb.Itzuli(rfvOF) : itzultzaileWeb.Itzuli(txtPorcentajeOF) : itzultzaileWeb.Itzuli(labelSubdivClient)
                itzultzaileWeb.Itzuli(btnAddOFTxt) : itzultzaileWeb.Itzuli(labelDivCabAnticipos) : itzultzaileWeb.Itzuli(labelEstAnti)
                itzultzaileWeb.Itzuli(labelFechaNec) : itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnVolver)
                itzultzaileWeb.Itzuli(ddlTipoViaje) : itzultzaileWeb.Itzuli(btnCancelar) : itzultzaileWeb.Itzuli(labelInfoCli)
                itzultzaileWeb.Itzuli(labelDocTitulo) : itzultzaileWeb.Itzuli(rfvDocTit) : itzultzaileWeb.Itzuli(labelDocAdj)
                itzultzaileWeb.Itzuli(btnSubirDoc) : itzultzaileWeb.Itzuli(lblTipoDesplaz) : itzultzaileWeb.Itzuli(lblTipoDesplaz)
                itzultzaileWeb.Itzuli(pnlCliente) : itzultzaileWeb.Itzuli(lblSinRegistros) : itzultzaileWeb.Itzuli(ddlUnidadOrg)
                itzultzaileWeb.Itzuli(revDescripcion) : itzultzaileWeb.Itzuli(btnAceptarModal) : itzultzaileWeb.Itzuli(labelCancelarModal)
                itzultzaileWeb.Itzuli(revComenAgencia) : itzultzaileWeb.Itzuli(labelSinProyectos) : itzultzaileWeb.Itzuli(labelIndicarComen)
                itzultzaileWeb.Itzuli(labelInfoFecha) : itzultzaileWeb.Itzuli(btnValidar) : itzultzaileWeb.Itzuli(labelTextoNoOkAnt1)
                itzultzaileWeb.Itzuli(labelTextoNoOkAnt2) : itzultzaileWeb.Itzuli(labelTextoNoOkAnt3) : itzultzaileWeb.Itzuli(labelTextoNoOkAnt4)
                itzultzaileWeb.Itzuli(lblTextoDiasAntelacionOk) : itzultzaileWeb.Itzuli(labelMensaGPS) : itzultzaileWeb.Itzuli(btnRechazar)
            End If

            Dim strKey As String = "changeFechaIScript"
            Dim strScript As String
            If Not Me.ClientScript.IsClientScriptBlockRegistered(Me.GetType(), strKey) Then
                strScript = "function changeFechasIda(sender,args) {if(confirm('" & itzultzaileWeb.Itzuli("¿Desea propagar la fecha de ida a todos los participantes?") & "'))" _
                                         & "document.getElementById('" & hfPropagar.ClientID & "').value = 1; else document.getElementById('" & hfPropagar.ClientID & "').value = 0; " _
                                         & "document.getElementById('" & hfIdFila.ClientID & "').value = sender._element.className; " _
                                         & ClientScript.GetPostBackEventReference(btnCambioFechasIda, String.Empty) & "}"
                Me.ClientScript.RegisterClientScriptBlock(Me.GetType(), strKey, strScript, True)
            End If
            strKey = "changeFechaVScript"
            If Not Me.ClientScript.IsClientScriptBlockRegistered(Me.GetType(), strKey) Then
                strScript = "function changeFechasVuelta(sender,args) {if(confirm('" & itzultzaileWeb.Itzuli("¿Desea propagar la fecha de vuelta a todos los participantes?") & "'))" _
                                        & "document.getElementById('" & hfPropagar.ClientID & "').value = 1; else document.getElementById('" & hfPropagar.ClientID & "').value = 0; " _
                                        & "document.getElementById('" & hfIdFila.ClientID & "').value = sender._element.className; " _
                                        & ClientScript.GetPostBackEventReference(btnCambioFechasVuelta, String.Empty) & "}"
                Me.ClientScript.RegisterClientScriptBlock(Me.GetType(), strKey, strScript, True)
            End If

            Dim ogdlActiv As WebControlsDropDown.OptionGroupDropDownList
            If (gvPersonas.Rows.Count > 0) Then
                For Each gvr As GridViewRow In gvPersonas.Rows
                    ogdlActiv = CType(gvr.FindControl("ogddlActiv"), WebControlsDropDown.OptionGroupDropDownList)
                    If (Request.Form(ogdlActiv.ClientID.Replace("_", "$")) IsNot Nothing) Then
                        ogdlActiv.SelectedValue = CInt(Request.Form(ogdlActiv.ClientID.Replace("_", "$")))
                    End If
                Next
            End If
            Dim script As New StringBuilder
            script.AppendLine("$('#dtFechaIda').datetimepicker({showClear:true,calendarWeeks:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
            script.AppendLine("$('#dtFechaVuelta').datetimepicker({showClear:true,calendarWeeks:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
            If (gvPersonas IsNot Nothing AndAlso gvPersonas.Rows.Count > 0) Then
                Dim dtFecha As HtmlGenericControl
                For Each gvItem As GridViewRow In gvPersonas.Rows
                    If (gvItem.RowType = DataControlRowType.DataRow) Then
                        dtFecha = DirectCast(gvItem.FindControl("dtFechaIntIda"), HtmlGenericControl)
                        script.AppendLine("$('#" & dtFecha.ClientID & "').datetimepicker({showClear:true,calendarWeeks:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'}).on('dp.change', function (e) { if(confirm('" & itzultzaileWeb.Itzuli("¿Desea propagar la fecha de ida a todos los participantes?") & "'))" _
                                        & "document.getElementById('" & hfPropagar.ClientID & "').value = 1; else document.getElementById('" & hfPropagar.ClientID & "').value = 0; " _
                                        & "document.getElementById('" & hfIdFila.ClientID & "').value = " & gvItem.RowIndex & ";" _
                                        & "document.getElementById('" & btnCambioFechasIda.ClientID & "').click();});")
                        dtFecha = DirectCast(gvItem.FindControl("dtFechaIntVuelta"), HtmlGenericControl)
                        script.AppendLine("$('#" & dtFecha.ClientID & "').datetimepicker({showClear:true,calendarWeeks:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'}).on('dp.change', function (e) { if(confirm('" & itzultzaileWeb.Itzuli("¿Desea propagar la fecha de vuelta a todos los participantes?") & "'))" _
                                        & "document.getElementById('" & hfPropagar.ClientID & "').value = 1; else document.getElementById('" & hfPropagar.ClientID & "').value = 0; " _
                                        & "document.getElementById('" & hfIdFila.ClientID & "').value = " & gvItem.RowIndex & ";" _
                                        & "document.getElementById('" & btnCambioFechasVuelta.ClientID & "').click();});")
                    End If
                Next
            End If
            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(Me, Me.GetType, "DatePicker", script.ToString, True)
            ''Si no se añade esto, el item.attributes, se borra con los postback
            'If (cblServAgencia.Items.Count > 0) Then
            '    Dim index As Integer = 0
            '    Dim esCoche As Boolean
            '    For Each item As ListItem In cblServAgencia.Items
            '        esCoche = False
            '        If (item.Value.Split("|")(1) = 1) Then esCoche = True
            '        item.Attributes.Add("onclick", "ServicioAgenciaCoche_Click(" & cblServAgencia.ClientID & "_" & index & "," & If(esCoche, 1, 0) & ");")
            '        index += 1
            '    Next
            'End If
        Catch ex As Exception
            log.Error("Error en la carga de la pagina", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa los controles del formulario
    ''' </summary>	
    ''' <param name="idViaje">Id del viaje</param>
    Private Sub inicializar(ByVal idViaje As Integer)
        'Datos iniciales
        lblEstadoViaje.Text = String.Empty
        txtDestino.Text = String.Empty : txtDestino.Visible = False
        txtDestino.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Especifique la ciudad o destino"))
        pnlValidacion.Visible = False : txtComentariosVal.Text = String.Empty
        cargarTarifasDestinos()
        cargarTiposViaje()
        txtFechaIda.Text = Now.ToShortDateString : txtFechaVuelta.Text = Now.ToShortDateString : lblFechaSolicitud.Text = Now.ToShortDateString
        txtDescripcionDatosIni.Text = String.Empty : lblPropietario.Text = String.Empty
        hfPropietario.Value = String.Empty : lblIdViaje.Text = String.Empty
        pnlIdViaje.Visible = False
        txtFechaIda.Enabled = True : spanFIdaCal.Visible = True
        txtFechaVuelta.Enabled = True : spanFVueltaCal.Visible = True
        hfIdLiq.Value = String.Empty
        cargarTiposDesplazamientos()
        pnlTipoDesplaz.Visible = False : pnlPlantasFiliales.Visible = False
        pnlCliente.Visible = False : pnlInfoUploadDocCli.Visible = False
        pnlDiasAntelacionOK.Visible = False : pnlDiasAntelacionNoOK.Visible = False
        pnlResulGerente.Visible = False
        txtDocTitulo.Text = String.Empty : lblSinRegistros.Visible = False
        rptDocumentosCliente.DataSource = Nothing : rptDocumentosCliente.DataBind()
        cargarPlantasFiliales()
        'Integrantes del viaje
        searchUser.Limpiar()
        txtFechaIda.Text = Now.ToShortDateString : txtFechaVuelta.Text = Now.ToShortDateString
        liquidadorModificable = True
        gvPersonas.DataSource = Nothing : gvPersonas.DataBind()
        pnlAddIntegrante.Visible = True : lblSinDpto.Text = String.Empty : pnlUserSinDpto.Visible = False
        'Datos agencia        
        pnlAgenciaDisabled.Visible = False : pnlComenAgencia.Visible = False
        cargarServiciosAgencia()
        txtComentariosAgencia.Text = String.Empty : pnlEstadoAgencia.Visible = False
        lblEstadoAgencia.Text = String.Empty
        pnlCocheAlquiler.Visible = False
        ddlConductores.DataSource = Nothing : ddlConductores.DataBind()
        'Proyecto
        pnlRequiereProyCli.Visible = False : pnlRequiereConSinProyecto.Visible = False
        pnlOF.Visible = False : pnlOFValidar.Visible = False
        rblConSinProyecto.SelectedIndex = -1
        txtOF.Text = String.Empty : txtOF.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Introducir el portador de coste completo"))
        pnlResulOFValidar.Visible = False : rfvOF.Enabled = False : pnlProyectos.Visible = False
        rptProyectos.DataSource = Nothing : rptProyectos.DataBind() : divSinProyectos.Visible = True
        btnAddOFTxt.Visible = False : btnAddProyCli.Visible = False
        txtPorcentajeOF.Visible = False : txtPorcentajeOF.Text = "0"
        txtPorcentajeProyCli.Visible = False : txtPorcentajeProyCli.Text = "0"
        cargarUnidadesOrg(Master.Ticket.IdDepartamento)
        'Anticipos
        hfIdEstAnti.Value = String.Empty
        pnlEstadoAnticipos.Visible = False
        lblEstadoAnticipo.Text = String.Empty
        pnlAnticiposDisabled.Visible = True : pnlAnticiposInfo.Visible = False : selImportes.Modificable = False
        calFechaNec.HabilitarCalendario = False : calFechaNec.Fecha = String.Empty
        Dim bEsPlanificador As Boolean = hasProfile(BLL.BidaiakBLL.Profiles.Planificador)
        'El panel de agencia solo sera no visible para el usuario financiero. El resto lo podran ver. Si a la vez es planificador, si lo vera
        pnlModuloAgencia.Visible = Not (hasProfile(BLL.BidaiakBLL.Profiles.Financiero) And Not bEsPlanificador)
        'El panel de anticipos solo sera no visible para el usuario de la agencia. El resto lo podran ver. Si a la vez es planificador, si lo vera
        pnlModuloAnticipos.Visible = Not (hasProfile(BLL.BidaiakBLL.Profiles.Agencia) And Not bEsPlanificador)
        labelAnticInfo.Text = itzultzaileWeb.Itzuli("El horario de entrega/devolucion de anticipos es de [HORA_I] a [HORA_F] y únicamente se entregaran/recibiran de forma excepcional, en horario de tarde").ToString.Replace("[HORA_I]", "10:30").Replace("[HORA_F]", "13:00")
        btnGuardar.Visible = True
        btnGuardar.CommandArgument = String.Empty
        If (idViaje = Integer.MinValue) Then
            btnGuardar.Text = itzultzaileWeb.Itzuli("Crear viaje")
            btnGuardar.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Solicitud de viaje"), itzultzaileWeb.Itzuli("¿Desea mandar la solicitud de viaje? Tenga en cuenta que la solicitud va a tener que ser validada por gerencia antes de que pueda empezar a ser tramitada"), String.Empty, "Save")
        Else
            btnGuardar.Text = itzultzaileWeb.Itzuli("Modificar")
        End If
        btnCancelar.Visible = (idViaje <> Integer.MinValue)
        btnCancelar.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Cancelacion de viaje"), itzultzaileWeb.Itzuli("¿Desea cancelar el viaje? Si continua, se avisara por email a los integrantes, a la agencia de viaje y a admnistracion si hubiera solicitado sus servicios"), String.Empty, "Cancel")
    End Sub

    ''' <summary>
    ''' Obtiene el numero de dias minimos necesarios para poder solicitar un anticipo
    ''' </summary>    
    Private Function getDiasSolicitudAnticipo() As Integer
        Dim bidaiakBLL As New BLL.BidaiakBLL
        Return bidaiakBLL.loadParameters(Master.IdPlantaGestion).DiasSolicitarAnticipo
    End Function

    ''' <summary>
    ''' Gestion los paneles a habilitar o deshabilitar dependiendo el perfil del usuario
    ''' </summary>
    ''' <param name="oViaje">Informacion del viaje</param>    
    ''' <param name="modAgen">Si se indica este campo, no se hara el calculo del principio para modificableAgencia</param>
    ''' <param name="modFinan">Si se indica este campo, no se hara el calculo del principio para modificableFinanciero</param>
    Private Sub gestionPerfiles(ByVal oViaje As ELL.Viaje, Optional ByVal modAgen As Nullable(Of Boolean) = Nothing, Optional ByVal modFinan As Nullable(Of Boolean) = Nothing)
        Dim bEsPropietario As Boolean = (Master.Ticket.IdUser = oViaje.IdUserSolicitador)
        Dim bEsLiquidador As Boolean = (oViaje.ResponsableLiquidacion IsNot Nothing AndAlso oViaje.ResponsableLiquidacion.Id = Master.Ticket.IdUser)
        Dim bModificarAsistentesYProyectos As Boolean = oViaje.Estado = ELL.Viaje.eEstadoViaje.Validado AndAlso (bEsPropietario Or hasProfile(BLL.BidaiakBLL.Profiles.Administrador)) 'Si el viaje esta pendiente de validacion, no se podrá tocar
        Dim modificableAgencia As Boolean = ((hasProfile(BLL.BidaiakBLL.Profiles.Agencia, BLL.BidaiakBLL.Profiles.Administrador)) _
                                             OrElse (oViaje.SolicitudAgencia IsNot Nothing AndAlso oViaje.SolicitudAgencia.Estado = ELL.SolicitudAgencia.EstadoAgencia.solicitado AndAlso bEsPropietario) _
                                             OrElse (oViaje.SolicitudAgencia Is Nothing AndAlso bEsPropietario)) AndAlso (oViaje.Estado = eEstadoViaje.Validado OrElse oViaje.Estado = eEstadoViaje.No_validado)
        Dim modificableFinanciero As Boolean = ((hasProfile(BLL.BidaiakBLL.Profiles.Financiero, BLL.BidaiakBLL.Profiles.Administrador)) _
                                              OrElse (oViaje.Anticipo IsNot Nothing AndAlso oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.solicitado AndAlso (bEsLiquidador Or bEsPropietario)) _
                                              OrElse (oViaje.Anticipo Is Nothing AndAlso (bEsPropietario Or bEsLiquidador))) AndAlso (oViaje.Estado = eEstadoViaje.Validado OrElse oViaje.Estado = eEstadoViaje.No_validado)
        If (oViaje.FechaVuelta.AddDays(5) < CDate(Now.ToShortDateString)) Then 'Una vez que pasen 5 dias de la finalizacion el viaje, no se podra modificar nada con excepciones
            bModificarAsistentesYProyectos = False
            If (Not (hasProfile(BLL.BidaiakBLL.Profiles.Financiero, BLL.BidaiakBLL.Profiles.Administrador))) Then modificableFinanciero = False
            If (Not (hasProfile(BLL.BidaiakBLL.Profiles.Agencia, BLL.BidaiakBLL.Profiles.Administrador))) Then modificableAgencia = False
        End If
        If (oViaje.Estado = ELL.Viaje.eEstadoViaje.Pendiente_validacion) Then
            Dim lIdUsersVal As List(Of String) = ConfigurationManager.AppSettings("validadoresViajes").ToString.Split(";").ToList
            pnlValidacion.Visible = lIdUsersVal.Exists(Function(o) o = Master.Ticket.IdUser)
        Else
            pnlValidacion.Visible = False
        End If
        If (modificableFinanciero AndAlso (bEsPropietario Or bEsLiquidador) AndAlso oViaje.Anticipo Is Nothing) Then   'si es propiertario o liquidador y no tiene un anticipo, habra que ver si la fecha cumple los dias para soliciarla
            Dim diasAntes As Integer = getDiasSolicitudAnticipo()
            Dim fIda, fVuelta As Date
            fIda = CDate(txtFechaIda.Text) : fVuelta = CDate(txtFechaVuelta.Text)
            If (fIda > fVuelta) Then
                fVuelta = fIda
                txtFechaVuelta.Text = txtFechaIda.Text
            End If
            Dim primeraFecha, ultimaFecha As Date
            Dim bPuedeCambiar As Boolean = False
            GestionFechasAnticipos(diasAntes, fIda, primeraFecha, ultimaFecha, bPuedeCambiar)
            If (Not bPuedeCambiar) Then modificableFinanciero = False
        ElseIf (modificableFinanciero AndAlso (bEsPropietario Or bEsLiquidador)) Then 'Hay que comprobar que si faltan menos de los dias estipulados para pedir un anticipo, para empezar el viaje, no se podra añadir ni quitar importes              
            Dim diasAntes As Integer = getDiasSolicitudAnticipo()
            modificableFinanciero = (oViaje.FechaIda.Subtract(New TimeSpan(diasAntes, 0, 0, 0)) > CDate(Now.ToShortDateString))
        End If
        'Si vienen informadas los parametros opcionales, se reemplaaran
        If (modAgen.HasValue) Then modificableAgencia = modAgen.Value
        If (modFinan.HasValue) Then modificableFinanciero = modFinan.Value
        'Datos iniciales
        txtDestino.Enabled = modificableAgencia
        ddlTipoViaje.Enabled = modificableAgencia
        ddlTipoDesplaz.Enabled = modificableAgencia
        ddlPais.Enabled = modificableAgencia
        Dim myControles As New List(Of Object)
        myControles.Add(New CheckBox With {.ID = "chbFilial"})
        DisableControlsRepeater(rptCheckFiliales, myControles, modificableAgencia)
        pnlInfoUploadDocCli.Visible = bEsPropietario Or hasProfile(BLL.BidaiakBLL.Profiles.Administrador)  '040413: Siempre podra subir documentos el organizador o el admin
        If (rptDocumentosCliente IsNot Nothing AndAlso Not (hasProfile(BLL.BidaiakBLL.Profiles.Agencia, BLL.BidaiakBLL.Profiles.Administrador) OrElse bEsPropietario)) Then
            myControles = New List(Of Object)
            myControles.Add(New LinkButton With {.ID = "lnkEliminar"})
            myControles.Add(New HyperLink With {.ID = "hkTitulo"})
            DisableControlsRepeater(rptDocumentosCliente, myControles)
        End If
        'Pasan a ser informativos. Una vez que se añada un asistente o se cree un viaje, no seran editables
        txtFechaIda.Enabled = False : txtFechaIda.Enabled = False
        spanFIdaCal.Visible = False : spanFVueltaCal.Visible = False
        txtDescripcionDatosIni.Enabled = (bEsPropietario Or hasProfile(BLL.BidaiakBLL.Profiles.Agencia, BLL.BidaiakBLL.Profiles.Administrador))
        If (btnCancelar.Visible) Then btnCancelar.Visible = (bEsPropietario Or hasProfile(BLL.BidaiakBLL.Profiles.Administrador, BLL.BidaiakBLL.Profiles.Agencia, BLL.BidaiakBLL.Profiles.Financiero))
        'Integrantes
        Dim lControles As New List(Of Object)
        pnlAddIntegrante.Visible = (modificableAgencia Or bModificarAsistentesYProyectos)
        If (Not modificableAgencia And Not bModificarAsistentesYProyectos) Then
            If (gvPersonas.Rows.Count > 0) Then  'Porque sino tuviera filas, fallaria
                If (Not modificableFinanciero) Then Dim rbt As New RadioButton : rbt.ID = "rbtLiq" : lControles.Add(rbt) 'EL FINANCIERO PODRA SELECCIONAR QUIEN ES EL LIQUIDADOR
                Dim lnk As New LinkButton : lnk.ID = "lnkElim" : lControles.Add(lnk)
                Dim txt As New TextBox : txt.ID = "txtObservacion" : lControles.Add(txt)
                Dim ogdl As New WebControlsDropDown.OptionGroupDropDownList : ogdl.ID = "ogddlActiv" : lControles.Add(ogdl)
                Dim chb As New CheckBox : chb.ID = "chbDesarraigado" : lControles.Add(chb)
                Dim drop As New DropDownList : drop.ID = "ddlPaP" : lControles.Add(drop)
                drop = New DropDownList : drop.ID = "ddlCondEsp" : lControles.Add(drop)
                txt = New TextBox : txt.ID = "txtFIda" : lControles.Add(txt)
                txt = New TextBox : txt.ID = "txtFVuelta" : lControles.Add(txt)
                DisableControlsGridview(gvPersonas, lControles)
            End If
        End If
        'Datos Agencia
        pnlAgenciaDisabled.Visible = (oViaje.SolicitudAgencia IsNot Nothing AndAlso oViaje.SolicitudAgencia.Estado <> ELL.SolicitudAgencia.EstadoAgencia.solicitado AndAlso oViaje.SolicitudAgencia.Estado <> ELL.SolicitudAgencia.EstadoAgencia.Tramite AndAlso Not (hasProfile(BLL.BidaiakBLL.Profiles.Agencia)))
        pnlEstadoAgencia.Visible = (oViaje.SolicitudAgencia IsNot Nothing)
        myControles = New List(Of Object)
        myControles.Add(New CheckBox With {.ID = "chbServicio"})
        DisableControlsRepeater(rptCheckFiliales, myControles, modificableAgencia)
        txtComentariosAgencia.Enabled = modificableAgencia
        pnlCocheAlquiler.Enabled = modificableAgencia
        'Proyecto
        ddlUnidadOrg.Enabled = (modificableAgencia Or bModificarAsistentesYProyectos)
        ddlProyecto.Enabled = (modificableAgencia Or bModificarAsistentesYProyectos)
        ddlCliente.Enabled = (modificableAgencia Or bModificarAsistentesYProyectos)
        rblConSinProyecto.Enabled = (modificableAgencia Or bModificarAsistentesYProyectos)
        txtOF.Enabled = (modificableAgencia Or bModificarAsistentesYProyectos)
        ddlOF.Enabled = (modificableAgencia Or bModificarAsistentesYProyectos)
        btnAddOFTxt.Visible = (modificableAgencia Or bModificarAsistentesYProyectos)
        btnAddProyCli.Visible = (modificableAgencia Or bModificarAsistentesYProyectos)
        txtPorcentajeOF.Visible = (modificableAgencia Or bModificarAsistentesYProyectos)
        txtPorcentajeProyCli.Visible = (modificableAgencia Or bModificarAsistentesYProyectos)
        lControles = New List(Of Object)
        If (rptProyectos.Items.Count > 0 And Not modificableAgencia And Not bModificarAsistentesYProyectos) Then  'Porque sino tuviera filas, fallaria            
            Dim txt As New TextBox
            txt.ID = "txtPorcentaje"
            lControles.Add(txt)
            Dim img As New ImageButton
            img.ID = "imgDelProy"
            lControles.Add(img)
            DisableControlsRepeater(rptProyectos, lControles)
        End If
        'Anticipos
        calFechaNec.HabilitarCalendario = modificableFinanciero
        pnlAnticiposDisabled.Visible = (((oViaje.Anticipo Is Nothing OrElse (oViaje.Anticipo IsNot Nothing AndAlso oViaje.Anticipo.Estado <> ELL.Anticipo.EstadoAnticipo.solicitado)) AndAlso oViaje.FechaIda >= CDate(Now.ToShortDateString) AndAlso Not modificableFinanciero) AndAlso Not hasProfile(BLL.BidaiakBLL.Profiles.Financiero))
        pnlAnticiposInfo.Visible = modificableFinanciero
        pnlEstadoAnticipos.Visible = (oViaje.Anticipo IsNot Nothing)
        If Not ((modificableFinanciero And (bEsPropietario Or hasProfile(BLL.BidaiakBLL.Profiles.Financiero, BLL.BidaiakBLL.Profiles.Administrador)))) Then
            Dim img As New ImageButton : img.ID = "imbQuitar" : lControles.Add(img)
            Dim txt As New TextBox : txt.ID = "txtObservacion" : lControles.Add(txt)
            Dim ogdl As New WebControlsDropDown.OptionGroupDropDownList : ogdl.ID = "ogddlActiv" : lControles.Add(ogdl)
        End If
        If (lControles.Count > 0) Then DisableControlsGridview(gvPersonas, lControles, modificableAgencia)
        selImportes.Modificable = modificableFinanciero
        selImportes.Anticipo = Not hasProfile(BLL.BidaiakBLL.Profiles.Financiero)  'Los de administracion, podran solicitar cualquier moneda. El resto, solo las que esten asignadas al anticipo
        selImportes.Inicializar()
        If (oViaje.Estado = ELL.Viaje.eEstadoViaje.Validado OrElse oViaje.Estado = eEstadoViaje.No_validado) Then
            If (hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) Then
                btnGuardar.Visible = modificableFinanciero
            ElseIf (hasProfile(BLL.BidaiakBLL.Profiles.Agencia)) Then
                btnGuardar.Visible = modificableAgencia
            Else
                btnGuardar.Visible = (bModificarAsistentesYProyectos Or modificableAgencia Or modificableFinanciero)
            End If
        Else
            btnGuardar.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Gestion los paneles de anticipos dependiendo del usuario
    ''' </summary>    
    Private Sub gestionPerfilesAnticipos()
        Dim idViaje As Integer = 0
        If (btnGuardar.CommandArgument <> String.Empty) Then idViaje = CInt(btnGuardar.CommandArgument)
        Dim diasAntes As Integer = getDiasSolicitudAnticipo()
        Dim fIda, fVuelta As Date
        fIda = CDate(txtFechaIda.Text) : fVuelta = CDate(txtFechaVuelta.Text)
        Dim primeraFecha, ultimaFecha As Date
        Dim bPuedeCambiar As Boolean = False
        GestionFechasAnticipos(diasAntes, fIda, primeraFecha, ultimaFecha, bPuedeCambiar)
        If (idViaje = 0) Then 'Viaje nuevo                        
            If (Not hasProfile(BLL.BidaiakBLL.Profiles.Financiero, BLL.BidaiakBLL.Profiles.Administrador)) Then  'El financiero no tendra filtros con la fecha de necesidad                
                pnlAnticiposDisabled.Visible = Not bPuedeCambiar
                pnlAnticiposInfo.Visible = bPuedeCambiar
                selImportes.Modificable = bPuedeCambiar
                calFechaNec.HabilitarCalendario = bPuedeCambiar
                calFechaNec.TodoElCalendarioDisponible = False
                If (Not bPuedeCambiar) Then
                    calFechaNec.Fecha = String.Empty
                    selImportes.Importes = Nothing
                End If
            Else
                pnlAnticiposDisabled.Visible = False
                pnlAnticiposInfo.Visible = True
                selImportes.Modificable = True
                calFechaNec.TodoElCalendarioDisponible = True
                calFechaNec.HabilitarCalendario = True
            End If
            selImportes.Anticipo = Not (hasProfile(BLL.BidaiakBLL.Profiles.Financiero))
            selImportes.Inicializar()
            calFechaNec.PrimeraFechaSeleccionable = primeraFecha
            calFechaNec.UltimaFechaSeleccionable = ultimaFecha
        Else
            Dim viajesBLL As New BLL.ViajesBLL
            Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(idViaje)
            Dim bEsPropietario As Boolean = (Master.Ticket.IdUser = oViaje.IdUserSolicitador)
            Dim bEsLiquidador As Boolean = (oViaje.ResponsableLiquidacion IsNot Nothing AndAlso oViaje.ResponsableLiquidacion.Id = Master.Ticket.IdUser)
            Dim modificableFinanciero As Boolean = ((hasProfile(BLL.BidaiakBLL.Profiles.Financiero, BLL.BidaiakBLL.Profiles.Administrador)) _
                                                  OrElse (oViaje.Anticipo IsNot Nothing AndAlso oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.solicitado AndAlso (bEsLiquidador Or bEsPropietario)) _
                                                  OrElse (oViaje.Anticipo Is Nothing AndAlso (bEsPropietario Or bEsLiquidador)))
            If (oViaje.FechaVuelta < CDate(Now.ToShortDateString)) Then 'Una vez finalizado el viaje, no se podra modificar nada
                modificableFinanciero = False
            End If
            If (modificableFinanciero AndAlso (bEsPropietario Or bEsLiquidador) AndAlso oViaje.Anticipo Is Nothing) Then   'si es propiertario o liquidador y no tiene un anticipo, habra que ver si la fecha cumple los dias para soliciarla
                If (Not bPuedeCambiar) Then modificableFinanciero = False
            End If
            calFechaNec.HabilitarCalendario = hasProfile(BLL.BidaiakBLL.Profiles.Financiero, BLL.BidaiakBLL.Profiles.Administrador)
            calFechaNec.PrimeraFechaSeleccionable = primeraFecha
            calFechaNec.UltimaFechaSeleccionable = ultimaFecha
            calFechaNec.TodoElCalendarioDisponible = modificableFinanciero
            pnlAnticiposDisabled.Visible = (((oViaje.Anticipo Is Nothing OrElse (oViaje.Anticipo IsNot Nothing AndAlso oViaje.Anticipo.Estado <> ELL.Anticipo.EstadoAnticipo.solicitado)) AndAlso oViaje.FechaIda >= CDate(Now.ToShortDateString) AndAlso Not modificableFinanciero) AndAlso Master.Perfil <> BLL.BidaiakBLL.Profiles.Financiero)
            pnlAnticiposInfo.Visible = modificableFinanciero
            pnlEstadoAnticipos.Visible = (oViaje.Anticipo IsNot Nothing)
            selImportes.Modificable = modificableFinanciero
            selImportes.Anticipo = Not (hasProfile(BLL.BidaiakBLL.Profiles.Financiero))  'Los de administracion, podran solicitar cualquier moneda. El resto, solo las que esten asignadas al anticipo
            selImportes.Inicializar()
        End If
    End Sub

    ''' <summary>
    ''' Deshabilita los controles del gridview de forma recursiva
    ''' </summary>
    Private Sub DisableControlsGridview(ByVal gv As GridView, ByVal controles As List(Of Object), Optional ByVal enabled As Boolean = False)
        For Each row As GridViewRow In gv.Rows
            DisableControlsGridview_Items(row, controles, enabled)
        Next
    End Sub

    ''' <summary>
    ''' Deshabilita los controles del repeater de forma recursiva
    ''' </summary>
    Private Sub DisableControlsRepeater(ByVal rpt As Repeater, ByVal controles As List(Of Object), Optional ByVal enabled As Boolean = False)
        For Each row As RepeaterItem In rpt.Items
            DisableControlsGridview_Items(row, controles, enabled)
        Next
    End Sub

    ''' <summary>
    ''' Deshabilita los controles del gridview de forma recursiva
    ''' </summary>
    ''' <param name="row">Control a comprobar</param>
    Private Sub DisableControlsGridview_Items(ByVal row As Control, ByVal controles As List(Of Object), ByVal enabled As Boolean)
        Dim nombreTipo As String
        Dim lObjetos As List(Of Object)
        For Each rContr As Control In row.Controls
            nombreTipo = rContr.GetType().Name
            lObjetos = controles.FindAll(Function(o As Object) o.GetType.Name = nombreTipo)
            If (lObjetos IsNot Nothing AndAlso lObjetos.Count > 0) Then
                For Each obj As Object In lObjetos
                    If (obj.Id = rContr.ID) Then
                        If (nombreTipo.ToLower = GetType(RadioButton).Name.ToLower) Then
                            CType(rContr, RadioButton).Enabled = enabled
                        ElseIf (nombreTipo.ToLower = GetType(TextBox).Name.ToLower) Then
                            CType(rContr, TextBox).Enabled = enabled
                        ElseIf (nombreTipo.ToLower = GetType(DropDownList).Name.ToLower) Then
                            CType(rContr, DropDownList).Enabled = enabled
                        ElseIf (nombreTipo.ToLower = GetType(WebControlsDropDown.OptionGroupDropDownList).Name.ToLower) Then
                            CType(rContr, WebControlsDropDown.OptionGroupDropDownList).Enabled = enabled
                        ElseIf (nombreTipo.ToLower = GetType(CheckBox).Name.ToLower) Then
                            CType(rContr, CheckBox).Enabled = enabled
                        ElseIf (nombreTipo.ToLower = GetType(HyperLink).Name.ToLower) Then
                            CType(rContr, HyperLink).Enabled = enabled
                        Else
                            rContr.Visible = enabled
                        End If
                    End If
                Next
            End If
            If (rContr.HasControls) Then DisableControlsGridview_Items(rContr, controles, enabled)
        Next
    End Sub

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
    Private Sub ShowModal(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True)
        End If
    End Sub

#End Region

#Region "Carga de datos y mostrar detalle solicitud"

    ''' <summary>
    ''' Carga las tarifas de destino
    ''' </summary>
    Private Sub cargarTarifasDestinos()
        If (ddlTarifaDestino.Items.Count = 0) Then
            ddlTarifaDestino.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
            Dim tarifaBLL As New BLL.TarifasServBLL
            Dim lTarifas As List(Of ELL.TarifaServicios) = tarifaBLL.loadTarifaList(New ELL.TarifaServicios, Master.IdPlantaGestion, True)
            lTarifas.Add(New ELL.TarifaServicios With {.Id = 0, .Destino = itzultzaileWeb.Itzuli("Resto")})
            lTarifas.Sort(Function(o1, o2) o1.Destino < o2.Destino)
            For Each oTarif As ELL.TarifaServicios In lTarifas
                ddlTarifaDestino.Items.Add(New ListItem(oTarif.Destino, oTarif.Id))
            Next
        End If
        ddlTarifaDestino.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Carga los tipos de viaje existentes
    ''' </summary>	
    Private Sub cargarTiposViaje()
        If (ddlTipoViaje.Items.Count = 0) Then
            ddlTipoViaje.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
            Dim texto As String
            For Each item As Integer In [Enum].GetValues(GetType(ELL.Viaje.eNivel))
                texto = [Enum].GetName(GetType(ELL.Viaje.eNivel), item).Replace("_", " ")
                ddlTipoViaje.Items.Add(New ListItem(texto, item))
            Next
        End If
        ddlTipoViaje.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Carga los tipos de desplazamientos
    ''' </summary>	
    Private Sub cargarPaisesTipoViaje(ByVal idTipoViaje As Integer)
        Dim viajesBLL As New BLL.ViajesBLL
        Dim lPaises As New List(Of ELL.Pais)

        ddlPais.Items.Clear()
        If (idTipoViaje = 0) Then
            ddlPais.DataSource = Nothing
            ddlPais.DataBind()
        Else
            lPaises = viajesBLL.GetPaisesTipoViaje(idTipoViaje)
            ddlPais.DataSource = lPaises
            ddlPais.DataBind()
            If (idTipoViaje <> ELL.Viaje.eNivel.Nacional) Then
                ddlPais.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), 0))
                ddlPais.SelectedValue = 0
            End If
        End If
    End Sub

    ''' <summary>
    ''' Carga los tipos de desplazamientos
    ''' </summary>	
    Private Sub cargarTiposDesplazamientos()
        If (ddlTipoDesplaz.Items.Count = 0) Then
            ddlTipoDesplaz.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), 0))
            Dim texto As String
            For Each item As Integer In [Enum].GetValues(GetType(ELL.Viaje.TipoDesplaz))
                If (item > 0) Then  'El item 0 no es seleccionable
                    texto = [Enum].GetName(GetType(ELL.Viaje.TipoDesplaz), item).Replace("_", " ")
                    ddlTipoDesplaz.Items.Add(New ListItem(texto, item))
                End If
            Next
        End If
        ddlTipoDesplaz.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Carga las unidades organizativas de la planta    
    ''' </summary>
    ''' <param name="idDep">Id del departamento del usuario</param>
    Private Sub cargarUnidadesOrg(ByVal idDep As String)
        If (ddlUnidadOrg.Items.Count = 0) Then
            Dim indexSelected, contador As Integer
            Dim oDep As New SabLib.ELL.Departamento With {.Id = idDep, .IdPlanta = Master.IdPlantaGestion}
            Dim depBLL As New SabLib.BLL.DepartamentosComponent
            oDep = depBLL.GetDepartamento(oDep)
            Dim unidadBLL As New BLL.UnidadOrgBLL
            Dim lUnidades As List(Of ELL.UnidadOrg) = unidadBLL.loadList(Master.IdPlantaGestion)
            indexSelected = 0 : contador = 0
            If (lUnidades IsNot Nothing AndAlso lUnidades.Count > 0) Then
                For Each unidad As ELL.UnidadOrg In lUnidades
                    ddlUnidadOrg.Items.Add(New ListItem(unidad.Nombre, unidad.Id))
                    If (oDep IsNot Nothing) Then
                        If (oDep.Nombre.StartsWith(unidad.DepartamentoIdentif)) Then indexSelected = contador
                    End If
                    contador += 1
                Next
            End If
            ddlUnidadOrg.SelectedIndex = indexSelected
        End If
    End Sub

    ''' <summary>
    ''' Carga los clientes
    ''' </summary>	
    ''' <param name="stringConexion">String de conexion de la bbdd donde se tiene que conectar</param> 
    Private Sub cargarClientes(ByVal stringConexion As String)
        If (ddlCliente.Items.Count = 0) Then
            Dim cliProyBLL As BLL.UO.IClienteProyecto = CType(New BLL.UO.CliProy_XBAT(), BLL.UO.IClienteProyecto)
            Dim lClientes As List(Of String()) = cliProyBLL.GetClientes(stringConexion)
            If (lClientes IsNot Nothing) Then lClientes = lClientes.OrderBy(Of String)(Function(o) o(1)).ToList
            ddlCliente.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
            For Each sCli In lClientes
                ddlCliente.Items.Add(New ListItem(sCli(1), sCli(0)))
            Next
        End If
        ddlCliente.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Carga los proyectos de un cliente
    ''' </summary>	
    ''' <param name="codCli">Codigo del cliente</param>
    ''' <param name="stringConexion">String de conexion de la bbdd donde se tiene que conectar</param> 
    Private Sub cargarProyectos(ByVal codCli As String, ByVal stringConexion As String)
        Dim cliProyBLL As BLL.UO.IClienteProyecto = CType(New BLL.UO.CliProy_XBAT(), BLL.UO.IClienteProyecto)
        Dim lProyectos As List(Of String()) = cliProyBLL.GetProyectos(codCli, stringConexion)
        If (lProyectos IsNot Nothing) Then lProyectos = lProyectos.OrderBy(Of String)(Function(o) o(1)).ToList
        ddlProyecto.Items.Clear()
        ddlProyecto.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
        For Each sProy In lProyectos
            ddlProyecto.Items.Add(New ListItem(sProy(1), sProy(0)))
        Next
        ddlProyecto.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Carga los servicios de agencia
    ''' </summary>	
    Private Sub cargarServiciosAgencia()
        Dim servAgenBLL As New BLL.ServicioDeAgenciaBLL
        Dim lServAgen As List(Of ELL.ServicioDeAgencia) = servAgenBLL.loadList(Master.IdPlantaGestion, True)
        Dim item As ListItem
        Dim esCoche As Boolean
        Dim lItems As New List(Of ListItem)
        For Each oServ As ELL.ServicioDeAgencia In lServAgen
            esCoche = False
            If (oServ.RequiereUsuario) Then esCoche = True
            item = New ListItem(oServ.Nombre, oServ.Id & "|" & If(esCoche, 1, 0))
            lItems.Add(item)
        Next
        rptCheckServAgen.DataSource = lItems
        rptCheckServAgen.DataBind()
    End Sub

    ''' <summary>
    ''' Carga las plantas filiales de los gerentes que esten informados
    ''' </summary>	
    Private Sub cargarPlantasFiliales()
        Dim bidaiakBLL As New BLL.BidaiakBLL
        Dim lGerentesPlantas As List(Of String()) = bidaiakBLL.loadGerentesPlantas() 'IdUser,idPlanta,Planta,NombreGerente
        If (lGerentesPlantas IsNot Nothing) Then
            'Nos quedamos con las plantas que tienen gerente asignado y que cuya planta no es la actual
            lGerentesPlantas = lGerentesPlantas.FindAll(Function(o As String()) CInt(o(1)) <> Master.IdPlantaGestion And Not String.IsNullOrEmpty(o(0)) AndAlso CInt(o(1)) <> 87)  '250220: Se quita a Brasil como planta filial
            lGerentesPlantas = lGerentesPlantas.OrderBy(Of String)(Function(o) o(2)).ToList
            rptCheckFiliales.DataSource = lGerentesPlantas
            rptCheckFiliales.DataBind()
            'For Each sGerente As String() In lGerentesPlantas
            '    chblFiliales.Items.Add(New ListItem(sGerente(2), sGerente(1)))
            'Next
        Else
            Throw New BatzException("No se ha encontrado ninguna planta filial a seleccionar", Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan las filiales
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptCheckFiliales_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptCheckFiliales.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item As String() = e.Item.DataItem
            Dim checkFilial As CheckBox = CType(e.Item.FindControl("chbFilial"), CheckBox)
            checkFilial.Text = item(2)
            checkFilial.Attributes.Add("value", item(1))
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los servicios de agencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptCheckServAgen_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptCheckServAgen.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item As ListItem = e.Item.DataItem
            Dim chbServ As CheckBox = CType(e.Item.FindControl("chbServicio"), CheckBox)
            chbServ.Text = item.Text
            chbServ.Attributes.Add("value", item.Value)
            Dim esCoche As Boolean = If(item.Value.Split("|")(1) = 1, True, False)
            chbServ.Attributes.Add("onclick", "ServicioAgenciaCoche_Click(" & chbServ.ClientID & "," & If(esCoche, 1, 0) & ");")  'cblServAgencia.Items.Count
        End If
    End Sub

    ''' <summary>
    ''' Carga la solicitud pasada como parametro
    ''' </summary>
    ''' <param name="idViaje">Id del viaje</param>	
    Private Sub mostrarDetalle(ByVal idViaje As Integer)
        Dim oViaje As ELL.Viaje = Nothing
        Dim viajeBLL As New BLL.ViajesBLL
        Dim primeraFecha As Date = Now.Date
        Dim ultimaFecha As Date = Now.Date
        Dim ofImproductiva As Boolean = False
        inicializar(idViaje)
        If (idViaje <> Integer.MinValue) Then
            oViaje = viajeBLL.loadInfo(idViaje)
            btnGuardar.CommandArgument = idViaje
            'Informacion
            pnlIdViaje.Visible = True
            lblIdViaje.Text = "V" & idViaje
            lblEstadoViaje.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Viaje.eEstadoViaje), oViaje.Estado).Replace("_", " "))
            Select Case oViaje.Estado
                Case ELL.Viaje.eEstadoViaje.Pendiente_validacion
                    lblEstadoViaje.CssClass = "label label-warning"
                Case ELL.Viaje.eEstadoViaje.Validado
                    lblEstadoViaje.CssClass = "label label-success"
                Case ELL.Viaje.eEstadoViaje.Cancelado, ELL.Viaje.eEstadoViaje.No_validado
                    lblEstadoViaje.CssClass = "label label-danger"
                Case Else
                    lblEstadoViaje.CssClass = "label label-default"
            End Select
            If (oViaje.IdTarifaDestino > 0) Then
                ddlTarifaDestino.SelectedValue = oViaje.IdTarifaDestino
            Else 'Si es 0 o integer.minvalue (antiguos), se muestra el destino   
                ddlTarifaDestino.SelectedValue = "0"
                txtDestino.Visible = True
                txtDestino.Text = oViaje.Destino
            End If
            'Se dejara modificar el destino mientras el presupuesto este en creado o el viaje no haya empezado
            If (oViaje.FechaIda <= Now.Date) Then
                ddlTarifaDestino.Enabled = False
                ddlTarifaDestino.ToolTip = itzultzaileWeb.Itzuli("No se puede modificar el destino porque el viaje ya ha comenzado")
            Else 'Se comprueba el estado del presupuesto
                Dim presupBLL As New BLL.PresupuestosBLL
                Dim oPresup As ELL.Presupuesto = presupBLL.loadInfo(idViaje)
                If (oPresup IsNot Nothing AndAlso oPresup.Estado <> ELL.Presupuesto.EstadoPresup.Creado) Then
                    ddlTarifaDestino.Enabled = False
                    ddlTarifaDestino.ToolTip = itzultzaileWeb.Itzuli("No se puede modificar el destino porque ya existe un presupuesto de agencia")
                End If
            End If
            ddlTipoViaje.SelectedValue = oViaje.Nivel
            cargarPaisesTipoViaje(oViaje.Nivel)
            If (oViaje.Pais <> Integer.MinValue) Then ddlPais.SelectedValue = oViaje.Pais
            txtFechaIda.Text = oViaje.FechaIda.ToShortDateString
            txtFechaVuelta.Text = oViaje.FechaVuelta.ToShortDateString
            lblFechaSolicitud.Text = oViaje.FechaSolicitud
            txtDescripcionDatosIni.Text = oViaje.Descripcion
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim oPropietario As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador}, False)
            lblPropietario.Text = oPropietario.NombreCompleto
            hfPropietario.Value = oPropietario.Id
            If (oViaje.ResponsableLiquidacion IsNot Nothing) Then hfIdLiq.Value = oViaje.ResponsableLiquidacion.Id
            Dim ofVisible, reqProyCli As Boolean
            ddlUnidadOrg.SelectedValue = oViaje.UnidadOrganizativa.Id
            pnlRequiereConSinProyecto.Visible = oViaje.UnidadOrganizativa.ReqConSinProyecto
            ofVisible = oViaje.UnidadOrganizativa.ReqOFImproductiva
            reqProyCli = False
            'Si requiere con o sin proyecto, habra que ver cual ha elegido. Dependiendo la eleccion, se mostrara el panel adecuado
            'If (oViaje.UnidadOrganizativa.ReqOFValidar) Then
            '    pnlOFTexto.Visible = True
            If (oViaje.UnidadOrganizativa.ReqConSinProyecto) Then
                If (oViaje.Proyectos IsNot Nothing AndAlso oViaje.Proyectos.Count > 0) Then
                    rblConSinProyecto.SelectedValue = 0
                    reqProyCli = True
                Else
                    rblConSinProyecto.SelectedValue = 1
                    reqProyCli = False
                    ofVisible = False
                End If
            Else
                reqProyCli = oViaje.UnidadOrganizativa.ReqProyCli
            End If
            pnlOF.Visible = ofVisible
            If (oViaje.UnidadOrganizativa.ReqProyCli) Then
                pnlRequiereProyCli.Visible = reqProyCli
            ElseIf (oViaje.UnidadOrganizativa.ReqOFValidar) Then
                pnlOFValidar.Visible = reqProyCli
            End If
            divSinProyectos.Visible = (oViaje.Proyectos Is Nothing OrElse (oViaje.Proyectos IsNot Nothing AndAlso oViaje.Proyectos.Count = 0))
            rptProyectos.DataSource = oViaje.Proyectos
            rptProyectos.DataBind()
            Dim bidaiakBLL As New BLL.BidaiakBLL
            If (oViaje.UnidadOrganizativa.ReqOFValidar) Then
                'pnlProyectos.Visible = True
            ElseIf (reqProyCli) Then
                cargarClientes(oViaje.UnidadOrganizativa.StringConexion)
                cargarProyectos(String.Empty, oViaje.UnidadOrganizativa.StringConexion)
                'pnlProyectos.Visible = True
            ElseIf (ofVisible) Then
            End If
            'Plantas filiales
            pnlTipoDesplaz.Visible = oViaje.TipoDesplazamiento <> ELL.Viaje.TipoDesplaz.Sin_especificar
            If (oViaje.TipoDesplazamiento = ELL.Viaje.TipoDesplaz.Plantas_Filiales) Then
                pnlPlantasFiliales.Visible = True : pnlResulGerente.Visible = True
                ddlTipoDesplaz.SelectedValue = ELL.Viaje.TipoDesplaz.Plantas_Filiales
                If (oViaje.SolicitudesPlantasFilial IsNot Nothing AndAlso oViaje.SolicitudesPlantasFilial.Count > 0) Then
                    Dim solPlantBLL As New BLL.BidaiakBLL
                    Dim oGerente As SabLib.ELL.Usuario
                    Dim planta As String
                    Dim myLabel As Label
                    Dim myImg As Image
                    Dim index As Integer = 0
                    Dim checksFiliales As List(Of CheckBox) = GetFilialChecks()
                    Dim checkFilial As CheckBox
                    For Each oSol As ELL.Viaje.SolicitudPlantaFilial In oViaje.SolicitudesPlantasFilial
                        checkFilial = checksFiliales.Find(Function(o) o.Attributes.Item("value") = oSol.IdPlantaFilial)
                        checkFilial.Checked = True
                        Dim myPanel As New Panel : myPanel.CssClass = "form-inline"
                        myLabel = New Label : myLabel.Style.Add("font-weight", "bold")
                        myImg = New Image With {.ImageUrl = ResolveUrl("~/App_Themes/Tema1/Images/info16.png")}
                        oGerente = solPlantBLL.loadGerentePlanta(oSol.IdPlantaFilial)
                        planta = checkFilial.Text
                        Select Case oSol.EstadoSolicitud
                            Case ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Solicitado
                                myLabel.Text = itzultzaileWeb.Itzuli("A la espera de aprobacion de la solicitud del gerente") & " '" & planta & "' => " & oGerente.NombreCompleto
                                myLabel.CssClass = "text-info"
                            Case ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Aprobada
                                myLabel.Text = itzultzaileWeb.Itzuli("Solicitud aprobada por el gerente") & " '" & planta & "' => " & oGerente.NombreCompleto
                                myLabel.CssClass = "text-success"
                                myImg.ToolTip = oSol.Observaciones
                            Case ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Rechazada
                                myLabel.Text = itzultzaileWeb.Itzuli("Solicitud rechazada por el gerente") & " '" & planta & "' => " & oGerente.NombreCompleto
                                myLabel.CssClass = "text-danger"
                                myImg.ToolTip = oSol.Observaciones
                            Case ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Sin_Solicitud
                                myLabel.Text = itzultzaileWeb.Itzuli("No es necesario la solicitud al gerente") & " '" & planta & "'"
                                myLabel.CssClass = "text-default"
                                myImg.ToolTip = String.Empty
                        End Select
                        myPanel.Controls.Add(myLabel)
                        If (myImg.ToolTip <> String.Empty) Then myPanel.Controls.Add(myImg)
                        myLabel = New Label
                        If (index = oViaje.SolicitudesPlantasFilial.Count - 1) Then 'En la ultima, solo se mete un espacio
                            myLabel.Text = "<br />"
                        Else
                            myLabel.Text = "<br /><br />"
                        End If
                        myPanel.Controls.Add(myLabel)
                        pnlResulGerente.Controls.Add(myPanel)
                        index += 1
                    Next
                End If
            ElseIf (oViaje.TipoDesplazamiento = ELL.Viaje.TipoDesplaz.Cliente Or oViaje.TipoDesplazamiento = ELL.Viaje.TipoDesplaz.Otros) Then  '05/06/13: Se añade lo de otros
                pnlCliente.Visible = True
                ddlTipoDesplaz.SelectedValue = oViaje.TipoDesplazamiento
                If ((oViaje.DocumentosCliente IsNot Nothing AndAlso oViaje.DocumentosCliente.Count > 0) OrElse
                (oViaje.DocumentosProyecto IsNot Nothing AndAlso oViaje.DocumentosProyecto.Count > 0)) Then
                    Dim lDocsCli As New List(Of ELL.Viaje.DocumentoCliente)
                    If (oViaje.DocumentosCliente IsNot Nothing) Then
                        lDocsCli = oViaje.DocumentosCliente
                    End If
                    If (oViaje.DocumentosProyecto IsNot Nothing) Then
                        Dim titDoc As String
                        For Each oDocProy As ELL.DocumentoProyecto In oViaje.DocumentosProyecto
                            titDoc = oDocProy.Descripcion & "(" & oDocProy.NombreProyecto & ")"
                            lDocsCli.Add(New ELL.Viaje.DocumentoCliente With {.Id = oDocProy.Id, .ContentType = oDocProy.ContentType, .Documento = oDocProy.Adjunto, .Titulo = titDoc})
                        Next
                    End If
                    rptDocumentosCliente.DataSource = lDocsCli
                    rptDocumentosCliente.DataBind()
                Else
                    lblSinRegistros.Visible = True
                End If
            End If
            'Integrantes
            txtFechaIda.Text = oViaje.FechaIda
            txtFechaVuelta.Text = oViaje.FechaVuelta
            If (oViaje.ListaIntegrantes IsNot Nothing AndAlso oViaje.ListaIntegrantes.Count > 0) Then oViaje.ListaIntegrantes = oViaje.ListaIntegrantes.OrderBy(Of String)(Function(o) o.Usuario.NombreCompleto).ToList
            Integrantes = oViaje.ListaIntegrantes
            If (oViaje.Anticipo IsNot Nothing) Then
                liquidadorModificable = (oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.solicitado OrElse oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.cancelada) 'Si el anticipo está solicitado, se podrá cambiar el liquidador. En caso contrario, no
            Else
                liquidadorModificable = True
            End If
            gvPersonas.DataSource = oViaje.ListaIntegrantes
            gvPersonas.DataBind()
            'Se comprueba si se ha solicitado agencia
            If (oViaje.SolicitudAgencia IsNot Nothing) Then
                txtComentariosAgencia.Text = oViaje.SolicitudAgencia.ComentariosUsuario
                Dim agenId As Integer
                Dim oServLine As ELL.SolicitudAgencia.Linea
                Dim checksServicios As List(Of CheckBox) = GetServiciosChecks()
                For Each item As CheckBox In checksServicios
                    agenId = CInt(item.Attributes.Item("value").Split("|")(0))
                    oServLine = oViaje.SolicitudAgencia.ServiciosSolicitados.Find(Function(o As ELL.SolicitudAgencia.Linea) If(o.ServicioAgencia IsNot Nothing, o.ServicioAgencia.Id = agenId, False))
                    item.Checked = (oServLine IsNot Nothing)
                    If (item.Checked AndAlso oServLine.ServicioAgencia.RequiereUsuario) Then  'habra que meterse unicamente cuando sea de tipo coche
                        pnlCocheAlquiler.Visible = True
                        ddlConductores.DataSource = oViaje.getUsuariosIntegrantes
                        ddlConductores.DataTextField = "NombreCompleto"
                        ddlConductores.DataValueField = "Id"
                        ddlConductores.DataBind()
                        ddlConductores.SelectedValue = oServLine.IdUserReq
                        labelMensaGPS.Visible = True
                    End If
                Next
                lblEstadoAgencia.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.SolicitudAgencia.EstadoAgencia), oViaje.SolicitudAgencia.Estado))
                Select Case oViaje.SolicitudAgencia.Estado
                    Case ELL.SolicitudAgencia.EstadoAgencia.solicitado
                        lblEstadoAgencia.CssClass = "label label-info"
                    Case ELL.SolicitudAgencia.EstadoAgencia.Tramite
                        lblEstadoAgencia.CssClass = "label label-primary"
                    Case ELL.SolicitudAgencia.EstadoAgencia.Gestionado
                        lblEstadoAgencia.CssClass = "label label-success"
                    Case ELL.SolicitudAgencia.EstadoAgencia.cancelada
                        lblEstadoAgencia.CssClass = "label label-danger"
                    Case ELL.SolicitudAgencia.EstadoAgencia.Cerrada
                        lblEstadoAgencia.CssClass = "label label-default"
                End Select
                Dim comentariosAgencia As String = oViaje.SolicitudAgencia.ComentariosDeLaAgencia
                If (comentariosAgencia <> String.Empty) Then
                    pnlComenAgencia.Visible = True
                    lblComenAgencia.Text = comentariosAgencia
                End If
            End If
            If (Not hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) Then  'El financiero no tendra filtros con la fecha de necesidad
                Dim diasAntes As Integer = getDiasSolicitudAnticipo()
                Dim bPuedeCambiar As Boolean = False
                GestionFechasAnticipos(diasAntes, oViaje.FechaIda, primeraFecha, ultimaFecha, bPuedeCambiar)
                calFechaNec.PrimeraFechaSeleccionable = primeraFecha
                calFechaNec.UltimaFechaSeleccionable = ultimaFecha
                If (Not bPuedeCambiar) Then
                    lblMensajeInfo.Text = itzultzaileWeb.Itzuli("No se pueden realizar peticiones ni modificaciones de anticipos los XX dias laborables antes del inicio del viaje. Si quisiera realizar algún cambio, pongase en contacto con administracion")
                    lblMensajeInfo.Text = lblMensajeInfo.Text.Replace("XX", diasAntes)
                End If
            End If
            'Se comprueba si se ha solicitado anticipos
            Dim idRespLiq As Integer = Integer.MinValue
            If (oViaje.Anticipo IsNot Nothing) Then
                'Esto es porque si no ha pedido un anticipo, pero se le ha hecho una transferencia, a la hora de guardar, al estar la fecha de necesidad informada pero sin anticipo solicitado, no dejara guardar
                If (oViaje.Anticipo.AnticiposSolicitados IsNot Nothing AndAlso oViaje.Anticipo.AnticiposSolicitados.Count > 0) Then calFechaNec.Fecha = oViaje.Anticipo.FechaNecesidad.ToShortDateString
                selImportes.Importes = oViaje.Anticipo.Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Solicitado)
                If (oViaje.Anticipo.Estado <> ELL.Anticipo.EstadoAnticipo.solicitado) Then
                    lblMensajeInfo.Text = itzultzaileWeb.Itzuli("En estos momentos, la solicitud de anticipos no se puede modificar ya que ha cambiado de estado. Si quisiera realizar algún cambio, pongase en contacto con administracion")
                End If
                lblEstadoAnticipo.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Anticipo.EstadoAnticipo), oViaje.Anticipo.Estado))
                hfIdEstAnti.Value = oViaje.Anticipo.Estado
                Select Case oViaje.Anticipo.Estado
                    Case ELL.Anticipo.EstadoAnticipo.solicitado
                        lblEstadoAnticipo.CssClass = "label label-info"
                    Case ELL.Anticipo.EstadoAnticipo.Preparado
                        lblEstadoAnticipo.CssClass = "label label-primary"
                    Case ELL.Anticipo.EstadoAnticipo.Entregado
                        lblEstadoAnticipo.CssClass = "label label-success"
                    Case ELL.Anticipo.EstadoAnticipo.cancelada
                        lblEstadoAnticipo.CssClass = "label label-danger"
                    Case ELL.Anticipo.EstadoAnticipo.cerrado
                        lblEstadoAnticipo.CssClass = "label label-default"
                End Select
                If (oViaje.ResponsableLiquidacion IsNot Nothing) Then idRespLiq = oViaje.ResponsableLiquidacion.Id
                rptTransfAnticipos.DataSource = oViaje.Anticipo.Movimientos.FindAll(Function(o) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia)
                rptTransfAnticipos.DataBind()
                'Cuando es detalle, no hay que avisarle que va a guardar.
                btnGuardar.OnClientClick = String.Empty
            End If
            'Si el viaje ya ha empezado, al cancelar el viaje no se mandaran emails
            If (oViaje.FechaIda <= CDate(Now.ToShortDateString)) Then btnCancelar.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Cancelacion de viaje"), itzultzaileWeb.Itzuli("¿Desea cancelar el viaje?"), String.Empty, "Cancel")
            gestionPerfiles(oViaje) 'Se habilitan o deshabilitan los controles dependiendo el perfil
            GestionPanelAnticipacion() 'Se gestiona el panel de anticipacion
        Else
            'Si es nueva, el propietario sera el mismo usuario
            lblPropietario.Text = Master.Ticket.NombreCompleto
            If (Master.Perfil <> BLL.BidaiakBLL.Profiles.Financiero) Then  'El financiero no tendra filtros con la fecha de necesidad
                Dim diasAntes As Integer = getDiasSolicitudAnticipo()
                lblMensajeInfo.Text = itzultzaileWeb.Itzuli("No se pueden realizar peticiones ni modificaciones de anticipos los XX dias laborables antes del inicio del viaje. Si quisiera realizar algún cambio, pongase en contacto con administracion")
                lblMensajeInfo.Text = lblMensajeInfo.Text.Replace("XX", diasAntes)
                selImportes.Modificable = False
                calFechaNec.UltimaFechaSeleccionable = ultimaFecha
            Else
                selImportes.Modificable = True
                calFechaNec.HabilitarCalendario = True
            End If
            selImportes.Anticipo = (Master.Perfil <> BLL.BidaiakBLL.Profiles.Financiero)
            selImportes.Inicializar()
            calFechaNec.PrimeraFechaSeleccionable = primeraFecha
            txtPorcentajeOF.Text = "100"
            txtPorcentajeProyCli.Text = "100"
            'Unidad organizativa            
            If (ddlUnidadOrg.SelectedValue = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No puede crear un viaje ya que en su planta no se han definido las unidades de organizacion")
            Else
                Dim unidadBLL As New BLL.UnidadOrgBLL
                Dim oUnidad As ELL.UnidadOrg = unidadBLL.load(ddlUnidadOrg.SelectedValue)
                pnlRequiereConSinProyecto.Visible = oUnidad.ReqConSinProyecto
                If (Not oUnidad.ReqConSinProyecto) Then
                    If (oUnidad.ReqProyCli) Then
                        pnlRequiereProyCli.Visible = True
                        cargarClientes(oUnidad.StringConexion)
                        cargarProyectos(String.Empty, oUnidad.StringConexion)
                        btnAddProyCli.Visible = True
                        txtPorcentajeProyCli.Visible = True
                        'pnlProyectos.Visible = True
                    ElseIf (oUnidad.ReqOFValidar) Then
                        pnlOFValidar.Visible = True
                        btnAddOFTxt.Visible = True
                        txtPorcentajeOF.Visible = True
                        'pnlProyectos.Visible = True
                    ElseIf (oUnidad.ReqOFImproductiva) Then
                        pnlOF.Visible = True
                        'cargarOFsImproductivas(oUnidad.Id)
                    End If
                End If
            End If
        End If
        pnlProyectos.Visible = (rblConSinProyecto.SelectedValue = "0")
    End Sub

    ''' <summary>
    ''' Gestion el texto a mostrar en el panel dados los dias de anticipacion
    ''' </summary>
    Private Sub GestionPanelAnticipacion()
        pnlDiasAntelacionOK.Visible = False : pnlDiasAntelacionNoOK.Visible = False
        If (Integrantes.Count > 0) Then
            Dim diasAntelacion As Integer
            Dim nivel As ELL.Viaje.eNivel
            Dim fIda, fSolicitud As Date
            fIda = CDate(txtFechaIda.Text) : fSolicitud = CDate(lblFechaSolicitud.Text)
            diasAntelacion = fIda.Subtract(fSolicitud).TotalDays
            nivel = CInt(ddlTipoViaje.SelectedValue)
            If (nivel >= 1) Then 'Se ha seleccionado el nivel            
                If (((nivel = ELL.Viaje.eNivel.Nacional OrElse nivel = ELL.Viaje.eNivel.Europa_y_norte_Africa) AndAlso (diasAntelacion >= DIAS_ANTELACION_EUROPA_Y_NORTE_AFRICA)) OrElse
                 (nivel = ELL.Viaje.eNivel.Resto_del_mundo AndAlso diasAntelacion >= DIAS_ANTELACION_RESTO_MUNDO)) Then
                    pnlDiasAntelacionOK.Visible = True
                Else 'No ok
                    pnlDiasAntelacionNoOK.Visible = True
                    labelTextoNoOkAnt3.Text = labelTextoNoOkAnt3.Text.Replace("[DIAS]", DIAS_ANTELACION_EUROPA_Y_NORTE_AFRICA)
                    labelTextoNoOkAnt4.Text = labelTextoNoOkAnt4.Text.Replace("[DIAS]", DIAS_ANTELACION_RESTO_MUNDO)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Comprueba el primer y ultimo dia seleccionable de los anticipos, dependiendo de las fechas del viaje
    ''' </summary>
    ''' <param name="diasAntes"></param>
    ''' <param name="FechaIda"></param>
    ''' <param name="primerFechaSel"></param>
    ''' <param name="ultimoFechaSel"></param>
    ''' <param name="sePuedeCambiar"></param>    
    Private Sub GestionFechasAnticipos(ByVal diasAntes As Integer, ByVal FechaIda As Date, ByRef primerFechaSel As Date, ByRef ultimoFechaSel As Date, ByRef sePuedeCambiar As Boolean)
        primerFechaSel = PrimeraFechaAnticiposSolicitable(diasAntes, CDate(Date.Now.ToShortDateString)) 'A partir del dia actual, tienen que pasar el minimo de dias para poder solicitar
        ultimoFechaSel = UltimaFechaAnticiposSolicitable(1, FechaIda) 'Podrán solicitar el dia antes laborable al viaje
        sePuedeCambiar = (primerFechaSel <= FechaIda And ultimoFechaSel < FechaIda)
    End Sub

    ''' <summary>
    ''' Comprueba la primera fecha en la que se puede solicitar un anticipo
    ''' </summary>
    ''' <param name="diasAntes">Indica cuantos dias laborables antes se puede solicitar o modificar un anticipo</param>
    ''' <param name="fechaIda">Fecha de ida del viaje</param>
    ''' <returns></returns>    
    Private Function PrimeraFechaAnticiposSolicitable(ByVal diasAntes As Integer, ByVal fechaIda As Date) As Date
        Try
            Dim fechaSolicitable As Date = fechaIda
            Dim esLaborable As Boolean
            Dim cont As Integer = 10 'Por seguridad, si pasa de 10 veces el bucle se saldra. Para prevenir que haga llamadas infinitas en caso de que el calendario no este bien, y todos devuelva no laborable
            Dim calComp As New RRHHLib.BLL.calendarioComponent
            While (diasAntes > 0)
                fechaSolicitable = fechaSolicitable.AddDays(1)
                calComp.tipoJornada(Session("TipoCalendario"), Session("Calendario"), fechaSolicitable, esLaborable)
                If (esLaborable) Then diasAntes -= 1
                cont -= 1
                If (cont = 0) Then Exit While
            End While
            Return fechaSolicitable
        Catch ex As Exception
            Throw New BidaiakLib.BatzException("Error al calcular la fecha de anticipo solicitable", ex)
        End Try
    End Function

    ''' <summary>
    ''' Comprueba la ultima fecha en la que se puede solicitar un anticipo
    ''' </summary>
    ''' <param name="diasAntes">Indica cuantos dias laborables antes se puede solicitar o modificar un anticipo</param>
    ''' <param name="fechaIda">Fecha de ida del viaje</param>
    ''' <returns></returns>    
    Private Function UltimaFechaAnticiposSolicitable(ByVal diasAntes As Integer, ByVal fechaIda As Date) As Date
        Try
            Dim fechaSolicitable As Date = fechaIda
            Dim cont As Integer = 10 'Por seguridad, si pasa de 10 veces el bucle se saldra. Para prevenir que haga llamadas infinitas en caso de que el calendario no este bien, y todos devuelva no laborable
            Dim esLaborable As Boolean
            Dim calComp As New RRHHLib.BLL.calendarioComponent
            While (diasAntes > 0)
                fechaSolicitable = fechaSolicitable.AddDays(-1)
                calComp.tipoJornada(Session("TipoCalendario"), CInt(Session("Calendario")), fechaSolicitable, esLaborable)
                If (esLaborable) Then diasAntes -= 1
                cont -= 1
                If (cont = 0) Then Exit While
            End While
            Return fechaSolicitable
        Catch ex As Exception
            Throw New BidaiakLib.BatzException("Error al calcular la fecha de anticipo solicitable", ex)
        End Try
    End Function

    ''' <summary>
    ''' Boton oculto que se ejecuta cuando se cambia la fecha de ida de un integrante
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCambioFechasIda_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCambioFechasIda.Click
        Try
            CambioFecha("I")
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cambiar las fechas")
        End Try
    End Sub

    ''' <summary>
    ''' Boton oculto que se ejecuta cuando se cambia la fecha de vuelta de un integrante
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCambioFechasVuelta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCambioFechasVuelta.Click
        Try
            CambioFecha("V")
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cambiar las fechas")
        End Try
    End Sub

    ''' <summary>
    ''' Gestiona las fechas del viaje y si tiene que mostrar o no los anticipos
    ''' </summary>
    ''' <param name="sFecha">Indica si viene a traves de la fecha de ida o de la de vuelta</param>
    Private Sub CambioFecha(sFecha As String)
        Dim fIda, fVuelta As Date
        Dim diasAntes As Integer = getDiasSolicitudAnticipo()
        Dim modifAgen, modifFinan, propagar As Boolean
        modifAgen = True : modifFinan = True
        propagar = If(hfPropagar.Value = "1", True, False)
        Dim idUser As Label = DirectCast(gvPersonas.Rows(CInt(hfIdFila.Value)).Controls(0).Controls(0), Label)
        Dim txtFIda As TextBox = DirectCast(gvPersonas.Rows(CInt(hfIdFila.Value)).Controls(5).Controls(1).Controls.Item(1), TextBox)
        Dim txtFVuelta As TextBox = DirectCast(gvPersonas.Rows(CInt(hfIdFila.Value)).Controls(6).Controls(1).Controls.Item(1), TextBox)
        fIda = CDate(txtFIda.Text)
        fVuelta = CDate(txtFVuelta.Text)
        If (fIda > fVuelta) Then
            fVuelta = fIda 'Si se pone una fecha de ida mayor que la fecha de vuelta, la fecha de vuelta, se iguala a la fecha de ida           
            txtFVuelta.Text = fVuelta.ToShortDateString
        End If
        'Se le cambia al integrante las fechas
        If (propagar) Then
            PropagaFecha(If(sFecha = "I", fIda, fVuelta), sFecha)
        Else
            Dim integr As ELL.Viaje.Integrante = Integrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = CInt(idUser.Text))
            integr.FechaIda = fIda : integr.FechaVuelta = fVuelta
            'Se comprueban las fechas maximas y minimas del viaje
            FechaMaximaYMinima(fIda, fVuelta)
        End If
        'Se cambian las fechas del viaje
        txtFechaIda.Text = fIda.ToShortDateString
        txtFechaVuelta.Text = fVuelta.ToShortDateString
        GestionPanelAnticipacion()
        upFechasViaje.Update()
        'Para saber si los anticipos se pueden habilitar, habra que consultar las fechas del viaje
        fIda = CDate(txtFechaIda.Text) : fVuelta = CDate(txtFechaVuelta.Text)
        If (Not hasProfile(BLL.BidaiakBLL.Profiles.Agencia) AndAlso (fIda < CDate(Now.ToShortDateString))) Then modifAgen = False
        gestionPerfilesAnticipos()
    End Sub

    ''' <summary>
    ''' Recoge la fecha minima y maxima de los integrantes
    ''' </summary>
    ''' <param name="fIdaMin">Donde se dejara la fecha minima</param>
    ''' <param name="fVueltaMax">Donde se dejara la fecha maxima</param>    
    Private Sub FechaMaximaYMinima(ByRef fIdaMin As Date, ByRef fVueltaMax As Date)
        fIdaMin = Date.MaxValue : fVueltaMax = Date.MinValue
        Dim fechaIda, fechaVuelta As Date
        For Each row As GridViewRow In gvPersonas.Rows
            fechaIda = CDate(DirectCast(row.Controls(5).Controls(1).Controls.Item(1), TextBox).Text)
            If (fechaIda < fIdaMin) Then fIdaMin = fechaIda
            fechaVuelta = CDate(DirectCast(row.Controls(6).Controls(1).Controls.Item(1), TextBox).Text)
            If (fechaVuelta > fVueltaMax) Then fVueltaMax = fechaVuelta
        Next
        If (fIdaMin = Date.MaxValue) Then
            fIdaMin = Now : fVueltaMax = Now
        End If
    End Sub

    ''' <summary>
    ''' Propaga la fecha a todos los participantes
    ''' </summary>
    ''' <param name="fecha">Indica la fecha a propagar</param>
    ''' <param name="sTipo">I:Ida, V:Vuelta</param>    
    Private Sub PropagaFecha(ByVal fecha As Date, ByVal sTipo As String)
        Dim txtFIda, txtFVuelta As TextBox
        Dim idUser As Integer
        Dim integr As ELL.Viaje.Integrante
        For Each row As GridViewRow In gvPersonas.Rows
            idUser = CInt(DirectCast(row.Controls(0).Controls(0), Label).Text)
            txtFIda = DirectCast(row.Controls(5).Controls(1).Controls.Item(1), TextBox)
            txtFVuelta = DirectCast(row.Controls(6).Controls(1).Controls.Item(1), TextBox)
            integr = Integrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = idUser)
            If (sTipo = "I") Then
                txtFIda.Text = fecha.ToShortDateString
                integr.FechaIda = fecha
            Else
                txtFVuelta.Text = fecha.ToShortDateString
                integr.FechaVuelta = fecha
            End If
        Next
    End Sub

    ''' <summary>
    ''' Evento lanzado desde javascript, para visualizar o no el panel de coches de alquiler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnAgenciaCoche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgenciaCoche.Click
        pnlCocheAlquiler.Visible = (hfAgencia.Value = "true")
        If (pnlCocheAlquiler.Visible) Then  'habra que meterse unicamente cuando sea de tipo coche            
            ddlConductores.Items.Clear()
            For Each Inte As ELL.Viaje.Integrante In Integrantes
                ddlConductores.Items.Add(New ListItem(Inte.Usuario.NombreCompleto, Inte.Usuario.Id))
            Next
            labelMensaGPS.Visible = True
            'chbConNavegador.Checked = False
        End If
    End Sub

    ''' <summary>
    ''' Si es nacional, solo mostrara las actividades no exentas
    ''' Si es otro valor, mostrara las actividades exentas y no exentas y se tendra que seleccionar el tipo de desplazamiento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlTipoViaje_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoViaje.SelectedIndexChanged
        Try
            Dim idTipoViaje As Integer = CInt(ddlTipoViaje.SelectedValue)
            pnlTipoDesplaz.Visible = (idTipoViaje <> Integer.MinValue AndAlso idTipoViaje <> ELL.Viaje.eNivel.Nacional)
            cargarPaisesTipoViaje(idTipoViaje)
            recargarDatosIntegrantes()
            GestionPanelAnticipacion()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Al cambiar este valor, se muestran o los paneles correspondientes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlTipoDesplaz_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoDesplaz.SelectedIndexChanged
        Dim idTipoDesplaz As ELL.Viaje.TipoDesplaz = CInt(ddlTipoDesplaz.SelectedValue)
        pnlPlantasFiliales.Visible = (idTipoDesplaz = ELL.Viaje.TipoDesplaz.Plantas_Filiales)
        pnlCliente.Visible = (btnGuardar.CommandArgument <> String.Empty AndAlso (idTipoDesplaz = ELL.Viaje.TipoDesplaz.Cliente OrElse idTipoDesplaz = ELL.Viaje.TipoDesplaz.Otros))
        'Solo podran subir ficheros los propietarios en la edicion o los que esten creando la solicitud
        pnlInfoUploadDocCli.Visible = (pnlCliente.Visible AndAlso (hfPropietario.Value = String.Empty OrElse (hfPropietario.Value = Master.Ticket.IdUser)))
    End Sub

    ''' <summary>
    ''' Si se selecciona el destino Resto, habrá que exigir que meta un texto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlTarifaDestino_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTarifaDestino.SelectedIndexChanged
        txtDestino.Visible = (CInt(ddlTarifaDestino.SelectedValue) = 0)
    End Sub

    ''' <summary>
    ''' Se recargan las posibles actividades que se hayan cargado en la tabla de integrantes indicando si es exenta o no si es un viaje al extranjero y sin indicar nada si es nacional
    ''' Se recargan las posibles condiciones especiales que se hayan cargado en la tabla de integrantes:Si es resto del mundo se visualizara condicion especial con id 1, sino no
    ''' </summary>    
    Private Sub recargarDatosIntegrantes()
        For Each row As GridViewRow In gvPersonas.Rows
            Dim lblIdSab As Label = DirectCast(row.FindControl("lblIdSab"), Label)
            Dim odlActividad As WebControlsDropDown.OptionGroupDropDownList = DirectCast(row.FindControl("ogddlActiv"), WebControlsDropDown.OptionGroupDropDownList)
            Dim chbDesarraigado As CheckBox = DirectCast(row.FindControl("chbDesarraigado"), CheckBox)
            Dim ddlCondEsp As DropDownList = DirectCast(row.FindControl("ddlCondEsp"), DropDownList)
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim activBLL As New BLL.ActividadesBLL
            Dim oUser As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(lblIdSab.Text)}, False)
            Dim lActiv As List(Of ELL.Actividad) = activBLL.loadListDpto(oUser.IdDepartamento, Master.IdPlantaGestion, 0)
            Dim oActiv As ELL.Actividad
            Dim bIndicarSiExento As Boolean = (CInt(ddlTipoViaje.SelectedValue) = ELL.Viaje.eNivel.Europa_y_norte_Africa Or CInt(ddlTipoViaje.SelectedValue) = ELL.Viaje.eNivel.Resto_del_mundo)
            For Each myItem In odlActividad.OptionGroups
                If (myItem.Items.count > 1) Then
                    For Each myActiv As ListItem In myItem.Items
                        oActiv = lActiv.Find(Function(o As ELL.Actividad) o.Id = CInt(myActiv.Value))
                        If (oActiv IsNot Nothing) Then
                            myActiv.Text = oActiv.Nombre
                            If (bIndicarSiExento) Then myActiv.Text &= " (" & If(oActiv.ExentaIRPF, itzultzaileWeb.Itzuli("Exenta"), itzultzaileWeb.Itzuli("No exenta")) & ")"
                        End If
                    Next
                End If
            Next
            If (chbDesarraigado.Checked) Then
                Dim idCondEspOld As Integer = ddlCondEsp.SelectedValue
                cargarCondicionesEspeciales(ddlCondEsp)
                'Se intenta asignar el mismo. Si tenía seleccionada la que ahora no esta, se seleccionara el primero
                ddlCondEsp.SelectedIndex = ddlCondEsp.Items.IndexOf(ddlCondEsp.Items.FindByValue(idCondEspOld))
            End If
        Next
    End Sub

#End Region

#Region "Unidad organizativa"

    ''' <summary>
    ''' Segun el valor elegido, hará visible uno u otro panel
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub ddlUnidadOrg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUnidadOrg.SelectedIndexChanged
        Try
            Dim unidadBLL As New BLL.UnidadOrgBLL
            Dim oUnidad As ELL.UnidadOrg = unidadBLL.load(ddlUnidadOrg.SelectedValue)
            pnlRequiereConSinProyecto.Visible = False : pnlRequiereProyCli.Visible = False : pnlOF.Visible = False
            pnlOFValidar.Visible = False : rfvOF.Enabled = False : btnAddOFTxt.Visible = False
            txtPorcentajeOF.Visible = False : btnAddProyCli.Visible = False : txtPorcentajeProyCli.Visible = False
            pnlProyectos.Visible = False : divSinProyectos.Visible = True
            rptProyectos.DataSource = Nothing : rptProyectos.DataBind()
            txtPorcentajeOF.Text = "100" : txtPorcentajeProyCli.Text = "100"
            If (oUnidad.ReqOFImproductiva) Then
                pnlOF.Visible = True
                'cargarOFsImproductivas(oUnidad.Id)
            ElseIf (oUnidad.ReqConSinProyecto) Then
                pnlRequiereConSinProyecto.Visible = True
                rblConSinProyecto.SelectedIndex = -1
            ElseIf (oUnidad.ReqProyCli) Then
                PrepararDatosProyectoCliente(oUnidad.StringConexion)
            ElseIf (oUnidad.ReqOFValidar) Then
                PrepararDatosOFValidar()
            End If
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
        End Try
    End Sub

    ''' <summary>
    ''' Carga el desplegable con los proyectos del cliente
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCliente.SelectedIndexChanged
        Try
            If (ddlCliente.SelectedValue = Integer.MinValue) Then
                ddlProyecto.Items.Clear()
                ddlProyecto.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Elegir un cliente"), Integer.MinValue))
            Else
                Dim unidadBLL As New BLL.UnidadOrgBLL
                Dim oUnidad As ELL.UnidadOrg = unidadBLL.load(ddlUnidadOrg.SelectedValue)
                cargarProyectos(ddlCliente.SelectedValue, oUnidad.StringConexion)
            End If
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Selecciona con o sin proyecto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rblConSinProyecto_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblConSinProyecto.SelectedIndexChanged
        Try
            Dim unidadBLL As New BLL.UnidadOrgBLL
            Dim oUnidad As ELL.UnidadOrg = unidadBLL.load(ddlUnidadOrg.SelectedValue)
            If (rblConSinProyecto.SelectedValue = 0) Then 'Con proyecto
                divSinProyectos.Visible = True
                rptProyectos.DataSource = Nothing : rptProyectos.DataBind()
                txtPorcentajeOF.Text = "100" : txtPorcentajeProyCli.Text = "100"
                If (oUnidad.ReqProyCli) Then
                    PrepararDatosProyectoCliente(oUnidad.StringConexion)
                ElseIf (oUnidad.ReqOFValidar) Then
                    PrepararDatosOFValidar()
                End If
            Else
                'Si elige sin proyecto, no se muestra nada            
            End If
            pnlRequiereProyCli.Visible = (oUnidad.ReqProyCli AndAlso rblConSinProyecto.SelectedValue = 0)
            pnlOFValidar.Visible = (oUnidad.ReqOFValidar AndAlso rblConSinProyecto.SelectedValue = 0)
            pnlOF.Visible = False
            btnAddProyCli.Visible = (rblConSinProyecto.SelectedValue = 0)
            txtPorcentajeProyCli.Visible = (rblConSinProyecto.SelectedValue = 0)
            pnlProyectos.Visible = (rblConSinProyecto.SelectedValue = 0)
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga los datos que se requieren cuando se elige un panel a seleccionar proyecto y cliente
    ''' Carga los clientes y dependiendo el cliente elegido, se mostraran sus proyectos
    ''' </summary>
    ''' <param name="stringConexion">String de conexion al que se tiene que conectar</param> 
    Private Sub PrepararDatosProyectoCliente(ByVal stringConexion As String)
        pnlRequiereProyCli.Visible = True
        cargarClientes(stringConexion)
        'Se libera 
        ddlProyecto.Items.Clear()
        ddlProyecto.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Elegir un cliente"), Integer.MinValue))
        btnAddProyCli.Visible = True
        txtPorcentajeProyCli.Visible = True
        pnlProyectos.Visible = True
    End Sub

    ''' <summary>
    ''' Prepara los controles para mostrar el panel de of de validacion
    ''' </summary>    
    Private Sub PrepararDatosOFValidar()
        pnlOFValidar.Visible = True : pnlResulOFValidar.Visible = False : rfvOF.Enabled = True
        btnAddOFTxt.Visible = True : txtPorcentajeOF.Visible = True : pnlProyectos.Visible = True
        txtOF.Text = String.Empty
    End Sub

    ''' <summary>
    ''' Consulta los datos de la of introducida
    ''' </summary>
    ''' <param name="numOf">Num de OF</param>
    Private Function consultarDatosOF(ByVal numOf As String) As String()
        Dim unidadBLL As New BLL.UnidadOrgBLL
        Dim ofBLL As BLL.UO.IOF = Nothing
        Dim oUnidad As ELL.UnidadOrg = unidadBLL.load(ddlUnidadOrg.SelectedValue)
        If (oUnidad.Sistema = "BRAIN") Then
            ofBLL = CType(New BLL.UO.OF_BRAIN(), BLL.UO.IOF)
        ElseIf (oUnidad.Sistema = "NAVISION") Then
            ofBLL = CType(New BLL.UO.OF_NAVISION(), BLL.UO.IOF)
        End If
        Dim sDatos As String() = ofBLL.consultarOF(oUnidad.StringConexion, numOf, Master.IdPlantaGestion)
        pnlResulOFValidar.Visible = True
        If (sDatos Is Nothing) Then
            lblOFValidacion.Text = itzultzaileWeb.Itzuli("OF no valida")
            pnlResulOFValidar.CssClass = "alert alert-danger"
        ElseIf (sDatos IsNot Nothing AndAlso sDatos(2).ToLower.Trim = "obs") Then
            lblOFValidacion.Text = itzultzaileWeb.Itzuli("OF obsoleta")
            pnlResulOFValidar.CssClass = "alert alert-warning"
        Else 'Cost carrier valido
            pnlResulOFValidar.Visible = False
            'lblOFValidacion.Text = itzultzaileWeb.Itzuli("OF valida")
            'pnlResulOFValidar.CssClass = "alert alert-success"
        End If
        Return sDatos
    End Function

    ''' <summary>
    ''' Se intenta añadir una OF de sistemas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnAddOFTxt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddOFTxt.Click
        Try
            Dim lProy As List(Of ELL.Viaje.Proyecto) = getDatosProyectos()
            Dim almacenado As Integer = lProy.Sum(Function(o As ELL.Viaje.Proyecto) o.Porcentaje)
            Dim porcentaje As Integer = If(txtPorcentajeOF.Text <> String.Empty, CInt(txtPorcentajeOF.Text), 0)
            If (porcentaje > 0 AndAlso (porcentaje + almacenado) <= 100) Then
                If (Not lProy.Exists(Function(o As ELL.Viaje.Proyecto) o.NumOF.ToLower = txtOF.Text.ToLower)) Then
                    If (txtOF.Text.Trim = String.Empty) Then
                        Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca la of")
                    Else
                        Dim sOF As String() = consultarDatosOF(txtOF.Text.Trim)
                        If (sOF IsNot Nothing AndAlso sOF(2).Trim.ToLower <> "obs") Then
                            lProy.Add(New ELL.Viaje.Proyecto With {.Porcentaje = porcentaje, .NumOF = txtOF.Text})
                            divSinProyectos.Visible = False
                            rptProyectos.DataSource = lProy
                            rptProyectos.DataBind()
                            Dim diferencia As Integer = 100 - (almacenado + porcentaje)
                            txtPorcentajeProyCli.Text = diferencia
                            txtPorcentajeOF.Text = diferencia
                            txtOF.Text = String.Empty
                        End If
                    End If
                Else
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El proyecto ya esta en la lista")
                End If
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El porcentaje del proyecto debe ser mayor que 0 y entre todos los añadidos, no debe sobrepasar el 100%")
            End If
        Catch ex As Exception
            log.Error("Error al añadir la OF en la solicitud de viaje", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al añadir el proyecto")
        End Try
    End Sub

    ''' <summary>
    ''' Se intenta añadir un proyecto con cliente
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnAddProyCli_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddProyCli.Click
        Try
            Dim lProy As List(Of ELL.Viaje.Proyecto) = getDatosProyectos()
            Dim almacenado As Integer = lProy.Sum(Function(o As ELL.Viaje.Proyecto) o.Porcentaje)
            Dim porcentaje As Integer = If(txtPorcentajeProyCli.Text <> String.Empty, CInt(txtPorcentajeProyCli.Text), 0)
            If (CInt(ddlProyecto.SelectedValue) <= 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un proyecto")
            ElseIf (porcentaje > 0 AndAlso (porcentaje + almacenado) <= 100) Then
                If (Not lProy.Exists(Function(o As ELL.Viaje.Proyecto) o.IdPrograma = CInt(ddlProyecto.SelectedValue))) Then
                    Dim unidadBLL As New BLL.UnidadOrgBLL
                    Dim oUnidad As ELL.UnidadOrg = unidadBLL.load(CInt(ddlUnidadOrg.SelectedValue))
                    Dim cliProyBLL As BLL.UO.IClienteProyecto = CType(New BLL.UO.CliProy_XBAT(), BLL.UO.IClienteProyecto)
                    Dim sProy As String() = cliProyBLL.GetProyecto(CInt(ddlProyecto.SelectedValue), oUnidad.StringConexion)
                    lProy.Add(New ELL.Viaje.Proyecto With {.Porcentaje = porcentaje, .IdPrograma = CInt(ddlProyecto.SelectedValue), .NumOF = sProy(3)})
                    divSinProyectos.Visible = False
                    rptProyectos.DataSource = lProy
                    rptProyectos.DataBind()
                    Dim diferencia As Integer = 100 - (almacenado + porcentaje)
                    txtPorcentajeProyCli.Text = diferencia
                    txtPorcentajeOF.Text = diferencia
                Else
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El proyecto ya esta en la lista")
                End If
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El porcentaje del proyecto debe ser mayor que 0 y entre todos los añadidos, no debe sobrepasar el 100%")
            End If
        Catch ex As Exception
            log.Error("Error al añadir el cliente/proyecto en la solicitud de viaje", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al añadir el proyecto")
        End Try
    End Sub

    ''' <summary>
    ''' Se pincha en la imagen de borrar
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>    
    Private Sub rptProyectos_ItemCommand(ByVal source As Object, ByVal e As RepeaterCommandEventArgs) Handles rptProyectos.ItemCommand
        Try
            Dim lProyectos As List(Of ELL.Viaje.Proyecto) = getDatosProyectos()
            lProyectos.RemoveAt(CInt(e.CommandArgument))
            divSinProyectos.Visible = (lProyectos.Count = 0)
            rptProyectos.DataSource = lProyectos
            rptProyectos.DataBind()
            Dim porcentaje As String = lProyectos.Sum(Function(o As ELL.Viaje.Proyecto) o.Porcentaje)
            txtPorcentajeOF.Text = 100 - porcentaje
            txtPorcentajeProyCli.Text = 100 - porcentaje
        Catch ex As Exception
            log.Error("Error al intentar quitar el proyecto de la solicitud de viaje", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al quitar el proyecto")
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rptProyectos_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptProyectos.ItemDataBound
        Try
            If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim oProy As ELL.Viaje.Proyecto = e.Item.DataItem
                Dim lblIdprograma As Label = CType(e.Item.FindControl("lblIdprograma"), Label)
                Dim lblNumOF As Label = CType(e.Item.FindControl("lblNumOF"), Label)
                Dim lblNombre As Label = CType(e.Item.FindControl("lblNombre"), Label)
                Dim txtPorcentaje As TextBox = CType(e.Item.FindControl("txtPorcentaje"), TextBox)
                Dim lnkDel As LinkButton = CType(e.Item.FindControl("lnkDel"), LinkButton)
                Dim unidadBLL As New BLL.UnidadOrgBLL
                Dim oUnidad As ELL.UnidadOrg = unidadBLL.load(ddlUnidadOrg.SelectedValue)
                If (oProy.IdPrograma <> Integer.MinValue) Then
                    Dim cliProyBLL As BLL.UO.IClienteProyecto = CType(New BLL.UO.CliProy_XBAT(), BLL.UO.IClienteProyecto)
                    Dim sProy As String() = cliProyBLL.GetProyecto(oProy.IdPrograma, oUnidad.StringConexion)
                    Dim sCli As String() = cliProyBLL.GetCliente(sProy(2), oUnidad.StringConexion)
                    lblIdprograma.Text = oProy.IdPrograma
                    lblNombre.Text = sCli(1) & "(" & sProy(1) & ")"
                    lblNumOF.Text = sProy(3)
                ElseIf (oProy.NumOF <> String.Empty) Then
                    Dim ofBLL As BLL.UO.IOF = Nothing
                    If (oUnidad.Sistema = "BRAIN") Then
                        ofBLL = CType(New BLL.UO.OF_BRAIN(), BLL.UO.IOF)
                    ElseIf (oUnidad.Sistema = "NAVISION") Then
                        ofBLL = CType(New BLL.UO.OF_NAVISION(), BLL.UO.IOF)
                    End If
                    lblNumOF.Text = oProy.NumOF
                    Dim myOF As String() = ofBLL.consultarOF(oUnidad.StringConexion, oProy.NumOF, Master.IdPlantaGestion)
                    If (myOF IsNot Nothing AndAlso myOF.Length > 1) Then lblNombre.Text = myOF(1)
                End If
                txtPorcentaje.Text = oProy.Porcentaje
                lnkDel.CommandArgument = e.Item.ItemIndex
            End If
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al cargar los proyectos de la solicitud de viaje", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los proyectos")
        End Try
    End Sub

    ''' <summary>
    ''' Recoge toda la informacion del repeater
    ''' </summary>
    ''' <returns></returns>    
    Private Function getDatosProyectos() As List(Of ELL.Viaje.Proyecto)
        Dim lProyectos As New List(Of ELL.Viaje.Proyecto)
        Dim oProy As ELL.Viaje.Proyecto
        Dim aux As String
        If (pnlProyectos.Visible) Then
            For Each rowItem As RepeaterItem In rptProyectos.Items
                oProy = New ELL.Viaje.Proyecto
                oProy.Descripcion = CType(rowItem.FindControl("lblNombre"), Label).Text
                aux = CType(rowItem.FindControl("lblIdPrograma"), Label).Text
                If (aux <> String.Empty) Then oProy.IdPrograma = CInt(aux)
                aux = CType(rowItem.FindControl("lblNumOF"), Label).Text
                If (aux <> String.Empty) Then oProy.NumOF = aux
                oProy.Porcentaje = CType(rowItem.FindControl("txtPorcentaje"), TextBox).Text
                lProyectos.Add(oProy)
            Next
        End If
        Return lProyectos
    End Function

#End Region

#Region "Botones"

    ''' <summary>
    ''' Al hacer click en el nombre a elegir, se genera este evento que lo añade a la pagina
    ''' </summary>
    ''' <param name="id">Id del usuario seleccionado</param>    
    Private Sub searchUser_ItemSeleccionado(id As Integer) Handles searchUser.ItemSeleccionado
        Try
            'Se comprueba que no este en la lista para que no se añada dos veces
            If (Integrantes.Exists(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = id)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El integrante ya estaba añadido")
            Else
                Dim tieneDeptos As Boolean
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim activBLL As New BLL.ActividadesBLL
                Dim oUser As New SabLib.ELL.Usuario With {.Id = id}
                oUser = userBLL.GetUsuario(oUser, False)
                Dim lActiv As List(Of ELL.Actividad) = activBLL.loadListDpto(oUser.IdDepartamento, Master.IdPlantaGestion, 0)
                tieneDeptos = (lActiv IsNot Nothing AndAlso lActiv.Count > 0)
                If (tieneDeptos) Then
                    pnlUserSinDpto.Visible = False
                    Integrantes.Add(New ELL.Viaje.Integrante With {.Usuario = oUser, .FechaIda = CDate(txtFechaIda.Text), .FechaVuelta = CDate(txtFechaVuelta.Text)})
                    hfIdLiq.Value = getLiquidadorSeleccionado()
                    gvPersonas.DataSource = Integrantes
                    gvPersonas.DataBind()
                    'Se añade la persona al desplegable
                    ddlConductores.DataSource = getUsuariosIntegrantes()
                    ddlConductores.DataTextField = "NombreCompleto"
                    ddlConductores.DataValueField = "Id"
                    ddlConductores.DataBind()
                    'Una vez que se haya añadido un participante, las fechas pasan a ser informativas                    
                    txtFechaIda.Enabled = False : txtFechaVuelta.Enabled = False
                    spanFIdaCal.Visible = False : spanFVueltaCal.Visible = False
                    GestionPanelAnticipacion()
                    upFechasViaje.Update()
                    gestionPerfilesAnticipos()
                Else
                    pnlUserSinDpto.Visible = True
                    lblSinDpto.Text = itzultzaileWeb.Itzuli("El departamento de [PERSONA] no tiene actividades asignadas y por tanto no se puede agregar al viaje. Se le ha comunicado a administracion para que registre las actividades").Replace("[PERSONA]", oUser.NombreCompleto)
                    'Se envia el email
                    Dim deptoBLL As New SabLib.BLL.DepartamentosComponent
                    Dim oDepto As SabLib.ELL.Departamento = deptoBLL.GetDepartamento(New SabLib.ELL.Departamento With {.Id = oUser.IdDepartamento, .IdPlanta = Master.IdPlantaGestion})
                    AvisarDepartamentoSinActividades(lblPropietario.Text, oUser.NombreCompleto, oDepto.Nombre.Trim, lblIdViaje.Text)
                End If
            End If
            searchUser.Limpiar()
            'Se tiene que hacer por script porque al estar fuera del updatepanel, no lo actualiza
            'Dim script As String = "var textbox=document.getElementById('" & txtNombrePersona.ClientID & "');" _
            '                      & "document.getElementById('" & id & "').value='';" _
            '                      & "textbox.value='';textbox.focus();"
            'ScriptManager.RegisterStartupScript(Page, Page.GetType, "resetear", script, True)
        Catch ex As Exception
            log.Error("Error al añadir una persona a la solicitud de viaje", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("errAñadir")
        End Try
    End Sub

    ''' <summary>
    ''' Comprueba si se ha seleccionado alguna filial
    ''' </summary>
    ''' <returns></returns>
    Private Function PlantasFilialesSeleccionadas() As Boolean
        Dim filialSeleccionada As Boolean = False
        For Each rptItem As RepeaterItem In rptCheckFiliales.Items
            If (CType(rptItem.FindControl("chbFilial"), CheckBox).Checked) Then
                filialSeleccionada = True
                Exit For
            End If
        Next
        Return filialSeleccionada
    End Function

    ''' <summary>
    ''' Devuelve los checks del repeater
    ''' </summary>
    ''' <returns></returns>
    Private Function GetFilialChecks() As List(Of CheckBox)
        Dim checks As New List(Of CheckBox)
        For Each rptItem As RepeaterItem In rptCheckFiliales.Items
            checks.Add(CType(rptItem.FindControl("chbFilial"), CheckBox))
        Next
        Return checks
    End Function

    ''' <summary>
    ''' Comprueba si se ha seleccionado alguna filial
    ''' </summary>
    ''' <returns></returns>
    Private Function ServiciosAgenciasSeleccionados() As Boolean
        Dim filialSeleccionada As Boolean = False
        For Each rptItem As RepeaterItem In rptCheckServAgen.Items
            If (CType(rptItem.FindControl("chbServicio"), CheckBox).Checked) Then
                filialSeleccionada = True
                Exit For
            End If
        Next
        Return filialSeleccionada
    End Function

    ''' <summary>
    ''' Devuelve los checks del repeater
    ''' </summary>
    ''' <returns></returns>
    Private Function GetServiciosChecks() As List(Of CheckBox)
        Dim checks As New List(Of CheckBox)
        For Each rptItem As RepeaterItem In rptCheckServAgen.Items
            checks.Add(CType(rptItem.FindControl("chbServicio"), CheckBox))
        Next
        Return checks
    End Function

    ''' <summary>
    ''' Pulsa el boton guardar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAceptarModal_Click(sender As Object, e As EventArgs) Handles btnAceptarModal.Click
        Select Case hfModalAction.Value
            Case "Save"
                GuardarViaje()
            Case "Cancel"
                CancelarViaje()
            Case "DelDocuClient"
                DeleteClientDocument(CInt(hfModalParam.Value))
            Case "Integrante"
                DeleteIntegrante(CInt(hfModalParam.Value))
        End Select
        ShowModal(False)
    End Sub

    ''' <summary>
    ''' Cuando se crea el viaje, se saca un pop up de confirmacion pero para las sucesivas veces no
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        GuardarViaje()
    End Sub

    ''' <summary>
    ''' Guarda los datos de la solicitud
    ''' </summary>	
    Private Sub GuardarViaje()
        Try
            Dim lProyectos As List(Of ELL.Viaje.Proyecto) = getDatosProyectos()
            If (CInt(ddlTarifaDestino.SelectedValue) < 0 OrElse (txtDestino.Visible AndAlso txtDestino.Text = String.Empty) OrElse ddlTipoViaje.SelectedValue = Integer.MinValue OrElse ddlUnidadOrg.SelectedValue = Integer.MinValue OrElse Integrantes.Count = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe rellenar todos los datos de las secciones A,B y D")
            ElseIf (btnGuardar.CommandArgument = String.Empty And CDate(txtFechaIda.Text) < CDate(Now.ToShortDateString)) Then  'Cuando es una nueva solicitud de viaje, no se podra solicitar un viaje para una fecha anterior al dia de hoy
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No se puede solicitar un viaje para una fecha pasada")
            ElseIf (CDate(txtFechaIda.Text) > CDate(txtFechaVuelta.Text)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La fecha de ida, debe ser menor o igual que la fecha de vuelta")
            ElseIf (selImportes.Importes IsNot Nothing AndAlso selImportes.Importes.Count > 0 AndAlso (getLiquidadorSeleccionado() = Integer.MinValue Or calFechaNec.Fecha = String.Empty)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar un responsable de liquidacion y/o introducir una fecha de necesidad")
            ElseIf (((selImportes.Importes IsNot Nothing AndAlso selImportes.Importes.Count = 0) OrElse (selImportes Is Nothing)) AndAlso (calFechaNec.Fecha <> String.Empty)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca cantidades de anticipos o quite la fecha de necesidad")
            ElseIf ((pnlRequiereConSinProyecto.Visible AndAlso rblConSinProyecto.SelectedIndex = -1) OrElse (pnlOF.Visible AndAlso ddlOF.SelectedIndex = 0) _
                    OrElse (pnlProyectos.Visible AndAlso lProyectos.Count = 0 AndAlso rblConSinProyecto.SelectedValue = 0)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar los datos de la seccion de proyectos")
            ElseIf (pnlProyectos.Visible AndAlso rblConSinProyecto.SelectedValue = 0 AndAlso lProyectos.Sum(Function(o As ELL.Viaje.Proyecto) o.Porcentaje) <> 100) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La suma del porcentaje de los proyectos debe ser el 100%")
            ElseIf (Not RellenarIntegrantes()) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Rellene las actividades o introduzca unas fechas validas de los integrantes del viaje")
            ElseIf (CInt(ddlTipoDesplaz.SelectedValue) = ELL.Viaje.TipoDesplaz.Plantas_Filiales AndAlso Not PlantasFilialesSeleccionadas()) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione alguna planta filial")
            ElseIf (pnlTipoDesplaz.Visible AndAlso CInt(ddlTipoDesplaz.SelectedValue) = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un tipo de desplazamiento")
            ElseIf (pnlTipoDesplaz.Visible AndAlso CInt(ddlPais.SelectedValue) = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un país")
            ElseIf (txtComentariosAgencia.Text.Length > 350) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Se ha excedido el numero de caracteres de los comentarios de agencia") & " : " & txtComentariosAgencia.Text.Length & " / 350"
            ElseIf (txtDescripcionDatosIni.Text.Length > 500) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Se ha excedido el numero de caracteres de la descripcion") & " : " & txtComentariosAgencia.Text.Length & " / 500"
            ElseIf (txtDescripcionDatosIni.Text.Length < 20) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe justificar claramente la necesidad del viaje") & ". " & itzultzaileWeb.Itzuli("Use mas de 20 caracteres")
            ElseIf (Not Page.IsValid) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Corriga los errores marcados")
            Else
                Dim viajeBLL As New BLL.ViajesBLL
                Dim oViaje As ELL.Viaje = Nothing
                Dim oViajeOld As ELL.Viaje = Nothing
                Dim ocupados As String = String.Empty
                Dim lIntNew, lIntOld, lIntUpdate As List(Of SabLib.ELL.Usuario)
                Dim lPlFilialesNew, lPlFilialesOld As List(Of Integer)
                Dim bAvisarAgencia, bAvisarFinanciero, bAvisarIntegrantes, bAvisarGerentes As Boolean
                bAvisarAgencia = False : bAvisarFinanciero = False : bAvisarIntegrantes = False : bAvisarGerentes = False
                lIntNew = Nothing : lIntOld = Nothing : lIntUpdate = Nothing : lPlFilialesNew = Nothing : lPlFilialesOld = Nothing
                Dim lFreeBusy As List(Of Integer()) = viajeBLL.freeBusyIntegrantes(If(btnGuardar.CommandArgument <> String.Empty, CInt(btnGuardar.CommandArgument), Integer.MinValue), getUsuariosIntegrantesFechas(), Master.IdPlantaGestion)
                Dim lBusy As List(Of Integer()) = lFreeBusy.FindAll(Function(o As Integer()) o(1) = 1)
                If (lBusy IsNot Nothing AndAlso lBusy.Count > 0) Then
                    Dim idBusy As Integer
                    For Each iBusy As Integer() In lBusy
                        idBusy = iBusy(0)
                        If (iBusy(1) = 1) Then
                            If (ocupados <> String.Empty) Then ocupados &= ","
                            ocupados &= Integrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = idBusy).Usuario.NombreCompleto
                        End If
                    Next
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No se puede guardar porque existen usuarios que estan en otro viaje en esas fechas") & ":" & ocupados
                    Exit Sub
                End If
                Dim mensaAdv As String = String.Empty
                oViaje = RecogerInfoViaje((btnGuardar.CommandArgument = String.Empty), oViajeOld, bAvisarAgencia, bAvisarFinanciero, lIntNew, lIntOld, lIntUpdate, bAvisarGerentes, lPlFilialesNew, lPlFilialesOld, lProyectos, mensaAdv)
                If (mensaAdv = String.Empty) Then
                    Dim idViaje As Integer = viajeBLL.Save(oViaje, Master.Ticket.IdUser)
                    oViaje.IdViaje = idViaje
                    Dim bVerResultado As Boolean = False
                    If (idViaje <> Integer.MinValue) Then
                        If (btnGuardar.CommandArgument = String.Empty) Then
                            Dim mensa As String = "SOLICITUD_VIAJE:El usuario " & Master.Ticket.NombreCompleto & " (" & Master.Ticket.IdUser & ") "
                            mensa &= "ha generado la solicitud de viaje [" & idViaje & "] para el dia " & oViaje.FechaIda.ToShortDateString & " a " & oViaje.Destino & " que esta PENDIENTE DE VALIDAR"
                            If (hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) Then
                                mensa &= ".El viaje no es programado. Se ha creado por parte del departamento financiero como un viaje con Anticipo"
                            ElseIf (hasProfile(BLL.BidaiakBLL.Profiles.Agencia)) Then
                                mensa &= ".Se ha creado por parte de la agencia de viajes"
                            End If
                            If (oViaje.SolicitudAgencia IsNot Nothing OrElse oViaje.Anticipo IsNot Nothing OrElse oViaje.SolicitudesPlantasFilial IsNot Nothing) Then
                                mensa &= "("
                                Dim servicios As String = String.Empty
                                If (oViaje.SolicitudAgencia IsNot Nothing) Then servicios = "Solicitud de agencia"
                                If (oViaje.Anticipo IsNot Nothing) Then
                                    If (servicios <> String.Empty) Then servicios &= " y "
                                    servicios &= "Anticipo"
                                End If
                                If (oViaje.SolicitudesPlantasFilial IsNot Nothing) Then
                                    If (servicios <> String.Empty) Then servicios &= " y "
                                    servicios &= "Solicitud de planta"
                                End If
                                mensa &= servicios & ")"
                            End If
                            log.Info(mensa)
                        Else
                            WriteChanges(oViaje, oViajeOld)
                        End If
                        bVerResultado = True
                    Else
                        Dim accion As String = "creando"
                        If (oViaje.IdViaje <> Integer.MinValue) Then accion = "modificando"
                        log.Error("Ha ocurrido un error " & accion & " la solicitud del viaje")
                        Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
                    End If
                    If (oViaje.Estado = ELL.Viaje.eEstadoViaje.Validado) Then 'Solo se manda el email cuando es una modificacion. Cuando se crea se queda en estado PENDIENTE DE VALIDACION
                        AvisarPorEmail(oViaje, bAvisarAgencia, bAvisarFinanciero, lIntNew, lIntOld, lIntUpdate, bAvisarGerentes, lPlFilialesNew, lPlFilialesOld, (btnGuardar.CommandArgument = String.Empty), False)
                    ElseIf (oViaje.Estado = ELL.Viaje.eEstadoViaje.Pendiente_validacion) Then
                        AvisarPorEmailValidacionViaje(oViaje)
                    End If
                    If (bVerResultado) Then VerResultado(idViaje) 'Muestra en caso necesario el resultado de la ejecucion
                Else
                    Master.MensajeAdvertencia = mensaAdv
                End If
            End If
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al guardar la solicitud de viaje", ex)
            Master.MensajeError = "Error al guardar la solicitud de viaje"
        End Try
    End Sub

    ''' <summary>
    ''' Cancela el viaje avisando a todas las partes afectadas
    ''' </summary>
    Private Sub CancelarViaje()
        Try
            Dim viajeBLL As New BLL.ViajesBLL
            log.Info("CANCELACION_VIAJE: Se va a cancelar el viaje " & btnGuardar.CommandArgument)
            Dim oViaje As ELL.Viaje = viajeBLL.loadInfo(CInt(btnGuardar.CommandArgument))
            viajeBLL.Delete(oViaje.IdViaje, Master.Ticket.IdUser, (oViaje.SolicitudAgencia IsNot Nothing), (oViaje.Anticipo IsNot Nothing))
            AvisarCancelacionPorEmail(oViaje)
            log.Info("CANCELACION_VIAJE: El viaje " & oViaje.IdViaje & " ha sido cancelado")
            Response.Redirect("Viajes.aspx", False)
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al cancelar el viaje" & btnGuardar.CommandArgument, ex)
            Master.MensajeError = "Error al cancelar el viaje" & btnGuardar.CommandArgument
        End Try
    End Sub

    ''' <summary>
    ''' Redirige a una pagina de resumen de modificacion del viaje
    ''' </summary>
    ''' <param name="idViaje">Id del viaje</param>
    Private Sub VerResultado(ByVal idViaje As Integer)
        Response.Redirect("InfoViaje.aspx?id=" & idViaje & "&nIn=" & bNotifInteg & "&nAg=" & bNotifAgencia & "&nFi=" & bNotifAdmin & "&nGe=" & bNotifGerente & "&ov=" & OrigenViajes, False)
    End Sub

    ''' <summary>
    ''' Recoge la informacion del viaje del formulario
    ''' </summary>
    ''' <param name="bViajeNew">Indica si el viaje es nuevo o se ha validado y hay que avisar a todos</param>
    ''' <param name="oViajeOld">Viaje antiguo</param>
    ''' <param name="bAvisarAgencia">Indicara si hay que avisar a la agencia</param>
    ''' <param name="bAvisarFinanciero">Indicara si hay que avisar al financiero si es un nuevo anticipo o se ha realizado alguna modificacion</param>    
    ''' <param name="lIntNew">Lista de integrantes añadidos al viaje</param>
    ''' <param name="lIntOld">Lista de integrantes quitados del viaje</param>
    ''' <param name="lIntUpdate">Lista de integrantes actualizados</param>
    ''' <param name="bAvisarGerentes">Indicara si hay que avisar a los gerentes por una solicitud de planta filial</param>
    ''' <param name="lPlFilialesNew">Lista de plantas filiales añadidas al viaje</param>
    ''' <param name="lPlFilialesOld">Lista de plantas filiales quitadas del viaje</param>    
    ''' <param name="lProyectos">Lista de proyectos. Si no viene informada, se vuelve a recoger</param>
    ''' <param name="mensaAdv">Mensaje de advertencia</param>
    ''' <returns></returns>    
    Private Function RecogerInfoViaje(ByRef bViajeNew As Boolean, ByRef oViajeOld As ELL.Viaje, ByRef bAvisarAgencia As Boolean, ByRef bAvisarFinanciero As Boolean, ByRef lIntNew As List(Of SabLib.ELL.Usuario), ByRef lIntOld As List(Of SabLib.ELL.Usuario), ByRef lIntUpdate As List(Of SabLib.ELL.Usuario), ByRef bAvisarGerentes As Boolean, ByRef lPlFilialesNew As List(Of Integer), ByRef lPlFilialesOld As List(Of Integer), Optional ByVal lProyectos As List(Of ELL.Viaje.Proyecto) = Nothing, Optional ByRef mensaAdv As String = "") As ELL.Viaje
        Try
            Dim viajeBLL As New BLL.ViajesBLL
            Dim oViaje As New ELL.Viaje
            If (lProyectos Is Nothing) Then lProyectos = getDatosProyectos()
            Dim bActividadesExentas As Boolean = False
            Dim filialesSeleccionadas As Boolean = False
            Dim fSolicitud As DateTime = DateTime.MinValue
            Dim myIdViaje As Integer = 0
            If (Not bViajeNew AndAlso btnGuardar.CommandArgument <> String.Empty) Then myIdViaje = CInt(btnGuardar.CommandArgument)
            If (myIdViaje > 0) Then
                Dim idUser As Integer
                oViaje.IdViaje = myIdViaje
                oViajeOld = viajeBLL.loadInfo(oViaje.IdViaje)
                fSolicitud = oViajeOld.FechaSolicitud
                oViaje.IdUserSolicitador = oViajeOld.IdUserSolicitador
                Select Case oViajeOld.Estado
                    Case eEstadoViaje.Validado
                        oViaje.Estado = eEstadoViaje.Validado
                    Case eEstadoViaje.No_validado
                        oViaje.Estado = eEstadoViaje.Pendiente_validacion
                End Select
                Dim bEsActualizacion As Boolean = (oViajeOld IsNot Nothing AndAlso (oViajeOld.FechaIda <> CDate(txtFechaIda.Text) Or oViajeOld.FechaVuelta <> CDate(txtFechaVuelta.Text) Or oViajeOld.IdTarifaDestino <> CInt(ddlTarifaDestino.SelectedValue) Or oViajeOld.Destino <> txtDestino.Text.Trim))
                'Se comprueban si hay integrantes nuevos o modificacion(cambio de dia) y tambien si ya ha empezado el viaje, se comprueba si ha cambiado la actividad de algun integrante de exento a no exento o viceversa
                Dim integrExistente As ELL.Viaje.Integrante
                Dim bidaiakBLL As New BLL.BidaiakBLL
                Dim plantBLL As New SabLib.BLL.PlantasComponent
                Dim actBLL As New BLL.ActividadesBLL
                Dim oPlant As SabLib.ELL.Planta = Nothing
                Dim exento As Nullable(Of Boolean)
                Dim lHVP As List(Of Object)
                For Each integr As ELL.Viaje.Integrante In Integrantes
                    idUser = integr.Usuario.Id
                    integrExistente = oViajeOld.ListaIntegrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = idUser)
                    If (integrExistente Is Nothing) Then
                        If (lIntNew Is Nothing) Then lIntNew = New List(Of SabLib.ELL.Usuario)
                        lIntNew.Add(integr.Usuario)
                    Else
                        'Si han cambiado las fechas o el tema de desarraigados se guarda para avisarle
                        If (integr.FechaIda <> integrExistente.FechaIda OrElse integr.FechaVuelta <> integrExistente.FechaVuelta OrElse
                            integr.EsDesarraigado <> integrExistente.EsDesarraigado OrElse integr.CondicionesEspeciales_Desarraigados <> integrExistente.CondicionesEspeciales_Desarraigados) Then
                            If (lIntUpdate Is Nothing) Then lIntUpdate = New List(Of SabLib.ELL.Usuario)
                            lIntUpdate.Add(integr.Usuario)
                        End If
                        If (oViajeOld.FechaIda < Now AndAlso integr.IdActividad <> integrExistente.IdActividad) Then 'Ya ha empezado el viaje
                            If (oPlant Is Nothing OrElse (oPlant IsNot Nothing AndAlso oPlant.Id <> integr.Usuario.IdPlanta)) Then
                                oPlant = plantBLL.GetPlanta(integr.Usuario.IdPlanta)
                            End If
                            exento = actBLL.loadInfo(integr.IdActividad, False).ExentaIRPF
                            lHVP = bidaiakBLL.loadHVP(integr.Usuario.CodPersona, oPlant.IdIzaro, oViajeOld.FechaIda, oViajeOld.FechaVuelta, Nothing)
                            If (lHVP IsNot Nothing AndAlso lHVP.Count > 0 AndAlso lHVP.First.Exenta <> exento) Then
                                If (lHVP.Exists(Function(o) o.Estado = "V")) Then
                                    mensaAdv = itzultzaileWeb.Itzuli("No se puede cambiar la actividad del integrante porque ya tiene integrada alguna hoja de viajes y pernoctas en Izaro. Hable con RRHH para que borren esas hojas, cambie el estado a solicitada y despues borrelas. Una vez hecho esto, vuelva a realizar la misma accion") & " (" & integr.Usuario.NombreCompleto & ")"
                                Else
                                    mensaAdv = itzultzaileWeb.Itzuli("No se puede cambiar la actividad del integrante porque ya tiene creada alguna hoja de viajes y pernoctas. Borrelas primero y despues vuelva a realizar la misma accion") & " (" & integr.Usuario.NombreCompleto & ")"
                                End If
                                Return Nothing
                            End If
                        End If
                    End If
                Next
                'Se comprueban los integrantes que se han quitado
                For Each oUser As ELL.Viaje.Integrante In oViajeOld.ListaIntegrantes
                    idUser = oUser.Usuario.Id
                    If (Not Integrantes.Exists(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = idUser)) Then
                        If (lIntOld Is Nothing) Then lIntOld = New List(Of SabLib.ELL.Usuario)
                        lIntOld.Add(oUser.Usuario)
                    End If
                Next
            Else  'Es nuevo el viaje                
                oViaje.IdUserSolicitador = Master.Ticket.IdUser
                fSolicitud = Now.Date
                oViaje.FechaSolicitud = fSolicitud
                oViaje.IdPlanta = Master.IdPlantaGestion
                oViaje.Estado = eEstadoViaje.Pendiente_validacion
                'Si lo ha creado un usuario de perfil financiero, habra que cambiar el perfil
                If (hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) Then oViaje.TipoViaje = ELL.Viaje.eTipoViaje.Anticipo
                lIntNew = getUsuariosIntegrantes()
            End If
            'Se comprueba si alguna actividad es exenta
            If (CType(ddlTipoViaje.SelectedValue, ELL.Viaje.eNivel) <> ELL.Viaje.eNivel.Nacional) Then
                Dim activBLL As New BLL.ActividadesBLL
                For Each row As GridViewRow In gvPersonas.Rows
                    Dim odlActividad As WebControlsDropDown.OptionGroupDropDownList = DirectCast(row.FindControl("ogddlActiv"), WebControlsDropDown.OptionGroupDropDownList)
                    If (activBLL.loadInfo(CInt(odlActividad.SelectedValue)).ExentaIRPF) Then
                        bActividadesExentas = True
                        Exit For
                    End If
                Next
            End If
            If (bActividadesExentas AndAlso fSolicitud >= New Date(2019, 6, 14) AndAlso pnlRequiereProyCli.Visible) Then 'Si hay actividades exentas y es de troqueleria, alguno de los proyectos tiene que tener OF
                If (Not lProyectos.Exists(Function(o) o.NumOF <> String.Empty)) Then
                    mensaAdv = itzultzaileWeb.Itzuli("No se puede crear/modificar el viaje porque al existir una actividad exenta, alguno de los proyectos tiene que tener una OF abierta. Contacte con comercial")
                    Return Nothing
                End If
            End If
            oViaje.IdTarifaDestino = CInt(ddlTarifaDestino.SelectedValue)
            oViaje.Destino = If(oViaje.IdTarifaDestino > 0, String.Empty, txtDestino.Text.Trim)
            oViaje.Nivel = CInt(ddlTipoViaje.SelectedValue)
            oViaje.FechaIda = CDate(txtFechaIda.Text)
            oViaje.FechaVuelta = CDate(txtFechaVuelta.Text)
            oViaje.Descripcion = txtDescripcionDatosIni.Text.Trim
            oViaje.Proyectos = lProyectos
            Dim idLiq As Integer = getLiquidadorSeleccionado()
            If (idLiq > 0) Then oViaje.ResponsableLiquidacion = New SabLib.ELL.Usuario With {.Id = idLiq}
            oViaje.UnidadOrganizativa = New ELL.UnidadOrg With {.Id = CInt(ddlUnidadOrg.SelectedValue)}
            oViaje.ListaIntegrantes = Integrantes
            If (oViaje.Nivel = ELL.Viaje.eNivel.Nacional) Then
                oViaje.TipoDesplazamiento = ELL.Viaje.TipoDesplaz.Sin_especificar
                oViaje.Pais = ConfigurationManager.AppSettings("codSpain")
            Else
                oViaje.TipoDesplazamiento = CInt(ddlTipoDesplaz.SelectedValue)
                oViaje.Pais = ddlPais.SelectedValue
                If (CInt(ddlTipoDesplaz.SelectedValue) = ELL.Viaje.TipoDesplaz.Plantas_Filiales) Then
                    Dim lSolicitudes As New List(Of ELL.Viaje.SolicitudPlantaFilial)
                    Dim solFilialOld, mySolFilial As ELL.Viaje.SolicitudPlantaFilial
                    Dim filial As CheckBox
                    For Each rptItem As RepeaterItem In rptCheckFiliales.Items
                        filial = CType(rptItem.FindControl("chbFilial"), CheckBox)
                        If (filial.Checked) Then
                            filialesSeleccionadas = True
                            mySolFilial = New ELL.Viaje.SolicitudPlantaFilial With {.IdPlantaFilial = filial.Attributes.Item("value")}
                            If (myIdViaje = 0 OrElse (oViajeOld IsNot Nothing AndAlso (oViajeOld.SolicitudesPlantasFilial Is Nothing))) Then  'Si es nueva la peticion se comprobara si alguna actividad es exenta. Si no hay ninguna, no se mandara
                                If (Not bActividadesExentas) Then mySolFilial.EstadoSolicitud = ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Sin_Solicitud
                            End If
                            If (oViajeOld IsNot Nothing AndAlso oViajeOld.SolicitudesPlantasFilial IsNot Nothing) Then
                                solFilialOld = oViajeOld.SolicitudesPlantasFilial.Find(Function(o As ELL.Viaje.SolicitudPlantaFilial) o.IdPlantaFilial = mySolFilial.IdPlantaFilial)
                                If (solFilialOld IsNot Nothing) Then
                                    'Si existe una solicitud anterior en estado 'Sin solicitud' y existe alguna actividad exenta, se cambia a solicitado
                                    If (solFilialOld.EstadoSolicitud = ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Sin_Solicitud AndAlso bActividadesExentas) Then
                                        mySolFilial.EstadoSolicitud = ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Solicitado
                                    Else
                                        mySolFilial.EstadoSolicitud = solFilialOld.EstadoSolicitud
                                    End If
                                    mySolFilial.IdViaje = solFilialOld.IdViaje
                                    mySolFilial.Observaciones = solFilialOld.Observaciones
                                End If
                            End If
                            lSolicitudes.Add(mySolFilial)
                        End If
                    Next
                    'Si existe alguna con el estado sin Solicitud, todas tendran el mismo estado
                    If (lSolicitudes IsNot Nothing AndAlso lSolicitudes.Exists(Function(o As ELL.Viaje.SolicitudPlantaFilial) o.EstadoSolicitud = ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Sin_Solicitud)) Then
                        For Each oSol As ELL.Viaje.SolicitudPlantaFilial In lSolicitudes
                            oSol.EstadoSolicitud = ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Sin_Solicitud
                        Next
                    End If
                    oViaje.SolicitudesPlantasFilial = lSolicitudes
                Else
                    oViaje.SolicitudesPlantasFilial = Nothing
                End If
            End If
            'Si es el organizador y el viaje esta en curso, no se podra poner una fecha de ida menor a la del dia de hoy
            If (myIdViaje > 0 AndAlso oViaje.IdUserSolicitador = Master.Ticket.IdUser AndAlso oViajeOld.FechaIda <= CDate(Now.ToShortDateString) AndAlso CDate(Now.ToShortDateString) <= oViajeOld.FechaVuelta) Then
                If (oViaje.FechaIda < oViajeOld.FechaIda) Then
                    mensaAdv = itzultzaileWeb.Itzuli("Como el viaje esta en curso, la fecha de ida no puede ser anterior a la planificada") & " " & oViajeOld.FechaIda.ToShortDateString
                    Return Nothing
                ElseIf (oViaje.FechaVuelta < oViajeOld.FechaIda) Then
                    mensaAdv = itzultzaileWeb.Itzuli("Como el viaje esta en curso, la fecha de vuelta no puede ser anterior a la fecha de ida planificada") & " " & oViajeOld.FechaIda.ToShortDateString
                    Return Nothing
                End If
            End If
            'Si se cambia la fecha de ida, fecha de vuelta o el destino, se avisara al de la agencia
            bAvisarAgencia = (oViajeOld IsNot Nothing AndAlso (oViajeOld.FechaIda <> oViaje.FechaIda Or oViajeOld.FechaVuelta <> oViaje.FechaVuelta Or oViajeOld.Destino <> oViaje.Destino))
            'Se comprueba si se ha solicitado agencia
            '--------------------------------------------
            If (ServiciosAgenciasSeleccionados()) Then
                Dim solAgencBLL As New BLL.SolicAgenciasBLL
                '---Cabecera agencia---
                Dim oSolAgencia As New ELL.SolicitudAgencia
                oSolAgencia.IdViaje = oViaje.IdViaje
                'Si ya existe en base de datos, miramos el estado que tiene
                If (oViaje.IdViaje <> Integer.MinValue) Then
                    If (oViajeOld.SolicitudAgencia IsNot Nothing) Then
                        If (oViajeOld.SolicitudAgencia.ComentariosUsuario <> txtComentariosAgencia.Text.Trim) Then bAvisarAgencia = True 'Si se han realizado cambios en los comentarios, se el avisara
                        oSolAgencia = oViajeOld.SolicitudAgencia.Clone 'Ya la hemos cargado                        
                    Else
                        bAvisarAgencia = True  'Es una modificacion de viaje en la que no existia una solicitud de agencia
                    End If
                    If (oSolAgencia Is Nothing) Then  'Si no tiene nada, se vuelve a inicializar y se le asigna el id del viaje
                        oSolAgencia = New ELL.SolicitudAgencia
                        oSolAgencia.IdViaje = oViaje.IdViaje
                    End If
                Else  'Es nueva la solicitud de viaje
                    bAvisarAgencia = True
                End If
                If (txtComentariosAgencia.Enabled AndAlso txtComentariosAgencia.Text.Length = 0) Then
                    mensaAdv = itzultzaileWeb.Itzuli("Debe rellenar los comentarios de agencia")
                    Return Nothing
                End If
                '---Lineas de la agencia---                
                Dim lineas As New List(Of ELL.SolicitudAgencia.Linea)
                Dim oLinea As ELL.SolicitudAgencia.Linea
                Dim xbatBLL As New BLL.XbatBLL
                Dim moneda As ELL.Moneda = Nothing
                Dim selectedItems As Integer = 0
                moneda = xbatBLL.GetMoneda("EUR")
                For Each checkServ As CheckBox In GetServiciosChecks()
                    If (checkServ.Checked) Then
                        selectedItems += 1
                        oLinea = New ELL.SolicitudAgencia.Linea
                        oLinea.IdViaje = oViaje.IdViaje
                        oLinea.ServicioAgencia = New ELL.ServicioDeAgencia With {.Id = checkServ.Attributes.Item("value").Split("|")(0)}
                        oLinea.Moneda = moneda
                        If (checkServ.Attributes.Item("value").Split("|")(1) = "1") Then  'ha elegido con coche
                            oLinea.IdUserReq = ddlConductores.SelectedValue
                            oLinea.NavegadorGPS = False
                            'oLinea.NavegadorGPS = chbConNavegador.Checked
                        End If
                        'Si ya existe en base de datos, miramos el coste y comentarios                            
                        If (oViaje.IdViaje <> Integer.MinValue AndAlso (oSolAgencia IsNot Nothing AndAlso oSolAgencia.ServiciosSolicitados IsNot Nothing AndAlso oSolAgencia.ServiciosSolicitados.Count > 0)) Then
                            Dim oLineaBBDD As ELL.SolicitudAgencia.Linea = oSolAgencia.ServiciosSolicitados.Find(Function(o As ELL.SolicitudAgencia.Linea) If(o.ServicioAgencia IsNot Nothing, o.ServicioAgencia.Id = oLinea.ServicioAgencia.Id, False))
                            If (oLineaBBDD IsNot Nothing) Then
                                oLinea.Id = oLineaBBDD.Id
                                oLinea.Coste = oLineaBBDD.Coste
                                oLinea.Comentario = oLineaBBDD.Comentario
                                If (oLineaBBDD.IdUserReq <> oLinea.IdUserReq Or oLineaBBDD.NavegadorGPS <> oLinea.NavegadorGPS) Then
                                    bAvisarAgencia = True
                                End If
                            Else
                                bAvisarAgencia = True   'Hay un elemento distinto
                            End If
                        End If
                        lineas.Add(oLinea)
                    End If
                Next
                If (oSolAgencia.ServiciosSolicitados IsNot Nothing AndAlso oSolAgencia.ServiciosSolicitados.Count <> selectedItems) Then bAvisarAgencia = True 'Se ha borrado o añadido algun serviciio
                Dim lOtrasLineas As List(Of ELL.SolicitudAgencia.Linea) = Nothing
                If (oSolAgencia.ServiciosSolicitados IsNot Nothing) Then
                    'Nos quedamos con las lineas de no servicios
                    lOtrasLineas = oSolAgencia.ServiciosSolicitados.FindAll(Function(o As ELL.SolicitudAgencia.Linea) o.Tipo <> ELL.SolicitudAgencia.Linea.TipoLinea.Servicios)
                End If
                oSolAgencia.ServiciosSolicitados = New List(Of ELL.SolicitudAgencia.Linea)
                If (lineas.Count > 0) Then oSolAgencia.ServiciosSolicitados = lineas
                If (lOtrasLineas IsNot Nothing) Then oSolAgencia.ServiciosSolicitados.AddRange(lOtrasLineas) 'Se añaden las otras lineas
                If (oSolAgencia.ServiciosSolicitados.Count = 0) Then oSolAgencia.ServiciosSolicitados = Nothing
                oSolAgencia.ComentariosUsuario = txtComentariosAgencia.Text.Trim
                oViaje.SolicitudAgencia = oSolAgencia
                If (Not bAvisarAgencia And oViajeOld IsNot Nothing) Then  'Si todavia no se ha marcado para avisar a la agencia, se comprueba si se ha modificado algun usuario
                    If (oViajeOld.ListaIntegrantes.Count <> oViaje.ListaIntegrantes.Count) Then
                        bAvisarAgencia = True
                    Else
                        Dim idIntegr As Integer
                        For Each integ As ELL.Viaje.Integrante In oViajeOld.ListaIntegrantes
                            idIntegr = integ.Usuario.Id
                            If (Not oViaje.ListaIntegrantes.Exists(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = idIntegr)) Then
                                'Se ha quitado un integrante
                                bAvisarAgencia = True
                                Exit For
                            End If
                        Next
                        If (Not bAvisarAgencia) Then
                            Dim integrExistente As ELL.Viaje.Integrante
                            For Each integ As ELL.Viaje.Integrante In oViaje.ListaIntegrantes
                                idIntegr = integ.Usuario.Id
                                integrExistente = oViajeOld.ListaIntegrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = idIntegr)
                                If (integrExistente Is Nothing) Then
                                    'Se ha añadido un integrante
                                    bAvisarAgencia = True
                                    Exit For
                                Else
                                    If (integrExistente.FechaIda <> integ.FechaIda OrElse integrExistente.FechaVuelta <> integ.FechaVuelta) Then
                                        'Se le han cambiado las fechas
                                        bAvisarAgencia = True
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If
                '---Presupuesto---
                Dim presupBLL As New BLL.PresupuestosBLL
                Dim oPresup As ELL.Presupuesto = Nothing
                Dim bRecalcular As Boolean = False
                If (oViaje.IdViaje = Integer.MinValue OrElse (oViaje.IdViaje <> Integer.MinValue AndAlso oViajeOld.SolicitudAgencia Is Nothing)) Then  'Si se está creando el viaje o ya está creado pero antes no había solicitud de agencia
                    oPresup = New ELL.Presupuesto With {.Estado = ELL.Presupuesto.EstadoPresup.Generado}
                    bRecalcular = True
                ElseIf ((oSolAgencia.Estado = ELL.SolicitudAgencia.EstadoAgencia.solicitado Or oSolAgencia.Estado = ELL.SolicitudAgencia.EstadoAgencia.Tramite) AndAlso
                            (oViaje.IdViaje <> Integer.MinValue AndAlso (lIntNew IsNot Nothing OrElse lIntOld IsNot Nothing))) Then
                    'Si es un viaje ya creado y han cambiado sus integrantes, habra que comprobar si el presupuesto no se ha enviado. Si no habra que recalcular el responsable de aprobacion del presupuesto
                    oPresup = presupBLL.loadInfo(oViaje.IdViaje)
                    bRecalcular = (oPresup Is Nothing OrElse (oPresup IsNot Nothing AndAlso (oPresup.Estado = ELL.Presupuesto.EstadoPresup.Generado OrElse oPresup.Estado = ELL.Presupuesto.EstadoPresup.Creado)))
                    If (bRecalcular) Then log.Info("Hay que recalcular el responsable de aprobacion del presupuesto del viaje " & oViaje.IdViaje)
                    If (oPresup Is Nothing) Then oPresup = New ELL.Presupuesto With {.Estado = ELL.Presupuesto.EstadoPresup.Generado}
                End If
                If (bRecalcular) Then
                    oPresup.IdUsuarioResponsable = presupBLL.RecalcularRespVal_Presupuesto(oViaje.IdUserSolicitador, oViaje.ListaIntegrantes)
                    If (oViaje.IdViaje = Integer.MinValue) Then
                        log.Info("El validador del presupuesto de agencia va a ser " & oPresup.IdUsuarioResponsable)
                    Else
                        log.Info("El validador actual del presupuesto de agencia  es " & oPresup.IdUsuarioResponsable)
                    End If
                End If
                'Asignar a la solicitud de agencia
                oViaje.SolicitudAgencia.Presupuesto = oPresup
            Else
                bAvisarAgencia = False
            End If
            'Se comprueba si se ha solicitado anticipos
            '--------------------------------------------
            'Si no tiene importes pero tiene un estado de anticipo, podria significar que tiene una transferencia y habria que entrar para que haría el proceso
            If ((selImportes.Importes IsNot Nothing AndAlso selImportes.Importes.Count > 0) OrElse (hfIdEstAnti.Value <> String.Empty AndAlso hfIdEstAnti.Value <> ELL.Anticipo.EstadoAnticipo.solicitado)) Then
                Dim anticBLL As New BLL.AnticiposBLL
                Dim oAnticBBDD As ELL.Anticipo = Nothing
                '---Cabecera anticipo---
                Dim oAnticipo As New ELL.Anticipo
                oAnticipo.IdViaje = oViaje.IdViaje
                'Si ya existe en base de datos, miramos el estado que tiene
                If (oViaje.IdViaje <> Integer.MinValue) Then
                    If (oViajeOld.Anticipo IsNot Nothing) Then
                        If (calFechaNec.IsFechaValida AndAlso oViajeOld.Anticipo.FechaNecesidad <> calFechaNec.Fecha) Then bAvisarFinanciero = True 'Si se cambia la fecha de necesidad, se avisa                
                        oAnticipo = oViajeOld.Anticipo.Clone 'Ya la hemos cargado                        
                    Else
                        bAvisarFinanciero = True  'Es una modificacion de viaje en la que no existia una solicitud de anticipo
                    End If
                    If (oAnticipo Is Nothing) Then  'Si no tiene nada, se vuelve a inicializar y se le asigna el id del viaje
                        oAnticipo = New ELL.Anticipo
                        oAnticipo.IdViaje = oViaje.IdViaje
                    End If
                ElseIf (oViaje.IdViaje = Integer.MinValue AndAlso hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) Then  'Nueva solicitud
                    oAnticipo.Estado = ELL.Anticipo.EstadoAnticipo.solicitado  'Cuando el viaje lo crea uno de administracion, el estado inicial es solitado (antes era tramitado que ya no existe)
                    bAvisarFinanciero = True
                Else 'Nueva solicitud
                    bAvisarFinanciero = True
                End If
                If (calFechaNec.IsFechaValida) Then oAnticipo.FechaNecesidad = calFechaNec.Fecha
                '---Movimientos---
                If (oAnticipo.AnticiposSolicitados IsNot Nothing AndAlso oAnticipo.AnticiposSolicitados.Count <> selImportes.Importes.Count) Then bAvisarFinanciero = True 'Se ha borrado o añadido alguna linea
                Dim lineas As New List(Of ELL.Anticipo.Movimiento)
                Dim idMov As Integer
                For Each mov As ELL.Anticipo.Movimiento In selImportes.Importes
                    If (mov.Id = Integer.MinValue) Then  'Nuevo movimiento
                        'mov.Cantidad, mov.Moneda, mov.ConversionEuros  'Ya estan informados
                        'El resto de datos no haria falta introducirlos
                        mov.Fecha = Now
                        mov.IdAnticipo = oAnticipo.IdViaje
                        mov.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Solicitado
                        mov.UserOrigen = New SabLib.ELL.Usuario With {.Id = Master.Ticket.IdUser}
                        bAvisarFinanciero = True  'Se ha introducido un nuevo movimiento
                    Else
                        idMov = mov.Id
                        Dim oMovBBDD As ELL.Anticipo.Movimiento = Nothing
                        If (oViajeOld IsNot Nothing AndAlso oViajeOld.Anticipo IsNot Nothing AndAlso oViajeOld.Anticipo.Movimientos IsNot Nothing) Then
                            oMovBBDD = oViajeOld.Anticipo.Movimientos.Find(Function(o As ELL.Anticipo.Movimiento) o.Id = idMov)
                        End If
                        If (oMovBBDD IsNot Nothing) Then
                            mov.Id = oMovBBDD.Id
                            mov.Fecha = oMovBBDD.Fecha
                            mov.IdAnticipo = oMovBBDD.IdAnticipo
                            mov.UserOrigen = oMovBBDD.UserOrigen
                            mov.UserDestino = oMovBBDD.UserDestino
                            mov.IdViajeOrigen = oMovBBDD.IdViajeOrigen
                            mov.IdViajeDestino = oMovBBDD.IdViajeDestino
                            mov.Comentarios = oMovBBDD.Comentarios
                            mov.TipoMov = oMovBBDD.TipoMov
                            mov.CambioMonedaEUR = oMovBBDD.CambioMonedaEUR
                            If (mov.Cantidad <> oMovBBDD.Cantidad) Then bAvisarFinanciero = True 'Si se cambia la cantidad, tambien se informa
                        End If
                    End If
                    lineas.Add(mov)
                Next
                Dim lOtrasLineas As List(Of ELL.Anticipo.Movimiento) = Nothing
                If (oAnticipo.Movimientos IsNot Nothing) Then
                    'Nos quedamos con las lineas que no sean de importes
                    lOtrasLineas = oAnticipo.Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov <> ELL.Anticipo.Movimiento.TipoMovimiento.Solicitado)
                End If
                oAnticipo.Movimientos = New List(Of ELL.Anticipo.Movimiento)
                If (lineas.Count > 0) Then oAnticipo.Movimientos = lineas
                If (lOtrasLineas IsNot Nothing) Then oAnticipo.Movimientos.AddRange(lOtrasLineas) 'Se añaden las otras lineas
                If (oAnticipo.Movimientos.Count = 0) Then oAnticipo.Movimientos = Nothing
                oViaje.Anticipo = oAnticipo
                'Si el viaje es nuevo o se ha cambiado de liquidador, se comprueba si no tiene pendiente de devolver un anticipo con mas de tres meses
                Dim idLiqOld, idLiqNew As Integer
                idLiqOld = If(oViajeOld IsNot Nothing AndAlso oViajeOld.ResponsableLiquidacion IsNot Nothing, oViajeOld.ResponsableLiquidacion.Id, 0)
                idLiqNew = If(oViaje.ResponsableLiquidacion IsNot Nothing, oViaje.ResponsableLiquidacion.Id, 0)
                If ((myIdViaje = 0) OrElse (oViajeOld IsNot Nothing AndAlso oViajeOld.Anticipo Is Nothing AndAlso idLiq > 0) OrElse (oViajeOld IsNot Nothing AndAlso oViajeOld.Anticipo IsNot Nothing AndAlso idLiqOld <> idLiqNew)) Then
                    Dim lAnticiposPen As List(Of Object) = anticBLL.loadAnticiposPendientes(Master.IdPlantaGestion, idLiq)
                    If (lAnticiposPen IsNot Nothing AndAlso lAnticiposPen.Exists(Function(o) CDate(o.FechaVueltaViaje).AddMonths(3).Subtract(Now).TotalDays < 0)) Then
                        mensaAdv = itzultzaileWeb.Itzuli("El liquidador seleccionado no puede solicitar un nuevo anticipo porque tiene alguno sin devolver desde hace mas de tres meses. Hable con administracion")
                        Return Nothing
                    End If
                End If
            Else
                oViaje.ResponsableLiquidacion = Nothing   'Sino se ha solicitado ningun anticipo, se quita el responsable de liquidacion
            End If
            'Se comprueba si se ha habido cambios en las plantas filiales
            '-------------------------------------------
            Dim idTipoDesplaz As Integer = oViaje.TipoDesplazamiento
            bAvisarGerentes = ((oViajeOld Is Nothing AndAlso filialesSeleccionadas) OrElse (oViajeOld IsNot Nothing AndAlso oViajeOld.SolicitudesPlantasFilial Is Nothing AndAlso filialesSeleccionadas) OrElse
                               (oViajeOld IsNot Nothing AndAlso oViajeOld.SolicitudesPlantasFilial IsNot Nothing AndAlso oViajeOld.SolicitudesPlantasFilial.Count > 0 AndAlso idTipoDesplaz <> ELL.Viaje.TipoDesplaz.Plantas_Filiales))
            If (idTipoDesplaz = ELL.Viaje.TipoDesplaz.Plantas_Filiales) Then
                Dim idPlanta As Integer
                If (oViajeOld IsNot Nothing AndAlso oViajeOld.SolicitudesPlantasFilial IsNot Nothing) Then
                    For Each solic As ELL.Viaje.SolicitudPlantaFilial In oViajeOld.SolicitudesPlantasFilial
                        idPlanta = solic.IdPlantaFilial
                        If (Not oViaje.SolicitudesPlantasFilial.Exists(Function(o As ELL.Viaje.SolicitudPlantaFilial) o.IdPlantaFilial = idPlanta)) Then
                            bAvisarGerentes = True
                            If (lPlFilialesOld Is Nothing) Then lPlFilialesOld = New List(Of Integer)
                            lPlFilialesOld.Add(idPlanta)
                        End If
                    Next
                End If
                For Each solic As ELL.Viaje.SolicitudPlantaFilial In oViaje.SolicitudesPlantasFilial
                    idPlanta = solic.IdPlantaFilial
                    If (oViajeOld Is Nothing OrElse (oViajeOld IsNot Nothing AndAlso oViajeOld.SolicitudesPlantasFilial IsNot Nothing AndAlso Not oViajeOld.SolicitudesPlantasFilial.Exists(Function(o As ELL.Viaje.SolicitudPlantaFilial) o.IdPlantaFilial = idPlanta))) Then
                        bAvisarGerentes = True
                        If (lPlFilialesNew Is Nothing) Then lPlFilialesNew = New List(Of Integer)
                        lPlFilialesNew.Add(idPlanta)
                    End If
                Next
            Else 'ahora se han quitado
                If (oViajeOld IsNot Nothing AndAlso oViajeOld.SolicitudesPlantasFilial IsNot Nothing AndAlso oViajeOld.SolicitudesPlantasFilial IsNot Nothing) Then
                    If (lPlFilialesOld Is Nothing) Then lPlFilialesOld = New List(Of Integer)
                    For Each plantaFil As ELL.Viaje.SolicitudPlantaFilial In oViajeOld.SolicitudesPlantasFilial
                        lPlFilialesOld.Add(plantaFil.IdPlantaFilial)
                    Next
                End If
            End If
            'Si existen solicitudes de plantas filial y existe alguna sin solicitud, no se avisa a los gerentes
            If (oViaje.SolicitudesPlantasFilial IsNot Nothing AndAlso oViaje.SolicitudesPlantasFilial.Exists(Function(o As ELL.Viaje.SolicitudPlantaFilial) o.EstadoSolicitud = ELL.Viaje.SolicitudPlantaFilial.EstadoSolFilial.Sin_Solicitud)) Then
                bAvisarGerentes = False
            End If

            Return oViaje
        Catch batzEx As BidaiakLib.BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BidaiakLib.BatzException("Error al recuperar la informacion del viaje", ex)
        End Try
    End Function

    ''' <summary>
    ''' Rellena la propiedad de integrantes, con los datos asociados a la actividad y observaciones
    ''' </summary>
    ''' <returns>False si faltan datos por introducir</returns>
    Private Function RellenarIntegrantes() As Boolean
        Dim bResul As Boolean = True
        Try
            Dim idUser, idActiv As Integer
            Dim fIda, fVuelta As Date
            Dim check As CheckBox
            Dim observ As String = String.Empty
            Dim oIntegr As ELL.Viaje.Integrante
            For Each gvr As GridViewRow In gvPersonas.Rows
                idUser = CInt(CType(gvr.FindControl("lblIdSab"), Label).Text)
                idActiv = CInt(CType(gvr.FindControl("ogddlActiv"), WebControlsDropDown.OptionGroupDropDownList).SelectedValue)
                observ = CType(gvr.FindControl("txtObservacion"), TextBox).Text
                fIda = CDate(CType(gvr.FindControl("txtFIda"), TextBox).Text)
                fVuelta = CDate(CType(gvr.FindControl("txtFVuelta"), TextBox).Text)
                check = CType(gvr.FindControl("chbDesarraigado"), CheckBox)
                If (idActiv = Integer.MinValue OrElse fIda > fVuelta) Then
                    bResul = False
                    Exit For
                Else
                    oIntegr = Integrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = idUser)
                    oIntegr.IdActividad = idActiv
                    oIntegr.Observaciones = observ
                    If (check.Checked) Then
                        oIntegr.esPaP_Desarraigados = If(CType(gvr.FindControl("ddlPaP"), DropDownList).SelectedValue = "1", True, False)
                        oIntegr.CondicionesEspeciales_Desarraigados = CInt(CType(gvr.FindControl("ddlCondEsp"), DropDownList).SelectedValue)
                    Else
                        oIntegr.esPaP_Desarraigados = False
                        oIntegr.CondicionesEspeciales_Desarraigados = Integer.MinValue
                    End If
                End If
            Next
        Catch ex As Exception
            log.Error("Se ha producido un error al comprobar los datos de los integrantes", ex)
            bResul = False
        End Try
        Return bResul
    End Function

    ''' <summary>
    ''' Sube el documento de cliente del viaje
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSubirDoc_Click(sender As Object, e As EventArgs) Handles btnSubirDoc.Click
        Try
            If (txtDocTitulo.Text = String.Empty Or Not fuDocumento.HasFile) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar todos los datos")
            ElseIf (fuDocumento.HasFile AndAlso fuDocumento.FileName.Length > 75) Then
                log.Warn("Doc_Cliente: Se ha excedido el maximo numero de caracteres del fichero a subir (" & fuDocumento.FileName.Length & ")")
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La longitud maxima del nombre del fichero son 75 caracteres")
            Else
                Dim idViaje As Integer = CInt(btnGuardar.CommandArgument)
                Dim oDoc As New ELL.Viaje.DocumentoCliente With {.Documento = fuDocumento.FileBytes, .ContentType = fuDocumento.PostedFile.ContentType, .NombreFichero = fuDocumento.FileName, .IdViaje = idViaje, .Titulo = txtDocTitulo.Text.Trim}
                Dim viajeBLL As New BLL.ViajesBLL
                Dim idDoc As Integer = viajeBLL.AddDocumentoCliente(oDoc)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Documento añadido")
                log.Info("Se ha subido el documento de cliente (" & idDoc & ") " & oDoc.Titulo & " al viaje " & idViaje)
                mostrarDetalle(idViaje)
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se valida el viaje y se mandan los emails
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnValidar_Click(sender As Object, e As EventArgs) Handles btnValidar.Click
        Try
            Dim idViaje As Integer = CInt(btnGuardar.CommandArgument)
            Dim viaBLL As New BLL.ViajesBLL
            viaBLL.CambiarEstadoViaje(idViaje, ELL.Viaje.eEstadoViaje.Validado, Now, Master.Ticket.IdUser, txtComentariosVal.Text)
            log.Info("Se ha validado el viaje " & lblIdViaje.Text & " con el comentario:" & txtComentariosVal.Text)
            AvisarValidacionViaje(idViaje, txtComentariosVal.Text)
            lblEstadoViaje.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Viaje.eEstadoViaje), ELL.Viaje.eEstadoViaje.Validado))
            lblEstadoViaje.CssClass = "label label-success"
            pnlValidacion.Visible = False
            Master.MensajeInfo = itzultzaileWeb.Itzuli("El viaje se ha validado correctamente y se ha avisado por email")
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se rechaza el viaje y se avisa al solicitante para que pueda cambiarlo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRechazar_Click(sender As Object, e As EventArgs) Handles btnRechazar.Click
        Try
            If (txtComentariosVal.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe introducir un comentario")
            Else
                Dim viaBLL As New BLL.ViajesBLL
                Dim idViaje As Integer = CInt(btnGuardar.CommandArgument)
                viaBLL.CambiarEstadoViaje(idViaje, ELL.Viaje.eEstadoViaje.No_validado, Now, Master.Ticket.IdUser, txtComentariosVal.Text)
                log.Info("Se ha rechazado el viaje " & lblIdViaje.Text & " con el siguiente comentario:" & txtComentariosVal.Text)
                AvisarRechazoViaje(viaBLL.loadInfo(idViaje, bSoloCabecera:=True), txtComentariosVal.Text)
                lblEstadoViaje.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Viaje.eEstadoViaje), ELL.Viaje.eEstadoViaje.No_validado).Replace("_", " "))
                lblEstadoViaje.CssClass = "label label-danger"
                pnlValidacion.Visible = False
                Master.MensajeInfo = itzultzaileWeb.Itzuli("El viaje se ha rechazado correctamente y se ha avisado por email")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se avisa a todos los que entren el juego, de que el viaje ha sido validado
    ''' </summary>
    ''' <param name="idViaje">Id del viaje</param>
    ''' <param name="comentarios">Comentarios que se han podido anexar al aprobar el viaje</param>
    Private Sub AvisarValidacionViaje(ByVal idViaje As Integer, ByVal comentarios As String)
        If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
            Dim viajeBLL As New BLL.ViajesBLL
            Dim oViajeOld As ELL.Viaje = Nothing
            Dim lIntNew, lIntOld, lIntUpdate As List(Of SabLib.ELL.Usuario)
            Dim lPlFilialesNew, lPlFilialesOld As List(Of Integer)
            Dim lProyectos As List(Of Proyecto) = Nothing
            Dim bAvisarAgencia, bAvisarFinanciero, bAvisarIntegrantes, bAvisarGerentes, bAvisarViajeRestoMundo As Boolean
            bAvisarAgencia = False : bAvisarFinanciero = False : bAvisarIntegrantes = False : bAvisarGerentes = False
            lIntNew = Nothing : lIntOld = Nothing : lIntUpdate = Nothing : lPlFilialesNew = Nothing : lPlFilialesOld = Nothing
            If (lProyectos Is Nothing) Then lProyectos = getDatosProyectos()
            Dim mensaAdv As String = String.Empty
            Dim oViaje As ELL.Viaje = RecogerInfoViaje(True, oViajeOld, bAvisarAgencia, bAvisarFinanciero, lIntNew, lIntOld, lIntUpdate, bAvisarGerentes, lPlFilialesNew, lPlFilialesOld, lProyectos, mensaAdv)
            oViaje.IdViaje = idViaje
            'Se añade esta parte de cogido porque en recogerInfoViaje, como se ejecuta en varias opciones, no podemos obtener lo que queremos
            Dim myViajeValidado As ELL.Viaje = viajeBLL.loadInfo(idViaje)
            oViaje.IdUserSolicitador = myViajeValidado.IdUserSolicitador
            oViaje.Estado = eEstadoViaje.Validado
            bAvisarViajeRestoMundo = (oViaje.Nivel = eNivel.Resto_del_mundo)
            If (mensaAdv = String.Empty) Then
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim myUser As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador})
                Dim mensa As String = "SOLICITUD_VIAJE:El usuario " & myUser.NombreCompleto & " (" & myUser.Id & ") "
                mensa &= "ha generado la solicitud de viaje [" & idViaje & "] para el dia " & oViaje.FechaIda.ToShortDateString & " a " & oViaje.Destino & "."
                If (hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) Then
                    mensa &= ".El viaje no es programado. Se ha creado por parte del departamento financiero como un viaje con Anticipo"
                ElseIf (hasProfile(BLL.BidaiakBLL.Profiles.Agencia)) Then
                    mensa &= ".Se ha creado por parte de la agencia de viajes"
                End If
                If (oViaje.SolicitudAgencia IsNot Nothing OrElse oViaje.Anticipo IsNot Nothing OrElse oViaje.SolicitudesPlantasFilial IsNot Nothing) Then
                    mensa &= "("
                    Dim servicios As String = String.Empty
                    If (oViaje.SolicitudAgencia IsNot Nothing) Then servicios = "Solicitud de agencia"
                    If (oViaje.Anticipo IsNot Nothing) Then
                        If (servicios <> String.Empty) Then servicios &= " y "
                        servicios &= "Anticipo"
                    End If
                    If (oViaje.SolicitudesPlantasFilial IsNot Nothing) Then
                        If (servicios <> String.Empty) Then servicios &= " y "
                        servicios &= "Solicitud de planta"
                    End If
                    mensa &= servicios & ")"
                End If
                log.Info(mensa)
                AvisarPorEmail(oViaje, bAvisarAgencia, bAvisarFinanciero, lIntNew, lIntOld, lIntUpdate, bAvisarGerentes, lPlFilialesNew, lPlFilialesOld, (btnGuardar.CommandArgument = String.Empty), bAvisarViajeRestoMundo)
            Else
                log.Warn("No se ha enviado ningun email para avisar de que se ha validado el viaje " & idViaje & ". Razon:" & mensaAdv)
                Master.MensajeAdvertencia = mensaAdv
            End If
        End If
    End Sub

    ''' <summary>
    ''' Se avisa al organizador de que el viaje no ha sido validado
    ''' </summary>
    ''' <param name="oViaje">Información de viajes</param>
    ''' <param name="motivoRechazo">Motivo de porqué se rechaza el viaje</param>
    Private Sub AvisarRechazoViaje(ByVal oViaje As ELL.Viaje, ByVal motivoRechazo As String)
        If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim perfBLL As New BLL.BidaiakBLL
            Dim emailsAccesoDirecto, emailsAccesoPortal, subject, body, bodyEmail, linkUrl As String
            Dim idRecAdmon As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon"))
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
            body = "Se ha rechazado el viaje " & vbCrLf & vbCrLf
            body &= oViaje.IdViaje & " - " & oViaje.Destino & " a realizar en las fechas " & oViaje.FechaIda.ToShortDateString & " - " & oViaje.FechaVuelta.ToShortDateString & "<br/><br/>"
            body &= "Motivo del rechazo: <br />" & motivoRechazo
            Try
                emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
                Dim oUser As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador})
                If (oUser IsNot Nothing) Then
                    Dim sPerfil As String() = perfBLL.loadProfile(Master.IdPlantaGestion, oUser.Id, idRecAdmon)
                    If (sPerfil(1) = "0") Then 'Acceso por portal
                        If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                        emailsAccesoPortal &= oUser.Email
                    Else 'Acceso directo
                        If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                        emailsAccesoDirecto &= oUser.Email
                    End If
                End If
                If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                    log.Info("NO_VALIDACION_VIAJE:No se han encontrado ningun email del planificador para avisar del rechazo del viaje (V" & oViaje.IdViaje & ")")
                Else
                    If (emailsAccesoPortal <> String.Empty) Then
                        linkUrl = String.Empty
                        subject = "Rechazo del viaje (V" & oViaje.IdViaje & ")"
                        bodyEmail = PageBase.getBodyHmtl("Rechazo del viaje", "V" & oViaje.IdViaje, body, linkUrl, True)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                        log.Info("NO_VALIDACION_VIAJE: Se ha enviado un email al planificador para avisar del rechazo del viaje(V" & oViaje.IdViaje & ") con acceso por el portal =>" & emailsAccesoPortal)
                    End If
                    If (emailsAccesoDirecto <> String.Empty) Then
                        linkUrl = String.Empty
                        subject = "Rechazo del viaje (V" & oViaje.IdViaje & ")"
                        bodyEmail = PageBase.getBodyHmtl("Rechazo del viaje", "V" & oViaje.IdViaje, body, linkUrl, False)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                        log.Info("NO_VALIDACION_VIAJE: Se ha enviado un email al planificador para avisar de la cancelacion del viaje(V" & oViaje.IdViaje & ") con acceso directo =>" & emailsAccesoDirecto)
                    End If
                End If
            Catch ex As Exception
                log.Error("NO_VALIDACION_VIAJE: No se ha podido avisar al planificador del rechazo del viaje (V" & oViaje.IdViaje & ")", ex)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Avisa por email al organizador, a los integrantes, agencia,financiero y gerentes de que el viaje se ha cancelado
    ''' </summary>    
    ''' <param name="oViaje">Viaje</param>    
    Private Sub AvisarCancelacionPorEmail(ByVal oViaje As ELL.Viaje)
        If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim perfBLL As New BLL.BidaiakBLL
            Dim emailsAccesoDirecto, emailsAccesoPortal, subject, body, bodyEmail, linkUrl As String
            Dim idRecAdmon As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon"))
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
            body = "Se ha cancelado el viaje " & vbCrLf & vbCrLf
            body &= oViaje.IdViaje & " - " & oViaje.Destino & " a realizar en las fechas " & oViaje.FechaIda.ToShortDateString & " - " & oViaje.FechaVuelta.ToShortDateString
            Try
                emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
                Dim oIntegr As ELL.Viaje.Integrante = Integrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = oViaje.IdUserSolicitador)
                If (oIntegr Is Nothing) Then  'No va como integrante, asi que se mete
                    Dim oUser As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador})
                    If (oUser IsNot Nothing) Then Integrantes.Add(New ELL.Viaje.Integrante With {.Usuario = oUser})
                End If
                'Integrantes
                For Each integr As ELL.Viaje.Integrante In Integrantes
                    Dim sPerfil As String() = perfBLL.loadProfile(Master.IdPlantaGestion, integr.Usuario.Id, idRecAdmon)
                    If (sPerfil(1) = "0") Then 'Acceso por portal
                        If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                        emailsAccesoPortal &= integr.Usuario.Email
                    Else 'Acceso directo
                        If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                        emailsAccesoDirecto &= integr.Usuario.Email
                    End If
                Next
                If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                    log.Info("CANCELACION_VIAJE:No se han encontrado ningun email de los integrantes y planificador para avisar de la cancelacion del viaje (V" & oViaje.IdViaje & ")")
                Else
                    If (emailsAccesoPortal <> String.Empty) Then
                        linkUrl = String.Empty
                        subject = "Cancelacion del viaje (V" & oViaje.IdViaje & ")"
                        bodyEmail = PageBase.getBodyHmtl("Cancelacion del viaje", "V" & oViaje.IdViaje, body, linkUrl, True)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                        log.Info("CANCELACION_VIAJE: Se ha enviado un email a los integrantes y planificador para avisar de la cancelacion del viaje(V" & oViaje.IdViaje & ") con acceso por el portal =>" & emailsAccesoPortal)
                        bNotifInteg = True
                    End If
                    If (emailsAccesoDirecto <> String.Empty) Then
                        linkUrl = String.Empty
                        subject = "Cancelacion del anticipo del viaje (V" & oViaje.IdViaje & ")"
                        bodyEmail = PageBase.getBodyHmtl("Cancelacion del anticipo del viaje", "V" & oViaje.IdViaje, body, linkUrl, False)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                        log.Info("CANCELACION_VIAJE: Se ha enviado un email a los integrantes y planificador para avisar de la cancelacion del viaje(V" & oViaje.IdViaje & ") con acceso directo =>" & emailsAccesoDirecto)
                        bNotifInteg = True
                    End If
                End If
            Catch ex As Exception
                log.Error("CANCELACION_VIAJE: No se ha podido avisar a los integrantes y planificador de la cancelacion del viaje (V" & oViaje.IdViaje & ")", ex)
            End Try
            'Financiero
            Try
                emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
                If (oViaje.Anticipo IsNot Nothing) Then
                    Dim lUsersFinan As List(Of String()) = perfBLL.loadUsersProfile(Master.IdPlantaGestion, BidaiakLib.BLL.BidaiakBLL.Profiles.Financiero, idRecAdmon, True)
                    If (lUsersFinan IsNot Nothing AndAlso lUsersFinan.Count > 0) Then
                        Dim oUser As SabLib.ELL.Usuario
                        For Each sFinanciero As String() In lUsersFinan
                            oUser = New SabLib.ELL.Usuario With {.Id = CInt(sFinanciero(0))}
                            oUser = userBLL.GetUsuario(oUser)
                            If (oUser IsNot Nothing) Then
                                If (sFinanciero(1) = "0") Then 'Acceso por portal
                                    If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                                    emailsAccesoPortal &= oUser.Email
                                Else 'Acceso directo
                                    If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                                    emailsAccesoDirecto &= oUser.Email
                                End If
                            End If
                        Next
                    End If
                End If
                If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                    log.Info("CANCELACION_VIAJE:No se han encontrado ningun email de financiero para avisar de la cancelacion del viaje (V" & oViaje.IdViaje & ")")
                Else
                    'Se comprueba si tiene un anticipo en estado 'entregado'
                    Dim bodyAnticipo, liquidador, sDevolver As String
                    bodyAnticipo = String.Empty : sDevolver = String.Empty
                    If (oViaje.Anticipo IsNot Nothing AndAlso oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.Entregado) Then
                        If (oViaje.ResponsableLiquidacion IsNot Nothing) Then
                            liquidador = oViaje.ResponsableLiquidacion.NombreCompleto
                        Else  'Por si no estuviera informado, para que no falle
                            liquidador = "el liquidador del viaje"
                        End If
                        bodyAnticipo = "<br /><br />" & "Recuerde que " & liquidador & " debe devolver el anticipo de:"
                        sDevolver = liquidador & " debe devolver el anticipo de =>"
                        For Each antic As ELL.Anticipo.Movimiento In oViaje.Anticipo.AnticiposSolicitados
                            bodyAnticipo &= "<br /> - " & antic.Cantidad & " " & antic.Moneda.Nombre
                            sDevolver &= antic.Cantidad & " " & antic.Moneda.Nombre & ","
                        Next
                        sDevolver = sDevolver.Substring(0, sDevolver.Length - 1)  'Para quitar la coma
                    End If

                    If (emailsAccesoPortal <> String.Empty) Then
                        linkUrl = String.Empty
                        subject = "Cancelacion del anticipo del viaje (V" & oViaje.IdViaje & ")"
                        bodyEmail = PageBase.getBodyHmtl("Cancelacion del anticipo del viaje", "V" & oViaje.IdViaje, body & bodyAnticipo, linkUrl, True)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                        log.Info("CANCELACION_VIAJE: Se ha enviado un email a los responsables financieros para avisar de la cancelacion del viaje(V" & oViaje.IdViaje & ") con acceso por el portal =>" & emailsAccesoPortal)
                        If (sDevolver <> String.Empty) Then log.Info("CANCELACION_VIAJE: " & sDevolver)
                        bNotifAdmin = True
                    End If
                    If (emailsAccesoDirecto <> String.Empty) Then
                        linkUrl = String.Empty
                        subject = "Cancelacion del anticipo del viaje (V" & oViaje.IdViaje & ")"
                        bodyEmail = PageBase.getBodyHmtl("Cancelacion del anticipo del viaje", "V" & oViaje.IdViaje, body & bodyAnticipo, linkUrl, False)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                        log.Info("CANCELACION_VIAJE: Se ha enviado un email a los responsables financieros para avisar de la cancelacion del viaje(V" & oViaje.IdViaje & ") con acceso directo =>" & emailsAccesoDirecto)
                        If (sDevolver <> String.Empty) Then log.Info("CANCELACION_VIAJE: " & sDevolver)
                        bNotifAdmin = True
                    End If
                End If
            Catch ex As Exception
                log.Error("CANCELACION_VIAJE: No se ha podido avisar a los integrantes, organizador y responsables financieros de la cancelacion del viaje (V" & oViaje.IdViaje & ")", ex)
            End Try
            'Agencia
            Try
                emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
                If (oViaje.SolicitudAgencia IsNot Nothing) Then
                    Dim lUsersAgencia As List(Of String()) = perfBLL.loadUsersProfile(Master.IdPlantaGestion, BLL.BidaiakBLL.Profiles.Agencia, idRecAdmon, True)
                    If (lUsersAgencia IsNot Nothing AndAlso lUsersAgencia.Count > 0) Then
                        Dim oUser As SabLib.ELL.Usuario
                        For Each sAgencia As String() In lUsersAgencia
                            oUser = New SabLib.ELL.Usuario With {.Id = CInt(sAgencia(0))}
                            oUser = userBLL.GetUsuario(oUser)
                            If (oUser IsNot Nothing) Then
                                If (sAgencia(1) = "0") Then 'Acceso por portal
                                    If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                                    emailsAccesoPortal &= oUser.Email
                                Else 'Acceso directo
                                    If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                                    emailsAccesoDirecto &= oUser.Email
                                End If
                            End If
                        Next
                    End If
                End If
                If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                    log.Info("CANCELACION_VIAJE: No se han encontrado ningun email de responsables de agencia para avisar de la cancelacion del viaje (V" & oViaje.IdViaje & ")")
                Else
                    body &= "<br />Acceda a bidaiak para mas informacion o para introducir algun coste de cancelacion adicional"
                    Dim https As Boolean = False
                    If (Master.IdPlantaGestion = 1 Or Master.IdPlantaGestion = 227) Then https = True 'A los de la agencia de Igorre, como estan en otra red, se les crea el link con https
                    If (emailsAccesoPortal <> String.Empty) Then
                        linkUrl = "Index.aspx?agencia=" & oViaje.IdViaje
                        subject = "Cancelacion de servicios del viaje (V" & oViaje.IdViaje & ")"
                        bodyEmail = PageBase.getBodyHmtl("Cancelacion de servicios del viaje", "V" & oViaje.IdViaje, body, linkUrl, True, https)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                        log.Info("CANCELACION_VIAJE: Se ha enviado un email a los responsables de agencia para avisar de la cancelacion del viaje (V" & oViaje.IdViaje & ") con acceso por el portal =>" & emailsAccesoPortal)
                        bNotifAgencia = True
                    End If
                    If (emailsAccesoDirecto <> String.Empty) Then
                        linkUrl = "Index_Directo.aspx?agencia=" & oViaje.IdViaje
                        subject = "Cancelacion de servicios del viaje (V" & oViaje.IdViaje & ")"
                        bodyEmail = PageBase.getBodyHmtl("Cancelacion de servicios del viaje", "V" & oViaje.IdViaje, body, linkUrl, False, https)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                        log.Info("CANCELACION_VIAJE: Se ha enviado un email a los responsables de agencia para avisar de la cancelacion del viaje (V" & oViaje.IdViaje & ")  con acceso directo =>" & emailsAccesoDirecto)
                        bNotifAgencia = True
                    End If
                End If
            Catch ex As Exception
                log.Error("CANCELACION_VIAJE: No se ha podido avisar a los responsables de agencia para avisarle de la cancelacion del viaje (V" & oViaje.IdViaje & ")", ex)
            End Try
            'Gerentes
            Try
                emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
                If (oViaje.SolicitudesPlantasFilial IsNot Nothing) Then
                    Dim gerentBLL As New BLL.BidaiakBLL
                    Dim lGerentes As New List(Of SabLib.ELL.Usuario)
                    Dim oGerente As SabLib.ELL.Usuario
                    For Each solicit As ELL.Viaje.SolicitudPlantaFilial In oViaje.SolicitudesPlantasFilial
                        oGerente = gerentBLL.loadGerentePlanta(solicit.IdPlantaFilial)
                        If (oGerente IsNot Nothing) Then
                            Dim sPerfil As String() = perfBLL.loadProfile(solicit.IdPlantaFilial, oGerente.Id, idRecAdmon)
                            If (sPerfil(1) = "0") Then 'Acceso por portal
                                If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                                emailsAccesoPortal &= oGerente.Email
                            Else 'Acceso directo
                                If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                                emailsAccesoDirecto &= oGerente.Email
                            End If
                        End If
                    Next
                End If
                If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                    log.Info("CANCELACION_VIAJE: No se han encontrado ningun email de gerentes para avisar de la cancelacion del viaje (V" & oViaje.IdViaje & ")")
                Else
                    If (emailsAccesoPortal <> String.Empty) Then
                        linkUrl = String.Empty
                        subject = "Cancelacion de las solicitudes de planta viaje (V" & oViaje.IdViaje & ")"
                        bodyEmail = PageBase.getBodyHmtl("Cancelacion de solicitud del viaje", "V" & oViaje.IdViaje, body, linkUrl, True)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                        log.Info("CANCELACION_VIAJE: Se ha enviado un email a los gerentes para avisar de la cancelacion del viaje(V" & oViaje.IdViaje & ") con acceso por el portal =>" & emailsAccesoPortal)
                        bNotifGerente = True
                    End If
                    If (emailsAccesoDirecto <> String.Empty) Then
                        linkUrl = String.Empty
                        subject = "Cancelacion de las solicitudes de planta viaje (V" & oViaje.IdViaje & ")"
                        bodyEmail = PageBase.getBodyHmtl("Cancelacion de solicitud del viaje", "V" & oViaje.IdViaje, body, linkUrl, False)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                        log.Info("CANCELACION_VIAJE: Se ha enviado un email a los gerentes para avisar de la cancelacion del viaje para avisar de la cancelacion del viaje(V" & oViaje.IdViaje & ") con acceso directo =>" & emailsAccesoDirecto)
                        bNotifGerente = True
                    End If
                End If
            Catch ex As Exception
                log.Error("CANCELACION_VIAJE: No se ha podido avisar a los gerentes de las plantas para avisarles de la cancelacion del viaje (V" & oViaje.IdViaje & ")", ex)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Se avisa a los validadores de viajes para que entren a validar el viaje
    ''' </summary>
    ''' <param name="oViaje">Viaje</param>    
    Private Sub AvisarPorEmailValidacionViaje(ByVal oViaje As ELL.Viaje)
        If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim perfBLL As New BLL.BidaiakBLL
            Dim emailsAccesoDirecto, emailsAccesoPortal, subject, body, bodyEmail, linkUrl As String
            Dim oUser As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador})
            Dim idRecAdmon As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon"))
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
            Dim destino As String = oViaje.Destino
            If (destino = String.Empty) Then

            End If
            body = oUser.NombreCompleto & " ha solicitado el viaje V" & oViaje.IdViaje & " - " & oViaje.Destino & " a realizar en las fechas " & oViaje.FechaIda.ToShortDateString & " - " & oViaje.FechaVuelta.ToShortDateString
            Try
                emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
                Dim lIdUsersVal As List(Of String) = ConfigurationManager.AppSettings("validadoresViajes").ToString.Split(";").ToList
                For Each idUserVal As Integer In lIdUsersVal
                    oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idUserVal})
                    Dim sPerfil As String() = perfBLL.loadProfile(Master.IdPlantaGestion, oUser.Id, idRecAdmon)
                    If (sPerfil(1) = "0") Then 'Acceso por portal
                        If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                        emailsAccesoPortal &= oUser.Email
                    Else 'Acceso directo
                        If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                        emailsAccesoDirecto &= oUser.Email
                    End If
                Next
                If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                    log.Info("VALIDACION_VIAJE:No se ha encontrado ningun email de los validadores de los viajes para validar el viaje (V" & oViaje.IdViaje & ")")
                Else
                    If (emailsAccesoPortal <> String.Empty) Then
                        linkUrl = "Index.aspx?valViaje=" & oViaje.IdViaje
                        subject = "Validacion del viaje (V" & oViaje.IdViaje & ")"
                        bodyEmail = PageBase.getBodyHmtl("Validacion del viaje", "V" & oViaje.IdViaje, body, linkUrl, True)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                        log.Info("VALIDACION_VIAJE: Se ha enviado un email a los validadores de viajes para avisar de que tienen que validar el viaje(V" & oViaje.IdViaje & ") con acceso por el portal =>" & emailsAccesoPortal)
                    End If
                    If (emailsAccesoDirecto <> String.Empty) Then
                        linkUrl = "Index.aspx?valViaje=" & oViaje.IdViaje
                        subject = "Validacion del viaje (V" & oViaje.IdViaje & ")"
                        bodyEmail = PageBase.getBodyHmtl("Rechazo del viaje", "V" & oViaje.IdViaje, body, linkUrl, False)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                        log.Info("VALIDACION_VIAJE: Se ha enviado un email a los validadores de viajes para avisar de que tienen que validar el viaje(V" & oViaje.IdViaje & ") con acceso directo =>" & emailsAccesoDirecto)
                    End If
                End If
            Catch ex As Exception
                log.Error("VALIDACION_VIAJE: No se ha podido avisar a los validadores de viajes de la validacion viaje (V" & oViaje.IdViaje & ")", ex)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Avisa por email a los integrantes, agencia y financiero
    ''' </summary>    
    ''' <param name="oViaje">Objeto del viaje</param>
    ''' <param name="bAvisarAgencia">Indica si debera avisar al responsable de agencia</param>
    ''' <param name="bAvisarFinanciero">Indica si debera avisar a los responsables de financiero</param>
    ''' <param name="lIntegrantesNew">Integrantes nuevos a los que hay que avisar. Tambien se avisara a su responsable</param>
    ''' <param name="lIntegrantesOld">Integrantes a los que hay que avisar de que se les ha quitado del viaje. Tambien se avisara a su responsable</param>
    ''' <param name="lIntegrantesUpdate">Integrantes a los que hay que avisar de que se les ha modificado las condiciones del viaje</param>    
    ''' <param name="bAvisarGerentes">Indicara si hay que avisar a los gerentes por una solicitud de planta filial</param>
    ''' <param name="lPlFilialesNew">Lista de plantas filiales añadidas al viaje</param>
    ''' <param name="lPlFilialesOld">Lista de plantas filiales quitadas del viaje</param>
    ''' <param name="viajeNew">True si es nuevo, false si es una modificacion</param>
    ''' <param name="bAvisarViajeRestoMundo">Indicara si hay que avisar a ciertas personas cuando el viaje sea nuevo y al resto del mundo</param>
    Private Sub AvisarPorEmail(ByVal oViaje As ELL.Viaje, ByVal bAvisarAgencia As Boolean, ByVal bAvisarFinanciero As Boolean, ByVal lIntegrantesNew As List(Of SabLib.ELL.Usuario), ByVal lIntegrantesOld As List(Of SabLib.ELL.Usuario), ByVal lIntegrantesUpdate As List(Of SabLib.ELL.Usuario), ByVal bAvisarGerentes As Boolean, ByVal lPlFilialesNew As List(Of Integer), ByVal lPlFilialesOld As List(Of Integer), ByVal viajeNew As Boolean, ByVal bAvisarViajeRestoMundo As Boolean)
        If (ConfigurationManager.AppSettings("avisarPorEmail") = "1" AndAlso (bAvisarAgencia OrElse bAvisarFinanciero OrElse lIntegrantesNew IsNot Nothing OrElse lIntegrantesOld IsNot Nothing OrElse lIntegrantesUpdate IsNot Nothing OrElse bAvisarGerentes)) Then
            Dim idViaje As Integer = oViaje.IdViaje
            Dim perfBLL As New BLL.BidaiakBLL
            Dim emailsAccesoDirecto, emailsAccesoPortal, subject, body, bodyEmail, linkUrl As String
            Dim idRecAdmon As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon"))
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
            If (lIntegrantesNew IsNot Nothing OrElse lIntegrantesOld IsNot Nothing OrElse lIntegrantesUpdate IsNot Nothing) Then
                If (lIntegrantesNew IsNot Nothing) Then AvisarIntegrantesResp(oViaje, lIntegrantesNew, 0)
                If (lIntegrantesOld IsNot Nothing) Then AvisarIntegrantesResp(oViaje, lIntegrantesOld, 1)
                If (lIntegrantesUpdate IsNot Nothing) Then AvisarIntegrantesResp(oViaje, lIntegrantesUpdate, 2)
            End If
            If (bAvisarAgencia) Then
                Try
                    emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
                    Dim lUsersAgencia As List(Of String()) = perfBLL.loadUsersProfile(Master.IdPlantaGestion, BLL.BidaiakBLL.Profiles.Agencia, idRecAdmon, True)
                    If (lUsersAgencia IsNot Nothing AndAlso lUsersAgencia.Count > 0) Then
                        Dim userBLL As New SabLib.BLL.UsuariosComponent
                        Dim oUser As SabLib.ELL.Usuario
                        For Each sAgencia As String() In lUsersAgencia
                            oUser = New SabLib.ELL.Usuario With {.Id = CInt(sAgencia(0))}
                            oUser = userBLL.GetUsuario(oUser)
                            If (oUser IsNot Nothing) Then
                                If (sAgencia(1) = "0") Then 'Acceso por portal
                                    If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                                    emailsAccesoPortal &= oUser.Email
                                Else 'Acceso directo
                                    If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                                    emailsAccesoDirecto &= oUser.Email
                                End If
                            End If
                        Next
                    End If
                    If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                        log.Info("SOLICITUD_VIAJE:No se han encontrado ningun email para avisar a la agencia de la nueva/modificacion de solicitud (V" & idViaje & ")")
                    Else
                        body = "Se han solicitado/modificado fechas o servicios de agencia para el viaje indicado. Acceda a bidaiak para mas informacion"
                        Dim https As Boolean = False
                        If (Master.IdPlantaGestion = 1 Or Master.IdPlantaGestion = 227) Then https = True 'A los de la agencia de Igorre, como estan en otra red, se les crea el link con https
                        If (emailsAccesoPortal <> String.Empty) Then
                            linkUrl = "Index.aspx?agencia=" & idViaje
                            subject = "Solicitud de servicios (V" & idViaje & ")"
                            bodyEmail = PageBase.getBodyHmtl("Solicitud de servicios de agencia", "V" & idViaje, body, linkUrl, True, https)
                            SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                            log.Info("SOLICITUD_VIAJE:Se ha enviado un email al responsable de agencia para avisarle de la nueva/modificacion de solicitud (V" & idViaje & ") con acceso por el portal => " & emailsAccesoPortal)
                            bNotifAgencia = True
                        End If
                        If (emailsAccesoDirecto <> String.Empty) Then
                            linkUrl = "Index_Directo.aspx?agencia=" & idViaje
                            subject = "Solicitud de servicios (V" & idViaje & ")"
                            bodyEmail = PageBase.getBodyHmtl("Solicitud de servicios de agencia", "V" & idViaje, body, linkUrl, False, https)
                            SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                            log.Info("SOLICITUD_VIAJE:Se ha enviado un email al responsable de agencia para avisarle de la nueva/modificacion de solicitud (V" & idViaje & ") con acceso directo => " & emailsAccesoDirecto)
                            bNotifAgencia = True
                        End If
                    End If
                Catch ex As Exception
                    log.Error("SOLICITUD_VIAJE: No se ha podido avisar al responsable de agencia para avisarle de la nueva/modificacion de solicitud (V" & idViaje & ")", ex)
                End Try
            End If
            If (bAvisarFinanciero) Then
                Try
                    emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
                    Dim lUsersFinan As List(Of String()) = perfBLL.loadUsersProfile(Master.IdPlantaGestion, BidaiakLib.BLL.BidaiakBLL.Profiles.Gestor_Anticipos, idRecAdmon, True)
                    If (lUsersFinan IsNot Nothing AndAlso lUsersFinan.Count > 0) Then
                        Dim userBLL As New SabLib.BLL.UsuariosComponent
                        Dim oUser As SabLib.ELL.Usuario
                        For Each sFinanciero As String() In lUsersFinan
                            oUser = New SabLib.ELL.Usuario With {.Id = CInt(sFinanciero(0))}
                            oUser = userBLL.GetUsuario(oUser)
                            If (oUser IsNot Nothing) Then
                                If (sFinanciero(1) = "0") Then 'Acceso por portal
                                    If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                                    emailsAccesoPortal &= oUser.Email
                                Else 'Acceso directo
                                    If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                                    emailsAccesoDirecto &= oUser.Email
                                End If
                            End If
                        Next
                    End If
                    If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                        log.Info("SOLICITUD_VIAJE:No se han encontrado ningun email para avisar a financiero de la nueva/modificacion de solicitud (V" & idViaje & ")")
                    Else
                        body = "Se han solicitado/modificado anticipos para el viaje indicado. Acceda a bidaiak para mas informacion"
                        If (emailsAccesoPortal <> String.Empty) Then
                            linkUrl = "Index.aspx?anticipo=" & idViaje
                            subject = "Solicitud de anticipos (V" & idViaje & ")"
                            bodyEmail = PageBase.getBodyHmtl("Solicitud de anticipos", "V" & idViaje, body, linkUrl, True)
                            SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                            log.Info("SOLICITUD_VIAJE:Se ha enviado un email al responsable de financiero para avisarle de la nueva solicitud de anticipos(V" & idViaje & ") con acceso por el portal => " & emailsAccesoPortal)
                            bNotifAdmin = True
                        End If
                        If (emailsAccesoDirecto <> String.Empty) Then
                            linkUrl = "Index_Directo.aspx?anticipo=" & idViaje
                            subject = "Solicitud de anticipos (V" & idViaje & ")"
                            bodyEmail = PageBase.getBodyHmtl("Solicitud de anticipos", "V" & idViaje, body, linkUrl, False)
                            SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                            log.Info("SOLICITUD_VIAJE:Se ha enviado un email al responsable de financiero para avisarle de la nueva/modificacion de la solicitud de anticipos(V" & idViaje & ") con acceso directo => " & emailsAccesoDirecto)
                            bNotifAdmin = True
                        End If
                    End If
                Catch ex As Exception
                    log.Error("SOLICITUD_VIAJE: No se ha podido avisar al responsable de financiero para avisarle de la nueva/modificacion de la solicitud de anticipos(V" & idViaje & ")", ex)
                End Try
            End If
            If (bAvisarGerentes) Then
                If (lPlFilialesNew IsNot Nothing) Then
                    For Each idPlanta As Integer In lPlFilialesNew
                        AvisarGerentes(oViaje, idPlanta, 0)
                    Next
                End If
                If (lPlFilialesOld IsNot Nothing) Then
                    For Each idPlanta As Integer In lPlFilialesOld
                        AvisarGerentes(oViaje, idPlanta, 1)
                    Next
                End If
            End If
            'Se comprueba si hay alguna persona mas a comunicar por el coronavirus. Solo se avisan cuando es un viaje nuevo
            Dim idUsersCoronavirus As String = ConfigurationManager.AppSettings("avisarCoronavirus")
            If Not (String.IsNullOrEmpty(idUsersCoronavirus) AndAlso viajeNew) Then
                AvisarCoronavirus(oViaje, idUsersCoronavirus)
            End If
            If (bAvisarViajeRestoMundo) Then
                emailsAccesoPortal = ConfigurationManager.AppSettings("avisarViajeRestoMundo")
                subject = "Solicitud de viaje al resto del mundo"
                body = "Se ha solicitado el viaje V" & idViaje & " con destino resto del mundo<br /><ul>"
                For Each inte In lIntegrantesNew
                    body &= "<ul>" & inte.NombreCompletoYCodpersona & "</ul>"
                Next
                body &= "</ul>"
                bodyEmail = PageBase.getBodyHmtl("Solicitud de viaje", "V" & idViaje, body, String.Empty, False)
                SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                log.Info("SOLICITUD_VIAJE:Se ha enviado un email a las personas que quieren ser avisadas al realizar un viaje al resto del mundo (V" & idViaje & ") => " & emailsAccesoPortal)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Se avisa a los integrantes y a sus responsables de que se les ha añadido o quitado del viaje
    ''' </summary>
    ''' <param name="oViaje">Informacion del viaje</param>
    ''' <param name="lIntegrantes">Integrantes</param>
    ''' <param name="Opcion">0:Convocarle a un nuevo viaje ,1:Cancelar un viaje, 2:Cambiarle las condiciones</param>
    Private Sub AvisarIntegrantesResp(ByVal oViaje As ELL.Viaje, ByVal lIntegrantes As List(Of SabLib.ELL.Usuario), ByVal opcion As Integer)
        Dim body, bodyEmail, linkUrl, subject As String
        Dim bidaiakBLL As New BLL.BidaiakBLL
        Dim idRecurso As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon"))
        body = String.Empty
        Dim paramBLL As New SabLib.BLL.ParametrosBLL
        Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
        Try
            Dim emailsAccesoDirecto, emailsAccesoPortal As String
            emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
            For Each oUser As SabLib.ELL.Usuario In lIntegrantes
                If (oUser.Id <> oViaje.IdUserSolicitador) Then  'Al que organiza no le llegara un email de que ha sido convocado a un viaje
                    Dim sPerfil As String() = bidaiakBLL.loadProfile(Master.IdPlantaGestion, oUser.Id, idRecurso)
                    If (sPerfil(1) = "0") Then 'Acceso por portal
                        If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                        emailsAccesoPortal &= oUser.Email
                    Else 'Acceso directo
                        If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                        emailsAccesoDirecto &= oUser.Email
                    End If
                End If
            Next
            If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                If (opcion = 0 Or opcion = 2) Then
                    log.Info("SOLICITUD_VIAJE:No se han encontrado ningun email para avisar a los integrantes de que se les ha convocado/modificado al viaje (V" & oViaje.IdViaje & ")")
                ElseIf (opcion = 1) Then
                    log.Info("SOLICITUD_VIAJE:No se han encontrado ningun email para avisar a los integrantes de que se les ha cancelado el viaje (V" & oViaje.IdViaje & ")")
                End If
            Else
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim myUser As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador})
                Dim solicitante As String = myUser.NombreCompleto
                If (opcion = 0) Then
                    body = solicitante & " le ha programado un viaje a " & oViaje.Destino & " del " & oViaje.FechaIda.ToShortDateString & " al " & oViaje.FechaVuelta.ToShortDateString & " . Acceda a bidaiak para mas informacion"
                ElseIf (opcion = 1) Then
                    body = solicitante & " le ha cancelado el viaje a " & oViaje.Destino & " del " & oViaje.FechaIda.ToShortDateString & " al " & oViaje.FechaVuelta.ToShortDateString
                ElseIf (opcion = 2) Then
                    body = solicitante & " ha cambiado las condiciones del viaje. Acceda a bidaiak para mas informacion"
                End If
                subject = "Viaje (V" & oViaje.IdViaje & ")"
                linkUrl = String.Empty
                If (emailsAccesoPortal <> String.Empty) Then
                    linkUrl = If(opcion = 0 Or opcion = 2, "Index.aspx?solViaje=" & oViaje.IdViaje, String.Empty)
                    bodyEmail = PageBase.getBodyHmtl("Viaje", "V" & oViaje.IdViaje, body, linkUrl, True)
                    SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                    bNotifInteg = True
                    If (opcion = 0 Or opcion = 2) Then
                        log.Info("SOLICITUD_VIAJE:Se ha enviado un email para indicar a los integrantes de que se les ha convocado/modificado al viaje (V" & oViaje.IdViaje & ") con acceso por el portal => " & emailsAccesoPortal)
                    ElseIf (opcion = 1) Then
                        log.Info("SOLICITUD_VIAJE:Se ha enviado un email para indicar a los integrantes de que se les ha cancelado el viaje (V" & oViaje.IdViaje & ") con acceso por el portal => " & emailsAccesoPortal)
                    End If
                End If
                If (emailsAccesoDirecto <> String.Empty) Then
                    linkUrl = If(opcion = 0 Or opcion = 2, "Index_Directo.aspx?solViaje=" & oViaje.IdViaje, String.Empty)
                    bodyEmail = PageBase.getBodyHmtl("Viaje", "V" & oViaje.IdViaje, body, linkUrl, False)
                    SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                    bNotifInteg = True
                    If (opcion = 0 Or opcion = 2) Then
                        log.Info("SOLICITUD_VIAJE:Se ha enviado un email para indicar a los integrantes de que se les ha convocado/modificado al viaje (V" & oViaje.IdViaje & ") con acceso directo => " & emailsAccesoDirecto)
                    ElseIf (opcion = 1) Then
                        log.Info("SOLICITUD_VIAJE:Se ha enviado un email para indicar a los integrantes de que se les ha cancelado el viaje (V" & oViaje.IdViaje & ") con acceso directo => " & emailsAccesoDirecto)
                    End If
                End If
            End If
        Catch ex As Exception
            If (opcion = 0 Or opcion = 2) Then
                log.Error("SOLICITUD_VIAJE: No se ha podido avisar a los integrantes de que se les ha convocado/modificado al viaje (V" & oViaje.IdViaje & ")", ex)
            ElseIf (opcion = 1) Then
                log.Error("SOLICITUD_VIAJE: No se ha podido avisar a los integrantes de que se les ha cancelado el viaje (V" & oViaje.IdViaje & ")", ex)
            End If
        End Try
        'Se avisa a los responsables
        Try
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim oUser As SabLib.ELL.Usuario
            Dim lRespPupilos As New Dictionary(Of Integer, List(Of SabLib.ELL.Usuario))
            Dim lUsers As List(Of SabLib.ELL.Usuario)
            Dim idResp As Integer
            'Se guarda un dicccionario con el idResponsable como key y una lista de sus pupilos como value
            For Each oUser In lIntegrantes
                idResp = bidaiakBLL.GetResponsable(oUser.IdDepartamento, oUser.IdResponsable, Master.IdPlantaGestion)
                If (lRespPupilos.ContainsKey(idResp)) Then
                    lRespPupilos.Item(idResp).Add(oUser)
                Else
                    lUsers = New List(Of SabLib.ELL.Usuario)
                    lUsers.Add(oUser)
                    lRespPupilos.Add(idResp, lUsers)
                End If
            Next
            If (lRespPupilos.Count = 0) Then
                If (opcion = 0 Or opcion = 2) Then
                    log.Info("SOLICITUD_VIAJE:No se ha encontrado ningun email para avisar a los responsables de que se les ha convocado/modificado a los integrantes el viaje (V" & oViaje.IdViaje & ")")
                ElseIf (opcion = 1) Then
                    log.Info("SOLICITUD_VIAJE:No se ha encontrado ningun email para avisar a los responsables de que se les ha cancelado a los integrantes el viaje (V" & oViaje.IdViaje & ")")
                End If
            Else
                'Se compone el body. Habra que recorrerse la coleccion para formar el mensaje                
                If (opcion = 0) Then
                    body = lblPropietario.Text & " ha organizado un viaje a " & oViaje.Destino & " del " & oViaje.FechaIda.ToShortDateString & " al " & oViaje.FechaVuelta.ToShortDateString & " para las siguientes personas: " & "<br />"
                ElseIf (opcion = 1) Then
                    body = lblPropietario.Text & " ha cancelado el viaje a " & oViaje.Destino & " del " & oViaje.FechaIda.ToShortDateString & " al " & oViaje.FechaVuelta.ToShortDateString & " a las siguientes personas: " & "<br />"
                ElseIf (opcion = 2) Then
                    body = lblPropietario.Text & " ha cambiado las condiciones del viaje a " & oViaje.Destino & " del " & oViaje.FechaIda.ToShortDateString & " al " & oViaje.FechaVuelta.ToShortDateString & " para las siguientes personas: " & "<br />"
                End If
                'Acceda a bidaiak para mas informacion"
                Dim lSABRespPupilos As New Dictionary(Of SabLib.ELL.Usuario, List(Of SabLib.ELL.Usuario))
                Dim respPupils As Dictionary(Of Integer, List(Of SabLib.ELL.Usuario)).Enumerator = lRespPupilos.GetEnumerator
                While respPupils.MoveNext
                    If (respPupils.Current.Key <> oViaje.IdUserSolicitador) Then  'Al organizador no le llegara un email de que se le ha convocado a su gente
                        oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = respPupils.Current.Key}, False)
                        lSABRespPupilos.Add(oUser, respPupils.Current.Value)
                        body &= "<br />" & "Dependientes de " & oUser.NombreCompleto & "<br />"
                        For Each oUser In respPupils.Current.Value
                            body &= " - " & oUser.NombreCompleto & "<br />"
                        Next
                    End If
                End While
                lRespPupilos = Nothing
                Dim respSABPupils As Dictionary(Of SabLib.ELL.Usuario, List(Of SabLib.ELL.Usuario)).Enumerator = lSABRespPupilos.GetEnumerator
                Dim notas As String = If(pnlDiasAntelacionNoOK.Visible, "¡¡El viaje se ha planificado fuera de plazo!!", String.Empty)
                While respSABPupils.MoveNext
                    Try
                        Dim sPerfil As String() = bidaiakBLL.loadProfile(Master.IdPlantaGestion, respSABPupils.Current.Key.Id, idRecurso)
                        subject = "Viaje (V" & oViaje.IdViaje & ")"
                        linkUrl = String.Empty
                        If (sPerfil(1) = "0") Then 'Acceso por portal                            
                            bodyEmail = PageBase.getBodyHmtl("Viaje", "V" & oViaje.IdViaje, body, linkUrl, True, notas:=notas)
                        Else 'Acceso directo                                                            
                            bodyEmail = PageBase.getBodyHmtl("Viaje", "V" & oViaje.IdViaje, body, linkUrl, False, notas:=notas)
                        End If
                        Dim emailTo As String = respSABPupils.Current.Key.Email
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailTo, subject, bodyEmail, serverEmail)
                        If (opcion = 0 Or opcion = 2) Then
                            log.Info("SOLICITUD_VIAJE:Se ha enviado un email al responsable " & respSABPupils.Current.Key.NombreCompleto & " para avisar de que han convocado/modificado a su gente el viaje (V" & oViaje.IdViaje & ") ")
                        ElseIf (opcion = 1) Then
                            log.Info("SOLICITUD_VIAJE:Se ha enviado un email al responsable " & respSABPupils.Current.Key.NombreCompleto & " para avisar de que le han cancelado a su gente el viaje (V" & oViaje.IdViaje & ") ")
                        End If
                    Catch ex As Exception
                        If (opcion = 0 Or opcion = 2) Then
                            log.Error("SOLICITUD_VIAJE: No se ha podido avisar al responsable " & respSABPupils.Current.Key.NombreCompleto & " de que le han convocado/modificado a su gente el viaje (V" & oViaje.IdViaje & ")", ex)
                        ElseIf (opcion = 1) Then
                            log.Error("SOLICITUD_VIAJE: No se ha podido avisar al responsable " & respSABPupils.Current.Key.NombreCompleto & " de que le han cancelado a su gente el viaje (V" & oViaje.IdViaje & ")", ex)
                        End If
                    End Try
                End While
                lSABRespPupilos = Nothing
                If (pnlDiasAntelacionNoOK.Visible) Then '12/06/2018: Se avisa a Roberto Eguia de que se ha planificado un viaje sin antelacion
                    oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador})
                    bodyEmail = "El viaje V" & oViaje.IdViaje & " con destino " & oViaje.Destino & " (" & [Enum].GetName(GetType(ELL.Viaje.eNivel), oViaje.Nivel).Replace("_", " ") & ") y fechas " & oViaje.FechaIda.ToShortDateString & " - " & oViaje.FechaVuelta.ToShortDateString & "  planificado por " & oUser.NombreCompleto & ", no se creado con la antelacion suficiente"
                    SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), "reguia@batz.es", "Viaje planificado sin antelacion suficiente", bodyEmail, serverEmail)
                End If
            End If
        Catch ex As Exception
            If (opcion = 0 Or opcion = 2) Then
                log.Error("SOLICITUD_VIAJE: No se ha podido avisar a los responsables de los integrantes de que les han convocado/modificado el viaje (V" & oViaje.IdViaje & ")", ex)
            ElseIf (opcion = 1) Then
                log.Error("SOLICITUD_VIAJE: No se ha podido avisar a los responsables de los integrantes que ses le han cancelado el viaje (V" & oViaje.IdViaje & ")", ex)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Se avisa a los integrantes y a sus responsables de que se les ha añadido o quitado del viaje
    ''' </summary>
    ''' <param name="oViaje">Informacion del viaje</param>
    ''' <param name="idPlanta">Id de la planta</param>
    ''' <param name="Opcion">0:Nueva solicitud ,1:Cancelacion de una solicitud</param>
    Private Sub AvisarGerentes(ByVal oViaje As ELL.Viaje, ByVal idPlanta As Integer, ByVal opcion As Integer)
        Dim body, bodyEmail, linkUrl, subject As String
        Dim perfBLL As New BLL.BidaiakBLL
        Dim idRecurso As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon"))
        body = String.Empty
        Dim paramBLL As New SabLib.BLL.ParametrosBLL
        Dim gerentBLL As New BLL.BidaiakBLL
        Dim userBLL As New SabLib.BLL.UsuariosComponent
        Dim plantBLL As New SabLib.BLL.PlantasComponent
        Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
        Dim nombrePlanta As String = String.Empty
        Try
            Dim emailsAccesoDirecto, emailsAccesoPortal As String
            emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
            nombrePlanta = plantBLL.GetPlanta(idPlanta).Nombre
            Dim oGerente As SabLib.ELL.Usuario = gerentBLL.loadGerentePlanta(idPlanta)
            If (oGerente IsNot Nothing) Then
                Dim sPerfil As String() = perfBLL.loadProfile(idPlanta, oGerente.Id, idRecurso) 'Se busca el recurso en la planta del gerente
                If (sPerfil(1) = "0") Then 'Acceso por portal
                    If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                    emailsAccesoPortal &= oGerente.Email
                Else 'Acceso directo
                    If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                    emailsAccesoDirecto &= oGerente.Email
                End If
            End If
            If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                If (opcion = 0) Then
                    log.Info("SOLICITUD_VIAJE:No se han encontrado ningun email para avisar al gerente de la nueva solicitud de planta (" & nombrePlanta & ") del viaje (V" & oViaje.IdViaje & ")")
                ElseIf (opcion = 1) Then
                    log.Info("SOLICITUD_VIAJE:No se han encontrado ningun email para avisar a los gerentes de que se les ha cancelado la solicitud de planta (" & nombrePlanta & ") del viaje (V" & oViaje.IdViaje & ")")
                End If
            Else
                Dim titulo As String = String.Empty
                subject = String.Empty
                Dim myUser As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador})
                Dim solicitante As String = myUser.NombreCompleto
                If (opcion = 0) Then
                    body = solicitante & " le ha realizado una solicitud para la planta '" & nombrePlanta & "' de viaje " & oViaje.Destino & " del " & oViaje.FechaIda.ToShortDateString & " al " & oViaje.FechaVuelta.ToShortDateString & " . Acceda a bidaiak para mas informacion"
                    subject = "Solicitud de planta '" & nombrePlanta & "' (V" & oViaje.IdViaje & ")"
                    titulo = "Solicitud de planta"
                ElseIf (opcion = 1) Then
                    body = solicitante & " le ha cancelado la solicitud para la planta '" & nombrePlanta & "' del viaje " & oViaje.Destino & " del " & oViaje.FechaIda.ToShortDateString & " al " & oViaje.FechaVuelta.ToShortDateString
                    subject = "Cancelacion de la solicitud de planta '" & nombrePlanta & "' (V" & oViaje.IdViaje & ")"
                    titulo = "Cancelacion de solicitud de planta"
                End If
                linkUrl = String.Empty
                Dim paramUrl As String = "?solPlanta=" & oViaje.IdViaje & "|" & idPlanta
                If (emailsAccesoPortal <> String.Empty) Then
                    linkUrl = If(opcion = 0, "Index.aspx" & paramUrl, String.Empty)
                    bodyEmail = PageBase.getBodyHmtl(titulo, "V" & oViaje.IdViaje, body, linkUrl, True)
                    SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                    bNotifGerente = True
                    If (opcion = 0) Then
                        log.Info("SOLICITUD_VIAJE:Se ha enviado un email para indicar al gerente de que se le ha realizado una solicitud de planta (" & nombrePlanta & ") del viaje (V" & oViaje.IdViaje & ") con acceso por el portal => " & emailsAccesoPortal)
                    ElseIf (opcion = 1) Then
                        log.Info("SOLICITUD_VIAJE:Se ha enviado un email para indicar al gerente de que se le ha cancelado la solicitud de planta (" & nombrePlanta & ") del viaje (V" & oViaje.IdViaje & ") con acceso por el portal => " & emailsAccesoPortal)
                    End If
                End If
                If (emailsAccesoDirecto <> String.Empty) Then
                    linkUrl = If(opcion = 0, "Index_Directo.aspx" & paramUrl, String.Empty)
                    bodyEmail = PageBase.getBodyHmtl(titulo, "V" & oViaje.IdViaje, body, linkUrl, False)
                    SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                    bNotifGerente = True
                    If (opcion = 0) Then
                        log.Info("SOLICITUD_VIAJE:Se ha enviado un email para indicar al gerente de que se le ha realizado una solicitud de planta (" & nombrePlanta & ") del viaje (V" & oViaje.IdViaje & ") con acceso directo => " & emailsAccesoDirecto)
                    ElseIf (opcion = 1) Then
                        log.Info("SOLICITUD_VIAJE:Se ha enviado un email para indicar al gerente de que se le ha cancelado la solicitud de planta (" & nombrePlanta & ") del viaje (V" & oViaje.IdViaje & ") con acceso directo => " & emailsAccesoDirecto)
                    End If
                End If
            End If
        Catch ex As Exception
            If (opcion = 0) Then
                log.Error("SOLICITUD_PLANTA: No se ha podido avisar a los gerentes de la nueva solicitud de planta (" & nombrePlanta & ") del viaje (V" & oViaje.IdViaje & ")", ex)
            ElseIf (opcion = 1) Then
                log.Error("SOLICITUD_VIAJE: No se ha podido avisar a los gerentes de que se les ha cancelado la solicitud de planta (" & nombrePlanta & ") del viaje (V" & oViaje.IdViaje & ")", ex)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Se avisa a los integrantes y a sus responsables de que se les ha añadido o quitado del viaje
    ''' </summary>
    ''' <param name="oViaje">Informacion del viaje</param>
    ''' <param name="idUsersAviso">Id de los usuarios a avisar separados por comas</param>
    Private Sub AvisarCoronavirus(ByVal oViaje As ELL.Viaje, ByVal idUsersAviso As String)
        Dim body, bodyEmail, linkUrl, subject As String
        Dim perfBLL As New BLL.BidaiakBLL
        Dim idRecurso As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon"))
        body = String.Empty
        Dim paramBLL As New SabLib.BLL.ParametrosBLL
        Dim userBLL As New SabLib.BLL.UsuariosComponent
        Dim plantBLL As New SabLib.BLL.PlantasComponent
        Dim userAviso As SabLib.ELL.Usuario
        Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
        Dim nombrePlanta As String = String.Empty
        Dim sPerfil As String()
        Try
            Dim emailsAccesoDirecto, emailsAccesoPortal As String
            emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
            Dim usersAviso As String() = idUsersAviso.Split(";")
            For Each userA In usersAviso
                userAviso = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(userA)})
                If (userAviso IsNot Nothing) Then
                    sPerfil = perfBLL.loadProfile(oViaje.IdPlanta, userAviso.Id, idRecurso)
                    If (sPerfil(1) = "0") Then 'Acceso por portal
                        If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                        emailsAccesoPortal &= userAviso.Email
                    Else 'Acceso directo
                        If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                        emailsAccesoDirecto &= userAviso.Email
                    End If
                End If
            Next
            If Not (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                Dim titulo As String = String.Empty
                subject = String.Empty
                Dim myUser As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador})
                Dim solicitante As String = myUser.NombreCompleto
                'If (opcion = 0) Then
                body = solicitante & " ha realizado una solicitud de viaje a " & oViaje.Destino & " del " & oViaje.FechaIda.ToShortDateString & " al " & oViaje.FechaVuelta.ToShortDateString & " . Acceda a bidaiak para mas informacion"
                subject = "Solicitud de viaje (V" & oViaje.IdViaje & ")"
                titulo = "Solicitud de viaje"
                'ElseIf (opcion = 1) Then
                '    body = solicitante & " le ha cancelado la solicitud para la planta '" & nombrePlanta & "' del viaje " & oViaje.Destino & " del " & oViaje.FechaIda.ToShortDateString & " al " & oViaje.FechaVuelta.ToShortDateString
                '    subject = "Cancelacion de la solicitud de planta '" & nombrePlanta & "' (V" & oViaje.IdViaje & ")"
                '    titulo = "Cancelacion de solicitud de planta"
                'linkUrl = String.Empty
                Dim paramUrl As String = "?solViaje=" & oViaje.IdViaje
                If (emailsAccesoPortal <> String.Empty) Then
                    linkUrl = "Index.aspx" & paramUrl
                    bodyEmail = PageBase.getBodyHmtl(titulo, "V" & oViaje.IdViaje, body, linkUrl, True)
                    SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                    'If (opcion = 0) Then
                    log.Info("SOLICITUD_VIAJE:Se ha enviado un email para indicar a las personas por el tema del coronavirus,  de que se le ha realizado una solicitud de viaje (V" & oViaje.IdViaje & ") con acceso por el portal => " & emailsAccesoPortal)
                    'ElseIf (opcion = 1) Then
                    'log.Info("SOLICITUD_VIAJE:Se ha enviado un email para indicar al gerente de que se le ha cancelado la solicitud de planta (" & nombrePlanta & ") del viaje (V" & oViaje.IdViaje & ") con acceso por el portal => " & emailsAccesoPortal)
                    'End If
                End If
                If (emailsAccesoDirecto <> String.Empty) Then
                    linkUrl = "Index_Directo.aspx" & paramUrl
                    bodyEmail = PageBase.getBodyHmtl(titulo, "V" & oViaje.IdViaje, body, linkUrl, False)
                    SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                    'If (opcion = 0) Then
                    log.Info("SOLICITUD_VIAJE:Se ha enviado un email para indicar a las personas por el tema del coronavirus,  de que se le ha realizado una solicitud de viaje (V" & oViaje.IdViaje & ") con acceso directo => " & emailsAccesoDirecto)
                    'ElseIf (opcion = 1) Then
                    'log.Info("SOLICITUD_VIAJE:Se ha enviado un email para indicar al gerente de que se le ha cancelado la solicitud de planta (" & nombrePlanta & ") del viaje (V" & oViaje.IdViaje & ") con acceso directo => " & emailsAccesoDirecto)
                    'End If
                End If
            End If
        Catch ex As Exception
            'If (opcion = 0) Then
            log.Error("SOLICITUD_CORONAVIRUS: No se ha podido avisar a las personas por el tema del coronavirus de la nueva solicitud de de viaje (V" & oViaje.IdViaje & ")", ex)
            'ElseIf (opcion = 1) Then
            'log.Error("SOLICITUD_VIAJE: No se ha podido avisar a los gerentes de que se les ha cancelado la solicitud de planta (" & nombrePlanta & ") del viaje (V" & oViaje.IdViaje & ")", ex)
            'End If
        End Try
    End Sub

    ''' <summary>
    ''' Avisa por email a administracion para avisarle de que un departamento no tiene activadades asociadas
    ''' </summary>    
    ''' <param name="organizador">Organizador del viaje</param>
    ''' <param name="asistenteSinActiv">Nombre del asistente que se ha intentado añadir</param>
    ''' <param name="departamento">Nombre del departamento sin actividades</param>
    ''' <param name="idViaje">Id del viaje</param>
    Private Sub AvisarDepartamentoSinActividades(ByVal organizador As String, ByVal asistenteSinActiv As String, ByVal departamento As String, ByVal idViaje As String)
        If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim perfBLL As New BLL.BidaiakBLL
            Dim emailsAccesoDirecto, emailsAccesoPortal, subject, body, bodyEmail, linkUrl As String
            Dim idRecAdmon As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon"))
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
            body = organizador.ToUpper & " ha intentado agregar a " & asistenteSinActiv.ToUpper & " al viaje " & idViaje & " pero no ha podido ya que el departamento " & departamento.ToUpper & " no tiene actividades asociadas.<br /><br />"
            body &= "Una vez registradas las actividades, comuniqueselo a " & organizador.ToUpper & " para que pueda finalizar la planificacion del viaje"
            Try
                emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
                Dim lUsersFinan As List(Of String()) = perfBLL.loadUsersProfile(Master.IdPlantaGestion, BidaiakLib.BLL.BidaiakBLL.Profiles.Financiero, idRecAdmon, True)
                If (lUsersFinan IsNot Nothing AndAlso lUsersFinan.Count > 0) Then
                    Dim oUser As SabLib.ELL.Usuario
                    For Each sFinanciero As String() In lUsersFinan
                        oUser = New SabLib.ELL.Usuario With {.Id = CInt(sFinanciero(0))}
                        oUser = userBLL.GetUsuario(oUser)
                        If (oUser IsNot Nothing) Then
                            If (sFinanciero(1) = "0") Then 'Acceso por portal
                                If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                                emailsAccesoPortal &= oUser.Email
                            Else 'Acceso directo
                                If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                                emailsAccesoDirecto &= oUser.Email
                            End If
                        End If
                    Next
                End If
                If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                    log.Info("DEPARTAMENTO_SIN_ACTIVIDADES:No se han encontrado ningun email de financiero para avisar del departamento sin actividades - " & departamento)
                Else
                    subject = "Departamento sin actividades"
                    If (emailsAccesoPortal <> String.Empty) Then
                        linkUrl = String.Empty
                        bodyEmail = PageBase.getBodyHmtl("Departamento sin actividades", idViaje, body, linkUrl, True)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                        log.Info("DEPARTAMENTO_SIN_ACTIVIDADES: Se ha enviado un email a los responsables financieros para que registren las actividades del departamento " & departamento & " con acceso por el portal =>" & emailsAccesoPortal)
                    End If
                    If (emailsAccesoDirecto <> String.Empty) Then
                        linkUrl = String.Empty
                        bodyEmail = PageBase.getBodyHmtl("Departamento sin actividades", idViaje, body, linkUrl, False)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                        log.Info("DEPARTAMENTO_SIN_ACTIVIDADES: Se ha enviado un email a los responsables financieros para que registren las actividades del departamento " & departamento & " con acceso directo =>" & emailsAccesoDirecto)
                    End If
                End If
            Catch ex As Exception
                log.Error("DEPARTAMENTO_SIN_ACTIVIDADES: No se ha podido avisar a los de administracion para que registren las activades del departamento " & departamento, ex)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Escribe en el log los cambios realizados
    ''' </summary>
    ''' <param name="oViajeNew">Viaje nuevo</param>
    ''' <param name="oViajeOld">Viaje antiguo</param>    
    Private Sub WriteChanges(ByVal oViajeNew As ELL.Viaje, ByVal oViajeOld As ELL.Viaje)
        Try
            Dim mensa As New Text.StringBuilder
            mensa.AppendLine("SOLICITUD_VIAJE:El usuario " & Master.Ticket.NombreCompleto & " (" & Master.Ticket.IdUser & ") ha realizado los siguientes cambios del viaje (" & oViajeNew.IdViaje & ")")
            'Cabecera
            If (oViajeNew.FechaIda <> oViajeOld.FechaIda) Then mensa.AppendLine("-Fecha de ida")
            If (oViajeNew.FechaVuelta <> oViajeOld.FechaVuelta) Then mensa.AppendLine("-Fecha de vuelta")
            If (oViajeNew.Destino <> oViajeOld.Destino) Then mensa.AppendLine("-Destino")
            If (oViajeNew.Nivel <> oViajeOld.Nivel) Then mensa.AppendLine("-Nivel(Europeo,nacional,...)")
            If (oViajeNew.Descripcion <> oViajeOld.Descripcion) Then mensa.AppendLine("-Descripcion")
            'Unidad Organizativa
            If (oViajeNew.UnidadOrganizativa.Id <> oViajeOld.UnidadOrganizativa.Id) Then mensa.AppendLine("-Unidad organizativa")
            If (oViajeNew.Proyectos.Count <> oViajeOld.Proyectos.Count) Then mensa.AppendLine("-Con/Sin Proyecto")
            Dim idProg, porcentaje As Integer
            Dim numOf As String
            For Each oTrip As ELL.Viaje.Proyecto In oViajeNew.Proyectos
                idProg = oTrip.IdPrograma : porcentaje = oTrip.Porcentaje : numOf = oTrip.NumOF
                If (Not oViajeOld.Proyectos.Exists(Function(o As ELL.Viaje.Proyecto) o.IdPrograma = idProg And o.NumOF = numOf And o.Porcentaje = porcentaje)) Then
                    mensa.AppendLine("-Se ha insertado algun/quitado algun proyecto o se ha cambiado su porcentaje")
                    Exit For
                End If
            Next
            'Integrantes
            Dim idAux As Integer
            Dim mensaAux As String = String.Empty
            Dim integrSearch As ELL.Viaje.Integrante
            For Each Integr As ELL.Viaje.Integrante In oViajeOld.ListaIntegrantes
                idAux = Integr.Usuario.Id
                integrSearch = oViajeNew.ListaIntegrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = idAux)
                If (integrSearch Is Nothing) Then
                    If (mensaAux <> String.Empty) Then mensaAux &= ","
                    mensaAux &= "Se han quitado "
                    Exit For
                Else  'existe. Hay que mirar si se ha modificado la actividad o la observacion o las fechas
                    If (integrSearch.IdActividad <> Integr.IdActividad OrElse integrSearch.Observaciones <> Integr.Observaciones OrElse integrSearch.FechaIda <> Integr.FechaIda OrElse
                        integrSearch.FechaVuelta <> Integr.FechaVuelta OrElse integrSearch.EsDesarraigado <> Integr.EsDesarraigado OrElse integrSearch.esPaP_Desarraigados <> Integr.esPaP_Desarraigados OrElse
                        integrSearch.CondicionesEspeciales_Desarraigados <> Integr.CondicionesEspeciales_Desarraigados) Then
                        If (mensaAux <> String.Empty) Then mensaAux &= ","
                        mensaAux &= "Se ha modificado la actividad u observaciones u fechas u datos de desarraigo de los "
                        Exit For
                    End If
                End If
            Next
            For Each Integr As ELL.Viaje.Integrante In oViajeNew.ListaIntegrantes
                idAux = Integr.Usuario.Id
                If (Not oViajeOld.ListaIntegrantes.Exists(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = idAux)) Then
                    If (mensaAux <> String.Empty) Then mensaAux &= ","
                    mensaAux &= "Se han añadido "
                    Exit For
                End If
            Next
            If (mensaAux <> String.Empty) Then
                mensaAux = "-" & mensaAux & "integrantes"
                mensa.AppendLine(mensaAux)
            End If
            'Agencia
            If (oViajeNew.SolicitudAgencia IsNot Nothing AndAlso oViajeOld.SolicitudAgencia Is Nothing) Then
                mensa.AppendLine("-Se ha añadido una solicitud de agencia")
            ElseIf (oViajeNew.SolicitudAgencia Is Nothing AndAlso oViajeOld.SolicitudAgencia IsNot Nothing) Then
                mensa.AppendLine("-Se ha quitado la solicitud de agencia")
            ElseIf (oViajeNew.SolicitudAgencia IsNot Nothing AndAlso oViajeOld.SolicitudAgencia IsNot Nothing) Then
                mensaAux = String.Empty
                For Each serv As ELL.SolicitudAgencia.Linea In oViajeOld.SolicitudAgencia.ServiciosSolicitados
                    idAux = serv.Id
                    If (Not oViajeNew.SolicitudAgencia.ServiciosSolicitados.Exists(Function(o As ELL.SolicitudAgencia.Linea) o.Id = idAux)) Then
                        If (mensaAux <> String.Empty) Then mensaAux &= ","
                        mensaAux &= "Se han quitado "
                        Exit For
                    End If
                Next
                For Each serv As ELL.SolicitudAgencia.Linea In oViajeNew.SolicitudAgencia.ServiciosSolicitados
                    idAux = serv.Id
                    If (Not oViajeOld.SolicitudAgencia.ServiciosSolicitados.Exists(Function(o As ELL.SolicitudAgencia.Linea) o.Id = idAux)) Then
                        If (mensaAux <> String.Empty) Then mensaAux &= ","
                        mensaAux &= "Se han añadido "
                        Exit For
                    End If
                Next
                If (mensaAux <> String.Empty) Then
                    mensaAux = "-" & mensaAux & "servicios de agencia"
                    mensa.AppendLine(mensaAux)
                End If

                If (oViajeNew.SolicitudAgencia.ComentariosUsuario <> oViajeOld.SolicitudAgencia.ComentariosUsuario) Then mensa.AppendLine("-Comentarios de usuario")
            End If
            'Anticipos
            If (oViajeNew.Anticipo IsNot Nothing AndAlso oViajeOld.Anticipo Is Nothing) Then
                mensa.AppendLine("-Se ha añadido una solicitud de anticipo")
            ElseIf (oViajeNew.Anticipo Is Nothing AndAlso oViajeOld.Anticipo IsNot Nothing) Then
                mensa.AppendLine("-Se ha quitado la solicitud de anticipo")
            ElseIf (oViajeNew.Anticipo IsNot Nothing AndAlso oViajeOld.Anticipo IsNot Nothing) Then
                If (oViajeNew.Anticipo.FechaNecesidad <> oViajeOld.Anticipo.FechaNecesidad) Then mensa.AppendLine("-Fecha de necesidad")
                Dim mensaAux2 As String
                mensaAux = String.Empty : mensaAux2 = String.Empty
                Dim oMov As ELL.Anticipo.Movimiento
                For Each ant As ELL.Anticipo.Movimiento In oViajeOld.Anticipo.Movimientos
                    idAux = ant.Id
                    oMov = oViajeNew.Anticipo.Movimientos.Find(Function(o As ELL.Anticipo.Movimiento) o.Id = idAux)
                    If (oMov Is Nothing) Then
                        If (mensaAux = String.Empty) Then mensaAux = "Se han quitado "
                    Else
                        If (mensaAux2 = String.Empty AndAlso oMov.Cantidad <> ant.Cantidad) Then
                            mensaAux2 &= "Se han modificado las cantidades de "
                        End If
                    End If
                Next
                For Each ant As ELL.Anticipo.Movimiento In oViajeNew.Anticipo.Movimientos
                    idAux = ant.Id
                    oMov = oViajeOld.Anticipo.Movimientos.Find(Function(o As ELL.Anticipo.Movimiento) o.Id = idAux)
                    If (oMov Is Nothing) Then
                        If (mensaAux <> String.Empty) Then mensaAux &= ","
                        mensaAux &= "Se han añadido "
                    Else
                        If (mensaAux2 = String.Empty) Then
                            If (oMov.Cantidad <> ant.Cantidad) Then
                                mensaAux2 = "Se han modificado las cantidades de "
                            End If
                        End If
                    End If
                Next
                If (mensaAux <> String.Empty) Then
                    mensaAux = "-" & mensaAux & "movimientos de anticipo"
                    mensa.AppendLine(mensaAux)
                End If
                If (mensaAux2 <> String.Empty) Then
                    mensaAux2 = "-" & mensaAux2 & "movimientos de anticipo"
                    mensa.AppendLine(mensaAux2)
                End If
            End If
            'Solicitud de plantas
            If (oViajeNew.SolicitudesPlantasFilial IsNot Nothing AndAlso oViajeOld.SolicitudesPlantasFilial Is Nothing) Then
                mensa.AppendLine("-Se han añadido solicitudes de planta")
            ElseIf (oViajeNew.SolicitudesPlantasFilial Is Nothing AndAlso oViajeOld.SolicitudesPlantasFilial IsNot Nothing) Then
                mensa.AppendLine("-Se han quitado solicitudes de planta")
            ElseIf (oViajeNew.SolicitudesPlantasFilial IsNot Nothing AndAlso oViajeOld.SolicitudesPlantasFilial IsNot Nothing) Then
                Dim mensaAux2 As String
                mensaAux = String.Empty : mensaAux2 = String.Empty
                Dim oSolic As ELL.Viaje.SolicitudPlantaFilial
                For Each solic As ELL.Viaje.SolicitudPlantaFilial In oViajeOld.SolicitudesPlantasFilial
                    idAux = solic.IdPlantaFilial
                    oSolic = oViajeNew.SolicitudesPlantasFilial.Find(Function(o As ELL.Viaje.SolicitudPlantaFilial) o.IdPlantaFilial = idAux)
                    If (oSolic Is Nothing) Then
                        If (mensaAux <> String.Empty) Then mensaAux &= ","
                        If (mensaAux = String.Empty) Then mensaAux = "Se han quitado "
                    End If
                Next
                For Each solic As ELL.Viaje.SolicitudPlantaFilial In oViajeNew.SolicitudesPlantasFilial
                    idAux = solic.IdPlantaFilial
                    oSolic = oViajeOld.SolicitudesPlantasFilial.Find(Function(o As ELL.Viaje.SolicitudPlantaFilial) o.IdPlantaFilial = idAux)
                    If (oSolic Is Nothing) Then
                        If (mensaAux <> String.Empty) Then mensaAux &= ","
                        mensaAux &= "Se han añadido "
                    End If
                Next
                If (mensaAux <> String.Empty) Then
                    mensaAux = "-" & mensaAux & "solicitudes de planta"
                    mensa.AppendLine(mensaAux)
                End If
                If (mensaAux2 <> String.Empty) Then
                    mensaAux2 = "-" & mensaAux2 & "solicitudes de planta"
                    mensa.AppendLine(mensaAux2)
                End If
            End If
            log.Info(mensa.ToString)
        Catch ex As Exception
            log.Error("Error al comprobar los cambios realizados en la solicitud", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado de viajes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Dim url As String = "Viajes.aspx"
        If (hasProfile(BLL.BidaiakBLL.Profiles.Agencia)) Then
            url = "~\Agencia\SolicitudAgencia.aspx"
            If (btnGuardar.CommandArgument <> String.Empty) Then url &= "?idViaje=" & btnGuardar.CommandArgument
        ElseIf (hasProfile(BLL.BidaiakBLL.Profiles.Financiero) AndAlso Not OrigenViajes) Then
            url = "~\Financiero\Anticipos\GestionAnticipos.aspx"
            If (btnGuardar.CommandArgument <> String.Empty) Then url &= "?idViaje=" & btnGuardar.CommandArgument
        End If
        Response.Redirect(url)
    End Sub

    ''' <summary>
    ''' Obtiene de los integrantes, el listado de los usuarios, omitiendo el resto de informacion
    ''' </summary>
    ''' <returns></returns>
    Private Function getUsuariosIntegrantes() As List(Of SabLib.ELL.Usuario)
        Dim lUsuariosIntegrantes As New List(Of SabLib.ELL.Usuario)
        If (Integrantes IsNot Nothing AndAlso Integrantes.Count > 0) Then
            For Each Integ As ELL.Viaje.Integrante In Integrantes
                lUsuariosIntegrantes.Add(Integ.Usuario)
            Next
        End If
        Return lUsuariosIntegrantes
    End Function

    ''' <summary>
    ''' Obtiene de los integrantes, el listado de los usuarios y sus fechas
    ''' </summary>
    ''' <returns></returns>
    Private Function getUsuariosIntegrantesFechas() As List(Of String())
        Dim lUsuariosIntegrantes As New List(Of String())
        If (Integrantes IsNot Nothing AndAlso Integrantes.Count > 0) Then
            For Each Integ As ELL.Viaje.Integrante In Integrantes
                lUsuariosIntegrantes.Add(New String() {Integ.Usuario.Id, Integ.FechaIda.ToShortDateString, Integ.FechaVuelta.ToShortDateString})
            Next
        End If
        Return lUsuariosIntegrantes
    End Function

#End Region

#Region "GridViews/Repeaters"

    ''' <summary>
    ''' Evento surgido al pulsar en el icono para quitar una persona del listado
    ''' Primero se comprueba que no sea el seleccionado
    ''' </summary>
    ''' <param name="idUsuario">Id del usuario</param>
    Protected Sub DeleteIntegrante(ByVal idUsuario As Integer)
        Try
            Dim idSeleccionado As Integer = getLiquidadorSeleccionado()
            If (idUsuario = idSeleccionado) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No puede quitar a un integrante que es liquidador del viaje. Eliga a otro antes de borrarlo")
            Else
                Integrantes.Remove(Integrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = idUsuario))
                hfIdLiq.Value = idSeleccionado
                gvPersonas.DataSource = Integrantes
                gvPersonas.DataBind()
                'Al quitar el integrante, se quita tambien de la lista de conductores
                If (pnlCocheAlquiler.Visible) Then
                    ddlConductores.Items.RemoveAt(ddlConductores.Items.IndexOf(ddlConductores.Items.FindByValue(idUsuario)))
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Se ha quitado la persona de la lista de conductores")
                End If
                'Se actualizan las fechas del viaje
                Dim fIda, fVuelta As Date
                FechaMaximaYMinima(fIda, fVuelta)
                txtFechaIda.Text = fIda.ToShortDateString
                txtFechaVuelta.Text = fVuelta.ToShortDateString
                GestionPanelAnticipacion()
                upFechasViaje.Update()
            End If
        Catch ex As Exception
            log.Error("Error al quitar una persona de la solicitud de viaje", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al quitar")
        End Try
    End Sub

    ''' <summary>
    '''  Carga del listado de integrantes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub gvPersonas_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPersonas.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim integrante As ELL.Viaje.Integrante = DirectCast(e.Row.DataItem, ELL.Viaje.Integrante)
            Dim rbtLiq As RadioButton = DirectCast(e.Row.FindControl("rbtLiq"), RadioButton)
            Dim lblIdSab As Label = DirectCast(e.Row.FindControl("lblIdSab"), Label)
            Dim lblPersona As Label = DirectCast(e.Row.FindControl("lblPersona"), Label)
            Dim odlActividad As WebControlsDropDown.OptionGroupDropDownList = DirectCast(e.Row.FindControl("ogddlActiv"), WebControlsDropDown.OptionGroupDropDownList)
            Dim txtObservacion As TextBox = DirectCast(e.Row.FindControl("txtObservacion"), TextBox)
            Dim txtFIda As TextBox = DirectCast(e.Row.FindControl("txtFIda"), TextBox)
            Dim txtFVuelta As TextBox = DirectCast(e.Row.FindControl("txtFVuelta"), TextBox)
            Dim dtFechaIntIda As HtmlGenericControl = DirectCast(e.Row.FindControl("dtFechaIntIda"), HtmlGenericControl)
            Dim dtFechaIntVuelta As HtmlGenericControl = DirectCast(e.Row.FindControl("dtFechaIntVuelta"), HtmlGenericControl)
            Dim pnlDesarraigo As Panel = DirectCast(e.Row.FindControl("pnlDesarraigo"), Panel)
            Dim labelPaP As Label = DirectCast(e.Row.FindControl("labelPaP"), Label)
            Dim labelCondEsp As Label = DirectCast(e.Row.FindControl("labelCondEsp"), Label)
            Dim ddlPaP As DropDownList = DirectCast(e.Row.FindControl("ddlPaP"), DropDownList)
            Dim ddlCondEsp As DropDownList = DirectCast(e.Row.FindControl("ddlCondEsp"), DropDownList)
            Dim chbDesarraigado As CheckBox = DirectCast(e.Row.FindControl("chbDesarraigado"), CheckBox)
            Dim lnkElim As LinkButton = CType(e.Row.FindControl("lnkElim"), LinkButton)  'No se utiliza directcast porque puede ser nothing
            itzultzaileWeb.Itzuli(labelPaP) : itzultzaileWeb.Itzuli(labelCondEsp)
            cargarActividades(integrante.Usuario.IdDepartamento, odlActividad)
            lblIdSab.Text = integrante.Usuario.Id
            lblPersona.Text = integrante.Usuario.NombreCompleto
            txtFIda.Text = integrante.FechaIda.ToShortDateString
            txtFVuelta.Text = integrante.FechaVuelta.ToShortDateString
            'Dim script As New StringBuilder
            'script.AppendLine("$('#" & dtFechaIntIda.ClientID & "').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'}).on('dp.change', function (e) { if(confirm('" & itzultzaileWeb.Itzuli("¿Desea propagar la fecha de ida a todos los participantes?") & "'))" _
            '                            & "document.getElementById('" & hfPropagar.ClientID & "').value = 1; else document.getElementById('" & hfPropagar.ClientID & "').value = 0; " _
            '                            & "document.getElementById('" & hfIdFila.ClientID & "').value = " & e.Row.RowIndex & ";" _
            '                            & "document.getElementById('" & btnCambioFechasIda.ClientID & "').click();});")
            'script.AppendLine("$('#" & dtFechaIntVuelta.ClientID & "').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'}).on('dp.change', function (e) { if(confirm('" & itzultzaileWeb.Itzuli("¿Desea propagar la fecha de vuelta a todos los participantes?") & "'))" _
            '                            & "document.getElementById('" & hfPropagar.ClientID & "').value = 1; else document.getElementById('" & hfPropagar.ClientID & "').value = 0; " _
            '                            & "document.getElementById('" & hfIdFila.ClientID & "').value = " & e.Row.RowIndex & ";" _
            '                            & "document.getElementById('" & btnCambioFechasVuelta.ClientID & "').click();});")
            'AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(Me, Me.GetType, "DatePicker_" & e.Row.RowIndex, script.ToString, True)
            'Se comprueba si podria ser liquidador
            Dim bidaiakBLL As New BLL.BidaiakBLL
            rbtLiq.Enabled = True
            If (Not bidaiakBLL.PuedeRecibirVisasAnticipos(integrante.Usuario, Master.IdPlantaGestion)) Then
                rbtLiq.Enabled = False
                rbtLiq.ToolTip = itzultzaileWeb.Itzuli("El usuario no tiene permitido recibir anticipos. En caso de ser necesario, hablar con RRHH")
            Else
                If (liquidadorModificable OrElse btnGuardar.CommandArgument = String.Empty) Then 'Si es modificable o es un viaje nuevo
                    rbtLiq.ToolTip = itzultzaileWeb.Itzuli("Hacer doble click sobre el elemento chequeado si se quiere dejar sin liquidador")
                Else
                    rbtLiq.Enabled = False
                    rbtLiq.ToolTip = itzultzaileWeb.Itzuli("No se puede cambiar el liquidador porque el anticipo ya ha sido entregado")
                End If
            End If
            If (Request.Form(odlActividad.ClientID.Replace("_", "$")) IsNot Nothing) Then
                odlActividad.SelectedValue = CInt(Request.Form(odlActividad.ClientID.Replace("_", "$")))
                txtObservacion.Text = Request.Form(txtObservacion.ClientID.Replace("_", "$"))
            Else
                odlActividad.SelectedValue = integrante.IdActividad
                txtObservacion.Text = integrante.Observaciones
            End If
            cargarPaP(ddlPaP)
            cargarCondicionesEspeciales(ddlCondEsp)
            If (Request.Form(chbDesarraigado.ClientID.Replace("_", "$")) IsNot Nothing) Then
                chbDesarraigado.Checked = True
                If (Request.Form(ddlPaP.ClientID.Replace("_", "$")) IsNot Nothing) Then ddlPaP.SelectedValue = CInt(Request.Form(ddlPaP.ClientID.Replace("_", "$")))
                If (Request.Form(ddlCondEsp.ClientID.Replace("_", "$")) IsNot Nothing) Then ddlCondEsp.SelectedValue = CInt(Request.Form(ddlCondEsp.ClientID.Replace("_", "$")))
            Else
                chbDesarraigado.Checked = integrante.EsDesarraigado
                ddlPaP.SelectedIndex = ddlPaP.Items.IndexOf(ddlPaP.Items.FindByValue(If(integrante.EsDesarraigado, SabLib.BLL.Utils.BooleanToInteger(integrante.esPaP_Desarraigados), 0)))
                ddlCondEsp.SelectedIndex = ddlCondEsp.Items.IndexOf(ddlCondEsp.Items.FindByValue(integrante.CondicionesEspeciales_Desarraigados))
            End If
            pnlDesarraigo.Visible = chbDesarraigado.Checked
            chbDesarraigado.ToolTip = itzultzaileWeb.Itzuli("Chequear solo para los desarraigados")
            If (hfIdLiq.Value <> String.Empty AndAlso CInt(hfIdLiq.Value) = integrante.Usuario.Id) Then rbtLiq.Checked = True
            If (lnkElim IsNot Nothing) Then
                lnkElim.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmacion de eliminacion"), itzultzaileWeb.Itzuli("ConfirmarEliminar"), integrante.Usuario.Id, "Integrante")
                itzultzaileWeb.Itzuli(lnkElim)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Carga los valores para la puesta a punto
    ''' </summary>    
    Private Sub cargarPaP(drop As DropDownList)
        drop.Items.Clear()
        drop.Items.Add(New ListItem(itzultzaileWeb.Itzuli("no"), 0))
        drop.Items.Add(New ListItem(itzultzaileWeb.Itzuli("si"), 1))
        drop.SelectedValue = 0
    End Sub

    ''' <summary>
    ''' Carga los valores para la puesta a punto
    ''' </summary>    
    Private Sub cargarCondicionesEspeciales(drop As DropDownList)
        drop.Items.Clear()
        If (lCondicionesEspeciales Is Nothing) Then
            'La primera ejecucion, se tendra que cargar de bbdd
            Dim bidaiakBLL As New BLL.BidaiakBLL
            lCondicionesEspeciales = bidaiakBLL.loadCondicionesEspeciales(Master.IdPlantaGestion, True)
        End If
        If (lCondicionesEspeciales IsNot Nothing) Then
            Dim bAnadir As Boolean
            For Each item As String() In lCondicionesEspeciales
                bAnadir = True
                If (CInt(item(0)) = 1 AndAlso CInt(ddlTipoViaje.SelectedValue) <> ELL.Viaje.eNivel.Resto_del_mundo) Then bAnadir = False
                If (bAnadir) Then drop.Items.Add(New ListItem(itzultzaileWeb.Itzuli(item(1)), item(0)))
            Next
        End If
        drop.SelectedIndex = drop.Items.IndexOf(drop.Items.FindByValue(0))
    End Sub

    ''' <summary>
    ''' Carga las actividades de un departamento y las carga en un dropdown especificado
    ''' Si el tipo de viaje es nacional, solo se mostraran las no exentas, sino todas
    ''' </summary>
    ''' <param name="idDpto">Id del departamento</param>
    ''' <param name="dropdown">Donde se cargaran los datos</param>    
    Private Sub cargarActividades(ByVal idDpto As String, ByRef dropdown As WebControlsDropDown.OptionGroupDropDownList)
        If (dropdown.Items.Count = 0) Then
            dropdown.OptionGroups.Clear()
            Dim value As Integer = 0
            Dim activBLL As New BLL.ActividadesBLL
            Dim bIndicarSiExento As Boolean = (CInt(ddlTipoViaje.SelectedValue) = ELL.Viaje.eNivel.Europa_y_norte_Africa Or CInt(ddlTipoViaje.SelectedValue) = ELL.Viaje.eNivel.Resto_del_mundo)
            Dim lActiv As List(Of ELL.Actividad) = activBLL.loadListDpto(idDpto, Master.IdPlantaGestion, 0) 'Todas
            If (lActiv IsNot Nothing) Then lActiv = lActiv.OrderBy(Of String)(Function(o) o.Nombre).ToList
            'Se ordena por si es exento o no y dentro de si es exento o no por nombre
            Dim group As WebControlsDropDown.OptionGroupDropDownList.OptionGroup = Nothing
            group = dropdown.OptionGroups.Add(itzultzaileWeb.Itzuli("Sin seleccion"))
            group.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
            group = dropdown.OptionGroups.Add(itzultzaileWeb.Itzuli("Actividades"))
            If (lActiv IsNot Nothing) Then
                For Each myActiv In lActiv
                    If (bIndicarSiExento) Then myActiv.Nombre &= " (" & If(myActiv.ExentaIRPF, itzultzaileWeb.Itzuli("Exenta"), itzultzaileWeb.Itzuli("No exenta")) & ")"
                    group.Items.Add(New ListItem(myActiv.Nombre, myActiv.Id))
                Next
            End If
        End If
    End Sub

    ''' <summary>
    ''' Cuando se chequea, muestran dos paneles referentes a los desarraigados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub CambioDesarraigo(sender As Object, e As EventArgs)
        Dim check As CheckBox = CType(sender, CheckBox)
        Dim row As GridViewRow = check.Parent.Parent
        Dim idSab As Integer = CInt(CType(row.FindControl("lblIdSab"), Label).Text)
        Dim pnlDesarraigo As Panel = CType(row.FindControl("pnlDesarraigo"), Panel)
        Dim drop As DropDownList = CType(row.FindControl("ddlCondEsp"), DropDownList)
        Dim integr As ELL.Viaje.Integrante = Integrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = idSab)
        cargarCondicionesEspeciales(drop)  'Se vuelve a recargar por si se ha cambiado el tipo de viaje y necesita quitarse o añadir la opcion de Paises en vias de desarrollo
        pnlDesarraigo.Visible = check.Checked
        If (Not check.Checked) Then  'Si  no se ha chequeado se resetea
            integr.esPaP_Desarraigados = False
            integr.CondicionesEspeciales_Desarraigados = Integer.MinValue
        End If
    End Sub

    ''' <summary>
    ''' Busca en el grid de personas, el liquidador seleccionado
    ''' </summary>
    ''' <returns></returns>    
    Private Function getLiquidadorSeleccionado() As Integer
        For Each row As GridViewRow In gvPersonas.Rows
            If (CType(row.Cells(2).FindControl("rbtLiq"), RadioButton).Checked) Then
                Return CInt(CType(row.Cells(1).FindControl("lblIdSab"), Label).Text)
            End If
        Next
        Return Integer.MinValue
    End Function

    ''' <summary>
    ''' Elimina el documento
    ''' </summary>
    ''' <param name="idDoc">Id del documento</param>    
    Private Sub DeleteClientDocument(ByVal idDoc As Integer)
        Try
            Dim viajeBLL As New BLL.ViajesBLL
            Dim idViaje As Integer = CInt(btnGuardar.CommandArgument)
            viajeBLL.DeleteDocumentoCliente(idDoc)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Documento borrado")
            log.Info("Se ha borrado el documento de cliente " & idDoc & " del viaje " & idViaje)
            mostrarDetalle(idViaje)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se cargan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rptDocumentosCliente_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptDocumentosCliente.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oDoc As ELL.Viaje.DocumentoCliente = e.Item.DataItem
            Dim hkTitulo As HyperLink = CType(e.Item.FindControl("hkTitulo"), HyperLink)
            Dim lnkEliminar As LinkButton = CType(e.Item.FindControl("lnkEliminar"), LinkButton)
            hkTitulo.Text = oDoc.Titulo
            If (oDoc.IdViaje > 0) Then
                hkTitulo.NavigateUrl = "~/Publico/ViewDocument.aspx?id=" & oDoc.Id & "&tipo=docCliViaje"
                If (oDoc.FechaSubida <> DateTime.MinValue) Then
                    hkTitulo.ToolTip = itzultzaileWeb.Itzuli("Subido el") & ": " & oDoc.FechaSubida.ToShortDateString & " - " & oDoc.FechaSubida.ToShortTimeString
                Else '031213: Todos los documentos antiguos, no tienen informado este campo
                    hkTitulo.ToolTip = itzultzaileWeb.Itzuli("Abrir documento")
                End If
            Else
                hkTitulo.NavigateUrl = String.Empty
                hkTitulo.ToolTip = itzultzaileWeb.Itzuli("Documento de proyecto")
                If (oDoc.FechaSubida <> DateTime.MinValue) Then
                    hkTitulo.ToolTip &= "." & itzultzaileWeb.Itzuli("Subido el") & ": " & oDoc.FechaSubida.ToShortDateString & " - " & oDoc.FechaSubida.ToShortTimeString
                End If
            End If

            lnkEliminar.Visible = (oDoc.IdViaje > 0)  'Si es un documento de los que estan en XBAT, no se podra ni ver ni borrar            
            lnkEliminar.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmacion de eliminacion"), itzultzaileWeb.Itzuli("ConfirmarEliminar"), oDoc.Id, "DelDocuClient")
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los movimientos de transferencias
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptTransfAnticipos_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptTransfAnticipos.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim transf As ELL.Anticipo.Movimiento = e.Item.DataItem
            Dim lblTransfAnticipo As Label = CType(e.Item.FindControl("lblTransfAnticipo"), Label)
            Dim mensaje As String = String.Empty
            If (transf.IdViajeOrigen.ToString = btnGuardar.CommandArgument) Then
                mensaje = itzultzaileWeb.Itzuli("Se han transferido [CANTIDAD] [MONEDA] al viaje [ID_VIAJE]")
                mensaje = mensaje.Replace("[ID_VIAJE]", "V" & transf.IdViajeDestino)
            Else
                mensaje = itzultzaileWeb.Itzuli("Se han recibido [CANTIDAD] [MONEDA] del viaje [ID_VIAJE]")
                mensaje = mensaje.Replace("[ID_VIAJE]", "V" & transf.IdViajeOrigen)
            End If
            mensaje = mensaje.Replace("[CANTIDAD]", transf.Cantidad).Replace("[MONEDA]", transf.Moneda.Abreviatura)
            lblTransfAnticipo.Text = mensaje
        End If
    End Sub

#End Region

#Region "Control Importes"

    ''' <summary>
    ''' Evento del control de importes para mostrar un mensaje
    ''' </summary>
    ''' <param name="mensa">Mensaje a mostrar</param>
    ''' <param name="tipo">Tipo de error</param>	
    Private Sub selImportes_MensajeImporte(ByVal mensa As String, ByVal tipo As Integer) Handles selImportes.MensajeImporte
        If (tipo = 1) Then 'Info
            'Master.MensajeInfo = mensa 'Cuando añadimos un importe, no queremos que muestre mensaje
        ElseIf (tipo = 2) Then 'Advertencia
            Master.MensajeAdvertencia = mensa
        Else 'Error
            Master.MensajeError = mensa
        End If
    End Sub

#End Region

End Class