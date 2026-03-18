Public Class PresupServicios
    Inherits PageBase

#Region "Properties"

    ''' <summary>
    ''' Devuelve el numero de planes que se han creado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property NumPlanes As Integer
        Get
            If (ViewState("NPlanes") Is Nothing) Then
                Return 1
            Else
                Return CInt(ViewState("NPlanes"))
            End If
        End Get
        Set(value As Integer)
            ViewState("NPlanes") = value
        End Set
    End Property

    ''' <summary>
    ''' Id del presupuesto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property IdPresupuesto As Integer
        Get
            Return CInt(btnGuardar.CommandArgument)
        End Get
        Set(value As Integer)
            btnGuardar.CommandArgument = value
        End Set
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Carga del presupuesto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Page.MaintainScrollPositionOnPostBack = True
                Master.SetTitle = "Presupuesto de servicios del viaje"
                IdPresupuesto = CInt(Request.QueryString("IdViaje"))
                cargarPresupuesto()
            End If
            Dim fechaIda As Date = CType(lblFIda.Text, Date)
            Dim fDefault As String = String.Format("{0}/{1}/{2}", fechaIda.Month, fechaIda.Day, fechaIda.Year)
            Dim script As New StringBuilder
            script.AppendLine("$('#dtDateLimit').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
            script.AppendLine("$('#dtAFecha').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',defaultDate: '" & fDefault & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
            script.AppendLine("$('#dtAHora').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'LT'});")
            script.AppendLine("$('#dtHFEntrada').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',defaultDate: '" & fDefault & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
            script.AppendLine("$('#dtHFSalida').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',defaultDate: '" & fDefault & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
            script.AppendLine("$('#dtCDiaRecog').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',defaultDate: '" & fDefault & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
            script.AppendLine("$('#dtCHoraReg').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'LT'});")
            script.AppendLine("$('#dtCDiaDev').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',defaultDate: '" & fDefault & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
            script.AppendLine("$('#dtCHoraDev').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'LT'});")
            script.AppendLine("$('#dtTDia').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',defaultDate: '" & fDefault & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
            script.AppendLine("$('#dtTHora').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'LT'});")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "DatePicker", script.ToString, True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub PresupServicios_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelDivInfo) : itzultzaileWeb.Itzuli(labelDivObserv) : itzultzaileWeb.Itzuli(labelDivServAereo)
            itzultzaileWeb.Itzuli(labelDivHotel) : itzultzaileWeb.Itzuli(labelDivCoche) : itzultzaileWeb.Itzuli(labelTitleModalDelete)
            itzultzaileWeb.Itzuli(labelDivTrenes) : itzultzaileWeb.Itzuli(labelFechas) : itzultzaileWeb.Itzuli(labelTitleModalTarifa)
            itzultzaileWeb.Itzuli(labelViaje) : itzultzaileWeb.Itzuli(labelSolicitante) : itzultzaileWeb.Itzuli(labelViajeros) : itzultzaileWeb.Itzuli(labelNumPlanes)
            itzultzaileWeb.Itzuli(labelSelPlanViaje) : itzultzaileWeb.Itzuli(labelIntegrantesPlan) : itzultzaileWeb.Itzuli(labelHCiudad) : itzultzaileWeb.Itzuli(labelHNombre)
            itzultzaileWeb.Itzuli(labelHFEntrada) : itzultzaileWeb.Itzuli(labelHFSalida) : itzultzaileWeb.Itzuli(labelHTipoHab) : itzultzaileWeb.Itzuli(labelHRegimen)
            itzultzaileWeb.Itzuli(labelHTarifaObjCab) : itzultzaileWeb.Itzuli(labelHTarifaRealCab) : itzultzaileWeb.Itzuli(labelTCab1PersoTarifa)
            itzultzaileWeb.Itzuli(labelHTarifaDia) : itzultzaileWeb.Itzuli(labelCConductor) : itzultzaileWeb.Itzuli(labelCCiudadRec)
            itzultzaileWeb.Itzuli(labelCDiaRecog) : itzultzaileWeb.Itzuli(labelCHoraRecog) : itzultzaileWeb.Itzuli(labelCDiaDev) : itzultzaileWeb.Itzuli(labelCHoraDev)
            itzultzaileWeb.Itzuli(labelCCategoria) : itzultzaileWeb.Itzuli(labelCCabReal) : itzultzaileWeb.Itzuli(labelCCab1DiaTarifa) : itzultzaileWeb.Itzuli(btnGuardar)
            itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(labelTCiudadOrigen) : itzultzaileWeb.Itzuli(labelTCiudadDestino)
            itzultzaileWeb.Itzuli(labelTFecha) : itzultzaileWeb.Itzuli(labelTHora) : itzultzaileWeb.Itzuli(labelTClase) : itzultzaileWeb.Itzuli(labelADia)
            itzultzaileWeb.Itzuli(labelACiudadOrigen) : itzultzaileWeb.Itzuli(labelAHoraSalida) : itzultzaileWeb.Itzuli(labelACiudadDestino) : itzultzaileWeb.Itzuli(btnACiudadTarif)
            itzultzaileWeb.Itzuli(labelATarifaObjCab) : itzultzaileWeb.Itzuli(labelATarifaRealCab) : itzultzaileWeb.Itzuli(labelTCabReal)
            itzultzaileWeb.Itzuli(labelATarifa1Persona) : itzultzaileWeb.Itzuli(labelCCiudadDev) : itzultzaileWeb.Itzuli(labelRespondidoPor) : itzultzaileWeb.Itzuli(btnEnviar)
            itzultzaileWeb.Itzuli(labelRespVal) : itzultzaileWeb.Itzuli(btnHCiudadTarif) : itzultzaileWeb.Itzuli(btnPrevisualizar)
            itzultzaileWeb.Itzuli(labelPresupTotal) : itzultzaileWeb.Itzuli(labelObjetivoTotal) : itzultzaileWeb.Itzuli(labelNewState)
            itzultzaileWeb.Itzuli(labelConfirmMessageModal) : itzultzaileWeb.Itzuli(btnAceptarModalDel) : itzultzaileWeb.Itzuli(labelCancelarModalDel)
            itzultzaileWeb.Itzuli(labelTitleConfirmEnvio) : itzultzaileWeb.Itzuli(labelModalEnvioMessage) : itzultzaileWeb.Itzuli(btnModalEnviarPresup)
            itzultzaileWeb.Itzuli(labelModalCancelEnviarPresup) : itzultzaileWeb.Itzuli(labelCCabObj)
        End If
        If (CInt(hfEstado.Value) = ELL.Presupuesto.EstadoPresup.Validado) Then disablePage() 'Si se acepta, no se podrán hacer modificaciones. Si se cancela sin embargo, si.
    End Sub

#End Region

#Region "Cargar presupuesto"

    ''' <summary>
    ''' Inicializa los controles del presupuesto
    ''' </summary>    
    Private Sub inicializarControles()
        lblIdViaje.Text = String.Empty : lblFIda.Text = String.Empty : lblSolicitante.Text = String.Empty : lblIntegrPlan.Text = "0"
        labelRespondidoPor.Visible = False : lblUserRespuesta.Text = String.Empty : lblFVuelta.Text = String.Empty
        lblRespVal.Text = String.Empty : hfRespVal.Value = String.Empty : btnEnviar.Visible = False : btnPrevisualizar.Visible = False
        lblEstado.Text = String.Empty : trNewPresupState.Visible = False : ddlEstado.Items.Clear()
        hfEstado.Value = "-1"
        btnEnviar.OnClientClick = "$('#divEnviar').modal('show'); return false;"
        btnPrevisualizar.OnClientClick = "window.open(""../Publico/Presupuestos/DetPresupuesto.aspx?idPresup=" & IdPresupuesto & "&ag=1"",""Presupuesto"");return false;"
        cargarNumPlanes()
        gvIntegrantes.DataSource = Nothing : gvIntegrantes.DataBind()
        gvAviones.DataSource = Nothing : gvAviones.DataBind()
        gvHoteles.DataSource = Nothing : gvHoteles.DataBind()
        gvTrenes.DataSource = Nothing : gvTrenes.DataBind()
        gvCoches.DataSource = Nothing : gvCoches.DataBind()
        inicializarControlesAvion()
        inicializarControlesHotel()
        inicializarControlesTren()
        inicializarControlesCocheAlquiler()
    End Sub

    ''' <summary>
    ''' Carga el desplegable con los numeros de planes que se pueden llegar a crear
    ''' </summary>    
    Private Sub cargarNumPlanes()
        If (ddlNumPlanViajes.Items.Count = 0) Then
            For Index As Integer = 1 To 5
                ddlNumPlanViajes.Items.Add(Index)
            Next
        End If
        ddlNumPlanViajes.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Carga los planes de viaje que se han creado
    ''' </summary>    
    Private Sub cargarPlanViajes()
        ddlPlanViaje.Items.Clear()
        If (NumPlanes < 1) Then NumPlanes = 1
        ddlPlanViaje.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Todos"), Integer.MinValue))
        For Index As Integer = 1 To NumPlanes
            ddlPlanViaje.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Plan de viaje " & Index), Index))
        Next
        ddlPlanViaje.SelectedValue = 1
        ChequearNumIntegrantesPlan()
    End Sub

    ''' <summary>
    ''' Carga los planes de viaje en un control
    ''' </summary>
    ''' <param name="dropdown">Desplegable</param>
    ''' <param name="numPlan">Numero de plan a seleccionar</param>    
    Private Sub cargarPlanViajes(ByVal dropdown As DropDownList, ByVal numPlan As Integer)
        For Index As Integer = 1 To NumPlanes
            dropdown.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Plan de viaje " & Index), Index))
        Next
        If (numPlan > 0) Then dropdown.SelectedValue = numPlan
    End Sub

    ''' <summary>
    ''' Carga los estados a los que se puede cambiar dependiendo del estado actual
    ''' </summary>    
    ''' <param name="estado">Estado en el que se encuentra</param>
    Private Sub cargarEstados(ByVal estado As Integer)
        ddlEstado.Items.Clear()
        ddlEstado.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), -1))
        Select Case estado
            Case ELL.Presupuesto.EstadoPresup.Enviado
                ddlEstado.Items.Add(New ListItem(itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), ELL.Presupuesto.EstadoPresup.Validado)), ELL.Presupuesto.EstadoPresup.Validado))
                ddlEstado.Items.Add(New ListItem(itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), ELL.Presupuesto.EstadoPresup.Rechazado)), ELL.Presupuesto.EstadoPresup.Rechazado))
            Case ELL.Presupuesto.EstadoPresup.Rechazado
                ddlEstado.Items.Add(New ListItem(itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), ELL.Presupuesto.EstadoPresup.Enviado)), ELL.Presupuesto.EstadoPresup.Enviado))
                ddlEstado.Items.Add(New ListItem(itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), ELL.Presupuesto.EstadoPresup.Validado)), ELL.Presupuesto.EstadoPresup.Validado))
        End Select
    End Sub

    ''' <summary>
    ''' Carga el desplegable con los tipos de habitaciones
    ''' </summary>    
    Private Sub cargarTipoHabitaciones()
        If (ddlHTipoHab.Items.Count = 0) Then
            ddlHTipoHab.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
            ddlHTipoHab.Items.Add(New ListItem(itzultzaileWeb.Itzuli("DUI"), 0))
            ddlHTipoHab.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Doble"), 1))
            ddlHTipoHab.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Individual"), 2))
        End If
        ddlHTipoHab.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Carga el desplegable con los regimenes de un hotel
    ''' </summary>    
    Private Sub cargarRegimenes()
        If (ddlHRegimen.Items.Count = 0) Then
            ddlHRegimen.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
            ddlHRegimen.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Alojamiento y desayuno"), 0))
            ddlHRegimen.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Solo alojamiento"), 1))
        End If
        ddlHRegimen.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Carga la informacion de un presupuesto
    ''' </summary>  
    Private Sub cargarPresupuesto()
        Try
            Dim viajesBLL As New BLL.ViajesBLL
            Dim presupBLL As New BLL.PresupuestosBLL
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(IdPresupuesto)
            Dim oPresupuesto As ELL.Presupuesto = presupBLL.loadInfo(IdPresupuesto)
            inicializarControles()
            hfEstado.Value = oPresupuesto.Estado
            NumPlanes = oViaje.ListaIntegrantes.Max(Function(o As ELL.Viaje.Integrante) o.NumPlan)
            oViaje.ListaIntegrantes = oViaje.ListaIntegrantes.OrderBy(Of String)(Function(o) o.Usuario.NombreCompleto).ToList
            cargarPlanViajes()
            'Info del viaje
            lblIdViaje.Text = oViaje.IdViaje & " - " & oViaje.Destino
            lblFIda.Text = oViaje.FechaIda.ToShortDateString : lblFVuelta.Text = oViaje.FechaVuelta.ToShortDateString
            lblSolicitante.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador}, False).NombreCompleto
            gvIntegrantes.DataSource = oViaje.ListaIntegrantes
            gvIntegrantes.DataBind()
            'Responsable de validacion
            lblRespVal.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oPresupuesto.IdUsuarioResponsable}, False).NombreCompleto
            hfRespVal.Value = oPresupuesto.IdUsuarioResponsable
            If (oViaje.SolicitudAgencia IsNot Nothing AndAlso oViaje.SolicitudAgencia.ServiciosSolicitados IsNot Nothing) Then
                Dim oServicioCoche As ELL.SolicitudAgencia.Linea = oViaje.SolicitudAgencia.ServiciosSolicitados.Find(Function(o As ELL.SolicitudAgencia.Linea) o.IdUserReq <> Integer.MinValue)
                If (oServicioCoche IsNot Nothing) Then
                    divCocheAlq.Visible = True
                    lblCConductor.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oServicioCoche.IdUserReq}, False).NombreCompleto
                Else
                    divCocheAlq.Visible = False
                End If
            End If
            Dim nPlan As Integer = CInt(ddlPlanViaje.SelectedValue)
            btnEnviar.Visible = (oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Creado)  'Solo se visualizara el boton cuando el estado sea creado
            btnGuardar.Visible = (oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Creado Or oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Enviado Or oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Rechazado)  'Una vez validado, ya no se podra tocar
            btnPrevisualizar.Visible = True 'Siempre que exista se podra previsualizar
            btnCrearPresup.Visible = (oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Generado)
            pnlServicios.Visible = (Not btnCrearPresup.Visible)
            pnlBotones.Visible = (Not btnCrearPresup.Visible)
            'pnlObservaciones.Visible = (Not btnCrearPresup.Visible)
            'Gestion del desplegable de nuevos estados
            lblEstado.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), oPresupuesto.Estado))
            Select Case oPresupuesto.Estado
                Case ELL.Presupuesto.EstadoPresup.Generado, ELL.Presupuesto.EstadoPresup.Creado, ELL.Presupuesto.EstadoPresup.Validado
                    trNewPresupState.Visible = False
                    If (btnCrearPresup.Visible) Then lblEstado.Text = itzultzaileWeb.Itzuli("Sin crear")
                    If (oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Validado) Then
                        lblEstado.CssClass = "label label-success"
                    Else
                        lblEstado.CssClass = "label label-default"
                    End If
                Case ELL.Presupuesto.EstadoPresup.Enviado, ELL.Presupuesto.EstadoPresup.Rechazado
                    trNewPresupState.Visible = True
                    cargarEstados(oPresupuesto.Estado)
                    If (oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Rechazado) Then
                        lblEstado.CssClass = "label label-danger"
                    ElseIf (oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Enviado) Then
                        lblEstado.CssClass = "label label-info"
                    End If
            End Select
            'Info del viaje del presupuesto
            ddlNumPlanViajes.SelectedValue = NumPlanes
            If (oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Validado Or oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Rechazado) Then
                labelRespondidoPor.Visible = True
                lblUserRespuesta.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oPresupuesto.IdUsuarioRespuesta}, False).NombreCompleto
            End If
            If (oPresupuesto.FechaLimiteEmision <> Date.MinValue) Then txtFechaLimite.Text = oPresupuesto.FechaLimiteEmision.ToShortDateString
            txtObservaciones.Text = oPresupuesto.Observaciones
            ChequearNumIntegrantesPlan()
            PintarServiciosAvion(IdPresupuesto)
            PintarServiciosHotel(IdPresupuesto)
            If (divCocheAlq.Visible) Then PintarServiciosCocheAlquiler(IdPresupuesto)
            PintarServiciosTren(IdPresupuesto)
            RefressTotal()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al cargar el presupuesto", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Refresca la cantidad total del presupuesto y del objetivo
    ''' </summary>
    Private Sub RefressTotal()
        Dim total, totalObjetivo As Decimal
        Dim viajesBLL As New BLL.ViajesBLL
        Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(IdPresupuesto)
        total = 0 : totalObjetivo = 0
        GetTotales(gvAviones, ELL.Presupuesto.Servicio.Tipo_Servicio.Avion, total, totalObjetivo, oViaje.ListaIntegrantes)
        GetTotales(gvHoteles, ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel, total, totalObjetivo, oViaje.ListaIntegrantes)
        If (divCocheAlq.Visible) Then GetTotales(gvCoches, ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler, total, totalObjetivo)
        If (divTrenes.Visible) Then GetTotales(gvTrenes, ELL.Presupuesto.Servicio.Tipo_Servicio.Tren, total, totalObjetivo)
        If (total <= totalObjetivo) Then
            lblTotal.CssClass = "label label-success"
        Else
            lblTotal.CssClass = "label label-danger"
        End If
        lblTotal.Text = Math.Round(total, 2)
        lblObjTotal.Text = Math.Round(totalObjetivo, 2)
    End Sub

    ''' <summary>
    ''' Obtiene la cantidades totales del gridview especificado
    ''' Dependiendo del plan, habra que multiplicar esta cantidad por el numero de integrantes del plan (Avion,hotel)
    ''' </summary>
    ''' <param name="gv">Gridview a chequear</param>
    ''' <param name="tipoServicio">Indica si el tipo de servicio</param>
    ''' <param name="total">Total real</param>
    ''' <param name="totalObjetivo">Total objetivo</param>
    ''' <param name="lIntegrantes">Lista de integrantes</param>
    Private Sub GetTotales(ByVal gv As GridView, ByVal tipoServicio As Integer, ByRef total As Decimal, ByRef totalObjetivo As Decimal, Optional ByVal lIntegrantes As List(Of ELL.Viaje.Integrante) = Nothing)
        Dim footRow As GridViewRow = gv.FooterRow
        Dim myTotObj, myTot As Decimal
        Dim numInteg As Integer
        myTotObj = 0 : myTot = 0 : numInteg = 0
        If (footRow IsNot Nothing AndAlso gv.Rows.Count > 0) Then
            Dim lblObj As Label = CType(footRow.FindControl("lblTotalObj"), Label)
            If (lblObj IsNot Nothing) Then myTotObj = CDec(lblObj.Text) 'Los coches de alquiler no tienen tarifa objetivo
            myTot = CDec(CType(footRow.FindControl("lblTotalReal"), Label).Text)
            If (tipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Avion Or tipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel OrElse tipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler) Then
                If (ddlPlanViaje.SelectedValue > 0) Then
                    myTotObj = myTotObj
                    myTot = myTot
                Else  'Se estan mostrando todos los planes
                    Dim lblTotObj, lblTotReal, lblNumPlan As Label
                    myTot = 0 : myTotObj = 0
                    For Each oRow As GridViewRow In gv.Rows
                        If (oRow.RowType = DataControlRowType.DataRow) Then
                            lblNumPlan = CType(oRow.FindControl("lblPlanViaje"), Label)
                            lblTotObj = CType(oRow.FindControl("lblTarifaObj"), Label)
                            lblTotReal = CType(oRow.FindControl("lblTarifaReal"), Label)
                            myTotObj += CDec(lblTotObj.Text)
                            myTot += CDec(lblTotReal.Text)
                        End If
                    Next
                End If
            End If
            If (tipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Tren) Then myTotObj = myTot 'En los trenes, no hay tarifa objetivo pero se pone la misma que la real
            totalObjetivo += myTotObj
            total += myTot
        End If
    End Sub

    ''' <summary>
    ''' Se deshabilitan todos los controles de la pagina
    ''' </summary>    
    Private Sub disablePage()
        btnEnviar.Visible = False : btnGuardar.Visible = False
        btnAGuardar.Visible = False : btnACancelar.Visible = False
        btnHGuardar.Visible = False : btnHCancelar.Visible = False
        btnTGuardar.Visible = False : btnTCancelar.Visible = False
        btnCGuardar.Visible = False : btnCCancelar.Visible = False
        Dim lControles As New List(Of Object)
        Dim lnk As New LinkButton : lnk.ID = "lnkEdit" : lControles.Add(lnk)
        lnk = New LinkButton : lnk.ID = "lnkElim" : lControles.Add(lnk)
        DisableControlsGridview(gvAviones, lControles)
        DisableControlsGridview(gvHoteles, lControles)
        DisableControlsGridview(gvTrenes, lControles)
        DisableControlsGridview(gvCoches, lControles)
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
    ''' Vuelve a recargar los servicios
    ''' </summary>
    ''' <param name="numPlan">Numero del plan</param>    
    Private Sub RecargarServiciosPlan(ByVal numPlan As Integer)
        PintarServiciosAvion(IdPresupuesto)
        PintarServiciosHotel(IdPresupuesto)
        PintarServiciosCocheAlquiler(IdPresupuesto)
        PintarServiciosTren(IdPresupuesto)
    End Sub

    ''' <summary>
    ''' Chequea el numero de usuarios que tienen asignado el plan seleccionado
    ''' </summary>    
    Private Sub ChequearNumIntegrantesPlan()
        Dim ddlPlan As DropDownList = Nothing
        Dim numIntegr As Integer = 0
        Dim numPlanSel As Integer = CInt(ddlPlanViaje.SelectedValue)
        For Each gvr As GridViewRow In gvIntegrantes.Rows
            ddlPlan = CType(gvr.FindControl("ddlPlanViaje"), DropDownList)
            If (numPlanSel = Integer.MinValue OrElse numPlanSel = CInt(ddlPlan.SelectedValue)) Then
                numIntegr += 1
            End If
        Next
        lblIntegrPlan.Text = numIntegr
    End Sub

    ''' <summary>
    ''' Al cambiar el numero de planes se actualizan ciertos controles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlNumPlanViajes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNumPlanViajes.SelectedIndexChanged
        Try
            NumPlanes = ddlNumPlanViajes.SelectedValue
            cargarPlanViajes()
            'Se actualizan los desplegables del gridview de integrantes
            Dim viajesBLL As New BLL.ViajesBLL
            Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(IdPresupuesto, False, False, True)
            gvIntegrantes.DataSource = oViaje.ListaIntegrantes
            gvIntegrantes.DataBind()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se cambia de plan de viaje
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlPlanViaje_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPlanViaje.SelectedIndexChanged
        Try
            Dim numPlanSel As Integer = CInt(ddlPlanViaje.SelectedValue)
            RecargarServiciosPlan(numPlanSel)
            ChequearNumIntegrantesPlan()
            RefressTotal()
            Dim bHabilitarGuardarServ As Boolean = (numPlanSel <> Integer.MinValue)
            btnAGuardar.Enabled = bHabilitarGuardarServ : btnHGuardar.Enabled = bHabilitarGuardarServ
            btnCGuardar.Enabled = bHabilitarGuardarServ : btnTGuardar.Enabled = bHabilitarGuardarServ
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Enlaza los integrantes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvIntegrantes_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvIntegrantes.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim integ As ELL.Viaje.Integrante = CType(e.Row.DataItem, ELL.Viaje.Integrante)
            CType(e.Row.FindControl("lblIdUser"), Label).Text = integ.Usuario.Id
            CType(e.Row.FindControl("lblIntegrante"), Label).Text = integ.Usuario.NombreCompleto
            CType(e.Row.FindControl("lblFechasViaje"), Label).Text = integ.FechaIda.ToShortDateString & " - " & integ.FechaVuelta.ToShortDateString
            cargarPlanViajes(CType(e.Row.FindControl("ddlPlanViaje"), DropDownList), integ.NumPlan)
        End If
    End Sub

    ''' <summary>
    ''' Obtiene los datos del presupuesto
    ''' </summary>
    ''' <param name="estadoOld">Estado antiguo</param>
    ''' <param name="estadoNew">Estado nuevo</param>
    ''' <returns></returns>    
    Private Function RecogerDatosPresup(ByRef estadoOld As Integer, ByRef estadoNew As Integer) As ELL.Presupuesto
        Dim presupBLL As New BLL.PresupuestosBLL
        Dim oPresup As ELL.Presupuesto
        oPresup = presupBLL.loadInfo(IdPresupuesto)
        If (estadoOld = -1) Then 'Hay que saber cual es el estado old y el new sera el mismo
            estadoOld = oPresup.Estado
            estadoNew = estadoOld
        End If
        oPresup.Estado = estadoNew
        oPresup.Observaciones = txtObservaciones.Text.Trim
        If (txtFechaLimite.Text <> String.Empty) Then oPresup.FechaLimiteEmision = CDate(txtFechaLimite.Text)
        If (estadoNew = ELL.Presupuesto.EstadoPresup.Validado Or estadoNew = ELL.Presupuesto.EstadoPresup.Rechazado) Then
            'Si se ha aceptado o denegado, el usuario de respuesta sera el usuario del ticket
            oPresup.IdUsuarioRespuesta = Master.Ticket.IdUser
        Else
            'Si no y el estado anterior es creado o enviado y el estado nuevo es aceptado o denegado, el usuario de respuesta sera el usuario del ticket
            If ((estadoOld = ELL.Presupuesto.EstadoPresup.Creado Or estadoOld = ELL.Presupuesto.EstadoPresup.Enviado) AndAlso
               (estadoNew = ELL.Presupuesto.EstadoPresup.Validado Or estadoNew = ELL.Presupuesto.EstadoPresup.Rechazado)) Then
                oPresup.IdUsuarioRespuesta = Master.Ticket.IdUser
            End If
        End If
        'Integrantes
        Dim lblIdUser As Label
        Dim ddlPlan As DropDownList
        Dim lInteg As New List(Of ELL.Viaje.Integrante)
        For Each row As GridViewRow In gvIntegrantes.Rows
            lblIdUser = CType(row.FindControl("lblIdUser"), Label)
            ddlPlan = CType(row.FindControl("ddlPlanViaje"), DropDownList)
            lInteg.Add(New ELL.Viaje.Integrante With {.Usuario = New SabLib.ELL.Usuario With {.Id = CInt(lblIdUser.Text)}, .NumPlan = CInt(ddlPlan.SelectedValue)})
        Next
        oPresup.Integrantes = lInteg
        Return oPresup
    End Function

    ''' <summary>
    ''' Muestra el panel modal al querer enviar
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalEnviar(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divEnviar').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divEnviar').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True) '2º intruccion necesaria por estar dentro de un updatePanel
        End If
    End Sub

