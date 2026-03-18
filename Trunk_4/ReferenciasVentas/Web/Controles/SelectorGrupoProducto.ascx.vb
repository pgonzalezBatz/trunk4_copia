
Partial Public Class SelectorGrupoProducto
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
    ''' Para acceder al cuadro de texto del grupo de producto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GrupoProducto() As String
        Get
            Return txtGrupoProducto.Text
        End Get
        Set(ByVal value As String)
            txtGrupoProducto.Text = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Descripcion del grupo de producto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdGrupoProducto() As String
        Get
            If Not (String.IsNullOrEmpty(hfGrupoProducto.Value)) Then
                Return hfGrupoProducto.Value
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            hfGrupoProducto.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Descripcion del grupo de producto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdGP() As String
        Get
            If Not (String.IsNullOrEmpty(hfIdGP.Value)) Then
                Return hfIdGP.Value
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            hfIdGP.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Habilitar/Deshabilitar el textbox del grupo de producto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GrupoProducto_Enabled()
        Get
            Return txtGrupoProducto.Enabled
        End Get
        Set(ByVal value)
            txtGrupoProducto.Enabled = value
            btnBuscar.Enabled = value
        End Set
    End Property

    ''' <summary>
    ''' Activar/Desactivar el RequiredFieldValidator del grupo de producto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RFV_GrupoProducto()
        Get
            Return rfvGrupoProducto.Enabled
        End Get
        Set(ByVal value)
            rfvGrupoProducto.Enabled = value
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
    ''' Evento que se produce al seleccionar un grupo de producto (Al dar al boton aceptar o al salir)
    ''' </summary>
    ''' <remarks></remarks>
    Public Event GrupoProductoSeleccionado()

#End Region

#Region "Eventos de Pagina"

    ''' <summary>
    ''' Registra un script para que al salir de la caja de texto se compruebe el grupo de material y al final, se recargue la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        aceGrupoProducto.OnClientItemSelected = "GrupoProductoElegido_" + Me.ID
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "GrupoProductoElegido_" & Me.ID, "function GrupoProductoElegido_" & Me.ID & "(source, eventArgs){ var hdnValueId = document.getElementById('" & hfGrupoProducto.ClientID & "'); hdnValueId.value = eventArgs.get_value();}", True)

        txtGrupoProducto.Attributes.Add("onkeypress", "comprobarEnter_" & Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobar_" & Me.ID, "function comprobarGrupoProducto_" & Me.ID & "(){" & Page.ClientScript.GetPostBackEventReference(btnComprobar, Nothing) & "}", True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobarEnter_" & Me.ID, "function comprobarEnter_" & Me.ID & "() {if (event.keyCode == 13) {comprobarGrupoProducto_" & Me.ID & "();}}", True)

        If Not (Page.IsPostBack) Then CargarGridView()
    End Sub

#End Region

#Region "GridView Grupos de Material"

    ''' <summary>
    ''' Iniciar el gridview 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_GrupoProducto_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_GrupoProducto.Init
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
    Private Sub gv_GrupoProducto_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_GrupoProducto.RowDataBound
        Try
            'Dim Fila As GridViewRow = e.Row
            'Fila.CrearAccionesFila()
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gv_GrupoProducto, "Select$" + e.Row.RowIndex.ToString)
        Catch ex As Exception
        End Try
    End Sub

    ' ''' <summary>
    ' ''' Cambio de página del gridview
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub gv_GrupoProducto_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_GrupoProducto.PageIndexChanging
    '    gv_GrupoProducto.PageIndex = e.NewPageIndex
    '    CargarGridView()
    '    mpe_SelectorGrupoProducto.Show()
    'End Sub

    ''' <summary>
    ''' Evento surgido al seleccionar un grupo de producto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gv_GrupoProducto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_GrupoProducto.SelectedIndexChanged
        Dim Tabla As GridView = sender
        ViewState("IdGrupoSel") = Tabla.SelectedDataKey.Item("ELTO").ToString.Trim
        ViewState("DescGrupoSel") = Tabla.SelectedDataKey.Item("DENO_S")
        IdGP = Tabla.SelectedDataKey.Item("CodigoProducto")

        comprobarGrupoProducto(New Button With {.CommandName = "A"}, Nothing)
        upSelector.Update()
    End Sub

    ''' <summary>
    ''' Cargar el gridview con el listado de grupos de producto
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarGridView()
        Dim oBrainBLL As New BLL.BrainBLL
        Dim listaGruposProducto As List(Of ELL.BrainBase)
        listaGruposProducto = oBrainBLL.CargarGruposProducto()
        Ordenar(listaGruposProducto, GridViewSortExpresion, GridViewSortDirection)
        gv_GrupoProducto.DataSource = listaGruposProducto
        gv_GrupoProducto.DataBind()
    End Sub

