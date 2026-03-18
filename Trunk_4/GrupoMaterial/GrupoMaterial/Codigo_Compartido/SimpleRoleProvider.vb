Public Class SimpleRoleProvider
    Inherits AuthorizeAttribute

    Private pRole() As Integer
    Public Sub New(ByVal ParamArray role() As Integer)
        pRole = role
    End Sub

    Protected Overrides Function AuthorizeCore(ByVal httpContext As System.Web.HttpContextBase) As Boolean
        If httpContext.User.Identity.IsAuthenticated AndAlso httpContext.Request.Cookies(FormsAuthentication.FormsCookieName) IsNot Nothing Then
            Dim cookie = httpContext.Request.Cookies(FormsAuthentication.FormsCookieName)
            Dim decrypted = FormsAuthentication.Decrypt(cookie.Value)
            Dim UserRole As Integer = decrypted.UserData
            For Each e In pRole
                If (UserRole And e) = e Then
                    Return True
                End If
            Next
        End If
        Throw New System.Security.SecurityException
    End Function
  
    Public Shared Function IsUserAuthorised(ByVal ParamArray pRole() As Integer) As Boolean
        If HttpContext.Current.User.Identity.IsAuthenticated AndAlso HttpContext.Current.Request.Cookies(FormsAuthentication.FormsCookieName) IsNot Nothing Then
            Dim cookie = HttpContext.Current.Request.Cookies(FormsAuthentication.FormsCookieName)
            Dim decrypted = FormsAuthentication.Decrypt(cookie.Value)
            Dim UserRole As Integer = decrypted.UserData
            For Each e In pRole
                If (UserRole And e) = e Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function
    Public Shared Function GetRole() As Integer
        Dim cookie = HttpContext.Current.Request.Cookies(FormsAuthentication.FormsCookieName)
        Dim decrypted = FormsAuthentication.Decrypt(cookie.Value)
        If IsNumeric(decrypted.UserData) Then
            Return decrypted.UserData
        End If
        Throw New System.Security.SecurityException("No se encuentra role en los datos de usuario")
    End Function
    Public Shared Function GetId() As Integer
        If String.IsNullOrEmpty(FormsAuthentication.FormsCookieName) Then
            Throw New System.Security.SecurityException("Error al extraer el Id de usuario desde el cookie")
        End If
        Dim cookie = HttpContext.Current.Request.Cookies(FormsAuthentication.FormsCookieName)
        If cookie Is Nothing Then
            Throw New System.Security.SecurityException("Error al extraer el Id de usuario desde el cookie")
        End If
        Dim decrypted = FormsAuthentication.Decrypt(cookie.Value)

        If IsNumeric(decrypted.Name) Then
            Return decrypted.Name
        End If
        Throw New System.Security.SecurityException("Error al extraer el Id de usuario desde el cookie")
    End Function

    Public Shared Sub SetAuthCookieWithRole(userName As String, fGetRole As Func(Of Integer))
        HttpContext.Current.Response.Cookies.Add(GetAuthCookieWithExtraData(userName, True, fGetRole()))
    End Sub
    Public Shared Sub SetAuthCookieWithRole(userName As String, fGetRole As Func(Of Integer), PersistentCookie As Boolean)
        HttpContext.Current.Response.Cookies.Add(GetAuthCookieWithExtraData(userName, PersistentCookie, fGetRole()))
    End Sub
    Public Shared Sub RemoveAuthCookie()
        FormsAuthentication.SignOut()
        'HttpContext.Current.Response.Cookies(FormsAuthentication.FormsCookieName).Expires = Date.MinValue
    End Sub

    Private Shared Function GetAuthCookieWithExtraData(idsab As String, rememberMe As Boolean, extraData As String)
        Dim cookie = FormsAuthentication.GetAuthCookie(idsab, rememberMe)
        Dim ticket = FormsAuthentication.Decrypt(cookie.Value)
        Dim newTicket = New FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, extraData, ticket.CookiePath)
        Dim encTicket = FormsAuthentication.Encrypt(newTicket)
        'Use existing cookie. Could create new one but would have to copy settings over...
        cookie.Value = encTicket
        cookie.Secure = True
        Return cookie
    End Function
End Class