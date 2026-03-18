Public Class PresupServiciosNew
    Inherits PageBase

#Region "Properties"

    ''' <summary>
    ''' Id del presupuesto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property IdPresupuesto As Integer
        Get
            Return CInt(btnGuardarTop.CommandArgument)
        End Get
        Set(value As Integer)
            btnGuardarTop.CommandArgument = value
        End Set
    End Property

#End Region

#Region "Temporizador"

    ''' <summary>
    ''' Se inicializa el temporizador con 3 minutos antes para que no caduque
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub temporizador_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles temporizador.Init
        temporizador.Interval = ((Session.Timeout - 2) * 60) * 1000
    End Sub

    ''' <summary>
    ''' Tick del timer
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub temporizador_Tick(sender As Object, e As EventArgs) Handles temporizador.Tick
        log.Info("Autorrefresco de los presupuestos de agencia para que no caduque la pagina")
    End Sub

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
            itzultzaileWeb.Itzuli(labelATarifaPerso) : itzultzaileWeb.Itzuli(labelFechas) : itzultzaileWeb.Itzuli(rfvATarifaRealPerso)
            itzultzaileWeb.Itzuli(labelViaje) : itzultzaileWeb.Itzuli(labelSolicitante)
            itzultzaileWeb.Itzuli(labelHDias) : itzultzaileWeb.Itzuli(rfvHNumDias)
            itzultzaileWeb.Itzuli(labelHTarifaDiaPerso) : itzultzaileWeb.Itzuli(labelCNumDias) : itzultzaileWeb.Itzuli(rfvCNumDias) : itzultzaileWeb.Itzuli(labelCTarifaObjCab)
            itzultzaileWeb.Itzuli(labelHTarifaObjCab) : itzultzaileWeb.Itzuli(labelHTarifaRealCab) : itzultzaileWeb.Itzuli(labelCTarifaRealCab)
            itzultzaileWeb.Itzuli(labelCTarifaObjDia) : itzultzaileWeb.Itzuli(btnGuardarTop) : itzultzaileWeb.Itzuli(btnVolverTop) : itzultzaileWeb.Itzuli(btnEnviarBottom)
            itzultzaileWeb.Itzuli(labelATarifaObjCab) : itzultzaileWeb.Itzuli(labelATarifaRealCab) : itzultzaileWeb.Itzuli(labelRespondidoPor) : itzultzaileWeb.Itzuli(btnEnviarTop)
            itzultzaileWeb.Itzuli(labelRespVal) : itzultzaileWeb.Itzuli(btnPrevisualizarTop) : itzultzaileWeb.Itzuli(btnGuardarBottom) : itzultzaileWeb.Itzuli(btnVolverBottom)
            itzultzaileWeb.Itzuli(labelPresupTotal) : itzultzaileWeb.Itzuli(labelObjetivoTotal) : itzultzaileWeb.Itzuli(btnPrevisualizarBottom)
            itzultzaileWeb.Itzuli(labelConfirmMessageModal) : itzultzaileWeb.Itzuli(btnAceptarModalDel) : itzultzaileWeb.Itzuli(labelCancelarModalDel)
            itzultzaileWeb.Itzuli(labelTitleConfirmEnvio) : itzultzaileWeb.Itzuli(btnModalEnviarPresup)
            itzultzaileWeb.Itzuli(labelModalCancelEnviarPresup) : itzultzaileWeb.Itzuli(labelNivel)
        End If
        If (CInt(hfEstado.Value) = ELL.Presupuesto.EstadoPresup.Validado) Then disablePage() 'Si se acepta, no se podrán hacer modificaciones. Si se cancela sin embargo, si.
    End Sub

#End Region

