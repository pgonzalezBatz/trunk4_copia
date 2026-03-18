Public Class dslsinterop


    Public Shared Function GetAvailableLicenses() As Dictionary(Of String, Object)
        Dim executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location
        Dim executableDir = System.IO.Path.GetDirectoryName(executablePath)
        Dim inputPath = executableDir + "\licenses_input.txt"
        Dim strCommand = """C:\Program Files\Dassault Systemes\DS License Server\win_b64\code\bin\DSLicSrv.exe"""
        'Dim args = " -admin  -run -ks" + """ c lobantzo 4084 -r;glu -all """
        Dim args = " -admin -i """ + inputPath + ""

        Dim p As Process = New Process()
        p.StartInfo.FileName = strCommand
        p.StartInfo.Arguments = args
        p.StartInfo.RedirectStandardOutput = True
        p.StartInfo.UseShellExecute = False
        p.StartInfo.CreateNoWindow = True

        p.Start()
        Dim lst As New List(Of Object)
        While Not p.StandardOutput.EndOfStream
            Dim line As String = p.StandardOutput.ReadLine()
            If line.Contains("customerId:") Then
                Dim spltStr = line.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)
                lst.Add(New With {.licenceName = spltStr(0).Replace(Constants.vbTab, ""), .count = spltStr(12), .used = spltStr(14)})
            End If
        End While
        Return lst.GroupBy(Function(l) l.licenceName, Function(key, l)
                                                          Return New With {.licenceName = key, .count = l.Sum(Function(l2) l2.count), .used = l.Sum(Function(l2) l2.used), .free = l.Sum(Function(l2) l2.count) - l.Sum(Function(l2) l2.used)}
                                                      End Function).ToDictionary(Of String, Object)(Function(l) l.licenceName, Function(l) l)
    End Function

    Public Shared Function GetAvailableLicense(targetLicense As String) As String
        Dim d = GetAvailableLicenses()
        Select Case targetLicense
            Case "STEP"
                If d("ST1").free > 0 AndAlso d("HD2").free > 0 Then
                    Return "ST1_HD2"
                ElseIf d("ST1").free > 0 AndAlso d("MD2").free > 0 Then
                    Return "ST1_MD2"
                End If
            Case Else
                If d("MD2").free > 0 Then
                    Return "MD2"
                ElseIf d("HD2").free > 0 Then
                    Return "HD2"

                End If
        End Select
        Return Nothing
    End Function
End Class
