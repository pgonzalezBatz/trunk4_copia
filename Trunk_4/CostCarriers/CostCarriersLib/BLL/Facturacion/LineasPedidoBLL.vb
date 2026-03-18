Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class LineasPedidoBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.LineaPedido
            Return DAL.LineasPedidoDAL.getObject(id)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado() As List(Of ELL.LineaPedido)
            Return DAL.LineasPedidoDAL.loadList()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal idPedido As Integer) As List(Of ELL.LineaPedido)
            Return DAL.LineasPedidoDAL.loadList(idPedido)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListadoPorPaso(ByVal idStep As Integer) As List(Of ELL.LineaPedido)
            Return DAL.LineasPedidoDAL.loadListByStep(idStep)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="lineasPedido"></param>
        ''' <param name="estadoFacturacion"></param>
        ''' <param name="idUsuario"></param>
        Public Shared Sub Guardar(ByVal lineasPedido As List(Of ELL.LineaPedido), ByVal estadoFacturacion As ELL.Pedido.EstadoFacturacion, ByVal idUsuario As Integer)
            DAL.LineasPedidoDAL.Save(lineasPedido, estadoFacturacion, idUsuario)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idLineaPedido"></param>
        ''' <param name="numFactura"></param>
        ''' <param name="idUsuario"></param>
        Public Shared Sub Facturar(ByVal idLineaPedido As Integer, ByVal numFactura As String, ByVal idUsuario As Integer)
            DAL.LineasPedidoDAL.Bill(idLineaPedido, numFactura, idUsuario)
        End Sub

#End Region

    End Class

End Namespace