#Region "Cargar presupuesto"

    ''' <summary>
    ''' Inicializa los controles del presupuesto
    ''' </summary>    
    Private Sub inicializarControles()
        lblIdViaje.Text = String.Empty : lblFIda.Text = String.Empty : lblSolicitante.Text = String.Empty
        labelRespondidoPor.Visible = False : lblUserRespuesta.Text = String.Empty : lblFVuelta.Text = String.Empty : lblNivel.Text = String.Empty
        lblRespVal.Text = String.Empty : hfRespVal.Value = String.Empty : btnEnviarTop.Visible = False : btnEnviarBottom.Visible = False
        btnPrevisualizarTop.Visible = False : btnPrevisualizarBottom.Visible = False
        lblEstado.Text = String.Empty
        divAvion.Visible = False : divHotel.Visible = False : divCocheAlq.Visible = False
        hfEstado.Value = "-1"
        Dim script As String = "window.open(""../Publico/Presupuestos/DetPresupuestoNew.aspx?idPresup=" & IdPresupuesto & "&ag=1"",""Presupuesto"");return false;"
        btnPrevisualizarTop.OnClientClick = script : btnPrevisualizarBottom.OnClientClick = script
        gvIntegrantes.DataSource = Nothing : gvIntegrantes.DataBind()
        lblATarifaObjPerso.Text = "0" : lblATarifaObjTotal.Text = "0" : lblATarifaObjTotal.Text = "0"
        lblHTarifaObjDiaPerso.Text = "0" : lblHTarifaObjTotal.Text = "0" : lblHTarifaRealTotal.Text = "0" : txtHNumDias.Text = "0"
        lblCTarifaObjDia.Text = "0" : lblCTarifaObjTotal.Text = "0" : lblCTarifaObjTotal.Text = "0"
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
            oViaje.ListaIntegrantes = oViaje.ListaIntegrantes.OrderBy(Of String)(Function(o) o.Usuario.NombreCompleto).ToList
            'Info del viaje
            lblIdViaje.Text = oViaje.IdViaje & " - " & oViaje.Destino
            lblFIda.Text = oViaje.FechaIda.ToShortDateString : lblFVuelta.Text = oViaje.FechaVuelta.ToShortDateString
            lblSolicitante.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador}, False).NombreCompleto
            lblNivel.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Viaje.eNivel), oViaje.Nivel).Replace("_", " "))
            hfNumInt.Value = oViaje.ListaIntegrantes.Count
            gvIntegrantes.DataSource = oViaje.ListaIntegrantes
            gvIntegrantes.DataBind()
            'Responsable de validacion
            lblRespVal.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oPresupuesto.IdUsuarioResponsable}, False).NombreCompleto
            hfRespVal.Value = oPresupuesto.IdUsuarioResponsable
            If (oViaje.SolicitudAgencia IsNot Nothing AndAlso oViaje.SolicitudAgencia.ServiciosSolicitados IsNot Nothing) Then
                For Each serv As ELL.SolicitudAgencia.Linea In oViaje.SolicitudAgencia.ServiciosSolicitados
                    If (serv.ServicioAgencia.Id = ELL.ServicioDeAgencia.TipoServicioAgencia.Avion) Then divAvion.Visible = True
                    If (serv.ServicioAgencia.Id = ELL.ServicioDeAgencia.TipoServicioAgencia.Hotel) Then divHotel.Visible = True
                    If (serv.ServicioAgencia.Id = ELL.ServicioDeAgencia.TipoServicioAgencia.Coche_Alquiler) Then divCocheAlq.Visible = True
                Next
            End If
            btnEnviarTop.Visible = (oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Creado OrElse oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Generado OrElse oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Rechazado)
            btnGuardarTop.Visible = btnEnviarTop.Visible
            btnPrevisualizarTop.Visible = True : btnPrevisualizarBottom.Visible = True 'Siempre que exista se podra previsualizar
            btnCrearPresupTop.Visible = (oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Generado)
            btnEnviarBottom.Visible = btnEnviarTop.Visible : btnGuardarBottom.Visible = btnGuardarTop.Visible
            btnCrearPresupBottom.Visible = btnCrearPresupTop.Visible
            pnlBotonesTop.Visible = (Not btnCrearPresupTop.Visible) : pnlBotonesBottom.Visible = pnlBotonesTop.Visible
            'Gestion del desplegable de nuevos estados
            lblEstado.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Presupuesto.EstadoPresup), oPresupuesto.Estado)).ToString.ToUpper
            If (oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Validado) Then 'Se mira a ver si esta autovalidado
                'Se ordenan descendentes los estados. El elemento 0 será validado y si el elemento 1 no es enviado, significara que se ha autovalidado
                Dim lEstados As List(Of ELL.Presupuesto.HistoricoEstado) = oPresupuesto.Estados.OrderByDescending(Function(f) f.ChangeDate).ToList
                If (lEstados.Item(1).State <> ELL.Presupuesto.EstadoPresup.Enviado) Then lblEstado.Text = itzultzaileWeb.Itzuli("Autovalidado").ToString.ToUpper
            End If
            Select Case oPresupuesto.Estado
                Case ELL.Presupuesto.EstadoPresup.Generado, ELL.Presupuesto.EstadoPresup.Creado, ELL.Presupuesto.EstadoPresup.Validado
                    If (btnCrearPresupTop.Visible) Then lblEstado.Text = itzultzaileWeb.Itzuli("Sin crear").ToString.ToUpper
                    If (oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Validado) Then
                        lblEstado.CssClass = "label label-success"
                    Else
                        lblEstado.CssClass = "label label-default"
                    End If
                Case ELL.Presupuesto.EstadoPresup.Enviado, ELL.Presupuesto.EstadoPresup.Rechazado

                    If (oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Rechazado) Then
                        lblEstado.CssClass = "label label-danger"
                    ElseIf (oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Enviado) Then
                        lblEstado.CssClass = "label label-info"
                    End If
            End Select
            'Info del viaje del presupuesto
            If (oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Validado Or oPresupuesto.Estado = ELL.Presupuesto.EstadoPresup.Rechazado) Then
                labelRespondidoPor.Visible = True
                lblUserRespuesta.Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oPresupuesto.IdUsuarioRespuesta}, False).NombreCompleto
            End If
            txtObservaciones.Text = oPresupuesto.Observaciones
            PintarServicios(oViaje, oPresupuesto)
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al cargar el presupuesto", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se deshabilitan todos los controles de la pagina
    ''' </summary>    
    Private Sub disablePage()
        btnEnviarTop.Visible = False : btnEnviarBottom.Visible = False
        btnGuardarTop.Visible = False : btnGuardarBottom.Visible = False
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
        Dim totalObj, totalReal As Decimal
        totalObj = DecimalValue(hfObjetivo.Value) : totalReal = DecimalValue(hfReal.Value)
        oPresup = presupBLL.loadInfo(IdPresupuesto)
        oPresup.Observaciones = txtObservaciones.Text.Trim
        'Integrantes
        Dim lblIdUser As Label
        Dim lInteg As New List(Of ELL.Viaje.Integrante)
        For Each row As GridViewRow In gvIntegrantes.Rows
            lblIdUser = CType(row.FindControl("lblIdUser"), Label)
            lInteg.Add(New ELL.Viaje.Integrante With {.Usuario = New SabLib.ELL.Usuario With {.Id = CInt(lblIdUser.Text)}})
        Next
        oPresup.Integrantes = lInteg
        Dim servicio As ELL.Presupuesto.Servicio = Nothing
        Dim tarifaReal, tarifaObjetivo As Decimal
        Dim numDias As Integer
        If (divAvion.Visible) Then
            If (oPresup.Servicios IsNot Nothing AndAlso oPresup.Servicios.Count > 0) Then servicio = oPresup.Servicios.Find(Function(o) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Avion)
            tarifaReal = PageBase.DecimalValue(txtATarifaRealPerso.Text)
            tarifaObjetivo = PageBase.DecimalValue(lblATarifaObjPerso.Text)
            If (servicio IsNot Nothing) Then
                servicio.TarifaReal = tarifaReal
            Else
                servicio = New ELL.Presupuesto.Servicio With {.IdViaje = oPresup.IdViaje, .TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Avion, .TarifaReal = tarifaReal, .TarifaObjetivo = tarifaObjetivo}
                oPresup.Servicios.Add(servicio)
            End If
            'totalObj += tarifaObjetivo : totalReal += tarifaReal
        End If
        If (divHotel.Visible) Then
            If (oPresup.Servicios IsNot Nothing AndAlso oPresup.Servicios.Count > 0) Then servicio = oPresup.Servicios.Find(Function(o) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel)
            tarifaReal = PageBase.DecimalValue(txtHTarifaRealDiaPerso.Text)
            tarifaObjetivo = PageBase.DecimalValue(lblHTarifaObjDiaPerso.Text)
            numDias = If(txtHNumDias.Text = String.Empty, 0, CInt(txtHNumDias.Text))
            If (servicio IsNot Nothing) Then
                servicio.TarifaReal = tarifaReal
                servicio.NumeroDias = numDias
            Else
                servicio = New ELL.Presupuesto.Servicio With {.IdViaje = oPresup.IdViaje, .TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel, .TarifaReal = tarifaReal, .TarifaObjetivo = tarifaObjetivo, .NumeroDias = numDias}
                oPresup.Servicios.Add(servicio)
            End If
            'totalObj += tarifaObjetivo : totalReal += tarifaReal
        End If
        If (divCocheAlq.Visible) Then
            If (oPresup.Servicios IsNot Nothing AndAlso oPresup.Servicios.Count > 0) Then servicio = oPresup.Servicios.Find(Function(o) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler)
            tarifaReal = PageBase.DecimalValue(txtCTarifaRealDia.Text)
            tarifaObjetivo = PageBase.DecimalValue(lblCTarifaObjDia.Text)
            numDias = If(txtCNumDias.Text = String.Empty, 0, CInt(txtCNumDias.Text))
            If (servicio IsNot Nothing) Then
                servicio.TarifaReal = tarifaReal
                servicio.NumeroDias = numDias
            Else
                servicio = New ELL.Presupuesto.Servicio With {.IdViaje = oPresup.IdViaje, .TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler, .TarifaReal = tarifaReal, .TarifaObjetivo = tarifaObjetivo, .NumeroDias = numDias}
                oPresup.Servicios.Add(servicio)
            End If
            'totalObj += tarifaObjetivo : totalReal += tarifaReal
        End If
        'Estado
        If (estadoOld = -1) Then 'Hay que saber cual es el estado old y el new sera el mismo
            estadoOld = oPresup.Estado
            estadoNew = estadoOld
        Else
            'Si se envia y el objetivo es mayor o igual que el real, se autovalida
            If (estadoNew = ELL.Presupuesto.EstadoPresup.Enviado AndAlso totalObj >= totalReal) Then estadoNew = ELL.Presupuesto.EstadoPresup.Validado
        End If
        oPresup.Estado = estadoNew
        'Si no y el estado anterior es creado o enviado y el estado nuevo es aceptado, el usuario de respuesta sera el usuario del ticket ya que se autovalida
        If ((estadoOld = ELL.Presupuesto.EstadoPresup.Creado Or estadoOld = ELL.Presupuesto.EstadoPresup.Enviado) AndAlso (estadoNew = ELL.Presupuesto.EstadoPresup.Validado)) Then
            oPresup.IdUsuarioRespuesta = Master.Ticket.IdUser
        End If
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

