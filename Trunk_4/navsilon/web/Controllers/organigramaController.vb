Imports System.Security.Permissions
Namespace web
    Public Class organigramaController
        Inherits System.Web.Mvc.Controller

        Dim strCnEpsilon As String = ConfigurationManager.ConnectionStrings("epsilon").ConnectionString
        Dim strCn As String = ConfigurationManager.ConnectionStrings("navsilon").ConnectionString

        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function Index() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function listNegociosAdministracion() As ActionResult
            Dim lstMO = db.GetListOfManoObraAdministracion(strCn)
            Dim lst = db.GetListOfNegocioAdministracion(strCn).Select(
                Function(n)
                    Return New With {.idNegocio = n.id, .nombreNegocio = n.nombre, .lstMO = lstMO.Select(
                            Function(mo)
                                Return New With {.idMO = mo.id, .nombreMO = mo.nombre,
                                                 .lstDepartamento = db.GetListOfDepartamentosAdministracion(n.id, mo.id, strCn).Where(Function(dep) Not dep.obsoleto).Select(Function(dep)
                                                                                                                                                                                 Dim lstMapeos = db.GetMapeosEpsilonAdministracion(strCn).Where(Function(m) m.idAdministracion = dep.id And Not m.obsoleto)
                                                                                                                                                                                 Return New With {.id = dep.id, .nombre = dep.nombre, .lantegi = dep.lantegi,
                                                                                                                                                                                 .nPersonas = lstMapeos.Sum(Function(s) db.GetNumeroPersonas(s.idepsilon, strCnEpsilon))}
                                                                                                                                                                             End Function).ToList,
                                                                                                                                   .npersonas = .lstDepartamento.Sum(Function(d) d.npersonas), .ndepartamentos = .lstDepartamento.count}
                            End Function).ToList, .npersonas = .lstMO.Sum(Function(mo) mo.npersonas), .ndepartamentos = .lstMO.Sum(Function(mo) mo.ndepartamentos)}
                End Function)
            ViewData("npersonas") = lst.Sum(Function(mo) mo.npersonas)
            Return View(lst.ToList)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function listMOAdministracion(idNegocio As Integer) As ActionResult
            ViewData("negocio") = db.GetListOfNegocioAdministracion(strCn).Where(Function(n) n.id = idNegocio).ToArray.First
            Return View(db.GetListOfManoObraAdministracion(strCn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function listDepartamentosAdministracion(idNegocio As Integer, idManoObra As Integer) As ActionResult
            ViewData("negocio") = db.GetListOfNegocioAdministracion(strCn).Where(Function(n) n.id = idNegocio).ToArray.First
            ViewData("manoobra") = db.GetListOfManoObraAdministracion(strCn).Where(Function(mo) mo.id = idManoObra).ToArray.First
            Return View(db.GetListOfDepartamentosAdministracion(idNegocio, idManoObra, strCn).Where(Function(dep) Not dep.obsoleto))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function createNegocioAdministracion() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function createNegocioAdministracion(nombre As String) As ActionResult
            If nombre Is Nothing Then
                ModelState.AddModelError("nombre", "Es necesario introducir el nombre del negocio")
            End If
            If ModelState.IsValid Then
                db.AddNegocioAdministracion(nombre, strCn)
                Return RedirectToAction("listnegociosadministracion")
            End If
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function createManoObraAdministracion() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function createManoObraAdministracion(nombre As String) As ActionResult
            If nombre Is Nothing Then
                ModelState.AddModelError("nombre", "Es necesario introducir el nombre de la mano de obra")
            End If
            If ModelState.IsValid Then
                db.AddManoObraAdministracion(nombre, strCn)
                Return RedirectToAction("listMOadministracion")
            End If
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function createDepartamentoAdministracion(idNegocio As Integer, idManoObra As Integer) As ActionResult
            ViewData("negocio") = db.GetListOfNegocioAdministracion(strCn).Where(Function(n) n.id = idNegocio).ToArray.First
            ViewData("manoobra") = db.GetListOfManoObraAdministracion(strCn).Where(Function(mo) mo.id = idManoObra).ToArray.First
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function createDepartamentoAdministracion(idNegocio As Integer, idManoObra As Integer, nombre As String, lantegi As String) As ActionResult
            If nombre Is Nothing Then
                ModelState.AddModelError("nombre", "Es necesario introducir el nombre del departamento")
            End If
            If lantegi Is Nothing Then
                ModelState.AddModelError("lantegi", "Es necesario introducir el lantegi")
            End If
            If ModelState.IsValid Then
                db.AddDepartamentoAdministracion(idNegocio, idManoObra, nombre, lantegi, strCn)
                Return RedirectToAction("listnegociosadministracion")
            End If
            ViewData("negocio") = db.GetListOfNegocioAdministracion(strCn).Where(Function(n) n.id = idNegocio).ToArray.First
            ViewData("manoobra") = db.GetListOfManoObraAdministracion(strCn).Where(Function(mo) mo.id = idManoObra).ToArray.First
            Return View()
        End Function

        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function editDepartamentoAdministracion(idNegocio As Integer, idManoObra As Integer, iddepartamento As Integer) As ActionResult
            ViewData("negocio") = db.GetListOfNegocioAdministracion(strCn).Where(Function(n) n.id = idNegocio).ToArray.First
            ViewData("manoobra") = db.GetListOfManoObraAdministracion(strCn).Where(Function(mo) mo.id = idManoObra).ToArray.First
            Return View("createDepartamentoAdministracion", db.GetDepartamentosAdministracion(iddepartamento, strCn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function editDepartamentoAdministracion(idNegocio As Integer, idManoObra As Integer, idDepartamento As Integer, nombre As String, lantegi As String, obsoleto As String) As ActionResult
            If nombre Is Nothing Then
                ModelState.AddModelError("nombre", "Es necesario introducir el nombre del departamento")
            End If
            If lantegi Is Nothing Then
                ModelState.AddModelError("lantegi", "Es necesario introducir el lantegi")
            End If
            If ModelState.IsValid Then
                db.EditDepartamentoAdministracion(idNegocio, idManoObra, idDepartamento, nombre, lantegi, obsoleto = "true", strCn)
                Return RedirectToAction("listnegociosadministracion")
            End If
            ViewData("negocio") = db.GetListOfNegocioAdministracion(strCn).Where(Function(n) n.id = idNegocio).ToArray.First
            ViewData("manoobra") = db.GetListOfManoObraAdministracion(strCn).Where(Function(mo) mo.id = idManoObra).ToArray.First
            Return View("createDepartamentoAdministracion")
        End Function

        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function deleteDepartamento(idDepartamento As Integer) As ActionResult
            ViewData("mapeos") = db.GetMapeosEpsilonAdministracion(strCn).Where(Function(e) e.idAdministracion = idDepartamento).ToList
            Return View(db.GetDepartamentosAdministracion(idDepartamento, strCn))
        End Function
        
    End Class
End Namespace