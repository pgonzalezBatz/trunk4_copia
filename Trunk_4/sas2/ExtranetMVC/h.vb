Imports System.Security
Partial Public Class h

    Public Shared Sub SendEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String)
        Dim b = New Net.Mail.MailMessage()
        b.Subject = subject
        b.Body = body
        b.From = New Net.Mail.MailAddress("segipe@batz.es")
        b.To.Add(recipients)
        Dim smtp = New Net.Mail.SmtpClient(getMailServer())
        smtp.Send(b)
    End Sub
    Public Shared Sub SendEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String, ByVal at As Net.Mail.Attachment)
        Dim b = New Net.Mail.MailMessage()
        b.Subject = subject
        b.Body = body
        b.IsBodyHtml = False
        b.Attachments.Add(at)
        b.From = New Net.Mail.MailAddress("segipe@batz.es")
        b.To.Add(recipients)
        Dim smtp = New Net.Mail.SmtpClient(getMailServer())
        smtp.Send(b)
    End Sub

    Public Shared Function getRole(ByVal idSab As Integer) As Integer
        If ConfigurationManager.AppSettings("inproductivas").Split(",").Contains(idSab) Then
            Return Role.inproductiva + Role.productiva
        Else
            Return Role.productiva
        End If
    End Function

    Public Shared Function getMailServer() As String
        Dim strCn As String = ConfigurationManager.ConnectionStrings("sab").ConnectionString
        Return db.getMailServer(strCn)
    End Function
End Class