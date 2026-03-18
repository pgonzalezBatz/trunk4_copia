Namespace BLL

    Public Class AlbaranesBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un albarán
        ''' </summary>
        ''' <param name="albaran"></param>
        ''' <param name="pedido"></param>
        ''' <param name="linea"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerItem(ByVal albaran As String, ByVal pedido As Integer, ByVal linea As Integer) As ELL.Albaran
            Return BRAIN.DAL.AlbaranesDAL.getItem(albaran, pedido, linea)
        End Function

        ''' <summary>
        ''' Obtiene un listado de albaranes para un proveedor
        ''' </summary>
        ''' <param name="proveedor"></param> 
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListadoPendRecep(ByVal proveedor As Integer, ByVal empresa As String, ByVal planta As String) As List(Of ELL.Albaran)
            Return BRAIN.DAL.AlbaranesDAL.loadListPendRecep(proveedor, empresa, planta)
        End Function

        ''' <summary>
        ''' Obtiene un listado de albaranes para un proveedor facturables por Batz
        ''' </summary>
        ''' <param name="proveedor"></param> 
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListadoFacturablesBatz(ByVal proveedor As Integer, ByVal empresa As String, ByVal planta As String) As List(Of ELL.Albaran)
            Return BRAIN.DAL.AlbaranesDAL.loadListFacturablesBatz(proveedor, empresa, planta)
        End Function

        ''' <summary>
        ''' Obtiene un listado de albaranes para un proveedor facturables por el proveedor
        ''' </summary>
        ''' <param name="proveedor"></param> 
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListadoFacturablesProveedor(ByVal proveedor As Integer, ByVal empresa As String, ByVal planta As String) As List(Of ELL.Albaran)
            Return BRAIN.DAL.AlbaranesDAL.loadListFacturablesProveedor(proveedor, empresa, planta)
        End Function

#End Region

    End Class

End Namespace