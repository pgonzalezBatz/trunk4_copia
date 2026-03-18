Public Class Unidades
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
            log.Error("Error al buscar las unidades en el page load", ex)
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
            itzultzaileWeb.Itzuli(labelNombre) : itzultzaileWeb.Itzuli(labelTextoM) : itzultzaileWeb.Itzuli(labelEsMoneda)
            itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnEliminar) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(rfvNombre)
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
                Dim negBLL As New BLL.UnidadesComponent
                Dim lItems As List(Of ELL.Unidad) = negBLL.loadListUnidades(New ELL.Unidad With {.Nombre = txtSearch.Text})
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
    Private Sub gvItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvItems.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim oItem As ELL.Unidad = e.Row.DataItem
            Dim chbEsMoneda As CheckBox = CType(e.Row.FindControl("chbEsMoneda"), CheckBox)
            Dim lblNombre As Label = CType(e.Row.FindControl("lblNombre"), Label)
            chbEsMoneda.Checked = oItem.EsMoneda
            lblNombre.Text = oItem.Nombre
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvItems, "Select$" + CStr(oItem.Id))
        End If
    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvItems_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvItems.RowCommand
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
    Protected Sub gvItems_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvItems.Sorting
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
    Private Sub Ordenar(ByRef lItems As List(Of ELL.Unidad), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr.ToLower
            Case "nombre"
                lItems.Sort(Function(oItem1 As ELL.Unidad, oItem2 As ELL.Unidad) _
                 If(sortDir = SortDirection.Ascending, itzultzaileWeb.Itzuli(oItem1.Nombre) < itzultzaileWeb.Itzuli(oItem2.Nombre), itzultzaileWeb.Itzuli(oItem1.Nombre) > itzultzaileWeb.Itzuli(oItem2.Nombre)))
            Case "textomostrar"
                lItems.Sort(Function(oItem1 As ELL.Unidad, oItem2 As ELL.Unidad) _
                 If(sortDir = SortDirection.Ascending, itzultzaileWeb.Itzuli(oItem1.TextoMostrar) < itzultzaileWeb.Itzuli(oItem2.TextoMostrar), itzultzaileWeb.Itzuli(oItem1.TextoMostrar) > itzultzaileWeb.Itzuli(oItem2.TextoMostrar)))
        End Select
    End Sub

    ''' <summary>
    ''' Se realiza la paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvItems_Paging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvItems.PageIndexChanging
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
                Dim negBLL As New BLL.UnidadesComponent
                Dim oItem As ELL.Unidad = negBLL.loadUnidad(id)
                btnGuardar.CommandArgument = oItem.Id
                txtNombre.Text = oItem.Nombre
                txtTextoMostrar.Text = oItem.TextoMostrar
                chbEsMoneda.Checked = oItem.EsMoneda
                btnEliminar.Visible = negBLL.CanDelete(id)
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("errCompDetalle", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa el detalle
    ''' </summary>    
    Private Sub inicializarDetalle()
        mView.ActiveViewIndex = 1
        txtNombre.Text = String.Empty : txtTextoMostrar.Text = String.Empty : chbEsMoneda.Checked = False
        btnGuardar.CommandArgument = String.Empty
        titDetalle.Texto = itzultzaileWeb.Itzuli("Detalle")
        btnEliminar.Visible = False
        btnEliminar.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("confirmarEliminar") & "');"        
    End Sub

    ''' <summary>
    ''' Se guardan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            If (txtNombre.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("introduzcaNombre")
            ElseIf (txtTextoMostrar.Text = String.Empty AndAlso Not chbEsMoneda.Checked) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca el texto a mostrar")
            Else
                Dim negBLL As New BLL.UnidadesComponent
                Dim oItem As New ELL.Unidad
                If (btnGuardar.CommandArgument <> String.Empty) Then oItem.Id = CInt(btnGuardar.CommandArgument)
                oItem.Nombre = txtNombre.Text
                oItem.TextoMostrar = txtTextoMostrar.Text
                oItem.EsMoneda = chbEsMoneda.Checked
                Dim idItem As Integer = negBLL.SaveUnidad(oItem)
                If (btnGuardar.CommandArgument <> String.Empty) Then
                    log.Info("Se ha modificado los datos de la unidad - " & idItem)
                Else
                    log.Info("Se ha insertado una nueva unidad - " & idItem)
                End If
                Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                cargarListado()
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
            log.Error("Error al guardar los datos de la unidad", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se elimina el item. Solo se podra si no esta enlazado con ningun elemento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Try
            Dim negBLL As New BLL.UnidadesComponent
            Dim idItem As Integer = CInt(btnGuardar.CommandArgument)
            If (negBLL.DeleteUnidad(idItem)) Then
                log.Info("Se ha borrado la unidad " & idItem & " - " & txtNombre.Text)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("elementoBorrado")
                cargarListado()
            Else
                Master.MensajeError = itzultzaileWeb.Itzuli("errBorrar")
                log.Error("Error al borrar la unidad " & txtNombre.Text)
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errBorrar")
            log.Error("Error al borrar la unidad " & txtNombre.Text, ex)
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
            log.Error("Error al buscar las unidades al volver del detalle", ex)
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