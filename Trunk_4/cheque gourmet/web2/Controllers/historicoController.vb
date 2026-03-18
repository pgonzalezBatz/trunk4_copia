Imports System.Security.Permissions
Imports System.Linq
Namespace web2
    Public Class historicoController
        Inherits System.Web.Mvc.Controller
        Private Const Empresa = 1
        Private strcn As String = ConfigurationManager.ConnectionStrings("gestionhoras").ConnectionString

        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function Index() As ActionResult
            Dim diaCorte = db.GetDiaCorte(Empresa, strcn)
            Dim o = GetFechasLimite(Now, diaCorte)
            ViewData("diaCorte") = diaCorte.ToString
            Dim lst As List(Of Object) = db.GetTalonariosMesActual(SimpleRoleProvider.GetId(), o.f0, o.f1, strcn)
            ViewData("havedata") = True
            If lst.Count = 0 Then
                ViewData("havedata") = False
                Return View(lst)
            End If
            Return View(lst.GroupBy(Function(e) e.tipo))
        End Function

        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function list() As ActionResult
            Dim l = db.GetlistOfTalonarios(SimpleRoleProvider.GetId(), strcn)
            Return View(l.GroupBy(Function(el) GetFechasLimite(el.fecha, db.GetDiaCorte(Empresa, strcn)).f1, Function(K, V) New With {.key = K, .list = V, .count = V.Count}))
        End Function
        Private Shared Function GetFechasLimite(ByVal fecha As DateTime, ByVal diaCorte As Integer) As Object
            Dim f0, f1 As New Date
            If fecha.Day <= diaCorte Then
                Dim anterior = fecha.AddMonths(-1)
                f0 = New Date(anterior.Year, anterior.Month, diaCorte)
                f1 = New Date(fecha.Year, fecha.Month, diaCorte)
            Else
                Dim posterior = fecha.AddMonths(1)
                f0 = New Date(fecha.Year, fecha.Month, diaCorte)
                f1 = New Date(posterior.Year, posterior.Month, diaCorte)
            End If
            Return New With {.f0 = f0, .f1 = f1}
        End Function
    End Class
End Namespace