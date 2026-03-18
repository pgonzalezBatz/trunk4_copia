
Namespace web
    Public Class accessController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAB").ConnectionString

        <SimpleRoleProvider(Role.creacion, Role.compras, Role.envio)>
        Function Index() As ActionResult
            Return RedirectToAction("list", h.GetRoleRedirection(SimpleRoleProvider.GetRole()))
        End Function
        Function testEmail() As ActionResult
            h.SendEmail(DBAccess.proba(strCn), "Proba ", "proba")
            Return RedirectToAction("index")
        End Function


        Function landingPage()
            Return View()
        End Function
    End Class
End Namespace