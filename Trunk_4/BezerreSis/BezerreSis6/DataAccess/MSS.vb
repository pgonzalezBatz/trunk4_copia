Imports System.Data.SqlClient

Public Class MSS
    'Friend Function getIndicadores(fecha_desde As Date?, fecha_hasta As Date?, lClientes_Selected As Integer?, lProductos_Selected As Integer?, idPlanta As String) As List(Of Object)
    '    Dim myDb As New myDb
    '    Dim oracleDb As New oracleDB
    '    Dim result As New List(Of Object)
    '    Dim lParam As New List(Of SqlParameter)
    '    Dim query As String = "SELECT [EMP],[ANNO],[MES],[NEGOCIO],[SUBGRUPO CLIENTE],sum([CANTIDAD REAL]),' ',' ',' ' FROM [supply].[dbo].[VENTAS_CSV]
    '                            where emp=@IDPLANTA "
    '    lParam.Add(New SqlParameter("@IDPLANTA", idPlanta))
    '    Dim annoMin As Integer = 2010
    '    Dim mesMin As Integer = 1
    '    Dim annoMax As Integer = Date.Today.Year
    '    Dim mesMax As Integer = Date.Today.Month
    '    If fecha_desde IsNot Nothing Then
    '        query &= "AND (ANNO > @ANNOMIN OR (ANNO = @ANNOMIN AND MES >= @MESMIN)) "
    '        lParam.Add(New SqlParameter("@ANNOMIN", "" & fecha_desde.Value.Year & ""))
    '        lParam.Add(New SqlParameter("@MESMIN", "" & fecha_desde.Value.Month & ""))
    '    Else
    '        query &= "AND (ANNO > @ANNOMIN OR (ANNO = @ANNOMIN AND MES >= @MESMIN)) "
    '        lParam.Add(New SqlParameter("@ANNOMIN", "" & Date.Today.AddMonths(-Configuration.ConfigurationManager.AppSettings("DefaultMonthRange")).Year & ""))
    '        lParam.Add(New SqlParameter("@MESMIN", "" & Date.Today.AddMonths(-Configuration.ConfigurationManager.AppSettings("DefaultMonthRange")).Month & ""))
    '    End If
    '    If fecha_hasta IsNot Nothing Then
    '        query &= "AND (ANNO < @ANNOMAX OR (ANNO = @ANNOMAX AND MES <= @MESMAX)) "
    '        lParam.Add(New SqlParameter("@ANNOMAX", "" & fecha_hasta.Value.Year & ""))
    '        lParam.Add(New SqlParameter("@MESMAX", "" & fecha_hasta.Value.Month & ""))
    '    Else
    '        query &= "AND (ANNO < @ANNOMAX OR (ANNO = @ANNOMAX AND MES <= @MESMAX)) "
    '        lParam.Add(New SqlParameter("@ANNOMAX", "" & Date.Today.AddDays(-Configuration.ConfigurationManager.AppSettings("CalculusDay")).AddMonths(-1).Year & ""))
    '        lParam.Add(New SqlParameter("@MESMAX", "" & Date.Today.AddDays(-Configuration.ConfigurationManager.AppSettings("CalculusDay")).AddMonths(-1).Month & ""))
    '    End If
    '    Dim clientName As String = ""
    '    If lClientes_Selected IsNot Nothing Then
    '        Dim clienteString As String = oracleDb.getClienteStringFromClienteId(lClientes_Selected) '''' si fuera una relación 1:N o N:N, esto tendría que ser un bucle
    '        query &= "AND [SUBGRUPO CLIENTE] LIKE @CLIENTE "
    '        lParam.Add(New SqlParameter("@CLIENTE", clienteString))
    '    End If
    '    Dim productName As String = ""
    '    If lProductos_Selected IsNot Nothing Then
    '        Dim productoString As String = oracleDb.getProductoStringFromProductoId(lProductos_Selected)
    '        query &= "AND [NEGOCIO] LIKE @PRODUCTO "
    '        lParam.Add(New SqlParameter("@PRODUCTO", productoString))
    '    End If
    '    query &= " group by [EMP],[ANNO],[MES],[NEGOCIO],[SUBGRUPO CLIENTE]
    '               order by [ANNO] DESC,[MES] DESC"
    '    Dim resultString As List(Of String()) = Memcached.SQLServerDirectAccess.Seleccionar(query, Configuration.ConfigurationManager.ConnectionStrings("CN_SUPPLY").ConnectionString, lParam.ToArray)
    '    result = resultString.Select(Function(f) CType(f, Object)).ToList
    '    Return result
    'End Function

    Friend Function getIndicadoresCalidad(fecha_desde As Date?, fecha_hasta As Date?, lClientes_Selected As Integer?, lProductos_Selected As Integer?, idPlanta As String) As List(Of Object)
        Dim myDb As New myDb
        Dim oracleDb As New oracleDB
        Dim result As New List(Of Object)
        Dim lParam As New List(Of SqlParameter)
        Dim query As String = "select T.[EMP],T.[ANNO],T.[MES],sum(T.suma),'','','' FROM (
                                    SELECT [EMP],[ANNO],[MES],[NEGOCIO],[SUBGRUPO CLIENTE],sum([CANTIDAD REAL]) as suma FROM [supply].[dbo].[VENTAS_CSV]
                                where emp=@IDPLANTA "
        lParam.Add(New SqlParameter("@IDPLANTA", idPlanta))
        Dim annoMin As Integer = 2010
        Dim mesMin As Integer = 1
        Dim annoMax As Integer = Date.Today.Year
        Dim mesMax As Integer = Date.Today.Month
        If fecha_desde IsNot Nothing Then
            condicionFechaMin(query, lParam, fecha_desde.Value)
        Else
            condicionFechaMin(query, lParam, Date.Today.AddMonths(-Configuration.ConfigurationManager.AppSettings("DefaultMonthRange")))
        End If
        If fecha_hasta IsNot Nothing Then
            condicionFechaMax(query, lParam, fecha_hasta.Value)
        Else
            condicionFechaMax(query, lParam, Date.Today.AddDays(-Configuration.ConfigurationManager.AppSettings("CalculusDay")).AddMonths(-1))
        End If
        Dim clientName As String = ""

        condicionClienteProducto("cliente", lClientes_Selected, query, lParam)
        condicionClienteProducto("producto", lProductos_Selected, query, lParam)

        query &= " group by [EMP],[ANNO],[MES],[NEGOCIO],[SUBGRUPO CLIENTE]) as T
                  GROUP BY T.[EMP],T.[ANNO],T.[MES]
                  order by [ANNO] DESC,[MES] DESC"
        Dim resultString As List(Of String()) = Memcached.SQLServerDirectAccess.Seleccionar(query, Configuration.ConfigurationManager.ConnectionStrings("CN_SUPPLY").ConnectionString, lParam.ToArray)
        result = resultString.Select(Function(f) CType(f, Object)).ToList
        If result.Count = 0 Then
            Dim monthCount = (fecha_hasta.Value.Year - fecha_desde.Value.Year) * 12 + fecha_hasta.Value.Month - fecha_desde.Value.Month + 1
            Dim c = 0
            While c < monthCount
                result.Add(New String() {idPlanta, fecha_desde.Value.AddMonths(c).Year, fecha_desde.Value.AddMonths(c).Month, "0", "0", "0", "0"})
                c += 1
            End While
        End If
        Return result
    End Function

    Private Sub condicionFechaMin(ByRef query As String, ByRef lParam As List(Of SqlParameter), ByVal value As Date)
        query &= "AND (ANNO > @ANNOMIN OR (ANNO = @ANNOMIN AND MES >= @MESMIN)) "
        lParam.Add(New SqlParameter("@ANNOMIN", "" & value.Year & ""))
        lParam.Add(New SqlParameter("@MESMIN", "" & value.Month & ""))
    End Sub

    Private Sub condicionFechaMax(ByRef query As String, ByRef lParam As List(Of SqlParameter), ByVal value As Date)
        query &= "AND (ANNO < @ANNOMAX OR (ANNO = @ANNOMAX AND MES <= @MESMAX)) "
        lParam.Add(New SqlParameter("@ANNOMAX", "" & value.Year & ""))
        lParam.Add(New SqlParameter("@MESMAX", "" & value.Month & ""))
    End Sub

    Private Sub condicionClienteProducto(tipo As String, selectedItems As Integer?, ByRef query As String, ByRef lParam As List(Of SqlParameter))
        Dim oracleDb As New oracleDB
        Dim columna = If(tipo.Equals("cliente"), "[SUBGRUPO CLIENTE]", "[NEGOCIO]")
        Dim parametro = If(tipo.Equals("cliente"), "@CLIENTE", "@PRODUCTO")
        If selectedItems IsNot Nothing Then
            Dim valorParametro As String = If(tipo.Equals("cliente"), oracleDb.getClienteStringFromClienteId(selectedItems), oracleDb.getProductoStringFromProductoId(selectedItems))
            query &= "AND " & columna & " LIKE " & parametro & " "
            lParam.Add(New SqlParameter(parametro, valorParametro))
        Else
            Dim totalItems As List(Of String) = If(tipo.Equals("cliente"), oracleDb.getTotalClientes(), oracleDb.getTotalProductos())
            query &= "AND " & columna & " IN ("
            Dim contador = 0
            For Each item In totalItems
                query &= parametro & contador.ToString & ","
                lParam.Add(New SqlParameter(parametro & contador.ToString, item))
                contador += 1
            Next
            Dim lastCommaIndex = query.LastIndexOf(",")
            query = query.Substring(0, lastCommaIndex)
            query &= ") "
        End If
    End Sub

    'Friend Function getSumaPiezasVendidasAcumuladas(monthYearDate As String, lProductos_Selected As Integer?, lClientes_Selected As Integer?, idPlanta As String, mesesAcumulados As Integer) As Object
    '    System.Diagnostics.Debug.WriteLine("    cálculo directo en SQL Server")
    '    Dim myDb As New myDb
    '    Dim oracleDb As New oracleDB
    '    Dim lParam As New List(Of SqlParameter)
    '    Dim query As String = "select sum([CANTIDAD REAL]) as suma FROM [supply].[dbo].[VENTAS_CSV]
    '                            where emp=@IDPLANTA "
    '    lParam.Add(New SqlParameter("@IDPLANTA", idPlanta))
    '    Dim fechaFinal = DateTime.ParseExact(monthYearDate, "MMyyyy", Globalization.CultureInfo.InvariantCulture)
    '    Dim fechaInicio = fechaFinal.AddMonths(mesesAcumulados)
    '    Dim monthYearStartDate = fechaInicio.ToString("MMyyyy")
    '    query &= "AND (ANNO > @ANNOMIN OR (ANNO = @ANNOMIN AND MES > @MESMIN)) AND (ANNO < @ANNOMAX OR (ANNO = @ANNOMAX AND MES <= @MESMAX)) "
    '    lParam.Add(New SqlParameter("@ANNOMIN", "" & monthYearStartDate.Substring(2, 4) & ""))
    '    lParam.Add(New SqlParameter("@MESMIN", "" & monthYearStartDate.Substring(0, 2) & ""))
    '    lParam.Add(New SqlParameter("@ANNOMAX", "" & monthYearDate.Substring(2, 4) & ""))
    '    lParam.Add(New SqlParameter("@MESMAX", "" & monthYearDate.Substring(0, 2) & ""))
    '    Dim clientName As String = ""
    '    If lClientes_Selected IsNot Nothing Then
    '        Dim clienteString As String = oracleDb.getClienteStringFromClienteId(lClientes_Selected) '''' si fuera una relación 1:N o N:N, esto tendría que ser un bucle
    '        query &= "AND [SUBGRUPO CLIENTE] LIKE @CLIENTE "
    '        lParam.Add(New SqlParameter("@CLIENTE", clienteString))
    '    Else
    '        Dim totalClientes As List(Of String) = oracleDb.getTotalClientes()
    '        query &= "AND [SUBGRUPO CLIENTE] IN ("
    '        Dim clienteCounter = 0
    '        For Each cliente In totalClientes
    '            query &= "@CLIENTE" & clienteCounter.ToString & ","
    '            lParam.Add(New SqlParameter("@CLIENTE" & clienteCounter.ToString, cliente))
    '            clienteCounter += 1
    '        Next
    '        Dim lastCommaIndex = query.LastIndexOf(",")
    '        query = query.Substring(0, lastCommaIndex)
    '        query &= ") "
    '    End If
    '    Dim productName As String = ""
    '    If lProductos_Selected IsNot Nothing Then
    '        Dim productoString As String = oracleDb.getProductoStringFromProductoId(lProductos_Selected)
    '        query &= "AND [NEGOCIO] LIKE @PRODUCTO "
    '        lParam.Add(New SqlParameter("@PRODUCTO", productoString))
    '    Else
    '        Dim totalProductos As List(Of String) = oracleDb.getTotalProductos()
    '        query &= "AND [NEGOCIO] IN ("
    '        Dim productoCounter = 0
    '        For Each producto In totalProductos
    '            query &= "@PRODUCTO" & productoCounter.ToString & ","
    '            lParam.Add(New SqlParameter("@PRODUCTO" & productoCounter.ToString, producto))
    '            productoCounter += 1
    '        Next
    '        Dim lastCommaIndex = query.LastIndexOf(",")
    '        query = query.Substring(0, lastCommaIndex)
    '        query &= ") "
    '    End If
    '    ''''query &= " group by [EMP],[ANNO],[MES],[NEGOCIO],[SUBGRUPO CLIENTE]"
    '    Dim result As String = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of String)(query, Configuration.ConfigurationManager.ConnectionStrings("CN_SUPPLY").ConnectionString, lParam.ToArray)
    '    Return result
    'End Function


    'Friend Function getSumaPiezasVendidasAcumuladas2(monthYearDate As String, lProductos_Selected As Integer?, lClientes_Selected As Integer?, idPlanta As String, mesesAcumulados As Integer) As Object
    '    System.Diagnostics.Debug.WriteLine("    cálculo en SQL Server - acumulados precalculados - sin producto por ahora")
    '    Dim myDb As New myDb
    '    Dim oracleDb As New oracleDB
    '    Dim lParam As New List(Of SqlParameter)
    '    Dim query As String = "select [SUMA PIEZAS] FROM [batzweb].[dbo].[Acumulados_Bezerresis]
    '                            where IDPLANTA=@IDPLANTA 
    '                            and PERIODO LIKE " & If(mesesAcumulados = -12, "'ANUAL' ", "'SEMESTRAL' ")
    '    lParam.Add(New SqlParameter("@IDPLANTA", idPlanta))
    '    Dim fechaCalculo = DateTime.ParseExact(monthYearDate, "MMyyyy", Globalization.CultureInfo.InvariantCulture)
    '    query &= "AND ANNO = @ANNO AND MES = @MES "
    '    lParam.Add(New SqlParameter("@ANNO", "" & fechaCalculo.Year & ""))
    '    lParam.Add(New SqlParameter("@MES", "" & fechaCalculo.Month & ""))
    '    Dim clientName As String = ""
    '    If lClientes_Selected IsNot Nothing Then
    '        Dim clienteString As String = oracleDb.getClienteStringFromClienteId(lClientes_Selected) '''' si fuera una relación 1:N o N:N, esto tendría que ser un bucle
    '        query &= "AND [CLIENTE] LIKE @CLIENTE "
    '        lParam.Add(New SqlParameter("@CLIENTE", clienteString))
    '    Else
    '        query &= "AND [CLIENTE] like 'TODOS' "
    '    End If
    '    Dim result As String = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of String)(query, Configuration.ConfigurationManager.ConnectionStrings("CN_ACUMULADOS").ConnectionString, lParam.ToArray)
    '    Return result
    'End Function
End Class
