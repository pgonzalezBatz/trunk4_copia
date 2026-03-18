Imports System.Security.Permissions
Namespace web
    Public Class adminController
        Inherits System.Web.Mvc.Controller
        Private Const Empresa = 1
        Private strcn As String = ConfigurationManager.ConnectionStrings("gestionhoras").ConnectionString
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function Index() As ActionResult
            Return View(db.GetListOfTipoDistribucion(Empresa, strcn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function Edit(ByVal id As Integer) As ActionResult
            ViewData("tipos") = db.GetListOfTipos()
            Return View(db.GetTipoDistribucion(Empresa, id, strcn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Edit(ByVal id As Integer, ByVal tp As TipoDistribucion) As ActionResult
            If ModelState.IsValid Then
                db.UpdateTipoDistribucion(tp, strcn)
                Return RedirectToAction("index")
            End If
            ViewData("tipos") = db.GetListOfTipos()
            Return View(tp)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function Add() As ActionResult
            ViewData("tipos") = db.GetListOfTipos()
            Dim tp = New TipoDistribucion With {.IdEmpresa = 1, .Id = 1}
            Return View("edit", tp)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Add(ByVal tp As TipoDistribucion) As ActionResult
            If ModelState.IsValid Then
                db.AddTipoDistribucion(tp, strcn)
                Return RedirectToAction("index")
            End If
            ViewData("tipos") = db.GetListOfTipos()
            Return View("edit")
        End Function
    End Class
End Namespace