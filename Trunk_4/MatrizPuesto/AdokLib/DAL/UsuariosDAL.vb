Imports Oracle.DataAccess.Client
Imports System.Configuration
Imports Oracle.ManagedDataAccess.Client

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
            Dim query As String = "SELECT ID, NOMBRE, ID_USUARIO, ID_ROL, FECHA_CREACION, FECHA_MODIFICACION, OBSOLETO FROM USUARIOS WHERE ID=:ID ORDER BY NOMBRE ASC"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idUsuario, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Usuarios)(Function(r As OracleDataReader) _
            New ELL.Usuarios With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .CodPersona = SabLib.BLL.Utils.integerNull(r("ID_USUARIO")), .IdRol = SabLib.BLL.Utils.integerNull(r("ID_ROL")),
                                   .FechaAlta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_CREACION")), .FechaModificacion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_MODIFICACION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))},
                                   query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene los datos del director de IKS
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function getUsuarioDirector() As ELL.Usuarios
            Dim query As String = "SELECT ID, NOMBRE, ID_USUARIO, ID_ROL, FECHA_CREACION, FECHA_MODIFICACION, OBSOLETO FROM USUARIOS WHERE DIRECTOR=1"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Usuarios)(Function(r As OracleDataReader) _
            New ELL.Usuarios With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .CodPersona = SabLib.BLL.Utils.integerNull(r("ID_USUARIO")), .IdRol = SabLib.BLL.Utils.integerNull(r("ID_ROL")),
                                   .FechaAlta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_CREACION")), .FechaModificacion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_MODIFICACION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))},
                                   query, CadenaConexion).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un usuario
        ''' </summary>
        ''' <param name="idSab">Id del usuario</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function getUsuarioSAB(ByVal idSab As Integer) As ELL.Usuarios
            Dim query As String = "SELECT ID, NOMBRE, APELLIDO1, APELLIDO2, CODPERSONA, NOMBREUSUARIO FROM SAB.USUARIOS WHERE ID=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idSab, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Usuarios)(Function(r As OracleDataReader) _
            New ELL.Usuarios With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")),
                                 .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")), .NombreUsuario = SabLib.BLL.Utils.stringNull(r("NOMBREUSUARIO")), .CodPersona = SabLib.BLL.Utils.integerNull(r("CODPERSONA"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function loadList(ByVal idRol As Integer) As List(Of ELL.Usuarios)
            Dim query As String = String.Empty

            If (idRol <> 0) Then
                query = "SELECT USUARIOS.ID as ID, USUARIOS.NOMBRE as NOMBRE_USUARIO, ID_USUARIO, ID_ROL, USUARIOS.FECHA_CREACION, USUARIOS.FECHA_MODIFICACION, ROLES.NOMBRE as NOMBRE_ROL, OBSOLETO  FROM USUARIOS, ROLES WHERE ROLES.ID = USUARIOS.ID_ROL AND ID_ROL=:ID_ROL ORDER BY USUARIOS.NOMBRE ASC"

                Dim parameter As New OracleParameter("ID_ROL", OracleDbType.Int32, idRol, ParameterDirection.Input)

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Usuarios)(Function(r As OracleDataReader) _
                New ELL.Usuarios With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE_USUARIO")), .CodPersona = SabLib.BLL.Utils.integerNull(r("ID_USUARIO")), .IdRol = SabLib.BLL.Utils.integerNull(r("ID_ROL")), .NombreRol = SabLib.BLL.Utils.stringNull(r("NOMBRE_ROL")), .FechaAlta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_CREACION")), .FechaModificacion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_MODIFICACION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexion, parameter)
            Else
                query = "SELECT USUARIOS.ID as ID, USUARIOS.NOMBRE as NOMBRE_USUARIO, ID_USUARIO, ID_ROL, USUARIOS.FECHA_CREACION, USUARIOS.FECHA_MODIFICACION, ROLES.NOMBRE as NOMBRE_ROL, OBSOLETO FROM USUARIOS, ROLES WHERE ROLES.ID = USUARIOS.ID_ROL ORDER BY USUARIOS.NOMBRE ASC"

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Usuarios)(Function(r As OracleDataReader) _
               New ELL.Usuarios With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE_USUARIO")), .CodPersona = SabLib.BLL.Utils.integerNull(r("ID_USUARIO")), .IdRol = SabLib.BLL.Utils.integerNull(r("ID_ROL")), .NombreRol = SabLib.BLL.Utils.stringNull(r("NOMBRE_ROL")), .FechaAlta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_CREACION")), .FechaModificacion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_MODIFICACION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))}, query, CadenaConexion)
            End If

        End Function

        ''' <summary>
        ''' Obtiene los datos de una persona por su código de empleado
        ''' </summary>
        ''' <param name="codPersona">Código de la persona</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getUsuarioByCodPersona(ByVal codPersona As Integer) As ELL.Usuarios
            Dim query As String = "SELECT ID, NOMBRE, ID_USUARIO, ID_ROL, FECHA_CREACION, FECHA_MODIFICACION, OBSOLETO FROM USUARIOS WHERE ID_USUARIO=:ID_USUARIO ORDER BY NOMBRE ASC"
            Dim parameter As New OracleParameter("ID_USUARIO", OracleDbType.Int32, codPersona, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Usuarios)(Function(r As OracleDataReader) _
            New ELL.Usuarios With {.Id = CInt(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .CodPersona = SabLib.BLL.Utils.integerNull(r("ID_USUARIO")), .IdRol = SabLib.BLL.Utils.integerNull(r("ID_ROL")),
                                   .FechaAlta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_CREACION")), .FechaModificacion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_MODIFICACION")), .Obsoleto = SabLib.BLL.Utils.booleanNull(r("OBSOLETO"))},
                                   query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene el número de incidencias activas de un usuario
        ''' </summary>
        ''' <param name="idUsuario"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getNumeroIncidenciasActivasUsuario(ByVal idUsuario As Integer) As Integer
            Dim lParameters1 As New List(Of OracleParameter)
            lParameters1.Add(New OracleParameter("ID_RESPONSABLE", OracleDbType.Int32, idUsuario, ParameterDirection.Input))
            lParameters1.Add(New OracleParameter("ID_ESTADO_ACTIVE", OracleDbType.Int16, ELL.EstadosIncidencia.EstadoIncidencia.Active, ParameterDirection.Input))
            lParameters1.Add(New OracleParameter("ID_ESTADO_FILTERED", OracleDbType.Int16, ELL.EstadosIncidencia.EstadoIncidencia.Filtered, ParameterDirection.Input))

            Try
                Dim query As String =
                    "SELECT COUNT(*) FROM INCIDENCIAS WHERE (ID_ESTADO=:ID_ESTADO_ACTIVE OR ID_ESTADO=:ID_ESTADO_FILTERED) " & _
                    "AND (ID_RESPONSABLE=:ID_RESPONSABLE " & _
                    "OR " & _
                    "(INCIDENCIAS.ID IN (SELECT INCIDENCIAS_GRUPOS.ID_INCIDENCIA " & _
                    "FROM GRUPOS_USUARIOS " & _
                    "INNER JOIN USUARIOS on USUARIOS.ID=GRUPOS_USUARIOS.ID_USUARIO " & _
                    "LEFT JOIN INCIDENCIAS_GRUPOS on GRUPOS_USUARIOS.ID_GRUPO = INCIDENCIAS_GRUPOS.ID_GRUPO " & _
                    "WHERE GRUPOS_USUARIOS.ID_USUARIO=:ID_RESPONSABLE)))"
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexion, lParameters1.ToArray())
            Catch ex As Exception
                Return 0
            End Try
        End Function

        ''' <summary>
        ''' Devuelve el Id interno del usuario
        ''' </summary>
        ''' <param name="idUsuario"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getIdUsuario(ByVal idUsuario As Integer) As Integer
            Dim query As String = "SELECT ID FROM USUARIOS WHERE ID_USUARIO=:ID_USUARIO"
            Dim parameter As New OracleParameter("ID_USUARIO", OracleDbType.Int32, idUsuario, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexion, parameter)
        End Function

        ''' <summary>
        ''' Devuelve el Id del usuario anónimo
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getUsuarioAnonimo() As Integer
            Dim query As String = "SELECT ID FROM SAB.USUARIOS WHERE NOMBREUSUARIO='anonimo'"

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString(), CadenaConexion)
        End Function

        ''' <summary>
        ''' Devuelve el email del usuario anónimo
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getEmailUsuarioAnonimo(ByVal idIncidencia As Integer) As String
            Dim query As String = "SELECT EMAIL FROM ANONIMO_EMAIL WHERE ID_INCIDENCIA=:ID_INCIDENCIA"
            Dim parameter As New OracleParameter("ID_INCIDENCIA", OracleDbType.Int32, idIncidencia, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query.ToString(), CadenaConexion, parameter)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Modifica los datos de un usuario
        ''' </summary>
        ''' <param name="usuario">Objeto Usuario</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function saveUsuario(ByVal usuario As ELL.Usuarios) As Boolean
            Dim resultado As Boolean = False
            Try
                Dim query As String = String.Empty
                Dim lParameters1 As New List(Of OracleParameter)

                query = "INSERT INTO USUARIOS (NOMBRE, ID_ROL, FECHA_MODIFICACION, ID_USUARIO, DIRECTOR, OBSOLETO) VALUES (:NOMBRE, :ID_ROL, :FECHA_MODIFICACION, :ID_USUARIO, :DIRECTOR, :OBSOLETO)"

                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 100, usuario.Nombre, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_ROL", OracleDbType.Int32, usuario.IdRol, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FECHA_MODIFICACION", OracleDbType.Date, usuario.FechaModificacion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, usuario.CodPersona, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("DIRECTOR", OracleDbType.Int16, 1, If(usuario.DirectorIKS, 1, 0), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("OBSOLETO", OracleDbType.Int16, If(usuario.Obsoleto, 1, 0), ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado

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

                query = "UPDATE USUARIOS SET ID_ROL=:ID_ROL, FECHA_MODIFICACION=:FECHA_MODIFICACION, OBSOLETO=:OBSOLETO WHERE ID_USUARIO=:ID_USUARIO"
                lParameters1.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, usuario.CodPersona, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_ROL", OracleDbType.Int32, usuario.IdRol, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("FECHA_MODIFICACION", OracleDbType.Date, usuario.FechaModificacion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("OBSOLETO", OracleDbType.Int16, usuario.Obsoleto, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado

        End Function

#End Region

    End Class

End Namespace