#End Region

#Region "Servicios de avion"

    ''' <summary>
    ''' Inicializa los controles del avion
    ''' </summary>    
    Private Sub inicializarControlesAvion()
        txtADia.Text = String.Empty
        txtACiudadOrigen.Text = String.Empty : txtACiudadDestino.Text = String.Empty
        txtAHoraSalida.Text = String.Empty : lblATarifaObj.Text = "0"
        txtATarifaReal.Text = "0" : btnAGuardar.CommandArgument = String.Empty
        lblACiudadTarif.Text = String.Empty : hfACiudadTarif.Value = String.Empty
        btnACancelar.Enabled = False
    End Sub

    ''' <summary>
    ''' Pinta la seccion de los servicios del avion. Ademas, si viene un servicio informado, se editaran los datos
    ''' </summary>
    ''' <param name="idPresupuesto">Id del presupuesto</param>
    ''' <param name="idServ">Id del servicio</param>
    Private Sub PintarServiciosAvion(ByVal idPresupuesto As Integer, Optional ByVal idServ As Integer = 0)
        Try
            Dim presupBLL As New BLL.PresupuestosBLL
            Dim lServicios As List(Of ELL.Presupuesto.Servicio) = Nothing
            inicializarControlesAvion()
            Dim oPresupuesto As ELL.Presupuesto = presupBLL.loadInfo(idPresupuesto)
            If (oPresupuesto.Servicios IsNot Nothing) Then
                Dim numPlanSel As Integer = CInt(ddlPlanViaje.SelectedValue)
                lServicios = oPresupuesto.Servicios.FindAll(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Avion AndAlso o.NumeroPlan = If(numPlanSel > 0, numPlanSel, o.NumeroPlan))
                If (lServicios.Count > 0) Then lServicios = lServicios.OrderBy(Of Integer)(Function(o) o.Id).ToList
            End If
            gvAviones.DataSource = lServicios : gvAviones.DataBind()
            If (idServ > 0) Then
                Dim oServicio As ELL.Presupuesto.Servicio = lServicios.Find(Function(o As ELL.Presupuesto.Servicio) o.Id = idServ)
                Dim tarifaObjetivo As Decimal = 0
                txtACiudadOrigen.Text = oServicio.Ciudad1
                txtACiudadDestino.Text = oServicio.Ciudad2
                If (oServicio.IdTarifaDestino > 0) Then
                    hfACiudadTarif.Value = oServicio.IdTarifaDestino
                    Dim tarifBLL As New BLL.TarifasServBLL
                    Dim oTarifa As ELL.TarifaServicios = tarifBLL.loadTarifaInfo(oServicio.IdTarifaDestino)
                    lblACiudadTarif.Text = oTarifa.Destino.ToUpper
                    Dim myTarifaAnno As ELL.TarifaServicios.Lineas = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = CDate(lblFIda.Text).Year)
                    If (myTarifaAnno IsNot Nothing) Then tarifaObjetivo = myTarifaAnno.TarifaAvion
                End If
                If (oServicio.Fecha1 <> DateTime.MinValue) Then
                    txtADia.Text = oServicio.Fecha1.ToShortDateString
                    If Not (oServicio.Fecha1.Hour = 0 And oServicio.Fecha1.Minute = 0) Then txtAHoraSalida.Text = oServicio.Fecha1.ToShortTimeString
                End If
                lblATarifaObj.Text = tarifaObjetivo
                txtATarifaReal.Text = oServicio.TarifaReal
                btnAGuardar.CommandArgument = oServicio.Id
                btnACancelar.Enabled = True
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al pintar la seccion de servicios de hotel", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos del avion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvAviones_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvAviones.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim serv As ELL.Presupuesto.Servicio = CType(e.Row.DataItem, ELL.Presupuesto.Servicio)
            Dim lnkDel As LinkButton = CType(e.Row.FindControl("lnkDel"), LinkButton)
            Dim tarifaBLL As New BLL.TarifasServBLL
            Dim tarifaObjetivo As Decimal = 0
            Dim numIntegPlan As Integer = CInt(lblIntegrPlan.Text)
            Dim oTarifa As ELL.TarifaServicios = Nothing
            If (serv.IdTarifaDestino > 0) Then
                oTarifa = tarifaBLL.loadTarifaInfo(serv.IdTarifaDestino)
                Dim myTarifaAnno As ELL.TarifaServicios.Lineas = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = CDate(lblFIda.Text).Year)
                If (myTarifaAnno IsNot Nothing) Then
                    tarifaObjetivo = myTarifaAnno.TarifaAvion
                End If
            End If
            CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandArgument = serv.Id
            lnkDel.CommandArgument = serv.Id
            lnkDel.OnClientClick = ConfigureModal("AV", serv.Id)
            CType(e.Row.FindControl("lblPlanViaje"), Label).Text = serv.NumeroPlan
            If (serv.Fecha1 <> DateTime.MinValue) Then CType(e.Row.FindControl("lblFecha"), Label).Text = serv.Fecha1.ToShortDateString
            CType(e.Row.FindControl("lblCiudadOrigen"), Label).Text = serv.Ciudad1
            CType(e.Row.FindControl("lblCiudadDestino"), Label).Text = serv.Ciudad2
            CType(e.Row.FindControl("lblTarifaObj"), Label).Text = CDec(tarifaObjetivo) * numIntegPlan
            CType(e.Row.FindControl("lblTarifaReal"), Label).Text = serv.TarifaReal * numIntegPlan
            gvAviones.Columns(1).Visible = (CInt(ddlPlanViaje.SelectedValue = Integer.MinValue))
        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
            Dim totalObj, totalReal As Decimal
            totalObj = 0 : totalReal = 0
            For Each gvr As GridViewRow In gvAviones.Rows
                Dim lblTarifaObj As Label = CType(gvr.FindControl("lblTarifaObj"), Label)
                Dim lblTarifaReal As Label = CType(gvr.FindControl("lblTarifaReal"), Label)
                totalObj += CDec(lblTarifaObj.Text.Trim)
                totalReal += CDec(lblTarifaReal.Text.Trim)
            Next
            CType(e.Row.FindControl("lblTotalObj"), Label).Text = totalObj
            CType(e.Row.FindControl("lblTotalReal"), Label).Text = totalReal
        End If
    End Sub

    ''' <summary>
    ''' Cancela la edicion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnACancelar_Click(sender As Object, e As EventArgs) Handles btnACancelar.Click
        inicializarControlesAvion()
    End Sub

    ''' <summary>
    ''' Registra o actualiza los datos del avion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnAGuardar_Click(sender As Object, e As EventArgs) Handles btnAGuardar.Click
        Try
            If (txtACiudadOrigen.Text = String.Empty OrElse txtACiudadDestino.Text = String.Empty OrElse txtADia.Text = String.Empty OrElse txtATarifaReal.Text = String.Empty OrElse txtATarifaReal.Text = "0" OrElse hfACiudadTarif.Value = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca los datos marcados")
            Else
                'Se chequea que las fechas del servicio esten dentro de las fechas del viaje
                Dim fecha As Date = CDate(txtADia.Text)
                If Not (CDate(lblFIda.Text) <= fecha And fecha <= CDate(lblFVuelta.Text)) Then
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No puede añadir el servicio porque esta fuera de las fechas del viaje")
                    Exit Sub
                End If
                Dim presupBLL As New BLL.PresupuestosBLL
                fecha = Date.MinValue
                Dim oServ As New ELL.Presupuesto.Servicio With {.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Avion, .IdViaje = IdPresupuesto}
                If (btnAGuardar.CommandArgument <> String.Empty) Then
                    oServ.Id = CInt(btnAGuardar.CommandArgument)
                    oServ = presupBLL.loadServicio(oServ.Id)
                End If
                oServ.NumeroPlan = CInt(ddlPlanViaje.SelectedValue)
                oServ.Ciudad1 = txtACiudadOrigen.Text.Trim
                oServ.Ciudad2 = txtACiudadDestino.Text.Trim
                fecha = CDate(txtADia.Text)
                If (txtAHoraSalida.Text <> String.Empty) Then
                    Dim sHoraInicio As String() = txtAHoraSalida.Text.Split(":")
                    fecha = fecha.AddHours(CInt(sHoraInicio(0))).AddMinutes(CInt(sHoraInicio(1)))
                End If
                oServ.Fecha1 = fecha
                If (txtATarifaReal.Text <> String.Empty) Then oServ.TarifaReal = PageBase.DecimalValue(txtATarifaReal.Text)
                If (hfACiudadTarif.Value <> String.Empty) Then oServ.IdTarifaDestino = CInt(hfACiudadTarif.Value)
                Dim bNew As Boolean = False
                Dim oPresupuesto As ELL.Presupuesto = RecogerDatosPresup(-1, -1)  'No hacen falta aqui saber el estado nuevo y el antiguo
                presupBLL.SaveServicio(oServ, Nothing)
                PintarServiciosAvion(IdPresupuesto)
                RefressTotal()
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Servicio modificado")
                If (oServ.Id = Integer.MinValue) Then
                    log.Info("Se ha añadido un servicio de avion al presupuesto " & IdPresupuesto)
                Else
                    log.Info("Se ha modificado el servicio de avion " & oServ.Id & " del presupuesto " & IdPresupuesto)
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al guardar los datos del avion del presupuesto " & IdPresupuesto)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar los datos del avion")
        End Try
    End Sub

    ''' <summary>
    ''' Se abre la ventana para que seleccione una ciudad destino
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnACiudadTarif_Click(sender As Object, e As EventArgs) Handles btnACiudadTarif.Click
        Try
            txtTarifDestino.Text = txtACiudadDestino.Text
            hfTipo.Value = "A"
            BuscarCiudadTarifa()
            ShowModalBoxTarifa(True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Servicios de hotel"

    ''' <summary>
    ''' Inicializa los controles del hotel
    ''' </summary>    
    Private Sub inicializarControlesHotel()
        txtHCiudad.Text = String.Empty : txtHNombre.Text = String.Empty
        txtHFEntrada.Text = String.Empty : txtHFSalida.Text = String.Empty
        lblHTarifaDiaO.Text = "0" : txtHTarifaDiaR.Text = "0"
        btnHGuardar.CommandArgument = String.Empty
        lblHCiudadTarif.Text = String.Empty : hfHCiudadTarif.Value = String.Empty
        cargarTipoHabitaciones() : cargarRegimenes()
        btnHCancelar.Enabled = False
    End Sub

    ''' <summary>
    ''' Pinta la seccion de los servicios del hotel. Ademas, si viene un servicio informado, se editaran los datos
    ''' </summary>
    ''' <param name="idPresupuesto">Id del presupuesto</param>
    ''' <param name="idServ">Id del servicio</param>
    Private Sub PintarServiciosHotel(ByVal idPresupuesto As Integer, Optional ByVal idServ As Integer = 0)
        Try
            Dim presupBLL As New BLL.PresupuestosBLL
            Dim lServicios As List(Of ELL.Presupuesto.Servicio) = Nothing
            Dim numDiasReserva As Integer
            inicializarControlesHotel()
            Dim oPresupuesto As ELL.Presupuesto = presupBLL.loadInfo(idPresupuesto)
            If (oPresupuesto.Servicios IsNot Nothing) Then
                Dim numPlanSel As Integer = CInt(ddlPlanViaje.SelectedValue)
                lServicios = oPresupuesto.Servicios.FindAll(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel AndAlso o.NumeroPlan = If(numPlanSel > 0, numPlanSel, o.NumeroPlan))
                If (lServicios.Count > 0) Then lServicios = lServicios.OrderBy(Of Integer)(Function(o) o.Id).ToList
            End If
            gvHoteles.DataSource = lServicios : gvHoteles.DataBind()
            If (idServ > 0) Then
                Dim oServicio As ELL.Presupuesto.Servicio = lServicios.Find(Function(o As ELL.Presupuesto.Servicio) o.Id = idServ)
                Dim tarifaObjetivo As Decimal = 0
                txtHCiudad.Text = oServicio.Ciudad1
                If (oServicio.IdTarifaDestino > 0) Then
                    hfHCiudadTarif.Value = oServicio.IdTarifaDestino
                    Dim tarifBLL As New BLL.TarifasServBLL
                    Dim oTarifa As ELL.TarifaServicios = tarifBLL.loadTarifaInfo(oServicio.IdTarifaDestino)
                    lblHCiudadTarif.Text = oTarifa.Destino.ToUpper
                    Dim myTarifaAnno As ELL.TarifaServicios.Lineas = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = CDate(lblFIda.Text).Year)
                    If (myTarifaAnno IsNot Nothing) Then tarifaObjetivo = myTarifaAnno.TarifaHotel
                End If
                txtHNombre.Text = oServicio.Nombre
                If (oServicio.Fecha1 <> DateTime.MinValue) Then txtHFEntrada.Text = oServicio.Fecha1.ToShortDateString
                If (oServicio.Fecha2 <> DateTime.MinValue) Then txtHFSalida.Text = oServicio.Fecha2.ToShortDateString
                If (oServicio.TipoHabitacion >= 0) Then ddlHTipoHab.SelectedValue = oServicio.TipoHabitacion
                If (oServicio.Regimen >= 0) Then ddlHRegimen.SelectedValue = oServicio.Regimen
                numDiasReserva = 1
                If (oServicio.Fecha1 <> Date.MinValue AndAlso oServicio.Fecha2 <> Date.MinValue) Then numDiasReserva = oServicio.Fecha2.Subtract(oServicio.Fecha1).Days
                lblHTarifaDiaO.Text = tarifaObjetivo
                txtHTarifaDiaR.Text = oServicio.TarifaReal
                btnHGuardar.CommandArgument = oServicio.Id
                btnHCancelar.Enabled = True
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al pintar la seccion de servicios de hotel", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos del hotel
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvHoteles_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvHoteles.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim serv As ELL.Presupuesto.Servicio = CType(e.Row.DataItem, ELL.Presupuesto.Servicio)
            Dim lnkDel As LinkButton = CType(e.Row.FindControl("lnkDel"), LinkButton)
            Dim tarifaBLL As New BLL.TarifasServBLL
            Dim numDiasReserva As Integer = 1
            Dim numIntegPlan As Integer = CInt(lblIntegrPlan.Text)
            Dim tarifaObjetivo As Decimal = 0
            Dim oTarifa As ELL.TarifaServicios = Nothing
            If (serv.IdTarifaDestino > 0) Then
                oTarifa = tarifaBLL.loadTarifaInfo(serv.IdTarifaDestino)
                Dim myTarifaAnno As ELL.TarifaServicios.Lineas = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = CDate(lblFIda.Text).Year)
                If (myTarifaAnno IsNot Nothing) Then
                    tarifaObjetivo = myTarifaAnno.TarifaHotel
                End If
            End If
            CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandArgument = serv.Id
            lnkDel.CommandArgument = serv.Id
            lnkDel.OnClientClick = ConfigureModal("HO", serv.Id)
            CType(e.Row.FindControl("lblPlanViaje"), Label).Text = serv.NumeroPlan
            CType(e.Row.FindControl("lblHotel"), Label).Text = serv.Nombre
            If (serv.Fecha1 <> DateTime.MinValue) Then CType(e.Row.FindControl("lblFEntrada"), Label).Text = serv.Fecha1.ToShortDateString
            If (serv.Fecha1 <> Date.MinValue AndAlso serv.Fecha2 <> Date.MinValue) Then numDiasReserva = serv.Fecha2.Subtract(serv.Fecha1).Days
            CType(e.Row.FindControl("lblTarifaObj"), Label).Text = CDec(tarifaObjetivo) * numDiasReserva * numIntegPlan
            CType(e.Row.FindControl("lblTarifaReal"), Label).Text = serv.TarifaReal * numDiasReserva * numIntegPlan
            gvHoteles.Columns(1).Visible = (CInt(ddlPlanViaje.SelectedValue = Integer.MinValue))
        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
            Dim totalObj, totalReal As Decimal
            totalObj = 0 : totalReal = 0
            For Each gvr As GridViewRow In gvHoteles.Rows
                Dim lblTarifaObj As Label = CType(gvr.FindControl("lblTarifaObj"), Label)
                Dim lblTarifaReal As Label = CType(gvr.FindControl("lblTarifaReal"), Label)
                totalObj += CDec(lblTarifaObj.Text.Trim)
                totalReal += CDec(lblTarifaReal.Text.Trim)
            Next
            CType(e.Row.FindControl("lblTotalObj"), Label).Text = totalObj
            CType(e.Row.FindControl("lblTotalReal"), Label).Text = totalReal
        End If
    End Sub

    ''' <summary>
    ''' Cancela la edicion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnHCancelar_Click(sender As Object, e As EventArgs) Handles btnHCancelar.Click
        inicializarControlesHotel()
    End Sub

    ''' <summary>
    ''' Registra o actualiza los datos del hotel
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnHGuardar_Click(sender As Object, e As EventArgs) Handles btnHGuardar.Click
        Try
            If (txtHCiudad.Text = String.Empty OrElse txtHNombre.Text = String.Empty OrElse txtHFEntrada.Text = String.Empty OrElse txtHFSalida.Text = String.Empty OrElse txtHTarifaDiaR.Text = String.Empty OrElse txtHTarifaDiaR.Text = "0" OrElse hfHCiudadTarif.Value = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca los datos marcados")
            Else
                'Se chequea que las fechas del servicio esten dentro de las fechas del viaje
                Dim fechaEntr, fechaSal As Date
                fechaEntr = CDate(txtHFEntrada.Text) : fechaSal = CDate(txtHFSalida.Text)
                If Not ((CDate(lblFIda.Text) <= fechaEntr And fechaEntr <= CDate(lblFVuelta.Text)) AndAlso (CDate(lblFIda.Text) <= fechaSal And fechaSal <= CDate(lblFVuelta.Text))) Then
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No puede añadir el servicio porque esta fuera de las fechas del viaje")
                    Exit Sub
                End If
                Dim presupBLL As New BLL.PresupuestosBLL
                Dim oServ As New ELL.Presupuesto.Servicio With {.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel, .IdViaje = IdPresupuesto}
                If (btnHGuardar.CommandArgument <> String.Empty) Then
                    oServ.Id = CInt(btnHGuardar.CommandArgument)
                    oServ = presupBLL.loadServicio(oServ.Id)
                End If
                oServ.NumeroPlan = CInt(ddlPlanViaje.SelectedValue)
                oServ.Ciudad1 = txtHCiudad.Text.Trim
                oServ.Nombre = txtHNombre.Text.Trim
                oServ.Fecha1 = CDate(txtHFEntrada.Text)
                oServ.Fecha2 = CDate(txtHFSalida.Text)
                oServ.TipoHabitacion = CInt(ddlHTipoHab.SelectedValue)
                oServ.Regimen = CInt(ddlHRegimen.SelectedValue)
                oServ.TarifaReal = PageBase.DecimalValue(txtHTarifaDiaR.Text)
                If (hfHCiudadTarif.Value <> String.Empty) Then oServ.IdTarifaDestino = CInt(hfHCiudadTarif.Value)
                Dim bNew As Boolean = False
                Dim oPresupuesto As ELL.Presupuesto = RecogerDatosPresup(-1, -1)
                presupBLL.SaveServicio(oServ, Nothing)
                PintarServiciosHotel(IdPresupuesto)
                RefressTotal()
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Servicio modificado")
                If (oServ.Id = Integer.MinValue) Then
                    log.Info("Se ha añadido un servicio de hotel al presupuesto " & IdPresupuesto)
                Else
                    log.Info("Se ha modificado el servicio de hotel " & oServ.Id & " del presupuesto " & IdPresupuesto)
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al guardar los datos del hotel del presupuesto " & IdPresupuesto)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar los datos del hotel")
        End Try
    End Sub

    ''' <summary>
    ''' Se abre la ventana para que seleccione una ciudad destino
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnHCiudadTarif_Click(sender As Object, e As EventArgs) Handles btnHCiudadTarif.Click
        Try
            txtTarifDestino.Text = txtHCiudad.Text
            hfTipo.Value = "H"
            BuscarCiudadTarifa()
            ShowModalBoxTarifa(True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Servicios de tren"

    ''' <summary>
    ''' Inicializa los controles del hotel
    ''' </summary>    
    Private Sub inicializarControlesTren()
        txtTCiudadOrigen.Text = String.Empty : txtTCiudadDestino.Text = String.Empty
        txtTClase.Text = String.Empty : txtTDia.Text = String.Empty : txtTTarifa1Perso.Text = "0"
        txtTHora.Text = String.Empty : btnTGuardar.CommandArgument = String.Empty
        btnTCancelar.Enabled = False
    End Sub

    ''' <summary>
    ''' Pinta la seccion de los servicios del tren. Ademas, si viene un servicio informado, se editaran los datos
    ''' </summary>
    ''' <param name="idPresupuesto">Id del presupuesto</param>
    ''' <param name="idServ">Id del servicio</param>
    Private Sub PintarServiciosTren(ByVal idPresupuesto As Integer, Optional ByVal idServ As Integer = 0)
        Try
            Dim presupBLL As New BLL.PresupuestosBLL
            Dim lServicios As List(Of ELL.Presupuesto.Servicio) = Nothing
            inicializarControlesTren()
            Dim oPresupuesto As ELL.Presupuesto = presupBLL.loadInfo(idPresupuesto)
            If (oPresupuesto.Servicios IsNot Nothing) Then
                Dim numPlanSel As Integer = CInt(ddlPlanViaje.SelectedValue)
                lServicios = oPresupuesto.Servicios.FindAll(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Tren AndAlso o.NumeroPlan = If(numPlanSel > 0, numPlanSel, o.NumeroPlan))
                If (lServicios.Count > 0) Then lServicios = lServicios.OrderBy(Of Integer)(Function(o) o.Id).ToList
            End If
            gvTrenes.DataSource = lServicios : gvTrenes.DataBind()
            If (idServ > 0) Then
                Dim oServicio As ELL.Presupuesto.Servicio = lServicios.Find(Function(o As ELL.Presupuesto.Servicio) o.Id = idServ)
                txtTCiudadOrigen.Text = oServicio.Ciudad1
                txtTCiudadDestino.Text = oServicio.Ciudad2
                If (oServicio.Fecha1 <> DateTime.MinValue) Then
                    txtTDia.Text = oServicio.Fecha1.ToShortDateString
                    txtTHora.Text = oServicio.Fecha1.ToShortTimeString
                End If
                txtTClase.Text = oServicio.Clase
                txtTTarifa1Perso.Text = oServicio.TarifaReal
                btnTGuardar.CommandArgument = oServicio.Id
                btnTCancelar.Enabled = True
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al pintar la seccion de servicios de tren", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos del tren
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvTrenes_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvTrenes.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim serv As ELL.Presupuesto.Servicio = CType(e.Row.DataItem, ELL.Presupuesto.Servicio)
            Dim lnkDel As LinkButton = CType(e.Row.FindControl("lnkDel"), LinkButton)
            CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandArgument = serv.Id
            lnkDel.CommandArgument = serv.Id
            lnkDel.OnClientClick = ConfigureModal("TR", serv.Id)
            Dim numIntegPlan As Integer = CInt(lblIntegrPlan.Text)
            CType(e.Row.FindControl("lblPlanViaje"), Label).Text = serv.NumeroPlan
            CType(e.Row.FindControl("lblCiudadOrigen"), Label).Text = serv.Ciudad1
            CType(e.Row.FindControl("lblCiudadDestino"), Label).Text = serv.Ciudad2
            CType(e.Row.FindControl("lblFecha"), Label).Text = serv.Fecha1.ToShortDateString
            CType(e.Row.FindControl("lblTarifaReal"), Label).Text = serv.TarifaReal * numIntegPlan
            gvTrenes.Columns(1).Visible = (CInt(ddlPlanViaje.SelectedValue = Integer.MinValue))
        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
            Dim totalReal As Decimal = 0
            For Each gvr As GridViewRow In gvTrenes.Rows
                Dim lblTarifaReal As Label = CType(gvr.FindControl("lblTarifaReal"), Label)
                totalReal += CDec(lblTarifaReal.Text.Trim)
            Next
            CType(e.Row.FindControl("lblTotalReal"), Label).Text = totalReal
        End If
    End Sub

    ''' <summary>
    ''' Cancela la edicion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnTCancelar_Click(sender As Object, e As EventArgs) Handles btnTCancelar.Click
        inicializarControlesTren()
    End Sub

    ''' <summary>
    ''' Registra o actualiza los datos del tren
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnTGuardar_Click(sender As Object, e As EventArgs) Handles btnTGuardar.Click
        Try
            If (txtTCiudadOrigen.Text = String.Empty OrElse txtTCiudadDestino.Text = String.Empty OrElse txtTDia.Text = String.Empty OrElse txtTTarifa1Perso.Text = String.Empty OrElse txtTTarifa1Perso.Text = "0") Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca los datos marcados")
            Else
                'Se chequea que las fechas del servicio esten dentro de las fechas del viaje
                Dim fecha As Date = CDate(txtTDia.Text)
                If Not (CDate(lblFIda.Text) <= fecha And fecha <= CDate(lblFVuelta.Text)) Then
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No puede añadir el servicio porque esta fuera de las fechas del viaje")
                    Exit Sub
                End If
                Dim presupBLL As New BLL.PresupuestosBLL
                fecha = Date.MinValue
                Dim oServ As New ELL.Presupuesto.Servicio With {.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Tren, .IdViaje = IdPresupuesto}
                If (btnTGuardar.CommandArgument <> String.Empty) Then
                    oServ.Id = CInt(btnTGuardar.CommandArgument)
                    oServ = presupBLL.loadServicio(oServ.Id)
                End If
                oServ.NumeroPlan = CInt(ddlPlanViaje.SelectedValue)
                oServ.Ciudad1 = txtTCiudadOrigen.Text.Trim
                oServ.Ciudad2 = txtTCiudadDestino.Text.Trim
                fecha = CDate(txtTDia.Text)
                If (txtTHora.Text <> String.Empty) Then
                    Dim sHoraInicio As String() = txtTHora.Text.Split(":")
                    fecha = fecha.AddHours(CInt(sHoraInicio(0))).AddMinutes(CInt(sHoraInicio(1)))
                End If
                oServ.Fecha1 = fecha
                oServ.Clase = txtTClase.Text.Trim
                If (txtTTarifa1Perso.Text <> String.Empty) Then oServ.TarifaReal = CDec(txtTTarifa1Perso.Text)
                Dim bNew As Boolean = False
                Dim oPresupuesto As ELL.Presupuesto = RecogerDatosPresup(-1, -1)
                presupBLL.SaveServicio(oServ, Nothing)
                PintarServiciosTren(IdPresupuesto)
                RefressTotal()
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Servicio modificado")
                If (oServ.Id = Integer.MinValue) Then
                    log.Info("Se ha añadido un servicio de tren al presupuesto " & IdPresupuesto)
                Else
                    log.Info("Se ha modificado el servicio de tren " & oServ.Id & " del presupuesto " & IdPresupuesto)
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al guardar los datos del tren del presupuesto " & IdPresupuesto)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar los datos del tren")
        End Try
    End Sub

