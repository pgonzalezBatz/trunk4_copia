Public Class HistoricoTramitadas
    Inherits PageBase

    Dim oSolicitudesBLL As New BLL.SolicitudesBLL
    Dim oReferenciaVentaBLL As New BLL.ReferenciaFinalVentaBLL

#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of ELL.Roles.RolUsuario)
            roles.Add(ELL.Roles.RolUsuario.Administrador)
            roles.Add(ELL.Roles.RolUsuario.DocumentationTechnician)
            Return roles
        End Get
    End Property

    ''' <summary>
    ''' Identificador del usuario logueado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdUser() As Integer
        Get
            Dim persona As New SabLib.ELL.Ticket
            Dim id As Integer = Integer.MinValue

            If (Session("Ticket") IsNot Nothing) Then
                persona = CType(Session("Ticket"), SabLib.ELL.Ticket)
                id = persona.IdUser
            End If

            Return id
        End Get
        Set(value As Integer)

        End Set
    End property

#End Region

#Region "Page_Load"

    ''' <summary>
    ''' Carga de la página
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ComprobarAcceso()
        Catch ex As Exception
            Response.Redirect("~/PermisoDenegado.aspx")
        End Try

        If Not (Page.IsPostBack) Then
            EstablecerFechas()
            BindSolicitudes()
        End If
    End Sub

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Comprueba que el perfil tenga acceso a la página
    ''' </summary>
    Private Sub ComprobarAcceso()
        ' Hay que comprobar que alguno de los roles del usuario está contenido en la lista de roles de acceso de la pagina
        Dim tieneAcceso As Boolean = ExisteRolEnPagina(PerfilUsuario.IdRol)

        ' El administrador puede entrar a todas la páginas aunque no se haya definido su rol explicitamente en cada página
        If ((RolesAcceso Is Nothing OrElse Not tieneAcceso) AndAlso Not (PerfilUsuario.IdRol = ELL.Roles.RolUsuario.Administrador)) Then
            Dim segmentos As String() = Page.Request.Url.Segments
            Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
            Response.Redirect("~/PermisoDenegado.aspx", False)
        End If
    End Sub

    ''' <summary>
    ''' Comprueba que el rol del usuario está contenido en la lista de roles de acceso de la pagina
    ''' </summary>
    ''' <param name="rolUsuario"></param>
    ''' <returns>True si existe alguno. False en caso contrario</returns>
    ''' <remarks></remarks>
    Private Function ExisteRolEnPagina(ByVal rolUsuario As Integer) As Boolean
        Dim idRol As Integer = Integer.MinValue
        Dim existe As Boolean = False
        If (RolesAcceso IsNot Nothing) Then
            existe = RolesAcceso.Exists(Function(f) f = [Enum].Parse(GetType(ELL.Roles.RolUsuario), rolUsuario.ToString()))
            If (existe) Then
                Return existe
            End If
        End If

        Return existe
    End Function

    ''' <summary>
    ''' Establecer qué fechas son seleccionables para el CalendarExtender
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EstablecerFechas()
        txtFechaCreacionDesde.Attributes.Add("readonly", "readonly")
        txtFechaCreacionHasta.Attributes.Add("readonly", "readonly")
        txtFechaResolucionDesde.Attributes.Add("readonly", "readonly")
        txtFechaResolucionHasta.Attributes.Add("readonly", "readonly")
        imgCalendarioCreacionDesde_CalendarExtender.EndDate = DateTime.Today
        imgCalendarioResolucionDesde_CalendarExtender.EndDate = DateTime.Today
        imgCalendarioCreacionHasta_CalendarExtender.EndDate = DateTime.Today
        imgCalendarioResolucionHasta_CalendarExtender.EndDate = DateTime.Today
    End Sub

    ''' <summary>
    ''' Bind del grid de solicitudes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindSolicitudes()
        Dim lista As IEnumerable(Of Object) = Nothing
        lista = oSolicitudesBLL.CargarSolicitudesTramitadas(New ELL.FiltradoHistorial With {.IdTipoSolicitud = 1,
                                                                                           .IdUsuario = If(String.IsNullOrEmpty(txtUsuarioFiltrado.IdUsuario), Integer.MinValue, CInt(txtUsuarioFiltrado.IdUsuario)),
                                                                                           .Aprobado = rblAprobadoFiltrado.SelectedValue,
                                                                                           .IdSolicitud = If(String.IsNullOrEmpty(txtIdFiltrado.Text), Integer.MinValue, CInt(txtIdFiltrado.Text)),
                                                                                           .FechaCreacionDesde = txtFechaCreacionDesde.Text,
                                                                                           .FechaCreacionHasta = txtFechaCreacionHasta.Text,
                                                                                           .FechaResolucionDesde = txtFechaResolucionDesde.Text,
                                                                                           .FechaResolucionHasta = txtFechaResolucionHasta.Text}, True, IdUser)
        Ordenar(lista, GridViewSortExpresion, GridViewSortDirection)
        gvSolicitudesTramitadas.DataSource = lista
        gvSolicitudesTramitadas.DataBind()
    End Sub

    ''' <summary>
    ''' Devuelve el index de la columna pasándole el nombre de la columna
    ''' </summary>
    ''' <param name="grid"></param>
    ''' <param name="columnName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetColumnIndex(ByVal grid As GridView, ByVal columnName As String) As Integer
        Dim index As Integer = Integer.MinValue
        For Each column As DataControlField In grid.Columns
            If column.AccessibleHeaderText.Trim().ToUpper() = columnName.Trim().ToUpper() Then
                index = grid.Columns.IndexOf(column)
                Exit For
            End If
        Next
        Return index
    End Function

    ''' <summary>
    ''' Indica la direccion en la que hay que ordenar
    ''' </summary>
    ''' <value></value>
    ''' <returns>Orden</returns>
    Public Property GridViewSortDirection() As SortDirection
        Get
            If (ViewState("sortDirection") Is Nothing) Then
                ViewState("sortDirection") = SortDirection.Descending
            End If
            Return CType(ViewState("sortDirection"), SortDirection)
        End Get
        Set(ByVal value As SortDirection)
            ViewState("sortDirection") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica la expresion de ordenacion
    ''' </summary>
    ''' <value></value>
    ''' <returns>Expresion</returns>
    Public Property GridViewSortExpresion() As String
        Get
            If (ViewState("sortExpresion") Is Nothing) Then
                ViewState("sortExpresion") = "Id"
            End If
            Return ViewState("sortExpresion").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("sortExpresion") = value
        End Set
    End Property

    ''' <summary>
    ''' Ordena la lista de Areas
    ''' </summary>
    ''' <param name="lIncidencias">Lista de incidencias</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>	
    Private Sub Ordenar(ByRef lIncidencias As List(Of ELL.Solicitudes), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr
            Case "Id"
                lIncidencias.Sort(Function(oA1 As ELL.Solicitudes, oA2 As ELL.Solicitudes) _
                 IIf(sortDir = SortDirection.Ascending, oA1.Id < oA2.Id, oA1.Id > oA2.Id))
            Case "FechaAlta"
                lIncidencias.Sort(Function(oA1 As ELL.Solicitudes, oA2 As ELL.Solicitudes) _
                 IIf(sortDir = SortDirection.Ascending, oA1.FechaAlta < oA2.FechaAlta, oA1.FechaAlta > oA2.FechaAlta))
            Case "FechaGestion"
                lIncidencias.Sort(Function(oA1 As ELL.Solicitudes, oA2 As ELL.Solicitudes) _
                 IIf(sortDir = SortDirection.Ascending, oA1.FechaAlta < oA2.FechaAlta, oA1.FechaAlta > oA2.FechaAlta))
        End Select
    End Sub

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvSolicitudesTramitadas_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSolicitudesTramitadas.Sorting
        Try
            If (GridViewSortDirection = SortDirection.Ascending) Then
                GridViewSortDirection = SortDirection.Descending
            Else
                GridViewSortDirection = SortDirection.Ascending
            End If

            If (GridViewSortExpresion Is Nothing) Then
                GridViewSortExpresion = String.Empty
            Else
                GridViewSortExpresion = e.SortExpression
            End If

            BindSolicitudes()
        Catch batzEx As Exception
            Master.MensajeError = "An error occurred while sorting the processed requests".ToUpper
        End Try
    End Sub

#End Region

#Region "Handlers"

    ' ''' <summary>
    ' ''' ItemDataBound del DataList
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub dlSolicitudes_ItemDataBound(sender As Object, e As DataListItemEventArgs)
    '    If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.EditItem Then
    '        Dim itemID As Integer = Integer.Parse(Me.dlSolicitudes.DataKeys(e.Item.ItemIndex).ToString())

    '        Dim gvSolicitudesPendientes As GridView = TryCast(e.Item.FindControl("gvSolicitudesPendientes"), GridView)
    '        If (gvSolicitudesPendientes IsNot Nothing) Then
    '            Dim lista As List(Of ELL.ReferenciaVenta) = oReferenciaVentaBLL.CargarReferenciasPendientesSolicitud(itemID)
    '            gvSolicitudesPendientes.DataSource = lista
    '            gvSolicitudesPendientes.DataBind()
    '            Select Case ddlEstadoSolicitud.SelectedValue
    '                Case "0"
    '                    gvSolicitudesPendientes.Columns(GetColumnIndex(gvSolicitudesPendientes, "BatzPartNumber")).Visible = False
    '                Case "1"
    '                    gvSolicitudesPendientes.Columns(GetColumnIndex(gvSolicitudesPendientes, "BatzPartNumber")).Visible = True
    '            End Select

    '        End If
    '    End If
    'End Sub

    ' ''' <summary>
    ' ''' RowDataBound del grid de solicitudes pendientes
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub gvSolicitudesPendientes_RowDataBound(sender As Object, e As GridViewRowEventArgs)
    '    Try
    '        If (e.Row.RowType = DataControlRowType.DataRow) Then
    '            Dim oPlantas As New BLL.PlantasBLL
    '            Dim plantas As New List(Of ELL.ReferenciaPlantas)

    '            Dim gvSolicitudesPendientes As GridView = CType(sender, GridView)
    '            Dim id As String = gvSolicitudesPendientes.DataKeys(e.Row.RowIndex).Values(0).ToString()

    '            Dim lblPlantsToCharge As Label = TryCast(e.Row.FindControl("lblPlantsToCharge"), Label)
    '            plantas = oReferenciaVentaBLL.CargarPlantasReferencia(id)
    '            For Each planta In plantas
    '                lblPlantsToCharge.Text += oPlantas.CargarPlanta(planta.IdPlanta).Nombre + ","
    '            Next
    '            lblPlantsToCharge.Text = lblPlantsToCharge.Text.Substring(0, lblPlantsToCharge.Text.Length - 1)
    '        End If
    '    Catch ex As Exception
    '    End Try
    'End Sub

    ''' <summary>
    ''' RowDataBound del grid de solicitudes tramitadas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvSolicitudesTramitadas_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim oSolicitudesBLL As New BLL.SolicitudesBLL
                Dim idSolicitud As String = gvSolicitudesTramitadas.DataKeys(e.Row.RowIndex).Values(0).ToString()
                Dim solicitud As ELL.Solicitudes = oSolicitudesBLL.CargarSolicitud(idSolicitud)
                Dim gvReferencias As GridView = CType(e.Row.FindControl("gvReferencias"), GridView)

                If (gvReferencias IsNot Nothing) Then
                    gvReferencias.DataSource = oReferenciaVentaBLL.CargarReferenciasSolicitud(gvSolicitudesTramitadas.DataKeys(e.Row.RowIndex).Value)
                    gvReferencias.DataBind()
                    If Not solicitud.Aprobado Then
                        gvReferencias.Columns(2).Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
            Master.MensajeError = "Error al cargar las referencia de las solicitudes".ToUpper()
        End Try
    End Sub

    ''' <summary>
    ''' Cambio de index del Dropdownlist de estados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlEstadoSolicitud_SelectedIndexChanged(sender As Object, e As EventArgs)
        BindSolicitudes()
    End Sub

    ''' <summary>
    ''' Cambio de página de solicitudes tramitadas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvSolicitudesTramitadas_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvSolicitudesTramitadas.PageIndex = e.NewPageIndex
        BindSolicitudes()
    End Sub

    ''' <summary>
    ''' RowDataBound del grid de referencias
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvReferencias_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim oPlantas As New BLL.PlantasBLL
                Dim plantas As New List(Of ELL.ReferenciaPlantas)

                Dim gvReferencias As GridView = CType(sender, GridView)
                Dim id As String = gvReferencias.DataKeys(e.Row.RowIndex).Values(0).ToString()

                Dim lblPlantsToCharge As Label = TryCast(e.Row.FindControl("lblPlantsToCharge"), Label)
                plantas = oReferenciaVentaBLL.CargarPlantasReferencia(id)
                For Each planta In plantas
                    lblPlantsToCharge.Text += oPlantas.CargarPlanta(planta.IdPlanta).Nombre + ","
                Next
                lblPlantsToCharge.Text = lblPlantsToCharge.Text.Substring(0, lblPlantsToCharge.Text.Length - 1)
            End If
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Limpiar los campos de filtrado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnkbLimpiarCampos_Click(sender As Object, e As EventArgs)
        txtIdFiltrado.Text = String.Empty
        rblAprobadoFiltrado.SelectedValue = "2"
        txtUsuarioFiltrado.Usuario = String.Empty
        txtUsuarioFiltrado.IdUsuario = String.Empty
        txtFechaCreacionDesde.Text = String.Empty
        txtFechaCreacionHasta.Text = String.Empty
        txtFechaResolucionDesde.Text = String.Empty
        txtFechaResolucionHasta.Text = String.Empty
    End Sub

    ''' <summary>
    ''' Filtrar las solicitudes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnkbFiltrado_Click(sender As Object, e As EventArgs)
        BindSolicitudes()
    End Sub

    ''' <summary>
    ''' Enlace a los datos de Brain
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub lnbtBrain_Click(sender As Object, e As EventArgs)
        Dim btn As LinkButton = DirectCast(sender, LinkButton)
        Dim CommandArgument As String = btn.CommandArgument

        Dim ref As ELL.ReferenciaVenta = oReferenciaVentaBLL.CargarReferencia(CommandArgument)

        'Response.Redirect("ReferenciaVentaBrain.aspx?IdRef=" + CommandArgument + "&IdSol=" + ref.IdSolicitud.ToString(), False)
        'Dim URL As String = "ReferenciaVentaBrain.aspx?IdRef=" + CommandArgument + "&IdSol=" + ref.IdSolicitud.ToString()
        Dim URL As String = "ReferenciaVentaBrain.aspx?IdRef=" & CommandArgument
        Dim script As String = "<script type=""text/javascript"">window.open('" & URL & "');</script>"
        ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
    End Sub

#End Region

End Class