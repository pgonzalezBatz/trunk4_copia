Imports System.Web.Mvc
Imports LinqKit
Namespace Controllers
    Public Class busquedaController
        Inherits Controller

        Private dbES As New Entities_soldadura
        Private strCn As String = ConfigurationManager.ConnectionStrings("oracle").ConnectionString

        Function Index(q As String, sortby As String) As ActionResult
            Dim dbInformes As IQueryable(Of INFORMES)
            If String.IsNullOrEmpty(q) Then
                Return View("index")
            Else
                dbInformes = dbES.INFORMES.AsExpandable.Where(h.searchFilter(Of INFORMES)(q, Function(i) i.VALOROF, Function(i) i.VALOROF + "-" + i.VALOROP, Function(i) i.CLIENTE, Function(i) i.PROYECTO, Function(i) i.MARCA))
            End If
            Select Case sortby
                Case "ofop"
                    dbInformes = dbInformes.OrderBy(Function(i) i.VALOROF + i.VALOROP)
                Case "tipoinforme"
                    dbInformes = dbInformes.OrderBy(Function(i) i.TIPOINFORME)
            End Select
            Return View("index", dbInformes)
        End Function
        Function downloadintercambio(lstInforme As List(Of Int32))
            Dim result = IO.Path.GetRandomFileName().TakeWhile(Function(c) c <> ".")
            Dim rootPath = "\\hpnas1\INTERCAMBIO\certificados_calidad\" + String.Join("", result) + "\"

            Dim mlstInforme = lstInforme.Select(Function(i)
                                                    Dim oinf = dbES.INFORMES.Find(i)
                                                    Return New With {.nombreTroquel = oinf.PROYECTO, .numord = oinf.VALOROF, .numope = oinf.VALOROP, .marca = oinf.MARCA, .idInforme = i}
                                                End Function)
            Dim j = 1
            Dim format = "{0:D" + mlstInforme.Count.ToString.Length.ToString + "}"
            For Each i In mlstInforme
                Dim uniqueFier = String.Format(format, j)
                Dim c = DependencyResolver.Current.GetService(Of INFORMEController)()
                c.ControllerContext = New ControllerContext(Request.RequestContext, c)
                Dim di = System.IO.Directory.CreateDirectory(rootPath + i.nombreTroquel + "\" + i.numord.ToString + "\" + i.numope.ToString)
                Dim file As New IO.FileStream(di.FullName + "\" + cleanToFileNameSafeChars(i.marca) + "____"+ uniqueFier  +".pdf", IO.FileMode.Create, IO.FileAccess.Write)
                Dim ms As IO.MemoryStream = c.InformePDF(i.idInforme).FileStream
                ms.WriteTo(file)
                file.Close()
                j = j + 1
            Next
            Return RedirectToAction("downloadConfirmation", h.ToRouteValues(h.ToRouteValuesDelete(Request.QueryString, "lstInforme"), New With {.path = (New System.Uri(rootPath)).AbsoluteUri}))
        End Function
        Function downloadConfirmation(path As String)
            Return View()
        End Function

        Function cleanToFileNameSafeChars(fileName As String) As String
            For Each c In IO.Path.GetInvalidFileNameChars
                fileName = fileName.Replace(c, "")
            Next
            Return fileName.Substring(0, Math.Min(fileName.Length, 150))
        End Function
        Function sendemail(q As String, sortby As String, id As Integer) As ActionResult
            'No podemos pasarle el id de SimpleRoleProvider por que puede ser el de uno de los proveedores seleccionados
            Dim email = db.getEmailUsuario(User.Identity.Name.ToLower, strCn)
            'Reuperar informe llamando a la funcion ya implementada en otro controlador
            Dim c = DependencyResolver.Current.GetService(Of INFORMEController)()
            c.ControllerContext = New ControllerContext(Request.RequestContext, c)
            Dim a = c.InformePDF(id)
            composeEmail(email, "Certificado Nº " + id.ToString, "Certificado adjuntado", New Net.Mail.Attachment(a.FileStream, "certificado_" + id.ToString, "application/pdf"))
            Return Index(q, sortby)
        End Function
        Private Shared Sub composeEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String, ParamArray ByVal at() As Net.Mail.Attachment)
            Dim b = New Net.Mail.MailMessage()
            b.Subject = subject
            b.Body = body
            b.IsBodyHtml = False
            For Each a In at
                b.Attachments.Add(a)
            Next
            b.From = New Net.Mail.MailAddress("salmetafakturazioa@batz.es")
            b.To.Add(recipients)
            Dim smtp = New Net.Mail.SmtpClient("posta.batz.com")
            smtp.Send(b)
        End Sub
    End Class
End Namespace