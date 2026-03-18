Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Security
Partial Public Class h


    Public Shared Function MySelectList(Of O)(ByVal items As IEnumerable(Of O), ByVal f As Func(Of O, Mvc.SelectListItem)) As List(Of Mvc.SelectListItem)
        Dim lst As New List(Of Mvc.SelectListItem)
        For Each i In items
            lst.Add(f(i))
        Next
        Return lst
    End Function
    Public Shared Sub SendEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String)
        Dim b = New Net.Mail.MailMessage()
        b.Subject = subject
        b.Body = body
        b.From = New Net.Mail.MailAddress("sas2@batz.es")
        b.To.Add(recipients)
        Dim smtp = New Net.Mail.SmtpClient(getMailServer())
        smtp.Send(b)
    End Sub
    Public Shared Sub SendEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String, ByVal at As Net.Mail.Attachment)
        Dim b = New Net.Mail.MailMessage()
        b.Subject = subject
        b.Body = body
        b.IsBodyHtml = False
        b.Attachments.Add(at)
        b.From = New Net.Mail.MailAddress("sas2@batz.es")
        b.To.Add(recipients)
        Dim smtp = New Net.Mail.SmtpClient(getMailServer())
        'Dim smtp = New Net.Mail.SmtpClient("ARANEKOARRIA.elkarekin.com")
        smtp.Send(b)
    End Sub
    Public Shared Function con(ByVal idSab As Integer) As String
        Select Case DBAccess.GetPlanta(idSab, ConfigurationManager.ConnectionStrings("SAB").ConnectionString)
            Case 1
                Return ConfigurationManager.ConnectionStrings("sas").ConnectionString
            Case 2
                Return ConfigurationManager.ConnectionStrings("sasmbtooling").ConnectionString
        End Select
        Throw New Exception("No existe conexion para la planta del usuario: " + idSab.ToString)
    End Function

    Public Shared Function getMailServer() As String
        Dim strCn As String = ConfigurationManager.ConnectionStrings("sab").ConnectionString
        Return DBAccess.getMailServer(strCn)
    End Function

    Public Shared Function GetSchemaFromStrCn(ByVal strCn As String) As String
        Return "XBAT"
    End Function
    Public Shared Function map(Of E1, E2)(ByVal l As IEnumerable(Of E1), ByVal f As Func(Of E1, E2)) As List(Of E2)
        Dim l2 As New List(Of E2)
        For Each e In l
            l2.Add(f(e))
        Next
        Return l2
    End Function

    Public Shared Function GetListOfDefaultEmpresaFromStrCn(ByVal strCn As String) As List(Of Object)
        If Regex.IsMatch(strCn, "Id=sas;") Then
            Return map(Of String, Object)(ConfigurationManager.AppSettings("idsigorre").Split(";").ToList(), Function(s) New With {.id = s.Split("=")(0), .nombre = s.Split("=")(1)})
        End If
        If Regex.IsMatch(strCn, "Id=sasmbtooling;") Then
            Return map(Of String, Object)(ConfigurationManager.AppSettings("idsmbtooling").Split(";").ToList(), Function(s) New With {.id = s.Split("=")(0), .nombre = s.Split("=")(1)})
        End If
        Throw New Exception("No existe Empresa de salida para la conexion: " + strCn)
    End Function

    Public Shared Function GetCount(ByVal page As String, ByVal idUsuario As Integer) As Integer
        Dim strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString
        Select Case page
            Case "pm"
                Return DBAccess.GetListPreMovimiento(strCn).Count
            Case "r"
                Return DBAccess.GetListOfRecogidas(strCn).Count
            Case "mm"
                Return DBAccess.GetListMovimientos(strCn).Count
            Case "am"
                Return DBAccess.GetListOfAgrupaciones(strCn).Count
            Case "a"
                Return DBAccess.GetListAlbaran(strCn).Count
            Case "v"
                Return DBAccess.GetListEnvio(False, "", strCn).Count
            Case "vt"
                Return DBAccess.getListOfVijeTaxi(strCn).Count
        End Select
        Throw New Exception("No hay razón para llegar hasta este punto")
    End Function
    Public Shared Function ModifiQueryString(ByVal oldCollection As Specialized.NameValueCollection, ByVal ParamArray replacements() As KeyValuePair(Of String, String)) As String
        Dim copy As New Dictionary(Of String, Object)
        oldCollection.CopyTo(copy)
        If (replacements IsNot Nothing) Then
            For Each rp As KeyValuePair(Of String, String) In replacements
                copy.Remove(rp.Key)
                copy.Add(rp.Key, rp.Value)
            Next
        End If
        Dim str As New Text.StringBuilder()
        For Each e In copy.AsEnumerable
            str.Append("&")
            str.Append(e.Key)
            str.Append("=")
            str.Append(e.Value)
        Next
        str.Replace("&", "?", 0, 1)
        Return str.ToString()
    End Function
    Public Shared Function RemoveFromQueryString(ByVal oldCollection As Specialized.NameValueCollection, ByVal ParamArray toRemove() As String) As String
        Dim filtered As NameValueCollection = New NameValueCollection(oldCollection)
        If (toRemove IsNot Nothing) Then
            For Each rp As String In toRemove
                filtered.Remove(rp)
            Next
        End If

        Dim str As New Text.StringBuilder()
        For Each e In filtered
            str.Append("&")
            str.Append(e)
            str.Append("=")
            str.Append(filtered.Item(e))
        Next
        If str.Length = 0 Then
            Return ""
        End If
        str.Replace("&", "?", 0, 1)
        Return str.ToString()
        '    Dim copy As New List(Of Object)
        '    oldCollection.CopyTo(copy)
        '    If (toRemove IsNot Nothing) Then
        '        For Each rp As String In toRemove
        '            copy.Remove(rp)
        '        Next
        '    End If
        '    If copy.Count = 0 Then
        '        Return ""
        '    End If
        '    Dim str As New Text.StringBuilder()
        '    For Each e In copy
        '        str.Append("&")
        '        str.Append(e.Key)
        '        str.Append("=")
        '        str.Append(e.Value)
        '    Next
        '    str.Replace("&", "?", 0, 1)
        '    Return str.ToString()
    End Function


    Public Shared Function GetRoleRedirection(ByVal role As Integer) As String
        If (role And 1) = 1 Then
            Return "movimientosmaterial"
        End If
        If (role And 2) = 2 Then
            Return "movimientosmaterial"
        End If
        If (role And 4) = 4 Then
            Return "transporte"
        End If
        Throw New SecurityException
    End Function
    Public Class BultoPageEvent
        Inherits PdfPageEventHelper
        Public fHeaderImage As Action(Of ColumnText)
        Public fTopTable As Action(Of ColumnText)
        Public Overrides Sub OnEndPage(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document)
            Dim bf = New Font(Font.FontFamily.HELVETICA, 5, 2)
            Dim dc = writer.DirectContent
            Dim Col As New ColumnText(dc)
            Col.SetSimpleColumn(30, document.PageSize.Height - 275, 585, document.PageSize.Height - 10)
            fHeaderImage(Col)
            Col.Go()
            Col.SetSimpleColumn(30, document.PageSize.Height - 275, 585, document.PageSize.Height - 100)

            fTopTable(Col)

            Col.Go()
            Col.SetSimpleColumn(30, -200, 540, 70)
            Col.AddElement(New Paragraph("* Nota: A partir de la fecha de recepción, 1 semana se plazo para posibles reclamaciones.", bf))
            Col.AddElement(New Paragraph("* En caso de no haber reclamaciones, se interpreta que el envío es correcto. Quedando Batz S. Coop. exenta de ninguna responsabilidad.", bf))
            Col.AddElement(New Paragraph("* Este albarán deberá ser enviado a Batz S. Coop debidamente sellado y con la fecha de recepción.", bf))
            Col.AddElement(New Paragraph("* Documento de control para el transporte de mercancias por carretera (orden FOM/2861/2012 de 13 de diciembre). Según el artículo del RD782/98 el responsable de residuo del envase usado para su correcta gestión ambiental, será el poseedor final.", bf))

            Col.Go()
            MyBase.OnEndPage(writer, document)
        End Sub
    End Class
    Public Shared Function bultoPdf(direccion As Object, id As Integer, lstMovimientos As List(Of Movimiento), peso As Decimal)
        Dim strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString
        Dim doc As Document = New Document(iTextSharp.text.PageSize.A4)
        Dim bf = New Font(Font.FontFamily.HELVETICA, 9, 0)
        Dim bf1 = New Font(Font.FontFamily.HELVETICA, 9, 1)
        doc.SetMargins(30, 10, 250, 100)
        Dim strMS As New MemoryStream()
        Dim img = iTextSharp.text.Image.GetInstance(HttpContext.Current.Request.PhysicalApplicationPath + "/Content/header.png")
        Dim par1 = New Paragraph("Nota: A partir de la fecha de recepción, 1 semana de plazo para posibles reclamaciones.", New Font(Font.FontFamily.HELVETICA, 6, 0))
        par1.Alignment = 1
        Dim wrtTest = PdfWriter.GetInstance(doc, strMS)
        wrtTest.CloseStream = False
        Dim events = New BultoPageEvent()
        wrtTest.PageEvent = events
        doc.Open()
        events.fHeaderImage = Sub(col) col.AddElement(img)
        events.fTopTable = Sub(col)
                               Dim t1 = New pdf.PdfPTable(2)
                               t1.HorizontalAlignment = pdf.PdfPCell.ALIGN_LEFT

                               t1.SetWidthPercentage(New Single(1) {400.0F, 160.0F}, PageSize.A4)

                               Dim c21 = New pdf.PdfPCell(New Paragraph(lstMovimientos.First.NombreProveedor, bf1)) : c21.BorderWidthBottom = 0 : c21.PaddingLeft = 100 : t1.AddCell(c21)
                               Dim c22 = New pdf.PdfPCell(New Paragraph("Bulto zenbakia", bf)) : c22.BorderWidthBottom = 0 : t1.AddCell(c22)

                               Dim c31 = New pdf.PdfPCell(New Paragraph(direccion.calle, bf1)) : c31.BorderWidthBottom = 0 : c31.BorderWidthTop = 0 : c31.PaddingLeft = 100 : t1.AddCell(c31)
                               Dim c32 = New pdf.PdfPCell(New Paragraph("Numbero de bulto", bf)) : c32.BorderWidthBottom = 0 : c32.BorderWidthTop = 0 : t1.AddCell(c32)

                               Dim c41 = New pdf.PdfPCell(New Paragraph(direccion.CodigoPostal + ", " + direccion.Poblacion, bf1)) : c41.BorderWidthBottom = 0 : c41.BorderWidthTop = 0 : c41.PaddingLeft = 100 : t1.AddCell(c41)
                               Dim c42 = New pdf.PdfPCell(New Paragraph("Package identification", bf)) : c42.BorderWidthBottom = 0 : c42.BorderWidthTop = 0 : t1.AddCell(c42)

                               Dim c51 = New pdf.PdfPCell(New Paragraph(direccion.Provincia, bf1)) : c51.BorderWidthBottom = 0 : c51.BorderWidthTop = 0 : c51.PaddingLeft = 100 : t1.AddCell(c51)
                               Dim c52 = New pdf.PdfPCell(New Paragraph(id.ToString, bf1)) : c52.Rowspan = 3 : c52.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : c52.VerticalAlignment = pdf.PdfPCell.ALIGN_MIDDLE : t1.AddCell(c52)

                               Dim c61 = New pdf.PdfPCell(New Paragraph(direccion.Pais, bf1)) : c61.BorderWidthBottom = 0 : c61.BorderWidthTop = 0 : c61.PaddingLeft = 100 : t1.AddCell(c61)

                               Dim c71 = New pdf.PdfPCell(New Paragraph("CIF " + lstMovimientos.First.cifProveedor, bf1)) : c71.BorderWidthTop = 0 : c71.PaddingLeft = 100 : t1.AddCell(c71)
                               col.AddElement(t1)
                           End Sub
        Dim f = Function()
                    Dim bf0 = New Font(Font.FontFamily.HELVETICA, 8, 0)
                    Dim t = New pdf.PdfPTable(5)
                    t.HorizontalAlignment = pdf.PdfPCell.ALIGN_LEFT
                    t.SetWidthPercentage(New Single(4) {20.0F, 70.0F, 350.0F, 50.0F, 70.0F}, PageSize.A4)

                    Dim c00 = New pdf.PdfPCell(New Paragraph("", bf1)) : c00.BorderWidthBottom = 0 : t.AddCell(c00)
                    Dim c01 = New pdf.PdfPCell(New Paragraph("Pisua (Kg)", bf1)) : c01.BorderWidthBottom = 0 : t.AddCell(c01)
                    Dim c02 = New pdf.PdfPCell(New Paragraph("Merkantziaren deskribapena", bf1)) : c02.BorderWidthBottom = 0 : t.AddCell(c02)
                    Dim c03 = New pdf.PdfPCell(New Paragraph("Kopurua", bf1)) : c03.BorderWidthBottom = 0 : t.AddCell(c03)
                    Dim c04 = New pdf.PdfPCell(New Paragraph("Erreferentzia", bf1)) : c04.BorderWidthBottom = 0 : t.AddCell(c04)

                    Dim c10 = New pdf.PdfPCell(New Paragraph("", bf1)) : c10.BorderWidthTop = 0 : c10.BorderWidthBottom = 0 : t.AddCell(c10)
                    Dim c11 = New pdf.PdfPCell(New Paragraph("Peso (Kg)", bf1)) : c11.BorderWidthTop = 0 : c11.BorderWidthBottom = 0 : t.AddCell(c11)
                    Dim c12 = New pdf.PdfPCell(New Paragraph("Detalle de la mercancia", bf1)) : c12.BorderWidthTop = 0 : c12.BorderWidthBottom = 0 : t.AddCell(c12)
                    Dim c13 = New pdf.PdfPCell(New Paragraph("Cantidad", bf1)) : c13.BorderWidthTop = 0 : c13.BorderWidthBottom = 0 : t.AddCell(c13)
                    Dim c14 = New pdf.PdfPCell(New Paragraph("Referencia", bf1)) : c14.BorderWidthTop = 0 : c14.BorderWidthBottom = 0 : t.AddCell(c14)

                    Dim c20 = New pdf.PdfPCell(New Paragraph("", bf1)) : c20.BorderWidthTop = 0 : t.AddCell(c20)
                    Dim c21 = New pdf.PdfPCell(New Paragraph("Weight (Kg)", bf1)) : c21.BorderWidthTop = 0 : t.AddCell(c21)
                    Dim c22 = New pdf.PdfPCell(New Paragraph("Description of goods", bf1)) : c22.BorderWidthTop = 0 : t.AddCell(c22)
                    Dim c23 = New pdf.PdfPCell(New Paragraph("Quantity", bf1)) : c23.BorderWidthTop = 0 : t.AddCell(c23)
                    Dim c24 = New pdf.PdfPCell(New Paragraph("Reference", bf1)) : c24.BorderWidthTop = 0 : t.AddCell(c24)
                    Dim i = 1
                    Dim c50 = New pdf.PdfPCell(New Paragraph(i.ToString, bf0)) : c50.Rowspan = lstMovimientos.Count : c50.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : t.AddCell(c50)
                    Dim c51 = New pdf.PdfPCell(New Paragraph(peso.ToString, bf0)) : c51.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : c51.Rowspan = lstMovimientos.Count : t.AddCell(c51)
                    For Each m In lstMovimientos
                        Dim strMarca = New Text.StringBuilder()
                        If m.Marca = "ZZZZ" Or m.Marca.Length = 0 Then
                            strMarca.Append(m.Material)
                        Else
                            strMarca.Append(m.Marca)
                            strMarca.Append(" ")
                            strMarca.Append(m.Material)
                        End If
                        If m.Otros IsNot Nothing AndAlso m.Otros.tipolista = "1" Then
                            strMarca.Append(" (")
                            strMarca.Append(m.Peso.ToString)
                            strMarca.Append("Kg)")
                        End If
                        If Not String.IsNullOrEmpty(m.Observacion) AndAlso m.Observacion.First = "#"c Then
                            strMarca.Append(m.Observacion.Substring(1))
                        End If
                        Dim C52 = New pdf.PdfPCell(New Paragraph(strMarca.ToString, bf0)) : t.AddCell(C52)
                        Dim C53 = New pdf.PdfPCell(New Paragraph(m.Cantidad, bf0)) : C53.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : t.AddCell(C53)
                        Dim C54 = New pdf.PdfPCell(New Paragraph(m.Numord.ToString + " " + m.Numope.ToString, bf0)) : C54.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : t.AddCell(C54)
                    Next
                    If lstMovimientos.Count < 28 Then
                        For j = 1 To 28 - (lstMovimientos.Count Mod 28)
                            Dim complete0 = New pdf.PdfPCell(New Paragraph(" ", bf)) : complete0.BorderWidthTop = 0 : complete0.BorderWidthBottom = 0 : t.AddCell(complete0)
                            t.DefaultCell.BorderWidthBottom = 0
                            t.DefaultCell.BorderWidthTop = 0
                            t.CompleteRow()
                        Next
                    End If
                    Return t
                End Function

        doc.Add(f())
        doc.Close()
        strMS.Seek(0, System.IO.SeekOrigin.Begin)
        Return strMS
    End Function
    Public Shared Function albaranSimple(idAlbaran As Integer)
        Dim strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString
        Dim a = DBAccess.GetAlbaran(idAlbaran, strCn)
        Dim doc As Document = New Document(iTextSharp.text.PageSize.A4)
        Dim bf = New Font(Font.FontFamily.HELVETICA, 9, 0)
        Dim bf1 = New Font(Font.FontFamily.HELVETICA, 9, 1)
        doc.SetMargins(30, 10, 250, 100)
        Dim strMS As New MemoryStream()
        Dim img = iTextSharp.text.Image.GetInstance(HttpContext.Current.Request.PhysicalApplicationPath + "/Content/header.png")
        Dim par1 = New Paragraph("Nota: A partir de la fecha de recepción, 1 semana de plazo para posibles reclamaciones.", New Font(Font.FontFamily.HELVETICA, 6, 0))
        par1.Alignment = 1
        Dim wrtTest = PdfWriter.GetInstance(doc, strMS)
        wrtTest.CloseStream = False
        Dim events = New MyPageEvents(img, bf, bf1, Nothing)

        doc.Open()
        wrtTest.PageEvent = events
        If idAlbaran > 0 Then
            events.a = a
            events.userempresa = h.GetListOfDefaultEmpresaFromStrCn(strCn).First.id
            doc.Add(MainTable(a, bf, bf1))
            Dim pTotal As New Paragraph()
            pTotal.Add(New Phrase("Peso total:", bf1))
            pTotal.Add(New Phrase(a.ListOfAgrupacion.Sum(Function(g) h.CalcularPesoTotal(g)).ToString + " Kg", bf))
            doc.Add(pTotal)
            doc.Add(New Paragraph("Oharrak / Observaciones / Remarks:", bf1))
            doc.Add(New Paragraph(a.Observaciones, bf))
            doc.NewPage()
            events.a = Nothing
        End If
        doc.Close()
        strMS.Seek(0, System.IO.SeekOrigin.Begin)
        Return strMS
    End Function
    Public Shared Function albaranPdf(idenvio As Integer, idAlbaran As Integer, idRecogida As Integer) As MemoryStream
        Dim strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString
        Dim v = DBAccess.GetEnvio(idenvio, strCn)
        Dim doc As Document = New Document(iTextSharp.text.PageSize.A4)
        Dim bf = New Font(Font.FontFamily.HELVETICA, 9, 0)
        Dim bf1 = New Font(Font.FontFamily.HELVETICA, 9, 1)
        doc.SetMargins(30, 10, 250, 100)
        Dim strMS As New MemoryStream()
        Dim img = iTextSharp.text.Image.GetInstance(HttpContext.Current.Request.PhysicalApplicationPath + "/Content/header.png")
        Dim par1 = New Paragraph("Nota: A partir de la fecha de recepción, 1 semana de plazo para posibles reclamaciones.", New Font(Font.FontFamily.HELVETICA, 6, 0))
        par1.Alignment = 1
        Dim wrtTest = PdfWriter.GetInstance(doc, strMS)
        wrtTest.CloseStream = False
        img.ScalePercent((doc.PageSize.Width - 160) / img.Width * 100)

        Dim events = New MyPageEvents(img, bf, bf1, v)

        doc.Open()
        wrtTest.PageEvent = events
        If idAlbaran > 0 Then
            Dim a = v.ListOfAlbaran.Find(Function(a1) a1.Id = idAlbaran)
            events.a = a
            events.userempresa = h.GetListOfDefaultEmpresaFromStrCn(strCn).First.id
            doc.Add(MainTable(a, bf, bf1))
            Dim pTotal As New Paragraph()
            pTotal.Add(New Phrase("Peso total:", bf1))
            pTotal.Add(New Phrase(a.ListOfAgrupacion.Sum(Function(g) h.CalcularPesoTotal(g)).ToString + " Kg", bf))
            doc.Add(pTotal)
            doc.Add(New Paragraph("Oharrak / Observaciones / Remarks:", bf1))
            doc.Add(New Paragraph(a.Observaciones, bf))
            doc.NewPage()
            events.a = Nothing
        End If
        If idRecogida > 0 Then
            Dim r = v.ListOfRecogida.Find(Function(r1) r1.Id = idRecogida)
            events.r = r
            doc.Add(tableListOfRecogidas(r, bf, bf1))
            doc.NewPage()
        End If
        doc.Close()
        strMS.Seek(0, System.IO.SeekOrigin.Begin)
        Return strMS
    End Function
    Public Shared Function viajePdf(ByVal id As Integer) As MemoryStream
        Dim strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString
        Dim v = DBAccess.GetEnvio(id, strCn)
        Dim doc As Document = New Document(iTextSharp.text.PageSize.A4)
        Dim bf = New Font(Font.FontFamily.HELVETICA, 9, 0)
        Dim bf1 = New Font(Font.FontFamily.HELVETICA, 9, 1)
        doc.SetMargins(30, 10, 250, 100)
        Dim strMS As New MemoryStream()
        Dim img = iTextSharp.text.Image.GetInstance(HttpContext.Current.Request.PhysicalApplicationPath + "/Content/header.png")
        Dim par1 = New Paragraph("Nota: A partir de la fecha de recepción, 1 semana de plazo para posibles reclamaciones.", New Font(Font.FontFamily.HELVETICA, 6, 0))
        par1.Alignment = 1
        Dim wrtTest = PdfWriter.GetInstance(doc, strMS)
        wrtTest.CloseStream = False
        img.ScalePercent((doc.PageSize.Width - 60) / img.Width * 100)
        Dim events = New MyPageEvents(img, bf, bf1, v)

        doc.Open()
        wrtTest.PageEvent = events
        For Each a In v.ListOfAlbaran
            events.a = a
            events.userempresa = h.GetListOfDefaultEmpresaFromStrCn(strCn).First.id
            doc.Add(MainTable(a, bf, bf1))
            Dim pTotal As New Paragraph()
            pTotal.Add(New Phrase("Peso total:", bf1))
            pTotal.Add(New Phrase(a.ListOfAgrupacion.Sum(Function(g) CalcularPesoTotal(g)).ToString + " Kg", bf))
            doc.Add(pTotal)
            doc.Add(New Paragraph("Oharrak / Observaciones / Remarks:", bf1))
            doc.Add(New Paragraph(a.Observaciones, bf))
            doc.NewPage()
        Next
        events.a = Nothing
        For Each r In v.ListOfRecogida
            events.r = r
            doc.Add(tableListOfRecogidas(r, bf, bf1))
            doc.NewPage()
        Next
        doc.Close()
        strMS.Seek(0, System.IO.SeekOrigin.Begin)
        Return strMS
    End Function
    Public Shared Function tableListOfRecogidas(ByVal r As Recogida, ByVal bf As Font, ByVal bf1 As Font)
        Dim bf0 = New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, 0)
        Dim t = New pdf.PdfPTable(4)
        t.HorizontalAlignment = pdf.PdfPCell.ALIGN_LEFT
        t.SetWidthPercentage(New Single(3) {20.0F, 90.0F, 90.0F, 360.0F}, PageSize.A4)
        Dim c00 = New pdf.PdfPCell(New Paragraph("", bf1)) : c00.BorderWidthBottom = 0 : t.AddCell(c00)
        Dim c01 = New pdf.PdfPCell(New Paragraph("OF : OP", bf1)) : c01.BorderWidthBottom = 0 : c01.HorizontalAlignment = PdfPCell.ALIGN_CENTER : c01.VerticalAlignment = PdfPCell.ALIGN_MIDDLE : c01.Rowspan = 3 : t.AddCell(c01)
        Dim c02 = New pdf.PdfPCell(New Paragraph("Pisua (Kg)", bf1)) : c02.BorderWidthBottom = 0 : t.AddCell(c02)
        Dim c03 = New pdf.PdfPCell(New Paragraph("Merkantziaren deskribapena", bf1)) : c03.BorderWidthBottom = 0 : t.AddCell(c03)

        Dim c10 = New pdf.PdfPCell(New Paragraph("", bf1)) : c10.BorderWidthTop = 0 : c10.BorderWidthBottom = 0 : t.AddCell(c10)
        Dim c12 = New pdf.PdfPCell(New Paragraph("Peso (Kg)", bf1)) : c12.BorderWidthTop = 0 : c12.BorderWidthBottom = 0 : t.AddCell(c12)
        Dim c13 = New pdf.PdfPCell(New Paragraph("Detalle de la mercancia", bf1)) : c13.BorderWidthTop = 0 : c13.BorderWidthBottom = 0 : t.AddCell(c13)

        Dim c20 = New pdf.PdfPCell(New Paragraph("", bf1)) : c20.BorderWidthTop = 0 : t.AddCell(c20)
        Dim c22 = New pdf.PdfPCell(New Paragraph("Weight (Kg)", bf1)) : c22.BorderWidthTop = 0 : t.AddCell(c22)
        Dim c23 = New pdf.PdfPCell(New Paragraph("Description of goods", bf1)) : c23.BorderWidthTop = 0 : t.AddCell(c23)

        Dim i = 1
        For Each g In r.ListOfOp
            Dim c50 = New pdf.PdfPCell(New Paragraph(i.ToString, bf0)) : c50.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : t.AddCell(c50)
            Dim C51 = New pdf.PdfPCell(New Paragraph(g.Numord.ToString + " : " + g.Numope.ToString, bf0)) : C51.HorizontalAlignment = PdfPCell.ALIGN_CENTER : t.AddCell(C51)
            Dim c52 = New pdf.PdfPCell(New Paragraph(g.Peso.ToString, bf0)) : c52.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : t.AddCell(c52)
            'If i = 1 And r.puerta.HasValue Then
            '    Dim pt = DBAccess.GetListPuerta(ConfigurationManager.ConnectionStrings("SAS").ConnectionString).First(Function(p) p.Value = r.puerta)
            '    Dim C53 = New pdf.PdfPCell(New Paragraph(r.Observacion + Environment.NewLine + "A entregar en " + pt.Text, bf0)) : C53.Rowspan = r.ListOfOp.Count : t.AddCell(C53)
            'Else
            Dim C53 = New pdf.PdfPCell(New Paragraph(r.Observacion, bf0)) : C53.Rowspan = r.ListOfOp.Count : t.AddCell(C53)
            'End If

            i = i + 1
        Next

        Return t
    End Function
    Public Shared Function MainTable(ByVal a As Albaran, ByVal bf As Font, ByVal bf1 As Font)
        Dim bf0 = New Font(Font.FontFamily.HELVETICA, 8, 0)
        Dim t = New pdf.PdfPTable(5)
        t.HeaderRows = 3
        t.HorizontalAlignment = pdf.PdfPCell.ALIGN_LEFT
        t.SetWidthPercentage(New Single(4) {20.0F, 70.0F, 350.0F, 50.0F, 70.0F}, PageSize.A4)

        Dim c00 = New pdf.PdfPCell(New Paragraph("", bf1)) : c00.BorderWidthBottom = 0 : t.AddCell(c00)
        Dim c01 = New pdf.PdfPCell(New Paragraph("Pisua (Kg)", bf1)) : c01.BorderWidthBottom = 0 : t.AddCell(c01)
        Dim c02 = New pdf.PdfPCell(New Paragraph("Merkantziaren deskribapena", bf1)) : c02.BorderWidthBottom = 0 : t.AddCell(c02)
        Dim c03 = New pdf.PdfPCell(New Paragraph("Kopurua", bf1)) : c03.BorderWidthBottom = 0 : t.AddCell(c03)
        Dim c04 = New pdf.PdfPCell(New Paragraph("Erreferentzia", bf1)) : c04.BorderWidthBottom = 0 : t.AddCell(c04)

        Dim c10 = New pdf.PdfPCell(New Paragraph("", bf1)) : c10.BorderWidthTop = 0 : c10.BorderWidthBottom = 0 : t.AddCell(c10)
        Dim c11 = New pdf.PdfPCell(New Paragraph("Peso (Kg)", bf1)) : c11.BorderWidthTop = 0 : c11.BorderWidthBottom = 0 : t.AddCell(c11)
        Dim c12 = New pdf.PdfPCell(New Paragraph("Detalle de la mercancia", bf1)) : c12.BorderWidthTop = 0 : c12.BorderWidthBottom = 0 : t.AddCell(c12)
        Dim c13 = New pdf.PdfPCell(New Paragraph("Cantidad", bf1)) : c13.BorderWidthTop = 0 : c13.BorderWidthBottom = 0 : t.AddCell(c13)
        Dim c14 = New pdf.PdfPCell(New Paragraph("Referencia", bf1)) : c14.BorderWidthTop = 0 : c14.BorderWidthBottom = 0 : t.AddCell(c14)

        Dim c20 = New pdf.PdfPCell(New Paragraph("", bf1)) : c20.BorderWidthTop = 0 : t.AddCell(c20)
        Dim c21 = New pdf.PdfPCell(New Paragraph("Weight (Kg)", bf1)) : c21.BorderWidthTop = 0 : t.AddCell(c21)
        Dim c22 = New pdf.PdfPCell(New Paragraph("Description of goods", bf1)) : c22.BorderWidthTop = 0 : t.AddCell(c22)
        Dim c23 = New pdf.PdfPCell(New Paragraph("Quantity", bf1)) : c23.BorderWidthTop = 0 : t.AddCell(c23)
        Dim c24 = New pdf.PdfPCell(New Paragraph("Reference", bf1)) : c24.BorderWidthTop = 0 : t.AddCell(c24)

        printNestedBultos(a.ListOfAgrupacion, t, 0, bf0)
        Return t
    End Function
    Public Shared Function CalcularPesoTotal(g As Agrupacion) As Decimal
        If g.children.Count = 0 Then
            Return g.Peso
        End If
        Return g.Peso + g.children.Sum(Function(gc) CalcularPesoTotal(gc))
    End Function
    Public Shared Sub printNestedBultos(lstG As IEnumerable(Of Agrupacion), t As PdfPTable, level As Integer, bf As Font)
        Dim lPadding = level * 10
        Dim i = 1
        For Each g In lstG
            Dim c0 As New PdfPCell, c1 As New PdfPCell, c2 As New PdfPCell, c3 As New PdfPCell, c4 As New PdfPCell
            Dim pg0 = New Paragraph(i.ToString, bf) : pg0.Alignment = pdf.PdfPCell.ALIGN_CENTER
                Dim pg1 = New Paragraph(CalcularPesoTotal(g).ToString + " Kg", bf) : pg1.Alignment = pdf.PdfPCell.ALIGN_CENTER
            c0.AddElement(pg0) : c1.AddElement(pg1)

            Dim msg As New StringBuilder
            msg.Append("Numero de Bulto ") : msg.Append(g.Id.ToString())
            c2.AddElement(New Paragraph(msg.ToString.PadLeft(msg.Length + lPadding, " "), bf)) : c3.AddElement(New Paragraph(Environment.NewLine, bf)) : c4.AddElement(New Paragraph(Environment.NewLine, bf))
            printListMovimiento(g.ListOfMovimiento, lPadding + 5, c2, c3, c4, bf)
            printNestedBultos(g.children, c0, c1, c2, c3, c4, level + 1, bf)
            t.AddCell(c0) : t.AddCell(c1) : t.AddCell(c2) : t.AddCell(c3) : t.AddCell(c4)
            i = i + 1
        Next
    End Sub
    Public Shared Sub printNestedBultos(lstG As IEnumerable(Of Agrupacion), c0 As PdfPCell, c1 As PdfPCell, c2 As PdfPCell, c3 As PdfPCell, c4 As PdfPCell, level As Integer, bf As Font)
        Dim lPadding = level * 10
        Dim i = 1
        For Each g In lstG
            Dim msg As New StringBuilder
            msg.Append("Numero de Bulto ") : msg.Append(g.Id.ToString())
            c2.AddElement(New Paragraph(msg.ToString.PadLeft(msg.Length + lPadding, " "), bf)) : c3.AddElement(New Paragraph(Environment.NewLine, bf)) : c4.AddElement(New Paragraph(Environment.NewLine, bf))
            printListMovimiento(g.ListOfMovimiento, lPadding + 5, c2, c3, c4, bf)
            printNestedBultos(g.children, c0, c1, c2, c3, c4, level + 1, bf)
            i = i + 1
        Next
    End Sub
    Private Shared Sub printListMovimiento(lstM As IEnumerable(Of Movimiento), lPadding As Integer, c2 As PdfPCell, c3 As PdfPCell, c4 As PdfPCell, bf As Font)
        For Each mm In lstM
            Dim strMm As New StringBuilder()
            If mm.Marca = "ZZZZ" Or mm.Marca.Length = 0 Then
                strMm.Append(mm.Material)
            Else
                strMm.Append(mm.Marca)
                strMm.Append(" ")
                strMm.Append(mm.Material)
            End If
            If mm.Otros.tipolista = "1" Then
                strMm.Append(" (")
                strMm.Append(mm.Peso.ToString)
                strMm.Append("Kg)")
            End If
            If Not String.IsNullOrEmpty(mm.Observacion) AndAlso mm.Observacion.First = "#"c Then
                strMm.Append(mm.Observacion.Substring(1))
            End If
            c2.AddElement(New Paragraph(strMm.ToString.PadLeft(strMm.Length + lPadding, " "), bf)) : c3.AddElement(New Paragraph(mm.Cantidad, bf)) : c4.AddElement(New Paragraph(mm.Numord.ToString + " " + mm.Numope.ToString, bf))
        Next
    End Sub
    Public Shared Sub printLabel(o As Object)
        Dim imgPath = HttpContext.Current.Server.MapPath("~") + "content\logo_batz.png"
        Dim worker As New Threading.Thread(New Threading.ThreadStart(Sub() ThermalLabelWorker(o, imgPath)))
        worker.SetApartmentState(Threading.ApartmentState.STA)
        worker.Name = "ThermalLabelWorker"
        worker.Start()
        worker.Join()
    End Sub
    Public Shared Sub ThermalLabelWorker(o As Object, logoPath As String)
        Dim ps As New Neodynamic.SDK.Printing.PrinterSettings()
        Dim tLabel = New Neodynamic.SDK.Printing.ThermalLabel() : tLabel.UnitType = Neodynamic.SDK.Printing.UnitType.Mm : tLabel.Height = 100 : tLabel.Width = 100

        Dim yPos = 5 : Dim xPos = 13

        'Dim tI1 = New Neodynamic.SDK.Printing.TextItem() : tI1.X = xPos : tI1.Y = yPos + 0.5 : tI1.Width = tLabel.Width - xPos : tI1.Height = 6 : tI1.Text = o.proveedor : tI1.Font.Name = "Arial" : tI1.Font.Bold = True : tI1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point : tI1.Font.Size = 15
        Dim tI1 = New Neodynamic.SDK.Printing.TextItem() : tI1.X = xPos : tI1.Y = yPos + 0.5 : tI1.Width = tLabel.Width - xPos - 5 : tI1.Height = 15 : tI1.Text = o.proveedor
        tI1.Font.Name = "Arial" : tI1.Font.Bold = True : tI1.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point : tI1.Font.Size = 15

        Dim tIDir = New Neodynamic.SDK.Printing.TextItem() : tIDir.X = xPos : tIDir.Y = yPos + 17 : tIDir.Width = (tLabel.Width - xPos) / 2 : tIDir.Height = 5 : tIDir.Font.Name = "Arial" : tIDir.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point : tIDir.Font.Size = 11 : tIDir.Font.Bold = True
        tIDir.Text = "Dirección"
        Dim tI2 = New Neodynamic.SDK.Printing.TextItem() : tI2.X = xPos : tI2.Y = yPos + 23 : tI2.Width = (tLabel.Width - xPos) / 2 : tI2.Height = 35 : tI2.Font.Name = "Arial" : tI2.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point : tI2.Font.Size = 10
        tI2.Text = o.calle + Environment.NewLine + o.codigopostal + " " + o.poblacion + Environment.NewLine + o.provincia + Environment.NewLine + "Tel." + o.telefono

        Dim tIDat = New Neodynamic.SDK.Printing.TextItem() : tIDat.X = xPos + ((tLabel.Width - xPos) / 2) : tIDat.Y = yPos + 17 : tIDat.Width = (tLabel.Width - xPos) / 2 : tIDat.Height = 5 : tIDat.Font.Name = "Arial" : tIDat.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point : tIDat.Font.Size = 11 : tIDat.Font.Bold = True
        tIDat.Text = "Contenido"
        Dim tI3 = New Neodynamic.SDK.Printing.TextItem() : tI3.X = xPos + ((tLabel.Width - xPos) / 2) : tI3.Y = yPos + 23 : tI3.Width = 49 : tI3.Height = 35 : tI3.Font.Name = "Arial" : tI3.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point : tI3.Font.Size = 10
        tI3.Text = "Peso: " + o.peso.ToString + " Kg" + Environment.NewLine + "Bulto Nº: " + o.idBulto.ToString

        Dim tIbotom = New Neodynamic.SDK.Printing.TextItem() : tIbotom.X = xPos : tIbotom.Y = 88 : tIbotom.Width = 54 : tIbotom.Height = 4.5 : tIbotom.Text = "Torrea Auzoa 2,  48140 IGORRE (Bizkaia) Spain " + Environment.NewLine + "Tel. (34) 94 630 50 00 Fax (34) 94 630 50 20" : tIbotom.Font.Name = "Arial" : tIbotom.Font.Unit = Neodynamic.SDK.Printing.FontUnit.Point : tIbotom.Font.Size = 6
        Dim sq = New Neodynamic.SDK.Printing.RectangleShapeItem() : sq.X = xPos : sq.Y = 93 : sq.FillColor = Neodynamic.SDK.Printing.Color.Black : sq.Width = 55 : sq.Height = 2
        Dim mg1 = New Neodynamic.SDK.Printing.ImageItem() : mg1.SourceFile = logoPath : mg1.X = 70 : mg1.Y = 88 : mg1.Width = 26 : mg1.Height = 7.3

        Dim b = New Neodynamic.SDK.Printing.BarcodeItem()
        b.Code = o.idBulto.ToString
        b.X = xPos
        b.Y = yPos + 55
        b.Height = 25
        b.Width = tLabel.Width - xPos * 2
        b.BarWidth = 0.6
        b.BarHeight = 25
        b.BarcodeAlignment = Neodynamic.SDK.Printing.BarcodeAlignment.MiddleCenter
        b.Symbology = Neodynamic.SDK.Printing.BarcodeSymbology.Code128
        b.DisplayCode = False
        tLabel.Items.Add(b)

        tLabel.Items.Add(tI1)
        tLabel.Items.Add(tIDir)
        tLabel.Items.Add(tI2)
        tLabel.Items.Add(tIDat)
        tLabel.Items.Add(tI3)
        tLabel.Items.Add(tIbotom)
        tLabel.Items.Add(sq)
        tLabel.Items.Add(mg1)
        ps.Dpi = 200

        'Network setup
        ps.Communication.CommunicationType = Neodynamic.SDK.Printing.CommunicationType.Network
        ps.Communication.NetworkIPAddress = System.Net.IPAddress.Parse("10.7.0.147")
        ps.Communication.NetworkPort = 9100
        Dim pj = New Neodynamic.SDK.Printing.PrintJob(ps)

        'Set Thermal Printer language
        pj.PrinterSettings.ProgrammingLanguage = Neodynamic.SDK.Printing.ProgrammingLanguage.ZPL
        pj.PrinterSettings.PrinterName = "Zebra  GK420t"
        Try
            pj.Print(tLabel)
            'pj.ExportToPdf(tLabel, "C:\Users\aazkuenaga.BATZNT\Documents\proba.pdf", 200)
            pj.CleanCache()
            pj.Dispose()
        Catch ex As Exception
            pj.CleanCache()
            pj.Dispose()
        End Try
    End Sub
