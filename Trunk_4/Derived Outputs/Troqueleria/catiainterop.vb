Public Class catiainterop

    Public Shared Sub executeInCatia(pathToBat As String, f As Action(Of INFITF.Application))
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        Dim fi = New IO.FileInfo(pathToBat)
        If Not fi.Exists Then
            Throw New ApplicationException("No se encuentra el ejecutable que levantara CATIA")
        End If
        Dim lstProcesos = System.Diagnostics.Process.GetProcessesByName("cnext")
        If lstProcesos.Count > 0 Then
            Throw New ApplicationException("Catia se ha debido de quedar abierto")
        End If
        Try
            Dim psi As New ProcessStartInfo(fi.FullName)
            psi.RedirectStandardError = True
            psi.RedirectStandardOutput = True
            psi.CreateNoWindow = False
            psi.WindowStyle = ProcessWindowStyle.Normal
            psi.UseShellExecute = False
            log.Debug("Ejecutando el .bat que hara la llamada a CATIA ...")
            Dim process As Process = Process.Start(psi)
            Threading.Thread.Sleep(10000)
            log.Debug("Intentamos enlazar el objeto CATIA con nuestro codigo ...")
            Dim oCatia As INFITF.Application = h.variosIntentos(Of INFITF.Application)(Function() System.Runtime.InteropServices.Marshal.GetActiveObject("CATIA.Application"), 15, 40000)
            log.Debug("Ok")
            h.variosIntentos(Sub()
                                 oCatia.DisplayFileAlerts = False
                             End Sub, 5, 5000)
            log.Debug("CATIA esta listo para abrir documentos")
            f(oCatia)
            h.variosIntentos(Sub()
                                 For Each p In System.Diagnostics.Process.GetProcessesByName("cnext")
                                     p.Kill()
                                 Next
                             End Sub, 5, 5000)
        Catch ex As Exception
            For Each p In System.Diagnostics.Process.GetProcessesByName("cnext")
                p.Kill()
            Next
            Throw
        End Try
    End Sub
    Declare Function FindWindow Lib "user32" Alias "FindWindowA" _
                 (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Declare Function FindWindowEx Lib "user32" Alias "FindWindowExA" _
                     (ByVal hWnd As IntPtr, ByVal hWndChildAfterA As IntPtr, ByVal lpszClass As String, ByVal lpszWindow As String) As IntPtr
    Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
                 (ByVal hWnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As IntPtr

    Public Shared Sub AbrirCerrarDocumentoCatia(oCatia As INFITF.Application, pathToAssembly As String, f As Action(Of INFITF.Document))
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        Dim fIn = New IO.FileInfo(pathToAssembly)
        If fIn.Exists Then
            log.Debug("Cerramos warm startup..")
            Dim destination As IntPtr = FindWindow(Nothing, "Warm Start")
            Dim destControl As IntPtr = FindWindowEx(destination, IntPtr.Zero, "Button", "Aceptar")
            SendMessage(destControl, &HF5&, 0, 0)
            SendMessage(destControl, &HF5&, 0, 0)
            log.Debug("Intentamos abrir archivo " + fIn.FullName + " ...")
            Dim doc As INFITF.Document = h.variosIntentos(Of INFITF.Document)(Function() oCatia.Documents.Open(fIn.FullName), 5, 5000)
            log.Debug("Ok")
            f(doc)
            doc.Close()
        Else
            Throw New ApplicationException("El archivo CAD no existe")
        End If
    End Sub

    Public Shared Sub Create3DText(oCatia As INFITF.Application, text As String)
        Dim activeDoc = oCatia.ActiveDocument




        'Dim Selection1 As Object
        Dim Selection1 = activeDoc.Selection
        Selection1.Clear()

        Dim IOT(0)
        IOT(0) = "PlanarFace"
        Dim strreturn As String
        strreturn = Selection1.SelectElement2(IOT, "Select a planar face", False)

        If strreturn <> "Normal" Then
            Exit Sub
        End If

        Dim reference1 As INFITF.Reference = Selection1.Item(1).Reference
        Selection1.Clear()

        Dim part1 As MECMOD.PartDocument
        If TypeName(activeDoc) = "ProductDocument" Then
            part1 = activeDoc.Selection.Item(1).LeafProduct.ReferenceProduct.Parent.Part
        ElseIf TypeName(activeDoc) = "PartDocument" Then
            part1 = activeDoc
        End If

        Dim InWorkObject1 = part1.Part.InWorkObject
        Dim documents1 = oCatia.Documents

        Dim drawingDocument1 As DRAFTINGITF.DrawingDocument = documents1.Add("Drawing")
        Dim drawingSheets1 As DRAFTINGITF.DrawingSheets = drawingDocument1.Sheets
        Dim drawingSheet1 As DRAFTINGITF.DrawingSheet = drawingSheets1.Item(1)
        drawingSheet1.[Scale] = 1.0#
        Dim drawingViews1 As DRAFTINGITF.DrawingViews = drawingSheet1.Views
        Dim drawingView1 As DRAFTINGITF.DrawingView = drawingViews1.Item("Main View")
        Dim drawingTexts1 As DRAFTINGITF.DrawingTexts = drawingView1.Texts

        Dim drawingText1 As DRAFTINGITF.DrawingText = drawingTexts1.Add(text, 0, 0)
        Dim iFontSize As Double
        iFontSize = 100.5
        drawingText1.SetFontSize(0, 0, iFontSize)

        ' TEMP DXF
        Dim TempDXF_Path As String = "C:\Users\aazkuenaga.BATZNT\Documents\TEMP.dxf"

        drawingDocument1.ExportData(TempDXF_Path, "dxf")
        drawingDocument1.Close()


        Dim document2 As INFITF.Document
        document2 = documents1.Open(TempDXF_Path)

        Dim drawingDocument2 As DRAFTINGITF.DrawingDocument = oCatia.ActiveDocument

        Dim selection2 As INFITF.Selection = drawingDocument2.Selection
        selection2.Search("Drafting.View;all")
        selection2.Copy()

        Selection1.Clear()
        Selection1.Add(reference1)
        Selection1.Paste()
        drawingDocument2.Close()

        System.IO.File.Delete(TempDXF_Path)
        Selection1.Remove(1)
        'Selection1.Item(2).Reference

        oCatia.StartCommand(“Change Sketch Support”)
    End Sub

    Public Shared Sub Traducir3DXML(doc As INFITF.Document, outputPath As String)
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        log.Debug("Exportando a 3DXML " + outputPath)
        doc.ExportData(outputPath, "3dxml")
        log.Debug("OK")
    End Sub
    Public Shared Sub traducirDRAW(draw As DRAFTINGITF.DrawingDocument, outputPath As String)
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        draw.Update()
        log.Debug("Exportando a PDF " + outputPath)
        Try
            h.variosIntentos(Sub() draw.ExportData(outputPath, "pdf"), 3, 1000)
        Catch ex As Exception

        End Try
        log.Debug("OK")
    End Sub

    Public Shared Function GetBatPathFromCliente(cliente As String, mappingPath As String) As String
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        Using reader As New IO.StreamReader(mappingPath, Text.Encoding.UTF8)
            'Avanzar la linea de comentario
            log.Debug("Buscando entorno para cliente " + cliente)
            reader.ReadLine()
            While Not reader.EndOfStream
                Dim l = reader.ReadLine()
                Dim s = l.Split(",")
                If s(0).ToLower = cliente.ToLower Then
                    log.Debug("Encontrado entorno " + s(1) + ".bat")
                    Return s(1) + ".bat"
                End If
            End While
        End Using
        Throw New ApplicationException("No se ha podido encontrar entorno para el cliente " + cliente)
    End Function
    Public Shared Function getListOfDrawings(folderRoot As String) As List(Of IO.FileInfo)
        Dim d = New IO.DirectoryInfo(folderRoot)
        Return d.GetFiles("*.CATDrawing", IO.SearchOption.AllDirectories).ToList()
    End Function
End Class
