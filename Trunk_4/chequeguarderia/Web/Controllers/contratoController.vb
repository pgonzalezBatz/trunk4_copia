Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Namespace web

    Public Class contratoController
        Inherits System.Web.Mvc.Controller

        Private Const Empresa = 1
        Dim strCnGestionHoras = ConfigurationManager.ConnectionStrings("gestionhoras").ConnectionString
        Dim strCnIzaro = ConfigurationManager.ConnectionStrings("izaro").ConnectionString
        Dim strCnEpsilon = ConfigurationManager.ConnectionStrings("epsilon").ConnectionString
        Dim strCnSab = ConfigurationManager.ConnectionStrings("sab").ConnectionString

        <SimpleRoleProvider(Role.administracion)>
        Function Index() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(Role.administracion)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Index(ByVal idtrabajador As String) As ActionResult
            If idtrabajador.Length = 0 Then
                Return View()
            End If
            If db.FirmadoContrato(idtrabajador, strCnGestionHoras) Then
                Return RedirectToAction("firmado")
            Else
                ViewData("idtrabajador") = idtrabajador
                Return View("firmar")
            End If
        End Function
        <SimpleRoleProvider(Role.administracion)>
        Function Firmado() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(Role.administracion)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Firmar(ByVal idtrabajador As String, ByVal firmado As String) As ActionResult
            If firmado IsNot Nothing Then
                db.SaveFirmaContrato(idtrabajador, strCnGestionHoras)
                Return RedirectToAction("index")
            Else
                ViewData("idtrabajador") = idtrabajador
                Return View("firmar")
            End If

        End Function
        <SimpleRoleProvider(Role.normal, Role.administracion)>
        Function contratopdf() As ActionResult
            Dim tra
            If Request("idtrabajador") Is Nothing Then
                tra = db.GetTrabajadorEpsilon(Empresa, SimpleRoleProvider.GetId(), strCnSab, strCnIzaro, strCnEpsilon)
            Else
                tra = db.GetTrabajadorEpsilonCodTra(Empresa, Request("idtrabajador"), strCnSab, strCnIzaro, strCnEpsilon)
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
            Dim imgContrato1 = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "/Content/contrato.png")
            imgContrato1.ScalePercent(36)
            imgContrato1.Alignment = Image.UNDERLYING
            doc.Add(imgContrato1)

            Dim Col As New ColumnText(wrtTest.DirectContent)
            Dim nombre = tra.nombre + " " + tra.apellido1 + " " + tra.apellido2
            Col.SetSimpleColumn(44, 591, 296, 500)
            Dim p1, p2 As New iTextSharp.text.Paragraph()
            p1.Add(New Chunk("ETA BESTETIK: ", bf1))
            p1.Add(New Chunk(nombre.ToString + " Jn/An, NAN zk. " + tra.nif.ToString.Trim(" ") + " duenak, " + tra.codtra.ToString + " bazkide zk.duna, adinez nagusia, eta bere izenez eta eskubideaz jokatzen duena (hemendik aurrera langilea),", bf))
            p1.SetLeading(0, 1.2)
            p1.Alignment = Element.ALIGN_RIGHT
            'p1.SetAl("Justify")
            Col.AddElement(p1)
            Col.Go()

            p2.Add(New Chunk("Y DE OTRA PARTE: ", bf1))
            p2.Add(New Chunk(" D. " + nombre.ToString + " socio número " + tra.codtra.ToString + ", mayor de edad, con D.N.I. " + tra.nif.ToString + ", quien actúa en su propio nombre y derecho (en adelante, el trabajador)", bf))
            p2.SetLeading(0, 1.2)
            p2.Alignment = Element.ALIGN_RIGHT
            'p2.SetAlignment("Justify")
            Col.SetSimpleColumn(315, 591, 570, 500)
            Col.AddElement(p2)
            Col.Go()
            doc.NewPage()

            Dim imgContrato2 = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "/Content/contrato2.png")
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
            Dim r = New FileStreamResult(strMS, "application/pdf") With {
                .FileDownloadName = "kontratua.pdf"
            }
            Return r
        End Function
    End Class
End Namespace