Imports BidaiakLib
Imports System.IO
Imports System.Configuration
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Namespace BLL

    Public Class GenerarPDF

#Region "Comun"

#Region "Variables"

        Private Shared fontVerdana As String = "Verdana"
        Private Shared colorCabecera As BaseColor = New BaseColor(159, 184, 255)
        Private Shared colorRecuadro As BaseColor = New BaseColor(216, 216, 216)
        Private Shared colorPie As BaseColor = New BaseColor(213, 234, 255)
        Private Shared font7 As Font = FontFactory.GetFont(fontVerdana, 7)
        Private Shared font7Red As Font = FontFactory.GetFont(fontVerdana, 7, Font.NORMAL, New BaseColor(255, 85, 85))
        Private Shared font9 As Font = FontFactory.GetFont(fontVerdana, 9)
        Private Shared font9Bold As Font = FontFactory.GetFont(fontVerdana, 9, Font.BOLD)
        Private Shared font9Blue As Font = FontFactory.GetFont(fontVerdana, 9, Font.NORMAL, New BaseColor(95, 135, 195))
        Private Shared font9BlueBold As Font = FontFactory.GetFont(fontVerdana, 9, Font.BOLD, New BaseColor(95, 135, 195))
        Private Shared font9Red As Font = FontFactory.GetFont(fontVerdana, 9, Font.NORMAL, New BaseColor(255, 85, 85))
        Private Shared font9RedBold As Font = FontFactory.GetFont(fontVerdana, 9, Font.BOLD, New BaseColor(255, 85, 85))
        Private Shared font12 As Font = FontFactory.GetFont(fontVerdana, 12, Font.NORMAL)
        Private Shared font12Bold As Font = FontFactory.GetFont(fontVerdana, 12, Font.BOLD)
        Private Shared font12BlueBold As Font = FontFactory.GetFont(fontVerdana, 12, Font.BOLD, New BaseColor(95, 135, 195))
        Private Shared fontTitle As Font = FontFactory.GetFont(fontVerdana, 18, Font.BOLD)
        Private Shared localPath As String = ConfigurationManager.AppSettings("LocalPath")

#End Region

        ''' <summary>
        ''' Obtiene el logo
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        Private Shared Function getLogo(ByVal idPlanta As Integer) As Image
            Dim pathImagen As String = ConfigurationManager.AppSettings("pathBidaiak") & "/App_Themes/Tema1/Logos/logo_"
            Select Case idPlanta
                Case 1
                    pathImagen &= "igorre.jpg"
                Case 227
                    pathImagen &= "energy.jpg"
                Case Else
                    pathImagen &= "igorre.jpg"
            End Select

            Dim img As Image = Image.GetInstance(pathImagen)
            img.ScalePercent(60, 60)  'Sino sale muy grande            
            Return img
        End Function

        ''' <summary>
        ''' Pinta el titulo de la tabla
        ''' </summary>
        ''' <param name="titulo">Titulo</param>
        ''' <returns></returns>        
        Private Shared Function getTituloTable(ByVal titulo As String) As PdfPTable
            Dim table As New iTextSharp.text.pdf.PdfPTable(1)
            Dim cell As iTextSharp.text.pdf.PdfPCell
            table.HorizontalAlignment = 0 : table.SpacingBefore = 10 : table.SpacingAfter = 15 : table.TotalWidth = 600
            cell = New pdf.PdfPCell(New Phrase(titulo.ToUpper, FontFactory.GetFont(FontFactory.COURIER, 15, iTextSharp.text.Font.BOLD)))
            table.AddCell(cell)
            Return table
        End Function

        ''' <summary>
        ''' Dada una cultura y el formato de los decimales, devuelve el decimal con la coma o punto
        ''' </summary>
        ''' <param name="sDec">Numero a convertir</param>
        ''' <returns></returns>	
        Public Shared Function DecimalValue(ByVal sDec As String) As Decimal
            If (Not String.IsNullOrEmpty(sDec)) Then
                Dim myDec As String = String.Empty
                If (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",") Then
                    myDec = sDec.Trim.Replace(".", ",")
                ElseIf (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".") Then
                    myDec = sDec.Trim.Replace(",", ".")
                End If
                myDec = If(myDec = String.Empty, "0", myDec)
                Return Convert.ToDecimal(myDec, Threading.Thread.CurrentThread.CurrentCulture.NumberFormat)
            Else
                Return 0
            End If
        End Function

#End Region

