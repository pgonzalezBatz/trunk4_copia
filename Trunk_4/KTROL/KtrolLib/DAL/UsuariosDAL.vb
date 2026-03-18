Imports Oracle.ManagedDataAccess.Client
Imports System.Configuration

Namespace DAL

    Public Class UsuariosDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un usuario
        ''' </summary>
        ''' <param name="idUsuario">Id del usuario</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function getUsuario(ByVal idUsuario As Integer) As ELL.Usuarios
            Dim query As String = "SELECT ID, COD_PERSONA, ID_ROL FROM USUARIOS WHERE COD_PERSONA=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idUsuario, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Usuarios)(Function(r As OracleDataReader) _
            New ELL.Usuarios With {.Id = CInt(r("ID")), .CodPersona = SabLib.BLL.Utils.integerNull(r("COD_PERSONA")),
                                    .IdRol = SabLib.BLL.Utils.integerNull(r("ID_ROL"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un usuario
        ''' </summary>
        ''' <param name="idUsuario">Id del usuario</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function getUsuarioPorId(ByVal idUsuario As Integer) As ELL.Usuarios
            Dim query As String = "SELECT ID, COD_PERSONA, ID_ROL FROM USUARIOS WHERE ID=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idUsuario, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Usuarios)(Function(r As OracleDataReader) _
            New ELL.Usuarios With {.Id = CInt(r("ID")), .CodPersona = SabLib.BLL.Utils.integerNull(r("COD_PERSONA")),
                                    .IdRol = SabLib.BLL.Utils.integerNull(r("ID_ROL"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function loadList() As List(Of ELL.Usuarios)
            Dim query As String = "SELECT ID, COD_PERSONA FROM USUARIOS"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Usuarios)(Function(r As OracleDataReader) _
            New ELL.Usuarios With {.Id = CInt(r("ID")), .CodPersona = SabLib.BLL.Utils.integerNull(r("COD_PERSONA"))}, query, CadenaConexion)
        End Function

        ''' <summary>
        ''' Obtiene los datos de una persona por su código de empleado
        ''' </summary>
        ''' <param name="codPersona">Código de la persona</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getUsuarioByCodPersona(ByVal codPersona As Integer) As ELL.Usuarios
            Dim query As String = "SELECT ID, COD_PERSONA FROM USUARIOS WHERE COD_PERSONA=:COD_PERSONA"
            Dim parameter As New OracleParameter("COD_PERSONA", OracleDbType.Int32, codPersona, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Usuarios)(Function(r As OracleDataReader) _
            New ELL.Usuarios With {.Id = CInt(r("ID")), .CodPersona = SabLib.BLL.Utils.integerNull(r("COD_PERSONA"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Verifica si un usuario existe
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function existUsuario(ByVal idUsuario As Integer) As Integer
            Dim parameter As New OracleParameter("ID_USUARIO", OracleDbType.Int32, idUsuario, ParameterDirection.Input)

            Try
                Dim query As String = "SELECT COUNT(*) FROM USUARIOS WHERE COD_PERSONA=:COD_PERSONA"
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexion, parameter)
            Catch ex As Exception
                Return 0
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guardar un usuario nuevo
        ''' </summary>
        ''' <param name="usuario">Objeto Usuarios</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function saveUsuario(ByVal usuario As ELL.Usuarios) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO USUARIOS(COD_PERSONA, ID_ROL) VALUES(:COD_PERSONA,:ID_ROL)"
                lParameters1.Add(New OracleParameter("COD_PERSONA", OracleDbType.Int32, usuario.CodPersona, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_ROL", OracleDbType.Int16, usuario.IdRol, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Modifica los datos de un usuario
        ''' </summary>
        ''' <param name="usuario">Objeto Usuario</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function updateUsuario(ByVal usuario As ELL.Usuarios) As Boolean
            Dim resultado As Boolean = False
            Try
                Dim query As String = String.Empty
                Dim lParameters1 As New List(Of OracleParameter)

                query = "UPDATE USUARIOS SET ID_ROL=:ID_ROL WHERE COD_PERSONA=:COD_PERSONA"
                lParameters1.Add(New OracleParameter("COD_PERSONA", OracleDbType.Int32, usuario.CodPersona, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_ROL", OracleDbType.Int32, usuario.IdRol, ParameterDirection.Input))               

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado

        End Function

        ''' <summary>
        ''' Elimina un usuario
        ''' </summary>
        ''' <param name="idUsuario"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function deleteUsuario(ByVal idUsuario As Integer) As Boolean
            Dim query As String = String.Empty

            Try
                query = "DELETE FROM USUARIOS WHERE ID=:ID"
                Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idUsuario, ParameterDirection.Input)
                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, parameter)

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

#End Region

    End Class

End Namespace