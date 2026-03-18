Imports System
Imports System.Collections.Generic

Namespace Controllers
    Public Class INDICADORES2Controller
        Inherits Controller

        Private db As New Entities_BezerreSis
        'Private db As New Entities_Bezerresis_Mock

        <PlantaSeleccionadaFilterAttribute()>
        <Authorize(Roles:="Usuario")>
        Function Calidad() As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            TempData("ReturnUrl") = "Calidad"
            Dim lIndicadores As New List(Of String())
            Dim cookies = Request.Cookies("BezerreSis_Filtro")
            Dim lClientes_Selected As New List(Of Integer)
            Dim lProductos_Selected As New List(Of Integer)
            Dim fecha_desde As New Nullable(Of Date)
            Dim fecha_hasta As New Nullable(Of Date)
            If Request IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(cookies.Values("IdCliente_ind")) Then
                    lClientes_Selected = cookies.Values("IdCliente_ind").Split(",").Select(Function(f) CInt(f)).ToList
                End If
                If Not String.IsNullOrWhiteSpace(cookies.Values("IdProducto_ind")) Then
                    lProductos_Selected = cookies.Values("IdProducto_ind").Split(",").Select(Function(f) CInt(f)).ToList
                End If
                If Not String.IsNullOrWhiteSpace(cookies.Values("FECHA_DESDE_ind")) Then
                    fecha_desde = cookies.Values("FECHA_DESDE_ind")
                End If
                If Not String.IsNullOrWhiteSpace(cookies.Values("FECHA_HASTA_ind")) Then
                    fecha_hasta = cookies.Values("FECHA_HASTA_ind")
                End If
            End If
            Dim oracleDb As New oracleDB
            'MvcApplication.Loguear("Info", "INDICADORES.Calidad. ActualizacionVentas START", User.Identity.Name.ToString)
            'oracleDb.ActualizarIncidenciasYRechazadas()
            'MvcApplication.Loguear("Info", "INDICADORES.Calidad. ActualizacionVentas END", User.Identity.Name.ToString)
            Dim idPlanta = cookies.Values("IdPlanta")
            ViewBag.FECHA_DESDE_ind = If(fecha_desde Is Nothing, Nothing, "" & fecha_desde.Value.Year & "/" & If(fecha_desde.Value.Month < 10, "0", "") & fecha_desde.Value.Month)
            ViewBag.FECHA_HASTA_ind = If(fecha_hasta Is Nothing, Nothing, "" & fecha_hasta.Value.Year & "/" & If(fecha_hasta.Value.Month < 10, "0", "") & fecha_hasta.Value.Month)
            ViewBag.lClientes_ind = db.CLIENTES.Where(Function(o) o.IDPLANTA = idPlanta AndAlso Not o.NOMBRE.Contains("default")).Select(Function(f) New Mvc.SelectListItem With {.Text = f.NOMBRE, .Value = f.ID, .Selected = lClientes_Selected.Contains(f.ID)}).ToList.OrderBy(Function(f) f.Text)
            If Not String.IsNullOrWhiteSpace(cookies.Values("productosAll")) AndAlso cookies.Values("productosAll") Then
                lProductos_Selected = (From prod As PRODUCTOS In db.PRODUCTOS Select CInt(prod.ID)).ToList
            End If
            ViewBag.lProductos_ind = db.PRODUCTOS.Where(Function(o) o.IDPLANTA = idPlanta).Select(Function(f) New Mvc.SelectListItem With {.Text = f.NOMBRE, .Value = f.ID, .Selected = lProductos_Selected.Contains(f.ID)}).ToList.OrderBy(Function(f) f.Text)

            'actualizarDatos() - incluimos la actualización de datos en la búsqueda
            UpdateData()

            lIndicadores = oracleDb.getIndicadoresCalidad(fecha_desde, fecha_hasta, lClientes_Selected, lProductos_Selected, idPlanta)
            Dim lIndicadoresFinal As New List(Of Object)
            Dim counter = 1
            If lClientes_Selected.Count > 0 AndAlso lProductos_Selected.Count > 0 Then
                For Each indicador In lIndicadores
                    Dim anno = CInt(indicador(1))
                    Dim mes = CInt(indicador(2))
                    Dim monthYearDate As String = If(mes < 10, "0", "")
                    monthYearDate &= mes.ToString & anno.ToString
                    Dim sumaPiezasVendidas = CInt(indicador(3))
                    indicador(4) = If(lClientes_Selected.Count = 1, oracleDb.getObjetivo(monthYearDate, lClientes_Selected(0)), oracleDb.getObjetivoDefault(monthYearDate))
                    Dim numIncidencias As Integer = 0
                    Dim numIncidenciasSemestre As Decimal = 0 '''' para que haga un cast automático a decimal, ya que para calcular el acumulado lo vamos a multiplicar por 10^9, así que con que haya más de dos incidencias se desbordaría el int
                    Dim numPiezasRechazadasAnyo As Integer = 0
                    Dim fechaCalculo = New Date(anno, mes, 1)
                    numIncidencias = oracleDb.getNumeroIncidenciasFecha(anno, mes, lProductos_Selected, lClientes_Selected) ''''TODO: esto debería ser lo mismo que indicador(5)
                    Dim acumuladoEndDate = fechaCalculo.AddMonths(1).AddDays(-1)
                    Dim acumuladoSemestreStartDate = fechaCalculo.AddMonths(-5)
                    Dim acumuladoAnualStartDate = fechaCalculo.AddMonths(-11)
                    numIncidenciasSemestre = oracleDb.getNumeroIncidenciasAcumulado(acumuladoSemestreStartDate, acumuladoEndDate, lProductos_Selected, lClientes_Selected) ''''TODO: NO COGE DE LA TABLA VENTAS
                    numPiezasRechazadasAnyo = oracleDb.getNumeroPiezasRechazadasAcumulado(acumuladoAnualStartDate, acumuladoEndDate, lProductos_Selected, lClientes_Selected) ''''TODO: NO COGE DE LA TABLA VENTAS
                    Dim ppmMensual = If(sumaPiezasVendidas = 0, 0, CInt(1000000 * indicador(6) / sumaPiezasVendidas))
                    Dim prrMensual = numIncidencias
                    Dim sumaPiezasVendidasAnyo = oracleDb.getSumaPiezasVendidasAcumuladas(monthYearDate, lProductos_Selected, lClientes_Selected, idPlanta, -12)
                    Dim sumaPiezasVendidasSemestre = oracleDb.getSumaPiezasVendidasAcumuladas(monthYearDate, lProductos_Selected, lClientes_Selected, idPlanta, -6)

                    Dim multiplicadorPPM = If(sumaPiezasVendidasAnyo > 0, 1000000 / sumaPiezasVendidasAnyo, 0)
                    Dim ppmAcumulado = If(sumaPiezasVendidasAnyo = 0, 0, numPiezasRechazadasAnyo * multiplicadorPPM)
                    Dim multiplicadorIPB = If(sumaPiezasVendidasSemestre > 0, 1000000000 / sumaPiezasVendidasSemestre, 0)
                    Dim ipbAcumulado = If(sumaPiezasVendidasSemestre = 0, 0, numIncidenciasSemestre * multiplicadorIPB)

                    'Dim ppmAcumulado = If(sumaPiezasVendidasAnyo = 0, 0, CDec(1000000 * numPiezasRechazadasAnyo / sumaPiezasVendidasAnyo))
                    'Dim ipbAcumulado = If(sumaPiezasVendidasSemestre = 0, 0, CInt(1000000000 * numIncidenciasSemestre / sumaPiezasVendidasSemestre))
                    Dim prrAcumulado = numIncidenciasSemestre
                    indicador(5) = ppmMensual & ";" & prrMensual & ";" & ppmAcumulado & ";" & prrAcumulado & ";" & ipbAcumulado
                    lIndicadoresFinal.Add(indicador)
                    counter += 1
                Next
            End If
            ViewBag.ShowTable = lClientes_Selected.Count > 0 AndAlso lProductos_Selected.Count > 0
            ViewBag.MuestraMsg = cookies.Values.AllKeys.Contains("IdCliente_ind") AndAlso Not ViewBag.ShowTable 'significa que ya se ha pulsado el botón de buscar
            ViewBag.clientesResumen = String.Join(", ", db.CLIENTES.Where(Function(o) o.IDPLANTA = idPlanta AndAlso If(lClientes_Selected.Count > 0, lClientes_Selected.Contains(o.ID), True)).Select(Function(f) f.NOMBRE).OrderBy(Function(f) f))
            ViewBag.productosResumen = String.Join(", ", db.PRODUCTOS.Where(Function(o) o.IDPLANTA = idPlanta AndAlso If(lProductos_Selected.Count > 0, lProductos_Selected.Contains(o.ID), True)).Select(Function(f) f.NOMBRE).OrderBy(Function(f) f))
            Return View(lIndicadoresFinal.ToList)
        End Function

        <PlantaSeleccionadaFilterAttribute()>
        <Authorize(Roles:="Usuario")>
        Function CalidadGarantias() As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            TempData("ReturnUrl") = "CalidadGarantias"

            Dim lIndicadores As New List(Of String())
            Dim cookies = Request.Cookies("BezerreSis_Filtro")
            Dim lClientes_Selected As New List(Of Integer)
            Dim lProductos_Selected As New List(Of Integer)
            Dim fecha_desde As New Nullable(Of Date)
            Dim fecha_hasta As New Nullable(Of Date)

            ' Obtener los valores de los filtros desde las cookies
            If Request IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(cookies.Values("IdCliente_ind")) Then
                    lClientes_Selected = cookies.Values("IdCliente_ind").Split(",").Select(Function(f) CInt(f)).ToList
                End If
                If Not String.IsNullOrWhiteSpace(cookies.Values("IdProducto_ind")) Then
                    lProductos_Selected = cookies.Values("IdProducto_ind").Split(",").Select(Function(f) CInt(f)).ToList
                End If
                If Not String.IsNullOrWhiteSpace(cookies.Values("FECHA_DESDE_ind")) Then
                    fecha_desde = Date.ParseExact(cookies.Values("FECHA_DESDE_ind"), "yyyy/MM", System.Globalization.DateTimeFormatInfo.InvariantInfo)
                End If
                If Not String.IsNullOrWhiteSpace(cookies.Values("FECHA_HASTA_ind")) Then
                    fecha_hasta = Date.ParseExact(cookies.Values("FECHA_HASTA_ind"), "yyyy/MM", System.Globalization.DateTimeFormatInfo.InvariantInfo)
                End If
            End If

            'actualizarDatos() - incluimos la actualización de datos en la búsqueda
            UpdateData()

            Dim oracleDb As New oracleDB
            Dim idPlanta = cookies.Values("IdPlanta")

            ' Obtener los datos de indicadores de calidad de garantías
            lIndicadores = oracleDb.getIndicadoresCalidadGarantias(fecha_desde, fecha_hasta, lClientes_Selected, lProductos_Selected, idPlanta)

            ' Mostrar resultados en la vista
            ViewBag.FECHA_DESDE_ind = If(fecha_desde Is Nothing, Nothing, "" & fecha_desde.Value.Year & "/" & If(fecha_desde.Value.Month < 10, "0", "") & fecha_desde.Value.Month)
            ViewBag.FECHA_HASTA_ind = If(fecha_hasta Is Nothing, Nothing, "" & fecha_hasta.Value.Year & "/" & If(fecha_hasta.Value.Month < 10, "0", "") & fecha_hasta.Value.Month)
            ViewBag.lClientes_ind = db.CLIENTES.Where(Function(o) o.IDPLANTA = idPlanta AndAlso Not o.NOMBRE.Contains("default")).Select(Function(f) New Mvc.SelectListItem With {.Text = f.NOMBRE, .Value = f.ID, .Selected = lClientes_Selected.Contains(f.ID)}).ToList.OrderBy(Function(f) f.Text)
            ViewBag.lProductos_ind = db.PRODUCTOS.Where(Function(o) o.IDPLANTA = idPlanta).Select(Function(f) New Mvc.SelectListItem With {.Text = f.NOMBRE, .Value = f.ID, .Selected = lProductos_Selected.Contains(f.ID)}).ToList.OrderBy(Function(f) f.Text)

            ViewBag.ShowTable = lClientes_Selected.Count > 0 AndAlso lProductos_Selected.Count > 0
            ViewBag.MuestraMsg = cookies.Values.AllKeys.Contains("IdCliente_ind") AndAlso Not ViewBag.ShowTable ' Mostrar mensaje si no se han seleccionado filtros adecuados
            ViewBag.clientesResumen = String.Join(", ", db.CLIENTES.Where(Function(o) o.IDPLANTA = idPlanta AndAlso If(lClientes_Selected.Count > 0, lClientes_Selected.Contains(o.ID), True)).Select(Function(f) f.NOMBRE).OrderBy(Function(f) f))
            ViewBag.productosResumen = String.Join(", ", db.PRODUCTOS.Where(Function(o) o.IDPLANTA = idPlanta AndAlso If(lProductos_Selected.Count > 0, lProductos_Selected.Contains(o.ID), True)).Select(Function(f) f.NOMBRE).OrderBy(Function(f) f))

            Return View(lIndicadores.ToList)
        End Function


        <PlantaSeleccionadaFilterAttribute()>
        <Authorize(Roles:="Usuario")>
        Function Proceso() As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            TempData("ReturnUrl") = "Proceso"
            Dim lIndicadores As New List(Of String())
            Dim cookies = Request.Cookies("BezerreSis_Filtro")
            Dim lClientes_Selected As New List(Of Integer)
            Dim lProductos_Selected As New List(Of Integer)
            Dim fecha_desde As New Nullable(Of Date)
            Dim fecha_hasta As New Nullable(Of Date)
            If Request IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(cookies.Values("IdCliente_ind")) Then
                    lClientes_Selected = cookies.Values("IdCliente_ind").Split(",").Select(Function(f) CInt(f)).ToList
                End If
                If Not String.IsNullOrWhiteSpace(cookies.Values("IdProducto_ind")) Then
                    lProductos_Selected = cookies.Values("IdProducto_ind").Split(",").Select(Function(f) CInt(f)).ToList
                End If
                If Not String.IsNullOrWhiteSpace(cookies.Values("FECHA_DESDE_ind")) Then
                    fecha_desde = cookies.Values("FECHA_DESDE_ind")
                End If
                If Not String.IsNullOrWhiteSpace(cookies.Values("FECHA_HASTA_ind")) Then
                    fecha_hasta = cookies.Values("FECHA_HASTA_ind")
                End If
            End If
            Dim oracleDb As New oracleDB
            Dim idPlanta = cookies.Values("IdPlanta")
            ViewBag.FECHA_DESDE_ind = If(fecha_desde Is Nothing, Nothing, "" & fecha_desde.Value.Year & "/" & If(fecha_desde.Value.Month < 10, "0", "") & fecha_desde.Value.Month)
            ViewBag.FECHA_HASTA_ind = If(fecha_hasta Is Nothing, Nothing, "" & fecha_hasta.Value.Year & "/" & If(fecha_hasta.Value.Month < 10, "0", "") & fecha_hasta.Value.Month)
            ViewBag.lClientes_ind = db.CLIENTES.Where(Function(o) o.IDPLANTA = idPlanta AndAlso Not o.NOMBRE.Contains("default")).Select(Function(f) New Mvc.SelectListItem With {.Text = f.NOMBRE, .Value = f.ID, .Selected = lClientes_Selected.Contains(f.ID)}).ToList.OrderBy(Function(f) f.Text)
            If Not String.IsNullOrWhiteSpace(cookies.Values("productosAll")) AndAlso cookies.Values("productosAll") Then
                lProductos_Selected = (From prod As PRODUCTOS In db.PRODUCTOS Select CInt(prod.ID)).ToList
            End If
            ViewBag.lProductos_ind = db.PRODUCTOS.Where(Function(o) o.IDPLANTA = idPlanta).Select(Function(f) New Mvc.SelectListItem With {.Text = f.NOMBRE, .Value = f.ID, .Selected = lProductos_Selected.Contains(f.ID)}).ToList.OrderBy(Function(f) f.Text)
            lIndicadores = oracleDb.getIndicadoresProceso(fecha_desde, fecha_hasta, lClientes_Selected, lProductos_Selected, idPlanta)
            Dim lObjetivos = oracleDb.getObjetivosProceso()
            Dim lIndicadoresFinal As New List(Of Object)
            Dim counter = 1
            For Each indicador In lIndicadores
                Dim anno = CInt(indicador(0))
                Dim mes = CInt(indicador(1))
                Dim numIncidencias = indicador(2)
                Dim sumaRepetitivas = indicador(3)
                Dim mediaDias14 = indicador(4)
                Dim mediaDias56 = indicador(5)

                Dim fechaCalculo = New Date(anno, mes, 1)
                Dim acumuladoEndDate = fechaCalculo.AddMonths(1).AddDays(-1)
                Dim acumuladoStartDate = fechaCalculo.AddMonths(-11)

                Dim numIncidenciasAcumulado = oracleDb.getNumeroIncidenciasProcesoAcumulado(acumuladoStartDate, acumuladoEndDate, lProductos_Selected, lClientes_Selected)
                Dim sumaRepetitivasAcumulado = oracleDb.getSumaRepetitivasAcumulado(acumuladoStartDate, acumuladoEndDate, lProductos_Selected, lClientes_Selected)
                Dim mediaDias14Acumulado = oracleDb.getMediaDiasAcumulado(acumuladoStartDate, acumuladoEndDate, lProductos_Selected, lClientes_Selected, "14")
                Dim mediaDias56Acumulado = oracleDb.getMediaDiasAcumulado(acumuladoStartDate, acumuladoEndDate, lProductos_Selected, lClientes_Selected, "56")

                Dim objObj = lObjetivos.Where(Function(o) o.ANNO = anno).FirstOrDefault
                Dim objRep = "-"
                Dim objD14 = "-"
                Dim objD56 = "-"
                If objObj IsNot Nothing Then
                    objRep = objObj.REPETITIVAS
                    objD14 = objObj.DIAS14
                    objD56 = objObj.DIAS56
                End If
                Dim indicadorFinal() As String = {anno, mes, numIncidencias, numIncidenciasAcumulado, sumaRepetitivas, sumaRepetitivasAcumulado, mediaDias14, mediaDias14Acumulado, mediaDias56, mediaDias56Acumulado, objRep, objD14, objD56}
                lIndicadoresFinal.Add(indicadorFinal)
                counter += 1
            Next
            Dim lIndicadoresDefinitivo As New List(Of String())
            Dim fInicio = fecha_desde
            Dim fFin = fecha_hasta
            While fInicio <= fFin
                If lIndicadoresFinal.Where(Function(o) o(0).Equals(fFin.Value.Year.ToString) AndAlso o(1).Equals(fFin.Value.Month.ToString)).Count > 0 Then
                    lIndicadoresDefinitivo.Add(lIndicadoresFinal.Where(Function(o) o(0).Equals(fFin.Value.Year.ToString) AndAlso o(1).Equals(fFin.Value.Month.ToString)).FirstOrDefault)
                Else
                    Dim anno = fFin.Value.Year
                    Dim mes = fFin.Value.Month
                    Dim numIncidencias = 0
                    Dim acumuladoEndDate = fFin.Value.AddMonths(1).AddDays(-1)
                    Dim acumuladoStartDate = fFin.Value.AddMonths(-11)
                    Dim numIncidenciasAcumulado = oracleDb.getNumeroIncidenciasProcesoAcumulado(acumuladoStartDate, acumuladoEndDate, lProductos_Selected, lClientes_Selected)
                    Dim sumaRepetitivas = 0
                    Dim sumaRepetitivasAcumulado = oracleDb.getSumaRepetitivasAcumulado(acumuladoStartDate, acumuladoEndDate, lProductos_Selected, lClientes_Selected)
                    Dim mediaDias14 = 0
                    Dim mediaDias14Acumulado = oracleDb.getMediaDiasAcumulado(acumuladoStartDate, acumuladoEndDate, lProductos_Selected, lClientes_Selected, "14")
                    Dim mediaDias56 = 0
                    Dim mediaDias56Acumulado = oracleDb.getMediaDiasAcumulado(acumuladoStartDate, acumuladoEndDate, lProductos_Selected, lClientes_Selected, "56")

                    Dim objObj = lObjetivos.Where(Function(o) o.ANNO = anno).FirstOrDefault
                    Dim objRep = "-"
                    Dim objD14 = "-"
                    Dim objD56 = "-"
                    If objObj IsNot Nothing Then
                        objRep = objObj.REPETITIVAS
                        objD14 = objObj.DIAS14
                        objD56 = objObj.DIAS56
                    End If
                    Dim indicadorVacio As String() = {anno, mes, numIncidencias, numIncidenciasAcumulado, sumaRepetitivas, sumaRepetitivasAcumulado, mediaDias14, mediaDias14Acumulado, mediaDias56, mediaDias56Acumulado, objRep, objD14, objD56}
                    lIndicadoresDefinitivo.Add(indicadorVacio)
                End If
                fFin = fFin.Value.AddMonths(-1)
            End While

            ViewBag.ShowTable = lClientes_Selected.Count > 0 AndAlso lProductos_Selected.Count > 0
            ViewBag.MuestraMsg = cookies.Values.AllKeys.Contains("IdCliente_ind") AndAlso Not ViewBag.ShowTable 'significa que ya se ha pulsado el botón de buscar
            ViewBag.clientesResumen = String.Join(", ", db.CLIENTES.Where(Function(o) o.IDPLANTA = idPlanta AndAlso If(lClientes_Selected.Count > 0, lClientes_Selected.Contains(o.ID), True)).Select(Function(f) f.NOMBRE).OrderBy(Function(f) f))
            ViewBag.productosResumen = String.Join(", ", db.PRODUCTOS.Where(Function(o) o.IDPLANTA = idPlanta AndAlso If(lProductos_Selected.Count > 0, lProductos_Selected.Contains(o.ID), True)).Select(Function(f) f.NOMBRE).OrderBy(Function(f) f))
            Return View(lIndicadoresDefinitivo.ToList)
        End Function

        <PlantaSeleccionadaFilterAttribute()>
        <Authorize(Roles:="Usuario")>
        Function Index_Filtro() As ActionResult
            If Session("Home") Then Return RedirectToAction("Index", "Home")
            Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
            aCookie.Values("IdCliente_ind") = Request("lClientes_ind")
            aCookie.Values("IdProducto_ind") = Request("lProductos_ind")
            aCookie.Values("productosAll") = Request("productosAll").Contains("active")
            '''' --> hemos usado un HFprouctosAll para testear si todos los productos estaban seleccionados en el anterior filtro
            '''' al depender los productos de los clientes, un filtrado específico acotaba los productos
            '''' al volver a un filtrado general, se mantenian los productos acotados... ahora ya no
            aCookie.Values("FECHA_DESDE_ind") = Request("FECHA_DESDE_ind")
            aCookie.Values("FECHA_HASTA_ind") = Request("FECHA_HASTA_ind")
            aCookie.HttpOnly = True
            Response.Cookies.Add(aCookie)
            Return RedirectToAction(TempData.Peek("ReturnUrl"))
        End Function


        <HttpPost()>
        <Authorize(Roles:="Usuario")>
        Function UpdateData() As ActionResult
            Try
                Dim oracleDB As New oracleDB
                MvcApplication.Loguear("Info", "Empieza 'UpdateData'", User.Identity.Name)
                oracleDB.ActualizarIncidenciasYRechazadas()
                MvcApplication.Loguear("Info", "Termina 'UpdateData'", User.Identity.Name)
                Return Json("OK", JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                MvcApplication.Loguear("Error", "Error en 'UpdateData'", User.Identity.Name, ex.InnerException.Message)
                Return Json("NOK", JsonRequestBehavior.AllowGet)
            End Try
        End Function
    End Class
End Namespace