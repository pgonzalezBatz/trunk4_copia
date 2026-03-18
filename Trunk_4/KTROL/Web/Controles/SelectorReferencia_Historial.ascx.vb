Imports KaPlanLib.BLL.DynamicLinq

Partial Public Class SelectorReferencia_Historial
    Inherits System.Web.UI.UserControl

    'Dim ItzultzaileWeb As New LocalizationLib.Itzultzaile

#Region "Propiedades"
    ''' <summary>
    ''' Referencia en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdReferencia() As String
        Get
            If UsarSession Then
                If (Session("IdReferencia") Is Nothing) Then
                    Return String.Empty
                Else
                    Return Session("IdReferencia").ToString
                End If
            Else
                If (ViewState("IdReferencia") Is Nothing) Then
                    Return String.Empty
                Else
                    Return ViewState("IdReferencia").ToString
                End If
            End If
        End Get
        Set(ByVal value As String)
            If (UsarSession) Then
                Session("IdReferencia") = value.Trim
            Else
                ViewState("IdReferencia") = value.Trim
            End If
        End Set
    End Property

    ''' <summary>
    ''' Descripcion de la referencia en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Descripcion() As String
        Get
            If (UsarSession) Then
                If (Session("DescripcionRef") Is Nothing) Then
                    Return String.Empty
                Else
                    Return Session("DescripcionRef").ToString
                End If
            Else
                If (ViewState("DescripcionRef") Is Nothing) Then
                    Return False
                Else
                    Return ViewState("DescripcionRef").ToString
                End If
            End If
        End Get
        Set(ByVal value As String)
            If (UsarSession) Then
                Session("DescripcionRef") = value.Trim
            Else
                ViewState("DescripcionRef") = value.Trim
            End If
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
            If (UsarSession) Then
                If (Session("EsValidaRef") Is Nothing) Then
                    Return False
                Else
                    Return CType(Session("EsValidaRef"), Boolean)
                End If
            Else
                If (ViewState("EsValidaRef") Is Nothing) Then
                    Return False
                Else
                    Return CType(ViewState("EsValidaRef"), Boolean)
                End If
            End If
        End Get
        Set(ByVal value As Boolean)
            If (UsarSession) Then
                Session("EsValidaRef") = value
            Else
                ViewState("EsValidaRef") = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Para acceder al cuadro de texto de la referencia
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Referencia() As String
        Get
            Return txtReferencia.Text
        End Get
        Set(ByVal value As String)
            txtReferencia.Text = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Se le indica si el control va a usar las variables de session, es decir si va a ser un control de referencias global o independiente
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UsarSession() As Boolean
        Get
            If (ViewState("usarSession") Is Nothing) Then
                ViewState("usarSession") = True  'Por defecto, se usuara la session
            End If
            Return ViewState("usarSession")
        End Get
        Set(ByVal value As Boolean)
            ViewState("usarSession") = value
        End Set
    End Property

    Private _FiltroResumen As whereResumen = whereResumen.Todo
    Public Property FiltroResumen() As whereResumen
        Get
            Return _FiltroResumen
        End Get
        Set(ByVal value As whereResumen)
            _FiltroResumen = value
        End Set
    End Property

    ''' <summary>
    ''' Filtra las referencias a mostrar con la tabla de resumen amfe, sin ella o mostrando todos
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum whereResumen As Integer
        Todo = 0
        ConResumen = 1
        SinResumen = 2
    End Enum

    ''' <summary>
    ''' Se produce al comprobar que la referencia no existe
    ''' </summary>
    ''' <remarks></remarks>
    <Obsolete> _
    Public Event ReferenciaNoExistente()
    ''' <summary>
    ''' Evento que se produce al seleccionar una referencia (Al dar al boton aceptar o al salir)
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ReferenciaSeleccionada()

    ''' <summary>
    ''' Se produce cuando el componente quiere mandar un mensaje personalizado al usuario.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ApplicationException(ByVal ex As ApplicationException)
    Public Event Exception(ByVal ex As Exception)
#End Region

