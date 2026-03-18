Imports System.Collections.Generic

Namespace BLL.Interface
    Public Interface IPlantasComponent

        ''' <summary>
        ''' Devuelve la informacion de la planta
        ''' </summary>
        ''' <param name="idPlanta">Identificador de la planta </param>
        ''' <returns>Objeto planta</returns>
        ''' <remarks></remarks>
        Function GetPlanta(ByVal idPlanta As Integer) As ELL.Planta

        ''' <summary>
        ''' Devuelve las plantas existentes
        ''' </summary>
        ''' <returns>Lista de plantas</returns>
        ''' <remarks></remarks>
        Function GetPlantas() As System.Collections.Generic.List(Of ELL.Planta)

        ''' <summary>
        ''' Devuelve las plantas en las que esta asociado un usuario
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <returns>Lista de plantas</returns>
        ''' <remarks></remarks>
        Function GetPlantasUsuario(ByVal idUsuario As Integer) As System.Collections.Generic.List(Of ELL.Planta)

        ''' <summary>
        ''' Inserta o modifica la planta
        ''' </summary>
        ''' <param name="oPlanta">Planta a guardar o modificar</param>        
        ''' <returns>Booleano</returns>
        Function Save(ByVal oPlanta As ELL.Planta) As Boolean

        ''' <summary>
        ''' Marca como obsoleto la planta
        ''' </summary>
        ''' <param name="idPlant">Identificador de la planta</param>
        ''' <returns>Booleano que indica si se ha eliminado o no</returns>        
        Function Delete(ByVal idPlant As Integer) As Boolean

    End Interface
End Namespace