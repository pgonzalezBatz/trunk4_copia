Imports System.Security.Permissions
Namespace web
    Public Class relacionesController
        Inherits System.Web.Mvc.Controller

        Dim strCnEpsilon As String = ConfigurationManager.ConnectionStrings("epsilon").ConnectionString
        Dim strCn As String = ConfigurationManager.ConnectionStrings("navsilon").ConnectionString

        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function list() As ActionResult
            ViewData("lstnegocio") = db.GetListOfNegocioAdministracion(strCn).Select(
                Function(n)
                    Return New With {.idNegocio = n.id, .nombreNegocio = n.nombre, .lstManoObra = db.GetListOfManoObraAdministracion(strCn).Select(
                            Function(mo)
                                Return New With {.idManoObra = mo.id, .nombreManoObra = mo.nombre, .lstDepartamento = db.GetListOfDepartamentosAdministracion(n.id, mo.id, strCn).Where(Function(dep) Not dep.obsoleto)}
                            End Function)}
                End Function)
            Dim lstMapeos = db.GetMapeosEpsilonAdministracion(strCn)
            Return View(db.GetListOfDepartamentosEpsilon(strCnEpsilon).Select(
                        Function(e)
                            Return New With {.id = e.id, .nombre = e.nombre, .relacion = CType(lstMapeos, List(Of Object)).Find(Function(l) l.idEpsilon = e.id)}
                        End Function).ToList)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function list(lstMapeo As List(Of String)) As ActionResult
            lstMapeo = lstMapeo.FindAll(Function(e) e <> "")
            If lstMapeo Is Nothing Then
                ModelState.AddModelError("lstMapeo", "Es necesario seleccionar algun elemento")
            End If
            If ModelState.IsValid Then
                db.UpdateMapeosEpsilonAdministracion(lstMapeo.Select(
                                                     Function(e)
                                                         Dim s = Split(e, ",")
                                                         Return New With {.idEpsilon = s(0), .idAdministracion = s(1)}
                                                     End Function).ToList(), strCn)
                Return RedirectToAction("list")
            End If
            Return list()
        End Function
    End Class
End Namespace