#Region "Eventos de Pagina"
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If UsarSession Then
            If (Session("IdReferencia") IsNot Nothing) Then Referencia = Session("IdReferencia").ToString
        End If
    End Sub

    ''' <summary>
    ''' Registra un script para que al salir de la caja de texto se compruebe la referencia y al final, se recargue la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtReferencia.Attributes.Add("onkeypress", "comprobarEnter_" & Me.ID & "();")
        'txtReferencia.Attributes.Add("onblur", "comprobarReferencia_" & Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobar_" & Me.ID, "function comprobarReferencia_" & Me.ID & "(){" & Page.ClientScript.GetPostBackEventReference(btnComprobar, Nothing) & "}", True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobarEnter_" & Me.ID, "function comprobarEnter_" & Me.ID & "() {if (event.keyCode == 13) {comprobarReferencia_" & Me.ID & "();}}", True)
        ace_txtReferencia.OnClientItemSelected = "comprobarReferencia_" & Me.ID

        txtReferencia.Text = Referencia
        lblDescripcion.Text = Descripcion

        'Select Case FiltroResumen
        '    Case whereResumen.Todo : lblTitulo.Text = "Pieza en uso"
        '    Case whereResumen.ConResumen : lblTitulo.Text = "Pieza origen"
        '    Case whereResumen.SinResumen : lblTitulo.Text = "Pieza destino"
        'End Select
        If Not IsPostBack Then CargarGridView()
    End Sub

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        '-----------------------------------
        'ItzultzaileWeb.Itzuli(lblTitulo)
        'ItzultzaileWeb.Itzuli(lblRef)
        'ItzultzaileWeb.Itzuli(btnBuscar)
        'ItzultzaileWeb.Itzuli(imgCerrar)
        '-----------------------------------

        '--------------------------------------------------------
        'pnlPiezaUso
        '--------------------------------------------------------
        'ItzultzaileWeb.Itzuli(Label2)
        'ItzultzaileWeb.Itzuli(gv_Articulos)
        '--------------------------------------------------------
    End Sub
#End Region

#Region "Eventos de Objetos"
    Private Sub ddl_Lantegis_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Lantegis.SelectedIndexChanged
        CargarGridView()
    End Sub
    Private Sub ace_txtReferencia_Init(sender As Object, e As EventArgs) Handles ace_txtReferencia.Init
        'Evitamos que de error cuando se caduca la sesion
        If Session("Planta") IsNot Nothing Then
            Dim obj As AjaxControlToolkit.AutoCompleteExtender = sender
            Dim sPlanta As String() = Session("Planta")
            obj.ContextKey = sPlanta(2)
            obj.UseContextKey = True
        End If
    End Sub
#End Region
#Region "GridView Articulos"
    Private Sub gv_Articulos_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Articulos.Init
        Dim Tabla As GridView = sender
        Tabla.CrearBotones()
    End Sub
    Private Sub gv_Articulos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Articulos.RowDataBound
        Try
            Dim Fila As GridViewRow = e.Row
            Fila.CrearAccionesFila()

            If (Fila.RowType = DataControlRowType.Header) Then
                AddSortImage(e.Row)
            End If
        Catch ex As ApplicationException
            lblDescripcion.Text = ex.Message

        Catch ex As Exception
            lblDescripcion.Text = ex.Message
        End Try
    End Sub
    Private Sub gv_Articulos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Articulos.PageIndexChanging
        gv_Articulos.PageIndex = e.NewPageIndex
        CargarGridView()
    End Sub
    ''' <summary>
    ''' Evento surgido al seleccionar los artículos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gv_Articulos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Articulos.SelectedIndexChanged
        Dim Tabla As GridView = sender
        ViewState("IdRefSel") = Tabla.SelectedDataKey.Item("CODIGO").ToString.Trim
        ViewState("DescRefSel") = Tabla.SelectedDataKey.Item("DENOMINACION")

        ComprobarReferencia(New Button With {.CommandName = "A"}, Nothing)
        upSelector.Update()
    End Sub
#End Region

