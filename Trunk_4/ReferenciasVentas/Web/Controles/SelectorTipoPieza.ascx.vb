
Partial Public Class SelectorTipoPieza
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
    ''' Para acceder al cuadro de texto del tipo de pieza
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TipoPieza() As String
        Get
            Return txtTiposPieza.Text
        End Get
        Set(ByVal value As String)
            txtTiposPieza.Text = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Identificador del tipo de pieza
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdTipoPieza() As String
        Get
            If Not (String.IsNullOrEmpty(hfTiposPieza.Value)) Then
                Return hfTiposPieza.Value
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            hfTiposPieza.Value = value
        End Set
    End Property

    ' ''' <summary>
    ' ''' Empresa con la que se va a trabajar
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property Empresa() As String
    '    Get
    '        If Not (String.IsNullOrEmpty(hfEmpresa.Value)) Then
    '            Return hfEmpresa.Value
    '        Else
    '            Return String.Empty
    '        End If
    '    End Get
    '    Set(ByVal value As String)
    '        hfEmpresa.Value = value
    '    End Set
    'End Property

    ''' <summary>
    ''' Habilitar/Deshabilitar el textbox del tipo de pieza
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TipoPieza_Enabled()
        Get
            Return txtTiposPieza.Enabled
        End Get
        Set(ByVal value)
            txtTiposPieza.Enabled = value
            btnBuscar.Enabled = value
        End Set
    End Property

    ''' <summary>
    ''' Habilitar/Deshabilitar el RequiredFieldValidator de la caja de texto de tipo de pieza
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RFV_TipoPieza()
        Get
            Return rfvTiposPieza.Enabled
        End Get
        Set(ByVal value)
            rfvTiposPieza.Enabled = value
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
    ''' Evento que se produce al seleccionar un tipo de pieza (Al dar al boton aceptar o al salir)
    ''' </summary>
    ''' <remarks></remarks>
    Public Event TipoPiezaSeleccionado()

#End Region

#Region "Eventos de Pagina"

    ''' <summary>
    ''' Registra un script para que al salir de la caja de texto se compruebe el tipo de pieza y al final, se recargue la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        aceTiposPieza.OnClientItemSelected = "TipoPiezaElegido_" + Me.ID
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "TipoPiezaElegido_" & Me.ID, "function TipoPiezaElegido_" & Me.ID & "(source, eventArgs){ var hdnValueId = document.getElementById('" & hfTiposPieza.ClientID & "'); hdnValueId.value = eventArgs.get_value();}", True)

        'txtTiposPieza.Attributes.Add("onkeyup", "TipoPiezaContextKey_" + Me.ID & "();")
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "TipoPiezaContextKey_" & Me.ID, "function TipoPiezaContextKey_" & Me.ID & "() {$find('" & aceTiposPieza.ClientID & "').set_contextKey('" & Empresa & "');}", True)

        txtTiposPieza.Attributes.Add("onkeypress", "comprobarEnter_" & Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobar_" & Me.ID, "function comprobarTipoPieza_" & Me.ID & "(){" & Page.ClientScript.GetPostBackEventReference(btnComprobar, Nothing) & "}", True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobarEnter_" & Me.ID, "function comprobarEnter_" & Me.ID & "() {if (event.keyCode == 13) {comprobarTipoPieza_" & Me.ID & "();}}", True)

        'txtTiposPieza.Attributes.Add("onblur", "comprobarBlur_" & Me.ID & "();")
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
    Private Sub gv_TipoPieza_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_TipoPieza.Init
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
    Private Sub gv_TipoPieza_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_TipoPieza.RowDataBound
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
    Private Sub gv_TipoPieza_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_TipoPieza.PageIndexChanging
        gv_TipoPieza.PageIndex = e.NewPageIndex
        CargarGridView()
        mpe_SelectorTipoPieza.Show()
    End Sub

    ''' <summary>
    ''' Evento surgido al seleccionar una categoría de producto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gv_CategoriaProducto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_TipoPieza.SelectedIndexChanged
        Dim Tabla As GridView = sender
        ViewState("IdTipoSel") = Tabla.SelectedDataKey.Item("ELTO").ToString.Trim
        ViewState("DescTipoSel") = Tabla.SelectedDataKey.Item("DENO_S")

        comprobarTipoPieza(New Button With {.CommandName = "A"}, Nothing)
        upSelector.Update()
    End Sub

    ''' <summary>
    ''' Cargar el gridview con el listado de tipos de pieza
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarGridView()
        Dim oBrainBLL As New BLL.BrainBLL
        Dim listaTiposPieza As List(Of ELL.BrainBase)
        listaTiposPieza = oBrainBLL.CargarTiposPieza()
        'listaTiposPieza = oBrainBLL.CargarTiposPieza(Empresa)
        Ordenar(listaTiposPieza, GridViewSortExpresion, GridViewSortDirection)
        gv_TipoPieza.DataSource = listaTiposPieza
        gv_TipoPieza.DataBind()
    End Sub

#End Region

#Region "Orden"

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_TipoPieza_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_TipoPieza.Sorting
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
        mpe_SelectorTipoPieza.Show()
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
        For index As Integer = 0 To gv_TipoPieza.Columns.Count - 1
            If (gv_TipoPieza.Columns(index).SortExpression = sortExp And gv_TipoPieza.Columns(index).Visible) Then
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
    '''' Ordena la lista de tipos de pieza
    '''' </summary>
    '''' <param name="listaTiposPieza">Lista de tipos de pieza</param>
    '''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    '''' <param name="sortDir">Direccion de ordenacion</param>
    '''' <remarks></remarks>
    Private Sub Ordenar(ByRef listaTiposPieza As List(Of ELL.BrainBase), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr
            Case "ELTO"
                listaTiposPieza.Sort(Function(o1 As Object, o2 As Object) _
                 If(sortDir = SortDirection.Ascending, o1.ELTO < o2.ELTO, o1.ELTO > o2.ELTO))
            Case "DENO_S"
                listaTiposPieza.Sort(Function(o1 As Object, o2 As Object) _
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
        comprobarTipoPieza(btnComprobar, Nothing)
    End Sub

    ''' <summary>
    ''' Boton oculto que comprobará que el tipo de pieza sea correcto en dos situaciones:
    '''    -Al perder el foco el texto del tipo 
    '''    -Al clickear el boton aceptar despues de haber seleccionado un tipo
    ''' </summary>    
    ''' <remarks></remarks>
    Protected Sub comprobarTipoPieza(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            If (myButton.CommandName = "A") Then  'Se ha pulsado el boton aceptar
                If (ViewState("IdTipoSel") IsNot Nothing AndAlso ViewState("DescTipoSel") IsNot Nothing) Then
                    IdTipoPieza = ViewState("IdTipoSel")
                    TipoPieza = ViewState("DescTipoSel")
                    EsValida = True

                    txtTiposPieza.Text = TipoPieza

                    RaiseEvent TipoPiezaSeleccionado()
                End If
            Else  ' Se ha lanzado este evento, al perder el foco del cuadro de texto del tipo de pieza
                TipoPieza = txtTiposPieza.Text
                If (txtTiposPieza.Text <> String.Empty) Then
                    Dim oBrainBLL As New BLL.BrainBLL
                    Dim tipo As ELL.BrainBase

                    'tipo = oBrainBLL.CargarTipoPieza(TipoPieza.ToLower, Empresa)
                    tipo = oBrainBLL.CargarTipoPieza(TipoPieza.ToLower)

                    If (tipo IsNot Nothing) Then
                        IdTipoPieza = tipo.ELTO
                        TipoPieza = tipo.DENO_S
                        EsValida = True
                        RaiseEvent TipoPiezaSeleccionado()
                    Else
                        TipoPieza = String.Empty
                        IdTipoPieza = String.Empty
                        EsValida = False
                    End If
                Else  'Se deja en blanco pero hay que lanzar un evento de tipo no existente
                    TipoPieza = String.Empty
                    EsValida = False
                End If
            End If

            'Se limpian las variables del viewstate de Seleccion de nuevas referencias para que entre distintas iteraciones, no de problemas
            ViewState("IdTipoSel") = Nothing
            ViewState("DescTipoSel") = Nothing
        Catch ex As Exception
            TipoPieza = String.Empty
            EsValida = False
        End Try
    End Sub

    ''' <summary>
    ''' Al realizarse el onblur, llama a comprobar tipo de pieza
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnComprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComprobar.Click
        comprobarTipoPieza(sender, e)
    End Sub

    ''' <summary>
    ''' Buscar unidad de medida en lugar de escribirlo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBuscar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnBuscar.Click
        Recargar()
        mpe_SelectorTipoPieza.Show()
    End Sub
#End Region

End Class