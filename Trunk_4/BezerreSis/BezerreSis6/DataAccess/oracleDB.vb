Imports System
Imports System.Collections.Generic
Imports BezerreSis
Imports SabLib

Public Class oracleDB

    Public cnStrBezerresis As String = Configuration.ConfigurationManager.ConnectionStrings("BEZERRESIS").ConnectionString '''' permisos para GTK y SAB
    ReadOnly idBatzDefault As Integer = 1001

#Region "SAB getters"

    Function getIdPlantaBrainFromIdPlantaSab(ByVal idPlantaSab As String) As String
        Dim query As String = "SELECT ID_BRAIN FROM SAB.PLANTAS WHERE ID = :ID"
        Dim cn As New OracleConnection(cnStrBezerresis)
        Dim cmd As New OracleCommand(query, cn)
        cmd.BindByName = True
        cmd.Parameters.Add(New OracleParameter("ID", OracleDbType.Int32, CInt(idPlantaSab), ParameterDirection.Input))
        Dim result As String = "0"
        Dim odr As OracleDataReader = Nothing
        Try
            cn.Open()
            odr = cmd.ExecuteReader()
            If odr.HasRows Then
                If (odr.Read()) Then
                    result = odr(0)
                End If
            End If
        Catch ex As Exception
            Throw
        Finally
            If (odr IsNot Nothing) Then odr.Close() : odr.Dispose()
            cmd.Dispose()
            cn.Close() : cn.Dispose()
        End Try
        Return CInt(result)
    End Function

    Function getUsuariosCreadores() As List(Of SelectListItem)
        Return getUsuarios(True)
    End Function

    Function getUsuarios(Optional ByVal creadores As Boolean = False) As List(Of SelectListItem)
        Dim query As String = "SELECT DISTINCT * FROM 
                                (SELECT ID, NOMBRE || ' ' || APELLIDO1 || ' ' || APELLIDO2, CASE WHEN FECHABAJA > SYSDATE OR FECHABAJA IS NULL THEN 1 ELSE 0 END 
                                FROM SAB.USUARIOS U
                                INNER JOIN SAB.USUARIOSGRUPOS UG ON UG.IDUSUARIOS = U.ID
                                INNER JOIN SAB.GRUPOSRECURSOS GR ON GR.IDGRUPOS = UG.IDGRUPOS
                                WHERE GR.IDRECURSOS = 372
                                AND (U.FECHABAJA IS NULL OR U.FECHABAJA > SYSDATE)
                                UNION ALL
                                SELECT DISTINCT U.ID, NOMBRE || ' ' || APELLIDO1 || ' ' || APELLIDO2, CASE WHEN FECHABAJA > SYSDATE OR FECHABAJA IS NULL THEN 1 ELSE 0 END 
                                FROM SAB.USUARIOS U
                                INNER JOIN RECLAMACIONES R ON R.CREADOR = U.ID)
                                WHERE 1=1 "
        Dim inClause = False
        Dim inCounter = 0
        Dim devAdmins
        If creadores AndAlso (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
            devAdmins = Configuration.ConfigurationManager.AppSettings("DevAdmins").Split(",").ToList()
            If devAdmins.Count > 0 Then
                inClause = True
                query &= " AND ID NOT IN ("
                For Each dev In devAdmins
                    inCounter += 1
                    query &= $":DEV{inCounter},"
                    ''''   query &= ":DEV" & inCounter & ","
                Next
                query = query.Trim(",")
                query &= ")"
            End If
        End If
        Dim cn As New OracleConnection(cnStrBezerresis)
        Dim cmd As New OracleCommand(query, cn)
        cmd.BindByName = True
        cmd.Parameters.Add(New OracleParameter("IDRECURSO", OracleDbType.Int32, CInt(Configuration.ConfigurationManager.AppSettings("RecursoWeb")), ParameterDirection.Input))
        If creadores AndAlso (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
            For counter = 1 To inCounter
#Disable Warning BC42104 ' Se asigna el valor a la variable en un If con la misma lógica
                cmd.Parameters.Add(New OracleParameter("DEV" & counter, OracleDbType.NVarchar2, devAdmins(counter - 1), ParameterDirection.Input))
#Enable Warning BC42104
            Next
        End If
        Dim lst As New List(Of SelectListItem)
        Dim odr As OracleDataReader = Nothing
        Try
            cn.Open()
            odr = cmd.ExecuteReader()
            If odr.HasRows Then
                While (odr.Read())
                    lst.Add(New SelectListItem With {.Text = odr(1), .Value = odr(2) & "-" & odr(0)})
                End While
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If (odr IsNot Nothing) Then odr.Close() : odr.Dispose()
            cmd.Dispose()
            cn.Close() : cn.Dispose()
        End Try
        Return lst
    End Function

    Friend Function getNumeroIncidenciasFecha(anno As Integer, mes As Integer, productos As List(Of Integer), clientes As List(Of Integer)) As Integer
        Dim startDate = New Date(anno, mes, 1)
        Dim endDate = startDate.AddMonths(1).AddDays(-1)
        Dim query As String = "SELECT COUNT(*)
                                FROM RECLAMACIONES 
                                where RECLAMACIONOFICIAL = 1  and FECHACREACION <= :ENDDATE and FECHACREACION >= :STARTDATE 
                                and AFECTA_INDICADORES = 1 " ' and cliente = :CLIENTE AND PRODUCTO = :PRODUCTO"
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("STARTDATE", OracleDbType.Date, startDate, ParameterDirection.Input))
        lParam.Add(New OracleParameter("ENDDATE", OracleDbType.Date, endDate, ParameterDirection.Input))
        If productos.Count > 0 Then
            query &= "AND PRODUCTO IN (" & String.Join(",", productos) & ") "
        End If
        If clientes.Count > 0 Then
            query &= "AND CLIENTE IN (" & String.Join(",", clientes) & ") "
        End If
        Dim numero As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStrBezerresis, lParam.ToArray)
        Return numero
    End Function

    Function getCreadorFromId(id As Decimal) As String
        Dim query As String = "SELECT NOMBRE || ' ' || APELLIDO1 || ' ' || APELLIDO2 AS NOMBRECOMPLETO FROM SAB.USUARIOS WHERE ID=:ID"
        Dim cn As New OracleConnection(cnStrBezerresis)
        Dim cmd As New OracleCommand(query, cn)
        cmd.BindByName = True
        cmd.Parameters.Add(New OracleParameter("ID", OracleDbType.Int32, CInt(id), ParameterDirection.Input))
        Dim result As String = ""
        Dim odr As OracleDataReader = Nothing
        Try
            cn.Open()
            odr = cmd.ExecuteReader()
            If odr.HasRows Then
                If (odr.Read()) Then
                    result = odr("NOMBRECOMPLETO")
                End If
            End If
        Catch ex As Exception
            Throw
        Finally
            If (odr IsNot Nothing) Then odr.Close() : odr.Dispose()
            cmd.Dispose()
            cn.Close() : cn.Dispose()
        End Try
        Return result
    End Function

    Function getIdSabPlantaForClienteFilial(id As Decimal) As Integer
        Dim query As String = "SELECT UPPER(NOMBRE) FROM CLIENTES WHERE ID = :ID"
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
        Dim namePlantaFilial = Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query, cnStrBezerresis, lParam.ToArray)

        Dim query2 As String = "SELECT ID FROM SAB.PLANTAS WHERE UPPER(NOMBRE) LIKE :NOMBRE"
        Dim lParam2 As New List(Of OracleParameter)
        lParam2.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, namePlantaFilial, ParameterDirection.Input))
        Dim idSabPlanta = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query2, cnStrBezerresis, lParam2.ToArray)
        Return idSabPlanta
    End Function

    Function getUserIdFromActiveDirectoryId(idDirectorioActivo As String) As Integer
        Dim query As String = "SELECT ID FROM SAB.USUARIOS
                                WHERE UPPER(IDDIRECTORIOACTIVO) LIKE :IDDIRECTORIOACTIVO
                                AND (FECHABAJA IS NULL OR FECHABAJA > SYSDATE)"
        Dim result = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStrBezerresis, New OracleParameter("IDDIRECTORIOACTIVO", OracleDbType.NVarchar2, idDirectorioActivo.ToUpper, ParameterDirection.Input))
        Return result
    End Function
#End Region

