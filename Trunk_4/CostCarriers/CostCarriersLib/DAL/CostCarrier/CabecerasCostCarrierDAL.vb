Imports System.Web.Script.Serialization
Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CabecerasCostCarrierDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una cabecera de cost carrier
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getCabecera(ByVal id As Integer) As ELL.CabeceraCostCarrier
            Dim query As String = "SELECT * FROM VCABECERA_COST_CARRIER WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CabeceraCostCarrier)(Function(r As OracleDataReader) _
            New ELL.CabeceraCostCarrier With {.Id = CInt(r("ID")), .IdTipoProyecto = CStr(r("ID_TIPO_PROYECTO")), .Proyecto = CStr(r("PROYECTO")),
                                              .Abreviatura = SabLib.BLL.Utils.stringNull(r("ABREVIATURA")),
                                              .NombreProyecto = CStr(r("NOMBRE_PROYECTO")), .IdOferta = SabLib.BLL.Utils.integerNull(r("ID_OFERTA")),
                                              .CodigoProyecto = SabLib.BLL.Utils.stringNull(r("CODIGO_PROYECTO")), .Responsable = CStr(r("RESPONSABLE")),
                                              .TipoProyecto = CStr(r("DESCRIPCION")), .CoOwner = SabLib.BLL.Utils.stringNull(r("COOWNER")),
                                              .Producto = CStr(r("PRODUCTO")), .TipoProyPtksis = CStr(r("TIPO_PROY_PTKSIS")).ToUpper(), .SOP = SabLib.BLL.Utils.dateTimeNull(r("SOP")),
                                              .SeriesYears = SabLib.BLL.Utils.integerNull(r("ANYOS_SERIE")), .Cliente = SabLib.BLL.Utils.stringNull(r("CLIENTE"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene una cabecera de cost carrier
        ''' </summary>
        ''' <param name="idProyecto"></param>
        ''' <returns></returns>
        Public Shared Function getCabeceraByProyecto(ByVal idProyecto As String) As ELL.CabeceraCostCarrier
            Dim query As String = "SELECT * FROM VCABECERA_COST_CARRIER WHERE PROYECTO=:PROYECTO"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CabeceraCostCarrier)(Function(r As OracleDataReader) _
            New ELL.CabeceraCostCarrier With {.Id = CInt(r("ID")), .IdTipoProyecto = CStr(r("ID_TIPO_PROYECTO")), .Proyecto = CStr(r("PROYECTO")),
                                              .Abreviatura = SabLib.BLL.Utils.stringNull(r("ABREVIATURA")),
                                              .NombreProyecto = CStr(r("NOMBRE_PROYECTO")), .IdOferta = SabLib.BLL.Utils.integerNull(r("ID_OFERTA")),
                                              .CodigoProyecto = SabLib.BLL.Utils.stringNull(r("CODIGO_PROYECTO")), .Responsable = CStr(r("RESPONSABLE")),
                                              .TipoProyecto = CStr(r("DESCRIPCION")), .CoOwner = SabLib.BLL.Utils.stringNull(r("COOWNER")),
                                              .Producto = CStr(r("PRODUCTO")), .TipoProyPtksis = CStr(r("TIPO_PROY_PTKSIS")).ToUpper(), .SOP = SabLib.BLL.Utils.dateTimeNull(r("SOP")),
                                              .SeriesYears = SabLib.BLL.Utils.integerNull(r("ANYOS_SERIE")), .Cliente = SabLib.BLL.Utils.stringNull(r("CLIENTE"))}, query, CadenaConexion, New OracleParameter("PROYECTO", OracleDbType.NVarchar2, idProyecto, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene cabeceras de cost carrier
        ''' </summary>
        ''' <param name="responsable"></param>
        ''' <param name="tiposProyectoPTKSIS"></param>
        ''' <returns></returns>
        Public Shared Function loadList(Optional ByVal responsable As String = Nothing, Optional ByVal tiposProyectoPTKSIS As List(Of String) = Nothing) As List(Of ELL.CabeceraCostCarrier)
            Dim query As String = "SELECT * FROM VCABECERA_COST_CARRIER{0}"
            Dim where As String = String.Empty
            Dim lParameters As New List(Of OracleParameter)

            If (Not String.IsNullOrEmpty(responsable)) Then
                where = " WHERE (LOWER(RESPONSABLE)=:RESPONSABLE OR LOWER(COOWNER)=:RESPONSABLE)"
                lParameters.Add(New OracleParameter("RESPONSABLE", OracleDbType.Varchar2, responsable.ToLower(), ParameterDirection.Input))
            End If

            If (Not tiposProyectoPTKSIS Is Nothing AndAlso tiposProyectoPTKSIS.Count > 0) Then
                If (Not String.IsNullOrEmpty(where)) Then
                    where &= " AND "
                Else
                    where &= " WHERE "
                End If

                For Each tipoProyecto In tiposProyectoPTKSIS
                    If (tiposProyectoPTKSIS.Count() = 1) Then
                        where &= "(TIPO_PROY_PTKSIS=:TP_" & tipoProyecto.Replace(" ", "_").Replace("-", "_") & ")"
                    ElseIf (tipoProyecto = tiposProyectoPTKSIS.First) Then
                        where &= "(TIPO_PROY_PTKSIS=:TP_" & tipoProyecto.Replace(" ", "_").Replace("-", "_")
                    ElseIf (tipoProyecto = tiposProyectoPTKSIS.Last) Then
                        where &= " OR TIPO_PROY_PTKSIS=:TP_" & tipoProyecto.Replace(" ", "_").Replace("-", "_") & ")"
                    Else
                        where &= " OR TIPO_PROY_PTKSIS=:TP_" & tipoProyecto.Replace(" ", "_").Replace("-", "_") & ")"
                    End If

                    lParameters.Add(New OracleParameter("TP_" & tipoProyecto.Replace(" ", "_").Replace("-", "_"), OracleDbType.Varchar2, tipoProyecto, ParameterDirection.Input))
                Next
            End If

            query = String.Format(query, where)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CabeceraCostCarrier)(Function(r As OracleDataReader) _
            New ELL.CabeceraCostCarrier With {.Id = CInt(r("ID")), .IdTipoProyecto = CStr(r("ID_TIPO_PROYECTO")), .Proyecto = CStr(r("PROYECTO")),
                                              .Abreviatura = SabLib.BLL.Utils.stringNull(r("ABREVIATURA")),
                                              .NombreProyecto = CStr(r("NOMBRE_PROYECTO")), .IdOferta = SabLib.BLL.Utils.integerNull(r("ID_OFERTA")),
                                              .CodigoProyecto = SabLib.BLL.Utils.stringNull(r("CODIGO_PROYECTO")), .Responsable = CStr(r("RESPONSABLE")),
                                              .TipoProyecto = CStr(r("DESCRIPCION")), .CoOwner = SabLib.BLL.Utils.stringNull(r("COOWNER")),
                                              .Producto = CStr(r("PRODUCTO")), .TipoProyPtksis = CStr(r("TIPO_PROY_PTKSIS")).ToUpper(), .SOP = SabLib.BLL.Utils.dateTimeNull(r("SOP")),
                                              .SeriesYears = SabLib.BLL.Utils.integerNull(r("ANYOS_SERIE")), .Cliente = SabLib.BLL.Utils.stringNull(r("CLIENTE"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' Comprueba si existe una entrada en cabecera
        ''' </summary>
        ''' <param name="idTipoProyecto"></param>
        ''' <param name="proyecto"></param>
        ''' <returns></returns>
        Public Shared Function exists(ByVal idTipoProyecto As Integer, ByVal proyecto As String) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM CABECERA_COST_CARRIER WHERE ID_TIPO_PROYECTO=:ID_TIPO_PROYECTO AND PROYECTO=:PROYECTO"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_TIPO_PROYECTO", OracleDbType.Int32, idTipoProyecto, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("PROYECTO", OracleDbType.NVarchar2, proyecto, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, lParameters.ToArray()) > 0
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codigoProyecto"></param>
        ''' <returns></returns>
        Public Shared Function exists(ByVal codigoProyecto As String) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM CABECERA_COST_CARRIER WHERE CODIGO_PROYECTO=:CODIGO_PROYECTO"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("CODIGO_PROYECTO", OracleDbType.NVarchar2, codigoProyecto, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, lParameters.ToArray()) > 0
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function containsOpenedStep(ByVal id As Integer) As Boolean
            ' Excluimos los pasos que se abren automaticamente para Batz Zamudio
            Dim query As String = "SELECT COUNT(*) 
                                   FROM CABECERA_COST_CARRIER CCC
                                   INNER JOIN PLANTAS P ON P.ID_CABECERA = CCC.ID
                                   INNER JOIN ESTADOS E ON E.ID_PLANTA = P.ID
                                   INNER JOIN COST_GROUP CG ON CG.ID_ESTADO = E.ID
                                   INNER JOIN STEP S ON S.ID_COST_GROUP = CG.ID
                                   INNER JOIN VALIDACION_LINEA VL ON VL.ID_STEP = S.ID
                                   INNER JOIN HISTORICO_ESTADO_LINEA HEL ON HEL.ID_VALIDACION_LINEA = VL.ID
                                   WHERE HEL.ID_ESTADO_VALIDACION = 40 AND P.ID_PLANTA <> 47 AND CCC.ID=:ID "

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, lParameters.ToArray()) > 0
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function containsApprovedStep(ByVal id As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) 
                                   FROM CABECERA_COST_CARRIER CCC
                                   INNER JOIN PLANTAS P ON P.ID_CABECERA = CCC.ID
                                   INNER JOIN ESTADOS E ON E.ID_PLANTA = P.ID
                                   INNER JOIN COST_GROUP CG ON CG.ID_ESTADO = E.ID
                                   INNER JOIN STEP S ON S.ID_COST_GROUP = CG.ID
                                   INNER JOIN VALIDACION_LINEA VL ON VL.ID_STEP = S.ID
                                   INNER JOIN HISTORICO_ESTADO_LINEA HEL ON HEL.ID_VALIDACION_LINEA = VL.ID
                                   WHERE CCC.ID=:ID AND (HEL.ID_ESTADO_VALIDACION = 20 OR HEL.ID_ESTADO_VALIDACION = 40 OR HEL.ID_ESTADO_VALIDACION = 50)"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, lParameters.ToArray()) > 0
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda una cabecera
        ''' </summary>
        ''' <param name="cabecera"></param>
        ''' <param name="idOferta"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal cabecera As ELL.CabeceraCostCarrier, ByVal idOferta As Integer)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty

            ' Primero vamos a ver que plantas nos devuelve la oferta
            Dim plantasOferta As List(Of ELL.PlantaOferta) = BLL.PlantasOfertaBLL.ObtenerPlantasOferta(cabecera.Proyecto, idOferta)
            If (plantasOferta.Count() = 0) Then
                Return
            End If

            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                ' 1º Guardarmos la cabecera
                query = "INSERT INTO CABECERA_COST_CARRIER (ID_TIPO_PROYECTO, PROYECTO, ABREVIATURA, ID_OFERTA) " _
                        & "VALUES (:ID_TIPO_PROYECTO, :PROYECTO, :ABREVIATURA, :ID_OFERTA) RETURNING ID INTO :RETURN_VALUE"

                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_TIPO_PROYECTO", OracleDbType.Int32, cabecera.IdTipoProyecto, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("PROYECTO", OracleDbType.NVarchar2, cabecera.Proyecto, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ABREVIATURA", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(cabecera.Abreviatura), DBNull.Value, cabecera.Abreviatura), ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_OFERTA", OracleDbType.Int32, If(idOferta = Integer.MinValue, DBNull.Value, idOferta), ParameterDirection.Input))

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParameters.Add(p)

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                cabecera.Id = lParameters.Last.Value

                ' 2º Ahora copiamos las plantas de la plantilla
                Dim plantilla As ELL.Plantilla = BLL.PlantillasBLL.ObtenerPorTipoProyecto(cabecera.IdTipoProyecto)
                Dim plantasPlantilla As List(Of ELL.PlantaPlantilla) = BLL.PlantasPlantillaBLL.CargarListado(plantilla.Id).Where(Function(f) plantasOferta.Exists(Function(x) x.IdPlant = f.IdPlanta)).ToList()

                If (plantasPlantilla.Count = 0) Then
                    cabecera.Id = Integer.MinValue
                    transact.Rollback()
                    Return
                End If

                'Dim plantasBLL As New SabLib.BLL.PlantasComponent()
                'Dim planta As SabLib.ELL.Planta
                'Dim idMoneda As Integer = 90 ' EUR
                For Each plantaPlantilla In plantasPlantilla
                    ' Antes de guardar la planta tenemos que saber la moneda con la que trabaja dicha planta
                    'If (plantaPlantilla.IdPlanta <> 0) Then
                    '    planta = plantasBLL.GetPlanta(plantaPlantilla.IdPlanta)
                    '    idMoneda = planta.IdMoneda
                    'End If

                    query = "INSERT INTO PLANTAS (ID_PLANTA, ID_CABECERA, ID_MONEDA, SOP, ANYOS_SERIE) " _
                            & "VALUES (:ID_PLANTA, :ID_CABECERA, :ID_MONEDA, :SOP, :ANYOS_SERIE) RETURNING ID INTO :RETURN_VALUE"

                    lParameters = New List(Of OracleParameter)
                    lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, plantaPlantilla.IdPlanta, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID_CABECERA", OracleDbType.Int32, cabecera.Id, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, plantasOferta.First(Function(f) f.IdPlant = plantaPlantilla.IdPlanta).IdCurrency, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("SOP", OracleDbType.Date, If(plantasOferta.First(Function(f) f.IdPlant = plantaPlantilla.IdPlanta).SOP = DateTime.MinValue, DBNull.Value, plantasOferta.First(Function(f) f.IdPlant = plantaPlantilla.IdPlanta).SOP), ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ANYOS_SERIE", OracleDbType.Int32, If(plantasOferta.First(Function(f) f.IdPlant = plantaPlantilla.IdPlanta).YearsLife = 0, DBNull.Value, plantasOferta.First(Function(f) f.IdPlant = plantaPlantilla.IdPlanta).YearsLife), ParameterDirection.Input))

                    p = New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                    p.DbType = DbType.Int32
                    lParameters.Add(p)

                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                    Dim idPlanta As Integer = lParameters.Last.Value

                    ' 3º Por cada planta copiamos sus estados
                    Dim estadosPlantilla As List(Of ELL.EstadoPlantilla) = BLL.EstadosPlantillaBLL.CargarListado(plantaPlantilla.Id)

                    For Each estadoPlantilla In estadosPlantilla
                        query = "INSERT INTO ESTADOS (ID_PLANTA, ID_ESTADO_PROYECTO) " _
                                & "VALUES (:ID_PLANTA, :ID_ESTADO_PROYECTO) RETURNING ID INTO :RETURN_VALUE"

                        lParameters = New List(Of OracleParameter)
                        lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_ESTADO_PROYECTO", OracleDbType.Int32, estadoPlantilla.IdEstadoProyecto, ParameterDirection.Input))

                        p = New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                        p.DbType = DbType.Int32
                        lParameters.Add(p)

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                        Dim idEstado As Integer = lParameters.Last.Value

                        ' 4º Por cada estado copiamos sus cost groups
                        Dim costGroupsPlantilla As List(Of ELL.CostGroupPlantilla) = BLL.CostsGroupPlantillaBLL.CargarListado(estadoPlantilla.Id)

                        For Each costGroupPlantilla In costGroupsPlantilla
                            query = "INSERT INTO COST_GROUP (DESCRIPCION, ID_ESTADO, ID_BONOS, ID_COST_GROUP_OT) " _
                                    & "VALUES (:DESCRIPCION, :ID_ESTADO, :ID_BONOS, :ID_COST_GROUP_OT) RETURNING ID INTO :RETURN_VALUE"

                            lParameters = New List(Of OracleParameter)
                            lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(costGroupPlantilla.Descripcion), DBNull.Value, costGroupPlantilla.Descripcion), ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID_BONOS", OracleDbType.Int32, If(costGroupPlantilla.IdBonos = Integer.MinValue, DBNull.Value, costGroupPlantilla.IdBonos), ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID_COST_GROUP_OT", OracleDbType.Int32, If(costGroupPlantilla.IdCostGroupOT = Integer.MinValue, DBNull.Value, costGroupPlantilla.IdCostGroupOT), ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID_ESTADO", OracleDbType.Int32, idEstado, ParameterDirection.Input))

                            p = New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                            p.DbType = DbType.Int32
                            lParameters.Add(p)

                            Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                            Dim idCostGroup As Integer = lParameters.Last.Value

                            ' 5º Por cada cost group copiamos sus steps
                            Dim stepsPlantilla As List(Of ELL.StepPlantilla) = BLL.StepsPlantillaBLL.CargarListado(costGroupPlantilla.Id)

                            For Each stepPlantilla In stepsPlantilla
                                query = "INSERT INTO STEP (DESCRIPCION, ID_COST_GROUP, OBC_ORIGEN_DATOS, TC_FORMULA, TC_FORMULA_CUSTOMER, GASTOS_AÑO_ORIGEN_DATOS, INGRESOS_AÑO_ORIGEN_DATOS, ORDEN, ORIGEN, ORIGEN_DATO_REAL, ES_INFO_GENERAL) " _
                                        & "VALUES (:DESCRIPCION, :ID_COST_GROUP, :OBC_ORIGEN_DATOS, :TC_FORMULA, :TC_FORMULA_CUSTOMER, :GASTOS_AÑO_ORIGEN_DATOS, :INGRESOS_AÑO_ORIGEN_DATOS, :ORDEN, :ORIGEN, :ORIGEN_DATO_REAL, :ES_INFO_GENERAL)"

                                lParameters = New List(Of OracleParameter)
                                lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, stepPlantilla.Descripcion, ParameterDirection.Input))
                                lParameters.Add(New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, idCostGroup, ParameterDirection.Input))
                                lParameters.Add(New OracleParameter("OBC_ORIGEN_DATOS", OracleDbType.Int32, stepPlantilla.OBCOrigenDatos, ParameterDirection.Input))
                                lParameters.Add(New OracleParameter("TC_FORMULA", OracleDbType.NVarchar2, stepPlantilla.TCFormula, ParameterDirection.Input))
                                lParameters.Add(New OracleParameter("TC_FORMULA_CUSTOMER", OracleDbType.NVarchar2, stepPlantilla.TCFormulaCustomer, ParameterDirection.Input))
                                lParameters.Add(New OracleParameter("GASTOS_AÑO_ORIGEN_DATOS", OracleDbType.Int32, stepPlantilla.GastosAñoOrigenDatos, ParameterDirection.Input))
                                lParameters.Add(New OracleParameter("INGRESOS_AÑO_ORIGEN_DATOS", OracleDbType.Int32, stepPlantilla.IngresosAñoOrigenDatos, ParameterDirection.Input))
                                lParameters.Add(New OracleParameter("ORDEN", OracleDbType.Int32, stepPlantilla.Orden, ParameterDirection.Input))
                                lParameters.Add(New OracleParameter("ORIGEN", OracleDbType.Int32, ELL.Step.OrigenStep.Plantilla, ParameterDirection.Input))
                                lParameters.Add(New OracleParameter("ORIGEN_DATO_REAL", OracleDbType.Int32, stepPlantilla.OrigenDatoReal, ParameterDirection.Input))
                                lParameters.Add(New OracleParameter("ES_INFO_GENERAL", OracleDbType.Int32, stepPlantilla.EsInfoGeneral, ParameterDirection.Input))

                                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                            Next

                            ' 6º Si el cost group es de oferta técnica y además tenemos id offer vamos a por los steps a la oferta técnica
                            If (costGroupPlantilla.IdCostGroupOT <> Integer.MinValue AndAlso idOferta <> Integer.MinValue) Then
                                Dim jss As New JavaScriptSerializer()
                                Dim listaStepsOferta As List(Of ELL.StepOferta) = Nothing
                                Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                                    Try
                                        listaStepsOferta = jss.Deserialize(Of List(Of ELL.StepOferta))(cliente.GetStepsFromTechnicalOffer(idOferta, plantaPlantilla.IdPlanta, costGroupPlantilla.IdCostGroupOT))
                                    Catch
                                    End Try
                                End Using

                                If (listaStepsOferta IsNot Nothing) Then
                                    ' En una reunión por Teams se decide que los NO visibles no se tengan en cuenta 11/02/2021
                                    For Each stepOferta In listaStepsOferta.Where(Function(f) f.Visible).ToList()
                                        query = "INSERT INTO STEP (DESCRIPCION, ID_COST_GROUP, ORDEN, ID_OFERTA, ORIGEN, VISIBLE, ORIGEN_DATO_REAL) " _
                                            & "VALUES (:DESCRIPCION, :ID_COST_GROUP, (SELECT NVL(MAX(ORDEN), 0) + 1 FROM STEP WHERE ID_COST_GROUP=:ID_COST_GROUP), :ID_OFERTA, :ORIGEN, :VISIBLE, :ORIGEN_DATO_REAL) RETURNING ID INTO :RETURN_VALUE"

                                        lParameters = New List(Of OracleParameter)
                                        lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, stepOferta.StepName, ParameterDirection.Input))
                                        lParameters.Add(New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, idCostGroup, ParameterDirection.Input))
                                        lParameters.Add(New OracleParameter("ID_OFERTA", OracleDbType.NVarchar2, stepOferta.Id, ParameterDirection.Input))
                                        lParameters.Add(New OracleParameter("ORIGEN", OracleDbType.Int32, ELL.Step.OrigenStep.Oferta, ParameterDirection.Input))
                                        lParameters.Add(New OracleParameter("VISIBLE", OracleDbType.Int32, stepOferta.Visible, ParameterDirection.Input))
                                        lParameters.Add(New OracleParameter("ORIGEN_DATO_REAL", OracleDbType.Int32, ELL.StepPlantilla.DatoRealOrigen.EXT, ParameterDirection.Input))

                                        p = New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                                        p.DbType = DbType.Int32
                                        lParameters.Add(p)

                                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                                        Dim idStep As Integer = lParameters.Last.Value

                                        '7º Hay que meter los valores del step. 
                                        query = "INSERT INTO VALORES_STEP (ID_STEP, ID_COLUMNA, VALOR, AÑO, TRIMESTRE) " _
                                            & "VALUES (:ID_STEP, :ID_COLUMNA, :VALOR, :AÑO, :TRIMESTRE)"

                                        Dim valor As Integer = Integer.MinValue
                                        Dim idColumna As Integer = Integer.MinValue
                                        For cont = 0 To 3
                                            If (cont = 0) Then
                                                valor = stepOferta.BudgetApproved
                                                idColumna = ELL.Columna.Tipo.Budget_approved_cost
                                            ElseIf (cont = 1) Then
                                                valor = stepOferta.PaidByCustomer
                                                idColumna = ELL.Columna.Tipo.Paid_by_customer
                                            ElseIf (cont = 2) Then
                                                valor = stepOferta.Offerbudget
                                                idColumna = ELL.Columna.Tipo.Offer_budget_cost
                                            ElseIf (cont = 3) Then
                                                valor = stepOferta.PaidByCustomerCR
                                                idColumna = ELL.Columna.Tipo.Paid_by_customer_CR
                                            End If

                                            lParameters = New List(Of OracleParameter)
                                            lParameters.Add(New OracleParameter("ID_STEP", OracleDbType.Int32, idStep, ParameterDirection.Input))
                                            lParameters.Add(New OracleParameter("ID_COLUMNA", OracleDbType.Int32, idColumna, ParameterDirection.Input))
                                            lParameters.Add(New OracleParameter("VALOR", OracleDbType.Int32, valor, ParameterDirection.Input))
                                            lParameters.Add(New OracleParameter("AÑO", OracleDbType.Int32, 0, ParameterDirection.Input))
                                            lParameters.Add(New OracleParameter("TRIMESTRE", OracleDbType.Int32, 0, ParameterDirection.Input))

                                            Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                                        Next
                                    Next
                                End If
                            End If
                        Next
                    Next
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
        ''' Cambiar la abreviatura de un proyecto
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="abreviatura"></param>
        ''' <param name="SOP"></param>
        ''' <param name="anyosSerie"></param>
        Public Shared Sub ChangeAbr(ByVal id As Integer, ByVal abreviatura As String, ByVal SOP As DateTime, ByVal anyosSerie As Integer)
            Dim query As String = "UPDATE CABECERA_COST_CARRIER SET ABREVIATURA=:ABREVIATURA, SOP=:SOP, ANYOS_SERIE=:ANYOS_SERIE WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ABREVIATURA", OracleDbType.NVarchar2, abreviatura, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("SOP", OracleDbType.Date, If(SOP = DateTime.MinValue, DBNull.Value, SOP), ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ANYOS_SERIE", OracleDbType.Int32, If(anyosSerie = Integer.MinValue, DBNull.Value, anyosSerie), ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

        ''' <summary>
        ''' Cambiar la abreviatura de un proyecto
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="abreviatura"></param>
        Public Shared Sub ChangeAbr(ByVal id As Integer, ByVal abreviatura As String)
            Dim query As String = "UPDATE CABECERA_COST_CARRIER SET ABREVIATURA=:ABREVIATURA WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ABREVIATURA", OracleDbType.NVarchar2, abreviatura, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>C:\Trunk_4\SBatz\CostCarriers\CostCarriersLib\DAL\CostCarrier\Validacion\
        ''' <param name="idOffer"></param>
        Public Shared Sub ChangeOffer(ByVal id As Integer, ByVal idOffer As Integer)
            Dim query As String = "UPDATE CABECERA_COST_CARRIER SET ID_OFERTA=:ID_OFERTA WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_OFERTA", OracleDbType.Int32, If(idOffer = 0, DBNull.Value, idOffer), ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

        ''' <summary>
        ''' Cambiar el código de proyecto
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub ChangeCodigoProyecto(ByVal id As Integer, ByVal codigo As String)
            Dim query As String = "UPDATE CABECERA_COST_CARRIER SET CODIGO_PROYECTO=:CODIGO_PROYECTO WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("CODIGO_PROYECTO", OracleDbType.NVarchar2, codigo, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="cabecera"></param>
        ''' <param name="plantaPlantilla"></param>
        Public Shared Sub AddPlant(ByVal cabecera As ELL.CabeceraCostCarrier, ByVal plantaPlantilla As ELL.PlantaPlantilla)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty

            ' Primero vamos a ver que plantas nos devuelve la oferta. Puede ser que no tenga oferta
            Dim plantasOferta As List(Of ELL.PlantaOferta) = BLL.PlantasOfertaBLL.ObtenerPlantasOferta(cabecera.Proyecto, cabecera.IdOferta)

            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                ' 1º Copiamos las plantas de la plantilla
                query = "INSERT INTO PLANTAS (ID_PLANTA, ID_CABECERA, ID_MONEDA, SOP, ANYOS_SERIE) " _
                        & "VALUES (:ID_PLANTA, :ID_CABECERA, :ID_MONEDA, :SOP, :ANYOS_SERIE) RETURNING ID INTO :RETURN_VALUE"

                Dim idMoneda As Integer = Integer.MinValue
                Dim sop As Date = DateTime.MinValue
                Dim anyos_serie As Integer = 0

                ' Buscamos la moneda y el resto de datos
                If (plantasOferta IsNot Nothing AndAlso plantasOferta.Exists(Function(f) f.IdPlant = plantaPlantilla.IdPlanta)) Then
                    idMoneda = plantasOferta.First(Function(f) f.IdPlant = plantaPlantilla.IdPlanta).IdCurrency
                    sop = plantasOferta.First(Function(f) f.IdPlant = plantaPlantilla.IdPlanta).SOP
                    anyos_serie = plantasOferta.First(Function(f) f.IdPlant = plantaPlantilla.IdPlanta).YearsLife
                Else
                    ' Sacamos el valor de moneda de la planta de oferta técnica
                    idMoneda = BLL.PlantasOfertaBLL.ObtenerMonedaPlanta(plantaPlantilla.IdPlanta)
                End If

                Dim lparameters As New List(Of OracleParameter)
                lparameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, plantaPlantilla.IdPlanta, ParameterDirection.Input))
                lparameters.Add(New OracleParameter("ID_CABECERA", OracleDbType.Int32, cabecera.Id, ParameterDirection.Input))
                lparameters.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, idMoneda, ParameterDirection.Input))
                lparameters.Add(New OracleParameter("SOP", OracleDbType.Date, If(sop = DateTime.MinValue, DBNull.Value, plantasOferta.First(Function(f) f.IdPlant = plantaPlantilla.IdPlanta).SOP), ParameterDirection.Input))
                lparameters.Add(New OracleParameter("ANYOS_SERIE", OracleDbType.Int32, If(anyos_serie = 0, DBNull.Value, plantasOferta.First(Function(f) f.IdPlant = plantaPlantilla.IdPlanta).YearsLife), ParameterDirection.Input))

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lparameters.Add(p)

                Memcached.OracleDirectAccess.NoQuery(query, con, lparameters.ToArray)

                Dim idPlanta As Integer = lparameters.Last.Value

                ' 3º Por cada planta copiamos sus estados
                Dim estadosPlantilla As List(Of ELL.EstadoPlantilla) = BLL.EstadosPlantillaBLL.CargarListado(plantaPlantilla.Id)

                For Each estadoPlantilla In estadosPlantilla
                    query = "INSERT INTO ESTADOS (ID_PLANTA, ID_ESTADO_PROYECTO) " _
                                & "VALUES (:ID_PLANTA, :ID_ESTADO_PROYECTO) RETURNING ID INTO :RETURN_VALUE"

                    lparameters = New List(Of OracleParameter)
                    lparameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                    lparameters.Add(New OracleParameter("ID_ESTADO_PROYECTO", OracleDbType.Int32, estadoPlantilla.IdEstadoProyecto, ParameterDirection.Input))

                    p = New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                    p.DbType = DbType.Int32
                    lparameters.Add(p)

                    Memcached.OracleDirectAccess.NoQuery(query, con, lparameters.ToArray)

                    Dim idEstado As Integer = lparameters.Last.Value

                    ' 4º Por cada estado copiamos sus cost groups
                    Dim costGroupsPlantilla As List(Of ELL.CostGroupPlantilla) = BLL.CostsGroupPlantillaBLL.CargarListado(estadoPlantilla.Id)

                    For Each costGroupPlantilla In costGroupsPlantilla
                        query = "INSERT INTO COST_GROUP (DESCRIPCION, ID_ESTADO, ID_BONOS, ID_COST_GROUP_OT) " _
                                    & "VALUES (:DESCRIPCION, :ID_ESTADO, :ID_BONOS, :ID_COST_GROUP_OT) RETURNING ID INTO :RETURN_VALUE"

                        lparameters = New List(Of OracleParameter)
                        lparameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(costGroupPlantilla.Descripcion), DBNull.Value, costGroupPlantilla.Descripcion), ParameterDirection.Input))
                        lparameters.Add(New OracleParameter("ID_BONOS", OracleDbType.Int32, If(costGroupPlantilla.IdBonos = Integer.MinValue, DBNull.Value, costGroupPlantilla.IdBonos), ParameterDirection.Input))
                        lparameters.Add(New OracleParameter("ID_COST_GROUP_OT", OracleDbType.Int32, If(costGroupPlantilla.IdCostGroupOT = Integer.MinValue, DBNull.Value, costGroupPlantilla.IdCostGroupOT), ParameterDirection.Input))
                        lparameters.Add(New OracleParameter("ID_ESTADO", OracleDbType.Int32, idEstado, ParameterDirection.Input))

                        p = New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                        p.DbType = DbType.Int32
                        lparameters.Add(p)

                        Memcached.OracleDirectAccess.NoQuery(query, con, lparameters.ToArray)

                        Dim idCostGroup As Integer = lparameters.Last.Value

                        ' 5º Por cada cost group copiamos sus steps
                        Dim stepsPlantilla As List(Of ELL.StepPlantilla) = BLL.StepsPlantillaBLL.CargarListado(costGroupPlantilla.Id)

                        For Each stepPlantilla In stepsPlantilla
                            query = "INSERT INTO STEP (DESCRIPCION, ID_COST_GROUP, OBC_ORIGEN_DATOS, TC_FORMULA, TC_FORMULA_CUSTOMER, GASTOS_AÑO_ORIGEN_DATOS, INGRESOS_AÑO_ORIGEN_DATOS, ORDEN, ORIGEN, ORIGEN_DATO_REAL) " _
                                        & "VALUES (:DESCRIPCION, :ID_COST_GROUP, :OBC_ORIGEN_DATOS, :TC_FORMULA, :TC_FORMULA_CUSTOMER, :GASTOS_AÑO_ORIGEN_DATOS, :INGRESOS_AÑO_ORIGEN_DATOS, :ORDEN, :ORIGEN, :ORIGEN_DATO_REAL)"

                            lparameters = New List(Of OracleParameter)
                            lparameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, stepPlantilla.Descripcion, ParameterDirection.Input))
                            lparameters.Add(New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, idCostGroup, ParameterDirection.Input))
                            lparameters.Add(New OracleParameter("OBC_ORIGEN_DATOS", OracleDbType.Int32, stepPlantilla.OBCOrigenDatos, ParameterDirection.Input))
                            lparameters.Add(New OracleParameter("TC_FORMULA", OracleDbType.NVarchar2, stepPlantilla.TCFormula, ParameterDirection.Input))
                            lparameters.Add(New OracleParameter("TC_FORMULA_CUSTOMER", OracleDbType.NVarchar2, stepPlantilla.TCFormulaCustomer, ParameterDirection.Input))
                            lparameters.Add(New OracleParameter("GASTOS_AÑO_ORIGEN_DATOS", OracleDbType.Int32, stepPlantilla.GastosAñoOrigenDatos, ParameterDirection.Input))
                            lparameters.Add(New OracleParameter("INGRESOS_AÑO_ORIGEN_DATOS", OracleDbType.Int32, stepPlantilla.IngresosAñoOrigenDatos, ParameterDirection.Input))
                            lparameters.Add(New OracleParameter("ORDEN", OracleDbType.Int32, stepPlantilla.Orden, ParameterDirection.Input))
                            lparameters.Add(New OracleParameter("ORIGEN", OracleDbType.Int32, ELL.Step.OrigenStep.Plantilla, ParameterDirection.Input))
                            lparameters.Add(New OracleParameter("ORIGEN_DATO_REAL", OracleDbType.Int32, stepPlantilla.OrigenDatoReal, ParameterDirection.Input))

                            Memcached.OracleDirectAccess.NoQuery(query, con, lparameters.ToArray)
                        Next

                        ' 6º Si el cost group es de oferta técnica y además tenemos id offer vamos a por los steps a la oferta técnica
                        If (costGroupPlantilla.IdCostGroupOT <> Integer.MinValue AndAlso cabecera.IdOferta <> Integer.MinValue) Then
                            Dim jss As New JavaScriptSerializer()
                            Dim listaStepsOferta As List(Of ELL.StepOferta) = Nothing
                            Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                                Try
                                    listaStepsOferta = jss.Deserialize(Of List(Of ELL.StepOferta))(cliente.GetStepsFromTechnicalOffer(cabecera.IdOferta, plantaPlantilla.IdPlanta, costGroupPlantilla.IdCostGroupOT))
                                Catch
                                End Try
                            End Using

                            If (listaStepsOferta IsNot Nothing) Then
                                ' En una reunión por Teams se decide que los NO visibles no se tengan en cuenta 11/02/2021
                                For Each stepOferta In listaStepsOferta.Where(Function(f) f.Visible).ToList()
                                    query = "INSERT INTO STEP (DESCRIPCION, ID_COST_GROUP, ORDEN, ID_OFERTA, ORIGEN, VISIBLE, ORIGEN_DATO_REAL) " _
                                            & "VALUES (:DESCRIPCION, :ID_COST_GROUP, (SELECT NVL(MAX(ORDEN), 0) + 1 FROM STEP WHERE ID_COST_GROUP=:ID_COST_GROUP), :ID_OFERTA, :ORIGEN, :VISIBLE, :ORIGEN_DATO_REAL) RETURNING ID INTO :RETURN_VALUE"

                                    lparameters = New List(Of OracleParameter)
                                    lparameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, stepOferta.StepName, ParameterDirection.Input))
                                    lparameters.Add(New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, idCostGroup, ParameterDirection.Input))
                                    lparameters.Add(New OracleParameter("ID_OFERTA", OracleDbType.NVarchar2, stepOferta.Id, ParameterDirection.Input))
                                    lparameters.Add(New OracleParameter("ORIGEN", OracleDbType.Int32, ELL.Step.OrigenStep.Oferta, ParameterDirection.Input))
                                    lparameters.Add(New OracleParameter("VISIBLE", OracleDbType.Int32, stepOferta.Visible, ParameterDirection.Input))
                                    lparameters.Add(New OracleParameter("ORIGEN_DATO_REAL", OracleDbType.Int32, ELL.StepPlantilla.DatoRealOrigen.EXT, ParameterDirection.Input))

                                    p = New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                                    p.DbType = DbType.Int32
                                    lparameters.Add(p)

                                    Memcached.OracleDirectAccess.NoQuery(query, con, lparameters.ToArray)

                                    Dim idStep As Integer = lparameters.Last.Value

                                    '7º Hay que meter los valores del step. 
                                    query = "INSERT INTO VALORES_STEP (ID_STEP, ID_COLUMNA, VALOR, AÑO, TRIMESTRE) " _
                                            & "VALUES (:ID_STEP, :ID_COLUMNA, :VALOR, :AÑO, :TRIMESTRE)"

                                    Dim valor As Integer = Integer.MinValue
                                    Dim idColumna As Integer = Integer.MinValue
                                    For cont = 0 To 3
                                        If (cont = 0) Then
                                            valor = stepOferta.BudgetApproved
                                            idColumna = ELL.Columna.Tipo.Budget_approved_cost
                                        ElseIf (cont = 1) Then
                                            valor = stepOferta.PaidByCustomer
                                            idColumna = ELL.Columna.Tipo.Paid_by_customer
                                        ElseIf (cont = 2) Then
                                            valor = stepOferta.Offerbudget
                                            idColumna = ELL.Columna.Tipo.Offer_budget_cost
                                        ElseIf (cont = 3) Then
                                            valor = stepOferta.PaidByCustomerCR
                                            idColumna = ELL.Columna.Tipo.Paid_by_customer_CR
                                        End If

                                        lparameters = New List(Of OracleParameter)
                                        lparameters.Add(New OracleParameter("ID_STEP", OracleDbType.Int32, idStep, ParameterDirection.Input))
                                        lparameters.Add(New OracleParameter("ID_COLUMNA", OracleDbType.Int32, idColumna, ParameterDirection.Input))
                                        lparameters.Add(New OracleParameter("VALOR", OracleDbType.Int32, valor, ParameterDirection.Input))
                                        lparameters.Add(New OracleParameter("AÑO", OracleDbType.Int32, 0, ParameterDirection.Input))
                                        lparameters.Add(New OracleParameter("TRIMESTRE", OracleDbType.Int32, 0, ParameterDirection.Input))

                                        Memcached.OracleDirectAccess.NoQuery(query, con, lparameters.ToArray)
                                    Next
                                Next
                            End If
                        End If
                    Next
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
        ''' Elimina un cost carrier
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Delete(ByVal id As Integer)
            Dim query As String = "DELETE FROM CABECERA_COST_CARRIER WHERE ID=:ID"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

    End Class

End Namespace