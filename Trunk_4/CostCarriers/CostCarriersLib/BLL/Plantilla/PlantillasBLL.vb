Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class PlantillasBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una plantilla
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.Plantilla
            Return DAL.PlantillasDAL.getPlantilla(id)
        End Function

        ''' <summary>
        ''' Obtiene una plantilla
        ''' </summary>
        ''' <param name="idTipoProyecto"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerPorTipoProyecto(ByVal idTipoProyecto As Integer) As ELL.Plantilla
            Return DAL.PlantillasDAL.getPlantillaByTipoProyecto(idTipoProyecto)
        End Function

        ''' <summary>
        ''' Obtiene todas las plantillas
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado() As List(Of ELL.Plantilla)
            Return DAL.PlantillasDAL.loadList()
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda una plantilla
        ''' </summary>
        ''' <param name="plantilla"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal plantilla As ELL.Plantilla)
            DAL.PlantillasDAL.Save(plantilla)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un plantilla en cascada
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Eliminar(ByVal id As Integer)
            DAL.PlantillasDAL.Delete(id)
        End Sub

#End Region

    End Class

End Namespace