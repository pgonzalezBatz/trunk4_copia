Imports NXOpen
Imports NXOpen.UF
Imports NXOpen.Drawings
Public Class nxinterop
    <CLSCompliant(False)>
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
    Public Function GetUnloadOption(ByVal dummy As String) As Integer

        'Unloads the image when the NX session terminates
        'GetUnloadOption = NXOpen.Session.LibraryUnloadOption.AtTermination

        '----Other unload options-------
        'Unloads the image immediately after execution within NX
        GetUnloadOption = NXOpen.Session.LibraryUnloadOption.Immediately

        'Unloads the image explicitly, via an unload dialog
        'GetUnloadOption = NXOpen.Session.LibraryUnloadOption.Explicitly
        '-------------------------------

    End Function
    <CLSCompliant(False)>
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
            Throw
        Finally
            nxSession.Parts.CloseAll(BasePart.CloseModified.CloseModified, Nothing)
        End Try
    End Sub
    <CLSCompliant(False)> _
    Private Shared Function getVisibleObjects(nxSession As Session, nxUfSession As UFSession) As NXObject()
        Dim o As NXOpen.Tag = Nothing
        Dim lstObj As New List(Of NXObject)
        Do
            nxUfSession.View.CycleObjects(Tag.Null, UFView.CycleObjectsEnum.VisibleObjects, o)
            If nxUfSession.Obj.AskStatus(o) = UFConstants.UF_OBJ_ALIVE Then
                Dim type As Integer = 0
                Dim subType As Integer = 0
                nxUfSession.Obj.AskTypeAndSubtype(o, type, subType)
                If type = UFConstants.UF_solid_type AndAlso subType = UFConstants.UF_solid_body_subtype OrElse type <> UFConstants.UF_solid_type Then
                    'UFConstants.UF_circular_subtype()
                    'If UFConstants.UF_coordinate_system_type <> type Then
                    'End If
                    Dim el = CType(Utilities.NXObjectManager.Get(o), NXObject)
                    Select el.GetType().Name
                        Case "Point"
                            lstObj.Add(CType(Utilities.NXObjectManager.Get(o), NXObject))
                        Case "Body"
                            lstObj.Add(CType(Utilities.NXObjectManager.Get(o), NXObject))
                        Case "Arc"
                            lstObj.Add(CType(Utilities.NXObjectManager.Get(o), NXObject))
                        Case "Line"
                            lstObj.Add(CType(Utilities.NXObjectManager.Get(o), NXObject))
                    End Select
                End If
            End If
        Loop While o <> Nothing
        Return lstObj.ToArray
    End Function

    <CLSCompliant(False)>
    Public Shared Sub traducirStep(nxSession As Session, nxUfSession As UFSession, outputPath As String)
        Dim stepFile = IO.Path.Combine("\\hpnas2\CATIAV5CFG\UGS\NX9.0.3_D1_DAIMLER\STEP214UG\", "ugstep214.def")
        nxSession.EnableRedo(False)
        Dim partFullName As String = String.Empty

        nxUfSession.Part.AskPartName(nxSession.Parts.Work.Tag, partFullName)

        Dim fo As New IO.FileInfo(outputPath)
        Dim step214Creator1 As StepCreator
        step214Creator1 = nxSession.DexManager.CreateStepCreator()
        step214Creator1.SettingsFile = stepFile
        step214Creator1.ObjectTypes.Solids = True
        step214Creator1.ObjectTypes.Surfaces = True
        step214Creator1.ObjectTypes.Curves = True

        step214Creator1.LayerMask = "1-256"
        step214Creator1.ExportSelectionBlock.SelectionScope = NXOpen.ObjectSelector.Scope.SelectedObjects
        step214Creator1.ExportSelectionBlock.SelectionComp.Add(getVisibleObjects(nxSession, nxUfSession))

        step214Creator1.InputFile = partFullName
        step214Creator1.OutputFile = fo.FullName
        step214Creator1.ProcessHoldFlag = True

        step214Creator1.Commit()
        step214Creator1.Destroy()
        Dim a = 1
    End Sub

    <CLSCompliant(False)>
    Public Shared Sub exporttoigs(nxSession As Session, ufs As UFSession, outputPath As String)
        Dim igsC As IgesCreator = nxSession.DexManager.CreateIgesCreator()
        igsC.SettingsFile = IO.Path.Combine("c:/CADSoft/NX/NX.8.5.1.3.mp03/IGES", "igesexport.def")
        Dim p As Part = nxSession.Parts.Work
        Dim fo As New IO.FileInfo(outputPath)
        'TODO: Clean up commented lines
        'igsC.ExportSelectionBlock.SelectionScope = ObjectSelector.Scope.EntirePart
        'Dim lst As New List(Of NXObject)


        'For Each displayableObject As DisplayableObject In p.Views.WorkView.AskVisibleObjects()
        '    If TypeOf displayableObject Is Body OrElse TypeOf displayableObject Is Curve OrElse TypeOf displayableObject Is Point OrElse TypeOf displayableObject Is Plane Then
        '        lst.Add(displayableObject)
        '    End If
        'Next

        igsC.ExportSelectionBlock.SelectionScope = NXOpen.ObjectSelector.Scope.SelectedObjects
        igsC.ExportSelectionBlock.SelectionComp.Add(getVisibleObjects(nxSession, ufs))
        'igsC.ExportSelectionBlock.SelectionComp.Add(lst.ToArray())
        'igsC.InputFile = nxSession.Parts.Work.FullPath
        igsC.OutputFile = fo.FullName
        igsC.LayerMask = "1-256"
        igsC.ProcessHoldFlag = True
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
    Public Shared Sub exportParasolid(nxSession As Session, ufs As UFSession, path As String)
        If IO.File.Exists(path + ".x_t") Then
            Exit Sub
        End If
        Dim lstB As New List(Of NXOpen.Tag)
        nxSession.Parts.Work.LoadFully()
        For Each displayableObject As DisplayableObject In nxSession.Parts.Work.Views.WorkView.AskVisibleObjects()
            If TypeOf displayableObject Is Body Then
                lstB.Add(displayableObject.Tag)
            End If
        Next
        Dim a(lstB.Count - 1) As NXOpen.Tag
        For i = 0 To lstB.Count - 1
            a(i) = lstB(i)
        Next
        ufs.Ps.ExportData(a, path)
    End Sub
    <CLSCompliant(False)> _
    Public Shared Sub importParasolid(nxSession As Session, ufs As UFSession, path As String)
        Dim a(0) As NXOpen.Tag
        a(0) = nxSession.Parts.Work.Tag
        ufs.Ps.ImportData(path, a)
    End Sub
    <CLSCompliant(False)>
    Public Shared Sub importAndMoveParasolid(nxSession As Session, ufs As UFSession, path As String, targetobjectPath As String, igsStepOutPath As String)
        Dim p = nxSession.Parts.Work
        Dim displayPart As Part = nxSession.Parts.Display
        Dim targetPart
        If nxSession.Parts.ToArray.Count = 1 Then
            targetPart = nxSession.Parts(0)
        Else
            Try
                targetPart = nxSession.Parts.FindObject(targetobjectPath)
            Catch ex As Exception
                Throw New ApplicationException("No se ha encontrado el target part " + targetobjectPath)
            End Try
        End If

        targetPart.LoadThisPartFully()

        Dim originPartCsys As NXOpen.Point3d
        Dim xDirectionPartcsys, yDirectionPartCsys As NXOpen.Vector3d
        Dim gotpartcsys = False
        Dim l = nxSession.Parts.Work.Views.WorkView.AskVisibleObjects().Where(Function(o) TypeOf o Is CoordinateSystem)
        If l.Count = 0 Then
            Throw New ApplicationException("No se ha encontrado CoordinateSystem en" + targetobjectPath)
        End If
        For Each displayableObject As DisplayableObject In l
            If TypeOf displayableObject Is CoordinateSystem Then
                Dim c As CoordinateSystem = displayableObject
                If c.Name = "PARTCSYS" Then
                    c.GetDirections(xDirectionPartcsys, yDirectionPartCsys)
                    originPartCsys = c.Origin
                    gotpartcsys = True
                End If
            End If
        Next
        If Not gotpartcsys Then
            For Each c As CoordinateSystem In targetPart.CoordinateSystems
                If c.Name = "PARTCSYS" Then
                    c.GetDirections(xDirectionPartcsys, yDirectionPartCsys)
                    originPartCsys = c.Origin
                End If
            Next
        End If
        'Import parasolid
        Dim im = displayPart.ImportManager.CreateParasolidImporter()
        im.FileName = path
        Dim psObject As Part = im.Commit()
        im.Destroy()
        Dim targetCSYS, absCsys As CoordinateSystem
        'Create new absolute axis
        Dim absXform As Xform = displayPart.Xforms.CreateXform(SmartObject.UpdateOption.WithinModeling, 1)
        absCsys = displayPart.CoordinateSystems.CreateCoordinateSystem(absXform, SmartObject.UpdateOption.WithinModeling)
        'Create new partcsys
        targetCSYS = displayPart.CoordinateSystems.CreateCoordinateSystem(originPartCsys, xDirectionPartcsys, yDirectionPartCsys)
        If targetCSYS Is Nothing Then
            Throw New ApplicationException("No se ha encontrado el PARTCSYS para el part " + targetobjectPath)
        End If
        If absCsys Is Nothing Then
            Throw New ApplicationException("No se ha encontrado el ABSCSYS para el part " + targetobjectPath)
        End If
        Dim nullFeatures_MoveObject As Features.MoveObject = Nothing
        Dim mob As Features.MoveObjectBuilder = p.BaseFeatures.CreateMoveObjectBuilder(nullFeatures_MoveObject) 'displayPart.BaseFeatures.CreateMoveObjectBuilder(nullFeatures_MoveObject)
        mob.TransformMotion.Option = GeometricUtilities.ModlMotion.Options.CsysToCsys
        mob.MoveObjectResult = Features.MoveObjectBuilder.MoveObjectResultOptions.MoveOriginal
        mob.TransformMotion.FromCsys = targetCSYS
        mob.TransformMotion.ToCsys = absCsys

        Dim lst As New List(Of NXObject)
        For Each displayableObject As DisplayableObject In nxSession.Parts.Work.Views.WorkView.AskVisibleObjects()
            If TypeOf displayableObject Is Body OrElse TypeOf displayableObject Is Curve OrElse TypeOf displayableObject Is Point OrElse TypeOf displayableObject Is Plane Then
                lst.Add(displayableObject)
            End If
        Next

        mob.ObjectToMoveObject.Add(psObject.Bodies.ToArray)
        mob.Commit()
        mob.Destroy()

        Dim igsC As IgesCreator = nxSession.DexManager.CreateIgesCreator()
        igsC.SettingsFile = IO.Path.Combine("c:/CADSoft/NX/NX.8.5.1.3.mp03/IGES", "igesexport.def")
        Dim fo As New IO.FileInfo(igsStepOutPath + ".igs")
        igsC.ExportSelectionBlock.SelectionScope = NXOpen.ObjectSelector.Scope.SelectedObjects
        igsC.ExportSelectionBlock.SelectionComp.Add(psObject.Bodies.ToArray())
        igsC.OutputFile = fo.FullName
        igsC.LayerMask = "1-256"
        igsC.ProcessHoldFlag = True
        igsC.Commit()
        igsC.Destroy()




        Dim stepFile = IO.Path.Combine("\\hpnas2\CATIAV5CFG\UGS\NX 11.0\STEP214UG\", "ugstep214.def")
        nxSession.EnableRedo(False)
        Dim foStep As New IO.FileInfo(igsStepOutPath + ".stp")
        Dim step214Creator1 As StepCreator
        step214Creator1 = nxSession.DexManager.CreateStepCreator()
        step214Creator1.SettingsFile = stepFile
        step214Creator1.ObjectTypes.Solids = True
        step214Creator1.LayerMask = "1-256"
        step214Creator1.ExportSelectionBlock.SelectionScope = NXOpen.ObjectSelector.Scope.SelectedObjects
        step214Creator1.ExportSelectionBlock.SelectionComp.Add(psObject.Bodies.ToArray())
        step214Creator1.OutputFile = foStep.FullName
        step214Creator1.ProcessHoldFlag = True
        step214Creator1.Commit()
        step214Creator1.Destroy()

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
