Imports System.Security.Permissions
Namespace efa
    Public Class recursoController
        Inherits System.Web.Mvc.Controller

        Private strCnSab As String = ConfigurationManager.ConnectionStrings("SAB").ConnectionString
        Private strCnEfa As String = ConfigurationManager.ConnectionStrings("BaliabideF").ConnectionString
        Private strCnTelef As String = ConfigurationManager.ConnectionStrings("Telefonia").ConnectionString
        Private strCnEpsilon As String = ConfigurationManager.ConnectionStrings("Epsilon").ConnectionString


        <SimpleRoleProvider(EfaRole.usuarioTemporal)>
        Function Accion() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(EfaRole.usuarioTemporal)>
        Function ListGrupo() As ActionResult
            Dim l = DB.GetListOfGrupo(DB.GetIdPlanta(SimpleRoleProvider.GetId(), strCnSab), strCnEfa)
            l.Add(New Grupo With {.Nombre = "Telefono"})
            Dim lstExcepciones = ConfigurationManager.AppSettings("gruposparanegociosespecificos").Split({";"c}, StringSplitOptions.RemoveEmptyEntries).Select(Function(s)
                                                                                                                                                                   Dim s2 = s.Split(":")
                                                                                                                                                                   Return New With {.recurso = s2(0), .negocio = s2(1)}
                                                                                                                                                               End Function).ToList
            ViewData("recursos") = l.Where(Function(r)
                                               Dim o = lstExcepciones.Find(Function(s) Regex.IsMatch(r.Nombre, s.recurso, RegexOptions.IgnoreCase))
                                               If o IsNot Nothing Then
                                                   Dim negocio = DB.getNegocio(SimpleRoleProvider.GetId(), strCnSab, strCnEpsilon)
                                                   If o.negocio <> negocio Then
                                                       Return False
                                                   End If
                                               End If
                                               Return True
                                           End Function)

            Return View()
        End Function
        <SimpleRoleProvider(EfaRole.usuarioTemporal)>
        Function Coger(ByVal nombre As String) As ActionResult
            If nombre.ToLower = "telefono" Then
                ViewData("recursos") = DB.GetListOfRecursoTelefonoLibre(nombre, DB.GetIdPlanta(SimpleRoleProvider.GetId(), strCnSab), strCnTelef)
                If ViewData("recursos").count = 0 Then
                    ViewData("quienlostiene") = DB.GetListOfRegistroCogidoTelefono(strCnTelef)
                End If
            Else
                ViewData("recursos") = DB.GetListOfRecursoNormalLibre(nombre, DB.GetIdPlanta(SimpleRoleProvider.GetId(), strCnSab), strCnEfa)
                If ViewData("recursos").count = 0 Then
                    ViewData("quienlostiene") = DB.GetListOfRegistroCogidoNormal(strCnEfa).Where(Function(r) r.nombreGrupo = nombre)
                End If
            End If
            Return View("list2")
        End Function
        <SimpleRoleProvider(EfaRole.usuarioTemporal)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Coger(ByVal r As Registro) As ActionResult
            r.Coger = CDate(Now.ToShortDateString)
            If r.NombreGrupo.ToLower = "telefono" Then
                DB.CogerRecursoTelefono(r, strCnTelef)
            Else
                DB.CogerRecursoNormal(r, strCnEfa)
            End If

            Return RedirectToAction("confirm")
        End Function
        <SimpleRoleProvider(EfaRole.usuarioTemporal)>
        Function Dejar() As ActionResult
            Dim l As List(Of Registro) = DB.GetListOfRegistroDeUsuarioNormal(SimpleRoleProvider.GetId(), strCnEfa)
            l.AddRange(DB.GetListOfRegistroDeUsuarioTelefono(SimpleRoleProvider.GetId(), strCnTelef))
            ViewData("registros") = l
            Return View("list2")
        End Function
        <SimpleRoleProvider(EfaRole.usuarioTemporal)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Dejar(ByVal r As Registro) As ActionResult
            r.Dejar = CDate(Now.ToShortDateString)
            If r.NombreGrupo.ToLower = "telefono" Then
                DB.DejarRecursoTelefono(r, strCnTelef)
            Else
                DB.DejarRecursoNormal(r, strCnEfa)
            End If
            Return RedirectToAction("confirm")
        End Function

        <SimpleRoleProvider(EfaRole.usuarioTemporal)>
        Function Seleccion(ByVal r As Recurso) As ActionResult
            Return View(r)
        End Function
        <SimpleRoleProvider(EfaRole.usuarioTemporal)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Seleccion(ByVal r As Registro) As ActionResult
            r.Planta = DB.GetIdPlanta(r.IdSab, strCnSab)
            r.Coger = CDate(Now.ToShortDateString)
            If r.NombreGrupo.ToLower = "telefono" Then
                DB.CogerRecursoTelefono(r, strCnTelef)
            Else
                DB.CogerRecursoNormal(r, strCnEfa)
            End If

            Return RedirectToAction("confirm")
        End Function
        <SimpleRoleProvider(EfaRole.usuarioTemporal)>
        Function Confirm() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(EfaRole.usuarioTemporal)>
        Function LogOut() As ActionResult
            Session.Abandon()
            FormsAuthentication.SignOut()
            Response.Cookies.Add(New HttpCookie("cultura", "es-ES"))
            Return RedirectToAction("index", "access", Nothing)
        End Function
        <SimpleRoleProvider(EfaRole.usuarioTemporal, EfaRole.admin)>
        Function ImageGrupo(ByVal nombreGrupo As String) As ActionResult
            If nombreGrupo.ToLower = "telefono" Then
                Return File(IO.File.ReadAllBytes(Request.MapPath(Url.Content("~/Content/movil.png"))), "img/png")
            End If
            Dim g = DB.GetGrupoNormal(nombreGrupo, strCnEfa)
            If g.Image Is Nothing Then
                Return File(IO.File.ReadAllBytes(Request.MapPath(Url.Content("~/Content/NoImage.png"))), "img/png")
            End If
            Return File(g.Image, "img/png")
        End Function
        <SimpleRoleProvider(EfaRole.usuarioTemporal)>
        Function cogertoner() As ActionResult
            Return View("tonerimpresoraocolor")
        End Function
        <SimpleRoleProvider(EfaRole.usuarioTemporal)>
        Function cogertonerimpresora() As ActionResult
            Return View("tonerimpresora", DB.GetListOfTonerImpresoraConStock(strCnEfa))
        End Function
        <SimpleRoleProvider(EfaRole.usuarioTemporal)>
        Function cogertonercolor(idImpresora As Nullable(Of Integer)) As ActionResult
            If idImpresora.HasValue Then
                Return View("tonercolor", DB.GetListOfTonerColor(idImpresora.Value, strCnEfa).Where(Function(c) c.idimpresora = idImpresora).ToList)
            Else
                Return View("tonercolor", DB.GetListOfTonerColor(strCnEfa))
            End If
        End Function
        <SimpleRoleProvider(EfaRole.usuarioTemporal)>
        Function cogertonercolorconfirmar(idImpresora As Integer, idcolor As String)
            DB.updateTonerColorDecrementarStock(idImpresora, idcolor, SimpleRoleProvider.GetId(), strCnEfa)
            Dim tonerColor = DB.GetListOfTonerColor(idImpresora, strCnEfa).Find(Function(c) c.idcolor = idcolor)
            If tonerColor.stock <= tonerColor.stockminimo Then
                Dim impresora = DB.GetListOfTonerImpresora(strCnEfa).Find(Function(i) i.id = idImpresora)
                SendEmail("helpdesk@batz.es", "Notificacion de stock para " + tonerColor.idcolor, "El color " + tonerColor.color + " de la impresora " + impresora.nombre + " esta bajo de stock.")
            End If
            Return RedirectToAction("confirm")
        End Function
        Protected Shared Sub SendEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String)
            Dim b = New Net.Mail.MailMessage()
            b.Subject = subject
            b.Body = body
            b.From = New Net.Mail.MailAddress("recursosinformaticos@batz.es")
            b.To.Add(recipients)
            b.IsBodyHtml = True
            Dim smtp = New Net.Mail.SmtpClient("posta.batz.es")
            smtp.Send(b)
        End Sub
    End Class

End Namespace