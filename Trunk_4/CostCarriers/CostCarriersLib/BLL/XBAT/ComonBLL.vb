Namespace BLL.XBAT

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ComonBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codmon"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal codmon As Integer) As ELL.XBAT.Comon
            Return DAL.XBAT.ComonDAL.getObject(codmon)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlantaBrain"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerPorIdPlantaBrain(ByVal idPlantaBrain As String) As ELL.XBAT.Comon
            Return DAL.XBAT.ComonDAL.getObjectByIdPlantaBrain(idPlantaBrain)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function CargarListado() As List(Of ELL.XBAT.Comon)
            Return DAL.XBAT.ComonDAL.loadList()
        End Function

#End Region

    End Class

End Namespace