#Region "Orden"
    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_Articulos_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_Articulos.Sorting
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
        For index As Integer = 0 To gv_Articulos.Columns.Count - 1
            If (gv_Articulos.Columns(index).SortExpression = sortExp And gv_Articulos.Columns(index).Visible) Then
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
                    sortImage.ImageUrl = "~/App_Themes/Tema1/Imagenes/sortascending.gif"
                    sortImage.AlternateText = "ordenAscendente"
                Else
                    sortImage.ImageUrl = "~/App_Themes/Tema1/Imagenes/sortdescending.gif"
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
    '''' <param name="lArticulos">Lista de articulos</param>
    '''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    '''' <param name="sortDir">Direccion de ordenacion</param>
    '''' <remarks></remarks>
    'Private Sub Ordenar(ByRef lArticulos As System.Data.Linq.DataQuery(Of KaPlanLib.Registro.MAESTRO_ARTICULOS), ByVal sortExpr As String, ByVal sortDir As SortDirection)
    '	Select Case sortExpr
    '		Case "CODIGO"
    '			lArticulos.Sort(Function(o1 As Object, o2 As Object) _
    '			 If(sortDir = SortDirection.Ascending, o1.CODIGO < o2.CODIGO, o1.CODIGO > o2.CODIGO))
    '		Case "REFERENCIA_CLIENTE"
    '			lArticulos.Sort(Function(o1 As Object, o2 As Object) _
    '			 If(sortDir = SortDirection.Ascending, o1.REFERENCIA_CLIENTE < o2.REFERENCIA_CLIENTE, o1.REFERENCIA_CLIENTE > o2.REFERENCIA_CLIENTE))
    '		Case "DENOMINACION"
    '			lArticulos.Sort(Function(o1 As Object, o2 As Object) _
    '			 If(sortDir = SortDirection.Ascending, o1.DENOMINACION < o2.DENOMINACION, o1.DENOMINACION > o2.DENOMINACION))
    '	End Select
    'End Sub
#End Region

#Region "Botones"
    ''' <summary>
    ''' Metodo de Refresco del control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Recargar()
        'btnAceptar.CommandName = String.Empty
        'ComprobarReferencia(btnAceptar, Nothing)

        '--------------------------------------------------------------------
        'ComprobarReferencia(Nothing, Nothing)
        '--------------------------------------------------------------------
        'FROGA:2011-11-16
        '--------------------------------------------------------------------
        ComprobarReferencia(btnComprobar, Nothing)
        '--------------------------------------------------------------------
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
                If (ViewState("IdRefSel") IsNot Nothing AndAlso ViewState("IdRefSel") <> String.Empty) Then
                    IdReferencia = ViewState("IdRefSel")
                    Descripcion = ViewState("DescRefSel")
                    EsValida = True

                    txtReferencia.Text = IdReferencia
                    lblDescripcion.Text = Descripcion
                    RaiseEvent ReferenciaSeleccionada()
                End If
            Else  ' Se ha lanzado este evento, al perder el foco del cuadro de texto de la referencia
                IdReferencia = txtReferencia.Text
                If (txtReferencia.Text <> String.Empty) Then
                    Dim linqComp As New KaPlanLib.BLL.LinqComponent
                    Dim regArticulo As KaPlanLib.Registro.MAESTRO_ARTICULOS = Nothing
                    regArticulo = linqComp.consultarMaestroArticulo(IdReferencia, FiltroResumen)

                    If (regArticulo IsNot Nothing) Then
                        lblDescripcion.Text = regArticulo.DENOMINACION
                        Descripcion = lblDescripcion.Text
                        EsValida = True
                        RaiseEvent ReferenciaSeleccionada()
                    Else
                        lblDescripcion.Text = String.Empty
                        Descripcion = String.Empty
                        EsValida = False
                        'RaiseEvent ReferenciaNoExistente()
                        'Throw New ApplicationException(String.Format("Referencia '{0}' no existente".Itzuli, IdReferencia))
                    End If
                Else  'Se deja en blanco pero hay que lanzar un evento de referencia no existente
                    lblDescripcion.Text = String.Empty
                    Descripcion = String.Empty
                    EsValida = False
                    'RaiseEvent ReferenciaNoExistente()
                    'Throw New ApplicationException(String.Format("Referencia '{0}' no existente".Itzuli, IdReferencia))
                End If
            End If

            'Se limpian las variables del viewstate de Seleccion de nuevas referencias para que entre distintas iteraciones, no de problemas
            ViewState("IdRefSel") = Nothing
            ViewState("DescRefSel") = Nothing

        Catch ex As ApplicationException
            lblDescripcion.Text = String.Empty
            Descripcion = String.Empty
            EsValida = False
            RaiseEvent ApplicationException(ex)
        Catch ex As Exception
            'Para indicar que no existe            
            lblDescripcion.Text = String.Empty
            'RaiseEvent ReferenciaNoExistente()
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

    Private Sub btnBuscar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnBuscar.Click
        Recargar()
        mpe_SelectorRef.Show()
    End Sub
#End Region

