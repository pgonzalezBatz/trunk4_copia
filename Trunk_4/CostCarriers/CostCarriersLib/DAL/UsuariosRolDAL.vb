Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class UsuariosRolDAL
        Inherits DALBase

#Region "Consultas"

        '''' <summary>
        '''' Obtiene un usuario
        '''' </summary>
        '''' <param name="idRol"></param>
        '''' <param name="idSab"></param>
        '''' <param name="idPlanta"></param>
        '''' <returns></returns>
        'Public Shared Function getUsuarioRol(ByVal idRol As Integer, ByVal idSab As Integer, ByVal idPlanta As Integer) As ELL.UsuarioRol
        '    Dim query As String = "SELECT * FROM VUSUARIOS_ROL WHERE ID_ROL=:ID_ROL AND ID_SAB=:ID_SAB{0}"

        '    Dim lParameters As New List(Of OracleParameter)
        '    lParameters.Add(New OracleParameter("ID_ROL", OracleDbType.Int32, idRol, ParameterDirection.Input))
        '    lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, idSab, ParameterDirection.Input))

        '    If (idPlanta = Integer.MinValue) Then
        '        query = String.Format(query, " AND ID_PLANTA IS NULL")
        '    Else
        '        query = String.Format(query, " AND ID_PLANTA=:ID_PLANTA")
        '        lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, If(idPlanta = Integer.MinValue, DBNull.Value, idPlanta), ParameterDirection.Input))
        '    End If

        '    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
        '    New ELL.UsuarioRol With {.Id = CInt(r("ID")), .IdRol = CInt(r("ID_ROL")), .IdSab = CInt(r("ID_SAB")), .Nombre = CStr(r("NOMBRE")),
        '                             .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")),
        '                             .DescripcionRol = CStr(r("DESCRIPCION")), .IdPlanta = SabLib.BLL.Utils.integerNull(r("ID_PLANTA")),
        '                             .Planta = SabLib.BLL.Utils.stringNull(r("PLANTA")), .PlantaActiva = CStr(r("PLANTA_ACTIVA"))}, query, CadenaConexion, lParameters.ToArray()).FirstOrDefault()
        'End Function

        '''' <summary>
        '''' Obtiene un usuario
        '''' </summary>
        '''' <param name="idUsuarioRol"></param>
        '''' <returns></returns>
        'Public Shared Function getUsuarioRol(ByVal idUsuarioRol As Integer) As ELL.UsuarioRol
        '    Dim query As String = "SELECT * FROM VUSUARIOS_ROL WHERE ID=:ID"

        '    Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idUsuarioRol, ParameterDirection.Input)

        '    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
        '    New ELL.UsuarioRol With {.Id = CInt(r("ID")), .IdRol = CInt(r("ID_ROL")), .IdSab = CInt(r("ID_SAB")), .Nombre = CStr(r("NOMBRE")),
        '                             .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")),
        '                             .DescripcionRol = CStr(r("DESCRIPCION")), .IdPlanta = SabLib.BLL.Utils.integerNull(r("ID_PLANTA")),
        '                             .Planta = SabLib.BLL.Utils.stringNull(r("PLANTA")), .PlantaActiva = CStr(r("PLANTA_ACTIVA"))}, query, CadenaConexion, parameter).FirstOrDefault()
        'End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <returns></returns>
        Public Shared Function loadList(ByVal idSab As Integer) As List(Of ELL.UsuarioRol)
            Dim query As String = "SELECT * FROM VUSUARIOS_ROL WHERE ID_SAB=:ID_SAB"

            Dim listaUsuariosRol As List(Of ELL.UsuarioRol) = Memcached.OracleDirectAccess.seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
            New ELL.UsuarioRol With {.IdRol = CInt(r("ID_ROL")), .IdSab = CInt(r("ID_SAB")), .Nombre = CStr(r("NOMBRE")),
                                     .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")),
                                     .DescripcionRol = CStr(r("DESCRIPCION_ROL")), .Email = SabLib.BLL.Utils.stringNull(r("EMAIL")),
                                     .IdPlanta = CInt(r("ID_PLANTA"))}, query, CadenaConexion, New OracleParameter("ID_SAB", OracleDbType.Int32, idSab, ParameterDirection.Input))

            Return listaUsuariosRol
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlantaSab"></param>
        ''' <returns></returns>
        Public Shared Function loadListByIdPlanta(ByVal idPlantaSab As Integer) As List(Of ELL.UsuarioRol)
            Dim query As String = "SELECT * FROM VUSUARIOS_ROL WHERE ID_PLANTA=:ID_PLANTA"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlantaSab, ParameterDirection.Input))

            Dim listaUsuariosRol As List(Of ELL.UsuarioRol) = Memcached.OracleDirectAccess.seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
            New ELL.UsuarioRol With {.IdRol = CInt(r("ID_ROL")), .IdSab = CInt(r("ID_SAB")), .Nombre = CStr(r("NOMBRE")),
                                     .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")),
                                     .DescripcionRol = CStr(r("DESCRIPCION_ROL")), .Email = SabLib.BLL.Utils.stringNull(r("EMAIL")),
                                     .IdPlanta = CInt(r("ID_PLANTA"))}, query, CadenaConexion, lParameters.ToArray())

            Return listaUsuariosRol
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idRol"></param>
        ''' <returns></returns>
        Public Shared Function loadListByIdRol(ByVal idRol As Integer) As List(Of ELL.UsuarioRol)
            Dim query As String = "SELECT * FROM VUSUARIOS_ROL WHERE ID_ROL=:ID_ROL"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_ROL", OracleDbType.Int32, idRol, ParameterDirection.Input))

            Dim listaUsuariosRol As List(Of ELL.UsuarioRol) = Memcached.OracleDirectAccess.seleccionar(Of ELL.UsuarioRol)(Function(r As OracleDataReader) _
            New ELL.UsuarioRol With {.IdRol = CInt(r("ID_ROL")), .IdSab = CInt(r("ID_SAB")), .Nombre = CStr(r("NOMBRE")),
                                     .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")),
                                     .DescripcionRol = CStr(r("DESCRIPCION_ROL")), .Email = SabLib.BLL.Utils.stringNull(r("EMAIL")),
                                     .IdPlanta = CInt(r("ID_PLANTA"))}, query, CadenaConexion, lParameters.ToArray())

            Return listaUsuariosRol
        End Function

        '''' <summary>
        '''' Comprueba si existe un usuario rol
        '''' </summary>
        '''' <param name="idRol">Id del rol</param>
        '''' <param name="idSab">Id del usuario</param>
        '''' <param name="idPlanta">Id planta</param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Shared Function existsUsuarioRol(ByVal idRol As Integer, ByVal idSab As Integer, ByVal idPlanta As Integer) As Boolean
        '    Dim query As String = "SELECT COUNT(*) FROM USUARIOS_ROL WHERE ID_ROL=:ID_ROL AND ID_SAB=:ID_SAB{0}"

        '    Dim lParameters As New List(Of OracleParameter)
        '    lParameters.Add(New OracleParameter("ID_ROL", OracleDbType.Int32, idRol, ParameterDirection.Input))
        '    lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, idSab, ParameterDirection.Input))

        '    If (idPlanta = Integer.MinValue) Then
        '        query = String.Format(query, " AND ID_PLANTA IS NULL")
        '    Else
        '        query = String.Format(query, " AND ID_PLANTA=:ID_PLANTA")
        '        lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, If(idPlanta = Integer.MinValue, DBNull.Value, idPlanta), ParameterDirection.Input))
        '    End If

        '    Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, lParameters.ToArray())
        '    Return filas > 0
        'End Function

        '''' <summary>
        '''' Comprueba si existe una planta
        '''' </summary>
        '''' <param name="idPlanta"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Shared Function existsPlanta(ByVal idPlanta As Integer) As Boolean
        '    Dim query As String = "SELECT COUNT(*) FROM USUARIOS_ROL WHERE ID_PLANTA=:ID_PLANTA"
        '    Dim parameter As New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)

        '    Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, parameter)
        '    Return filas > 0
        'End Function