#Region "Hojas de gastos"

        ''' <summary>
        ''' Genera el pdf de la hoja de gastos especificada
        ''' </summary>
        ''' <param name="idhoja">Id de la hoja</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="cultura">Cultura del usuario para poder traducir</param>
        ''' <param name="idSession">Id de la session con el que se nombraran los pdf para que no se repitan</param>
        ''' <param name="idPerfil">Dependiendo del perfil se podran hacer distintas cosas</param>
        ''' <returns></returns>        
        Public Shared Function HojaGastos(ByVal idhoja As Integer, ByVal idPlanta As Integer, ByVal cultura As String, ByVal idSession As String, ByVal idPerfil As Integer) As String
            Dim document As Document = Nothing
            Dim wrtTest As PdfWriter = Nothing
            Dim path As String = String.Empty
            Dim myFile As FileStream = Nothing
            Try
                path = ConfigurationManager.AppSettings("pathBidaiak") & "\Temp\" & "HG" & idSession + ".pdf"
                Dim resultadoHoja As String = String.Empty
                Dim viajesBLL As New BLL.ViajesBLL
                Dim fechaDesde, fechaHasta As Date
                Dim hojasBLL As New BLL.HojasGastosBLL
                Dim oHoja As ELL.HojaGastos = hojasBLL.loadHoja(idhoja)
                Dim oViaje As ELL.Viaje = Nothing
                Dim bActividadExenta As Boolean = False
                If (oHoja.IdViaje <> Integer.MinValue) Then
                    oViaje = viajesBLL.loadInfo(oHoja.IdViaje)
                    fechaDesde = oViaje.FechaIda
                    fechaHasta = oViaje.FechaVuelta
                    Dim oInteg As ELL.Viaje.Integrante = oViaje.ListaIntegrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = oHoja.Usuario.Id)
                    If (oInteg IsNot Nothing) Then
                        Dim activBLL As New BLL.ActividadesBLL
                        Dim oActiv As ELL.Actividad = activBLL.loadInfo(oInteg.IdActividad)
                        bActividadExenta = (oActiv IsNot Nothing AndAlso oActiv.ExentaIRPF AndAlso oViaje.Nivel <> ELL.Viaje.eNivel.Nacional)
                    End If
                Else
                    fechaDesde = oHoja.FechaDesde
                    fechaHasta = oHoja.FechaHasta
                End If

                document = New Document(PageSize.A4, 30, 30, 145, 75)  'Se establecen los margenes para que salve la cabecera y empiece a escribir ahi
                myFile = New FileStream(path, FileMode.Create)
                wrtTest = PdfWriter.GetInstance(document, myFile)
                document.Open()

                '***Cabecera***                
                'Esta clase es para que al romperse la tabla, vuelva a dibujar las cabeceras y tablas
                Dim pathImagen As String = ConfigurationManager.AppSettings("pathBidaiak") & "/App_Themes/Tema1/Logos/logo_"
                Select Case idPlanta
                    Case 1
                        pathImagen &= "igorre.jpg"
                    Case 227
                        pathImagen &= "energy.jpg"
                    Case Else
                        pathImagen &= "igorre.jpg"
                End Select
                Dim img As Image = Image.GetInstance(pathImagen)
                img.ScalePercent(60, 60)  'Sino sale muy grande   

                Dim funcDatosIni = Function() DatosInicialesHG(oHoja, oViaje, cultura, fechaDesde, fechaHasta, idPerfil, idPlanta)
                Dim pie As New Phrase(AccesoGenerico.GetTerminoStatic("Hoja de gastos validada por", cultura, localPath) & " " & oHoja.Validador.NombreCompleto & vbCrLf, font9)
                pie.Add(New Phrase(AccesoGenerico.GetTerminoStatic("Fecha de impresion", cultura, localPath) & ":  " & Now.ToShortDateString & "  -  " & Now.ToShortTimeString, font7))

                wrtTest.PageEvent = New MyPageTempEvents(getLogo(idPlanta), getTituloTable(AccesoGenerico.GetTerminoStatic("Hoja de gastos", cultura, localPath)), funcDatosIni, pie)
                wrtTest.CloseStream = False

                '***Lineas de la hoja***
                Dim totalGastosRecibos, totalGastosKm, totalGastosTransferencias, totalGastosVisa, totalDevuelto, totalMovAjuste As Decimal
                totalGastosRecibos = 0 : totalGastosKm = 0 : totalGastosTransferencias = 0 : totalGastosVisa = 0 : totalDevuelto = 0 : totalMovAjuste = 0
                If (oHoja.Lineas IsNot Nothing AndAlso oHoja.Lineas.Count > 0) Then
                    Dim hojLineas As List(Of ELL.HojaGastos.Linea)
                    'Gastos en metalico
                    hojLineas = oHoja.Lineas.FindAll(Function(o As ELL.HojaGastos.Linea) o.TipoGasto = ELL.HojaGastos.Linea.eTipoGasto.Metalico)
                    If (hojLineas IsNot Nothing AndAlso hojLineas.Count > 0) Then totalGastosRecibos = GestionarLineasHojasGastos(hojLineas, ELL.HojaGastos.Linea.eTipoGasto.Metalico, document, cultura, idPlanta)

                    'Gastos de visa                    
                    If (oHoja.MovimientosVisa IsNot Nothing AndAlso oHoja.MovimientosVisa.Count > 0) Then totalGastosVisa = GestionarLineasVisaHojasGastos(oHoja, document, cultura, idPlanta)

                    'Gastos de kilometraje
                    hojLineas = oHoja.Lineas.FindAll(Function(o As ELL.HojaGastos.Linea) o.TipoGasto = ELL.HojaGastos.Linea.eTipoGasto.Kilometraje)
                    If (hojLineas IsNot Nothing AndAlso hojLineas.Count > 0) Then totalGastosKm += GestionarLineasHojasGastos(hojLineas, ELL.HojaGastos.Linea.eTipoGasto.Kilometraje, document, cultura, idPlanta)
                Else  'Solo ha habido gastos de visa                  
                    If (oHoja.MovimientosVisa IsNot Nothing AndAlso oHoja.MovimientosVisa.Count > 0) Then totalGastosVisa = GestionarLineasVisaHojasGastos(oHoja, document, cultura, idPlanta)
                End If

                'Transferencias y cantidad devuelta
                If (oViaje IsNot Nothing AndAlso oViaje.Anticipo IsNot Nothing AndAlso oViaje.Anticipo.Estado <> ELL.Anticipo.EstadoAnticipo.cancelada AndAlso oViaje.ResponsableLiquidacion.Id = oHoja.Usuario.Id) Then
                    If (oViaje.Anticipo.Transferencias IsNot Nothing AndAlso oViaje.Anticipo.Transferencias.Count > 0) Then
                        totalGastosTransferencias = GestionarTransferencias(oViaje.Anticipo.Transferencias, document, oViaje.IdViaje, cultura, idPlanta)
                    End If
                    If (oViaje.Anticipo.Devoluciones IsNot Nothing AndAlso oViaje.Anticipo.Devoluciones.Count > 0) Then
                        For Each oDev As ELL.Anticipo.Movimiento In oViaje.Anticipo.Devoluciones
                            totalDevuelto += oDev.ImporteEuros
                        Next
                    End If
                End If

                If (oViaje IsNot Nothing AndAlso oViaje.Anticipo IsNot Nothing AndAlso oViaje.Anticipo.Movimientos IsNot Nothing AndAlso oViaje.Anticipo.Movimientos.Exists(Function(o) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Diferencia_Cambio OrElse o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Manual)) Then
                    Dim lMovAjuste As List(Of ELL.Anticipo.Movimiento) = oViaje.Anticipo.Movimientos.FindAll(Function(o) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Diferencia_Cambio OrElse o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Manual)
                    For Each mov In lMovAjuste
                        totalMovAjuste += mov.ImporteEuros 'Esto puede ser negativo o positivo
                    Next
                End If

                If (bActividadExenta) Then
                    Dim lDocsIntegr As List(Of ELL.Viaje.DocumentoIntegrante) = viajesBLL.loadDocumentosIntegrante(oViaje.IdViaje, oHoja.Usuario.Id)
                    DocumentosIntegranteViaje(lDocsIntegr, document, cultura)
                End If

                'Para que la tabla abarcara toda el ancho de la pagina, ha habido que ponerle las propiedades LockedWith y TotalWidth. Despues los tamaños del setWidths actuan como porcentajes
                Dim table As New PdfPTable(2)
                Dim cell As PdfPCell
                table.SetWidths(New Integer() {3, 1}) : table.LockedWidth = True : table.TotalWidth = 500 : table.HorizontalAlignment = 0 : table.SpacingAfter = 20
                If (bActividadExenta) Then table.SpacingBefore = 20

                '***Anticipo***   
                Dim diferencia As Decimal
                If (oViaje IsNot Nothing AndAlso oViaje.Anticipo IsNot Nothing AndAlso (oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.Entregado Or oViaje.Anticipo.Estado = ELL.Anticipo.EstadoAnticipo.cerrado) AndAlso oViaje.ResponsableLiquidacion.Id = oHoja.Usuario.Id) Then  'Si tiene anticipo y es el liquidador
                    Dim transf As String = String.Empty
                    If (totalGastosTransferencias <> 0) Then transf = "(*)"
                    cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Anticipo recibido", cultura, localPath).ToUpper & transf, font9)) : cell.HorizontalAlignment = Element.ALIGN_CENTER
                    table.AddCell(cell)

                    transf = Math.Round(oViaje.Anticipo.EurosEntregados, 2)
                    If (totalGastosTransferencias > 0) Then
                        transf &= "+" & totalGastosTransferencias
                    ElseIf (totalGastosTransferencias <> 0) Then  'Si viene un 0, no se le añade porque sino, en vez de ser 300€, serían 3000€
                        transf &= totalGastosTransferencias  'ya viene el menos en el numero
                    End If
                    cell = New PdfPCell(New Phrase(transf & " €", font9Bold)) : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                    table.AddCell(cell)
                    If (totalDevuelto > 0) Then
                        cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Anticipo devuelto", cultura, localPath).ToUpper, font9)) : cell.HorizontalAlignment = Element.ALIGN_CENTER
                        table.AddCell(cell)
                        cell = New PdfPCell(New Phrase((totalDevuelto) & " €", font9Bold)) : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                        table.AddCell(cell)
                    End If
                    cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Gastos en metalico", cultura, localPath).ToUpper, font9)) : cell.HorizontalAlignment = Element.ALIGN_CENTER
                    table.AddCell(cell)
                    cell = New PdfPCell(New Phrase((totalGastosRecibos) & " €", font9Bold)) : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                    table.AddCell(cell)
                    If (totalMovAjuste <> 0) Then
                        cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Movimientos de ajuste", cultura, localPath).ToUpper, font9)) : cell.HorizontalAlignment = Element.ALIGN_CENTER
                        table.AddCell(cell)
                        cell = New PdfPCell(New Phrase((totalMovAjuste) & " €", font9Bold)) : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                        table.AddCell(cell)
                    End If
                    cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Diferencia", cultura, localPath).ToUpper, font9)) : cell.HorizontalAlignment = Element.ALIGN_CENTER
                    table.AddCell(cell)
                    cell = New PdfPCell(New Phrase(Math.Round(oViaje.Anticipo.EurosEntregados, 2) + totalGastosTransferencias + totalMovAjuste - totalGastosRecibos - totalDevuelto & " €", font9Bold)) : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                    table.AddCell(cell)
                    document.Add(table)
                    If (totalGastosTransferencias <> 0) Then document.Add(New Paragraph("(*) " & AccesoGenerico.GetTerminoStatic("Al existir movimientos de traspaso entre viajes, se  le sumara o restara dependiendo cual sea el viaje destino", cultura, localPath).ToUpper, FontFactory.GetFont(FontFactory.HELVETICA, 6)))
                    document.Add(Chunk.NEWLINE)
                    diferencia = Math.Round(oViaje.Anticipo.EurosEntregados, 2) + totalGastosTransferencias + totalMovAjuste - totalGastosRecibos - totalDevuelto
                    If (diferencia > 0) Then  'Si tiene dinero por justificar, tendra que abonarlo en metalico y despues se le pagara kilometraje si tuviera
                        resultadoHoja = AccesoGenerico.GetTerminoStatic("El trabajador debe devolver", cultura, localPath) & " " & Math.Abs(diferencia) & " €"
                        document.Add(New Phrase(resultadoHoja, font9RedBold))
                        document.Add(Chunk.NEWLINE)
                        document.Add(New Phrase(AccesoGenerico.GetTerminoStatic("El anticipo se debera entregar en mano en administracion", cultura, localPath), font9BlueBold))
                        If (totalGastosKm) Then
                            resultadoHoja = AccesoGenerico.GetTerminoStatic("Total a recibir por el trabajador", cultura, localPath).ToUpper & " " & totalGastosKm & " €"
                            document.Add(Chunk.NEWLINE) : document.Add(New Phrase(resultadoHoja, font9BlueBold))
                        End If
                    ElseIf (diferencia = 0) Then  'la hoja esta liquidada, si tuviera kilometraje se le cobraria
                        If (totalGastosKm > 0) Then
                            resultadoHoja = AccesoGenerico.GetTerminoStatic("Total a recibir por el trabajador", cultura, localPath).ToUpper & " " & totalGastosKm & " €"
                            document.Add(New Phrase(resultadoHoja, font9BlueBold))
                        Else
                            resultadoHoja = AccesoGenerico.GetTerminoStatic("Hoja de gastos liquidada", cultura, localPath)
                            document.Add(New Phrase(resultadoHoja, FontFactory.GetFont(FontFactory.HELVETICA, 9, 0, New iTextSharp.text.BaseColor(0, 102, 204))))
                        End If

                    Else 'Le tienen que devolver dinero. Si tuviera kilometraje se el sumara
                        Dim cantidadRecibir As Decimal = diferencia
                        If (totalGastosKm > 0) Then cantidadRecibir = Math.Abs(cantidadRecibir) + totalGastosKm
                        resultadoHoja = AccesoGenerico.GetTerminoStatic("Total a recibir por el trabajador", cultura, localPath).ToUpper & " " & Math.Abs(cantidadRecibir) & " €"
                        document.Add(New Phrase(resultadoHoja, font9BlueBold))

                        'If (totalGastosKm > 0) Then cantidadRecibir += totalGastosKm
                        'If (cantidadRecibir > 0) Then  'puede ser que sea negativo el resultado y al sumarle los gastos del km se quede negativo o pase a positivo
                        '    resultadoHoja = "Total a recibir".Itzuli & " " & cantidadRecibir & " €"
                        '    font9Bold.Color = New iTextSharp.text.BaseColor(85, 153, 255)
                        '    document.Add(New Phrase(resultadoHoja, font9Bold))
                        'Else  'hay que separa los kilometrajes si lo tuviera
                        '    'Se le resta el gasto de km porque antes se lo hemos sumado y esto no habra que devolverlo
                        '    resultadoHoja = "El trabajador debe devolver".Itzuli & " " & Math.Abs(cantidadRecibir - totalGastosKm) & " €"
                        '    font9Bold.Color = New iTextSharp.text.BaseColor(255, 85, 85)
                        '    document.Add(New Phrase(resultadoHoja, font9Bold))
                        '    If (totalGastosKm > 0) Then
                        '        resultadoHoja = "Total a recibir".Itzuli & " " & totalGastosKm & " €"
                        '        font9Bold.Color = New iTextSharp.text.BaseColor(85, 153, 255)
                        '        document.Add(Chunk.NEWLINE)
                        '        document.Add(New Phrase(resultadoHoja, font9Bold))
                        '    End If
                        'End If
                    End If
                    Else  'no hay anticipo
                        cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Total gastos", cultura, localPath).ToUpper, font9))
                    cell.HorizontalAlignment = Element.ALIGN_CENTER
                    table.AddCell(cell)
                    cell = New PdfPCell(New Phrase(totalGastosRecibos + totalGastosKm + totalGastosVisa & " €", font9Bold))
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT
                    table.AddCell(cell)
                    document.Add(table)
                    resultadoHoja = AccesoGenerico.GetTerminoStatic("Total a recibir por el trabajador", cultura, localPath).ToUpper & " " & (totalGastosRecibos + totalGastosKm) & " €"
                    document.Add(New Phrase(resultadoHoja, font9BlueBold))
                End If
                Return path
            Catch batzEx As BidaiakLib.BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al generar el pdf de la hoja de gastos", ex)
            Finally
                If (document IsNot Nothing) Then
                    document.Close()
                    document.Dispose()
                    myFile.Close()
                    myFile.Dispose()
                End If
            End Try
        End Function

        ''' <summary>
        ''' Gestiona las tablas de las lineas con y sin recibo de la hoja de gastos
        ''' </summary>
        ''' <param name="document">Documento donde se añadiran los objetos</param>        
        ''' <param name="tipo">Con recibo, Sin recibo, Kilometraje</param>
        ''' <param name="lineas">Lineas a procesar</param>
        ''' <param name="cultura">Cultura para traducir</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>Devuelve el total de las lineas</returns>
        Private Shared Function GestionarLineasHojasGastos(ByVal lineas As List(Of ELL.HojaGastos.Linea), ByVal tipo As ELL.HojaGastos.Linea.eTipoGasto, ByRef document As Document, ByVal cultura As String, ByVal idPlanta As Integer) As Decimal
            Dim table As PdfPTable
            Dim numColumns As Integer = 5
            Dim cell As PdfPCell
            Dim strAux As String
            table = New PdfPTable(numColumns)
            table.HeaderRows = 1 : table.LockedWidth = True : table.TotalWidth = 500 : table.HorizontalAlignment = 0 : table.SpacingBefore = 10 : table.SpacingAfter = 15

            '*******CABECERA*********
            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("fecha", cultura, localPath), font9Bold)) : cell.BackgroundColor = colorCabecera : cell.HorizontalAlignment = Element.ALIGN_CENTER
            table.AddCell(cell)
            If (tipo = ELL.HojaGastos.Linea.eTipoGasto.Metalico) Then
                table.SetWidths(New Integer() {1, 4, 1, 1, 1})
                strAux = AccesoGenerico.GetTerminoStatic("Gastos en metalico", cultura, localPath)
                cell = New PdfPCell(New Phrase(strAux.ToUpper, font9Bold))
                cell.BackgroundColor = colorCabecera : cell.HorizontalAlignment = Element.ALIGN_CENTER
                table.AddCell(cell)
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Gastos (otra divisa)", cultura, localPath).ToUpper, font9Bold))
                cell.BackgroundColor = colorCabecera : cell.HorizontalAlignment = Element.ALIGN_CENTER
                table.AddCell(cell)
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Gastos €", cultura, localPath).ToUpper, font9Bold))
                cell.BackgroundColor = colorCabecera : cell.HorizontalAlignment = Element.ALIGN_CENTER
                table.AddCell(cell)
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Recibo", cultura, localPath).ToUpper, font9Bold))
                cell.BackgroundColor = colorCabecera : cell.HorizontalAlignment = Element.ALIGN_CENTER
                table.AddCell(cell)
            ElseIf (tipo = ELL.HojaGastos.Linea.eTipoGasto.Kilometraje) Then
                table.SetWidths(New Integer() {1, 2, 2, 1, 1})
                Dim table2 As New PdfPTable(2)

                Dim cellTable As New PdfPCell
                cellTable.BackgroundColor = colorCabecera : cellTable.Colspan = 2

                Dim bidaiakBLL As New BLL.BidaiakBLL
                Dim precioKm As String = bidaiakBLL.loadParameters(idPlanta).PrecioKm.ToString
                precioKm = String.Format("{0:0.##}", DecimalValue(precioKm))
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Gastos de kilometraje", cultura, localPath).ToUpper & " (" & precioKm & " €/Km)", font9Bold))
                cell.BorderWidthLeft = 0 : cell.BorderWidthRight = 0 : cell.BorderWidthTop = 0 : cell.BackgroundColor = colorCabecera : cell.Colspan = 2 : cell.HorizontalAlignment = Element.ALIGN_CENTER
                table2.AddCell(cell)
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("De donde", cultura, localPath).ToUpper, font9Bold))
                cell.BackgroundColor = colorCabecera : cell.BorderWidthLeft = 0 : cell.BorderWidthBottom = 0 : cell.BorderWidthTop = 0 : cell.HorizontalAlignment = Element.ALIGN_CENTER
                table2.AddCell(cell)
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("A donde", cultura, localPath).ToUpper, font9Bold))
                cell.BackgroundColor = colorCabecera : cell.BorderWidthLeft = 0 : cell.BorderWidthRight = 0 : cell.BorderWidthBottom = 0 : cell.BorderWidthTop = 0 : cell.HorizontalAlignment = Element.ALIGN_CENTER
                table2.AddCell(cell)

                cellTable.Table = table2
                table.AddCell(cellTable)

                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Km", cultura, localPath).ToUpper, font9Bold))
                cell.BackgroundColor = colorCabecera : cell.HorizontalAlignment = Element.ALIGN_CENTER
                table.AddCell(cell)
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Gastos km", cultura, localPath).ToUpper, font9Bold))
                cell.BackgroundColor = colorCabecera : cell.HorizontalAlignment = Element.ALIGN_CENTER
                table.AddCell(cell)
            End If

            ''*******DATOS*********
            Dim totalEuros, numDec As Decimal
            Dim si, no As String
            si = AccesoGenerico.GetTerminoStatic("si", cultura, localPath).ToUpper
            no = AccesoGenerico.GetTerminoStatic("no", cultura, localPath).ToUpper
            totalEuros = 0
            lineas = (From linea As ELL.HojaGastos.Linea In lineas Select linea Order By linea.Usuario.NombreCompleto Ascending, linea.Fecha Ascending, If(linea.ConceptoBatz IsNot Nothing, linea.ConceptoBatz.Nombre, linea.Concepto) Ascending).ToList
            For Each oLinea As ELL.HojaGastos.Linea In lineas
                cell = New PdfPCell(New Phrase(oLinea.Fecha.ToShortDateString, font9))
                cell.HorizontalAlignment = Element.ALIGN_CENTER
                table.AddCell(cell)
                If (tipo = ELL.HojaGastos.Linea.eTipoGasto.Metalico) Then
                    strAux = oLinea.ConceptoBatz.Nombre
                    If (oLinea.Concepto <> String.Empty) Then strAux &= "(" & oLinea.Concepto & ")"
                    cell = New PdfPCell(New Phrase(strAux, font9))
                    table.AddCell(cell)
                    'La columna de gastos en otras divisas solo se informara si el la linea no esta en euros
                    strAux = String.Empty
                    If (oLinea.Moneda.Abreviatura.ToLower <> "eur") Then strAux = oLinea.Cantidad & " " & oLinea.Moneda.Abreviatura
                    cell = New PdfPCell(New Phrase(strAux, font9)) : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                    table.AddCell(cell)
                    'Si la linea no esta en euros, se hara una conversion, sino lo que ponga
                    If (oLinea.Moneda.Abreviatura.ToLower = "eur") Then
                        strAux = oLinea.Cantidad
                        totalEuros += oLinea.Cantidad
                    Else
                        numDec = oLinea.ImporteEuros
                        totalEuros += numDec
                        strAux = numDec
                    End If
                    cell = New PdfPCell(New Phrase(strAux, font9)) : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                    table.AddCell(cell)
                    cell = New PdfPCell(New Phrase(If(oLinea.Recibo, si, no), font9)) : cell.HorizontalAlignment = Element.ALIGN_CENTER
                    table.AddCell(cell)
                ElseIf (tipo = ELL.HojaGastos.Linea.eTipoGasto.Kilometraje) Then
                    cell = New PdfPCell(New Phrase(oLinea.LugarOrigen, font9))
                    table.AddCell(cell)
                    cell = New PdfPCell(New Phrase(oLinea.LugarDestino, font9))
                    table.AddCell(cell)
                    cell = New PdfPCell(New Phrase(oLinea.Kilometros, font9)) : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                    table.AddCell(cell)
                    cell = New PdfPCell(New Phrase(oLinea.Cantidad, font9)) : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                    totalEuros += oLinea.Cantidad
                    table.AddCell(cell)
                End If
            Next

            '*******PIE*********
            strAux = String.Empty
            If (tipo = ELL.HojaGastos.Linea.eTipoGasto.Metalico) Then
                strAux = AccesoGenerico.GetTerminoStatic("Total gastos en metalico", cultura, localPath)
                cell = New PdfPCell(New Phrase(strAux.ToUpper, font9Bold))
                cell.Colspan = 3
                cell.BackgroundColor = colorPie : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                table.AddCell(cell)
                cell = New PdfPCell(New Phrase(totalEuros & " €", font9Bold))
                cell.BackgroundColor = colorPie : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                table.AddCell(cell)
                cell = New PdfPCell()
                cell.BackgroundColor = colorPie
                table.AddCell(cell)
            ElseIf (tipo = ELL.HojaGastos.Linea.eTipoGasto.Kilometraje) Then
                strAux = AccesoGenerico.GetTerminoStatic("Total gastos de kilometraje", cultura, localPath)
                cell = New PdfPCell(New Phrase(strAux.ToUpper, font9Bold))
                cell.Colspan = 4
                cell.BackgroundColor = colorPie : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                table.AddCell(cell)
                cell = New PdfPCell(New Phrase(totalEuros & " €", font9Bold))
                cell.BackgroundColor = colorPie : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                table.AddCell(cell)
            End If
            document.Add(table)

            Return totalEuros
        End Function

        ''' <summary>
        ''' Gestiona las tablas de las lineas de visa de la hoja de gastos
        ''' </summary>
        ''' <param name="oHoja">Objeto con los datos de la hoja</param>
        ''' <param name="document">Documento donde se añadiran los objetos</param>                        
        ''' <param name="cultura">Cultura para traducir</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>Devuelve el total de las lineas</returns>
        Private Shared Function GestionarLineasVisaHojasGastos(ByVal oHoja As ELL.HojaGastos, ByRef document As Document, ByVal cultura As String, ByVal idPlanta As Integer) As Decimal
            Dim table As PdfPTable
            Dim numColumns As Integer = 4
            Dim cell As PdfPCell
            Dim strAux As String
            Dim lineas As List(Of ELL.Visa.Movimiento) = oHoja.MovimientosVisa
            table = New PdfPTable(numColumns)
            table.HeaderRows = 1 : table.LockedWidth = True : table.TotalWidth = 500 : table.HorizontalAlignment = 0 : table.SpacingBefore = 10 : table.SpacingAfter = 15
            table.SetWidths(New Integer() {1, 4, 1, 1})
            '*******CABECERA*********            
            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("fecha", cultura, localPath), font9Bold)) : cell.BackgroundColor = colorCabecera : cell.HorizontalAlignment = Element.ALIGN_CENTER
            table.AddCell(cell)
            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Gastos de visa", cultura, localPath).ToUpper, font9Bold))
            cell.BackgroundColor = colorCabecera : cell.HorizontalAlignment = Element.ALIGN_CENTER
            table.AddCell(cell)
            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Gastos (otra divisa)", cultura, localPath).ToUpper, font9Bold))
            cell.BackgroundColor = colorCabecera : cell.HorizontalAlignment = Element.ALIGN_CENTER
            table.AddCell(cell)
            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Gastos €", cultura, localPath).ToUpper, font9Bold))
            cell.BackgroundColor = colorCabecera : cell.HorizontalAlignment = Element.ALIGN_CENTER
            table.AddCell(cell)

            ''*******DATOS*********
            Dim totalEuros As Decimal = 0
            Dim pathImagen As String = ConfigurationManager.AppSettings("pathBidaiak") & "/App_Themes/Tema1/Images/warning16.png"
            Dim img As Image
            Dim p As Paragraph
            lineas = (From linea As ELL.Visa.Movimiento In lineas Select linea Order By linea.NombreUsuario Ascending, linea.Fecha Ascending, linea.Sector Ascending).ToList
            For Each oLinea As ELL.Visa.Movimiento In lineas
                cell = New PdfPCell With {.HorizontalAlignment = Element.ALIGN_CENTER}
                p = New Paragraph
                If Not (oHoja.FechaDesde <= oLinea.Fecha AndAlso oLinea.Fecha <= oHoja.FechaHasta) Then
                    img = Image.GetInstance(pathImagen)
                    img.ScalePercent(60, 60)  'Sino sale muy grande  
                    p.Add(New Chunk(img, 0, 0))
                End If
                p.Add(New Phrase(oLinea.Fecha.ToShortDateString, font9))
                cell.AddElement(p)
                table.AddCell(cell)
                cell = New PdfPCell()
                cell.AddElement(New Paragraph(oLinea.Sector, font9))
                If (oLinea.Comentarios <> String.Empty) Then cell.AddElement(New Paragraph(oLinea.Comentarios, font7))
                If (oLinea.Estado = ELL.Visa.Movimiento.eEstado.Cargado) Then cell.AddElement(New Paragraph(AccesoGenerico.GetTerminoStatic("Gasto sin comentar", cultura, localPath).ToUpper, font7Red))
                table.AddCell(cell)
                'La columna de gastos en otras divisas solo se informara si el la linea no esta en euros
                strAux = String.Empty
                If (oLinea.MonedaGasto.Id <> oLinea.Moneda.Id) Then strAux = oLinea.ImporteMonedaGasto & " " & oLinea.MonedaGasto.Abreviatura
                cell = New PdfPCell(New Phrase(strAux, font9)) : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                table.AddCell(cell)
                strAux = oLinea.ImporteEuros
                totalEuros += oLinea.ImporteEuros
                cell = New PdfPCell(New Phrase(strAux, font9)) : cell.HorizontalAlignment = Element.ALIGN_RIGHT
                table.AddCell(cell)
            Next

            '*******PIE*********
            strAux = AccesoGenerico.GetTerminoStatic("Total gastos de visa", cultura, localPath)
            cell = New PdfPCell(New Phrase(strAux.ToUpper, font9Bold))
            cell.Colspan = 3
            cell.BackgroundColor = colorPie : cell.HorizontalAlignment = Element.ALIGN_RIGHT
            table.AddCell(cell)
            cell = New PdfPCell(New Phrase(totalEuros & " €", font9Bold))
            cell.BackgroundColor = colorPie : cell.HorizontalAlignment = Element.ALIGN_RIGHT
            table.AddCell(cell)

            document.Add(table)

            Return totalEuros
        End Function

        ''' <summary>
        ''' Gestiona las tablas de las transferencias
        ''' </summary>        
        ''' <param name="mov">Lineas de transferencias a procesar</param>
        ''' <param name="document">Documento donde se añadiran los objetos</param>                
        ''' <param name="idViaje">Viaje actual</param>
        ''' <param name="cultura">Cultura para traducir</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>Devuelve el total de las lineas de transferencia. Tendra signo:negativo si se ha transferido a otros viajes y positivo si viene de otros viajes</returns>        
        Private Shared Function GestionarTransferencias(ByVal mov As List(Of ELL.Anticipo.Movimiento), ByRef document As Document, ByVal idViaje As Integer, ByVal cultura As String, ByVal idPlanta As Integer) As Decimal
            Dim oViaje As ELL.Viaje
            Dim viajesBLL As New BLL.ViajesBLL
            Dim table As New PdfPTable(5)
            Dim cell As PdfPCell

            table.HeaderRows = 1 : table.LockedWidth = True : table.TotalWidth = 500 : table.HorizontalAlignment = 0 : table.SpacingBefore = 10 : table.SpacingAfter = 15

            '*******CABECERA*********
            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("fecha", cultura, localPath), font9Bold))
            cell.BackgroundColor = colorCabecera
            cell.HorizontalAlignment = Element.ALIGN_CENTER
            table.AddCell(cell)

            table.SetWidths(New Integer() {1, 2, 2, 1, 1})
            Dim table2 As New PdfPTable(2)

            Dim cellTable As New PdfPCell
            cellTable.BackgroundColor = colorCabecera
            cellTable.Colspan = 2

            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim precioKm As String = bidaiakBLL.loadParameters(idPlanta).PrecioKm
            precioKm = String.Format("{0:0.##}", DecimalValue(precioKm))
            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Traspaso metalico entre viajes", cultura, localPath).ToUpper, font9Bold))
            cell.BorderWidthLeft = 0 : cell.BorderWidthRight = 0 : cell.BorderWidthTop = 0 : cell.BackgroundColor = colorCabecera : cell.Colspan = 2 : cell.HorizontalAlignment = Element.ALIGN_CENTER
            table2.AddCell(cell)
            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Origen", cultura, localPath).ToUpper, font9Bold))
            cell.BackgroundColor = colorCabecera : cell.BorderWidthLeft = 0 : cell.BorderWidthBottom = 0 : cell.BorderWidthTop = 0 : cell.HorizontalAlignment = Element.ALIGN_CENTER
            table2.AddCell(cell)
            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Destino", cultura, localPath).ToUpper, font9Bold))
            cell.BackgroundColor = colorCabecera : cell.BorderWidthLeft = 0 : cell.BorderWidthRight = 0 : cell.BorderWidthBottom = 0 : cell.BorderWidthTop = 0 : cell.HorizontalAlignment = Element.ALIGN_CENTER
            table2.AddCell(cell)

            cellTable.Table = table2
            table.AddCell(cellTable)

            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Cantidad (Otra divisa)", cultura, localPath).ToUpper, font9Bold))
            cell.BackgroundColor = colorCabecera : cell.HorizontalAlignment = Element.ALIGN_CENTER
            table.AddCell(cell)
            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Cantidad €", cultura, localPath).ToUpper, font9Bold))
            cell.BackgroundColor = colorCabecera : cell.HorizontalAlignment = Element.ALIGN_CENTER
            table.AddCell(cell)

            '*******DATOS*********
            Dim totalEuros, numDec As Decimal
            Dim cantidad As String
            totalEuros = 0
            mov.Sort(Function(o1 As ELL.Anticipo.Movimiento, o2 As ELL.Anticipo.Movimiento) o1.Fecha < o2.Fecha)
            Dim anticipBLL As New BLL.AnticiposBLL
            Dim xbatBLL As New BLL.XbatBLL
            Dim fechaCambio As Date = anticipBLL.loadAnticipoFechaEntrega(idViaje)
            Dim fechaCambioAux As Date
            For Each oMov As ELL.Anticipo.Movimiento In mov
                cell = New PdfPCell(New Phrase(oMov.Fecha.ToShortDateString, font9))
                cell.HorizontalAlignment = Element.ALIGN_CENTER
                table.AddCell(cell)
                cell = New PdfPCell(New Phrase("V" & oMov.IdViajeOrigen & "-" & oMov.UserOrigen.Nombre & " " & oMov.UserOrigen.Apellido1, font9))
                table.AddCell(cell)
                oViaje = viajesBLL.loadInfo(oMov.IdViajeDestino)
                cell = New PdfPCell(New Phrase("V" & oMov.IdViajeDestino & "-" & oMov.UserDestino.Nombre & " " & oMov.UserDestino.Apellido1, font9))
                table.AddCell(cell)
                cantidad = String.Empty
                If (oMov.Moneda.Abreviatura.ToLower <> "eur") Then cantidad = oMov.Cantidad & " " & oMov.Moneda.Abreviatura
                cell = New PdfPCell(New Phrase(cantidad, font9))
                cell.HorizontalAlignment = Element.ALIGN_RIGHT
                table.AddCell(cell)

                If (oMov.Moneda.Abreviatura.ToLower = "eur") Then
                    cantidad = oMov.Cantidad
                    If (idViaje = oMov.IdViajeOrigen) Then
                        totalEuros -= oMov.Cantidad
                    Else
                        totalEuros += oMov.Cantidad
                    End If
                Else
                    fechaCambioAux = fechaCambio
                    If (fechaCambioAux = DateTime.MinValue) Then fechaCambioAux = oMov.Fecha
                    numDec = Math.Round(xbatBLL.ObtenerRateEuros(oMov.Moneda.Id, oMov.Cantidad, fechaCambioAux, 0), 2)
                    cantidad = numDec
                    If (idViaje = oMov.IdViajeOrigen) Then
                        totalEuros -= numDec
                    Else
                        totalEuros += numDec
                    End If
                End If
                cell = New PdfPCell(New Phrase(cantidad, font9))
                cell.HorizontalAlignment = Element.ALIGN_RIGHT
                table.AddCell(cell)
            Next
            '*******PIE*********
            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Total gastos de transferencias", cultura, localPath).ToUpper, font9Bold))
            cell.Colspan = 3 : cell.BackgroundColor = colorPie : cell.HorizontalAlignment = Element.ALIGN_RIGHT
            table.AddCell(cell)
            cell = New PdfPCell()
            cell.BackgroundColor = colorPie
            table.AddCell(cell)
            cell = New PdfPCell(New Phrase(Math.Abs(totalEuros) & " €", font9Bold))
            cell.BackgroundColor = colorPie : cell.HorizontalAlignment = Element.ALIGN_RIGHT
            table.AddCell(cell)

            document.Add(table)

            Return totalEuros
        End Function

        ''' <summary>
        ''' Datos de cabecera para las hojas de gasto. Se llamara en todas las paginas
        ''' </summary>
        ''' <param name="oHoja">Hoja de gastos</param>
        ''' <param name="oViaje">Viaje</param>
        ''' <param name="cultura">Cultura del usuario</param>
        ''' <param name="fechaDesde">Fecha desde del viaje u hoja</param>
        ''' <param name="fechaHasta">Fecha hasta del viaje u hoja</param>
        ''' <param name="idPerfil">Si es de administracion, se mostrara la cuenta contable y la organizacion</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Private Shared Function DatosInicialesHG(ByVal oHoja As ELL.HojaGastos, ByVal oViaje As ELL.Viaje, ByVal cultura As String, ByVal fechaDesde As Date, ByVal fechaHasta As Date, ByVal idPerfil As Integer, ByVal idPlanta As Integer) As Paragraph
            Dim prResul As New Paragraph
            Dim pr As New Paragraph
            pr.Add(New Phrase(AccesoGenerico.GetTerminoStatic("Id Hoja", cultura, localPath) & " ", font9))
            pr.Add(New Phrase(If(oHoja.IdSinViaje <> Integer.MinValue, "H" & oHoja.IdSinViaje, "V" & oHoja.IdViaje) & "    ", FontFactory.GetFont(FontFactory.COURIER_BOLD, 13)))
            pr.Add(New Phrase("(" & fechaDesde.ToShortDateString & " - " & fechaHasta.ToShortDateString & ")", font9BlueBold))
            prResul.Add(pr)

            pr = New Paragraph
            pr.Add(New Phrase("D/Dñ ", font9))
            pr.Add(New Phrase(oHoja.Usuario.NombreCompleto & " (" & oHoja.Usuario.CodPersona & ")          ", font9BlueBold))
            If (oViaje IsNot Nothing) Then
                pr.Add(New Phrase(AccesoGenerico.GetTerminoStatic("Destino", cultura, localPath) & " ", font9))
                pr.Add(New Phrase(oViaje.Destino, font9BlueBold))
            End If
            prResul.Add(pr)

            'Si es financiero o administrador, se mostrara la cuenta contable y la organizacion
            Try
                If (idPerfil = BLL.BidaiakBLL.Profiles.Financiero AndAlso oHoja.Usuario.IdDepartamento <> String.Empty) Then
                    Dim dptoBLL As New BLL.DepartamentosBLL
                    Dim oDpto As ELL.Departamento = dptoBLL.loadInfo(oHoja.Usuario.IdDepartamento, idPlanta)
                    pr = New Paragraph
                    pr.Add(New Phrase(AccesoGenerico.GetTerminoStatic("Cuenta contable", cultura, localPath) & " :", font9))
                    pr.Add(New Phrase(oDpto.Cuenta0 & "         ", font9BlueBold))

                    Dim epsilonBLL As New BLL.Epsilon(idPlanta)
                    Dim info As String() = epsilonBLL.getInfoOrdenDepartamento(oHoja.Usuario.IdDepartamento)
                    If (info IsNot Nothing AndAlso info(0) = "00985" AndAlso info(3) <> String.Empty) Then
                        pr.Add(New Phrase(AccesoGenerico.GetTerminoStatic("Organizacion", cultura, localPath) & ": ", font9))
                        pr.Add(New Phrase(info(3), font9BlueBold))
                    End If
                    prResul.Add(pr)
                End If
            Catch ex As Exception
                pr = New Paragraph
                pr.Add(New Phrase("Error al consultar la cuenta contable y la organizacion de la hoja", font9))
                prResul.Add(pr)
            End Try

            Return prResul
        End Function

        ''' <summary>
        ''' Se muestran los documentos del integrante del viaje
        ''' </summary>
        ''' <param name="lDocs">Lista de los documentos de la hoja</param> 
        ''' <param name="document">Documento donde se añadiran los objetos</param>       
        ''' <param name="cultura">Cultura del usuario</param>                
        Private Shared Sub DocumentosIntegranteViaje(ByVal lDocs As List(Of ELL.Viaje.DocumentoIntegrante), ByRef document As Document, ByVal cultura As String)
            If (lDocs IsNot Nothing AndAlso lDocs.Count > 0) Then
                lDocs.Sort(Function(o1 As ELL.Viaje.DocumentoIntegrante, o2 As ELL.Viaje.DocumentoIntegrante) o1.Titulo < o2.Titulo)
                document.Add(New Phrase(AccesoGenerico.GetTerminoStatic("La persona ha realizado una actividad exenta y ha subido los siguientes documentos", cultura, localPath) & " ", font9))
                Dim listDoc As List = New List(List.UNORDERED)
                listDoc.IndentationLeft = 30.0F
                For Each oDoc As ELL.Viaje.DocumentoIntegrante In lDocs
                    listDoc.Add(New ListItem(New Chunk(oDoc.Titulo, font9)))
                Next
                document.Add(listDoc)
            Else 'Sin documentos
                document.Add(New Phrase(AccesoGenerico.GetTerminoStatic("La persona ha realizado una actividad exenta pero no ha subido ningun documento que lo acredite", cultura, localPath) & " ", font9RedBold))
            End If
        End Sub

