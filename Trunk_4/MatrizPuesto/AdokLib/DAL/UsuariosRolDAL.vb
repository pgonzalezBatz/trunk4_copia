Imports Oracle.DataAccess.Client
Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class UsuariosRolDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de usuarios roles
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function loadList() As List(Of ELL.UsuarioRol)
            Dim query As String = "SELECT ID_USUARIO, ID_ROL FROM USUARIOS_ROLES"

            Dim listaUsuariosRol As List(Of ELL.UsuarioRol) = Memcached.OracleDirectAccess.seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
            New ELL.UsuarioRol With {.IdRol = CInt(r("ID_ROL")), .IdUsuario = CInt(r("ID_USUARIO"))}, query, CadenaConexion)

            Return listaUsuariosRol
        End Function

        ''' <summary>
        ''' Obtiene un listado de usuarios roles
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function loadListUsuario(ByVal idUsuario As Integer) As List(Of ELL.UsuarioRol)
            Dim query As String = String.Empty
            Dim listaUsuariosRol As New List(Of ELL.UsuarioRol)

            query = "SELECT ID_USUARIO, ID_ROL FROM USUARIOS WHERE ID_USUARIO=:ID_USUARIO"
            Dim parameter2 As New OracleParameter("ID_USUARIO", OracleDbType.Int32, idUsuario, ParameterDirection.Input)

            listaUsuariosRol = Memcached.OracleDirectAccess.seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
            New ELL.UsuarioRol With {.IdRol = CInt(r("ID_ROL")), .IdUsuario = CInt(r("ID_USUARIO"))}, query, CadenaConexion, parameter2)

            Return listaUsuariosRol
        End Function

#End Region

    End Class

End Namespace