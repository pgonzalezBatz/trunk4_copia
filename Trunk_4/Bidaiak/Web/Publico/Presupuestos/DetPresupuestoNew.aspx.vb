Public Class DetPresupuestoNew
    Inherits PageBase

#Region "Properties"

    Private total As Decimal = 0
    Private totalObjetivo As Decimal = 0

    ''' <summary>
    ''' Devuelve el numero de planes distintos
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property NumPlanes As Integer
        Get
            If (ViewState("NumPlanes") Is Nothing) Then
                Return 0
            Else
                Return CInt(ViewState("NumPlanes"))
            End If
        End Get
        Set(value As Integer)
            ViewState("NumPlanes") = value
        End Set
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Carga de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Detalle del presupuesto"
                btnVolver.Visible = (Request.QueryString("ag") Is Nothing)  'Cuando viene de la agencia, se abre un pop up
                mostrarDetalle(CInt(Request.QueryString("idPresup")))
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelViaje) : itzultzaileWeb.Itzuli(labelRespVal)
            itzultzaileWeb.Itzuli(labelSolicitante) : itzultzaileWeb.Itzuli(labelEstado)
            itzultzaileWeb.Itzuli(labelRespondidoPor) : itzultzaileWeb.Itzuli(labelObserAg) : itzultzaileWeb.Itzuli(btnAprobar)
            itzultzaileWeb.Itzuli(btnRechazar) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(labelObserVal)
            itzultzaileWeb.Itzuli(labelPresupTotal) : itzultzaileWeb.Itzuli(labelObjetivoTotal) : itzultzaileWeb.Itzuli(labelDesglose)
            itzultzaileWeb.Itzuli(lblTextoDiasAntelacionOk) : itzultzaileWeb.Itzuli(labelTextoNoOkAnt1)
            itzultzaileWeb.Itzuli(labelTextoNoOkAnt2) : itzultzaileWeb.Itzuli(labelTextoNoOkAnt3) : itzultzaileWeb.Itzuli(labelTextoNoOkAnt4)
            itzultzaileWeb.Itzuli(lnkHistoricoEstados) : itzultzaileWeb.Itzuli(labelTitleModalHistorico) : itzultzaileWeb.Itzuli(labelNivel)
        End If
    End Sub

#End Region

