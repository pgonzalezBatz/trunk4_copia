Imports Oracle.ManagedDataAccess.Client

Namespace Entidades
    Public Class FaPersonal_DAL
        Inherits XBATLib.FaPersonal_ELL
        ''' <summary>
        ''' Carga de Personas del XBAT.
        ''' </summary>
        ''' <returns>List(Of FaPersonal)</returns>
        ''' <remarks></remarks>
        Public Function Load() As List(Of FaPersonal_DAL)
            Dim CerrarConexion As Boolean = False
            Dim sSQL As String = "SELECT CODPER, USUARIO FROM FAPERSONAL"
            Dim sqlWHERE As String = String.Empty
            Dim Parametros As New List(Of OracleParameter)
            '------------------------------------------------------------------------------
            'Parametros de Busqueda.
            '------------------------------------------------------------------------------
            If Me.CODPER IsNot Nothing Then
                sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " CODPER = :p_CODPER"
                Parametros.Add(New OracleParameter("p_CODPER", OracleDbType.Int32, Me.CODPER, ParameterDirection.Input))
            End If
            If Not String.IsNullOrEmpty(Me.USUARIO) Then
                sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " LOWER(USUARIO) = :p_USUARIO"
                Parametros.Add(New OracleParameter("p_USUARIO", OracleDbType.NVarchar2, Me.USUARIO.Trim.ToLower, ParameterDirection.Input))
            End If
            '------------------------------------------------------------------------------
            '------------------------------------------------------------------------------
            'Cargamos los datos especificos del objeto.
            '------------------------------------------------------------------------------
            sSQL &= sqlWHERE
            If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
            Load = Memcached.OracleDirectAccess.seleccionar(Of FaPersonal_DAL) _
             (Function(ODR As OracleDataReader) New FaPersonal_DAL _
             With {
              .CODPER = If(ODR("CODPER") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("CODPER"))) _
             , .USUARIO = If(ODR("USUARIO") Is DBNull.Value, Nothing, ODR("USUARIO"))
              }, sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
            If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
            '------------------------------------------------------------------------------
            Return Load
        End Function
    End Class
End Namespace