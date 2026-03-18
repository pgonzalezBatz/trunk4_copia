Public Class Asignacion
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Carga la pagina de busqueda de viajes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Asignacion de anticipos"
            inicializar()
        End If
        Dim script As New StringBuilder
        script.AppendLine("$('#dtFechaIda').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
        script.AppendLine("$('#dtFechaVuelta').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
        AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(Me, Me.GetType, "DatePicker", script.ToString, True)
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelTitle) : itzultzaileWeb.Itzuli(labelFIda) : itzultzaileWeb.Itzuli(labelFV)
            itzultzaileWeb.Itzuli(labelFV) : itzultzaileWeb.Itzuli(btnSearch) : itzultzaileWeb.Itzuli(btnReset)
        End If
    End Sub

    ''' <summary>
    ''' Se inicializan los controles
    ''' </summary>    
    Private Sub inicializar()
        searchUserF.Limpiar()
        txtIdViaje.Text = String.Empty : txtFechaIda.Text = String.Empty : txtFechaVuelta.Text = String.Empty
        gvViajes.DataSource = Nothing : gvViajes.DataBind() : gvViajes.Visible = False
        txtIdViaje.Attributes.Add("placeholder", itzultzaileWeb.Itzuli("Nº de viaje"))
        searchUserF.PlaceHolder = itzultzaileWeb.Itzuli("Nombre, apellidos, nombre de usuario o numero de trabajador")
        gvViajes.Attributes("CurrentSortField") = "IdViaje"
        gvViajes.Attributes("CurrentSortDirection") = SortDirection.Descending
    End Sub

#End Region

#Region "Busqueda"

    ''' <summary>
    ''' Realiza la busqueda de viajes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        Try
            If (searchUserF.SelectedId = String.Empty AndAlso txtFechaIda.Text = String.Empty AndAlso txtFechaVuelta.Text = String.Empty AndAlso txtIdViaje.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe rellenar algun dato")
            ElseIf ((txtFechaIda.Text <> String.Empty And txtFechaVuelta.Text = String.Empty) Or (txtFechaIda.Text = String.Empty And txtFechaVuelta.Text <> String.Empty)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Rellena las fechas")
            Else
                BuscarViajes()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Resetea los valores del filtro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        inicializar()
    End Sub

    ''' <summary>
    ''' Busca los viajes que cumplan con el filtro
    ''' </summary>    
    Private Sub BuscarViajes()
        Try
            Dim oViaje As New ELL.Viaje With {.Estado = ELL.Viaje.eEstadoViaje.Validado}
            Dim viajesBLL As New BLL.ViajesBLL
            If (searchUserF.SelectedId <> String.Empty) Then
                oViaje.IdUserSolicitador = CInt(searchUserF.SelectedId)
                oViaje.ListaIntegrantes = New List(Of ELL.Viaje.Integrante)
                oViaje.ListaIntegrantes.Add(New ELL.Viaje.Integrante With {.Usuario = New SabLib.ELL.Usuario With {.Id = CInt(searchUserF.SelectedId)}})
            End If
            If (txtIdViaje.Text <> String.Empty) Then oViaje.IdViaje = CInt(txtIdViaje.Text)
            If (txtFechaIda.Text <> String.Empty) Then oViaje.FechaIda = CDate(txtFechaIda.Text)
            If (txtFechaVuelta.Text <> String.Empty) Then oViaje.FechaVuelta = CDate(txtFechaVuelta.Text)
            Dim lViajes As List(Of ELL.Viaje) = viajesBLL.BuscarViajes(oViaje, False, Master.IdPlantaGestion, False, True)
            If (lViajes IsNot Nothing AndAlso lViajes.Count > 0) Then
                lViajes = lViajes.FindAll(Function(o) o.Estado = ELL.Viaje.eEstadoViaje.Validado)
                Ordenar(lViajes)
            End If
            gvViajes.Visible = True
            gvViajes.DataSource = lViajes
            gvViajes.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al buscar los viajes", ex)
        End Try
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvViajes_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvViajes.PageIndexChanging
        Try
            gvViajes.PageIndex = e.NewPageIndex
            BuscarViajes()
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
            BuscarViajes()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvViajes_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvViajes.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oViaje As ELL.Viaje = e.Row.DataItem
            Dim integrantes As New StringBuilder
            For Each integr As ELL.Viaje.Integrante In oViaje.ListaIntegrantes
                If (integrantes.ToString <> String.Empty) Then integrantes.Append("<br />")
                If (oViaje.ResponsableLiquidacion IsNot Nothing AndAlso oViaje.ResponsableLiquidacion.Id = integr.Usuario.Id) Then
                    integrantes.Append("<b>" & integr.Usuario.NombreCompleto & "</b>")
                Else
                    integrantes.Append(integr.Usuario.NombreCompleto)
                End If
            Next
            CType(e.Row.FindControl("lblIdViaje"), Label).Text = "V" & oViaje.IdViaje
            CType(e.Row.FindControl("lblIntegrantes"), Label).Text = integrantes.ToString
            CType(e.Row.FindControl("lblFechaIda"), Label).Text = oViaje.FechaIda.ToShortDateString
            CType(e.Row.FindControl("lblFechaVuelta"), Label).Text = oViaje.FechaVuelta.ToShortDateString
            Dim sAnticipo As String = String.Empty
            If (oViaje.Anticipo IsNot Nothing) Then sAnticipo = [Enum].GetName(GetType(ELL.Anticipo.EstadoAnticipo), oViaje.Anticipo.Estado)
            CType(e.Row.FindControl("lblAnticipos"), Label).Text = itzultzaileWeb.Itzuli(sAnticipo)
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvViajes, "Select$" + oViaje.IdViaje.ToString)
        End If
    End Sub

    ''' <summary>
    ''' Se redirige a la pagina de detalle de un viaje
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvViajes_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvViajes.RowCommand
        If (e.CommandName = "Select") Then
            Response.Redirect("~/Viaje/SolicitudViaje.aspx?id=" & e.CommandArgument)
        End If
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

#End Region

End Class