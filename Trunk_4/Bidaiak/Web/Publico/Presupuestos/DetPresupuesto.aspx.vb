Public Class DetPresupuesto
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
            itzultzaileWeb.Itzuli(labelViaje) : itzultzaileWeb.Itzuli(labelRespVal) : itzultzaileWeb.Itzuli(labelViajeros)
            itzultzaileWeb.Itzuli(labelSolicitante) : itzultzaileWeb.Itzuli(labelFLimite) : itzultzaileWeb.Itzuli(labelEstado)
            itzultzaileWeb.Itzuli(labelRespondidoPor) : itzultzaileWeb.Itzuli(labelObserAg) : itzultzaileWeb.Itzuli(btnAprobar)
            itzultzaileWeb.Itzuli(btnRechazar) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(labelObserVal)
            itzultzaileWeb.Itzuli(labelPresupTotal) : itzultzaileWeb.Itzuli(labelObjetivoTotal) : itzultzaileWeb.Itzuli(labelDesglose)
            itzultzaileWeb.Itzuli(labelInfoPlan) : itzultzaileWeb.Itzuli(lblTextoDiasAntelacionOk) : itzultzaileWeb.Itzuli(labelTextoNoOkAnt1)
            itzultzaileWeb.Itzuli(labelTextoNoOkAnt2) : itzultzaileWeb.Itzuli(labelTextoNoOkAnt3) : itzultzaileWeb.Itzuli(labelTextoNoOkAnt4)
            itzultzaileWeb.Itzuli(lnkHistoricoEstados) : itzultzaileWeb.Itzuli(labelTitleModalHistorico)
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
                btnAprobar.Enabled = (oPresup.Estado = ELL.Presupuesto.EstadoPresup.Enviado OrElse oPresup.Estado = ELL.Presupuesto.EstadoPresup.Rechazado)
                btnRechazar.Enabled = (oPresup.Estado = ELL.Presupuesto.EstadoPresup.Enviado OrElse oPresup.Estado = ELL.Presupuesto.EstadoPresup.Validado)
                btnAprobar.CommandArgument = oPresup.IdViaje
                lblIdViaje.Text = oViaje.IdViaje & " - " & oViaje.Destino
                lblSolicitante.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador}, False).NombreCompleto
                lblRespVal.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oPresup.IdUsuarioResponsable}, False).NombreCompleto
                lblDescViaje.Text = oViaje.Descripcion
                NumPlanes = oViaje.ListaIntegrantes.Max(Function(o As ELL.Viaje.Integrante) o.NumPlan)
                labelInfoPlan.Visible = (NumPlanes > 0)
                oViaje.ListaIntegrantes = oViaje.ListaIntegrantes.OrderBy(Of String)(Function(o) o.Usuario.NombreCompleto).ToList
                gvIntegrantes.DataSource = oViaje.ListaIntegrantes
                gvIntegrantes.DataBind()
                lblEstado.Text = [Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), oPresup.Estado).ToUpper
                Select Case oPresup.Estado
                    Case ELL.Presupuesto.EstadoPresup.Validado
                        lblEstado.CssClass = "label label-success"
                    Case ELL.Presupuesto.EstadoPresup.Rechazado
                        lblEstado.CssClass = "label label-danger"
                    Case ELL.Presupuesto.EstadoPresup.Enviado
                        lblEstado.CssClass = "label label-info"
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
                If (oPresup.FechaLimiteEmision <> Date.MinValue) Then lblFLimite.Text = oPresup.FechaLimiteEmision.ToShortDateString
                lblObservArg.Text = If(oPresup.Observaciones <> String.Empty, oPresup.Observaciones, "S/C")
                txtObservVal.Text = oPresup.ObservacionesValidador
                If (oPresup.Servicios.Exists(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Avion)) Then PintarServiciosAvion(oPresup.Servicios, oViaje.ListaIntegrantes)
                If (oPresup.Servicios.Exists(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel)) Then PintarServiciosHotel(oPresup.Servicios, oViaje.ListaIntegrantes)
                If (oPresup.Servicios.Exists(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler)) Then PintarServiciosCocheAlquiler(oPresup.Servicios, oViaje.ListaIntegrantes)
                If (oPresup.Servicios.Exists(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Tren)) Then PintarServiciosTren(oPresup.Servicios, oViaje.ListaIntegrantes)
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
            CType(e.Row.FindControl("lblPlanViaje"), Label).Text = integ.NumPlan
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
    ''' Añade los controles a la cabecera
    ''' </summary>
    ''' <param name="headerText"></param>
    Private Sub AddHeader(ByVal headerText As String, ByVal headerCssClass As String, ByRef divPanel As HtmlGenericControl, ByRef divBody As HtmlGenericControl)
        divPanel = New HtmlGenericControl With {.TagName = "div"}
        divPanel.Attributes.Add("class", headerCssClass)
        Dim myDivHeader As New HtmlGenericControl With {.TagName = "div"}
        myDivHeader.Attributes.Add("class", "panel-heading")
        Dim myLabel As New Label With {.Text = headerText}
        myLabel.Style.Add("font-weight", "bold")
        myDivHeader.Controls.Add(myLabel)
        divPanel.Controls.Add(myDivHeader)
        divBody = New HtmlGenericControl With {.TagName = "div"}
        divBody.Attributes.Add("class", "panel-body")
    End Sub

    ''' <summary>
    ''' Se añade una linea con posibles 4 columnas
    ''' </summary>
    ''' <param name="myDivBody">Div por referencia donde se añade la linea</param>    
    ''' <param name="value1">Objeto con los valores para las primeras dos columnas</param>    
    Private Sub AddLinea(ByRef myDivBody As HtmlGenericControl, ByVal value1 As Object, Optional ByVal value2 As Object = Nothing)
        Dim myDiv, myDiv2 As HtmlGenericControl
        Dim myLabel As Label
        myDiv = New HtmlGenericControl With {.TagName = "div"}
        myDiv.Attributes.Add("class", "row")
        myDiv2 = New HtmlGenericControl With {.TagName = "div"}
        myDiv2.Attributes.Add("class", "col-sm-2")
        myLabel = New Label With {.Text = value1.HeaderText}
        myDiv2.Controls.Add(myLabel)
        If (value1.Tooltip <> String.Empty) Then
            Dim span As New HtmlGenericControl With {.TagName = "span"} : span.Attributes.Add("class", "glyphicon glyphicon-info-sign text-info") : span.Attributes.Add("title", value1.Tooltip) : span.Style.Add("cursor", "help") : span.Style.Add("margin-left", "5px")
            myDiv2.Controls.Add(span)
        End If
        myDiv.Controls.Add(myDiv2)
        myDiv2 = New HtmlGenericControl With {.TagName = "div"}
        myDiv2.Attributes.Add("class", "col-sm-4")
        myLabel = New Label With {.Text = value1.Value} : myLabel.Style.Add("font-weight", "bold")
        If (value1.CssClass <> String.Empty) Then myLabel.Attributes.Add("class", value1.CssClass)
        If (value1.Styles IsNot Nothing) Then
            For Each myStyle In value1.Styles
                myLabel.Style.Add(myStyle.Key, myStyle.Value)
            Next
        End If
        myDiv2.Controls.Add(myLabel) : myDiv.Controls.Add(myDiv2)
        If (value2 IsNot Nothing) Then
            myDiv2 = New HtmlGenericControl With {.TagName = "div"}
            myDiv2.Attributes.Add("class", "col-sm-2")
            myLabel = New Label With {.Text = value2.HeaderText}
            myDiv2.Controls.Add(myLabel) : myDiv.Controls.Add(myDiv2)
            myDiv2 = New HtmlGenericControl With {.TagName = "div"}
            myDiv2.Attributes.Add("class", "col-sm-4")
            myLabel = New Label With {.Text = value2.Value} : myLabel.Style.Add("font-weight", "bold")
            If (value2.CssClass <> String.Empty) Then myLabel.Attributes.Add("class", value2.CssClass)
            If (value2.Styles IsNot Nothing) Then
                For Each myStyle In value2.Styles
                    myLabel.Style.Add(myStyle.Key, myStyle.Value)
                Next
            End If
            myDiv2.Controls.Add(myLabel) : myDiv.Controls.Add(myDiv2)
        End If
        myDivBody.Controls.Add(myDiv)
    End Sub

    ''' <summary>
    ''' Se pintan los servicios aereos si tuvieran
    ''' </summary>
    ''' <param name="lServ">Lista de servicios</param>   
    ''' <param name="lIntegrantes">Lista de integrantes</param> 
    Private Sub PintarServiciosAvion(ByVal lServ As List(Of ELL.Presupuesto.Servicio), ByVal lIntegrantes As List(Of ELL.Viaje.Integrante))
        Try
            lServ = lServ.FindAll(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Avion)
            If (lServ IsNot Nothing AndAlso lServ.Count > 0) Then
                Dim lServPlan As List(Of ELL.Presupuesto.Servicio)
                Dim tarifBLL As New BLL.TarifasServBLL
                Dim oTarifa As ELL.TarifaServicios
                Dim numPlanActual, numIntegrPlan, indexServ As Integer : Dim myLabel As Label
                Dim cssClass, tooltip As String
                Dim fIda As Date
                Dim lIntegrantesPlan As List(Of ELL.Viaje.Integrante)
                Dim styles As Dictionary(Of String, String) = Nothing
                Dim tarifaObjetivo, tarifaReal As Decimal
                Dim myDivPanel, myDivBody, myDiv, myDivPanelServ, myDivBodyServ As HtmlGenericControl
                myDivPanel = Nothing : myDivBody = Nothing
                AddHeader(itzultzaileWeb.Itzuli("Aviones"), "panel panel-primary", myDivPanel, myDivBody)
                For iPlan As Integer = 1 To NumPlanes
                    numPlanActual = iPlan
                    lServPlan = lServ.FindAll(Function(o As ELL.Presupuesto.Servicio) o.NumeroPlan = numPlanActual)
                    If (lServPlan.Count > 0) Then lServPlan = lServPlan.OrderBy(Of Date)(Function(o) o.Fecha1).ToList
                    If (NumPlanes > 1 AndAlso lServPlan.Count > 0) Then
                        Dim numIntegr As Integer = lIntegrantes.FindAll(Function(o As ELL.Viaje.Integrante) o.NumPlan = numPlanActual).Count
                        myDiv = New HtmlGenericControl With {.TagName = "div"}
                        myDiv.Attributes.Add("class", "form-group")
                        myLabel = New Label : myLabel.Style.Add("font-weight", "bold")
                        myLabel.Text = itzultzaileWeb.Itzuli("Plan de viaje") & " " & iPlan & " (" & numIntegr & " " & If(iPlan = 1, itzultzaileWeb.Itzuli("Integrante"), itzultzaileWeb.Itzuli("Integrantes")) & ")"
                        myDiv.Controls.Add(myLabel)
                        myDivBody.Controls.Add(myDiv)
                    End If
                    indexServ = 1
                    lIntegrantesPlan = lIntegrantes.FindAll(Function(o As ELL.Viaje.Integrante) o.NumPlan = numPlanActual)
                    numIntegrPlan = lIntegrantesPlan.Count
                    fIda = lIntegrantesPlan.Min(Function(o) o.FechaIda)
                    For Each oServ As ELL.Presupuesto.Servicio In lServPlan
                        myDivPanelServ = Nothing : myDivBodyServ = Nothing
                        AddHeader(itzultzaileWeb.Itzuli("Avion") & " " & indexServ, "panel panel-info", myDivPanelServ, myDivBodyServ)
                        AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Fecha"), .Value = GetDateTime(oServ.Fecha1), .cssClass = "", .Tooltip = "", .Styles = Nothing})
                        AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Ciudad origen"), .Value = oServ.Ciudad1, .cssClass = "", .Tooltip = "", .Styles = Nothing},
                                                New With {.HeaderText = itzultzaileWeb.Itzuli("Ciudad destino"), .Value = oServ.Ciudad2, .cssClass = "", .Tooltip = "", .Styles = Nothing})
                    tarifaObjetivo = 0
                    If (oServ.IdTarifaDestino <> Integer.MinValue) Then
                        oTarifa = tarifBLL.loadTarifaInfo(oServ.IdTarifaDestino)
                            Dim myTarifaAnno As ELL.TarifaServicios.Lineas = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = fIda.Year)
                            If (myTarifaAnno IsNot Nothing) Then tarifaObjetivo = (myTarifaAnno.TarifaAvion * numIntegrPlan)
                    End If
                    totalObjetivo += tarifaObjetivo
                        tarifaReal = (oServ.TarifaReal * numIntegrPlan)
                        total += tarifaReal
                        cssClass = If(oServ.TarifaReal > tarifaObjetivo, "label label-danger", "label label-success")
                        tooltip = itzultzaileWeb.Itzuli("Tarifa por persona") & " x " & numIntegrPlan & " "
                        tooltip &= " " & If(numIntegrPlan > 1, itzultzaileWeb.Itzuli("personas").ToString.ToLower, itzultzaileWeb.Itzuli("persona").ToString.ToLower)
                        styles = New Dictionary(Of String, String)
                        styles.Add("font-size", "16px")
                        AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Objetivo"), .Value = tarifaObjetivo, .cssClass = "", .Tooltip = tooltip, .Styles = Nothing},
                                                New With {.HeaderText = itzultzaileWeb.Itzuli("Real"), .Value = tarifaReal, .CssClass = cssClass, .Tooltip = "", .Styles = styles})
                        myDivPanelServ.Controls.Add(myDivBodyServ)
                        myDivBody.Controls.Add(myDivPanelServ)
                        indexServ += 1
                    Next
                Next
                myDivPanel.Controls.Add(myDivBody)
                phDetalle.Controls.Add(myDivPanel)
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar los servicios aereos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se pintan los servicios de hotel si tuvieran
    ''' </summary>
    ''' <param name="lServ">Lista de servicios</param>    
    ''' <param name="lIntegrantes">Lista de integrantes</param> 
    Private Sub PintarServiciosHotel(ByVal lServ As List(Of ELL.Presupuesto.Servicio), ByVal lIntegrantes As List(Of ELL.Viaje.Integrante))
        Try
            lServ = lServ.FindAll(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel)
            If (lServ IsNot Nothing AndAlso lServ.Count > 0) Then
                Dim lServPlan As List(Of ELL.Presupuesto.Servicio)
                Dim tarifBLL As New BLL.TarifasServBLL
                Dim oTarifa As ELL.TarifaServicios
                Dim cssClass, tooltip As String
                Dim fIda As Date
                Dim lIntegrantesPlan As List(Of ELL.Viaje.Integrante)
                Dim styles As Dictionary(Of String, String) = Nothing
                Dim numPlanActual, numIntegrPlan As Integer : Dim myLabel As Label
                Dim tarifaObjetivo, tarifaReal As Decimal : Dim numDiasReserva, indexServ As Integer
                Dim myDivPanel, myDivBody, myDiv, myDivPanelServ, myDivBodyServ As HtmlGenericControl
                myDivPanel = Nothing : myDivBody = Nothing
                AddHeader(itzultzaileWeb.Itzuli("Hoteles"), "panel panel-primary", myDivPanel, myDivBody)
                For iPlan As Integer = 1 To NumPlanes
                    numPlanActual = iPlan
                    lServPlan = lServ.FindAll(Function(o As ELL.Presupuesto.Servicio) o.NumeroPlan = numPlanActual)
                    If (lServPlan.Count > 0) Then lServPlan = lServPlan.OrderBy(Of Date)(Function(o) o.Fecha1).ToList
                    If (NumPlanes > 1 AndAlso lServPlan.Count > 0) Then
                        Dim numIntegr As Integer = lIntegrantes.FindAll(Function(o As ELL.Viaje.Integrante) o.NumPlan = numPlanActual).Count
                        myDiv = New HtmlGenericControl With {.TagName = "div"}
                        myDiv.Attributes.Add("class", "form-group")
                        myLabel = New Label : myLabel.Style.Add("font-weight", "bold")
                        myLabel.Text = itzultzaileWeb.Itzuli("Plan de viaje") & " " & iPlan & " (" & numIntegr & " " & If(iPlan = 1, itzultzaileWeb.Itzuli("Integrante"), itzultzaileWeb.Itzuli("Integrantes")) & ")"
                        myDiv.Controls.Add(myLabel)
                        myDivBody.Controls.Add(myDiv)
                    End If
                    indexServ = 1
                    lIntegrantesPlan = lIntegrantes.FindAll(Function(o As ELL.Viaje.Integrante) o.NumPlan = numPlanActual)
                    numIntegrPlan = lIntegrantesPlan.Count
                    fIda = lIntegrantesPlan.Min(Function(o) o.FechaIda)
                    For Each oServ As ELL.Presupuesto.Servicio In lServPlan
                        myDivPanelServ = Nothing : myDivBodyServ = Nothing
                        AddHeader(itzultzaileWeb.Itzuli("Hotel") & " " & indexServ, "panel panel-info", myDivPanelServ, myDivBodyServ)
                        AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Fecha entrada"), .Value = GetDate(oServ.Fecha1), .cssClass = "", .Tooltip = "", .Styles = Nothing},
                                                New With {.HeaderText = itzultzaileWeb.Itzuli("Fecha salida"), .Value = GetDate(oServ.Fecha2), .cssClass = "", .Tooltip = "", .Styles = Nothing})
                        AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Ciudad"), .Value = oServ.Ciudad1, .cssClass = "", .Tooltip = "", .Styles = Nothing},
                                                New With {.HeaderText = itzultzaileWeb.Itzuli("Tipo habitacion"), .Value = If(oServ.TipoHabitacion >= 0, [Enum].GetName(GetType(ELL.Presupuesto.Servicio.Tipo_Habitacion), oServ.TipoHabitacion), String.Empty), .cssClass = "", .Tooltip = "", .Styles = Nothing})
                        AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Hotel"), .Value = oServ.Nombre, .cssClass = "", .Tooltip = "", .Styles = Nothing},
                                                New With {.HeaderText = itzultzaileWeb.Itzuli("Regimen"), .Value = If(oServ.Regimen >= 0, [Enum].GetName(GetType(ELL.Presupuesto.Servicio.eRegimen), oServ.Regimen).ToString.Replace("_", " "), String.Empty), .cssClass = "", .Tooltip = "", .Styles = Nothing})
                        tarifaObjetivo = 0 : numDiasReserva = 0
                        If (oServ.Fecha1 <> Date.MinValue AndAlso oServ.Fecha2 <> Date.MinValue) Then numDiasReserva = oServ.Fecha2.Subtract(oServ.Fecha1).Days
                        If (oServ.IdTarifaDestino <> Integer.MinValue) Then
                            oTarifa = tarifBLL.loadTarifaInfo(oServ.IdTarifaDestino)
                            Dim myTarifaAnno As ELL.TarifaServicios.Lineas = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = fIda.Year)
                            If (myTarifaAnno IsNot Nothing) Then tarifaObjetivo = (myTarifaAnno.TarifaHotel * numDiasReserva * numIntegrPlan)
                        End If
                        totalObjetivo += tarifaObjetivo
                        tarifaReal = (oServ.TarifaReal * numDiasReserva * numIntegrPlan)
                        total += tarifaReal
                        cssClass = If(oServ.TarifaReal > tarifaObjetivo, "label label-danger", "label label-success")
                        tooltip = itzultzaileWeb.Itzuli("Tarifa por persona y noche") & " x " & numDiasReserva
                        tooltip &= " " & If(numDiasReserva > 1, itzultzaileWeb.Itzuli("dias").ToString.ToLower, itzultzaileWeb.Itzuli("dia").ToString.ToLower) & " x " & numIntegrPlan & " "
                        tooltip &= " " & If(numIntegrPlan > 1, itzultzaileWeb.Itzuli("personas").ToString.ToLower, itzultzaileWeb.Itzuli("persona").ToString.ToLower)
                        styles = New Dictionary(Of String, String)
                        styles.Add("font-size", "16px")
                        AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Objetivo"), .Value = tarifaObjetivo, .cssClass = "", .Tooltip = tooltip, .Styles = Nothing},
                                                New With {.HeaderText = itzultzaileWeb.Itzuli("Real"), .Value = tarifaReal, .cssClass = cssClass, .Tooltip = "", .Styles = styles})
                        myDivPanelServ.Controls.Add(myDivBodyServ)
                        myDivBody.Controls.Add(myDivPanelServ)
                        indexServ += 1
                    Next
                Next
                myDivPanel.Controls.Add(myDivBody)
                phDetalle.Controls.Add(myDivPanel)
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar los servicios de hotel", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se pintan los servicios de coche de alquiler si tuvieran
    ''' </summary>
    ''' <param name="lServ">Lista de servicios</param>  
    ''' <param name="lIntegrantes">Lista de integrantes</param>       
    Private Sub PintarServiciosCocheAlquiler(ByVal lServ As List(Of ELL.Presupuesto.Servicio), ByVal lIntegrantes As List(Of ELL.Viaje.Integrante))
        Try
            lServ = lServ.FindAll(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler)
            If (lServ IsNot Nothing AndAlso lServ.Count > 0) Then
                Dim lServPlan As List(Of ELL.Presupuesto.Servicio)
                Dim numPlanActual, indexServ As Integer : Dim myLabel As Label : Dim numDiasReserva As Integer = 0
                Dim styles As Dictionary(Of String, String) = Nothing
                Dim tarifaReal, tarifaObjetivo As Decimal
                Dim tarifBLL As New BLL.TarifasServBLL
                Dim oTarifa As ELL.TarifaServicios
                Dim tooltip, cssClass As String
                Dim myDivPanel, myDivBody, myDiv, myDivPanelServ, myDivBodyServ As HtmlGenericControl
                myDivPanel = Nothing : myDivBody = Nothing
                AddHeader(itzultzaileWeb.Itzuli("Coches de alquiler"), "panel panel-primary", myDivPanel, myDivBody)
                For iPlan As Integer = 1 To NumPlanes
                    numPlanActual = iPlan
                    lServPlan = lServ.FindAll(Function(o As ELL.Presupuesto.Servicio) o.NumeroPlan = numPlanActual)
                    If (lServPlan.Count > 0) Then lServPlan = lServPlan.OrderBy(Of Date)(Function(o) o.Fecha1).ToList
                    If (NumPlanes > 1 AndAlso lServPlan.Count > 0) Then
                        Dim numIntegr As Integer = lIntegrantes.FindAll(Function(o As ELL.Viaje.Integrante) o.NumPlan = numPlanActual).Count
                        myDiv = New HtmlGenericControl With {.TagName = "div"}
                        myDiv.Attributes.Add("class", "form-group")
                        myLabel = New Label : myLabel.Style.Add("font-weight", "bold")
                        myLabel.Text = itzultzaileWeb.Itzuli("Plan de viaje") & " " & iPlan & " (" & numIntegr & " " & If(iPlan = 1, itzultzaileWeb.Itzuli("Integrante"), itzultzaileWeb.Itzuli("Integrantes")) & ")"
                        myDiv.Controls.Add(myLabel)
                        myDivBody.Controls.Add(myDiv)
                    End If
                    indexServ = 1
                    For Each oServ As ELL.Presupuesto.Servicio In lServPlan
                        myDivPanelServ = Nothing : myDivBodyServ = Nothing
                        AddHeader(itzultzaileWeb.Itzuli("Coche") & " " & indexServ, "panel panel-info", myDivPanelServ, myDivBodyServ)
                        AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Fecha recogida"), .Value = GetDateTime(oServ.Fecha1), .cssClass = "", .Tooltip = "", .Styles = Nothing},
                                                New With {.HeaderText = itzultzaileWeb.Itzuli("Lugar recogida"), .Value = oServ.Ciudad1, .cssClass = "", .Tooltip = "", .Styles = Nothing})
                        AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Fecha devolucion"), .Value = GetDateTime(oServ.Fecha2), .cssClass = "", .Tooltip = "", .Styles = Nothing},
                                                New With {.HeaderText = itzultzaileWeb.Itzuli("Lugar devolucion"), .Value = oServ.Ciudad2, .cssClass = "", .Tooltip = "", .Styles = Nothing})
                        AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Categoria"), .Value = oServ.Categoria, .cssClass = "", .Tooltip = "", .Styles = Nothing})
                        If (oServ.Fecha1 <> Date.MinValue AndAlso oServ.Fecha2 <> Date.MinValue) Then
                            numDiasReserva = Math.Ceiling(CDate(oServ.Fecha2).Subtract(CDate(oServ.Fecha1)).TotalHours / 24)
                            If (numDiasReserva = 0) Then numDiasReserva = 1 'Si se coge y se deja en el mismo dia, sera solo un dia
                        End If
                        tarifaObjetivo = 0
                        If (oServ.IdTarifaDestino <> Integer.MinValue) Then
                            oTarifa = tarifBLL.loadTarifaInfo(oServ.IdTarifaDestino)
                            Dim myTarifaAnno As ELL.TarifaServicios.Lineas = oTarifa.LineasTarifa.Find(Function(o As ELL.TarifaServicios.Lineas) o.Anno = oServ.Fecha1.Year)
                            If (myTarifaAnno IsNot Nothing) Then tarifaObjetivo = (myTarifaAnno.TarifaCocheAlquiler * numDiasReserva)
                        End If
                        tarifaReal = (oServ.TarifaReal * numDiasReserva)
                        totalObjetivo += tarifaObjetivo
                        total += tarifaReal
                        cssClass = If(tarifaReal = 0, "label label-default", "label label-success")
                        tooltip = itzultzaileWeb.Itzuli("Tarifa coche por dia") & " x " & numDiasReserva
                        tooltip &= " " & If(numDiasReserva > 1, itzultzaileWeb.Itzuli("dias").ToString.ToLower, itzultzaileWeb.Itzuli("dia").ToString.ToLower)
                        styles = New Dictionary(Of String, String)
                        styles.Add("font-size", "16px")
                        AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Objetivo"), .Value = tarifaObjetivo, .cssClass = "", .Tooltip = tooltip, .Styles = Nothing},
                                                New With {.HeaderText = itzultzaileWeb.Itzuli("Real"), .Value = tarifaReal, .cssClass = cssClass, .Tooltip = "", .Styles = styles})
                        myDivPanelServ.Controls.Add(myDivBodyServ)
                        myDivBody.Controls.Add(myDivPanelServ)
                        indexServ += 1
                    Next
                Next
                myDivPanel.Controls.Add(myDivBody)
                phDetalle.Controls.Add(myDivPanel)
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar los servicios de coche de alquiler", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se pintan los servicios de tren si tuvieran
    ''' </summary>
    ''' <param name="lServ">Lista de servicios</param>
    ''' <param name="lIntegrantes">Lista de integrantes</param>         
    Private Sub PintarServiciosTren(ByVal lServ As List(Of ELL.Presupuesto.Servicio), ByVal lIntegrantes As List(Of ELL.Viaje.Integrante))
        Try
            lServ = lServ.FindAll(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Tren)
            If (lServ IsNot Nothing AndAlso lServ.Count > 0) Then
                Dim lServPlan As List(Of ELL.Presupuesto.Servicio)
                Dim numPlanActual, indexServ, numIntegrPlan As Integer
                Dim myLabel As Label
                Dim styles As Dictionary(Of String, String) = Nothing
                Dim tooltip, cssClass As String
                Dim tarifaReal As Decimal
                Dim myDivPanel, myDivBody, myDiv, myDivPanelServ, myDivBodyServ As HtmlGenericControl
                myDivPanel = Nothing : myDivBody = Nothing
                AddHeader(itzultzaileWeb.Itzuli("Trenes"), "panel panel-primary", myDivPanel, myDivBody)
                For iPlan As Integer = 1 To NumPlanes
                    numPlanActual = iPlan
                    lServPlan = lServ.FindAll(Function(o As ELL.Presupuesto.Servicio) o.NumeroPlan = numPlanActual)
                    If (lServPlan.Count > 0) Then lServPlan = lServPlan.OrderBy(Of Date)(Function(o) o.Fecha1).ToList
                    If (NumPlanes > 1 AndAlso lServPlan.Count > 0) Then
                        Dim numIntegr As Integer = lIntegrantes.FindAll(Function(o As ELL.Viaje.Integrante) o.NumPlan = numPlanActual).Count
                        myDiv = New HtmlGenericControl With {.TagName = "div"}
                        myDiv.Attributes.Add("class", "form-group")
                        myLabel = New Label : myLabel.Style.Add("font-weight", "bold")
                        myLabel.Text = itzultzaileWeb.Itzuli("Plan de viaje") & " " & iPlan & " (" & numIntegr & " " & If(iPlan = 1, itzultzaileWeb.Itzuli("Integrante"), itzultzaileWeb.Itzuli("Integrantes")) & ")"
                        myDiv.Controls.Add(myLabel)
                        myDivBody.Controls.Add(myDiv)
                    End If
                    indexServ = 1
                    numIntegrPlan = lIntegrantes.FindAll(Function(o As ELL.Viaje.Integrante) o.NumPlan = numPlanActual).Count
                    For Each oServ As ELL.Presupuesto.Servicio In lServPlan
                        myDivPanelServ = Nothing : myDivBodyServ = Nothing
                        AddHeader(itzultzaileWeb.Itzuli("Tren") & " " & indexServ, "panel panel-info", myDivPanelServ, myDivBodyServ)
                        AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Fecha"), .Value = GetDateTime(oServ.Fecha1), .cssClass = "", .Tooltip = "", .Styles = Nothing},
                                                New With {.HeaderText = itzultzaileWeb.Itzuli("Clase"), .Value = oServ.Clase, .cssClass = "", .Tooltip = "", .Styles = Nothing})
                        AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Ciudad origen"), .Value = oServ.Ciudad1, .cssClass = "", .Tooltip = "", .Styles = Nothing},
                                                New With {.HeaderText = itzultzaileWeb.Itzuli("Ciudad destino"), .Value = oServ.Ciudad2, .cssClass = "", .Tooltip = "", .Styles = Nothing})
                        tarifaReal = (oServ.TarifaReal * numIntegrPlan)
                        totalObjetivo += tarifaReal  'Se le suma la misma cantidad porque aqui no hay objetivo
                        total += tarifaReal
                        tooltip = itzultzaileWeb.Itzuli("Tarifa por persona") & " x " & numIntegrPlan & " "
                        tooltip &= " " & If(numIntegrPlan > 1, itzultzaileWeb.Itzuli("personas").ToString.ToLower, itzultzaileWeb.Itzuli("persona").ToString.ToLower)
                        cssClass = If(tarifaReal = 0, "label label-default", "label label-success") 'Como no hay objetivo, siempre se cumple
                        styles = New Dictionary(Of String, String)
                        styles.Add("font-size", "16px")
                        AddLinea(myDivBodyServ, New With {.HeaderText = itzultzaileWeb.Itzuli("Objetivo"), .Value = tarifaReal, .cssClass = "", .Tooltip = tooltip, .Styles = Nothing},
                                                New With {.HeaderText = itzultzaileWeb.Itzuli("Real"), .Value = tarifaReal, .cssClass = cssClass, .Tooltip = "", .Styles = styles})
                        myDivPanelServ.Controls.Add(myDivBodyServ)
                        myDivBody.Controls.Add(myDivPanelServ)
                        indexServ += 1
                    Next
                Next
                myDivPanel.Controls.Add(myDivBody)
                phDetalle.Controls.Add(myDivPanel)
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar los servicios de tren", ex)
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
            oPresup.IdUsuarioRespuesta = Master.Ticket.IdUser
            oPresup.Estado = If(bAprobar, ELL.Presupuesto.EstadoPresup.Validado, ELL.Presupuesto.EstadoPresup.Rechazado)
            oPresup.ObservacionesValidador = txtObservVal.Text
            presupBLL.SavePresupuesto(oPresup, False, Master.Ticket.IdUser)
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