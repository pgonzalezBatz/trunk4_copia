Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class HistoricosEstadoFacturacionDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal id As Integer) As ELL.HistoricoEstadoFacturacion
            Dim query As String = "SELECT * FROM VHISTORICO_ESTADO_FACTURACION WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.HistoricoEstadoFacturacion)(Function(r As OracleDataReader) _
            New ELL.HistoricoEstadoFacturacion With {.Id = CInt(r("ID")), .IdLineaPedido = CInt(r("ID_LINEA_PEDIDO")), .IdEstadoFacturacioin = CInt(r("ID_ESTADO_FACTURACION")), .FechaAlta = CDate(r("FECHA_ALTA")),
                                                     .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")), .NombreUsuarioAlta = CStr(r("NOMBRE_USUARIO_ALTA"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal idLineaPedido As Integer) As List(Of ELL.HistoricoEstadoFacturacion)
            Dim query As String = "SELECT * FROM VHISTORICO_ESTADO_FACTURACION WHERE ID_LINEA_PEDIDO=:ID_LINEA_PEDIDO"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.HistoricoEstadoFacturacion)(Function(r As OracleDataReader) _
            New ELL.HistoricoEstadoFacturacion With {.Id = CInt(r("ID")), .IdLineaPedido = CInt(r("ID_LINEA_PEDIDO")), .IdEstadoFacturacioin = CInt(r("ID_ESTADO_FACTURACION")), .FechaAlta = CDate(r("FECHA_ALTA")),
                                                     .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")), .NombreUsuarioAlta = CStr(r("NOMBRE_USUARIO_ALTA"))}, query, CadenaConexion, New OracleParameter("ID_LINEA_PEDIDO", OracleDbType.Int32, idLineaPedido, ParameterDirection.Input))
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="historicoEstadoFacturacion"></param>
        Public Shared Sub Add(ByVal historicoEstadoFacturacion As ELL.HistoricoEstadoFacturacion)
            Dim query As String = "INSERT INTO HISTORICO_ESTADO_FACTURACION (ID_LINEA_PEDIDO, ID_ESTADO_FACTURACION, ID_USUARIO_ALTA) VALUES(:ID_LINEA_PEDIDO, :ID_ESTADO_FACTURACION, :ID_USUARIO_ALTA)"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_LINEA_PEDIDO", OracleDbType.Int32, historicoEstadoFacturacion.IdLineaPedido, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_ESTADO_FACTURACION", OracleDbType.Int32, historicoEstadoFacturacion.IdEstadoFacturacioin, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_USUARIO_ALTA", OracleDbType.Int32, historicoEstadoFacturacion.IdUsuarioAlta, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)
        End Sub

#End Region

    End Class

End Namespace