Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ConsultasWS
    Inherits System.Web.Services.WebService

#Region "BRAIN"

#Region "Categorías de producto"

    <WebMethod()> _
    Public Function CargarCategoriasProducto(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim oBrainBLL As New BLL.BrainBLL
        Dim KeyValue As New List(Of String)()

        Dim listaCategoriasProducto As List(Of ELL.BrainBase)
        listaCategoriasProducto = oBrainBLL.CargarCategoriasProducto(prefixText, contextKey)
        For Each categoria In listaCategoriasProducto
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(categoria.DENO_S, categoria.ELTO)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

#End Region

#Region "Tipos de Pieza"

    <WebMethod()> _
    Public Function CargarTiposPieza(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim oBrainBLL As New BLL.BrainBLL
        Dim KeyValue As New List(Of String)()

        Dim listaTiposPieza As List(Of ELL.BrainBase)
        listaTiposPieza = oBrainBLL.CargarTiposPieza(prefixText)
        For Each tipo In listaTiposPieza
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(tipo.DENO_S, tipo.ELTO)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

    '<WebMethod()> _
    'Public Function CargarTiposPieza(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
    '    Dim oBrainBLL As New BLL.BrainBLL
    '    Dim KeyValue As New List(Of String)()

    '    Dim listaTiposPieza As List(Of ELL.BrainBase)
    '    listaTiposPieza = oBrainBLL.CargarTiposPieza(prefixText, contextKey)
    '    For Each tipo In listaTiposPieza
    '        Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(tipo.DENO_S, tipo.ELTO)
    '        KeyValue.Add(item)
    '    Next
    '    Return KeyValue.ToArray
    'End Function

#End Region

#Region "Unidades de medida"

    <WebMethod()> _
    Public Function CargarUnidadesMedida(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim oBrainBLL As New BLL.BrainBLL
        Dim KeyValue As New List(Of String)()

        Dim listaUnidadesMedida As List(Of ELL.BrainBase)
        listaUnidadesMedida = oBrainBLL.CargarUnidadesMedida(prefixText, contextKey)
        For Each unidad In listaUnidadesMedida
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(unidad.DENO_S, unidad.ELTO)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

#End Region

#Region "Grupos de material"

    <WebMethod()> _
    Public Function CargarGruposMaterial(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim oBrainBLL As New BLL.BrainBLL
        Dim KeyValue As New List(Of String)()

        Dim listaGruposMaterial As List(Of ELL.BrainBase)
        listaGruposMaterial = oBrainBLL.CargarGruposMaterial(prefixText)
        For Each grupo In listaGruposMaterial
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(grupo.DENO_S, grupo.ELTO)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

#End Region

#Region "Grupos de producto"

    <WebMethod()> _
    Public Function CargarGruposProducto(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim oBrainBLL As New BLL.BrainBLL
        Dim KeyValue As New List(Of String)()

        Dim listaGruposProducto As List(Of ELL.BrainBase)
        listaGruposProducto = oBrainBLL.CargarGruposProducto(prefixText)
        For Each grupo In listaGruposProducto
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(grupo.DENO_S, grupo.ELTO)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

#End Region

#Region "Tipos de producto"

    <WebMethod()> _
    Public Function CargarTiposProducto(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim oBrainBLL As New BLL.BrainBLL
        Dim KeyValue As New List(Of String)()

        Dim listaTiposProducto As List(Of ELL.BrainBase)
        listaTiposProducto = oBrainBLL.CargarTiposProducto(prefixText)
        For Each tipo In listaTiposProducto
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(tipo.DENO_S, tipo.ELTO)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

#End Region

#Region "Disponentes"

    <WebMethod()> _
    Public Function CargarDisponentes(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim oBrainBLL As New BLL.BrainBLL
        Dim KeyValue As New List(Of String)()

        Dim listaDisponentes As List(Of ELL.BrainBase)
        listaDisponentes = oBrainBLL.CargarDisponentes(prefixText, contextKey)
        For Each disponente In listaDisponentes
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(disponente.DENO_S, disponente.ELTO)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

#End Region

#Region "Almacenes"

    <WebMethod()> _
    Public Function CargarAlmacenes(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim oBrainBLL As New BLL.BrainBLL
        Dim KeyValue As New List(Of String)()

        Dim listaAlmacenes As List(Of ELL.BrainBase)
        listaAlmacenes = oBrainBLL.CargarAlmacenes(prefixText, contextKey)
        For Each almacen In listaAlmacenes
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(almacen.DENO_S, almacen.ELTO)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

#End Region

#Region "Proyectos"

    <WebMethod()> _
    Public Function CargarProyectosBrain(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim oProyectoBLL As New BLL.BrainBLL

        Dim KeyValue As New List(Of String)()
        Dim listaProyectos As List(Of ELL.BrainBase)
        listaProyectos = oProyectoBLL.CargarProyectosBrain(prefixText)
        For Each proyecto In listaProyectos
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(proyecto.ELTO + " - " + proyecto.DENO_S, proyecto.ELTO)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray

    End Function

#End Region

#Region "Subproyectos"

    <WebMethod()> _
    Public Function CargarSubproyectos(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim oProyectoBLL As New BLL.BrainBLL

        Dim KeyValue As New List(Of String)()
        Dim listaSubproyectos As List(Of ELL.BrainBase)
        listaSubproyectos = oProyectoBLL.CargarSubproyectos(prefixText, contextKey)
        For Each subproyecto In listaSubproyectos
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(subproyecto.ELTO + " - " + subproyecto.DENO_S, subproyecto.ELTO)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

#End Region

#Region "Previous Batz Part Number"

    <WebMethod()> _
    Public Function CargarPreviousBatzPN(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim oProyectoBLL As New BLL.BrainBLL

        Dim KeyValue As New List(Of String)()
        Dim listaPreviousBatzPN As List(Of ELL.BrainBase)
        listaPreviousBatzPN = oProyectoBLL.CargarPreviousBatzPN(prefixText)
        For Each prevBatzPN In listaPreviousBatzPN
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(prevBatzPN.DENO_S, prevBatzPN.ELTO)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

    '<WebMethod()> _
    'Public Function CargarPreviousBatzPN(ByVal prefixText As String, ByVal count As Integer) As List(Of String())
    '    Dim oReferenciaFinalVentaBLL As New BLL.ReferenciaFinalVentaBLL
    '    Return oReferenciaFinalVentaBLL.CargarPreviousBatzPN(prefixText)
    'End Function

#End Region

#End Region

#Region "USUARIOS"

    <WebMethod()> _
    Public Function CargarUsuarios(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim SabComponent As New SabLib.BLL.UsuariosComponent
        Dim listaUsuarios As List(Of SabLib.ELL.Usuario) = SabComponent.GetUsuariosBusquedaSAB_Optimizado(prefixText)

        Dim KeyValue As New List(Of String)()

        For Each usuario In listaUsuarios
            If Not (usuario.DadoBaja) Then
                Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(usuario.NombreCompleto, usuario.Id)
                KeyValue.Add(item)
            End If
        Next
        Return KeyValue.ToArray
    End Function

#End Region

#Region "REFERENCIAS VENTAS"

    <WebMethod()> _
    Public Function GetTypesByProduct(ByVal idProduct As String) As List(Of String)
        Dim listaProductos As New List(Of String)
        Dim oProductoBLL As New BLL.ProductoBLL
        Dim productos As List(Of ELL.ProductoType)

        productos = oProductoBLL.CargarTiposProducto(idProduct)
        For Each producto In productos
            listaProductos.Add(producto.IdType)
        Next
        Return listaProductos
    End Function

#End Region

#Region "PTKSIS"

    <WebMethod()>
    Public Function CargarProyectosPTKSis(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim oProyectoBLL As New BLL.ProyectosPTKSisBLL

        Dim KeyValue As New List(Of String)()
        Dim listaProyectos As List(Of ELL.Objeto)
        listaProyectos = oProyectoBLL.CargarProyectosPTKSisPorTexto(prefixText)
        For Each proyecto In listaProyectos
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(proyecto.Nombre, String.Format("{0}@{1}", proyecto.Id, proyecto.IdPtksis))
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

#End Region

End Class