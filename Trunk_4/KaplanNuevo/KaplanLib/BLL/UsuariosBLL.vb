Imports Oracle.DataAccess.Client

Namespace BLL

    Public Class UsuariosBLL

        Private usuariosDAL As New DAL.UsuariosDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un usuario
        ''' </summary>
        ''' <param name="idUsuario">Id del usuario</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ObtenerUsuario(ByVal idUsuario As Integer) As ELL.Usuarios
            Return usuariosDAL.getUsuario(idUsuario)
        End Function

        ''' <summary>
        ''' Obtiene el director de IKS
        ''' </summary>      
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ObtenerUsuarioDirector() As ELL.Usuarios
            Return usuariosDAL.getUsuarioDirector()
        End Function

        ''' <summary>
        ''' Obtiene un usuario
        ''' </summary>
        ''' <param name="idSab">Id del usuario</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ObtenerUsuarioSAB(ByVal idSab As Integer) As ELL.Usuarios
            Return usuariosDAL.getUsuarioSAB(idSab)
        End Function

        ''' <summary>
        ''' Obtiene un listado de usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarUsuarios(ByVal idRol As Integer) As List(Of ELL.Usuarios)
            Return usuariosDAL.loadList(idRol)
        End Function


        ''' <summary>
        ''' Obtiene un listado de usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarUsuario(ByVal idUsuarioTabla As Integer) As ELL.Usuarios
            Return usuariosDAL.getUsuario(idUsuarioTabla)
        End Function

        ''' <summary>
        ''' Obtiene un listado de usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarUsuariosSABTexto(ByVal query As String) As List(Of Sablib.ELL.Usuario)
            Dim SabComponent As New Sablib.BLL.UsuariosComponent
            Return SabComponent.GetUsuariosBusquedaSAB_Optimizado(query, conDirectorioActivo:=True)
        End Function

        ''' <summary>
        ''' Obtiene un usuario
        ''' </summary>
        ''' <param name="codPersona">Código persona</param>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function ObtenerUsuarioPorCodPersona(ByVal codPersona As Integer) As ELL.Usuarios
            Return usuariosDAL.getUsuarioByCodPersona(codPersona)
        End Function

        ''' <summary>
        ''' Obtiene el número de incidencias activas de un usuario
        ''' </summary>
        ''' <param name="idUsuario">Identificador de usuario</param>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function ObtenerNumeroIncidenciasActivasUsuario(ByVal idUsuario As Integer) As Integer
            Return usuariosDAL.getNumeroIncidenciasActivasUsuario(idUsuario)
        End Function

        ''' <summary>
        ''' Devuelve el identificador del usuario
        ''' </summary>
        ''' <param name="idUser"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarIdUsuario(ByVal idUser As Integer) As Integer
            Return usuariosDAL.getIdUsuario(idUser)
        End Function

        ''' <summary>
        ''' Devuelve los datos del usuario anónimo
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarUsuarioAnonimo() As Integer
            Return usuariosDAL.getUsuarioAnonimo()
        End Function

        ''' <summary>
        ''' Devuelve el correo del usuario anónimo
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarEmailUsuarioAnonimo(ByVal idIncidencia As Integer) As String
            Return usuariosDAL.getEmailUsuarioAnonimo(idIncidencia)
        End Function
#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guardar los datos de un usuario
        ''' </summary>
        ''' <param name="usuario">Objeto usuario</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarUsuario(ByVal usuario As ELL.Usuarios) As Boolean
            Return usuariosDAL.SaveUsuario(usuario)
        End Function

        ''' <summary>
        ''' Modifica los datos de un usuario
        ''' </summary>
        ''' <param name="usuario">Objeto usuario</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ModificarUsuario(ByVal usuario As ELL.Usuarios) As Boolean
            Return usuariosDAL.updateUsuario(usuario)
        End Function

#End Region

    End Class

End Namespace
