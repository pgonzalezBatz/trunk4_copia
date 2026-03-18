
Partial Public Class SelectorCategoriaProducto
    Inherits System.Web.UI.UserControl

#Region "Propiedades"

    ''' <summary>
    ''' Obtiene la cadena de conexión
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Protected ReadOnly Property CadenaConexion As String
        Get
            Return System.Configuration.ConfigurationManager.ConnectionStrings("BRAIN").ConnectionString
        End Get
    End Property

    ''' <summary>
    ''' Indica si la unidad de medida es valida. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EsValida() As Boolean
        Get
            If (ViewState("EsValida") Is Nothing) Then
                Return False
            Else
                Return CType(ViewState("EsValida"), Boolean)
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("EsValida") = value
        End Set
    End Property

    ''' <summary>
    ''' Para acceder al cuadro de texto del proyecto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CategoriaProducto() As String
        Get
            Return txtCategoriasProducto.Text
        End Get
        Set(ByVal value As String)
            txtCategoriasProducto.Text = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Descripcion de la referencia en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdCategoriaProducto() As String
        Get
            If Not (String.IsNullOrEmpty(hfCategoriasProducto.Value)) Then
                Return hfCategoriasProducto.Value
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            hfCategoriasProducto.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Empresa con la que se va a trabajar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Empresa() As String
        Get
            If Not (String.IsNullOrEmpty(hfEmpresa.Value)) Then
                Return hfEmpresa.Value
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            hfEmpresa.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Habilitar/Deshabilitar el textbox de categoría de producto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CategoriaProducto_Enabled()
        Get
            Return txtCategoriasProducto.Enabled
        End Get
        Set(ByVal value)
            txtCategoriasProducto.Enabled = value
            btnBuscar.Enabled = value
        End Set
    End Property

    ''' <summary>
    ''' Descripcion de la referencia en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RFV_CategoriasProducto()
        Get
            Return rfvCategoriasProducto.Enabled
        End Get
        Set(ByVal value)
            rfvCategoriasProducto.Enabled = value
        End Set
    End Property

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
                ViewState("sortExpresion") = "ELTO"
            End If
            Return ViewState("sortExpresion").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("sortExpresion") = value
        End Set
    End Property

    ''' <summary>
    ''' Evento que se produce al seleccionar una unidad de medida (Al dar al boton aceptar o al salir)
    ''' </summary>
    ''' <remarks></remarks>
    Public Event CategoriaProductoSeleccionado()

#End Region

#Region "Eventos de Pagina"

    ''' <summary>
    ''' Registra un script para que al salir de la caja de texto se compruebe la unidad de medida y al final, se recargue la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        aceCategoriasProducto.OnClientItemSelected = "CategoriaProductoElegida_" + Me.ID
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "CategoriaProductoElegida_" & Me.ID, "function CategoriaProductoElegida_" & Me.ID & "(source, eventArgs){ var hdnValueId = document.getElementById('" & hfCategoriasProducto.ClientID & "'); hdnValueId.value = eventArgs.get_value();}", True)

        txtCategoriasProducto.Attributes.Add("onkeyup", "CategoriasProductoContextKey_" + Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "CategoriasProductoContextKey_" & Me.ID, "function CategoriasProductoContextKey_" & Me.ID & "() {$find('" & aceCategoriasProducto.ClientID & "').set_contextKey('" & Empresa & "');}", True)

        txtCategoriasProducto.Attributes.Add("onkeypress", "comprobarEnter_" & Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobar_" & Me.ID, "function comprobarCategoriaProducto_" & Me.ID & "(){" & Page.ClientScript.GetPostBackEventReference(btnComprobar, Nothing) & "}", True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobarEnter_" & Me.ID, "function comprobarEnter_" & Me.ID & "() {if (event.keyCode == 13) {comprobarCategoriaProducto_" & Me.ID & "();}}", True)

        'txtCategoriasProducto.Attributes.Add("onblur", "comprobarBlur_" & Me.ID & "();")
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobarBlur_" & Me.ID, "function comprobarBlur_" & Me.ID & "() {" & Page.ClientScript.GetPostBackEventReference(btnComprobar, Nothing) & "}", True)

        If Not (Page.IsPostBack) Then CargarGridView()
    End Sub

