Public Class h2
    Public Shared Sub SendEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String)
        If String.IsNullOrEmpty(recipients) Then
            Exit Sub
        End If
        Dim b = New Net.Mail.MailMessage()
        b.Subject = subject
        b.Body = body
        b.From = New Net.Mail.MailAddress("chequegourmet@batz.es")
        b.To.Add(recipients)
        Dim smtp = New Net.Mail.SmtpClient("posta.batz.com")
        smtp.Credentials = New System.Net.NetworkCredential("tareas", "tareas123")
        smtp.DeliveryFormat = Net.Mail.SmtpDeliveryFormat.International
        smtp.Send(b)
    End Sub
End Class
