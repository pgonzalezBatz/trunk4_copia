Public Class Perfiles
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
#End Region

#Region "Funciones y Procesos"
    'Sub CargarNodos(lEstructura As List(Of gtkEstructura), TreeNodo As TreeNode)
    '    If lEstructura IsNot Nothing Then
    '        lEstructura = lEstructura.OrderBy(Function(o) o.Descripcion).ToList 'Ordenamos la lista
    '        For Each Estructura As gtkEstructura In lEstructura
    '            Dim Nodo As New TreeNode With {.Value = Estructura.Id, .Text = Estructura.Descripcion}
    '            Nodo.Selected = (Seleccionado IsNot Nothing AndAlso Nodo.Value = Seleccionado) 'Marcamos el nodo seleccionado.
    '            If TreeNodo Is Nothing Then tvEstructura.Nodes.Add(Nodo) Else TreeNodo.ChildNodes.Add(Nodo)
    '            If Estructura.Nodos IsNot Nothing Then CargarNodos(Estructura.Nodos, Nodo)
    '        Next
    '    End If
    'End Sub
#End Region

    Private Sub tvEstructura_Load(sender As Object, e As System.EventArgs) Handles tvEstructura.Load
        'CargarNodos(New gtkEstructura() With {.idTipoIncidencia = TipoIncidencia.gtkMantenimientoSist}.Listado, Nothing)
        '------------------------------------------------------------------------------------------------------------------------------------------
        'Dim Estructuras As List(Of gtkEstructura) = New gtkEstructura() With {.idTipoIncidencia = TipoIncidencia.gtkMantenimientoSist}.Listado _
        '.Where(Function(o) o.Id = 4 Or o.Id = 21 Or o.Id = 22).ToList()
        '------------------------------------------------------------------------------------------------------------------------------------------
        'Cargamos el listado de estructuras expecifico.
        '------------------------------------------------------------------------------------------------------------------------------------------
		'#If DEBUG Then
		'        Dim ListaIDs As New List(Of Integer) From {5}
		'#Else
		Dim ListaIDs As New List(Of Integer) From {5}
		'#End If
        Dim Estructuras As List(Of gtkEstructura) = New gtkEstructura() With {.idTipoIncidencia = TipoIncidencia.gtkMantenimientoSist}.Listado _
                                                    .Where(Function(o) ListaIDs.Find(Function(i) i = o.Id)).ToList()
        '------------------------------------------------------------------------------------------------------------------------------------------
        'CargarNodos(Estructuras, Nothing)

        If Estructuras IsNot Nothing Then
            Estructuras = Estructuras.OrderBy(Function(o) o.Descripcion).ToList 'Ordenamos la lista
            For Each Estructura As gtkEstructura In Estructuras
                Dim Nodo As New TreeNode With {.Value = Estructura.Id, .Text = Estructura.Descripcion}
                Nodo.SelectAction = TreeNodeSelectAction.Expand
                tvEstructura.Nodes.Add(Nodo)
                If Estructura.Nodos IsNot Nothing Then
                    For Each Perfil As gtkEstructura In Estructura.Nodos
                        Dim NodoPerfil As New TreeNode With {.Value = Perfil.Id, .Text = Perfil.Descripcion}
                        'NodoPerfil.SelectAction = TreeNodeSelectAction.Expand
                        Nodo.ChildNodes.Add(NodoPerfil) 'Agregamos los Perfiles de los usuarios a la estructura
                        If Perfil.Nodos IsNot Nothing Then
                            Dim Usuarios As New List(Of Sablib.ELL.Usuario)
                            For Each NodoUsuario As gtkEstructura In Perfil.Nodos
                                Dim UsuarioSab As New Sablib.ELL.Usuario : Dim fUsrSab As New Sablib.BLL.UsuariosComponent
                                UsuarioSab.Id = NodoUsuario.Descripcion : UsuarioSab = fUsrSab.GetUsuario(UsuarioSab, False)
                                If UsuarioSab IsNot Nothing Then Usuarios.Add(UsuarioSab)
                            Next
                            If Usuarios IsNot Nothing Then
                                Usuarios.OrderBy(Function(o) o.NombreCompleto).ToList _
                                    .ForEach(Sub(o) NodoPerfil.ChildNodes.Add( _
                                                 New TreeNode With {.Value = o.Id, .Text = o.NombreCompleto, .SelectAction = TreeNodeSelectAction.None}))
                            End If
                        End If
                    Next
                End If
            Next
        End If
    End Sub

    Private Sub tvEstructura_SelectedNodeChanged(sender As Object, e As System.EventArgs) Handles tvEstructura.SelectedNodeChanged
        Seleccionado = CType(sender, TreeView).SelectedNode.Value
        Server.Transfer("SelectorUsuarios.aspx")
    End Sub
End Class