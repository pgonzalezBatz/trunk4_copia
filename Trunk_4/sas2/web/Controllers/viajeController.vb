Imports System.Security.Permissions
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Data.Entity.Core.Common.EntitySql
Namespace web
    Public Class viajeController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString

        <SimpleRoleProvider(Role.envio)>
        Function List(ByVal filter As String, ByVal q As String) As ActionResult
            ViewData("listoftipoviaje") = h.MySelectList(DBAccess.GetListOfTiposViaje(strCn),
                                                         Function(o) New Mvc.SelectListItem With {.Value = o.id, .Text = o.nombre})
            Dim l = DBAccess.GetListEnvio(False, "", strCn)
            Select Case Request("f")
                Case "transportista"
                    l = l.Where(Function(el) el.IdTransportista = Request("v")).ToList
            End Select
            ViewData("puertas") = DBAccess.GetListPuerta(strCn)
            Dim l2 = l.OrderBy(Function(el) el.fechaCreacion)
            Return View(l2.ToList)
        End Function
        <SimpleRoleProvider(Role.envio)>
        Function filter(ByVal f As String) As ActionResult
            Select Case f
                Case "transportista"
                    Dim l = DBAccess.GetListEnvio(False, "", strCn)
                    Dim l2 = l.GroupBy(Function(v)
                                           Dim t = DBAccess.GetTransportista(v.IdTransportista, ConfigurationManager.ConnectionStrings("sas").ConnectionString)
                                           Return New With {Key .id = v.IdTransportista, Key .nombre = t.nombre}
                                       End Function, Function(k, el) New With {.codpro = k.id, .nombre = k.nombre})
                    ViewData("list") = l2
            End Select
            Return View("~/Views/transporte/filter.aspx")
        End Function
        <SimpleRoleProvider(Role.envio)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function Enviar(ByVal id As Integer, ByVal fecha As Nullable(Of DateTime), ByVal notificar As String, listoftipoviaje As Nullable(Of Integer), productivo As String, comentarioalmacen As String, facturarportiempo As String, preciotrpdia As Nullable(Of Decimal)) As ActionResult
            If Not listoftipoviaje.HasValue Then
                ModelState.AddModelError("listoftipoviaje", "Es necesario introducir un tipo viaje")
                ViewData("listoftipoviaje") = h.MySelectList(DBAccess.GetListOfTiposViaje(strCn),
                                                         Function(o) New Mvc.SelectListItem With {.Value = o.id, .Text = o.nombre})
                Return View("list", DBAccess.GetListEnvio(False, "", strCn))
            End If
            DBAccess.DarSalidaAEnvio(id, fecha, listoftipoviaje, Not String.IsNullOrEmpty(notificar), If(String.IsNullOrEmpty(productivo), 1, 0), comentarioalmacen, If(String.IsNullOrEmpty(facturarportiempo), 0, 1), strCn)
            Dim env = DBAccess.GetEnvio(id, strCn)
            Dim t = DBAccess.GetTransportista(env.IdTransportista, strCn)
            If Not h.GetListOfDefaultEmpresaFromStrCn(strCn).Exists(Function(o) o.id = env.IdTransportista) Then
                If Not String.IsNullOrEmpty(notificar) AndAlso t.email.length > 0 AndAlso Regex.IsMatch(t.email, ".+@.+") Then
                    Dim str As New Text.StringBuilder
                    str.Append("Este es un viaje de la división de troquelería.") : str.Append(Environment.NewLine)
                    str.Append("El documento de viaje se encuentra adjuntado.") : str.Append(Environment.NewLine)
                    str.Append("Responder a este email con la valoración del viaje.")
                    h.SendEmail(t.email, "Batz salida viaje Troquelería Nº " + id.ToString, str.ToString, New Net.Mail.Attachment(h.viajePdf(id), "Viaje_" + id.ToString, "application/pdf"))
                End If
                If Not String.IsNullOrEmpty(notificar) AndAlso t.email2.length > 0 AndAlso Regex.IsMatch(t.email2, ".+@.+") Then
                    Dim str As New Text.StringBuilder
                    str.Append("Este es un viaje de la división de troquelería.") : str.Append(Environment.NewLine)
                    str.Append("El documento de viaje se encuentra adjuntado.") : str.Append(Environment.NewLine)
                    str.Append("Responder a este email con la valoración del viaje.")
                    h.SendEmail(t.email2, "Batz salida viaje Troquelería Nº " + id.ToString, str.ToString, New Net.Mail.Attachment(h.viajePdf(id), "Viaje_" + id.ToString, "application/pdf"))
                End If
            End If
            If Not String.IsNullOrEmpty(facturarportiempo) Then
                DBAccess.InsertOUpdatePedido(id, preciotrpdia.Value, SimpleRoleProvider.GetId(), True, strCn)
            End If
            If DBAccess.GetOFSinSubcontratar(id, strCn).Count > 0 Then
                Return RedirectToAction("subcontratacion", New With {.id = id})
            End If
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.envio)>
        Function Subcontratacion(ByVal id As Integer)
            Return View(DBAccess.GetOFSinSubcontratar(id, strCn))
        End Function
        <SimpleRoleProvider(Role.envio)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function Subcontratacion2(ByVal id As Integer)
            Dim str As New Text.StringBuilder("Se han enviado estas OFs a proveedores:")
            str.Append(Environment.NewLine) : str.Append(Environment.NewLine)
            For Each o In DBAccess.GetOFSinSubcontratar(id, strCn)
                str.Append(o.numord.ToString)
                str.Append(Environment.NewLine)
            Next
            h.SendEmail(ConfigurationManager.AppSettings("nofiticarsubcontratacion"), "SAS2. Notificación para crear pedidos de subcontratación", str.ToString)
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.envio)>
        Function Create(ByVal albaran As List(Of Integer), ByVal recogida As List(Of Integer)) As ActionResult
            Dim negocios As Integer = 0
            If (albaran IsNot Nothing AndAlso albaran.Count > 0) Or (recogida IsNot Nothing AndAlso recogida.Count > 0) Then
                ViewData("matriculas") = DBAccess.GetListOfMatricula(strCn)
                If albaran IsNot Nothing AndAlso recogida IsNot Nothing Then
                    Dim listaAlbaranes = DBAccess.GetListAlbaran(strCn).FindAll(Function(a) albaran.Exists(Function(i) i = a.Id))
                    Dim negociosAlbaranes = listaAlbaranes.Select(Function(o) o.IdNegocio).Distinct()
                    Dim listaRecogidas = DBAccess.GetListOfRecogidas(strCn).FindAll(Function(a) recogida.Exists(Function(i) i = a.Id))
                    Dim negociosRecogidas = listaRecogidas.Select(Function(o) o.IdNegocio).Distinct()
                    negocios = negociosAlbaranes.Union(negociosRecogidas).Distinct().Count()
                    If negocios > 1 Then
                        ModelState.AddModelError("Negocio", "Los albaranes y recogidas no se pueden agrupar, pertenecen a diferentes negocios")
                        Return RedirectToAction("list", "viaje")
                    End If
                ElseIf albaran IsNot Nothing Then
                    Dim listaAlbaranes = DBAccess.GetListAlbaran(strCn).FindAll(Function(a) albaran.Exists(Function(i) i = a.Id))
                    negocios = listaAlbaranes.Select(Function(o) o.IdNegocio).Distinct().Count()
                    If negocios > 1 Then
                        ModelState.AddModelError("Negocio", "Los albaranes no se pueden agrupar, pertenecen a diferentes negocios")
                        Return RedirectToAction("list", "albaran")
                    End If
                    ViewData("listOfAlbaran") = listaAlbaranes
                ElseIf recogida IsNot Nothing Then
                    Dim listaRecogidas = DBAccess.GetListOfRecogidas(strCn).FindAll(Function(a) recogida.Exists(Function(i) i = a.Id))
                    negocios = listaRecogidas.Select(Function(o) o.IdNegocio).Distinct().Count()
                    If negocios > 1 Then
                        ModelState.AddModelError("Negocio", "Las recogidas no se pueden agrupar, pertenecen a diferentes negocios")
                        Return RedirectToAction("list", "recogida")
                    End If
                    ViewData("listOfRecogida") = listaRecogidas
                End If
                ViewData("Transportista") = DBAccess.GetListOfTransportista(strCn)
                Return View("create")
            End If
            Return Redirect(Request.UrlReferrer.AbsolutePath)
        End Function
        <SimpleRoleProvider(Role.envio)> _
        Function SaveEnvio(ByVal transportista As String, ByVal matricula1 As String, ByVal matricula2 As String, ByVal albaran As List(Of Integer), ByVal recogida As List(Of Integer)) As ActionResult
            If albaran IsNot Nothing AndAlso albaran.Count > 0 Then
                'DBAccess.ExisteAlgunALbaran(albaran, strCn)
                If Not DBAccess.UnicoNegocioEnAlbaranes(albaran, strCn) Then
                    ModelState.AddModelError("", "No se puede crear un viaje con albaranes de distintos negocios")
                    Return Create(albaran, Nothing)
                End If
                Dim id_viaje = DBAccess.SaveEnvio(transportista, matricula1, matricula2, albaran, recogida, strCn)
                Return RedirectToAction("list")
            End If
            If recogida IsNot Nothing AndAlso recogida.Count > 0 Then
                'DBAccess.ExisteAlgunALbaran(albaran, strCn)
                If Not DBAccess.UnicoNegocioEnRecogidas(recogida, strCn) Then
                    ModelState.AddModelError("", "No se puede crear un viaje con recogidas de distintos negocios")
                    Return Create(Nothing, recogida)
                End If
                Dim id_viaje = DBAccess.SaveEnvio(transportista, matricula1, matricula2, albaran, recogida, strCn)
                Return RedirectToAction("list")
            End If
            Throw New Exception("No se esta siguiendo la logica de la aplicación")
        End Function
        <SimpleRoleProvider(Role.envio)>
        Function Edit(ByVal id As Integer) As ActionResult
            If DBAccess.GetListEnvio(False, "", strCn).Where(Function(v) v.Id = id).Any() Then
                Return View(DBAccess.GetEnvio(id, strCn))
            End If
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.envio)>
        Function RemoveMovimiento(id As Integer, ByVal movimiento As Integer)
            DBAccess.RemoveMovimientoFromGrupo(movimiento, strCn)
            Return RedirectToAction("edit", New With {.id = id})
        End Function
        <SimpleRoleProvider(Role.envio)>
        Function RemoveBulto(id As Integer, ByVal bulto As Integer)
            DBAccess.RemoveGrupoFromAlbaran(bulto, strCn)
            Return RedirectToAction("edit", New With {.id = id})
        End Function
        <SimpleRoleProvider(Role.envio)>
        Function RemoveAlbaran(id As Integer, ByVal albaran As Integer)
            DBAccess.RemoveDocumentoFromViaje(albaran, "A", strCn)
            Return RedirectToAction("edit", New With {.id = id})
        End Function
        <SimpleRoleProvider(Role.envio)>
        Function removeRecogida(id As Integer, ByVal recogida As Integer)
            DBAccess.RemoveDocumentoFromViaje(recogida, "R", strCn)
            Return RedirectToAction("edit", New With {.id = id})
        End Function
        <SimpleRoleProvider(Role.envio)> _
        Function RemoveViaje(ByVal viaje As Integer)
            DBAccess.RemoveViaje(viaje, strCn)
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.envio)> _
        Function PackingList(ByVal id As Integer) As FileStreamResult
            Dim v = DBAccess.GetEnvio(id, strCn)
            Dim doc As Document = New Document(iTextSharp.text.PageSize.A4)
            Dim bf = New Font(Font.FontFamily.HELVETICA, 9, 0)
            Dim bf1 = New Font(Font.FontFamily.HELVETICA, 10, 1)
            doc.SetMargins(30, 10, 30, 30)
            Dim strMS As New MemoryStream()

            Dim img As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("~/") + "/Content/header.png")
            img.ScalePercent((doc.PageSize.Width - 60) / img.Width * 100)
            Dim wrtTest = PdfWriter.GetInstance(doc, strMS)
            wrtTest.CloseStream = False
            doc.Open()
            doc.Add(img)
            doc.Add(New Phrase(Environment.NewLine))
            doc.Add(New Phrase("PackingList:", bf1))
            doc.Add(New Phrase(Environment.NewLine))
            For Each a In v.ListOfAlbaran
                Dim direccion = DBAccess.GetDireccionAlbaran(a.Id, strCn)

                doc.Add(New Phrase("Albaran Nº " + a.Id.ToString, bf)) : doc.Add(New Phrase(Environment.NewLine, bf))
                doc.Add(New Phrase(direccion.nombreEmpresa, bf)) : doc.Add(New Phrase(Environment.NewLine, bf))
                doc.Add(New Phrase(direccion.calle, bf)) : doc.Add(New Phrase(Environment.NewLine, bf))
                doc.Add(New Phrase(direccion.codigopostal + " " + direccion.poblacion, bf)) : doc.Add(New Phrase(Environment.NewLine, bf))
                doc.Add(New Phrase(direccion.pais, bf))
                Dim t = New pdf.PdfPTable(2)
                t.AddCell(New Phrase("Id bulto", bf)) : t.AddCell(New Phrase("Peso (Kg)", bf))
                For Each b In a.ListOfAgrupacion
                    t.AddCell(New Phrase(b.Id.ToString, bf))
                    t.AddCell(New Phrase(h.CalcularPesoTotal(b).ToString, bf))
                Next

                doc.Add(t)
                doc.Add(New Phrase(Environment.NewLine))
            Next
            For Each r In v.ListOfRecogida
                doc.Add(New Phrase("Recogida Nº " + r.Id.ToString + " , procedencia: " + r.nombreEmpresaRecogida + " , Destino: " + r.nombreEmpresaEntrega, bf))
                Dim direccion = DBAccess.GetDireccionProveedor(r.IdEmpresaEntrega, strCn)
                Dim t = New pdf.PdfPTable(3)
                t.AddCell(New Phrase("Direccion", bf)) : t.AddCell(New Phrase("Nº Bultos", bf)) : t.AddCell(New Phrase("Peso (Kg)", bf))
                t.AddCell(New Phrase(direccion.calle + ", " + direccion.codigopostal + " " + direccion.poblacion + Environment.NewLine + direccion.pais, bf))
                t.AddCell(New Phrase(r.ListOfOp.Count.ToString, bf))
                t.AddCell(New Phrase(r.ListOfOp.Sum(Function(g) g.Peso).ToString, bf))
                doc.Add(t)
                doc.Add(New Phrase(Environment.NewLine))
            Next
            doc.Close()
            strMS.Seek(0, System.IO.SeekOrigin.Begin)
            Return New FileStreamResult(strMS, "application/pdf")
        End Function
        <SimpleRoleProvider(0)> _
        Function Imprimir(ByVal id As Integer) As FileStreamResult
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=viaje_" + id.ToString + ".pdf")
            Return New FileStreamResult(h.viajePdf(id), "application/pdf")
        End Function
        <SimpleRoleProvider(Role.envio)>
        Function Etiqueta(ByVal id As Integer) As ActionResult
            Dim v = DBAccess.GetEnvio(id, strCn)
            If v.ListOfAlbaran.Count = 0 Then
                Throw New Exception("El viaje no tiene albaranes")
            End If
            For Each a In v.ListOfAlbaran
                For Each gr In a.ListOfAgrupacion
                    Dim hel = DBAccess.GetDireccionAlbaran(a.Id, strCn) ' DBAccess.GetProveedorConDireccion(gr.ListOfMovimiento.First.CodPro, strCn)
                    Dim t1 = New pdf.PdfPTable(2)

                    h.printLabel(New With {.proveedor = hel.nombreEmpresa, .calle = hel.calle, .codigoPostal = hel.codigopostal,
                                 .poblacion = hel.poblacion, .provincia = hel.provincia, .telefono = hel.telefono, .peso = h.CalcularPesoTotal(gr), .idBulto = gr.Id})
                Next
            Next
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.envio)>
        Function AddAlbaran() As ActionResult
            Dim idViaje = Request("id")
            Dim negocios = DBAccess.GetNegociosFromViaje(idViaje, strCn)
            If negocios.count > 1 Then
                '''ERROR
                RedirectToAction("list", "viaje")
            End If
            Dim idNegocio = negocios(0)
            Dim lst = DBAccess.GetListAlbaran(strCn).FindAll(Function(o) o.IdNegocio = idNegocio)
            If lst.Count > 0 Then
                If lst.First().IdHelbide.HasValue Then
                    ViewData("helbide") = DBAccess.GetHelbide(lst.First().IdHelbide, strCn)
                Else
                    If lst.First().ListOfAgrupacion.Count > 0 Then
                        ViewData("helbide") = DBAccess.GetDireccionProveedor(lst.First().ListOfAgrupacion.First().ListOfMovimiento.First().CodPro, strCn)
                    End If
                End If
            End If
            Return View(lst)
        End Function
        <SimpleRoleProvider(Role.envio)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function AddAlbaran(ByVal id As Integer, ByVal idAlbaran As Integer) As ActionResult
            '''' no hacemos el chequeo de negocio porque ya se ha hecho en la vista
            DBAccess.AddAlbaranRecogidaToViaje(id, idAlbaran, "A", strCn)
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.envio)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function AddToExistingEnvio(ByVal idViaje As Nullable(Of Integer), ByVal albaran As List(Of Integer), recogida As List(Of Integer)) As ActionResult
            If Not idViaje.HasValue Then
                Return Create(albaran, Nothing)
            End If
            If albaran IsNot Nothing Then
                If Not DBAccess.UnicoNegocioEnAlbaranes(albaran, strCn) Then
                    ModelState.AddModelError("", "No se puede crear un albaran con movimientos de distintos negocios")
                    Return Create(albaran, Nothing)
                End If
                If Not DBAccess.MismoNegocioAlbaranViaje(albaran.FirstOrDefault(), idViaje, strCn) Then
                    ModelState.AddModelError("", "No se puede crear un albaran con movimientos de distintos negocios")
                    Return Create(albaran, Nothing)
                End If
                DBAccess.AddAlbaran(idViaje, albaran, strCn)
            End If
            If recogida IsNot Nothing Then
                For Each r In recogida
                    DBAccess.AddAlbaranRecogidaToViaje(idViaje, r, "R", strCn)
                Next
            End If
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.envio)> _
        Function decamino(idViaje As Integer) As ActionResult
            Dim viaje = DBAccess.GetEnvio(idViaje, strCn)
            For Each a In viaje.ListOfAlbaran
                Dim hel = DBAccess.GetDireccionAlbaran(a.Id, strCn)
                Dim email = DBAccess.GetEmailempresa(hel.idEmpresa, strCn)
                h.SendEmail(email, "Batz salida viaje Nº " + idViaje.ToString, "Próximamente recibirán el material que les indicamos en el albarán adjunto, deberán verificar si todo lo que indica el albarán ha llegado a sus instalaciones y si en el plazo de 3 días desde su recepción no hemos recibido ninguna comunicación por su parte daremos como recepcionados por su parte los materiales indicados.", New Net.Mail.Attachment(h.albaranPdf(idViaje, a.Id, 0), "Viaje_" + idViaje.ToString, "application/pdf"))
            Next
            DBAccess.updateViajeSetDeCamino(idViaje, strCn)
            Return RedirectToAction("list")
        End Function
     
    End Class
End Namespace