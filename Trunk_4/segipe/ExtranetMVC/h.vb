Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Partial Public Class h
    Private Shared strCnSab = ConfigurationManager.ConnectionStrings("sab").ConnectionString
    Public Shared Sub SendEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String)
        Dim b = New Net.Mail.MailMessage()
        b.Subject = subject
        b.Body = body
        b.From = New Net.Mail.MailAddress("segipe@batz.es")
        b.To.Add(recipients)
        Dim smtp = New Net.Mail.SmtpClient(db.getMailServer(strCnSab))
        Dim SMTPUserInfo As New System.Net.NetworkCredential("tareas", "tareas123")
        smtp.UseDefaultCredentials = False
        smtp.Credentials = SMTPUserInfo
        smtp.Send(b)
    End Sub
    Public Shared Sub SendEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String, ByVal at As Net.Mail.Attachment)
        Dim b = New Net.Mail.MailMessage()
        b.Subject = subject
        b.Body = body
        b.IsBodyHtml = False
        b.Attachments.Add(at)
        b.From = New Net.Mail.MailAddress("segipe@batz.es")
        b.To.Add(recipients)
        Dim smtp = New Net.Mail.SmtpClient(db.getMailServer(strCnSab))
        Dim SMTPUserInfo As New System.Net.NetworkCredential("tareas", "tareas123")
        smtp.UseDefaultCredentials = False
        smtp.Credentials = SMTPUserInfo
        smtp.Send(b)
    End Sub
    Public Shared Function GeneratePdf(ByVal cabecera As Object, ByVal listOfLinea As List(Of Object)) As FileStreamResult
        Dim doc As Document = New Document(iTextSharp.text.PageSize.A4) ' Document(pgSize, 15, 15, 80, 0)
        Dim bf = New Font(Font.FontFamily.HELVETICA, 8, 0)
        Dim bfBold = New Font(Font.FontFamily.HELVETICA, 8, 1)
        Dim bfs = New Font(Font.FontFamily.HELVETICA, 6, 2)
        Dim strMS As New MemoryStream()
        Dim wrtTest = PdfWriter.GetInstance(doc, strMS)
        wrtTest.CloseStream = False
        doc.SetMargins(0, 0, 190, 150)
        Dim events = New MyPageEvents(cabecera, System.Web.HttpContext.Current.Server.MapPath("~/") + "/Content/header.png", bf, bfBold, bfs)
        doc.Open()
        wrtTest.PageEvent = events
        doc.Add(table1(listOfLinea, bf, bfBold))
        doc.Close()
        strMS.Seek(0, System.IO.SeekOrigin.Begin)
        Return New FileStreamResult(strMS, "application/pdf")
    End Function
    Private Shared Function table1(ByVal lineas As Object, ByVal bf As Font, ByVal bfBold As Font) As PdfPTable
        Dim bfT = New Font(Font.FontFamily.HELVETICA, 7, 0)
        Dim t As New PdfPTable(9)
        t.SetWidths(New Single(8) {5, 15, 37, 8, 6, 6, 8, 6, 9})
        t.HeaderRows = 1
        t.AddCell(td1("Nº", bfBold))
        t.AddCell(td1("Código", bfBold))
        t.AddCell(td1("Articulo", bfBold))
        t.AddCell(td1("OP-OP", bfBold))
        t.AddCell(td1("Marca", bfBold))
        t.AddCell(td1("Cant.", bfBold))
        t.AddCell(td1("Precio", bfBold))
        t.AddCell(td1("Desc.", bfBold))
        t.AddCell(td1("Importe", bfBold))
        For Each l As Object In lineas
            t.AddCell(td1(l.linea, bfT))
            t.AddCell(td1(l.articulo, bfT))
            Dim s = New System.Text.StringBuilder(l.descripciongcarticu.ToString)
            If l.observacion2.length > 0 Then
                s.Append(Environment.NewLine) : s.Append(l.observacion2)
            End If
            s.Append(Environment.NewLine) : s.append(l.desc.ToString)
            If l.observacion1.length > 0 Then
                s.Append(Environment.NewLine) : s.Append("Material: ") : s.Append(l.observacion1)
            End If
            If Not String.IsNullOrEmpty(l.ref_prov) Then
                s.Append(Environment.NewLine) : s.Append("Ref_Prov: ") : s.Append(l.ref_prov)
            End If

            If l.Diametro <> 0 Or l.Largo <> 0 Or l.Ancho <> 0 Or l.Grueso <> 0 Then
                s.Append(Environment.NewLine) : s.Append("Diametro: ") : s.Append(l.diametro) : s.Append(" Largo: ") : s.Append(l.largo)
                s.Append(" Ancho: ") : s.Append(l.ancho) : s.Append(" Grueso: ") : s.Append(l.grueso)
            End If
            If l.norma.length > 0 Then
                s.Append(Environment.NewLine) : s.Append(" Norma: ") : s.Append(l.norma)
            End If
            If l.comenta.length > 0 Then
                s.Append(Environment.NewLine) : s.Append(l.comenta)
            End If
            If l.descripcion2gcarticu.length > 0 Then
                s.Append(Environment.NewLine) : s.Append(l.descripcion2gcarticu)
            End If

            t.AddCell(td1(s.ToString, bfT))
            t.AddCell(td1(l.numord.ToString + " - " + l.numope.ToString, bfT))
            t.AddCell(td1(l.marca, bfT))
            t.AddCell(td1(l.cantidad, bfT))
            t.AddCell(td1(CDec(l.precio).ToString("###,###.##") + "€", bfT))
            t.AddCell(td1(l.descuento.ToString + "%", bfT))
            t.AddCell(td1(CDec(l.precio * l.cantidad - (l.precio * l.cantidad * l.descuento / 100)).ToString("###,###.##") + "€", bfT))
        Next

        Return t
    End Function
    Private Shared Function td1(ByVal s As String, ByVal bf As Font) As PdfPCell
        Dim c = New PdfPCell(New Phrase(s, bf))
        Return c
    End Function
