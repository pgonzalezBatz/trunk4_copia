Public Class NegociosAreas
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
            log.Error("Error al buscar los negocios en el page load", ex)
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
            itzultzaileWeb.Itzuli(labelNombre) : itzultzaileWeb.Itzuli(lnkNuevaArea) : itzultzaileWeb.Itzuli(labelNombreArea)
            itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(labelCopiarInfo)
            itzultzaileWeb.Itzuli(labelNegOrig) : itzultzaileWeb.Itzuli(labelAreaOrig) : itzultzaileWeb.Itzuli(labelNegDest)
            itzultzaileWeb.Itzuli(btnCopiar) : itzultzaileWeb.Itzuli(lnkCopiarArea) : itzultzaileWeb.Itzuli(pnlAreas)
            itzultzaileWeb.Itzuli(labelAreaInfo) : itzultzaileWeb.Itzuli(btnSaveArea) : itzultzaileWeb.Itzuli(btnDeleteArea)
            itzultzaileWeb.Itzuli(labelCopiarInfo) : itzultzaileWeb.Itzuli(labelNegOrig) : itzultzaileWeb.Itzuli(labelAreaOrig)
            itzultzaileWeb.Itzuli(labelNegDest) : itzultzaileWeb.Itzuli(labelNombreArea) : itzultzaileWeb.Itzuli(btnCopiar)
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
                Dim negBLL As New BLL.NegociosComponent
                Dim lItems As List(Of ELL.Negocio) = negBLL.loadListNegocios(New ELL.Negocio With {.Nombre = txtSearch.Text})
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
    ''' Prepara el formulario para copiar un area
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lnkCopiarArea_Click(sender As Object, e As EventArgs) Handles lnkCopiarArea.Click
        Try
            inicializarCopiado()
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
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
            Dim oItem As ELL.Negocio = e.Row.DataItem
            Dim lblNombre As Label = CType(e.Row.FindControl("lblNombre"), Label)
            Dim chbTieneAreas As CheckBox = CType(e.Row.FindControl("chbTieneAreas"), CheckBox)
            lblNombre.Text = oItem.Nombre
            Dim areaBLL As New BLL.AreasComponent
            Dim lAreas As List(Of ELL.Area) = areaBLL.loadListAreas(New ELL.Area With {.IdNegocio = oItem.Id})
            chbTieneAreas.Checked = (lAreas IsNot Nothing AndAlso lAreas.Count > 0)

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
    Private Sub Ordenar(ByRef lItems As List(Of ELL.Negocio), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr.ToLower
            Case "nombre"
                lItems.Sort(Function(oItem1 As ELL.Negocio, oItem2 As ELL.Negocio) _
                 If(sortDir = SortDirection.Ascending, itzultzaileWeb.Itzuli(oItem1.Nombre) < itzultzaileWeb.Itzuli(oItem2.Nombre), itzultzaileWeb.Itzuli(oItem1.Nombre) > itzultzaileWeb.Itzuli(oItem2.Nombre)))
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
                Dim negBLL As New BLL.NegociosComponent
                Dim oItem As ELL.Negocio = negBLL.loadNegocio(id)
                btnGuardar.CommandArgument = oItem.Id
                txtNombre.Text = oItem.Nombre
                cargarAreas(id)
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
        txtNombre.Text = String.Empty : pnlAreas.Visible = False
        itzultzaileWeb.Itzuli(titDetalle)
        gvAreas.DataSource = Nothing
        btnGuardar.CommandArgument = String.Empty
    End Sub

    ''' <summary>
    ''' Carga las areas de un negocio
    ''' </summary>    
    ''' <param name="idNeg">Id del negocio</param>
    Private Sub cargarAreas(ByVal idNeg As Integer)
        Dim areasBLL As New BLL.AreasComponent
        pnlAreas.Visible = True
        Dim lAreas As List(Of ELL.Area) = areasBLL.loadListAreas(New ELL.Area With {.IdNegocio = idNeg})
        If (lAreas IsNot Nothing) Then lAreas.Sort(Function(o1 As ELL.Area, o2 As ELL.Area) o1.Nombre < o2.Nombre)        
        gvAreas.DataSource = lAreas
        gvAreas.DataBind()
    End Sub

    ''' <summary>
    ''' Se guardan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            If (txtNombre.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe introducir todos los datos")
            Else
                Dim negBLL As New BLL.NegociosComponent
                Dim oItem As New ELL.Negocio
                If (btnGuardar.CommandArgument <> String.Empty) Then oItem.Id = CInt(btnGuardar.CommandArgument)
                oItem.Nombre = txtNombre.Text
                Dim idItem As Integer = negBLL.SaveNegocio(oItem)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                If (btnGuardar.CommandArgument <> String.Empty) Then
                    log.Info("Se ha modificado los datos del negocio - " & idItem)
                    cargarListado() 'Si ya existia se vuelve al listado
                Else
                    log.Info("Se ha insertado un nuevo negocio - " & idItem)
                    btnGuardar.CommandArgument = idItem
                    cargarAreas(idItem) 'Si no existia, se muestran las areas
                End If
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
            log.Error("Error al guardar los datos del negocio", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Try
            Volver(True) 'Si se ha añadido un nuevo item, no saldria en el listado, ya que al añadir, no se vuelve al listado, sino que muestra las areas
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errMostrarListado")
            log.Error("Error al buscar los negocios al volver del detalle", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>    
    ''' <param name="reload">Indica si se recargara el listado</param>
    Private Sub Volver(ByVal reload As Boolean)
        cargarListado(reload)
    End Sub

    ''' <summary>
    ''' Evento que surge cuando se enlazan los items
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvAreas_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAreas.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim oItem As ELL.Area = e.Row.DataItem
            Dim lblNombre As Label = CType(e.Row.FindControl("lblNombre"), Label)
            Dim lblValores As Label = CType(e.Row.FindControl("lblValores"), Label)
            Dim lblIndicadores As Label = CType(e.Row.FindControl("lblIndicadores"), Label)
            lblNombre.Text = oItem.Nombre
            Dim areaBLL As New BLL.AreasComponent
            Dim lValores As List(Of ELL.Valor) = areaBLL.loadListValores(New ELL.Area With {.Id = oItem.Id})
            Dim lIndicadores As List(Of ELL.Indicador) = areaBLL.loadListIndicadores(New ELL.Area With {.Id = oItem.Id})
            lblValores.Text = If(lValores IsNot Nothing, lValores.Count, "0")
            lblIndicadores.Text = If(lIndicadores IsNot Nothing, lIndicadores.Count, "0")
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvAreas, "Select$" + CStr(oItem.Id))
        End If
    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvAreas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAreas.RowCommand
        Try
            If (e.CommandName = "Select") Then
                mostrarDetalleArea(CInt(e.CommandArgument))
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errCompDetalle")
            log.Error("Error al ver el detalle del area", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se intenta registrar un area nueva
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkNuevaArea_Click(sender As Object, e As EventArgs) Handles lnkNuevaArea.Click
        Try
            mostrarDetalleArea(Integer.MinValue)
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errCompDetalle")
            log.Error("Error al ver el detalle del area", ex)
        End Try
    End Sub

#Region "Area Info"

    ''' <summary>
    ''' Muestra el detalle de un area
    ''' </summary>
    ''' <param name="idArea">Id del area</param>    
    Private Sub mostrarDetalleArea(ByVal idArea As Integer)
        inicializarDetalleArea()
        If (idArea > 0) Then
            Dim areaBLL As New BLL.AreasComponent(True, True)
            Dim oArea As ELL.Area = areaBLL.loadArea(idArea)
            btnSaveArea.CommandArgument = oArea.Id
            txtAreaInfo.Text = oArea.Nombre
            btnDeleteArea.Visible = Not ((oArea.Valores IsNot Nothing AndAlso oArea.Valores.Count > 0) OrElse (oArea.Indicadores IsNot Nothing AndAlso oArea.Indicadores.Count > 0))
        End If
        mpeArea.Show()
    End Sub

    ''' <summary>
    ''' Inicializa el detalle del area
    ''' </summary>
    Private Sub inicializarDetalleArea()
        txtAreaInfo.Text = String.Empty
        btnSaveArea.CommandArgument = String.Empty
        btnDeleteArea.Visible = False
        btnDeleteArea.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("confirmarEliminar") & "');"
    End Sub

    ''' <summary>
    ''' Se realiza la paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvAreas_Paging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAreas.PageIndexChanging
        Try
            gvItems.PageIndex = e.NewPageIndex
            cargarAreas(CInt(btnGuardar.CommandArgument))
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Guarda los datos del area
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSaveArea_Click(sender As Object, e As EventArgs) Handles btnSaveArea.Click
        Try
            If (txtNombre.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe introducir todos los datos")
                mpeArea.Show()
            Else
                Dim negBLL As New BLL.AreasComponent
                Dim oItem As New ELL.Area
                If (btnSaveArea.CommandArgument <> String.Empty) Then
                    oItem.Id = CInt(btnSaveArea.CommandArgument)
                Else
                    oItem.IdNegocio = CInt(btnGuardar.CommandArgument) 'El negocio no se actualiza
                End If
                oItem.Nombre = txtAreaInfo.Text
                Dim idItem As Integer = negBLL.SaveArea(oItem)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                If (btnSaveArea.CommandArgument <> String.Empty) Then
                    log.Info("Se ha modificado los datos del area - " & idItem)
                Else
                    log.Info("Se ha insertado un nuevo area - " & idItem)
                End If
                cargarAreas(oItem.IdNegocio) 'Si no existia, se muestran las areas
                mpeArea.Hide()
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
            log.Error("Error al guardar los datos del area", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Borra el area
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnDeleteArea_Click(sender As Object, e As EventArgs) Handles btnDeleteArea.Click
        Try
            Dim negBLL As New BLL.AreasComponent
            Dim idItem As Integer = CInt(btnSaveArea.CommandArgument)
            If (negBLL.DeleteArea(idItem)) Then
                log.Info("Se ha borrado el area " & idItem & " - " & txtNombre.Text)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("elementoBorrado")
                cargarAreas(CInt(btnGuardar.CommandArgument))
            Else
                Master.MensajeError = itzultzaileWeb.Itzuli("errBorrar")
                log.Error("Error al borrar el area " & txtNombre.Text)
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errBorrar")
            log.Error("Error al borrar el area " & txtNombre.Text, ex)
        End Try
    End Sub

#End Region

#End Region

#Region "Vista copiado"

    ''' <summary>
    ''' Inicializa el formulario para copiar un area
    ''' </summary>    
    Private Sub inicializarCopiado()
        mView.ActiveViewIndex = 2
        txtNombreArea.Text = String.Empty
        titCopiado.Texto = itzultzaileWeb.Itzuli("Copiar datos")
        cargarNegocios()
    End Sub

    ''' <summary>
    ''' Carga los negocios
    ''' </summary>
    Private Sub cargarNegocios()
        If (ddlNegOrig.Items.Count = 0) Then
            Dim negBLL As New BLL.NegociosComponent
            Dim lNegocios As List(Of ELL.Negocio) = negBLL.loadListNegocios(New ELL.Negocio)
            If (lNegocios IsNot Nothing) Then lNegocios = lNegocios.OrderBy(Function(o As ELL.Negocio) o.Nombre).ToList
            ddlNegOrig.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
            ddlNegDest.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
            ddlNegOrig.DataSource = lNegocios : ddlNegOrig.DataBind()
            ddlNegDest.DataSource = lNegocios : ddlNegDest.DataBind()

            ddlAreasOrig.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un negocio"), 0))
        Else
            ddlAreasOrig.Items.Clear()
            ddlAreasOrig.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un negocio"), 0))
        End If
        ddlNegOrig.SelectedIndex = -1 : ddlNegDest.SelectedIndex = -1
    End Sub

    ''' <summary>
    ''' Se cargan las areas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlNegOrig_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNegOrig.SelectedIndexChanged
        Try
            If (ddlNegOrig.SelectedValue = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un negocio")
            Else
                ddlAreasOrig.Items.Clear()
                Dim areaBLL As New BLL.AreasComponent
                Dim lAreas As List(Of ELL.Area) = areaBLL.loadListAreas(New ELL.Area With {.IdNegocio = CInt(ddlNegOrig.SelectedValue)})
                If (lAreas IsNot Nothing AndAlso lAreas.Count > 0) Then
                    lAreas = lAreas.OrderBy(Function(o As ELL.Area) o.Nombre).ToList
                    ddlAreasOrig.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
                Else
                    ddlAreasOrig.Items.Add(New ListItem(itzultzaileWeb.Itzuli("No tiene ninguna area asignada"), 0))
                End If
                ddlAreasOrig.DataSource = lAreas : ddlAreasOrig.DataBind()                
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
        End Try
    End Sub

    ''' <summary>
    ''' Se copia el area seleccionada al negocio destino
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnCopiar_Click(sender As Object, e As EventArgs) Handles btnCopiar.Click
        Try
            If (ddlAreasOrig.SelectedValue = 0 Or ddlNegDest.SelectedValue = 0 Or txtNombreArea.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione los datos")
            Else
                Dim areaBLL As New BLL.AreasComponent
                areaBLL.CopiarArea(CInt(ddlAreasOrig.SelectedValue), CInt(ddlNegDest.SelectedValue), txtNombreArea.Text)
                log.Info("Se ha copiado la informacion del area " & ddlAreasOrig.SelectedItem.Text & " al negocio " & ddlNegDest.SelectedItem.Text)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Datos copiados")
                inicializarCopiado()
            End If
        Catch batzEx As SabLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al copiar los datos")
            log.Error("Error al copiar los datos del area", ex)
        End Try
    End Sub

#End Region

End Class