Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ValidacionesLineaDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene lista de validaciones línea
        ''' </summary>
        ''' <param name="idValidacion"></param>
        ''' <returns></returns>
        Public Shared Function loadListByValidacion(ByVal idValidacion As Integer) As List(Of ELL.ValidacionLinea)
            Dim query As String = "SELECT * FROM VVALIDACION_LINEA WHERE ID_VALIDACION=:ID_VALIDACION"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionLinea)(Function(r As OracleDataReader) _
            New ELL.ValidacionLinea With {.Id = CInt(r("ID")), .IdValidacion = CInt(r("ID_VALIDACION")), .IdStep = CInt(r("ID_STEP")),
                                          .PaidByCustomer = CInt(r("PAID_BY_CUSTOMER")), .BudgetApproved = CInt(r("BUDGET_APPROVED")),
                                          .OfferBudget = SabLib.BLL.Utils.integerNull(r("OFFER_BUDGET")), .Hours = CInt(r("HOURS")),
                                          .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                                          .IdPlanta = CInt(r("ID_PLANTA"))}, query, CadenaConexion, New OracleParameter("ID_VALIDACION", OracleDbType.Int32, idValidacion, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' Obtiene lista de validaciones línea
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <returns></returns>
        Public Shared Function loadListByCostGroup(ByVal idCostGroup As Integer) As List(Of ELL.ValidacionLinea)
            Dim query As String = "SELECT * FROM VVALIDACION_LINEA WHERE ID_COST_GROUP=:ID_COST_GROUP"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionLinea)(Function(r As OracleDataReader) _
            New ELL.ValidacionLinea With {.Id = CInt(r("ID")), .IdValidacion = CInt(r("ID_VALIDACION")), .IdStep = CInt(r("ID_STEP")),
                                          .PaidByCustomer = CInt(r("PAID_BY_CUSTOMER")), .BudgetApproved = CInt(r("BUDGET_APPROVED")),
                                          .OfferBudget = SabLib.BLL.Utils.integerNull(r("OFFER_BUDGET")), .Hours = CInt(r("HOURS")),
                                          .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                                          .IdPlanta = CInt(r("ID_PLANTA"))}, query, CadenaConexion, New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, idCostGroup, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' Obtiene lista de validaciones línea
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <returns></returns>
        Public Shared Function loadListByStep(ByVal idStep As Integer) As List(Of ELL.ValidacionLinea)
            Dim query As String = "SELECT * FROM VVALIDACION_LINEA WHERE ID_STEP=:ID_STEP"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionLinea)(Function(r As OracleDataReader) _
            New ELL.ValidacionLinea With {.Id = CInt(r("ID")), .IdValidacion = CInt(r("ID_VALIDACION")), .IdStep = CInt(r("ID_STEP")),
                                          .PaidByCustomer = CInt(r("PAID_BY_CUSTOMER")), .BudgetApproved = CInt(r("BUDGET_APPROVED")),
                                          .OfferBudget = SabLib.BLL.Utils.integerNull(r("OFFER_BUDGET")), .Hours = CInt(r("HOURS")),
                                          .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                                          .IdPlanta = CInt(r("ID_PLANTA"))}, query, CadenaConexion, New OracleParameter("ID_STEP", OracleDbType.Int32, idStep, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' Lista de validaciones linea aprobados por cost group
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <returns></returns>
        Public Shared Function loadListValidadosByCostGroup(ByVal idCostGroup As Integer) As List(Of ELL.ValidacionLinea)
            Dim query As String = "SELECT * FROM VVALIDACION_LINEA_VALIDADOS WHERE ID_COST_GROUP=:ID_COST_GROUP"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionLinea)(Function(r As OracleDataReader) _
            New ELL.ValidacionLinea With {.IdCostGroup = CInt(r("ID_COST_GROUP")), .PaidByCustomer = CInt(r("PAID_BY_CUSTOMER")), .BudgetApproved = CInt(r("BUDGET_APPROVED")),
                                          .Hours = If(r("HOURS") Is DBNull.Value, 0, CInt(r("HOURS"))), .OfferBudget = SabLib.BLL.Utils.integerNull(r("OFFER_BUDGET"))}, query, CadenaConexion, New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, idCostGroup, ParameterDirection.Input))
        End Function


        '''' <summary>
        '''' Obtiene lista de validaciones línea validados. Datos calculados
        '''' </summary>
        '''' <param name="idValidacion"></param>
        '''' <returns></returns>
        'Public Shared Function loadListValidatedByValidacion(ByVal idValidacion As Integer) As List(Of ELL.ValidacionLinea)
        '    Dim query As String = "SELECT * FROM VVALIDACION_LINEA_VALIDADOS WHERE ID_VALIDACION=:ID_VALIDACION"

        '    Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionLinea)(Function(r As OracleDataReader) _
        '    New ELL.ValidacionLinea With {.IdPlanta = CInt(r("ID_PLANTA")), .BudgetApproved = CInt(r("BUDGET_APPROVED")),
        '                                  .Hours = CInt(r("HOURS")), .OfferBudget = SabLib.BLL.Utils.integerNull(r("OFFER_BUDGET")),
        '                                  .IdCostGroup = CInt(r("ID_COST_GROUP")), .PaidByCustomer = CInt(r("PAID_BY_CUSTOMER"))}, query, CadenaConexion, New OracleParameter("ID_VALIDACION", OracleDbType.Int32, idValidacion, ParameterDirection.Input))
        'End Function

        ''' <summary>
        ''' Obtiene lista de validaciones línea
        ''' </summary>
        ''' <param name="idValidacion"></param>
        ''' <returns></returns>
        Public Shared Function loadListUltimoByValidacion(ByVal idValidacion As Integer) As List(Of ELL.ValidacionLinea)
            Dim query As String = "SELECT * FROM VVALIDACION_LINEA_ULTIMO WHERE ID_VALIDACION=:ID_VALIDACION"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_VALIDACION", OracleDbType.Int32, idValidacion, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionLinea)(Function(r As OracleDataReader) _
            New ELL.ValidacionLinea With {.Id = CInt(r("ID")), .IdValidacion = CInt(r("ID_VALIDACION")), .IdStep = CInt(r("ID_STEP")),
                                          .PaidByCustomer = CInt(r("PAID_BY_CUSTOMER")), .BudgetApproved = CInt(r("BUDGET_APPROVED")),
                                          .Hours = CInt(r("HOURS")), .OfferBudget = SabLib.BLL.Utils.integerNull(r("OFFER_BUDGET")),
                                          .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .IdEstadoValidacion = CInt(r("ID_ESTADO_VALIDACION")),
                                          .IdUser = CInt(r("ID_USER")), .IdPlanta = CInt(r("ID_PLANTA"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' Para un idStep nos devuelve validación línea más reciente que esté en estado aprobado, abierto o cerrado
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <returns></returns>
        Public Shared Function getObjectUltimoAprobado(ByVal idStep As Integer) As ELL.ValidacionLinea
            Dim query As String = "SELECT * FROM VVALIDACION_LINEA_ULTIMO_VALID WHERE ID_STEP=:ID_STEP"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_STEP", OracleDbType.Int32, idStep, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionLinea)(Function(r As OracleDataReader) _
            New ELL.ValidacionLinea With {.Id = CInt(r("ID")), .IdValidacion = CInt(r("ID_VALIDACION")), .IdStep = CInt(r("ID_STEP")),
                                          .PaidByCustomer = CInt(r("PAID_BY_CUSTOMER")), .BudgetApproved = CInt(r("BUDGET_APPROVED")),
                                          .Hours = CInt(r("HOURS")), .OfferBudget = SabLib.BLL.Utils.integerNull(r("OFFER_BUDGET")),
                                          .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE"))}, query, CadenaConexion, lParameters.ToArray()).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene una validación línea validada por costgroup. Los datos ya están calculados
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <returns></returns>
        Public Shared Function getObjectValidadosByCostGroup(ByVal idCostGroup As Integer) As ELL.ValidacionLinea
            Dim query As String = "SELECT * FROM VVALIDACION_LINEA_VALIDADOS WHERE ID_COST_GROUP=:ID_COST_GROUP"
            Dim where As String = String.Empty
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, idCostGroup, ParameterDirection.Input))

            Dim lista As List(Of ELL.ValidacionLinea) = Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionLinea)(Function(r As OracleDataReader) _
            New ELL.ValidacionLinea With {.IdCostGroup = CInt(r("ID_COST_GROUP")), .PaidByCustomer = CInt(r("PAID_BY_CUSTOMER")), .BudgetApproved = CInt(r("BUDGET_APPROVED")),
                                          .Hours = If(r("HOURS") Is DBNull.Value, 0, CInt(r("HOURS"))), .OfferBudget = SabLib.BLL.Utils.integerNull(r("OFFER_BUDGET"))}, query, CadenaConexion, lParameters.ToArray())

            ' Puede venir mas de una fila porque la vista está agrupada por ID_VALIDACION. Agrupamos a mano los valores
            Dim validacionLinea As New ELL.ValidacionLinea With {.IdCostGroup = idCostGroup}
            validacionLinea.PaidByCustomer = lista.Sum(Function(f) f.PaidByCustomer)
            validacionLinea.BudgetApproved = lista.Sum(Function(f) f.BudgetApproved)
            validacionLinea.Hours = lista.Sum(Function(f) f.Hours)
            validacionLinea.OfferBudget = lista.Sum(Function(f) If(f.OfferBudget = Integer.MinValue, 0, f.OfferBudget))

            Return validacionLinea
        End Function

        ''' <summary>
        ''' Obtiene una validaciones línea
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal id As Integer) As ELL.ValidacionLinea
            Dim query As String = "SELECT * FROM VVALIDACION_LINEA WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionLinea)(Function(r As OracleDataReader) _
            New ELL.ValidacionLinea With {.Id = CInt(r("ID")), .IdValidacion = CInt(r("ID_VALIDACION")), .IdStep = CInt(r("ID_STEP")),
                                          .PaidByCustomer = CInt(r("PAID_BY_CUSTOMER")), .BudgetApproved = CInt(r("BUDGET_APPROVED")),
                                          .OfferBudget = SabLib.BLL.Utils.integerNull(r("OFFER_BUDGET")), .Hours = CInt(r("HOURS")),
                                          .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                                          .IdPlanta = CInt(r("ID_PLANTA"))}, query, CadenaConexion, lParameters.ToArray()).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene unavalidaciones línea
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <returns></returns>
        Public Shared Function getObjectByIdStep(ByVal idStep As Integer) As ELL.ValidacionLinea
            Dim query As String = "SELECT * FROM VVALIDACION_LINEA_ULTIMO WHERE ID_STEP=:ID_STEP"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_STEP", OracleDbType.Int32, idStep, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ValidacionLinea)(Function(r As OracleDataReader) _
            New ELL.ValidacionLinea With {.Id = CInt(r("ID")), .IdValidacion = CInt(r("ID_VALIDACION")), .IdStep = CInt(r("ID_STEP")),
                                          .PaidByCustomer = CInt(r("PAID_BY_CUSTOMER")), .BudgetApproved = CInt(r("BUDGET_APPROVED")),
                                          .OfferBudget = SabLib.BLL.Utils.integerNull(r("OFFER_BUDGET")), .Hours = CInt(r("HOURS")),
                                          .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .IdEstadoValidacion = CInt(r("ID_ESTADO_VALIDACION")),
                                          .IdUser = CInt(r("ID_USER")), .IdPlanta = CInt(r("ID_PLANTA"))}, query, CadenaConexion, lParameters.ToArray()).FirstOrDefault()
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idValidacionLinea"></param>
        ''' <param name="idUser"></param>
        ''' <param name="comentarios"></param>
        ''' <param name="estadoValidacion"></param>
        ''' <param name="orden"></param>
        ''' <param name="idAccionValidacion"></param>
        ''' <param name="fecha"></param>
        Public Shared Sub AddValidationState(ByVal idValidacionLinea As Integer, ByVal idUser As Integer, ByVal comentarios As String, ByVal estadoValidacion As Integer, ByVal idAccionValidacion As Integer, ByVal orden As Integer, Optional ByVal fecha As DateTime? = Nothing)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty

            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                ' 1.- Guardamos el nuevo estado
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_ESTADO_VALIDACION", OracleDbType.Int32, estadoValidacion, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("COMENTARIOS", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(comentarios), DBNull.Value, comentarios), ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_VALIDACION_LINEA", OracleDbType.Int32, idValidacionLinea, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_ACCION_VALIDACION", OracleDbType.Int32, idAccionValidacion, ParameterDirection.Input))

                If (fecha Is Nothing) Then
                    query = "INSERT INTO HISTORICO_ESTADO_LINEA (ID_ESTADO_VALIDACION, ID_USER, COMENTARIOS, ID_VALIDACION_LINEA, ID_ACCION_VALIDACION)
                                   VALUES (:ID_ESTADO_VALIDACION, :ID_USER, :COMENTARIOS, :ID_VALIDACION_LINEA, :ID_ACCION_VALIDACION)"
                Else
                    query = "INSERT INTO HISTORICO_ESTADO_LINEA (ID_ESTADO_VALIDACION, ID_USER, COMENTARIOS, ID_VALIDACION_LINEA, ID_ACCION_VALIDACION, FECHA)
                                   VALUES (:ID_ESTADO_VALIDACION, :ID_USER, :COMENTARIOS, :ID_VALIDACION_LINEA, :ID_ACCION_VALIDACION, :FECHA)"

                    lParameters.Add(New OracleParameter("FECHA", OracleDbType.Date, fecha, ParameterDirection.Input))
                End If

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                ' 2.- Borramos del flujo de aprobación la línea que toque
                If (orden <> Integer.MinValue) Then
                    lParameters = New List(Of OracleParameter)
                    lParameters.Add(New OracleParameter("ID_VALIDACION_LINEA", OracleDbType.Int32, idValidacionLinea, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ORDEN", OracleDbType.Int32, orden, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, idUser, ParameterDirection.Input))

                    query = "DELETE FROM FLUJO_APROBACION WHERE ID_VALIDACION_LINEA=:ID_VALIDACION_LINEA AND ORDEN=:ORDEN AND ID_SAB=:ID_SAB"

                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                End If

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
        ''' 
        ''' </summary>
        ''' <param name="idValidacionLinea"></param>
        ''' <param name="idUser"></param>
        ''' <param name="estadoValidacion"></param>
        ''' <param name="idAccionValidacion"></param>
        ''' <param name="orden"></param>
        ''' <param name="porcentaje"></param>
        Public Shared Sub DeleteValidationState(ByVal idValidacionLinea As Integer, ByVal idUser As Integer, ByVal estadoValidacion As Integer, ByVal idAccionValidacion As Integer, ByVal orden As Integer, ByVal porcentaje As Decimal)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty

            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                ' 1.- Eliminamos el estado
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_ESTADO_VALIDACION", OracleDbType.Int32, estadoValidacion, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_VALIDACION_LINEA", OracleDbType.Int32, idValidacionLinea, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_ACCION_VALIDACION", OracleDbType.Int32, idAccionValidacion, ParameterDirection.Input))

                query = "DELETE FROM HISTORICO_ESTADO_LINEA WHERE ID_ESTADO_VALIDACION=:ID_ESTADO_VALIDACION AND ID_USER=:ID_USER AND ID_VALIDACION_LINEA=:ID_VALIDACION_LINEA AND ID_ACCION_VALIDACION=:ID_ACCION_VALIDACION"

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                ' 2.- Añadimos al flujo de aprobación la línea que toque
                lParameters = New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ORDEN", OracleDbType.Int32, orden, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, idUser, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_VALIDACION_LINEA", OracleDbType.Int32, idValidacionLinea, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("PORCENTAJE", OracleDbType.Decimal, If(porcentaje = Decimal.MinValue, DBNull.Value, porcentaje), ParameterDirection.Input))

                query = "INSERT INTO FLUJO_APROBACION (ORDEN, ID_SAB, ID_VALIDACION_LINEA, PORCENTAJE) VALUES (:ORDEN, :ID_SAB, :ID_VALIDACION_LINEA, :PORCENTAJE)"

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

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