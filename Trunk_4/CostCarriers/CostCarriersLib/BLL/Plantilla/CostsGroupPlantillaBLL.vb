Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CostsGroupPlantillaBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un cost group plantilla
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.CostGroupPlantilla
            Return DAL.CostsGroupPlantillaDAL.getCostGroupPlantilla(id)
        End Function

        ''' <summary>
        ''' Obtiene costs group plantilla
        ''' </summary>
        ''' <param name="idEstadoPlantilla"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal idEstadoPlantilla As Integer) As List(Of ELL.CostGroupPlantilla)
            Return DAL.CostsGroupPlantillaDAL.loadlist(idEstadoPlantilla)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un cost group plantila
        ''' </summary>
        ''' <param name="costGroupPlantilla"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal costGroupPlantilla As ELL.CostGroupPlantilla)
            DAL.CostsGroupPlantillaDAL.Save(costGroupPlantilla)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un cost group plantilla en cascada
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Eliminar(ByVal id As Integer)
            DAL.CostsGroupPlantillaDAL.Delete(id)
        End Sub

#End Region

    End Class

End Namespace