#Region "Bezerresis getters"

    Friend Function getObjetivo(monthYearDate As String, clienteId As Integer) As Object
        Dim query As String = "select PPM_MENSUAL, PPM_ANUAL, IPB_SEMESTRAL from OBJETIVOS where to_char(FECHA, 'yyyy') = :YEARDATE and IDCLIENTE=:IDCLIENTE"
        Dim lParam2 As New List(Of OracleParameter)
        lParam2.Add(New OracleParameter("YEARDATE", OracleDbType.NVarchar2, monthYearDate.Substring(2, 4), ParameterDirection.Input))
        lParam2.Add(New OracleParameter("IDCLIENTE", OracleDbType.Int32, clienteId, ParameterDirection.Input))
        Dim resultados = Memcached.OracleDirectAccess.seleccionar(Of String)(Function(r As OracleDataReader) If(r(0) IsNot DBNull.Value, r(0), "-") & ";" & If(r(1) IsNot DBNull.Value, r(1), "-") & ";" & If(r(2) IsNot DBNull.Value, r(2), "-"), query, cnStrBezerresis, lParam2.ToArray).FirstOrDefault
        Return If(resultados, "-;-;-")
    End Function

    Friend Function getObjetivoDefault(monthYearDate As String) As Object
        Dim query As String = "select PPM_MENSUAL, PPM_ANUAL, IPB_SEMESTRAL from OBJETIVOS where to_char(FECHA, 'yyyy') = :YEARDATE and IDCLIENTE=:IDCLIENTE"
        Dim lParam2 As New List(Of OracleParameter)
        lParam2.Add(New OracleParameter("YEARDATE", OracleDbType.NVarchar2, monthYearDate.Substring(2, 4), ParameterDirection.Input))
        lParam2.Add(New OracleParameter("IDCLIENTE", OracleDbType.Int32, idBatzDefault, ParameterDirection.Input))
        Dim resultados = Memcached.OracleDirectAccess.seleccionar(Of String)(Function(r As OracleDataReader) If(r(0) IsNot DBNull.Value, r(0), "-") & ";" & If(r(1) IsNot DBNull.Value, r(1), "-") & ";" & If(r(2) IsNot DBNull.Value, r(2), "-"), query, cnStrBezerresis, lParam2.ToArray).FirstOrDefault
        Return If(resultados, "-;-;-")
    End Function

    Friend Function getSumaPiezasVendidasAcumuladas(monthYearDate As String, productos As List(Of Integer), clientes As List(Of Integer), idPlanta As String, mesesAcumulados As Integer) As Object
        Dim fechaFinal = DateTime.ParseExact(monthYearDate, "MMyyyy", Globalization.CultureInfo.InvariantCulture)
        Dim fechaInicio = fechaFinal.AddMonths(mesesAcumulados + 1)
        Dim lParam As New List(Of OracleParameter)
        Dim query As String = "select SUM(ENVIADAS) FROM VENTAS
                                where IDPLANTA=:IDPLANTA "
        lParam.Add(New OracleParameter("IDPLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        condicionFechaMin(query, lParam, fechaInicio)
        condicionFechaMax(query, lParam, fechaFinal)
        condicionClienteProducto("cliente", clientes, query, lParam)
        condicionClienteProducto("producto", productos, query, lParam)
        Dim result As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStrBezerresis, lParam.ToArray)
        Return result
    End Function

    Friend Function getIndicadoresCalidad(fecha_desde As Date?, fecha_hasta As Date?, clientes As List(Of Integer), productos As List(Of Integer), idPlanta As String) As List(Of String())
        Dim lParam As New List(Of OracleParameter)
        Dim query As String = "select IDPLANTA, ANNO, MES, SUM(TOTAL),'',SUM(INCIDENCIAS),SUM(RECHAZADAS) FROM VENTAS
                               where IDPLANTA=:IDPLANTA "
        lParam.Add(New OracleParameter("IDPLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        Dim fechaInicio As Date
        Dim fechaFinal As Date
        fechaInicio = If(fecha_desde, Date.Today.AddMonths(-Configuration.ConfigurationManager.AppSettings("DefaultMonthRange")))
        fechaFinal = If(fecha_hasta, Date.Today.AddDays(-Configuration.ConfigurationManager.AppSettings("CalculusDay")).AddMonths(-1))
        condicionFechaMin(query, lParam, fechaInicio)
        condicionFechaMax(query, lParam, fechaFinal)
        condicionClienteProducto("cliente", clientes, query, lParam)
        condicionClienteProducto("producto", productos, query, lParam)
        query &= "GROUP BY ANNO,MES,IDPLANTA ORDER BY ANNO DESC, MES DESC"
        Dim resultString As List(Of String()) = Memcached.OracleDirectAccess.Seleccionar(query, cnStrBezerresis, lParam.ToArray)
        Dim result As List(Of String()) = transformResultCalidad(idPlanta, fechaInicio, fechaFinal, resultString)
        Return result
    End Function

    Public Function getIndicadoresCalidadGarantias(fecha_desde As Nullable(Of Date), fecha_hasta As Nullable(Of Date), lClientes As List(Of Integer), lProductos As List(Of Integer), idPlanta As Integer) As List(Of String())
        Dim resultado As New List(Of String())

        ' Crear el comando SQL parametrizado
        Dim query As String = "
    SELECT 
        TO_CHAR(r.FECHACREACION, 'yyyy/MM') AS FECHA,
        COUNT(*) AS NUM_INCIDENCIAS,
        NVL(SUM(r.NUMPIEZASNOK), 0) AS PIEZAS_RECHAZADAS
    FROM
        RECLAMACIONES r
        JOIN CLIENTES c ON r.CLIENTE = c.ID
    WHERE
        c.IDPLANTA = :idPlanta
        AND r.CLASIFICACION = 2 
        AND r.AFECTA_INDICADORES = 1       
        AND r.FECHACREACION >= :fechaDesde
        AND r.FECHACREACION <= :fechaHasta"

        ' Añadir cláusulas para los clientes y productos si existen
        If lClientes IsNot Nothing AndAlso lClientes.Count > 0 Then
            query &= " AND r.CLIENTE IN (" & String.Join(",", lClientes) & ") "
        End If

        If lProductos IsNot Nothing AndAlso lProductos.Count > 0 Then
            query &= " AND r.PRODUCTO IN (" & String.Join(",", lProductos) & ") "
        End If

        query &= "
    GROUP BY
        TO_CHAR(r.FECHACREACION, 'yyyy/MM')
    ORDER BY
        TO_CHAR(r.FECHACREACION, 'yyyy/MM') DESC"

        ' Crear la lista de parámetros para la consulta
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("idPlanta", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        lParam.Add(New OracleParameter("fechaDesde", OracleDbType.Date, If(fecha_desde.HasValue, fecha_desde.Value, Date.MinValue), ParameterDirection.Input))
        lParam.Add(New OracleParameter("fechaHasta", OracleDbType.Date, If(fecha_hasta.HasValue, fecha_hasta.Value, Date.MaxValue), ParameterDirection.Input))

        ' Ejecutar la consulta utilizando Memcached.OracleDirectAccess
        Try
            resultado = Memcached.OracleDirectAccess.Seleccionar(query, cnStrBezerresis, lParam.ToArray())
        Catch ex As Exception
            ' Manejo de errores para identificar cualquier problema durante la ejecución de la consulta
            Throw New Exception("Error ejecutando la consulta en getIndicadoresCalidadGarantias", ex)
        End Try

        Return resultado
    End Function


    Friend Function getMediaDiasAcumulado(fechaInicio As Date, fechaFinal As Date, productos As List(Of Integer), clientes As List(Of Integer), tipo As String) As Decimal
        Dim column = If(tipo.Equals("14"), "DIAS14", "DIAS56")
        Dim query As String = "select round(avg(" & column & "),2) from INDICADORES_PROCESO "
        Dim lParam As New List(Of OracleParameter)
        condicionFechaMin(query, lParam, fechaInicio)
        condicionFechaMax(query, lParam, fechaFinal)
        condicionClienteProducto("cliente", clientes, query, lParam)
        condicionClienteProducto("producto", productos, query, lParam)
        Dim numero As Decimal = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Decimal)(query, cnStrBezerresis, lParam.ToArray)
        Return numero
    End Function

    Friend Function getSumaRepetitivasAcumulado(fechaInicio As Date, fechaFinal As Date, productos As List(Of Integer), clientes As List(Of Integer)) As Integer
        Dim query As String = "select sum(REPETITIVA) from INDICADORES_PROCESO "
        Dim lParam As New List(Of OracleParameter)
        condicionFechaMin(query, lParam, fechaInicio)
        condicionFechaMax(query, lParam, fechaFinal)
        condicionClienteProducto("cliente", clientes, query, lParam)
        condicionClienteProducto("producto", productos, query, lParam)
        Dim numero As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStrBezerresis, lParam.ToArray)
        Return numero
    End Function

    'Friend Function getObjetivosProceso() As List(Of String())
    Friend Function getObjetivosProceso() As List(Of OBJETIVOPROCESO)
        'Dim query As String = "SELECT EXTRACT(YEAR FROM FECHA),REPETITIVAS,DIAS14,DIAS56 FROM OBJETIVOS_PROCESO ORDER BY FECHA DESC"
        Dim query As String = "SELECT ID,EXTRACT(YEAR FROM FECHA),REPETITIVAS,DIAS14,DIAS56 FROM OBJETIVOS_PROCESO ORDER BY FECHA DESC"
        'Dim result As List(Of OBJETIVOPROCESO) = Memcached.OracleDirectAccess.Seleccionar(query, cnStrBezerresis)
        Dim result As List(Of OBJETIVOPROCESO) = Memcached.OracleDirectAccess.seleccionar(Of OBJETIVOPROCESO)(Function(o) New OBJETIVOPROCESO With {.ID = o(0), .ANNO = o(1), .REPETITIVAS = o(2), .DIAS14 = o(3), .DIAS56 = o(4)}, query, cnStrBezerresis)
        Return result
    End Function

    'Friend Function getObjetivoProceso(year As Integer) As String()º
    'Friend Function getObjetivoProceso(year As Integer) As OBJETIVOPROCESO
    Friend Function getObjetivoProceso(id As Integer) As OBJETIVOPROCESO
        'Dim query As String = "SELECT REPETITIVAS,DIAS14,DIAS56 FROM OBJETIVOS_PROCESO WHERE EXTRACT(YEAR FROM FECHA) = :ANNO"
        'Dim query As String = "SELECT ID,REPETITIVAS,DIAS14,DIAS56 FROM OBJETIVOS_PROCESO WHERE EXTRACT(YEAR FROM FECHA) = :ANNO"
        Dim query As String = "SELECT ID,REPETITIVAS,DIAS14,DIAS56,EXTRACT(YEAR FROM FECHA) FROM OBJETIVOS_PROCESO WHERE ID = :ID"
        'Dim result As String() = Memcached.OracleDirectAccess.Seleccionar(query, cnStrBezerresis, New OracleParameter("ANNO", OracleDbType.Int32, year, ParameterDirection.Input)).FirstOrDefault
        Dim result As OBJETIVOPROCESO = Memcached.OracleDirectAccess.seleccionar(Of OBJETIVOPROCESO)(Function(o) New OBJETIVOPROCESO With {.ID = o(0), .REPETITIVAS = o(1), .DIAS14 = o(2), .DIAS56 = o(3), .ANNO = o(4)', .ANNO = Year()
        }, query, cnStrBezerresis, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault
        Return result
    End Function

    Friend Function getReclamacionesFromView(ByVal oficial As Boolean) As List(Of ReclamacionViewModel)
        Dim query As String = "SELECT ESTADO,CODIGOGTK,FECHACREACION,FFIN_PREVISTO_E56,CREADOR,RESPONSABLE_O_PERSEGUIDOR,REFINTERNAPIEZA,DENOMINACION,CLIENTE,PRODUCTO,CODXCLIENTE,NUMPIEZASNOK,
                                      DESCRIPCION,PROCEDENCIA,CLASIFICACION,REPETITIVA,OFICIAL,CREADOR_ID,CLIENTE_ID,PRODUCTO_ID,FECHA_CIERRE,FECHA_RESP_CONTENCION,FECHA_RESP_CORRECTIVAS,ID,AFECTA_INDICADORES,FECHA_CIERRECLIENTE
                               FROM W_RECLAMACIONES "
        If oficial Then
            query &= "WHERE OFICIAL = 'Sí'"
        End If
        'Dim result As List(Of ReclamacionViewModel) = Memcached.OracleDirectAccess.seleccionar(Of ReclamacionViewModel)(Function(r) New ReclamacionViewModel With {.ESTADO = r(0), .CODIGOGTK = SabLib.BLL.Utils.integerNull(r(1)), .FECHACREACION = r(2), .FFIN_PREVISTO_E56 = BLL.Utils.DateNull(r(3)), .CREADOR = r(4), .RESPONSABLE_O_PERSEGUIDOR = BLL.Utils.stringNull(r(5)), .REFINTERNAPIEZA = r(6), .DENOMINACION = r(7), .CLIENTE = r(8), .PRODUCTO = r(9), .CODXCLIENTE = r(10), .NUMPIEZASNOK = r(11), .DESCRIPCION = r(12), .PROCEDENCIA = r(13), .CLASIFICACION = r(14), .REPETITIVA = r(15), .OFICIAL = r(16), .CREADOR_ID = r(17), .CLIENTE_ID = r(18), .PRODUCTO_ID = r(19), .FECHA_CIERRE = BLL.Utils.dateTimeNull(r(20)), .FECHA_RESP_CONTENCION = BLL.Utils.dateTimeNull(r(21)), .FECHA_RESP_CORRECTIVAS = BLL.Utils.dateTimeNull(r(22)), .ID = CInt(r(23))}, query, cnStrBezerresis, Nothing)
        'Dim result As List(Of ReclamacionViewModel) = Memcached.OracleDirectAccess.seleccionar(Of ReclamacionViewModel)(Function(r) New ReclamacionViewModel With {.ESTADO = SabLib.BLL.Utils.stringNull(r("ESTADO")), .CODIGOGTK = SabLib.BLL.Utils.integerNull(r("CODIGOGTK")), .FECHACREACION = BLL.Utils.DateNull(r("FECHACREACION")), .FFIN_PREVISTO_E56 = BLL.Utils.DateNull(r("FFIN_PREVISTO_E56")), .ID = CInt(r("ID"))}, query, cnStrBezerresis, Nothing)
        Dim result As List(Of ReclamacionViewModel) = Memcached.OracleDirectAccess.Seleccionar(Of ReclamacionViewModel)(Function(r) New ReclamacionViewModel With {
                                                                                                                        .ESTADO = SabLib.BLL.Utils.stringNull(r("ESTADO")),
                                                                                                                        .CODIGOGTK = SabLib.BLL.Utils.integerNull(r("CODIGOGTK"), Nothing),
                                                                                                                        .FECHACREACION = BLL.Utils.dateTimeNull(r("FECHACREACION")),
                                                                                                                        .FFIN_PREVISTO_E56 = BLL.Utils.dateTimeNull(r("FFIN_PREVISTO_E56")),
                                                                                                                        .CREADOR = BLL.Utils.stringNull(r("CREADOR")),
                                                                                                                        .RESPONSABLE_O_PERSEGUIDOR = BLL.Utils.stringNull(r("RESPONSABLE_O_PERSEGUIDOR")),
                                                                                                                        .REFINTERNAPIEZA = BLL.Utils.stringNull(r("REFINTERNAPIEZA")),
                                                                                                                        .DENOMINACION = BLL.Utils.stringNull(r("DENOMINACION")),
                                                                                                                        .CLIENTE = BLL.Utils.stringNull(r("CLIENTE")),
                                                                                                                        .PRODUCTO = BLL.Utils.stringNull(r("PRODUCTO")),
                                                                                                                        .CODXCLIENTE = BLL.Utils.stringNull(r("CODXCLIENTE")),
                                                                                                                        .NUMPIEZASNOK = BLL.Utils.integerNull(r("NUMPIEZASNOK")),
                                                                                                                        .DESCRIPCION = BLL.Utils.stringNull(r("DESCRIPCION")),
                                                                                                                        .PROCEDENCIA = BLL.Utils.stringNull(r("PROCEDENCIA")),
                                                                                                                        .CLASIFICACION = BLL.Utils.stringNull(r("CLASIFICACION")),
                                                                                                                        .REPETITIVA = BLL.Utils.stringNull(r("REPETITIVA")),
                                                                                                                        .OFICIAL = BLL.Utils.stringNull(r("OFICIAL")),
                                                                                                                        .CREADOR_ID = BLL.Utils.integerNull(r("CREADOR_ID")),
                                                                                                                        .CLIENTE_ID = BLL.Utils.integerNull(r("CLIENTE_ID")),
                                                                                                                        .PRODUCTO_ID = BLL.Utils.integerNull(r("PRODUCTO_ID")),
                                                                                                                        .FECHA_CIERRECLIENTE = BLL.Utils.dateTimeNull(r("FECHA_CIERRECLIENTE")),
                                                                                                                        .FECHA_RESP_CONTENCION = BLL.Utils.dateTimeNull(r("FECHA_RESP_CONTENCION")),
                                                                                                                        .FECHA_RESP_CORRECTIVAS = BLL.Utils.dateTimeNull(r("FECHA_RESP_CORRECTIVAS")),
                                                                                                                        .ID = BLL.Utils.integerNull(r("ID")),
                                                                                                                        .AFECTA_INDICADORES = BLL.Utils.integerNull(r("AFECTA_INDICADORES"))
                                                                                                                        }, query, cnStrBezerresis, Nothing)
        Return result
    End Function

    Friend Sub EditObjetivoProceso(obj As OBJETIVOPROCESO)
        Dim query As String = "UPDATE OBJETIVOS_PROCESO SET REPETITIVAS = :REPETITIVAS, DIAS14 = :DIAS14, DIAS56 = :DIAS56 WHERE ID=:ID"
        Dim lParameters As New List(Of OracleParameter)
        lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, obj.ID, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("REPETITIVAS", OracleDbType.Int32, obj.REPETITIVAS, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("DIAS14", OracleDbType.Int32, obj.DIAS14, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("DIAS56", OracleDbType.Int32, obj.DIAS56, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, cnStrBezerresis, lParameters.ToArray)
    End Sub

    Friend Sub CreateObjetivoProceso(obj As OBJETIVOPROCESO)
        Dim query As String = "INSERT INTO OBJETIVOS_PROCESO(FECHA,REPETITIVAS,DIAS14,DIAS56) VALUES(:FECHA,:REPETITIVAS,:DIAS14,:DIAS56)"
        Dim lParameters As New List(Of OracleParameter)
        lParameters.Add(New OracleParameter("FECHA", OracleDbType.Date, New Date(obj.ANNO, 1, 1), ParameterDirection.Input))
        lParameters.Add(New OracleParameter("REPETITIVAS", OracleDbType.Int32, obj.REPETITIVAS, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("DIAS14", OracleDbType.Int32, obj.DIAS14, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("DIAS56", OracleDbType.Int32, obj.DIAS56, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, cnStrBezerresis, lParameters.ToArray)
    End Sub

    Friend Sub deleteObjetivoProceso(id As Integer)
        Dim query As String = "DELETE FROM OBJETIVOS_PROCESO WHERE ID = :ID"
        Memcached.OracleDirectAccess.NoQuery(query, cnStrBezerresis, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
    End Sub

    Friend Function getIndicadoresProceso(fecha_desde As Date?, fecha_hasta As Date?, lClientes_Selected As List(Of Integer), lProductos_Selected As List(Of Integer), idPlanta As String) As List(Of String())
        Dim lParam As New List(Of OracleParameter)
        Dim query As String = "SELECT ANNO,MES,COUNT(*) AS ""TOTAL"",SUM(REPETITIVA),ROUND(AVG(DIAS14),2),ROUND(AVG(DIAS56),2)
                                FROM INDICADORES_PROCESO "
        Dim startDate As Date
        Dim endDate As Date
        startDate = If(fecha_desde, Date.Today.AddMonths(-Configuration.ConfigurationManager.AppSettings("DefaultMonthRange")))
        endDate = If(fecha_hasta, Date.Today.AddDays(-Configuration.ConfigurationManager.AppSettings("CalculusDay")).AddMonths(-1))
        condicionFechaMin(query, lParam, startDate)
        condicionFechaMax(query, lParam, endDate)
        condicionClienteProducto("cliente", lClientes_Selected, query, lParam)
        condicionClienteProducto("producto", lProductos_Selected, query, lParam)
        query &= "GROUP BY ANNO,MES
                  ORDER BY ANNO DESC,MES DESC"
        Dim resultString As New List(Of String())
        resultString = Memcached.OracleDirectAccess.Seleccionar(query, cnStrBezerresis, lParam.ToArray)
        Return resultString.Select(Function(l) l.Select(Function(s) s.Replace(".", ",")).ToArray).ToList
    End Function


#Region "Bezerresis getter helpers"
    Private Function transformResultCalidad(idPlanta As String, startDate As Date, endDate As Date, resultString As List(Of String())) As List(Of String())
        Dim monthCount = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month + 1
        Dim result As New List(Of String())
        Dim c = 0
        Dim itemCounter = 0
        Dim monthValue
        Dim yearValue
        For Each item In resultString
            monthValue = endDate.AddMonths(c).Month
            yearValue = endDate.AddMonths(c).Year
            While Not (item(1).Equals(yearValue.ToString) AndAlso item(2).Equals(monthValue.ToString))
                result.Add(New String() {idPlanta.ToString, yearValue.ToString, monthValue.ToString, "0", "0", "0", "0"})
                c -= 1
                monthValue = endDate.AddMonths(c).Month
                yearValue = endDate.AddMonths(c).Year
            End While
            result.Add(resultString(itemCounter))
            itemCounter += 1
            c -= 1
        Next
        While c + monthCount > 0
            monthValue = endDate.AddMonths(c).Month
            yearValue = endDate.AddMonths(c).Year
            result.Add(New String() {idPlanta.ToString, yearValue.ToString, monthValue.ToString, "0", "0", "0", "0"})
            c -= 1
        End While
        Return result
    End Function

    ''' <summary>
    ''' *v1.1 => no tiene en cuenta la planta
    ''' 
    ''' </summary>
    ''' <param name="startDate"></param>
    ''' <param name="endDate"></param>
    ''' <param name="resultString"></param>
    ''' <returns></returns>
    Private Function transformResultProceso(startDate As Date, endDate As Date, resultString As List(Of String())) As List(Of String())
        Dim monthCount = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month + 1
        Dim result As New List(Of String())
        Dim c = 0
        Dim itemCounter = 0
        Dim monthValue
        Dim yearValue
        For Each item In resultString
            monthValue = endDate.AddMonths(c).Month
            yearValue = endDate.AddMonths(c).Year
            While Not (item(0).Equals(yearValue.ToString) AndAlso item(1).Equals(monthValue.ToString))
                result.Add(New String() {yearValue.ToString, monthValue.ToString, "0", "0", "0", "0", "0", "0"})
                c -= 1
                monthValue = endDate.AddMonths(c).Month
                yearValue = endDate.AddMonths(c).Year
            End While
            result.Add(resultString(itemCounter))
            itemCounter += 1
            c -= 1
        Next
        While c + monthCount > 0
            monthValue = endDate.AddMonths(c).Month
            yearValue = endDate.AddMonths(c).Year
            result.Add(New String() {yearValue.ToString, monthValue.ToString, "0", "0", "0", "0", "0", "0"})
            c -= 1
        End While
        Return result
    End Function

    Private Sub condicionClienteProducto(tipo As String, selectedItems As List(Of Integer), ByRef query As String, ByRef lParam As List(Of OracleParameter))
        Dim conector = ""
        If query.ToUpper.Contains("WHERE") Then
            conector = "AND "
        Else
            conector = "WHERE "
        End If
        Dim columna = tipo.ToUpper()
        Dim parametro = ":" & columna
        If selectedItems.Count = 0 Then
            Dim totalItems As List(Of String) = If(tipo.Equals("cliente"), getTotalClientes(), getTotalProductos())
            query &= conector & columna & " IN ('" & String.Join("','", totalItems) & "') "
        Else
            Dim listaValoresParametros = If(tipo.Equals("cliente"), getClienteStringsFromClienteIds(selectedItems), getProductoStringsFromProductoIds(selectedItems))
            ''''without parametrising:
            query &= conector & columna & " IN ('" & String.Join("','", listaValoresParametros) & "') "

        End If

        ''''parametrised:
        'Dim contador = 0
        'For Each param In listaValoresParametros
        '    query &= ":PARAM" & contador & ","
        '    lParam.Add(New OracleParameter("PARAM", OracleDbType.NVarchar2, param, ParameterDirection.Input))
        '    contador += 1
        'Next
        'Dim lastCommaIndex = query.LastIndexOf(",")
        'query = query.Substring(0, lastCommaIndex)
        'query &= ") "
    End Sub

    Private Sub condicionFechaMin(ByRef query As String, ByRef lParam As List(Of OracleParameter), ByVal value As Date)
        Dim conector = ""
        If query.ToUpper.Contains("WHERE") Then
            conector = "AND"
        Else
            conector = "WHERE"
        End If
        query &= conector & " (ANNO > :ANNOMIN OR (ANNO = :ANNOMIN AND MES >= :MESMIN)) "
        lParam.Add(New OracleParameter("ANNOMIN", OracleDbType.Int32, value.Year, ParameterDirection.Input))
        lParam.Add(New OracleParameter("MESMIN", OracleDbType.Int32, value.Month, ParameterDirection.Input))
    End Sub

    Private Sub condicionFechaMax(ByRef query As String, ByRef lParam As List(Of OracleParameter), ByVal value As Date)
        Dim conector = ""
        If query.ToUpper.Contains("WHERE") Then
            conector = "AND"
        Else
            conector = "WHERE"
        End If
        query &= conector & " (ANNO < :ANNOMAX OR (ANNO = :ANNOMAX AND MES <= :MESMAX)) "
        lParam.Add(New OracleParameter("ANNOMAX", OracleDbType.Int32, value.Year, ParameterDirection.Input))
        lParam.Add(New OracleParameter("MESMAX", OracleDbType.Int32, value.Month, ParameterDirection.Input))
    End Sub
#End Region

    'Friend Function getNumeroPiezasRechazadasAcumulado(fechaInicio As Date, fechaFinal As Date, productos As List(Of Integer), clientes As List(Of Integer)) As Integer
    '    Dim query As String = "select sum(RECHAZADAS) from VENTAS where FECHA <= :ENDDATE and  FECHA  >= :STARTDATE "
    '    Dim lParam As New List(Of OracleParameter)
    '    lParam.Add(New OracleParameter("STARTDATE", OracleDbType.Date, fechaInicio, ParameterDirection.Input))
    '    lParam.Add(New OracleParameter("ENDDATE", OracleDbType.Date, fechaFinal, ParameterDirection.Input))
    '    If productos.Count > 0 Then
    '        query &= "AND IDPRODUCTO IN (" & String.Join(",", productos) & ") "
    '    End If
    '    If clientes.Count > 0 Then
    '        query &= "AND IDCLIENTE IN (" & String.Join(",", clientes) & ") "
    '    End If
    '    Dim numero As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStrBezerresis, lParam.ToArray)
    '    Return numero
    'End Function


    Friend Function getNumeroPiezasRechazadasAcumulado(fechaInicio As Date, fechaFinal As Date, productos As List(Of Integer), clientes As List(Of Integer)) As Integer
        ''''TODO: ESTE ES EL 'REAL' COGIDO DE LA TABLA RECLAMACIONES.
        '''' HABRIA QUE COGERLO DE VENTAS TAL VEZ? AL FINAL NO QUEDÓ CLARO
        Dim query As String = "SELECT NVL(SUM(NUMPIEZASNOK),0) 
                                FROM RECLAMACIONES 
                                where RECLAMACIONOFICIAL=1 and FECHACREACION <= :ENDDATE and  FECHACREACION  >= :STARTDATE AND AFECTA_INDICADORES = 1 "
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("STARTDATE", OracleDbType.Date, fechaInicio, ParameterDirection.Input))
        lParam.Add(New OracleParameter("ENDDATE", OracleDbType.Date, fechaFinal, ParameterDirection.Input))
        If productos.Count > 0 Then
            query &= "AND PRODUCTO IN (" & String.Join(",", productos) & ") "
        End If
        If clientes.Count > 0 Then
            query &= "AND CLIENTE IN (" & String.Join(",", clientes) & ") "
        End If
        Dim numero As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStrBezerresis, lParam.ToArray)
        Return numero
    End Function

    'Friend Function getNumeroIncidenciasAcumulado(fechaInicio As Date, fechaFinal As Date, productos As List(Of Integer), clientes As List(Of Integer)) As Integer
    '    Dim query As String = "select sum(INCIDENCIAS) from VENTAS where FECHA <= :ENDDATE and FECHA >= :STARTDATE " ' and cliente = :CLIENTE AND PRODUCTO = :PRODUCTO"
    '    Dim lParam As New List(Of OracleParameter)
    '    lParam.Add(New OracleParameter("STARTDATE", OracleDbType.Date, fechaInicio, ParameterDirection.Input))
    '    lParam.Add(New OracleParameter("ENDDATE", OracleDbType.Date, fechaFinal, ParameterDirection.Input))
    '    If productos.Count > 0 Then
    '        query &= "AND IDPRODUCTO IN (" & String.Join(",", productos) & ") "
    '    End If
    '    If clientes.Count > 0 Then
    '        query &= "AND IDCLIENTE IN (" & String.Join(",", clientes) & ") "
    '    End If
    '    Dim numero As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStrBezerresis, lParam.ToArray)
    '    Return numero
    'End Function


    Friend Function getNumeroIncidenciasAcumulado(fechaInicio As Date, fechaFinal As Date, productos As List(Of Integer), clientes As List(Of Integer)) As Integer
        Dim query As String = "SELECT COUNT(*)
                                FROM RECLAMACIONES 
                                where RECLAMACIONOFICIAL = 1  and FECHACREACION <= :ENDDATE and FECHACREACION >= :STARTDATE AND AFECTA_INDICADORES = 1 " ' and cliente = :CLIENTE AND PRODUCTO = :PRODUCTO"
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("STARTDATE", OracleDbType.Date, fechaInicio, ParameterDirection.Input))
        lParam.Add(New OracleParameter("ENDDATE", OracleDbType.Date, fechaFinal, ParameterDirection.Input))
        If productos.Count > 0 Then
            query &= "AND PRODUCTO IN (" & String.Join(",", productos) & ") "
        End If
        If clientes.Count > 0 Then
            query &= "AND CLIENTE IN (" & String.Join(",", clientes) & ") "
        End If
        Dim numero As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStrBezerresis, lParam.ToArray)
        Return numero
    End Function

    Friend Function getNumeroIncidenciasProcesoAcumulado(fechaInicio As Date, fechaFinal As Date, productos As List(Of Integer), clientes As List(Of Integer)) As Integer
        Dim query As String = "select count(*) from INDICADORES_PROCESO " ' and cliente = :CLIENTE AND PRODUCTO = :PRODUCTO"
        Dim lParam As New List(Of OracleParameter)
        condicionFechaMin(query, lParam, fechaInicio)
        condicionFechaMax(query, lParam, fechaFinal)
        condicionClienteProducto("cliente", clientes, query, lParam)
        condicionClienteProducto("producto", productos, query, lParam)
        Dim numero As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStrBezerresis, lParam.ToArray)
        Return numero
    End Function

    '''' CORRESPONDENCIA SAB.PLANTAS.ID <---> INCIDENCIAS.GTK.IDTIPOINCIDENCIA  <---> SAB.PLANTAS.IDBRAIN
    '''' Batz Igorre:           1       <--->                9                  <--->       1
    '''' Batz Czech:            6       <--->               11                  <--->       2   
    '''' Batz Kunshan:          4       <--->               12                  <--->       3
    '''' Batz Mexicana:         5       <--->               13                  <--->       4
    '''' Batz Chengdu:         107      <--->               16                  <--->       6
    Private Function getTipoIncidenciaFromIdSabPlantacliente(emp As String) As Integer
        Dim idTipoIncidencia = 0
        Select Case emp
            Case "1"
                idTipoIncidencia = 9
            Case "6"
                idTipoIncidencia = 11
            Case "4"
                idTipoIncidencia = 12
            Case "5"
                idTipoIncidencia = 13
            Case "107"
                idTipoIncidencia = 16
        End Select
        Return idTipoIncidencia
    End Function

    Private Function getTipoIncidenciaFromIdBrainPlantaCliente(plantaCliente As String) As Integer
        Select Case plantaCliente
            Case "1" 'Igorre
                Return 9
            Case "2" 'Chequia
                Return 11
            Case "3" 'Kunshan
                Return 12
            Case "4" 'Mexico
                Return 13
            Case "6" 'Chengdu
                Return 16
        End Select
        Return 0
    End Function

#End Region

#Region "GTK getters"

    Function getCodigoGtk(ByVal idBezerresis As Decimal) As String
        Dim pre As String = getPrefijoGtk(idBezerresis)
        Dim num As String = getNumeroGtk(idBezerresis)
        Return pre & num
    End Function

    Function getCodigoNumericoGtk(ByVal idBezerresis As Decimal) As String
        Return getNumeroGtk(idBezerresis)
    End Function

    Function getNumeroGtk(idBezerresis As Decimal) As String
        Dim query As String = "SELECT ID FROM INCIDENCIAS.GERTAKARIAK WHERE ID_BEZERRESIS = :ID_BEZERRESIS"
        Dim cn As New OracleConnection(cnStrBezerresis)
        Dim cmd As New OracleCommand(query, cn)
        cmd.BindByName = True
        cmd.Parameters.Add(New OracleParameter("ID_BEZERRESIS", OracleDbType.Int32, CInt(idBezerresis), ParameterDirection.Input))
        Dim result As String = "0"
        Dim odr As OracleDataReader = Nothing
        Try
            cn.Open()
            odr = cmd.ExecuteReader()
            If odr.HasRows Then
                If (odr.Read()) Then
                    result = odr(0)
                End If
            End If
        Catch ex As Exception
            Throw
        Finally
            If (odr IsNot Nothing) Then odr.Close() : odr.Dispose()
            cmd.Dispose()
            cn.Close() : cn.Dispose()
        End Try
        Dim id = CInt(result)
        Return If(id > 0, id.ToString, "")
    End Function

    Private Function getPrefijoGtk(idBezerresis As Decimal) As String
        Dim query As String = "SELECT PROCEDENCIANC FROM INCIDENCIAS.GERTAKARIAK WHERE ID_BEZERRESIS = :ID_BEZERRESIS"
        Dim cn As New OracleConnection(cnStrBezerresis)
        Dim cmd As New OracleCommand(query, cn)
        cmd.BindByName = True
        cmd.Parameters.Add(New OracleParameter("ID_BEZERRESIS", OracleDbType.Int32, CInt(idBezerresis), ParameterDirection.Input))
        Dim result As String = "0"
        Dim returningString = ""
        Dim odr As OracleDataReader = Nothing
        Try
            cn.Open()
            odr = cmd.ExecuteReader()
            If odr.HasRows Then
                If (odr.Read()) Then
                    result = odr(0)
                End If
            End If
        Catch ex As Exception
            Throw
        Finally
            If (odr IsNot Nothing) Then odr.Close() : odr.Dispose()
            cmd.Dispose()
            cn.Close() : cn.Dispose()
        End Try
        Dim id = CInt(result)
        Select Case id
            Case 0
                returningString = "-"
            Case 1
                returningString = "NCI-"
            Case 2
                returningString = "NCP-"
            Case 3
                returningString = "NCPP-"
            Case 6
                returningString = "NCC-"
        End Select
        Return returningString
    End Function

    Function getEstructura(ByVal idIturria As Integer) As List(Of SelectListItem)
        Dim query As String = "SELECT * FROM INCIDENCIAS.ESTRUCTURA WHERE IDITURRIA = :IDITURRIA ORDER BY ORDEN"
        Dim cn As New OracleConnection(cnStrBezerresis)
        Dim cmd As New OracleCommand(query, cn)
        cmd.BindByName = True
        cmd.Parameters.Add(New OracleParameter("IDITURRIA", OracleDbType.Int32, idIturria, ParameterDirection.Input))
        Dim lst As New List(Of SelectListItem)
        Dim odr As OracleDataReader = Nothing
        Try
            cn.Open()
            odr = cmd.ExecuteReader()
            If odr.HasRows Then
                While (odr.Read())
                    lst.Add(New SelectListItem With {.Text = odr("DESCRIPCION"), .Value = odr("ID")})
                End While
            End If
        Catch ex As Exception
            Throw
        Finally
            If (odr IsNot Nothing) Then odr.Close() : odr.Dispose()
            cmd.Dispose()
            cn.Close() : cn.Dispose()
        End Try
        Return lst
    End Function

    Function getMyEstructura(ByVal idIturria As Integer, ByVal id As Integer) As String
        Dim query As String = "SELECT DESCRIPCION FROM INCIDENCIAS.ESTRUCTURA WHERE IDITURRIA = :IDITURRIA AND ID=:ID ORDER BY ORDEN"
        Dim cn As New OracleConnection(cnStrBezerresis)
        Dim cmd As New OracleCommand(query, cn)
        cmd.BindByName = True
        cmd.Parameters.Add(New OracleParameter("IDITURRIA", OracleDbType.Int32, idIturria, ParameterDirection.Input))
        cmd.Parameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
        Dim result As String = ""
        Dim odr As OracleDataReader = Nothing
        Try
            cn.Open()
            odr = cmd.ExecuteReader()
            If odr.HasRows Then
                If (odr.Read()) Then
                    result = odr("DESCRIPCION")
                End If
            End If
        Catch ex As Exception
            Throw
        Finally
            If (odr IsNot Nothing) Then odr.Close() : odr.Dispose()
            cmd.Dispose()
            cn.Close() : cn.Dispose()
        End Try
        Return result
    End Function

    Function getNCsToLink(emp As String, id As String) As List(Of Object)
        Dim db As New Entities_BezerreSis
        Dim rec As RECLAMACIONES = db.RECLAMACIONES.Find(CInt(id))

        Dim idTipoIncidencia As Integer = getTipoIncidenciaFromIdSabPlantacliente(emp)
        Dim query As String = "select G.ID,G.DESCRIPCIONPROBLEMA FROM incidencias.GERTAKARIAK G
                                INNER JOIN INCIDENCIAS.G8D G8 ON G.ID = G8.IDGTK
                                INNER JOIN INCIDENCIAS.CARACTERISTICAS C ON C.IDINCIDENCIA = G.ID
                                WHERE G.IDTIPOINCIDENCIA = :IDTIPOINCIDENCIA
                                AND G8.IDREFART = :REFERENCIAINTERNAPIEZA
                                AND G.IDCREADOR = :IDCREADOR
                                AND G.ID_BEZERRESIS IS NULL
                                AND C.IDESTRUCTURA = :IDESTRUCTURA
                                ORDER BY G.ID DESC"
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("IDTIPOINCIDENCIA", OracleDbType.Int32, idTipoIncidencia, ParameterDirection.Input))
        lParam.Add(New OracleParameter("REFERENCIAINTERNAPIEZA", OracleDbType.NVarchar2, rec.REFINTERNAPIEZA, ParameterDirection.Input))
        lParam.Add(New OracleParameter("IDCREADOR", OracleDbType.Int32, rec.CREADOR, ParameterDirection.Input))
        lParam.Add(New OracleParameter("IDESTRUCTURA", OracleDbType.Int32, CInt(Configuration.ConfigurationManager.AppSettings("idDeteccionCliente")), ParameterDirection.Input))
        Dim result As List(Of Object) = Memcached.OracleDirectAccess.seleccionar(Of Object)(Function(r) New With {.id = r(0), .label = r(1)}, query, cnStrBezerresis, lParam.ToArray)
        Return result
    End Function

    Function getPerseguidor(idGtk As Integer) As String
        Dim query As String = "SELECT UPPER(NOMBRE || ' ' || APELLIDO1 || ' ' || APELLIDO2) FROM SAB.USUARIOS U
                               INNER JOIN INCIDENCIAS.RESPONSABLES_GERTAKARIAK RG ON U.ID=RG.IDUSUARIO 
                               WHERE RG.IDINCIDENCIA = :IDGTK"
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("IDGTK", OracleDbType.Int32, idGtk, ParameterDirection.Input))
        Dim result As List(Of String) = Memcached.OracleDirectAccess.seleccionar(Of String)(Function(r) r(0), query, cnStrBezerresis, lParam.ToArray)
        Return String.Join(",<br /> ", result)
    End Function

    Function getResponsable(idGtk As Integer) As String
        Dim query As String = "SELECT UPPER(NOMBRE || ' ' || APELLIDO1 || ' ' || APELLIDO2) FROM SAB.USUARIOS U
                               INNER JOIN INCIDENCIAS.EQUIPORESOLUCION E ON U.ID=E.IDASISTENTE 
                               WHERE E.IDINCIDENCIA = :IDGTK"
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("IDGTK", OracleDbType.Int32, idGtk, ParameterDirection.Input))
        Dim result As List(Of String) = Memcached.OracleDirectAccess.seleccionar(Of String)(Function(r) r(0), query, cnStrBezerresis, lParam.ToArray)
        Return String.Join(",<br /> ", result)
    End Function

    Function getFechaFinPrevisto(idGtk As Integer) As String
        Dim query As String = "SELECT FECHAFIN FROM INCIDENCIAS.G8D_E56 GE
                               INNER JOIN INCIDENCIAS.G8D G ON G.ID_E56 = GE.ID
                               WHERE G.IDGTK = :IDGTK"
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("IDGTK", OracleDbType.Int32, idGtk, ParameterDirection.Input))
        Dim result As String = Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query, cnStrBezerresis, lParam.ToArray)
        Return IIf(String.IsNullOrWhiteSpace(result), "?", result)
    End Function

    Function isFechaFinOutdated(idGtk As Integer, fechaFinString As String) As Boolean
        Dim query As String = "SELECT FECHACIERRE FROM INCIDENCIAS.G8D_E56 GE
                               INNER JOIN INCIDENCIAS.G8D G ON G.ID_E56 = GE.ID
                               WHERE G.IDGTK = :IDGTK"
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("IDGTK", OracleDbType.Int32, idGtk, ParameterDirection.Input))
        Dim fechaCierreString = Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query, cnStrBezerresis, lParam.ToArray)
        Dim fechaFin = dateTimeNull(fechaFinString)
        Dim fechaCierre = dateTimeNull(fechaCierreString)
        Return (fechaCierre = DateTime.MinValue AndAlso fechaFin < Now.Date) OrElse fechaFin < fechaCierre
    End Function

    Public Function dateTimeNull(ByVal o As Object) As DateTime
        Dim dtResul As DateTime = DateTime.MinValue
        If Not (o Is Nothing Or o Is DBNull.Value) Then
            dtResul = CType(o.ToString(), DateTime)
        End If
        Return dtResul
    End Function
