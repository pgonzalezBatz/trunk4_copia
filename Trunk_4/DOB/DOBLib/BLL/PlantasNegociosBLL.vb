Imports DOBLib.DAL

Namespace BLL

    Public Class PlantasNegociosBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una planta/negocio
        ''' </summary>
        ''' <param name="idPlantaNegocio"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function ObtenerPlantaNegocio(ByVal idPlantaNegocio As Integer) As ELL.PlantaNegocio
            Return PlantasNegociosDAL.getPlantaNegocio(idPlantaNegocio)
        End Function

        ''' <summary>
        ''' Obtiene un listado de plantas/negocios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Cargarlistado() As List(Of ELL.PlantaNegocio)
            Return PlantasNegociosDAL.loadList()
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta una planta/negocio
        ''' </summary>
        ''' <param name="oObjeto"></param> 
        Public Shared Function Guardar(ByVal oObjeto As ELL.PlantaNegocio) As Integer
            Return PlantasNegociosDAL.SavePlantaNegocio(oObjeto)
        End Function

        ''' <summary>
        ''' Comprueba si existe una planta negocio y gerente
        ''' </summary>
        ''' <param name="idNegocio"></param>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Shared Function Existe(ByVal idNegocio As Integer, ByVal idPlanta As Integer) As Boolean
            Return PlantasNegociosDAL.existsPlantaNegocio(idNegocio, idPlanta)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Shared Function ExistePlanta(ByVal idPlanta As Integer) As Boolean
            Return PlantasNegociosDAL.existsPlanta(idPlanta)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idNegocio"></param>
        ''' <returns></returns>
        Public Shared Function ExisteNegocio(ByVal idNegocio As Integer) As Boolean
            Return PlantasNegociosDAL.existsNegocio(idNegocio)
        End Function

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina una objeto
        ''' </summary>
        ''' <param name="id">Id</param>
        Public Shared Sub Eliminar(ByVal id As Integer)
            PlantasNegociosDAL.Delete(id)
        End Sub

#End Region

    End Class

End Namespace