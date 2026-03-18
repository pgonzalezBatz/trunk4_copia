Public Class DetalleAnticipo
    Inherits PageBase

    ''' <summary>
    ''' Obtiene el Id del viaje
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property IdViaje As Integer
        Get
            Return CInt(Request.QueryString("idViaje"))
        End Get
    End Property

    ''' <summary>
    ''' Carga del detalle
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Detalle del anticipo"
                mostrarDetalle()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar")
        End Try
        If (Request.QueryString("download") IsNot Nothing) Then
            log.Info("ANTICIPO:Se va a generar el pdf del recibo")
            Dim sb As String = "window.open('../../Publico/ViewDocument.aspx?tipo=reciboAnt&id=" & IdViaje & "','Recibo');"
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Recibo", sb, True)
        End If
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DetalleAnticipo_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelDivCab) : itzultzaileWeb.Itzuli(labelIdViaje) : itzultzaileWeb.Itzuli(lnkVerViaje) : itzultzaileWeb.Itzuli(labelSolicitante)
            itzultzaileWeb.Itzuli(labelLiquidador) : itzultzaileWeb.Itzuli(labelCtaContable) : itzultzaileWeb.Itzuli(labelLantegi) : itzultzaileWeb.Itzuli(labelOrganiz)
            itzultzaileWeb.Itzuli(labelDivEstados) : itzultzaileWeb.Itzuli(lnkHistoricoEstados) : itzultzaileWeb.Itzuli(lblEtSolicitado)
            itzultzaileWeb.Itzuli(lblEtPreparado) : itzultzaileWeb.Itzuli(lblEtEntregado) : itzultzaileWeb.Itzuli(lblEtCerrado)
            itzultzaileWeb.Itzuli(imgSolicitar) : itzultzaileWeb.Itzuli(imgPreparar) : itzultzaileWeb.Itzuli(imgEntregar) : itzultzaileWeb.Itzuli(imgCerrar)
            itzultzaileWeb.Itzuli(labelCancelado) : itzultzaileWeb.Itzuli(btnAnular) : itzultzaileWeb.Itzuli(labelDivAnt) : itzultzaileWeb.Itzuli(labelEurSol)
            itzultzaileWeb.Itzuli(labelEurosEntreg) : itzultzaileWeb.Itzuli(labelDivMov) : itzultzaileWeb.Itzuli(btnDevolucion) : itzultzaileWeb.Itzuli(btnTransferencia)
            itzultzaileWeb.Itzuli(btnDifCambio) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(labelTitleMHistorico) : itzultzaileWeb.Itzuli(labelTitleMEntrega)
            itzultzaileWeb.Itzuli(labelAntEntr) : itzultzaileWeb.Itzuli(chbImprimir) : itzultzaileWeb.Itzuli(btnCambioEstadoUser) : itzultzaileWeb.Itzuli(labelConfirmDeleteTitleMov)
            itzultzaileWeb.Itzuli(labelConfirmMessageMov) : itzultzaileWeb.Itzuli(btnEliminarModalMov) : itzultzaileWeb.Itzuli(labelCancelarModalMov)
            itzultzaileWeb.Itzuli(btnAceptarModalM) : itzultzaileWeb.Itzuli(labelCancelarModal) : itzultzaileWeb.Itzuli(labelInfoTransf) : itzultzaileWeb.Itzuli(labelViajeOrigTransf)
            itzultzaileWeb.Itzuli(labelPersonaTransf) : itzultzaileWeb.Itzuli(btnImprimir) : itzultzaileWeb.Itzuli(labelNoEntregadoCancelacion)
            itzultzaileWeb.Itzuli(btnMovManual) : itzultzaileWeb.Itzuli(labelTitleModalManual) : itzultzaileWeb.Itzuli(labelManualAccion) : itzultzaileWeb.Itzuli(btnManualGuardar)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa los controles del detalle
    ''' </summary>    
    Private Sub inicializarDetalle()
        lblSolicitante.Text = String.Empty : lblLiquidador.Text = String.Empty : lblIdViaje.Text = String.Empty : lblAviso.Text = String.Empty
        lblCtaContable.Text = String.Empty : lblLantegi.Text = String.Empty : lblOrganiz.Text = String.Empty : lblAvisoHGLiquidada.Text = String.Empty
        imgSolicitar.Enabled = True : imgPreparar.Enabled = True : imgPreparar.Enabled = True : imgCerrar.Enabled = True
        imgSolicitar.ForeColor = Drawing.Color.Blue : imgPreparar.ForeColor = Drawing.Color.Blue : imgPreparar.ForeColor = Drawing.Color.Blue : imgCerrar.ForeColor = Drawing.Color.Blue
        lblCtaContable.CssClass = String.Empty : labelCancelado.Visible = False
        pnlOrganizacion.Visible = False : pnlAnticSinDev.Visible = False : btnAnular.Visible = False : pnlAvisosHojasGastos.Visible = False : pnlAvisoHojaLiquidada.Visible = False
        lblViajeOrigTransf.Text = String.Empty : lblPersonaTransf.Text = String.Empty
        pnlAnticSolicitado.Visible = True : pnlAnticPorTransferencia.Visible = False : pnlNoEntregadoCancelacion.Visible = False
        gvResumen.DataSource = Nothing : gvResumen.DataBind()
    End Sub

    ''' <summary>
    ''' Muestra el detalle de un anticipo
    ''' </summary>	
    Private Sub mostrarDetalle()
        Try
            inicializarDetalle()
            Dim viajesBLL As New BLL.ViajesBLL
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(IdViaje, True)
            'Datos de cabecera
            Dim oUser As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador}, False)
            lblIdViaje.Text = "V" & IdViaje
            lblSolicitante.Text = oUser.NombreCompletoYCodpersona
            lblLiquidador.Text = oViaje.ResponsableLiquidacion.NombreCompletoYCodpersona
            hfEmailLiquidador.Value = oViaje.ResponsableLiquidacion.Email
            'Datos que dependen del departamento
            '*******************************************
            Dim dptoBLL As New BLL.DepartamentosBLL
            Dim epsilonBLL As New BLL.Epsilon(Master.IdPlantaGestion)
            Dim oDpto As ELL.Departamento = dptoBLL.loadInfoCuentaPlantaActiva(New SabLib.ELL.Usuario With {.Id = oViaje.ResponsableLiquidacion.Id}, oViaje.FechaSolicitud, Master.IdPlantaGestion)
            'Dim oDpto As ELL.Departamento = dptoBLL.loadInfo(oViaje.ResponsableLiquidacion.IdDepartamento, Master.IdPlantaGestion)
            If (oDpto IsNot Nothing) Then
                lblCtaContable.Text = oDpto.Cuenta0
                Dim info As String() = Nothing
                info = epsilonBLL.getInfoOrdenDepartamento(oDpto.CodigoDepartamento)
                If (info IsNot Nothing AndAlso info(0) = "00985") Then
                    pnlOrganizacion.Visible = True
                    lblOrganiz.Text = info(3) 'Unicamente si es de sistemas, se pintara la organizacion                    
                End If
                lblLantegi.Text = epsilonBLL.getInfoLantegi(oDpto.CodigoDepartamento)
            Else
                lblCtaContable.Text = itzultzaileWeb.Itzuli("El usuario no tiene un departamento asociado")
                lblCtaContable.CssClass = "text-danger"
            End If
            '*******************************************
            Dim anticBLL As New BLL.AnticiposBLL
            Dim lAnticipSinDev As List(Of Object) = anticBLL.loadAnticiposPendientes(Master.IdPlantaGestion, oViaje.ResponsableLiquidacion.Id, IdViaje)
            pnlAnticSinDev.Visible = (lAnticipSinDev IsNot Nothing AndAlso lAnticipSinDev.Count > 0)
            rptAnticSinDev.DataSource = lAnticipSinDev : rptAnticSinDev.DataBind()
            'Estados)
            cargarEstados(oViaje.Anticipo.Estado)
            labelCancelado.Visible = (oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.cancelada)
            hfEstadoActual.Value = oViaje.Anticipo.Estado
            'Anticipos solicitados
            Dim existeAnticipo As Boolean = (oViaje.Anticipo.AnticiposSolicitados.Count > 0)
            pnlAnticSolicitado.Visible = existeAnticipo : pnlAnticPorTransferencia.Visible = Not existeAnticipo
            If (existeAnticipo) Then
                lblFechaRequiere.Text = oViaje.Anticipo.FechaNecesidad.ToShortDateString
                selImportes.Importes = oViaje.Anticipo.AnticiposSolicitados
                selImportes.Inicializar()
                lblEurosSolicitados.Text = Math.Round(oViaje.Anticipo.EurosSolicitados, 2)
                Dim eurosEntreg As Decimal = oViaje.Anticipo.EurosEntregados
                pnlEurosEntregados.Visible = (eurosEntreg > 0)
                If (eurosEntreg > 0) Then lblEurosEntregados.Text = Math.Round(eurosEntreg, 2)
            Else
                Dim myTrans As ELL.Anticipo.Movimiento = oViaje.Anticipo.Transferencias.Find(Function(o) o.IdViajeDestino = oViaje.IdViaje)
                lblViajeOrigTransf.Text = "V" & myTrans.IdViajeOrigen
                lblPersonaTransf.Text = myTrans.UserOrigen.NombreCompletoYCodpersona
            End If
            'Se mira si se tiene que mostrar la alerta del anticipo no entregado si ha sido cancelado antes de entregar
            pnlNoEntregadoCancelacion.Visible = (oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.cancelada AndAlso Not oViaje.Anticipo.Movimientos.Exists(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Entregado))
            'Resumen movimientos)
            Dim total As Decimal = Math.Round(oViaje.Anticipo.EurosPendientes, 2)
            oViaje.Anticipo.Movimientos = oViaje.Anticipo.Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov <> ELL.Anticipo.Movimiento.TipoMovimiento.Solicitado)
            oViaje.Anticipo.Movimientos = oViaje.Anticipo.Movimientos.OrderBy(Of Date)(Function(o) o.Fecha).ToList
            Dim lMovimientos As List(Of ELL.Anticipo.Movimiento) = oViaje.Anticipo.Movimientos
            Dim numHojasValidadas As Integer = 0
            Dim idLiq As Integer = If(oViaje.ResponsableLiquidacion IsNot Nothing, oViaje.ResponsableLiquidacion.Id, Integer.MinValue)
            total -= AñadirHojasGastos(oViaje.HojasGastos, lMovimientos, numHojasValidadas, idLiq, True)
            'UnificarTransferencias(lMovimientos)
            lMovimientos = lMovimientos.OrderBy(Of Date)(Function(o) o.Fecha).ToList
            gvResumen.DataSource = lMovimientos
            gvResumen.DataBind()
            'Si el numero de hojas validadas es mayor que una, se coge la columna de usuario de la ultima fila añadida(la de hojas de gastos), y se reemplaza el nombre del usuario por un texto acumulado
            If (numHojasValidadas > 1) Then CType(gvResumen.Rows(lMovimientos.Count - 1).Cells(6).FindControl("lblUsuario"), Label).Text = "Acumulado ( " & numHojasValidadas & " )"
            If (oViaje.Anticipo.Movimientos.Count > 0) Then
                Dim myEtiqTotal As Label = CType(gvResumen.FooterRow.Cells(4).FindControl("lblEtiquetaTotal"), Label)
                Dim myLblTotal As Label = CType(gvResumen.FooterRow.Cells(5).FindControl("lblTotal"), Label)
                myLblTotal.Text = Math.Abs(total) & " €"
                myEtiqTotal.CssClass = String.Empty
                If (total > 0) Then
                    myEtiqTotal.Text = itzultzaileWeb.Itzuli("Pendiente de justificar")
                    myEtiqTotal.CssClass = "text-danger"
                ElseIf (total < 0) Then
                    myEtiqTotal.Text = itzultzaileWeb.Itzuli("Exceso")
                    myEtiqTotal.CssClass = "text-warning"
                Else
                    myEtiqTotal.Text = itzultzaileWeb.Itzuli("Liquidado")
                    myEtiqTotal.CssClass = "text-success"
                End If
            End If
            Dim hgLiq As ELL.HojaGastos = oViaje.HojasGastos.Find(Function(o) o.Usuario.Id = oViaje.ResponsableLiquidacion.Id)
            'Avisos hoja de gastos            
            If (hgLiq IsNot Nothing) Then
                If (hgLiq.Estado = ELL.HojaGastos.eEstado.Enviada) Then
                    pnlAvisosHojasGastos.Visible = True
                    lblAviso.Text = itzultzaileWeb.Itzuli("Existen hojas de gastos enviadas al responsable para su validacion")
                ElseIf (hgLiq.Estado = ELL.HojaGastos.eEstado.Rellenada) Then
                    pnlAvisosHojasGastos.Visible = True
                    If (lblAviso.Text <> String.Empty) Then lblAviso.Text &= "<br />"
                    lblAviso.Text &= "       " & itzultzaileWeb.Itzuli("Existen hojas de gastos creadas todavia sin enviar al responsable")
                End If
            Else
                pnlAvisosHojasGastos.Visible = True
                lblAviso.Text &= "       " & itzultzaileWeb.Itzuli("Todavia no se ha creado ninguna hoja de gastos")
            End If
            'Avisos hoja de gastos liquidada            
            If (hgLiq IsNot Nothing) Then
                If (hgLiq.Estados.Exists(Function(o) o(0) = ELL.HojaGastos.eEstado.Liquidada)) Then
                    pnlAvisoHojaLiquidada.Visible = True
                    Dim hojaBLL As New BLL.HojasGastosBLL
                    Dim importeLiquidacion As Decimal = hojaBLL.loadImporteLiquidacion(hgLiq.Id)
                    lblAvisoHGLiquidada.Text = itzultzaileWeb.Itzuli("Se ha abonado la hoja de gastos del liquidador") & " (" & importeLiquidacion & " €)"
                End If
            End If
            'Botones
            Dim visualizarBotones As Boolean = (Now.Date >= oViaje.FechaIda And (oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.Entregado Or oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.cerrado Or oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.cancelada))
            btnDevolucion.Visible = visualizarBotones : btnTransferencia.Visible = visualizarBotones
            btnDifCambio.Visible = visualizarBotones : btnImprimir.Visible = visualizarBotones
            btnMovManual.Visible = visualizarBotones
            btnDifCambio.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmar accion"), itzultzaileWeb.Itzuli("Al realizar una diferencia de cambio, se introducirá un nuevo movimiento para que el total quede a 0. ¿Desea continuar?"), "DifCambio")

        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New Exception("Error al mostrar el detalle del anticipo del viaje (" & IdViaje & ")", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Configura la pantalla modal
    ''' </summary>
    ''' <param name="titleMessage">Titulo</param>
    ''' <param name="confirmMessage">Mensaje de confirmacion</param>
    ''' <param name="action">Tipo de accion</param>
    ''' <param name="param">Parametro opcional</param>
    Private Function ConfigureModal(ByVal titleMessage As String, ByVal confirmMessage As String, ByVal action As String, Optional ByVal param As String = "") As String
        Dim script As New StringBuilder
        script.AppendLine("$('#" & labelTitleModal.ClientID & "').text('" & titleMessage & "');")
        script.AppendLine("$('#" & labelConfirmMessageModal.ClientID & "').text('" & confirmMessage.Replace("'", "") & "');")
        script.AppendLine("$('#" & hfModalAction.ClientID & "').val('" & action & "');")
        If (param <> String.Empty) Then script.AppendLine("$('#" & hfModalParam.ClientID & "').val('" & param & "');")
        script.AppendLine("$('#divModal').modal('show'); return false;")
        Return script.ToString
    End Function

    ''' <summary>
    ''' Gestiona las imagenes de los estados
    ''' </summary>	
    Private Sub cargarEstados(ByVal idEstado As ELL.Anticipo.EstadoAnticipo)
        Dim smsDelEntrega As String = itzultzaileWeb.Itzuli("Se van a eliminar las lineas de entrega del anticipo") & ". "
        If (idEstado <> ELL.Anticipo.EstadoAnticipo.Entregado) Then smsDelEntrega = String.Empty 'Solamente saldra este mensaje, cuando estando en entregado se vuelva para atras
        lblEtSolicitado.ForeColor = Drawing.Color.Black
        imgSolicitar.Enabled = False
        imgSolicitar.CommandArgument = ELL.Anticipo.EstadoAnticipo.solicitado
        imgSolicitar.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmar accion"), itzultzaileWeb.Itzuli("¿Desea cambiar el estado a solicitado?"), "Estado", imgSolicitar.CommandArgument)
        lblEtPreparado.ForeColor = Drawing.Color.Black
        imgPreparar.Enabled = False
        imgPreparar.CommandArgument = ELL.Anticipo.EstadoAnticipo.Preparado
        imgPreparar.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmar accion"), itzultzaileWeb.Itzuli("Al marcarlo como preparado, se le enviara un email al usuario para que informarle de que puede venir a recoger. ¿Desea continuar?"), "Estado", imgPreparar.CommandArgument)
        lblEtEntregado.ForeColor = Drawing.Color.Black
        imgEntregar.Enabled = False
        imgEntregar.CommandArgument = ELL.Anticipo.EstadoAnticipo.Entregado
        imgEntregar.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmar accion"), itzultzaileWeb.Itzuli("Se ha entregado el anticipo a la persona. ¿Desea continuar?"), "Estado", imgEntregar.CommandArgument)
        lblEtCerrado.ForeColor = Drawing.Color.Black
        imgCerrar.Enabled = False
        imgCerrar.CommandArgument = ELL.Anticipo.EstadoAnticipo.cerrado
        imgCerrar.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmar accion"), itzultzaileWeb.Itzuli("¿Desea cerrar la solicitud del anticipo?"), "Estado", imgCerrar.CommandArgument)
        btnAnular.OnClientClick = ConfigureModal(itzultzaileWeb.Itzuli("Confirmar accion"), itzultzaileWeb.Itzuli("Si anula el anticipo, se borrara y el viaje constara sin anticipo solicitado. ¿Desea continuar?"), "Anular")
        btnAnular.Visible = (idEstado = ELL.Anticipo.EstadoAnticipo.solicitado Or idEstado = ELL.Anticipo.EstadoAnticipo.Preparado)
        Select Case idEstado
            Case ELL.Anticipo.EstadoAnticipo.solicitado
                imgPreparar.Enabled = True : imgEntregar.Enabled = True
                imgSolicitar.ImageUrl = "~\App_Themes\Tema1\Images\Estados\uno_act.png"
                lblEtSolicitado.ForeColor = Drawing.Color.Blue
            Case ELL.Anticipo.EstadoAnticipo.Preparado
                imgSolicitar.Enabled = True : imgEntregar.Enabled = True
                imgPreparar.ImageUrl = "~\App_Themes\Tema1\Images\Estados\dos_act.png"
                lblEtPreparado.ForeColor = Drawing.Color.Blue
            Case ELL.Anticipo.EstadoAnticipo.Entregado
                imgSolicitar.Enabled = True : imgPreparar.Enabled = False : imgEntregar.Enabled = False : imgCerrar.Enabled = True '01/10/14:Se deja que una vez entregado, vuelva atrás por el tema de que una vez hecha una transferencia si se hace un anticipo, no se puede entregar
                imgEntregar.ImageUrl = "~\App_Themes\Tema1\Images\Estados\tres_act.png"
                lblEtEntregado.ForeColor = Drawing.Color.Blue
            Case ELL.Anticipo.EstadoAnticipo.cerrado
                imgCerrar.ImageUrl = "~\App_Themes\Tema1\Images\Estados\cuatro_act.png"
                lblEtCerrado.ForeColor = Drawing.Color.Blue
        End Select
        If (idEstado <> ELL.Anticipo.EstadoAnticipo.solicitado) Then imgSolicitar.ImageUrl = If(imgSolicitar.Enabled, "~\App_Themes\Tema1\Images\Estados\uno_on.png", "~\App_Themes\Tema1\Images\Estados\uno_off.png")
        If (idEstado <> ELL.Anticipo.EstadoAnticipo.Preparado) Then imgPreparar.ImageUrl = If(imgPreparar.Enabled, "~\App_Themes\Tema1\Images\Estados\dos_on.png", "~\App_Themes\Tema1\Images\Estados\dos_off.png")
        If (idEstado <> ELL.Anticipo.EstadoAnticipo.Entregado) Then imgEntregar.ImageUrl = If(imgEntregar.Enabled, "~\App_Themes\Tema1\Images\Estados\tres_on.png", "~\App_Themes\Tema1\Images\Estados\tres_off.png")
        If (idEstado <> ELL.Anticipo.EstadoAnticipo.cerrado) Then imgCerrar.ImageUrl = If(imgCerrar.Enabled, "~\App_Themes\Tema1\Images\Estados\cuatro_on.png", "~\App_Themes\Tema1\Images\Estados\cuatro_off.png")
    End Sub

    ''' <summary>
    ''' Añade a los movimientos existentes, movimientos de la hoja de gastos si estan validadas y pertenecen al liquidador. Solo los gastos con recibo y sin recibo. Los de kilometraje y los de visa, no contaran aqui
    ''' Se añadiran los bloques de 'Metalico', 'Kilometraje' y 'Visa'
    ''' </summary>
    ''' <param name="lHojas"></param>
    ''' <param name="lMovimientos"></param>    
    ''' <param name="numHojasValidadas">Numero de hojas validadas</param>
    ''' <param name="idLiquidador">Id del responsable de liquidacion</param>
    ''' <param name="bParaAnticipo">Cuando es para anticipo, los gastos de visa y kilometraje no cuentan</param>
    ''' <returns>Devuelve el coste de las hojas de gastos en euros</returns>
    Private Function AñadirHojasGastos(ByVal lHojas As List(Of ELL.HojaGastos), ByRef lMovimientos As List(Of ELL.Anticipo.Movimiento), ByRef numHojasValidadas As Integer, ByVal idLiquidador As Integer, Optional ByVal bParaAnticipo As Boolean = False) As Decimal
        Try
            Dim total As Decimal = 0
            If (lHojas IsNot Nothing) Then
                Dim movimiento As ELL.Anticipo.Movimiento = Nothing
                'Nos quedamos solo con las validadas
                If (idLiquidador <> Integer.MinValue) Then
                    lHojas = lHojas.FindAll(Function(o As ELL.HojaGastos) (o.Estado <> ELL.HojaGastos.eEstado.Rellenada AndAlso o.Estado <> ELL.HojaGastos.eEstado.Enviada) And o.Usuario.Id = idLiquidador)
                Else
                    lHojas = lHojas.FindAll(Function(o As ELL.HojaGastos) o.Estado <> ELL.HojaGastos.eEstado.Rellenada AndAlso o.Estado <> ELL.HojaGastos.eEstado.Enviada)
                End If
                numHojasValidadas = lHojas.Count
                Dim xbatBLL As New BLL.XbatBLL
                Dim moneda As ELL.Moneda = xbatBLL.GetMoneda("EUR")
                For Each hoja As ELL.HojaGastos In lHojas
                    If (hoja.Lineas.Count > 0) Then
                        For Each gasto As ELL.HojaGastos.Linea In hoja.Lineas
                            If (Not bParaAnticipo OrElse (bParaAnticipo AndAlso (gasto.TipoGasto = ELL.HojaGastos.Linea.eTipoGasto.Metalico))) Then
                                If (movimiento Is Nothing AndAlso (gasto.TipoGasto = ELL.HojaGastos.Linea.eTipoGasto.Metalico)) Then
                                    movimiento = New ELL.Anticipo.Movimiento With {.Id = gasto.Id, .IdAnticipo = gasto.IdHoja}
                                    movimiento.Cantidad = gasto.ImporteEuros
                                    movimiento.Moneda = moneda  'Lo ponemos en euros
                                    movimiento.UserOrigen = hoja.Usuario
                                    movimiento.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Hoja_Gastos
                                    movimiento.Fecha = hoja.GetFechaEstado(ELL.HojaGastos.eEstado.Validada)
                                Else
                                    movimiento.Cantidad += gasto.ImporteEuros
                                End If
                                movimiento.ImporteEuros = movimiento.Cantidad
                            End If
                        Next
                    Else 'Solo Visas
                        If (movimiento Is Nothing) Then
                            movimiento = New ELL.Anticipo.Movimiento With {.IdAnticipo = hoja.Id, .TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Hoja_Gastos, .ImporteEuros = 0, .Moneda = New ELL.Moneda With {.Id = 90, .Abreviatura = "Eur"},
                                                                       .UserOrigen = hoja.Usuario, .Fecha = hoja.GetFechaEstado(ELL.HojaGastos.eEstado.Validada)}
                        End If
                    End If
                Next
                If (lMovimientos Is Nothing) Then lMovimientos = New List(Of ELL.Anticipo.Movimiento)
                If (movimiento IsNot Nothing) Then
                    lMovimientos.Add(movimiento)
                    total = Math.Round(movimiento.ImporteEuros, 2)
                End If
            End If
            Return total
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al añadir las hojas de gastos", ex)
        End Try
    End Function

    ''' <summary>
    ''' Mete en un mismo concepto, los movimientos de una misma transferencia
    ''' </summary>
    ''' <param name="lMovimientos">Movimientos</param>    
    Private Sub UnificarTransferencias(ByRef lMovimientos As List(Of ELL.Anticipo.Movimiento))
        Try
            If (lMovimientos.Exists(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia)) Then
                '1º Se van a meter en una lista las transferencias como movimientos unicos, siempre y cuando el idUserOrigen,IdUserDestino y IdViajeDestino sea el mismo
                'Si son distintos, sera otra transferencia
                'A la vez, se eliminan los movimientos de transferencia separados de la lista
                Dim lTransferencias As New List(Of ELL.Anticipo.Movimiento)
                Dim myMov, oMov As ELL.Anticipo.Movimiento
                Dim xbatBLL As New BLL.XbatBLL
                Dim moneda As ELL.Moneda = xbatBLL.GetMoneda("EUR")
                Dim idViajeDest, idUserOrigen, idUserDest As Integer
                For index As Integer = lMovimientos.Count - 1 To 0 Step -1
                    oMov = lMovimientos.Item(index)
                    If (oMov.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia) Then
                        idViajeDest = oMov.IdViajeDestino : idUserOrigen = oMov.UserOrigen.Id : idUserDest = oMov.UserDestino.Id
                        myMov = lTransferencias.Find(Function(o As ELL.Anticipo.Movimiento) o.IdViajeDestino = idViajeDest And o.UserOrigen.Id = idUserOrigen And o.UserDestino.Id = idUserDest)
                        If (myMov Is Nothing) Then  'Es un nuevo movimiento
                            myMov = New ELL.Anticipo.Movimiento With {.Id = oMov.Id, .IdAnticipo = oMov.IdAnticipo}
                            myMov.Cantidad = oMov.ImporteEuros
                            myMov.Fecha = oMov.Fecha
                            myMov.ImporteEuros = myMov.Cantidad
                            myMov.Moneda = moneda  'Lo ponemos en euros
                            myMov.UserOrigen = oMov.UserOrigen
                            myMov.UserDestino = oMov.UserDestino
                            myMov.IdViajeOrigen = oMov.IdViajeOrigen
                            myMov.IdViajeDestino = oMov.IdViajeDestino
                            myMov.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia
                            myMov.CambioMonedaEUR = oMov.CambioMonedaEUR
                            lTransferencias.Add(myMov)
                        Else 'ya existe, asi que hay que sumarle las cantidades                            
                            myMov.Cantidad += oMov.ImporteEuros
                            myMov.ImporteEuros = myMov.Cantidad
                        End If
                        lMovimientos.Remove(oMov)
                    End If
                Next
                '2º Se añade el nuevo rango
                If (lTransferencias.Count > 0) Then lMovimientos.AddRange(lTransferencias)
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al unificar las transferencias", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Dado una moneda, obtiene el valor en euros
    ''' Se dividira el valor del importe por el rate especificado de la moneda
    ''' </summary>
    ''' <param name="idMoneda">Id de la moneda</param>
    ''' <param name="importe">Importe</param>
    ''' <returns>Decimal con 2 decimales</returns>	
    Private Function ObtenerRateEuros(ByVal idMoneda As Integer, ByVal importe As Decimal) As Decimal
        Try
            Dim rateEuros As Decimal = 0
            Dim xbatComp As New BLL.XbatBLL
            Dim oMon As ELL.Moneda = xbatComp.GetMoneda(idMoneda)
            If (oMon IsNot Nothing) Then
                rateEuros = Math.Round(importe / (PageBase.DecimalValue(oMon.ConversionEuros)), 2)
            Else
                Throw New Exception
            End If
            Return rateEuros
        Catch ex As Exception
            Throw New BatzException("Error al obtener el valor en euros", ex)
        End Try
    End Function

    ''' <summary>
    ''' Se enlazan los movimientos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub gvResumen_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvResumen.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header OrElse e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim xbatBLL As New BLL.XbatBLL
            Dim mov As ELL.Anticipo.Movimiento = DirectCast(e.Row.DataItem, ELL.Anticipo.Movimiento)
            Dim lnkVer As LinkButton = DirectCast(e.Row.FindControl("lnkVer"), LinkButton)
            Dim lnkElim As LinkButton = DirectCast(e.Row.FindControl("lnkElim"), LinkButton)
            Dim lblDebeMov As Label = DirectCast(e.Row.FindControl("lblDebeMov"), Label)
            Dim lblHaberMov As Label = DirectCast(e.Row.FindControl("lblHaberMov"), Label)
            Dim lblConversionEuros As Label = DirectCast(e.Row.FindControl("lblConversionEuros"), Label)
            Dim lblModo As Label = DirectCast(e.Row.FindControl("lblModo"), Label)
            If (mov.Fecha <> Date.MinValue) Then DirectCast(e.Row.FindControl("lblFechaMov"), Label).Text = mov.Fecha.ToShortDateString
            lblModo.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Anticipo.Movimiento.TipoMovimiento), mov.TipoMov).Replace("_", " "))
            If (mov.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia) Then
                lnkVer.CommandArgument = mov.IdViajeOrigen & "_" & mov.IdViajeDestino & "_" & mov.UserOrigen.Id & "_" & mov.UserDestino.Id
                Dim viajesBLL As New BLL.ViajesBLL
                Dim oViaje As ELL.Viaje = Nothing
                Dim nombreLiquidador As String = String.Empty
                Dim idViajeLiquidador As Integer = 0
                If (IdViaje = mov.IdViajeOrigen) Then
                    idViajeLiquidador = mov.IdViajeDestino
                    lblModo.Text &= " ( " & itzultzaileWeb.Itzuli("a") & " V" & mov.IdViajeDestino & " )"
                Else
                    idViajeLiquidador = mov.IdViajeOrigen
                    lblModo.Text &= " ( " & itzultzaileWeb.Itzuli("de") & " V" & mov.IdViajeOrigen & " )"
                End If
                oViaje = viajesBLL.loadInfo(idViajeLiquidador, False, False, True)
                If (oViaje.ResponsableLiquidacion IsNot Nothing) Then nombreLiquidador = "<br />L:" & oViaje.ResponsableLiquidacion.NombreCompleto & "(" & oViaje.ResponsableLiquidacion.CodPersona & ")"
                lblModo.Text &= nombreLiquidador
            Else
                lnkVer.CommandArgument = mov.UserOrigen.Id
            End If
            lnkVer.CommandName = mov.TipoMov
            lnkVer.Visible = (mov.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia OrElse mov.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Hoja_Gastos)
            lnkElim.Visible = False   'De momento, solo se podra eliminar movimientos del tipo devuelto, diferencia de cambio y manuales
            Select Case mov.TipoMov
                Case ELL.Anticipo.Movimiento.TipoMovimiento.Entregado
                    lblDebeMov.Text = Math.Round(DecimalValue(mov.Cantidad), 2)
                Case ELL.Anticipo.Movimiento.TipoMovimiento.Hoja_Gastos
                    lblHaberMov.Text = Math.Round(DecimalValue(mov.Cantidad), 2)
                    If (mov.Cantidad = 0) Then
                        lblModo.Text &= " (" & itzultzaileWeb.Itzuli("Solo visas") & ")"
                    Else
                        lblConversionEuros.Text = "-"
                    End If
                Case ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion
                    lblHaberMov.Text = Math.Round(DecimalValue(mov.Cantidad), 2)
                    lnkElim.Visible = True
                    lblConversionEuros.Text = "-"
                Case ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia
                    If (mov.IdViajeOrigen = IdViaje) Then  'Transferencia realizada desde este viaje
                        lblHaberMov.Text = Math.Round(DecimalValue(mov.Cantidad), 2)
                        lblConversionEuros.Text = "-"
                    Else 'Transferencia realizada a este viaje
                        lblDebeMov.Text = Math.Round(DecimalValue(mov.Cantidad), 2)
                    End If
                Case ELL.Anticipo.Movimiento.TipoMovimiento.Diferencia_Cambio
                    If (mov.Cantidad > 0) Then  'Si la cantidad es positiva es que se le debia dinero al usuario
                        lblDebeMov.Text = Math.Abs(Math.Round(DecimalValue(mov.Cantidad), 2))
                    Else 'Si es negativa es que debia dinero
                        lblHaberMov.Text = Math.Abs(Math.Round(DecimalValue(mov.Cantidad), 2))
                        lblConversionEuros.Text = "-"
                    End If
                    lnkElim.Visible = True
                Case ELL.Anticipo.Movimiento.TipoMovimiento.Manual
                    If (mov.Cantidad > 0) Then
                        lblDebeMov.Text = Math.Abs(Math.Round(DecimalValue(mov.Cantidad), 2))
                    Else
                        lblHaberMov.Text = Math.Abs(Math.Round(DecimalValue(mov.Cantidad), 2))
                        lblConversionEuros.Text = "-"
                    End If
                    If (mov.Comentarios <> String.Empty) Then lblModo.Text &= " (" & mov.Comentarios & ")"
                    lnkElim.Visible = True
            End Select
            lblConversionEuros.Text &= Math.Abs(Math.Round(mov.ImporteEuros, 2))
            lnkElim.OnClientClick = "$('#" & hfDeleteMovId.ClientID & "').val(" & mov.Id & ");$('#confirmDeleteMov').modal('show'); return false;"
            DirectCast(e.Row.FindControl("lblCambio"), Label).Text = If(mov.Moneda.Id = 90, "-", mov.CambioMonedaEUR)
            DirectCast(e.Row.FindControl("lblMonedaMov"), Label).Text = mov.Moneda.Nombre
            DirectCast(e.Row.FindControl("lblUsuario"), Label).Text = mov.UserOrigen.NombreCompleto
            itzultzaileWeb.Itzuli(lnkVer) : itzultzaileWeb.Itzuli(lnkElim)
        End If
    End Sub

    ''' <summary>
    ''' Evento que surge al pinchar en el icono de la lupa. Muestra el detalle de la transferencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub VerDetalleMovimiento(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Dim tipo As ELL.Anticipo.Movimiento.TipoMovimiento = CType(lnk.CommandName, ELL.Anticipo.Movimiento.TipoMovimiento)
        If (tipo = ELL.Anticipo.Movimiento.TipoMovimiento.Hoja_Gastos) Then
            Response.Redirect("..\..\Viaje\HojaGastos.aspx?idViaje=" & IdViaje & "&IdUser=" & lnk.CommandArgument & "&idViajeVolver=" & IdViaje)
        Else
            Dim params As String() = lnk.CommandArgument.Split("_")
            '(0):IdViajeOrigen,(1):IdViajeDestino,(2):idUserOrig,(3):idUserDest                        
            Dim url As String = "..\..\Viaje\TransferenciaAnticipo.aspx?orig=ANT"
            If (CInt(params(0)) = IdViaje) Then
                url &= "&idViaje=" & params(0) & "&AB=" & params(1)
            Else
                url &= "&idViaje=" & params(1) & "&BA=" & params(0)
            End If
            Response.Redirect(url)
        End If
    End Sub

    ''' <summary>
    ''' Elimina la tarifa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnEliminarModalMov_Click(sender As Object, e As EventArgs) Handles btnEliminarModalMov.Click
        Try
            'Id del movimiento
            Dim idMov As Integer = CInt(hfDeleteMovId.Value)
            Dim bidaiBLL As New BLL.BidaiakBLL
            Dim anticBLL As New BLL.AnticiposBLL
            Dim lDev As List(Of ELL.Anticipo.Movimiento) = anticBLL.loadMovimientos(IdViaje, ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion, True)
            anticBLL.DeleteMovimiento(idMov)
            If (lDev IsNot Nothing AndAlso lDev.Count > 0) Then
                Dim myDev As ELL.Anticipo.Movimiento = lDev.Find(Function(o As ELL.Anticipo.Movimiento) o.Id = idMov)
                If (myDev IsNot Nothing) Then  'Es una devolucion que se va a borrar. Se quita de la caja
                    bidaiBLL.saveSaldoCaja(New ELL.SaldoCaja With {.Cantidad = myDev.Cantidad, .Fecha = Now, .IdUsuario = Master.Ticket.IdUser, .IdPlanta = Master.IdPlantaGestion, .Operacion = ELL.SaldoCaja.EOperacion.Eliminar_Devolucion,
                                                                   .IdMoneda = myDev.Moneda.Id, .Comentario = "Se borra una devolucion en metalico del usuario " & myDev.UserOrigen.NombreCompleto & " del viaje " & myDev.IdAnticipo})
                End If
            End If
            log.Info("ANTICIPO:Se ha borrado(marcado como obsoleto) el movimiento (" & idMov & ")")
            mostrarDetalle()
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
            'Response.Redirect("DetalleAnticipo.aspx?idViaje=" & IdViaje, False)  'Se hace un response, porque si mostrabamos el detalle, el control selImportes era nothing
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
            DirectCast(e.Row.FindControl("lblEstado"), Label).Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Anticipo.EstadoAnticipo), CInt(estado(0))))
            DirectCast(e.Row.FindControl("lblFecha"), Label).Text = fecha.ToShortDateString & " " & fecha.ToShortTimeString
        End If
    End Sub

    ''' <summary>
    ''' Cambia el estado del anticipo
    ''' </summary>
    Private Sub Estado()
        Try
            Dim anticBLL As New BLL.AnticiposBLL
            Dim idEstado As Integer = CInt(hfModalParam.Value)
            If (idEstado <> ELL.Anticipo.EstadoAnticipo.Entregado) Then
                anticBLL.CambiarEstado(IdViaje, CInt(idEstado))
                If (CInt(hfEstadoActual.Value) = ELL.Anticipo.EstadoAnticipo.Entregado And idEstado <> ELL.Anticipo.EstadoAnticipo.cerrado) Then
                    'Se vuelve para atras. Se supone que esto se habilito ya que tenian que darle mas dinero. Entonces se vuelve para atras, se modifica y se vuelve a entregar. Si no se volveria a entregar
                    'la caja no quedaría cuadrada ya que al ir para atras, se ha puesto como si se devolviera dinero
                    Dim bidaiBLL As New BLL.BidaiakBLL
                    Dim lMov As List(Of ELL.Anticipo.Movimiento) = anticBLL.loadMovimientos(IdViaje, ELL.Anticipo.Movimiento.TipoMovimiento.Solicitado, False) 'Se cogen los solicitados porque al cambiar de estado para atras, se borran los mov entregados
                    For Each mov As ELL.Anticipo.Movimiento In lMov
                        bidaiBLL.saveSaldoCaja(New ELL.SaldoCaja With {.Cantidad = mov.Cantidad, .Fecha = Now, .IdUsuario = Master.Ticket.IdUser, .IdPlanta = Master.IdPlantaGestion, .Operacion = ELL.SaldoCaja.EOperacion.Devolucion_Anticipo,
                                                                       .IdMoneda = mov.Moneda.Id, .Comentario = "Se vuelve para atras en el estado del anticipo del viaje " & IdViaje & " para modificar cantidades"})
                    Next
                End If
                log.Info("ANTICIPO:Se ha cambiado el estado del anticipo del viaje(" & lblIdViaje.Text & ") de '" & [Enum].GetName(GetType(ELL.Anticipo.EstadoAnticipo), CInt(hfEstadoActual.Value)) & "' a '" & [Enum].GetName(GetType(ELL.Anticipo.EstadoAnticipo), idEstado) & "'")
                If (idEstado = ELL.Anticipo.EstadoAnticipo.Preparado) Then
                    If (ConfigurationManager.AppSettings("avisarPorEmail") = "1" AndAlso hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) Then
                        'Se le envia un email a la persona
                        Try
                            Dim paramBLL As New SabLib.BLL.ParametrosBLL
                            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                            Dim subject As String = "Anticipo" & "(" & lblIdViaje.Text & ")"
                            Dim body As String = "Ya tiene preparado su anticipo. Pásese por administración en los siguientes horarios:<br/>"
                            body &= "<ul><li>De lunes a viernes- 10:00 a 13:00</li></ul>"
                            body = PageBase.getBodyHmtl(itzultzaileWeb.Itzuli("Gestion de anticipos"), lblIdViaje.Text, body, String.Empty)
                            SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), hfEmailLiquidador.Value, subject, body, serverEmail)
                            log.Info("ANTICIPO:Se ha enviado un email para indicar que ya tiene preparado el anticipo del viaje(" & lblIdViaje.Text & ") a " & lblLiquidador.Text)
                        Catch ex As Exception
                            log.Error("ANTICIPO:No se ha podido enviar un email para indicar que ya tiene preparado el anticipo del viaje(" & lblIdViaje.Text & ") a " & lblLiquidador.Text, ex)
                        End Try
                    End If
                End If
                Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                Try
                    Response.Redirect("DetalleAnticipo.aspx?idViaje=" & IdViaje, False)  'Se hace un response, porque si mostrabamos el detalle, el control selImportes era nothing
                Catch batzEx As BatzException
                    Master.MensajeError = itzultzaileWeb.Itzuli("Estado cambiado con exito. Ha ocurrido un error al mostrar el detalle")
                End Try
            Else
                cargarUsuariosViaje(IdViaje)
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("ANTICIPO:Ha ocurrido un error al cambiar de estado el anticipo del viaje (" & IdViaje & ")", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
        End Try
    End Sub

    ''' <summary>
    ''' Carga los integrantes del viaje seleccionado
    ''' </summary>
    ''' <param name="idViaje">Id del viaje</param>	
    Private Sub cargarUsuariosViaje(ByVal idViaje As Integer)
        Try
            Dim viajeBLL As New BLL.ViajesBLL
            Dim oViaje As ELL.Viaje = viajeBLL.loadInfo(idViaje)
            lblUserEntrega.Text = oViaje.ResponsableLiquidacion.NombreCompleto
            btnCambioEstadoUser.CommandArgument = oViaje.ResponsableLiquidacion.Id
            ShowModalEntrega(True)
        Catch ex As Exception
            Throw New BatzException("Error al cargar los integrantes del viaje", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el historico de estados de anticipos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkHistoricoEstados_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkHistoricoEstados.Click
        Try
            Dim anticBLL As New BLL.AnticiposBLL
            Dim oAntic As ELL.Anticipo = anticBLL.loadInfo(IdViaje, False)
            oAntic.Estados = oAntic.Estados.OrderBy(Of Date)(Function(o) CDate(o(1))).ToList
            gvEstados.DataSource = oAntic.Estados
            gvEstados.DataBind()
            ShowModalHistorico()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el panel modal para mostrar el historico
    ''' </summary>
    Private Sub ShowModalHistorico()
        ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divHistorico').modal('show');", True)
    End Sub

    ''' <summary>
    ''' Muestra el panel modal al entregar un anticipo
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalEntrega(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divEntrega').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divEntrega').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True) '2º intruccion necesaria por estar dentro de un updatePanel
        End If
    End Sub

    ''' <summary>
    ''' Muestra el panel modal al añadir un movimiento manual
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalMovimientoManual(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divMovManual').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divMovManual').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True) '2º intruccion necesaria por estar dentro de un updatePanel
        End If
    End Sub

    ''' <summary>
    ''' Muestra el panel modal generico para todas las confirmaciones
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModal(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True) '2º intruccion necesaria por estar dentro de un updatePanel
        End If
    End Sub

    ''' <summary>
    ''' Cuando se acepta en la pantalla modal generica
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAceptarModalM_Click(sender As Object, e As EventArgs) Handles btnAceptarModalM.Click
        Try
            Select Case hfModalAction.Value
                Case "DifCambio"
                    DiferenciaCambio()
                    mostrarDetalle()
                Case "Anular"
                    Anular()
                Case "Estado"
                    Estado()
            End Select
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al completar la accion")
        End Try
        ShowModal(False)
    End Sub

    ''' <summary>
    ''' Cambia el estado del usuario a entregado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCambioEstadoUser_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCambioEstadoUser.Click
        Try
            Dim bidaiBLL As New BLL.BidaiakBLL
            Dim anticBLL As New BLL.AnticiposBLL
            anticBLL.CambiarEstado(IdViaje, ELL.Anticipo.EstadoAnticipo.Entregado, CInt(btnCambioEstadoUser.CommandArgument))
            Dim lMovs As List(Of ELL.Anticipo.Movimiento) = anticBLL.loadMovimientos(IdViaje, ELL.Anticipo.Movimiento.TipoMovimiento.Solicitado)
            If (lMovs IsNot Nothing AndAlso lMovs.Count > 0) Then
                For Each oMov As ELL.Anticipo.Movimiento In lMovs
                    bidaiBLL.saveSaldoCaja(New ELL.SaldoCaja With {.Cantidad = oMov.Cantidad, .Fecha = Now, .IdUsuario = Master.Ticket.IdUser, .IdPlanta = Master.IdPlantaGestion, .Operacion = ELL.SaldoCaja.EOperacion.Entrego_Anticipo,
                                                               .IdMoneda = oMov.Moneda.Id, .Comentario = "Entrega de anticipo al usuario " & lblUserEntrega.Text & " del viaje " & oMov.IdAnticipo})
                Next
            End If
            log.Info("ANTICIPO:Se ha cambiado el estado del anticipo del viaje(" & lblIdViaje.Text & ") a 'Entregado' y se le ha entregado a " & lblUserEntrega.Text & " (" & btnCambioEstadoUser.CommandArgument & ")")
            Dim param As String = String.Empty
            If (chbImprimir.Checked) Then param = "&download=1"
            Response.Redirect("DetalleAnticipo.aspx?idViaje=" & IdViaje & param) 'Se hace un response, porque si mostrabamos el detalle, el control selImportes era nothing            
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Redirige a la pagina de devoluciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnDevolucion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDevolucion.Click
        Response.Redirect("Devolucion.aspx?idViaje=" & IdViaje, False)
    End Sub

    ''' <summary>
    ''' Redirige a otra pagina para transferir anticipos a otro viaje
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnTransferencia_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTransferencia.Click
        Response.Redirect("..\..\Viaje/TransferenciaAnticipo.aspx?idViaje=" & IdViaje & "&orig=ANT", False)
    End Sub

    ''' <summary>
    ''' Se abre una ventana para insertar el movimiento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnMovManual_Click(sender As Object, e As EventArgs) Handles btnMovManual.Click
        txtManualCantidad.Text = String.Empty : txtManualCantidad.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Cantidad €"))
        If (ddlManualAccion.Items.Count = 0) Then
            ddlManualAccion.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Añadir gasto"), "0"))
            ddlManualAccion.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Quitar gasto"), "1"))
        End If
        ddlManualAccion.SelectedValue = "0"
        txtManualComen.Text = String.Empty
        ShowModalMovimientoManual(True)
    End Sub

    ''' <summary>
    ''' Se imprime un pdf necesario para Esti
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        Dim fs As IO.FileStream = Nothing
        Try
            Dim ruta As String = BLL.GenerarPDF.Anticipo(IdViaje, Master.Ticket.IdPlanta, Master.Ticket.Culture, Session.SessionID)
            fs = IO.File.OpenRead(ruta)
            Dim bytes(fs.Length) As Byte
            fs.Read(bytes, 0, CInt(fs.Length))
            fs.Close()
            log.Info("ANTICIPO: Se a generado el pdf del anticipo " & IdViaje)
            Response.Clear()
            Response.AppendHeader("Content-Disposition", "attachment; filename=Anticipo_V" & IdViaje & ".pdf")
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
    ''' Introduce un movimiento con el tipo diferencia de cambio
    ''' </summary>  
    Private Sub DiferenciaCambio()
        Try
            Dim anticBLL As New BLL.AnticiposBLL
            Dim oMov As New ELL.Anticipo.Movimiento With {.IdAnticipo = IdViaje}
            Dim xbatBLL As New BLL.XbatBLL
            Dim lMovs As List(Of ELL.Anticipo.Movimiento) = anticBLL.loadMovimientos(IdViaje)
            Dim cant As Decimal = 0
            For Each mov As ELL.Anticipo.Movimiento In lMovs
                Select Case mov.TipoMov
                    Case ELL.Anticipo.Movimiento.TipoMovimiento.Entregado
                        cant += Math.Round(mov.ImporteEuros, 2)
                    Case ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion
                        cant -= Math.Round(mov.ImporteEuros, 2)
                    Case ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia
                        If (mov.IdViajeOrigen = IdViaje) Then
                            cant -= Math.Round(mov.ImporteEuros, 2)
                        Else
                            cant += Math.Round(mov.ImporteEuros, 2)
                        End If
                    Case ELL.Anticipo.Movimiento.TipoMovimiento.Diferencia_Cambio 'En el caso de la dif de cambio, dependera del signo que tenga la cantidad
                        cant += mov.Cantidad
                End Select
            Next
            Dim numHojasValidadas As Integer = 0
            Dim viajesBLL As New BLL.ViajesBLL
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(IdViaje, True)
            cant -= AñadirHojasGastos(oViaje.HojasGastos, lMovs, numHojasValidadas, oViaje.ResponsableLiquidacion.Id, True)
            oMov.Cantidad = Math.Round(cant * (-1), 2)  'Se multiplica por -1 para cambiarle el signo. Si cant=30, significa que debe 30€ y para contrarrestar, le haria falta -30            
            oMov.Comentarios = "Diferencia de cambio"
            oMov.Fecha = Now
            Dim oMon As ELL.Moneda = xbatBLL.GetMoneda(ELL.Moneda.EURO)
            oMov.Moneda = oMon
            oMov.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Diferencia_Cambio
            oMov.UserOrigen = New SabLib.ELL.Usuario With {.Id = Master.Ticket.IdUser}
            anticBLL.SaveMovimiento(oMov)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Movimiento insertado")
            log.Info("DIFERENCIA_CAMBIO_ANTICIPO:Se ha realizado la diferencia de cambio del viaje(" & lblIdViaje.Text & "). La diferencia ha sido de " & oMov.Cantidad)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
            Exit Sub
        End Try
        Try
            mostrarDetalle()
        Catch batzEx As BatzException
            Master.MensajeError = "Movimiento insertado pero error al mostrar el detalle"
        End Try
    End Sub

    ''' <summary>
    ''' Se borrara el anticipo y se avisara por email al liquidador y a su validador
    ''' </summary>
    Private Sub Anular()
        Try
            Dim anticBLL As New BLL.AnticiposBLL
            Dim idLiquidador As Integer = anticBLL.Anular(IdViaje)
            log.Info("ANULAR_ANTICIPO:Se ha anulado el anticipo del viaje(" & lblIdViaje.Text & ")")
            Try
                AvisarAnulacion(IdViaje, idLiquidador)
            Catch ex As Exception
                log.Error("ANULAR_ANTICIPO:Ha fallado el envio de emails del viaje(" & lblIdViaje.Text & ")", ex)
            End Try
            Volver()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se añade el movimiento manual
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnManualGuardar_Click(sender As Object, e As EventArgs) Handles btnManualGuardar.Click
        Try
            If (txtManualCantidad.text = String.Empty OrElse txtManualComen.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Inserte todos los datos")
                ShowModalMovimientoManual(True)
            Else
                Dim anticBLL As New BLL.AnticiposBLL
                Dim xbatBLL As New BLL.XbatBLL
                Dim oMov As New ELL.Anticipo.Movimiento With {.IdAnticipo = IdViaje, .TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Manual, .Fecha = Now}
                Dim signo As Integer = If(ddlManualAccion.SelectedValue = "0", -1, 1)
                oMov.Cantidad = Math.Round(PageBase.DecimalValue(txtManualCantidad.Text) * signo, 2)
                oMov.Comentarios = txtManualComen.Text
                oMov.Moneda = xbatBLL.GetMoneda(ELL.Moneda.EURO)
                oMov.CambioMonedaEUR = 1
                oMov.UserOrigen = New SabLib.ELL.Usuario With {.Id = Master.Ticket.IdUser}
                anticBLL.SaveMovimiento(oMov)
                log.Info("Se ha añadido un movimiento manual de " & txtManualCantidad.Text & " * " & signo & " al anticipo " & IdViaje)
                ShowModalMovimientoManual(False)
                Try
                    mostrarDetalle()
                Catch batzEx As BatzException
                    Master.MensajeError = "Movimiento insertado pero error al mostrar el detalle"
                End Try
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se redirige a la pagina de solicitud de un viaje para su modificacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkVerViaje_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkVerViaje.Click
        Response.Redirect("~\Viaje\SolicitudViaje.aspx?id=" & IdViaje, False)
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Volver()
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>	
    Private Sub Volver()
        Dim origen As String = If(Request.QueryString("orig") Is Nothing, String.Empty, Request.QueryString("orig"))
        If (origen = "HGS") Then
            Response.Redirect("../HojasGastos.aspx?return=1")
        Else
            Response.Redirect("GestionAnticipos.aspx?returnIdViaje=" & IdViaje)
        End If
    End Sub

    ''' <summary>
    ''' Avisa por email al liquidador y al organizador
    ''' </summary>    
    ''' <param name="idViaje">Id del viaje</param>
    ''' <param name="idLiquidador">Id del liquidador. Se le pasa porque al haberse anulado, el idLiquidador se ha marcado a nulo</param>
    Private Sub AvisarAnulacion(ByVal idViaje As Integer, ByVal idLiquidador As Integer)
        If (ConfigurationManager.AppSettings("avisarPorEmail") = "1" AndAlso hasProfile(BLL.BidaiakBLL.Profiles.Financiero)) Then  'El administrador, no mandara emails
            Dim emailsAccesoDirecto, emailsAccesoPortal, subject, body, bodyEmail As String
            Dim idRecurso As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon"))
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
            Dim viajesBLL As New BLL.ViajesBLL
            Dim perfBLL As New BLL.BidaiakBLL
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim sPerfil As String()
            emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
            Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(idViaje, False, False, True)
            If (idLiquidador > 0) Then
                sPerfil = perfBLL.loadProfile(Master.IdPlantaGestion, idLiquidador, idRecurso)
                oViaje.ResponsableLiquidacion = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idLiquidador})
                If (sPerfil(1) = "0") Then 'Acceso por portal
                    If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                    emailsAccesoPortal &= oViaje.ResponsableLiquidacion.Email
                Else 'Acceso directo
                    If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                    emailsAccesoDirecto &= oViaje.ResponsableLiquidacion.Email
                End If
            Else
                log.Error("ANULACION_ANTICIPO: No se ha podido encontrar el liquidador del viaje para avisarle de la cancelacion del anticipo de la solicitud (V" & idViaje & ")")
            End If
            Dim oOrganizador As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador}, False)
            sPerfil = perfBLL.loadProfile(Master.IdPlantaGestion, oOrganizador.Id, idRecurso)
            If (sPerfil(1) = "0") Then 'Acceso por portal
                If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                emailsAccesoPortal &= oOrganizador.Email
            Else 'Acceso directo
                If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                emailsAccesoDirecto &= oOrganizador.Email
            End If
            If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                log.Info("ANULACION_ANTICIPO:No se han encontrado ningun email para avisar de la cancelacion del anticipo del viaje (V" & oViaje.IdViaje & ")")
            Else
                body = "Desde el departamento financiero se ha anulado el anticipo del viaje"
                subject = "Cancelacion de anticipo (V" & idViaje & ")"
                If (emailsAccesoPortal <> String.Empty) Then
                    Try
                        bodyEmail = PageBase.getBodyHmtl("Cancelacion de anticipo", "V" & idViaje, body, String.Empty, True)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                        log.Info("ANULACION_ANTICIPO:Se ha enviado un email para avisar de la cancelacion del anticipo de la solicitud (V" & idViaje & ") con acceso por el portal => " & emailsAccesoPortal)
                    Catch ex As Exception
                        log.Error("ANULACION_ANTICIPO: No se ha podido enviar un email para avisar de la cancelacion del anticipo de la solicitud (V" & idViaje & ")", ex)
                    End Try
                End If
                If (emailsAccesoDirecto <> String.Empty) Then
                    Try
                        bodyEmail = PageBase.getBodyHmtl("Cancelacion de anticipo", "V" & idViaje, body, String.Empty, False)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                        log.Info("ANULACION_ANTICIPO:Se ha enviado un email para avisar de la cancelacion del anticipo de la solicitud (V" & idViaje & ") con acceso directo => " & emailsAccesoDirecto)
                    Catch ex As Exception
                        log.Error("ANULACION_ANTICIPO: No se ha podido enviar un email para avisar de la cancelacion del anticipo de la solicitud (V" & idViaje & ")", ex)
                    End Try
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Se muestran los anticipos que no se han devuelto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rptAnticSinDev_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptAnticSinDev.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim oAntic = e.Item.DataItem
            Dim hlVerAntic As HyperLink = CType(e.Item.FindControl("hlVerAntic"), HyperLink)
            CType(e.Item.FindControl("lblAnticSinDev"), Label).Text = itzultzaileWeb.Itzuli("Tiene un anticipo pendiente") & "=>" & oAntic.CantidadPendiente
            hlVerAntic.Text = "V" & oAntic.IdAnticipo
            hlVerAntic.NavigateUrl = "DetalleAnticipo.aspx?idViaje=" & oAntic.IdAnticipo
            itzultzaileWeb.Itzuli(hlVerAntic)
        End If
    End Sub

End Class