#End Region

#Region "Entrega de anticipos"

        ''' <summary>
        ''' Genera el pdf de la hoja de gastos especificada
        ''' </summary>
        ''' <param name="idAnticipo">Id del anticipo hoja</param>                        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idSession">Id de la session con el que se nombraran los pdf para que no se repitan</param>
        ''' <returns></returns>        
        Public Shared Function ReciboAnticipoEntregado(ByVal idAnticipo As Integer, ByVal idPlanta As Integer, ByVal cultura As String, ByVal idSession As String) As String
            Dim document As Document = Nothing
            Dim wrtTest As PdfWriter = Nothing
            Dim path As String = String.Empty
            Dim myFile As FileStream = Nothing
            Try
                path = ConfigurationManager.AppSettings("pathBidaiak") & "/Temp/" & "ReciboAnticipo" & idSession + ".pdf"
                Dim resultadoHoja As String = String.Empty
                Dim anticipoBLL As New BLL.AnticiposBLL
                Dim oAnticipo As ELL.Anticipo = anticipoBLL.loadInfo(idAnticipo)

                document = New Document(PageSize.A4, 30, 30, 135, 75)  'Se establecen los margenes para que salve la cabecera y empiece a escribir ahi
                myFile = New FileStream(path, FileMode.Create)
                wrtTest = PdfWriter.GetInstance(document, myFile)
                document.Open()
                Dim cb = wrtTest.DirectContent
                '1ª Copia
                Dim Col As New ColumnText(cb)
                Col.SetSimpleColumn(50, document.PageSize.Height - 50, document.PageSize.Width, document.PageSize.Height / 2)
                PintarRecibo(Col, oAnticipo, idPlanta, cultura)
                Col.Go()  'Escribe el contenido en el stream                  
                Col.SetSimpleColumn(50, (document.PageSize.Height / 2) * (-1), document.PageSize.Width, document.PageSize.Height / 2 + 25 + 25)
                Col.AddElement(New Phrase(AccesoGenerico.GetTerminoStatic("El anticipo se debera entregar en mano en administracion", cultura, localPath).ToUpper, font9))
                Col.Go()
                '2ª Copia                
                Col.SetSimpleColumn(50, (document.PageSize.Height / 2) * (-1), document.PageSize.Width, document.PageSize.Height / 2 + 25)
                Col.AddElement(New Phrase("----------------------------------------------------------------------------------"))
                Col.AddElement(New Paragraph(" "))
                PintarRecibo(Col, oAnticipo, idPlanta, cultura)
                Col.Go()
                Col.SetSimpleColumn(50, (document.PageSize.Height / 5) * (-1), document.PageSize.Width, document.PageSize.Height / 5 + 25 - 150)
                Col.AddElement(New Phrase(AccesoGenerico.GetTerminoStatic("El anticipo se debera entregar en mano en administracion", cultura, localPath).ToUpper, font9))
                Col.Go()
                Return path
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al generar el pdf de la hoja de gastos", ex)
            Finally
                If (document IsNot Nothing) Then
                    document.Close()
                    document.Dispose()
                    myFile.Close()
                    myFile.Dispose()
                End If
            End Try
        End Function


        ''' <summary>
        ''' Pinta un documento
        ''' </summary>
        ''' <param name="column">Columna del documento donde se añadiran los controles</param>        
        Private Shared Sub PintarRecibo(ByRef column As ColumnText, ByVal oAnticipo As ELL.Anticipo, ByVal idPlanta As Integer, ByVal cultura As String)
            Dim frase As Phrase
            Dim parrafo As Paragraph
            Dim pathImagen As String = ConfigurationManager.AppSettings("pathBidaiak") & "/App_Themes/Tema1/Logos/logo_"
            Select Case idPlanta
                Case 1
                    pathImagen &= "igorre.jpg"
                Case 227
                    pathImagen &= "energy.jpg"
                Case Else
                    pathImagen &= "igorre.jpg"
            End Select
            Dim img As Image = Image.GetInstance(pathImagen)
            img.ScalePercent(60, 60)  'Sino sale muy grande   

            Dim table As New PdfPTable(3)
            table.TotalWidth = 700 : table.HorizontalAlignment = 0 : table.SpacingBefore = 15 : table.SpacingAfter = 30
            table.SetWidths(New Integer() {4, 1, 1})
            Dim cell As New PdfPCell() : cell.BorderWidth = 0
            cell.AddElement(img)
            table.AddCell(cell)
            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Id Viaje", cultura, localPath), font9)) : cell.BorderWidth = 0 : cell.HorizontalAlignment = Element.ALIGN_RIGHT : cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)
            cell = New PdfPCell(New Phrase("V" & oAnticipo.IdViaje, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 15)))
            cell.BackgroundColor = colorRecuadro : cell.HorizontalAlignment = Element.ALIGN_CENTER : cell.VerticalAlignment = Element.ALIGN_MIDDLE : cell.BorderWidth = 0
            table.AddCell(cell)
            column.AddElement(table)

            Dim userEntregado As SabLib.ELL.Usuario = oAnticipo.Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.IdAnticipo = oAnticipo.IdViaje And o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Entregado).First.UserOrigen
            frase = New Phrase
            frase.Add(New Chunk(userEntregado.NombreCompleto.ToUpper, font9Bold))
            frase.Add(New Chunk(" (e)k (" & userEntregado.CodPersona & ") " & AccesoGenerico.GetTerminoStatic("ha recibido los siguientes anticipos", cultura, localPath) & ":", font9))
            column.AddElement(frase)

            Dim lMovEntreg As List(Of ELL.Anticipo.Movimiento) = oAnticipo.Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Entregado)
            For Each oMov As ELL.Anticipo.Movimiento In lMovEntreg
                parrafo = New Paragraph("   - " & oMov.Cantidad & " " & oMov.Moneda.Nombre.ToUpper, font9) : parrafo.IndentationLeft = 50
                column.AddElement(parrafo)
            Next
            table = New PdfPTable(2)
            table.LockedWidth = True : table.TotalWidth = 75 : table.HorizontalAlignment = 0 : table.SpacingBefore = 20
            table.SetWidths(New Integer() {2, 2})
            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Total", cultura, localPath), font9)) : cell.BorderWidth = 0
            table.AddCell(cell)
            'cell = New PdfPCell(New Phrase(Math.Round(oAnticipo.EurosSolicitados, 2) & " €", font9Bold))
            cell = New PdfPCell(New Phrase(Math.Round(oAnticipo.EurosEntregados, 2) & " €", font9Bold))
            cell.BackgroundColor = colorRecuadro : cell.HorizontalAlignment = Element.ALIGN_CENTER : cell.BorderWidth = 0
            table.AddCell(cell)
            column.AddElement(table)

            column.AddElement(New Paragraph(" ")) 'Salto de linea                      
            If (cultura = "eu-ES") Then
                parrafo = New Paragraph("Igorre, " & Now.Year & "(e)ko " & AccesoGenerico.GetTerminoStatic(MonthName(Now.Month), cultura, localPath) & "ren " & Now.Day & "(a)", font9)
            Else
                parrafo = New Paragraph("Igorre, " & Now.Day & " de " & MonthName(Now.Month) & " del " & Now.Year, font9)
            End If
            parrafo.IndentationLeft = 100
            column.AddElement(parrafo)
        End Sub

        ''' <summary>
        ''' Muestra los datos iniciales del viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <returns></returns>        
        Private Shared Function DatosInicialesRecibo(idViaje As Integer) As Paragraph
            Dim prResul As New Paragraph
            Dim pr As New Paragraph
            pr.Add(New Phrase(idViaje, font9))
            prResul.Add(pr)

            Return prResul
        End Function


#End Region

#Region "Devolucion del anticipo"

        ''' <summary>
        ''' Genera el pdf de la hoja de gastos especificada
        ''' </summary>
        ''' <param name="idAnticipo">Id del anticipo</param>
        ''' <param name="movimientos">Ids de los movimientos</param>                        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idSession">Id de la session con el que se nombraran los pdf para que no se repitan</param>
        ''' <returns></returns>        
        Public Shared Function ReciboDevolucionAnticipo(ByVal idAnticipo As Integer, ByVal movimientos As String(), ByVal idPlanta As Integer, ByVal cultura As String, ByVal idSession As String) As String
            Dim document As Document = Nothing
            Dim wrtTest As PdfWriter = Nothing
            Dim path As String = String.Empty
            Dim myFile As FileStream = Nothing
            Try
                path = ConfigurationManager.AppSettings("pathBidaiak") & "/Temp/" & "ReciboDevAnticipo" & idSession + ".pdf"
                Dim resultadoHoja As String = String.Empty
                Dim anticipoBLL As New AnticiposBLL
                document = New Document(PageSize.A4, 30, 30, 135, 75)  'Se establecen los margenes para que salve la cabecera y empiece a escribir ahi
                myFile = New FileStream(path, FileMode.Create)
                wrtTest = PdfWriter.GetInstance(document, myFile)
                document.Open()
                Dim cb = wrtTest.DirectContent
                Dim oAnticipo As ELL.Anticipo = anticipoBLL.loadInfo(idAnticipo, False)
                '1ª Copia
                Dim Col As New ColumnText(cb)
                Col.SetSimpleColumn(50, document.PageSize.Height - 50, document.PageSize.Width, document.PageSize.Height / 2)
                PintarReciboDev(Col, oAnticipo, movimientos, idPlanta, cultura)
                Col.Go()  'Escribe el contenido en el stream                              
                '2ª Copia                
                Col.SetSimpleColumn(50, (document.PageSize.Height / 2) * (-1), document.PageSize.Width, document.PageSize.Height / 2 + 25)
                Col.AddElement(New Phrase("----------------------------------------------------------------------------------"))
                Col.AddElement(New Paragraph(" "))
                PintarReciboDev(Col, oAnticipo, movimientos, idPlanta, cultura)
                Col.Go()
                Return path
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al generar el pdf de la devolucion del anticipo", ex)
            Finally
                If (document IsNot Nothing) Then
                    document.Close()
                    document.Dispose()
                    myFile.Close()
                    myFile.Dispose()
                End If
            End Try
        End Function


        ''' <summary>
        ''' Pinta un documento
        ''' </summary>
        ''' <param name="column">Columna del documento donde se añadiran los controles</param>      
        ''' <param name="oAnticipo">Info del anticipo</param>
        ''' <param name="movimientos">Lista de movimientos de la devolucion</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="cultura">Cultura de los textos</param>
        Private Shared Sub PintarReciboDev(ByRef column As ColumnText, ByVal oAnticipo As ELL.Anticipo, ByVal movimientos As String(), ByVal idPlanta As Integer, ByVal cultura As String)
            Dim frase As Phrase
            Dim parrafo As Paragraph
            Dim pathImagen As String = ConfigurationManager.AppSettings("pathBidaiak") & "/App_Themes/Tema1/Logos/logo_"
            Select Case idPlanta
                Case 1
                    pathImagen &= "igorre.jpg"
                Case 227
                    pathImagen &= "energy.jpg"
                Case Else
                    pathImagen &= "igorre.jpg"
            End Select
            Dim img As Image = Image.GetInstance(pathImagen)
            img.ScalePercent(60, 60)  'Sino sale muy grande   

            Dim table As New PdfPTable(3)
            table.TotalWidth = 700 : table.HorizontalAlignment = 0 : table.SpacingBefore = 15 : table.SpacingAfter = 30
            table.SetWidths(New Integer() {4, 1, 1})
            Dim cell As New PdfPCell() : cell.BorderWidth = 0
            cell.AddElement(img)
            table.AddCell(cell)
            cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Id Viaje", cultura, localPath), font9)) : cell.BorderWidth = 0 : cell.HorizontalAlignment = Element.ALIGN_RIGHT : cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)
            cell = New PdfPCell(New Phrase("V" & oAnticipo.IdViaje, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 15)))
            cell.BackgroundColor = colorRecuadro : cell.HorizontalAlignment = Element.ALIGN_CENTER : cell.VerticalAlignment = Element.ALIGN_MIDDLE : cell.BorderWidth = 0
            table.AddCell(cell)
            column.AddElement(table)

            Dim userDevolucion As SabLib.ELL.Usuario = oAnticipo.Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.IdAnticipo = oAnticipo.IdViaje And o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion).First.UserOrigen
            frase = New Phrase
            frase.Add(New Chunk(userDevolucion.NombreCompleto.ToUpper, font9Bold))
            frase.Add(New Chunk(" (e)k (" & userDevolucion.CodPersona & ") " & AccesoGenerico.GetTerminoStatic("ha devuelto los siguientes anticipos", cultura, localPath) & ":", font9))
            column.AddElement(frase)

            Dim lMovEntreg As List(Of ELL.Anticipo.Movimiento) = oAnticipo.Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion)
            For Each oMov As ELL.Anticipo.Movimiento In lMovEntreg
                If (movimientos.Contains(oMov.Id.ToString)) Then
                    parrafo = New Paragraph("   - " & oMov.Cantidad & " " & oMov.Moneda.Nombre.ToUpper & "  (" & oMov.Fecha.ToShortDateString & ")", font9) : parrafo.IndentationLeft = 50
                    column.AddElement(parrafo)
                End If
            Next

            column.AddElement(New Paragraph(" ")) 'Salto de linea                      
            If (cultura = "eu-ES") Then
                parrafo = New Paragraph("Igorre, " & Now.Year & "(e)ko " & AccesoGenerico.GetTerminoStatic(MonthName(Now.Month), cultura, localPath) & "ren " & Now.Day & "(a)", font9)
            Else
                parrafo = New Paragraph("Igorre, " & Now.Day & " de " & MonthName(Now.Month) & " del " & Now.Year, font9)
            End If
            parrafo.IndentationLeft = 100
            column.AddElement(parrafo)
        End Sub


#End Region

#Region "Anticipo"

        ''' <summary>
        ''' Genera el pdf del anticipo para entregar a Esti
        ''' </summary>
        ''' <param name="IdAnticipo">Id del anticipo</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="cultura">Cultura del usuario para poder traducir</param>
        ''' <param name="idSession">Id de la session con el que se nombraran los pdf para que no se repitan</param>
        ''' <returns></returns>        
        Public Shared Function Anticipo(ByVal IdAnticipo As Integer, ByVal idPlanta As Integer, ByVal cultura As String, ByVal idSession As String) As String
            Dim document As Document = Nothing
            Dim wrtTest As PdfWriter = Nothing
            Dim path As String = String.Empty
            Dim myFile As FileStream = Nothing
            Try
                path = ConfigurationManager.AppSettings("pathBidaiak") & "\Temp\" & "ANT" & idSession + ".pdf"
                Dim resultadoHoja As String = String.Empty
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim viajesBLL As New ViajesBLL
                Dim anticBLL As New AnticiposBLL
                Dim oAnticipo As ELL.Anticipo = anticBLL.loadInfo(IdAnticipo, False)
                Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(IdAnticipo, True, bSoloCabecera:=True)
                document = New Document(PageSize.A4, 30, 30, 80, 75)  'Se establecen los margenes para que salve la cabecera y empiece a escribir ahi
                myFile = New FileStream(path, FileMode.Create)
                wrtTest = PdfWriter.GetInstance(document, myFile)
                document.Open()

                '***Cabecera***                
                'Esta clase es para que al romperse la tabla, vuelva a dibujar las cabeceras y tablas
                Dim pathImagen As String = ConfigurationManager.AppSettings("pathBidaiak") & "/App_Themes/Tema1/Logos/logo_igorre.jpg"
                Dim img As Image = Image.GetInstance(pathImagen)
                img.ScalePercent(60, 60)  'Sino sale muy grande
                Dim funcDatosIni = Function() DatosInicialesAnt(IdAnticipo, cultura)
                Dim pie As New Phrase(AccesoGenerico.GetTerminoStatic("Fecha de impresion", cultura, localPath) & " " & Now.ToShortDateString & "  -  " & Now.ToShortTimeString, font7)
                wrtTest.PageEvent = New MyPageEvents(getLogo(idPlanta), AccesoGenerico.GetTerminoStatic("Anticipo", cultura, localPath), funcDatosIni, pie)
                wrtTest.CloseStream = False

                '***Datos cabecera***
                Dim table As New PdfPTable(4)
                table.SetWidths(New Integer() {1, 2, 1, 2}) : table.LockedWidth = True : table.TotalWidth = 500 : table.HorizontalAlignment = 0 : table.SpacingAfter = 30
                Dim cell As PdfPCell
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Datos de cabecera", cultura, localPath).ToUpper, font12Bold)) : cell.Border = PdfPCell.BOTTOM_BORDER : cell.Colspan = 4 : table.AddCell(cell)
                'Fila en blanco
                cell = New PdfPCell(New Phrase(" ")) : cell.Border = 0 : cell.Colspan = 4 : table.AddCell(cell)
                'Fila 1
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Num Viaje", cultura, localPath), font9)) : cell.Border = 0 : table.AddCell(cell)
                cell = New PdfPCell(New Phrase("V" & IdAnticipo, font9BlueBold)) : cell.Colspan = 3 : cell.Border = 0 : table.AddCell(cell)
                'Fila 2
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Solicitante", cultura, localPath).ToUpper, font9)) : cell.Border = 0 : table.AddCell(cell)
                cell = New PdfPCell(New Phrase(userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = oViaje.IdUserSolicitador}, False).NombreCompletoYCodpersona, font9BlueBold)) : cell.Colspan = 3 : cell.Border = 0 : table.AddCell(cell)
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Liquidador", cultura, localPath).ToUpper, font9)) : cell.Border = 0 : table.AddCell(cell)
                cell = New PdfPCell(New Phrase(oViaje.ResponsableLiquidacion.NombreCompletoYCodpersona, font9BlueBold)) : cell.Border = 0 : cell.Colspan = 3 : table.AddCell(cell)
                'Fila 3
                Dim ctaContable, lantegi, organizacion As String
                ctaContable = String.Empty : lantegi = String.Empty : organizacion = String.Empty
                If (oViaje.ResponsableLiquidacion.IdDepartamento <> String.Empty) Then
                    Dim dptoBLL As New DepartamentosBLL
                    Dim epsilonBLL As New Epsilon(idPlanta)
                    Dim oDpto As ELL.Departamento = dptoBLL.loadInfo(oViaje.ResponsableLiquidacion.IdDepartamento, idPlanta)
                    ctaContable = If(oDpto IsNot Nothing, oDpto.Cuenta0, "-")
                    lantegi = epsilonBLL.getInfoLantegi(oViaje.ResponsableLiquidacion.IdDepartamento)
                    Dim info As String() = Nothing
                    info = epsilonBLL.getInfoOrdenDepartamento(oViaje.ResponsableLiquidacion.IdDepartamento)
                    If (info IsNot Nothing AndAlso info(0) = "00985") Then organizacion = info(3) 'Unicamente si es de sistemas, se pintara la organizacion                                    
                Else
                    ctaContable = AccesoGenerico.GetTerminoStatic("El usuario no tiene un departamento asociado", cultura, localPath)
                End If
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Cuenta contable", cultura, localPath), font9)) : cell.Border = 0 : table.AddCell(cell)
                cell = New PdfPCell(New Phrase(ctaContable, font9BlueBold)) : cell.Border = 0 : table.AddCell(cell)
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Lantegi", cultura, localPath), font9)) : cell.Border = 0 : table.AddCell(cell)
                cell = New PdfPCell(New Phrase(lantegi, font9BlueBold)) : cell.Border = 0 : table.AddCell(cell)
                'Fila 4
                If (organizacion <> String.Empty) Then
                    cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Organizacion", cultura, localPath), font9)) : cell.Border = 0 : table.AddCell(cell)
                    cell = New PdfPCell(New Phrase(organizacion, font9BlueBold)) : cell.Border = 0 : cell.Colspan = 3 : table.AddCell(cell)
                End If
                document.Add(table)
                document.Add(New Paragraph(AccesoGenerico.GetTerminoStatic("Se muestra el resumen de los movimientos del anticipo", cultura, localPath), font9))
                document.Add(New Paragraph(" "))
                '***Resumen movimientos***
                table = New PdfPTable(6)
                table.LockedWidth = True : table.TotalWidth = 500 : table.HorizontalAlignment = 0
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Fecha", cultura, localPath).ToUpper, font9Bold)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Justificado", cultura, localPath).ToUpper, font9Bold)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Gasto", cultura, localPath).ToUpper, font9Bold)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Moneda", cultura, localPath).ToUpper, font9Bold)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("En euros", cultura, localPath).ToUpper, font9Bold)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                cell = New PdfPCell(New Phrase(AccesoGenerico.GetTerminoStatic("Modo", cultura, localPath).ToUpper, font9Bold)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                Dim lMovimientos As List(Of ELL.Anticipo.Movimiento) = oAnticipo.Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov <> ELL.Anticipo.Movimiento.TipoMovimiento.Solicitado)
                Dim numHojasValidadas As Integer = 0
                AñadirHojasGastos(oViaje.HojasGastos, lMovimientos, numHojasValidadas, oViaje.ResponsableLiquidacion.Id, True)
                lMovimientos = lMovimientos.OrderBy(Of Date)(Function(o) o.Fecha).ToList
                Dim debe, haber, totalEuros As Decimal
                Dim modo, conversionEuros, estadoTotalEuros As String
                totalEuros = 0
                For Each mov As ELL.Anticipo.Movimiento In lMovimientos
                    debe = 0 : haber = 0 : conversionEuros = String.Empty
                    modo = AccesoGenerico.GetTerminoStatic([Enum].GetName(GetType(ELL.Anticipo.Movimiento.TipoMovimiento), mov.TipoMov).Replace("_", " "), cultura, localPath)
                    cell = New PdfPCell(New Phrase(mov.Fecha.ToShortDateString, font9)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                    If (mov.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia) Then
                        Dim nombreLiquidador As String = String.Empty
                        Dim idViajeLiquidador As Integer = 0
                        If (IdAnticipo = mov.IdViajeOrigen) Then
                            idViajeLiquidador = mov.IdViajeDestino
                            modo &= " ( " & AccesoGenerico.GetTerminoStatic("a", cultura, localPath) & " V" & mov.IdViajeDestino & " )"
                        Else
                            idViajeLiquidador = mov.IdViajeOrigen
                            modo &= " ( " & AccesoGenerico.GetTerminoStatic("de", cultura, localPath) & " V" & mov.IdViajeOrigen & " )"
                        End If
                        oViaje = viajesBLL.loadInfo(idViajeLiquidador, False, False, True)
                        If (oViaje.ResponsableLiquidacion IsNot Nothing) Then nombreLiquidador = "| L:" & oViaje.ResponsableLiquidacion.NombreCompleto & "(" & oViaje.ResponsableLiquidacion.CodPersona & ")"
                        modo &= nombreLiquidador
                    End If
                    Select Case mov.TipoMov
                        Case ELL.Anticipo.Movimiento.TipoMovimiento.Entregado
                            debe = Math.Round(DecimalValue(mov.Cantidad), 2)
                        Case ELL.Anticipo.Movimiento.TipoMovimiento.Hoja_Gastos
                            haber = Math.Round(DecimalValue(mov.Cantidad), 2)
                            If (mov.Cantidad = 0) Then
                                modo &= " (" & AccesoGenerico.GetTerminoStatic("Solo visas", cultura, localPath) & ")"
                            Else
                                conversionEuros = "-"
                            End If
                        Case ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion
                            haber = Math.Round(DecimalValue(mov.Cantidad), 2)
                            conversionEuros = "-"
                        Case ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia
                            If (mov.IdViajeOrigen = IdAnticipo) Then  'Transferencia realizada desde este viaje
                                haber = Math.Round(DecimalValue(mov.Cantidad), 2)
                                conversionEuros = "-"
                            Else 'Transferencia realizada a este viaje
                                debe = Math.Round(DecimalValue(mov.Cantidad), 2)
                            End If
                        Case ELL.Anticipo.Movimiento.TipoMovimiento.Diferencia_Cambio
                            If (mov.Cantidad > 0) Then  'Si la cantidad es positiva es que se le debia dinero al usuario
                                debe = Math.Abs(Math.Round(DecimalValue(mov.Cantidad), 2))
                            Else 'Si es negativa es que debia dinero
                                haber = Math.Abs(Math.Round(DecimalValue(mov.Cantidad), 2))
                                conversionEuros = "-"
                            End If
                        Case ELL.Anticipo.Movimiento.TipoMovimiento.Manual
                            If (mov.Cantidad > 0) Then
                                debe = Math.Abs(Math.Round(DecimalValue(mov.Cantidad), 2))
                            Else
                                haber = Math.Abs(Math.Round(DecimalValue(mov.Cantidad), 2))
                                conversionEuros = "-"
                            End If
                            If (mov.Comentarios <> String.Empty) Then modo &= " (" & mov.Comentarios & ")"
                    End Select
                    conversionEuros &= Math.Abs(Math.Round(mov.ImporteEuros, 2))
                    totalEuros += DecimalValue(conversionEuros)
                    cell = New PdfPCell(New Phrase(If(haber = 0, String.Empty, haber.ToString), font9)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                    cell = New PdfPCell(New Phrase(If(debe = 0, String.Empty, debe.ToString), font9)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                    cell = New PdfPCell(New Phrase(mov.Moneda.Nombre, font9)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                    cell = New PdfPCell(New Phrase(conversionEuros, font9)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                    cell = New PdfPCell(New Phrase(modo, font9)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                Next
                'Añadimos el pie para tener el total de euros
                cell = New PdfPCell(New Phrase("", font9)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                cell = New PdfPCell(New Phrase("", font9)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                cell = New PdfPCell(New Phrase("", font9)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                If (totalEuros > 0) Then
                    estadoTotalEuros = AccesoGenerico.GetTerminoStatic("Pendiente de justificar", cultura, localPath)
                ElseIf (totalEuros < 0) Then
                    estadoTotalEuros = AccesoGenerico.GetTerminoStatic("Exceso", cultura, localPath)
                Else
                    estadoTotalEuros = AccesoGenerico.GetTerminoStatic("Liquidado", cultura, localPath)
                End If
                cell = New PdfPCell(New Phrase(estadoTotalEuros, font9)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                cell = New PdfPCell(New Phrase(Math.Abs(totalEuros) & " €", font9Bold)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                cell = New PdfPCell(New Phrase("", font9Bold)) : cell.Border = PdfPCell.BOTTOM_BORDER : table.AddCell(cell)
                document.Add(table)
                Return path
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al generar el pdf del anticipo", ex)
            Finally
                If (document IsNot Nothing) Then
                    document.Close()
                    document.Dispose()
                    myFile.Close()
                    myFile.Dispose()
                End If
            End Try
        End Function

        ''' <summary>
        ''' Añade a los movimientos existentes, movimientos de la hoja de gastos si estan validadas y pertenecen al liquidador. Solo los gastos con recibo y sin recibo. Los de kilometraje y los de visa, no contaran aqui
        ''' Se añadiran los bloques de 'Metalico', 'Kilometraje' y 'Visa'
        ''' </summary>
        ''' <param name="lHojas"></param>
        ''' <param name="lMovimientos"></param>    
        ''' <param name="numHojasValidadas">Numero de hojas validadas</param>
        ''' <param name="idLiquidador">Id del responsable de liquidacion</param>
        ''' <param name="bParaAnticipo">Cuando es para anticipo, los gastos de visa y kilometraje no cuentan</param>
        ''' <returns>Devuelve el coste de las hojas de gastos en euros</returns>
        Private Shared Function AñadirHojasGastos(ByVal lHojas As List(Of ELL.HojaGastos), ByRef lMovimientos As List(Of ELL.Anticipo.Movimiento), ByRef numHojasValidadas As Integer, ByVal idLiquidador As Integer, Optional ByVal bParaAnticipo As Boolean = False) As Decimal
            Try
                Dim total As Decimal = 0
                If (lHojas IsNot Nothing) Then
                    Dim movimiento As ELL.Anticipo.Movimiento = Nothing
                    'Nos quedamos solo con las validadas
                    If (idLiquidador <> Integer.MinValue) Then
                        lHojas = lHojas.FindAll(Function(o As ELL.HojaGastos) (o.Estado <> ELL.HojaGastos.eEstado.Rellenada AndAlso o.Estado <> ELL.HojaGastos.eEstado.Enviada) And o.Usuario.Id = idLiquidador)
                    Else
                        lHojas = lHojas.FindAll(Function(o As ELL.HojaGastos) o.Estado <> ELL.HojaGastos.eEstado.Rellenada AndAlso o.Estado <> ELL.HojaGastos.eEstado.Enviada)
                    End If
                    numHojasValidadas = lHojas.Count
                    Dim xbatBLL As New BLL.XbatBLL
                    Dim moneda As ELL.Moneda = xbatBLL.GetMoneda("EUR")
                    For Each hoja As ELL.HojaGastos In lHojas
                        If (hoja.Lineas.Count > 0) Then
                            For Each gasto As ELL.HojaGastos.Linea In hoja.Lineas
                                If (Not bParaAnticipo OrElse (bParaAnticipo AndAlso (gasto.TipoGasto = ELL.HojaGastos.Linea.eTipoGasto.Metalico))) Then
                                    If (movimiento Is Nothing AndAlso (gasto.TipoGasto = ELL.HojaGastos.Linea.eTipoGasto.Metalico)) Then
                                        movimiento = New ELL.Anticipo.Movimiento With {.Id = gasto.Id, .IdAnticipo = gasto.IdHoja}
                                        movimiento.Cantidad = gasto.ImporteEuros
                                        movimiento.Moneda = moneda  'Lo ponemos en euros
                                        movimiento.UserOrigen = hoja.Usuario
                                        movimiento.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Hoja_Gastos
                                        movimiento.Fecha = hoja.GetFechaEstado(ELL.HojaGastos.eEstado.Validada)
                                    Else
                                        movimiento.Cantidad += gasto.ImporteEuros
                                    End If
                                    movimiento.ImporteEuros = movimiento.Cantidad
                                End If
                            Next
                        Else 'Solo Visas
                            If (movimiento Is Nothing) Then
                                movimiento = New ELL.Anticipo.Movimiento With {.IdAnticipo = hoja.Id, .TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Hoja_Gastos, .ImporteEuros = 0, .Moneda = New ELL.Moneda With {.Id = 90, .Abreviatura = "Eur"},
                                                                       .UserOrigen = hoja.Usuario, .Fecha = hoja.GetFechaEstado(ELL.HojaGastos.eEstado.Validada)}
                            End If
                        End If
                    Next
                    If (lMovimientos Is Nothing) Then lMovimientos = New List(Of ELL.Anticipo.Movimiento)
                    If (movimiento IsNot Nothing) Then
                        lMovimientos.Add(movimiento)
                        total = Math.Round(movimiento.ImporteEuros, 2)
                    End If
                End If
                Return total
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al añadir las hojas de gastos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Datos de cabecera para los anticipos. De momento, no se pinta nada
        ''' </summary>
        ''' <param name="idAnticipo">Id del anticipo</param>
        ''' <param name="cultura">Cultura en la que se traducira</param>
        Private Shared Function DatosInicialesAnt(ByVal idAnticipo As Integer, ByVal cultura As String) As Paragraph
            Return New Paragraph(" ")
        End Function

