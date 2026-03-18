Public Class h

    <CLSCompliant(False)> _
    Public Shared Function variosIntentos(Of T)(f As Func(Of T), nIntentos As Integer, tiempoReintento As Integer) As T
        Dim i = 1
        Do
            Try
                Dim ret As T = f()
                Return ret
            Catch ex As System.ApplicationException
                Throw ex
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

    Public Shared Sub deleteFiles(lstFileName As List(Of String))
        For Each f In lstFileName
            Dim fi As New IO.FileInfo(f)
            If fi.Exists Then
                fi.Delete()
            End If
        Next
    End Sub

    Public Shared Sub SendEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String)
        Dim b = New Net.Mail.MailMessage()
        b.Subject = subject
        b.Body = body
        b.From = New Net.Mail.MailAddress("traductor_cad@batz.es")
        b.To.Add(recipients)
        b.IsBodyHtml = True
        Dim smtp = New Net.Mail.SmtpClient("posta.batz.es")
        smtp.Send(b)
    End Sub

End Class
