Namespace BLL.BRAIN

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CCProductionPlantBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigoPortador"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal empresa As String, ByVal planta As String, ByVal codigoPortador As String) As List(Of ELL.BRAIN.CCProductionPlant)
            Return DAL.BRAIN.CCProductionPlantDAL.loadList(empresa, planta, codigoPortador)
        End Function

#End Region

    End Class

End Namespace