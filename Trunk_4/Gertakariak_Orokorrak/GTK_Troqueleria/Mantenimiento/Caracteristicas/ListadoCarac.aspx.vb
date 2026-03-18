Imports Oracle.ManagedDataAccess.Client

Public Class ListadoCarac
    Inherits PageBase

#Region "Propiedades"
    ''' <summary>
    ''' Entidades de la base de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    Property tvEstructura_Propiedades() As gtkGridView
        Get
            If (Session("tvEstructura_Propiedades") Is Nothing) Then Session("tvEstructura_Propiedades") = New gtkGridView
            Return CType(Session("tvEstructura_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("tvEstructura_Propiedades") = value
        End Set
    End Property
#End Region

#Region "Eventos Pagina"
    Private Sub ListadoCarac_InitComplete(sender As Object, e As EventArgs) Handles Me.InitComplete
        CargarDatos()
    End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'End Sub
#End Region
#Region "Eventos de Objetos"
    Private Sub dlEstructuras_ItemDataBound(sender As Object, e As DataListItemEventArgs) Handles dlEstructuras.ItemDataBound
        Dim ListItem As DataListItem = e.Item
        Dim tvEstructura As TreeView = ListItem.FindControl("tvEstructura")
        Dim Estructura As BatzBBDD.ESTRUCTURA = ListItem.DataItem
        '-----------------------------------------------------------------------
        'Cargamos la estructura para su correspondiente Familia.
        '-----------------------------------------------------------------------
		If Estructura IsNot Nothing AndAlso tvEstructura IsNot Nothing Then
            CargarTreeView(tvEstructura, Estructura, Nothing)
            ExpandirSeleccionados(tvEstructura)
            tvEstructura.DataBind()
        End If
        '-----------------------------------------------------------------------
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As ImageClickEventArgs) Handles btnEditar.Click
        Try
            tvEstructura_Propiedades.IdSeleccionado = hfIdNodo.Value
            Response.Redirect("~/Mantenimiento/Caracteristicas/DetalleCarac.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnNuevoSub_Click(sender As Object, e As ImageClickEventArgs) Handles btnNuevoSub.Click
        Try
            tvEstructura_Propiedades.IdSeleccionado = hfIdNodo.Value
            Response.Redirect("~/Mantenimiento/Caracteristicas/DetalleCarac.aspx?IdIturria=" & tvEstructura_Propiedades.IdSeleccionado, True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnEliminar_Click(sender As Object, e As ImageClickEventArgs) Handles btnEliminar.Click
        Try
            tvEstructura_Propiedades.IdSeleccionado = hfIdNodo.Value
            Dim Estructura As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = tvEstructura_Propiedades.IdSeleccionado Select Reg).SingleOrDefault

            If Estructura.GERTAKARIAK.Any Then Throw New ApplicationException("No se puede eliminar por estar relacionada con alguna incidencia")
            If Estructura.ESTRUCTURA1.Any Then Throw New ApplicationException("No se puede eliminar si tiene elementos secundarios")
            If Estructura.IDITURRIA Is Nothing OrElse Estructura.IDITURRIA = 0 Then Throw New ApplicationException("No se puede eliminar la raiz")

            BBDD.ESTRUCTURA.DeleteObject(Estructura)
            BBDD.SaveChanges()

            CargarDatos()
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "Funciones y Procesos"
    Sub CargarDatos()
        'If Not IsPostBack Then
        dlEstructuras.DataSource = From Clas As BatzBBDD.CLASIFICACION In BBDD.CLASIFICACION
                                   Where Clas.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia _
                                       And Clas.ESTRUCTURA.ID <> My.Settings.IdNotificacionesUG
                                   Order By Clas.ESTRUCTURA.ORDEN, Clas.ESTRUCTURA.DESCRIPCION
                                   Select Clas.ESTRUCTURA
        dlEstructuras.DataBind()
        'End If
    End Sub
    Sub CargarTreeView(ByRef TreeView As TreeView, ByRef Estructura As BatzBBDD.ESTRUCTURA, Optional ByRef TreeNodo As TreeNode = Nothing)
        If Estructura IsNot Nothing Then
            '-------------------------------------------------------------------------------
            'Creamos el nodo. 
            '-------------------------------------------------------------------------------
            Dim Nodo As New TreeNode With
                        {.Value = Estructura.ID, .Text = Estructura.DESCRIPCION _
                        , .SelectAction = TreeNodeSelectAction.Select _
                        , .NavigateUrl = "javascript:NodoSeleccionado(" & Estructura.ID & ")" _
                        , .Checked = If(tvEstructura_Propiedades Is Nothing OrElse tvEstructura_Propiedades.IdSeleccionado Is Nothing, False, (tvEstructura_Propiedades.IdSeleccionado = Estructura.ID))}
            '-------------------------------------------------------------------------------

            '-------------------------------------------------------------------------------
            'Indicamos si el nodo es "Primario" o "Secundario".
            '-------------------------------------------------------------------------------
            If TreeNodo Is Nothing Then TreeView.Nodes.Add(Nodo) Else TreeNodo.ChildNodes.Add(Nodo)
            '-------------------------------------------------------------------------------

            '-------------------------------------------------------------------------------
            'Generamos el siguiente Nodo.
            '---------------------------------------------------------------------------------------------
            If Estructura.ESTRUCTURA1.Any Then
                'For Each Reg As BatzBBDD.ESTRUCTURA In Estructura.ESTRUCTURA1.OrderBy(Function(o) o.DESCRIPCION).OrderBy(Function(o) o.ORDEN)
                For Each Reg As BatzBBDD.ESTRUCTURA In Estructura.ESTRUCTURA1.OrderBy(Function(o) o.ORDEN).ThenBy(Function(o) o.DESCRIPCION)
                    CargarTreeView(TreeView, Reg, Nodo)
                Next
            End If
            '-------------------------------------------------------------------------------
        End If
    End Sub

    Sub ExpandirSeleccionados(ByRef Arbol As TreeView)
        For Each chkNodo As TreeNode In Arbol.CheckedNodes
            ExpandirNodo(chkNodo)
        Next
    End Sub
    Sub ExpandirNodo(ByRef Nodo As TreeNode)
        If Nodo.Parent IsNot Nothing Then
            Nodo.Parent.Expanded = True
            ExpandirNodo(Nodo.Parent)
        End If
    End Sub
#End Region
End Class