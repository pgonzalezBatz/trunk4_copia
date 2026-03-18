Imports System.Security.Permissions
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Namespace web
    Public Class DistribucionController
        Inherits System.Web.Mvc.Controller
        Private Const Empresa = 1
        Dim strCnGestionHoras = ConfigurationManager.ConnectionStrings("gestionhoras").ConnectionString
        Dim strCnIzaro = ConfigurationManager.ConnectionStrings("izaro").ConnectionString
        Dim strCnEpsilon = ConfigurationManager.ConnectionStrings("epsilon").ConnectionString
        Dim strCnSab = ConfigurationManager.ConnectionStrings("sab").ConnectionString



        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Index() As ActionResult
            ViewData("listOfTipos") = DB.GetListOfTipoCheque(Empresa, strCnGestionHoras).GroupBy(Function(o) o.tipo)
            Return View()
        End Function

        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function ConfirmarDistribucion(ByVal d As Distribucion) As ActionResult
            If ModelState.IsValid Then
                Dim l1 = CInt(d.Lectura1.Substring(0, 9))
                Dim l2 = CInt(d.Lectura2.Substring(0, 9))
                Dim talonario = DB.GetTipoCheque(Empresa, d.IdTipo, strCnGestionHoras)
                If l2 - l1 + 1 <> talonario.numeroCheques Then
                    ModelState.AddModelError("lectura1", "Error en la lectura. Asegurate de escanear la primera y última página.")
                    ViewData("listOfTipos") = DB.GetListOfTipoCheque(Empresa, strCnGestionHoras).GroupBy(Function(o) o.tipo)
                    Return View("index", d)
                End If

                If CDec(talonario.precio).ToString("000.00").Replace(",", "") <> d.Lectura1.Substring(11, 5) Then
                    ModelState.AddModelError("", "Error en la lectura. El talonario seleccionado no coincide con la lectura.")
                    ViewData("listOfTipos") = DB.GetListOfTipoCheque(Empresa, strCnGestionHoras).GroupBy(Function(o) o.tipo)
                    Return View("index", d)
                End If
                Dim catconv = DB.GetCategoriaCOnvenio(d.IdTrabajador, strCnEpsilon)
                If catconv.Count = 1 Then
                    If catconv.First().idconvenio <> "001" AndAlso catconv.First().idconvenio <> "002" Then
                        ModelState.AddModelError("", "Error en la lectura. Las personas que no pertenezcan al convenio coperativista o Regimen General no pueden coger talonarios")
                        ViewData("listOfTipos") = DB.GetListOfTipoCheque(Empresa, strCnGestionHoras).GroupBy(Function(o) o.tipo)
                        Return View("index", d)
                    End If
                    If catconv.First().idconvenio = "001" And catconv.First().idcategoria = "007" AndAlso d.IdTipo <> 2 Then 'Los de fagor solo pueden coger este talonario
                        ModelState.AddModelError("", "Error en la lectura. Los reubicados solo pueden coger el talonario de 3 euros")
                        ViewData("listOfTipos") = DB.GetListOfTipoCheque(Empresa, strCnGestionHoras).GroupBy(Function(o) o.tipo)
                        Return View("index", d)
                    End If
                End If

                If DB.AsegurarUnicoTalonarioTipo(Empresa, d.IdTrabajador, talonario.tipo, l1, DB.GetDiaCorte(Empresa, strCnGestionHoras).ToString, strCnGestionHoras) Then
                    If talonario.tipo = "C" AndAlso Not DB.FirmadoContrato(d.IdTrabajador, strCnGestionHoras) Then
                        Return RedirectToAction("contrato", d)
                    Else
                        Dim t = DB.DatosTrabajador(d.IdTrabajador, strCnGestionHoras)
                        Return View(New DistribucionDesconpuesta With {.Desde = l1, .Hasta = l2, .Tipo = talonario.Id, .Precio = talonario.precio, .DNI = t.dni, .Nombre = t.nombre,
                                                                       .Apellido1 = t.apellido1, .Apellido2 = t.apellido2, .IdTrabajador = d.IdTrabajador, .Email = t.email, .NumeroCheques = talonario.numerocheques})
                    End If
                End If
                ModelState.AddModelError("lectura1", "El trabajador ya ha recibido este tipo de talonario este mes.")
            End If
            ViewData("listOfTipos") = DB.GetListOfTipoCheque(Empresa, strCnGestionHoras).GroupBy(Function(o) o.tipo)
            Return View("index", d)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function GuardarDistribucion(ByVal d As DistribucionDesconpuesta) As ActionResult
            Dim diaCorte = DB.GetDiaCorte(Empresa, strCnGestionHoras)
            Dim talonario = DB.GetTipoCheque(Empresa, d.Tipo, strCnGestionHoras)
            If ModelState.IsValid AndAlso CInt(d.Hasta) - CInt(d.Desde) + 1 = talonario.numeroCheques Then
                If DB.AsegurarUnicoTalonarioTipo(Empresa, d.IdTrabajador, talonario.tipo, d.Desde, diaCorte.ToString, strCnGestionHoras) Then
                    DB.SaveDistribucion(Empresa, d, diaCorte, strCnGestionHoras)
                    If talonario.tipo = "C" Then 'Descuenta de nomina
                        h2.SendEmail(d.Email, "Txeketegiaren  banaketa konfirmazioa / Confirmación de la distribución de talonario",
                                    (d.Precio * talonario.numeroCheques).ToString + "€ balio duen txeketegia jaso berri duzu. Hau hurrengo nominatik deskontatuko zaizu." + Environment.NewLine _
                                    + "Ezin izango duzu '" + talonario.nombre + "' motako txeketegirik " + (CInt(diaCorte) + 1).ToString + " rate jaso." _
                                    + Environment.NewLine + Environment.NewLine _
                                    + "Le confirmamos que acaba de recibir un talonario con valor " + (d.Precio * talonario.numeroCheques).ToString + "€ que se le descontará de la proxima nomina" + Environment.NewLine _
                                        + "Hasta el proximo día " + (CInt(diaCorte) + 1).ToString + " no prodra recibir otro talonario del tipo '" + talonario.nombre + "'.")
                    Else
                        h2.SendEmail(d.Email, "Txeketegiaren  banaketa konfirmazioa / Confirmación de la distribución de talonario",
                                    (d.Precio * talonario.numeroCheques).ToString + "€ balio duen txeketegia jaso berri duzu." + Environment.NewLine _
                                    + "Ezin izango duzu '" + talonario.nombre + "' motako txeketegirik " + (CInt(diaCorte) + 1).ToString + " rate jaso." _
                                    + Environment.NewLine + Environment.NewLine _
                                    + "Le confirmamos que acaba de recibir un talonario con valor " + (d.Precio * talonario.numeroCheques).ToString + "€." + Environment.NewLine _
                                        + "Hasta el proximo día " + (CInt(diaCorte) + 1).ToString + " no prodra recibir otro talonario del tipo '" + talonario.nombre + "'")
                    End If


                    Return RedirectToAction("realizada")
                End If
                ModelState.AddModelError("lectura1", "El trabajador ya ha recibido este tipo de talonario este mes.")
            End If
            Return View("confirmardistribucion", d)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Contrato(ByVal d As Distribucion) As ActionResult
            Return View(d)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function Contrato(ByVal d As Distribucion, ByVal firmado As String) As ActionResult
            If firmado IsNot Nothing Then
                DB.SaveFirmaContrato(d.IdTrabajador, strCnGestionHoras)
                ViewData("listOfTipos") = DB.GetListOfTipoCheque(Empresa, strCnGestionHoras).GroupBy(Function(o) o.tipo)
                Return View("index", d)
            End If
            Return View(d)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Contratopdf() As ActionResult
            Dim tra
            If Request("idtrabajador") Is Nothing Then
                tra = DB.GetTrabajadorEpsilon(Empresa, SimpleRoleProvider.GetId(), strCnSab, strCnIzaro, strCnEpsilon)
            Else
                tra = DB.GetTrabajadorEpsilonCodTra(Empresa, Request("idtrabajador"), strCnSab, strCnIzaro, strCnEpsilon)
            End If
            Dim doc As Document = New Document(iTextSharp.text.PageSize.A4)
            Dim totalfonts = FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts")
            Dim bf = FontFactory.GetFont("Times New Roman", 8.5F)
            Dim bf1 = New Font(Font.FontFamily.TIMES_ROMAN, 8.5, 1)
            Dim t1f = New Font(Font.FontFamily.TIMES_ROMAN, 11, 1)
            doc.SetMargins(0, 0, 0, 0)
            Dim strMS As New MemoryStream()
            Dim wrtTest = PdfWriter.GetInstance(doc, strMS)
            wrtTest.CloseStream = False
            doc.Open()
            Dim imgContrato1 = iTextSharp.text.Image.GetInstance(Server.MapPath("~") + "/Content/contrato.png")
            imgContrato1.ScalePercent(36)
            imgContrato1.Alignment = Image.UNDERLYING
            doc.Add(imgContrato1)

            Dim Col As New ColumnText(wrtTest.DirectContent)
            Dim nombre = tra.nombre + " " + tra.apellido1 + " " + tra.apellido2
            Col.SetSimpleColumn(44, 591, 296, 500)
            Dim p1, p2 As New Paragraph()
            p1.Add(New Chunk("ETA BESTETIK: ", bf1))
            p1.Add(New Chunk(nombre.ToString + " Jn/An, NAN zk. " + tra.nif.ToString.Trim(" ") + " duenak, " + tra.codtra.ToString + " bazkide zk.duna, adinez nagusia, eta bere izenez eta eskubideaz jokatzen duena (hemendik aurrera langilea),", bf))
            p1.SetLeading(0, 1.2)
            'p1.SetAlignment("Justify")
            Col.AddElement(p1)
            Col.Go()

            p2.Add(New Chunk("Y DE OTRA PARTE: ", bf1))
            p2.Add(New Chunk(" D. " + nombre.ToString + " socio número " + tra.codtra.ToString + ", mayor de edad, con D.N.I. " + tra.nif.ToString + ", quien actúa en su propio nombre y derecho (en adelante, el trabajador)", bf))
            p2.SetLeading(0, 1.2)
            'p2.SetAlignment("Justify")
            Col.SetSimpleColumn(315, 591, 570, 500)
            Col.AddElement(p2)
            Col.Go()
            doc.NewPage()

            Dim imgContrato2 = iTextSharp.text.Image.GetInstance(Server.MapPath("~") + "/Content/contrato2.png")
            imgContrato2.ScalePercent(36)

            imgContrato2.Alignment = Image.UNDERLYING
            doc.Add(imgContrato2)

            Col.SetSimpleColumn(170, 206, 250, 0)
            Dim p3, p4, p5 As New Paragraph()
            p3.Add(New Chunk(Now.ToString("yyyy/MM/dd"), bf))
            Col.AddElement(p3)
            Col.Go()
            Col.SetSimpleColumn(312, 196, 370, 0)
            p4.Add(New Chunk(Now.ToString("dd/MM/yyyy"), bf))
            Col.AddElement(p4)
            Col.Go()
            Col.SetSimpleColumn(325, 54, 550, 0)
            p5.Add(New Chunk(nombre.ToString, bf))
            Col.AddElement(p5)
            Col.Go()
            doc.Close()
            strMS.Seek(0, System.IO.SeekOrigin.Begin)
            Dim r = New FileStreamResult(strMS, "application/pdf")
            r.FileDownloadName = "kontratua.pdf"
            Return r
        End Function

        Function Realizada() As ActionResult
            Return View()
        End Function

        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function List() As ActionResult
            Return View(DB.GetListadoDiario(Empresa, strCnGestionHoras))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Delete(ByVal desde As Integer, ByVal hasta As Integer, ByVal codtra As Integer) As ActionResult
            Return View(DB.GetDistribucion(Empresa, desde, hasta, codtra, strCnGestionHoras))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function DeletePost(ByVal desde As Integer, ByVal hasta As Integer, ByVal codtra As Integer) As ActionResult
            DB.DeleteDistribucion(Empresa, desde, hasta, codtra, strCnGestionHoras)
            Return RedirectToAction("list")
        End Function
    End Class
End Namespace