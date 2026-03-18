Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports LinqKit
Imports iTextSharp.text
Namespace Controllers
    Public Class INFORMEController
        Inherits System.Web.Mvc.Controller

        Private dbES As New Entities_soldadura
        Private strCn As String = ConfigurationManager.ConnectionStrings("oracle").ConnectionString

        <SimpleRoleProvider(Roles.externo, Roles.interno)>
        Function Index(displayEntregadas As Nullable(Of Boolean), q As String, sortby As String) As ActionResult
            Dim dbInformes As IQueryable(Of INFORMES)
            If String.IsNullOrEmpty(q) Then
                dbInformes = dbES.INFORMES
            Else
                dbInformes = dbES.INFORMES.AsExpandable.Where(h.searchFilter(Of INFORMES)(q, Function(i) i.VALOROF, Function(i) i.CLIENTE, Function(i) i.PROYECTO, Function(i) i.MARCA, Function(i) i.VALOROF + "-" + i.VALOROP))
            End If

            displayEntregadas = If(displayEntregadas, False)
            ViewData("displayEntregadas") = displayEntregadas
            Dim idSab As Integer = SimpleRoleProvider.GetId()
            ViewData("nombreproveedor") = db.getNombreProveedor(idSab, strCn)
            Dim ofSub = db.GetOFOPMarcaSubcontratadas(SimpleRoleProvider.GetId(), strCn)

            Dim l = dbES.INFORME_PROVEEDOR.Where(Function(i) i.ID_SAB = idSab).Join(dbInformes, Function(ip) ip.ID_INFORME, Function(i) i.IDINFORME,
                                                                                     Function(ip, i) New With {.CLIENTE = i.CLIENTE, .PROYECTO = i.PROYECTO, .TIPOINFORME = i.TIPOINFORME,
                                                                                     .MARCA = i.MARCA, .IDINFORME = i.IDINFORME, .VALOROF = i.VALOROF, .VALOROP = i.VALOROP, .VALIDADO = ip.VALIDADO,
                                                                                     .adjuntos = i, .pedido = ip.NUMPEDCAB})
            Select Case sortby
                Case "ofop"
                    l = l.OrderBy(Function(i) i.VALOROF + i.VALOROP)
                Case "tipoinforme"
                    l = l.OrderBy(Function(i) i.TIPOINFORME)
            End Select

            If displayEntregadas Then
                Return View(l.ToList.Where(Function(i) Not ofSub.Any(Function(o) o.numpedlin = i.pedido And o.numord = i.VALOROF And o.numope = i.VALOROP And i.MARCA.Split("|").Contains(o.marca.trim(" ")))))
            Else
                Return View(l.ToList.Where(Function(i) ofSub.Any(Function(o) (o.numpedlin = i.pedido Or i.pedido Is Nothing) And o.numord = i.VALOROF And o.numope = i.VALOROP And i.MARCA.Split("|").Contains(o.marca.trim(" ")))))
            End If
        End Function
        <SimpleRoleProvider(Roles.externo, Roles.interno)>
        Function marcasPendientes() As ActionResult
            Dim idSab As Integer = SimpleRoleProvider.GetId()
            ViewData("nombreproveedor") = db.getNombreProveedor(idSab, strCn)
            Dim ofSub = db.GetOFOPSubcontratadas(SimpleRoleProvider.GetId(), strCn)
            Return View(ofSub.Select(Function(ofop) New With {.numord = ofop.numord, .numope = ofop.numope, .lstMarca = db.GetMarcasSinInforme(SimpleRoleProvider.GetId(), ofop.numord, ofop.numope, ofop.numpedlin, ConfigurationManager.ConnectionStrings("oracle").ConnectionString)}))
        End Function
        <SimpleRoleProvider(Roles.externo, Roles.interno)>
        Function Details(ByVal idinforme As Decimal) As ActionResult
            If IsNothing(idinforme) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim oInf As INFORMES = dbES.INFORMES.Find(idinforme)
            If IsNothing(oInf) Then
                Return HttpNotFound()
            End If
            If oInf.TIPOINFORME = "soldadura" Then
                Return View("details_soldadura", oInf)
            Else
                Return View("details_temple", oInf)
            End If
        End Function
        <SimpleRoleProvider(Roles.externo)>
        Function Create1() As ActionResult
            ViewData("tipoInforme") = New List(Of Mvc.SelectListItem)() From {New SelectListItem() With {.Value = "soldadura", .Text = h.traducir("Informe de soldadura")},
                New SelectListItem() With {.Value = "soldadura laser", .Text = h.traducir("Informe de soldadura laser")},
                New SelectListItem() With {.Value = "temple induccion", .Text = h.traducir("Informe de temple inducción")},
                New SelectListItem() With {.Value = "temple laser", .Text = h.traducir("Informe de temple laser")},
                New SelectListItem() With {.Value = "temple total", .Text = h.traducir("Informe de temple total")},
                New SelectListItem() With {.Value = "tratamiento secundario", .Text = h.traducir("Informe de tratamiento secundario")},
                 New SelectListItem() With {.Value = "fundicion", .Text = h.traducir("Certificado de fundicion")}}
            Return View("edit1")
        End Function
        <SimpleRoleProvider(Roles.externo)>
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create1(tipoInforme As String, numord As Integer?, numope As Integer?) As ActionResult
            If String.IsNullOrEmpty(tipoInforme) Then
                ModelState.AddModelError("tipoInforme", h.traducir("Es necesario elegir el tipo de informe"))
            End If
            If Not numord.HasValue Then
                ModelState.AddModelError("numord", h.traducir("Es necesario introducir numero de OF"))
            End If
            If Not numope.HasValue Then
                ModelState.AddModelError("numope", h.traducir("Es necesario introducir numero de OP"))
            End If
            Dim lstNumordNumope = db.GetOFOPSubcontratadas(SimpleRoleProvider.GetId(), strCn)
            If numord.HasValue AndAlso numope.HasValue Then
                If Not lstNumordNumope.Any(Function(s) s.numord = numord.Value) Then
                    ModelState.AddModelError("numord", h.traducir("No existe pedido para la OF introducida"))
                End If
            End If
            If numope.HasValue AndAlso numope.HasValue Then
                If Not lstNumordNumope.Any(Function(s) s.numope = numope.Value) Then
                    ModelState.AddModelError("numope", h.traducir("No existe pedido para la OP introducida"))
                End If
            End If
            If ModelState.IsValid Then
                Dim lstFiltered = lstNumordNumope.Where(Function(s) s.numope = numope.Value And s.numord = numord.Value)
                If lstFiltered.Count > 1 Then
                    Return RedirectToAction("pickPedido", h.ToRouteValuesDelete(Request.Form, "__RequestVerificationToken"))
                ElseIf lstFiltered.Count = 1 Then
                    Return RedirectToAction("create2", h.ToRouteValues(h.ToRouteValuesDelete(Request.Form, "__RequestVerificationToken"), New With {.numpedlin = lstFiltered.First.numpedlin}))
                End If
                'Dim numpedlin = lstNumordNumope.First(Function(s) s.numope = numope.Value And s.numord = numord.Value).numpedlin
            End If
            Return Create1()
        End Function
        Function pickPedido(tipoInforme As String, numord As Integer?, numope As Integer?) As ActionResult
            Return View(db.GetOFOPSubcontratadas(SimpleRoleProvider.GetId(), strCn).Where(Function(s) s.numope = numope.Value And s.numord = numord.Value))
        End Function
        <SimpleRoleProvider(Roles.externo)>
        Function Create2(tipoInforme As String, numord As Integer, numope As Integer, numpedlin As Integer) As ActionResult
            Return View("edit2", db.GetMarcasSinInforme(SimpleRoleProvider.GetId(), numord, numope, numpedlin, ConfigurationManager.ConnectionStrings("oracle").ConnectionString))
        End Function
        <SimpleRoleProvider(Roles.externo)>
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create2(tipoInforme As String, numord As Integer, numope As Integer, numpedlin As Integer, marca As List(Of marcaViewModel)) As ActionResult
            Dim lstMarca = marca.Where(Function(m) m.isSelected).Select(Function(m) m.id).ToList
            Dim lstM As New List(Of marca)
            If lstMarca Is Nothing Then
                ModelState.AddModelError("marca", h.traducir("Es necesario seleccionar una marca como minimo"))
            Else
                lstM = db.GetMarcasSinInforme(SimpleRoleProvider.GetId(), numord, numope, numpedlin, dbES.Database.Connection.ConnectionString).Where(Function(m) lstMarca.Contains(m.marca)).ToList
                If lstM.GroupBy(Function(m) m.material + m.tratamiento + m.tratamientoSecundario).Count > 1 Then
                    ModelState.AddModelError("marca", h.traducir("Es necesario seleccionar marcas del mismo material, tratamiento y tratamiento secundario"))
                End If
            End If
            If ModelState.IsValid Then
                Return Create3(tipoInforme, numord, numope, numpedlin, lstM)
            End If
            Return Create2(tipoInforme, numord, numope, numpedlin)
        End Function
        <SimpleRoleProvider(Roles.externo)>
        Function Create3(tipoInforme As String, numord As Integer, numope As Integer, numpedlin As Integer, marca As IEnumerable(Of marca)) As ActionResult
            Dim oInf As INFORMES = db.GetDatosOF(numord, numope, strCn)
            oInf.VALOROF = numord : oInf.VALOROP = numope : oInf.TIPOINFORME = tipoInforme
            ViewData("marca") = marca
            ViewData("comunesMarca") = marca.First
            ViewData("numpedlin") = numpedlin
            If tipoInforme = "soldadura" Then
                ViewData("lsttiposoldaduraduro") = dbES.TIPOSOLDADURA.Select(Function(s) New Mvc.SelectListItem() With {.Value = s.TIPO, .Text = s.TIPO})
                ViewData("lstmaterialaportacionsoldduro") = New List(Of Mvc.SelectListItem)
                ViewData("lstvarillasoldaduraduro") = New List(Of Mvc.SelectListItem)
                ViewData("lstintensidadsoldaduraduro") = New List(Of Mvc.SelectListItem) 'Loaded by ajax
                ViewData("lsttiposoldadurablando") = dbES.TIPOSOLDADURA.Select(Function(s) New Mvc.SelectListItem() With {.Value = s.TIPO, .Text = s.TIPO})
                ViewData("lstmaterialaportacionsoldblando") = New List(Of Mvc.SelectListItem)
                ViewData("lstvarillasoldadurablando") = New List(Of Mvc.SelectListItem)
                ViewData("lstintensidadsoldadurablando") = New List(Of Mvc.SelectListItem) 'Loaded by ajax
                Return View("edit3_soldadura", oInf)
            ElseIf tipoInforme.ToLower.Contains("temple total") OrElse tipoInforme.ToLower.Contains("fundicion") OrElse tipoInforme.ToLower.Contains("soldadura laser") OrElse tipoInforme.ToLower.Contains("tratamiento secundario") Then
                Return View("edit3_temple_total", oInf)
            Else
                ViewData("durezarequerida") = Regex.Match(If(ViewData("comunesMarca").TRATAMIENTO, ""), "([\d\-]*) ThenH?R?C?$", RegexOptions.IgnoreCase).Groups(0)
                Return View("edit3_temple", oInf)
            End If
        End Function
        <SimpleRoleProvider(Roles.externo)>
        Function Edit3(idInforme As Integer) As ActionResult
            If TempData("ViewData") IsNot Nothing Then
                ViewData = CType(TempData("ViewData"), ViewDataDictionary)
            End If


            Dim oInf = dbES.INFORMES.Find(idInforme)
            Dim pInf = dbES.INFORME_PROVEEDOR.Find(idInforme)
            Dim lMarca = db.GetMarcasTodas(SimpleRoleProvider.GetId(), oInf.VALOROF, oInf.VALOROP, pInf.NUMPEDCAB, ConfigurationManager.ConnectionStrings("oracle").ConnectionString).Where(Function(m) oInf.MARCA.Split("|").Contains(m.marca))
            ViewData("marca") = lMarca
            ViewData("comunesMarca") = lMarca.First
            ViewData("numpedlin") = pInf.NUMPEDCAB
            If oInf.TIPOINFORME = "soldadura" Then
                ViewData("lsttiposoldaduraduro") = dbES.TIPOSOLDADURA.Select(Function(s) New Mvc.SelectListItem() With {.Value = s.TIPO, .Text = s.TIPO})
                ViewData("lstmaterialaportacionsoldduro") = New SelectList(dbES.VALORESSOLDADURA.Where(Function(vs) vs.TIPOSOLDADURA = oInf.TIPOSOLDADURADURO And vs.OPCION = "Duro" And vs.TIPO = "Material"), "valor", "valor")
                ViewData("lstvarillasoldaduraduro") = New SelectList(dbES.VALORESSOLDADURA.Where(Function(vs) vs.TIPOSOLDADURA = oInf.TIPOSOLDADURADURO And vs.OPCION = "Duro" And vs.TIPO = "Varilla"), "valor", "valor", oInf.VARILLASOLDADURADURO)
                ViewData("lsttiposoldadurablando") = dbES.TIPOSOLDADURA.Select(Function(s) New Mvc.SelectListItem() With {.Value = s.TIPO, .Text = s.TIPO})
                ViewData("lstmaterialaportacionsoldblando") = New SelectList(dbES.VALORESSOLDADURA.Where(Function(vs) vs.TIPOSOLDADURA = oInf.TIPOSOLDADURABLANDO And vs.OPCION = "Blando" And vs.TIPO = "Material"), "valor", "valor")
                ViewData("lstvarillasoldadurablando") = New SelectList(dbES.VALORESSOLDADURA.Where(Function(vs) vs.TIPOSOLDADURA = oInf.TIPOSOLDADURABLANDO And vs.OPCION = "Blando" And vs.TIPO = "Varilla"), "valor", "valor", oInf.VARILLASOLDADURABLANDO)
                ViewData("lstintensidadsoldaduraduro") = New SelectList(dbES.VALORESSOLDADURA.Where(Function(vs) vs.TIPOSOLDADURA = oInf.TIPOSOLDADURADURO And vs.OPCION = "Duro" And vs.TIPO = "Intensidad"), "valor", "valor", oInf.INTENSIDADSOLDADURADURO)
                ViewData("lstintensidadsoldadurablando") = New SelectList(dbES.VALORESSOLDADURA.Where(Function(vs) vs.TIPOSOLDADURA = oInf.TIPOSOLDADURABLANDO And vs.OPCION = "Blando" And vs.TIPO = "Intensidad"), "valor", "valor", oInf.INTENSIDADSOLDADURABLANDO)
                Return View("edit3_soldadura", oInf)
            ElseIf oInf.TIPOINFORME.ToLower.Contains("temple total") OrElse oInf.TIPOINFORME.ToLower.Contains("fundicion") OrElse oInf.TIPOINFORME.ToLower.Contains("soldadura laser") OrElse oInf.TIPOINFORME.ToLower.Contains("tratamiento secundario") Then
                ViewData("lstInformes") = dbES.CONTIENE.Where(Function(c) c.IDINFORME = idInforme)
                Return View("edit3_temple_total", oInf)
            Else
                ViewData("durezarequerida") = Regex.Match(If(ViewData("comunesMarca").TRATAMIENTO, ""), "([\d\-]*)H?R?C?$", RegexOptions.IgnoreCase).Groups(0)
                Return View("edit3_temple", oInf)
            End If
        End Function
        <SimpleRoleProvider(Roles.externo)>
        Function createEditSoldadura(inf As INFORMES, numpedlin As Integer) As ActionResult
            If ModelState.IsValid Then
                Dim fAction
                If inf.IDINFORME > 0 Then
                    'edit
                    dbES.Entry(inf).State = System.Data.Entity.EntityState.Modified
                    dbES.Entry(inf).Property(Function(x) x.CREADOPOR).IsModified = False
                    dbES.Entry(inf).Property(Function(x) x.FECHAINFORME).IsModified = False
                    fAction = RedirectToAction("index")
                Else
                    'create
                    inf.ESTADO = "Definitivo"
                    inf.CREADOPOR = db.getNombreProveedor(SimpleRoleProvider.GetId(), strCn)
                    inf.FECHAINFORME = Now
                    inf.IDINFORME = dbES.INFORMES.Max(Function(i) i.IDINFORME) + 1
                    dbES.INFORMES.Add(inf)
                    dbES.INFORME_PROVEEDOR.Add(New INFORME_PROVEEDOR With {.ID_INFORME = inf.IDINFORME, .ID_SAB = SimpleRoleProvider.GetId(), .NUMPEDCAB = numpedlin})
                    fAction = RedirectToAction("EditImages", New With {.id = inf.IDINFORME})
                End If
                dbES.SaveChanges()
                Return fAction
            End If
            If inf.IDINFORME > 0 Then
                Return Edit3(inf.IDINFORME)
            Else
                Return Create3(inf.TIPOINFORME, inf.VALOROF, inf.VALOROP, numpedlin, inf.MARCA.Split(",").Select(Function(ma) New marca With {.marca = ma, .tratamiento = inf.TRATAMIENTO, .dureza = inf.DUREZA, .tratamientoSecundario = inf.TRATAMSEC,
                                                                                                          .material = inf.MATERIAL}))
            End If
        End Function
        <SimpleRoleProvider(Roles.externo)>
        <HttpPost()>
        Function AttachTempletotal(inf As INFORMES, numpedlin As Integer, file As HttpPostedFileWrapper) As ActionResult
            If file Is Nothing Then
                ModelState.AddModelError("file", h.Traducir("Es necesario seleccionar cerfificado a adjuntar"))
            Else
                If System.IO.Path.GetFileName(file.FileName).Length > 100 Then
                    ModelState.AddModelError("file", h.Traducir("El nombre del archivo no puede superar los 100 caracteres"))
                End If
            End If
            Dim fileName = System.IO.Path.GetFileName(file.FileName)
            If Not fileName.ToUpper.EndsWith(".PDF") Then
                ModelState.AddModelError("file", "Debe ser un PDF")
                TempData("ViewData") = ViewData
                Return RedirectToAction("Edit3", New With {.idinforme = inf.IDINFORME})
            End If
            If ModelState.IsValid Then
                Using bR = New IO.BinaryReader(file.InputStream)
                    Dim Img = bR.ReadBytes(file.ContentLength)
                    If inf.IDINFORME > 0 Then
                        'Editar
                        Dim nuevaId = dbES.CERTIFICADO.Select(Function(f) f.IDCERTIFICADO).Max() + 1
                        dbES.CONTIENE.Add(New CONTIENE With {.IDINFORME = inf.IDINFORME, .IDCERTIFICADO = nuevaId})
                        dbES.CERTIFICADO.Add(New CERTIFICADO With {.IDCERTIFICADO = nuevaId, .FECHACERTIFICADO = Now, .NOMBRECERTF = fileName, .CERTIFICADO1 = Img})
                        dbES.SaveChanges()
                    Else
                        inf.ESTADO = "Definitivo"
                        inf.CREADOPOR = db.getNombreProveedor(SimpleRoleProvider.GetId(), strCn)
                        inf.FECHAINFORME = Now
                        inf.IDINFORME = dbES.INFORMES.Max(Function(i) i.IDINFORME) + 1
                        dbES.INFORMES.Add(inf)
                        Dim nuevaId = dbES.CERTIFICADO.Select(Function(f) f.IDCERTIFICADO).Max() + 1
                        dbES.CONTIENE.Add(New CONTIENE With {.IDINFORME = inf.IDINFORME, .IDCERTIFICADO = nuevaId})
                        dbES.CERTIFICADO.Add(New CERTIFICADO With {.IDCERTIFICADO = nuevaId, .FECHACERTIFICADO = Now, .NOMBRECERTF = fileName, .CERTIFICADO1 = Img})
                        dbES.INFORME_PROVEEDOR.Add(New INFORME_PROVEEDOR With {.ID_INFORME = inf.IDINFORME, .ID_SAB = SimpleRoleProvider.GetId(), .NUMPEDCAB = numpedlin})
                        dbES.SaveChanges()
                    End If
                End Using
                Return RedirectToAction("Edit3", New With {.idinforme = inf.IDINFORME})
            End If
            Return Create3(inf.TIPOINFORME, inf.VALOROF, inf.VALOROP, numpedlin, inf.MARCA.Split(",").Select(Function(ma) New marca With {.marca = ma, .tratamiento = inf.TRATAMIENTO, .dureza = inf.DUREZA, .tratamientoSecundario = inf.TRATAMSEC,
                                                                                                          .material = inf.MATERIAL}))
        End Function
        <SimpleRoleProvider(Roles.externo, Roles.interno)>
        Function displayAttachmentTempleTotal(idInforme As Integer, idcertificado As Integer) As FileResult
            Return New FileContentResult(dbES.CERTIFICADO.First(Function(f) f.IDCERTIFICADO = idcertificado).CERTIFICADO1, "application/pdf")
        End Function
        <SimpleRoleProvider(Roles.externo)>
        Function deleteAttachmentTempleTotal(idInforme As Integer, idCertificado As Integer) As ActionResult
            Return View(dbES.CERTIFICADO.First(Function(f) f.IDCERTIFICADO = idCertificado))
        End Function
        <SimpleRoleProvider(Roles.externo)>
        <HttpPost()>
        Function deleteAttachmentTempleTotal(idInforme As Integer, idCertificado As Integer, confirmation As String) As ActionResult
            If ModelState.IsValid Then
                Dim ph = dbES.CERTIFICADO.First(Function(f) f.IDCERTIFICADO = idCertificado)
                Dim ab = dbES.CONTIENE.First(Function(a) a.IDCERTIFICADO = idCertificado)
                dbES.CERTIFICADO.Remove(ph)
                dbES.CONTIENE.Remove(ab)
                dbES.SaveChanges()
                Return RedirectToAction("Edit3", New With {.idinforme = idInforme})
            End If
            Return View()
        End Function

        Function getValoresSoldadura(tipoSoldadura As String, opcion As String, tipo As String) As JsonResult
            Dim lst = dbES.VALORESSOLDADURA.Where(Function(vs) vs.TIPOSOLDADURA = tipoSoldadura And vs.OPCION = opcion And vs.TIPO = tipo)
            Return Json(Enumerable.Concat(New List(Of VALORESSOLDADURA) From {New VALORESSOLDADURA With {.VALOR = ""}}, lst), JsonRequestBehavior.AllowGet)
        End Function

        <SimpleRoleProvider(Roles.externo)>
        Function EditImages(id As Integer) As ActionResult
            Return View("EditImages", dbES.ABARCA.Where(Function(a) a.IDINFORME = id).Join(dbES.FOTOS, Function(a As ABARCA) a.IDFOTO, Function(f As FOTOS) f.IDFOTO, Function(a, f) f))
        End Function
        <SimpleRoleProvider(Roles.externo)>
        <HttpPost()>
        Function EditImages(id As Integer, file As HttpPostedFileBase)
            If file Is Nothing OrElse file.ContentLength = 0 Then
                ModelState.AddModelError("file", h.traducir("Es necesario seleccionar imagenes"))
            Else
                If dbES.ABARCA.Where(Function(a) a.IDINFORME = id).Count >= 4 Then
                    ModelState.AddModelError("file", h.traducir("El número máximo de imagenes adjuntables es 4"))
                End If
            End If
            If ModelState.IsValid Then
                Dim a = (From s In dbES.FOTOS Select s.IDFOTO).Max()
                Dim nuevaId = dbES.FOTOS.Select(Function(f) f.IDFOTO).Max() + 1
                Dim foto As New FOTOS With {.IDFOTO = nuevaId, .FECHAIMAGEN = Now}
                Try
                    foto.IMAGEN = scale(file.InputStream, 572, 428)
                Catch ex As Exception
                    ModelState.AddModelError("file", h.traducir("Error al intentar guardar la imagen. ¿Es una imagen JPG, GIF o PNG? Recuerda que no se pueden adjuntar pdfs"))
                    Return EditImages(id)
                End Try
                dbES.ABARCA.Add(New ABARCA With {.IDFOTO = nuevaId, .IDINFORME = id})
                dbES.FOTOS.Add(foto)

                dbES.SaveChanges()
                Return RedirectToAction("EditImages", h.ToRouteValues(Request.QueryString, Nothing))
            End If
            Return EditImages(id)
        End Function
        <SimpleRoleProvider(Roles.externo)>
        Function deleteImage(id As Integer, idPhoto As Integer) As ActionResult
            Return View(dbES.FOTOS.First(Function(f) f.IDFOTO = idPhoto))
        End Function
        <SimpleRoleProvider(Roles.externo)>
        <HttpPost()>
        Function deleteImage(id As Integer, idphoto As Integer, confirmation As String) As ActionResult
            If ModelState.IsValid Then
                Dim ph = dbES.FOTOS.First(Function(f) f.IDFOTO = idphoto)
                Dim ab = dbES.ABARCA.First(Function(a) a.IDFOTO = idphoto)
                dbES.FOTOS.Remove(ph)
                dbES.ABARCA.Remove(ab)
                dbES.SaveChanges()
                Return RedirectToAction("Editimages", h.ToRouteValuesDelete(Request.QueryString, "idphoto"))
            End If
            Return View()
        End Function
        <SimpleRoleProvider(Roles.externo, Roles.interno)>
        Function photo(idphoto As Integer) As Mvc.FileContentResult
            Return New FileContentResult(dbES.FOTOS.First(Function(f) f.IDFOTO = idphoto).IMAGEN, "image/jpeg")
        End Function
        <SimpleRoleProvider(Roles.externo)>
        Function Delete(ByVal idInforme As Decimal) As ActionResult
            If IsNothing(idInforme) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim iNFORMES As INFORMES = dbES.INFORMES.Find(idInforme)
            If IsNothing(iNFORMES) Then
                Return HttpNotFound()
            End If
            Dim pInf = dbES.INFORME_PROVEEDOR.Find(idInforme)
            Dim lMarca = db.GetMarcasTodas(SimpleRoleProvider.GetId(), iNFORMES.VALOROF, iNFORMES.VALOROP, pInf.NUMPEDCAB, ConfigurationManager.ConnectionStrings("oracle").ConnectionString).Where(Function(m) iNFORMES.MARCA.Split("|").Contains(m.marca))
            ViewData("marca") = lMarca
            ViewData("comunesMarca") = lMarca.First
            Return View(iNFORMES)
        End Function
        <SimpleRoleProvider(Roles.externo)>
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal idInforme As Decimal) As ActionResult
            Dim iNFORMES As INFORMES = dbES.INFORMES.Find(idInforme)
            Dim iProveedor As INFORME_PROVEEDOR = dbES.INFORME_PROVEEDOR.First(Function(ip) ip.ID_INFORME = idInforme)
            Dim abarcas = dbES.ABARCA.Where(Function(i) i.IDINFORME = idInforme)
            Dim contieneCerts = dbES.CONTIENE.Where(Function(i) i.IDINFORME = idInforme)

            For Each a In abarcas
                dbES.FOTOS.Remove(dbES.FOTOS.Find(a.IDFOTO))
                dbES.ABARCA.Remove(a)
            Next
            For Each c In contieneCerts
                dbES.CERTIFICADO.Remove(dbES.CERTIFICADO.Find(c.IDCERTIFICADO))
                dbES.CONTIENE.Remove(c)
            Next
            dbES.INFORME_PROVEEDOR.Remove(iProveedor)
            dbES.INFORME_PROVEEDOR.Remove(iProveedor)
            dbES.INFORMES.Remove(iNFORMES)
            dbES.SaveChanges()
            Return RedirectToAction("Index")
        End Function
        <SimpleRoleProvider(Roles.externo, Roles.interno)>
        Function InformePDF(id As Integer) As FileStreamResult
            'Get data
            Dim oInf = dbES.INFORMES.Find(id)
            Dim lstAdjuntos = dbES.CONTIENE.Where(Function(a) a.IDINFORME = id)
            If lstAdjuntos.Count = 1 Then
                Return New FileStreamResult(New IO.MemoryStream(dbES.CERTIFICADO.FirstOrDefault(Function(a) a.IDCERTIFICADO = lstAdjuntos.FirstOrDefault.IDCERTIFICADO).CERTIFICADO1), "application/pdf")
            ElseIf lstAdjuntos.Count = 0 Then
                Dim lstA = dbES.ABARCA.Where(Function(a) a.IDINFORME = id)
                Return GenerarInformePdf(oInf, lstA, Me)
            Else
                Dim lstM As New List(Of IO.MemoryStream)
                For Each i In lstAdjuntos
                    lstM.Add(New IO.MemoryStream(dbES.CERTIFICADO.FirstOrDefault(Function(a) a.IDCERTIFICADO = i.IDCERTIFICADO).CERTIFICADO1))
                Next
                Return New FileStreamResult(mergePdfs(lstM), "application/pdf")
                'Throw New NotImplementedException("El informe " + id.ToString + " tiene varios adjuntos.")
            End If
        End Function
        Public Shared Function mergePdfs(lPdf As IEnumerable(Of System.IO.Stream)) As System.IO.MemoryStream
            Dim doc As Document = New Document(PageSize.A4)
            Dim ms = New System.IO.MemoryStream
            Dim copy As pdf.PdfCopy = New pdf.PdfCopy(doc, ms)
            copy.CloseStream = False
            doc.Open()
            For Each m In lPdf
                'm.Position = 0
                copy.AddDocument(New pdf.PdfReader(m))
                m.Dispose()
            Next
            copy.Close()
            ms.Position = 0
            Return ms
        End Function
        <SimpleRoleProvider(Roles.externo)>
        Function TransferToExtranet() As ActionResult
            Dim sessionId = Session.SessionID
            db.setTicket(sessionId, User.Identity.Name, strCn)
            FormsAuthentication.SignOut()
            Session.Abandon()
            Return Redirect("/extranetlogin/?IdSession=" + sessionId)
        End Function
        <SimpleRoleProvider(Roles.validador)>
        <HttpPost()>
        Function validarInforme(id As Integer) As ActionResult
            If ModelState.IsValid Then
                Dim ip = dbES.INFORME_PROVEEDOR.Find(id)
                ip.VALIDADO = True
                dbES.Entry(ip).State = System.Data.Entity.EntityState.Modified



                dbES.SaveChanges()
                marcarPedidoOKCompensar(ip)
            End If
            Return RedirectToAction("index", h.ToRouteValuesDelete(Request.QueryString, "id"))
        End Function
        Private Sub marcarPedidoOKCompensar(Ip As INFORME_PROVEEDOR)
            Dim i = dbES.INFORMES.Find(Ip.ID_INFORME)
            Dim lstM = i.MARCA.Split("|")
            For Each m In lstM
                'Dim o = dbES.GCLINPED.First(Function(gcl) gcl.NUMORDF = i.VALOROF And gcl.NUMOPE = i.VALOROP And gcl.NUMMAR.Trim = m.Trim And gcl.NUMPEDLIN = Ip.NUMPEDCAB)
                'Select Case o.ID_ESTADO
                '    Case 17
                '        o.ID_ESTADO = 1
                '    Case 18
                '        o.ID_ESTADO = 14
                '    Case 19
                '        o.ID_ESTADO = 9
                '    Case 20
                '        o.ID_ESTADO = 13
                'End Select
                'dbES.Entry(o).State = EntityState.Modified
                'dbES.SaveChanges()
                Dim oList As List(Of GCLINPED) = dbES.GCLINPED.Where(Function(gcl) gcl.NUMORDF = i.VALOROF And gcl.NUMOPE = i.VALOROP And gcl.NUMMAR.Trim = m.Trim And gcl.NUMPEDLIN = Ip.NUMPEDCAB).ToList()

                'Dim oList = dbES.GCLINPED.Select(Function(gcl) gcl.NUMORDF = i.VALOROF And gcl.NUMOPE = i.VALOROP And gcl.NUMMAR.Trim = m.Trim And gcl.NUMPEDLIN = Ip.NUMPEDCAB)
                For Each o As GCLINPED In oList
                    Select Case o.ID_ESTADO
                        Case 17
                            o.ID_ESTADO = 1
                        Case 18
                            o.ID_ESTADO = 14
                        Case 19
                            o.ID_ESTADO = 9
                        Case 20
                            o.ID_ESTADO = 13
                    End Select
                    dbES.Entry(o).State = EntityState.Modified
                    dbES.SaveChanges()
                Next
            Next
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                dbES.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Public Function Scale(str As IO.Stream, wPixels As Integer, hPixels As Integer) As Byte()
            Dim fullsizeImage As Drawing.Image = Drawing.Image.FromStream(str)
            ' wPixels = fullsizeImage.Width
            ' hPixels = fullsizeImage.Height
            Dim destRect As Drawing.Rectangle = New Drawing.Rectangle(0, 0, wPixels, hPixels)
            Dim destImage = New Drawing.Bitmap(wPixels, hPixels)
            destImage.SetResolution(fullsizeImage.HorizontalResolution, fullsizeImage.VerticalResolution)

            Using graphics = Drawing.Graphics.FromImage(destImage)
                graphics.SmoothingMode = Drawing.Drawing2D.SmoothingMode.HighQuality
                graphics.InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                graphics.PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.HighQuality
                graphics.CompositingQuality = Drawing.Drawing2D.CompositingQuality.HighQuality
                Using wrapMode = New Drawing.Imaging.ImageAttributes()
                    wrapMode.SetWrapMode(Drawing.Drawing2D.WrapMode.TileFlipXY)
                    graphics.DrawImage(fullsizeImage, destRect, 0, 0, fullsizeImage.Width, fullsizeImage.Height, Drawing.GraphicsUnit.Pixel, wrapMode)

                End Using
            End Using

            Dim encoderParameters = New System.Drawing.Imaging.EncoderParameters(1)
            encoderParameters.Param(0) = New System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90)

            ' Dim newImage As Drawing.Image = fullsizeImage.GetThumbnailImage(wPixels, hPixels, Nothing, IntPtr.Zero)
            Dim bt() As Byte
            Using rStrm As IO.MemoryStream = New IO.MemoryStream()
                destImage.Save(rStrm, GetImageCodeInfo("image/jpeg"), encoderParameters)
                bt = rStrm.ToArray()
            End Using
            Return bt
        End Function

        Public Function GetImageCodeInfo(mimeType As String) As Drawing.Imaging.ImageCodecInfo
            Dim info = Drawing.Imaging.ImageCodecInfo.GetImageEncoders()
            For Each ici In info
                If ici.MimeType.Equals(mimeType, StringComparison.OrdinalIgnoreCase) Then
                    Return ici
                End If
            Next
            Return Nothing
        End Function

        Public Shared Function RenderViewToString(viewName As String, viewDataModel As Object, c As Controller) As String
            Using writer = New IO.StringWriter()
                Dim viewResult = ViewEngines.Engines.FindView(c.ControllerContext, viewName, "")
                Dim viewContext = New ViewContext(c.ControllerContext, viewResult.View, c.ViewData, c.TempData, writer)
                c.ViewData.Model = viewDataModel
                viewResult.View.Render(viewContext, writer)
                Return writer.GetStringBuilder().ToString()
            End Using
        End Function


        Private Function GenerarInformePdf(oinf As INFORMES, lstA As IQueryable(Of ABARCA), ctrll As Controller) As FileStreamResult
            Dim doc As iTextSharp.text.Document = New iTextSharp.text.Document(iTextSharp.text.PageSize.A4)
            Dim strMS As New System.IO.MemoryStream()
            Dim wrtTest = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, strMS)
            wrtTest.CloseStream = False
            doc.Open()
            Dim example_html = RenderViewToString("../INFORME/viewPDF", New With {.informe = oinf, .imagenes = lstA}, ctrll)
            Using msHtml = New IO.MemoryStream(Encoding.UTF8.GetBytes(example_html))
                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(wrtTest, doc, msHtml, Encoding.UTF8)
            End Using

            Dim tImg = New iTextSharp.text.pdf.PdfPTable(2) : tImg.DefaultCell.Border = 0
            For Each img In lstA
                Dim c = New iTextSharp.text.pdf.PdfPCell : c.Border = 0

                Dim i = iTextSharp.text.Image.GetInstance(Scale(New IO.MemoryStream(photo(img.IDFOTO).FileContents), 572, 428))
                i.ScalePercent(30)
                c.AddElement(i)
                tImg.AddCell(c)
            Next
            tImg.CompleteRow()
            doc.Add(tImg)

            Dim tBottom = New iTextSharp.text.pdf.PdfPTable(1) : tBottom.WidthPercentage = 100
            tBottom.ExtendLastRow = True
            Dim cBottom = New iTextSharp.text.pdf.PdfPCell
            cBottom.VerticalAlignment = iTextSharp.text.pdf.PdfPCell.ALIGN_BOTTOM : cBottom.Border = 0
            Dim tCreador As New iTextSharp.text.pdf.PdfPTable(2) : tCreador.WidthPercentage = 100
            Dim cCreador1, cCreador2, cCreador3, cCreador4 As New iTextSharp.text.pdf.PdfPCell
            cCreador1.AddElement(New iTextSharp.text.Chunk("Templado y Verificado Por/ Welding by:")) : cCreador1.BackgroundColor = New iTextSharp.text.BaseColor(173, 215, 247)
            cCreador2.AddElement(New iTextSharp.text.Chunk(oinf.CREADOPOR)) : cCreador2.BackgroundColor = New iTextSharp.text.BaseColor(173, 215, 247)
            cCreador3.AddElement(New iTextSharp.text.Chunk("Fecha/Date:")) : cCreador3.BackgroundColor = New iTextSharp.text.BaseColor(173, 215, 247)
            cCreador4.AddElement(New iTextSharp.text.Chunk(oinf.FECHAINFORME)) : cCreador4.BackgroundColor = New iTextSharp.text.BaseColor(173, 215, 247)
            tCreador.AddCell(cCreador1) : tCreador.AddCell(cCreador2) : tCreador.AddCell(cCreador3) : tCreador.AddCell(cCreador4)
            cBottom.AddElement(tCreador)
            tBottom.AddCell(cBottom)

            doc.Add(tBottom)
            doc.Close()
            strMS.Seek(0, System.IO.SeekOrigin.Begin)
            Return New FileStreamResult(strMS, "application/pdf") 'FileContentResult(strMS.ToArray, "application/pdf")
        End Function
    End Class
End Namespace