#Region "Mostrar detalle"

    ''' <summary>
    ''' Carga el detalle del presupuesto
    ''' </summary>
    ''' <param name="idPresupuesto">Id del presupuesto</param>    
    Private Sub mostrarDetalle(ByVal idPresupuesto As Integer)
        Try
            btnAprobar.Enabled = False : btnRechazar.Enabled = False : labelRespondidoPor.Visible = False : pnlDiasAntelacionOK.Visible = False : pnlDiasAntelacionNoOK.Visible = False : pnlDiasAntelacionSolicitud.Visible = False
            Dim presupBLL As New BLL.PresupuestosBLL
            Dim oPresup As ELL.Presupuesto = presupBLL.loadInfo(idPresupuesto)
            If (oPresup Is Nothing) Then
                log.Error("Se ha intentado previsualizar el presupuesto " & idPresupuesto & " del cual no se han obtenido datos")
                Master.MensajeError = itzultzaileWeb.Itzuli("No existe el presupuesto especificado")
            Else
                Dim viajesBLL As New BLL.ViajesBLL
                Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(oPresup.IdViaje)
                hfIdViaje.Value = oViaje.IdViaje
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                btnAprobar.Enabled = Not hasProfile(BLL.BidaiakBLL.Profiles.Agencia) AndAlso (oPresup.Estado = ELL.Presupuesto.EstadoPresup.Enviado OrElse oPresup.Estado = ELL.Presupuesto.EstadoPresup.Rechazado)
                btnRechazar.Enabled = Not hasProfile(BLL.BidaiakBLL.Profiles.Agencia) AndAlso (oPresup.Estado = ELL.Presupuesto.EstadoPresup.Enviado OrElse oPresup.Estado = ELL.Presupuesto.EstadoPresup.Validado)
                btnAprobar.CommandArgument = oPresup.IdViaje
                lblIdViaje.Text = oViaje.IdViaje & " - " & oViaje.Destino
                lblSolicitante.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador}, False).NombreCompleto
                lblRespVal.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oPresup.IdUsuarioResponsable}, False).NombreCompleto
                lblDescViaje.Text = If(oViaje.Descripcion.Length > 0, oViaje.Descripcion, "S/C")
                lblNivel.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Viaje.eNivel), oViaje.Nivel).Replace("_", " "))
                NumPlanes = oViaje.ListaIntegrantes.Max(Function(o As ELL.Viaje.Integrante) o.NumPlan)
                oViaje.ListaIntegrantes = oViaje.ListaIntegrantes.OrderBy(Of String)(Function(o) o.Usuario.NombreCompleto).ToList
                gvIntegrantes.DataSource = oViaje.ListaIntegrantes
                gvIntegrantes.DataBind()
                lblEstado.Text = [Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), oPresup.Estado).ToUpper
                Select Case oPresup.Estado
                    Case ELL.Presupuesto.EstadoPresup.Validado
                        Dim lEstados As List(Of ELL.Presupuesto.HistoricoEstado) = oPresup.Estados.OrderByDescending(Function(f) f.ChangeDate).ToList
                        If (lEstados.Item(1).State <> ELL.Presupuesto.EstadoPresup.Enviado) Then lblEstado.Text = itzultzaileWeb.Itzuli("Autovalidado").ToString.ToUpper
                        lblEstado.CssClass = "label label-success"
                    Case ELL.Presupuesto.EstadoPresup.Rechazado
                        lblEstado.CssClass = "label label-danger"
                    Case ELL.Presupuesto.EstadoPresup.Enviado
                        lblEstado.CssClass = "label label-info"
                    Case ELL.Presupuesto.EstadoPresup.Creado, ELL.Presupuesto.EstadoPresup.Generado
                        lblEstado.CssClass = "label label-default"
                End Select
                'Se muestra el mensaje de anticipacion
                If (oPresup.Estado = ELL.Presupuesto.EstadoPresup.Enviado OrElse oPresup.Estado = ELL.Presupuesto.EstadoPresup.Validado OrElse oPresup.Estado = ELL.Presupuesto.EstadoPresup.Rechazado) Then
                    'Mensaje del validador
                    Dim diasAntelacion As Integer
                    Dim fValidacion As Date
                    Dim estadoPresup As ELL.Presupuesto.HistoricoEstado
                    Select Case oPresup.Estado
                        Case ELL.Presupuesto.EstadoPresup.Enviado : fValidacion = Now.Date
                        Case ELL.Presupuesto.EstadoPresup.Validado, ELL.Presupuesto.EstadoPresup.Rechazado
                            If (oPresup.Estados IsNot Nothing AndAlso oPresup.Estados.Count > 0) Then
                                oPresup.Estados = oPresup.Estados.OrderByDescending(Of Date)(Function(o) o.ChangeDate).ToList 'Se ordena descendente para que siempre coja el mas reciente primero                                
                                estadoPresup = oPresup.Estados.Find(Function(o) o.State = oPresup.Estado)
                                If (estadoPresup IsNot Nothing) Then
                                    fValidacion = estadoPresup.ChangeDate
                                Else 'Es antiguo
                                    fValidacion = DateTime.MinValue
                                End If
                            End If
                    End Select
                    If (fValidacion = DateTime.MinValue) Then
                        diasAntelacion = 30 'si viene datetime.minvalue, es anterior a la creacion de la tabla de estados y por tanto, lo dejamos como validado ok.
                    Else
                        diasAntelacion = oViaje.FechaIda.Subtract(fValidacion).TotalDays
                    End If
                    If (((oViaje.Nivel = ELL.Viaje.eNivel.Nacional OrElse oViaje.Nivel = ELL.Viaje.eNivel.Europa_y_norte_Africa) AndAlso (diasAntelacion >= SolicitudViaje.DIAS_ANTELACION_EUROPA_Y_NORTE_AFRICA)) OrElse
                        (oViaje.Nivel = ELL.Viaje.eNivel.Resto_del_mundo AndAlso diasAntelacion >= SolicitudViaje.DIAS_ANTELACION_RESTO_MUNDO)) Then
                        pnlDiasAntelacionOK.Visible = True
                    Else 'No ok
                        pnlDiasAntelacionNoOK.Visible = True
                        labelTextoNoOkAnt3.Text = labelTextoNoOkAnt3.Text.Replace("[DIAS]", SolicitudViaje.DIAS_ANTELACION_EUROPA_Y_NORTE_AFRICA)
                        labelTextoNoOkAnt4.Text = labelTextoNoOkAnt4.Text.Replace("[DIAS]", SolicitudViaje.DIAS_ANTELACION_RESTO_MUNDO)
                    End If
                    'Mensaje de la solicitud                    
                    pnlDiasAntelacionSolicitud.Visible = True
                    diasAntelacion = oViaje.FechaIda.Subtract(oViaje.FechaSolicitud).TotalDays
                    If (((oViaje.Nivel = ELL.Viaje.eNivel.Nacional OrElse oViaje.Nivel = ELL.Viaje.eNivel.Europa_y_norte_Africa) AndAlso (diasAntelacion >= SolicitudViaje.DIAS_ANTELACION_EUROPA_Y_NORTE_AFRICA)) OrElse
                        (oViaje.Nivel = ELL.Viaje.eNivel.Resto_del_mundo AndAlso diasAntelacion >= SolicitudViaje.DIAS_ANTELACION_RESTO_MUNDO)) Then
                        pnlDiasAntelacionSolicitud.CssClass = "alert alert-success"
                        lblTextoDiasAntelacionSol.Text = itzultzaileWeb.Itzuli("El viaje se ha planificado con antelacion suficiente")
                    Else 'No ok
                        pnlDiasAntelacionSolicitud.CssClass = "alert alert-danger"
                        lblTextoDiasAntelacionSol.Text = itzultzaileWeb.Itzuli("La planificacion del viaje se ha realizando fuera de plazo") & " : " & oViaje.FechaSolicitud.ToShortDateString
                    End If
                End If
                '---------------------------------------------------
                If (oPresup.Estado = ELL.Presupuesto.EstadoPresup.Validado OrElse oPresup.Estado = ELL.Presupuesto.EstadoPresup.Rechazado) Then
                    labelRespondidoPor.Visible = True
                    lblUserRespuesta.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oPresup.IdUsuarioRespuesta}, False).NombreCompleto
                    lblUserRespuesta.CssClass = If(oPresup.Estado = ELL.Presupuesto.EstadoPresup.Validado, "text-success", "text-danger")
                End If
                lblObservArg.Text = If(oPresup.Observaciones <> String.Empty, oPresup.Observaciones, "S/C")
                txtObservVal.Text = oPresup.ObservacionesValidador
                divAvion.Visible = False : divHotel.Visible = False : divCocheAlq.Visible = False
                PintarServicios(oViaje, oPresup)
                'If (oPresup.Servicios.Exists(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Avion)) Then PintarServiciosAvion(oPresup.Servicios, oViaje.ListaIntegrantes)
                'If (oPresup.Servicios.Exists(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel)) Then PintarServiciosHotel(oPresup.Servicios, oViaje.ListaIntegrantes)
                'If (oPresup.Servicios.Exists(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler)) Then PintarServiciosCocheAlquiler(oPresup.Servicios, oViaje.ListaIntegrantes)
                If (total <= totalObjetivo) Then
                    lblTotal.CssClass = "label label-success"
                Else
                    lblTotal.CssClass = "label label-danger"
                End If
                lblTotal.Style.Add("font-size", "18px")
                lblTotal.Text = Math.Round(total, 2)
                lblObjTotal.Text = Math.Round(totalObjetivo, 2)
                pnlBotones.Visible = Not (hasProfile(BLL.BidaiakBLL.Profiles.Agencia))
                btnAprobar.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmar accion"), itzultzaileWeb.Itzuli("¿Desea aprobar el presupuesto?"), "Approve")
                btnRechazar.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmar accion"), itzultzaileWeb.Itzuli("Va a rechazar el presupuesto. ¿Desea continuar?"), "Reject")
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar del detalle del presupuesto", ex)
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
            CType(e.Row.FindControl("lblIntegrante"), Label).Text = integ.Usuario.NombreCompleto
            CType(e.Row.FindControl("lblFechasViaje"), Label).Text = integ.FechaIda.ToShortDateString & " - " & integ.FechaVuelta.ToShortDateString
        End If
    End Sub

    ''' <summary>
    ''' Muestra el historico de estados de anticipos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkHistoricoEstados_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkHistoricoEstados.Click
        Try
            Dim presupBLL As New BLL.PresupuestosBLL
            Dim lEstados As List(Of ELL.Presupuesto.HistoricoEstado) = presupBLL.loadHistoricoEstados(CInt(hfIdViaje.Value))
            lEstados = lEstados.OrderBy(Of Date)(Function(o) o.ChangeDate).ToList
            gvEstados.DataSource = lEstados
            gvEstados.DataBind()
            ShowModalHistorico()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Configura la pantalla modal
    ''' </summary>
    ''' <param name="titleMessage">Titulo</param>
    ''' <param name="confirmMessage">Mensaje de confirmacion</param>
    ''' <param name="action">Tipo de accion</param>    
    Private Function ConfigureModal(ByVal titleMessage As String, ByVal confirmMessage As String, ByVal action As String) As String
        Dim script As New StringBuilder
        script.AppendLine("$('#" & labelTitleModal.ClientID & "').text('" & titleMessage & "');")
        script.AppendLine("$('#" & labelConfirmMessageModal.ClientID & "').text('" & confirmMessage.Replace("'", "") & "');")
        script.AppendLine("$('#" & hfModalAction.ClientID & "').val('" & action & "');")
        script.AppendLine("$('#divModal').modal('show'); return false;")
        Return script.ToString
    End Function

    ''' <summary>
    ''' Configura la pantalla modal de historico de estados
    ''' </summary>    
    Private Sub ShowModalHistorico()
        ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModalHistorico').modal('show');", True)
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
            Dim estado As ELL.Presupuesto.HistoricoEstado = DirectCast(e.Row.DataItem, ELL.Presupuesto.HistoricoEstado)
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim myUser As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = estado.IdUser})
            DirectCast(e.Row.FindControl("lblEstado"), Label).Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), estado.State))
            DirectCast(e.Row.FindControl("lblFecha"), Label).Text = estado.ChangeDate.ToShortDateString & " " & estado.ChangeDate.ToShortTimeString
            DirectCast(e.Row.FindControl("lblUsuario"), Label).Text = myUser.NombreCompleto
        End If
    End Sub

#End Region

#Region "Servicios"

    ''' <summary>
    ''' Pinta la seccion de los servicios del avion, hotel y coche de alquiler
    ''' </summary>
    ''' <param name="oViaje">Informacion del viaje</param>
    ''' <param name="oPresup">Presupuesto</param>    
    Private Sub PintarServicios(ByVal oViaje As ELL.Viaje, ByVal oPresup As ELL.Presupuesto)
        Try
            Dim presupBLL As New BLL.PresupuestosBLL
            Dim servicio As ELL.Presupuesto.Servicio = Nothing
            Dim origenTarifa As Integer
            Dim numInteg As Integer = oViaje.ListaIntegrantes.Count
            Dim numDiasViaje As Integer = oViaje.FechaVuelta.Subtract(oViaje.FechaIda).TotalDays + 1
            Dim tarifaObj, tarifaReal As Decimal
            'Avion
            '--------------------------------------------------------------------------------------
            servicio = oPresup.Servicios.Find(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Avion)
            If (servicio IsNot Nothing) Then
                divAvion.Visible = True
                origenTarifa = -1
                presupBLL.CalculateTarifaObjetivoPresupNew(oViaje, oPresup.Estado, servicio, ELL.Presupuesto.Servicio.Tipo_Servicio.Avion, origenTarifa)
                tarifaObj = servicio.TarifaObjetivoTotal(numInteg)
                tarifaReal = servicio.TarifaRealTotal(numInteg)
                lblATarifaObj.Text = tarifaObj : lblATarifaReal.Text = tarifaReal
                totalObjetivo += tarifaObj : total += tarifaReal
                If (tarifaObj >= tarifaReal) Then
                    lblATarifaReal.CssClass = "label label-success"
                Else
                    lblATarifaReal.CssClass = "label label-danger"
                End If
            End If
            'Hotel
            '---------------------------------------------------------------------------------------
            servicio = oPresup.Servicios.Find(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel)
            If (servicio IsNot Nothing) Then
                divHotel.Visible = True
                origenTarifa = -1
                presupBLL.CalculateTarifaObjetivoPresupNew(oViaje, oPresup.Estado, servicio, ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel, origenTarifa)
                tarifaObj = servicio.TarifaObjetivoTotal(numInteg)
                tarifaReal = servicio.TarifaRealTotal(numInteg)
                lblHDias.Text = servicio.NumeroDias
                lblHTarifaObj.Text = tarifaObj : lblHTarifaReal.Text = tarifaReal
                totalObjetivo += tarifaObj : total += tarifaReal
                If (tarifaObj >= tarifaReal) Then
                    lblHTarifaReal.CssClass = "label label-success"
                Else
                    lblHTarifaReal.CssClass = "label label-danger"
                End If
            End If
            'Coche alquiler            
            '---------------------------------------------------------------------------------------
            servicio = oPresup.Servicios.Find(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler)
            If (servicio IsNot Nothing) Then
                divCocheAlq.Visible = True
                origenTarifa = -1
                presupBLL.CalculateTarifaObjetivoPresupNew(oViaje, oPresup.Estado, servicio, ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler, origenTarifa)
                tarifaObj = servicio.TarifaObjetivoTotal(numInteg)
                tarifaReal = servicio.TarifaRealTotal(numInteg)
                lblCNumDias.Text = servicio.NumeroDias
                lblCTarifaObj.Text = tarifaObj : lblCTarifaReal.Text = tarifaReal
                totalObjetivo += tarifaObj : total += tarifaReal
                If (tarifaObj >= tarifaReal) Then
                    lblCTarifaReal.CssClass = "label label-success"
                Else
                    lblCTarifaReal.CssClass = "label label-danger"
                End If
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al pintar la seccion de servicios", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene la fecha
    ''' </summary>
    ''' <param name="fecha">Fecha</param>
    ''' <returns></returns>    
    Private Function GetDate(ByVal fecha As Date) As String
        Dim myfecha As String = String.Empty
        If (fecha <> DateTime.MinValue) Then myfecha = fecha.ToShortDateString
        Return myfecha
    End Function

    ''' <summary>
    ''' Obtiene la fecha y la hora
    ''' </summary>
    ''' <param name="fecha">Fecha</param>
    ''' <returns></returns>    
    Private Function GetDateTime(ByVal fecha As DateTime) As String
        Dim myfecha As String = String.Empty
        If (fecha <> DateTime.MinValue) Then
            myfecha = fecha.ToShortDateString
            If (fecha.Hour <> 0 Or fecha.Minute <> 0) Then myfecha &= " " & fecha.ToShortTimeString
        End If
        Return myfecha
    End Function

