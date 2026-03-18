Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class PedidosBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.Pedido
            Return DAL.PedidosDAL.getObject(id)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal idCabecera As Integer) As List(Of ELL.Pedido)
            Return DAL.PedidosDAL.loadList(idCabecera)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="pedido"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal pedido As ELL.Pedido)
            DAL.PedidosDAL.Save(pedido)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPedido"></param>
        ''' <param name="comentarios"></param>
        Public Shared Sub ActualizarComentarios(ByVal idPedido As Integer, ByVal comentarios As String)
            DAL.PedidosDAL.UpdateComments(idPedido, comentarios)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Eliminar(ByVal id As Integer)
            DAL.PedidosDAL.Delete(id)
        End Sub

#End Region

    End Class

End Namespace