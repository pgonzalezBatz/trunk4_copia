Public Class plminterop
    Dim wsPLMCADUtils As New DerivedOutputs.plmcadutilsservice.PLMCADUtilsService

    Public Shared Sub ClearFolders(path As String)
        IO.Directory.Delete(path, True)
    End Sub
    Public Shared Sub CreateInOutDirectories(Path As String)
        IO.Directory.CreateDirectory(Path + "/in")
        IO.Directory.CreateDirectory(Path + "/out")
    End Sub

    Public Shared Function ReadyCADFiles(path As String, Id As String, Id2 As String, estado As String)
        Dim wsPLMCADUtils As New DerivedOutputs.plmcadutilsservice.PLMCADUtilsService
        Dim xmlResponse = wsPLMCADUtils.CADCheckoutRequest(Id, Id2, estado, "AsStored")
        Dim el = ParseCADCheckoutRequest(xmlResponse)
        For Each f In el.listOfFiles
            If f.type = "Native" Then
                Dim netPath = New IO.DirectoryInfo(path + "/in")
                Dim feedback = wsPLMCADUtils.CADCheckoutFile(el.id, f.format, f.name, netPath.FullName)
                If feedback <> "Success" Then
                    Throw New Exception(feedback)
                End If
            End If
        Next
        For Each c In el.listOfComponent
            For Each f In c.listOfFiles
                If f.type = "Native" Then
                    Dim feedback = wsPLMCADUtils.CADCheckoutFile(c.id, f.format, f.name, path + "/in")
                    If feedback <> "Success" Then
                        Throw New Exception(feedback)
                    End If
                End If
            Next
        Next
        Return el
    End Function

    Public Shared Sub checkInCADFiles(path As String, cadId As String, cadVersionId As String, estado As String)
        Dim wsPLMCADUtils As New DerivedOutputs.plmcadutilsservice.PLMCADUtilsService
        '-Dim lst = IO.Directory.EnumerateFiles(path)
        Dim lst = IO.Directory.GetFiles(path)
        For Each f In lst
            Dim fi = New IO.FileInfo(f)
            Dim format
            Select Case Text.RegularExpressions.Regex.Match(f, ".*\.(\w*)$").Groups(1).Value
                Case "stp"
                    format = "STEP"
                Case "tif"
                    format = "TIF"
                Case "pdf"
                    format = "PDF"
                Case "igs"
                    format = "IGES"
                Case Else
                    Continue For
            End Select
            Dim feedback
            If estado = "Preliminary" Or estado = "Review" Then
                feedback = wsPLMCADUtils.CADCheckinFile(cadVersionId, format, fi.Name, path)
            Else
                feedback = wsPLMCADUtils.CADCheckinFile(cadId, format, fi.Name, path)
            End If
            If feedback <> "Success" Then
                Throw New Exception(feedback)
            End If
        Next
    End Sub


    Public Shared Function ParseCADCheckoutRequest(s As String)
        Dim xDoc = Xml.Linq.XDocument.Parse(s)
        If xDoc.Descendants.Elements("ResponseStatus").Value = "ERROR" Then
            Throw New Exception(xDoc.Descendants.Elements("ErrorMsg").Value)
        Else
            Return xDoc.Descendants.Elements("ResponseData").Select(
                Function(e)
                    Dim lstFiles = e.Element("Files").Elements("File").Select(
                                       Function(f)
                                           Return New plmFile With {.type = f.Attribute("type").Value,
                                                            .format = f.Element("FileFormat").Value,
                                                         .name = f.Element("FileName").Value}
                                       End Function).ToList
                    If lstFiles.Where(Function(f) f.type = "Native").ToList.Count > 1 Then
                        Throw New Exception("Hay más de un formato nativo")
                    End If
                    Return New With {.type = e.Element("rootType").Value,
                                   .id = e.Element("ID").Value,
                                   .listOfFiles = lstFiles,
                                   .listOfComponent = e.Element("SubComponents").Elements("SubComponent").Select(
                                       Function(c)
                                           Return New With {.id = c.Element("ID").Value,
                                                                      .listOfFiles = c.Element("Files").Elements("File").Select(Function(f)
                                                                                                                                    Return New plmFile With {.type = f.Attribute("type").Value,
                                                                                                                                                     .format = f.Element("FileFormat").Value,
                                                                                                                                                  .name = f.Element("FileName").Value}
                                                                                                                                End Function).ToList}
                                       End Function).ToList}
                End Function).ToList.First
        End If
    End Function
End Class