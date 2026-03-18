Namespace BLL.Navision

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CostCarriersBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal codigo As String) As ELL.BRAIN.CostCarrier
            Return DAL.Navision.CostCarriersDAL.getObject(codigo)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal codigo As String) As List(Of ELL.BRAIN.CostCarrier)
            Return DAL.Navision.CostCarriersDAL.loadList(codigo)
        End Function

#End Region

    End Class

End Namespace