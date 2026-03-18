Public Class ListadoFormacionISO
    Inherits PageBase

    ''' <summary>
    ''' Inicia la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>	
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Gente a formar ISOs"
            txtFechaInicio.Text = Now.AddMonths(-1).ToShortDateString
            txtFechaFin.Text = Now.ToShortDateString
            gvPersonas.DataSource = Nothing : gvPersonas.DataBind()
            gvPersonas.Attributes("CurrentSortField") = "NombreCompleto"
            gvPersonas.Visible = False
        End If
        Dim script As New StringBuilder
        script.AppendLine("$('#dtFechaIni').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
        script.AppendLine("$('#dtFechaFin').datetimepicker({showClear:true,locale:'" & Master.Ticket.Culture & "',format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
        AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(Me, Me.GetType, "DatePicker", script.ToString, True)
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelSOS) : itzultzaileWeb.Itzuli(labelFIni) : itzultzaileWeb.Itzuli(labelFFin)
            itzultzaileWeb.Itzuli(btnBuscar)
        End If
    End Sub

    ''' <summary>
    ''' Se busca las personas que han viajado entre fechas sin el documento de ISOS
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            If (txtFechaInicio.Text.Trim = String.Empty OrElse txtFechaFin.Text.Trim = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Rellene todos los datos")
            ElseIf (CDate(txtFechaInicio.Text) > CDate(txtFechaFin.Text)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("La fecha de ida no puede ser mayor que la fecha de vuelta")
            Else
                gvPersonas.Visible = True
                cargarPersonas()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra las personas que no tienen el documento ISOS
    ''' </summary>
    Private Sub cargarPersonas()
        Dim bidaiakBLL As New BLL.BidaiakBLL
        Dim lPersonas As List(Of Object) = bidaiakBLL.GetPersonasViajerasSinFormacionISOS(CDate(txtFechaInicio.Text), CDate(txtFechaFin.Text))
        OrdenarLista(lPersonas, gvPersonas.Attributes("CurrentSortField"), gvPersonas.Attributes("CurrentSortDirection"))
        gvPersonas.DataSource = lPersonas
        gvPersonas.DataBind()
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvPersonas_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvPersonas.PageIndexChanging
        Try
            gvPersonas.PageIndex = e.NewPageIndex
            cargarPersonas()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordenacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvPersonas_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvPersonas.Sorting
        Try
            gvPersonas.Attributes("CurrentSortField") = e.SortExpression
            If (gvPersonas.Attributes("CurrentSortDirection") Is Nothing) Then
                gvPersonas.Attributes("CurrentSortDirection") = SortDirection.Ascending
            Else
                gvPersonas.Attributes("CurrentSortDirection") = If(gvPersonas.Attributes("CurrentSortDirection") = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            End If
            cargarPersonas()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordena una lista sin conocer el tipo que es
    ''' </summary>
    ''' <param name="lista"></param>    
    ''' <param name="CampoOrden"></param>
    ''' <param name="DireccionCampo"></param>        
    Private Sub OrdenarLista(ByRef lista As List(Of Object), Optional ByVal CampoOrden As String = Nothing, Optional ByVal DireccionCampo As SortDirection = SortDirection.Ascending)
        Select Case CampoOrden
            Case "CodPersona"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Int32)(Function(o) CInt(o.CodPersona)).ToList
                Else
                    lista = lista.OrderByDescending(Of Int32)(Function(o) CInt(o.CodPersona)).ToList
                End If
            Case "NumViajes"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Int32)(Function(o) CInt(o.NumViajes)).ToList
                Else
                    lista = lista.OrderByDescending(Of Int32)(Function(o) CInt(o.NumViajes)).ToList
                End If
            Case "NombreCompleto"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.NombreCompleto).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.NombreCompleto).ToList
                End If
        End Select
    End Sub

End Class