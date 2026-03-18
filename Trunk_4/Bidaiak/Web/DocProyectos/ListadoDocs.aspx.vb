
Public Class ListadoDocs
    Inherits PageBase

#Region "Properties"

    ''' <summary>
    ''' Obtiene el string de conexion para obtener los clientes y los proyectos
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property StringConexionTroq As String
        Get
            If (ViewState("conTroq") Is Nothing) Then ViewState("conTroq") = String.Empty
            Return ViewState("conTroq")
        End Get
        Set(value As String)
            ViewState("conTroq") = value
        End Set
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Se carga los clientes para poder ver la documentacion de los proyectos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Documentacion de proyectos"
                inicializarPagina()
                If (Request.QueryString("idProy") IsNot Nothing) Then
                    Dim idProy As Integer = CInt(Request.QueryString("idProy"))
                    Dim xbatBLL As New BLL.XbatBLL
                    Dim info As String() = xbatBLL.GetInfoClienteProyecto(idProy).FirstOrDefault
                    ddlCliente.SelectedValue = info(2)
                    cargarProyectos(info(2))
                    ddlProyecto.SelectedValue = idProy
                    mostrarDocumentos(idProy)
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se inicializa la pagina
    ''' De momento es solo para troqueleria
    ''' </summary>    
    Private Sub inicializarPagina()
        pnlResultado.Visible = False
        gvDocumentos.Attributes("CurrentSortField") = "Descripcion"
        gvDocumentos.Attributes("CurrentSortDirection") = SortDirection.Ascending
        If (Master.IdPlantaGestion = 1) Then 'Para Igorre, se cargara la UO de Troqueleria
            Dim unidOrgBLL As New BLL.UnidadOrgBLL
            Dim oUnit As ELL.UnidadOrg = unidOrgBLL.load(1)
            StringConexionTroq = oUnit.StringConexion
            cargarClientes()
            ddlProyecto.Items.Clear()
            ddlProyecto.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Elegir un cliente"), Integer.MinValue))
        Else
            Throw New BatzException("No se ha definido la Unidad Organizativa de esta empresa", Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Traduccion de controles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelCliente) : itzultzaileWeb.Itzuli(labelProyecto) : itzultzaileWeb.Itzuli(lnkAnadir)
        End If
    End Sub

#End Region

