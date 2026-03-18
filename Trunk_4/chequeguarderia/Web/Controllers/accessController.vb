Imports System.Security.Permissions
Namespace web
    Public Class AccessController
        Inherits System.Web.Mvc.Controller

        Private ReadOnly StrCn As String = ConfigurationManager.ConnectionStrings("SAB").ConnectionString
        Function Index() As ActionResult
            Select Case SimpleRoleProvider.GetRole
                Case Role.administracion
                    Return RedirectToAction("index", "admin")
                Case Role.normal
                    Return RedirectToAction("index", "alta")
                Case Else
                    Return Redirect("~/accesodenegado.htm")
            End Select
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function CerrarSession() As ActionResult
            FormsAuthentication.SignOut()
            Return Redirect("//intranet2.batz.es")
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)>
        Function Volverportal() As ActionResult
            Dim sessionId = Session.SessionID
            DB.setTicket(sessionId, SimpleRoleProvider.GetId(), strCn)
            FormsAuthentication.SignOut()
            Return Redirect("//intranet2.batz.es/langileen_txokoa/menuempleado.aspx?id=" + sessionId)
        End Function
    End Class
End Namespace