Imports DOBLib.DAL

Namespace BLL

    Public Class PlantasDespliegueBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado plantas
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado() As List(Of ELL.PlantasDespliegue)
            Return PlantasDespliegueDAL.loadList()
        End Function

        ''' <summary>
        ''' Obtiene un listado plantas
        ''' </summary>
        ''' <param name="idPlantaPadre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListadoPorPlantaPadre(ByVal idPlantaPadre As Integer) As List(Of ELL.PlantasDespliegue)
            Return PlantasDespliegueDAL.loadListByPlantaPadre(idPlantaPadre)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda las plantas despliegue
        ''' </summary>
        ''' <param name="idPlantaPadre"></param>
        ''' <param name="plantaHijas"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal idPlantaPadre As Integer, ByVal plantaHijas As List(Of Integer))
            PlantasDespliegueDAL.Save(idPlantaPadre, plantaHijas)
        End Sub

#End Region

    End Class

End Namespace