#End Region

#Region "Cliente/Producto getters"

    Function getClienteStringsFromClienteIds(lClientes_Selected As List(Of Integer)) As List(Of String) ''''without parametrising
        Dim query As String = "Select NOMBRE FROM CLIENTES WHERE ID In (" & String.Join(",", lClientes_Selected) & ")"
        Return Memcached.OracleDirectAccess.seleccionar(Of String)(Function(r As OracleDataReader) r("NOMBRE"), query, cnStrBezerresis, Nothing)
    End Function

    Function getProductoStringsFromProductoIds(lProductos_Selected As List(Of Integer)) As List(Of String) ''''without parametrising
        Dim query As String = "Select NOMBRE FROM PRODUCTOS WHERE ID In (" & String.Join(",", lProductos_Selected) & ")"
        Return Memcached.OracleDirectAccess.seleccionar(Of String)(Function(r As OracleDataReader) r("NOMBRE"), query, cnStrBezerresis, Nothing)
    End Function

    Function getProductosForCliente(input1 As String, input2 As String) As List(Of SelectListItem)
        Dim lst As New List(Of SelectListItem)
        If input1.Trim.Equals("") Then
            Return lst
        End If
        Dim query As String = "Select P.ID,P.NOMBRE FROM PRODUCTOS P
                                INNER JOIN CLIPRO CP ON P.ID = CP.IDPRO
                                WHERE CP.IDCLI = :IDCLI"
        Dim cn As New OracleConnection(cnStrBezerresis)
        Dim cmd As New OracleCommand(query, cn)
        cmd.BindByName = True
        cmd.Parameters.Add(New OracleParameter("IDCLI", OracleDbType.Int32, CInt(input1), ParameterDirection.Input))
        Dim result As String = "0"
        Dim odr As OracleDataReader = Nothing
        Try
            cn.Open()
            odr = cmd.ExecuteReader()
            If odr.HasRows Then
                While (odr.Read())
                    lst.Add(New SelectListItem With {.Text = odr(1), .Value = odr(0)})
                End While
            End If
        Catch ex As Exception
            Throw
        Finally
            If (odr IsNot Nothing) Then odr.Close() : odr.Dispose()
            cmd.Dispose()
            cn.Close() : cn.Dispose()
        End Try
        Return lst
    End Function

    Function getProductosForPlantaLogueada(idPlantaLogueada As String) As List(Of SelectListItem)
        Dim lst As New List(Of SelectListItem)
        Dim query As String = "SELECT P.ID,P.NOMBRE FROM PRODUCTOS P
                                WHERE IDPLANTA = :IDPLANTA"
        Dim cn As New OracleConnection(cnStrBezerresis)
        Dim cmd As New OracleCommand(query, cn)
        cmd.BindByName = True
        cmd.Parameters.Add(New OracleParameter("IDPLANTA", OracleDbType.Int32, CInt(idPlantaLogueada), ParameterDirection.Input))
        Dim result As String = "0"
        Dim odr As OracleDataReader = Nothing
        Try
            cn.Open()
            odr = cmd.ExecuteReader()
            If odr.HasRows Then
                While (odr.Read())
                    lst.Add(New SelectListItem With {.Text = odr(1), .Value = odr(0)})
                End While
            End If
        Catch ex As Exception
            Throw
        Finally
            If (odr IsNot Nothing) Then odr.Close() : odr.Dispose()
            cmd.Dispose()
            cn.Close() : cn.Dispose()
        End Try
        Return lst
    End Function

    Friend Function getTotalProductos() As List(Of String)
        Dim query As String = "select NOMBRE FROM PRODUCTOS"
        Dim lst As New List(Of String)
        Dim cn As New OracleConnection(cnStrBezerresis)
        Dim cmd As New OracleCommand(query, cn)
        Dim odr As OracleDataReader = Nothing
        Try
            cn.Open()
            odr = cmd.ExecuteReader()
            If odr.HasRows Then
                While (odr.Read())
                    lst.Add(odr(0))
                End While
            End If
        Catch ex As Exception
            Throw
        Finally
            If (odr IsNot Nothing) Then odr.Close() : odr.Dispose()
            cmd.Dispose()
            cn.Close() : cn.Dispose()
        End Try
        Return lst
    End Function

    Friend Function getProductoIdFromName(productoName As String, idplanta As String) As String
        Dim query As String = "SELECT ID FROM PRODUCTOS WHERE IDPLANTA=:IDPLANTA AND NOMBRE=:NOMBRE"
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("IDPLANTA", OracleDbType.Int32, CInt(idplanta), ParameterDirection.Input))
        lParam.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, productoName, ParameterDirection.Input))
        Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cnStrBezerresis, lParam.ToArray)
    End Function

    Friend Function getTotalClientes() As List(Of String)
        Dim query As String = "select NOMBRE FROM CLIENTES"
        Dim lst As New List(Of String)
        Dim cn As New OracleConnection(cnStrBezerresis)
        Dim cmd As New OracleCommand(query, cn)
        Dim odr As OracleDataReader = Nothing
        Try
            cn.Open()
            odr = cmd.ExecuteReader()
            If odr.HasRows Then
                While (odr.Read())
                    lst.Add(odr(0))
                End While
            End If
        Catch ex As Exception
            Throw
        Finally
            If (odr IsNot Nothing) Then odr.Close() : odr.Dispose()
            cmd.Dispose()
            cn.Close() : cn.Dispose()
        End Try
        Return lst
    End Function
