Imports System.Web.Mvc

Namespace Controllers
    Public Class CorporativoController
        Inherits Controller

        ReadOnly strCnOracle As String = ConfigurationManager.ConnectionStrings("oracle").ConnectionString
        Public Const TakeLimit As Integer = 30
        Public Const skipMin As Integer = 0

        Function Index(q As String, skip As Integer?, take As Integer?) As ActionResult
            skip = If(skip, skipMin)
            take = If(take, TakeLimit)
            Dim lst = Db.GetListOfProveedorCorporativo(strCnOracle)
            If Not String.IsNullOrEmpty(q) Then
                lst = lst.Where(Function(o) h.searchFilter(q, o.cif, o.nombre, o.localidad, o.provincia))
            End If
            Return View("Index", lst.Skip(skip).Take(take))
        End Function
        Function Create() As ActionResult
            Return View("edit")
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function Create(ep As EmpresaCorporativa) As ActionResult
            If ModelState.IsValid Then
                Db.InsertCorporativa(ep, strCnOracle)
                Return RedirectToAction("index", h.ToRouteValues(Request.QueryString, Nothing))
            End If
            Return View("edit")
        End Function
        Function Edit(id As Integer, q As String) As ActionResult
            Dim pc = Db.GetListOfProveedorCorporativo(strCnOracle).First(Function(e) e.id = id)
            If Not String.IsNullOrEmpty(q) Then
                Dim lstEmpresas = Db.GetListOfProveedorPorPlantasSinCorporativo(strCnOracle)
                ViewData("lstEmpresasSinCorporativo") = lstEmpresas.
                    Where(Function(o) Not CType(pc.lstEmpresas, IEnumerable(Of Object)).Any(Function(e) e.idplantaEmpresa = o.idPlanta)).
                    Where(Function(o) h.searchFilter(q, o.cif, o.nombre, o.localidad, o.provincia)).
                    GroupBy(Function(o) New With {Key .idPlanta = o.idPlanta, Key .nombrePlanta = o.nombrePlanta})
            End If

            ViewData("lstUsuarios") = Db.GetListOfUsuariosCorporativo(pc.id, strCnOracle).Select(Function(u) New SelectListItem() With {.Value = u.id, .Text = u.nombreUsuario, .Selected = pc IsNot Nothing AndAlso pc.idUsuarioAdministrador = u.id})
            Return View("edit", pc)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function Edit(ep As EmpresaCorporativa, q As String) As ActionResult
            Dim pc = Db.GetListOfProveedorCorporativo(strCnOracle).First(Function(e) e.id = ep.Id)
            If ModelState.IsValid Then
                Db.UpdateEmpresaCorporativa(ep, strCnOracle)
                Return RedirectToAction("edit", h.ToRouteValues(Request.QueryString, Nothing))
            End If
            Return View("edit", pc)
        End Function

        <AcceptVerbs(HttpVerbs.Post)>
        Function UsuarioAdministradorSet(id As Integer, idUsuario As Integer) As JsonResult
            If ModelState.IsValid Then
                Db.SetUsuarioAdministradorYRecursoSABProveedor(id, idUsuario, ConfigurationManager.AppSettings("sabprovgrupo"), strCnOracle)
            End If
            Return Json(New With {.success = True})
        End Function

        Function Delete(id As Integer) As ActionResult
            Return View(Db.GetListOfProveedorCorporativo(strCnOracle).First(Function(e) e.id = id))
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        <ActionName("delete")>
        Function Delet2Confirmation(id As Integer) As ActionResult
            If ModelState.IsValid Then
                Db.DeleteEmpresaCorporativa(id, strCnOracle)
                Return RedirectToAction("index")
            End If
            Return View(Db.GetListOfProveedorCorporativo(strCnOracle).First(Function(e) e.id = id))
        End Function

        <AcceptVerbs(HttpVerbs.Post)>
        Function RemoveEmpresa(id As Integer, idEmpresa As Integer) As ActionResult
            If ModelState.IsValid Then
                Db.DeleteEmpresaCorporativa(id, idEmpresa, strCnOracle)
                Return RedirectToAction("edit", h.ToRouteValues(Request.QueryString, Nothing))
            End If
            Return Edit(id, Nothing)
        End Function

        Function AddEmpresa(id As Integer, idEmpresa As Integer) As ActionResult
            If ModelState.IsValid Then
                Db.InsertEmpresaCorporativa(id, idEmpresa, strCnOracle)
                Return RedirectToAction("edit", h.ToRouteValues(Request.QueryString, Nothing))
            End If
            Return Edit(id, Nothing)
        End Function

        Function BusquedaConsulta(q As String)
            Dim lst = Db.GetListOfProveedorCorporativo(strCnOracle)
            If Not String.IsNullOrEmpty(q) Then
                Return View(Db.GetListOfProveedorPorPlantasSinCorporativo(strCnOracle).
                    Where(Function(o) h.searchFilter(q, o.cif, o.nombre, o.localidad, o.provincia)).
                    GroupBy(Function(o) New With {Key .idPlanta = o.idPlanta, Key .nombrePlanta = o.nombrePlanta}))
            End If
            Return View()
        End Function
    End Class
End Namespace