#Region "Servicios"

    ''' <summary>
    ''' Pinta la seccion de los servicios del avion, hotel y coche de alquiler
    ''' </summary>
    ''' <param name="oViaje">Informacion del viaje</param>
    ''' <param name="oPresup">Presupuesto</param>    
    Private Sub PintarServicios(ByVal oViaje As ELL.Viaje, ByVal oPresup As ELL.Presupuesto)
        Try
            Dim presupBLL As New BLL.PresupuestosBLL
            Dim estado As ELL.Presupuesto.EstadoPresup = ELL.Presupuesto.EstadoPresup.Creado
            Dim servicio As ELL.Presupuesto.Servicio = Nothing
            Dim servicios As List(Of ELL.Presupuesto.Servicio) = Nothing
            Dim numDias, origenTarifa As Integer
            If (oViaje Is Nothing) Then
                Dim viajesBLL As New BLL.ViajesBLL
                oViaje = viajesBLL.loadInfo(IdPresupuesto)
            End If
            If (oPresup Is Nothing) Then oPresup = presupBLL.loadInfo(IdPresupuesto)
            If (oPresup IsNot Nothing) Then
                estado = oPresup.Estado
                servicios = oPresup.Servicios
            End If
            Dim numInteg As Integer = oViaje.ListaIntegrantes.Count
            Dim numDiasViaje As Integer = oViaje.FechaVuelta.Subtract(oViaje.FechaIda).TotalDays + 1
            Dim totalReal, totalObjetivo, tarifaObjTotal, tarifaRealTotal As Decimal
            totalReal = 0 : totalObjetivo = 0
            'Avion
            '--------------------------------------------------------------------------------------
            If (divAvion.Visible) Then
                origenTarifa = -1
                If (servicios IsNot Nothing) Then servicio = servicios.Find(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Avion)
                presupBLL.CalculateTarifaObjetivoPresupNew(oViaje, oPresup.Estado, servicio, ELL.Presupuesto.Servicio.Tipo_Servicio.Avion, origenTarifa)
                labelATarifaTotal.Text = itzultzaileWeb.Itzuli("Total") & "(" & numInteg & " " & If(numInteg = 1, itzultzaileWeb.Itzuli("Persona"), itzultzaileWeb.Itzuli("Personas")) & ")"
                tarifaObjTotal = servicio.TarifaObjetivoTotal(numInteg)
                tarifaRealTotal = servicio.TarifaRealTotal(numInteg)
                lblATarifaObjPerso.Text = servicio.TarifaObjetivo
                lblATarifaObjTotal.Text = tarifaObjTotal
                txtATarifaRealPerso.Text = servicio.TarifaReal
                lblATarifaRealTotal.Text = tarifaRealTotal
                OrigenTarifaDestino(lblAOrigenTarifa, origenTarifa)
                totalReal += tarifaRealTotal
                totalObjetivo += tarifaObjTotal
            End If
            '---------------------------------------------------------------------------------------
            'Hotel
            If (divHotel.Visible) Then
                numDias = 0 : origenTarifa = -1
                If (servicios IsNot Nothing) Then servicio = servicios.Find(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel)
                If (servicio IsNot Nothing AndAlso servicio.NumeroDias > 0) Then
                    numDias = servicio.NumeroDias
                Else
                    numDias = numDiasViaje - 1 'El numero de noches siempre es uno menos
                    servicio = New ELL.Presupuesto.Servicio With {.IdViaje = oViaje.IdViaje, .TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel, .NumeroDias = numDias, .IdTarifaDestino = oViaje.IdTarifaDestino}
                End If
                presupBLL.CalculateTarifaObjetivoPresupNew(oViaje, oPresup.Estado, servicio, ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel, origenTarifa)
                txtHNumDias.Text = numDias
                labelHTarifaTotal.Text = itzultzaileWeb.Itzuli("Total") & "(" & numDias & " " & If(numDias = 1, itzultzaileWeb.Itzuli("noche"), itzultzaileWeb.Itzuli("noches")) & " / " & numInteg & " " & If(numInteg = 1, itzultzaileWeb.Itzuli("Persona"), itzultzaileWeb.Itzuli("Personas")) & ")"
                tarifaObjTotal = servicio.TarifaObjetivoTotal(numInteg)
                tarifaRealTotal = servicio.TarifaRealTotal(numInteg)
                lblHTarifaObjDiaPerso.Text = servicio.TarifaObjetivo
                lblHTarifaObjTotal.Text = tarifaObjTotal
                txtHTarifaRealDiaPerso.Text = servicio.TarifaReal
                lblHTarifaRealTotal.Text = tarifaRealTotal
                OrigenTarifaDestino(lblHOrigenTarifa, origenTarifa)
                totalReal += tarifaRealTotal
                totalObjetivo += tarifaObjTotal
            End If
            'Coche alquiler
            If (divCocheAlq.Visible) Then
                numDias = 0 : origenTarifa = -1
                If (servicios IsNot Nothing) Then servicio = servicios.Find(Function(o As ELL.Presupuesto.Servicio) o.TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler)
                If (servicio IsNot Nothing AndAlso servicio.NumeroDias > 0) Then
                    numDias = servicio.NumeroDias
                Else
                    numDias = numDiasViaje
                    servicio = New ELL.Presupuesto.Servicio With {.IdViaje = oViaje.IdViaje, .TipoServicio = ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler, .NumeroDias = numDiasViaje, .IdTarifaDestino = oViaje.IdTarifaDestino}
                End If
                presupBLL.CalculateTarifaObjetivoPresupNew(oViaje, oPresup.Estado, servicio, ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler, origenTarifa)
                txtCNumDias.Text = numDias
                labelCTarifaTotal.Text = itzultzaileWeb.Itzuli("Total") & "(" & numDias & " " & If(numDias = 1, itzultzaileWeb.Itzuli("dia"), itzultzaileWeb.Itzuli("dias")) & ")"
                tarifaObjTotal = servicio.TarifaObjetivoTotal(numInteg)
                tarifaRealTotal = servicio.TarifaRealTotal(numInteg)
                lblCTarifaObjDia.Text = servicio.TarifaObjetivo
                lblCTarifaObjTotal.Text = tarifaObjTotal
                txtCTarifaRealDia.Text = servicio.TarifaReal
                lblCTarifaRealTotal.Text = tarifaRealTotal
                OrigenTarifaDestino(lblCOrigenTarifa, origenTarifa)
                totalReal += tarifaRealTotal
                totalObjetivo += tarifaObjTotal
            End If
            'Totales
            If (totalReal <= totalObjetivo) Then
                lblTotal.CssClass = "label label-success"
            Else
                lblTotal.CssClass = "label label-danger"
            End If
            lblTotal.Text = Math.Round(totalReal, 2) : hfReal.Value = Math.Round(totalReal, 2)
            lblObjTotal.Text = Math.Round(totalObjetivo, 2) : hfObjetivo.Value = Math.Round(totalObjetivo, 2)
            Dim editable As Boolean = (oPresup.Estado <> ELL.Presupuesto.EstadoPresup.Validado AndAlso oPresup.Estado <> ELL.Presupuesto.EstadoPresup.Enviado)
            txtATarifaRealPerso.Enabled = editable : txtCNumDias.Enabled = editable
            txtCTarifaRealDia.Enabled = editable : txtHNumDias.Enabled = editable
            txtHTarifaRealDiaPerso.Enabled = editable
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al pintar la seccion de servicios", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Indica de donde coge la tarifa
    ''' </summary>
    ''' <param name="lbl">Label donde escribira</param>
    ''' <param name="origen">Origen de la tarifa</param>
    Private Sub OrigenTarifaDestino(ByVal lbl As Label, ByVal origen As Integer)
        Select Case origen
            Case 0 : lbl.Text = itzultzaileWeb.Itzuli("La tarifa objetivo se obtiene de las tarifas de servicios")
            Case 1 : lbl.Text = itzultzaileWeb.Itzuli("La tarifa objetivo se obtiene de las tarifas de servicios genericas")
            Case 2 : lbl.Text = itzultzaileWeb.Itzuli("Como el presupuesto esta aprobado, la tarifa objetivo se obtiene de los datos guardados en el presupuesto")
        End Select
    End Sub

