Public Class Inicio1
    Inherits PageBase
#Region "Propiedades"
    Public Property Seleccionado As Nullable(Of Integer)
        Get
            Return ViewState("Seleccionado")
        End Get
        Set(ByVal value As Nullable(Of Integer))
            ViewState("Seleccionado") = value
        End Set
    End Property
#End Region
#Region "Eventos Pagina"
    Private Sub Inicio1_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If PreviousPage IsNot Nothing Then
            Dim myPropertyID As System.Reflection.PropertyInfo = Array.Find(PreviousPage.GetType.GetProperties, Function(o As System.Reflection.PropertyInfo) o.Name = "Seleccionado")
            If myPropertyID.GetValue(PreviousPage, Nothing) IsNot Nothing Then Seleccionado = myPropertyID.GetValue(PreviousPage, Nothing)
        End If
    End Sub
    Private Sub tvEstructura_Load(sender As Object, e As System.EventArgs) Handles tvEstructura.Load
        'CargarNodos(New gtkEstructura() With {.idTipoIncidencia = TipoIncidencia.gtkMantenimientoSist}.Listado, Nothing)
        '------------------------------------------------------------------------------------------------------------------------------------------
        'Dim Estructuras As List(Of gtkEstructura) = New gtkEstructura() With {.idTipoIncidencia = TipoIncidencia.gtkMantenimientoSist}.Listado _
        '.Where(Function(o) o.Id = 4 Or o.Id = 21 Or o.Id = 22).ToList()
        '------------------------------------------------------------------------------------------------------------------------------------------
        'Cargamos el listado de estructuras expecifico.
        '------------------------------------------------------------------------------------------------------------------------------------------
		'#If DEBUG Then
		'        Dim ListaIDs As New List(Of Integer) From {4, 21}
		'#Else
		Dim ListaIDs As New List(Of Integer) From {4}
		'#End If
        Dim Estructuras As List(Of gtkEstructura) = New gtkEstructura() With {.idTipoIncidencia = TipoIncidencia.gtkMantenimientoSist}.Listado _
                                                    .Where(Function(o) ListaIDs.Find(Function(i) i = o.Id)).ToList()
        '------------------------------------------------------------------------------------------------------------------------------------------
        CargarNodos(Estructuras, Nothing)
    End Sub
    Private Sub tvEstructura_SelectedNodeChanged(sender As Object, e As System.EventArgs) Handles tvEstructura.SelectedNodeChanged
        Seleccionado = CType(sender, TreeView).SelectedNode.Value
        Server.Transfer("Detalle.aspx")
    End Sub
#End Region
#Region "Acciones"

#End Region
#Region "Funciones y Procesos"
    Sub CargarNodos(lEstructura As List(Of gtkEstructura), TreeNodo As TreeNode)
        If lEstructura IsNot Nothing Then
            lEstructura = lEstructura.OrderBy(Function(o) o.Descripcion).ToList 'Ordenamos la lista
            For Each Estructura As gtkEstructura In lEstructura
                Dim Nodo As New TreeNode With {.Value = Estructura.Id, .Text = Estructura.Descripcion}
                Nodo.Selected = (Seleccionado IsNot Nothing AndAlso Nodo.Value = Seleccionado) 'Marcamos el nodo seleccionado.
                If TreeNodo Is Nothing Then tvEstructura.Nodes.Add(Nodo) Else TreeNodo.ChildNodes.Add(Nodo)
                If Estructura.Nodos IsNot Nothing Then CargarNodos(Estructura.Nodos, Nodo)
            Next
        End If
    End Sub
#End Region
End Class