#Region "Funciones y Procesos"
    ''' <summary>
    ''' Revisar codigo cuando se sepa hacer consultas dinamicas con Linq.
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarGridView()

        Dim BBDD As New KaPlanLib.DAL.ELL
        Dim ItemsLinq = Nothing
        'TODO:(B)Revisar codigo cuando se sepa hacer consultas dinamicas con Linq
        Select Case Me.FiltroResumen
            Case whereResumen.Todo
                If ddl_Lantegis.SelectedValue = String.Empty Then
                    ItemsLinq = From Articulos In BBDD.MAESTRO_ARTICULOS _
                    Where Articulos.LANTEGI Is Nothing _
                    Select Articulos Distinct _
                    .OrderBy(GridViewSortExpresion & " " & If(GridViewSortDirection = SortDirection.Ascending, "ASC", "DESC"))
                Else
                    ItemsLinq = From Articulos In BBDD.MAESTRO_ARTICULOS _
                    Where Articulos.LANTEGI = ddl_Lantegis.SelectedValue _
                    Select Articulos Distinct _
                    .OrderBy(GridViewSortExpresion & " " & If(GridViewSortDirection = SortDirection.Ascending, "ASC", "DESC"))
                End If
            Case whereResumen.ConResumen
                If ddl_Lantegis.SelectedValue = String.Empty Then
                    ItemsLinq = From Articulos In BBDD.MAESTRO_ARTICULOS _
                      Join AmfesPieza In BBDD.AMFES_PIEZA On Articulos.CODIGO Equals AmfesPieza.CODIGO _
                      Join ResumenAmfe In BBDD.RESUMEN_AMFE On AmfesPieza.ID_AMFE_PIEZA Equals ResumenAmfe.ID_AMFE _
                      Where Articulos.LANTEGI Is Nothing _
                      Select Articulos Distinct _
                      .OrderBy(GridViewSortExpresion & " " & If(GridViewSortDirection = SortDirection.Ascending, "ASC", "DESC"))
                Else
                    ItemsLinq = From Articulos In BBDD.MAESTRO_ARTICULOS _
                      Join AmfesPieza In BBDD.AMFES_PIEZA On Articulos.CODIGO Equals AmfesPieza.CODIGO _
                      Join ResumenAmfe In BBDD.RESUMEN_AMFE On AmfesPieza.ID_AMFE_PIEZA Equals ResumenAmfe.ID_AMFE _
                      Where Articulos.LANTEGI = ddl_Lantegis.SelectedValue _
                      Select Articulos Distinct _
                    .OrderBy(GridViewSortExpresion & " " & If(GridViewSortDirection = SortDirection.Ascending, "ASC", "DESC"))
                End If
            Case whereResumen.SinResumen
                If ddl_Lantegis.SelectedValue = String.Empty Then
                    ItemsLinq = From Articulos In BBDD.MAESTRO_ARTICULOS _
                     Join AmfesPieza In BBDD.AMFES_PIEZA On Articulos.CODIGO Equals AmfesPieza.CODIGO _
                     Group Join ResumenAmfe In BBDD.RESUMEN_AMFE On AmfesPieza.ID_AMFE_PIEZA Equals ResumenAmfe.ID_AMFE Into resumenAmfesPieza = Group _
                     From resAmfe In resumenAmfesPieza.DefaultIfEmpty _
                     Where resAmfe.ID_AMFE Is Nothing And Articulos.LANTEGI Is Nothing _
                     Select Articulos Distinct _
                     .OrderBy(GridViewSortExpresion & " " & If(GridViewSortDirection = SortDirection.Ascending, "ASC", "DESC"))
                Else
                    ItemsLinq = From Articulos In BBDD.MAESTRO_ARTICULOS _
                     Join AmfesPieza In BBDD.AMFES_PIEZA On Articulos.CODIGO Equals AmfesPieza.CODIGO _
                     Group Join ResumenAmfe In BBDD.RESUMEN_AMFE On AmfesPieza.ID_AMFE_PIEZA Equals ResumenAmfe.ID_AMFE Into resumenAmfesPieza = Group _
                     From resAmfe In resumenAmfesPieza.DefaultIfEmpty _
                     Where resAmfe.ID_AMFE Is Nothing And Articulos.LANTEGI = ddl_Lantegis.SelectedValue _
                     Select Articulos Distinct _
                     .OrderBy(GridViewSortExpresion & " " & If(GridViewSortDirection = SortDirection.Ascending, "ASC", "DESC"))
                End If
        End Select

        gv_Articulos.DataSource = ItemsLinq
        gv_Articulos.DataBind()
    End Sub
#End Region

End Class