#End Region

#Region "update"

    Friend Sub copiarDatosVinculacion(gtkId As Integer, bezerresisId As Integer, con As OracleConnection)
        Dim db As New Entities_BezerreSis
        Dim rec As RECLAMACIONES = db.RECLAMACIONES.Find(bezerresisId)

        updateInGertakariakByIdGTK(gtkId, rec.DESCRIPCION, rec.PROCEDENCIA, rec.FECHACREACION, con)
        updateInG8D(rec.NUMPIEZASNOK, rec.REFINTERNAPIEZA, gtkId, con)
        updateCaracteristicaConcreta(rec.NIVELIMPORTANCIA, CInt(Configuration.ConfigurationManager.AppSettings("nivelImportanciaId")), gtkId, con)
        updateCaracteristicaConcreta(rec.REPETITIVA, CInt(Configuration.ConfigurationManager.AppSettings("repetitivaId")), gtkId, con)

    End Sub

    Friend Function updateInGTK(rec As RECLAMACIONES, con As OracleConnection) As String
        Dim idGtk As Integer = updateInGertakariakByIdBezerresis(rec, con)
        If idGtk = 0 Then
            Return " - "
        End If
        updateInG8D(rec.NUMPIEZASNOK, rec.REFINTERNAPIEZA, idGtk, con)
        updateCaracteristicaConcreta(rec.NIVELIMPORTANCIA, CInt(Configuration.ConfigurationManager.AppSettings("nivelImportanciaId")), idGtk, con)
        updateCaracteristicaConcreta(rec.REPETITIVA, CInt(Configuration.ConfigurationManager.AppSettings("repetitivaId")), idGtk, con)
        Return idGtk.ToString
    End Function

    Private Function updateInGertakariakByIdBezerresis(rec As RECLAMACIONES, con As OracleConnection) As Integer
        Dim query As String = "UPDATE INCIDENCIAS.GERTAKARIAK SET DESCRIPCIONPROBLEMA=:DESCRIPCIONPROBLEMA, PROCEDENCIANC=:PROCEDENCIANC WHERE ID_BEZERRESIS=:ID_BEZERRESIS RETURNING ID INTO :RESUL"
        Dim lParameters As New List(Of OracleParameter)
        lParameters.Add(New OracleParameter("RESUL", OracleDbType.Int32, ParameterDirection.ReturnValue))
        lParameters.Add(New OracleParameter("ID_BEZERRESIS", OracleDbType.Int32, rec.ID, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("DESCRIPCIONPROBLEMA", OracleDbType.NVarchar2, rec.DESCRIPCION, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("PROCEDENCIANC", OracleDbType.Int32, rec.PROCEDENCIA, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
        Dim temp = lParameters.Item(0).Value
        If temp.IsNull Then
            Return 0
        Else
            Return CInt(lParameters.Item(0).Value.Value)
        End If
    End Function

    Private Sub updateInGertakariakByIdGTK(idGtk As Integer, descripcion As String, procedencia As Integer, fechaCreacion As Date, con As OracleConnection)
        Dim query As String = "UPDATE INCIDENCIAS.GERTAKARIAK SET DESCRIPCIONPROBLEMA=:DESCRIPCIONPROBLEMA, PROCEDENCIANC=:PROCEDENCIANC, FECHAAPERTURA=:FECHAAPERTURA WHERE ID=:ID_GTK"
        Dim lParameters As New List(Of OracleParameter)
        lParameters.Add(New OracleParameter("ID_GTK", OracleDbType.Int32, idGtk, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("DESCRIPCIONPROBLEMA", OracleDbType.NVarchar2, descripcion, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("PROCEDENCIANC", OracleDbType.Int32, procedencia, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("FECHAAPERTURA", OracleDbType.Date, fechaCreacion, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
    End Sub

    Private Sub updateInG8D(numPiezasNok As Integer, idRefart As String, idGtk As Integer, con As OracleConnection)
        Dim query As String = "UPDATE INCIDENCIAS.G8D SET NUMPIEZAS=:NUMPIEZAS, IDREFART=:IDREFART WHERE IDGTK=:IDGTK"
        Dim lParameters As New List(Of OracleParameter)
        lParameters.Add(New OracleParameter("NUMPIEZAS", OracleDbType.Int32, numPiezasNok, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("IDREFART", OracleDbType.NVarchar2, idRefart, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("IDGTK", OracleDbType.NVarchar2, idGtk, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
    End Sub

    Private Sub updateCaracteristicaConcreta(newValue As Integer, oldValueParent As Integer, idGtk As Integer, con As OracleConnection)
        Dim query As String = "UPDATE INCIDENCIAS.CARACTERISTICAS SET IDESTRUCTURA=:NEWESTRUCTURA 
                                WHERE IDINCIDENCIA=:IDINCIDENCIA AND IDESTRUCTURA IN (
                                    SELECT DISTINCT C.IDESTRUCTURA
                                    FROM INCIDENCIAS.CARACTERISTICAS C
                                    INNER JOIN INCIDENCIAS.ESTRUCTURA E
                                    ON C.IDESTRUCTURA = E.ID
                                    WHERE E.IDITURRIA=:IDITURRIA)"
        Dim lParameters As New List(Of OracleParameter)
        lParameters.Add(New OracleParameter("IDINCIDENCIA", OracleDbType.Int32, idGtk, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("NEWESTRUCTURA", OracleDbType.Int32, newValue, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("IDITURRIA", OracleDbType.Int32, oldValueParent, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
    End Sub

    Friend Sub vincularIncidencia(gtkId As String, bezerresisId As String, con As OracleConnection)
        Dim query As String = "UPDATE INCIDENCIAS.GERTAKARIAK SET ID_BEZERRESIS = :ID_BEZERRESIS WHERE ID = :ID"
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("ID_BEZERRESIS", OracleDbType.Int32, CInt(bezerresisId), ParameterDirection.Input))
        lParam.Add(New OracleParameter("ID", OracleDbType.Int32, CInt(gtkId), ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, con, lParam.ToArray)
    End Sub

    Friend Sub desvincularIncidencia(bezerresisId As String)
        Dim query As String = "UPDATE INCIDENCIAS.GERTAKARIAK SET ID_BEZERRESIS = NULL WHERE ID_BEZERRESIS = :ID_BEZERRESIS"
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("ID_BEZERRESIS", OracleDbType.Int32, CInt(bezerresisId), ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, cnStrBezerresis, lParam.ToArray)
    End Sub
#End Region

#Region "create"
    Friend Function crearEnGtk(rec As RECLAMACIONES, idBezerresis As Integer, con As OracleConnection) As Integer
        Dim idGtk As Integer = createInGertakariak(rec, idBezerresis, con)
        createInG8D(rec, idGtk, con)
        createInCaracteristicas(rec.NIVELIMPORTANCIA, idGtk, con)
        createInCaracteristicas(rec.REPETITIVA, idGtk, con)
        createInCaracteristicas(CInt(Configuration.ConfigurationManager.AppSettings("idDeteccionCliente")), idGtk, con) ''''idDetecciónCliente = 688
        Return idGtk
    End Function

    Private Function createInGertakariak(rec As RECLAMACIONES, idBezerresis As Integer, con As OracleConnection) As Integer
        Dim resultId As Integer = Integer.MinValue
        Dim query As String = "Insert into INCIDENCIAS.GERTAKARIAK (ID,ID_BEZERRESIS,IDTIPOINCIDENCIA,DESCRIPCIONPROBLEMA,FECHAAPERTURA,PROCEDENCIANC,IDCREADOR,COMPENSADO,IDRECEPCION,IDPROVEEDOR) " _
                            & "VALUES (:ID,:ID_BEZERRESIS,:IDTIPOINCIDENCIA,:DESCRIPCIONPROBLEMA,:FECHAAPERTURA,:PROCEDENCIANC,:IDCREADOR,:COMPENSADO,:IDRECEPCION,:CLIENTE) " _
                            & "RETURNING ID INTO :RESUL"
        Dim lParameters As New List(Of OracleParameter)
        lParameters.Add(New OracleParameter("RESUL", OracleDbType.Int32, ParameterDirection.ReturnValue))
        lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("ID_BEZERRESIS", OracleDbType.Int32, idBezerresis, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("IDTIPOINCIDENCIA", OracleDbType.NVarchar2, getTipoIncidenciaFromIdBrainPlantaCliente(rec.PLANTACLIENTE), ParameterDirection.Input))
        lParameters.Add(New OracleParameter("DESCRIPCIONPROBLEMA", OracleDbType.NVarchar2, rec.DESCRIPCION, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("FECHAAPERTURA", OracleDbType.Date, rec.FECHACREACION, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("PROCEDENCIANC", OracleDbType.NVarchar2, rec.PROCEDENCIA, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("IDCREADOR", OracleDbType.NVarchar2, rec.CREADOR, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("COMPENSADO", OracleDbType.NVarchar2, 0, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("IDRECEPCION", OracleDbType.Int32, 0, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("CLIENTE", OracleDbType.Int32, rec.CLIENTE, ParameterDirection.Input)) ''''IDPROVEEDOR
        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
        resultId = CInt(lParameters.Item(0).Value.Value)
        Return resultId
    End Function

    Private Sub createInG8D(rec As RECLAMACIONES, igGtk As Integer, con As OracleConnection)
        Dim query As String = "Insert into INCIDENCIAS.G8D (ID,IDGTK,IDREFART,NUMPIEZAS) " _
                            & "VALUES (:ID,:IDGTK,:REFINTERNAPIEZA,:NUMPIEZASNOK)"
        Dim lParameters As New List(Of OracleParameter)
        lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("IDGTK", OracleDbType.Int32, igGtk, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("REFINTERNAPIEZA", OracleDbType.NVarchar2, rec.REFINTERNAPIEZA, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("NUMPIEZASNOK", OracleDbType.Int32, rec.NUMPIEZASNOK, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
    End Sub

    Private Sub createInCaracteristicas(idEstructura As Integer, idGtk As Integer, con As OracleConnection)
        Dim query As String = "Insert into INCIDENCIAS.CARACTERISTICAS (IDINCIDENCIA,IDESTRUCTURA) " _
                            & "VALUES (:IDINCIDENCIA,:IDESTRUCTURA)"
        Dim lParameters As New List(Of OracleParameter)
        lParameters.Add(New OracleParameter("IDINCIDENCIA", OracleDbType.Int32, idGtk, ParameterDirection.Input))
        lParameters.Add(New OracleParameter("IDESTRUCTURA", OracleDbType.Int32, idEstructura, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
    End Sub


    Public Sub ActualizarIncidenciasYRechazadas()
        Dim query = "UPDATE VENTAS v
                        SET INCIDENCIAS = (SELECT COUNT(*) FROM RECLAMACIONES 
				                WHERE TO_CHAR(FECHACREACION, 'mmyyyy') LIKE CONCAT(LPAD(v.mes,2,'0'),v.anno) 
				                AND RECLAMACIONOFICIAL = 1 
                                AND AFECTA_INDICADORES = 1
				                AND CLIENTE = (SELECT ID FROM CLIENTES WHERE NOMBRE=v.cliente AND IDPLANTA=v.idplanta)
				                AND PRODUCTO = (SELECT ID FROM PRODUCTOS WHERE NOMBRE=v.producto AND IDPLANTA=v.idplanta))
                        , RECHAZADAS = (SELECT NVL(SUM(NUMPIEZASNOK),0) 
				                FROM RECLAMACIONES 
				                WHERE TO_CHAR(FECHACREACION, 'mmyyyy') LIKE CONCAT(LPAD(v.mes,2,'0'),v.anno) 
				                AND RECLAMACIONOFICIAL=1 
                                AND AFECTA_INDICADORES = 1
				                AND CLIENTE=(SELECT ID FROM CLIENTES WHERE NOMBRE=v.cliente AND IDPLANTA=v.idplanta)
				                AND PRODUCTO=(SELECT ID FROM PRODUCTOS WHERE NOMBRE=v.producto AND IDPLANTA=v.idplanta))"
        Memcached.OracleDirectAccess.NoQuery(query, cnStrBezerresis, Nothing)
    End Sub

#End Region


    Public Sub cerrarEnGTK(id As Decimal, fechaCierre As Date?)
        cambiarFechaCierreEnGTK(id, fechaCierre)
    End Sub

    Public Sub abrirEnGTK(id As Decimal)
        cambiarFechaCierreEnGTK(id, Nothing)
    End Sub

    Public Sub cambiarFechaCierreEnGTK(id As Decimal, fechaCierre As Date?)
        Dim query As String = "UPDATE INCIDENCIAS.GERTAKARIAK SET FECHACIERRE=:FECHACIERRE WHERE ID_BEZERRESIS=:ID"
        Dim lParam As New List(Of OracleParameter)
        lParam.Add(New OracleParameter("FECHACIERRE", OracleDbType.Date, If(fechaCierre, DBNull.Value), ParameterDirection.Input))
        lParam.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, System.Configuration.ConfigurationManager.ConnectionStrings("BEZERRESIS").ConnectionString, lParam.ToArray)
    End Sub


End Class