#Region "Carga de los datos del filtro"

    ''' <summary>
    ''' Carga los clientes
    ''' </summary>	    
    Private Sub cargarClientes()
        Try
            If (ddlCliente.Items.Count = 0) Then
                Dim cliProyBLL As BLL.UO.IClienteProyecto = CType(New BLL.UO.CliProy_XBAT(), BLL.UO.IClienteProyecto)
                Dim lClientes As List(Of String()) = cliProyBLL.GetClientes(StringConexionTroq)
                If (lClientes IsNot Nothing) Then lClientes = lClientes.OrderBy(Of String)(Function(o) o(1)).ToList
                ddlCliente.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
                For Each sCli In lClientes
                    ddlCliente.Items.Add(New ListItem(sCli(1), sCli(0)))
                Next
            End If
            ddlCliente.SelectedIndex = 0
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al cargar los clientes", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los proyectos de un cliente
    ''' </summary>	
    ''' <param name="codCli">Codigo del cliente</param>    
    Private Sub cargarProyectos(ByVal codCli As String)
        Try
            Dim cliProyBLL As BLL.UO.IClienteProyecto = CType(New BLL.UO.CliProy_XBAT(), BLL.UO.IClienteProyecto)
            Dim lProyectos As List(Of String()) = cliProyBLL.GetProyectos(codCli, StringConexionTroq)
            If (lProyectos IsNot Nothing) Then lProyectos = lProyectos.OrderBy(Of String)(Function(o) o(1)).ToList
            ddlProyecto.Items.Clear()
            ddlProyecto.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
            For Each sProy In lProyectos
                ddlProyecto.Items.Add(New ListItem(sProy(1), sProy(0)))
            Next
            ddlProyecto.SelectedIndex = 0
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al cargar los proyectos de un cliente", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el desplegable con los proyectos del cliente
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCliente.SelectedIndexChanged
        Try
            pnlResultado.Visible = False
            If (ddlCliente.SelectedValue = Integer.MinValue) Then
                ddlProyecto.Items.Clear()
                ddlProyecto.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Elegir un cliente"), Integer.MinValue))
            Else
                cargarProyectos(ddlCliente.SelectedValue)
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se muestran los documentos de un proyecto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlProyecto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProyecto.SelectedIndexChanged
        Try
            If (ddlProyecto.SelectedValue = Integer.MinValue) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un proyecto")
            Else
                mostrarDocumentos(CInt(ddlProyecto.SelectedValue))
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al mostrar los documentos")
        End Try
    End Sub

#End Region

#Region "Listado"

    ''' <summary>
    ''' Muestra los documentos de un proyecto
    ''' </summary>
    ''' <param name="idProyecto">Id del proyecto</param>    
    Private Sub mostrarDocumentos(idProyecto As Integer)
        Try
            pnlResultado.Visible = True
            Dim xbatBLL As New BLL.XbatBLL
            Dim lDocs As List(Of ELL.DocumentoProyecto) = xbatBLL.GetDocumentosProyecto(idProyecto)
            If (lDocs IsNot Nothing AndAlso lDocs.Count > 0) Then Ordenar(lDocs)
            gvDocumentos.DataSource = lDocs
            gvDocumentos.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar los documentos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los documentos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvDocumentos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDocumentos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            ItzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oDoc As ELL.DocumentoProyecto = e.Row.DataItem
            Dim userBLL As New Sablib.BLL.UsuariosComponent
            DirectCast(e.Row.FindControl("lblFecha"), Label).Text = oDoc.Fecha.ToShortDateString
            DirectCast(e.Row.FindControl("lblSubidoPor"), Label).Text = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oDoc.IdUsuario}, False).NombreCompleto
            e.Row.ToolTip = itzultzaileWeb.Itzuli("Ver detalle")
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvDocumentos, "Select$" + CStr(oDoc.Id))
        End If
    End Sub

    ''' <summary>
    ''' Eventos de la fila
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvDocumentos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDocumentos.RowCommand
        If (e.CommandName = "Select") Then Response.Redirect("DetalleDoc.aspx?idDoc=" & e.CommandArgument & "&idProy=" & ddlProyecto.SelectedValue)
    End Sub

    ''' <summary>
    ''' Se redirige al formulario para añadir un documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkAnadir_Click(sender As Object, e As EventArgs) Handles lnkAnadir.Click
        Response.Redirect("DetalleDoc.aspx?idProy=" & ddlProyecto.SelectedValue)
    End Sub

    ''' <summary>
    ''' Orden
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvDocumentos_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvDocumentos.Sorting
        Try
            gvDocumentos.Attributes("CurrentSortField") = e.SortExpression
            If (gvDocumentos.Attributes("CurrentSortDirection") Is Nothing) Then
                gvDocumentos.Attributes("CurrentSortDirection") = SortDirection.Ascending
            Else
                gvDocumentos.Attributes("CurrentSortDirection") = If(gvDocumentos.Attributes("CurrentSortDirection") = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            End If
            mostrarDocumentos(CInt(ddlProyecto.SelectedValue))
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Realiza la paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvDocumentos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvDocumentos.PageIndexChanging
        Try
            gvDocumentos.PageIndex = e.NewPageIndex
            mostrarDocumentos(CInt(ddlProyecto.SelectedValue))
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordena la lista
    ''' </summary>
    ''' <param name="lista">Lista</param>
    Public Sub Ordenar(ByRef lista As List(Of ELL.DocumentoProyecto))
        Select Case gvDocumentos.Attributes("CurrentSortField")
            Case "Descripcion"
                If (gvDocumentos.Attributes("CurrentSortDirection") = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.Descripcion).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.Descripcion).ToList
                End If
        End Select
    End Sub

#End Region

End Class