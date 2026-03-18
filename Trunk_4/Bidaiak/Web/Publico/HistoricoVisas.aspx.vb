Public Class HistoricoVisas
    Inherits PageBase

    Private hashUsuarios As New Hashtable

#Region "Properties"

    ''' <summary>
    ''' Indica si esta en modo edicion para actualizar los comentarios de los gastos de visa
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property Edicion As Boolean
        Get
            Return CType(ViewState("Edicion"), Boolean)
        End Get
        Set(value As Boolean)
            ViewState("Edicion") = value
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para saber que movimientos se tienen que visualizar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property VerSinJustificar As Integer
        Get
            If (hfOpcionSel.Value = String.Empty) Then
                Return 0
            Else 'No se porque (debe ser por el update panel), hace un append de los valores
                If (hfOpcionSel.Value.IndexOf(",") = -1) Then
                    Return CInt(hfOpcionSel.Value)
                Else
                    Return CInt(hfOpcionSel.Value.Split(",")(0))
                End If
            End If
        End Get
        Set(ByVal value As Integer)
            hfOpcionSel.Value = value
        End Set
    End Property

#End Region

#Region "Eventos de pagina"

    ''' <summary>
    ''' Carga la pagina con las hojas de gastos entre las fechas del mes actual
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If (Master.ConVisa) Then
                    Master.SetTitle = "Historico visas"
                    Edicion = False 'La primera vez que entre, estara en modo consulta
                    Dim fMesAnterior As Date = Now.AddMonths(-1)
                    Dim mes As Integer = If(Request.QueryString("mes") IsNot Nothing AndAlso Request.QueryString("mes") <> String.Empty, CInt(Request.QueryString("mes")), fMesAnterior.Month)
                    Dim año As Integer = If(Request.QueryString("ano") IsNot Nothing AndAlso Request.QueryString("ano") <> String.Empty, CInt(Request.QueryString("ano")), fMesAnterior.Year)
                    VerSinJustificar = 1
                    rbtPorFechas.Checked = False : rbtVerTodos.Checked = False : rbtSinJustificar.Checked = True
                    rbtPorFechas.Attributes.Add("onclick", "ChangeRadio('0');")
                    rbtSinJustificar.Attributes.Add("onclick", "ChangeRadio('1');")
                    rbtVerTodos.Attributes.Add("onclick", "ChangeRadio('2');")
                    CargarMesesYAños(mes, año)
                    gvVisa.Attributes("CurrentSortField") = "Fecha"
                    gvVisa.Attributes("CurrentSortDirection") = SortDirection.Ascending
                    BuscarGastosVisa()
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "init", "$('textarea').autogrow();", True)
                Else
                    Response.Redirect("~/PermisoDenegado.aspx?mensa=3", False)
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelTitle) : itzultzaileWeb.Itzuli(rbtPorFechas) : itzultzaileWeb.Itzuli(rbtSinJustificar)
            itzultzaileWeb.Itzuli(btnSearchF) : itzultzaileWeb.Itzuli(btnGuardar1) : itzultzaileWeb.Itzuli(rbtVerTodos)
            itzultzaileWeb.Itzuli(btnGuardar2) : itzultzaileWeb.Itzuli(btnCancelar1) : itzultzaileWeb.Itzuli(btnCancelar2)
            itzultzaileWeb.Itzuli(labelTitleModal) : itzultzaileWeb.Itzuli(btnJustificarModal) : itzultzaileWeb.Itzuli(labelCancelarModal)
            itzultzaileWeb.Itzuli(labelTarjetaM) : itzultzaileWeb.Itzuli(labelFechaM) : itzultzaileWeb.Itzuli(labelViajeHojaM)
            itzultzaileWeb.Itzuli(labelLocalidad) : itzultzaileWeb.Itzuli(labelSectorM) : itzultzaileWeb.Itzuli(labelEstablecimientoM)
            itzultzaileWeb.Itzuli(labelImporteEurM) : itzultzaileWeb.Itzuli(labelEstadoM) : itzultzaileWeb.Itzuli(labelObservacionesM)
            itzultzaileWeb.Itzuli(btnAvisarJustif) : itzultzaileWeb.Itzuli(labelImporteMonGastoM)
        End If
    End Sub

    ''' <summary>
    ''' <para>Dado un mes y un año, carga los desplegable solo con los valores que podria elegir</para>
    ''' <para>Para el año actual, solo se podra elegir hasta el mes actual</para>
    ''' </summary>
    ''' <param name="month">Mes</param>
    ''' <param name="year">Año</param>
    Private Sub CargarMesesYAños(ByVal month As Integer, ByVal year As Integer)
        Try
            Dim i As Integer
            Dim año As Integer = CInt(Now.Date.Year)
            Dim mesesPintar As Integer = month
            ddlMes.Items.Clear()
            ddlAño.Items.Clear()
            mesesPintar = If(Now.Date.Year <> year, 12, Now.Date.Month)
            For i = 1 To mesesPintar
                ddlMes.Items.Add(New ListItem(MonthName(i).ToUpper, i))
            Next i
            'Cuando se cambio del año anterior al año actual, puede que el mes marcado no se tenga que visualizar en el año actual, asi que se mostrar el maximo que se puede mostrar
            ddlMes.SelectedIndex = If(month > ddlMes.Items.Count, ddlMes.Items.Count - 1, month - 1)
            'Años
            ddlAño.Items.Add(New ListItem(año - 1, año - 1))
            ddlAño.Items.Add(New ListItem(año, año))
            ddlAño.SelectedIndex = ddlAño.Items.IndexOf(ddlAño.Items.FindByValue(year))
        Catch ex As Exception
            Throw New BatzException("errIKScargandoFechas", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se cargan los meses que le correspondan al año
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlAño_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAño.SelectedIndexChanged
        Try
            CargarMesesYAños(1, ddlAño.SelectedValue)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Buscar gastos de visa"

    ''' <summary>
    ''' Evento del boton buscar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSearchF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchF.Click
        Try
            VerSinJustificar = If(rbtSinJustificar.Checked, 1, 0)
            BuscarGastosVisa()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Busca los gastos de visa de la gente al cargo del validador en la fecha seleccionada
    ''' </summary>    
    Private Sub BuscarGastosVisa()
        Dim visaBLL As New BLL.VisasBLL
        Dim fechaInicio, fechaFin As Date
        pnlJust1.Visible = False : pnlJust2.Visible = False : pnlJustificar.Visible = Not Edicion
        Dim lVisas As List(Of ELL.Visa.Movimiento) = Nothing
        If (VerSinJustificar = 1) Then
            lVisas = visaBLL.loadMovimientos(Master.Ticket.IdUser, Integer.MinValue, Master.IdPlantaGestion, Date.MinValue, Date.MinValue, False, False, True)
        ElseIf (rbtVerTodos.Checked) Then
            fechaInicio = Now.AddDays(-(30 * 3))
            fechaFin = Now
            lVisas = visaBLL.loadMovimientos(Master.Ticket.IdUser, Integer.MinValue, Master.IdPlantaGestion, fechaInicio, fechaFin, False, False, False)
        Else
            fechaInicio = New Date(ddlAño.SelectedValue, ddlMes.SelectedValue, 1)
            fechaFin = New Date(ddlAño.SelectedValue, ddlMes.SelectedValue, Date.DaysInMonth(ddlAño.SelectedValue, ddlMes.SelectedValue))
            lVisas = visaBLL.loadMovimientos(Master.Ticket.IdUser, Integer.MinValue, Master.IdPlantaGestion, fechaInicio, fechaFin, False)
        End If
        If (lVisas IsNot Nothing AndAlso lVisas.Count > 0) Then
            Ordenar(lVisas)
            pnlJust1.Visible = Edicion : pnlJust2.Visible = Edicion
        End If
        gvVisa.DataSource = lVisas
        gvVisa.DataBind()
        gvVisa.Columns(5).Visible = Edicion
        If (Not Edicion) Then pnlJustificar.Visible = (lVisas.Count > 0)
        pnlAvisarJustif.Visible = (lVisas.Count > 0)
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Enlaza los controles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvVisa_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvVisa.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oVisa As ELL.Visa.Movimiento = e.Row.DataItem
            Dim lblTipo As Label = CType(e.Row.FindControl("lblTipo"), Label)
            Dim imgEstado As Image = CType(e.Row.FindControl("imgEstado"), Image)
            If (oVisa.IdViaje <> Integer.MinValue) Then
                Dim viajeBLL As BLL.ViajesBLL = New BLL.ViajesBLL
                Dim oViaje As ELL.Viaje = viajeBLL.loadInfo(oVisa.IdViaje, False, False, True)
                lblTipo.Text = "V" & oViaje.IdViaje
                lblTipo.CssClass = "label label-info"
            ElseIf (oVisa.IdHoja <> Integer.MinValue) Then
                Dim hojaBLL As BLL.HojasGastosBLL = New BLL.HojasGastosBLL
                Dim oHoja As ELL.HojaGastos = hojaBLL.loadHoja(oVisa.IdHoja, False)
                If (oHoja IsNot Nothing) Then
                    lblTipo.Text = "H" & oHoja.IdSinViaje
                    lblTipo.CssClass = "label label-primary"
                End If
            Else
                lblTipo.Text = itzultzaileWeb.Itzuli("Libre")
                lblTipo.CssClass = "label label-default"
            End If
            CType(e.Row.FindControl("lblFecha"), Label).Text = oVisa.Fecha.ToShortDateString
            CType(e.Row.FindControl("txtJustificacion"), TextBox).Text = oVisa.Comentarios
            CType(e.Row.FindControl("lblImporteEur"), Label).Text = oVisa.ImporteEuros & " " & oVisa.Moneda.Abreviatura
            CType(e.Row.FindControl("lblImporteMonGasto"), Label).Text = oVisa.ImporteMonedaGasto & " " & oVisa.MonedaGasto.Abreviatura
            Select Case oVisa.Estado
                Case ELL.Visa.Movimiento.eEstado.Cargado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Sin_Validar.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Cargado")
                    imgEstado.Visible = False : CType(e.Row.FindControl("labelSinJustificar"), Label).Visible = True
                Case ELL.Visa.Movimiento.eEstado.Conforme
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Validado.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Conforme")
                Case ELL.Visa.Movimiento.eEstado.No_Conforme
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Rechazada.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("No conforme")
                Case ELL.Visa.Movimiento.eEstado.Justificado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Justificado.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Justicado")
                Case ELL.Visa.Movimiento.eEstado.Liquidado
                    imgEstado.ImageUrl = "~/App_Themes/Tema1/Images/EstadosValidacion/Integrado.png"
                    imgEstado.ToolTip = itzultzaileWeb.Itzuli("Integrado")
            End Select
            e.Row.ToolTip = itzultzaileWeb.Itzuli("Gastos de") & " " & oVisa.NombreUsuario.Trim
            If (Not Edicion) Then
                e.Row.ToolTip &= vbCrLf & If(oVisa.Comentarios <> String.Empty, oVisa.Comentarios, "(" & itzultzaileWeb.Itzuli("Sin comentarios") & ")")
            End If
            CType(e.Row.FindControl("lnkJustif"), LinkButton).CommandArgument = oVisa.Id
        End If
    End Sub

    ''' <summary>
    ''' Ordena los elementos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvVisa_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvVisa.Sorting
        Try
            gvVisa.Attributes("CurrentSortField") = e.SortExpression
            If (gvVisa.Attributes("CurrentSortDirection") Is Nothing) Then
                gvVisa.Attributes("CurrentSortDirection") = SortDirection.Ascending
            Else
                gvVisa.Attributes("CurrentSortDirection") = If(gvVisa.Attributes("CurrentSortDirection") = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            End If
            BuscarGastosVisa()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="lista"></param>
    Public Sub Ordenar(ByRef lista As List(Of ELL.Visa.Movimiento))
        Select Case gvVisa.Attributes("CurrentSortField")
            Case "Fecha"
                If (gvVisa.Attributes("CurrentSortDirection") = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Date)(Function(o) o.Fecha).ToList
                Else
                    lista = lista.OrderByDescending(Of Date)(Function(o) o.Fecha).ToList
                End If
            Case "NumTarjeta"
                If (gvVisa.Attributes("CurrentSortDirection") = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.NumTarjeta).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.NumTarjeta).ToList
                End If
            Case "Localidad"
                If (gvVisa.Attributes("CurrentSortDirection") = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.Localidad).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.Localidad).ToList
                End If
            Case "Sector"
                If (gvVisa.Attributes("CurrentSortDirection") = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.Sector).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.Sector).ToList
                End If
            Case "Establecimiento"
                If (gvVisa.Attributes("CurrentSortDirection") = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.Establecimiento).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.Establecimiento).ToList
                End If
        End Select
    End Sub

    ''' <summary>
    ''' Muestra el panel modal para justificar un gasto
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModal(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True)
        End If
    End Sub

    ''' <summary>
    ''' Se prepara el formulario para justificar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub lnkJustif_Click(sender As Object, e As EventArgs)
        Try
            Dim idMov As Integer = CInt(CType(sender, LinkButton).CommandArgument)
            Dim visasBLL As New BLL.VisasBLL
            Dim oMov As ELL.Visa.Movimiento = visasBLL.loadMovimiento(idMov)
            With oMov
                If (.IdViaje <> Integer.MinValue) Then
                    Dim viajeBLL As BLL.ViajesBLL = New BLL.ViajesBLL
                    Dim oViaje As ELL.Viaje = viajeBLL.loadInfo(.IdViaje, False, False, True)
                    lblViajeHojaM.Text = "V" & oViaje.IdViaje
                    lblViajeHojaM.CssClass = "label label-info"
                ElseIf (.IdHoja <> Integer.MinValue) Then
                    Dim hojaBLL As BLL.HojasGastosBLL = New BLL.HojasGastosBLL
                    Dim oHoja As ELL.HojaGastos = hojaBLL.loadHoja(.IdHoja, False)
                    lblViajeHojaM.Text = "H" & oHoja.IdSinViaje
                    lblViajeHojaM.CssClass = "label label-primary"
                Else
                    lblViajeHojaM.Text = itzultzaileWeb.Itzuli("Libre")
                    lblViajeHojaM.CssClass = "label label-default"
                End If
                Select Case .Estado
                    Case ELL.Visa.Movimiento.eEstado.Cargado
                        lblEstadoM.Text = itzultzaileWeb.Itzuli("Sin justificar")
                        lblEstadoM.CssClass = "label label-warning"
                    Case ELL.Visa.Movimiento.eEstado.Conforme
                        lblEstadoM.Text = itzultzaileWeb.Itzuli("Conforme")
                        lblEstadoM.CssClass = "label label-success"
                    Case ELL.Visa.Movimiento.eEstado.No_Conforme
                        lblEstadoM.Text = itzultzaileWeb.Itzuli("No conforme")
                        lblEstadoM.CssClass = "label label-danger"
                    Case ELL.Visa.Movimiento.eEstado.Justificado
                        lblEstadoM.Text = itzultzaileWeb.Itzuli("Justicado")
                        lblEstadoM.CssClass = "label label-info"
                    Case ELL.Visa.Movimiento.eEstado.Liquidado
                        lblEstadoM.Text = itzultzaileWeb.Itzuli("Integrado")
                        lblEstadoM.CssClass = "label label-primary"
                End Select
                lblTarjetaM.Text = .NumTarjeta
                lblFechaM.Text = .Fecha.ToShortDateString
                lblLocalidadM.Text = .Localidad
                lblSectorM.Text = .Sector
                lblEstablecimientoM.Text = .Establecimiento
                lblImporteEurM.Text = .ImporteEuros & " " & .Moneda.Abreviatura
                lblImporteMonGastoM.Text = .ImporteMonedaGasto & " " & .MonedaGasto.Abreviatura
                txtObservacionesM.Text = .Comentarios
                btnJustificarModal.CommandArgument = oMov.Id
            End With
            ShowModal(True)
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar")
            ShowModal(True)
        End Try
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Guarda los comentarios de justificacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar1.Click, btnGuardar2.Click
        Try
            Dim visasBLL As New BLL.VisasBLL
            Dim lMovs As New List(Of ELL.Visa.Movimiento)
            Dim sIdLineas As String = String.Empty
            Dim bExcesoCaracteres As Boolean = False
            Dim oldState, newState As ELL.Visa.Movimiento.eEstado
            Dim comentarios As String
            Dim idLinea As Integer
            For Each row As GridViewRow In gvVisa.Rows
                idLinea = CInt(gvVisa.DataKeys(row.RowIndex)(0))
                oldState = CInt(gvVisa.DataKeys(row.RowIndex)(1))
                newState = oldState
                comentarios = CType(row.FindControl("txtJustificacion"), TextBox).Text.Trim
                If (Not bExcesoCaracteres) Then bExcesoCaracteres = (comentarios.Length > 300)
                'Si el estado actual es cargado, se le pone justificado, sino el que tenia antes
                If (oldState = ELL.Visa.Movimiento.eEstado.Cargado) Then newState = ELL.Visa.Movimiento.eEstado.Justificado
                lMovs.Add(New ELL.Visa.Movimiento With {.Id = idLinea, .Estado = newState, .Comentarios = comentarios})
                sIdLineas &= If(sIdLineas = String.Empty, "", ",") & idLinea
            Next
            If (Not bExcesoCaracteres) Then
                visasBLL.CambiarEstadoMovimientos(lMovs)
                log.Info("Se ha modificado/justificado los comentarios de los gastos de visa (" & sIdLineas & ")")
                Try
                    Edicion = False
                    BuscarGastosVisa()
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
                Catch batzEx As BatzException
                    log.Error("Se han guardado los comentarios pero ha dado error al listar los gastos de visa", batzEx.Excepcion)
                    Master.MensajeError = itzultzaileWeb.Itzuli("Se han guardado los comentarios pero ha dado un error a listar")
                End Try
                'Se avisa al validador de que ha validado gastos de visa
                If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
                    Try
                        Dim paramBLL As New SabLib.BLL.ParametrosBLL
                        Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                        Dim bidaiakBLL As New BLL.BidaiakBLL
                        Dim userBLL As New SabLib.BLL.UsuariosComponent
                        Dim idResp As Integer = CInt(Session("IdResponsable"))
                        Dim oResp As New SabLib.ELL.Usuario With {.Id = idResp}
                        oResp = userBLL.GetUsuario(oResp)
                        Dim subject As String = "Justificación de gastos de visa"
                        Dim sPerfil As String() = bidaiakBLL.loadProfile(Master.IdPlantaGestion, idResp, CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon")))
                        Dim linkUrl As String = If(sPerfil(1) = "0", "index.aspx", "index_Directo.aspx")
                        linkUrl &= "?gastosVisa=1&IdUser=" & Master.Ticket.IdUser & "&Mes=" & ddlMes.SelectedValue & "&ano=" & ddlAño.SelectedValue
                        Dim body As String = PageBase.getBodyHmtl("Movimientos de visa", String.Empty, Master.Ticket.NombreCompleto & " ha justificado movimientos de visa para ser validados. Acceda a bidaiak para consultarlos", linkUrl, (sPerfil(1) = "0"))
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), oResp.Email, subject, body, serverEmail)
                        log.Info("MOV_VISA:Se ha enviado un email al responsable " & oResp.NombreCompleto & " para avisarle de la justificacion de movimientos de visa del usuario " & Master.Ticket.NombreCompleto)
                    Catch ex As Exception
                        log.Error("MOV_VISA: No se ha podido avisar al responsable de la justificacion de las visas de " & Master.Ticket.NombreCompleto, ex)
                    End Try
                End If
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("300 caracteres maximo")
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al guardar los comentarios de justificacion", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
        End Try
    End Sub

    ''' <summary>
    ''' Pone en modo edicion los gastos de visa
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnJustificar_Click(sender As Object, e As EventArgs) Handles btnJustificar.Click
        Try
            Edicion = True
            BuscarGastosVisa()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se cancela la edicion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar1.Click, btnCancelar2.Click
        Try
            Edicion = False
            BuscarGastosVisa()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Justifica un movimiento en el movil
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnJustificarModal_Click(sender As Object, e As EventArgs) Handles btnJustificarModal.Click
        Try
            Dim idMov As Integer = CInt(btnJustificarModal.CommandArgument)
            Dim visasBLL As New BLL.VisasBLL
            Dim oMov As ELL.Visa.Movimiento = visasBLL.loadMovimiento(idMov)
            Dim bExcesoCaracteres As Boolean = (txtObservacionesM.Text.Length > 300)
            'Si el estado actual es cargado, se le pone justificado, sino el que tenia antes
            If (oMov.Estado = ELL.Visa.Movimiento.eEstado.Cargado) Then oMov.Estado = ELL.Visa.Movimiento.eEstado.Justificado
            If (Not bExcesoCaracteres) Then
                oMov.Comentarios = txtObservacionesM.Text
                Dim lMovs As New List(Of ELL.Visa.Movimiento)
                lMovs.Add(oMov)
                visasBLL.CambiarEstadoMovimientos(lMovs)
                log.Info("Se ha modificado/justificado los comentarios del gasto de visa (" & idMov & ")")
                Try
                    Edicion = False
                    BuscarGastosVisa()
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos guardados")
                    ShowModal(False)
                Catch batzEx As BatzException
                    log.Error("Se ha guardado el comentario pero ha dado error al listar los gastos de visa", batzEx.Excepcion)
                    Master.MensajeError = itzultzaileWeb.Itzuli("Se ha guardado el comentario pero ha dado un error a listar")
                    ShowModal(True)
                End Try
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("300 caracteres maximo")
                ShowModal(True)
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
            ShowModal(True)
        Catch ex As Exception
            log.Error("Error al guardar los comentarios de justificacion", ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
            ShowModal(True)
        End Try
    End Sub

    ''' <summary>
    ''' Se avisa al responsable de que se han justificado movimientos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAvisarJustif_Click(sender As Object, e As EventArgs) Handles btnAvisarJustif.Click
        If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
            Try
                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                Dim bidaiakBLL As New BLL.BidaiakBLL
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim idResp As Integer = CInt(Session("IdResponsable"))
                Dim oResp As New SabLib.ELL.Usuario With {.Id = idResp}
                oResp = userBLL.GetUsuario(oResp)
                Dim subject As String = "Justificación de gastos de visa"
                Dim sPerfil As String() = bidaiakBLL.loadProfile(Master.IdPlantaGestion, idResp, CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon")))
                Dim linkUrl As String = If(sPerfil(1) = "0", "index.aspx", "index_Directo.aspx")
                linkUrl &= "?gastosVisa=1&IdUser=" & Master.Ticket.IdUser & "&Mes=" & ddlMes.SelectedValue & "&ano=" & ddlAño.SelectedValue
                Dim body As String = PageBase.getBodyHmtl("Movimientos de visa", String.Empty, Master.Ticket.NombreCompleto & " ha justificado movimientos de visa para ser validados. Acceda a bidaiak para consultarlos", linkUrl, (sPerfil(1) = "0"))
                SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), oResp.Email, subject, body, serverEmail)
                log.Info("MOV_VISA:Se ha enviado un email al responsable " & oResp.NombreCompleto & " para avisarle de la justificacion de movimientos de visa del usuario " & Master.Ticket.NombreCompleto & " a traves del movil")
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Responsable avisado")
            Catch ex As Exception
                log.Error("MOV_VISA: No se ha podido avisar al responsable de la justificacion de las visas de " & Master.Ticket.NombreCompleto & " a traves del movil", ex)
                Master.MensajeError = itzultzaileWeb.Itzuli("No se ha podido avisar al responsable")
            End Try
        End If
    End Sub

#End Region

#Region "Temporizador"

    ''' <summary>
    ''' Se inicializa el temporizador con 3 minutos antes para que no caduque
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub temporizador_Init(ByVal sender As Object, ByVal e As EventArgs) Handles temporizador.Init
        temporizador.Interval = ((Session.Timeout - 2) * 60) * 1000
    End Sub

    ''' <summary>
    ''' Tick del timer
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub temporizador_Tick(sender As Object, e As EventArgs) Handles temporizador.Tick
        log.Info("Autorrefresco del historico de visas para que no caduque la pagina")
    End Sub

#End Region

End Class