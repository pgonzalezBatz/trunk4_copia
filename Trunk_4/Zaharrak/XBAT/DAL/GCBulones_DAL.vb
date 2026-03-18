Imports Oracle.ManagedDataAccess.Client

Namespace Entidades
    Public Class GCBulones_DAL
        Inherits XBATLib.GCBulones_ELL
        ''' <summary>
        ''' Carga de Cilindros.
        ''' </summary>
        ''' <returns>List(Of gtkGertakariak)</returns>
        ''' <remarks></remarks>
        Protected Function Load() As List(Of GCBulones_DAL)
            Dim CerrarConexion As Boolean = False
            Dim sSQL As String = "SELECT ID, CODART, CODPRO, NUMALBAR, NUMPED, NUMLIN, SERIE FROM GCBULONES"
            Dim sqlWHERE As String = String.Empty
            Dim Parametros As New List(Of OracleParameter)
            '------------------------------------------------------------------------------
            'Parametros de Busqueda.
            '------------------------------------------------------------------------------
            If Me.ID IsNot Nothing Then
                sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " ID = :p_ID"
                Parametros.Add(New OracleParameter("p_ID", OracleDbType.Int32, Me.ID, ParameterDirection.Input))
            End If
            If Me.NUMALBAR IsNot Nothing Then
                sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " NUMALBAR = :p_NUMALBAR"
                Parametros.Add(New OracleParameter("p_NUMALBAR", OracleDbType.Int32, Me.NUMALBAR, ParameterDirection.Input))
            End If
            If Me.NUMPED IsNot Nothing Then
                sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " NUMPED = :p_NUMPED"
                Parametros.Add(New OracleParameter("p_NUMPED", OracleDbType.Int32, Me.NUMPED, ParameterDirection.Input))
            End If
            If Me.NUMLIN IsNot Nothing Then
                sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " NUMLIN = :p_NUMLIN"
                Parametros.Add(New OracleParameter("p_NUMLIN", OracleDbType.Int32, Me.NUMLIN, ParameterDirection.Input))
            End If
            '------------------------------------------------------------------------------
            '------------------------------------------------------------------------------
            'Cargamos los datos especificos del objeto.
            '------------------------------------------------------------------------------
            sSQL &= sqlWHERE
            If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
            Load = Memcached.OracleDirectAccess.seleccionar(Of GCBulones_DAL) _
             (Function(ODR As OracleDataReader) New GCBulones_DAL _
             With {
              .ID = If(ODR("ID") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("ID"))) _
             , .CODART = If(ODR("CODART") Is DBNull.Value, Nothing, ODR("CODART")) _
             , .CODPRO = If(ODR("CODPRO") Is DBNull.Value, Nothing, ODR("CODPRO")) _
             , .NUMALBAR = If(ODR("NUMALBAR") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("NUMALBAR"))) _
             , .NUMLIN = If(ODR("NUMLIN") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("NUMLIN"))) _
             , .NUMPED = If(ODR("NUMPED") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("NUMPED"))) _
             , .SERIE = If(ODR("SERIE") Is DBNull.Value, Nothing, ODR("SERIE"))
              }, sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
            If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
            '------------------------------------------------------------------------------
            Return Load
        End Function
        Protected Sub Delete()
            Dim CerrarConexion As Boolean = False
            Dim sSQL As String = "DELETE FROM GCBULONES WHERE ID=:p_id"
            Dim Parametros As New List(Of OracleParameter)
            Try

                If Me.ID Is Nothing Then Throw New ApplicationException("Falta el identificador unico del registro.", New ApplicationException)
                '------------------------------------------------------------------------------
                'Parametros
                '------------------------------------------------------------------------------
                Dim p_id As New OracleParameter("p_id", OracleDbType.Int32, Me.ID, ParameterDirection.Input)
                '------------------------------------------------------------------------------
                If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
                Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, p_id)
                If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
            Catch ex As Exception
                Throw
            End Try
        End Sub
        ''' <summary>
        ''' Pendiente de Desarrollar.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub Insert()
            'Dim sSQL As String = "INSERT INTO RESPONSABLES_GERTAKARIAK " _
            ' & "(IDUSUARIO, IDINCIDENCIA) " _
            ' & "VALUES (:p_IDUSUARIO, :p_IDINCIDENCIA) " _
            ' & "RETURNING ID INTO :p_id "
            ''------------------------------------------------------------------------------
            ''Parametros
            ''------------------------------------------------------------------------------
            'Dim Parametros As New List(Of OracleParameter)
            'Parametros.Add(New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, Me.IdUsuario, ParameterDirection.Input))
            'Parametros.Add(New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
            'Parametros.Add(New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.InputOutput))
            ''------------------------------------------------------------------------------
            ''------------------------------------------------------------------------------
            ''Insertamos los datos especificos del objeto.
            ''------------------------------------------------------------------------------
            'Memcached.OracleDirectAccess.NoQuery(sSQL _
            ' , If(My.Settings.ConexionOracle.State = ConnectionState.Closed, My.Settings.ConexionOracle.ConnectionString, My.Settings.ConexionOracle) _
            ' , Parametros.ToArray)
            ''------------------------------------------------------------------------------
            'Me.Id = Parametros.Find(Function(o As OracleParameter) o.ParameterName = "p_id").Value.ToString
        End Sub
        ''' <summary>
        ''' Pendiente de Desarrollar.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub Update()
            'Dim sSQL As String = "UPDATE RESPONSABLES_GERTAKARIAK SET IDUSUARIO = :p_IDUSUARIO,  IDINCIDENCIA = :p_IDINCIDENCIA " _
            '  & "WHERE ID = :p_id"
            'Dim Parametros As New List(Of OracleParameter)
            'Try
            '	If Me.Id Is Nothing Then Throw New ApplicationException("Falta el identificador unico del registro.", New ApplicationException)
            '	'------------------------------------------------------------------------------
            '	'Parametros
            '	'------------------------------------------------------------------------------
            '	Parametros.Add(New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, Me.IdUsuario, ParameterDirection.Input))
            '	Parametros.Add(New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
            '	Parametros.Add(New OracleParameter("p_id", OracleDbType.Int32, Me.Id, ParameterDirection.Input))
            '	'------------------------------------------------------------------------------
            '	Memcached.OracleDirectAccess.NoQuery(sSQL _
            '	 , If(My.Settings.ConexionOracle.State = ConnectionState.Closed, My.Settings.ConexionOracle.ConnectionString, My.Settings.ConexionOracle) _
            '	, Parametros.ToArray)
            'Catch ex As Exception
            '	throw 
            'End Try
        End Sub
    End Class
End Namespace