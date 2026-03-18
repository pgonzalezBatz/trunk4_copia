Public Class Viajes
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Carga los viajes de la persona
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                Master.SetTitle = "Viajes y Hojas de Gastos"
                inicializar()
                If (Session("filtroViajes") IsNot Nothing) Then
                    Dim filtro = Session("filtroViajes")
                    chbUsarFechas.Checked = filtro.UsarFechas
                    txtFechaInicio.Text = filtro.FechaInicio : txtFechaFin.Text = filtro.FechaFin
                    txtFilter.Text = filtro.TextoFiltro
                End If
                cargarInformacion()
            Catch batzEx As BatzException
                Master.MensajeError = batzEx.Termino
            End Try
        End If
        Dim script As New StringBuilder
        script.AppendLine("$('#dtFechaIni').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
        script.AppendLine("$('#dtFechaFin').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
        AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(Me, Me.GetType, "DatePicker", script.ToString, True)
    End Sub

    ''' <summary>
    ''' Se le dice los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Viajes_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(lnkVerHGSinViaje) : itzultzaileWeb.Itzuli(chbUsarFechas)
            itzultzaileWeb.Itzuli(labelFIni) : itzultzaileWeb.Itzuli(labelFFin) : itzultzaileWeb.Itzuli(labelInfoVisa)
            itzultzaileWeb.Itzuli(hlAcceder) : itzultzaileWeb.Itzuli(btnBuscar) : itzultzaileWeb.Itzuli(btnVerCancelados)
            itzultzaileWeb.Itzuli(btnVerTodos) : itzultzaileWeb.Itzuli(labelTitle) : itzultzaileWeb.Itzuli(labelInfo2)
            itzultzaileWeb.Itzuli(labelInfo2) : itzultzaileWeb.Itzuli(lnkVerseguridad)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa la pagina
    ''' </summary>
    Private Sub inicializar()
        gvViajes.Attributes("CurrentSortField") = "IdViaje"
        gvViajes.Attributes("CurrentSortDirection") = SortDirection.Descending
        chbUsarFechas.Checked = True
        txtFechaInicio.Text = New Date(Now.Year, Now.Month, 1).AddMonths(-2).ToShortDateString
        Dim fFin As Date = Now.AddMonths(1)
        txtFechaFin.Text = New Date(fFin.Year, fFin.Month, Date.DaysInMonth(fFin.Year, fFin.Month)).ToShortDateString
        txtFilter.Text = String.Empty
        If (hasProfile(BLL.BidaiakBLL.Profiles.Planificador)) Then
            labelInfo1.Text = itzultzaileWeb.Itzuli("Se muestran todos los viajes planificados por usted o aquellos en los que esta como integrante")
        Else
            labelInfo1.Text = itzultzaileWeb.Itzuli("Se muestran todos los viajes en los que usted esta como integrante")
        End If
        txtFilter.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Viaje,Destino"))
        pnlGastosVisaLibres.Visible = False
        lnkVerseguridad.OnClientClick = "window.open('../Ayuda/Informacion_SOS.pdf','Seguridad SOS');return false;"
    End Sub

    ''' <summary>
    ''' Carga la informacion de la pagina
    ''' </summary>  
    Private Sub cargarInformacion()
        cargarViajes()
        If (Not Page.IsPostBack AndAlso CType(Session("ConVisa"), Boolean)) Then
            Dim visasBLL As New BLL.VisasBLL
            Dim fechaInicio As Date = Now.AddMonths(-2)
            fechaInicio = New DateTime(fechaInicio.Year, fechaInicio.Month, 1)
            Dim fechaFin As New Date(Now.Year, Now.Month, Date.DaysInMonth(Now.Year, Now.Month))
            Dim lVisas As List(Of ELL.Visa.Movimiento) = visasBLL.loadMovimientos(Master.Ticket.IdUser, Integer.MinValue, Master.IdPlantaGestion, fechaInicio, fechaFin, bUserYPupilos:=False, bSinJustificar:=True)
            pnlGastosVisaLibres.Visible = (lVisas IsNot Nothing AndAlso lVisas.Count > 0)
        End If
    End Sub

    ''' <summary>
    ''' Carga los viajes en los que el sea organizador y en los que sea integrante
    ''' </summary>
    Private Sub cargarViajes()
        Try
            Dim viajesBLL As New BLL.ViajesBLL
            Dim oViaje As New ELL.Viaje With {.IdUserSolicitador = Master.Ticket.IdUser, .IdPlanta = Master.IdPlantaGestion, .Estado = -1}
            Dim filtro = Session("filtroViajes")
            If (filtro IsNot Nothing AndAlso filtro.Cancelados) Then oViaje.Estado = ELL.Viaje.eEstadoViaje.Cancelado
            'Se comprueba si en el filtro, viene solo el idViaje, el idViaje con la v o sino, será el destino
            If (txtFilter.Text <> String.Empty) Then
                If (IsNumeric(txtFilter.Text)) Then
                    oViaje.IdViaje = CInt(txtFilter.Text)
                Else
                    If (txtFilter.Text.ToLower.StartsWith("v")) Then
                        Dim myIdViaje As String = txtFilter.Text.Substring(1)
                        If (IsNumeric(myIdViaje)) Then
                            oViaje.IdViaje = CInt(myIdViaje)
                        Else
                            oViaje.Destino = txtFilter.Text
                        End If
                    Else
                        oViaje.Destino = txtFilter.Text
                    End If
                End If
            End If
            If (chbUsarFechas.Checked) Then
                If (txtFechaInicio.Text <> String.Empty) Then oViaje.FechaIda = CDate(txtFechaInicio.Text)
                If (txtFechaFin.Text <> String.Empty) Then oViaje.FechaVuelta = CDate(txtFechaFin.Text)
            End If
            oViaje.ListaIntegrantes = New List(Of ELL.Viaje.Integrante)
            oViaje.ListaIntegrantes.Add(New ELL.Viaje.Integrante With {.Usuario = New SabLib.ELL.Usuario With {.Id = Master.Ticket.IdUser}})
            Dim lViajes As List(Of ELL.Viaje) = viajesBLL.loadListViajes(oViaje, False, Master.IdPlantaGestion, True, True)
            If (filtro IsNot Nothing AndAlso Not filtro.Cancelados) Then
                If (Not filtro.Todos) Then
                    lViajes = lViajes.FindAll(Function(o) o.Estado = ELL.Viaje.eEstadoViaje.Pendiente_validacion OrElse o.Estado = ELL.Viaje.eEstadoViaje.Validado OrElse o.Estado = ELL.Viaje.eEstadoViaje.No_validado)
                End If
            ElseIf (filtro Is Nothing) Then
                lViajes = lViajes.FindAll(Function(o) o.Estado = ELL.Viaje.eEstadoViaje.Pendiente_validacion OrElse o.Estado = ELL.Viaje.eEstadoViaje.Validado OrElse o.Estado = ELL.Viaje.eEstadoViaje.No_validado)
            End If
            If (lViajes IsNot Nothing AndAlso lViajes.Count > 0) Then Ordenar(lViajes)
            gvViajes.DataSource = lViajes
            gvViajes.DataBind()
            gvViajes.Columns(2).Visible = gvViajes.Columns(2).Visible And CType(Session("ConVisa"), Boolean)  'Sera visible si tiene visa y antes estaba preparada para visualizarse
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al cargar los viajes", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Redirige a la pagina de listado de hojas de gastos libres
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkVerHGSinViaje_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkVerHGSinViaje.Click
        Response.Redirect("HojasGastosSinViaje.aspx", False)
    End Sub

    ''' <summary>
    ''' Se buscan los viajes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            If (chbUsarFechas.Checked AndAlso txtFechaInicio.Text = String.Empty AndAlso txtFechaFin.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe informar alguna fecha")
            Else
                Dim filtro = New With {.UsarFechas = chbUsarFechas.Checked, .FechaInicio = txtFechaInicio.Text, .FechaFin = txtFechaFin.Text, .TextoFiltro = txtFilter.Text, .Cancelados = False, .Todos = False}
                Session("filtroViajes") = filtro
                cargarViajes()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra todos los viajes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVerTodos_Click(sender As Object, e As EventArgs) Handles btnVerTodos.Click
        Try
            chbUsarFechas.Checked = False : txtFilter.Text = String.Empty
            Dim filtro = New With {.UsarFechas = False, .FechaInicio = String.Empty, .FechaFin = String.Empty, .TextoFiltro = String.Empty, .Cancelados = False, .Todos = True}
            Session("filtroViajes") = filtro
            cargarViajes()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra solo los viajes cancelados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVerCancelados_Click(sender As Object, e As EventArgs) Handles btnVerCancelados.Click
        Try
            chbUsarFechas.Checked = False : txtFilter.Text = String.Empty
            Dim filtro = New With {.UsarFechas = False, .FechaInicio = String.Empty, .FechaFin = String.Empty, .TextoFiltro = String.Empty, .Cancelados = True, .Todos = False}
            Session("filtroViajes") = filtro
            cargarViajes()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvViajes_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvViajes.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oViaje As ELL.Viaje = e.Row.DataItem
            Dim lblViaje As Label = CType(e.Row.FindControl("lblViaje"), Label)
            Dim lblAgencia As Label = CType(e.Row.FindControl("lblAgencia"), Label)
            Dim lblAnticipos As Label = CType(e.Row.FindControl("lblAnticipos"), Label)
            Dim imgVer As ImageButton = CType(e.Row.FindControl("imgVerSolicitud"), ImageButton)
            Dim imgLineasGastos As ImageButton = CType(e.Row.FindControl("imgLineasGastos"), ImageButton)
            Dim imgGastosVisa As ImageButton = CType(e.Row.FindControl("imgGastosVisa"), ImageButton)
            Dim imgDocs As ImageButton = CType(e.Row.FindControl("imgDocs"), ImageButton)
            imgLineasGastos.Visible = (Date.Now >= oViaje.FechaIda)  'El boton de lineas de gastos y el de documentos del viaje solo sera visibles si ha comenzado el viaje            
            imgDocs.Visible = (Date.Now >= oViaje.FechaIda)
            imgGastosVisa.Visible = False
            Dim bShowDocs As Boolean = False
            Dim bEsIntegrante As Boolean = False
            Dim integrantes As New StringBuilder
            Dim activBLL As New BLL.ActividadesBLL
            oViaje.ListaIntegrantes = oViaje.ListaIntegrantes.OrderBy(Of String)(Function(o) o.Usuario.NombreCompleto).ToList
            For Each integr As ELL.Viaje.Integrante In oViaje.ListaIntegrantes
                If (integrantes.ToString <> String.Empty) Then integrantes.Append("<br />")
                If (oViaje.ResponsableLiquidacion IsNot Nothing AndAlso oViaje.ResponsableLiquidacion.Id = integr.Usuario.Id) Then
                    integrantes.Append("<b>" & integr.Usuario.NombreCompleto & "</b>")
                Else
                    integrantes.Append(integr.Usuario.NombreCompleto)
                End If
                If (Not bEsIntegrante) Then
                    bEsIntegrante = (integr.Usuario.Id = Master.Ticket.IdUser)
                    If (bEsIntegrante AndAlso oViaje.Nivel <> ELL.Viaje.eNivel.Nacional And Not bShowDocs) Then
                        If (activBLL.loadInfo(integr.IdActividad, False).ExentaIRPF) Then bShowDocs = True
                    End If
                End If
            Next
            CType(e.Row.FindControl("lblIdViaje"), Label).Text = "V" & oViaje.IdViaje
            imgVer.ToolTip = itzultzaileWeb.Itzuli("Ver solicitud")
            imgVer.CommandArgument = oViaje.IdViaje
            imgLineasGastos.CommandArgument = oViaje.IdViaje
            imgGastosVisa.CommandArgument = oViaje.IdViaje
            imgDocs.CommandArgument = oViaje.IdViaje
            CType(e.Row.FindControl("lblIntegrantes"), Label).Text = integrantes.ToString
            CType(e.Row.FindControl("lblFechaIda"), Label).Text = oViaje.FechaIda.ToShortDateString
            CType(e.Row.FindControl("lblFechaVuelta"), Label).Text = oViaje.FechaVuelta.ToShortDateString
            Dim sAgencia, sAnticipo, sViaje As String
            sAgencia = String.Empty : sAnticipo = String.Empty : sViaje = String.Empty
            sViaje = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Viaje.eEstadoViaje), oViaje.Estado).Replace("_", " "))
            lblViaje.Text = sViaje
            Select Case oViaje.Estado
                Case ELL.Viaje.eEstadoViaje.Pendiente_validacion
                    lblViaje.CssClass = "label label-warning"
                Case ELL.Viaje.eEstadoViaje.Cancelado, ELL.Viaje.eEstadoViaje.No_validado
                    lblViaje.CssClass = "label label-danger"
                Case ELL.Viaje.eEstadoViaje.Validado
                    lblViaje.CssClass = "label label-success"
            End Select
            If (oViaje.SolicitudAgencia IsNot Nothing) Then
                sAgencia = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.SolicitudAgencia.EstadoAgencia), oViaje.SolicitudAgencia.Estado))
                lblAgencia.Text = sAgencia
                Select Case oViaje.SolicitudAgencia.Estado
                    Case ELL.SolicitudAgencia.EstadoAgencia.solicitado
                        lblAgencia.CssClass = "label label-info"
                    Case ELL.SolicitudAgencia.EstadoAgencia.Tramite
                        lblAgencia.CssClass = "label label-primary"
                    Case ELL.SolicitudAgencia.EstadoAgencia.Gestionado
                        lblAgencia.CssClass = "label label-success"
                    Case ELL.SolicitudAgencia.EstadoAgencia.cancelada
                        lblAgencia.CssClass = "label label-danger"
                    Case ELL.SolicitudAgencia.EstadoAgencia.Cerrada
                        lblAgencia.CssClass = "label label-default"
                End Select
            End If
            If (oViaje.Anticipo IsNot Nothing) Then
                sAnticipo = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Anticipo.EstadoAnticipo), oViaje.Anticipo.Estado))
                lblAnticipos.Text = sAnticipo
                Select Case oViaje.Anticipo.Estado
                    Case ELL.Anticipo.EstadoAnticipo.solicitado
                        lblAnticipos.CssClass = "label label-info"
                    Case ELL.Anticipo.EstadoAnticipo.Preparado
                        lblAnticipos.CssClass = "label label-primary"
                    Case ELL.Anticipo.EstadoAnticipo.Entregado
                        lblAnticipos.CssClass = "label label-success"
                    Case ELL.Anticipo.EstadoAnticipo.cancelada
                        lblAnticipos.CssClass = "label label-danger"
                    Case ELL.Anticipo.EstadoAnticipo.cerrado
                        lblAnticipos.CssClass = "label label-default"
                End Select
            End If
            'Se comprueba el icono a poner a la columna de documentos
            If (bEsIntegrante) Then
                If (bShowDocs) Then
                    Dim viajesBLL As New BLL.ViajesBLL
                    Dim lDocs As List(Of ELL.Viaje.DocumentoIntegrante) = viajesBLL.loadDocumentosIntegrante(oViaje.IdViaje, Master.Ticket.IdUser)
                    If (lDocs IsNot Nothing AndAlso lDocs.Count > 0) Then
                        imgDocs.ImageUrl = "~/App_Themes/Tema1/Images/EstadosHojas/Documentos.png"
                        imgDocs.ToolTip = itzultzaileWeb.Itzuli("Ver documentos de exencion(tarjetas de embarque,facturas,...)")
                    Else
                        imgDocs.ImageUrl = "~/App_Themes/Tema1/Images/EstadosHojas/Upload.png"
                        imgDocs.ToolTip = itzultzaileWeb.Itzuli("Subir documentos para justificar la exencion(tarjetas de embarque,facturas,...)")
                    End If
                Else
                    imgDocs.OnClientClick = "return false;"
                    imgDocs.Style.Add("cursor", "default")
                    imgDocs.ImageUrl = "~/App_Themes/Tema1/Images/EstadosHojas/SinVisa.png"
                    imgDocs.ToolTip = itzultzaileWeb.Itzuli("No es necesario adjuntar documentos porque no se va a aplicar exencion")
                End If
            Else
                imgDocs.Visible = False   'Si no es integrante no vera el icono para que sepa que no le incumbe
            End If
            'Ver si tiene la persona gastos de visa en las fechas del viaje
            If (Session("ConVisa") IsNot Nothing AndAlso CType(Session("ConVisa"), Boolean) = True) Then
                Dim visasBLL As New BLL.VisasBLL
                Dim bFicheroVisasCargado As Boolean = visasBLL.FicheroVisasCargado(oViaje.FechaVuelta.Month, oViaje.FechaVuelta.Year, Master.IdPlantaGestion)
                If (bFicheroVisasCargado) Then 'Si se ha cargado el fichero de visas, se mira si tiene algun movimiento
                    Dim lMov As List(Of ELL.Visa.Movimiento) = visasBLL.loadMovimientos(Master.Ticket.IdUser, oViaje.IdViaje, Master.IdPlantaGestion, oViaje.FechaIda, oViaje.FechaVuelta, False)
                    If (lMov IsNot Nothing AndAlso lMov.Count > 0) Then  'Tiene movimientos
                        imgGastosVisa.ImageUrl = "~/App_Themes/Tema1/Images/EstadosHojas/ConVisa.png"
                        imgGastosVisa.ToolTip = itzultzaileWeb.Itzuli("Se han cargado automaticamente sus gastos de visa")
                    Else
                        imgGastosVisa.OnClientClick = "return false;"
                        imgGastosVisa.Style.Add("cursor", "default")
                        imgGastosVisa.ImageUrl = "~/App_Themes/Tema1/Images/EstadosHojas/SinVisa.png"
                        imgGastosVisa.ToolTip = itzultzaileWeb.Itzuli("No se han realizado pagos con visa")
                    End If
                Else 'Todavia no se ha cargado el fichero de visas
                    imgGastosVisa.OnClientClick = "return false;"
                    imgGastosVisa.Style.Add("cursor", "default")
                    imgGastosVisa.ImageUrl = "~/App_Themes/Tema1/Images/EstadosHojas/WaitingVisa.png"
                    imgGastosVisa.ToolTip = itzultzaileWeb.Itzuli("Esperando a la carga del extracto de visas")
                End If
            End If
            If (hasProfile(BLL.BidaiakBLL.Profiles.Consultor, BLL.BidaiakBLL.Profiles.Administrador, BLL.BidaiakBLL.Profiles.Financiero)) Then
                If (oViaje.HojasGastos IsNot Nothing AndAlso oViaje.HojasGastos.Count > 0) Then
                    Dim myHoja As ELL.HojaGastos = oViaje.HojasGastos.Find(Function(o As ELL.HojaGastos) o.Usuario.Id = Master.Ticket.IdUser)
                    If (myHoja IsNot Nothing) Then
                        Select Case myHoja.Estado
                            Case ELL.HojaGastos.eEstado.Enviada
                                imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Enviada.png"
                                imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Hoja de gastos enviada")
                            Case ELL.HojaGastos.eEstado.Validada
                                imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Aceptada.png"
                                imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Hoja de gastos validada")
                            Case ELL.HojaGastos.eEstado.Liquidada
                                imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Liquidada.png"
                                imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Hoja de gastos liquidada")
                            Case ELL.HojaGastos.eEstado.NoValidada
                                imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Rechazada.png"
                                imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Hoja de gastos rechazada")
                            Case ELL.HojaGastos.eEstado.Rellenada
                                imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/SinEnviar.png"
                                imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Continuar rellenando hoja de gastos")
                            Case ELL.HojaGastos.eEstado.Transferida
                                imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Transferida.png"
                                imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Hoja de gastos transferida a su empresa para que le realice el pago")
                        End Select
                    Else  'Todavia no la ha rellenado
                        imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Crear.png"
                        imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Crear hoja de gastos")
                    End If
                Else  'Todavia no la ha rellenado
                    imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Crear.png"
                    imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Crear hoja de gastos")
                End If
                imgGastosVisa.Visible = True  'Si tiene gastos de visa, se visualizara
            ElseIf (hasProfile(BLL.BidaiakBLL.Profiles.Planificador)) Then
                If (oViaje.HojasGastos IsNot Nothing AndAlso oViaje.HojasGastos.Count > 0) Then
                    If (oViaje.ListaIntegrantes.Count = 1 AndAlso oViaje.HojasGastos.Count = 1 AndAlso oViaje.HojasGastos.First.Usuario.Id = Master.Ticket.IdUser) Then 'Viaje con el planificador como integrante
                        Select Case oViaje.HojasGastos.First.Estado
                            Case ELL.HojaGastos.eEstado.Enviada
                                imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Enviada.png"
                                imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Hoja de gastos enviada")
                            Case ELL.HojaGastos.eEstado.Validada
                                imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Aceptada.png"
                                imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Hoja de gastos validada")
                            Case ELL.HojaGastos.eEstado.Liquidada
                                imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Liquidada.png"
                                imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Hoja de gastos liquidada")
                            Case ELL.HojaGastos.eEstado.NoValidada
                                imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Rechazada.png"
                                imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Hoja de gastos rechazada")
                            Case ELL.HojaGastos.eEstado.Rellenada
                                imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/SinEnviar.png"
                                imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Continuar rellenando hoja de gastos")
                        End Select
                        imgGastosVisa.Visible = True
                    Else  'Con mas asistentes
                        Dim numValidadas, numLiquidadas, numEnviadasNoValid As Integer
                        numValidadas = 0 : numLiquidadas = 0 : numEnviadasNoValid = 0
                        For Each oHoja As ELL.HojaGastos In oViaje.HojasGastos
                            If (oHoja.Usuario.Id = Master.Ticket.IdUser Or oHoja.Validador.Id = Master.Ticket.IdUser) Then  'Cuando sea el mismo usuario o sea el validador
                                'Puede que todos los integrantes del viaje, no tenga que validar el las hojas
                                Select Case oHoja.Estado
                                    Case ELL.HojaGastos.eEstado.Validada
                                        numValidadas += 1
                                    Case ELL.HojaGastos.eEstado.Liquidada, ELL.HojaGastos.eEstado.Transferida
                                        numLiquidadas += 1
                                    Case ELL.HojaGastos.eEstado.Enviada, ELL.HojaGastos.eEstado.NoValidada, ELL.HojaGastos.eEstado.Rellenada
                                        numEnviadasNoValid += 1
                                End Select
                            End If
                        Next
                        If (numValidadas = oViaje.HojasGastos.Count) Then 'Todas validadas
                            imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Aceptada.png"
                            imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Hojas de gastos validadas")
                        ElseIf (numLiquidadas = oViaje.HojasGastos.Count) Then 'Todas liquidadas
                            imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Liquidada.png"
                            imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Hojas de gastos liquidadas")
                        ElseIf (numValidadas > 0 Or numLiquidadas > 0 Or numEnviadasNoValid > 0) Then
                            imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/HojasSinValidar.png"
                            imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Ver hojas de gastos")
                        ElseIf (bEsIntegrante) Then   'Si es integrante, deberia tener el icono para poder meter su HG
                            imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Crear.png"
                            imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Crear hoja de gastos")
                        Else
                            imgLineasGastos.Visible = False  'De los pupilos suyos, ninguno ha enviado la hoja de gastos
                        End If
                    End If
                Else
                    'El icono de lineas de gastos solo sera visible si el organizador es tambien asistente de un viaje o el planificador (no es el planificador del viaje), es un integrante
                    'El boton de lineas de gastos solo sera visibles si ha comenzado el viaje                    
                    imgLineasGastos.Visible = (Date.Now >= oViaje.FechaIda) AndAlso oViaje.ListaIntegrantes.Exists(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = Master.Ticket.IdUser)
                    If (imgLineasGastos.Visible) Then
                        imgLineasGastos.ImageUrl = "../App_Themes/Tema1/Images/EstadosHojas/Crear.png"
                        imgLineasGastos.ToolTip = itzultzaileWeb.Itzuli("Crear hoja de gastos")
                    End If
                End If
                imgGastosVisa.Visible = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Se redirige al detalle de un viaje
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvViajes_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvViajes.RowCommand
        If (e.CommandName = "ver") Then
            Response.Redirect("SolicitudViaje.aspx?id=" & e.CommandArgument, False)
        ElseIf (e.CommandName = "linea") Then
            Response.Redirect("HojaGastos.aspx?idViaje=" & e.CommandArgument & "&orig=VIA", False)
        ElseIf (e.CommandName = "docs") Then
            Response.Redirect("DocsIntegrViaje.aspx?idViaje=" & e.CommandArgument, False)
        End If
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvViajes_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvViajes.PageIndexChanging
        Try
            gvViajes.PageIndex = e.NewPageIndex
            cargarViajes()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordenacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvViajes_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvViajes.Sorting
        Try
            gvViajes.Attributes("CurrentSortField") = e.SortExpression
            If (gvViajes.Attributes("CurrentSortDirection") Is Nothing) Then
                gvViajes.Attributes("CurrentSortDirection") = SortDirection.Ascending
            Else
                gvViajes.Attributes("CurrentSortDirection") = If(gvViajes.Attributes("CurrentSortDirection") = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            End If
            cargarViajes()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordena la lista
    ''' </summary>
    ''' <param name="lista">Lista</param>
    Public Sub Ordenar(ByRef lista As List(Of ELL.Viaje))
        Select Case gvViajes.Attributes("CurrentSortField")
            Case "IdViaje"
                If (gvViajes.Attributes("CurrentSortDirection") = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Integer)(Function(o) o.IdViaje).ToList
                Else
                    lista = lista.OrderByDescending(Of Integer)(Function(o) o.IdViaje).ToList
                End If
            Case "FechaIda"
                If (gvViajes.Attributes("CurrentSortDirection") = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Date)(Function(o) o.FechaIda).ToList
                Else
                    lista = lista.OrderByDescending(Of Date)(Function(o) o.FechaIda).ToList
                End If
            Case "FechaVuelta"
                If (gvViajes.Attributes("CurrentSortDirection") = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Date)(Function(o) o.FechaVuelta).ToList
                Else
                    lista = lista.OrderByDescending(Of Date)(Function(o) o.FechaVuelta).ToList
                End If
            Case "Destino"
                If (gvViajes.Attributes("CurrentSortDirection") = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.Destino).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.Destino).ToList
                End If
        End Select
    End Sub

    '''' <summary>
    '''' Se descarga el documento de serguridad
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    'Private Sub lnkVerseguridad_Click(sender As Object, e As EventArgs) Handles lnkVerseguridad.Click
    '    'Si el Explorador reconoce el archivo lo abrira dentro de él, sino pedira al usuario la accion a realizar.
    '    Dim objStreamReader As IO.StreamReader = IO.File.OpenText(Server.MapPath("~/Ayuda/Informacion_ISOS.pdf"))
    '    Dim fs As IO.FileStream = IO.File.Open(Server.MapPath("~/Ayuda/Informacion_ISOS.pdf"), IO.FileMode.Open, IO.FileAccess.Read)
    '    Dim buff(fs.Length) As Byte
    '    If fs.Length > 0 Then
    '        fs.Read(buff, 0, fs.Length - 1)
    '        fs.Close()
    '        buff = Nothing : fs = Nothing
    '    End If
    '    Response.Clear() : Response.ClearHeaders() : Response.ClearContent()
    '    Response.Buffer = True
    '    Response.AppendHeader("Content-Disposition", "inline; filename=Informacion_ISOS")
    '    Response.ContentType = ContentType
    '    Response.BinaryWrite(buff)
    '    Response.End()
    'End Sub

#End Region

End Class