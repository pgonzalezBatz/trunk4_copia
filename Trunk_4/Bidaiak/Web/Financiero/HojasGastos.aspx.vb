Public Class HojasGastos
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Carga la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Hojas de gastos"
            Try
                inicializarFilter()
                If (Request.QueryString("return") Is Nothing) Then
                    Session("filterHG") = Nothing
                Else ''Cuando vuelva de la hg, no se borrara el filtro
                    Buscar()
                End If
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
    ''' Inicializa el filtro
    ''' </summary>
    Private Sub inicializarFilter()
        chbUsarFechas.Checked = False
        txtFechaInicio.Text = New Date(Now.Year, Now.Month, 1).AddMonths(-1).ToShortDateString
        txtFechaFin.Text = New Date(Now.Year, Now.Month, Now.Day).ToShortDateString
        'txtIdViajeHojaF.Focus()
        txtIdViajeHojaF.Text = String.Empty
        searchUserF.Limpiar()
        txtIdViajeHojaF.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Nº de la V o H de la hoja"))
        searchUserF.PlaceHolder = itzultzaileWeb.Itzuli("Nombre, apellidos, nombre de usuario o numero de trabajador")
        gvHojasGastos.DataSource = Nothing : gvHojasGastos.DataBind()
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub HojaGastos_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelTitle) : itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(labelFIni)
            itzultzaileWeb.Itzuli(labelFFin) : itzultzaileWeb.Itzuli(btnSearchF) : itzultzaileWeb.Itzuli(btnResetF)
            itzultzaileWeb.Itzuli(chbUsarFechas)
        End If
    End Sub

#End Region

