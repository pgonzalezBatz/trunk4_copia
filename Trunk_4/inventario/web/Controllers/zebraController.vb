Imports Neodynamic.SDK.Printing
Imports System.Security.Permissions
Public Class zebraController
    Inherits System.Web.Mvc.Controller

    Private strCn As String = ConfigurationManager.ConnectionStrings("inventario").ConnectionString

    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
    Function printLabels() As ActionResult
        Return View()
    End Function
    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
    <AcceptVerbs(HttpVerbs.Post)> _
    Function printLabels(lineas As Nullable(Of Integer)) As ActionResult
        If lineas.HasValue Then
            For i = 1 To lineas
                printLine(db.GetNumeroParaEtiqueta(strCn).ToString("000000"), False)
            Next
            Return RedirectToAction("index", "inventario")
        End If
        ModelState.AddModelError("lineas", "Es obligatorio introducir el numero de lineas a imprimir")
        Return View()
    End Function
    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
    Function reprintLabels() As ActionResult
        Return View()
    End Function
    <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
    <AcceptVerbs(HttpVerbs.Post)> _
    Function reprintLabels(idetiqueta As Nullable(Of Integer)) As ActionResult
        If idetiqueta.HasValue Then
            printLine(idetiqueta.Value.ToString("000000"), True)
            Return RedirectToAction("index", "inventario")
        End If
        ModelState.AddModelError("lineas", "Es obligatorio introducir el numero de lineas a imprimir")
        Return View()
    End Function

    Private Sub printLine(code As String, isReprint As Boolean)
        Dim worker As New Threading.Thread(New Threading.ThreadStart(Sub() Me.ThermalLabelWorker(code, isReprint)))
        worker.SetApartmentState(Threading.ApartmentState.STA)
        worker.Name = "ThermalLabelWorker"
        worker.Start()
        worker.Join()
    End Sub
    Private Sub ThermalLabelWorker(code As String, isReprint As Boolean)
        Dim ps As New PrinterSettings()
        Dim tLabel = New ThermalLabel(UnitType.Mm, 22, 12)
        Dim tcItem1 = New TextItem() : Dim bcItem2 = New BarcodeItem() : Dim tcItem3 = New TextItem()

        tcItem1.Text = "Batz S.Coop."
        tcItem1.X = 5.5
        tcItem1.Y = 0.5
        tcItem1.Width = 22
        tcItem1.Height = 3
        tcItem1.UnitType = UnitType.Mm
        tcItem1.Font.Name = "Arial"
        tcItem1.Font.Unit = FontUnit.Point
        tcItem1.Font.Size = 5

        bcItem2.UnitType = UnitType.Mm
        bcItem2.Symbology = BarcodeSymbology.Code128
        bcItem2.Code = code
        bcItem2.AddChecksum = False
        bcItem2.BarWidth = 0.28
        bcItem2.BarHeight = 5
        bcItem2.X = 0
        bcItem2.Y = 2.7
        bcItem2.Width = 21
        bcItem2.Height = 9.2
        bcItem2.CounterStep = 1
        bcItem2.CounterUseLeadingZeros = True

        tLabel.LabelsPerRow = 3
        tLabel.LabelsHorizontalGapLength = 3
        tLabel.GapLength = 2
        tLabel.Items.Add(tcItem1)
        tLabel.Items.Add(bcItem2)
        ps.Dpi = 203
        'Netwrok setup
        ps.Communication.CommunicationType = CommunicationType.Network
        ps.Communication.NetworkIPAddress = System.Net.IPAddress.Parse("10.1.0.150")
        ps.Communication.NetworkPort = 9100

        Dim pj = New PrintJob(ps)

        If isReprint Then
            pj.Copies = 1
        Else
            pj.Copies = 3
        End If


        'Set Thermal Printer language
        pj.PrinterSettings.ProgrammingLanguage = ProgrammingLanguage.ZPL
        pj.PrinterSettings.PrinterName = "Zebra  GK420t"
        Try
            pj.Print(tLabel)
            'pj.ExportToPdf(tLabel, "C:\Users\aazkuenaga\Documents\Trunk_4\SBatz\inventario\web\obj\Debug\proba.pdf", 200)
            pj.CleanCache()
            pj.Dispose()
        Catch ex As Exception
            pj.CleanCache()
            pj.Dispose()
        End Try
    End Sub
End Class