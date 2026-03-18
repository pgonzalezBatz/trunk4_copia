Public Class Plantas
    Inherits PageBase

#Region "Vista listado"

    ''' <summary>
    ''' Carga de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                inicializarListado()
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errMostrarListado")
            log.Error("Error al buscar las plantas en el page load", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelSearch) : itzultzaileWeb.Itzuli(btnBuscar) : itzultzaileWeb.Itzuli(lnkNuevo)
            itzultzaileWeb.Itzuli(labelNombre) : itzultzaileWeb.Itzuli(labelNombre2) : itzultzaileWeb.Itzuli(labelMoneda)
            itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnEliminar) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(labelNombrePlant) : itzultzaileWeb.Itzuli(rfvNombre) : itzultzaileWeb.Itzuli(labelAvisar)
            itzultzaileWeb.Itzuli(chbAvisar)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa la vista listado
    ''' </summary>    
    Private Sub inicializarListado()
        txtSearch.Text = String.Empty
        cargarListado()
    End Sub

    ''' <summary>
    ''' Busca un elemento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            cargarListado()
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Indica si recargara el listado o no
    ''' </summary>
    ''' <param name="reload">Indica si se recargara el listado o no</param>    
    Private Sub cargarListado(Optional ByVal reload As Boolean = True)
        Try
            mView.ActiveViewIndex = 0
            If (reload) Then
                Dim plantBLL As New BLL.PlantasComponent
                Dim lItems As List(Of ELL.Planta) = plantBLL.loadListPlantas(New ELL.Planta With {.Nombre = txtSearch.Text})
                Ordenar(lItems, GridViewSortExpresion, GridViewSortDirection)                
                gvItems.DataSource = lItems
                gvItems.DataBind()
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("errMostrarListado", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se prepara el formulario para un nuevo item
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkNuevo_Click(sender As Object, e As EventArgs) Handles lnkNuevo.Click
        Try
            mostrarDetalle(Integer.MinValue)
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Evento que surge cuando se enlazan los items
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvItems_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvItems.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim oItem As ELL.Planta = e.Row.DataItem
            CType(e.Row.FindControl("chbAvisar"), CheckBox).Checked = oItem.Avisar
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvItems, "Select$" + CStr(oItem.Id))
        End If
    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvItems_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvItems.RowCommand
        Try
            If (e.CommandName = "Select") Then
                mostrarDetalle(CInt(e.CommandArgument))
            End If
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvItems_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvItems.Sorting
        Try
            GridViewSortDirection = If(GridViewSortDirection = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            GridViewSortExpresion = If(GridViewSortExpresion Is Nothing, String.Empty, e.SortExpression)

            cargarListado()
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

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
    Private Sub Ordenar(ByRef lItems As List(Of ELL.Planta), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr.ToLower
            Case "nombre"
                lItems.Sort(Function(oItem1 As ELL.Planta, oItem2 As ELL.Planta) _
                 If(sortDir = SortDirection.Ascending, itzultzaileWeb.Itzuli(oItem1.Nombre) < itzultzaileWeb.Itzuli(oItem2.Nombre), itzultzaileWeb.Itzuli(oItem1.Nombre) > itzultzaileWeb.Itzuli(oItem2.Nombre)))
            Case "nombremoneda"
                lItems.Sort(Function(oItem1 As ELL.Planta, oItem2 As ELL.Planta) _
                 If(sortDir = SortDirection.Ascending, itzultzaileWeb.Itzuli(oItem1.NombreMoneda) < itzultzaileWeb.Itzuli(oItem2.NombreMoneda), itzultzaileWeb.Itzuli(oItem1.NombreMoneda) > itzultzaileWeb.Itzuli(oItem2.NombreMoneda)))
        End Select
    End Sub

    ''' <summary>
    ''' Se realiza la paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvItems_Paging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvItems.PageIndexChanging
        Try
            gvItems.PageIndex = e.NewPageIndex
            cargarListado()
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Vista detalle"

    ''' <summary>
    ''' Se muestra el detalle del item
    ''' </summary>
    ''' <param name="id">Id del item</param>    
    Private Sub mostrarDetalle(ByVal id As Integer)
        Try
            inicializarDetalle()
            If (id > 0) Then
                Dim negBLL As New BLL.PlantasComponent
                Dim oItem As ELL.Planta = negBLL.loadPlanta(id)
                btnGuardar.CommandArgument = oItem.Id
                pnlNew.Visible = False : pnlUpdate.Visible = True
                txtNombre.Text = oItem.Nombre
                ddlMonedas.SelectedIndex = ddlMonedas.Items.IndexOf(ddlMonedas.Items.FindByValue(oItem.IdMoneda))
                chbAvisar.Checked = oItem.Avisar
                btnEliminar.Visible = negBLL.CanDeletePlanta(id)                
            Else
                cargarPlantasRestantes()
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("errCompDetalle", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga la moneda de la planta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlPlantas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPlantas.SelectedIndexChanged
        If (ddlPlantas.SelectedIndex = 0) Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("seleccionePlanta")
        Else            
            ddlMonedas.SelectedValue = CInt(ddlPlantas.SelectedValue.Split("_")(1))
            txtNombreNuevo.Text = ddlPlantas.SelectedItem.Text
        End If
    End Sub

    ''' <summary>
    ''' Inicializa el detalle
    ''' </summary>    
    Private Sub inicializarDetalle()
        mView.ActiveViewIndex = 1
        txtNombre.Text = String.Empty : txtNombreNuevo.Text = String.Empty
        chbAvisar.Checked = False
        pnlNew.Visible = True : pnlUpdate.Visible = False
        cargarMonedas()
        titDetalle.Texto = itzultzaileWeb.Itzuli("Detalle")
        btnGuardar.CommandArgument = String.Empty
        btnEliminar.Visible = False
        btnEliminar.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("confirmarEliminar") & "');"
    End Sub

    ''' <summary>
    ''' Carga las monedas
    ''' </summary>    
    Private Sub cargarMonedas()
        If (ddlMonedas.Items.Count = 0) Then
            Dim xbatBLL As New BLL.XbatComponent
            Dim lMonedas As List(Of String()) = xbatBLL.loadListMonedas()
            ddlMonedas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), "0"))
            For Each sMon As String() In lMonedas
                ddlMonedas.Items.Add(New ListItem(sMon(1), sMon(0)))
            Next            
        End If
        ddlMonedas.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Carga las plantas en el desplegable que no han sido ya añadidas
    ''' </summary>    
    Private Sub CargarPlantasRestantes()
        ddlPlantas.Items.Clear()
        ddlPlantas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), "0"))
        Dim plantBLL As New BLL.PlantasComponent
        Dim lPlantas As List(Of SabLib.ELL.Planta) = plantBLL.loadListPlantasLibres()
        If (lPlantas IsNot Nothing) Then lPlantas.Sort(Function(o1 As SabLib.ELL.Planta, o2 As SabLib.ELL.Planta) o1.Nombre < o2.Nombre)
        For Each oPlant As SabLib.ELL.Planta In lPlantas
            ddlPlantas.Items.Add(New ListItem(oPlant.Nombre, oPlant.Id & "_" & If(oPlant.IdMoneda > 0, oPlant.IdMoneda, 0)))  'Value: Id_IdMoneda
        Next
        ddlPlantas.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Se guardan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            If ((pnlNew.Visible AndAlso txtNombreNuevo.Text = String.Empty) OrElse (pnlUpdate.Visible AndAlso txtNombre.Text = String.Empty AndAlso ddlPlantas.SelectedIndex = 0)) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe introducir todos los datos")
            Else
                Dim negBLL As New BLL.PlantasComponent
                Dim oItem As New ELL.Planta
                If (pnlUpdate.Visible) Then
                    oItem.Id = CInt(btnGuardar.CommandArgument)
                    oItem.Nombre = txtNombre.Text
                Else 'Nuevo
                    Dim value As String() = ddlPlantas.SelectedValue.Split("_")
                    oItem.IdPlantaSAB = CInt(value(0))
                    oItem.Nombre = txtNombreNuevo.Text
                End If
                oItem.IdMoneda = CInt(ddlMonedas.SelectedValue)
                oItem.Avisar = chbAvisar.Checked
                Dim idItem As Integer = negBLL.SavePlanta(oItem)
                If (pnlUpdate.Visible) Then
                    log.Info("Se ha modificado los datos de la planta - " & idItem)
                Else
                    log.Info("Se ha insertado una nueva planta - " & idItem)
                End If
                Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                cargarListado()
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
            log.Error("Error al guardar los datos de la planta", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se elimina el item. Solo se podra si no esta enlazado con ningun elemento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Try
            Dim negBLL As New BLL.PlantasComponent
            Dim idItem As Integer = CInt(btnGuardar.CommandArgument)
            If (negBLL.DeletePlanta(idItem)) Then
                log.Info("Se ha borrado la planta " & idItem & " - " & txtNombre.Text)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("elementoBorrado")
                cargarListado()
            Else
                Master.MensajeError = itzultzaileWeb.Itzuli("errBorrar")
                log.Error("Error al borrar la planta " & txtNombre.Text)
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errBorrar")
            log.Error("Error al borrar la planta " & txtNombre.Text, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Try
            Volver(False)
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errMostrarListado")
            log.Error("Error al buscar las plantas al volver del detalle", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>    
    ''' <param name="reload">Indica si se recargara el listado</param>
    Private Sub Volver(ByVal reload As Boolean)
        cargarListado(reload)
    End Sub

#End Region

End Class