#End Region

        Public Class MyPageTempEvents
            Inherits PdfPageEventHelper

            Private logo As Image
            Private t1 As PdfPTable
            Private myFunc As Func(Of Paragraph)
            Private pie As Phrase

            Public Sub New(ByVal _logo As Image, ByVal _t1 As PdfPTable, ByVal _myfunc As Func(Of Paragraph), ByVal _pie As Phrase)
                logo = _logo
                t1 = _t1
                myFunc = _myfunc
                pie = _pie
            End Sub

            ''' <summary>
            ''' En este evento, cuando se rompa la hoja, se pintara la cabecera
            ''' </summary>
            ''' <param name="writer"></param>
            ''' <param name="document"></param>   
            Public Overrides Sub OnEndPage(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document)
                Dim dc = writer.DirectContent
                Dim Col As New ColumnText(dc)
                'Espacios del documento
                Col.SetSimpleColumn(30, document.PageSize.Height - 275, 585, document.PageSize.Height - 20)
                'Logo
                Col.AddElement(logo)
                'Titulo
                Col.AddElement(t1)
                'Texto que siempre saldra en todas las paginas
                Col.AddElement(myFunc())
                Col.Go()
                'Pie
                Col.SetSimpleColumn(document.PageSize.Width / 2 - 100, -200, 585, 50)
                Col.AddElement(pie)
                Col.Go()
                MyBase.OnEndPage(writer, document)
            End Sub
        End Class

        Public Class MyPageEvents
            Inherits PdfPageEventHelper

            Private logo As Image
            Private title As String
            Private myFunc As Func(Of Paragraph)
            Private pie As Phrase

            Public Sub New(ByVal _logo As Image, ByVal _title As String, ByVal _myfunc As Func(Of Paragraph), ByVal _pie As Phrase)
                logo = _logo
                title = _title
                myFunc = _myfunc
                pie = _pie
            End Sub

            ''' <summary>
            ''' En este evento, cuando se rompa la hoja, se pintara la cabecera
            ''' </summary>
            ''' <param name="writer"></param>
            ''' <param name="document"></param>   
            Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal document As Document)
                Dim dc = writer.DirectContent
                Dim Col As New ColumnText(dc)
                'Espacios del documento
                Col.SetSimpleColumn(30, document.PageSize.Height - 30, document.PageSize.Width - 50, 0)  '1: Donde empieza la X,2: Donde empieza la y (se mide desde abajo), 3: Donde termina la x (Param 1 + width), 4: Donde termina la y (Param2 + height) (NO INFLUYE EN NADA)
                Col.Go()
                'Logo y titulo
                Dim table As New PdfPTable(2) : table.LockedWidth = True : table.TotalWidth = 550
                Dim cell As New PdfPCell(New Phrase(New Chunk(logo, 0, 0, True))) : cell.Border = 0 : table.AddCell(cell)
                cell = New PdfPCell(New Phrase(New Chunk(title.ToUpper, fontTitle))) : cell.Border = 0 : cell.HorizontalAlignment = Element.ALIGN_RIGHT : table.AddCell(cell)
                Col.AddElement(table)
                Col.Go()
                'Texto que siempre saldra en todas las paginas
                Col.AddElement(myFunc())
                Col.Go()
                'Pie
                Col.SetSimpleColumn(document.PageSize.Width / 2 - 100, -200, 585, 50)
                Col.AddElement(pie)
                Col.Go()
                MyBase.OnEndPage(writer, document)
            End Sub
        End Class

    End Class

End Namespace