#End Region

#Region "GridView Cantidad de Unidades de Medida"

    ''' <summary>
    ''' Iniciar el gridview 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_CategoriaProducto_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_CategoriaProducto.Init
        Dim Tabla As GridView = sender
        Tabla.CrearBotones()
        Tabla.PageIndex = 0
    End Sub

    ''' <summary>
    ''' RowDataBound del Gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_CategoriaProducto_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_CategoriaProducto.RowDataBound
        Try
            Dim Fila As GridViewRow = e.Row
            Fila.CrearAccionesFila()
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Cambio de página del gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_CategoriaProducto_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_CategoriaProducto.PageIndexChanging
        gv_CategoriaProducto.PageIndex = e.NewPageIndex
        CargarGridView()
        mpe_SelectorCategoriaProducto.Show()
    End Sub

    ''' <summary>
    ''' Evento surgido al seleccionar una categoría de producto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gv_CategoriaProducto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_CategoriaProducto.SelectedIndexChanged
        Dim Tabla As GridView = sender
        ViewState("IdCategoriaSel") = Tabla.SelectedDataKey.Item("ELTO").ToString.Trim
        ViewState("DescCategoriaSel") = Tabla.SelectedDataKey.Item("DENO_S")

        comprobarCategoriaProducto(New Button With {.CommandName = "A"}, Nothing)
        upSelector.Update()
    End Sub

    ''' <summary>
    ''' Cargar el gridview con el listado de unidades de medida
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarGridView()
        Dim oBrainBLL As New BLL.BrainBLL
        Dim listaCategoriasProducto As List(Of ELL.BrainBase)
        listaCategoriasProducto = oBrainBLL.CargarCategoriasProducto(Empresa)
        Ordenar(listaCategoriasProducto, GridViewSortExpresion, GridViewSortDirection)
        gv_CategoriaProducto.DataSource = listaCategoriasProducto
        gv_CategoriaProducto.DataBind()
    End Sub

#End Region

