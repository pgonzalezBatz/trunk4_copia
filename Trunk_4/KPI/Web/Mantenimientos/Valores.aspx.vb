Public Class Valores
    Inherits PageBase

#Region "Vista listado"

    Private hNegocios As Hashtable = Nothing
    Private hAreas As Hashtable = Nothing
    Private hUnidades As Hashtable = Nothing

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
            log.Error("Error al buscar los valores en el page load", ex)
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
            itzultzaileWeb.Itzuli(labelNombre) : itzultzaileWeb.Itzuli(labelDescr) : itzultzaileWeb.Itzuli(labelUnit)
            itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnEliminar) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(labelArea) : itzultzaileWeb.Itzuli(labelOtrasAreas) : itzultzaileWeb.Itzuli(labelNeg)
            itzultzaileWeb.Itzuli(labelAreas) : itzultzaileWeb.Itzuli(imgAddArea) : itzultzaileWeb.Itzuli(btnOrdenar)
            itzultzaileWeb.Itzuli(labelMetAcum) : itzultzaileWeb.Itzuli(labelArea) : itzultzaileWeb.Itzuli(labelNegSearch)
            itzultzaileWeb.Itzuli(labelNegocio) : itzultzaileWeb.Itzuli(btnActivar)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa la vista listado
    ''' </summary>    
    Private Sub inicializarListado()
        txtSearch.Text = String.Empty
        cargarNegocios(ddlNegSearch, ddlAreaSearch)
        If (Not Page.IsPostBack AndAlso Session("valFiltro") IsNot Nothing) Then 'Si es la primera vez y viene con un filtro, lo restauramos            
            Dim filtro As String() = Session("valFiltro").ToString.Split(",")
            txtSearch.Text = filtro(0)
            ddlNegSearch.SelectedValue = filtro(1)
            If (CInt(filtro(1)) > 0) Then
                cargarAreas(ddlAreaSearch, itzultzaileWeb.Itzuli("Seleccione uno"), CInt(filtro(1)))
                ddlAreaSearch.SelectedValue = filtro(2)
            End If
        End If
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
            If (txtSearch.Text.Trim.Length > 0 OrElse ddlNegSearch.SelectedValue > 0) Then
                Session("valFiltro") = txtSearch.Text.Trim & "," & ddlNegSearch.SelectedValue & "," & ddlAreaSearch.SelectedValue
            Else
                Session("valFiltro") = Nothing
            End If
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
                Dim negBLL As New BLL.AreasComponent
                Dim oArea As New ELL.Area With {.Nombre = txtSearch.Text}
                If (ddlAreaSearch.SelectedValue <> "0") Then oArea.Id = CInt(ddlAreaSearch.SelectedValue)
                If (ddlNegSearch.SelectedValue <> "0") Then oArea.IdNegocio = CInt(ddlNegSearch.SelectedValue)
                Dim lItems As List(Of ELL.Valor) = negBLL.loadListValores(oArea, True, False)
                Ordenar(lItems, GridViewSortExpresion, GridViewSortDirection)
                hAreas = New Hashtable : hUnidades = New Hashtable : hNegocios = New Hashtable
                gvItems.DataSource = lItems
                gvItems.DataBind()
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al cargar el listado", ex)
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
            Dim oItem As ELL.Valor = e.Row.DataItem
            Dim lblNombre As Label = CType(e.Row.FindControl("lblNombre"), Label)
            Dim lblNegocio As Label = CType(e.Row.FindControl("lblNegocio"), Label)
            Dim lblArea As Label = CType(e.Row.FindControl("lblArea"), Label)
            Dim lblUnidad As Label = CType(e.Row.FindControl("lblUnidad"), Label)
            Dim lblMetodoAcum As Label = CType(e.Row.FindControl("lblMetodoAcum"), Label)
            lblNombre.Text = oItem.Nombre
            If (hAreas.ContainsKey(oItem.IdArea)) Then
                lblArea.Text = hAreas(oItem.IdArea)
                lblNegocio.Text = hNegocios(oItem.IdArea)  'Se accede a traves del idArea
            Else
                Dim areaBLL As New BLL.AreasComponent
                Dim negBLL As New BLL.NegociosComponent
                Dim oArea As ELL.Area = areaBLL.loadArea(oItem.IdArea)
                Dim oNeg As ELL.Negocio = negBLL.loadNegocio(oArea.IdNegocio)
                lblArea.Text = oArea.Nombre
                lblNegocio.Text = oNeg.Nombre
                hAreas.Add(oArea.Id, oArea.Nombre)
                hNegocios.Add(oArea.Id, oNeg.Nombre)
            End If
            If (hUnidades.ContainsKey(oItem.IdUnidad)) Then
                lblUnidad.Text = hUnidades(oItem.IdUnidad)
            Else
                Dim unitBLL As New BLL.UnidadesComponent
                Dim oUnit As ELL.Unidad = unitBLL.loadUnidad(oItem.IdUnidad)
                lblUnidad.Text = oUnit.Nombre
                hUnidades.Add(oUnit.Id, oUnit.Nombre)
            End If
            lblMetodoAcum.Text = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Valor.MetodoAcum), oItem.MetodoAcumulado).Replace("_", " "))
            If (oItem.Obsoleto) Then
                e.Row.CssClass = "trRojo"
            End If
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
    Private Sub Ordenar(ByRef lItems As List(Of ELL.Valor), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr.ToLower
            Case "nombre"
                lItems.Sort(Function(oItem1 As ELL.Valor, oItem2 As ELL.Valor) _
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

    ''' <summary>
    ''' Redirige a la pagina para su ordenacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnOrdenar_Click(sender As Object, e As EventArgs) Handles btnOrdenar.Click
        Dim url As String = "OrdenarItemsArea.aspx?Tipo=0"
        If (ddlNegSearch.SelectedValue > 0) Then url &= "&idNeg=" & ddlNegSearch.SelectedValue
        If (ddlAreaSearch.SelectedValue > 0) Then url &= "&idArea=" & ddlAreaSearch.SelectedValue
        Response.Redirect(url)
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
                Dim areaBLL As New BLL.AreasComponent()
                Dim oItem As ELL.Valor = areaBLL.loadValor(id)
                Dim oArea As ELL.Area = areaBLL.loadArea(oItem.IdArea)
                btnGuardar.CommandArgument = oItem.Id                
                txtNombre.Text = oItem.Nombre
                txtDescripcion.Text = oItem.Descripcion
                ddlNegocios.SelectedIndex = ddlNegocios.Items.IndexOf(ddlNegocios.Items.FindByValue(oArea.IdNegocio))
                cargarAreas(ddlAreas, itzultzaileWeb.Itzuli("Seleccione uno"), oArea.IdNegocio)
                ddlAreas.SelectedIndex = ddlAreas.Items.IndexOf(ddlAreas.Items.FindByValue(oItem.IdArea))
                ddlUnidades.SelectedIndex = ddlUnidades.Items.IndexOf(ddlUnidades.Items.FindByValue(oItem.IdUnidad))
                ddlMetAcum.SelectedValue = oItem.MetodoAcumulado
                inicializarOtrasAreas()
                btnActivar.Visible = oItem.Obsoleto
                btnEliminar.Visible = Not oItem.Obsoleto
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar el detalle", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa el detalle
    ''' </summary>    
    Private Sub inicializarDetalle()
        mView.ActiveViewIndex = 1
        txtNombre.Text = String.Empty : txtDescripcion.Text = String.Empty
        pnlOtrasAreas.Visible = False
        cargarUnidades() : cargarMetodosAcum()
        cargarNegocios(ddlNegocios, ddlAreas)
        titDetalle.Texto = itzultzaileWeb.Itzuli("Detalle")
        btnGuardar.CommandArgument = String.Empty
        btnEliminar.Visible = False : btnActivar.Visible = False
        btnEliminar.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("confirmarEliminar") & "');"
    End Sub

    ''' <summary>
    ''' Carga los negocios
    ''' </summary>    
    ''' <param name="dropdown">Desplegable donde va a insertar los datos</param>
    ''' <param name="dropdownAreas">Desplegable del area a modificar</param>
    Private Sub cargarNegocios(ByVal dropdown As DropDownList, ByVal dropdownAreas As DropDownList)
        If (dropdown.Items.Count = 0) Then
            Dim negBLL As New BLL.NegociosComponent
            Dim lNegocios As List(Of ELL.Negocio) = negBLL.loadListNegocios(New ELL.Negocio)
            If (lNegocios IsNot Nothing) Then lNegocios.Sort(Function(o1 As ELL.Negocio, o2 As ELL.Negocio) o1.Nombre < o2.Nombre)
            dropdown.DataSource = lNegocios
            dropdown.DataBind()
            dropdown.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
        End If
        dropdown.SelectedIndex = -1

        dropdownAreas.Items.Clear()
        dropdownAreas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un negocio"), 0))
    End Sub

    ''' <summary>
    ''' Se cargan sus areas de busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlNegSearch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNegSearch.SelectedIndexChanged
        Try
            If (ddlNegSearch.SelectedValue = 0) Then
                ddlAreaSearch.Items.Clear()
                ddlAreaSearch.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un negocio"), 0))
            Else
                cargarAreas(ddlAreaSearch, itzultzaileWeb.Itzuli("Seleccione uno"), CInt(ddlNegSearch.SelectedValue))
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
        End Try
    End Sub

    ''' <summary>
    ''' Se cargan sus areas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlNegocios_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNegocios.SelectedIndexChanged
        Try
            If (ddlNegocios.SelectedValue = 0) Then
                ddlAreas.Items.Clear()
                ddlAreas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un negocio"), 0))
            Else
                cargarAreas(ddlAreas, itzultzaileWeb.Itzuli("Seleccione uno"), CInt(ddlNegocios.SelectedValue))
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
        End Try
    End Sub

    ''' <summary>
    ''' Carga las areas
    ''' </summary>    
    ''' <param name="dropdown">Desplegable donde va a insertar los datos</param>
    ''' <param name="textFirstItem">Texto del primer elemento</param>
    ''' <param name="idNegocio">Parametro opcional para especificar el negocio a filtrar</param>
    Private Sub cargarAreas(ByVal dropdown As DropDownList, ByVal textFirstItem As String, Optional ByVal idNegocio As Integer = Integer.MinValue)
        dropdown.Items.Clear()
        Dim areaBLL As New BLL.AreasComponent
        Dim lAreas As List(Of ELL.Area) = areaBLL.loadListAreas(New ELL.Area With {.IdNegocio = idNegocio})
        If (lAreas IsNot Nothing) Then
            lAreas.Sort(Function(o1 As ELL.Area, o2 As ELL.Area) o1.Nombre < o2.Nombre)
            lAreas.Insert(0, New ELL.Area With {.Id = 0, .Nombre = textFirstItem})
        End If
        dropdown.DataSource = lAreas
        dropdown.DataBind()
        dropdown.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Carga las unidades
    ''' </summary>    
    Private Sub cargarUnidades()
        If (ddlUnidades.Items.Count = 0) Then
            Dim unitBLL As New BLL.UnidadesComponent
            Dim lUnidades As List(Of ELL.Unidad) = unitBLL.loadListUnidades(New ELL.Unidad)
            If (lUnidades IsNot Nothing) Then
                lUnidades.Sort(Function(o1 As ELL.Unidad, o2 As ELL.Unidad) o1.Nombre < o2.Nombre)
                lUnidades.Insert(0, New ELL.Unidad With {.Id = 0, .Nombre = itzultzaileWeb.Itzuli("Seleccione uno")})
            End If
            ddlUnidades.DataSource = lUnidades
            ddlUnidades.DataBind()
        End If
        ddlUnidades.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Carga los metodos de acumulado
    ''' </summary>    
    Private Sub cargarMetodosAcum()
        If (ddlMetAcum.Items.Count = 0) Then
            Dim lItems As New List(Of ListItem)
            ddlMetAcum.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), -1))
            For Each iMetodo As Integer In [Enum].GetValues(GetType(ELL.Valor.MetodoAcum))
                lItems.Add(New ListItem(itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Valor.MetodoAcum), iMetodo).Replace("_", " ")), iMetodo))
            Next
            lItems.Sort(Function(o1, o2) o1.Text < o2.Text)
            For Each item In lItems
                ddlMetAcum.Items.Add(item)
            Next
        End If
        ddlMetAcum.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Se guardan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            If (txtNombre.Text = String.Empty Or ddlAreas.SelectedIndex = 0 Or ddlUnidades.SelectedIndex = 0 Or ddlMetAcum.SelectedIndex = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe introducir todos los datos")
            Else
                Dim negBLL As New BLL.AreasComponent
                Dim oItem As New ELL.Valor
                If (btnGuardar.CommandArgument <> String.Empty) Then oItem.Id = CInt(btnGuardar.CommandArgument)
                oItem.Nombre = txtNombre.Text
                oItem.Descripcion = txtDescripcion.Text
                oItem.IdArea = CInt(ddlAreas.SelectedValue)
                oItem.IdUnidad = CInt(ddlUnidades.SelectedValue)
                oItem.MetodoAcumulado = CInt(ddlMetAcum.SelectedValue)
                Dim idItem As Integer = negBLL.SaveValor(oItem)
                If (btnGuardar.CommandArgument <> String.Empty) Then
                    log.Info("Se ha modificado los datos del valor - " & idItem)
                Else
                    log.Info("Se ha insertado un nuevo valor - " & idItem)
                End If
                Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                If (btnGuardar.CommandArgument = String.Empty) Then 'Si es nuevo, se muestran las otras areas, sino, se vuelve
                    btnGuardar.CommandArgument = idItem
                    inicializarOtrasAreas()
                Else
                    Volver(True)
                End If
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
            log.Error("Error al guardar los datos del valor", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se elimina el item. Solo se podra si no esta enlazado con ningun elemento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Try
            Dim negBLL As New BLL.AreasComponent
            Dim idItem As Integer = CInt(btnGuardar.CommandArgument)
            Dim lIndicators As List(Of ELL.Indicador) = negBLL.loadIndicadorsItemExistInFormula(idItem, True)
            If (lIndicators.Count = 0) Then
                Dim resul As Integer = negBLL.DeleteValor(idItem)
                If (resul = 0 Or resul = 1) Then
                    If (resul = 0) Then
                        log.Info("Se ha borrado el valor " & idItem & " - " & txtNombre.Text)
                    Else
                        log.Info("Se ha marcado como obsoleto el valor " & idItem & " - " & txtNombre.Text)
                    End If
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("elementoBorrado")
                    cargarListado()
                Else
                    Master.MensajeError = itzultzaileWeb.Itzuli("errBorrar")
                    log.Error("Error al borrar el valor " & txtNombre.Text)
                End If
            Else
                Dim areaBLL As New BLL.AreasComponent
                Dim area As ELL.Area
                Dim info As String = String.Empty
                For Each ind In lIndicators
                    area = areaBLL.loadArea(ind.IdArea)
                    info &= If(info <> String.Empty, "<br />", "") & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- " & ind.Nombre & "(" & area.Nombre & ")"
                Next
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No se puede borrar el valor ya que esta contenido en la formula de los siguientes indicadores") & ":<br />" & info
                log.Warn("No se puede borrar el valor ya que esta contenido la formula de los siguientes indicadores:" & info.Replace("&nbsp;", ""))
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errBorrar")
            log.Error("Error al borrar el valor " & txtNombre.Text, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se marca como activo el valor
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        Try
            Dim negBLL As New BLL.AreasComponent
            Dim idItem As Integer = CInt(btnGuardar.CommandArgument)
            negBLL.ActivarValor(idItem)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Elemento activado")
            cargarListado()
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al activar")
            log.Error("Error al activar el valor " & txtNombre.Text, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Try
            Volver(True) 'Siempre se recarga porque al crear un item nuevo, no se vuelve al listado
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errMostrarListado")
            log.Error("Error al buscar los valores al volver del detalle", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>    
    ''' <param name="reload">Indica si se recargara el listado</param>
    Private Sub Volver(ByVal reload As Boolean)
        cargarListado(reload)
    End Sub

