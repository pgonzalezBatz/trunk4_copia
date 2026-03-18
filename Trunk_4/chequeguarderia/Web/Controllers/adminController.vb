Imports System.IO
Namespace web
    Public Class AdminController
        Inherits System.Web.Mvc.Controller
        Const Empresa = 1
        ReadOnly strCnGestionHoras = ConfigurationManager.ConnectionStrings("gestionhoras").ConnectionString
        ReadOnly strCnIzaro = ConfigurationManager.ConnectionStrings("izaro").ConnectionString
        ReadOnly strCnEpsilon = ConfigurationManager.ConnectionStrings("epsilon").ConnectionString
        ReadOnly params As Parametros = DB.GetConstantes(strCnGestionHoras)
        <SimpleRoleProvider(Role.administracion)>
        Function Index() As ActionResult
            ViewData("costantes") = params
            ViewData("year") = H2.MySelectList(DB.GetEjerciciosActivos(strCnGestionHoras), Function(o) New Mvc.SelectListItem With {.Value = o.ToString, .Text = o.ToString, .Selected = o = Now.Year})
            ViewData("yearmonth") = H2.MySelectList(DB.GetEjercicioMesActivos(strCnGestionHoras),
                                                   Function(o) New Mvc.SelectListItem With {.Value = o.ejercicio.ToString + "-" + CInt(o.mes).ToString("00"), .Text = o.ejercicio.ToString + "-" + CInt(o.mes).ToString("00"), .Selected = o.ejercicio = Now.Year And o.mes = Now.Month})
            Return View()
        End Function
        <SimpleRoleProvider(Role.administracion)>
        Function List(ByVal yearmonth As String) As ActionResult
            Dim h = yearmonth.Split("-")
            ViewData("ejercicio") = h(0)
            ViewData("mes") = h(1)
            ViewData("solicitudesguarderia") = DB.GetSolicitudesGuarderiaMes(h(0), h(1), strCnGestionHoras, strCnEpsilon)
            ViewData("existenguarderiasnuevas") = DB.GetExistenNuevasGuarderias(h(0), h(1), strCnGestionHoras)
            Dim diacorte = DB.GetDiaCorte(Empresa, strCnGestionHoras)
            Dim mes = New Date(h(0), h(1), 1)
            Dim desdefecha = New Date(DateAdd(DateInterval.Month, -1, mes).Year, DateAdd(DateInterval.Month, -1, mes).Month, diacorte + 1)
            Dim hastafecha = New Date(mes.Year, mes.Month, diacorte + 1)
            ViewData("solicitudesgourmet") = DB.GetSolicitudesGourmetMes(desdefecha, hastafecha, params.PorcentajeTramite, strCnGestionHoras)
            Return View("list")
        End Function
        <SimpleRoleProvider(Role.administracion)>
        Function Rangos() As ActionResult
            ViewData("rango") = H2.MySelectList(DB.GetListOfRango(strCnGestionHoras), Function(o) New Mvc.SelectListItem With {.Value = o.rango, .Text = o.rango, .Selected = o.rango = params.RangoActual})
            Return View()
        End Function
        <SimpleRoleProvider(Role.administracion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function Rangos(ByVal rango As Integer) As ActionResult
            DB.UpdateRangoActual(rango, strCnGestionHoras)
            Return RedirectToAction("Index")
        End Function
        <SimpleRoleProvider(Role.administracion)>
        Function VistaEjercicio(ByVal year As Integer) As ActionResult
            ViewData("solicitudes") = DB.GetSolicitudesEjercicio(year, strCnGestionHoras)
            Return View()
        End Function
        <SimpleRoleProvider(Role.administracion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function Exportepsilon(ByVal yearmonth As String, ByVal type As List(Of String)) As ActionResult
            If type Is Nothing Then
                Return List(yearmonth)
            End If
            Dim strMS As New MemoryStream()
            Dim returnWStream = New StreamWriter(strMS, Encoding.UTF8)

            If type.Contains("guarderia") Then
                ExportGuarderia(yearmonth, returnWStream)
            End If
            If type.Contains("gourmet") Then
                ExportGourmet(yearmonth, returnWStream)
            End If
            If type.Contains("cargos") Then
                ExportCostesTramite(yearmonth, returnWStream)
            End If
            If type.Contains("lagunaro") Then
                ExportBeneficiariosLagunaro(yearmonth, returnWStream)
            End If
            returnWStream.Flush()
            strMS.Seek(0, IO.SeekOrigin.Begin)
            Return File(strMS, "application/octet-stream", "gourmet.txt")
        End Function
        Sub ExportGuarderia(ByVal yearmonth As String, ByVal returnWStream As StreamWriter)
            Dim h = yearmonth.Split("-")
            Dim d As New Date(h(0), h(1), 1)
            Dim diacorte = DB.GetDiaCorte(Empresa, strCnGestionHoras)
            Dim dCorte As New Date(h(0), h(1), diacorte)
            For Each p In DB.GetImportes(h(0), h(1), strCnGestionHoras)
                Dim strb As New Text.StringBuilder(h(0)) 'ejercicio
                'IMPORTE
                strb.Append(h(1)) 'mes
                strb.Append("00001") 'codigo empresa
                'Eventual?
                If p.codtra > 3000 OrElse DB.IsSocio(Empresa, p.codtra, dCorte, d, strCnIzaro) Then
                    strb.Append(CDec(p.codtra).ToString("000000")) 'codigo de trabajadore
                Else
                    strb.Append(CDec(p.codtra).ToString("900000")) 'codigo de trabajadore
                End If
                strb.Append("   ") 'codigo de secuencia
                strb.Append("   ") 'codigo de parametro
                strb.Append("089") ' concepto salarial
                strb.Append("2") 'tipo incidencia
                strb.Append(CDec(p.importe).ToString("000000000.00").Replace(",", "")) : strb.Append("00")
                strb.Append("S")
                strb.Append("          ")
                returnWStream.WriteLine(strb)
            Next
        End Sub
        Public Sub ExportGourmet(ByVal yearmonth As String, ByVal returnWStream As StreamWriter)
            Dim h = yearmonth.Split("-")
            Dim diacorte = DB.GetDiaCorte(Empresa, strCnGestionHoras)
            Dim mes = New Date(h(0), h(1), 1)
            Dim desdefecha = New Date(DateAdd(DateInterval.Month, -1, mes).Year, DateAdd(DateInterval.Month, -1, mes).Month, diacorte + 1)
            Dim hastafecha = New Date(mes.Year, mes.Month, diacorte + 1)
            Dim dCorte As New Date(h(0), h(1), diacorte)

            For Each p In DB.GetImportesGourmet(desdefecha, hastafecha, strCnGestionHoras)
                Dim strb As New Text.StringBuilder(h(0)) 'ejercicio
                strb.Append(h(1)) 'mes
                strb.Append("00001") 'codigo empresa
                'Eventual?
                If p.codtra > 3000 OrElse DB.IsSocio(Empresa, p.codtra, dCorte, mes, strCnIzaro) Then
                    strb.Append(CDec(p.codtra).ToString("000000")) 'codigo de trabajadores
                Else
                    strb.Append(CDec(p.codtra).ToString("900000")) 'codigo de trabajadores
                End If
                strb.Append("   ") 'codigo de secuencia
                strb.Append("   ") 'codigo de parametro
                strb.Append("087") ' concepto salarial
                strb.Append("2") 'tipo incidencia
                strb.Append(CDec(p.importe).ToString("000000000.00").Replace(",", "")) : strb.Append("00")
                strb.Append("S")
                strb.Append("          ")
                returnWStream.WriteLine(strb)
            Next
        End Sub
        Public Sub ExportBeneficiariosLagunaro(yearmonth As String, ByVal returnWStream As StreamWriter)
            Dim h = yearmonth.Split("-")
            Dim mes = New Date(h(0), h(1), 1)
            Dim importeLimite = 500
            Dim conceptosalarial = "091"
            Dim diacorte = DB.GetDiaCorte(Empresa, strCnGestionHoras)
            Dim dCorte As New Date(h(0), h(1), diacorte)
            Dim lstBeneficiarios = DB.GetBeneficiariosLagunaroDeAlta(strCnGestionHoras).GroupBy(Function(e) e.idSab).Select(
                Function(group)
                    Dim importe = group.Sum(Function(a) DB.ImporteBeneficiarioAPagar(a.fechaNacimiento))
                    Dim acumulado = DB.GetImporteAnualAcumuladoBeneficiario(mes, group.Key, conceptosalarial, ConfigurationManager.ConnectionStrings("sab").ConnectionString, strCnEpsilon)
                    Return New With {.idsab = group.Key, .idTrabajador = acumulado.idTrabajador, .importe = importe, .acumulado = acumulado.importe}
                End Function).Where(Function(r) importeLimite - r.acumulado >= r.importe AndAlso DB.EsUsuarioAltaEpsilon(r.idTrabajador, strCnEpsilon))

            For Each p In lstBeneficiarios
                Dim strb As New Text.StringBuilder(h(0)) 'ejercicio
                strb.Append(h(1)) 'mes
                strb.Append("00001") 'codigo empresa
                'Eventual?
                strb.Append(p.idTrabajador.ToString()) 'codigo de trabajadore
                strb.Append("   ") 'codigo de secuencia
                strb.Append("   ") 'codigo de parametro
                strb.Append(conceptosalarial) ' concepto salarial
                strb.Append("2") 'tipo incidencia
                strb.Append(CDec(p.importe).ToString("000000000.00").Replace(",", "")) : strb.Append("00")
                strb.Append("S")
                strb.Append("          ")
                returnWStream.WriteLine(strb)
            Next
        End Sub
        Public Sub ExportCostesTramite(ByVal yearmonth As String, ByVal returnWStream As StreamWriter)
            Dim h = yearmonth.Split("-")
            Dim diacorte = DB.GetDiaCorte(Empresa, strCnGestionHoras)
            Dim mes = New Date(h(0), h(1), 1)
            Dim desdefecha = New Date(DateAdd(DateInterval.Month, -1, mes).Year, DateAdd(DateInterval.Month, -1, mes).Month, diacorte + 1)
            Dim hastafecha = New Date(mes.Year, mes.Month, diacorte + 1)
            Dim dCorte As New Date(h(0), h(1), diacorte)
            For Each p In DB.GetTramites(desdefecha, hastafecha, params.PorcentajeTramite, strCnGestionHoras)
                Dim strb As New Text.StringBuilder(h(0)) 'ejercicio
                strb.Append(h(1)) 'mes
                strb.Append("00001") 'codigo empresa
                'Eventual?
                If p.codtra > 3000 OrElse DB.IsSocio(Empresa, p.codtra, dCorte, mes, strCnIzaro) Then
                    strb.Append(CDec(p.codtra).ToString("000000")) 'codigo de trabajadore
                Else
                    strb.Append(CDec(p.codtra).ToString("900000")) 'codigo de trabajadore
                End If
                strb.Append("   ") 'codigo de secuencia
                strb.Append("   ") 'codigo de parametro
                strb.Append("090") ' concepto salarial
                strb.Append("2") 'tipo incidencia
                strb.Append(CDec(p.importe).ToString("000000000.00").Replace(",", "")) : strb.Append("00")
                strb.Append("S")
                strb.Append("          ")
                returnWStream.WriteLine(strb)
            Next
        End Sub
        <SimpleRoleProvider(Role.administracion)>
        Function ExportGuarderiaListado(ByVal yearmonth As String) As ActionResult
            Dim h = yearmonth.Split("-")
            Dim d As New Date(h(0), h(1), 1)
            Dim strMS As New MemoryStream()
            Dim returnWStream = New StreamWriter(strMS, Encoding.UTF8)

            returnWStream.WriteLine("IDBATZ;DNI;NOMBRE COMPLETO;EMAIL;NIF HIJO/A;NOMBRE HIJO/A;EJERCICIO;MES;IMPORTE;COSTE TRAMITE;NOMBRE GUARDERIA;CODIGO POSTAL;TIPO;DIRECCION;POBLACION;PROVINCIA;TELEFONO;EMAIL;RESPONSABLE")
            For Each p In DB.GetGuarderiasSinNumero(h(0), h(1), strCnGestionHoras)
                Dim strb As New Text.StringBuilder()
                strb.Append(Trim(p(0))) : strb.Append(";")
                strb.Append(Trim(p(1))) : strb.Append(";")
                strb.Append(Trim(p(2))) : strb.Append(";")
                strb.Append(Trim(p(3))) : strb.Append(";")
                strb.Append(Trim(p(4))) : strb.Append(";")
                strb.Append(DB.GetNombrePersonaEpsilon(Trim(p(4)), strCnEpsilon)) : strb.Append(";")
                strb.Append(Trim(p(5))) : strb.Append(";")
                strb.Append(Trim(p(6))) : strb.Append(";")
                strb.Append(Trim(p(7))) : strb.Append(";")
                strb.Append(Trim(p(8))) : strb.Append(";")
                strb.Append(Trim(p(9))) : strb.Append(";")
                strb.Append(Trim(p(10))) : strb.Append(";")
                strb.Append(Trim(p(11))) : strb.Append(";")
                strb.Append(Trim(p(12))) : strb.Append(";")
                strb.Append(Trim(p(13))) : strb.Append(";")
                strb.Append(Trim(p(14))) : strb.Append(";")
                strb.Append(Trim(p(15))) : strb.Append(";")
                strb.Append(Trim(p(16))) : strb.Append(";")
                strb.Append(Trim(p(17))) : strb.Append(";")
                returnWStream.WriteLine(strb)
            Next
            returnWStream.Flush()
            strMS.Seek(0, IO.SeekOrigin.Begin)
            Return File(strMS, "application/octet-stream", "guarderia.txt")
        End Function
        <SimpleRoleProvider(Role.administracion)>
        Function ExportGuarderiaPedido(ByVal yearmonth As String) As ActionResult
            Dim h = yearmonth.Split("-")
            Dim d As New Date(h(0), h(1), 1)
            Dim strMS As New MemoryStream()
            Dim returnWStream = New StreamWriter(strMS, Encoding.UTF8)
            returnWStream.WriteLine("Codigo cliente;Codigo guarderia;Matricula;Apellidos nombre padre/madre;Nº talonarios;Nº cheques;Importe;Total;Nombre/id niño/niña;email padre/madre;")
            For Each p In DB.GetSolicitudesGuarderiaMes(h(0), h(1), strCnGestionHoras, strCnEpsilon)
                Dim strb As New Text.StringBuilder()
                strb.Append("10864;")
                strb.Append(p.idGuarderiaGourmet) : strb.Append(";")
                strb.Append(";") 'Matricula
                strb.Append(p.nombre.ToString().Trim(" ")) : strb.Append(";")
                strb.Append("1") : strb.Append(";")
                strb.Append("1") : strb.Append(";")
                strb.Append(p.importe) : strb.Append(";")
                strb.Append(p.importe) : strb.Append(";")
                strb.Append(p.idHijaGOurmet) : strb.Append(";")
                strb.Append(p.email)
                returnWStream.WriteLine(strb)
            Next
            returnWStream.Flush()
            strMS.Seek(0, IO.SeekOrigin.Begin)
            Return File(strMS, "application/octet-stream", "guarderia.txt")
        End Function
        <SimpleRoleProvider(Role.administracion)>
        Function Import(ByVal filewithids As HttpPostedFileBase, ByVal yearmonth As String) As ActionResult
            If filewithids.ContentLength > 0 Then
                Dim sr As New StreamReader(filewithids.InputStream, System.Text.Encoding.UTF8)
                sr.ReadLine() 'Quitar la linea de titulos
                While Not sr.EndOfStream
                    Dim s = sr.ReadLine().Split(";")
                    Dim idTrabajador = DB.GetIdTrabajador(s(1), strCnEpsilon)
                    If idTrabajador > 900000 Then
                        idTrabajador = idTrabajador - 900000
                    End If
                    DB.UpdateIdGuarderiaMes(idTrabajador, s(4), s(20), s(0), s(19), strCnGestionHoras)
                End While
            End If
            Return RedirectToAction("list", New With {.yearmonth = yearmonth})
        End Function


    End Class
End Namespace