End Class
Public Class MyPageEvents
    Inherits PdfPageEventHelper
    Private cabecera, img, bf, bfBold, bfs
    Public Sub New(ByVal cab As Object, ByVal img_ As String, ByVal bf_ As Font, ByVal bfBold_ As Font, ByVal bfs_ As Font)
        cabecera = cab : img = img_ : bf = bf_ : bfBold = bfBold_ : bfs = bfs_
    End Sub
    Public Overrides Sub OnEndPage(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document)
        Dim dc = writer.DirectContent
        Dim Col As New ColumnText(dc)
        Col.SetSimpleColumn(30, document.PageSize.Height - 20, document.PageSize.Width - 30, 10)
        Dim imgPdf = iTextSharp.text.Image.GetInstance(img.ToString)
        imgPdf.ScalePercent(((document.PageSize.Width - 60) / imgPdf.Width) * 100)
        Col.AddElement(imgPdf)
        Col.Go()
        Col.SetSimpleColumn(0, document.PageSize.Height - 90, document.PageSize.Width, 50)
        Col.AddElement(table0(cabecera, bf, bfBold))
        Col.Go()
        Col.SetSimpleColumn(0, 150, document.PageSize.Width, 50)

        Col.AddElement(tableObservaciones(cabecera, bf, bfBold))
        Col.AddElement(New Paragraph(New Phrase(cabecera.pedido.comenta.ToString, bf)))
        Col.Go()
        Col.SetSimpleColumn(30, 90, document.PageSize.Width, 50)
        Col.AddElement(New Phrase("El proveedor/subcontratista deberá consultar las normas y especificaciones necesarias a través de la EXTRANET de BATZ para la ejecución de sus trabajos y realizar éstos según sus contenidos.", bfs))
        Col.AddElement(New Phrase("El vendedor declara conocer y aceptar en su integridad el contenido de las condiciones de compra publicadas en la EXTRANET DE BATZ.", bfs))
        Col.AddElement(New Phrase("La vigencia de los contenidos de los documentos sólo queda garantizada en su consulta a través de la extranet", bfs))
        Col.AddElement(New Phrase("Las copias o descargas de dichos documentos pueden quedar obsoletas en el momento en que se realice cualquier modificación de los contenidos ", bfs))
        Col.Go()
        MyBase.OnEndPage(writer, document)
    End Sub
    Private Function table0(ByVal cabecera As Object, ByVal bf As Font, ByVal bfBold As Font) As PdfPTable
        Dim t As New PdfPTable(3)
        t.SetWidths(New Single(2) {40, 30, 30})

        t.AddCell(td0("Proveedor", bfBold))
        t.AddCell(td0("Pedido", bfBold))
        t.AddCell(td0("Fecha Entrega", bfBold))

        t.AddCell(td0(cabecera.direccion.proveedor.ToString, bf))
        t.AddCell(td0(cabecera.pedido.pedido, bf))
        t.AddCell(td0(cabecera.pedido.fEntrega, bf))

        t.AddCell(td0(cabecera.direccion.direccion.ToString, bf))
        t.AddCell(td0("Proyecto", bfBold))
        t.AddCell(td0("Fecha pedido", bfBold))

        t.AddCell(td0(cabecera.direccion.localidad.ToString, bf))
        t.AddCell(td0(cabecera.pedido.proyecto, bf))
        t.AddCell(td0(cabecera.pedido.fEntrega, bf))

        t.AddCell(td0(cabecera.direccion.provincia.ToString.ToString, bf))
        t.AddCell(td0("Respondable", bfBold))
        t.AddCell(td0("Fecha salida", bfBold))

        If String.IsNullOrEmpty(cabecera.pedido.proveedorEnvio) Then
            t.AddCell(td0("", bf))
        Else
            t.AddCell(td0("Direccion de entrega", bfBold))
        End If
        t.AddCell(td0(cabecera.pedido.responsable, bf))
        t.AddCell(td0(cabecera.pedido.fLanzamiento, bf))

        If String.IsNullOrEmpty(cabecera.pedido.proveedorEnvio) Then
            t.AddCell(td0("", bf))
        Else
            t.AddCell(td0(cabecera.direccionEnvio.nombreEnvio, bf))
        End If
        t.AddCell(td0("N. Troquel", bfBold))
        t.AddCell(td0("Descarga", bfBold))

        If String.IsNullOrEmpty(cabecera.pedido.proveedorEnvio) Then
            t.AddCell(td0("", bf))
        Else
            t.AddCell(td0(cabecera.direccionEnvio.domicilioEnvio, bf))
        End If
        t.AddCell(td0(cabecera.pedido.plano, bf))
        t.AddCell(td0(cabecera.pedido.puerta, bf))

        If String.IsNullOrEmpty(cabecera.pedido.proveedorEnvio) Then
            t.AddCell(td0("", bf))
        Else
            t.AddCell(td0(cabecera.direccionEnvio.distritoEnvio + ", " + cabecera.direccionEnvio.localidadEnvio, bf))
        End If
        t.AddCell(td0("Ref. pieza", bfBold))
        t.AddCell(td0("Forma de pago", bfBold))

        If String.IsNullOrEmpty(cabecera.pedido.proveedorEnvio) Then
            t.AddCell(td0("", bf))
        Else
            t.AddCell(td0(cabecera.direccionEnvio.provinciaEnvio + " Tlfno:" + cabecera.direccionEnvio.telefonoEnvio, bf))
        End If
        t.AddCell(td0(cabecera.pedido.ref, bf))
        t.AddCell(td0(cabecera.pedido.pago, bf))

        If String.IsNullOrEmpty(cabecera.pedido.proveedorEnvio) Then
            t.AddCell(td0("", bf))
        Else
            t.AddCell(td0(cabecera.direccionEnvio.paisEnvio, bf))
        End If
        t.AddCell(td0("", bf))
        t.AddCell(td0("", bf))

        Return t
    End Function
    Private Function tableObservaciones(ByVal cabecera As Object, ByVal bf As Font, ByVal bfBold As Font) As PdfPTable
        Dim t As New PdfPTable(2)
        t.SetWidths(New Single(1) {20, 80})
        If cabecera.observaciones.count = 0 Then
            Return t
        End If
        t.AddCell(td0("Observaciones", bfBold))
        t.AddCell(td0("", bfBold))
        For Each o As Object In cabecera.observaciones
            t.AddCell(td0(o.nombre.ToString, bf))
            t.AddCell(td0(o.texto.ToString, bf))
        Next
        Return t
    End Function

    Private Shared Function td0(ByVal s As String, ByVal bf As Font) As PdfPCell
        Dim c = New PdfPCell(New Phrase(s, bf)) : c.BorderWidth = 0 : c.Padding = 0.3
        Return c
    End Function
End Class