End Class


Public Class MyPageEvents
    Inherits PdfPageEventHelper
    Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString
    Public img, bf, bf1, v, a, userempresa, r
    Public Sub New(ByVal img_ As iTextSharp.text.Image, ByVal bf_ As Font, ByVal bf1_ As Font, ByVal v_ As Viaje)
        img = img_ : bf = bf_ : bf1 = bf1_ : v = v_
    End Sub
    Public Overrides Sub OnEndPage(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document)
        Dim bf = New Font(Font.FontFamily.HELVETICA, 5, 2)
        Dim dc = writer.DirectContent
        Dim Col As New ColumnText(dc)
        Col.SetSimpleColumn(30, document.PageSize.Height - 275, document.PageSize.Width - 30, document.PageSize.Height - 10)
        Col.AddElement(img)
        Col.Go()
        Col.SetSimpleColumn(30, document.PageSize.Height - 275, document.PageSize.Width - 30, document.PageSize.Height - 100)
        If a IsNot Nothing Then
            Col.AddElement(GetTable1(v, a, bf, bf1))
        End If
        If r IsNot Nothing Then
            Col.AddElement(GetTable1Recogidas(v, r, bf, bf1))
        End If
        Col.AddElement(New Paragraph(""))
        If v IsNot Nothing Then
            Col.AddElement(GetTable2(v.Matricula1, v.matricula2, v.IdTransportista, bf, bf1))
        End If

        If a IsNot Nothing Then
            If CType(a, Albaran).ListOfAgrupacion.Count > 0 AndAlso CType(a, Albaran).ListOfAgrupacion.First.ListOfMovimiento.Count > 0 AndAlso CType(a, Albaran).ListOfAgrupacion.First.ListOfMovimiento.First.CodPro = userempresa Then
                Col.AddElement(GetTableRecogida(a, bf, bf1))
            End If
        End If
        Col.Go()
        Col.SetSimpleColumn(30, -200, document.PageSize.Width - 30, 70)
        Col.AddElement(New Paragraph("* Nota: A partir de la fecha de recepción, 1 semana se plazo para posibles reclamaciones.", bf))
        Col.AddElement(New Paragraph("* En caso de no haber reclamaciones, se interpreta que el envío es correcto. Quedando Batz S. Coop. exenta de ninguna responsabilidad.", bf))
        Col.AddElement(New Paragraph("* Este albarán deberá ser enviado a Batz S. Coop debidamente sellado y con la fecha de recepción.", bf))
        Col.AddElement(New Paragraph("* Documento de control para el transporte de mercancias por carretera (orden FOM/2861/2012 de 13 de diciembre). Según el artículo del RD782/98 el responsable de residuo del envase usado para su correcta gestión ambiental, será el poseedor final.", bf))

        Col.Go()


        If v IsNot Nothing AndAlso v.salida IsNot Nothing Then
            'Sello
            Const imageWidth As Integer = 120
            Col.SetSimpleColumn(document.PageSize.Width / 2, 0, (document.PageSize.Width / 2) + imageWidth, 50 + imageWidth)
            Dim imgSelloBatz = iTextSharp.text.Image.GetInstance(HttpContext.Current.Request.PhysicalApplicationPath + "/Content/sello.png")
            imgSelloBatz.WidthPercentage = imageWidth
            Col.AddElement(imgSelloBatz)
            Col.Go()

            Dim fnt = FontFactory.GetFont("Arial", 20, New BaseColor(114, 189, 251))
            Col.SetSimpleColumn((document.PageSize.Width / 2) + 20, 0, (document.PageSize.Width / 2) + imageWidth + 20, 25 + imageWidth)
            Col.AddElement(New Paragraph(CDate(v.salida).ToShortDateString, fnt))
            Col.Go()
        End If

        MyBase.OnEndPage(writer, document)
    End Sub
    Public Function GetTable1Recogidas(ByVal v As Viaje, ByVal r As Recogida, ByVal bf As Font, ByVal bf1 As Font) As pdf.PdfPTable
        Dim t1 = New pdf.PdfPTable(5)
        t1.HorizontalAlignment = pdf.PdfPCell.ALIGN_LEFT
        t1.SetWidthPercentage(New Single(4) {165.0F, 175.0F, 100.0F, 60.0F, 80.0F}, PageSize.A4)
        Dim h1 = DBAccess.GetDireccionProveedor(r.IdEmpresaRecogida, strCn)
        Dim h2 = DBAccess.GetDireccionProveedor(r.IdEmpresaEntrega, strCn)
        If String.IsNullOrEmpty(r.observacionesdireccion) Then
            Dim c20 = New pdf.PdfPCell(New Paragraph(r.nombreEmpresaRecogida, bf1)) : c20.BorderWidthBottom = 0 : t1.AddCell(c20)
        Else
            Dim c20 = New pdf.PdfPCell(New Paragraph(r.observacionesdireccion, bf1)) : c20.Rowspan = 4 : c20.BorderWidthBottom = 0 : t1.AddCell(c20)
        End If

        Dim c21 = New pdf.PdfPCell(New Paragraph(r.nombreEmpresaEntrega, bf1)) : c21.BorderWidthBottom = 0 : t1.AddCell(c21)
        Dim c22 = New pdf.PdfPCell(New Paragraph("Biltegiko jasotze-agiria", bf)) : c22.BorderWidthBottom = 0 : t1.AddCell(c22)
        Dim c23 = New pdf.PdfPCell(New Paragraph("Bidaia", bf)) : c23.BorderWidthBottom = 0 : t1.AddCell(c23)
        Dim c24 = New pdf.PdfPCell(New Paragraph("Bidaltze data", bf)) : c24.BorderWidthBottom = 0 : t1.AddCell(c24)

        If String.IsNullOrEmpty(r.observacionesdireccion) Then
            Dim c30 = New pdf.PdfPCell(New Paragraph(h1.Calle, bf1)) : c30.BorderWidthBottom = 0 : c30.BorderWidthTop = 0 : t1.AddCell(c30)
        End If

        Dim c31 = New pdf.PdfPCell(New Paragraph(h2.Calle, bf1)) : c31.BorderWidthBottom = 0 : c31.BorderWidthTop = 0 : t1.AddCell(c31)
        Dim c32 = New pdf.PdfPCell(New Paragraph("Albaran de recogida de almacén", bf)) : c32.BorderWidthBottom = 0 : c32.BorderWidthTop = 0 : t1.AddCell(c32)
        Dim c33 = New pdf.PdfPCell(New Paragraph("Viaje", bf)) : c33.BorderWidthBottom = 0 : c33.BorderWidthTop = 0 : t1.AddCell(c33)
        Dim c34 = New pdf.PdfPCell(New Paragraph("Fecha de envío", bf)) : c34.BorderWidthBottom = 0 : c34.BorderWidthTop = 0 : t1.AddCell(c34)

        If String.IsNullOrEmpty(r.observacionesdireccion) Then
            Dim c40 = New pdf.PdfPCell(New Paragraph(h1.CodigoPostal + ", " + h1.Poblacion, bf1)) : c40.BorderWidthBottom = 0 : c40.BorderWidthTop = 0 : t1.AddCell(c40)
        End If

        Dim c41 = New pdf.PdfPCell(New Paragraph(h2.CodigoPostal + ", " + h2.Poblacion, bf1)) : c41.BorderWidthBottom = 0 : c41.BorderWidthTop = 0 : t1.AddCell(c41)
        Dim c42 = New pdf.PdfPCell(New Paragraph("Warehouse Delivery note", bf)) : c42.BorderWidthBottom = 0 : c42.BorderWidthTop = 0 : t1.AddCell(c42)
        Dim c43 = New pdf.PdfPCell(New Paragraph("Trip", bf)) : c43.BorderWidthBottom = 0 : c43.BorderWidthTop = 0 : t1.AddCell(c43)
        Dim c44 = New pdf.PdfPCell(New Paragraph("Delivery date", bf)) : c44.BorderWidthBottom = 0 : c44.BorderWidthTop = 0 : t1.AddCell(c44)

        If String.IsNullOrEmpty(r.observacionesdireccion) Then
            Dim c50 = New pdf.PdfPCell(New Paragraph(h1.Provincia, bf1)) : c50.BorderWidthBottom = 0 : c50.BorderWidthTop = 0 : t1.AddCell(c50)
        End If

        Dim c51 = New pdf.PdfPCell(New Paragraph(h2.Provincia, bf1)) : c51.BorderWidthBottom = 0 : c51.BorderWidthTop = 0 : t1.AddCell(c51)
        Dim c52 = New pdf.PdfPCell(New Paragraph(r.Id.ToString, bf1)) : c52.Rowspan = 3 : c52.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : c52.VerticalAlignment = pdf.PdfPCell.ALIGN_MIDDLE : t1.AddCell(c52)
        Dim c53 = New pdf.PdfPCell(New Paragraph(v.Id.ToString, bf1)) : c53.Rowspan = 3 : c53.VerticalAlignment = pdf.PdfPCell.ALIGN_MIDDLE : c53.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : c53.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#e1ff00")) : t1.AddCell(c53)
        Dim c54 = New pdf.PdfPCell(New Paragraph(If(v.Salida.HasValue, v.Salida.Value.ToShortDateString, v.fechaCreacion.ToShortDateString), bf1)) : c54.Rowspan = 3 : c54.VerticalAlignment = pdf.PdfPCell.ALIGN_MIDDLE : c54.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : t1.AddCell(c54)

        If String.IsNullOrEmpty(r.observacionesdireccion) Then
            Dim c60 = New pdf.PdfPCell(New Paragraph(h1.Pais, bf1)) : c60.BorderWidthBottom = 0 : c60.BorderWidthTop = 0 : t1.AddCell(c60)
        End If
        Dim c61 = New pdf.PdfPCell(New Paragraph(h2.Pais, bf1)) : c61.BorderWidthBottom = 0 : c61.BorderWidthTop = 0 : t1.AddCell(c61)

        'Dim c71 = New pdf.PdfPCell(New Paragraph("CIF " + a.ListOfAgrupacion.First.ListOfMovimiento.First.cifProveedor, bf1)) : c71.BorderWidthTop = 0 : c71.PaddingLeft = 100 : t1.AddCell(c71)
        Return t1
    End Function
    Public Function GetTable1(ByVal v As Viaje, ByVal a As Albaran, ByVal bf As Font, ByVal bf1 As Font) As pdf.PdfPTable
        Dim h = DBAccess.GetDireccionAlbaran(a.Id, strCn)


        If v Is Nothing Then
            Dim t1 = New pdf.PdfPTable(2)
            t1.HorizontalAlignment = pdf.PdfPCell.ALIGN_LEFT

            t1.SetWidthPercentage(New Single(1) {460.0F, 120.0F}, PageSize.A4)

            Dim c21 = New pdf.PdfPCell(New Paragraph(h.nombreEmpresa, bf1)) : c21.BorderWidthBottom = 0 : c21.PaddingLeft = 100 : t1.AddCell(c21)
            Dim c22 = New pdf.PdfPCell(New Paragraph("Biltegiko emate-agiria", bf)) : c22.BorderWidthBottom = 0 : t1.AddCell(c22)

            Dim c31 = New pdf.PdfPCell(New Paragraph(h.Calle, bf1)) : c31.BorderWidthBottom = 0 : c31.BorderWidthTop = 0 : c31.PaddingLeft = 100 : t1.AddCell(c31)
            Dim c32 = New pdf.PdfPCell(New Paragraph("Albaran de Entrega de almacén", bf)) : c32.BorderWidthBottom = 0 : c32.BorderWidthTop = 0 : t1.AddCell(c32)

            Dim c41 = New pdf.PdfPCell(New Paragraph(h.CodigoPostal + ", " + h.Poblacion, bf1)) : c41.BorderWidthBottom = 0 : c41.BorderWidthTop = 0 : c41.PaddingLeft = 100 : t1.AddCell(c41)
            Dim c42 = New pdf.PdfPCell(New Paragraph("Warehouse Delivery note", bf)) : c42.BorderWidthBottom = 0 : c42.BorderWidthTop = 0 : t1.AddCell(c42)

            Dim c51 = New pdf.PdfPCell(New Paragraph(h.Provincia, bf1)) : c51.BorderWidthBottom = 0 : c51.BorderWidthTop = 0 : c51.PaddingLeft = 100 : t1.AddCell(c51)
            Dim c52 = New pdf.PdfPCell(New Paragraph(a.Id.ToString, bf1)) : c52.Rowspan = 3 : c52.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : c52.VerticalAlignment = pdf.PdfPCell.ALIGN_MIDDLE : t1.AddCell(c52)

            Dim c61 = New pdf.PdfPCell(New Paragraph(h.Pais, bf1)) : c61.BorderWidthBottom = 0 : c61.BorderWidthTop = 0 : c61.PaddingLeft = 100 : t1.AddCell(c61)

            Dim c71 = New pdf.PdfPCell(New Paragraph("CIF " + a.ListOfAgrupacion.First.ListOfMovimiento.First.cifProveedor, bf1)) : c71.BorderWidthTop = 0 : c71.PaddingLeft = 100 : t1.AddCell(c71)
            Return t1
        Else
            Dim t1 = New pdf.PdfPTable(4)
            t1.HorizontalAlignment = pdf.PdfPCell.ALIGN_LEFT

            t1.SetWidthPercentage(New Single(3) {340.0F, 100.0F, 60.0F, 80.0F}, PageSize.A4)

            Dim c21 = New pdf.PdfPCell(New Paragraph(h.nombreEmpresa, bf1)) : c21.BorderWidthBottom = 0 : c21.PaddingLeft = 100 : t1.AddCell(c21)
            Dim c22 = New pdf.PdfPCell(New Paragraph("Biltegiko emate-agiria", bf)) : c22.BorderWidthBottom = 0 : t1.AddCell(c22)
            Dim c23 = New pdf.PdfPCell(New Paragraph("Bidaia", bf)) : c23.BorderWidthBottom = 0 : t1.AddCell(c23)
            Dim c24 = New pdf.PdfPCell(New Paragraph("Bidaltze data", bf)) : c24.BorderWidthBottom = 0 : t1.AddCell(c24)

            Dim c31 = New pdf.PdfPCell(New Paragraph(h.Calle, bf1)) : c31.BorderWidthBottom = 0 : c31.BorderWidthTop = 0 : c31.PaddingLeft = 100 : t1.AddCell(c31)
            Dim c32 = New pdf.PdfPCell(New Paragraph("Albaran de Entrega de almacén", bf)) : c32.BorderWidthBottom = 0 : c32.BorderWidthTop = 0 : t1.AddCell(c32)
            Dim c33 = New pdf.PdfPCell(New Paragraph("Viaje", bf)) : c33.BorderWidthBottom = 0 : c33.BorderWidthTop = 0 : t1.AddCell(c33)
            Dim c34 = New pdf.PdfPCell(New Paragraph("Fecha de envío", bf)) : c34.BorderWidthBottom = 0 : c34.BorderWidthTop = 0 : t1.AddCell(c34)

            Dim c41 = New pdf.PdfPCell(New Paragraph(h.CodigoPostal + ", " + h.Poblacion, bf1)) : c41.BorderWidthBottom = 0 : c41.BorderWidthTop = 0 : c41.PaddingLeft = 100 : t1.AddCell(c41)
            Dim c42 = New pdf.PdfPCell(New Paragraph("Warehouse Delivery note", bf)) : c42.BorderWidthBottom = 0 : c42.BorderWidthTop = 0 : t1.AddCell(c42)
            Dim c43 = New pdf.PdfPCell(New Paragraph("Trip", bf)) : c43.BorderWidthBottom = 0 : c43.BorderWidthTop = 0 : t1.AddCell(c43)
            Dim c44 = New pdf.PdfPCell(New Paragraph("Delivery date", bf)) : c44.BorderWidthBottom = 0 : c44.BorderWidthTop = 0 : t1.AddCell(c44)

            Dim c51 = New pdf.PdfPCell(New Paragraph(h.Provincia, bf1)) : c51.BorderWidthBottom = 0 : c51.BorderWidthTop = 0 : c51.PaddingLeft = 100 : t1.AddCell(c51)
            Dim c52 = New pdf.PdfPCell(New Paragraph(a.Id.ToString, bf1)) : c52.Rowspan = 3 : c52.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : c52.VerticalAlignment = pdf.PdfPCell.ALIGN_MIDDLE : t1.AddCell(c52)
            Dim c53 = New pdf.PdfPCell(New Paragraph(v.Id.ToString, bf1)) : c53.Rowspan = 3 : c53.VerticalAlignment = pdf.PdfPCell.ALIGN_MIDDLE : c53.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : c53.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#e1ff00")) : t1.AddCell(c53)
            Dim c54 = New pdf.PdfPCell(New Paragraph(If(v.Salida.HasValue, v.Salida.Value.ToShortDateString, v.fechaCreacion.ToShortDateString), bf1)) : c54.Rowspan = 3 : c54.VerticalAlignment = pdf.PdfPCell.ALIGN_MIDDLE : c54.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : t1.AddCell(c54)

            Dim c61 = New pdf.PdfPCell(New Paragraph(h.Pais, bf1)) : c61.BorderWidthBottom = 0 : c61.BorderWidthTop = 0 : c61.PaddingLeft = 100 : t1.AddCell(c61)

            Dim c71 = New pdf.PdfPCell(New Paragraph("CIF " + h.cifEmpresa, bf1)) : c71.BorderWidthTop = 0 : c71.PaddingLeft = 100 : t1.AddCell(c71)
            Return t1
        End If
    End Function

    Public Function GetTable1PackingList(ByVal v As Viaje, ByVal bf As Font, ByVal bf1 As Font) As pdf.PdfPTable
        Dim h
        If a.IdHelbide.HasValue Then
            h = DBAccess.GetHelbide(a.IdHelbide, strCn)
        Else
            h = DBAccess.GetDireccionProveedor(a.ListOfAgrupacion.First.ListOfMovimiento.First.CodPro, strCn)
        End If

        Dim t1 = New pdf.PdfPTable(3)
        t1.HorizontalAlignment = pdf.PdfPCell.ALIGN_LEFT
        t1.SetWidthPercentage(New Single(2) {330.0F, 150.0F, 80.0F}, PageSize.A4)

        Dim c21 = New pdf.PdfPCell(New Paragraph(a.ListOfAgrupacion.First.ListOfMovimiento.First.NombreProveedor, bf1)) : c21.BorderWidthBottom = 0 : c21.PaddingLeft = 100 : t1.AddCell(c21)
        Dim c22 = New pdf.PdfPCell(New Paragraph("Biltegiko emate-agiria", bf)) : c22.BorderWidthBottom = 0 : t1.AddCell(c22)
        Dim c23 = New pdf.PdfPCell(New Paragraph("Bidaltze data", bf)) : c23.BorderWidthBottom = 0 : t1.AddCell(c23)

        Dim c31 = New pdf.PdfPCell(New Paragraph(h.Calle, bf1)) : c31.BorderWidthBottom = 0 : c31.BorderWidthTop = 0 : c31.PaddingLeft = 100 : t1.AddCell(c31)
        Dim c32 = New pdf.PdfPCell(New Paragraph("Albaran de Entrega de almacén", bf)) : c32.BorderWidthBottom = 0 : c32.BorderWidthTop = 0 : t1.AddCell(c32)
        Dim c33 = New pdf.PdfPCell(New Paragraph("Fecha de envío", bf)) : c33.BorderWidthBottom = 0 : c33.BorderWidthTop = 0 : t1.AddCell(c33)

        Dim c41 = New pdf.PdfPCell(New Paragraph(h.CodigoPostal + ", " + h.Poblacion, bf1)) : c41.BorderWidthBottom = 0 : c41.BorderWidthTop = 0 : c41.PaddingLeft = 100 : t1.AddCell(c41)
        Dim c42 = New pdf.PdfPCell(New Paragraph("Warehouse Delivery note", bf)) : c42.BorderWidthBottom = 0 : c42.BorderWidthTop = 0 : t1.AddCell(c42)
        Dim c43 = New pdf.PdfPCell(New Paragraph("Delivery date", bf)) : c43.BorderWidthBottom = 0 : c43.BorderWidthTop = 0 : t1.AddCell(c43)

        Dim c51 = New pdf.PdfPCell(New Paragraph(h.Provincia, bf1)) : c51.BorderWidthBottom = 0 : c51.BorderWidthTop = 0 : c51.PaddingLeft = 100 : t1.AddCell(c51)
        Dim c52 = New pdf.PdfPCell(New Paragraph(a.Id.ToString, bf1)) : c52.Rowspan = 3 : c52.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : c52.VerticalAlignment = pdf.PdfPCell.ALIGN_MIDDLE : t1.AddCell(c52)
        Dim c53 = New pdf.PdfPCell(New Paragraph(Now.ToShortDateString, bf1)) : c53.Rowspan = 3 : c53.VerticalAlignment = pdf.PdfPCell.ALIGN_MIDDLE : c53.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : t1.AddCell(c53)

        Dim c61 = New pdf.PdfPCell(New Paragraph(h.Pais, bf1)) : c61.BorderWidthBottom = 0 : c61.BorderWidthTop = 0 : c61.PaddingLeft = 100 : t1.AddCell(c61)

        Dim c71 = New pdf.PdfPCell(New Paragraph("CIF " + a.ListOfAgrupacion.First.ListOfMovimiento.First.cifProveedor, bf1)) : c71.BorderWidthTop = 0 : c71.PaddingLeft = 100 : t1.AddCell(c71)
        Return t1
    End Function
    Public Function GetTable2(ByVal matricula1 As String, ByVal matricula2 As String, ByVal idTransportista As String, ByVal bf As Font, ByVal bf1 As Font) As pdf.PdfPTable
        Dim transportista = DBAccess.GetTransportista(idTransportista, strCn)
        Dim t = New pdf.PdfPTable(2)
        t.HorizontalAlignment = pdf.PdfPCell.ALIGN_LEFT
        t.SetWidthPercentage(New Single(1) {410.0F, 170.0F}, PageSize.A4)
        Dim c00 = New pdf.PdfPCell(New Paragraph("Garraio bidea / Medi transporte / Transport / Anlieferung / Moyen de transport", bf)) : c00.BorderWidthBottom = 0 : t.AddCell(c00)
        Dim c01 = New pdf.PdfPCell(New Paragraph("Matrikula / Matricula / Enrollment", bf)) : c01.BorderWidthBottom = 0 : t.AddCell(c01)

        Dim c10 = New pdf.PdfPCell(New Paragraph(transportista.Nombre, bf1)) : c10.BorderWidthBottom = 0 : c10.BorderWidthTop = 0 : c10.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : t.AddCell(c10)
        Dim c11 = New pdf.PdfPCell(New Paragraph(If(matricula2.Length > 0, matricula1 + "/" + matricula2, matricula1), bf)) : c11.BorderWidthBottom = 0 : c11.BorderWidthTop = 0 : c11.HorizontalAlignment = pdf.PdfPCell.ALIGN_CENTER : c11.VerticalAlignment = pdf.PdfPCell.ALIGN_MIDDLE : t.AddCell(c11)

        Dim c20 = New pdf.PdfPCell(New Paragraph(transportista.Calle.ToString.Trim(" ") + " " + transportista.CodigoPostal.ToString.Trim(" ") + " " + transportista.Poblacion.ToString.Trim(" ") + " " + transportista.Provincia.ToString.Trim(" ") + " " + transportista.Pais.ToString.Trim(" ") + " NIF/CIF " + transportista.cif, bf)) : c20.BorderWidthTop = 0 : t.AddCell(c20)
        Dim c21 = New pdf.PdfPCell() : c21.BorderWidthTop = 0 : t.AddCell(c21)

        Return t
    End Function

    Public Function GetTableRecogida(ByVal a As Albaran, ByVal bf As Font, ByVal bf1 As Font) As pdf.PdfPTable
        Dim h = DBAccess.GetDireccionProveedor(a.ListOfAgrupacion.First.ListOfMovimiento.First.EmpresaSalida, strCn)
        Dim t = New pdf.PdfPTable(1)
        t.HorizontalAlignment = pdf.PdfPCell.ALIGN_LEFT
        t.SetWidthPercentage(New Single(0) {560.0F}, PageSize.A4)
        Dim c00 = New pdf.PdfPCell(New Paragraph("Bilketa helbidea / Empresa de recogida", bf)) : c00.BorderWidthBottom = 0 : t.AddCell(c00)
        Dim c10 = New pdf.PdfPCell(New Paragraph(h.Calle + ", " + h.CodigoPostal + " " + h.Poblacion + ", " + h.Provincia + ", " + h.Pais, bf1)) : c10.BorderWidthTop = 0 : t.AddCell(c10)
        Return t
    End Function
End Class