#End Region

#Region "Orden"

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_GrupoProducto_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_GrupoProducto.Sorting
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
        mpe_SelectorGrupoProducto.Show()
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
        For index As Integer = 0 To gv_GrupoProducto.Columns.Count - 1
            If (gv_GrupoProducto.Columns(index).SortExpression = sortExp And gv_GrupoProducto.Columns(index).Visible) Then
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
    '''' Ordena la lista de grupos de producto
    '''' </summary>
    '''' <param name="listaCategoriasProducto">Lista de grupos de producto</param>
    '''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    '''' <param name="sortDir">Direccion de ordenacion</param>
    '''' <remarks></remarks>
    Private Sub Ordenar(ByRef listaGruposProducto As List(Of ELL.BrainBase), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr
            Case "ELTO"
                listaGruposProducto.Sort(Function(o1 As Object, o2 As Object) _
                 If(sortDir = SortDirection.Ascending, o1.ELTO < o2.ELTO, o1.ELTO > o2.ELTO))
            Case "DENO_S"
                listaGruposProducto.Sort(Function(o1 As Object, o2 As Object) _
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
        comprobarGrupoProducto(btnComprobar, Nothing)
    End Sub

    ''' <summary>
    ''' Boton oculto que comprobará que el grupo de producto sea correcta en dos situaciones:
    '''    -Al perder el foco el texto del grupo 
    '''    -Al clickear el boton aceptar despues de haber seleccionado un grupo
    ''' </summary>    
    ''' <remarks></remarks>
    Protected Sub comprobarGrupoProducto(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            If (myButton.CommandName = "A") Then  'Se ha pulsado el boton aceptar
                If (ViewState("IdGrupoSel") IsNot Nothing AndAlso ViewState("DescGrupoSel") IsNot Nothing) Then
                    IdGrupoProducto = ViewState("IdGrupoSel")
                    GrupoProducto = ViewState("DescGrupoSel")
                    EsValida = True

                    txtGrupoProducto.Text = GrupoProducto

                    RaiseEvent GrupoProductoSeleccionado()
                End If
            Else  ' Se ha lanzado este evento, al perder el foco del cuadro de texto del grupo de producto
                GrupoProducto = txtGrupoProducto.Text
                If (txtGrupoProducto.Text <> String.Empty) Then
                    Dim oBrainBLL As New BLL.BrainBLL
                    Dim grupo As ELL.BrainBase

                    grupo = oBrainBLL.CargarGrupoProducto(GrupoProducto.ToLower)

                    If (grupo IsNot Nothing) Then
                        IdGrupoProducto = grupo.ELTO
                        GrupoProducto = grupo.DENO_S
                        EsValida = True
                        RaiseEvent GrupoProductoSeleccionado()
                    Else
                        GrupoProducto = String.Empty
                        IdGrupoProducto = String.Empty
                        EsValida = False
                    End If
                Else  'Se deja en blanco pero hay que lanzar un evento de grupo no existente
                    GrupoProducto = String.Empty
                    EsValida = False
                End If
            End If

            'Se limpian las variables del viewstate de Seleccion de nuevos grupos para que entre distintas iteraciones, no de problemas
            ViewState("IdCategoriaSel") = Nothing
            ViewState("DescCategoriaSel") = Nothing

        Catch ex As Exception
            GrupoProducto = String.Empty
            EsValida = False
        End Try
    End Sub

    ''' <summary>
    ''' Al realizarse el onblur u onfocus, llama a comprobar grupo de material
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnComprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComprobar.Click
        comprobarGrupoProducto(sender, e)
    End Sub

    ''' <summary>
    ''' Buscar grupo de producto en lugar de escribirlo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBuscar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnBuscar.Click
        Recargar()
        mpe_SelectorGrupoProducto.Show()
    End Sub

#End Region

End Class