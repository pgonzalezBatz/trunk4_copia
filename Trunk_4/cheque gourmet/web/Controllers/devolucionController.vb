Imports System.Security.Permissions
Namespace web
    Public Class devolucionController
        Inherits System.Web.Mvc.Controller

        Private Const Empresa = 1
        Private strcn As String = ConfigurationManager.ConnectionStrings("gestionhoras").ConnectionString
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function Index() As ActionResult
            Return View(db.getDevolucionesPendientesDeReparto(strcn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function Devolver() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Devolver(ByVal cheque As String, ByVal numerocheques As String) As ActionResult
            If String.IsNullOrEmpty(cheque) AndAlso cheque.Length > 9 Then
                ModelState.AddModelError("cheque", "Es necesario leer el codigo de barras de uno de los cheques")
            End If
            If String.IsNullOrEmpty(numerocheques) Then
                ModelState.AddModelError("numerocheques", "Es necesario introducir el numero de cheques devueltos")
            End If
            If ModelState.IsValid Then
                Return RedirectToAction("confirmar", New With {.cheque = cheque, .numerocheques = numerocheques})
            End If
            ViewData("cheque") = cheque
            ViewData("numerocheques") = numerocheques
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function Confirmar(ByVal cheque As String, ByVal numerocheques As String) As ActionResult
            If String.IsNullOrEmpty(cheque) AndAlso cheque.Length > 9 Then
                ModelState.AddModelError("cheque", "Es necesario leer el codigo de barras de uno de los cheques")
            End If
            If String.IsNullOrEmpty(numerocheques) Then
                ModelState.AddModelError("numerocheques", "Es necesario introducir el numero de cheques devueltos")
            End If
            If ModelState.IsValid Then
                Dim numero = cheque.Substring(0, 9)
                Dim talonario = db.GetTalonarioDesdeCheque(Empresa, numero, strcn)
                If talonario.count = 0 Then
                    Return RedirectToAction("talonarionoencontrado")
                End If
                ViewData("numerocheques") = numerocheques
                Return View("confirmar", talonario(0))
            End If
            Return RedirectToAction("devolver")
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function Guardar(ByVal codtra As Nullable(Of Integer), ByVal numerocheques As String, ByVal tipo As Integer) As ActionResult
            If Not codtra.HasValue Then
                ModelState.AddModelError("codtra", "Se ha producido algun error al intentar leer el código de trabajador")
            End If
            If ModelState.IsValid Then
                Dim t = db.DatosTrabajador(codtra.Value, strcn)
                db.SaveDevolucion(codtra.Value, numerocheques, tipo, strcn)
                Dim recip = t.email
                h2.SendEmail(recip, "Confirmación de devolución", "Le confirmamos que ha devuelto " + numerocheques.ToString + " cheques.")
            End If
            Return RedirectToAction("index")
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function TalonarioNoEncontrado() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function ConfirmarReparto(ByVal id As Integer) As ActionResult
            Return View(db.getDevolucion(id, strcn))
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function GuardarReparto(ByVal id As Integer) As ActionResult
            db.UpdateDevolucionRedistribuir(id, strcn)
            Return RedirectToAction("index")
        End Function
    End Class
End Namespace