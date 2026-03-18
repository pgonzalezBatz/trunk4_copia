Public Class TelefonodirectoController
    Inherits System.Web.Mvc.Controller

    ReadOnly strCnOracle As String = ConfigurationManager.ConnectionStrings("oracle").ConnectionString

    <SimpleRoleProvider(roles.telefonosdirectos)>
    Function List() As ActionResult
        Dim lst = Db.GetListOftelefonodirecto(strCnOracle)
        Return View(lst)
    End Function

    <SimpleRoleProvider(roles.telefonosdirectos)>
    Function Create() As ActionResult
        Return View("edit")
    End Function
    <SimpleRoleProvider(roles.telefonosdirectos)>
    <HttpPost()>
    Function Create(ByVal o As TelefonoDirecto) As ActionResult
        If ModelState.IsValid Then

            Db.Inserttelefonodirecto(o, strCnOracle)
            Return RedirectToAction("list")
        End If
        Return View("edit")
    End Function

    <SimpleRoleProvider(roles.telefonosdirectos)>
    Function Edit(ByVal id As Integer) As ActionResult
        Return View(Db.Gettelefonodirecto(id, strCnOracle))
    End Function
    <SimpleRoleProvider(roles.telefonosdirectos)>
    <HttpPost()>
    Function Edit(ByVal o As TelefonoDirecto, confirmation As String) As ActionResult
        If ModelState.IsValid Then
            Db.updatetelefonodirecto(o, strCnOracle)
            Return RedirectToAction("list")
        End If
        Return View()
    End Function

    <SimpleRoleProvider(roles.telefonosdirectos)>
    Function Delete(ByVal id As Integer) As ActionResult
        Return View(Db.Gettelefonodirecto(id, strCnOracle))
    End Function
    <SimpleRoleProvider(roles.telefonosdirectos)>
    <HttpPost()>
    Function Delete(id As Integer, confirmation As String) As ActionResult
        If ModelState.IsValid Then
            Db.deletetelefonodirecto(id, strCnOracle)
            Return RedirectToAction("list")
        End If
        Return View(Db.Gettelefonodirecto(id, strCnOracle))
    End Function
End Class