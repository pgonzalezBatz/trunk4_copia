Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class PedidosDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal id As Integer) As ELL.Pedido
            Dim query As String = "SELECT * FROM VPEDIDOS WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Pedido)(Function(r As OracleDataReader) _
            New ELL.Pedido With {.Id = CInt(r("ID")), .NumPedido = CStr(r("NUM_PEDIDO")), .ImporteTotal = CDec(r("IMPORTE_TOTAL")), .IdCabecera = CInt(r("ID_CABECERA")),
                                 .ImporteFacturado = CDec(r("IMPORTE_FACTURADO")), .Comentarios = SabLib.BLL.Utils.stringNull(r("COMENTARIOS")),
                                 .IdMoneda = SabLib.BLL.Utils.integerNull(r("ID_MONEDA")), .Moneda = SabLib.BLL.Utils.stringNull(r("MONEDA"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal idCabecera As Integer) As List(Of ELL.Pedido)
            Dim query As String = "SELECT * FROM VPEDIDOS WHERE ID_CABECERA=:ID_CABECERA"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Pedido)(Function(r As OracleDataReader) _
            New ELL.Pedido With {.Id = CInt(r("ID")), .NumPedido = CStr(r("NUM_PEDIDO")), .ImporteTotal = CDec(r("IMPORTE_TOTAL")), .IdCabecera = CInt(r("ID_CABECERA")),
                                 .ImporteFacturado = CDec(r("IMPORTE_FACTURADO")), .Comentarios = SabLib.BLL.Utils.stringNull(r("COMENTARIOS")),
                                 .IdMoneda = SabLib.BLL.Utils.integerNull(r("ID_MONEDA")), .Moneda = SabLib.BLL.Utils.stringNull(r("MONEDA"))}, query, CadenaConexion, New OracleParameter("ID_CABECERA", OracleDbType.Int32, idCabecera, ParameterDirection.Input))
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="pedido"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal pedido As ELL.Pedido)
            Dim query As String = "INSERT INTO PEDIDO (NUM_PEDIDO, IMPORTE_TOTAL, ID_CABECERA, COMENTARIOS, ID_MONEDA) VALUES(:NUM_PEDIDO, :IMPORTE_TOTAL, :ID_CABECERA, :COMENTARIOS, :ID_MONEDA)"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("NUM_PEDIDO", OracleDbType.NVarchar2, pedido.NumPedido, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("IMPORTE_TOTAL", OracleDbType.Decimal, pedido.ImporteTotal, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_CABECERA", OracleDbType.Int32, pedido.IdCabecera, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("COMENTARIOS", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(pedido.Comentarios), DBNull.Value, pedido.Comentarios), ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, pedido.IdMoneda, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPedido"></param>
        ''' <param name="comentarios"></param>
        Public Shared Sub UpdateComments(ByVal idPedido As Integer, ByVal comentarios As String)
            Dim query As String = "UPDATE PEDIDO SET COMENTARIOS=:COMENTARIOS WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idPedido, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("COMENTARIOS", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(comentarios), DBNull.Value, comentarios), ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Delete(ByVal id As Integer)
            Dim query As String = "DELETE FROM PEDIDO WHERE ID=:ID"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

    End Class

End Namespace