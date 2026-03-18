Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ValidacionesAñoDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene lista de validaciones año
        ''' </summary>
        ''' <param name="idValidacionLinea"></param>
        ''' <returns></returns>
        Public Shared Function loadListByValidacionLinea(ByVal idValidacionLinea As Integer) As List(Of ELL.ValidacionAño)
            Dim query As String = "SELECT * FROM VALIDACION_AÑO WHERE ID_VALIDACION_LINEA=:ID_VALIDACION_LINEA"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionAño)(Function(r As OracleDataReader) _
            New ELL.ValidacionAño With {.Id = CInt(r("ID")), .IdValidacionLinea = CInt(r("ID_VALIDACION_LINEA")), .Año = CInt(r("AÑO")),
                                          .Trimestre = CInt(r("TRIMESTRE")), .Valor = CInt(r("VALOR")), .IdColumna = CInt(r("ID_COLUMNA"))}, query, CadenaConexion, New OracleParameter("ID_VALIDACION_LINEA", OracleDbType.Int32, idValidacionLinea, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' Obtiene lista de validaciones año
        ''' </summary>
        ''' <param name="idValidacionLinea"></param>
        ''' <returns></returns>
        Public Shared Function loadListUltimosValidadosByValidacionLinea(ByVal idValidacionLinea As Integer) As List(Of ELL.ValidacionAño)
            Dim query As String = "SELECT * FROM VVALIDACION_AÑO_ULTIMOS_VALID WHERE ID_VALIDACION_LINEA=:ID_VALIDACION_LINEA"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionAño)(Function(r As OracleDataReader) _
            New ELL.ValidacionAño With {.Id = CInt(r("ID")), .IdValidacionLinea = CInt(r("ID_VALIDACION_LINEA")), .Año = CInt(r("AÑO")),
                                          .Trimestre = CInt(r("TRIMESTRE")), .Valor = CInt(r("VALOR")), .IdColumna = CInt(r("ID_COLUMNA"))}, query, CadenaConexion, New OracleParameter("ID_VALIDACION_LINEA", OracleDbType.Int32, idValidacionLinea, ParameterDirection.Input))
        End Function

#End Region

    End Class

End Namespace