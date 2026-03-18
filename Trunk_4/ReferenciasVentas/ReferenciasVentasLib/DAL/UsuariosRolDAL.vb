Imports Oracle.DataAccess.Client

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
            Dim query As String = "SELECT ID_SAB, ID_ROL FROM USUARIOS_ROLES"
            
            Dim listaUsuariosRol As List(Of ELL.UsuarioRol) = Memcached.OracleDirectAccess.Seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
            New ELL.UsuarioRol With {.IdRol = CInt(r("ID_ROL")), .IdSab = CInt(r("ID_SAB"))}, query, CadenaConexion)

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

            query = "SELECT ID_SAB, ID_ROL FROM USUARIOS_ROLES WHERE ID_SAB=:ID_SAB"
            Dim parameter2 As New OracleParameter("ID_SAB", OracleDbType.Int32, idUsuario, ParameterDirection.Input)

            listaUsuariosRol = Memcached.OracleDirectAccess.Seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
            New ELL.UsuarioRol With {.IdRol = CInt(r("ID_ROL")), .IdSab = CInt(r("ID_SAB"))}, query, CadenaConexion, parameter2)

            Return listaUsuariosRol
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarUsuariosPorRol(ByVal idRol As Integer) As List(Of ELL.UsuarioRol)
            Dim query As String = String.Empty

            If (idRol <> 0) Then
                query = "SELECT ID_SAB, ID_ROL FROM USUARIOS_ROLES WHERE ID_ROL=:ID_ROL"

                Dim parameter As New OracleParameter("ID_ROL", OracleDbType.Int32, idRol, ParameterDirection.Input)

                Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
                New ELL.UsuarioRol With {.IdRol = CInt(r("ID_ROL")), .IdSab = CInt(r("ID_SAB"))}, query, CadenaConexion, parameter)
            Else
                query = "SELECT ID_SAB, ID_ROL FROM USUARIOS_ROLES"

                Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
               New ELL.UsuarioRol With {.IdRol = CInt(r("ID_ROL")), .IdSab = CInt(r("ID_SAB"))}, query, CadenaConexion)
            End If

        End Function

        ''' <summary>
        ''' Comprueba si existe una relación usuario rol
        ''' </summary>
        ''' <param name="usuarioRol">Objeto usuarioRol</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExisteUsuarioRol(ByVal usuarioRol As ELL.UsuarioRol) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM USUARIOS_ROLES WHERE ID_ROL=:ID_ROL AND ID_SAB=:ID_SAB"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_ROL", OracleDbType.Int32, usuarioRol.IdRol, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, usuarioRol.IdSab, ParameterDirection.Input))

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, lParameters.ToArray())
            Return filas > 0
        End Function

        ''' <summary>
        ''' Cargar los datos de un usuario
        ''' </summary>
        ''' <param name="idSab">Número de trabajador</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarUsuario(ByVal idSab As Integer) As ELL.UsuarioRol
            Dim query As String = "SELECT ID_SAB, ID_ROL FROM USUARIOS_ROLES WHERE ID_SAB=:ID_SAB"

            Dim parameter As New OracleParameter("ID_SAB", OracleDbType.Int32, idSab, ParameterDirection.Input)

            Dim usuarioRol As ELL.UsuarioRol = Memcached.OracleDirectAccess.Seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
            New ELL.UsuarioRol With {.IdRol = CInt(r("ID_ROL")), .IdSab = CInt(r("ID_SAB"))}, query, CadenaConexion, parameter).FirstOrDefault()

            Return usuarioRol
        End Function
#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta un nuevo rol al usuario
        ''' </summary>
        ''' <param name="oUsuarioRol">Objeto usuario rol con la informacion</param>
        Public Function GuardarUsuarioRol(ByRef oUsuarioRol As ELL.UsuarioRol) As Boolean
            Try
                Dim query As String = String.Empty
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_ROL", OracleDbType.Int32, oUsuarioRol.IdRol, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, oUsuarioRol.IdSab, ParameterDirection.Input))

                query = "INSERT INTO USUARIOS_ROLES (ID_ROL, ID_SAB)" _
                    & "VALUES (:ID_ROL, :ID_SAB)"

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Inserta un nuevo rol al usuario
        ''' </summary>
        ''' <param name="oUsuarioRol">Objeto usuario rol con la informacion</param>
        Public Function ModificarUsuarioRol(ByRef oUsuarioRol As ELL.UsuarioRol) As Boolean
            Try
                Dim query As String = "UPDATE USUARIOS_ROLES SET ID_ROL=:ID_ROL WHERE ID_SAB=:ID_SAB"

                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_ROL", OracleDbType.Int32, oUsuarioRol.IdRol, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, oUsuarioRol.IdSab, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un usuario rol
        ''' </summary>
        ''' <param name="idSab">Número de trabajador</param>
        Public Function EliminarUsuarioRol(ByVal idSab As Integer) As Boolean
            Try
                Dim query As String = "DELETE FROM USUARIOS_ROLES WHERE ID_SAB=:ID_SAB "
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, idSab, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

#End Region

    End Class

End Namespace