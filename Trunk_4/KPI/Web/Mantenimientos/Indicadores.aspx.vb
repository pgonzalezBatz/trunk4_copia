Public Class Indicadores
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
                If (Request.QueryString("id") IsNot Nothing) Then 'Accedemos al detalle
                    mostrarDetalle(CInt(Request.QueryString("id")))
                End If
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
            itzultzaileWeb.Itzuli(labelArea) : itzultzaileWeb.Itzuli(labelCalculo) : itzultzaileWeb.Itzuli(labelObjetivo)
            itzultzaileWeb.Itzuli(btnOrdenar) : itzultzaileWeb.Itzuli(labelArea) : itzultzaileWeb.Itzuli(lnkConfigurar)
            itzultzaileWeb.Itzuli(labelNegSearch) : itzultzaileWeb.Itzuli(labelNegocio) : itzultzaileWeb.Itzuli(rfvNombre)
            itzultzaileWeb.Itzuli(btnActivar) : itzultzaileWeb.Itzuli(labelAreaResponsable)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa la vista listado
    ''' </summary>    
    Private Sub inicializarListado()
        txtSearch.Text = String.Empty
        cargarNegocios(ddlNegSearch, ddlAreaSearch)
        If (Not Page.IsPostBack AndAlso Session("IndFiltro") IsNot Nothing) Then 'Si es la primera vez y viene con un filtro, lo restauramos            
            Dim filtro As String() = Session("IndFiltro").ToString.Split(",")            
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
                Session("indFiltro") = txtSearch.Text.Trim & "," & ddlNegSearch.SelectedValue & "," & ddlAreaSearch.SelectedValue
            Else
                Session("indFiltro") = Nothing
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
                Dim lItems As List(Of ELL.Indicador) = negBLL.loadListIndicadores(oArea, False)
                Ordenar(lItems, GridViewSortExpresion, GridViewSortDirection)
                hAreas = New Hashtable : hUnidades = New Hashtable : hNegocios=New Hashtable
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
            Dim oItem As ELL.Indicador = e.Row.DataItem
            Dim lblNombre As Label = CType(e.Row.FindControl("lblNombre"), Label)
            Dim lblNegocio As Label = CType(e.Row.FindControl("lblNegocio"), Label)
            Dim lblArea As Label = CType(e.Row.FindControl("lblArea"), Label)
            Dim lblAreaResponsable As Label = CType(e.Row.FindControl("lblAreaResponsable"), Label)
            Dim lblUnidad As Label = CType(e.Row.FindControl("lblUnidad"), Label)
            Dim imgTendencia As Image = CType(e.Row.FindControl("imgTendencia"), Image)
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
            If (hAreas.ContainsKey(oItem.IdAreaResponsable)) Then
                lblAreaResponsable.Text = hAreas(oItem.IdAreaResponsable)
            Else
                Dim areaBLL As New BLL.AreasComponent
                Dim oArea As ELL.Area = areaBLL.loadArea(oItem.IdAreaResponsable)
                lblAreaResponsable.Text = oArea.Nombre
                hAreas.Add(oArea.Id, oArea.Nombre)
            End If
            If (hUnidades.ContainsKey(oItem.IdUnidad)) Then
                lblUnidad.Text = hUnidades(oItem.IdUnidad)
            Else
                Dim unitBLL As New BLL.UnidadesComponent
                Dim oUnit As ELL.Unidad = unitBLL.loadUnidad(oItem.IdUnidad)
                lblUnidad.Text = oUnit.Nombre
                hUnidades.Add(oUnit.Id, oUnit.Nombre)
            End If
            If (oItem.TendenciaObjetivo = ELL.Indicador.Tendencia.Ascendente) Then
                imgTendencia.ImageUrl = "~/App_Themes/Tema1/Images/Ascending_16.png"
            Else
                imgTendencia.ImageUrl = "~/App_Themes/Tema1/Images/Descending_16.png"
            End If
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
            GridViewSortExpresion = If(GridViewSortExpresion Is Nothing, "num_orden", e.SortExpression)

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
    Private Sub Ordenar(ByRef lItems As List(Of ELL.Indicador), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr.ToLower
            Case "nombre"
                lItems.Sort(Function(oItem1 As ELL.Indicador, oItem2 As ELL.Indicador) _
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
        Dim url As String = "OrdenarItemsArea.aspx?Tipo=1"
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
                Dim areaBLL As New BLL.AreasComponent
                Dim oItem As ELL.Indicador = areaBLL.loadIndicador(id)
                Dim oArea As ELL.Area = areaBLL.loadArea(oItem.IdArea)                
                btnGuardar.CommandArgument = oItem.Id
                txtNombre.Text = oItem.Nombre
                txtDescripcion.Text = oItem.Descripcion
                trCalculo.Visible = True
                If (oItem.Calculo.Trim.Length = 0) Then
                    lblCalculo.Text = itzultzaileWeb.Itzuli("Dato no informado")
                    lblCalculo.CssClass = "textoRojo"
                Else
                    lblCalculo.Text = areaBLL.TransformarFormulaCalculo(oItem.Calculo, oArea.IdNegocio)
                    lblCalculo.CssClass = String.Empty
                End If
                ddlNegocios.SelectedIndex = ddlNegocios.Items.IndexOf(ddlNegocios.Items.FindByValue(oArea.IdNegocio))
                cargarAreas(ddlAreas, itzultzaileWeb.Itzuli("Seleccione uno"), oArea.IdNegocio)
                ddlAreas.SelectedIndex = ddlAreas.Items.IndexOf(ddlAreas.Items.FindByValue(oItem.IdArea))
                cargarAreas(ddlAreasRespon, itzultzaileWeb.Itzuli("Seleccione uno"), oArea.IdNegocio)
                ddlAreasRespon.SelectedIndex = ddlAreasRespon.Items.IndexOf(ddlAreasRespon.Items.FindByValue(oItem.IdAreaResponsable))
                ddlUnidades.SelectedIndex = ddlUnidades.Items.IndexOf(ddlUnidades.Items.FindByValue(oItem.IdUnidad))
                ddlTendenciaObj.SelectedValue = oItem.TendenciaObjetivo
                btnActivar.Visible = oItem.Obsoleto
                btnEliminar.Visible = Not oItem.Obsoleto 'areaBLL.CanDeleteIndicador(id)
                For Each item As ListItem In ltbPlantas.Items
                    item.Selected = (oItem.Plantas.Exists(Function(o) o = CInt(item.Value)))
                Next
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
        txtNombre.Text = String.Empty : txtDescripcion.Text = String.Empty
        cargarNegocios(ddlNegocios, ddlAreas)
        cargarNegocios(ddlNegocios, ddlAreasRespon)
        cargarUnidades() : cargarTendencias()
        cargarPlantas()
        btnGuardar.CommandArgument = String.Empty
        titDetalle.Texto = itzultzaileWeb.Itzuli("Detalle")
        trCalculo.Visible = False : lblCalculo.Text = String.Empty
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
    ''' Se cargan sus areas
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
                ddlAreasRespon.Items.Clear()
                ddlAreasRespon.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione un negocio"), 0))
            Else
                cargarAreas(ddlAreas, itzultzaileWeb.Itzuli("Seleccione uno"), CInt(ddlNegocios.SelectedValue))
                cargarAreas(ddlAreasRespon, itzultzaileWeb.Itzuli("Seleccione uno"), CInt(ddlNegocios.SelectedValue))
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
    ''' Carga los tipos de tendencias
    ''' </summary>    
    Private Sub cargarTendencias()
        If (ddlTendenciaObj.Items.Count = 0) Then
            ddlTendenciaObj.Items.Add(New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), -1))
            For Each iTendencia As Integer In [Enum].GetValues(GetType(ELL.Indicador.Tendencia))
                ddlTendenciaObj.Items.Add(New ListItem(itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Indicador.Tendencia), iTendencia)), iTendencia))
            Next
        End If
        ddlTendenciaObj.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' Carga las plantas
    ''' </summary>    
    Private Sub cargarPlantas()
        If (ltbPlantas.Items.Count = 0) Then
            Dim plantBLL As New BLL.PlantasComponent
            ltbPlantas.DataSource = plantBLL.loadListPlantas(New ELL.Planta)
            ltbPlantas.DataBind()
        End If
        'Por defecto, se seleccionan todos
        For Each item As ListItem In ltbPlantas.Items
            item.Selected = True
        Next
    End Sub

    ''' <summary>
    ''' Se guardan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            If (txtNombre.Text = String.Empty OrElse ddlAreas.SelectedIndex = 0 OrElse ddlAreasRespon.SelectedIndex = 0 OrElse ddlUnidades.SelectedIndex = 0 OrElse ddlTendenciaObj.SelectedValue = -1 OrElse ltbPlantas.SelectedIndex = -1) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe introducir todos los datos")
            Else
                Dim negBLL As New BLL.AreasComponent
                Dim oItem As New ELL.Indicador
                If (btnGuardar.CommandArgument <> String.Empty) Then oItem.Id = CInt(btnGuardar.CommandArgument)
                oItem.Nombre = txtNombre.Text
                oItem.Descripcion = txtDescripcion.Text
                oItem.IdArea = CInt(ddlAreas.SelectedValue)
                oItem.IdAreaResponsable = CInt(ddlAreasRespon.SelectedValue)
                oItem.IdUnidad = CInt(ddlUnidades.SelectedValue)
                oItem.TendenciaObjetivo = ddlTendenciaObj.SelectedValue
                oItem.Plantas = New List(Of Integer)
                For Each index As Integer In ltbPlantas.GetSelectedIndices
                    oItem.Plantas.Add(CInt(ltbPlantas.Items(index).Value))
                Next
                Dim idItem As Integer = negBLL.SaveIndicador(oItem)
                If (btnGuardar.CommandArgument <> String.Empty) Then
                    log.Info("Se ha modificado los datos del indicador - " & idItem)
                Else
                    log.Info("Se ha insertado un nuevo indicador - " & idItem)
                End If
                Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                If (trCalculo.Visible) Then 'Si ya se habia informado, volvemos al listado
                    cargarListado()
                Else 'Se redirige a la pagina de configuracion para guardar el calculo
                    Response.Redirect("ConfigCalculo.aspx?idInd=" & idItem, False)
                End If
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
            log.Error("Error al guardar los datos del indicador", ex)
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
            Dim lIndicators As List(Of ELL.Indicador) = negBLL.loadIndicadorsItemExistInFormula(idItem, False)
            If (lIndicators.Count = 0) Then
                Dim resul As Integer = negBLL.DeleteIndicador(idItem)
                If (resul = 0 Or resul = 1) Then
                    If (resul = 0) Then
                        log.Info("Se ha borrado el indicador " & idItem & " - " & txtNombre.Text)
                    Else
                        log.Info("Se ha marcado como obsoleto el indicador " & idItem & " - " & txtNombre.Text)
                    End If
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("elementoBorrado")
                    cargarListado()
                Else
                    Master.MensajeError = itzultzaileWeb.Itzuli("errBorrar")
                    log.Error("Error al borrar el indicador " & txtNombre.Text)
                End If
            Else
                Dim areaBLL As New BLL.AreasComponent
                Dim area As ELL.Area
                Dim info As String = String.Empty
                For Each ind In lIndicators
                    area = areaBLL.loadArea(ind.IdArea)
                    info &= If(info <> String.Empty, "<br />", "") & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- " & ind.Nombre & "(" & area.Nombre & ")"
                Next
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("No se puede borrar el indicador ya que esta contenido en otras areas/indicadores") & ":<br />" & info
                log.Warn("No se puede borrar el indicador ya que esta contenido en otras areas/indicadores:" & info.Replace("&nbsp;", ""))
            End If
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errBorrar")
            log.Error("Error al borrar el indicador " & txtNombre.Text, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se marca como activo el indicador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        Try
            Dim negBLL As New BLL.AreasComponent
            Dim idItem As Integer = CInt(btnGuardar.CommandArgument)
            negBLL.ActivarIndicador(idItem)
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Elemento activado")
            cargarListado()
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al activar")
            log.Error("Error al activar el indicador " & txtNombre.Text, ex)
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
            log.Error("Error al buscar los indicadores al volver del detalle", ex)
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
    ''' Accede a la pagina de configuracion de la formula del calculo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkConfigurar_Click(sender As Object, e As EventArgs) Handles lnkConfigurar.Click
        Response.Redirect("ConfigCalculo.aspx?idInd=" & btnGuardar.CommandArgument, False)
    End Sub

#End Region

End Class