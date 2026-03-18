Imports System.Web.Mvc

Namespace Controllers
    Public Class proveedorController
        Inherits Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("oracle").ConnectionString

        <SimpleRoleProvider(Roles.interno)>
        Function Index() As ActionResult
            Return View(db.GetListOfProveedorConRecurso(ConfigurationManager.AppSettings("IDRECURSOPROVEEDOR"), strCn))
        End Function
        <SimpleRoleProvider(Roles.interno)>
        Function identityChange(idProveedor As Integer)
            SimpleRoleProvider.setAuthCookieWithRole(idProveedor, Function() SimpleRoleProvider.GetRole)
            Return RedirectToAction("index", "informe")
        End Function
    End Class
End Namespace