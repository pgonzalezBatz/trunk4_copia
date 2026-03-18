Imports System.Security.Permissions
Namespace efa
    Public Class reasignController
        Inherits System.Web.Mvc.Controller
        Private strCnEfa As String = ConfigurationManager.ConnectionStrings("BaliabideF").ConnectionString
        Private strCnTelef As String = ConfigurationManager.ConnectionStrings("Telefonia").ConnectionString
        Private strCnSab As String = ConfigurationManager.ConnectionStrings("SAB").ConnectionString
        Private strCnEpsilon As String = ConfigurationManager.ConnectionStrings("Epsilon").ConnectionString

        <SimpleRoleProvider(EfaRole.jefegrupo)>
        Function Index() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(EfaRole.jefegrupo)>
        Function listreasignacion() As ActionResult
            Dim lst = DB.getTrabajadoresRecursosDeEncargado(SimpleRoleProvider.GetId(), strCnSab, strCnEpsilon, strCnTelef)
            lst.RemoveAll(Function(o) o.listOfRecurso Is Nothing)
            Return View(lst)
        End Function
        <SimpleRoleProvider(EfaRole.jefegrupo)>
        Function reasignar(id As String) As ActionResult
            Return View(DB.getTrabajadoresRecursosDeEncargado(SimpleRoleProvider.GetId(), strCnSab, strCnEpsilon, strCnTelef))
        End Function
        <SimpleRoleProvider(EfaRole.jefegrupo)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function reasignar(id As String, codpersona As Integer) As ActionResult
            If codpersona = 0 Then
                ModelState.AddModelError("codpersona", "Es obligatorio elegir una persona y un recurso")
            End If
            If ModelState.IsValid Then
                DB.ReasignarTelefono(codpersona, id, strCnTelef)
                Return RedirectToAction("listreasignacion")
            End If
            Return View(DB.getTrabajadoresRecursosDeEncargado(SimpleRoleProvider.GetId(), strCnSab, strCnEpsilon, strCnTelef))
        End Function
    End Class
End Namespace