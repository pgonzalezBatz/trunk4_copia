Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class EstadosPlantillaBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un estado plantilla
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.EstadoPlantilla
            Return DAL.EstadosPlantillaDAL.getEstadoPlantilla(id)
        End Function

        ''' <summary>
        ''' Obtiene esatdos plantilla por planta plantilla
        ''' </summary>
        ''' <param name="idPlantaPlantilla"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal idPlantaPlantilla As Integer) As List(Of ELL.EstadoPlantilla)
            Return DAL.EstadosPlantillaDAL.loadlist(idPlantaPlantilla)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un estado plantilla
        ''' </summary>
        ''' <param name="estadoPlantilla"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal estadoPlantilla As ELL.EstadoPlantilla)
            DAL.EstadosPlantillaDAL.Save(estadoPlantilla)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un estado plantilla en cascada
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Eliminar(ByVal id As Integer)
            DAL.EstadosPlantillaDAL.Delete(id)
        End Sub

#End Region

    End Class

End Namespace