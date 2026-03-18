Imports NXOpen
Imports NXOpen.UF
Imports NXOpen.Drawings
Imports System.Runtime.InteropServices
Public Class nxinterop
    <CLSCompliant(False)> _
    Public Shared Sub opennx(pathToNx As String, f As System.Action(Of NXOpen.Session, NXOpen.UF.UFSession))
        Dim fi = New IO.FileInfo(pathToNx)
        If Not fi.Exists Then
            Throw New Exception("El archivo bat para lanzar NX no existe")
        End If
        Dim i = Shell(fi.FullName, AppWinStyle.NormalFocus, False)
        Threading.Thread.Sleep(10000)
        Try
            Dim theSession As Session = Session.GetSession()
            Dim theUfSession As UFSession = UFSession.GetUFSession()
            f(theSession, theUfSession)
            For Each p In System.Diagnostics.Process.GetProcessesByName("ugraf")
                p.Kill()
            Next
        Catch ex As Exception
            For Each p In System.Diagnostics.Process.GetProcessesByName("ugraf")
                p.Kill()
            Next
            Throw
        End Try
    End Sub
    <CLSCompliant(False)> _
    Public Shared Sub AbrirArchivo(path As String, outputPath As String, nxSession As Session, nxUfSession As UFSession, f As Action(Of Session, UFSession, String))
        Dim fi As New IO.FileInfo(path)
        If Not fi.Exists Then
            Throw New Exception("El archivo bat para lanzar NX no existe")
        End If
        Dim er As UFPart.LoadStatus
        nxUfSession.Part.Open(fi.FullName, NXOpen.Tag.Null, er)
        Try
            f(nxSession, nxUfSession, outputPath)
        Catch ex As Exception
            nxUfSession.Part.CloseAll()
            Throw
        End Try
        nxUfSession.Part.CloseAll()
    End Sub

    <CLSCompliant(False)> _
    Public Shared Sub traducirStep(nxSession As Session, nxUfSession As UFSession, outputPath As String)
        Dim step214File = IO.Path.Combine("c:/CADSoft/NX/NX.8.5.1.3.mp03/STEP214UG/", "ugstep214.def")
        Dim p As Part = nxSession.Parts.Work
        Dim fo As New IO.FileInfo(outputPath)
        Dim step214Creator1 As Step214Creator
        step214Creator1 = nxSession.DexManager.CreateStep214Creator()
        step214Creator1.SettingsFile = step214File
        step214Creator1.ObjectTypes.Solids = True
        step214Creator1.ObjectTypes.Surfaces = True
        step214Creator1.ObjectTypes.Curves = True
        step214Creator1.ObjectTypes.Structures = True
        step214Creator1.LayerMask = "1-256"
        step214Creator1.InputFile = p.FullPath
        step214Creator1.OutputFile = fo.FullName
        step214Creator1.Commit()
        step214Creator1.Destroy()
    End Sub
    <CLSCompliant(False)> _
    Public Shared Function traducirJT(derivedPath As String, startFile As String, jtBatPath As String) As IO.FileInfo
        Dim psi As New ProcessStartInfo
        Dim p = New Process With {.StartInfo = psi}
        p.StartInfo.FileName = "cmd.exe"
        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        p.StartInfo.CreateNoWindow = True
        'Needed to redirect standard error/output/input
        p.StartInfo.UseShellExecute = False
        p.StartInfo.RedirectStandardInput = True
        'p.StartInfo.RedirectStandardOutput = True

        'Add handler for when data is received
        'AddHandler p.OutputDataReceived, AddressOf SDR
        p.Start()
        'Begin async data reading
        'p.BeginOutputReadLine()
        'p.StandardInput.WriteLine("pushd " + derivedPath)
        p.StandardInput.WriteLine(jtBatPath + " " + "-force_output_dir=""" + derivedPath + """ """ + derivedPath + "\" + startFile + """")
        p.StandardInput.WriteLine("exit")
        p.WaitForExit()
        Dim fi = New IO.FileInfo(derivedPath + "\" + startFile.Replace(".prt", ".jt"))
        If fi.Exists Then
            Return fi
        End If
        Throw New Exception("No se ha podido crear el archivo JT")
    End Function
    'Private Shared Sub SDR(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
    '    Trace.WriteLine(e.Data)
    'End Sub
    <CLSCompliant(False)> _
    Public Shared Sub exporttoigs(nxSession As Session, ufs As UFSession, outputPath As String)
        Dim igsC As IgesCreator = nxSession.DexManager.CreateIgesCreator()
        igsC.SettingsFile = IO.Path.Combine("c:/CADSoft/NX/NX.8.5.1.3.mp03/IGES", "igesexport.def")
        Dim p As Part = nxSession.Parts.Work
        Dim fo As New IO.FileInfo(outputPath)
        igsC.ExportSelectionBlock.SelectionScope = ObjectSelector.Scope.EntirePart
        igsC.ObjectTypes.Structures = True
        igsC.ObjectTypes.Curves = True
        igsC.ObjectTypes.Solids = True
        igsC.ObjectTypes.Surfaces = True
        igsC.InputFile = p.FullPath
        igsC.OutputFile = fo.FullName
        igsC.LayerMask = "1-256"
        igsC.Commit()
        igsC.Destroy()
    End Sub
    <CLSCompliant(False)> _
    Public Shared Sub exporttoigs(nxSession As Session, ufs As UFSession, outputPath As String, targetCysName As String)
        Dim igsC As IgesCreator = nxSession.DexManager.CreateIgesCreator()
        igsC.SettingsFile = IO.Path.Combine("c:/CADSoft/NX/NX.8.5.1.3.mp03/IGES", "igesexport.def")
        Dim p As Part = nxSession.Parts.Work

        Dim fo As New IO.FileInfo(outputPath)
        If Not String.IsNullOrEmpty(targetCysName) Then
            Dim targetPart = nxSession.Parts.FindObject(targetCysName)
            targetPart.LoadThisPartFully()
            Dim targetCSYS As CoordinateSystem
            For Each c As CoordinateSystem In targetPart.CoordinateSystems
                If c.Name = "PARTCSYS" Then
                    targetCSYS = c
                End If
            Next
            igsC.Csys = targetCSYS 'targetPart.CoordinateSystems.ToArray().
        End If
        igsC.ExportSelectionBlock.SelectionScope = ObjectSelector.Scope.EntirePart
        igsC.ObjectTypes.Structures = True
        igsC.ObjectTypes.Curves = True
        igsC.ObjectTypes.Solids = True
        igsC.ObjectTypes.Surfaces = True
        igsC.InputFile = p.FullPath
        igsC.OutputFile = fo.FullName
        igsC.LayerMask = "1-256"
        igsC.Commit()
        igsC.Destroy()
    End Sub

    <CLSCompliant(False)> _
    Public Shared Sub exporttopdf(nxSession As Session, outputPath As String, watermark As String)
        Dim printPDFBuilder1 As PrintPDFBuilder
        Dim p As Part = nxSession.Parts.Work
        printPDFBuilder1 = p.PlotManager.CreatePrintPdfbuilder()
        Dim drawingSheets As DrawingSheet() = p.DrawingSheets.ToArray
        Dim fo As New IO.FileInfo(outputPath)
        Dim dwgUnits As DrawingSheet.Unit = drawingSheets(0).Units
        If dwgUnits = DrawingSheet.Unit.Inches Then
            printPDFBuilder1.Units = PrintPDFBuilder.UnitsOption.English
        End If

        If dwgUnits = DrawingSheet.Unit.Millimeters Then
            printPDFBuilder1.Units = PrintPDFBuilder.UnitsOption.Metric
        End If
        printPDFBuilder1.XDimension = drawingSheets(0).Length()
        printPDFBuilder1.YDimension = drawingSheets(0).Height()
        printPDFBuilder1.Scale = 1.0
        printPDFBuilder1.Action = PrintPDFBuilder.ActionOption.New
        printPDFBuilder1.Colors = PrintPDFBuilder.Color.BlackOnWhite
        printPDFBuilder1.Widths = PrintPDFBuilder.Width.StandardWidths
        printPDFBuilder1.Size = PrintPDFBuilder.SizeOption.ScaleFactor
        printPDFBuilder1.ImageResolution = PrintPDFBuilder.ImageResolutionOption.High
        printPDFBuilder1.RasterImages = True
        printPDFBuilder1.ShadedGeometry = True
        printPDFBuilder1.SourceBuilder.SetSheets(drawingSheets)
        printPDFBuilder1.Filename = fo.FullName
        printPDFBuilder1.Watermark = watermark
        printPDFBuilder1.AddWatermark = watermark.Length > 0
        printPDFBuilder1.Commit()
        printPDFBuilder1.Destroy()
    End Sub
    <CLSCompliant(False)> _
    Public Shared Sub exporttotiff(inPath As String, outPath As String)
        Dim fi As New IO.FileInfo(inPath)
        Dim fo As New IO.FileInfo(outPath)
        Dim imgmgk As New Process()
        With imgmgk.StartInfo
            .FileName = "C:\Program Files\ImageMagick-6.8.9-Q16\convert.exe"
            .UseShellExecute = False
            .CreateNoWindow = True
            .RedirectStandardOutput = True
            .RedirectStandardError = True
            .RedirectStandardInput = False
            .Arguments = """" + fi.FullName + """ """ + fo.FullName + """"
        End With
        imgmgk.Start()
        Dim output As String = imgmgk.StandardOutput.ReadToEnd()
        Dim errorMsg As String = imgmgk.StandardError.ReadToEnd()
        imgmgk.WaitForExit()
        imgmgk.Close()
    End Sub
    <CLSCompliant(False)> _
    Public Shared Sub MoveComponent(nxSession As Session, targetobjectPath As String)
        Dim p = nxSession.Parts.Work
        Dim targetPart = nxSession.Parts.FindObject(targetobjectPath)
        targetPart.LoadThisPartFully()
        Dim targetCSYS, absCsys As CoordinateSystem
        For Each c As CoordinateSystem In targetPart.CoordinateSystems
            If c.Name = "PARTCSYS" Then
                targetCSYS = c
            End If
            If c.Name = "" Then
                absCsys = c
            End If
        Next
        If targetCSYS Is Nothing Then
            Throw New Exception()
        End If

        Dim nullFeatures_MoveObject As Features.MoveObject = Nothing
        Dim mob As Features.MoveObjectBuilder = p.BaseFeatures.CreateMoveObjectBuilder(nullFeatures_MoveObject)
        mob.TransformMotion.Option = GeometricUtilities.ModlMotion.Options.CsysToCsys
        mob.TransformMotion.DistanceAngle.OrientXpress.AxisOption = GeometricUtilities.OrientXpressBuilder.Axis.Passive
        mob.TransformMotion.DistanceAngle.OrientXpress.PlaneOption = GeometricUtilities.OrientXpressBuilder.Plane.Passive
        mob.TransformMotion.OrientXpress.AxisOption = GeometricUtilities.OrientXpressBuilder.Axis.Passive
        mob.TransformMotion.OrientXpress.PlaneOption = GeometricUtilities.OrientXpressBuilder.Plane.Passive
        mob.TransformMotion.DeltaEnum = GeometricUtilities.ModlMotion.Delta.ReferenceWcsWorkPart
        mob.MoveObjectResult = Features.MoveObjectBuilder.MoveObjectResultOptions.MoveOriginal
        mob.TransformMotion.FromCsys = targetCSYS
        mob.TransformMotion.ToCsys = absCsys

        'For Each pr As Part In nxSession.Parts
        '    mob.ObjectToMoveObject.Add(pr.Bodies.ToArray)
        'Next
        If p.Bodies.ToArray.Count = 0 Then
            'Throw New Exception 'Todo: Move asssemblies
            For Each pr As Part In nxSession.Parts
                mob.ObjectToMoveObject.Add(pr.Bodies.ToArray)
            Next
        Else
            mob.ObjectToMoveObject.Add(p.Bodies.ToArray)
        End If
        mob.Commit()
        mob.Destroy()
    End Sub

    <CLSCompliant(False)> _
    Shared Sub getAllComponents2(ByVal comp As Assemblies.Component, ByRef allComp As ArrayList)
        Dim child As Assemblies.Component = Nothing
        Dim space As String = Nothing
        For Each child In comp.GetChildren()
            allComp.Add(child)
            getAllComponents2(child, allComp)
        Next
    End Sub
End Class
