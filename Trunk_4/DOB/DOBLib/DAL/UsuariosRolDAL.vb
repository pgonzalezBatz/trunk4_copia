Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class UsuariosRolDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un usuario
        ''' </summary>
        ''' <param name="idRol"></param>
        ''' <param name="idSab"></param>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Shared Function getUsuarioRol(ByVal idRol As Integer, ByVal idSab As Integer, ByVal idPlanta As Integer) As ELL.UsuarioRol
            Dim query As String = "SELECT * FROM VUSUARIOS_ROL WHERE ID_ROL=:ID_ROL AND ID_SAB=:ID_SAB{0}"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_ROL", OracleDbType.Int32, idRol, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, idSab, ParameterDirection.Input))

            If (idPlanta = Integer.MinValue) Then
                query = String.Format(query, " AND ID_PLANTA IS NULL")
            Else
                query = String.Format(query, " AND ID_PLANTA=:ID_PLANTA")
                lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, If(idPlanta = Integer.MinValue, DBNull.Value, idPlanta), ParameterDirection.Input))
            End If

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
            New ELL.UsuarioRol With {.Id = CInt(r("ID")), .IdRol = CInt(r("ID_ROL")), .IdSab = CInt(r("ID_SAB")), .Nombre = CStr(r("NOMBRE")),
                                     .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")),
                                     .DescripcionRol = CStr(r("DESCRIPCION")), .IdPlanta = SabLib.BLL.Utils.integerNull(r("ID_PLANTA")),
                                     .Planta = SabLib.BLL.Utils.stringNull(r("PLANTA")), .PlantaActiva = CStr(r("PLANTA_ACTIVA")),
                                     .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHABAJA")), .FechaBajaDOB = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA_DOB")),
                                     .FechaBajaPlanta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA_PLANTA"))}, query, CadenaConexion, lParameters.ToArray()).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un usuario
        ''' </summary>
        ''' <param name="idUsuarioRol"></param>
        ''' <returns></returns>
        Public Shared Function getUsuarioRol(ByVal idUsuarioRol As Integer) As ELL.UsuarioRol
            Dim query As String = "SELECT * FROM VUSUARIOS_ROL WHERE ID=:ID"

            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idUsuarioRol, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
            New ELL.UsuarioRol With {.Id = CInt(r("ID")), .IdRol = CInt(r("ID_ROL")), .IdSab = CInt(r("ID_SAB")), .Nombre = CStr(r("NOMBRE")),
                                     .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")),
                                     .DescripcionRol = CStr(r("DESCRIPCION")), .IdPlanta = SabLib.BLL.Utils.integerNull(r("ID_PLANTA")),
                                     .Planta = SabLib.BLL.Utils.stringNull(r("PLANTA")), .PlantaActiva = CStr(r("PLANTA_ACTIVA")),
                                     .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHABAJA")), .FechaBajaDOB = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA_DOB")),
                                     .FechaBajaPlanta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA_PLANTA"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado de usuarios roles
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <param name="idPlanta"></param>
        ''' <param name="listaIdRoles"></param> 
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(Optional ByVal idSab As Nullable(Of Integer) = Nothing, Optional ByVal idPlanta As Nullable(Of Integer) = Nothing, Optional ByVal listaIdRoles As List(Of Integer) = Nothing) As List(Of ELL.UsuarioRol)
            'Dim query As String = "SELECT * FROM VUSUARIOS_ROL{0}"
            'Dim query As String = "SELECT UR.ID, U.NOMBRE, U.APELLIDO1, U.APELLIDO2, R.DESCRIPCION, UR.ID_ROL, UR.ID_SAB, UR.ID_PLANTA,
            '                        VPL.PLANTA, PLA.NOMBRE AS PLANTA_ACTIVA, PLA.ID AS PLANTA_ACTIVA_ID, U.FECHABAJA, UR.FECHA_BAJA AS FECHA_BAJA_DOB, VPL.FECHA_BAJA AS FECHA_BAJA_PLANTA
            '                        FROM USUARIOS_ROL UR 
            '                        INNER JOIN SAB.USUARIOS U ON U.ID = UR.ID_SAB
            '                        INNER JOIN SAB.PLANTAS PLA ON PLA.ID = U.IDPLANTA
            '                        INNER JOIN ROL R ON R.ID = UR.ID_ROL
            '                        LEFT JOIN VPLANTAS VPL ON VPL.ID = UR.ID_PLANTA
            '                        WHERE VPL.ID_PLANTA_PADRE = 35
            '                        ORDER BY U.NOMBRE, U.APELLIDO1, U.APELLIDO2;"
            Dim query As String = "SELECT UR.ID, U.NOMBRE, U.APELLIDO1, U.APELLIDO2, R.DESCRIPCION, UR.ID_ROL, UR.ID_SAB, UR.ID_PLANTA,
                                    VPL.PLANTA, vpl.ID_PLANTA_PADRE AS PLANTA_PADRE, PLA.NOMBRE AS PLANTA_ACTIVA, PLA.ID AS PLANTA_ACTIVA_ID, U.FECHABAJA, UR.FECHA_BAJA AS FECHA_BAJA_DOB, VPL.FECHA_BAJA AS FECHA_BAJA_PLANTA
                                    FROM USUARIOS_ROL UR 
                                    INNER JOIN SAB.USUARIOS U ON U.ID = UR.ID_SAB
                                    INNER JOIN SAB.PLANTAS PLA ON PLA.ID = U.IDPLANTA
                                    INNER JOIN ROL R ON R.ID = UR.ID_ROL
                                    LEFT JOIN VPLANTAS VPL ON VPL.ID = UR.ID_PLANTA{0}"
            'Dim where As String = String.Empty
            Dim where As String = " WHERE ((FECHABAJA IS NULL OR FECHABAJA > SYSDATE) 
                                    AND (UR.FECHA_BAJA IS NULL OR UR.FECHA_BAJA > SYSDATE)) "
            Dim lParameters As New List(Of OracleParameter)

            If (idSab IsNot Nothing) Then
                lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, idSab, ParameterDirection.Input))
                where &= " AND ID_SAB=:ID_SAB"
            End If

            If (idPlanta IsNot Nothing AndAlso idPlanta <> Integer.MinValue) Then
                lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                'If (String.IsNullOrEmpty(where)) Then
                '    where = " WHERE ID_PLANTA=:ID_PLANTA"
                'Else
                '    where &= " AND ID_PLANTA=:ID_PLANTA"
                'End If
                where &= " AND (VPL.ID_PLANTA_PADRE = :ID_PLANTA OR UR.ID_PLANTA = :ID_PLANTA)"
            End If

            If (listaIdRoles IsNot Nothing AndAlso listaIdRoles.Count > 0) Then
                Dim contador As Integer = 0
                For Each idRol In listaIdRoles
                    lParameters.Add(New OracleParameter("ID_ROL" & contador, OracleDbType.Int32, idRol, ParameterDirection.Input))
                    If (String.IsNullOrEmpty(where)) Then
                        where = " WHERE (ID_ROL" & "=:ID_ROL" & contador

                        If (listaIdRoles.IndexOf(idRol) = listaIdRoles.Count - 1) Then
                            where &= ")"
                        End If
                    Else
                        If (contador = 0) Then
                            where &= " AND (ID_ROL" & "=:ID_ROL" & contador
                        Else
                            where &= " OR ID_ROL" & "=:ID_ROL" & contador
                        End If

                        If (listaIdRoles.IndexOf(idRol) = listaIdRoles.Count - 1) Then
                            where &= ")"
                        End If
                    End If
                    contador += 1
                Next
            End If

            query = String.Format(query, where)

            Dim listaUsuariosRol As List(Of ELL.UsuarioRol) = Memcached.OracleDirectAccess.seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
            New ELL.UsuarioRol With {.Id = CInt(r("ID")), .IdRol = CInt(r("ID_ROL")), .IdSab = CInt(r("ID_SAB")), .Nombre = CStr(r("NOMBRE")),
                                     .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")),
                                     .DescripcionRol = CStr(r("DESCRIPCION")), .IdPlanta = SabLib.BLL.Utils.integerNull(r("ID_PLANTA")),
                                     .Planta = SabLib.BLL.Utils.stringNull(r("PLANTA")), .PlantaActiva = CStr(r("PLANTA_ACTIVA")),
                                     .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHABAJA")), .FechaBajaDOB = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA_DOB")),
                                     .FechaBajaPlanta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA_PLANTA"))}, query, CadenaConexion, lParameters.ToArray())

            Return listaUsuariosRol
        End Function

        ''' <summary>
        ''' Comprueba si existe un usuario rol
        ''' </summary>
        ''' <param name="idRol">Id del rol</param>
        ''' <param name="idSab">Id del usuario</param>
        ''' <param name="idPlanta">Id planta</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function existsUsuarioRol(ByVal idRol As Integer, ByVal idSab As Integer, ByVal idPlanta As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM USUARIOS_ROL WHERE ID_ROL=:ID_ROL AND ID_SAB=:ID_SAB{0}"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_ROL", OracleDbType.Int32, idRol, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, idSab, ParameterDirection.Input))

            If (idPlanta = Integer.MinValue) Then
                query = String.Format(query, " AND ID_PLANTA IS NULL")
            Else
                query = String.Format(query, " AND ID_PLANTA=:ID_PLANTA")
                lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, If(idPlanta = Integer.MinValue, DBNull.Value, idPlanta), ParameterDirection.Input))
            End If

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, lParameters.ToArray())
            Return filas > 0
        End Function

        ''' <summary>
        ''' Comprueba si existe una planta
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function existsPlanta(ByVal idPlanta As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM USUARIOS_ROL WHERE ID_PLANTA=:ID_PLANTA"
            Dim parameter As New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, parameter)
            Return filas > 0
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta un usuario rol
        ''' </summary>
        ''' <param name="oUsuarioRol">Objeto usuario rol con la informacion</param>
        ''' <param name="conexion"></param>
        Public Shared Sub SaveUsuarioRol(ByRef oUsuarioRol As ELL.UsuarioRol, Optional conexion As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Try
                If (conexion IsNot Nothing) Then
                    con = conexion
                Else
                    con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                    con.Open()
                End If

                Dim query As String = String.Empty

                ' Primero vamos a comprobr si existe un usuario de baja con esos datos. Si es así le quitamos la fecha de baja
                Dim usuarioRol As ELL.UsuarioRol = getUsuarioRol(oUsuarioRol.IdRol, oUsuarioRol.IdSab, oUsuarioRol.IdPlanta)
                If (usuarioRol IsNot Nothing AndAlso usuarioRol.FechaBajaDOB <> DateTime.MinValue) Then
                    query = "UPDATE USUARIOS_ROL SET FECHA_BAJA=NULL WHERE ID_ROL=:ID_ROL AND ID_SAB=:ID_SAB AND ID_PLANTA=:ID_PLANTA"
                Else
                    query = "INSERT INTO USUARIOS_ROL (ID_ROL, ID_SAB, ID_PLANTA) VALUES (:ID_ROL, :ID_SAB, :ID_PLANTA)"
                End If

                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_ROL", OracleDbType.Int32, oUsuarioRol.IdRol, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, oUsuarioRol.IdSab, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, If(oUsuarioRol.IdPlanta = Integer.MinValue, DBNull.Value, oUsuarioRol.IdPlanta), ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
            Catch ex As Exception
                Throw ex
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed AndAlso conexion Is Nothing) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Marca el usuario rol como obsoleto
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub MarkAsObsoleteUsuarioRol(ByVal id As Integer)
            Dim query As String = "UPDATE USUARIOS_ROL SET FECHA_BAJA=:FECHA_BAJA WHERE ID=:ID"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("FECHA_BAJA", OracleDbType.Date, DateTime.Now, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="nuevoRol"></param>
        Public Shared Sub ChangeRole(ByVal id As Integer, ByVal nuevoRol As Integer)
            Dim query As String = "UPDATE USUARIOS_ROL SET ID_ROL=:ID_ROL WHERE ID=:ID"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_ROL", OracleDbType.Int32, nuevoRol, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un usuario rol
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub DeleteUsuarioRol(ByVal id As Integer)
            Dim query As String = "DELETE FROM USUARIOS_ROL WHERE ID=:ID"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

    End Class

End Namespace