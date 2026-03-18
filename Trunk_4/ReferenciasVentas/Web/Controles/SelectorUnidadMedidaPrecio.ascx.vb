
Partial Public Class SelectorUnidadMedidaPrecio
    Inherits System.Web.UI.UserControl

#Region "Propiedades"

    ''' <summary>
    ''' Obtiene la cadena de conexión
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Protected ReadOnly Property CadenaConexion As String
        Get
            'Dim status As String = "BRAINLIVE"
            'If (System.Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BRAINLIVE"
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
    Public Property UnidadMedidaPrecio() As String
        Get
            Return txtUnidadMedidaCantidad.Text
        End Get
        Set(ByVal value As String)
            txtUnidadMedidaCantidad.Text = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Descripcion de la referencia en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdUnidadMedidaPrecio() As String
        Get
            If Not (String.IsNullOrEmpty(hfUnidadMedidaCantidad.Value)) Then
                Return hfUnidadMedidaCantidad.Value
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            hfUnidadMedidaCantidad.Value = value
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
    ''' Habilitar/Deshabilitar el textbox de unidad de medida precio
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UnidadMedidaPrecio_Enabled()
        Get
            Return txtUnidadMedidaCantidad.Enabled
        End Get
        Set(ByVal value)
            txtUnidadMedidaCantidad.Enabled = value
            btnBuscar.Enabled = value
        End Set
    End Property

    ''' <summary>
    ''' Descripcion de la referencia en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RFV_UnidadMedidaCantidad()
        Get
            Return rfvUnidadMedidaCantidad.Enabled
        End Get
        Set(ByVal value)
            rfvUnidadMedidaCantidad.Enabled = value
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
    Public Event UnidadMedidaPrecioSeleccionado()

#End Region

#Region "Eventos de Pagina"

    ''' <summary>
    ''' Registra un script para que al salir de la caja de texto se compruebe la unidad de medida y al final, se recargue la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        aceUnidadMedidaCantidad.OnClientItemSelected = "UnidadMedidaPrecioElegida_" + Me.ID
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "UnidadMedidaPrecioElegida_" & Me.ID, "function UnidadMedidaPrecioElegida_" & Me.ID & "(source, eventArgs){ var hdnValueId = document.getElementById('" & hfUnidadMedidaCantidad.ClientID & "'); hdnValueId.value = eventArgs.get_value();}", True)

        txtUnidadMedidaCantidad.Attributes.Add("onkeyup", "UnidadMedidaCantidadContextKey_" + Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "UnidadMedidaCantidadContextKey_" & Me.ID, "function UnidadMedidaCantidadContextKey_" & Me.ID & "() {$find('" & aceUnidadMedidaCantidad.ClientID & "').set_contextKey('" & Empresa & "');}", True)

        txtUnidadMedidaCantidad.Attributes.Add("onkeypress", "comprobarEnter_" & Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobar_" & Me.ID, "function comprobarUnidadMedida_" & Me.ID & "(){" & Page.ClientScript.GetPostBackEventReference(btnComprobar, Nothing) & "}", True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobarEnter_" & Me.ID, "function comprobarEnter_" & Me.ID & "() {if (event.keyCode == 13) {comprobarUnidadMedida_" & Me.ID & "();}}", True)

        'txtUnidadMedidaCantidad.Attributes.Add("onblur", "comprobarBlur_" & Me.ID & "();")
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
    Private Sub gv_UnidadMedida_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_UnidadMedida.Init
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
    Private Sub gv_UnidadMedida_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_UnidadMedida.RowDataBound
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
    Private Sub gv_UnidadMedida_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_UnidadMedida.PageIndexChanging
        gv_UnidadMedida.PageIndex = e.NewPageIndex
        mpe_SelectorUnidadMedida.Show()
    End Sub

    ''' <summary>
    ''' Evento surgido al seleccionar una unidad de medida
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gv_UnidadMedida_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_UnidadMedida.SelectedIndexChanged
        Dim Tabla As GridView = sender
        ViewState("IdUnidadSel") = Tabla.SelectedDataKey.Item("ELTO").ToString.Trim
        ViewState("DescUnidadSel") = Tabla.SelectedDataKey.Item("DENO_S")

        ComprobarUnidadMedida(New Button With {.CommandName = "A"}, Nothing)
        upSelector.Update()
    End Sub

    ''' <summary>
    ''' Cargar el gridview con el listado de unidades de medida
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarGridView()
        Dim oBrainBLL As New BLL.BrainBLL
        Dim listaUnidadesMedida As List(Of ELL.BrainBase)
        listaUnidadesMedida = oBrainBLL.CargarUnidadesMedida(Empresa)
        Ordenar(listaUnidadesMedida, GridViewSortExpresion, GridViewSortDirection)
        gv_UnidadMedida.DataSource = listaUnidadesMedida
        gv_UnidadMedida.DataBind()
    End Sub

#End Region

#Region "Orden"

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_UnidadMedida_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_UnidadMedida.Sorting
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
        mpe_SelectorUnidadMedida.Show()
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
        For index As Integer = 0 To gv_UnidadMedida.Columns.Count - 1
            If (gv_UnidadMedida.Columns(index).SortExpression = sortExp And gv_UnidadMedida.Columns(index).Visible) Then
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
    '''' <param name="lArticulos">Lista de articulos</param>
    '''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    '''' <param name="sortDir">Direccion de ordenacion</param>
    '''' <remarks></remarks>
    Private Sub Ordenar(ByRef listaUnidadesMedida As List(Of ELL.BrainBase), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr
            Case "ELTO"
                listaUnidadesMedida.Sort(Function(o1 As Object, o2 As Object) _
                 If(sortDir = SortDirection.Ascending, o1.ELTO < o2.ELTO, o1.ELTO > o2.ELTO))
            Case "DENO_S"
                listaUnidadesMedida.Sort(Function(o1 As Object, o2 As Object) _
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
        ComprobarUnidadMedida(btnComprobar, Nothing)
    End Sub

    ''' <summary>
    ''' Boton oculto que comprobará que la unidad de medida sea correcta en dos situaciones:
    '''    -Al perder el foco el texto de la unidad 
    '''    -Al clickear el boton aceptar despues de haber seleccionado una unidad
    ''' </summary>    
    ''' <remarks></remarks>
    Protected Sub ComprobarUnidadMedida(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            If (myButton.CommandName = "A") Then  'Se ha pulsado el boton aceptar
                If (ViewState("IdUnidadSel") IsNot Nothing AndAlso ViewState("DescUnidadSel") IsNot Nothing) Then
                    IdUnidadMedidaPrecio = ViewState("IdUnidadSel")
                    UnidadMedidaPrecio = ViewState("DescUnidadSel")
                    EsValida = True

                    txtUnidadMedidaCantidad.Text = UnidadMedidaPrecio

                    RaiseEvent UnidadMedidaPrecioSeleccionado()
                End If
            Else  ' Se ha lanzado este evento, al perder el foco del cuadro de texto de la referencia
                UnidadMedidaPrecio = txtUnidadMedidaCantidad.Text
                If (txtUnidadMedidaCantidad.Text <> String.Empty) Then
                    Dim oBrainBLL As New BLL.BrainBLL
                    Dim unidadMedida As ELL.BrainBase

                    unidadMedida = oBrainBLL.CargarUnidadMedida(UnidadMedidaPrecio.ToLower, Empresa)

                    If (unidadMedida IsNot Nothing) Then
                        IdUnidadMedidaPrecio = unidadMedida.ELTO
                        UnidadMedidaPrecio = unidadMedida.DENO_S
                        EsValida = True
                        RaiseEvent UnidadMedidaPrecioSeleccionado()
                    Else
                        UnidadMedidaPrecio = String.Empty
                        IdUnidadMedidaPrecio = String.Empty
                        EsValida = False
                    End If
                Else  'Se deja en blanco pero hay que lanzar un evento de referencia no existente
                    UnidadMedidaPrecio = String.Empty
                    EsValida = False
                End If
            End If

            'Se limpian las variables del viewstate de Seleccion de nuevas referencias para que entre distintas iteraciones, no de problemas
            ViewState("IdUnidadSel") = Nothing
            ViewState("DescUnidadSel") = Nothing

        Catch ex As Exception
            UnidadMedidaPrecio = String.Empty
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
        ComprobarUnidadMedida(sender, e)
    End Sub

    ''' <summary>
    ''' Buscar unidad de medida en lugar de escribirlo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBuscar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnBuscar.Click
        Recargar()
        mpe_SelectorUnidadMedida.Show()
    End Sub
#End Region

#Region "Funciones y Procesos"

#End Region

End Class