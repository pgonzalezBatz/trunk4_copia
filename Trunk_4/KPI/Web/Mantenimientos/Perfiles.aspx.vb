Public Class Perfiles
    Inherits PageBase

#Region "Eventos de pagina"

    ''' <summary>
    ''' Carga de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                inicializarFiltros()
            End If
        Catch batzEx As SabLib.BatzException
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
            itzultzaileWeb.Itzuli(labelUsuario) : itzultzaileWeb.Itzuli(btnBuscar) : itzultzaileWeb.Itzuli(labelPlanta)
            itzultzaileWeb.Itzuli(btnAnadir) : itzultzaileWeb.Itzuli(labelArea) : itzultzaileWeb.Itzuli(labelNegocio)
        End If
    End Sub

#End Region

#Region "Filtros"

    ''' <summary>
    ''' Inicializa los controles de filtros
    ''' </summary>    
    Private Sub inicializarFiltros()
        Try
            GridViewSortDirection = SortDirection.Ascending
            GridViewSortExpresion = "NombreUsuario"
            searchUser.Limpiar()
            'acGV.Limpiar()
            cargarPlantas() : cargarNegocios()
        Catch ex As Exception
            Throw New SabLib.BatzException("errRellenarFiltros", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga las plantas existentes
    ''' </summary>    
    Private Sub cargarPlantas()
        If (ddlPlantas.Items.Count = 0) Then
            Dim plantBLL As New BLL.PlantasComponent
            Dim lPlantas As List(Of ELL.Planta) = plantBLL.loadListPlantas(New ELL.Planta)
            ddlPlantas.DataSource = (From plant In lPlantas Order By plant.Nombre Select plant.Id, plant.IdMoneda, plant.Nombre, plant.IdPlantaSAB).Distinct().ToList()
            ddlPlantas.DataBind()
            ddlPlantas.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
        End If
        ddlPlantas.SelectedIndex = -1
    End Sub

    ''' <summary>
    ''' Carga las negocios existentes
    ''' </summary>    
    Private Sub cargarNegocios()
        If (ddlNegocios.Items.Count = 0) Then
            Dim negBLL As New BLL.NegociosComponent
            Dim lNegocios As List(Of ELL.Negocio) = negBLL.loadListNegocios(New ELL.Negocio)
            If (lNegocios IsNot Nothing) Then lNegocios.Sort(Function(o1 As ELL.Negocio, o2 As ELL.Negocio) o1.Nombre < o2.Nombre)
            ddlNegocios.DataSource = lNegocios
            ddlNegocios.DataBind()
            ddlNegocios.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
        End If
        ddlNegocios.SelectedIndex = -1

        ddlAreas.Items.Clear()
        ddlAreas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un negocio"), 0))
    End Sub

    ''' <summary>
    ''' Carga las areas de un negocio
    ''' </summary>    
    ''' <param name="idNegocio">Id del negocio</param>
    Private Sub cargarAreas(ByVal idNegocio As Integer)
        ddlAreas.Items.Clear()
        Dim areaBLL As New BLL.AreasComponent
        Dim lAreas As List(Of ELL.Area) = areaBLL.loadListAreas(New ELL.Area With {.IdNegocio = idNegocio})
        If (lAreas IsNot Nothing) Then lAreas.Sort(Function(o1 As ELL.Area, o2 As ELL.Area) o1.Nombre < o2.Nombre)
        ddlAreas.DataSource = lAreas
        ddlAreas.DataBind()
        If (lAreas.Count = 0) Then
            ddlAreas.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("No existen areas"), 0))
        Else
            ddlAreas.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
        End If
        ddlAreas.SelectedIndex = -1
    End Sub

    ''' <summary>
    ''' Muestra los perfiles que cumplan las condiciones de los filtros
    ''' </summary>    
    Private Sub mostrarPerfiles()
        Try
            Dim perfBLL As New BLL.PerfilAreaComponent
            Dim oPerfil As New ELL.PerfilArea
            If (searchUser.SelectedId <> String.Empty) Then oPerfil.IdUsuario = CInt(searchUser.SelectedId)
            If (ddlAreas.SelectedValue > 0) Then oPerfil.IdArea = CInt(ddlAreas.SelectedValue)
            If (ddlPlantas.SelectedValue > 0) Then oPerfil.IdPlanta = CInt(ddlPlantas.SelectedValue)
            Dim lPerfiles As List(Of ELL.PerfilArea) = perfBLL.loadListPerfiles(oPerfil)
            Ordenar(lPerfiles, GridViewSortExpresion, GridViewSortDirection)
            gvPerfiles.DataSource = lPerfiles
            gvPerfiles.DataBind()
        Catch ex As Exception
            Throw New SabLib.BatzException("errMostrarDatos", ex)
        End Try
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Indica la direccion en la que hay que ordenar
    ''' </summary>
    ''' <value></value>
    ''' <returns>Orden</returns>
    Public Property GridViewSortDirection() As SortDirection
        Get
            If (ViewState("sortDirection") Is Nothing) Then
                ViewState("sortDirection") = SortDirection.Ascending
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
                ViewState("sortExpresion") = "Nombre"
            End If
            Return ViewState("sortExpresion").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("sortExpresion") = value
        End Set
    End Property

    ''' <summary>
    ''' Ordena la lista de items
    ''' </summary>
    ''' <param name="lItems">Lista de items</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>
    Private Sub Ordenar(ByRef lItems As List(Of ELL.PerfilArea), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr.ToLower
            Case "nombreusuario"
                lItems.Sort(Function(oItem1 As ELL.PerfilArea, oItem2 As ELL.PerfilArea) _
                 If(sortDir = SortDirection.Ascending, itzultzaileWeb.Itzuli(oItem1.NombreUsuario) < itzultzaileWeb.Itzuli(oItem2.NombreUsuario), itzultzaileWeb.Itzuli(oItem1.NombreUsuario) > itzultzaileWeb.Itzuli(oItem2.NombreUsuario)))
            Case "nombrearea"
                lItems.Sort(Function(oItem1 As ELL.PerfilArea, oItem2 As ELL.PerfilArea) _
                 If(sortDir = SortDirection.Ascending, itzultzaileWeb.Itzuli(oItem1.NombreArea) < itzultzaileWeb.Itzuli(oItem2.NombreArea), itzultzaileWeb.Itzuli(oItem1.NombreArea) > itzultzaileWeb.Itzuli(oItem2.NombreArea)))
            Case "nombreplanta"
                lItems.Sort(Function(oItem1 As ELL.PerfilArea, oItem2 As ELL.PerfilArea) _
                 If(sortDir = SortDirection.Ascending, itzultzaileWeb.Itzuli(oItem1.NombrePlanta) < itzultzaileWeb.Itzuli(oItem2.NombrePlanta), itzultzaileWeb.Itzuli(oItem1.NombrePlanta) > itzultzaileWeb.Itzuli(oItem2.NombrePlanta)))
        End Select
    End Sub

    ''' <summary>
    ''' Se muestran los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvPerfiles_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPerfiles.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim oItem As ELL.PerfilArea = e.Row.DataItem
            Dim imgDelete As ImageButton = CType(e.Row.FindControl("imgDelete"), ImageButton)            
            imgDelete.CommandArgument = oItem.IdUsuario & "_" & oItem.IdPlanta & "_" & oItem.IdArea
            imgDelete.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("confirmarEliminar") & "');"
            itzultzaileWeb.Itzuli(imgDelete)
        End If
    End Sub

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvPerfiles_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvPerfiles.Sorting
        Try
            GridViewSortDirection = If(GridViewSortDirection = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            GridViewSortExpresion = If(GridViewSortExpresion Is Nothing, String.Empty, e.SortExpression)

            mostrarPerfiles()
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvPerfiles_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPerfiles.RowCommand
        Try
            If (e.CommandName = "Del") Then
                Dim perfilBLL As New BLL.PerfilAreaComponent
                Dim datos As String() = e.CommandArgument.ToString.Split("_")
                Dim oPerfil As New ELL.PerfilArea With {.IdUsuario = CInt(datos(0)), .IdPlanta = CInt(datos(1)), .IdArea = CInt(datos(2))}
                If (perfilBLL.DeletePerfil(oPerfil)) Then
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("elementoBorrado")                    
                    log.Info("Se ha quitado el perfil para el usuario " & datos(0) & " | Area: " & datos(2) & " | Planta: " & datos(1))
                    mostrarPerfiles()
                Else
                    Master.MensajeError = itzultzaileWeb.Itzuli("errBorrar")
                    log.Error("Error al borrar el perfil")
                End If
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errBorrar")
            log.Error("Error al borrar el perfil", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se realiza la paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvPerfiles_Paging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPerfiles.PageIndexChanging
        Try
            gvPerfiles.PageIndex = e.NewPageIndex
            mostrarPerfiles()
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Se cargan las areas de un negocio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlNegocios_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNegocios.SelectedIndexChanged
        Try
            If (ddlNegocios.SelectedValue > 0) Then
                cargarAreas(CInt(ddlNegocios.SelectedValue))
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un negocio")
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
            log.Error("Error al cargar las areas del negocio " & ddlNegocios.SelectedValue, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Busca segun los filtros
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            mostrarPerfiles()
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Añade un perfil
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnAnadir_Click(sender As Object, e As EventArgs) Handles btnAnadir.Click
        Try
            If (searchUser.SelectedId = String.Empty OrElse ddlAreas.SelectedValue = 0 OrElse ddlPlantas.SelectedValue = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe introducir todos los datos")
            Else
                Dim perfBLL As New BLL.PerfilAreaComponent
                Dim oPerfil As New ELL.PerfilArea With {.IdArea = CInt(ddlAreas.SelectedValue), .IdPlanta = CInt(ddlPlantas.SelectedValue), .IdUsuario = CInt(searchUser.SelectedId)}
                Dim resul As Integer = perfBLL.AddPerfil(oPerfil)
                If (resul = 0) Then
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                    log.Info("Se ha añadido el perfil para el usuario " & searchUser.Texto & " | Area: " & ddlAreas.SelectedItem.Text & " | Planta: " & ddlPlantas.SelectedValue)
                    mostrarPerfiles()
                ElseIf (resul = 1) Then
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El elemento ya existe")
                End If
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
            log.Error("Error al añadir un perfil", ex)
        End Try
    End Sub

#End Region

End Class