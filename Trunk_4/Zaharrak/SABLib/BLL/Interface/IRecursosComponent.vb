Namespace BLL.Interface
    Public Interface IRecursosComponent

        ''' <summary>
        ''' Obtiene el recurso asociado
        ''' </summary>
        ''' <param name="IdRecurso">Identificador de la cultura</param>
        ''' <returns>Objeto recursos</returns>
        Function GetRecurso(ByVal IdRecurso As Integer) As ELL.recurso

        ''' <summary>
        ''' Obtiene un recurso para una cultura dada
        ''' </summary>
        ''' <param name="idRecurso">Identificador del recurso</param>
        ''' <param name="idCultura">Identificador de la cultura</param>
        ''' <returns>Devuelve una lista con los grupos que cumplen la condicion</returns>
        Function GetRecursoCultura(ByVal idRecurso As Integer, ByVal idCultura As String) As ELL.recurso

        ''' <summary>
        ''' Obtiene los recursos que cumplan las condiciones
        ''' </summary>
        ''' <param name="oRecurso">Recurso</param>
        ''' <param name="bConIcono">Indicara si tiene que devolver el conjunto de bytes del icono</param>
        ''' <returns>Devuelve una lista con los recurso que cumplen la condicion</returns>
        Function GetRecursosCultura(ByVal oRecurso As ELL.recurso, Optional ByVal bConIcono As Boolean = False) As List(Of ELL.recurso)

        ''' <summary>
        ''' Obtiene todos los recursos de una cultura asignados a un grupo
        ''' </summary>
        ''' <param name="idGrupo">Grupo al que estan asignados</param>
        ''' <param name="IdCultura">Identificador de la cultura</param>
        ''' <returns>Lista de recursos</returns>
		Function GetRecursosCultura(ByVal idGrupo As Integer, ByVal IdCultura As String) As List(Of ELL.recurso)

		''' <summary>
		''' Obtiene todos los recursos culturizados de un usuario
		''' </summary>
		''' <param name="idUser">Id del usuario</param>
		''' <param name="idCultura">Cultura</param>
		''' <returns>Devuelve una lista con los recursos que tiene acceso un usuario</returns>
		Function GetRecursosCulturaAll(ByVal idUser As Integer, ByVal idCultura As String) As System.Collections.Generic.List(Of ELL.recurso)

        ''' <summary>
        ''' Añade recurso para todas las culturas existentes
        ''' Devuelve el id del recurso añadido. Devuelve -1 si ha habido algun error
        ''' </summary>
        ''' <param name="nombreRecurso">Nombre del recurso</param>
        ''' <param name="objRecurso">Objeto recurso que contiene los datos</param>
        ''' <returns>Booleano que indica si ha añadido correctamente</returns>
        Function AddRecurso(ByVal nombreRecurso As String, ByVal objRecurso As ELL.recurso) As Integer

        ''' <summary>
        ''' Elimina el recurso especificado
        ''' </summary>
        ''' <param name="idRecurso">Identificador del recurso</param>
        ''' <returns>Booleano que indica si se ha eliminado correctamente</returns>
        Function Delete(ByVal idRecurso As Integer) As Boolean

        ''' <summary>
        ''' Elimina la imagen de un recurso
        ''' </summary>        
        ''' <param name="idRecurso">Identificador del recurso</param>        
        ''' <returns>Booleano que indica si se ha borrado la imagen correctatemente</returns>
		Function DeleteImageRecurso(ByVal idRecurso As Integer) As Boolean

		''' <summary>
		''' Elimina el fichero de un recurso
		''' </summary>        
		''' <param name="idRecurso">Identificador del recurso</param>        
		''' <returns>Booleano que indica si se ha borrado la imagen correctatemente</returns>
		Function DeleteFicheroRecurso(ByVal idRecurso As Integer) As Boolean

        ''' <summary>
        ''' Guarda un recurso
        ''' </summary>
        ''' <param name="objRec">Objeto recurso que tiene los datos</param>
        ''' <returns>Entero con el id del recurso</returns>
        Function SaveRecurso(ByVal objRec As ELL.recurso) As Integer

        ''' <summary>
        ''' Inserta o modifica el termino del recurso en la cultura especificada
        ''' </summary>
        ''' <param name="oRecurso">Recurso</param>
        ''' <param name="bnuevo">Indica si hay que insertar o modificar</param>
        ''' <returns>Booleano</returns>
        Function SaveRecursoCultura(ByVal oRecurso As ELL.recurso, ByVal bnuevo As Boolean) As Boolean

        ''' <summary>
        ''' Guarda las traducciones de las distintas culturas de un recurso y el resto de datos
        ''' Si viene informado una traduccion y no existe se inserta, sino, se actualiza
        ''' </summary>
        ''' <param name="lRecursos">Lista de recursos a modificar</param>
        ''' <param name="oRecurso">Recurso con los datos</param>
        ''' <returns>Devuelve el idGrupo</returns>  
        Function ActualizarRecursosCultura(ByVal lRecursos As System.Collections.Generic.List(Of ELL.recurso), ByVal oRecurso As ELL.recurso) As Integer


    End Interface

End Namespace
