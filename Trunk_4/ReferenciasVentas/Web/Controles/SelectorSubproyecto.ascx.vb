
Partial Public Class SelectorSubproyecto
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
    ''' Para acceder al cuadro de texto del subproyecto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Subproyecto() As String
        Get
            Return txtSubproyecto.Text
        End Get
        Set(ByVal value As String)
            txtSubproyecto.Text = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Id del Subproyecto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdSubproyecto() As String
        Get
            If Not (String.IsNullOrEmpty(hfSubproyecto.Value)) Then
                Return hfSubproyecto.Value
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            hfSubproyecto.Value = value
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
    ''' Habilitar/Deshabilitar el textbox del subproyecto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Subproyecto_Enabled()
        Get
            Return txtSubproyecto.Enabled
        End Get
        Set(ByVal value)
            txtSubproyecto.Enabled = value
            btnBuscar.Enabled = value
        End Set
    End Property

    ''' <summary>
    ''' Activar/Desactivar el RequiredFieldValidator del subproyecto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RFV_Subproyecto()
        Get
            Return rfvSubproyecto.Enabled
        End Get
        Set(ByVal value)
            rfvSubproyecto.Enabled = value
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
    ''' Evento que se produce al seleccionar un subproyecto (Al dar al boton aceptar o al salir)
    ''' </summary>
    ''' <remarks></remarks>
    Public Event SubproyectoSeleccionado()

#End Region

#Region "Eventos de Pagina"

    ''' <summary>
    ''' Registra un script para que al salir de la caja de texto se compruebe el subproyecto y al final, se recargue la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        aceSubproyecto.OnClientItemSelected = "SubproyectoElegido_" + Me.ID
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "SubproyectoElegido_" & Me.ID, "function SubproyectoElegido_" & Me.ID & "(source, eventArgs){ var hdnValueId = document.getElementById('" & hfSubproyecto.ClientID & "'); hdnValueId.value = eventArgs.get_value();}", True)

        txtSubproyecto.Attributes.Add("onkeyup", "SubproyectoContextKey_" + Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "SubproyectoContextKey_" & Me.ID, "function SubproyectoContextKey_" & Me.ID & "() {$find('" & aceSubproyecto.ClientID & "').set_contextKey('" & Empresa & "');}", True)

        txtSubproyecto.Attributes.Add("onkeypress", "comprobarEnter_" & Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobar_" & Me.ID, "function comprobarSubproyecto_" & Me.ID & "(){" & Page.ClientScript.GetPostBackEventReference(btnComprobar, Nothing) & "}", True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobarEnter_" & Me.ID, "function comprobarEnter_" & Me.ID & "() {if (event.keyCode == 13) {comprobarSubproyecto_" & Me.ID & "();}}", True)

        'txtSubproyecto.Attributes.Add("onblur", "comprobarBlur_" & Me.ID & "();")
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobarBlur_" & Me.ID, "function comprobarBlur_" & Me.ID & "() {" & Page.ClientScript.GetPostBackEventReference(btnComprobar, Nothing) & "}", True)

        If Not (Page.IsPostBack) Then CargarGridView()
    End Sub

#End Region

#Region "GridView Subproyectos"

    ''' <summary>
    ''' Iniciar el gridview 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_Subproyecto_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Subproyecto.Init
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
    Private Sub gv_Subproyecto_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Subproyecto.RowDataBound
        Try
            'Dim Fila As GridViewRow = e.Row
            'Fila.CrearAccionesFila()
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gv_Subproyecto, "Select$" + e.Row.RowIndex.ToString)
        Catch ex As Exception
        End Try
    End Sub

    ' ''' <summary>
    ' ''' Cambio de página del gridview
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Private Sub gv_Subproyecto_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Subproyecto.PageIndexChanging
    '    gv_Subproyecto.PageIndex = e.NewPageIndex
    '    CargarGridView()
    '    mpe_SelectorSubproyecto.Show()
    'End Sub

    ''' <summary>
    ''' Evento surgido al seleccionar un grupo de producto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gv_Subproyecto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Subproyecto.SelectedIndexChanged
        Dim Tabla As GridView = sender
        ViewState("IdSubproyectoSel") = Tabla.SelectedDataKey.Item("ELTO").ToString.Trim
        'ViewState("DescSubproyectoSel") = Tabla.SelectedDataKey.Item("DENO_S")
        ViewState("DescSubproyectoSel") = Tabla.SelectedDataKey.Item("ELTO").ToString() + "-" + Tabla.SelectedDataKey.Item("DENO_S").ToString()
        comprobarSubproyecto(New Button With {.CommandName = "A"}, Nothing)
        upSelector.Update()
    End Sub

    ''' <summary>
    ''' Cargar el gridview con el listado de disponentes
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarGridView()
        Dim oBrainBLL As New BLL.BrainBLL
        Dim listaSubproyectos As List(Of ELL.BrainBase)
        listaSubproyectos = oBrainBLL.CargarSubproyectos(Empresa)
        Ordenar(listaSubproyectos, GridViewSortExpresion, GridViewSortDirection)
        gv_Subproyecto.DataSource = listaSubproyectos
        gv_Subproyecto.DataBind()
    End Sub

