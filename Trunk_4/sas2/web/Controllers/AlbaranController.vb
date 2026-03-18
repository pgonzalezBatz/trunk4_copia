Imports System.Security.Permissions
Namespace web
    Public Class AlbaranController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString

        <SimpleRoleProvider(Role.creacion)>
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function List() As ActionResult
            Dim lst = DBAccess.GetListAlbaran(strCn).OrderBy(Function(m) m.ListOfAgrupacion.First.ListOfMovimiento.First.CodPro).ToList
            Dim empresasLocales = h.GetListOfDefaultEmpresaFromStrCn(strCn)
            ViewData("empresaUsuario") = h.GetListOfDefaultEmpresaFromStrCn(strCn)
            ViewData("listOfAlbaran") = lst
            Return View()
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Create(ByVal bultos As List(Of Integer)) As ActionResult
            Dim negocios As Integer = 0
            If bultos IsNot Nothing AndAlso bultos.Count > 0 Then
                Dim lst = DBAccess.GetListOfAgrupaciones(strCn).FindAll(Function(g) bultos.Exists(Function(b) b = g.Id))
                negocios = lst.Select(Function(o) o.IdNegocio).Distinct().Count()
                If negocios > 1 Then
                    ModelState.AddModelError("Negocio", "Los bultos no se pueden agrupar, pertenecen a diferentes negocios")
                    Return RedirectToAction("list", "agrupacion")
                End If
                If IsNumeric(Request("idhelbide")) Then
                    ViewData("direcionenvio") = DBAccess.GetHelbide(Request("idhelbide"), strCn)
                    ViewData("idHelbide") = Request("idhelbide")
                Else

                    ViewData("direcionenvio") = DBAccess.GetDireccionProveedor(lst.First(Function(m) m.ListOfMovimiento.Count > 0).ListOfMovimiento.First().CodPro, strCn)
                    ViewData("idproveedordestino") = lst.First(Function(m) m.ListOfMovimiento.Count > 0).ListOfMovimiento.First().CodPro
                End If
                Dim empresaSalida = h.GetListOfDefaultEmpresaFromStrCn(strCn)
                ViewData("idempresasalida") = empresaSalida.First.id
                ViewData("empresasalida") = empresaSalida.First.nombre
                Return View("Create", lst)
            End If
            Return RedirectToAction("list", "albaran")
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function SaveAlbaran(ByVal bultos As List(Of Integer), ByVal observacion As String, ByVal idHelbide As Nullable(Of Integer)) As ActionResult
            If Not DBAccess.UnicoProveedorEnBultos(bultos, strCn) Then
                ModelState.AddModelError("", "No se puede crear un albaran con movimientos de distintos proveedores")
                Return Create(bultos)
            End If
            If Not DBAccess.UnicoNegocioEnBultos(bultos, strCn) Then
                ModelState.AddModelError("", "No se puede crear un albaran con movimientos de distintos negocios")
                Return Create(bultos)
            End If
            If bultos IsNot Nothing AndAlso bultos.Count > 0 Then
                DBAccess.SaveAlbaran(observacion, idHelbide, bultos, strCn)
                Return RedirectToAction("list")
            End If
            Throw New Exception("El programa no esta preparado para esto")
        End Function
        <SimpleRoleProvider(Role.creacion)>
        Function AddToExistingAlbaran(ByVal bultos As List(Of Integer), ByVal albaran As Integer)
            If Not DBAccess.UnicoProveedorEnBultos(bultos, strCn) Then
                ModelState.AddModelError("", "No se puede crear un albaran con movimientos de distintos proveedores")
                Return Create(bultos)
            End If
            If Not DBAccess.UnicoNegocioEnBultos(bultos, strCn) Then
                ModelState.AddModelError("", "No se puede crear un albaran con movimientos de distintos negocios")
                Return Create(bultos)
            End If
            If Not DBAccess.MismoNegocioBultoAlbaran(bultos.FirstOrDefault(), albaran, strCn) Then
                ModelState.AddModelError("", "No se puede crear un albaran con movimientos de distintos negocios")
                Return Create(bultos)
            End If

            If bultos IsNot Nothing AndAlso bultos.Count > 0 Then
                DBAccess.AddGruposToAlbaran(albaran, bultos, strCn)
                Return RedirectToAction("list")
            End If
            Throw New Exception("El programa no esta preparado para esto")
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Helbide() As ActionResult
            ViewData("querystring") = Regex.Replace(Request.UrlReferrer.Query, "&idhelbide=\d*", "") + "&idhelbide="
            Return View()
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Edit(ByVal id As Integer) As ActionResult

            Return View(DBAccess.GetAlbaran(id, strCn))
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function RemoveMarca(id As Integer, ByVal movimiento As Integer) As ActionResult
            DBAccess.RemoveMovimientoFromGrupo(movimiento, strCn)
            Dim mc = Runtime.Caching.MemoryCache.Default
            mc.Remove("albaran" + id.ToString)
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function RemoveBulto(id As Integer, ByVal agrupacion As Integer) As ActionResult
            DBAccess.RemoveGrupoFromAlbaran(agrupacion, strCn)
            Dim mc = Runtime.Caching.MemoryCache.Default
            mc.Remove("albaran" + id.ToString)
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function RemoveAlbaran(ByVal albaran As Integer) As ActionResult
            DBAccess.RemoveAlbaran(albaran, strCn)
            Dim mc = Runtime.Caching.MemoryCache.Default
            mc.Remove("albaran" + albaran.ToString)
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.creacion)>
        Function AddBulto() As ActionResult
            Return View(DBAccess.GetListOfAgrupaciones(strCn))
        End Function
        <SimpleRoleProvider(Role.creacion)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function AddBulto(ByVal id As Integer, ByVal idGrupo As Integer) As ActionResult
            DBAccess.AddGrupo(id, idGrupo, strCn)
            Return RedirectToAction("list")
        End Function
        Function etiquetas(idAlbaran As Integer) As ActionResult
            Dim a = DBAccess.GetAlbaran(idAlbaran, strCn)
            For Each gr In a.ListOfAgrupacion
                Dim hel = DBAccess.GetDireccionAlbaran(a.Id, strCn)
                h.printLabel(New With {.proveedor = hel.nombreEmpresa, .calle = hel.calle, .codigoPostal = hel.codigopostal,
                                 .poblacion = hel.poblacion, .provincia = hel.provincia, .telefono = hel.telefono, .peso = gr.Peso, .idBulto = gr.Id})
            Next

            Return RedirectToAction("list")
        End Function
        Function printAlbaran(idAlbaran As Integer) As ActionResult
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=viaje_" + idAlbaran.ToString + ".pdf")
            Return New FileStreamResult(h.albaranSimple(idAlbaran), "application/pdf")
        End Function
    End Class
End Namespace