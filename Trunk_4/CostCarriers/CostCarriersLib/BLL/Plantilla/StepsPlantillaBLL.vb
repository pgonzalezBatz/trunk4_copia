Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class StepsPlantillaBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un step plantilla
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.StepPlantilla
            Return DAL.StepsPlantillaDAL.getStepPlantilla(id)
        End Function

        ''' <summary>
        ''' Obtiene steps plantilla
        ''' </summary>
        ''' <param name="idCostGroupPlantilla"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal idCostGroupPlantilla As Integer) As List(Of ELL.StepPlantilla)
            Return DAL.StepsPlantillaDAL.loadList(idCostGroupPlantilla)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un step plantila
        ''' </summary>
        ''' <param name="stepPlantilla"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal stepPlantilla As ELL.StepPlantilla)
            DAL.StepsPlantillaDAL.Save(stepPlantilla)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ordenActual"></param>
        Public Shared Sub CambiarOrdenSteps(ByVal ordenActual() As Integer)
            DAL.StepsPlantillaDAL.SwapOrderSteps(ordenActual)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un step plantilla en cascada
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Eliminar(ByVal id As Integer)
            DAL.StepsPlantillaDAL.Delete(id)
        End Sub

#End Region

    End Class

End Namespace