#End Region

#Region "Generico servicios"

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

#Region "Botones"

    ''' <summary>
    ''' Guarda los datos genericosb
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardarTop.Click, btnGuardarBottom.Click
        Try
            Dim stateNew, stateOld As Integer
            stateOld = -1 : stateNew = -1
            If (SavePresupuesto(stateNew, stateOld, False, False)) Then
                'If (stateNew = ELL.Presupuesto.EstadoPresup.Enviado And (stateOld = ELL.Presupuesto.EstadoPresup.Creado OrElse stateOld = ELL.Presupuesto.EstadoPresup.Rechazado)) Then
                '    AvisarPorEmail()
                'End If
                PintarServicios(Nothing, Nothing)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
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
    Private Function SavePresupuesto(ByRef stateNew As Integer, ByRef stateOld As Integer, ByVal changeState As Boolean, ByRef autovalidado As Boolean) As Boolean
        Dim presupBLL As New BLL.PresupuestosBLL
        Dim oPresupuesto As ELL.Presupuesto = RecogerDatosPresup(stateOld, stateNew)
        Dim idUser As Integer = If(changeState, Master.Ticket.IdUser, 0) 'Si es nuevo o cambia el estado, se guarda el cambio de estado        
        Dim fIda As Date = CDate(lblFIda.Text) : Dim fVuelta As Date = CDate(lblFVuelta.Text)
        Dim numDiasViaje As Integer = fVuelta.Subtract(fIda).TotalDays + 1
        autovalidado = False
        If (oPresupuesto.Servicios.Exists(Function(o) o.NumeroDias > numDiasViaje)) Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El numero de dias informado no puede exceder los dias del viaje")
            Return False
        Else
            presupBLL.SavePresupuestoNew(oPresupuesto, False, idUser)
            If (changeState AndAlso stateNew = ELL.Presupuesto.EstadoPresup.Validado) Then
                Dim lServicios As List(Of ELL.Presupuesto.Servicio) = presupBLL.loadServicios(oPresupuesto.IdViaje)
                'Se actualizan las tarifas con el valor objetivo                   
                For Each serv As ELL.Presupuesto.Servicio In lServicios
                    Select Case serv.TipoServicio
                        Case ELL.Presupuesto.Servicio.Tipo_Servicio.Avion
                            serv.TarifaObjetivo = DecimalValue(lblATarifaObjPerso.Text)
                        Case ELL.Presupuesto.Servicio.Tipo_Servicio.Hotel
                            serv.TarifaObjetivo = DecimalValue(lblHTarifaObjDiaPerso.Text)
                        Case ELL.Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler
                            serv.TarifaObjetivo = DecimalValue(lblCTarifaObjDia.Text)
                    End Select
                Next
                presupBLL.UpdateServiciosTarifasObjetivo(lServicios)
                autovalidado = True
            End If
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
    ''' <param name="autovalidado">Si se autovalida, mandara un email informativo, si no le especificara para que valide</param>
    Private Sub AvisarPorEmail(ByVal autovalidado As Boolean)
        Try
            'Se envia el email al responsable para que pueda aceptarlo si no se autovalida
            If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
                Try
                    Dim paramBLL As New SabLib.BLL.ParametrosBLL
                    Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                    Dim bidaiakBLL As New BLL.BidaiakBLL
                    Dim userBLL As New SabLib.BLL.UsuariosComponent
                    Dim idResp As Integer = CInt(hfRespVal.Value)
                    Dim oResp As New SabLib.ELL.Usuario With {.Id = idResp}
                    oResp = userBLL.GetUsuario(oResp)
                    Dim subject As String = "Presupuesto de viaje" & " (" & IdPresupuesto & ")"
                    Dim sPerfil As String() = bidaiakBLL.loadProfile(Master.IdPlantaGestion, idResp, CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon")))
                    Dim linkUrl As String = If(sPerfil(1) = "0", "index.aspx", "index_Directo.aspx")
                    linkUrl &= "?presup=" & IdPresupuesto
                    Dim bodyAux As String = "ha enviado el presupuesto del viaje (" & lblIdViaje.Text & ") para ser revisado. Acceda a bidaiak para consultarlo"
                    If (autovalidado) Then bodyAux = "ha generado el presupuesto del viaje (" & lblIdViaje.Text & ") y como el importe es menor que el objetivo, se ha autovalidado. Acceda a bidaiak si quiere consultarlo"
                    Dim body As String = PageBase.getBodyHmtl("Presupuesto de viaje", "V" & IdPresupuesto, Master.Ticket.NombreCompleto & " " & bodyAux, linkUrl, (sPerfil(1) = "0"))
                    SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), oResp.Email, subject, body, serverEmail)
                    If (autovalidado) Then
                        log.Info("PRESUPUESTO AGENCIA: Se ha enviado un email al responsable " & oResp.NombreCompleto & " para avisarle de la autovalidacion del presupuesto del viaje (" & IdPresupuesto & ")")
                    Else
                        log.Info("PRESUPUESTO AGENCIA: Se ha enviado un email al responsable " & oResp.NombreCompleto & " para avisarle del presupuesto del viaje (" & IdPresupuesto & ")")
                    End If
                Catch ex As Exception
                    If (autovalidado) Then
                        log.Error("PRESUPUESTO AGENCIA: No se ha podido avisar al responsable de la autovalidacion del presupuesto", ex)
                    Else
                        log.Error("PRESUPUESTO AGENCIA: No se ha podido avisar al responsable del envio del presupuesto", ex)
                    End If
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
    Private Sub btnCrearPresup_Click(sender As Object, e As EventArgs) Handles btnCrearPresupTop.Click, btnCrearPresupBottom.Click
        Try
            If (SavePresupuesto(ELL.Presupuesto.EstadoPresup.Creado, ELL.Presupuesto.EstadoPresup.Generado, True, False)) Then
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
                Dim autovalidado As Boolean = False
                Dim newState As ELL.Presupuesto.EstadoPresup = If(real > objetivo, ELL.Presupuesto.EstadoPresup.Enviado, ELL.Presupuesto.EstadoPresup.Validado)
                If (SavePresupuesto(newState, ELL.Presupuesto.EstadoPresup.Creado, True, autovalidado)) Then
                    AvisarPorEmail(autovalidado)
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
    ''' Muestra un mensaje para enviar el presupuesto. Si el objetivo es mayor, se validará sola
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnEnviarTop_Click(sender As Object, e As EventArgs) Handles btnEnviarTop.Click, btnEnviarBottom.Click
        Dim totalObj, totalReal As Decimal
        totalObj = DecimalValue(hfObjetivo.Value) : totalReal = DecimalValue(hfReal.Value)
        'Si se ha escrito algo en el momento, se pierden valores
        lblObjTotal.Text = totalObj : lblTotal.Text = totalReal
        If (totalReal = 0) Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El presupuesto tiene que tener un coste asignado")
        Else
            If (totalObj >= totalReal) Then
                labelModalEnvioMessage.CssClass = "text-primary"
                labelModalEnvioMessage.Text = itzultzaileWeb.Itzuli("Si continua se guardaran los datos y se autovalidara el presupuesto por estar dentro del objetivo. Podra comenzar con la tramitacion del viaje ¿Desea continuar?")
            Else
                labelModalEnvioMessage.CssClass = "text-danger"
                labelModalEnvioMessage.Text = itzultzaileWeb.Itzuli("Si continua se guardaran los datos y se avisara al responsable del envio del presupuesto. Debera esperar a su validacion para tramitar el viaje. ¿Desea continuar?")
            End If
            ShowModalEnviar(True)
        End If
    End Sub

    ''' <summary>
    ''' Vuelve a la solicitud del viaje
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolverTop.Click, btnVolverBottom.Click
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