Imports Neodynamic.SDK.Printing
Public Class zebraController
    Inherits System.Web.Mvc.Controller

    '
    ' GET: /zebra

    Function Index() As ActionResult
        printLabel(New With {.proveedor = "INMETRO, S:L.", .calle = "Bª sarasola, S/N", .codigoPostal = "48142", .poblacion = "Artea", .provincia = "Bizkaia", .telefono = "946318006", .peso = 15})
        Return View()
    End Function

    Private Sub printLabel(o As Object)
        Dim worker As New Threading.Thread(New Threading.ThreadStart(Sub() Me.ThermalLabelWorker(o)))
        worker.SetApartmentState(Threading.ApartmentState.STA)
        worker.Name = "ThermalLabelWorker"
        worker.Start()
        worker.Join()
    End Sub
    Private Sub ThermalLabelWorker(o As Object)
        Dim ps As New PrinterSettings()
        Dim tLabel = New ThermalLabel() : tLabel.UnitType = UnitType.Mm : tLabel.Height = 100 : tLabel.Width = 100

        Dim yPos = 5 : Dim xPos = 13

        Dim tI1 = New TextItem() : tI1.X = xPos : tI1.Y = yPos + 0.5 : tI1.Width = tLabel.Width - xPos : tI1.Height = 6 : tI1.Text = o.proveedor : tI1.Font.Name = "Arial" : tI1.Font.Bold = True : tI1.Font.Unit = FontUnit.Point : tI1.Font.Size = 15
        Dim tIDir = New TextItem() : tIDir.X = xPos : tIDir.Y = yPos + 9 : tIDir.Width = (tLabel.Width - xPos) / 2 : tIDir.Height = 5 : tIDir.Font.Name = "Arial" : tIDir.Font.Unit = FontUnit.Point : tIDir.Font.Size = 11 : tIDir.Font.Bold = True
        tIDir.Text = "Dirección"
        Dim tI2 = New TextItem() : tI2.X = xPos : tI2.Y = yPos + 14 : tI2.Width = (tLabel.Width - xPos) / 2 : tI2.Height = 35 : tI2.Font.Name = "Arial" : tI2.Font.Unit = FontUnit.Point : tI2.Font.Size = 11
        tI2.Text = o.calle + Environment.NewLine + o.codigopostal + " " + o.poblacion + Environment.NewLine + o.provincia + Environment.NewLine + "Tel." + o.telefono


        Dim tIDat = New TextItem() : tIDat.X = xPos + ((tLabel.Width - xPos) / 2) : tIDat.Y = yPos + 9 : tIDat.Width = (tLabel.Width - xPos) / 2 : tIDat.Height = 5 : tIDat.Font.Name = "Arial" : tIDat.Font.Unit = FontUnit.Point : tIDat.Font.Size = 11 : tIDat.Font.Bold = True
        tIDat.Text = "Contenido"
        Dim tI3 = New TextItem() : tI3.X = xPos + ((tLabel.Width - xPos) / 2) : tI3.Y = yPos + 14 : tI3.Width = 49 : tI3.Height = 35 : tI3.Font.Name = "Arial" : tI3.Font.Unit = FontUnit.Point : tI3.Font.Size = 11
        tI3.Text = "Peso: " + o.peso.ToString + " Kg" + Environment.NewLine + "Bulto Nº:"

        Dim tIbotom = New TextItem() : tIbotom.X = xPos : tIbotom.Y = 88 : tIbotom.Width = 54 : tIbotom.Height = 4.5 : tIbotom.Text = "Torrea Auzoa 2,  48140 IGORRE (Bizkaia) Spain " + Environment.NewLine + "Tel. (34) 94 630 50 00 Fax (34) 94 630 50 20" : tIbotom.Font.Name = "Arial" : tIbotom.Font.Unit = FontUnit.Point : tIbotom.Font.Size = 6
        Dim sq = New RectangleShapeItem() : sq.X = xPos : sq.Y = 93 : sq.FillColor = Color.Black : sq.Width = 55 : sq.Height = 2
        Dim mg1 = New ImageItem() : mg1.SourceFile = Server.MapPath("~") + "content\logo_batz.png" : mg1.X = 70 : mg1.Y = 88 : mg1.Width = 26 : mg1.Height = 7.3


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
        ps.Communication.CommunicationType = CommunicationType.Network
        ps.Communication.NetworkIPAddress = System.Net.IPAddress.Parse("10.7.0.147")
        ps.Communication.NetworkPort = 9100
        Dim pj = New PrintJob(ps)

        'Set Thermal Printer language
        pj.PrinterSettings.ProgrammingLanguage = ProgrammingLanguage.ZPL
        pj.PrinterSettings.PrinterName = "Zebra  GK420t"
        Try
            'pj.Print(tLabel)
            pj.ExportToPdf(tLabel, "C:\proba.pdf", 200)
            pj.CleanCache()
            pj.Dispose()
        Catch ex As Exception
            pj.CleanCache()
            pj.Dispose()
        End Try
    End Sub

End Class