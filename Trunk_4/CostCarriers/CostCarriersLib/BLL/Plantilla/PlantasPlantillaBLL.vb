Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class PlantasPlantillaBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una planta plantilla
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.PlantaPlantilla
            Return DAL.PlantasPlantillaDAL.getPlantaPlantilla(id)
        End Function

        ''' <summary>
        ''' Obtiene plantas plantilla por plantilla
        ''' </summary>
        ''' <param name="idPlantilla"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal idPlantilla As Integer) As List(Of ELL.PlantaPlantilla)
            Return DAL.PlantasPlantillaDAL.loadlist(idPlantilla)
        End Function

        ''' <summary>
        ''' Obtiene plantas plantilla por plantilla
        ''' </summary>
        ''' <param name="idTipoProyecto"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorTipoProyecto(ByVal idTipoProyecto As Integer) As List(Of ELL.PlantaPlantilla)
            Return DAL.PlantasPlantillaDAL.loadListByIdTipoProyecto(idTipoProyecto)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda una planta plantilla
        ''' </summary>
        ''' <param name="plantaPlantilla"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal plantaPlantilla As ELL.PlantaPlantilla)
            DAL.PlantasPlantillaDAL.Save(plantaPlantilla)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un planta plantilla en cascada
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Eliminar(ByVal id As Integer)
            DAL.PlantasPlantillaDAL.Delete(id)
        End Sub

#End Region

    End Class

End Namespace