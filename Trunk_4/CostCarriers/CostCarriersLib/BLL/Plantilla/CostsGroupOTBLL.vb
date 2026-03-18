Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CostsGroupOTBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un cost group de oferta técnica
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.CostGroupOT
            Return DAL.CostsGroupOTDAL.getCostGroupPlantilla(id)
        End Function

        ''' <summary>
        ''' Obtiene costs group de oferta técnica
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function CargarListado() As List(Of ELL.CostGroupOT)
            Return DAL.CostsGroupOTDAL.loadList()
        End Function

#End Region

    End Class

End Namespace