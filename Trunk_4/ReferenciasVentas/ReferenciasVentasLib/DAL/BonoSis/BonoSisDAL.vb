Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class BonoSisDAL
        Inherits DALBase

#Region "PTKSis - Proyectos"

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function loadList(ByVal texto As String) As List(Of ELL.Proyectos)
            Dim status As String = "BONOSISTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BONOSISLIVE"
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString

            Dim query As String = "SELECT ID, NOMBRE FROM PROYECTOS WHERE LOWER(NOMBRE) LIKE '%' || :TEXTO || '%'  ORDER BY NOMBRE ASC"
            Dim parameter As New OracleParameter("TEXTO", OracleDbType.Varchar2, 20, texto, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.Proyectos)(Function(r As OracleDataReader) _
            New ELL.Proyectos With {.Id = SabLib.BLL.Utils.stringNull(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE"))}, query, conexion, parameter)
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarProyectoPorId(ByVal idProyecto As String) As ELL.Proyectos
            Dim status As String = "BONOSISTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BONOSISLIVE"
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString

            Dim query As String = "SELECT ID, NOMBRE FROM PROYECTOS WHERE ID=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.NVarchar2, 30, idProyecto, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.Proyectos)(Function(r As OracleDataReader) _
            New ELL.Proyectos With {.Id = SabLib.BLL.Utils.stringNull(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE"))}, query, conexion, parameter).FirstOrDefault
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarProyecto(ByVal texto As String) As ELL.Proyectos
            Dim status As String = "BONOSISTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BONOSISLIVE"
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString

            Dim query As String = "SELECT ID, NOMBRE FROM PROYECTOS WHERE LOWER(NOMBRE)=:TEXTO"
            Dim parameter As New OracleParameter("TEXTO", OracleDbType.Varchar2, 50, texto, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.Proyectos)(Function(r As OracleDataReader) _
            New ELL.Proyectos With {.Id = SabLib.BLL.Utils.stringNull(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE"))}, query, conexion, parameter).FirstOrDefault
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarProyectosPTKSis(ByVal texto As String) As List(Of String())
            Dim status As String = "BONOSISTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BONOSISLIVE"
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString

            'select distinct Estado from Proyectos where Estado is not null and lower(Estado) not like '%archived%'
            Dim query As String = "SELECT NOMBRE FROM PROYECTOS WHERE LOWER(NOMBRE) LIKE '%' || :TEXTO || '%' AND ESTADO IS NOT NULL AND LOWER(ESTADO) NOT LIKE '%archived%' ORDER BY NOMBRE ASC"
            Dim parameter As New OracleParameter("TEXTO", OracleDbType.Varchar2, 20, texto.ToLower, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(query, conexion, parameter).ToList()
        End Function

#End Region

#Region "ROLES - USUARIOS"

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarRolUsuario(ByVal idUsuario As Integer) As ELL.UsuarioRol
            Dim status As String = "BONOSISTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BONOSISLIVE"
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString

            Dim query As String = "SELECT CASE WHEN ROLES.ID_PADRE IS NULL THEN ROLES.ID ELSE ROLES.ID_PADRE END AS ID_ROL, ROLES.NOMBRE as ROL, (SAB.USUARIOS.NOMBRE || ' ' || SAB.USUARIOS.APELLIDO1 || ' ' || SAB.USUARIOS.APELLIDO2) as USUARIO, SAB.USUARIOS.ID as ID_USUARIO " &
                                    "FROM ROLES_USUARIOS RU " &
                                    "INNER JOIN SAB.USUARIOS on SAB.USUARIOS.ID = RU.ID_USUARIO " &
                                    "INNER JOIN ROLES on ROLES.ID = RU.ID_ROL " &
                                    "AND RU.FECHA=(SELECT MAX(FECHA) FROM ROLES_USUARIOS RU2 WHERE RU2.ID_USUARIO=RU.ID_USUARIO) " &
                                    "AND ID_USUARIO =:ID_USUARIO"

            Dim parameter As New OracleParameter("ID_USUARIO", OracleDbType.Int32, idUsuario, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
            New ELL.UsuarioRol With {.IdSab = SabLib.BLL.Utils.integerNull(r("ID_USUARIO")), .IdRol = SabLib.BLL.Utils.integerNull(r("ID_ROL"))}, query, conexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene los usuarios de un rol concreto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarUsuariosRol(ByVal idRol As Integer) As List(Of ELL.UsuarioRol)
            Dim status As String = "BONOSISTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BONOSISLIVE"
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString

            'Dim query As String = "SELECT ID_USUARIO, ID_ROL FROM ROLES_USUARIOS WHERE ID_ROL=:ID_ROL"
            Dim query As String = "SELECT ROLES.ID as ID_ROL, ROLES.NOMBRE as ROL, (SAB.USUARIOS.NOMBRE || ' ' || SAB.USUARIOS.APELLIDO1 || ' ' || SAB.USUARIOS.APELLIDO2) as USUARIO, SAB.USUARIOS.ID as ID_USUARIO " & _
                                    "FROM ROLES_USUARIOS RU " & _
                                    "INNER JOIN SAB.USUARIOS on SAB.USUARIOS.ID = RU.ID_USUARIO " & _
                                    "INNER JOIN ROLES on ROLES.ID = RU.ID_ROL " & _
                                    "AND RU.FECHA=(SELECT MAX(FECHA) FROM ROLES_USUARIOS RU2 WHERE RU2.ID_USUARIO=RU.ID_USUARIO) " & _
                                    "AND ID_ROL =:ID_ROL"
            Dim parameter As New OracleParameter("ID_ROL", OracleDbType.Int32, idRol, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
            New ELL.UsuarioRol With {.IdSab = SabLib.BLL.Utils.integerNull(r("ID_USUARIO")), .IdRol = SabLib.BLL.Utils.integerNull(r("ID_ROL"))}, query, conexion, parameter)
        End Function

        '''' <summary>
        '''' Obtiene los usuarios con rol 'Project Leader'
        '''' </summary>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function CargarUsuariosProjectLeader() As List(Of ELL.Objecto)
        '    Dim status As String = "BONOSISTEST"
        '    If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BONOSISLIVE"
        '    Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString

        '    Dim query As String = "SELECT RU.ID_USUARIO as ID, (USU.NOMBRE || ' ' || USU.APELLIDO1 || ' ' || USU.APELLIDO2 ) AS NOMBRE " &
        '                            "FROM ROLES_USUARIOS RU " &
        '                            "INNER JOIN ROLES RO ON RO.ID = RU.ID_ROL " &
        '                            "INNER JOIN SAB.USUARIOS USU ON (USU.ID = RU.ID_USUARIO) " &
        '                            "WHERE(RO.ID = :ID_ROL_PROJECTLEADER OR RO.ID_PADRE = :ID_ROL_PROJECTLEADER) " &
        '                            "AND FECHA=(SELECT MAX(FECHA) FROM ROLES_USUARIOS RU2 WHERE RU2.ID_USUARIO=RU.ID_USUARIO) " &
        '                            "AND (USU.FECHABAJA IS NULL OR (USU.FECHABAJA IS NOT NULL AND USU.FECHABAJA>SYSDATE)) " &
        '                            "AND (RO.ID_PLANTALOCAL IS NULL OR RO.ID_PLANTALOCAL = 1)"

        '    Dim parameter As New OracleParameter("ID_ROL_PROJECTLEADER", OracleDbType.Int32, ELL.Roles.RolUsuario.ProjectLeader, ParameterDirection.Input)

        '    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Objecto)(Function(r As OracleDataReader) _
        '    New ELL.Objecto With {.Id = SabLib.BLL.Utils.integerNull(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE"))}, query, conexion, parameter)
        'End Function

        ''' <summary>
        ''' Obtiene los usuarios con rol 'Project Leader'
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarUsuariosRol(ByVal listaRoles As List(Of ELL.Roles.RolUsuario)) As List(Of ELL.Usuarios)
            Dim status As String = "BONOSISTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BONOSISLIVE"
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString

            Dim query As String =
                "SELECT RU.ID_USUARIO as ID, (USU.NOMBRE || ' ' || USU.APELLIDO1 || ' ' || USU.APELLIDO2 ) AS NOMBRE, RO.ID as ID_ROL, RO.NOMBRE AS NOMBRE_ROL " &
                "FROM ROLES_USUARIOS RU " &
                "INNER JOIN ROLES RO ON RO.ID = RU.ID_ROL " &
                "INNER JOIN SAB.USUARIOS USU ON (USU.ID = RU.ID_USUARIO) " &
                "WHERE(RO.ID IN ("
            For Each rol In listaRoles
                query &= rol & ","
            Next
            query = query.Substring(0, query.Length - 1)
            query &= ") OR RO.ID_PADRE IN ("
            For Each rol In listaRoles
                query &= rol & ","
            Next
            query = query.Substring(0, query.Length - 1)
            query &= ")) AND FECHA=(SELECT MAX(FECHA) FROM ROLES_USUARIOS RU2 WHERE RU2.ID_USUARIO=RU.ID_USUARIO) " &
                    "AND (USU.FECHABAJA IS NULL OR (USU.FECHABAJA IS NOT NULL AND USU.FECHABAJA>SYSDATE)) " &
                    "AND (RO.ID_PLANTALOCAL IS NULL OR RO.ID_PLANTALOCAL = 1)"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Usuarios)(Function(r As OracleDataReader) _
            New ELL.Usuarios With {.IdSab = SabLib.BLL.Utils.integerNull(r("ID")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")),
                                   .IdRol = SabLib.BLL.Utils.integerNull(r("ID_ROL")), .NombreRol = CStr(r("NOMBRE_ROL"))}, query, conexion)
        End Function
#End Region

#Region "ROLES"

        ''' <summary>
        ''' Obtiene el rol
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarRol(ByVal idRol As Integer) As ELL.Roles
            Dim status As String = "BONOSISTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BONOSISLIVE"
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString

            Dim query As String = "SELECT * FROM ROLES WHERE ID=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idRol, ParameterDirection.Input)


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Roles)(Function(r As OracleDataReader) _
            New ELL.Roles With {.IdRol = CInt(r("ID")), .Nombre = CStr(r("NOMBRE"))}, query, conexion, parameter).FirstOrDefault()
        End Function

#End Region

    End Class

End Namespace