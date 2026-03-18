Public Class EvaluacionController
    Inherits System.Web.Mvc.Controller
    Private ReadOnly strCn As String = ConfigurationManager.ConnectionStrings("oracle").ConnectionString
    Private ReadOnly strCnMicrosoft As String = ConfigurationManager.ConnectionStrings("microsoft").ConnectionString

    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function Index() As ActionResult
        ViewData("lstFormularioEvaluacionesEpsilon") = New Script.Serialization.JavaScriptSerializer().Deserialize(Of List(Of FormularioVencimientos))(ConfigurationManager.AppSettings("formulario_vencimientos"))
        ViewData("tiposformulario") = db.GetListOfTiposFormulario(strCn)
        Return View("listadocolaboradores", GetColaboradoresYPreguntas(SimpleRoleProvider.GetId()))
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function AdministrarFormularioList(idFormulario As Integer) As ActionResult
        Return View("listPreguntas", db.GetListOfPregunta(idFormulario, strCn))
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function AddPregunta() As ActionResult
        ViewData("tipopregunta") = [Enum].GetValues(GetType(TipoPregunta)).Cast(Of TipoPregunta)().Select(Function(tp) New Mvc.SelectListItem With {.Value = tp, .Text = [Enum].GetName(GetType(TipoPregunta), tp)})
        Return View("editPregunta")
    End Function
    <AcceptVerbs(HttpVerbs.Post)>
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function AddPregunta(p As Pregunta) As ActionResult
        If ModelState.IsValid Then
            db.SetPregunta(p, strCn)
            Return RedirectToAction("AdministrarFormularioList", H.ToRouteValues(Request.QueryString, Nothing))
        End If
        ViewData("tipopregunta") = [Enum].GetValues(GetType(TipoPregunta)).Cast(Of TipoPregunta)().Select(Function(tp) New Mvc.SelectListItem With {.Value = tp, .Text = [Enum].GetName(GetType(TipoPregunta), tp), .Selected = p.tipoPregunta = tp})
        Return View("editPregunta")
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function EditPregunta(idFormulario As Integer, idPregunta As Integer) As ActionResult
        Dim pregunta = db.GetListOfPregunta(idFormulario, strCn).Find(Function(p) idPregunta = p.id)
        ViewData("tipopregunta") = [Enum].GetValues(GetType(TipoPregunta)).Cast(Of TipoPregunta)().Select(Function(tp) New Mvc.SelectListItem With {.Value = tp, .Text = [Enum].GetName(GetType(TipoPregunta), tp), .Selected = pregunta.tipoPregunta = tp})
        Return View("editPregunta", pregunta)
    End Function
    <AcceptVerbs(HttpVerbs.Post)>
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function EditPregunta(idPregunta As Integer, p As Pregunta) As ActionResult
        If ModelState.IsValid Then
            p.id = idPregunta
            db.updatePregunta(p, strCn)
            Return RedirectToAction("AdministrarFormularioList", H.ToRouteValues(Request.QueryString, Nothing))
        End If
        ViewData("tipopregunta") = [Enum].GetValues(GetType(TipoPregunta)).Cast(Of TipoPregunta)().Select(Function(tp) New Mvc.SelectListItem With {.Value = tp, .Text = [Enum].GetName(GetType(TipoPregunta), tp), .Selected = p.tipoPregunta = tp})
        Return View("editPregunta")
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function Addrespuestaposible(idFormulario As Integer, idPregunta As Integer) As ActionResult
        Dim pregunta = db.GetListOfPregunta(idFormulario, strCn).Find(Function(p) idPregunta = p.id)
        Return View("editrespuestaposible")
    End Function
    <AcceptVerbs(HttpVerbs.Post)>
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function Addrespuestaposible(idPregunta As Integer, r As RespuestaPosible) As ActionResult
        If ModelState.IsValid Then
            db.SetRespuestaPosible(idPregunta, r, strCn)
            Return RedirectToAction("AdministrarFormularioList", H.ToRouteValues(Request.QueryString, Nothing))
        End If
        Return View("editrespuestaposible", r)
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function Editrespuestaposible(idFormulario As Integer, idPregunta As Integer, idRespuesta As Integer) As ActionResult
        Return View("editrespuestaposible", db.GetListOfPregunta(idFormulario, strCn).Find(Function(p) p.id = idPregunta).respuestasPosibles.Find(Function(r) r.id = idRespuesta))
    End Function
    <AcceptVerbs(HttpVerbs.Post)>
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function Editrespuestaposible(idPregunta As Integer, idRespuesta As Integer, r As RespuestaPosible) As ActionResult
        If ModelState.IsValid Then
            r.id = idRespuesta
            db.UpdateRespuestaPosible(idPregunta, r, strCn)
            Return RedirectToAction("AdministrarFormularioList", H.ToRouteValues(Request.QueryString, Nothing))
        End If
        Return View("editrespuestaposible", r)
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function ListadoColaboradores() As ActionResult
        ViewData("lstFormularioEvaluacionesEpsilon") = New Script.Serialization.JavaScriptSerializer().Deserialize(Of List(Of FormularioVencimientos))(ConfigurationManager.AppSettings("formulario_vencimientos"))
        ViewData("tiposformulario") = db.GetListOfTiposFormulario(strCn)
        Return View("listadocolaboradores", GetColaboradoresYPreguntas(SimpleRoleProvider.GetId()))
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function Editarnotificados() As ActionResult
        ViewData("lstFormularioEvaluacionesEpsilon") = New Script.Serialization.JavaScriptSerializer().Deserialize(Of List(Of FormularioVencimientos))(ConfigurationManager.AppSettings("formulario_vencimientos"))
        ViewData("tiposformulario") = db.GetListOfTiposFormulario(strCn)
        Return View("editarnotificados", GetColaboradoresYPreguntasCerradas(SimpleRoleProvider.GetId()))
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function RellenarFormulario(idFormulario As Integer, idSabColaborador As Integer, idTrabajador As Integer, ticksVencimiento As Long) As ActionResult
        'Function RellenarFormulario(idFormulario As Integer, idSabColaborador As Integer, ticksVencimiento As Long) As ActionResult
        'ViewData("listOfRespuesta") = db.GetListOfUltimasRespuestas(idSabColaborador, idFormulario, strCn) 'New List(Of Respuesta)
        'ViewData("listOfRespuesta").exists(Function(z As Respuesta) z.idRespuesta = r.id)

        Dim fechaVencimiento = New DateTime(ticksVencimiento, DateTimeKind.Utc)
        Dim tipoEvaluacion = db.GetTipoEvaluacion(idTrabajador, fechaVencimiento, strCnMicrosoft, strCn)?.Split(";")
        Dim fechaFinContrato = db.GetFechaFinContrato(idTrabajador, strCnMicrosoft)
        Dim nombreEvaluacion = If(tipoEvaluacion IsNot Nothing AndAlso tipoEvaluacion.Any(), tipoEvaluacion(0), "")
        Dim descEvaluacion = If(tipoEvaluacion IsNot Nothing AndAlso tipoEvaluacion.Count >= 2, tipoEvaluacion(1), "")

        'Dim extrainfo = New With {fechaVencimiento, idSabColaborador}
        Dim extrainfo = New With {fechaVencimiento, fechaFinContrato, nombreEvaluacion, descEvaluacion}
        ViewData("extrainfo") = extrainfo
        Return View("formulario", db.GetListOfPregunta(idFormulario, strCn).GroupBy(Function(p) p.tipoPregunta))
    End Function
    <AcceptVerbs(HttpVerbs.Post)>
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function RellenarFormulario(idformulario As Integer, idSabColaborador As Integer, ticksVencimiento As Long, respuestas As List(Of String)) As ActionResult
        Dim fechaVencimiento = New DateTime(ticksVencimiento)
        Dim fecha = Now
        Dim lstResp As New List(Of Respuesta)
        'Agrupar por pregunta
        Dim lstGroup = Request.Form.AllKeys.Where(Function(r) r.Contains("|")).Select(Function(r)
                                                                                          Dim s = r.Split("|")

                                                                                          Return New With {.id = s(0), .key = s(1)}

                                                                                      End Function).GroupBy(Function(g) g.id)
        For Each g In lstGroup 'Agrupadas las respuestas por pregunta
            Dim r As New Respuesta With {
                .fecha = fecha,
            .idSab = idSabColaborador,
            .fechaVencimiento = fechaVencimiento
            }
            Dim pregunta = db.GetListOfPregunta(idformulario, strCn).Find(Function(pre) pre.id = g.Key)

            r.tituloPregunta = pregunta.titulo
            r.descripcionPregunta = pregunta.descripcion
            r.tipopregunta = pregunta.tipoPregunta
            If pregunta.tipoPregunta = TipoPregunta.libre Then
                r.texto = Request(g.Key + "|texto")
                If r.texto.Length > 1000 Then
                    ModelState.AddModelError(g.Key + "|texto", H.Traducir("El texto no puede sobrepasar los 1000 caracteres"))
                End If
            Else 'Respuesta de puntuacion
                r.pesoPregunta = pregunta.peso
                r.puntuacionMax = pregunta.respuestasPosibles.Max(Function(rp) rp.puntuacion)
                If Not IsNumeric(Request(g.Key + "|puntos")) OrElse Request(g.Key + "|puntos") > r.puntuacionMax OrElse Request(g.Key + "|puntos") < 0 Then
                    ModelState.AddModelError(g.Key + "|puntos", H.Traducir("Es necesario responder con una puntuacion entre los limites marcados"))
                Else
                    'Dim respPosible = pregunta.respuestasPosibles.Find(Function(rp) rp.id = Request(g.Key + "|puntos"))
                    r.puntuacion = Request(g.Key + "|puntos") 'respPosible.puntuacion
                    r.texto = "" 'respPosible.descripcion
                End If
            End If
            lstResp.Add(r)
        Next

        If ModelState.IsValid Then
            db.SetListOfRespuesta(idformulario, SimpleRoleProvider.GetId(), lstResp, strCn)
            'Caso excepcional solicitado por RRHH
            If SimpleRoleProvider.GetId() = 57932 Then
                'SendEmail("andonialonso@batz.es", "Notificación del portal del manager", db.WrapTextOnHtmlBase(String.Format(Text, Responsable.nombre, Responsable.apellido1, Responsable.apellido2, Colaborador.nombre, Colaborador.apellido1, Colaborador.apellido2)))
            End If
            Return RedirectToAction("listadoColaboradores")
        End If
        ViewData("listOfRespuesta") = New List(Of Respuesta)

        For Each g In lstGroup 'Agrupadas las respuestas por pregunta
            Dim pregunta = db.GetListOfPregunta(idformulario, strCn).Find(Function(pre) pre.id = g.Key)

            If pregunta.tipoPregunta = TipoPregunta.libre Then
                ViewData(g.Key + "|texto") = Request(g.Key + "|texto")
            Else 'Respuesta de puntuacion
                ViewData(g.Key + "|puntos") = Request(g.Key + "|puntos")
            End If
        Next

        Dim idTrabajador = db.GetIdTrabajadorFromIdSab(idSabColaborador, strCn)
        'Dim fechaVencimiento = New DateTime(ticksVencimiento, DateTimeKind.Utc)
        Dim tipoEvaluacion = db.GetTipoEvaluacion(idTrabajador, fechaVencimiento, strCnMicrosoft, strCn)?.Split(";")
        Dim fechaFinContrato = db.GetFechaFinContrato(idTrabajador, strCnMicrosoft)
        Dim nombreEvaluacion = If(tipoEvaluacion IsNot Nothing AndAlso tipoEvaluacion.Any(), tipoEvaluacion(0), "")
        Dim descEvaluacion = If(tipoEvaluacion IsNot Nothing AndAlso tipoEvaluacion.Count >= 2, tipoEvaluacion(1), "")
        Dim extrainfo = New With {fechaVencimiento, fechaFinContrato, nombreEvaluacion, descEvaluacion}
        ViewData("extrainfo") = extrainfo
        Return View("formulario", db.GetListOfPregunta(idformulario, strCn).GroupBy(Function(p) p.tipoPregunta))
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function VerFormulario(idFormulario As Integer, idSabColaborador As Integer, ticksVencimiento As Long) As ActionResult
        ViewData("listOfRespuesta") = New List(Of Respuesta)
        Dim fechaVencimiento = New DateTime(ticksVencimiento)
        ViewData("notificado") = db.GetNotificacionColaborador(idSabColaborador, fechaVencimiento, strCn)
        Return View("verformulario", db.GetListOfRespuesta(idFormulario, strCn).Where(Function(r) r.fechaVencimiento = fechaVencimiento And r.idSab = idSabColaborador).GroupBy(Function(p) p.tipopregunta).OrderBy(Function(g) g.Key))
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function DeleteFormulario(idSabColaborador As Integer, ticksVencimiento As Long) As ActionResult
        Return View("deleteformulario")
    End Function
    <AcceptVerbs(HttpVerbs.Post)>
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function DeleteFormulario(idSabColaborador As Integer, ticksVencimiento As Long, confirmation As String) As ActionResult
        Dim fechaVencimiento = New DateTime(ticksVencimiento)
        db.deleteFormulario(idSabColaborador, fechaVencimiento, strCn)
        Return RedirectToAction("listadoColaboradores")
    End Function

    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function Addpropuestacontinuidad(idSabColaborador As Integer, ticksVencimiento As Long) As ActionResult
        Dim fechaVencimiento = New DateTime(ticksVencimiento)
        Dim ultimaPropuesta = db.GetListOfUltimaPropuesta(idSabColaborador, strCn)
        If ultimaPropuesta IsNot Nothing Then
            If ultimaPropuesta.continua Then
                'ViewData("duracion") = ultimaPropuesta.duracion
                ViewData("indice") = ultimaPropuesta.indice
            Else
                ViewData("motivo") = ultimaPropuesta.motivo
            End If
        End If
        Return View("editpropuestacontinuidad")
    End Function
    <AcceptVerbs(HttpVerbs.Post)>
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function Addpropuestacontinuidad(idSabColaborador As Integer, ticksVencimiento As Long, propuesta As propuestaContinuidad) As ActionResult
        Dim fechaVencimiento = New DateTime(ticksVencimiento)
        propuesta.fechaVencimiento = fechaVencimiento
        propuesta.idSab = idSabColaborador
        If propuesta.continua AndAlso Not propuesta.indice.HasValue Then
            ModelState.AddModelError("indice", "Si la propuesta de continuidad es positiva, es necesario introducir indice")
        End If
        If Not propuesta.continua AndAlso propuesta.motivo Is Nothing Then
            ModelState.AddModelError("motivo", "Si la propuesta de continuidad es negativa, es necesaio introducir el motivo")
        End If
        If ModelState.IsValid Then
            db.SetPropuestaContinuidad(propuesta, strCn)
            Return RedirectToAction("listadoColaboradores")
        End If
        Return View("editpropuestacontinuidad")
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function Editpropuestacontinuidad(id As Integer) As ActionResult
        Dim propuesta = db.GetListOfPropuestaContinuidad(strCn).Find(Function(pc) pc.id = id)
        Return View("editpropuestacontinuidad", propuesta)
    End Function
    <AcceptVerbs(HttpVerbs.Post)>
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function Editpropuestacontinuidad(id As Integer, propuesta As propuestaContinuidad) As ActionResult
        If ModelState.IsValid Then
            propuesta.id = id
            If propuesta.continua Then
                propuesta.motivo = ""
            Else
                propuesta.indice = Nothing
                propuesta.duracion = ""
            End If
            db.UpdatePropuestaContinuidad(propuesta, strCn)
            Return RedirectToAction("listadoColaboradores")
        End If
        Return View("editpropuestacontinuidad", propuesta)
    End Function
    Function Verpropuestacontinuidad(id As Integer)
        Dim propuesta = db.GetListOfPropuestaContinuidad(strCn).Find(Function(pc) pc.id = id)
        Return View(propuesta)
    End Function
    Function Notificadocolaborador(idSabColaborador As Integer, ticksVencimiento As Long)
        Dim fechaVencimiento = New DateTime(ticksVencimiento)
        'Asegurarnos de que no se hace click dos veces
        If db.GetNotificacionColaborador(idSabColaborador, fechaVencimiento, strCn) IsNot Nothing Then
            Return RedirectToAction("index")
        End If
        db.SetNotificacionVencimiento(idSabColaborador, fechaVencimiento, Now, strCn)
        Dim Responsable = db.GetUsuarioSab(SimpleRoleProvider.GetId(), strCn)
        Dim Colaborador = db.GetUsuarioSab(idSabColaborador, strCn)

        Dim text = "<strong>{0} {1} {2}</strong> ha realizado las acciones y comunicado a <strong>{3}  {4} {5}</strong>. Para ver mas datos, puedes entrar en el <a href=""" + Request.Url.Scheme + "://" + Request.Url.Authority + "/portalmanager/evaluacion/editmultipleevaluacion" + """>portal del manager</a>"
        SendEmail(ConfigurationManager.AppSettings("notificar"), "Notificación del portal del manager", db.WrapTextOnHtmlBase(String.Format(text, Responsable.nombre, Responsable.apellido1, Responsable.apellido2, Colaborador.nombre, Colaborador.apellido1, Colaborador.apellido2)))
        If Not String.IsNullOrEmpty(Colaborador.email) Then
            'SendEmail(ConfigurationManager.AppSettings("notificar"), "Notificación del portal del manager", db.WrapTextOnHtmlBase(String.Format(text, Responsable.nombre, Responsable.apellido1, Responsable.apellido2, Colaborador.nombre, Colaborador.apellido1, Colaborador.apellido2)))
        End If
        Return RedirectToAction("index")
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function Editmultipleevaluacion() As ActionResult
        Return View(db.GetColaboradoresConUltimaEvaluacion(SimpleRoleProvider.GetId(), strCn).
                    GroupBy(Function(g) New With {Key g.idsab, Key g.fechaVencimiento}).
                    Select(Function(g)
                               Dim firstEl = g.First()
                               Dim lstEvaluacion = g.ToList()
                               Return New With {firstEl.nombre, firstEl.apellido1, firstEl.apellido2,
                                                .nota = Math.Round(lstEvaluacion.Sum(Function(p) CDec(p.puntuacion / p.puntuacionMax) * CDec(p.pesoPregunta) * 0.1), 3),
                                                 lstEvaluacion, firstEl.idFormulario, firstEl.continua, firstEl.indice}
                           End Function))
    End Function
    Function HistoricoEvaluaciones(TipoFormulario As Integer?) As ActionResult
        ViewData("TipoFormulario") = db.GetListOfTiposFormulario(strCn)
        Return View(db.GetHistoricoEvaluaciones(SimpleRoleProvider.GetId(), If(TipoFormulario, 1), strCn, strCnMicrosoft))
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function Viewrespuestatexto(idsab As Integer, ticksVencimiento As Long, fecha1 As String, fecha2 As String, fecha3 As String, tipoEv As String, descEv As String) As ActionResult
        Dim fechaVencimiento = New DateTime(ticksVencimiento)
        Dim extrainfo = New With {fecha1, fecha2, fecha3, tipoEv, descEv}
        ViewData("extrainfo") = extrainfo
        Return View(db.GetrespuestasTexto(idsab, fechaVencimiento, strCn))
    End Function

    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function editFormulario(idFormulario As Integer, idSabColaborador As Integer, ticksVencimiento As Long) As ActionResult
        Dim fechaVencimiento = New DateTime(ticksVencimiento)

        Return View("editformulario", db.GetListOfRespuesta(idFormulario, strCn).Where(Function(r) r.fechaVencimiento = fechaVencimiento And r.idSab = idSabColaborador).GroupBy(Function(p) p.tipopregunta).OrderBy(Function(g) g.Key))
    End Function
    <AcceptVerbs(HttpVerbs.Post)>
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function editFormulario(idFormulario As Integer, idSabColaborador As Integer, ticksVencimiento As Long, respuestas As List(Of String)) As ActionResult
        Dim fechaVencimiento = New DateTime(ticksVencimiento)
        Dim fecha = Now
        Dim lstResp As New List(Of Respuesta)
        'Agrupar por pregunta
        Dim lstRespuestas = Request.Form.AllKeys.Where(Function(r) r.Contains("|")).Select(Function(r)
                                                                                               Dim s = r.Split("|")
                                                                                               Return New With {.id = s(0), .key = s(1)}
                                                                                           End Function)
        For Each ri In lstRespuestas
            Dim r As New Respuesta
            r.fecha = fecha : r.idSab = idSabColaborador : r.fechaVencimiento = fechaVencimiento : r.id = ri.id
            If ri.key = "puntos" Then
                r.puntuacion = Request(ri.id + "|" + ri.key)
                r.tipopregunta = TipoPregunta.puntuacion
            Else
                r.tipopregunta = TipoPregunta.libre
                r.texto = Request(ri.id + "|" + ri.key)
                If r.texto.Length > 1000 Then
                    ModelState.AddModelError(ri.id + "|" + ri.key, H.Traducir("El texto no puede sobrepasar los 1000 caracteres"))
                End If
            End If
            lstResp.Add(r)
        Next

        If ModelState.IsValid Then
            db.UpdatePuntuacionesYTextoRespuesta(lstResp, strCn)
            Return RedirectToAction("listadoColaboradores", H.ToRouteValues(Request.QueryString, Nothing))
        End If
        Return View("editformulario", db.GetListOfRespuesta(idFormulario, strCn).Where(Function(r) r.fechaVencimiento = fechaVencimiento And r.idSab = idSabColaborador).GroupBy(Function(p) p.tipopregunta).OrderBy(Function(g) g.Key))
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function cambiarPuntuacion(idformulario As Integer, idSab As Integer, idRespuesta As Integer) As ActionResult
        ViewData("usuario") = db.GetUsuarioSab(idSab, strCn)
        ViewData("respuesta") = db.GetListOfRespuesta(idformulario, strCn).Find(Function(r) r.id = idRespuesta)
        Return View()
    End Function
    <AcceptVerbs(HttpVerbs.Post)>
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function cambiarPuntuacion(idformulario As Integer, idSab As Integer, idRespuesta As Integer, puntuacion As Nullable(Of Decimal)) As ActionResult
        Dim respuesta = db.GetListOfRespuesta(idformulario, strCn).Find(Function(r) r.id = idRespuesta)
        If Not puntuacion.HasValue OrElse puntuacion.Value > respuesta.puntuacionMax OrElse puntuacion.Value < 0 Then
            ModelState.AddModelError("puntuacion", "Es Necesario introducir la nueva puntuacion entre los valores 0 y " + respuesta.puntuacionMax.ToString)
        End If
        If ModelState.IsValid Then
            db.UpdatePuntuacionRespuesta(idformulario, idRespuesta, puntuacion.Value, strCn)
        End If
        Return RedirectToAction("editmultipleevaluacion")
    End Function
    '<SimpleRoleProvider(Role.responsable, Role.rrhh)>
    'Function HistoricoEvaluaciones() As ActionResult
    '    Return View(db.GetColaboradores(SimpleRoleProvider.GetId(), strCn, strCnMicrosof).Where(Function(c) c.notificacionColaborador IsNot Nothing))
    'End Function

    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    Function addconsolidacioncambiopuesto(idSabColaborador As Integer, ticksVencimiento As Long) As ActionResult
        Return View("editconsolidacionpuesto")
    End Function
    <SimpleRoleProvider(Role.responsable, Role.rrhh)>
    <AcceptVerbs(HttpVerbs.Post)>
    Function addconsolidacioncambiopuesto(idSabColaborador As Integer, ticksVencimiento As Long, consolidacionCP As ConsolidacionCambioPuesto) As ActionResult
        Dim fechaVencimiento = New DateTime(ticksVencimiento)

        Return View("editconsolidacionpuesto")
    End Function


    Private Function GetColaboradoresYPreguntas(idSab As Integer)
        '#If DEBUG Then
        '        idSab = 60197
        '        idSab = 57532
        '        idSab = 60197 'xabat goti
        '#End If
        Dim result = db.GetColaboradores(idSab, strCn, strCnMicrosoft).Where(Function(c)
                                                                                 Return c.notificacionColaborador Is Nothing
                                                                             End Function).GroupBy(Function(c) c.idTrabajador)
        'Dim result = db.GetColaboradores(idSab, strCn, strCnMicrosof).Where(Function(c)
        '                                                                        Return c.notificacionColaborador Is Nothing
        '                                                                    End Function).GroupBy(Function(c) c.idTrabajador)
        Return result
    End Function
    Private Function GetColaboradoresYPreguntasCerradas(idSab As Integer)
        Return db.GetColaboradores(idSab, strCn, strCnMicrosoft).Where(Function(c)
                                                                           Return c.notificacionColaborador IsNot Nothing
                                                                       End Function).GroupBy(Function(c) c.idTrabajador)
    End Function
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

    Function Test(idSabColaborador As Integer, ticksVencimiento As Long) As ActionResult
        Notificadocolaborador(idSabColaborador, ticksVencimiento)
        Return RedirectToAction("Index")
    End Function
End Class
