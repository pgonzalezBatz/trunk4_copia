Imports System.Globalization
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Namespace web
    Public Class AltaController
        Inherits System.Web.Mvc.Controller

        Const Empresa = 1
        ReadOnly strCnSab = ConfigurationManager.ConnectionStrings("sab").ConnectionString
        ReadOnly strCnIzaro = ConfigurationManager.ConnectionStrings("izaro").ConnectionString
        ReadOnly strCnEpsilon = ConfigurationManager.ConnectionStrings("epsilon").ConnectionString
        ReadOnly strCnGestionHoras = ConfigurationManager.ConnectionStrings("gestionhoras").ConnectionString
        ReadOnly params As Parametros = DB.GetConstantes(strCnGestionHoras)

        <SimpleRoleProvider(Role.normal, Role.administracion)>
        Function Index() As ActionResult
            If DB.GetCultura(SimpleRoleProvider.GetId(), strCnSab) = "eu-ES" Then
                Return View("index2")
            Else
                Return View()
            End If
        End Function
        <SimpleRoleProvider(Role.normal, Role.administracion)>
        Function Nohijas() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(Role.normal, Role.administracion)>
        Function ListSolicitudes() As ActionResult
            Return View(DB.GetSolicitudes(SimpleRoleProvider.GetId(), params.EjercicioActual, strCnGestionHoras).GroupBy(Function(o) o.ejercicio, Function(k, l) New With {.ejercicio = k, .total = l.Sum(Function(y) y.importe), .lst = l}))
        End Function
        <SimpleRoleProvider(Role.normal, Role.administracion)>
        Function FormularioAlta() As ActionResult
            Dim idsab = SimpleRoleProvider.GetId()
            Dim o = DB.GetTrabajadorEpsilon(Empresa, idsab, strCnSab, strCnIzaro, strCnEpsilon)
            If Not DB.FirmadoContrato(o.codtra, strCnGestionHoras) Then
                Return RedirectToAction("firmar")
            End If

            Dim solUI As New SolicitudUI With {
                .idSab = idsab
            }
            ViewData("solicitante") = o.nombre + " " + o.apellido1 + " " + o.apellido2
            ViewData("mail") = o.email
            ViewData("telefono") = o.telefono
            Dim lstHijas = DB.GetHijos(o.codtra, strCnEpsilon)
            'Restarle los hijos a los cuales ya se han asignado soicitudes
            Dim lstHijasAsignadas = DB.GetHijosAsignadosARango(params.EjercicioActual, params.RangoActual, idsab, strCnGestionHoras)
            If lstHijas.Count = 0 Then
                Return RedirectToAction("nohijas")
            End If
            'If lstHijas.Count > lstHijasAsignadas.Count Then
            '    lstHijas.RemoveAll(Function(h) lstHijasAsignadas.Exists(Function(h2) h2.nif = h.nif))
            'End If
            ViewData("nifhija") = H2.MySelectList(lstHijas, Function(el) New Mvc.SelectListItem With {.Value = el.nif, .Text = el.nombre.tolower + " " + el.apellido1.tolower + " " + el.apellido2.tolower + " (" + el.fechaNacimiento + ")"})
            ViewData("tipoguarderia") = H2.MySelectList(Of String)(ConfigurationManager.AppSettings("tipoguarderia").Split(";").ToList, Function(s) New Mvc.SelectListItem With {.Value = s.Split("=")(0), .Text = s.Split("=")(1)})
            'TODO: como elegir el año y rango a mostrar?
            Dim diaCorte = DB.GetDiaCorte(Empresa, strCnGestionHoras)
            ViewData("mes") = H2.MySelectList(DB.GetMesesDeRangoAbiertos(params.EjercicioActual, params.RangoActual, 25, strCnGestionHoras), Function(el) New Mvc.SelectListItem With {.Value = el, .Text = H2.GetMonthName(el)})
            solUI.Ejercicio = params.EjercicioActual
            'Asegurarnos de que no tiene registros ya asignados para tod@s l@s hij@s
            'If db.hasRegistrosAsignados(idsab, lstHijas, params.RangoActual, solUI.Ejercicio, strCnGestionHoras) Then
            '    Return RedirectToAction("listSolicitudes")
            'End If
            Return View(solUI)
        End Function
        <SimpleRoleProvider(Role.normal, Role.administracion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function FormularioAlta(ByVal solUi As SolicitudUI) As ActionResult
            Dim idsab = SimpleRoleProvider.GetId()
            Dim o = DB.GetTrabajadorEpsilon(Empresa, idsab, strCnSab, strCnIzaro, strCnEpsilon)
            ViewData("solicitante") = o.nombre + " " + o.apellido1 + " " + o.apellido2
            ViewData("mail") = o.email
            ViewData("telefono") = o.telefono
            solUi.idSab = idsab
            Dim diaCorte = DB.GetDiaCorte(Empresa, strCnGestionHoras)
            If ModelState.IsValid Then
                Dim lstejercicioActual = solUi.Mes.FindAll(Function(y) y >= Now.Month)
                Dim lstejercicioSiguiente = solUi.Mes.FindAll(Function(y) y < Now.Month)
                'Comprobar que el usuario no sobrepasa el lìmite anual
                If lstejercicioActual.Count * solUi.Importe <= (params.LimiteRango - DB.GetImporteAcumuladoEjercicio(params.EjercicioActual, idsab, solUi.NifHija, strCnGestionHoras)) And
                   lstejercicioSiguiente.Count * solUi.Importe <= (params.LimiteRango - DB.GetImporteAcumuladoEjercicio(params.EjercicioActual + 1, idsab, solUi.NifHija, strCnGestionHoras)) Then
                    DB.InsertMesGuarderia(solUi, strCnGestionHoras)
                    Return RedirectToAction("listSolicitudes")
                Else
                    ModelState.AddModelError("importe", "La acumulación del importe durante los meses seleccionados no puede superar los " + params.LimiteRango.ToString + " €")
                End If
            End If
            Dim lstHijas = DB.GetHijos(o.codtra, strCnEpsilon)
            ViewData("nifhija") = H2.MySelectList(lstHijas, Function(el) New Mvc.SelectListItem With {.Value = el.nif, .Text = el.nombre.tolower + " " + el.apellido1.tolower + " " + el.apellido2.tolower, .Selected = el.nif = solUi.NifHija})
            ViewData("tipoguarderia") = H2.MySelectList(Of String)(ConfigurationManager.AppSettings("tipoguarderia").Split(";").ToList, Function(s) New Mvc.SelectListItem With {.Value = s.Split("=")(0), .Text = s.Split("=")(1), .Selected = s.Split("=")(0) = solUi.TipoGuarderia})

            ViewData("mes") = H2.MySelectList(DB.GetMesesDeRangoAbiertos(params.EjercicioActual, params.RangoActual, diaCorte, strCnGestionHoras), Function(el) New Mvc.SelectListItem With {.Value = el, .Text = H2.GetMonthName(el), .Selected = IsSelected(solUi.Mes, el)})
            Return View(solUi)
        End Function
        <SimpleRoleProvider(Role.normal, Role.administracion)>
        Function EliminarMes(ByVal mes As Integer, ByVal nifhija As String)
            Return View()
        End Function
        <SimpleRoleProvider(Role.normal, Role.administracion)>
        Function CantidadAjax(ByVal tipoguarderia As String) As PartialViewResult
            ViewData("tipoguarderia") = H2.MySelectList(Of String)(ConfigurationManager.AppSettings("tipoguarderia").Split(";").ToList, Function(s) New Mvc.SelectListItem With {.Value = s.Split("=")(0), .Text = s.Split("=")(1), .Selected = s.Split("=")(0) = tipoguarderia})
            Return PartialView("cantidad")
        End Function
        <SimpleRoleProvider(Role.normal, Role.administracion)>
        Function Firmar() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(Role.normal, Role.administracion)>
        Function Contratopdf() As ActionResult
            Dim doc As Document = New Document(iTextSharp.text.PageSize.A4)
            Dim bf = New Font(Font.FontFamily.HELVETICA, 9, 0)
            Dim bf1 = New Font(Font.FontFamily.HELVETICA, 9, 1)
            doc.SetMargins(30, 10, 10, 100)
            Dim strMS As New MemoryStream()
            Dim img = iTextSharp.text.Image.GetInstance(Server.MapPath("..") + "/Content/header.png")
            img.ScaleAbsolute(550, 60)
            Dim wrtTest = PdfWriter.GetInstance(doc, strMS)
            wrtTest.CloseStream = False
            doc.Open()
            doc.Add(img)
            doc.Close()
            strMS.Seek(0, System.IO.SeekOrigin.Begin)
            Return New FileStreamResult(strMS, "application/pdf")
        End Function

        Private Function IsSelected(ByVal lst As List(Of Integer), ByVal val As Integer) As Boolean
            If lst Is Nothing Then
                Return False
            End If
            Return lst.Exists(Function(z) z = val)
        End Function

    End Class
End Namespace