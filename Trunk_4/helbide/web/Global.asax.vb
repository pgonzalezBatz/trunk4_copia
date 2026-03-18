Imports System.Web.Mvc
Imports System.Web.Routing

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        RouteConfig.RegisterRoutes(RouteTable.Routes)
    End Sub
    Sub Application_BeginRequest()
        Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(h.GetCulture())
    End Sub
    Private Sub MvcApplication_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        Dim err = Server.GetLastError()
        If err.GetType().Name = "SecurityException" Then
            FormsAuthentication.SignOut()
            Response.Redirect("~/accesodenegado.html")
            Exit Sub
        End If
        'File the error
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
        log.Error(err.Message, err)
#If Not DEBUG Then
        Response.Redirect("~/error.html")
#End If
    End Sub
End Class
