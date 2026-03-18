
Partial Public Class SelectorProyectoPTKSIS
    Inherits System.Web.UI.UserControl

#Region "Propiedades"

    ''' <summary>
    ''' Referencia en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdReferencia() As String
        Get
            If (ViewState("IdReferencia") Is Nothing) Then
                Return String.Empty
            Else
                Return ViewState("IdReferencia").ToString
            End If
        End Get
        Set(ByVal value As String)
            ViewState("IdReferencia") = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Indica si la referencia activa es valida. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EsValida() As Boolean
        Get
            If (ViewState("EsValidaRef") Is Nothing) Then
                Return False
            Else
                Return CType(ViewState("EsValidaRef"), Boolean)
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("EsValidaRef") = value
        End Set
    End Property

    ''' <summary>
    ''' Para acceder al cuadro de texto del proyecto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Proyecto() As String
        Get
            Return txtCustProjectName.Text
        End Get
        Set(ByVal value As String)
            txtCustProjectName.Text = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Descripcion de la referencia en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdProyecto() As String
        Get
            If Not (String.IsNullOrEmpty(hfCustProjectName.Value)) Then
                Return hfCustProjectName.Value
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            hfCustProjectName.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Id del proyecto de ptksys
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdProjectPtksis() As String
        Get
            If Not (String.IsNullOrEmpty(hfIdProjectPtksis.Value)) Then
                Return hfIdProjectPtksis.Value
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            hfIdProjectPtksis.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Descripcion de la referencia en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RFV_Proyecto()
        Get
            Return rfvCustPN.Enabled
        End Get
        Set(ByVal value)
            rfvCustPN.Enabled = value
        End Set
    End Property

    ''' <summary>
    ''' Se produce al comprobar que la referencia no existe
    ''' </summary>
    ''' <remarks></remarks>
    <Obsolete>
    Public Event ProyectoNoExistente()

    ''' <summary>
    ''' Evento que se produce al seleccionar una referencia (Al dar al boton aceptar o al salir)
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ProyectoSeleccionado()

    ''' <summary>
    ''' Se produce cuando el componente quiere mandar un mensaje personalizado al usuario.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ApplicationException(ByVal ex As ApplicationException)

    Public Event Exception(ByVal ex As Exception)

    Public Event CambioPagina()

#End Region

#Region "Eventos de Pagina"

    ''' <summary>
    ''' Registra un script para que al salir de la caja de texto se compruebe la referencia y al final, se recargue la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim parent As String = Me.Parent.ID()
        txtCustProjectName.Attributes.Add("onkeypress", "comprobarEnter_" & Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobar_" & Me.ID, "function comprobarReferencia_" & Me.ID & "(){" & Page.ClientScript.GetPostBackEventReference(btnComprobar, Nothing) & "}", True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobarEnter_" & Me.ID, "function comprobarEnter_" & Me.ID & "() {if (event.keyCode == 13) {comprobarReferencia_" & Me.ID & "();}}", True)

        If Not IsPostBack Then CargarGridView()
    End Sub

#End Region

#Region "GridView Articulos"

    Private Sub gv_Proyectos_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Proyectos.Init
        Dim Tabla As GridView = sender
        Tabla.CrearBotones()
        Tabla.PageIndex = 0
    End Sub

    Private Sub gv_Proyectos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Proyectos.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            e.Row.Attributes.Add("onclick", String.Format("ProyectoElegidoCliente('{0}', '{1}', '{2}')", e.Row.Cells(1).Text, gv_Proyectos.DataKeys(e.Row.RowIndex).Values(0).ToString(), gv_Proyectos.DataKeys(e.Row.RowIndex).Values(2).ToString()))
        End If
    End Sub

    ''' <summary>
    ''' Evento surgido al cambiar de index de página 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_Proyectos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Proyectos.PageIndexChanging
        gv_Proyectos.PageIndex = e.NewPageIndex
        CargarGridView()
        mpe_SelectorProyecto.Show()
        RaiseEvent CambioPagina()
    End Sub

    '''' <summary>
    '''' Evento surgido al seleccionar los artículos
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Protected Sub gv_Proyectos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Proyectos.SelectedIndexChanged
    '    Dim Tabla As GridView = sender
    '    ViewState("IdProSel") = Tabla.SelectedDataKey.Item("ELTO").ToString.Trim
    '    ViewState("DescProSel") = Tabla.SelectedDataKey.Item("DENO_S")

    '    ComprobarReferencia(New Button With {.CommandName = "A"}, Nothing)
    '    'upSelector.Update()
    'End Sub

    ''' <summary>
    ''' Cargar el gridview con el listado de grupos de material
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarGridView()
        Dim oProyectosPTKSisBLL As New BLL.ProyectosPTKSisBLL
        Dim listaProyectos As List(Of ELL.Objeto)
        listaProyectos = oProyectosPTKSisBLL.CargarProyectosPTKSis()
        Ordenar(listaProyectos, GridViewSortExpresion, GridViewSortDirection)
        gv_Proyectos.DataSource = listaProyectos
        gv_Proyectos.DataBind()
    End Sub

