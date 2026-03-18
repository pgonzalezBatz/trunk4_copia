Imports Oracle.DataAccess.Client

Namespace BLL

    Public Class UsuariosRolBLL

        Private usuariosRolesDAL As New DAL.UsuariosRolDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de usuario roles
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarUsuarioRol(Optional ByVal idUsuario As Nullable(Of Integer) = Nothing) As List(Of ELL.UsuarioRol)
            If (idUsuario Is Nothing) Then
                Return usuariosRolesDAL.loadList()
            Else
                Return usuariosRolesDAL.loadListUsuario(idUsuario)
            End If
        End Function

#End Region

    End Class

End Namespace
