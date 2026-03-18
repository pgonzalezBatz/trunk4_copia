Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class FacturasProvDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de facturas para un proveedor
        ''' </summary>
        ''' <param name="proveedor"></param> 
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal proveedor As Integer, ByVal empresa As String, ByVal planta As String) As List(Of ELL.FacturaProv)
            Dim query As String = "SELECT * FROM FACTURA_PROV WHERE PROVEEDOR=:PROVEEDOR AND EMPRESA=:EMPRESA AND PLANTA=:PLANTA"

            Dim lParameters As New List(Of OracleParameter)

            lParameters.Add(New OracleParameter("PROVEEDOR", OracleDbType.Int32, proveedor, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("EMPRESA", OracleDbType.NVarchar2, empresa, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("PLANTA", OracleDbType.NVarchar2, planta, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.FacturaProv)(Function(r As OracleDataReader) _
            New ELL.FacturaProv With {.Id = CInt(r("ID")), .RutaFactura = SabLib.BLL.Utils.stringNull(r("RUTA")), .NumFactura = CStr(r("NUM_FACTURA")),
                                      .Proveedor = CInt(r("PROVEEDOR")), .Empresa = CStr(r("EMPRESA")), .Planta = CStr(r("PLANTA")),
                                      .FechaAlta = CDate(r("FECHA_ALTA"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta una factura de proveedor
        ''' </summary>
        ''' <param name="facturaProv"></param> 
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal facturaProv As ELL.FacturaProv, ByVal lineasFacturaProv As List(Of ELL.LineasFacturaProv))
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                ' Insertamos la factura del proveedor
                Dim query As String = "INSERT INTO FACTURA_PROV (RUTA, NUM_FACTURA, PROVEEDOR, EMPRESA, PLANTA) VALUES (:RUTA, :NUM_FACTURA, :PROVEEDOR, :EMPRESA, :PLANTA) RETURNING ID INTO :RETURN_VALUE"
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("RUTA", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(facturaProv.RutaFactura), DBNull.Value, facturaProv.RutaFactura), ParameterDirection.Input))
                lParameters.Add(New OracleParameter("NUM_FACTURA", OracleDbType.NVarchar2, facturaProv.NumFactura, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("PROVEEDOR", OracleDbType.Int32, facturaProv.Proveedor, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("EMPRESA", OracleDbType.NVarchar2, facturaProv.Empresa, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("PLANTA", OracleDbType.NVarchar2, facturaProv.Planta, ParameterDirection.Input))
                lParameters.Add(New OracleParameter() With {.ParameterName = "RETURN_VALUE", .OracleDbType = OracleDbType.Int32, .Direction = ParameterDirection.ReturnValue, .DbType = DbType.Int32})

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray())

                Dim idfacturaProv As Integer = lParameters.Last.Value

                ' Insertamos la líneas que componen la factura
                query = "INSERT INTO LINEAS_FACTURA_PROV (ID_FACTURA, ALBARAN, PEDIDO, LINEA) VALUES (:ID_FACTURA, :ALBARAN, :PEDIDO, :LINEA)"
                For Each linea In lineasFacturaProv
                    lParameters.Clear()
                    lParameters.Add(New OracleParameter("ID_FACTURA", OracleDbType.Int32, idfacturaProv, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ALBARAN", OracleDbType.NVarchar2, linea.Albaran, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("PEDIDO", OracleDbType.Int32, linea.Pedido, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("LINEA", OracleDbType.Int32, linea.Linea, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray())
                Next

                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw ex
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Borra una factura de proveedor
        ''' </summary>
        ''' <param name="idFacturaProv"></param> 
        ''' <remarks></remarks>
        Public Shared Sub Delete(ByVal idFacturaProv As Integer)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                ' Borramos las líneas que pueda tener
                Dim query As String = "DELETE FROM LINEAS_FACTURA_PROV WHERE ID_FACTURA=:ID_FACTURA"
                Dim param As New OracleParameter("ID_FACTURA", OracleDbType.Int32, idFacturaProv, ParameterDirection.Input)
                Memcached.OracleDirectAccess.NoQuery(query, con, param)

                ' Borramos la factura del proveedor
                query = "DELETE FROM FACTURA_PROV WHERE ID=:ID"
                param = New OracleParameter("ID", OracleDbType.Int32, idFacturaProv, ParameterDirection.Input)
                Memcached.OracleDirectAccess.NoQuery(query, con, param)

                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw ex
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Sub

#End Region

    End Class

End Namespace