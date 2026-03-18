
Partial Public Class SelectorAlmacen
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
    ''' Indica si el almacen es valido. Guardada en session
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
    ''' Para acceder al cuadro de texto del almcen
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Almacen() As String
        Get
            Return txtAlmacen.Text
        End Get
        Set(ByVal value As String)
            txtAlmacen.Text = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Identificador del almacen
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdAlmacen() As String
        Get
            If Not (String.IsNullOrEmpty(hfAlmacen.Value)) Then
                Return hfAlmacen.Value
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            hfAlmacen.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Habilitar/Deshabilitar el textbox de almacén
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Almacen_Enabled()
        Get
            Return txtAlmacen.Enabled
        End Get
        Set(ByVal value)
            txtAlmacen.Enabled = value
            btnBuscar.Enabled = value
        End Set
    End Property

    ''' <summary>
    ''' Activar/Desactivar el RequiredFieldValidator del almacen
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RFV_Almacen()
        Get
            Return rfvAlmacen.Enabled
        End Get
        Set(ByVal value)
            rfvAlmacen.Enabled = value
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
    ''' Evento que se produce al seleccionar un almacén (Al dar al boton aceptar o al salir)
    ''' </summary>
    ''' <remarks></remarks>
    Public Event AlmacenSeleccionado()

#End Region

#Region "Eventos de Pagina"

    ''' <summary>
    ''' Registra un script para que al salir de la caja de texto se compruebe el almacen y al final, se recargue la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        aceAlmacen.OnClientItemSelected = "AlmacenElegido_" + Me.ID
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "AlmacenElegido_" & Me.ID, "function AlmacenElegido_" & Me.ID & "(source, eventArgs){ var hdnValueId = document.getElementById('" & hfAlmacen.ClientID & "'); hdnValueId.value = eventArgs.get_value();}", True)

        txtAlmacen.Attributes.Add("onkeyup", "AlmacenContextKey_" + Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "AlmacenContextKey_" & Me.ID, "function AlmacenContextKey_" & Me.ID & "() {$find('" & aceAlmacen.ClientID & "').set_contextKey('" & Empresa & "');}", True)

        txtAlmacen.Attributes.Add("onkeypress", "comprobarEnter_" & Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobar_" & Me.ID, "function comprobarAlmacen_" & Me.ID & "(){" & Page.ClientScript.GetPostBackEventReference(btnComprobar, Nothing) & "}", True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobarEnter_" & Me.ID, "function comprobarEnter_" & Me.ID & "() {if (event.keyCode == 13) {comprobarAlmacen_" & Me.ID & "();}}", True)

        'txtAlmacen.Attributes.Add("onblur", "comprobarBlur_" & Me.ID & "();")
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobarBlur_" & Me.ID, "function comprobarBlur_" & Me.ID & "() {" & Page.ClientScript.GetPostBackEventReference(btnComprobar, Nothing) & "}", True)

        If Not (Page.IsPostBack) Then CargarGridView()
    End Sub

#End Region

#Region "GridView Almacenes"

    ''' <summary>
    ''' Iniciar el gridview 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_Almacen_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Almacen.Init
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
    Private Sub gv_Almacen_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Almacen.RowDataBound
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
    Private Sub gv_Almacen_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Almacen.PageIndexChanging
        gv_Almacen.PageIndex = e.NewPageIndex
        CargarGridView()
        mpe_SelectorAlmacen.Show()
    End Sub

    ''' <summary>
    ''' Evento surgido al seleccionar un almacen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gv_Almacen_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Almacen.SelectedIndexChanged
        Dim Tabla As GridView = sender
        ViewState("IdAlmacenSel") = Tabla.SelectedDataKey.Item("ELTO").ToString.Trim
        ViewState("DescAlmacenSel") = Tabla.SelectedDataKey.Item("DENO_S")

        comprobarAlmacen(New Button With {.CommandName = "A"}, Nothing)
        upSelector.Update()
    End Sub

    ''' <summary>
    ''' Cargar el gridview con el listado de almacenes
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarGridView()
        Dim oBrainBLL As New BLL.BrainBLL
        Dim listaAlmacenes As List(Of ELL.BrainBase)
        listaAlmacenes = oBrainBLL.CargarAlmacenes(Empresa)
        Ordenar(listaAlmacenes, GridViewSortExpresion, GridViewSortDirection)
        gv_Almacen.DataSource = listaAlmacenes
        gv_Almacen.DataBind()
    End Sub

#End Region

#Region "Orden"

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_Almacen_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_Almacen.Sorting
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
        mpe_SelectorAlmacen.Show()
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
        For index As Integer = 0 To gv_Almacen.Columns.Count - 1
            If (gv_Almacen.Columns(index).SortExpression = sortExp And gv_Almacen.Columns(index).Visible) Then
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
    '''' Ordena la lista de almacenes
    '''' </summary>
    '''' <param name="listaAlmacenes">Lista de almacenes</param>
    '''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    '''' <param name="sortDir">Direccion de ordenacion</param>
    '''' <remarks></remarks>
    Private Sub Ordenar(ByRef listaAlmacenes As List(Of ELL.BrainBase), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr
            Case "ELTO"
                listaAlmacenes.Sort(Function(o1 As Object, o2 As Object) _
                 If(sortDir = SortDirection.Ascending, o1.ELTO < o2.ELTO, o1.ELTO > o2.ELTO))
            Case "DENO_S"
                listaAlmacenes.Sort(Function(o1 As Object, o2 As Object) _
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
        comprobarAlmacen(btnComprobar, Nothing)
    End Sub

    ''' <summary>
    ''' Boton oculto que comprobará que el almacen sea correcta en dos situaciones:
    '''    -Al perder el foco el texto del almacen 
    '''    -Al clickear el boton aceptar despues de haber seleccionado un almacen
    ''' </summary>    
    ''' <remarks></remarks>
    Protected Sub comprobarAlmacen(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            If (myButton.CommandName = "A") Then  'Se ha pulsado el boton aceptar
                If (ViewState("IdAlmacenSel") IsNot Nothing AndAlso ViewState("DescAlmacenSel") IsNot Nothing) Then
                    IdAlmacen = ViewState("IdAlmacenSel")
                    Almacen = ViewState("DescAlmacenSel")
                    EsValida = True

                    txtAlmacen.Text = Almacen

                    RaiseEvent AlmacenSeleccionado()
                End If
            Else  ' Se ha lanzado este evento, al perder el foco del cuadro de texto del almacen
                Almacen = txtAlmacen.Text
                If (txtAlmacen.Text <> String.Empty) Then
                    Dim oBrainBLL As New BLL.BrainBLL
                    Dim almac As ELL.BrainBase

                    almac = oBrainBLL.CargarAlmacen(Almacen.ToLower, Empresa)

                    If (almac IsNot Nothing) Then
                        IdAlmacen = almac.ELTO
                        Almacen = almac.DENO_S
                        EsValida = True
                        RaiseEvent AlmacenSeleccionado()
                    Else
                        Almacen = String.Empty
                        IdAlmacen = String.Empty
                        EsValida = False
                    End If
                Else  'Se deja en blanco pero hay que lanzar un evento de disponente no existente
                    Almacen = String.Empty
                    EsValida = False
                End If
            End If

            'Se limpian las variables del viewstate de Seleccion de nuevos grupos para que entre distintas iteraciones, no de problemas
            ViewState("IdDisponenteSel") = Nothing
            ViewState("DescDisponenteSel") = Nothing

        Catch ex As Exception
            Almacen = String.Empty
            IdAlmacen = String.Empty
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
        comprobarAlmacen(sender, e)
    End Sub

    ''' <summary>
    ''' Buscar grupo de producto en lugar de escribirlo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBuscar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnBuscar.Click
        Recargar()
        mpe_SelectorAlmacen.Show()
    End Sub
#End Region

End Class