Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class HistoricosEstadoLineaBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene lista 
        ''' </summary>
        ''' <param name="idValidacionLinea"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorValidacionLinea(ByVal idValidacionLinea As Integer) As List(Of ELL.HistoricoEstadoLinea)
            Return DAL.HistoricosEstadoLineaDAL.loadListByValidacionLinea(idValidacionLinea)
        End Function

#End Region

    End Class

End Namespace