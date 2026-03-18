Namespace BLL

    Public Class ProductosPtksisBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un producto por proyecto
        ''' </summary>
        ''' <param name="proyecto"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ObtenerProducto(ByVal proyecto As String) As ELL.ProductoPtksis
            Return DAL.ProductosPtksisDAL.getProducto(proyecto)
        End Function


        ''' <summary>
        ''' Obtiene un listado
        ''' </summary>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado() As List(Of ELL.ProductoPtksis)
            Return DAL.ProductosPtksisDAL.loadList()
        End Function

#End Region

    End Class

End Namespace