#Region "Orden"

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_CategoriaProducto_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_CategoriaProducto.Sorting
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

        CargarGridView()
        mpe_SelectorCategoriaProducto.Show()
    End Sub

    ''' <summary>
    ''' Devuelve el tipo de ordenacion que se va a realizar
    ''' </summary>
    ''' <param name="sortDirection">Orden</param>
    ''' <returns>String con el orden a realizar ("ASC o DESC")</returns>
    Private Function GetSortDirection(ByVal sortDirection As SortDirection) As String
        Dim newSortDirection As String = String.Empty
        Select Case sortDirection
            Case sortDirection.Ascending
                newSortDirection = "ASC"
            Case Else
                newSortDirection = "DESC"
        End Select

        Return newSortDirection
    End Function

    ''' <summary>
    ''' Obtiene el indice de una columna
    ''' </summary>
    ''' <param name="sortExp">Expresion de orden</param>
    ''' <returns>Indice</returns>
    Private Function getColumnIndex(ByVal sortExp As String) As Integer
        For index As Integer = 0 To gv_CategoriaProducto.Columns.Count - 1
            If (gv_CategoriaProducto.Columns(index).SortExpression = sortExp And gv_CategoriaProducto.Columns(index).Visible) Then
                Return index
            End If
        Next index
        Return Integer.MinValue
    End Function

    ''' <summary>
    ''' Añade una imagen a la cabecera, indicando si el orden es ascendente o descendente
    ''' </summary>
    ''' <param name="headerRow"></param>
    ''' <remarks></remarks>
    Private Sub AddSortImage(ByVal headerRow As GridViewRow)
        If (GridViewSortExpresion <> String.Empty) Then
            Dim sortExp As String = GridViewSortExpresion
            Dim idCol As Integer = getColumnIndex(sortExp)
            If (idCol <> -1) Then
                Dim sortImage As New Image()
                If (GridViewSortDirection = SortDirection.Ascending) Then
                    sortImage.ImageUrl = "~/App_Themes/Batz/Imagenes/sortascending.gif"
                    sortImage.AlternateText = "ordenAscendente"
                Else
                    sortImage.ImageUrl = "~/App_Themes/Batz/Imagenes/sortdescending.gif"
                    sortImage.AlternateText = "ordenDescendente"
                End If

                headerRow.Cells(idCol).Controls.Add(sortImage)
            End If
        End If
    End Sub

    '''' <summary>
    '''' Ordena la lista de articulos
    '''' </summary>
    '''' <param name="listaCategoriasProducto">Lista de categorías de producto</param>
    '''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    '''' <param name="sortDir">Direccion de ordenacion</param>
    '''' <remarks></remarks>
    Private Sub Ordenar(ByRef listaCategoriasProducto As List(Of ELL.BrainBase), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr
            Case "ELTO"
                listaCategoriasProducto.Sort(Function(o1 As Object, o2 As Object) _
                 If(sortDir = SortDirection.Ascending, o1.ELTO < o2.ELTO, o1.ELTO > o2.ELTO))
            Case "DENO_S"
                listaCategoriasProducto.Sort(Function(o1 As Object, o2 As Object) _
                 If(sortDir = SortDirection.Ascending, o1.DENO_S < o2.DENO_S, o1.DENO_S > o2.DENO_S))
        End Select
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Metodo de Refresco del control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Recargar()
        comprobarCategoriaProducto(btnComprobar, Nothing)
    End Sub

    ''' <summary>
    ''' Boton oculto que comprobará que la categoría del producto sea correcta en dos situaciones:
    '''    -Al perder el foco el texto de la unidad 
    '''    -Al clickear el boton aceptar despues de haber seleccionado una unidad
    ''' </summary>    
    ''' <remarks></remarks>
    Protected Sub comprobarCategoriaProducto(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            If (myButton.CommandName = "A") Then  'Se ha pulsado el boton aceptar
                If (ViewState("IdCategoriaSel") IsNot Nothing AndAlso ViewState("DescCategoriaSel") IsNot Nothing) Then
                    IdCategoriaProducto = ViewState("IdCategoriaSel")
                    CategoriaProducto = ViewState("DescCategoriaSel")
                    EsValida = True

                    txtCategoriasProducto.Text = CategoriaProducto

                    RaiseEvent CategoriaProductoSeleccionado()
                End If
            Else  ' Se ha lanzado este evento, al perder el foco del cuadro de texto de la referencia
                CategoriaProducto = txtCategoriasProducto.Text
                If (txtCategoriasProducto.Text <> String.Empty) Then
                    Dim oBrainBLL As New BLL.BrainBLL
                    Dim categoria As ELL.BrainBase

                    categoria = oBrainBLL.CargarCategoriaProducto(CategoriaProducto.ToLower, Empresa)

                    If (categoria IsNot Nothing) Then
                        IdCategoriaProducto = categoria.ELTO
                        CategoriaProducto = categoria.DENO_S
                        EsValida = True
                        RaiseEvent CategoriaProductoSeleccionado()
                    Else
                        CategoriaProducto = String.Empty
                        IdCategoriaProducto = String.Empty
                        EsValida = False
                    End If
                Else  'Se deja en blanco pero hay que lanzar un evento de referencia no existente
                    CategoriaProducto = String.Empty
                    EsValida = False
                End If
            End If

            'Se limpian las variables del viewstate de Seleccion de nuevas referencias para que entre distintas iteraciones, no de problemas
            ViewState("IdCategoriaSel") = Nothing
            ViewState("DescCategoriaSel") = Nothing

        Catch ex As Exception
            CategoriaProducto = String.Empty
            EsValida = False
        End Try
    End Sub

    ''' <summary>
    ''' Al realizarse el onblur, llama a comprobar referencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnComprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComprobar.Click
        comprobarCategoriaProducto(sender, e)
    End Sub

    ''' <summary>
    ''' Buscar unidad de medida en lugar de escribirlo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBuscar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnBuscar.Click
        Recargar()
        mpe_SelectorCategoriaProducto.Show()
    End Sub
#End Region

End Class