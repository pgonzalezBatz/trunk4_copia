Imports Oracle.ManagedDataAccess.Client

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
        ''' Obtiene un usuario
        ''' </summary>
        ''' <param name="idUsuario">Id del usuario</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ObtenerUsuarioPorId(ByVal idUsuario As Integer) As ELL.Usuarios
            Return usuariosDAL.getUsuarioPorId(idUsuario)
        End Function

        ''' <summary>
        ''' Obtiene un listado de usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarUsuarios() As List(Of ELL.Usuarios)
            Return usuariosDAL.loadList()
        End Function

        ''' <summary>
        ''' Verificar si un usuario existe en la aplicación
        ''' </summary>
        ''' <param name="idUsuario"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExisteUsuario(ByVal idUsuario As Integer) As Boolean
            If (usuariosDAL.existUsuario(idUsuario) > 0) Then
                Return True
            Else
                Return False
            End If
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un usuario nuevo
        ''' </summary>
        ''' <param name="usuario">Objeto usuario</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarUsuario(ByVal usuario As ELL.Usuarios) As Integer
            Return usuariosDAL.saveUsuario(usuario)
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

        ''' <summary>
        ''' Elimina un usuario
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminarUsuario(ByVal idUsuario As Integer) As Boolean
            Return usuariosDAL.deleteUsuario(idUsuario)
        End Function

#End Region

    End Class

End Namespace
