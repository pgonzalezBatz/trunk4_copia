Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class StepsPlantillaDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un step plantilla
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getStepPlantilla(ByVal id As Integer) As ELL.StepPlantilla
            Dim query As String = "SELECT * FROM STEP_PLANTILLA WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.StepPlantilla)(Function(r As OracleDataReader) _
            New ELL.StepPlantilla With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION")), .IdCostGroupPlantilla = CInt(r("ID_COST_GROUP_PLANTILLA")),
                                        .OBCOrigenDatos = CInt(r("OBC_ORIGEN_DATOS")), .TCFormula = CStr(r("TC_FORMULA")), .GastosAñoOrigenDatos = CInt(r("GASTOS_AÑO_ORIGEN_DATOS")),
                                        .IngresosAñoOrigenDatos = CInt(r("INGRESOS_AÑO_ORIGEN_DATOS")), .Orden = CInt(r("ORDEN")),
                                        .OrigenDatoReal = CInt(r("ORIGEN_DATO_REAL")), .TCFormulaCustomer = SabLib.BLL.Utils.stringNull(r("TC_FORMULA_CUSTOMER")),
                                        .EsInfoGeneral = CBool(r("ES_INFO_GENERAL"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene steps plantilla
        ''' </summary>
        ''' <param name="idCostGroupPlantilla"></param>
        ''' <returns></returns>
        Public Shared Function loadList(ByVal idCostGroupPlantilla As Integer) As List(Of ELL.StepPlantilla)
            Dim query As String = "SELECT * FROM STEP_PLANTILLA WHERE ID_COST_GROUP_PLANTILLA=:ID_COST_GROUP_PLANTILLA ORDER BY ORDEN"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.StepPlantilla)(Function(r As OracleDataReader) _
            New ELL.StepPlantilla With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION")), .IdCostGroupPlantilla = CInt(r("ID_COST_GROUP_PLANTILLA")),
                                        .OBCOrigenDatos = CInt(r("OBC_ORIGEN_DATOS")), .TCFormula = CStr(r("TC_FORMULA")), .GastosAñoOrigenDatos = CInt(r("GASTOS_AÑO_ORIGEN_DATOS")),
                                        .IngresosAñoOrigenDatos = CInt(r("INGRESOS_AÑO_ORIGEN_DATOS")), .Orden = CInt(r("ORDEN")),
                                        .OrigenDatoReal = CInt(r("ORIGEN_DATO_REAL")), .TCFormulaCustomer = SabLib.BLL.Utils.stringNull(r("TC_FORMULA_CUSTOMER")),
                                        .EsInfoGeneral = CBool(r("ES_INFO_GENERAL"))}, query, CadenaConexion, New OracleParameter("ID_COST_GROUP_PLANTILLA", OracleDbType.Int32, idCostGroupPlantilla, ParameterDirection.Input))
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un step plantila
        ''' </summary>
        ''' <param name="stepPlantilla"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal stepPlantilla As ELL.StepPlantilla)
            Dim query As String = String.Empty
            Dim bNuevo As Boolean = (stepPlantilla.Id = 0)

            ' Guardamos
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, stepPlantilla.Descripcion, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("OBC_ORIGEN_DATOS", OracleDbType.Int32, stepPlantilla.OBCOrigenDatos, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("TC_FORMULA", OracleDbType.NVarchar2, stepPlantilla.TCFormula, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("TC_FORMULA_CUSTOMER", OracleDbType.NVarchar2, stepPlantilla.TCFormulaCustomer, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("GASTOS_AÑO_ORIGEN_DATOS", OracleDbType.Int32, stepPlantilla.GastosAñoOrigenDatos, ParameterDirection.Input))
            ' El origen de datos del ingresos por año siempre es nada
            lParameters.Add(New OracleParameter("INGRESOS_AÑO_ORIGEN_DATOS", OracleDbType.Int32, ELL.OrigenDatosStep.OrigenDatosStep.Manual, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ORIGEN_DATO_REAL", OracleDbType.Int32, stepPlantilla.OrigenDatoReal, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ES_INFO_GENERAL", OracleDbType.Int32, stepPlantilla.EsInfoGeneral, ParameterDirection.Input))

            If (bNuevo) Then
                query = "INSERT INTO STEP_PLANTILLA (DESCRIPCION, ID_COST_GROUP_PLANTILLA, OBC_ORIGEN_DATOS, TC_FORMULA, TC_FORMULA_CUSTOMER, GASTOS_AÑO_ORIGEN_DATOS, INGRESOS_AÑO_ORIGEN_DATOS, ORDEN, ORIGEN_DATO_REAL, ES_INFO_GENERAL) " _
                        & "VALUES (:DESCRIPCION, :ID_COST_GROUP_PLANTILLA, :OBC_ORIGEN_DATOS, :TC_FORMULA, :TC_FORMULA_CUSTOMER, :GASTOS_AÑO_ORIGEN_DATOS, :INGRESOS_AÑO_ORIGEN_DATOS, (SELECT NVL(MAX(ORDEN), 0) + 1 FROM STEP_PLANTILLA WHERE ID_COST_GROUP_PLANTILLA=:ID_COST_GROUP_PLANTILLA), :ORIGEN_DATO_REAL, :ES_INFO_GENERAL) RETURNING ID INTO :RETURN_VALUE"

                lParameters.Add(New OracleParameter("ID_COST_GROUP_PLANTILLA", OracleDbType.Int32, stepPlantilla.IdCostGroupPlantilla, ParameterDirection.Input))

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParameters.Add(p)
            Else
                query = "UPDATE STEP_PLANTILLA SET DESCRIPCION=:DESCRIPCION, OBC_ORIGEN_DATOS=:OBC_ORIGEN_DATOS, " _
                        & "TC_FORMULA=:TC_FORMULA, TC_FORMULA_CUSTOMER=:TC_FORMULA_CUSTOMER, GASTOS_AÑO_ORIGEN_DATOS=:GASTOS_AÑO_ORIGEN_DATOS, INGRESOS_AÑO_ORIGEN_DATOS=:INGRESOS_AÑO_ORIGEN_DATOS, ORIGEN_DATO_REAL=:ORIGEN_DATO_REAL, ES_INFO_GENERAL=:ES_INFO_GENERAL WHERE ID=:ID"

                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, stepPlantilla.Id, ParameterDirection.Input))
            End If

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)

            If (bNuevo) Then
                stepPlantilla.Id = lParameters.Last.Value
            End If
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ordenActual"></param>
        Public Shared Sub SwapOrderSteps(ByVal ordenActual() As Integer)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty
            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                query = "UPDATE STEP_PLANTILLA SET ORDEN=:ORDEN WHERE ID=:ID"
                Dim lParameters As New List(Of OracleParameter)
                Dim contador As Integer = 1
                For Each idStep In ordenActual
                    lParameters = New List(Of OracleParameter)
                    lParameters.Add(New OracleParameter("ORDEN", OracleDbType.Int32, contador, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idStep, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    contador += 1
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
        ''' Elimina un step plantilla en cascada
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Delete(ByVal id As Integer)
            Dim query As String = "DELETE FROM STEP_PLANTILLA WHERE ID=:ID"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

    End Class

End Namespace