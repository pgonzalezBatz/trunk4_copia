Imports System.Drawing
Imports System.Drawing.Imaging
Public Class h

    <CLSCompliant(False)> _
    Public Shared Function variosIntentos(Of T)(f As Func(Of T), nIntentos As Integer, tiempoReintento As Integer) As T
        Dim i = 1
        Do
            Try
                Dim ret As T = f()
                Return ret
            Catch ex As System.ApplicationException
                Throw
            Catch ex As Exception
                If i = nIntentos Then
                    Throw
                End If
                i = i + 1
                If i = nIntentos Then
                    Throw
                End If
                Threading.Thread.Sleep(tiempoReintento)
            End Try
        Loop
    End Function
    <CLSCompliant(False)> _
    Public Shared Sub variosIntentos(f As Action, nIntentos As Integer, tiempoReintento As Integer)
        Dim i = 1
        Do
            Try
                f()
                Exit Do
            Catch ex As System.ApplicationException
                Throw
            Catch ex As Exception
                If i = nIntentos Then
                    Throw
                End If
                i = i + 1
                If i = nIntentos Then
                    Throw
                End If
                Threading.Thread.Sleep(tiempoReintento)
            End Try
        Loop
    End Sub
    Public Shared Sub mergeAndWatermarkTiff(lstFileName As List(Of String), outputFileName As String, watermarkText As String)
        'get the codec for tiff files
        Dim info As ImageCodecInfo = ImageCodecInfo.GetImageEncoders().Where(Function(ie) ie.MimeType = "image/tiff").First()
        'use the save encoder
        Dim enc As Encoder = Encoder.SaveFlag
        Dim ep As EncoderParameters = New EncoderParameters(1)
        ep.Param(0) = New EncoderParameter(enc, EncoderValue.MultiFrame)
        Dim pages As Bitmap = Nothing
        Dim frame As Integer = 0


        For Each f In lstFileName
            If frame = 0 Then
                'First frame
                Dim tempBmp = Image.FromFile(f)
                tempBmp.SelectActiveFrame(FrameDimension.Page, 0)
                
                pages = New Bitmap(tempBmp.Width, tempBmp.Height)
                Dim canvas As Graphics = Graphics.FromImage(pages)
                canvas.DrawImage(tempBmp, 0, 0, pages.Width, pages.Height)
                Dim font As Font = New Font("Verdana", CType(Math.Round(tempBmp.Width * 2 / 100), Single), FontStyle.Bold)
                Dim textSize As SizeF = New SizeF()
                textSize = canvas.MeasureString(watermarkText, font)
                Dim position As Point = New Point(tempBmp.Width / 2 - (textSize.Width / 2), tempBmp.Height / 2)
                canvas.DrawString(watermarkText, font, New SolidBrush(Color.FromArgb(128, 255, 0, 0)), position)




                pages.Save(outputFileName, info, ep)
                tempBmp.Dispose()
                canvas.Dispose()
            Else
                ep.Param(0) = New EncoderParameter(enc, EncoderValue.FrameDimensionPage)
                Dim tempBmp = Image.FromFile(f)
                tempBmp.SelectActiveFrame(FrameDimension.Page, 0)
                Dim bm As Bitmap = New Bitmap(tempBmp.Width, tempBmp.Height)
                Dim canvas As Graphics = Graphics.FromImage(bm)
                canvas.DrawImage(tempBmp, 0, 0, bm.Width, bm.Height)
                Dim font As Font = New Font("Verdana", CType(Math.Round(tempBmp.Width * 2 / 100), Single), FontStyle.Bold)
                Dim textSize As SizeF = New SizeF()
                textSize = canvas.MeasureString(watermarkText, font)
                Dim position As Point = New Point(tempBmp.Width / 2 - (textSize.Width / 2), tempBmp.Height / 2)
                canvas.DrawString(watermarkText, font, New SolidBrush(Color.FromArgb(128, 255, 0, 0)), position)
                pages.SaveAdd(bm, ep)
                bm.Dispose()
                tempBmp.Dispose()
                canvas.Dispose()
            End If
            frame = frame + 1
        Next
        pages.Dispose()
    End Sub
    Public Shared Sub deleteFiles(lstFileName As List(Of String))
        For Each f In lstFileName
            Dim fi As New IO.FileInfo(f)
            If fi.Exists Then
                fi.Delete()
            End If
        Next
    End Sub
    ''' <summary>
    ''' Add text as the watermark to each page of the source pdf to create a new pdf with text watermark
    ''' </summary>
    ''' <param name="sourceFile">the full path to the source pdf file</param>
    ''' <param name="outputFile">the full path where the watermarked pdf file will be saved to</param>
    ''' <param name="watermarkText">the string array conntaining the text to use as the watermark. Each element is treated as a line in the watermark</param>
    ''' <param name="watermarkFont">the font to use for the watermark. The default font is HELVETICA</param>
    ''' <param name="watermarkFontSize">the size of the font. The default size is 48</param>
    ''' <param name="watermarkFontColor">the color of the watermark. The default color is blue</param>
    ''' <param name="watermarkFontOpacity">the opacity of the watermark. The default opacity is 0.3</param>
    ''' <param name="watermarkRotation">the rotation in degree of the watermark. The default rotation is 45 degree</param>
    ''' <remarks></remarks>
    Public Shared Sub AddWatermarkText(ByVal sourceFile As String, ByVal outputFile As String, ByVal watermarkText As List(Of String), _
                                       Optional ByVal watermarkFontOpacity As Single = 0.5F, _
                                       Optional ByVal watermarkRotation As Single = 55.0F)

        Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
        Dim stamper As iTextSharp.text.pdf.PdfStamper = Nothing
        Dim gstate As iTextSharp.text.pdf.PdfGState = Nothing
        Dim underContent As iTextSharp.text.pdf.PdfContentByte = Nothing
        Dim rect As iTextSharp.text.Rectangle = Nothing
        Dim currentY As Single = 0.0F
        Dim offset As Single = 0.0F
        Dim pageCount As Integer = 0
        Try
            reader = New iTextSharp.text.pdf.PdfReader(sourceFile)

            stamper = New iTextSharp.text.pdf.PdfStamper(reader, New System.IO.FileStream(outputFile, IO.FileMode.Create))
            Dim watermarkFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1252, _
                                                              iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED)
            Dim watermarkFontColor = iTextSharp.text.BaseColor.RED

            gstate = New iTextSharp.text.pdf.PdfGState()
            gstate.FillOpacity = watermarkFontOpacity
            gstate.StrokeOpacity = watermarkFontOpacity
            pageCount = reader.NumberOfPages()
            For i As Integer = 1 To pageCount
                rect = reader.GetPageSizeWithRotation(i)
                Dim watermarkFontSize = rect.Width / 50
                underContent = stamper.GetOverContent(i)
                With underContent
                    .SaveState()
                    .SetGState(gstate)
                    .SetColorFill(watermarkFontColor)
                    .BeginText()
                    .SetFontAndSize(watermarkFont, watermarkFontSize)
                    .SetTextMatrix(30, 30)
                    If watermarkText.Count > 1 Then
                        currentY = (rect.Height / 2) + ((watermarkFontSize * watermarkText.Count) / 2)
                    Else
                        currentY = (rect.Height / 2)
                    End If
                    For j As Integer = 0 To watermarkText.Count - 1
                        If j > 0 Then
                            offset = (j * watermarkFontSize) + (watermarkFontSize / 4) * j
                        Else
                            offset = 0.0F
                        End If
                        .ShowTextAligned(iTextSharp.text.Element.ALIGN_CENTER, watermarkText(j), rect.Width / 2, currentY - offset, watermarkRotation)
                    Next
                    .EndText()
                    .RestoreState()
                End With
            Next
            stamper.Close()
            reader.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
