Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class HistoricosEstadoLineaDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene lista de validaciones año
        ''' </summary>
        ''' <param name="idValidacionLinea"></param>
        ''' <returns></returns>
        Public Shared Function loadListByValidacionLinea(ByVal idValidacionLinea As Integer) As List(Of ELL.HistoricoEstadoLinea)
            Dim query As String = "SELECT * FROM VHISTORICO_ESTADO_LINEA WHERE ID_VALIDACION_LINEA=:ID_VALIDACION_LINEA"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.HistoricoEstadoLinea)(Function(r As OracleDataReader) _
            New ELL.HistoricoEstadoLinea With {.IdEstadoValidacion = CInt(r("ID_ESTADO_VALIDACION")), .Fecha = CDate(r("FECHA")), .IdUser = CInt(r("ID_USER")),
                                               .Comentarios = SabLib.BLL.Utils.stringNull(r("COMENTARIOS")), .IdValidacionLinea = CInt(r("ID_VALIDACION_LINEA")),
                                               .EmailUsuario = SabLib.BLL.Utils.stringNull(r("EMAIL_USUARIO")), .IdAccionValidacion = CInt(r("ID_ACCION_VALIDACION")),
                                               .Usuario = SabLib.BLL.Utils.stringNull(r("USUARIO")), .AccionValidacion = CStr(r("ACCION_VALIDACION")),
                                               .EstadoValdiacion = CStr(r("ESTADO_VALIDACION"))}, query, CadenaConexion, New OracleParameter("ID_VALIDACION_LINEA", OracleDbType.Int32, idValidacionLinea, ParameterDirection.Input))
        End Function

#End Region

    End Class

End Namespace