#End Region

#Region "Modificaciones"

        '''' <summary>
        '''' Inserta un usuario rol
        '''' </summary>
        '''' <param name="oUsuarioRol">Objeto usuario rol con la informacion</param>
        '''' <param name="conexion"></param>
        'Public Shared Sub SaveUsuarioRol(ByRef oUsuarioRol As ELL.UsuarioRol, Optional conexion As Oracle.DataAccess.Client.OracleConnection = Nothing)
        '    Dim con As Oracle.DataAccess.Client.OracleConnection = Nothing
        '    Try
        '        If (conexion IsNot Nothing) Then
        '            con = conexion
        '        Else
        '            con = New Oracle.DataAccess.Client.OracleConnection(CadenaConexion)
        '            con.Open()
        '        End If

        '        Dim query As String = "INSERT INTO USUARIOS_ROL (ID_ROL, ID_SAB, ID_PLANTA) VALUES (:ID_ROL, :ID_SAB, :ID_PLANTA)"
        '        Dim lParameters As New List(Of OracleParameter)
        '        lParameters.Add(New OracleParameter("ID_ROL", OracleDbType.Int32, oUsuarioRol.IdRol, ParameterDirection.Input))
        '        lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, oUsuarioRol.IdSab, ParameterDirection.Input))
        '        lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, If(oUsuarioRol.IdPlanta = Integer.MinValue, DBNull.Value, oUsuarioRol.IdPlanta), ParameterDirection.Input))

        '        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed AndAlso conexion Is Nothing) Then
        '            con.Close()
        '            con.Dispose()
        '        End If
        '    End Try
        'End Sub

#End Region

#Region "Eliminaciones"

        '''' <summary>
        '''' Elimina un usuario rol
        '''' </summary>
        '''' <param name="id"></param>
        'Public Shared Sub DeleteUsuarioRol(ByVal id As Integer)
        '    Dim query As String = "DELETE FROM USUARIOS_ROL WHERE ID=:ID"
        '    Dim lParameters As New List(Of OracleParameter)
        '    lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

        '    Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        'End Sub

#End Region

    End Class

End Namespace