#Region "Buscar"

    ''' <summary>
    ''' Busca las hojas de gastos entra las fechas seleccionadas
    ''' </summary>
    Private Sub Buscar()
        Try
            Dim hojasGastosBLL As New BLL.HojasGastosBLL
            Dim oHoja As New ELL.HojaGastos
            Dim fechaIni, fechaFin As Date
            Dim sFiltro(5) As String
            Dim idPerso As Integer
            fechaIni = Date.MinValue : fechaFin = Date.MinValue
            'Si tiene filtro y es la primera vez, se cargan los valores
            If (Not Page.IsPostBack AndAlso Session("filterHG") IsNot Nothing) Then
                Dim filter As String() = CType(Session("filterHG"), String())
                chbUsarFechas.Checked = (filter(0) = "1")
                txtFechaInicio.Text = filter(1)
                txtFechaFin.Text = filter(2)
                txtIdViajeHojaF.Text = filter(3)
                searchUserF.SelectedId = filter(4)
                searchUserF.Texto = filter(5)
            End If
            If (chbUsarFechas.Checked) Then
                fechaIni = CType(txtFechaInicio.Text, Date)
                fechaFin = CType(txtFechaFin.Text, Date)
                If (fechaIni > fechaFin) Then
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La fecha de inicio tiene que ser menor que la fecha de fin")
                    Exit Sub
                End If
            End If
            oHoja.FechaDesde = fechaIni : oHoja.FechaHasta = fechaFin
            oHoja.IdViaje = If(txtIdViajeHojaF.Text <> String.Empty, CInt(txtIdViajeHojaF.Text), Integer.MinValue)
            idPerso = If(searchUserF.SelectedId <> String.Empty, CInt(searchUserF.SelectedId), Integer.MinValue)
            If (idPerso <> Integer.MinValue) Then
                oHoja.Usuario = New SabLib.ELL.Usuario
                oHoja.Usuario.Id = idPerso
            End If
            'Se actualiza el filtro
            sFiltro(0) = If(chbUsarFechas.Checked, "1", "0")
            sFiltro(1) = txtFechaInicio.Text
            sFiltro(2) = txtFechaFin.Text
            sFiltro(3) = If(txtIdViajeHojaF.Text <> String.Empty, txtIdViajeHojaF.Text, "")
            sFiltro(4) = If(searchUserF.SelectedId <> String.Empty, searchUserF.SelectedId, "")
            sFiltro(5) = If(searchUserF.SelectedId <> String.Empty, searchUserF.Texto, "")
            Session("filterHG") = sFiltro
            Dim lsHojas As List(Of String()) = hojasGastosBLL.loadHGFinanciero(oHoja, Master.IdPlantaGestion)
            If (lsHojas IsNot Nothing) Then Ordenar(lsHojas)
            gvHojasGastos.DataSource = lsHojas
            gvHojasGastos.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("errBuscar", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Al presionar el buton se redirige a un metodo de busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Private Sub btnSearchF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchF.Click
        Try
            Buscar()
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
        inicializarFilter()
    End Sub

#End Region

#Region "Eventos Gridview"

    ''' <summary>
    ''' Evento que surge al realizar un evento en la fila
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvHojasGastos_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvHojasGastos.RowCommand
        If (e.CommandName = "VH") Then
            Response.Redirect("../Viaje/HojaGastos.aspx?id=" & e.CommandArgument & "&orig=HGS")
        ElseIf (e.CommandName = "VA") Then
            Response.Redirect("Anticipos/GestionAnticipos.aspx?idViaje=" & e.CommandArgument & "&orig=HGS")
        End If
    End Sub

    ''' <summary>
    ''' Evento que surge al enlazar los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>  
    ''' <remarks>HG.ID,HGE.FECHA_ENVIO,HG.ID_VIAJE,HG.ID_SIN_VIAJES,V.DESCRIPCION,V.ID_RESP_LIQUIDACION,ANTICIPO,HG.ID_USER,U.NOMBRE_COMPLETO </remarks>  
    Private Sub gvHojasGastos_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvHojasGastos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim shoja As String() = e.Row.DataItem
            Dim imgHGSinEnviar As Image = CType(e.Row.FindControl("imgHGSinEnviar"), Image)
            Dim lblIdViaje As Label = CType(e.Row.FindControl("lblIdViaje"), Label)
            Dim chbAnticipo As CheckBox = CType(e.Row.FindControl("chbAnticipo"), CheckBox)
            Dim lnkVerHoja As LinkButton = CType(e.Row.FindControl("lnkVerHoja"), LinkButton)
            Dim lnkVerAnticipo As LinkButton = CType(e.Row.FindControl("lnkVerAnticipo"), LinkButton)
            Dim pnlSinEntregar As Panel = CType(e.Row.FindControl("pnlSinEntregar"), Panel)
            Dim pnlEntregado As Panel = CType(e.Row.FindControl("pnlEntregado"), Panel)
            Dim pnlFechaEnt As Panel = CType(e.Row.FindControl("pnlFechaEnt"), Panel)
            CType(e.Row.FindControl("lblFechaEnvio"), Label).Text = If(CDate(shoja(1)) = Date.MinValue, String.Empty, CDate(shoja(1)).ToShortDateString)
            imgHGSinEnviar.Visible = (shoja(11) = "0") 'Sin validar
            If (imgHGSinEnviar.Visible) Then
                Select Case shoja(10)
                    Case ELL.HojaGastos.eEstado.Rellenada
                        imgHGSinEnviar.ToolTip = "Hoja de gastos creada sin enviar al responsable"
                        imgHGSinEnviar.ImageUrl = "..\App_Themes\Tema1\Images\EstadosValidacion\Justificado.png"
                    Case ELL.HojaGastos.eEstado.Enviada
                        imgHGSinEnviar.ToolTip = "Hoja de gastos enviada al responsable"
                        imgHGSinEnviar.ImageUrl = "..\App_Themes\Tema1\Images\EstadosValidacion\Sin_Validar.png"
                    Case Else 'Este caso no se deberia dar
                        imgHGSinEnviar.ToolTip = "Estado: " & shoja(10) & ". Contacto con informatica"
                        imgHGSinEnviar.ImageUrl = "..\App_Themes\Tema1\Images\EstadosValidacion\Rechazada.png"
                End Select
            End If
            Dim oViaje As ELL.Viaje = Nothing
            If (shoja(2) IsNot Nothing) Then
                lblIdViaje.Text = "V" & shoja(2)
                e.Row.ToolTip = shoja(4)
            Else
                lblIdViaje.Text = "H" & shoja(3)
            End If
            CType(e.Row.FindControl("lblPersona"), Label).Text = shoja(8)
            lnkVerHoja.CommandArgument = shoja(0)
            itzultzaileWeb.Itzuli(lnkVerHoja)
            chbAnticipo.Checked = False : lnkVerAnticipo.Visible = False
            If (shoja(6) IsNot Nothing AndAlso CInt(shoja(5)) = CInt(shoja(7))) Then  'Si no es el responsable de liquidacion, no se mostrarán los anticipos
                chbAnticipo.Checked = True
                lnkVerAnticipo.CommandArgument = CInt(shoja(6))
                lnkVerAnticipo.Visible = True
                itzultzaileWeb.Itzuli(lnkVerAnticipo)
            End If
            pnlSinEntregar.Visible = False : pnlEntregado.Visible = False
            If (shoja(9) = String.Empty OrElse CDate(shoja(9)) = DateTime.MinValue) Then
                pnlSinEntregar.Visible = True And Not imgHGSinEnviar.Visible
                CType(e.Row.FindControl("txtFechaEntrega"), TextBox).Text = Now.ToShortDateString
                Dim script As New StringBuilder
                script.AppendLine("$('#" & pnlFechaEnt.ClientID & "').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
                AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(Me, Me.GetType, "DatePicker_" & shoja(0), script.ToString, True)
            Else
                pnlEntregado.Visible = True And Not imgHGSinEnviar.Visible
                CType(e.Row.FindControl("lblFechaEntrega"), Label).Text = CDate(shoja(9)).ToShortDateString
            End If
            CType(e.Row.FindControl("btnEntregar"), Button).CommandArgument = shoja(0)
        End If
    End Sub

    ''' <summary>
    ''' Se entrega la HG
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub btnEntregar_Click(sender As Object, e As EventArgs)
        Dim btn As Button = CType(sender, Button)
        Dim idHoja As Integer = CInt(btn.CommandArgument)
        Try
            Dim sFecha As String = CType(btn.Parent.FindControl("txtFechaEntrega"), TextBox).Text
            If (sFecha = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduce una fecha valida")
            Else
                Dim fEntrega As Date = CDate(sFecha)
                If (fEntrega > Now) Then
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La fecha no puede ser mayor al dia de hoy")
                Else
                    Dim hojaBLL As New BLL.HojasGastosBLL
                    hojaBLL.EntregarHGAdministracion(idHoja, fEntrega)
                    log.Info("La hoja " & idHoja & " ha sido entregada en administracion")
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("Hoja entregada")
                    Buscar()
                End If
            End If
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
            log.Error("Error al intentar entregar la HG " & idHoja & " en administracion", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ordenacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvHojasGastos_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvHojasGastos.Sorting
        Try
            gvHojasGastos.Attributes("CurrentSortField") = e.SortExpression
            If (gvHojasGastos.Attributes("CurrentSortDirection") Is Nothing) Then
                gvHojasGastos.Attributes("CurrentSortDirection") = SortDirection.Ascending
            Else
                gvHojasGastos.Attributes("CurrentSortDirection") = If(gvHojasGastos.Attributes("CurrentSortDirection") = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            End If
            Buscar()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordena la lista de hojas de gastos
    ''' </summary>
    ''' <param name="lHojas">Lista de hojas</param>
    ''' <remarks>HG.ID,HGE.FECHA_ENVIO,HG.ID_VIAJE,HG.ID_SIN_VIAJES,V.DESCRIPCION,V.ID_RESP_LIQUIDACION,ANTICIPO,HG.ID_USER,U.NOMBRE_COMPLETO </remarks>
    Private Sub Ordenar(ByRef lHojas As List(Of String()))
        Dim sortExp As String = "FechaEnvio"
        Dim sortDir As SortDirection = SortDirection.Descending
        If (gvHojasGastos.Attributes("CurrentSortField") IsNot Nothing) Then sortExp = gvHojasGastos.Attributes("CurrentSortField").ToString
        If (gvHojasGastos.Attributes("CurrentSortDirection") IsNot Nothing) Then sortDir = CType(gvHojasGastos.Attributes("CurrentSortDirection"), SortDirection)
        Select Case sortExp
            Case "FechaEnvio"
                If (sortDir = SortDirection.Ascending) Then
                    lHojas = lHojas.OrderBy(Of Date)(Function(o) CDate(o(1))).ToList
                Else
                    lHojas = lHojas.OrderByDescending(Of Date)(Function(o) CDate(o(1))).ToList
                End If
            Case "Persona"
                If (sortDir = SortDirection.Ascending) Then
                    lHojas = lHojas.OrderBy(Of String)(Function(o) o(8)).ToList
                Else
                    lHojas = lHojas.OrderByDescending(Of Date)(Function(o) o(8)).ToList
                End If
        End Select
    End Sub

    ''' <summary>
    ''' Se realiza la paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvHojasGastos_Paging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvHojasGastos.PageIndexChanging
        Try
            gvHojasGastos.PageIndex = e.NewPageIndex
            Buscar()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

End Class