Imports SABLib_Z

Namespace BLL.Interface
    Public Interface IGruposComponent

        ''' <summary>
        ''' Obtiene un grupo para una cultura dada
        ''' </summary>
        ''' <param name="idGrupo">Identificador del grupo</param>
        ''' <param name="idCultura">Identificador de la cultura</param>
        ''' <returns>Devuelve una lista con los grupos que cumplen la condicion</returns>
        Function GetGrupoCultura(ByVal idGrupo As Integer, ByVal idCultura As String) As ELL.grupo

        ''' <summary>
        ''' Obtiene los grupos que cumplan las condiciones
        ''' </summary>
        ''' <param name="oGrupo">Grupo</param>
        ''' <returns>Devuelve una vista con los grupos que cumplen la condicion</returns>
        Function GetGruposCultura(ByVal oGrupo As ELL.grupo) As List(Of ELL.grupo)

        ''' <summary>
        ''' Obtiene todos los grupos de una cultura asignados a un recurso
        ''' </summary>
        ''' <param name="idRecurso">recurso al que estan asignados</param>
        ''' <param name="IdCultura">Identificador de la cultura</param>
        ''' <returns>Lista de grupos</returns>
        Function GetGruposCultura(ByVal idRecurso As Integer, ByVal IdCultura As String) As System.Collections.Generic.List(Of ELL.grupo)

        ''' <summary>
        ''' Obtiene los grupos, que pertenezcan a alguna planta de las de la lista
        ''' </summary>
        ''' <param name="lPlantas">Plantas a las que debe pertenecer un grupo</param>
        ''' <param name="IdCultura">Cultura en la que ha que mostrar los grupos</param>
        ''' <returns>Lista de usuarios</returns>        
        Function GetGruposCultura(ByVal lPlantas As List(Of Integer), ByVal idCultura As String) As List(Of ELL.grupo)

        ''' <summary>
        ''' Obtiene los usuarios de un grupo
        ''' </summary>
        ''' <param name="idGrupo">Identificador del grupo</param>
        ''' <returns>Lista de usuarios</returns>        
        Function GetUsuariosGrupo(ByVal idGrupo As Integer) As List(Of ELL.Usuario)

        ''' <summary>
        ''' Devuelve las plantas en las que esta asociado un grupo
        ''' </summary>
        ''' <param name="idGrupo">Identificador del grupo</param>
        ''' <returns>Lista de plantas</returns>
        ''' <remarks></remarks>
        Function GetPlantas(ByVal idGrupo As Integer) As System.Collections.Generic.List(Of ELL.Planta)

        ''' <summary>
        ''' Añade un grupo
        ''' </summary>        
        ''' <returns>Identificador del nuevo grupo</returns>
        Function AddGrupo() As Integer

        ''' <summary>
        ''' Añade un usuario a un grupo
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <param name="idGrupo">Identificador del grupo</param>
        Function AddUsuario(ByVal idUsuario As Integer, ByVal idGrupo As Integer) As Boolean

        ''' <summary>
        ''' Añade un recurso al grupo
        ''' </summary>
        ''' <param name="IdGrupo">Identificador del grupo</param>
        ''' <param name="idRecurso">Identificador del recurso</param>
        ''' <remarks>Booleano que indica si ha añadido correctamente</remarks>
        Function AddRecurso(ByVal IdGrupo As Integer, ByVal idRecurso As Integer) As Boolean

        ''' <summary>
        ''' Borra un grupo
        ''' </summary>
        ''' <param name="idGrupo">Identificador del Grupo</param>
        ''' <returns>Booleano indicando si se ha borrado correctamente</returns>
        Function Delete(ByVal idGrupo As Integer) As Boolean

        ''' <summary>
        ''' Borra un usuario de un grupo
        ''' </summary>
        ''' <param name="idGrupo">Identificador del grupo</param>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <returns>Booleano indicando si se ha borrado correctamente</returns>
        Function DeleteUsuario(ByVal idGrupo As Integer, ByVal idUsuario As Integer) As Boolean

        ''' <summary>
        ''' Borra el recurso del grupo
        ''' </summary>
        ''' <param name="idGrupo">Identificador del Grupo</param>
        ''' <param name="idRecurso">Identificador del recurso</param>
        ''' <returns>Booleano indicando si se ha borrado correctamente</returns>
        Function DeleteRecurso(ByVal idGrupo As Integer, ByVal idRecurso As String) As Boolean

        ''' <summary>
        ''' Inserta o modifica el termino del grupo en la cultura especificada
        ''' </summary>
        ''' <param name="oGrupo">Grupo</param>
        ''' <param name="bnuevo">Indica si hay que insertar o modificar</param>
        ''' <returns>Booleano</returns>
        Function SaveGrupoCultura(ByVal oGrupo As ELL.grupo, ByVal bnuevo As Boolean) As Boolean

        ''' <summary>
        ''' Guarda las traducciones de las distintas culturas de un grupo
        ''' </summary>
        ''' <param name="lGrupos">Lista de grupos a modificar</param>
        ''' <param name="lPlantas">Listado de plantas a guardar</param>
        ''' <param name="bnuevo">Indica si es un nuevo grupo</param> 
        ''' <returns>Identificador del grupo</returns>        
        Function ActualizarGruposCultura(ByVal lGrupos As List(Of ELL.grupo), ByVal lPlantas As List(Of ELL.Planta), ByVal bnuevo As Boolean) As Integer

    End Interface
End Namespace
