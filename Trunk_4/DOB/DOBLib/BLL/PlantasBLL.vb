Imports DOBLib.DAL

Namespace BLL

    Public Class PlantasBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function ObtenerPlanta(ByVal idPlanta As Integer) As ELL.Planta
            Return PlantasDAL.getPlanta(idPlanta)
        End Function

        ''' <summary>
        ''' Obtiene un listado de plantas
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado() As List(Of ELL.Planta)
            Return PlantasDAL.loadList()
        End Function

        ''' <summary>
        ''' Obtiene 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerPlantaPadre(ByVal id As Integer) As Integer
            Dim planta As ELL.Planta = PlantasDAL.getPlanta(id)

            If (planta.IdPlantaPadre = Integer.MinValue OrElse Not planta.HeredaRetos) Then
                Return id
            Else
                Return ObtenerPlantaPadre(planta.IdPlantaPadre)
            End If
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Agregar una planta
        ''' </summary>
        ''' <param name="planta"></param> 
        Public Shared Sub AgregarPlanta(ByVal planta As ELL.Planta)
            PlantasDAL.AddPlanta(planta)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina una objeto
        ''' </summary>
        ''' <param name="id">Id</param>
        Public Shared Sub Eliminar(ByVal id As Integer)
            PlantasDAL.Delete(id)
        End Sub

        ''' <summary>
        ''' Marca como eliminado un objetivo
        ''' </summary>
        ''' <param name="planta"></param>
        ''' <param name="baja"></param> 
        Public Shared Sub DarBajaAlta(ByVal planta As ELL.Planta, ByVal baja As Boolean)
            PlantasDAL.MarkDeleted(planta, baja)
        End Sub

#End Region

    End Class

End Namespace