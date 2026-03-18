Imports System.Runtime.InteropServices
Public Class catiainterop
    <CLSCompliant(False)> _
    Public Shared Sub executeInCatia(pathToCatia As String, f As Action(Of INFITF.Application))
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        Dim fi = New IO.FileInfo(pathToCatia)
        If Not fi.Exists Then
            Throw New Exception("No se encuentra el ejecutable de CATIA")
        End If
        Dim lstProcesos = System.Diagnostics.Process.GetProcessesByName("cnext")
        If lstProcesos.Count > 0 Then
            Throw New Exception("Catia se ha debido de quedar abierto")
        End If
        Try
            log.Info("Startin process ...")
            Dim UserTokenHandle As IntPtr = IntPtr.Zero
            WTSQueryUserToken(WTSGetActiveConsoleSessionId, UserTokenHandle)

            Dim ProcInfo As New PROCESS_INFORMATION
            Dim StartInfo As New STARTUPINFOW
            StartInfo.cb = CUInt(Runtime.InteropServices.Marshal.SizeOf(StartInfo))

            CreateProcessAsUser(UserTokenHandle, pathToCatia, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, False, 0, IntPtr.Zero, Nothing, StartInfo, ProcInfo)
            If Not UserTokenHandle = IntPtr.Zero Then
                CloseHandle(UserTokenHandle)
            End If
            log.Info("Process started")
            Threading.Thread.Sleep(1000 * 50)
            log.Info("Capturing CATIA ...")
            Dim oCatia As INFITF.Application = h.variosIntentos(Of INFITF.Application)(Function() System.Runtime.InteropServices.Marshal.GetActiveObject("CATIA.Application"), 3, 15000)  'GetObject(, "CATIA.Application")
            log.Info("CATIA captured")
            oCatia.DisplayFileAlerts = False
            f(oCatia)
            Threading.Thread.Sleep(1000)
            oCatia.Quit()
            Threading.Thread.Sleep(1000)
            For Each p In System.Diagnostics.Process.GetProcessesByName("cnext")
                p.Kill()
            Next
        Catch ex As Exception
            'Dejar Catia cerrado
            For Each p In System.Diagnostics.Process.GetProcessesByName("cnext")
                p.Kill()
            Next
            Throw
        End Try
    End Sub
    <CLSCompliant(False)> _
    Public Shared Sub traducirDRAW(oCatia As INFITF.Application, pathToAssembly As String, formats As String, outputPath As String, watermark As String, estado As String)
        Dim fOut = New IO.FileInfo(outputPath)
        AbrirCerrarDocumentoCatia(oCatia, pathToAssembly,
                                  Function(draw As DRAFTINGITF.DrawingDocument)
                                      For Each e In formats.Split(",")
                                          Select Case e
                                              Case "PDF"
                                                  draw.Update()
                                                  draw.ExportData(fOut.FullName + ".pdf", "pdf")
                                              Case "TIF"
                                                  Dim lstTempFileName As New List(Of String)
                                                  For i = 1 To draw.Sheets.Count
                                                      draw.Sheets.Item(i).Activate()
                                                      draw.Update()
                                                      Dim tempFileNameTif As String = fOut.FullName + "_" + draw.Sheets.Item(i).Name.Replace("*", "") + ".tif"
                                                      lstTempFileName.Add(tempFileNameTif)
                                                      draw.ExportData(tempFileNameTif, "tif")
                                                  Next
                                              Case Else
                                                  Throw New Exception("Formato no aceptado para Draw")
                                          End Select
                                      Next
                                      Return True
                                  End Function)
    End Sub
    <CLSCompliant(False)> _
    Public Shared Sub traducirPart(oCatia As INFITF.Application, pathToAssembly As String, formats As String, outputPath As String, coordSys As String)
        Dim fOut = New IO.FileInfo(outputPath)
        AbrirCerrarDocumentoCatia(oCatia, pathToAssembly,
                                  Function(doc As MECMOD.PartDocument)
                                      Dim part = doc.Part
                                      If coordSys = "PARTCSYS" Then 'Se necesita translacion de coordenadas
                                          AxistoAxisSelectioin(oCatia, part.Name)
                                      End If
                                      For Each e In formats.Split(",")
                                          Select Case e
                                              Case "STEP"
                                                  doc.ExportData(fOut.FullName + ".stp", "stp")
                                              Case "IGES"
                                                  doc.ExportData(fOut.FullName + ".igs", "igs")
                                          End Select
                                      Next
                                      Return True
                                  End Function)
    End Sub
    <CLSCompliant(False)> _
    Public Shared Function IsLicenseDSLAvailable(ocatia As INFITF.Application, outputFormat As String) As Boolean
        Dim license As String
        Select Case outputFormat
            Case "STEP"
                license = "ST1.prd"
            Case Else
                license = "_HD2.slt+"
        End Select
        Dim objLicCtrl As INFITF.LicenseSettingAtt = ocatia.SettingControllers.Item("CATSysLicenseSettingCtrl")
        If objLicCtrl.GetLicense(license) = "NotRequested" Then
            Return False
        Else
            Return True
        End If
    End Function

    <CLSCompliant(False)> _
    Public Shared Sub traducirAssy(oCatia As INFITF.Application, pathToAssembly As String, formats As String, outputPath As String, coordSys As String, auxName As String)
        Dim fOut = New IO.FileInfo(outputPath)
        AbrirCerrarDocumentoCatia(oCatia, pathToAssembly,
                                  Function(doc As INFITF.Document)
                                      If coordSys.ToUpper = "PARTCSYS" Then 'Se necesita translacion de coordenada
                                          'oldaxistoaxis(oCatia, auxName)
                                          doc = AxistoAxisSelectioin(oCatia, auxName)
                                      End If
                                      For Each e In formats.Split(",")
                                          Select Case e.ToUpper
                                              Case "STEP"
                                                  doc.ExportData(fOut.FullName + ".stp", "stp")
                                              Case "IGES"
                                                  doc.ExportData(fOut.FullName + ".igs", "igs")
                                          End Select
                                      Next
                                      Return True
                                  End Function)
        
    End Sub
    <CLSCompliant(False)> _
    Public Shared Sub allcatpart(oCatia As INFITF.Application)
        Dim s As INFITF.Selection = oCatia.ActiveDocument.Selection
        s.Add(oCatia.ActiveDocument.product)
        oCatia.StartCommand("Generate CATPart from Product")
        Threading.Thread.Sleep(1000)
        Dim destination As IntPtr = FindWindow(Nothing, "Generate CATPart from Product")
        Dim destControl As IntPtr = FindWindowEx(destination, IntPtr.Zero, "Button", "OK")
        SendMessage(destControl, &HF5&, 0, 0)
    End Sub

    Declare Function FindWindow Lib "user32" Alias "FindWindowA" _
                 (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Declare Function FindWindowEx Lib "user32" Alias "FindWindowExA" _
                     (ByVal hWnd As IntPtr, ByVal hWndChildAfterA As IntPtr, ByVal lpszClass As String, ByVal lpszWindow As String) As IntPtr
    Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
                 (ByVal hWnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As IntPtr

    <CLSCompliant(False)> _
    Private Shared Sub AbrirCerrarDocumentoCatia(oCatia As INFITF.Application, pathToAssembly As String, f As Func(Of INFITF.Document, Boolean))
        Dim fIn = New IO.FileInfo(pathToAssembly)
        If fIn.Exists Then
            Dim destination As IntPtr = FindWindow(Nothing, "Warm Start")
            Dim destControl As IntPtr = FindWindowEx(destination, IntPtr.Zero, "Button", "Aceptar")
            SendMessage(destControl, &HF5&, 0, 0)
            SendMessage(destControl, &HF5&, 0, 0)
            Dim doc As INFITF.Document = oCatia.Documents.Open(fIn.FullName)
            f(doc)
            doc.Close()
        Else
            Throw New Exception("El archivo CAD no existe")
        End If
    End Sub


    <CLSCompliant(False)> _
    Public Shared Function findPart(docs As INFITF.Documents, auxName As String)
        For Each doc As INFITF.Document In docs
            Dim part = findPartRec(doc, auxName)
            If part IsNot Nothing Then
                Return part
            End If
        Next
        Return Nothing
    End Function
    <CLSCompliant(False)> _
    Public Shared Function findPartRec(doc As INFITF.Document, auxName As String)
        If TypeName(doc).ToLower() = "partdocument" Then
            Dim docpart As MECMOD.PartDocument = doc
            If doc.Name.ToLower = auxName.ToLower + ".catpart" Then
                Return docpart.Part
            End If
        ElseIf TypeName(doc).ToLower() = "productdocument" Then
            Dim docpro As ProductStructureTypeLib.ProductDocument = doc
            loopTroughProducts(docpro.Product, auxName)
        End If
        Return Nothing
    End Function
    <CLSCompliant(False)> _
    Public Shared Function loopTroughProducts(pro As ProductStructureTypeLib.Product, auxName As String)
        For i = 1 To pro.Products.Count
            Dim p = pro.Products.Item(i)
            If TypeName(p).ToLower = "part" Then
                Dim part As MECMOD.Part = p
                If p.Name = auxName Then
                    Return p
                End If
            Else
                Dim pr As ProductStructureTypeLib.Product = p
                If pr.Products.Count > 0 Then
                    Return loopTroughProducts(pr, auxName)
                End If
            End If
        Next
        Return Nothing
    End Function
    <CLSCompliant(False)> _
    Public Shared Sub oldaxistoaxis(oCatia As INFITF.Application, auxName As String)
        Dim s As INFITF.Selection = oCatia.ActiveDocument.Selection
        Dim m As INFITF.Document = oCatia.ActiveDocument
        s.Search("'Part Design'.Part.Name='" + auxName + "',all")
        If s.Count = 0 Then
            Dim part = findPart(oCatia.Documents, auxName)
            If part Is Nothing Then
                Throw New ApplicationException("The auxiliary part name for the target axis could not be found. It might be disabled")
            Else
                s.Add(part)
            End If
        End If
        Dim p As MECMOD.Part = s.Item(1).Value

        Dim partCSys As MECMOD.AxisSystem
        Try
            partCSys = p.AxisSystems.Item("PARTCSYS")
            partCSys.Name = p.Name + "PARTCSYS"
        Catch ex As Exception
            Throw New ApplicationException("No se ha encontrado el PARTCSYS del par " + p.Name, ex)
        End Try
        Dim ABSCSYS As MECMOD.AxisSystem = p.AxisSystems.Add() 'Nuevo origen
        ABSCSYS.Name = p.Name + "ABSCSYS"
        p.Update()
        Dim pDoc As INFITF.Document = p.Parent
        pDoc.Activate()
        Dim sP As INFITF.Selection = oCatia.ActiveDocument.Selection
        'sP.Search("('Part Design'.'Axis System'.name=" + ABSCSYS.Name + "ABSCSYS + 'Part Design'.'Axis System'.name=" + partCSys.Name + "PARTCSYS);sel")
        s.Clear()
        sP.Add(ABSCSYS)
        sP.Add(partCSys)
        sP.VisProperties.SetShow(INFITF.CatVisPropertyShow.catVisPropertyShowAttr)
        'sP.Copy()
        'm.Activate()
        'Dim s2 As INFITF.Selection = oCatia.ActiveDocument.Selection
        's2.Search("'Part Design'.Part.Visibility=Visible;all")
        'For i = 1 To s2.Count
        '    Dim prt As MECMOD.Part = s2.Item(i).Value
        '    Dim doc As INFITF.Document = prt.Parent
        '    doc.Activate()
        '    Dim sel2 = oCatia.ActiveDocument.Selection
        '    sel2.Clear()
        '    sel2.Add(prt)
        '    sel2.PasteSpecial("CATPrtResultWithOutLink")
        '    sel2.Clear()
        '    prt.Update()
        '    m.Activate()
        '    s2 = oCatia.ActiveDocument.Selection
        '    s2.Search("'Part Design'.Part.Visibility=Visible;all")
        'Next
        's2.Clear()
        m.Activate()
        sP = oCatia.ActiveDocument.Selection
        sP.Search("'Part Design'.Body.visibility=Shown,all")
        For i = 1 To sP.Count
            Dim bo As MECMOD.Body = sP.Item(i).Value
            If Not bo.InBooleanOperation Then
                Dim newpart As MECMOD.Part = bo.Parent
                Dim newpartdoc As MECMOD.PartDocument = newpart.Parent
                newpartdoc.Activate()

                Dim mySF As MECMOD.Factory = newpart.ShapeFactory
                newpart.InWorkObject = bo
                Try
                    mySF.AddNewAxisToAxis2(newpart.CreateReferenceFromObject(partCSys), newpart.CreateReferenceFromObject(ABSCSYS))
                    newpart.Update()
                Catch ex As Exception
                    Throw New ApplicationException("Error al intentar AxisToAxis del body " + bo.Name, ex)
                End Try
            End If
        Next
    End Sub

    <CLSCompliant(False)> _
    Public Shared Function AxistoAxisSelectioin(oCatia As INFITF.Application, auxName As String) As INFITF.Document
        Dim s As INFITF.Selection = oCatia.ActiveDocument.Selection
        'Seleccionar aux part
        s.Search("'Part Design'.Part.Name='" + auxName + "',all")
        If s.Count = 0 Then
            Dim part = findPart(oCatia.Documents, auxName)
            If part Is Nothing Then
                Throw New ApplicationException("The auxiliary part name for the target axis could not be found. It might be disabled")
            Else
                s.Add(part)
            End If
        End If
        Dim p As MECMOD.Part = s.Item(1).Value
        Dim partCSys As MECMOD.AxisSystem
        Try
            partCSys = p.AxisSystems.Item("PARTCSYS")
            partCSys.Name = p.Name + "PARTCSYS"
        Catch ex As Exception
            Throw New ApplicationException("No se ha encontrado el PARTCSYS del par " + p.Name, ex)
        End Try
        Dim ABSCSYS As MECMOD.AxisSystem = p.AxisSystems.Add() 'Nuevo origen
        ABSCSYS.Name = p.Name + "ABSCSYS"
        p.Update()
        s.Clear()
        s.Add(partCSys)
        s.Add(ABSCSYS)
        s.VisProperties.SetShow(INFITF.CatVisPropertyShow.catVisPropertyShowAttr)
        s.Clear()
        Dim d As INFITF.Document = oCatia.ActiveDocument
        If TypeName(d).ToLower = "productdocument" Then
            allcatpart(oCatia)
        End If

        Dim s2 As INFITF.Selection = oCatia.ActiveDocument.Selection
        Dim allPartDoc As MECMOD.PartDocument = oCatia.ActiveDocument
        Dim allPart As MECMOD.Part = allPartDoc.Part

        Dim newPartCsys = allPart.AxisSystems.Item(p.Name + "PARTCSYS")
        Dim newABSCsys = allPart.AxisSystems.Item(p.Name + "ABSCSYS")
        s2.Search("'Part Design'.Body.visibility=Shown,all")
        For i = 1 To s2.Count
            Dim bo As MECMOD.Body = s2.Item(i).Value
            If Not bo.InBooleanOperation Then
                Dim mySF As MECMOD.Factory = allPart.ShapeFactory
                allPart.InWorkObject = bo
                Try
                    mySF.AddNewAxisToAxis2(allPart.CreateReferenceFromObject(newPartCsys), allPart.CreateReferenceFromObject(newABSCsys))
                    allPart.Update()
                Catch ex As Exception
                    Throw New ApplicationException("Error al intentar AxisToAxis del body " + bo.Name, ex)
                End Try
            End If
        Next
        s2.Clear()
        'Nuevo lugar para dejar las transformaciones hibridas
        Dim NewHB = allPart.HybridBodies.Add()
        NewHB.Name = "trans"
        allPart.Update()
        'Mover puntos, superficies y curvas
        s2.Search("('Part Design'.Point.Visibility=Shown+'Part Design'.Surface.Visibility=Shown+'Part Design'.Curve.Visibility=Shown);all")
        For i = 1 To s2.Count
            Dim hs As MECMOD.HybridShape = s2.Item(i).Value
            Dim myHSF As MECMOD.Factory = allPart.HybridShapeFactory
            Try
                Dim T = myHSF.AddNewAxisToAxis(allPart.CreateReferenceFromObject(hs), allPart.CreateReferenceFromObject(newPartCsys), allPart.CreateReferenceFromObject(newABSCsys))
                NewHB.AppendHybridShape(T)
                allPart.Update()
            Catch ex As Exception
                Throw New ApplicationException("Error al intentar AxisToAxis de puntos, curvas y superficies elemento: " + hs.Name, ex)
            End Try
        Next
        s2.VisProperties.SetShow(INFITF.CatVisPropertyShow.catVisPropertyNoShowAttr)
        Return oCatia.ActiveDocument
    End Function
    '<CLSCompliant(False)> _
    'Public Shared Function ParentBodyAlreadyTreated(o As INFITF.AnyObject, hs As HashSet(Of String)) As Boolean
    '    If Text.RegularExpressions.Regex.IsMatch(o.Name, "CNEXT") Then
    '        Return False
    '    End If
    '    If TypeName(o).ToLower = "bodies" Then
    '        Dim bs As MECMOD.Bodies = o

    '        For i = 1 To bs.Count
    '            If hs.Contains(bs.Item(i).Name) Then
    '                Return True
    '            End If
    '        Next
    '    End If
    '    If hs.Contains(o.Name) Then
    '        Return True
    '    Else
    '        Return ParentBodyAlreadyTreated(o.Parent, hs)
    '    End If
    'End Function
    <CLSCompliant(False)> _
    Public Shared Function returnParentPart(o As INFITF.AnyObject) As MECMOD.Part
        If Text.RegularExpressions.Regex.IsMatch(o.Name, "\.CATPart", Text.RegularExpressions.RegexOptions.IgnoreCase) Then
            Dim partdoc As MECMOD.PartDocument = o
            Return partdoc.Part
        Else
            Return returnParentPart(o.Parent)
        End If
    End Function

    <StructLayout(LayoutKind.Sequential)> _
    <CLSCompliant(False)> _
    Public Structure SECURITY_ATTRIBUTES
        Public nLength As UInteger
        Public lpSecurityDescriptor As IntPtr
        <MarshalAs(UnmanagedType.Bool)> _
        Public bInheritHandle As Boolean
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    <CLSCompliant(False)> _
    Public Structure STARTUPINFOW
        Public cb As UInteger
        <MarshalAs(UnmanagedType.LPWStr)> _
        Public lpReserved As String
        <MarshalAs(UnmanagedType.LPWStr)> _
        Public lpDesktop As String
        <MarshalAs(UnmanagedType.LPWStr)> _
        Public lpTitle As String
        Public dwX As UInteger
        Public dwY As UInteger
        Public dwXSize As UInteger
        Public dwYSize As UInteger
        Public dwXCountChars As UInteger
        Public dwYCountChars As UInteger
        Public dwFillAttribute As UInteger
        Public dwFlags As UInteger
        Public wShowWindow As UShort
        Public cbReserved2 As UShort
        Public lpReserved2 As IntPtr
        Public hStdInput As IntPtr
        Public hStdOutput As IntPtr
        Public hStdError As IntPtr
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    <CLSCompliant(False)> _
    Public Structure PROCESS_INFORMATION
        Public hProcess As IntPtr
        Public hThread As IntPtr
        Public dwProcessId As UInteger
        Public dwThreadId As UInteger
    End Structure
    <DllImport("Wtsapi32.dll", EntryPoint:="WTSQueryUserToken", SetLastError:=True)> _
    <CLSCompliant(False)> _
    Public Shared Function WTSQueryUserToken(ByVal SessionId As UInteger, ByRef phToken As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    <DllImport("kernel32.dll", EntryPoint:="WTSGetActiveConsoleSessionId", SetLastError:=True)> _
    <CLSCompliant(False)> _
    Public Shared Function WTSGetActiveConsoleSessionId() As UInteger
    End Function
    <DllImport("advapi32.dll", EntryPoint:="CreateProcessAsUserW", SetLastError:=True)> _
    <CLSCompliant(False)> _
    Public Shared Function CreateProcessAsUser(<InAttribute()> ByVal hToken As IntPtr, _
                                                    <InAttribute(), MarshalAs(UnmanagedType.LPWStr)> ByVal lpApplicationName As String, _
                                                    ByVal lpCommandLine As System.IntPtr, _
                                                    <InAttribute()> ByVal lpProcessAttributes As IntPtr, _
                                                    <InAttribute()> ByVal lpThreadAttributes As IntPtr, _
                                                    <MarshalAs(UnmanagedType.Bool)> ByVal bInheritHandles As Boolean, _
                                                    ByVal dwCreationFlags As UInteger, _
                                                    <InAttribute()> ByVal lpEnvironment As IntPtr, _
                                                    <InAttribute(), MarshalAsAttribute(UnmanagedType.LPWStr)> ByVal lpCurrentDirectory As String, _
                                                    <InAttribute()> ByRef lpStartupInfo As STARTUPINFOW, _
                                                    <OutAttribute()> ByRef lpProcessInformation As PROCESS_INFORMATION) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    <DllImport("kernel32.dll", EntryPoint:="CloseHandle", SetLastError:=True)> _
    Public Shared Function CloseHandle(<InAttribute()> ByVal hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
End Class
