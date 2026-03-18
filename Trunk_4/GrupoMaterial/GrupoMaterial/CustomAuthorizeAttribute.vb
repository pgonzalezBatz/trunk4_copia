Imports System.Security

Public Class CustomAuthorizeAttribute
    Inherits AuthorizeAttribute

    Protected Overrides Function AuthorizeCore(httpContext As HttpContextBase) As Boolean
        httpContext.Session("isAuthorized") = 1
        Return True


        Dim userName = httpContext.User.Identity.Name.ToLower
        If httpContext.Session("isAuthorized") = 1 Then
            Return True
            'ElseIf httpContext.Session("isAuthorized") = 0 Then
            '    Return False
        End If
        Dim oracleDB As New OracleDataAccess
        Dim authorizedUsers = oracleDB.getAuthorizedUsers()

        If Not authorizedUsers.FirstOrDefault(Function(o) o.IdDirectorioActivo = userName) IsNot Nothing Then
            httpContext.Session("isAuthorized") = 1
            Return True
        Else
            httpContext.Session("isAuthorized") = 0
            Return False
        End If
    End Function

    Protected Overrides Sub HandleUnauthorizedRequest(filterContext As System.Web.Mvc.AuthorizationContext)
        If filterContext.HttpContext.Request.IsAuthenticated Then
            filterContext.Result = New System.Web.Mvc.HttpStatusCodeResult(CInt(System.Net.HttpStatusCode.Forbidden))
        Else
            MyBase.HandleUnauthorizedRequest(filterContext)
        End If
    End Sub
End Class