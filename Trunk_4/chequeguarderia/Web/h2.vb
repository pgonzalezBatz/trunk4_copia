Imports System.Globalization
Imports System.Security
Public Class H2

    Public Shared Function MySelectList(Of O)(ByVal items As IEnumerable(Of O), ByVal f As Func(Of O, Mvc.SelectListItem)) As List(Of Mvc.SelectListItem)
        Dim lst As New List(Of Mvc.SelectListItem)
        For Each i In items
            lst.Add(f(i))
        Next
        Return lst
    End Function
    Public Shared Function GetMonthName(ByVal m As Integer) As String
        Dim d As New Dictionary(Of Integer, String) From {
            {1, "Enero"},
            {2, "Febrero"},
            {3, "Marzo"},
            {4, "Abril"},
            {5, "Mayo"},
            {6, "Junio"},
            {7, "Julio"},
            {8, "Agosto"},
            {9, "Septiembre"},
            {10, "Octubre"},
            {11, "Noviembre"},
            {12, "Diciembre"}
        }
        Return d(m)
    End Function
    Public Shared Function CalcularCorte(ByVal f As DateTime, ByVal fechaCorte As Integer) As DateTime
        Dim newDate As DateTime
        If f.Day < fechaCorte Then
            newDate = f
        Else
            newDate = f.AddMonths(1)
        End If
        Return New Date(newDate.Year, newDate.Month, 1)
    End Function
    Public Shared Sub SendEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String)
        If String.IsNullOrEmpty(recipients) Then
            Exit Sub
        End If

        Dim b = New Net.Mail.MailMessage With {
            .Subject = subject,
            .Body = body,
            .From = New Net.Mail.MailAddress("chequegourmet@batz.es")
        }
        b.To.Add(recipients)
        Dim smtp = New Net.Mail.SmtpClient("posta.batz.es")
        smtp.Send(b)
    End Sub

    Public Shared Function ListOfIntegerToString(ByVal l As List(Of Integer)) As String
        Dim s As New Text.StringBuilder()
        For Each i In l
            s.Append(i.ToString + ", ")
        Next
        s.Remove(s.Length - 1, 1) ' Quitar la ultima coma
        Return s.ToString
    End Function
    Public Shared Sub SetCulture(ByVal idSab As Integer, ByVal strCn As String)
        If HttpContext.Current.Request.Cookies("culture") Is Nothing Then
            HttpContext.Current.Response.Cookies.Add(New HttpCookie("culture", DB.GetUserCulture(idSab, strCn)))
        End If
    End Sub
    Public Shared Function GetCulture(ByVal idSab As Integer, ByVal strCn As String) As String
        If HttpContext.Current.Request.Cookies("culture") Is Nothing Then
            Dim cookie = DB.GetUserCulture(idSab, strCn)
            HttpContext.Current.Response.Cookies.Add(New HttpCookie("culture", cookie))
            Return cookie
        End If
        Return HttpContext.Current.Request.Cookies("culture").Value
    End Function
    Public Shared Function GetCulture() As String
        If HttpContext.Current.Request.Cookies("culture") Is Nothing Then
            Return "es-ES"
        End If
        Return HttpContext.Current.Request.Cookies("culture").Value
    End Function
End Class
