Namespace BLL.Interface
    Public Interface IUsuariosComponent

        ''' <summary>
        ''' Devuelve el usuario que tenga idUsuario como id
        ''' </summary>
        ''' <param name="idUsuario"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetUsuario(ByVal idUsuario As Integer, Optional ByVal foto As Boolean = False) As ELL.Usuario

        ''' <summary>
        ''' Devuelve el usuario que cumpla las condiciones del objeto
        ''' </summary>
        ''' <param name="oUser"></param>
        ''' <returns></returns>
        Function GetUsuario(ByVal oUser As ELL.Usuario, Optional ByVal Vigentes As Boolean = True, Optional ByVal foto As Boolean = False) As ELL.Usuario

		''' <summary>
		''' Obtiene los grupos de un usuario y una cultura
		''' </summary>
		''' <param name="idUsuario">Identificador del usuario</param>
		''' <param name="idCultura">Identificador de la cultura</param>
		''' <returns>Vista con los grupos</returns>        
		Function GetGruposUsuario(ByVal idUsuario As Integer, ByVal idCultura As String) As List(Of ELL.grupo)

		''' <summary>
		''' Obtiene los grupos de un usuario y una cultura de todos los usuarios, aunque esten dados de baja
		''' </summary>
		''' <param name="idUsuario">Identificador del usuario</param>
		''' <param name="idCultura">Identificador de la cultura</param>
		''' <returns>Vista con los grupos</returns>        
		Function GetGruposUsuarioAll(ByVal idUsuario As Integer, ByVal idCultura As String) As List(Of ELL.grupo)

        ''' <summary>
        ''' Obtiene los usuarios que cumplan las condiciones del objeto ordenados por un campo
        ''' </summary>
        ''' <param name="oUser">Objeto usuario</param>
        ''' <param name="vigentes">Parametro opcional para mostrar todos los usuarios o solo vigentes</param>
        ''' <param name="sortField">Campo a ordenar</param>
        ''' <returns>Lista de usuarios</returns> 
        Function GetUsuarios(ByVal oUser As ELL.Usuario, Optional ByVal vigentes As Boolean = True, Optional ByVal sortField As String = DAL.USUARIOS.ColumnNames.NOMBREUSUARIO) As List(Of ELL.Usuario)

        ''' <summary>
        ''' Obtiene los usuarios activos, que pertenezcan a alguna planta de las de la lista
        ''' </summary>
        ''' <param name="lPlantas">Plantas a las que debe pertenecer un usuario</param>
        ''' <returns>Lista de usuarios</returns>        
        Function GetUsuarios(ByVal lPlantas As List(Of Integer)) As List(Of ELL.Usuario)

        ''' <summary>
        ''' Obtiene los usuarios activos que cumplan las condiciones del objeto y que pertenezcan a alguna planta de las de la lista
        ''' </summary>
        ''' <param name="oUser">Objeto usuario con las condiciones</param>
        ''' <returns>Lista de usuarios</returns>        
		Function GetUsuariosPlanta(ByVal oUser As SABLib_Z.ELL.Usuario) As List(Of ELL.Usuario)

        ''' <summary>
        ''' Realiza una busqueda de usuarios que tengan acceso a un recurso
        ''' </summary>
		''' <param name="idRecurso">Recurso</param>
		''' <param name="vigentes">Indica si se listaran todos o solo los vigentes</param>
		''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>Lista de usuarios</returns>
        ''' <remarks></remarks>
		Function GetUsuariosConRecurso(ByVal idRecurso As Integer, Optional ByVal vigentes As Boolean = False, Optional ByVal idPlanta As Integer = Integer.MinValue) As List(Of ELL.Usuario)

        ''' <summary>
        ''' Realiza una busqueda de usuarios a partir de un texto en la aplicacion de SAB
        ''' </summary>
        ''' <param name="texto">Texto a buscar</param>
        ''' <returns>Lista de usuarios</returns>
        ''' <remarks></remarks>
		Function GetUsuariosBusquedaSAB(ByVal texto As String) As List(Of ELL.Usuario)

		''' <summary>
		''' Realiza una busqueda de usuarios a partir de varias condiciones. Es para utilizarlo con el nuevo control de seleccion de usuarios
		''' </summary>
		''' <param name="texto">Texto a buscar</param>
		''' <param name="bConIdDirectorioActivo">Indica si tendra que tener IdDirectorioActivo o no</param>
		''' <param name="bConEmail">Indica si debe tener email</param>
		''' <param name="bConCodPersona">Indica si debe tener codigo de persona</param>
		''' <param name="bVigentes">Indica si se quieren solo los vigentes, o todos</param>
		''' <param name="idEmpresa">Se puede indicar el id de la empresa</param>		
		''' <returns>Lista de usuarios</returns>      
		Function GetUsuariosBusquedaSAB(ByVal texto As String, ByVal bConIdDirectorioActivo As Nullable(Of Boolean), ByVal bConEmail As Nullable(Of Boolean), ByVal bConCodPersona As Nullable(Of Boolean), ByVal bVigentes As Boolean, Optional ByVal idEmpresa As Integer = Integer.MinValue) As List(Of ELL.Usuario)

        ''' <summary>
		''' Realiza una busqueda de usuarios a partir de un texto en la aplicacion de SAB con un algoritmo mas eficaz.Si no se indica nada en el texto, no obtiene ninguno todos
        ''' </summary>
        ''' <param name="texto">Texto a buscar</param>
		''' <param name="recurso">Parametro opcional indicando el recurso.Si se indica el recurso, la lista de usuarios que se obtiene para la busqueda, sera la de usuarios que pertenecen a ese recurso</param>
		''' <param name="bGetTodosSiTextoVacio">Parametro opcional para indicar que si el texto que se manda es vacio, obtenga todos los usuarios encontrados</param>
        ''' <returns>Lista de usuarios</returns>
        ''' <remarks></remarks>
		Function GetUsuariosBusquedaSAB2(ByVal texto As String, Optional ByVal recurso As Integer = Integer.MinValue, Optional ByVal bGetTodosSiTextoVacio As Boolean = False) As List(Of ELL.Usuario)

        ''' <summary>
        ''' Devuelve las plantas en las que esta asociado un usuario
        ''' </summary>
        ''' <param name="idUser">Identificador del usuario</param>
        ''' <returns>Lista de plantas</returns>
        ''' <remarks></remarks>
        Function GetPlantas(ByVal idUser As Integer) As System.Collections.Generic.List(Of ELL.Planta)

        ''' <summary>
        ''' Borra el usuario
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <returns>Booleano indicando si se ha borrado correctamente</returns>
        ''' <remarks></remarks>
        Function Delete(ByVal idUsuario As Integer) As Boolean

        ''' <summary>
        ''' Guarda los datos del usuario y la planta en el caso en que sea nuevo
        ''' </summary>        
        ''' <param name="objUsuario">Objeto usuario a guardar</param>
        ''' <returns>Identificador del usuario</returns>
        Function Save(ByVal objUsuario As ELL.Usuario) As Integer

        ''' <summary>
        ''' Guarda la password encriptada
        ''' </summary>        
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="password">Password</param>
        ''' <returns>Booleano que indica si se ha guardado correctamente</returns>
        Function SavePassword(ByVal idUser As Integer, ByVal password As String) As Boolean

        ''' <summary>
        ''' Guarda la foto
        ''' </summary>        
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="foto">Foto</param>
        ''' <returns>Booleano que indica si se ha guardado correctamente</returns>
        Function SaveFoto(ByVal idUser As Integer, ByVal foto As Byte()) As Boolean

        ''' <summary>
        ''' Guarda el nombre de usuario en SAB y en IZARO
        ''' </summary>        
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="nombre">Nombre de usuario a guardar</param>
        ''' <returns>Booleano que indica si se ha guardado correctamente</returns>
		Function SaveUserSABIzaro(ByVal idUser As Integer, ByVal nombre As String) As Boolean

		''' <summary>
		''' Guarda la nueva fecha de baja de usuario en SAB y en IZARO
		''' </summary>        
		''' <param name="idUser">Id del usuario</param>
		''' <param name="fechaBaja">Fecha de baja</param>
		''' <returns>Booleano que indica si se ha guardado correctamente</returns>
		Function SaveFechaBajaUserSABIzaro(ByVal idUser As Integer, ByVal fechaBaja As Date) As Boolean

        ''' <summary>
        ''' Añade la planta al usuario
        ''' </summary>        
        ''' <param name="idUsuario">Id del usuario</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>Booleano que indica si se ha guardado correctamente</returns>
        Function AddPlanta(ByVal idUsuario As Integer, ByVal idPlanta As Integer) As Boolean

        ''' <summary>
        ''' Actualiza los datos necesarios en la creacion de usuarios en KEM
        ''' </summary>        
        ''' <param name="objUsuario">Objeto usuario a guardar</param>
        Function updateKEM(ByVal objUsuario As ELL.Usuario) As Boolean

        ''' <summary>
        ''' Elimina la planta al usuario
        ''' </summary>        
        ''' <param name="idUsuario">Id del usuario</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>Booleano que indica si se ha borrado correctamente</returns>
		Function DeletePlanta(ByVal idUsuario As Integer, ByVal idPlanta As Integer) As Boolean

		''' <summary>
		''' Cambia la password del usuario en LDAP (Itxina)
		''' </summary>
		''' <param name="email">Email</param>
		''' <param name="currentPassword">Password actual</param>
		''' <param name="newPassword">Nueva contraseña</param>		
		Sub ChangePasswordLDAP(ByVal email As String, ByVal currentPassword As String, ByVal newPassword As String)

		''' <summary>
		''' Devuelve los usuarios con el recurso idRecurso y que no pertenezca a Batz
		''' </summary>
		''' <param name="idRecurso"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetProveedoresConRecurso(ByVal idRecurso As Integer) As System.Collections.Generic.List(Of ELL.Empresa)

		''' <summary>
		''' Importa los datos seleccionados (se indican el nombre del campo a actualizar) del usuario origen al destino
		''' </summary>
		''' <param name="idUserOrigen">Id del usuario origen. Del que se importan</param>
		''' <param name="idUserDestino">Id del usuario destino. Al que se importan</param>
		''' <param name="datos">Array con los nombres de las columnas. Indica los campos a copiar</param>
		''' <returns></returns>		
		Function ImportarDatos(ByVal idUserOrigen As Integer, ByVal idUserDestino As Integer, ByVal datos As String()) As Boolean

    End Interface
End Namespace