#End Region

#Region "Botones"

    ''' <summary>
    ''' Acepta o rechaza el presupuesto y se envia un email al de la agencia para avisarle
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAceptarModalM_Click(sender As Object, e As EventArgs) Handles btnAceptarModalM.Click
        Try
            Dim bAprobar As Boolean = (hfModalAction.Value = "Approve")
            Dim idPresup As Integer = CInt(btnAprobar.CommandArgument)
            Dim presupBLL As New BLL.PresupuestosBLL
            Dim oPresup As ELL.Presupuesto = presupBLL.loadInfo(idPresup)
            Dim oldStatus As ELL.Presupuesto.EstadoPresup = oPresup.Estado
            oPresup.IdUsuarioRespuesta = Master.Ticket.IdUser
            oPresup.ObservacionesValidador = txtObservVal.Text
            Dim lServicios As List(Of ELL.Presupuesto.Servicio) = oPresup.Servicios
            'Se actualizan las tarifas. Si lo rechaza se ponen null, si no el valor
            Dim viajesBLL As New BLL.ViajesBLL
            Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(idPresup, bSoloCabecera:=True)
            For Each serv As ELL.Presupuesto.Servicio In lServicios
                Select Case serv.TipoServicio
                    Case ELL.Presupuesto.Servicio.Tipo_Servicio.Avion
                        If (bAprobar) Then
                            presupBLL.CalculateTarifaObjetivoPresupNew(oViaje, oPresup.Estado, serv, ELL.Presupuesto.Servicio.Tipo_Servicio.Avion, 0)
                        Else
                            serv.TarifaObjetivo = Decimal.MinValue
                        End If
                    Case ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel
                        If (bAprobar) Then
                            presupBLL.CalculateTarifaObjetivoPresupNew(oViaje, oPresup.Estado, serv, ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel, 0)
                        Else
                            serv.TarifaObjetivo = Decimal.MinValue
                        End If
                    Case ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler
                        If (bAprobar) Then
                            presupBLL.CalculateTarifaObjetivoPresupNew(oViaje, oPresup.Estado, serv, ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler, 0)
                        Else
                            serv.TarifaObjetivo = Decimal.MinValue
                        End If
                End Select
            Next
            oPresup.Estado = If(bAprobar, ELL.Presupuesto.EstadoPresup.Validado, ELL.Presupuesto.EstadoPresup.Rechazado)
            presupBLL.SavePresupuesto(oPresup, False, Master.Ticket.IdUser)
            presupBLL.UpdateServiciosTarifasObjetivo(lServicios)
            If (bAprobar) Then
                log.Info("Se ha aprobado el presupuesto " & idPresup)
            Else
                log.Info("Se ha rechazado el presupuesto " & idPresup)
            End If
            'Se envia el email al de la agencia            
            If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
                Try
                    Dim paramBLL As New SabLib.BLL.ParametrosBLL
                    Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                    Dim bidaiakBLL As New BLL.BidaiakBLL
                    Dim emailsAccesoDirecto, emailsAccesoPortal As String
                    emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
                    Dim lUsersAgencia As List(Of String()) = bidaiakBLL.loadUsersProfile(Master.IdPlantaGestion, BLL.BidaiakBLL.Profiles.Agencia, CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon")), True)
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
                        log.Info("PRESUPUESTO AGENCIA:No se han encontrado ningun email para avisar a la agencia " & If(bAprobar, "de la aprobacion", "del rechazo") & " del presupuesto (" & idPresup & ")")
                    Else
                        Dim linkUrl, body, bodyEmail As String
                        Dim subject As String = "Presupuesto de viaje " & " (" & idPresup & ") : " & If(bAprobar, "APROBADO", "RECHAZADO")
                        If (bAprobar) Then
                            body = Master.Ticket.NombreCompleto & "<span style='color:#56be56;'> ha aprobado el presupuesto del viaje (" & lblIdViaje.Text & ").</span>"
                        Else
                            body = Master.Ticket.NombreCompleto & "<span style='color:#FF0000;'> ha rechazado el presupuesto del viaje (" & lblIdViaje.Text & ").</span>"
                        End If
                        body &= " Acceda a bidaiak para consultarlo"
                        Dim https As Boolean = False
                        If (Master.IdPlantaGestion = 1) Then https = True 'A los de la agencia de Igorre, como estan en otra red, se les crea el link con https
                        If (emailsAccesoPortal <> String.Empty) Then
                            linkUrl = "Index.aspx?presup=" & idPresup
                            bodyEmail = PageBase.getBodyHmtl("Presupuesto de viaje", "V" & idPresup, body, linkUrl, True, https)
                            SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                            log.Info("PRESUPUESTO AGENCIA: Se ha enviado un email al responsable de agencia para avisarle " & If(bAprobar, "de la aprobacion", "del rechazo") & " del presupuesto (" & idPresup & ") con acceso por el portal => " & emailsAccesoPortal)
                        End If
                        If (emailsAccesoDirecto <> String.Empty) Then
                            linkUrl = "Index_Directo.aspx?presup=" & idPresup
                            bodyEmail = PageBase.getBodyHmtl("Presupuesto de viaje", "V" & idPresup, body, linkUrl, False, https)
                            SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                            log.Info("PRESUPUESTO AGENCIA: Se ha enviado un email al responsable de agencia para avisarle " & If(bAprobar, "de la aprobacion", "del rechazo") & " del presupuesto (" & idPresup & ") con acceso directo => " & emailsAccesoDirecto)
                        End If
                    End If
                Catch ex As Exception
                    log.Error("PRESUPUESTO AGENCIA: No se ha podido avisar al responsable la agencia " & If(bAprobar, "de la aprobacion", "del rechazo") & " del presupuesto (" & idPresup & ")", ex)
                End Try
            End If
            Volver()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Ha ocurrido un error al responder al presupuesto " & btnAprobar.CommandArgument, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al responder al presupuesto")
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado o al presupuesto dependiendo del perfil
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Volver()
    End Sub

    ''' <summary>
    ''' Vuelve
    ''' </summary>    
    Private Sub Volver()
        If (hasProfile(BLL.BidaiakBLL.Profiles.Agencia)) Then
            Response.Redirect("../../Agencia/Presupservicios.aspx?IdViaje=" & btnAprobar.CommandArgument, False)
        Else
            Response.Redirect("Presupuestos.aspx", False)
        End If
    End Sub

#End Region

End Class