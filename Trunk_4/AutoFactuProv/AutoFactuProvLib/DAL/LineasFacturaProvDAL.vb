Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class LineasFacturaProvDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de líneas de una factura
        ''' </summary>
        ''' <param name="idFacturaProv"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal idFacturaProv As Integer) As List(Of ELL.LineasFacturaProv)
            Dim query As String = "SELECT * FROM LINEAS_FACTURA_PROV WHERE ID_FACTURA=:ID_FACTURA"

            Dim param As New OracleParameter("ID_FACTURA", OracleDbType.Int32, idFacturaProv, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.LineasFacturaProv)(Function(r As OracleDataReader) _
            New ELL.LineasFacturaProv With {.IdFactura = CInt(r("ID_FACTURA")), .Albaran = CStr(r("ALBARAN")), .Pedido = CInt(r("PEDIDO")),
                                            .Linea = CInt(r("LINEA"))}, query, CadenaConexion, param)
        End Function

        ''' <summary>
        ''' Obtiene un listado de líneas de un factura de proveedor
        ''' </summary>
        ''' <param name="proveedor"></param>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal proveedor As Integer, ByVal empresa As String, ByVal planta As String) As List(Of ELL.LineasFacturaProv)
            Dim query As String = "SELECT LF.* FROM LINEAS_FACTURA_PROV LF " _
                                  & "INNER JOIN FACTURA_PROV F ON F.ID = LF.ID_FACTURA " _
                                  & "WHERE F.PROVEEDOR=:PROVEEDOR AND F.EMPRESA=:EMPRESA AND F.PLANTA=:PLANTA"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("PROVEEDOR", OracleDbType.Int32, proveedor, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("EMPRESA", OracleDbType.NVarchar2, empresa, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("PLANTA", OracleDbType.NVarchar2, planta, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.LineasFacturaProv)(Function(r As OracleDataReader) _
            New ELL.LineasFacturaProv With {.IdFactura = CInt(r("ID_FACTURA")), .Albaran = CStr(r("ALBARAN")), .Pedido = CInt(r("PEDIDO")),
                                            .Linea = CInt(r("LINEA"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

#End Region

    End Class

End Namespace