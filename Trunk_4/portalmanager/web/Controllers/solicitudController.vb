Public Class solicitudController
    Inherits System.Web.Mvc.Controller

    Private strCnMicrosof As String = ConfigurationManager.ConnectionStrings("microsoft").ConnectionString
    Private strCnOracle As String = ConfigurationManager.ConnectionStrings("oracle").ConnectionString

    <SimpleRoleProvider(Role.responsable, Role.rrhh)> _
    Function Index() As ActionResult
        Dim abiertas = String.IsNullOrEmpty(Request("cerradas"))
        ViewData("coberturaPuesto") = db.GetSolicitudesCoberturaPuesto(abiertas, strCnOracle, strCnMicrosof).Where(Function(cp)
                                                                                                                       Return cp.responsable = SimpleRoleProvider.GetId() OrElse
                                                                                                                          SimpleRoleProvider.IsUserAuthorised(Role.rrhh) OrElse
                                                                                                                       cp.listOfValidacion.Exists(Function(v) v.idSab = SimpleRoleProvider.GetId())
                                                                                                                   End Function).OrderByDescending(Function(o) o.ultimaValidacion).ToList
        ViewData("becaria") = db.GetSolicitudesBecaria(abiertas, strCnOracle, strCnMicrosof).Where(Function(b)
                                                                                                       Return b.responsable = SimpleRoleProvider.GetId() OrElse SimpleRoleProvider.IsUserAuthorised(Role.rrhh) OrElse b.listOfValidacion.Exists(Function(v) v.idSab = SimpleRoleProvider.GetId())
                                                                                                   End Function).OrderByDescending(Function(o) o.ultimaValidacion).ToList
        Return View()
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)> _
    Function CreateBecaria() As ActionResult
        Dim u = db.GetUsuarioSab(SimpleRoleProvider.GetId(), strCnOracle)
        bindNegocioDepartamento(SimpleRoleProvider.GetId())
        Dim b As New becaria With {.responsable = u.id, .nombreResponsable = u.nombre + " " + u.apellido1 + " " + u.apellido2}
        ViewData("horario") = ConfigurationManager.AppSettings("horario").Split({";"c}, StringSplitOptions.RemoveEmptyEntries).ToList().Select(Function(s) New Mvc.SelectListItem() With {.Value = s, .Text = s})
        ViewData("fecha") = ""
        Return View("editcreatebecaria", b)
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)> _
    Function CreateEstructural() As ActionResult
        Dim u = db.GetUsuarioSab(SimpleRoleProvider.GetId(), strCnOracle)
        bindNegocioDepartamento(SimpleRoleProvider.GetId())
        Dim e As New coberturaPuesto With {.responsable = u.id, .nombreResponsable = u.nombre + " " + u.apellido1 + " " + u.apellido2}
        ViewData("horario") = ConfigurationManager.AppSettings("horario").Split({";"c}, StringSplitOptions.RemoveEmptyEntries).ToList().Select(Function(s) New Mvc.SelectListItem() With {.Value = s, .Text = s})
        ViewData("fecha") = ""
        Return View("editcreateestructural", e)
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function EditEstructural(idsolicitud As Integer, abiertas As Boolean) As ActionResult
        Dim u = db.GetUsuarioSab(SimpleRoleProvider.GetId(), strCnOracle)
        bindNegocioDepartamento(SimpleRoleProvider.GetId())
        Dim cp = db.GetSolicitudesCoberturaPuesto(abiertas, strCnOracle, strCnMicrosof).Find(Function(sc) sc.id = idsolicitud)
        ViewData("horario") = ConfigurationManager.AppSettings("horario").Split({";"c}, StringSplitOptions.RemoveEmptyEntries).ToList().Select(Function(s) New Mvc.SelectListItem() With {.Value = s, .Text = s, .Selected = cp.Horario = s})
        Return View("editcreateestructural", cp)
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)> _
    <AcceptVerbs(HttpVerbs.Post)> _
    Function EditEstructural(idsolicitud As Integer, cp As coberturaPuesto) As ActionResult
        If ModelState.IsValid Then
            cp.id = idsolicitud
            db.updateCoberturaPuesto(cp, strCnOracle)
            Return RedirectToAction("index")
        End If
        Dim u = db.GetUsuarioSab(SimpleRoleProvider.GetId(), strCnOracle)
        bindNegocioDepartamento(SimpleRoleProvider.GetId())
        ViewData("horario") = ConfigurationManager.AppSettings("horario").Split({";"c}, StringSplitOptions.RemoveEmptyEntries).ToList().Select(Function(s) New Mvc.SelectListItem() With {.Value = s, .Text = s, .Selected = cp.horario = s})
        Return View("editcreateestructural", cp)
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)> _
    Function EditBecaria(idsolicitud As Integer) As ActionResult
        Dim u = db.GetUsuarioSab(SimpleRoleProvider.GetId(), strCnOracle)
        bindNegocioDepartamento(SimpleRoleProvider.GetId())
        Dim b = db.GetSolicitudesBecaria(True, strCnOracle, strCnMicrosof).Find(Function(sc) sc.id = idsolicitud)
        ViewData("horario") = ConfigurationManager.AppSettings("horario").Split({";"c}, StringSplitOptions.RemoveEmptyEntries).ToList().Select(Function(s) New Mvc.SelectListItem() With {.Value = s, .Text = s, .Selected = b.horario = s})
        Return View("editcreatebecaria", b)
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)> _
    <AcceptVerbs(HttpVerbs.Post)> _
    Function EditBecaria(idsolicitud As Integer, b As becaria) As ActionResult
        If ModelState.IsValid Then
            b.id = idsolicitud
            db.updateBecaria(b, strCnOracle)
            Return RedirectToAction("index")
        End If
        Dim u = db.GetUsuarioSab(SimpleRoleProvider.GetId(), strCnOracle)
        bindNegocioDepartamento(SimpleRoleProvider.GetId())
        ViewData("horario") = ConfigurationManager.AppSettings("horario").Split({";"c}, StringSplitOptions.RemoveEmptyEntries).ToList().Select(Function(s) New Mvc.SelectListItem() With {.Value = s, .Text = s, .Selected = b.horario = s})
        Return View("editcreatebecaria", b)
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)> _
    <AcceptVerbs(HttpVerbs.Post)> _
    Function CreateBecaria(o As becaria) As ActionResult
        If ModelState.IsValid Then
            Dim idvalidador1 = db.GetResponsable(o.responsable, strCnOracle, strCnMicrosof)
            Dim UsuarioSabCreador = db.GetUsuarioSab(o.responsable, strCnOracle)
            o.listOfValidacion = New List(Of Validacion)
            Dim ordenValidacion = 1
            o.listOfValidacion.Add(New Validacion With {.idSab = idvalidador1, .orden = ordenValidacion})
            o.creador = SimpleRoleProvider.GetId()
            Dim idValidador2 = db.GetResponsable(idvalidador1, strCnOracle, strCnMicrosof)
            While idValidador2 <> idvalidador1 And idValidador2
                ordenValidacion = ordenValidacion + 1
                o.listOfValidacion.Add(New Validacion With {.idSab = idValidador2, .orden = ordenValidacion})
                If idValidador2 = ConfigurationManager.AppSettings("idSabGerencia") Then
                    Exit While
                End If
                idvalidador1 = idValidador2
                idValidador2 = db.GetResponsable(idValidador2, strCnOracle, strCnMicrosof)
            End While
            db.SetBecaria(o, strCnOracle)
            Dim email = GetSiguienteResponsable(o, o.responsable)
            EmailSolicitudPersonal(o.id, UsuarioSabCreador.nombre, UsuarioSabCreador.apellido1, UsuarioSabCreador.apellido2, email)
            Return RedirectToAction("index")
        End If
        Dim u = db.GetUsuarioSab(o.responsable, strCnOracle)
        o.nombreResponsable = u.nombre + " " + u.apellido1 + " " + u.apellido2
        bindNegocioDepartamento(SimpleRoleProvider.GetId(), o.negocio, o.departamento)
        ViewData("horario") = ConfigurationManager.AppSettings("horario").Split({";"c}, StringSplitOptions.RemoveEmptyEntries).ToList().Select(Function(s) New Mvc.SelectListItem() With {.Value = s, .Text = s})
        Return View("editcreatebecaria", o)
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)> _
    <AcceptVerbs(HttpVerbs.Post)> _
    Function CreateEstructural(o As coberturaPuesto) As ActionResult
        If ModelState.IsValid Then
            Dim idvalidador1 = db.GetResponsable(o.responsable, strCnOracle, strCnMicrosof)
            Dim UsuarioSabCreador = db.GetUsuarioSab(o.responsable, strCnOracle)
            Dim ordenValidacion = 1
            o.listOfValidacion = New List(Of Validacion)
            o.listOfValidacion.Add(New Validacion With {.idSab = idvalidador1, .orden = ordenValidacion})
            Dim idValidador2 = Nothing
            If idvalidador1 <> ConfigurationManager.AppSettings("idSabGerencia") Then
                idValidador2 = db.GetResponsable(idvalidador1, strCnOracle, strCnMicrosof)
            End If
            While idValidador2 IsNot Nothing AndAlso idValidador2 <> idvalidador1
                ordenValidacion = ordenValidacion + 1
                o.ListOfValidacion.Add(New Validacion With {.idSab = idValidador2, .orden = ordenValidacion})
                If idValidador2 = ConfigurationManager.AppSettings("idSabGerencia") Then
                    Exit While
                End If
                idvalidador1 = idValidador2
                idValidador2 = db.GetResponsable(idValidador2, strCnOracle, strCnMicrosof)
            End While
            o.creador = SimpleRoleProvider.GetId()
                db.SetCoberturaPuesto(o, strCnOracle)
                Dim email = GetSiguienteResponsable(o, o.responsable)
                EmailSolicitudPersonal(o.id, UsuarioSabCreador.nombre, UsuarioSabCreador.apellido1, UsuarioSabCreador.apellido2, email)
                Return RedirectToAction("index")
            End If
            Dim u = db.GetUsuarioSab(o.responsable, strCnOracle)
        o.nombreResponsable = u.nombre + " " + u.apellido1 + " " + u.apellido2
        bindNegocioDepartamento(SimpleRoleProvider.GetId(), o.negocio, o.departamento)
        ViewData("horario") = ConfigurationManager.AppSettings("horario").Split({";"c}, StringSplitOptions.RemoveEmptyEntries).ToList().Select(Function(s) New Mvc.SelectListItem() With {.Value = s, .Text = s})
        Return View("editcreateestructural", o)
    End Function
    Function DeleteEstructural(idSolicitud As Integer)
        Dim abiertas = String.IsNullOrEmpty(Request("cerradas"))
        Dim cp = db.GetSolicitudesCoberturaPuesto(abiertas, strCnOracle, strCnMicrosof).Find(Function(sc) sc.id = idSolicitud)
        Return View("deletesolicitud", cp)
    End Function
    Function DeleteBecaria(idSolicitud As Integer)
        Dim abiertas = String.IsNullOrEmpty(Request("cerradas"))
        Dim b = db.GetSolicitudesBecaria(abiertas, strCnOracle, strCnMicrosof).Find(Function(sc) sc.id = idSolicitud)
        Return View("deletesolicitud", b)
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)> _
    <AcceptVerbs(HttpVerbs.Post)> _
    Function DeleteEstructural(idSolicitud As Integer, confirm As String)
        If ModelState.IsValid Then
            db.deleteCoberturaPuesto(idSolicitud, strCnOracle)
        End If
        Return RedirectToAction("index")
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    <AcceptVerbs(HttpVerbs.Post)>
    Function DeleteBecaria(idSolicitud As Integer, confirm As String)
        If ModelState.IsValid Then
            db.deletebecaria(idSolicitud, strCnOracle)
        End If
        Return RedirectToAction("index")
    End Function
    <SimpleRoleProvider(Role.rrhh)>
    Function Cerrarsolicitud(idSolicitud As Integer)
        Return View()
    End Function
    <SimpleRoleProvider(Role.rrhh)>
    Function Editincorporacion(idSolicitud As Integer)
        Dim datos_incorporacion = db.GetDatosIncorporacion(idSolicitud, strCnOracle)
        ViewData("datos_incorporacion") = datos_incorporacion
        Return View()
    End Function
    <SimpleRoleProvider(Role.rrhh)>
    <AcceptVerbs(HttpVerbs.Post)>
    Function Cerrarsolicitud(idSolicitud As Integer, datosIncorporacion As String)
        db.CloseSolicitud(idSolicitud, datosIncorporacion, SimpleRoleProvider.GetId(), strCnOracle)
        Return RedirectToAction("index")
    End Function
    <SimpleRoleProvider(Role.rrhh)>
    <AcceptVerbs(HttpVerbs.Post)>
    Function Editincorporacion(idSolicitud As Integer, datos_incorporacion As String)
        db.EditIncorporacion(idSolicitud, datos_incorporacion, strCnOracle)
        Return RedirectToAction("index")
    End Function
    Function detailEstructural(idSolicitud As Integer) As ActionResult
        Dim abiertas = String.IsNullOrEmpty(Request("cerradas"))
        Return View(db.GetSolicitudesCoberturaPuesto(abiertas, strCnOracle, strCnMicrosof).Find(Function(e) e.id = idSolicitud))
    End Function
    Function detailbecaria(idSolicitud As Integer) As ActionResult
        Dim abiertas = String.IsNullOrEmpty(Request("cerradas"))
        Return View(db.GetSolicitudesBecaria(abiertas, strCnOracle, strCnMicrosof).Find(Function(e) e.id = idSolicitud))
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)> _
    Function validacionesEstructural(idsolicitud As Integer) As ActionResult
        Dim abiertas = String.IsNullOrEmpty(Request("cerradas"))
        ViewData("solicitud") = db.GetSolicitudesCoberturaPuesto(abiertas, strCnOracle, strCnMicrosof).Find(Function(c) c.id = idsolicitud)
        ViewData("validaciones") = db.GetListOfValidaciones(idsolicitud, strCnOracle)
        Return View("validaciones")
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)> _
    Function validacionesBecaria(idsolicitud As Integer) As ActionResult
        Dim abiertas = String.IsNullOrEmpty(Request("cerradas"))
        ViewData("solicitud") = db.GetSolicitudesBecaria(abiertas, strCnOracle, strCnMicrosof).Find(Function(c) c.id = idsolicitud)
        ViewData("validaciones") = db.GetListOfValidaciones(idsolicitud, strCnOracle)
        Return View("validaciones")
    End Function
    <AcceptVerbs(HttpVerbs.Post)> _
    <SimpleRoleProvider(Role.responsable, Role.rrhh)> _
    Function validacionesEstructural(idsolicitud As Integer, idsabvalidador As Integer, validar As String, rechazar As String) As ActionResult
        Dim s = db.GetSolicitudesCoberturaPuesto(True, strCnOracle, strCnMicrosof).Find(Function(o) o.id = idsolicitud)
        If validar IsNot Nothing Then
            db.setValidacion(idsolicitud, True, idsabvalidador, strCnOracle)
            Dim email = GetSiguienteResponsable(s, idsabvalidador)
            EmailSolicitudPersonal(s.id, s.nombreResponsable, s.apellido1Responsable, s.apellifdo2Responsable, email)
        Else
            db.setValidacion(idsolicitud, False, idsabvalidador, strCnOracle)
            Dim email = GetAnteriorResponsable(s, idsabvalidador)
            Dim u = db.GetUsuarioSab(idsabvalidador, strCnOracle)
            EmailRechazoSolicitudPersonal(s.id, u.nombre, u.apellido1, u.apellido2, email)
        End If
        Return RedirectToAction("validacionesEstructural", New With {.idsolicitud = idsolicitud})
    End Function
    <AcceptVerbs(HttpVerbs.Post)> _
    <SimpleRoleProvider(Role.responsable, Role.rrhh)> _
    Function validacionesBecaria(idsolicitud As Integer, idsabvalidador As Integer, validar As String, rechazar As String) As ActionResult
        Dim s = db.GetSolicitudesBecaria(True, strCnOracle, strCnMicrosof).Find(Function(o) o.id = idsolicitud)
        If validar IsNot Nothing Then
            db.setValidacion(idsolicitud, True, idsabvalidador, strCnOracle)
            Dim email = GetSiguienteResponsable(db.GetSolicitudesBecaria(True, strCnOracle, strCnMicrosof).Find(Function(o) o.id = idsolicitud), idsabvalidador)
            EmailSolicitudPersonal(s.id, s.nombreResponsable, s.apellido1Responsable, s.apellifdo2Responsable, email)
        Else
            db.setValidacion(idsolicitud, False, idsabvalidador, strCnOracle)
            Dim email = GetAnteriorResponsable(db.GetSolicitudesBecaria(True, strCnOracle, strCnMicrosof).Find(Function(o) o.id = idsolicitud), idsabvalidador)
            Dim u = db.GetUsuarioSab(idsabvalidador, strCnOracle)
            EmailRechazoSolicitudPersonal(s.id, u.nombre, u.apellido1, u.apellido2, email)
        End If
        Return RedirectToAction("validacionesBecaria", New With {.idsolicitud = idsolicitud})
    End Function

    Private Function GetSiguienteResponsable(s As solicitud, idSabAccion As Integer) As String
        Dim lst = db.GetListOfValidaciones(s.id, strCnOracle)
        If s.responsable = idSabAccion Then
            'Notificar primer responsable
            Return db.GetUsuarioSab(lst.First().idSab, strCnOracle).email
        Else
            'Notificar siguiente responsable
            Dim index = lst.FindIndex(Function(o) o.idSab = idSabAccion)
            If index = lst.Count - 1 Then
                'No hay mas responsables
                Return ConfigurationManager.AppSettings("notificar")
            Else
                Return db.GetUsuarioSab(lst(index + 1).idSab, strCnOracle).email
            End If
        End If

    End Function
    Private Function GetAnteriorResponsable(s As solicitud, idSabAccion As Integer) As String
        Dim lst = db.GetListOfValidaciones(s.id, strCnOracle)
        If s.responsable = idSabAccion Then
            'Es el inicio de la cadena
            Return ""
        Else
            'Notificar responsable anterior
            Dim index = lst.FindIndex(Function(o) o.idSab = idSabAccion)
            Dim strbEmails As New Text.StringBuilder(db.GetUsuarioSab(s.responsable, strCnOracle).email.ToString) 'Solicitador
            While index > 0
                strbEmails.Append(",")
                strbEmails.Append(db.GetUsuarioSab(lst(index - 1).idSab, strCnOracle).email)
                index = index - 1
            End While
            Return strbEmails.ToString

        End If

    End Function

    Protected Shared Sub EmailSolicitudPersonal(idSolicitud As Integer, nombreSolicitador As String, apellido1Solicitador As String, apellido2Solicitador As String, emailsNotificar As String)
        Dim subject As New StringBuilder("Solicitud de personal Nº")
        subject.Append(" ") : subject.Append(idSolicitud)
        Dim body As New StringBuilder()
        If emailsNotificar = ConfigurationManager.AppSettings("notificar") Then
            'Email para RRHH
            Dim text = "<strong>{0} {1} {2}</strong> ha realizado la solicitud de personal Nº {3}.<br /> No hay mas validaciones pendientes por lo que se puede tramitar siguiendo <a href='https://intranet2.batz.es/portalmanager/access/index?controller=solicitud'>este vínculo</a>"
            body.Append(db.WrapTextOnHtmlBase(String.Format(text, nombreSolicitador, apellido1Solicitador, apellido2Solicitador, idSolicitud.ToString)))
        Else
            Dim text = "<strong>{0} {1} {2}</strong> ha realizado la solicitud de personal Nº {3}.<br />Para dar poder dar curso a dicha solicitud desde RRHH, necesitamos que des tu consentimiento siguiendo <a href='https://intranet2.batz.es/langileentxokoa/?url=http://intranet2.batz.es/portalmanager/access/index?controller=solicitud'>este vínculo</a>"
            body.Append(db.WrapTextOnHtmlBase(String.Format(text, nombreSolicitador, apellido1Solicitador, apellido2Solicitador, idSolicitud.ToString)))
        End If
        SendEmail(emailsNotificar, subject.ToString, body.ToString)
    End Sub
    Protected Shared Sub EmailRechazoSolicitudPersonal(idSolicitud As Integer, nombreSolicitador As String, apellido1Solicitador As String, apellido2Solicitador As String, emailsNotificar As String)
        Dim subject As New StringBuilder("Solicitud de personal Nº")
        subject.Append(" ") : subject.Append(idSolicitud) : subject.Append(" RECHAZADA")

        Dim body As New StringBuilder()
        Dim text = "<strong>{0} {1} {2}</strong> ha rechazado la solicitud de personal Nº {3}.<br /> Puede ver mas datos acerca de dicha solicitud siguiendo<a href='https://intranet2.batz.es/langileentxokoa/?url=http://intranet2.batz.es/portalmanager/access/index?controller=solicitud'>este vínculo</a>"
        body.Append(db.WrapTextOnHtmlBase(String.Format(text, nombreSolicitador, apellido1Solicitador, apellido2Solicitador, idSolicitud.ToString)))
        SendEmail(emailsNotificar, subject.ToString, body.ToString)
    End Sub

    Protected Shared Sub SendEmail(ByVal recipients As String, ByVal subject As String, ByVal body As String)
        Dim b = New Net.Mail.MailMessage()
        b.Subject = subject
        b.Body = body
        b.From = New Net.Mail.MailAddress("portalmanager@batz.es")
        b.To.Add(recipients)
        b.IsBodyHtml = True
        Dim smtp = New Net.Mail.SmtpClient("posta.batz.com")
        smtp.Send(b)
    End Sub
    Private Sub bindNegocioDepartamento(idSab As Integer)
        Dim idDepartamento = db.GetUsuarioSab(idSab, strCnOracle).idDepartamento
        Dim idNegocio = db.GetNegocioDesdeDepartamento(idDepartamento, strCnMicrosof).id
        bindNegocioDepartamento(idSab, idNegocio, idDepartamento)
    End Sub
    Private Sub bindNegocioDepartamento(idSab As Integer, idNegocio As String, idDepartamento As String)
        ViewData("negocio") = db.GetListOfNegocio(strCnMicrosof).Select(Of Mvc.SelectListItem)(Function(o) New Mvc.SelectListItem With {.Value = o.id, .Text = o.name, .Selected = o.id = idNegocio})
        ViewData("departamento") = db.GetListOfDepartamento(idNegocio, strCnMicrosof).Select(Of Mvc.SelectListItem)(Function(o) New Mvc.SelectListItem With {.Value = o.id, .Text = o.name, .Selected = o.id = idDepartamento})
    End Sub



    Function emailTest(idSolicitud As Integer) As ActionResult
        Dim s = db.GetSolicitudesBecaria(True, strCnOracle, strCnMicrosof).Find(Function(o) o.id = idSolicitud)
        Dim email = GetSiguienteResponsable(db.GetSolicitudesBecaria(True, strCnOracle, strCnMicrosof).Find(Function(o) o.id = idSolicitud), 57164)
        EmailSolicitudPersonal(s.id, s.nombreResponsable, s.apellido1Responsable, s.apellifdo2Responsable, email)
        Return RedirectToAction("Index")
    End Function
End Class