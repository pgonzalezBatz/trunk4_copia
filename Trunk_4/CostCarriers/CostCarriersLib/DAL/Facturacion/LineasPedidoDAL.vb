Imports Oracle.ManagedDataAccess.Client
Imports System.Web.Script.Serialization

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class LineasPedidoDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal id As Integer) As ELL.LineaPedido
            Dim query As String = "SELECT * FROM VLINEAS_PEDIDO WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.LineaPedido)(Function(r As OracleDataReader) _
            New ELL.LineaPedido With {.Id = CInt(r("ID")), .IdPedido = CInt(r("ID_PEDIDO")), .IdStep = CInt(r("ID_STEP")), .Posiciones = CStr(r("POSICIONES")),
                                      .Porcentaje = CInt(r("PORCENTAJE")), .Importe = CDec(r("IMPORTE")), .NombreStep = CStr(r("NOMBRE_STEP")),
                                      .IdEstadoFacturacion = CInt(r("ID_ESTADO_FACTURACION")), .EstadoFacturacion = CStr(r("ESTADO_FACTURACION")),
                                      .CostCarrier = CStr(r("COST_CARRIER")), .NumPedido = CStr(r("NUM_PEDIDO")), .FechaAlta = CDate(r("FECHA_ALTA")),
                                      .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")), .CodigoProyecto = CStr(r("CODIGO_PROYECTO")), .NombreProyecto = CStr(r("NOMBRE_PROYECTO")),
                                      .IdCabecera = CInt(r("ID_CABECERA")), .ImporteTotal = CDec(r("IMPORTE_TOTAL")), .NumFactura = SabLib.BLL.Utils.stringNull(r("NUM_FACTURA")),
                                      .Cliente = SabLib.BLL.Utils.stringNull(r("CLIENTE")), .IdMoneda = SabLib.BLL.Utils.integerNull(r("ID_MONEDA")),
                                      .MonedaFacturacion = SabLib.BLL.Utils.stringNull(r("MONEDA_FACTURACION")), .IdMonedaFacturacion = SabLib.BLL.Utils.integerNull(r("ID_MONEDA_FACTURACION")),
                                      .Owner = CStr(r("OWNER")), .Pais = SabLib.BLL.Utils.stringNull(r("PAIS"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList() As List(Of ELL.LineaPedido)
            Dim query As String = "SELECT * FROM VLINEAS_PEDIDO"

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.LineaPedido)(Function(r As OracleDataReader) _
            New ELL.LineaPedido With {.Id = CInt(r("ID")), .IdPedido = CInt(r("ID_PEDIDO")), .IdStep = CInt(r("ID_STEP")), .Posiciones = CStr(r("POSICIONES")),
                                      .Porcentaje = CInt(r("PORCENTAJE")), .Importe = CDec(r("IMPORTE")), .NombreStep = CStr(r("NOMBRE_STEP")),
                                      .IdEstadoFacturacion = CInt(r("ID_ESTADO_FACTURACION")), .EstadoFacturacion = CStr(r("ESTADO_FACTURACION")),
                                      .CostCarrier = CStr(r("COST_CARRIER")), .NumPedido = CStr(r("NUM_PEDIDO")), .FechaAlta = CDate(r("FECHA_ALTA")),
                                      .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")), .CodigoProyecto = CStr(r("CODIGO_PROYECTO")), .NombreProyecto = CStr(r("NOMBRE_PROYECTO")),
                                      .IdCabecera = CInt(r("ID_CABECERA")), .ImporteTotal = CDec(r("IMPORTE_TOTAL")), .NumFactura = SabLib.BLL.Utils.stringNull(r("NUM_FACTURA")),
                                      .Cliente = SabLib.BLL.Utils.stringNull(r("CLIENTE")), .IdMoneda = SabLib.BLL.Utils.integerNull(r("ID_MONEDA")),
                                      .MonedaFacturacion = SabLib.BLL.Utils.stringNull(r("MONEDA_FACTURACION")), .IdMonedaFacturacion = SabLib.BLL.Utils.integerNull(r("ID_MONEDA_FACTURACION")),
                                      .Owner = CStr(r("OWNER")), .Pais = SabLib.BLL.Utils.stringNull(r("PAIS"))}, query, CadenaConexion, Nothing)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal idPedido As Integer) As List(Of ELL.LineaPedido)
            Dim query As String = "SELECT * FROM VLINEAS_PEDIDO WHERE ID_PEDIDO=:ID_PEDIDO"

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.LineaPedido)(Function(r As OracleDataReader) _
            New ELL.LineaPedido With {.Id = CInt(r("ID")), .IdPedido = CInt(r("ID_PEDIDO")), .IdStep = CInt(r("ID_STEP")), .Posiciones = CStr(r("POSICIONES")),
                                      .Porcentaje = CInt(r("PORCENTAJE")), .Importe = CDec(r("IMPORTE")), .NombreStep = CStr(r("NOMBRE_STEP")),
                                      .CostCarrier = CStr(r("COST_CARRIER")), .NumPedido = CStr(r("NUM_PEDIDO")),
                                      .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")), .CodigoProyecto = CStr(r("CODIGO_PROYECTO")), .NombreProyecto = CStr(r("NOMBRE_PROYECTO")),
                                      .IdCabecera = CInt(r("ID_CABECERA")), .ImporteTotal = CDec(r("IMPORTE_TOTAL")),
                                      .Cliente = SabLib.BLL.Utils.stringNull(r("CLIENTE")), .IdMoneda = SabLib.BLL.Utils.integerNull(r("ID_MONEDA")),
                                      .MonedaFacturacion = SabLib.BLL.Utils.stringNull(r("MONEDA_FACTURACION")), .IdMonedaFacturacion = SabLib.BLL.Utils.integerNull(r("ID_MONEDA_FACTURACION")),
                                      .IdEstadoFacturacion = SabLib.BLL.Utils.integerNull(r("ID_ESTADO_FACTURACION")), .EstadoFacturacion = SabLib.BLL.Utils.stringNull(r("ESTADO_FACTURACION")),
                                      .NumFactura = SabLib.BLL.Utils.stringNull(r("NUM_FACTURA")),
                                      .FechaAlta = SabLib.BLL.Utils.dateTimeNull(r("FECHA_ALTA")),
                                      .Owner = CStr(r("OWNER")), .CodigoPais = SabLib.BLL.Utils.integerNull(r("CODIGOPAIS")), .Pais = SabLib.BLL.Utils.stringNull(r("PAIS"))}, query, CadenaConexion, New OracleParameter("ID_PEDIDO", OracleDbType.Int32, idPedido, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadListByStep(ByVal idStep As Integer) As List(Of ELL.LineaPedido)
            Dim query As String = "SELECT * FROM VLINEAS_PEDIDO WHERE ID_STEP=:ID_STEP"

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.LineaPedido)(Function(r As OracleDataReader) _
            New ELL.LineaPedido With {.Id = CInt(r("ID")), .IdPedido = CInt(r("ID_PEDIDO")), .IdStep = CInt(r("ID_STEP")), .Posiciones = CStr(r("POSICIONES")),
                                      .Porcentaje = CInt(r("PORCENTAJE")), .Importe = CDec(r("IMPORTE")), .NombreStep = CStr(r("NOMBRE_STEP")),
                                      .IdEstadoFacturacion = CInt(r("ID_ESTADO_FACTURACION")), .EstadoFacturacion = CStr(r("ESTADO_FACTURACION")),
                                      .CostCarrier = CStr(r("COST_CARRIER")), .NumPedido = CStr(r("NUM_PEDIDO")), .FechaAlta = CDate(r("FECHA_ALTA")),
                                      .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")), .CodigoProyecto = CStr(r("CODIGO_PROYECTO")), .NombreProyecto = CStr(r("NOMBRE_PROYECTO")),
                                      .IdCabecera = CInt(r("ID_CABECERA")), .ImporteTotal = CDec(r("IMPORTE_TOTAL")), .NumFactura = SabLib.BLL.Utils.stringNull(r("NUM_FACTURA")),
                                      .Cliente = SabLib.BLL.Utils.stringNull(r("CLIENTE")), .IdMoneda = SabLib.BLL.Utils.integerNull(r("ID_MONEDA")),
                                      .MonedaFacturacion = SabLib.BLL.Utils.stringNull(r("MONEDA_FACTURACION")), .IdMonedaFacturacion = SabLib.BLL.Utils.integerNull(r("ID_MONEDA_FACTURACION")),
                                      .Owner = CStr(r("OWNER")), .Pais = SabLib.BLL.Utils.stringNull(r("PAIS"))}, query, CadenaConexion, New OracleParameter("ID_STEP", OracleDbType.Int32, idStep, ParameterDirection.Input))
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="lineasPedido"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal lineasPedido As List(Of ELL.LineaPedido), ByVal estadoFacturacion As ELL.Pedido.EstadoFacturacion, ByVal idUsuario As Integer)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty
            Dim lParameters As New List(Of OracleParameter)
            Dim idLineaPedido As Integer

            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                For Each linea In lineasPedido
                    ' 1º Guardarmos las linea

                    query = "INSERT INTO LINEAS_PEDIDO (ID_PEDIDO, ID_STEP, POSICIONES, PORCENTAJE, IMPORTE, PAIS) VALUES(:ID_PEDIDO, :ID_STEP, :POSICIONES, :PORCENTAJE, :IMPORTE, :PAIS) RETURNING ID INTO :RETURN_VALUE"
                    lParameters = New List(Of OracleParameter)
                    lParameters.Add(New OracleParameter("ID_PEDIDO", OracleDbType.Int32, linea.IdPedido, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID_STEP", OracleDbType.Int32, linea.IdStep, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("POSICIONES", OracleDbType.NVarchar2, linea.Posiciones, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("PORCENTAJE", OracleDbType.Int32, linea.Porcentaje, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, linea.Importe, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("PAIS", OracleDbType.Int32, linea.CodigoPais, ParameterDirection.Input))

                    Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                    p.DbType = DbType.Int32
                    lParameters.Add(p)

                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                    idLineaPedido = lParameters.Last.Value

                    ' 2º Guardarmos el historico
                    query = "INSERT INTO HISTORICO_ESTADO_FACTURACION (ID_LINEA_PEDIDO, ID_ESTADO_FACTURACION, ID_USUARIO_ALTA) VALUES(:ID_LINEA_PEDIDO, :ID_ESTADO_FACTURACION, :ID_USUARIO_ALTA)"

                    lParameters = New List(Of OracleParameter)
                    lParameters.Add(New OracleParameter("ID_LINEA_PEDIDO", OracleDbType.Int32, idLineaPedido, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID_ESTADO_FACTURACION", OracleDbType.Int32, estadoFacturacion, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID_USUARIO_ALTA", OracleDbType.Int32, idUsuario, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
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

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idLineaPedido"></param>
        ''' <param name="numFactura"></param>
        ''' <param name="idUsuario"></param>
        Public Shared Sub Bill(ByVal idLineaPedido As Integer, ByVal numFactura As String, ByVal idUsuario As Integer)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty
            Dim lParameters As New List(Of OracleParameter)

            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                ' 1º Guardarmos el numero de factura
                query = "UPDATE LINEAS_PEDIDO SET NUM_FACTURA=:NUM_FACTURA WHERE ID=:ID"

                lParameters = New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("NUM_FACTURA", OracleDbType.NVarchar2, numFactura, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idLineaPedido, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                ' 2º Guardarmos el historico
                query = "INSERT INTO HISTORICO_ESTADO_FACTURACION (ID_LINEA_PEDIDO, ID_ESTADO_FACTURACION, ID_USUARIO_ALTA) VALUES(:ID_LINEA_PEDIDO, :ID_ESTADO_FACTURACION, :ID_USUARIO_ALTA)"

                lParameters = New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_LINEA_PEDIDO", OracleDbType.Int32, idLineaPedido, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_ESTADO_FACTURACION", OracleDbType.Int32, ELL.Pedido.EstadoFacturacion.Invoiced, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_USUARIO_ALTA", OracleDbType.Int32, idUsuario, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                ' 3º Guardamos el tipo de cambio en pedido si la moneda de facturación y de planta son diferentes
                Try
                    Dim lineaPedido As ELL.LineaPedido = BLL.LineasPedidoBLL.Obtener(idLineaPedido)
                    Dim paso As ELL.Step = BLL.StepsBLL.Obtener(lineaPedido.IdStep)

                    If (lineaPedido.IdMonedaFacturacion <> paso.IdMoneda) Then
                        Dim jss As New JavaScriptSerializer()
                        Dim cambio As Decimal = 1
                        Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                            cambio = cliente.GetOfferExchange(DateTime.Today, lineaPedido.IdMonedaFacturacion, paso.IdMoneda)
                        End Using

                        query = "UPDATE PEDIDO SET CAMBIO=:CAMBIO WHERE ID=:ID"

                        lParameters = New List(Of OracleParameter)
                        lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, lineaPedido.IdPedido, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("CAMBIO", OracleDbType.Decimal, cambio, ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)
                    End If
                Catch
                End Try

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