#End Region

#Region "Orden"
    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_Proyectos_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_Proyectos.Sorting
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
        mpe_SelectorProyecto.Show()
    End Sub

    ''' <summary>
    ''' Devuelve el tipo de ordenacion que se va a realizar
    ''' </summary>
    ''' <param name="sortDirection">Orden</param>
    ''' <returns>String con el orden a realizar ("ASC o DESC")</returns>
    Private Function GetSortDirection(ByVal sortDirection As SortDirection) As String
        Dim newSortDirection As String = String.Empty
        Select Case sortDirection
            Case SortDirection.Ascending
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
        For index As Integer = 0 To gv_Proyectos.Columns.Count - 1
            If (gv_Proyectos.Columns(index).SortExpression = sortExp And gv_Proyectos.Columns(index).Visible) Then
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
                ViewState("sortExpresion") = "CODIGO"
            End If
            Return ViewState("sortExpresion").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("sortExpresion") = value
        End Set
    End Property

    '''' <summary>
    '''' Ordena la lista de articulos
    '''' </summary>
    '''' <param name="listaCategoriasProducto">Lista de categorías de producto</param>
    '''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    '''' <param name="sortDir">Direccion de ordenacion</param>
    '''' <remarks></remarks>
    Private Sub Ordenar(ByRef listaProyectosBrain As List(Of ELL.Objeto), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Select Case sortExpr
            Case "Id"
                listaProyectosBrain.Sort(Function(o1 As Object, o2 As Object) _
                 If(sortDir = SortDirection.Ascending, o1.Id < o2.Id, o1.Id > o2.Id))
            Case "Nombre"
                listaProyectosBrain.Sort(Function(o1 As Object, o2 As Object) _
                 If(sortDir = SortDirection.Ascending, o1.Nombre < o2.Nombre, o1.Nombre > o2.Nombre))
        End Select
    End Sub

#End Region

#Region "Botones"
    ''' <summary>
    ''' Metodo de Refresco del control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Recargar()
        ComprobarReferencia(btnComprobar, Nothing)
    End Sub

    ''' <summary>
    ''' Boton oculto que comprobará que la referencia sea correcta en dos situaciones:
    '''    -Al perder el foco el texto de la referencia 
    '''    -Al clickear el boton aceptar despues de haber seleccionado una referencia
    ''' </summary>    
    ''' <remarks></remarks>
    Protected Sub ComprobarReferencia(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            If (myButton.CommandName = "A") Then  'Se ha pulsado el boton aceptar
                If (ViewState("IdProSel") IsNot Nothing AndAlso ViewState("IdProSel") <> String.Empty) Then
                    IdProyecto = ViewState("IdProSel")
                    Proyecto = ViewState("DescProSel")
                    EsValida = True

                    txtCustProjectName.Text = Proyecto

                    RaiseEvent ProyectoSeleccionado()

                End If
            Else  ' Se ha lanzado este evento, al perder el foco del cuadro de texto de la referencia
                Proyecto = txtCustProjectName.Text
                If (txtCustProjectName.Text <> String.Empty) Then
                    Dim oBrainBLL As New BLL.ProyectosPTKSisBLL
                    Dim pro As ELL.Proyectos

                    pro = oBrainBLL.CargarProyecto(Proyecto.ToLower)

                    If (pro IsNot Nothing) Then
                        IdProyecto = pro.Id
                        Proyecto = pro.Nombre
                        EsValida = True
                        RaiseEvent ProyectoSeleccionado()
                    Else
                        Proyecto = String.Empty
                        EsValida = False
                    End If
                Else  'Se deja en blanco pero hay que lanzar un evento de referencia no existente
                    Proyecto = String.Empty
                    IdProyecto = String.Empty
                    EsValida = False
                End If
            End If

            'Se limpian las variables del viewstate de Seleccion de nuevas referencias para que entre distintas iteraciones, no de problemas
            ViewState("IdProSel") = Nothing
            ViewState("DescProSel") = Nothing

        Catch ex As ApplicationException
            Proyecto = String.Empty
            EsValida = False
            RaiseEvent ApplicationException(ex)
        Catch ex As Exception
            'Para indicar que no existe            
            RaiseEvent Exception(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Al realizarse el onblur, llama a comprobar referencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnComprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComprobar.Click
        ComprobarReferencia(sender, e)
    End Sub

#End Region

End Class