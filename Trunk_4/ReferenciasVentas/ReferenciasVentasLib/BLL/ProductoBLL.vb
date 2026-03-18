
Namespace BLL

    Public Class ProductoBLL

        Private productoDAL As New DAL.ProductoDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene el listado
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarLista() As List(Of ELL.Producto)
            Return productoDAL.loadList()
        End Function

        ''' <summary>
        ''' Obtiene el listado de productos activos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarProductosActivos() As List(Of ELL.Producto)
            Return productoDAL.CargarProductosActivos()
        End Function

        ''' <summary>
        ''' Obtiene un producto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarProducto(ByVal id As Integer) As ELL.Producto
            Return productoDAL.CargarProducto(id)
        End Function

        ''' <summary>
        ''' Comprobar si una cadena existe en la tabla
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Existe(ByVal nombre As String) As Boolean
            If (productoDAL.existe(nombre) > 0) Then
                Return True
            Else : Return False
            End If
        End Function

        ''' <summary>
        ''' Cargar los tipos relacionados con un producto
        ''' </summary>
        ''' <param name="idProducto">Identificador de producto</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTiposProducto(ByVal idProducto As Integer) As List(Of ELL.ProductoType)
            Return productoDAL.CargarTiposProducto(idProducto)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un nuevo registro
        ''' </summary>
        ''' <param name="producto">Objeto Producto</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarProducto(ByVal producto As ELL.Producto) As Boolean
            Return productoDAL.Save(producto)
        End Function

        ''' <summary>
        ''' Modifica los datos de un registro
        ''' </summary>
        ''' <param name="producto">Objeto Producto</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ModificarProducto(ByVal producto As ELL.Producto) As Boolean
            Return productoDAL.Update(producto)
        End Function

        ' ''' <summary>
        ' ''' Elimina un registro
        ' ''' </summary>
        ' ''' <param name="idProducto">Identificador del Producto</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function EliminarProducto(ByVal idProducto As Integer) As Boolean
        '    Return productoDAL.Delete(idProducto)
        'End Function

#End Region

    End Class

End Namespace