#Region "Otras areas"

    ''' <summary>
    ''' Inicializa el listado de otras areas
    ''' </summary>    
    Private Sub inicializarOtrasAreas()
        Dim idValor As Integer = CInt(btnGuardar.CommandArgument)
        pnlOtrasAreas.Visible = True
        cargarDesplegableNegocios()
        cargarOtrasAreas(idValor)
        'If (gvOtrasAreas.Rows.Count = 0) Then 'Si no tiene otras areas, habra que calcularlo
        '    Dim areaBLL As New BLL.AreasComponent()
        '    btnEliminar.Visible = areaBLL.CanDeleteValor(idValor)
        'Else 'Si tiene elementos nos se podra borrar
        '    btnEliminar.Visible = False
        'End If
    End Sub

    ''' <summary>
    ''' Se selecciona un negocio. Se carga las areas correspondientes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlNeg_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNeg.SelectedIndexChanged
        Try
            If (ddlNeg.SelectedValue = "0") Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione un negocio")
            Else
                cargarDesplegableAreasNegocio(CInt(ddlNeg.SelectedValue))
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
        End Try
    End Sub

    ''' <summary>
    ''' Se añade una area extra al valor
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub imgAddArea_Click(sender As Object, e As ImageClickEventArgs) Handles imgAddArea.Click
        Try
            If (ddlNeg.SelectedIndex > 0 AndAlso ddlOtrasAreas.SelectedIndex > 0) Then
                Dim idValor As Integer = CInt(btnGuardar.CommandArgument)
                Dim negBLL As New BLL.AreasComponent
                Dim resul As Integer = negBLL.AddAreaValor(CInt(ddlOtrasAreas.SelectedValue), idValor)
                If (resul = 0) Then
                    log.Info("Se ha añadido el acceso del valor " & txtNombre.Text & " al area " & ddlOtrasAreas.SelectedItem.Text)
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                    inicializarOtrasAreas()
                Else
                    Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("El elemento ya existe")
                End If
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe introducir todos los datos")
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
            log.Error("Error al añadir un area de un valor", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga las areas añadidas al valor
    ''' </summary>    
    ''' <param name="idValor">Id del valor</param>
    Private Sub cargarOtrasAreas(ByVal idValor As Integer)
        Dim areasBLL As New BLL.AreasComponent
        Dim lAreas As List(Of ELL.Area) = areasBLL.loadListAreasValor(idValor)
        If (lAreas IsNot Nothing) Then lAreas.Sort(Function(o1 As ELL.Area, o2 As ELL.Area) o1.Nombre < o2.Nombre)
        hNegocios = New Hashtable
        gvOtrasAreas.DataSource = lAreas
        gvOtrasAreas.DataBind()
    End Sub

    ''' <summary>
    ''' Carga los negocios menos el del detalle
    ''' </summary>    
    Private Sub cargarDesplegableNegocios()
        If (ddlNeg.Items.Count = 0) Then
            Dim negBLL As New BLL.NegociosComponent
            Dim lNeg As List(Of ELL.Negocio) = negBLL.loadListNegocios(New ELL.Negocio)
            If (lNeg IsNot Nothing) Then lNeg.Sort(Function(o1 As ELL.Negocio, o2 As ELL.Negocio) o1.Nombre < o2.Nombre)
            ddlNeg.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), "0"))
            ddlNeg.DataSource = lNeg : ddlNeg.DataBind()
        End If
        ddlNeg.SelectedIndex = -1

        ddlOtrasAreas.Items.Clear()
        ddlOtrasAreas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un negocio"), "0"))
    End Sub

    ''' <summary>
    ''' Carga los areas de los negocios
    ''' </summary>    
    Private Sub cargarDesplegableAreasNegocio(ByVal idNegocio As Integer)
        ddlOtrasAreas.Items.Clear()
        Dim areasBLL As New BLL.AreasComponent
        Dim lAreas As List(Of ELL.Area) = areasBLL.loadListAreas(New ELL.Area With {.IdNegocio = idNegocio})
        If (lAreas IsNot Nothing) Then
            lAreas.Sort(Function(o1 As ELL.Area, o2 As ELL.Area) o1.Nombre < o2.Nombre)
            lAreas.Remove(lAreas.Find(Function(o As ELL.Area) o.Id = CInt(ddlAreas.SelectedValue)))
        End If
        ddlOtrasAreas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), "0"))
        ddlOtrasAreas.DataSource = lAreas : ddlOtrasAreas.DataBind()
        ddlOtrasAreas.SelectedIndex = -1
    End Sub

    ''' <summary>
    ''' Evento que surge cuando se enlazan los items
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvOtrasAreas_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOtrasAreas.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim oItem As ELL.Area = e.Row.DataItem
            Dim lblNegocio As Label = CType(e.Row.FindControl("lblNegocio"), Label)
            Dim imgDelArea As ImageButton = CType(e.Row.FindControl("imgDelArea"), ImageButton)
            itzultzaileWeb.Itzuli(imgDelArea)
            imgDelArea.CommandArgument = oItem.Id
            If (hNegocios.ContainsKey(oItem.IdNegocio)) Then
                lblNegocio.Text = hNegocios(oItem.IdNegocio)
            Else
                Dim negBLL As New BLL.NegociosComponent
                Dim oNeg As ELL.Negocio = negBLL.loadNegocio(oItem.IdNegocio)
                lblNegocio.Text = oNeg.Nombre
                hNegocios.Add(oNeg.Id, oNeg.Nombre)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Se ha pinchado en el icono para borrar el area
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub imgDelArea_Click(sender As Object, e As ImageClickEventArgs)
        Try
            Dim img As ImageButton = CType(sender, ImageButton)
            Dim areaBLL As New BLL.AreasComponent
            Dim idValor As Integer = CInt(btnGuardar.CommandArgument)
            Dim idArea As Integer = CInt(img.CommandArgument)
            areaBLL.DeleteAreaValor(idArea, idValor)
            log.Info("Se ha quitado el acceso del valor " & idValor & " al area " & idArea)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("elementoBorrado")
            inicializarOtrasAreas()
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errBorrar")
            log.Error("Error al borrar el acceso del valor del area", ex)
        End Try
    End Sub

#End Region

#End Region

End Class