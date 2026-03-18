Imports System.Web.Mvc
Imports log4net

Namespace Controllers
    Public Class errorController
        Inherits Controller

        Dim log As ILog = log4net.LogManager.GetLogger("root")

        <SimpleRoleProvider(roles.normal)>
        Function PageNotFound(msg As String) As ActionResult
            Response.TrySkipIisCustomErrors = True
            Response.StatusCode = 404
#If DEBUG Then
            Dim lu As New LoginUser
            lu.msg = msg
            Return View(lu)
#Else
            Return View()
#End If
        End Function

        Function logoff() As ActionResult
            log.Info("Loggin off (IdSab:" & SimpleRoleProvider.GetId() & ")")
            FormsAuthentication.SignOut()
            For Each k In Request.Cookies.AllKeys
                Response.Cookies(k).Expires = DateTime.Now.AddDays(-1)
            Next
            Return RedirectToAction("index", "access", h.ToRouteValues(Request.QueryString, Nothing))
        End Function
    End Class
End Namespace