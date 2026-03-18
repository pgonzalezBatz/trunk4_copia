Public Class accessController
    Inherits System.Web.Mvc.Controller

 
    Function Index() As ActionResult
        Return RedirectToAction("search", "proveedor")


        'If User.Identity.IsAuthenticated Then
        '    Return RedirectToAction("search", "proveedor")
        'End If
        'Dim lst = db.GetIdSab(Request.LogonUserIdentity.Name.ToLower, ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
        'If lst.Count = 0 Then
        '    Return Redirect("~/accesodenegado.htm")
        'End If
        'Dim idSab = lst(0)
        'If Request.LogonUserIdentity.Name.ToLower.Contains("aazkuenaga") Then
        '    idSab = 57151
        'End If
        'h.SetCulture(idSab, ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
        'SimpleRoleProvider.setAuthCookieWithRole(idSab, Function() db.GetRole(idSab, ConfigurationManager.AppSettings("recurso"), ConfigurationManager.ConnectionStrings("oracle").ConnectionString))
        'Return RedirectToAction("search", "proveedor")
    End Function
End Class