#End Region

#Region "Orden"

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_Subproyecto_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_Subproyecto.Sorting
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
        mpe_SelectorSubproyecto.Show()
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
        For index As Integer = 0 To gv_Subproyecto.Columns.Count - 1
            If (gv_Subproyecto.Columns(index).SortExpression = sortExp And gv_Subproyecto.Columns(index).Visible) Then
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
    '''' Ordena la lista de subproyectos
    '''' </summary>
    '''' <param name="listaSubproyectos">Lista de subproyectos</param>
    '''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    '''' <param name="sortDir">Direccion de ordenacion</param>
    '''' <remarks></remarks>
    Private Sub Ordenar(ByRef listaSubproyectos As List(Of ELL.BrainBase), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr
            Case "ELTO"
                listaSubproyectos.Sort(Function(o1 As Object, o2 As Object) _
                 If(sortDir = SortDirection.Ascending, o1.ELTO < o2.ELTO, o1.ELTO > o2.ELTO))
            Case "DENO_S"
                listaSubproyectos.Sort(Function(o1 As Object, o2 As Object) _
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
        comprobarSubproyecto(btnComprobar, Nothing)
    End Sub

    ''' <summary>
    ''' Boton oculto que comprobará que el subproyecto sea correcto en dos situaciones:
    '''    -Al perder el foco el texto del subproyecto 
    '''    -Al clickear el boton aceptar despues de haber seleccionado un subproyecto
    ''' </summary>    
    ''' <remarks></remarks>
    Protected Sub comprobarSubproyecto(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            If (myButton.CommandName = "A") Then  'Se ha pulsado el boton aceptar
                If (ViewState("IdSubproyectoSel") IsNot Nothing AndAlso ViewState("DescSubproyectoSel") IsNot Nothing) Then
                    IdSubproyecto = ViewState("IdSubproyectoSel")
                    Subproyecto = ViewState("DescSubproyectoSel")
                    EsValida = True

                    txtSubproyecto.Text = Subproyecto

                    RaiseEvent SubproyectoSeleccionado()
                End If
            Else  ' Se ha lanzado este evento, al perder el foco del cuadro de texto del subproyecto
                Subproyecto = txtSubproyecto.Text
                If (txtSubproyecto.Text <> String.Empty) Then
                    Dim oBrainBLL As New BLL.BrainBLL
                    Dim subpro As ELL.BrainBase

                    subpro = oBrainBLL.CargarSubproyecto(Subproyecto.ToLower, Empresa)

                    If (subpro IsNot Nothing) Then
                        IdSubproyecto = subpro.ELTO
                        Subproyecto = subpro.DENO_S
                        EsValida = True
                        RaiseEvent SubproyectoSeleccionado()
                    Else
                        Subproyecto = String.Empty
                        IdSubproyecto = String.Empty
                        EsValida = False
                    End If
                Else  'Se deja en blanco pero hay que lanzar un evento de disponente no existente
                    Subproyecto = String.Empty
                    EsValida = False
                End If
            End If

            'Se limpian las variables del viewstate de Seleccion de nuevos grupos para que entre distintas iteraciones, no de problemas
            ViewState("IdSubproyectoSel") = Nothing
            ViewState("DescSubproyectoSel") = Nothing

        Catch ex As Exception
            Subproyecto = String.Empty
            IdSubproyecto = String.Empty
            EsValida = False
        End Try
    End Sub

    ''' <summary>
    ''' Al realizarse el onblur u onfocus, llama a comprobar disponente
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnComprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComprobar.Click
        comprobarSubproyecto(sender, e)
    End Sub

    ''' <summary>
    ''' Buscar grupo de producto en lugar de escribirlo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBuscar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnBuscar.Click
        Recargar()
        mpe_SelectorSubproyecto.Show()
    End Sub
#End Region

End Class