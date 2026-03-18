Imports System.Security.Permissions
Namespace ExtranetMVC
    Public Class pedidosController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("segipe").ConnectionString

        <SimpleRoleProvider(1)>
        Function NuevosCabecera() As ActionResult
            Dim lst As New List(Of Integer) : lst.Add(Estados.sinLeer)
            ViewData("actiontolineas") = "nuevoslinea"
            Return View("cabeceragenerica", db.GetCabeceras(SimpleRoleProvider.GetId(), lst, strCn))
        End Function
        <SimpleRoleProvider(1)>
        Function NuevosLinea(ByVal pedido As Integer) As ActionResult
            Dim lstNuevos As New List(Of Integer) : lstNuevos.Add(Estados.sinLeer)
            ViewData("fromaction") = "leidoslinea"
            ViewData("observaciones") = db.GetObservaciones(pedido, strCn)
            ViewData("adjuntos") = db.GetAdjuntos(pedido, strCn)
            ViewData("cabecera") = db.GetCabecera(pedido, strCn)

            Dim lineas = db.GetLineas(pedido, lstNuevos, strCn)
            db.UpdateEstado(pedido, Estados.leido, strCn)
            Return View(lineas)
        End Function
        <SimpleRoleProvider(1)>
        Function LeidosCabecera() As ActionResult
            Dim lst As New List(Of Integer) : lst.Add(Estados.leido)
            ViewData("actiontolineas") = "leidoslinea"
            Return View("cabeceragenerica", db.GetCabeceras(SimpleRoleProvider.GetId(), lst, strCn))
        End Function
        <SimpleRoleProvider(1)>
        Function LeidosLinea(ByVal pedido As Integer) As ActionResult
            Dim lst As New List(Of Integer) : lst.Add(Estados.leido)
            ViewData("fromaction") = "leidoslinea"
            ViewData("observaciones") = db.GetObservaciones(pedido, strCn)
            ViewData("adjuntos") = db.GetAdjuntos(pedido, strCn)
            ViewData("cabecera") = db.GetCabecera(pedido, strCn)
            Return View("nuevoslinea", db.GetLineas(pedido, lst, strCn))
        End Function
        <SimpleRoleProvider(1)>
        Function PropuestosCabecera() As ActionResult
            Dim lst As New List(Of Integer) : lst.Add(Estados.propuesto)
            ViewData("actiontolineas") = "propuestoslinea"
            Return View("cabeceragenerica", db.GetCabeceras(SimpleRoleProvider.GetId(), lst, strCn))
        End Function
        <SimpleRoleProvider(1)>
        Function PropuestosLinea(ByVal pedido As Integer) As ActionResult
            Dim lst As New List(Of Integer) : lst.Add(Estados.propuesto)
            ViewData("observaciones") = db.GetObservaciones(pedido, strCn)
            ViewData("adjuntos") = db.GetAdjuntos(pedido, strCn)
            ViewData("cabecera") = db.GetCabecera(pedido, strCn)
            Return View(db.GetLineasConPropuesta(pedido, lst, strCn))
        End Function
        <SimpleRoleProvider(1)>
        Function AceptadosCabecera() As ActionResult
            Dim lst As New List(Of Integer) From {Estados.aceptado, Estados.aVencer, Estados.aVencerAceptado}
            ViewData("actiontolineas") = "aceptadoslinea"
            Return View("cabeceragenerica", db.GetCabeceras(SimpleRoleProvider.GetId(), lst, strCn))
        End Function
        <SimpleRoleProvider(1)>
        Function AceptadosLinea(ByVal pedido As Integer) As ActionResult
            Dim lst As New List(Of Integer) From {Estados.aceptado, Estados.aVencer, Estados.aVencerAceptado}
            ViewData("fromaction") = "aceptadoslinea"
            ViewData("observaciones") = db.GetObservaciones(pedido, strCn)
            ViewData("adjuntos") = db.GetAdjuntos(pedido, strCn)
            ViewData("cabecera") = db.GetCabecera(pedido, strCn)
            Return View(db.GetLineas(pedido, lst, strCn))
        End Function
        <SimpleRoleProvider(1)>
        Function VencerCabecera() As ActionResult
            Dim lst As New List(Of Integer) : lst.Add(Estados.aVencer) : lst.Add(Estados.aVencerAceptado)
            ViewData("actiontolineas") = "vencerlinea"
            Return View("cabeceragenerica", db.GetCabeceras(SimpleRoleProvider.GetId(), lst, strCn))
        End Function
        <SimpleRoleProvider(1)>
        Function VencerLinea(ByVal pedido As Integer) As ActionResult
            Dim lst As New List(Of Integer) : lst.Add(Estados.aVencer) : lst.Add(Estados.aVencerAceptado)
            ViewData("fromaction") = "vencerlinea"
            ViewData("observaciones") = db.GetObservaciones(pedido, strCn)
            ViewData("adjuntos") = db.GetAdjuntos(pedido, strCn)
            ViewData("cabecera") = db.GetCabecera(pedido, strCn)
            Return View("aceptadoslinea", db.GetLineas(pedido, lst, strCn))
        End Function
        <SimpleRoleProvider(1)>
        Function Propuestos2Cabecera() As ActionResult
            Dim lst As New List(Of Integer) : lst.Add(Estados.propuesto2)
            ViewData("actiontolineas") = "propuestos2linea"
            Return View("cabeceragenerica", db.GetCabeceras(SimpleRoleProvider.GetId(), lst, strCn))
        End Function
        <SimpleRoleProvider(1)>
        Function propuestos2Linea(ByVal pedido As Integer) As ActionResult
            Dim lst As New List(Of Integer) : lst.Add(Estados.propuesto2)
            ViewData("fromaction") = "propuestos2linea"
            ViewData("observaciones") = db.GetObservaciones(pedido, strCn)
            ViewData("adjuntos") = db.GetAdjuntos(pedido, strCn)
            ViewData("cabecera") = db.GetCabecera(pedido, strCn)
            Return View("propuestoslinea", db.GetLineasConPropuesta(pedido, lst, strCn))
        End Function
        <SimpleRoleProvider(1)>
        Function AceptarCabecera(ByVal pedido As Integer, ByVal fromaction As String) As ActionResult
            If DateDiff(DateInterval.Day, Now, db.GetFechaEntregaCabecera(pedido, strCn)) < 7 Then
                db.UpdateEstado(pedido, Estados.aVencer, strCn)
            Else
                db.UpdateEstado(pedido, Estados.aceptado, strCn)
            End If
            Return RedirectToAction(fromaction, New With {.pedido = pedido})
        End Function
        <SimpleRoleProvider(1)>
        Function AceptarLinea(ByVal pedido As Integer, ByVal linea As Integer, ByVal fromaction As String) As ActionResult
            If DateDiff(DateInterval.Day, db.GetFechaEntregaLinea(pedido, linea, strCn), Now) < 7 Then
                db.UpdateEstado(pedido, linea, Estados.aVencer, strCn)
            Else
                db.UpdateEstado(pedido, linea, Estados.aceptado, strCn)
            End If
            Return RedirectToAction(fromaction, New With {.pedido = pedido})
        End Function
        <SimpleRoleProvider(1)>
        Function EnviarCabecera(ByVal pedido As Integer, ByVal fromaction As String) As ActionResult
            db.UpdateEstado(pedido, Estados.enviado, strCn)
            Return RedirectToAction(fromaction, New With {.pedido = pedido})
        End Function
        <SimpleRoleProvider(1)>
        Function EnviarLinea(ByVal pedido As Integer, ByVal linea As Integer, ByVal fromaction As String) As ActionResult
            db.UpdateEstado(pedido, linea, Estados.enviado, strCn)
            Return RedirectToAction(fromaction, New With {.pedido = pedido})
        End Function
        <SimpleRoleProvider(1)>
        Function ProponerCabecera(ByVal pedido As Integer, ByVal descuento As Nullable(Of Decimal), ByVal fecha As Nullable(Of DateTime), ByVal fromaction As String) As ActionResult
            If Not descuento.HasValue OrElse Not fecha.HasValue Then
                ModelState.AddModelError("descuento", "Es obligatorio introducir el descuento o la fecha para solicitar un cambio")
                ModelState.AddModelError("fecha", "Es obligatorio introducir el descuento o la fecha para solicitar un cambio")
            ElseIf Not IsNumeric(descuento.Value) Then
                ModelState.AddModelError("descuento", "El descuento no es valido")
            ElseIf Not IsNumeric(fecha.Value) Then
                ModelState.AddModelError("fecha", "La fecha no es valida")
            End If
            If ModelState.IsValid Then
                Dim lstEstado = New List(Of Estados)
                lstEstado.Add(Estados.sinLeer) : lstEstado.Add(Estados.leido)
                db.InsertPropuesta(pedido, descuento, fecha, lstEstado, Estados.propuesto, strCn)
                Return RedirectToAction(fromaction, New With {.pedido = pedido})
            End If
            If fromaction = "leidoslinea" Then
                Return LeidosLinea(pedido)
            ElseIf fromaction = "leidoslinea" Then
                Return NuevosLinea(pedido)
            End If
            Return RedirectToAction(fromaction, New With {.pedido = pedido})
        End Function
        <SimpleRoleProvider(1)>
        Function ProponerLinea(ByVal pedido As Integer, ByVal linea As Integer, ByVal precio As Nullable(Of Decimal), ByVal descuento As Nullable(Of Decimal), ByVal fecha As Nullable(Of DateTime), ByVal fromaction As String) As ActionResult
            If precio.HasValue OrElse descuento.HasValue OrElse fecha.HasValue Then
                db.InsertPropuesta(pedido, linea, precio, descuento, fecha, Estados.propuesto, strCn)
                Return RedirectToAction(fromaction, New With {.pedido = pedido})
            End If
            Return RedirectToAction(fromaction, New With {.pedido = pedido})
        End Function
        <SimpleRoleProvider(1)>
        Function Proponer2Cabecera(ByVal pedido As Integer, ByVal fecha As Nullable(Of DateTime), ByVal fromaction As String) As ActionResult
            If fecha.HasValue Then
                Dim lstEstado = New List(Of Estados)
                lstEstado.Add(Estados.aceptado) : lstEstado.Add(Estados.aVencer) : lstEstado.Add(Estados.aVencerAceptado)
                db.InsertPropuesta(pedido, Nothing, fecha, lstEstado, Estados.propuesto2, strCn)
                Return RedirectToAction(fromaction, New With {.pedido = pedido})
            End If
            Return RedirectToAction(fromaction)
        End Function
        <SimpleRoleProvider(1)>
        Function Proponer2Linea(ByVal pedido As Integer, ByVal linea As Integer, ByVal fecha As Nullable(Of DateTime), ByVal fromaction As String) As ActionResult
            If fecha.HasValue Then
                db.InsertPropuesta(pedido, linea, Nothing, Nothing, fecha, Estados.propuesto2, strCn)
                Return RedirectToAction(fromaction, New With {.pedido = pedido})
            End If
            Return RedirectToAction(fromaction, New With {.pedido = pedido})
        End Function
        <SimpleRoleProvider(1)>
        Function buscador() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(1)>
        Function buscadorporpedido(ByVal desde As Nullable(Of Integer), ByVal hasta As Nullable(Of Integer)) As ActionResult
            If Not desde.HasValue Then : ModelState.AddModelError("desde", "Es obligatorio introducir desde numero de pedido") : End If
            If Not hasta.HasValue Then : ModelState.AddModelError("hasta", "Es obligatorio introducir hasta numero de pedido") : End If
            If ModelState.IsValid Then
                Dim lst As New List(Of Integer) : lst.Add(Estados.sinLeer) : lst.Add(Estados.leido) : lst.Add(Estados.propuesto) : lst.Add(Estados.aceptado)
                lst.Add(Estados.aVencer) : lst.Add(Estados.aVencerAceptado) : lst.Add(Estados.propuesto2) : lst.Add(Estados.enviado) : lst.Add(Estados.recibido)
                ViewData("actiontolineas") = "buscadorlinea"
                Dim filter = db.GetCabeceras(SimpleRoleProvider.GetId(), lst, strCn).FindAll(Function(c) c.pedido >= desde And c.pedido <= hasta)
                Return View("cabeceragenerica", filter)
            End If
            Return View("buscador")
        End Function
        <SimpleRoleProvider(1)>
        Function buscadorporfecha(ByVal desde As Nullable(Of DateTime), ByVal hasta As Nullable(Of DateTime)) As ActionResult
            If Not desde.HasValue Then : ModelState.AddModelError("desde", "Es obligatorio introducir desde fecha de pedido") : End If
            If Not hasta.HasValue Then : ModelState.AddModelError("hasta", "Es obligatorio introducir hasta fecha de pedido") : End If
            If ModelState.IsValid Then
                Dim lst As New List(Of Integer) : lst.Add(Estados.sinLeer) : lst.Add(Estados.leido) : lst.Add(Estados.propuesto) : lst.Add(Estados.aceptado)
                lst.Add(Estados.aVencer) : lst.Add(Estados.aVencerAceptado) : lst.Add(Estados.propuesto2) : lst.Add(Estados.enviado) : lst.Add(Estados.recibido)
                ViewData("actiontolineas") = "buscadorlinea"
                Dim filter = db.GetCabeceras(SimpleRoleProvider.GetId(), lst, strCn).FindAll(Function(c) c.fechaEntrega >= desde And c.fechaEntrega <= hasta)
                Return View("cabeceragenerica", filter)
            End If
            Return View("buscador")
        End Function
        <SimpleRoleProvider(1)>
        Function buscadorlinea(ByVal pedido As Integer) As ActionResult
            Dim lst As New List(Of Integer) : lst.Add(Estados.sinLeer) : lst.Add(Estados.leido) : lst.Add(Estados.propuesto) : lst.Add(Estados.aceptado)
            lst.Add(Estados.aVencer) : lst.Add(Estados.aVencerAceptado) : lst.Add(Estados.propuesto2) : lst.Add(Estados.enviado) : lst.Add(Estados.recibido)
            ViewData("observaciones") = db.GetObservaciones(pedido, strCn)
            ViewData("adjuntos") = db.GetAdjuntos(pedido, strCn)
            ViewData("cabecera") = db.GetCabecera(pedido, strCn)
            Return View(db.GetLineas(pedido, lst, strCn))
        End Function

        <SimpleRoleProvider(1)>
        Function observaciones(ByVal pedido As Integer, ByVal texto As String, ByVal fromaction As String) As ActionResult
            If texto Is Nothing OrElse texto.Length = 0 Then
                ModelState.AddModelError("texto", "Es obligatorio introducir texto para guardar el comentario")
            End If
            If ModelState.IsValid Then
                db.insertObservacion(pedido, texto, SimpleRoleProvider.GetId(), strCn)
                Dim o = db.GetProveedorEmailLangile(pedido, strCn)
                h.SendEmail(o.email, "Observación Nº pedido: " + pedido.ToString, o.nombre + Environment.NewLine + texto)
                Return Redirect(fromaction)
            End If
            Return Redirect(fromaction)
        End Function
        <SimpleRoleProvider(1)>
        Function adjunto(ByVal id As Integer, ByVal nombre As String) As ActionResult
            Return File(db.GetAdjunto(id, strCn), "application/octet-stream", nombre)
        End Function
        <SimpleRoleProvider(1)>
        Function pdf(ByVal pedido As Integer) As ActionResult
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=pedido.pdf")
            Return h.GeneratePdf(db.GetCabeceraPdf(pedido, SimpleRoleProvider.GetId(), strCn), db.GetLineasPdf(pedido, strCn))
        End Function
    End Class
End Namespace