#End Region

#Region "Servicios Coche Alquiler"

    ''' <summary>
    ''' Inicializa los controles del coche de alquiler
    ''' </summary>    
    Private Sub inicializarControlesCocheAlquiler()
        txtCLugarRecogida.Text = String.Empty : txtCTarifa1Dia.Text = "0" : lblCTarifaDiaO.Text = "0"
        txtCDiaRecog.Text = String.Empty : txtCDiaDev.Text = String.Empty : txtCCategoria.Text = String.Empty
        txtCHoraRecog.Text = String.Empty : txtCHoraDev.Text = String.Empty : txtCLugarDevolucion.Text = String.Empty
        btnCCancelar.Enabled = False : btnCGuardar.CommandArgument = String.Empty
        lblCCiudadTarif.Text = String.Empty : hfCCiudadTarif.Value = String.Empty
    End Sub

    ''' <summary>
    ''' Pinta la seccion de los servicios de coche de alquiler
    ''' </summary>
    ''' <param name="idPresupuesto">Presupuesto</param>
    ''' <param name="idServ">Id del servicio</param> 
    Private Sub PintarServiciosCocheAlquiler(ByVal idPresupuesto As Integer, Optional ByVal idServ As Integer = 0)
        Try
            Dim presupBLL As New BLL.PresupuestosBLL
            Dim lServicios As List(Of ELL.Presupuesto.Servicio) = Nothing
            Dim numDiasReserva As Integer
            inicializarControlesCocheAlquiler()
            Dim oPresupuesto As ELL.Presupuesto = presupBLL.loadInfo(idPresupuesto)
            If (oPresupuesto.Servicios IsNot Nothing) Then
                Dim numPlanSel As Integer = CInt(ddlPlanViaje.SelectedValue)
                lServicios = oPresupuesto.Servicios.FindAll(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler AndAlso o.NumeroPlan = If(numPlanSel > 0, numPlanSel, o.NumeroPlan))
                If (lServicios.Count > 0) Then lServicios = lServicios.OrderBy(Of Integer)(Function(o) o.Id).ToList
            End If
            gvCoches.DataSource = lServicios : gvCoches.DataBind()
            If (idServ > 0) Then
                Dim tarifaObjetivo As Decimal = 0
                Dim oServicio As ELL.Presupuesto.Servicio = lServicios.Find(Function(o As ELL.Presupuesto.Servicio) o.Id = idServ)
                txtCLugarRecogida.Text = oServicio.Ciudad1
                txtCLugarDevolucion.Text = oServicio.Ciudad2
                If (oServicio.Fecha1 <> DateTime.MinValue) Then
                    txtCDiaRecog.Text = If(oServicio.Fecha1 <> DateTime.MinValue, oServicio.Fecha1.ToShortDateString, String.Empty)
                    txtCHoraRecog.Text = If(oServicio.Fecha1.Hour <> 0 Or oServicio.Fecha1.Minute <> 0, oServicio.Fecha1.ToShortTimeString, String.Empty)
                End If
                If (oServicio.Fecha2 <> DateTime.MinValue) Then
                    txtCDiaDev.Text = If(oServicio.Fecha2 <> DateTime.MinValue, oServicio.Fecha2.ToShortDateString, String.Empty)
                    txtCHoraDev.Text = If(oServicio.Fecha2.Hour <> 0 Or oServicio.Fecha2.Minute <> 0, oServicio.Fecha2.ToShortTimeString, String.Empty)
                End If
                txtCCategoria.Text = oServicio.Categoria
                numDiasReserva = 1
                If (oServicio.Fecha1 <> Date.MinValue AndAlso oServicio.Fecha2 <> Date.MinValue) Then
                    numDiasReserva = Math.Ceiling(oServicio.Fecha2.Subtract(oServicio.Fecha1).TotalHours / 24)
                    If (numDiasReserva = 0) Then numDiasReserva = 1 'Si se coge y se deja en el mismo dia, sera solo un dia
                End If
                If (oServicio.IdTarifaDestino > 0) Then
                    hfCCiudadTarif.Value = oServicio.IdTarifaDestino
                    Dim tarifBLL As New BLL.TarifasServBLL
                    Dim oTarifa As ELL.TarifaServicios = tarifBLL.loadTarifaInfo(oServicio.IdTarifaDestino)
                    lblCCiudadTarif.Text = oTarifa.Destino.ToUpper
                    Dim myTarifaAnno As ELL.TarifaServicios.Lineas = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = CDate(lblFIda.Text).Year)
                    If (myTarifaAnno IsNot Nothing) Then tarifaObjetivo = myTarifaAnno.TarifaCocheAlquiler
                End If
                lblCTarifaDiaO.Text = tarifaObjetivo
                txtCTarifa1Dia.Text = oServicio.TarifaReal
                btnCGuardar.CommandArgument = oServicio.Id
                btnCCancelar.Enabled = True
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al pintar la seccion de servicios de coches de alquiler", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos del coche de alquiler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvCoches_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCoches.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim serv As ELL.Presupuesto.Servicio = CType(e.Row.DataItem, ELL.Presupuesto.Servicio)
            Dim lnkDel As LinkButton = CType(e.Row.FindControl("lnkDel"), LinkButton)
            Dim numDiasReserva As Integer = 1
            CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandArgument = serv.Id
            lnkDel.CommandArgument = serv.Id
            lnkDel.OnClientClick = ConfigureModal("CA", serv.Id)
            CType(e.Row.FindControl("lblPlanViaje"), Label).Text = serv.NumeroPlan
            CType(e.Row.FindControl("lblLugarRecogida"), Label).Text = serv.Ciudad1
            CType(e.Row.FindControl("lblFecha"), Label).Text = serv.Fecha1.ToShortDateString
            Dim tarifaObjetivo As Decimal = 0
            Dim oTarifa As ELL.TarifaServicios = Nothing
            If (serv.IdTarifaDestino > 0) Then
                Dim tarifaBLL As New BLL.TarifasServBLL
                oTarifa = tarifaBLL.loadTarifaInfo(serv.IdTarifaDestino)
                Dim myTarifaAnno As ELL.TarifaServicios.Lineas = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = CDate(lblFIda.Text).Year)
                If (myTarifaAnno IsNot Nothing) Then tarifaObjetivo = myTarifaAnno.TarifaCocheAlquiler
            End If
            If (serv.Fecha1 <> Date.MinValue AndAlso serv.Fecha2 <> Date.MinValue) Then
                numDiasReserva = Math.Ceiling(serv.Fecha2.Subtract(serv.Fecha1).TotalDays)
                If (numDiasReserva = 0) Then numDiasReserva = 1 'Si se coge y se deja en el mismo dia, sera solo un dia
            End If
            CType(e.Row.FindControl("lblTarifaObj"), Label).Text = CDec(tarifaObjetivo) * numDiasReserva
            CType(e.Row.FindControl("lblTarifaReal"), Label).Text = serv.TarifaReal * numDiasReserva
            gvCoches.Columns(1).Visible = (CInt(ddlPlanViaje.SelectedValue = Integer.MinValue))
        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
            Dim totalObj, totalReal As Decimal
            Dim lblTarifaObj, lblTarifaReal As Label
            totalObj = 0 : totalReal = 0
            For Each gvr As GridViewRow In gvCoches.Rows
                lblTarifaObj = CType(gvr.FindControl("lblTarifaObj"), Label)
                lblTarifaReal = CType(gvr.FindControl("lblTarifaReal"), Label)
                totalObj += CDec(lblTarifaObj.Text.Trim)
                totalReal += CDec(lblTarifaReal.Text.Trim)
            Next
            CType(e.Row.FindControl("lblTotalObj"), Label).Text = totalObj
            CType(e.Row.FindControl("lblTotalReal"), Label).Text = totalReal
        End If
    End Sub

    ''' <summary>
    ''' Cancela la edicion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnCCancelar_Click(sender As Object, e As EventArgs) Handles btnCCancelar.Click
        inicializarControlesCocheAlquiler()
    End Sub

    ''' <summary>
    ''' Registra o actualiza los datos del coche de alquiler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnCGuardar_Click(sender As Object, e As EventArgs) Handles btnCGuardar.Click
        Try
            If (txtCLugarRecogida.Text = String.Empty OrElse txtCLugarDevolucion.Text = String.Empty OrElse txtCDiaRecog.Text = String.Empty OrElse txtCDiaDev.Text = String.Empty OrElse txtCTarifa1Dia.Text = String.Empty OrElse txtCTarifa1Dia.Text = "0" OrElse hfCCiudadTarif.Value = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca los datos marcados")
            Else
                'Se chequea que las fechas del servicio esten dentro de las fechas del viaje
                Dim fechaEntr, fechaSal, fecha As Date
                fechaEntr = CDate(txtCDiaRecog.Text) : fechaSal = CDate(txtCDiaDev.Text)
                If Not ((CDate(lblFIda.Text) <= fechaEntr And fechaEntr <= CDate(lblFVuelta.Text)) AndAlso (CDate(lblFIda.Text) <= fechaSal And fechaSal <= CDate(lblFVuelta.Text))) Then
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No puede añadir el servicio porque esta fuera de las fechas del viaje")
                    Exit Sub
                End If
                Dim presupBLL As New BLL.PresupuestosBLL
                fecha = Date.MinValue
                Dim oServ As New ELL.Presupuesto.Servicio With {.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler, .IdViaje = IdPresupuesto}
                If (btnCGuardar.CommandArgument <> String.Empty) Then
                    oServ.Id = CInt(btnCGuardar.CommandArgument)
                    oServ = presupBLL.loadServicio(oServ.Id)
                End If
                oServ.NumeroPlan = CInt(ddlPlanViaje.SelectedValue)
                oServ.Ciudad1 = txtCLugarRecogida.Text.Trim
                oServ.Ciudad2 = txtCLugarDevolucion.Text.Trim
                fecha = CDate(txtCDiaRecog.Text)
                If (txtCHoraRecog.Text <> String.Empty) Then
                    Dim sHoraInicio As String() = txtCHoraRecog.Text.Split(":")
                    fecha = fecha.AddHours(CInt(sHoraInicio(0))).AddMinutes(CInt(sHoraInicio(1)))
                End If
                oServ.Fecha1 = fecha
                fecha = CDate(txtCDiaDev.Text)
                If (txtCHoraDev.Text <> String.Empty) Then
                    Dim sHoraInicio As String() = txtCHoraDev.Text.Split(":")
                    fecha = fecha.AddHours(CInt(sHoraInicio(0))).AddMinutes(CInt(sHoraInicio(1)))
                End If
                oServ.Fecha2 = fecha
                oServ.Categoria = txtCCategoria.Text.Trim
                oServ.TarifaReal = PageBase.DecimalValue(txtCTarifa1Dia.Text)
                If (hfCCiudadTarif.Value <> String.Empty) Then oServ.IdTarifaDestino = CInt(hfCCiudadTarif.Value)
                Dim bNew As Boolean = False
                Dim oPresupuesto As ELL.Presupuesto = RecogerDatosPresup(-1, -1)
                presupBLL.SaveServicio(oServ, Nothing)
                PintarServiciosCocheAlquiler(IdPresupuesto)
                RefressTotal()
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Servicio modificado")
                If (oServ.Id = Integer.MinValue) Then
                    log.Info("Se ha añadido un servicio de coche de alquiler al presupuesto " & IdPresupuesto)
                Else
                    log.Info("Se ha modificado el servicio de coche de alquiler " & oServ.Id & " del presupuesto " & IdPresupuesto)
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al guardar los datos del coche de alquiler del presupuesto " & IdPresupuesto)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar los datos del coche de alquiler")
        End Try
    End Sub

    ''' <summary>
    ''' Se abre la ventana para que seleccione una ciudad destino
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnCCiudadTarif_Click(sender As Object, e As EventArgs) Handles btnCCiudadTarif.Click
        Try
            txtTarifDestino.Text = txtCLugarRecogida.Text
            hfTipo.Value = "C"
            BuscarCiudadTarifa()
            ShowModalBoxTarifa(True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Generico servicios"

    ''' <summary>
    ''' Eventos del gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub grid_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvAviones.RowCommand, gvHoteles.RowCommand, gvTrenes.RowCommand, gvCoches.RowCommand
        If (e.CommandName = "Sel") Then
            Try
                Dim idServ As Integer = CInt(e.CommandArgument)
                Select Case CType(sender, GridView).ID
                    Case "gvAviones"
                        PintarServiciosAvion(IdPresupuesto, idServ)
                    Case "gvHoteles"
                        PintarServiciosHotel(IdPresupuesto, idServ)
                    Case "gvTrenes"
                        PintarServiciosTren(IdPresupuesto, idServ)
                    Case "gvCoches"
                        PintarServiciosCocheAlquiler(IdPresupuesto, idServ)
                End Select
            Catch batzEx As BatzException
                Master.MensajeInfo = batzEx.Termino
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Cuando se acepta en la pantalla modal generica
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAceptarModalDel_Click(sender As Object, e As EventArgs) Handles btnAceptarModalDel.Click
        Try
            Dim idServ As Integer = CInt(hfModalParam.Value)
            Dim logmensa As String = "Se ha borrado el servicio de [SERVICIO] " & idServ & " del presupuesto " & IdPresupuesto
            Dim presupBLL As New BLL.PresupuestosBLL
            presupBLL.DeleteServicio(idServ)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Servicio borrado")
            Select Case hfModalAction.Value
                Case "AV"
                    PintarServiciosAvion(IdPresupuesto)
                    logmensa = logmensa.Replace("[SERVICIO]", "avion")
                Case "HO"
                    PintarServiciosHotel(IdPresupuesto)
                    logmensa = logmensa.Replace("[SERVICIO]", "hotel")
                Case "TR"
                    PintarServiciosTren(IdPresupuesto)
                    logmensa = logmensa.Replace("[SERVICIO]", "tren")
                Case "CA"
                    PintarServiciosCocheAlquiler(IdPresupuesto)
                    logmensa = logmensa.Replace("[SERVICIO]", "coche de alquiler")
            End Select
            log.Info(logmensa)
            RefressTotal()
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al completar la accion")
        End Try
        ShowModal(False)
    End Sub

    ''' <summary>
    ''' Configura la pantalla modal
    ''' </summary>    
    ''' <param name="action">Tipo de accion</param>
    ''' <param name="param">Parametro opcional</param>
    Private Function ConfigureModal(ByVal action As String, Optional ByVal param As String = "") As String
        Dim script As New StringBuilder
        script.AppendLine("$('#" & hfModalAction.ClientID & "').val('" & action & "');")
        If (param <> String.Empty) Then script.AppendLine("$('#" & hfModalParam.ClientID & "').val('" & param & "');")
        script.AppendLine("$('#divModalDel').modal('show'); return false;")
        Return script.ToString
    End Function

    ''' <summary>
    ''' Muestra el panel modal generico para todas las confirmaciones
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModal(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModalDel').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModalDel').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True) '2º intruccion necesaria por estar dentro de un updatePanel
        End If
    End Sub

#End Region

#Region "Buscador de tarifas"

    ''' <summary>
    ''' Busca las tarifas
    ''' </summary>
    Private Sub BuscarCiudadTarifa()
        Dim tarifBLL As New BLL.TarifasServBLL
        Dim lTarifas As List(Of ELL.TarifaServicios) = tarifBLL.loadTarifaList(New ELL.TarifaServicios With {.IdPlanta = Master.IdPlantaGestion, .Destino = txtTarifDestino.Text}, Master.IdPlantaGestion)
        gvCiudadesTarifas.DataSource = lTarifas
        gvCiudadesTarifas.DataBind()
    End Sub

    ''' <summary>
    ''' Se busca entre los destinos existentes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSearchT_Click(sender As Object, e As EventArgs) Handles btnSearchT.ServerClick
        Try
            BuscarCiudadTarifa()
            ShowModalBoxTarifa(True)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvCiudadesTarifas_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvCiudadesTarifas.PageIndexChanging
        gvCiudadesTarifas.PageIndex = e.NewPageIndex
        BuscarCiudadTarifa()
        ShowModalBoxTarifa(True)
    End Sub

    ''' <summary>
    ''' Se añade la ciudad de la tarifa al control del que proviene
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvCiudadesTarifas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCiudadesTarifas.RowCommand
        If (e.CommandName = "Select") Then
            Dim idTarifa As Integer = CInt(e.CommandArgument)
            Dim tarifBLL As New BLL.TarifasServBLL
            Dim oTarifa As ELL.TarifaServicios = tarifBLL.loadTarifaInfo(idTarifa)
            Dim myTarifaAnno As ELL.TarifaServicios.Lineas
            Select Case hfTipo.Value
                Case "H"
                    lblHCiudadTarif.Text = oTarifa.Destino.ToUpper
                    hfHCiudadTarif.Value = idTarifa
                    myTarifaAnno = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = CDate(lblFIda.Text).Year)
                    If (myTarifaAnno IsNot Nothing) Then
                        lblHTarifaDiaO.Text = myTarifaAnno.TarifaHotel
                        If (txtHFEntrada.Text <> String.Empty AndAlso txtHFSalida.Text <> String.Empty) Then
                            Dim timeDays As Integer = CDate(txtHFSalida.Text).Subtract(txtHFEntrada.Text).Days
                            lblHTarifaDiaO.Text = Math.Round(timeDays * myTarifaAnno.TarifaHotel, 2)
                        End If
                    End If
                Case "A"
                    lblACiudadTarif.Text = oTarifa.Destino.ToUpper
                    hfACiudadTarif.Value = idTarifa
                    myTarifaAnno = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = CDate(lblFIda.Text).Year)
                    If (myTarifaAnno IsNot Nothing) Then
                        lblATarifaObj.Text = myTarifaAnno.TarifaAvion
                    End If
                Case "C"
                    lblCCiudadTarif.Text = oTarifa.Destino.ToUpper
                    hfCCiudadTarif.Value = idTarifa
                    myTarifaAnno = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = CDate(lblFIda.Text).Year)
                    If (myTarifaAnno IsNot Nothing) Then
                        lblCTarifaDiaO.Text = myTarifaAnno.TarifaCocheAlquiler
                        If (txtCDiaRecog.Text <> String.Empty AndAlso txtCDiaDev.Text <> String.Empty) Then
                            Dim timeDays As Integer = CDate(txtCDiaDev.Text).Subtract(txtCDiaRecog.Text).Days
                            If (timeDays = 0) Then timeDays = 1 'Si solo se selecciona un dia
                            lblCTarifaDiaO.Text = Math.Round(timeDays * myTarifaAnno.TarifaCocheAlquiler, 2)
                        End If
                    End If
            End Select
            ShowModalBoxTarifa(False)
        End If
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvCiudadesTarifas_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvCiudadesTarifas.RowDataBound
        If (e.Row.RowType = DataControlRowType.EmptyDataRow OrElse e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim tarif As ELL.TarifaServicios = e.Row.DataItem
            CType(e.Row.FindControl("lblDestino"), Label).Text = tarif.Destino
            CType(e.Row.FindControl("lblNivel"), Label).Text = tarif.Destino
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvCiudadesTarifas, "Select$" + CStr(tarif.Id))
        End If
    End Sub

    ''' <summary>
    ''' Muestra el panel modal para cambiar los datos de tarifa
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalBoxTarifa(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModalTarifa').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModalTarifa').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True) '2º intruccion necesaria por estar dentro de un updatePanel
        End If
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Guarda los datos genericosb
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            Dim stateNew, stateOld As Integer
            stateOld = -1 : stateNew = -1
            If (trNewPresupState.Visible = True) Then
                If (CInt(ddlEstado.SelectedValue) = ELL.Presupuesto.EstadoPresup.Enviado) Then
                    stateOld = CInt(hfEstado.Value)
                    stateNew = ELL.Presupuesto.EstadoPresup.Enviado
                ElseIf (CInt(ddlEstado.SelectedValue) = ELL.Presupuesto.EstadoPresup.Validado) Then
                    stateOld = CInt(hfEstado.Value)
                    stateNew = ELL.Presupuesto.EstadoPresup.Validado
                ElseIf (CInt(ddlEstado.SelectedValue) = ELL.Presupuesto.EstadoPresup.Rechazado) Then
                    stateOld = CInt(hfEstado.Value)
                    stateNew = ELL.Presupuesto.EstadoPresup.Rechazado
                End If
            End If
            If (SavePresupuesto(stateNew, stateOld, (trNewPresupState.Visible = True AndAlso ddlEstado.SelectedValue <> "-1"))) Then
                If (stateNew = ELL.Presupuesto.EstadoPresup.Enviado And (stateOld = ELL.Presupuesto.EstadoPresup.Creado OrElse stateOld = ELL.Presupuesto.EstadoPresup.Rechazado)) Then
                    AvisarPorEmail()
                End If
                Volver()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al guardar los datos del presupuesto " & IdPresupuesto)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar")
        End Try
    End Sub

    ''' <summary>
    ''' Se guardan los datos
    ''' </summary>
    ''' <returns></returns>
    Private Function SavePresupuesto(ByRef stateNew As Integer, ByRef stateOld As Integer, ByVal changeState As Boolean) As Boolean
        If (txtFechaLimite.Text <> String.Empty AndAlso CDate(txtFechaLimite.Text) > CDate(lblFVuelta.Text)) Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La fecha limite de emision no puede ser mayor que la fecha de vuelta del viaje")
            Return False
        Else
            Dim presupBLL As New BLL.PresupuestosBLL
            Dim oPresupuesto As ELL.Presupuesto = RecogerDatosPresup(stateOld, stateNew)
            Dim idUser As Integer = If(changeState, Master.Ticket.IdUser, 0) 'Si es nuevo o cambia el estado, se guarda el cambio de estado
            presupBLL.SavePresupuesto(oPresupuesto, False, idUser)
            Dim sms As String = String.Empty
            sms = "Se han guardado los cambios del presupuesto " & IdPresupuesto
            If (stateOld <> stateNew) Then
                sms &= " y a cambiado del estado " & [Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), stateOld) & " al estado " & [Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), stateNew)
            End If
            log.Info(sms)
            Return True
        End If
    End Function

    ''' <summary>
    ''' Envia un email al responsable de validacion
    ''' </summary>    
    Private Sub AvisarPorEmail()
        Try
            'Se envia el email al responsable para que pueda aceptarlo
            If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
                Try
                    Dim paramBLL As New SabLib.BLL.ParametrosBLL
                    Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                    Dim bidaiakBLL As New BLL.BidaiakBLL
                    Dim userBLL As New SabLib.BLL.UsuariosComponent
                    Dim idResp As Integer = CInt(hfRespVal.Value)
                    Dim oResp As New SabLib.ELL.Usuario With {.Id = idResp}
                    oResp = userBLL.GetUsuario(oResp)
                    Dim subject As String = "Presupuesto de viaje " & " (" & IdPresupuesto & ")"
                    Dim sPerfil As String() = bidaiakBLL.loadProfile(Master.IdPlantaGestion, idResp, CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon")))
                    Dim linkUrl As String = If(sPerfil(1) = "0", "index.aspx", "index_Directo.aspx")
                    linkUrl &= "?presup=" & IdPresupuesto
                    Dim body As String = PageBase.getBodyHmtl("Presupuesto de viaje", "V" & IdPresupuesto, Master.Ticket.NombreCompleto & " ha enviado el presupuesto del viaje (" & lblIdViaje.Text & ") para ser revisado. Acceda a bidaiak para consultarlo", linkUrl, (sPerfil(1) = "0"))
                    SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), oResp.Email, subject, body, serverEmail)
                    log.Info("PRESUPUESTO AGENCIA: Se ha enviado un email al responsable " & oResp.NombreCompleto & " para avisarle del presupuesto del viaje (" & IdPresupuesto & ")")
                Catch ex As Exception
                    log.Error("PRESUPUESTO AGENCIA: No se ha podido avisar al responsable del envio del presupuesto", ex)
                End Try
            End If
        Catch batzEx As BatzException
            log.Error(batzEx.Termino, batzEx.Excepcion)
        Catch ex As Exception
            log.Error("No se ha podido avisar por email al responsable " & lblRespVal.Text & " del envio del presupuesto " & IdPresupuesto, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Muestra las lineas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnCrearPresup_Click(sender As Object, e As EventArgs) Handles btnCrearPresup.Click
        Try
            If (SavePresupuesto(ELL.Presupuesto.EstadoPresup.Creado, ELL.Presupuesto.EstadoPresup.Generado, True)) Then
                cargarPresupuesto()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se envia el presupuesto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnModalEnviarPresup_Click(sender As Object, e As EventArgs) Handles btnModalEnviarPresup.Click
        Try
            Dim objetivo, real As Decimal
            objetivo = CDec(lblObjTotal.Text) : real = CDec(lblTotal.Text)
            If (real > objetivo AndAlso txtObservaciones.Text = String.Empty) Then
                ShowModalEnviar(False)
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Al exceder el objetivo, debe escribir un comentario explicando la razon")
            Else
                If (SavePresupuesto(ELL.Presupuesto.EstadoPresup.Enviado, ELL.Presupuesto.EstadoPresup.Creado, True)) Then
                    AvisarPorEmail()
                    Volver()
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al enviar el presupuesto " & IdPresupuesto)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al enviar")
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve a la solicitud del viaje
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Volver()
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>    
    Private Sub Volver()
        Response.Redirect("SolicitudAgencia.aspx?idViaje=" & IdPresupuesto, False)
    End Sub

#End Region

End Class