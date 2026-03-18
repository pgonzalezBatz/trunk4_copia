Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class StepsDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un step
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal id As Integer) As ELL.Step
            Dim query As String = "SELECT * FROM VSTEPS WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Step)(Function(r As OracleDataReader) _
            New ELL.Step With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                               .OBCOrigenDatos = SabLib.BLL.Utils.integerNull(r("OBC_ORIGEN_DATOS")), .TCFormula = SabLib.BLL.Utils.stringNull(r("TC_FORMULA")), .GastosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("GASTOS_AÑO_ORIGEN_DATOS")),
                               .IngresosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("INGRESOS_AÑO_ORIGEN_DATOS")), .FechaValidacion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_VALIDACION")),
                               .IdValidador = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR")), .DescripcionBRAIN = SabLib.BLL.Utils.integerNull(r("DESCRIPCION_BRAIN")),
                               .Code = SabLib.BLL.Utils.integerNull(r("CODE")),
                               .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .Estado = CStr(r("ESTADO")),
                               .Proyecto = CStr(r("PROYECTO")), .IdPlanta = CInt(r("ID_PLANTA")), .Planta = CStr(r("PLANTA")), .CambiarPlanta = CBool(r("CAMBIAR_PLANTA")),
                               .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")), .IdOferta = SabLib.BLL.Utils.stringNull(r("ID_OFERTA")),
                               .Origen = CInt(r("ORIGEN")), .Porcentaje = SabLib.BLL.Utils.integerNull(r("PORCENTAJE")), .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")),
                               .CambioPorcentaje = SabLib.BLL.Utils.booleanNull(r("CAMBIO_PORCENTAJE")), .IdCabecera = CInt(r("ID_CABECERA")),
                               .IdEstado = CInt(r("ID_ESTADO")), .Visible = CBool(r("VISIBLE")), .OrigenDatoReal = CInt(r("ORIGEN_DATO_REAL")),
                               .CostCarrier = SabLib.BLL.Utils.stringNull(r("COST_CARRIER")), .IdEstadoValidacion = SabLib.BLL.Utils.integerNull(r("ID_ESTADO_VALIDACION")),
                               .EstadoValidacion = SabLib.BLL.Utils.stringNull(r("ESTADO_VALIDACION")), .IdUsuarioValidador = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_VALIDADOR")),
                               .IdValidacionLinea = SabLib.BLL.Utils.integerNull(r("ID_VALIDACION_LINEA")), .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")),
                               .Abreviatura = SabLib.BLL.Utils.stringNull(r("ABREVIATURA")), .IdMoneda = CInt(r("ID_MONEDA")), .Moneda = CStr(r("MONEDA")), .TCFormulaCustomer = SabLib.BLL.Utils.stringNull(r("TC_FORMULA_CUSTOMER")),
                               .Comentarios = SabLib.BLL.Utils.stringNull(r("COMENTARIOS")), .CorrelativoCC = SabLib.BLL.Utils.integerNull(r("CORRELATIVO_CC")), .IdEstadoProyecto = CInt(r("ID_ESTADO_PROYECTO")),
                               .EsInfoGeneral = CBool(r("ES_INFO_GENERAL"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene steps
        ''' </summary>
        ''' <param name="idPlantaSab"></param>
        ''' <returns></returns>
        Public Shared Function loadListByPlantaConEstadoValidacion(Optional ByVal idPlantaSab As Integer? = Nothing) As List(Of ELL.Step)
            Dim query As String = "SELECT * FROM VSTEPS WHERE ID_ESTADO_VALIDACION IS NOT NULL{0}"
            Dim where As String = String.Empty

            Dim lParameters As New List(Of OracleParameter)
            If (idPlantaSab IsNot Nothing) Then
                lParameters.Add(New OracleParameter("ID_PLANTA_SAB", OracleDbType.Int32, idPlantaSab, ParameterDirection.Input))

                where = " AND ID_PLANTA_SAB=:ID_PLANTA_SAB"
            End If

            query = String.Format(query, where)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Step)(Function(r As OracleDataReader) _
            New ELL.Step With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                               .OBCOrigenDatos = SabLib.BLL.Utils.integerNull(r("OBC_ORIGEN_DATOS")), .TCFormula = SabLib.BLL.Utils.stringNull(r("TC_FORMULA")), .GastosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("GASTOS_AÑO_ORIGEN_DATOS")),
                               .IngresosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("INGRESOS_AÑO_ORIGEN_DATOS")), .FechaValidacion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_VALIDACION")),
                               .IdValidador = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR")), .DescripcionBRAIN = SabLib.BLL.Utils.integerNull(r("DESCRIPCION_BRAIN")),
                               .Code = SabLib.BLL.Utils.integerNull(r("CODE")),
                               .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .Estado = CStr(r("ESTADO")),
                               .Proyecto = CStr(r("PROYECTO")), .IdPlanta = CInt(r("ID_PLANTA")), .Planta = CStr(r("PLANTA")), .CambiarPlanta = CBool(r("CAMBIAR_PLANTA")),
                               .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")), .IdOferta = SabLib.BLL.Utils.stringNull(r("ID_OFERTA")),
                               .Origen = CInt(r("ORIGEN")), .Porcentaje = SabLib.BLL.Utils.integerNull(r("PORCENTAJE")), .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")),
                               .CambioPorcentaje = SabLib.BLL.Utils.booleanNull(r("CAMBIO_PORCENTAJE")), .IdCabecera = CInt(r("ID_CABECERA")),
                               .IdEstado = CInt(r("ID_ESTADO")), .Visible = CBool(r("VISIBLE")), .OrigenDatoReal = CInt(r("ORIGEN_DATO_REAL")),
                               .CostCarrier = SabLib.BLL.Utils.stringNull(r("COST_CARRIER")), .IdEstadoValidacion = SabLib.BLL.Utils.integerNull(r("ID_ESTADO_VALIDACION")),
                               .EstadoValidacion = SabLib.BLL.Utils.stringNull(r("ESTADO_VALIDACION")), .IdUsuarioValidador = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_VALIDADOR")),
                               .IdValidacionLinea = SabLib.BLL.Utils.integerNull(r("ID_VALIDACION_LINEA")), .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")),
                               .Abreviatura = SabLib.BLL.Utils.stringNull(r("ABREVIATURA")), .IdMoneda = CInt(r("ID_MONEDA")), .Moneda = CStr(r("MONEDA")), .TCFormulaCustomer = SabLib.BLL.Utils.stringNull(r("TC_FORMULA_CUSTOMER")),
                               .Comentarios = SabLib.BLL.Utils.stringNull(r("COMENTARIOS")), .CorrelativoCC = SabLib.BLL.Utils.integerNull(r("CORRELATIVO_CC")), .IdEstadoProyecto = CInt(r("ID_ESTADO_PROYECTO")),
                               .EsInfoGeneral = CBool(r("ES_INFO_GENERAL"))}, query, CadenaConexion, lParameters.ToArray)
        End Function

        ''' <summary>
        ''' Obtiene los steps validados para un costgroup y un usuario
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <param name="idUser"></param>
        ''' <returns></returns>
        Public Shared Function loadListValidados(ByVal idCostGroup As Integer, ByVal idUser As Integer) As List(Of ELL.Step)
            Dim query As String = "SELECT VS.*
                                   FROM VSTEPS VS
                                   INNER JOIN HISTORICO_ESTADO_LINEA HEL ON HEL.ID_VALIDACION_LINEA = VS.ID_VALIDACION_LINEA AND HEL.ID_ESTADO_VALIDACION = 10 AND HEL.ID_ACCION_VALIDACION = 20
                                   WHERE VS.ID_ESTADO_VALIDACION= 10 AND VS.ID_COST_GROUP=:ID_COST_GROUP AND HEL.ID_USER=:ID_USER"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, idCostGroup, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Step)(Function(r As OracleDataReader) _
            New ELL.Step With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                               .OBCOrigenDatos = SabLib.BLL.Utils.integerNull(r("OBC_ORIGEN_DATOS")), .TCFormula = SabLib.BLL.Utils.stringNull(r("TC_FORMULA")), .GastosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("GASTOS_AÑO_ORIGEN_DATOS")),
                               .IngresosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("INGRESOS_AÑO_ORIGEN_DATOS")), .FechaValidacion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_VALIDACION")),
                               .IdValidador = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR")), .DescripcionBRAIN = SabLib.BLL.Utils.integerNull(r("DESCRIPCION_BRAIN")),
                               .Code = SabLib.BLL.Utils.integerNull(r("CODE")),
                               .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .Estado = CStr(r("ESTADO")),
                               .Proyecto = CStr(r("PROYECTO")), .IdPlanta = CInt(r("ID_PLANTA")), .Planta = CStr(r("PLANTA")), .CambiarPlanta = CBool(r("CAMBIAR_PLANTA")),
                               .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")), .IdOferta = SabLib.BLL.Utils.stringNull(r("ID_OFERTA")),
                               .Origen = CInt(r("ORIGEN")), .Porcentaje = SabLib.BLL.Utils.integerNull(r("PORCENTAJE")), .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")),
                               .CambioPorcentaje = SabLib.BLL.Utils.booleanNull(r("CAMBIO_PORCENTAJE")), .IdCabecera = CInt(r("ID_CABECERA")),
                               .IdEstado = CInt(r("ID_ESTADO")), .Visible = CBool(r("VISIBLE")), .OrigenDatoReal = CInt(r("ORIGEN_DATO_REAL")),
                               .CostCarrier = SabLib.BLL.Utils.stringNull(r("COST_CARRIER")), .IdEstadoValidacion = SabLib.BLL.Utils.integerNull(r("ID_ESTADO_VALIDACION")),
                               .EstadoValidacion = SabLib.BLL.Utils.stringNull(r("ESTADO_VALIDACION")), .IdUsuarioValidador = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_VALIDADOR")),
                               .IdValidacionLinea = SabLib.BLL.Utils.integerNull(r("ID_VALIDACION_LINEA")), .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")),
                               .Abreviatura = SabLib.BLL.Utils.stringNull(r("ABREVIATURA")), .IdMoneda = CInt(r("ID_MONEDA")), .Moneda = CStr(r("MONEDA")), .TCFormulaCustomer = SabLib.BLL.Utils.stringNull(r("TC_FORMULA_CUSTOMER")),
                               .Comentarios = SabLib.BLL.Utils.stringNull(r("COMENTARIOS")), .CorrelativoCC = SabLib.BLL.Utils.integerNull(r("CORRELATIVO_CC")), .IdEstadoProyecto = CInt(r("ID_ESTADO_PROYECTO")),
                               .EsInfoGeneral = CBool(r("ES_INFO_GENERAL"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' Obtiene los steps validados para un costgroup
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <returns></returns>
        Public Shared Function loadListValidadosFinal(ByVal idCostGroup As Integer) As List(Of ELL.Step)
            Dim query As String = "SELECT VS.*
                                   FROM VSTEPS VS
                                   INNER JOIN HISTORICO_ESTADO_LINEA HEL ON HEL.ID_VALIDACION_LINEA = VS.ID_VALIDACION_LINEA AND HEL.ID_ESTADO_VALIDACION IN (20, 40, 50)
                                   WHERE VS.ID_COST_GROUP=:ID_COST_GROUP"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, idCostGroup, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Step)(Function(r As OracleDataReader) _
            New ELL.Step With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                               .OBCOrigenDatos = SabLib.BLL.Utils.integerNull(r("OBC_ORIGEN_DATOS")), .TCFormula = SabLib.BLL.Utils.stringNull(r("TC_FORMULA")), .GastosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("GASTOS_AÑO_ORIGEN_DATOS")),
                               .IngresosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("INGRESOS_AÑO_ORIGEN_DATOS")), .FechaValidacion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_VALIDACION")),
                               .IdValidador = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR")), .DescripcionBRAIN = SabLib.BLL.Utils.integerNull(r("DESCRIPCION_BRAIN")),
                               .Code = SabLib.BLL.Utils.integerNull(r("CODE")),
                               .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .Estado = CStr(r("ESTADO")),
                               .Proyecto = CStr(r("PROYECTO")), .IdPlanta = CInt(r("ID_PLANTA")), .Planta = CStr(r("PLANTA")), .CambiarPlanta = CBool(r("CAMBIAR_PLANTA")),
                               .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")), .IdOferta = SabLib.BLL.Utils.stringNull(r("ID_OFERTA")),
                               .Origen = CInt(r("ORIGEN")), .Porcentaje = SabLib.BLL.Utils.integerNull(r("PORCENTAJE")), .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")),
                               .CambioPorcentaje = SabLib.BLL.Utils.booleanNull(r("CAMBIO_PORCENTAJE")), .IdCabecera = CInt(r("ID_CABECERA")),
                               .IdEstado = CInt(r("ID_ESTADO")), .Visible = CBool(r("VISIBLE")), .OrigenDatoReal = CInt(r("ORIGEN_DATO_REAL")),
                               .CostCarrier = SabLib.BLL.Utils.stringNull(r("COST_CARRIER")), .IdEstadoValidacion = SabLib.BLL.Utils.integerNull(r("ID_ESTADO_VALIDACION")),
                               .EstadoValidacion = SabLib.BLL.Utils.stringNull(r("ESTADO_VALIDACION")), .IdUsuarioValidador = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_VALIDADOR")),
                               .IdValidacionLinea = SabLib.BLL.Utils.integerNull(r("ID_VALIDACION_LINEA")), .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")),
                               .Abreviatura = SabLib.BLL.Utils.stringNull(r("ABREVIATURA")), .IdMoneda = CInt(r("ID_MONEDA")), .Moneda = CStr(r("MONEDA")), .TCFormulaCustomer = SabLib.BLL.Utils.stringNull(r("TC_FORMULA_CUSTOMER")),
                               .Comentarios = SabLib.BLL.Utils.stringNull(r("COMENTARIOS")), .CorrelativoCC = SabLib.BLL.Utils.integerNull(r("CORRELATIVO_CC")), .IdEstadoProyecto = CInt(r("ID_ESTADO_PROYECTO")),
                               .EsInfoGeneral = CBool(r("ES_INFO_GENERAL"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' Obtiene steps
        ''' </summary>
        ''' <param name="idValidacion"></param>
        ''' <returns></returns>
        Public Shared Function loadListByValidacion(ByVal idValidacion As Integer) As List(Of ELL.Step)
            Dim query As String = "SELECT VS.* 
                                   FROM VSTEPS VS
                                   INNER JOIN VALIDACION_LINEA VL ON VL.ID_STEP = VS.ID WHERE VL.ID_VALIDACION=:ID_VALIDACION"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Step)(Function(r As OracleDataReader) _
            New ELL.Step With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                               .OBCOrigenDatos = SabLib.BLL.Utils.integerNull(r("OBC_ORIGEN_DATOS")), .TCFormula = SabLib.BLL.Utils.stringNull(r("TC_FORMULA")), .GastosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("GASTOS_AÑO_ORIGEN_DATOS")),
                               .IngresosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("INGRESOS_AÑO_ORIGEN_DATOS")), .FechaValidacion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_VALIDACION")),
                               .IdValidador = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR")), .DescripcionBRAIN = SabLib.BLL.Utils.integerNull(r("DESCRIPCION_BRAIN")),
                               .Code = SabLib.BLL.Utils.integerNull(r("CODE")),
                               .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .Estado = CStr(r("ESTADO")),
                               .Proyecto = CStr(r("PROYECTO")), .IdPlanta = CInt(r("ID_PLANTA")), .Planta = CStr(r("PLANTA")), .CambiarPlanta = CBool(r("CAMBIAR_PLANTA")),
                               .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")), .IdOferta = SabLib.BLL.Utils.stringNull(r("ID_OFERTA")),
                               .Origen = CInt(r("ORIGEN")), .Porcentaje = SabLib.BLL.Utils.integerNull(r("PORCENTAJE")), .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")),
                               .CambioPorcentaje = SabLib.BLL.Utils.booleanNull(r("CAMBIO_PORCENTAJE")), .IdCabecera = CInt(r("ID_CABECERA")),
                               .IdEstado = CInt(r("ID_ESTADO")), .Visible = CBool(r("VISIBLE")), .OrigenDatoReal = CInt(r("ORIGEN_DATO_REAL")),
                               .CostCarrier = SabLib.BLL.Utils.stringNull(r("COST_CARRIER")), .IdEstadoValidacion = SabLib.BLL.Utils.integerNull(r("ID_ESTADO_VALIDACION")),
                               .EstadoValidacion = SabLib.BLL.Utils.stringNull(r("ESTADO_VALIDACION")), .IdUsuarioValidador = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_VALIDADOR")),
                               .IdValidacionLinea = SabLib.BLL.Utils.integerNull(r("ID_VALIDACION_LINEA")), .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")),
                               .Abreviatura = SabLib.BLL.Utils.stringNull(r("ABREVIATURA")), .IdMoneda = CInt(r("ID_MONEDA")), .Moneda = CStr(r("MONEDA")), .TCFormulaCustomer = SabLib.BLL.Utils.stringNull(r("TC_FORMULA_CUSTOMER")),
                               .Comentarios = SabLib.BLL.Utils.stringNull(r("COMENTARIOS")), .CorrelativoCC = SabLib.BLL.Utils.integerNull(r("CORRELATIVO_CC")), .IdEstadoProyecto = CInt(r("ID_ESTADO_PROYECTO")),
                               .EsInfoGeneral = CBool(r("ES_INFO_GENERAL"))}, query, CadenaConexion, New OracleParameter("ID_VALIDACION", OracleDbType.Int32, idValidacion, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' Obtiene steps
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <returns></returns>
        Public Shared Function loadList(ByVal idCostGroup As Integer) As List(Of ELL.Step)
            Dim query As String = "SELECT * FROM VSTEPS WHERE ID_COST_GROUP=:ID_COST_GROUP ORDER BY ORDEN"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Step)(Function(r As OracleDataReader) _
            New ELL.Step With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                               .OBCOrigenDatos = SabLib.BLL.Utils.integerNull(r("OBC_ORIGEN_DATOS")), .TCFormula = SabLib.BLL.Utils.stringNull(r("TC_FORMULA")), .GastosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("GASTOS_AÑO_ORIGEN_DATOS")),
                               .IngresosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("INGRESOS_AÑO_ORIGEN_DATOS")), .FechaValidacion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_VALIDACION")),
                               .IdValidador = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR")), .DescripcionBRAIN = SabLib.BLL.Utils.integerNull(r("DESCRIPCION_BRAIN")),
                               .Code = SabLib.BLL.Utils.integerNull(r("CODE")),
                               .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .Estado = CStr(r("ESTADO")),
                               .Proyecto = CStr(r("PROYECTO")), .IdPlanta = CInt(r("ID_PLANTA")), .Planta = CStr(r("PLANTA")), .CambiarPlanta = CBool(r("CAMBIAR_PLANTA")),
                               .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")), .IdOferta = SabLib.BLL.Utils.stringNull(r("ID_OFERTA")),
                               .Origen = CInt(r("ORIGEN")), .Porcentaje = SabLib.BLL.Utils.integerNull(r("PORCENTAJE")), .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")),
                               .CambioPorcentaje = SabLib.BLL.Utils.booleanNull(r("CAMBIO_PORCENTAJE")), .IdCabecera = CInt(r("ID_CABECERA")),
                               .IdEstado = CInt(r("ID_ESTADO")), .Visible = CBool(r("VISIBLE")), .OrigenDatoReal = CInt(r("ORIGEN_DATO_REAL")),
                               .CostCarrier = SabLib.BLL.Utils.stringNull(r("COST_CARRIER")), .IdEstadoValidacion = SabLib.BLL.Utils.integerNull(r("ID_ESTADO_VALIDACION")),
                               .EstadoValidacion = SabLib.BLL.Utils.stringNull(r("ESTADO_VALIDACION")), .IdUsuarioValidador = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_VALIDADOR")),
                               .IdValidacionLinea = SabLib.BLL.Utils.integerNull(r("ID_VALIDACION_LINEA")), .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")),
                               .Abreviatura = SabLib.BLL.Utils.stringNull(r("ABREVIATURA")), .IdMoneda = CInt(r("ID_MONEDA")), .Moneda = CStr(r("MONEDA")), .TCFormulaCustomer = SabLib.BLL.Utils.stringNull(r("TC_FORMULA_CUSTOMER")),
                               .Comentarios = SabLib.BLL.Utils.stringNull(r("COMENTARIOS")), .CorrelativoCC = SabLib.BLL.Utils.integerNull(r("CORRELATIVO_CC")), .IdEstadoProyecto = CInt(r("ID_ESTADO_PROYECTO")),
                               .EsInfoGeneral = CBool(r("ES_INFO_GENERAL"))}, query, CadenaConexion, New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, idCostGroup, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' Obtiene steps
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function loadList() As List(Of ELL.Step)
            Dim query As String = "SELECT * FROM VSTEPS ORDER BY ORDEN"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Step)(Function(r As OracleDataReader) _
            New ELL.Step With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                               .OBCOrigenDatos = SabLib.BLL.Utils.integerNull(r("OBC_ORIGEN_DATOS")), .TCFormula = SabLib.BLL.Utils.stringNull(r("TC_FORMULA")), .GastosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("GASTOS_AÑO_ORIGEN_DATOS")),
                               .IngresosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("INGRESOS_AÑO_ORIGEN_DATOS")), .FechaValidacion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_VALIDACION")),
                               .IdValidador = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR")), .DescripcionBRAIN = SabLib.BLL.Utils.integerNull(r("DESCRIPCION_BRAIN")),
                               .Code = SabLib.BLL.Utils.integerNull(r("CODE")),
                               .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .Estado = CStr(r("ESTADO")),
                               .Proyecto = CStr(r("PROYECTO")), .IdPlanta = CInt(r("ID_PLANTA")), .Planta = CStr(r("PLANTA")), .CambiarPlanta = CBool(r("CAMBIAR_PLANTA")),
                               .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")), .IdOferta = SabLib.BLL.Utils.stringNull(r("ID_OFERTA")),
                               .Origen = CInt(r("ORIGEN")), .Porcentaje = SabLib.BLL.Utils.integerNull(r("PORCENTAJE")), .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")),
                               .CambioPorcentaje = SabLib.BLL.Utils.booleanNull(r("CAMBIO_PORCENTAJE")), .IdCabecera = CInt(r("ID_CABECERA")),
                               .IdEstado = CInt(r("ID_ESTADO")), .Visible = CBool(r("VISIBLE")), .OrigenDatoReal = CInt(r("ORIGEN_DATO_REAL")),
                               .CostCarrier = SabLib.BLL.Utils.stringNull(r("COST_CARRIER")), .IdEstadoValidacion = SabLib.BLL.Utils.integerNull(r("ID_ESTADO_VALIDACION")),
                               .EstadoValidacion = SabLib.BLL.Utils.stringNull(r("ESTADO_VALIDACION")), .IdUsuarioValidador = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_VALIDADOR")),
                               .IdValidacionLinea = SabLib.BLL.Utils.integerNull(r("ID_VALIDACION_LINEA")), .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")),
                               .Abreviatura = SabLib.BLL.Utils.stringNull(r("ABREVIATURA")), .IdMoneda = CInt(r("ID_MONEDA")), .Moneda = CStr(r("MONEDA")), .TCFormulaCustomer = SabLib.BLL.Utils.stringNull(r("TC_FORMULA_CUSTOMER")),
                               .Comentarios = SabLib.BLL.Utils.stringNull(r("COMENTARIOS")), .CorrelativoCC = SabLib.BLL.Utils.integerNull(r("CORRELATIVO_CC")), .IdEstadoProyecto = CInt(r("ID_ESTADO_PROYECTO")),
                               .EsInfoGeneral = CBool(r("ES_INFO_GENERAL"))}, query, CadenaConexion, Nothing)
        End Function

        ''' <summary>
        ''' Obtiene steps
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Public Shared Function loadListByCabecera(ByVal idCabecera As Integer) As List(Of ELL.Step)
            Dim query As String = "SELECT * FROM VSTEPS WHERE ID_CABECERA=:ID_CABECERA "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Step)(Function(r As OracleDataReader) _
            New ELL.Step With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                               .OBCOrigenDatos = SabLib.BLL.Utils.integerNull(r("OBC_ORIGEN_DATOS")), .TCFormula = SabLib.BLL.Utils.stringNull(r("TC_FORMULA")), .GastosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("GASTOS_AÑO_ORIGEN_DATOS")),
                               .IngresosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("INGRESOS_AÑO_ORIGEN_DATOS")), .FechaValidacion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_VALIDACION")),
                               .IdValidador = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR")), .DescripcionBRAIN = SabLib.BLL.Utils.integerNull(r("DESCRIPCION_BRAIN")),
                               .Code = SabLib.BLL.Utils.integerNull(r("CODE")),
                               .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .Estado = CStr(r("ESTADO")),
                               .Proyecto = CStr(r("PROYECTO")), .IdPlanta = CInt(r("ID_PLANTA")), .Planta = CStr(r("PLANTA")), .CambiarPlanta = CBool(r("CAMBIAR_PLANTA")),
                               .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")), .IdOferta = SabLib.BLL.Utils.stringNull(r("ID_OFERTA")),
                               .Origen = CInt(r("ORIGEN")), .Porcentaje = SabLib.BLL.Utils.integerNull(r("PORCENTAJE")), .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")),
                               .CambioPorcentaje = SabLib.BLL.Utils.booleanNull(r("CAMBIO_PORCENTAJE")), .IdCabecera = CInt(r("ID_CABECERA")),
                               .IdEstado = CInt(r("ID_ESTADO")), .Visible = CBool(r("VISIBLE")), .OrigenDatoReal = CInt(r("ORIGEN_DATO_REAL")),
                               .CostCarrier = SabLib.BLL.Utils.stringNull(r("COST_CARRIER")), .IdEstadoValidacion = SabLib.BLL.Utils.integerNull(r("ID_ESTADO_VALIDACION")),
                               .EstadoValidacion = SabLib.BLL.Utils.stringNull(r("ESTADO_VALIDACION")), .IdUsuarioValidador = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_VALIDADOR")),
                               .IdValidacionLinea = SabLib.BLL.Utils.integerNull(r("ID_VALIDACION_LINEA")), .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")),
                               .Abreviatura = SabLib.BLL.Utils.stringNull(r("ABREVIATURA")), .IdMoneda = CInt(r("ID_MONEDA")), .Moneda = CStr(r("MONEDA")), .TCFormulaCustomer = SabLib.BLL.Utils.stringNull(r("TC_FORMULA_CUSTOMER")),
                               .Comentarios = SabLib.BLL.Utils.stringNull(r("COMENTARIOS")), .CorrelativoCC = SabLib.BLL.Utils.integerNull(r("CORRELATIVO_CC")), .IdEstadoProyecto = CInt(r("ID_ESTADO_PROYECTO")),
                               .EsInfoGeneral = CBool(r("ES_INFO_GENERAL"))}, query, CadenaConexion, New OracleParameter("ID_CABECERA", OracleDbType.Int32, idCabecera, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' Obtiene steps
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <returns></returns>
        Public Shared Function loadListByCodigo(ByVal codigo As String) As List(Of ELL.Step)
            Dim query As String = "SELECT * FROM VSTEPS WHERE CODIGO_PROYECTO=:CODIGO_PROYECTO"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Step)(Function(r As OracleDataReader) _
            New ELL.Step With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION")), .IdCostGroup = CInt(r("ID_COST_GROUP")),
                               .OBCOrigenDatos = SabLib.BLL.Utils.integerNull(r("OBC_ORIGEN_DATOS")), .TCFormula = SabLib.BLL.Utils.stringNull(r("TC_FORMULA")), .GastosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("GASTOS_AÑO_ORIGEN_DATOS")),
                               .IngresosAñoOrigenDatos = SabLib.BLL.Utils.integerNull(r("INGRESOS_AÑO_ORIGEN_DATOS")), .FechaValidacion = SabLib.BLL.Utils.dateTimeNull(r("FECHA_VALIDACION")),
                               .IdValidador = SabLib.BLL.Utils.integerNull(r("ID_VALIDADOR")), .DescripcionBRAIN = SabLib.BLL.Utils.integerNull(r("DESCRIPCION_BRAIN")),
                               .Code = SabLib.BLL.Utils.integerNull(r("CODE")),
                               .IdBonos = SabLib.BLL.Utils.integerNull(r("ID_BONOS")), .Estado = CStr(r("ESTADO")),
                               .Proyecto = CStr(r("PROYECTO")), .IdPlanta = CInt(r("ID_PLANTA")), .Planta = CStr(r("PLANTA")), .CambiarPlanta = CBool(r("CAMBIAR_PLANTA")),
                               .IdCostGroupOT = SabLib.BLL.Utils.integerNull(r("ID_COST_GROUP_OT")), .IdOferta = SabLib.BLL.Utils.stringNull(r("ID_OFERTA")),
                               .Origen = CInt(r("ORIGEN")), .Porcentaje = SabLib.BLL.Utils.integerNull(r("PORCENTAJE")), .IdAgrupacion = SabLib.BLL.Utils.integerNull(r("ID_AGRUPACION")),
                               .CambioPorcentaje = SabLib.BLL.Utils.booleanNull(r("CAMBIO_PORCENTAJE")), .IdCabecera = CInt(r("ID_CABECERA")),
                               .IdEstado = CInt(r("ID_ESTADO")), .Visible = CBool(r("VISIBLE")), .OrigenDatoReal = CInt(r("ORIGEN_DATO_REAL")),
                               .CostCarrier = SabLib.BLL.Utils.stringNull(r("COST_CARRIER")), .IdEstadoValidacion = SabLib.BLL.Utils.integerNull(r("ID_ESTADO_VALIDACION")),
                               .EstadoValidacion = SabLib.BLL.Utils.stringNull(r("ESTADO_VALIDACION")), .IdUsuarioValidador = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_VALIDADOR")),
                               .IdValidacionLinea = SabLib.BLL.Utils.integerNull(r("ID_VALIDACION_LINEA")), .IdPlantaSAB = CInt(r("ID_PLANTA_SAB")),
                               .Abreviatura = SabLib.BLL.Utils.stringNull(r("ABREVIATURA")), .IdMoneda = CInt(r("ID_MONEDA")), .Moneda = CStr(r("MONEDA")), .TCFormulaCustomer = SabLib.BLL.Utils.stringNull(r("TC_FORMULA_CUSTOMER")),
                               .Comentarios = SabLib.BLL.Utils.stringNull(r("COMENTARIOS")), .CorrelativoCC = SabLib.BLL.Utils.integerNull(r("CORRELATIVO_CC")), .IdEstadoProyecto = CInt(r("ID_ESTADO_PROYECTO")),
                               .EsInfoGeneral = CBool(r("ES_INFO_GENERAL"))}, query, CadenaConexion, New OracleParameter("CODIGO_PROYECTO", OracleDbType.NVarchar2, codigo, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idPlanta"></param>
        ''' <param name="idEstado"></param>
        ''' <param name="idAgrupacion"></param>
        ''' <returns></returns>
        Public Shared Function getNumStepsAgrupados(ByVal idCabecera As Integer, ByVal idPlanta As Integer, ByVal idEstado As String, ByVal idAgrupacion As Integer) As Integer
            Dim query As String = "SELECT COUNT(*) FROM VSTEPS WHERE ID_CABECERA=:ID_CABECERA AND ID_PLANTA=:ID_PLANTA AND ID_ESTADO=:ID_ESTADO AND ID_AGRUPACION=:ID_AGRUPACION AND CAMBIO_PORCENTAJE=1 AND ORIGEN=1 AND OBC_ORIGEN_DATOS=1"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_CABECERA", OracleDbType.Int32, idCabecera, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_ESTADO", OracleDbType.Int32, idEstado, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_AGRUPACION", OracleDbType.Int32, idAgrupacion, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Public Shared Function getLastSecuencia(ByVal idCabecera As Integer) As Integer
            Dim query As String = "SELECT MAX(COST_CARRIER) AS ULTIMO_CODIGO FROM VSTEPS WHERE ID_CABECERA=:ID_CABECERA"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_CABECERA", OracleDbType.Int32, idCabecera, ParameterDirection.Input))

            Dim ultimoCodigo As String = Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query, CadenaConexion, lParameters.ToArray())
            Dim proximoCodigo As Integer = 0

            If (Not String.IsNullOrEmpty(ultimoCodigo)) Then
                ' Obtenemos el codigo de proyecto de la cabecera
                Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(idCabecera)
                ultimoCodigo = ultimoCodigo.Replace(cabecera.CodigoProyecto, String.Empty)
                proximoCodigo = CInt(ultimoCodigo)
            End If

            Return proximoCodigo
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <param name="idCostGroup"></param>
        Public Shared Sub ChangeCompany(ByVal idStep As Integer, idCostGroup As Integer)
            Dim query As String = "UPDATE STEP SET ID_COST_GROUP=:ID_COST_GROUP WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, idCostGroup, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idStep, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)
        End Sub

        ''' <summary>
        ''' Borrar, inserta y actualiza steps
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <param name="listaStepsInsertar"></param>
        ''' <param name="listaIdStepsOfertaBorrar"></param>
        ''' <param name="listaStepsActualizar"></param>
        Public Shared Sub DeleteOldSaveNew(ByVal idCostGroup As Integer, ByVal listaStepsInsertar As List(Of ELL.StepOferta), ByVal listaIdStepsOfertaBorrar As List(Of String), ByVal listaStepsActualizar As List(Of ELL.StepOferta))
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty
            Dim lParameters As List(Of OracleParameter)
            Dim p As OracleParameter

            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                ' 1º Eliminamos los steps
                If (listaIdStepsOfertaBorrar.Count > 0) Then
                    query = "DELETE FROM STEP WHERE ID_OFERTA IN ({0}) AND ID_COST_GROUP=:ID_COST_GROUP"
                    Dim where As String = String.Empty
                    For Each idStep In listaIdStepsOfertaBorrar
                        If (String.IsNullOrEmpty(where)) Then
                            where &= "'" & idStep & "'"
                        Else
                            where &= ", '" & idStep & "'"
                        End If
                    Next

                    query = String.Format(query, where)

                    Memcached.OracleDirectAccess.NoQuery(query, con, New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, idCostGroup, ParameterDirection.Input))
                End If

                ' 2º Insertamos los nuevos
                If (listaStepsInsertar.Count > 0) Then
                    For Each stepOferta In listaStepsInsertar
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

                        '7º Hay que meter los valores del step
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

                ' 3º Actualizamos los existentes
                If (listaStepsActualizar.Count > 0) Then
                    Dim idStep As Integer = Integer.MinValue
                    For Each stepOferta In listaStepsActualizar
                        query = "UPDATE STEP SET DESCRIPCION=:DESCRIPCION WHERE ID_OFERTA=:ID_OFERTA AND ID_COST_GROUP=:ID_COST_GROUP"

                        lParameters = New List(Of OracleParameter)
                        lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, stepOferta.StepName, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_OFERTA", OracleDbType.NVarchar2, stepOferta.Id, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, idCostGroup, ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                        query = "SELECT ID FROM STEP WHERE ID_OFERTA=:ID_OFERTA AND ID_COST_GROUP=:ID_COST_GROUP"

                        lParameters = New List(Of OracleParameter)
                        lParameters.Add(New OracleParameter("ID_OFERTA", OracleDbType.NVarchar2, stepOferta.Id, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, idCostGroup, ParameterDirection.Input))

                        idStep = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, con, lParameters.ToArray)

                        'Hay que actualizar los valores del step
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

                            query = "UPDATE VALORES_STEP " _
                                    & "SET VALOR=:VALOR WHERE ID_STEP=:ID_STEP AND ID_COLUMNA=:ID_COLUMNA"

                            lParameters = New List(Of OracleParameter)
                            lParameters.Add(New OracleParameter("ID_STEP", OracleDbType.Int32, idStep, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID_COLUMNA", OracleDbType.Int32, idColumna, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("VALOR", OracleDbType.Int32, valor, ParameterDirection.Input))

                            Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                        Next
                    Next
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

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <param name="descripcion"></param>
        Public Shared Sub ChangeDescription(ByVal idStep As Integer, ByVal descripcion As String)
            Dim query As String = "UPDATE STEP SET DESCRIPCION=:DESCRIPCION WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, descripcion, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idStep, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <param name="porcentaje"></param>
        Public Shared Sub ChangePercentage(ByVal idStep As Integer, ByVal porcentaje As Integer)
            Dim query As String = "UPDATE STEP SET PORCENTAJE=:PORCENTAJE WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("PORCENTAJE", OracleDbType.Int32, porcentaje, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idStep, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <param name="codigoPortador"></param>
        Public Shared Sub ChangeCodigoPortador(ByVal idStep As Integer, ByVal codigoPortador As String)
            Dim query As String = "UPDATE STEP SET COST_CARRIER=:COST_CARRIER WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("COST_CARRIER", OracleDbType.NVarchar2, codigoPortador, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idStep, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)
        End Sub

        ''' <summary>
        ''' Guarda un step
        ''' </summary>
        ''' <param name="step"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal [step] As ELL.Step)
            Dim query As String = String.Empty
            Dim bNuevo As Boolean = ([step].Id = 0)

            ' Guardamos
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, [step].Descripcion, ParameterDirection.Input))

            If (bNuevo) Then
                query = "INSERT INTO STEP (DESCRIPCION, ID_COST_GROUP, OBC_ORIGEN_DATOS, GASTOS_AÑO_ORIGEN_DATOS, ORDEN, ORIGEN, PORCENTAJE, ORIGEN_DATO_REAL, COST_CARRIER, ES_INFO_GENERAL) " _
                        & "VALUES (:DESCRIPCION, :ID_COST_GROUP, :OBC_ORIGEN_DATOS, :GASTOS_AÑO_ORIGEN_DATOS, (SELECT NVL(MAX(ORDEN), 0) + 1 FROM STEP WHERE ID_COST_GROUP=:ID_COST_GROUP), :ORIGEN, :PORCENTAJE, :ORIGEN_DATO_REAL, :COST_CARRIER, :ES_INFO_GENERAL) RETURNING ID INTO :RETURN_VALUE"

                lParameters.Add(New OracleParameter("OBC_ORIGEN_DATOS", OracleDbType.Int32, [step].OBCOrigenDatos, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("GASTOS_AÑO_ORIGEN_DATOS", OracleDbType.Int32, [step].GastosAñoOrigenDatos, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_COST_GROUP", OracleDbType.Int32, [step].IdCostGroup, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ORIGEN", OracleDbType.Int32, [step].Origen, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("PORCENTAJE", OracleDbType.Int32, If([step].Porcentaje = Integer.MinValue, DBNull.Value, [step].Porcentaje), ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ORIGEN_DATO_REAL", OracleDbType.Int32, [step].OrigenDatoReal, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("COST_CARRIER", OracleDbType.NVarchar2, If(String.IsNullOrEmpty([step].CostCarrier), DBNull.Value, [step].CostCarrier), ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ES_INFO_GENERAL", OracleDbType.Int32, [step].EsInfoGeneral, ParameterDirection.Input))

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParameters.Add(p)
            Else
                query = "UPDATE STEP SET DESCRIPCION=:DESCRIPCION WHERE ID=:ID"

                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, [step].Id, ParameterDirection.Input))
            End If

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)

            If (bNuevo) Then
                [step].Id = lParameters.Last.Value
            End If
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <param name="correlativo"></param>
        Public Shared Sub ChangeCorrelative(ByVal idStep As Integer, ByVal correlativo As Integer)
            Dim query As String = "UPDATE STEP SET CORRELATIVO_CC=:CORRELATIVO_CC WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("CORRELATIVO_CC", OracleDbType.Int32, correlativo, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idStep, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un step en cascada
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Delete(ByVal id As Integer)
            Dim query As String = "DELETE FROM STEP WHERE ID=:ID"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

    End Class

End Namespace