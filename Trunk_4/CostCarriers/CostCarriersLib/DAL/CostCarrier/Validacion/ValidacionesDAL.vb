Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ValidacionesDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene validación
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal id As Integer) As ELL.Validacion
            Dim query As String = "SELECT * FROM VALIDACION WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Validacion)(Function(r As OracleDataReader) _
            New ELL.Validacion With {.Id = CInt(r("ID")), .Fecha = CDate(r("FECHA")), .IdUser = CInt(r("ID_USER")),
                                     .Denominacion = CStr(r("DENOMINACION")), .Descripcion = CStr(r("DESCRIPCION")),
                                     .PrevistoPG = CBool(r("PREVISTO_PG")),
                                     .IdCabecera = CInt(r("ID_CABECERA"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene validación
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Public Shared Function getLastObjectByCabecera(ByVal idCabecera As Integer) As ELL.Validacion
            Dim query As String = "SELECT * FROM VALIDACION VAL
                                   WHERE ID_CABECERA=:ID_CABECERA AND VAL.FECHA = (SELECT MAX(FECHA) FROM VALIDACION WHERE ID_CABECERA=:ID_CABECERA)"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Validacion)(Function(r As OracleDataReader) _
            New ELL.Validacion With {.Id = CInt(r("ID")), .Fecha = CDate(r("FECHA")), .IdUser = CInt(r("ID_USER")),
                                     .Denominacion = CStr(r("DENOMINACION")), .Descripcion = CStr(r("DESCRIPCION")),
                                     .PrevistoPG = CBool(r("PREVISTO_PG")),
                                     .IdCabecera = CInt(r("ID_CABECERA"))}, query, CadenaConexion, New OracleParameter("ID_CABECERA", OracleDbType.Int32, idCabecera, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Carga las validaciones por cabecera
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Public Shared Function loadListByCabecera(ByVal idCabecera As Integer) As List(Of ELL.Validacion)
            Dim query As String = "SELECT * FROM VALIDACION WHERE ID_CABECERA=:ID_CABECERA"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Validacion)(Function(r As OracleDataReader) _
            New ELL.Validacion With {.Id = CInt(r("ID")), .Fecha = CDate(r("FECHA")), .IdUser = CInt(r("ID_USER")),
                                     .Denominacion = CStr(r("DENOMINACION")), .Descripcion = CStr(r("DESCRIPCION")),
                                     .PrevistoPG = CBool(r("PREVISTO_PG")),
                                     .IdCabecera = CInt(r("ID_CABECERA"))}, query, CadenaConexion, New OracleParameter("ID_CABECERA", OracleDbType.Int32, idCabecera, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' Carga las validaciones por planta con el ultimo estado de cada step
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Shared Function loadListByPlanta(ByVal idPlanta As Integer) As List(Of ELL.Validacion)
            Dim query As String = "SELECT * FROM VVALIDACION_ULTIMO WHERE ID_PLANTA=:ID_PLANTA"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Validacion)(Function(r As OracleDataReader) _
            New ELL.Validacion With {.Id = CInt(r("ID")), .Fecha = CDate(r("FECHA")), .IdUser = CInt(r("ID_USER")),
                                     .Denominacion = CStr(r("DENOMINACION")), .Descripcion = CStr(r("DESCRIPCION")),
                                     .PrevistoPG = CBool(r("PREVISTO_PG")),
                                     .IdEstadoValidacion = CInt(r("ID_ESTADO_VALIDACION")), .IdPlanta = CInt(r("ID_PLANTA")),
                                     .Planta = CStr(r("Planta"))}, query, CadenaConexion, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda
        ''' </summary>
        ''' <param name="validacion"></param>
        ''' <remarks></remarks>
        Public Shared Function Save(ByVal validacion As ELL.Validacion) As List(Of ELL.FlujoAprobacion)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty
            Dim listaFlujoAprobacion As New List(Of ELL.FlujoAprobacion)

            Try
                ' Vamos a ver el tipo de proyecto ptksis (policy) para luego crear el flujo de validación
                Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(validacion.IdCabecera, False)

                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                Dim lParameters As New List(Of OracleParameter)

                ' 1- Guardamos la validación
                query = "INSERT INTO VALIDACION (ID_USER, DENOMINACION, DESCRIPCION, PREVISTO_PG, ID_CABECERA)
                        VALUES (:ID_USER, :DENOMINACION, :DESCRIPCION, :PREVISTO_PG, :ID_CABECERA) RETURNING ID INTO :RETURN_VALUE"

                lParameters.Clear()
                lParameters.Add(New OracleParameter("ID_USER", OracleDbType.Int32, validacion.IdUser, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("DENOMINACION", OracleDbType.NVarchar2, validacion.Denominacion, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, validacion.Descripcion, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("PREVISTO_PG", OracleDbType.Int32, validacion.PrevistoPG, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_CABECERA", OracleDbType.Int32, validacion.IdCabecera, ParameterDirection.Input))

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParameters.Add(p)

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                validacion.Id = lParameters.Last.Value

                ' 2- Guardamos las validaciones de línea
                For Each validacionLinea In validacion.ValidacionesLinea
                    query = "INSERT INTO VALIDACION_LINEA (ID_VALIDACION, ID_STEP, OFFER_BUDGET, PAID_BY_CUSTOMER, BUDGET_APPROVED, HOURS, NOMBRE)
                        VALUES (:ID_VALIDACION, :ID_STEP, :OFFER_BUDGET, :PAID_BY_CUSTOMER, :BUDGET_APPROVED, :HOURS, :NOMBRE) RETURNING ID INTO :RETURN_VALUE"

                    lParameters.Clear()
                    lParameters.Add(New OracleParameter("ID_VALIDACION", OracleDbType.Int32, validacion.Id, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID_STEP", OracleDbType.Int32, validacionLinea.IdStep, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("OFFER_BUDGET", OracleDbType.Int32, If(validacionLinea.OfferBudget = Integer.MinValue, DBNull.Value, validacionLinea.OfferBudget), ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("PAID_BY_CUSTOMER", OracleDbType.Int32, validacionLinea.PaidByCustomer, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("BUDGET_APPROVED", OracleDbType.Int32, validacionLinea.BudgetApproved, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("HOURS", OracleDbType.Int32, validacionLinea.Hours, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(validacionLinea.Nombre), DBNull.Value, validacionLinea.Nombre), ParameterDirection.Input))

                    p = New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                    p.DbType = DbType.Int32
                    lParameters.Add(p)

                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                    validacionLinea.Id = lParameters.Last.Value

                    ' ************************* Composición del flujo de validación por cada validación linea ************************************
                    listaFlujoAprobacion = BLL.FlujosAprobacionBLL.ComponerFlujoAprobacion(cabecera, validacionLinea)

                    ' Guardamos los flujos de aprobación
                    For Each flujoAprobacion In listaFlujoAprobacion
                        query = "INSERT INTO FLUJO_APROBACION (ORDEN, ID_SAB, ID_VALIDACION_LINEA, PORCENTAJE) 
                                 VALUES (:ORDEN, :ID_SAB, :ID_VALIDACION_LINEA, :PORCENTAJE)"

                        lParameters.Clear()
                        lParameters.Add(New OracleParameter("ORDEN", OracleDbType.Int32, flujoAprobacion.Orden, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, flujoAprobacion.IdSab, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_VALIDACION_LINEA", OracleDbType.Int32, flujoAprobacion.IdValidacionLinea, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("PORCENTAJE", OracleDbType.Decimal, If(flujoAprobacion.Porcentaje = Decimal.MinValue, DBNull.Value, flujoAprobacion.Porcentaje), ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    Next
                    ' *****************************************************************************************************************************

                    ' 3- Guardamos los valores de años para cada linea
                    For Each validacionAño In validacionLinea.ValidacionesAño
                        query = "INSERT INTO VALIDACION_AÑO (ID_VALIDACION_LINEA, AÑO, TRIMESTRE, VALOR, ID_COLUMNA)
                            VALUES (:ID_VALIDACION_LINEA, :AÑO, :TRIMESTRE, :VALOR, :ID_COLUMNA)"

                        lParameters.Clear()
                        lParameters.Add(New OracleParameter("ID_VALIDACION_LINEA", OracleDbType.Int32, validacionLinea.Id, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("AÑO", OracleDbType.Int32, validacionAño.Año, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("TRIMESTRE", OracleDbType.Int32, validacionAño.Trimestre, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("VALOR", OracleDbType.Int32, validacionAño.Valor, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_COLUMNA", OracleDbType.Int32, validacionAño.IdColumna, ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    Next

                    ' 4- Guardamos una entrada por cada línea en el historico
                    query = "INSERT INTO HISTORICO_ESTADO_LINEA (ID_ESTADO_VALIDACION, ID_USER, ID_VALIDACION_LINEA, ID_ACCION_VALIDACION)
                            VALUES (:ID_ESTADO_VALIDACION, :ID_USER, :ID_VALIDACION_LINEA, :ID_ACCION_VALIDACION)"

                    lParameters.Clear()
                    lParameters.Add(New OracleParameter("ID_ESTADO_VALIDACION", OracleDbType.Int32, ELL.Validacion.Estado.Waiting_for_approval, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID_USER", OracleDbType.Int32, validacion.IdUser, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID_VALIDACION_LINEA", OracleDbType.Int32, validacionLinea.Id, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID_ACCION_VALIDACION", OracleDbType.Int32, ELL.Validacion.Accion.Send_to_validate, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                Next

                ' 5- Guardamos las validaciones info adicional
                If (validacion.ValidacionesInfoAdicional IsNot Nothing AndAlso validacion.ValidacionesInfoAdicional.Count > 0) Then
                    For Each validacionInfo In validacion.ValidacionesInfoAdicional

                        lParameters.Clear()
                        lParameters.Add(New OracleParameter("NET_MARGIN", OracleDbType.Decimal, If(validacionInfo.NetMargin = Decimal.MinValue, DBNull.Value, validacionInfo.NetMargin), ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("EFFECTIVE_SALES", OracleDbType.Int32, If(validacionInfo.EffectiveSales = Integer.MinValue, DBNull.Value, validacionInfo.EffectiveSales), ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("CUSTOMER_PROPERTY", OracleDbType.Decimal, If(validacionInfo.CustomerProperty = Decimal.MinValue, DBNull.Value, validacionInfo.CustomerProperty), ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("CUSTOMER_PLANTS", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(validacionInfo.CustomerPlants), DBNull.Value, validacionInfo.CustomerPlants), ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("SOP", OracleDbType.Date, If(validacionInfo.SOP = DateTime.MinValue, DBNull.Value, validacionInfo.SOP), ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("AVERAGE_VOLUMEN", OracleDbType.Int32, If(validacionInfo.AverageVolumen = Integer.MinValue, DBNull.Value, validacionInfo.AverageVolumen), ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ANYOS_SERIE", OracleDbType.Int32, If(validacionInfo.SeriesYears = Integer.MinValue, DBNull.Value, validacionInfo.SeriesYears), ParameterDirection.Input))

                        If (validacionInfo.Id = 0) Then
                            lParameters.Add(New OracleParameter("ID_VALIDACION", OracleDbType.Int32, validacion.Id, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, validacionInfo.IdPlanta, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("TIPO", OracleDbType.Int32, validacionInfo.Tipo, ParameterDirection.Input))

                            query = "INSERT INTO VALIDACION_INFO_ADICIONAL (ID_VALIDACION, ID_PLANTA, NET_MARGIN, EFFECTIVE_SALES, CUSTOMER_PROPERTY, CUSTOMER_PLANTS, SOP, AVERAGE_VOLUMEN, TIPO, ANYOS_SERIE)
                                     VALUES (:ID_VALIDACION, :ID_PLANTA, :NET_MARGIN, :EFFECTIVE_SALES, :CUSTOMER_PROPERTY, :CUSTOMER_PLANTS, :SOP, :AVERAGE_VOLUMEN, :TIPO, :ANYOS_SERIE)"
                        Else
                            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, validacionInfo.Id, ParameterDirection.Input))

                            query = "UPDATE VALIDACION_INFO_ADICIONAL SET NET_MARGIN=:NET_MARGIN, EFFECTIVE_SALES=:EFFECTIVE_SALES, CUSTOMER_PROPERTY=:CUSTOMER_PROPERTY, CUSTOMER_PLANTS=:CUSTOMER_PLANTS,
                                     SOP=:SOP, AVERAGE_VOLUMEN=:AVERAGE_VOLUMEN, ANYOS_SERIE=:ANYOS_SERIE WHERE ID=:ID"
                        End If

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    Next
                End If

                transact.Commit()
                Return listaFlujoAprobacion
            Catch ex As Exception
                transact.Rollback()
                Throw ex
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Function

#End Region

    End Class

End Namespace