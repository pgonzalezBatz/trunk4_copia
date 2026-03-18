Imports System.IO
Partial Public Class h

    Private Shared strCnSab = ConfigurationManager.ConnectionStrings("sab").ConnectionString
    Public Shared Sub SendEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String)
        Dim b = New Net.Mail.MailMessage()
        b.Subject = subject
        b.Body = body
        b.From = New Net.Mail.MailAddress("segipe@batz.es")
        b.To.Add(recipients)
        Dim smtp = New Net.Mail.SmtpClient(db.getMailServer(strCnSab))
        Dim SMTPUserInfo As New System.Net.NetworkCredential("tareas", "tareas123")
        smtp.UseDefaultCredentials = False
        smtp.Credentials = SMTPUserInfo
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
        Dim smtp = New Net.Mail.SmtpClient(db.getMailServer(strCnSab))
        Dim SMTPUserInfo As New System.Net.NetworkCredential("tareas", "tareas123")
        smtp.UseDefaultCredentials = False
        smtp.Credentials = SMTPUserInfo
        smtp.Send(b)
    End Sub
End Class

