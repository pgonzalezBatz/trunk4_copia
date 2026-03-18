Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class HistoricosEstadoFacturacionBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.HistoricoEstadoFacturacion
            Return DAL.HistoricosEstadoFacturacionDAL.getObject(id)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal idLineaPedido As Integer) As List(Of ELL.HistoricoEstadoFacturacion)
            Return DAL.HistoricosEstadoFacturacionDAL.loadList(idLineaPedido)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="historicoEstadoFacturacion"></param>
        Public Shared Sub Añadir(ByVal historicoEstadoFacturacion As ELL.HistoricoEstadoFacturacion)
            DAL.HistoricosEstadoFacturacionDAL.Add(historicoEstadoFacturacion)
        End Sub

#End Region

    End Class

End Namespace