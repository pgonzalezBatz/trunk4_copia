' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
' visit http://go.microsoft.com/?LinkId=9394802
Public Class MvcApplication
    Inherits System.Web.HttpApplication


    Sub Application_Start()
        log4net.Config.XmlConfigurator.Configure()
        AreaRegistration.RegisterAllAreas()
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\log4netConfig.config"))
    End Sub
    Sub Application_BeginRequest()
        Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(h.GetCulture())
        Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(h.GetCulture())
    End Sub
    Private Sub MvcApplication_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        Dim err = Server.GetLastError()
        Dim log = log4net.LogManager.GetLogger("root")
        Server.ClearError()
        Response.Clear()
        Dim httpEx = CType(err, HttpException)
        Response.StatusCode = If(err IsNot Nothing, httpEx.GetHttpCode(), 500)
        Response.TrySkipIisCustomErrors = True
        Response.Status = Response.Status
        If err.GetType().Name = "SecurityException" Then
            log.Error("SecurityException - Not logged")
            FormsAuthentication.SignOut()
            Dim url = New UrlHelper(HttpContext.Current.Request.RequestContext)
            Response.Redirect(url.Action("index", "access", h.ToRouteValues(Request.QueryString, Nothing)))
            Return
            Exit Sub
        ElseIf err.GetType().Name = "HttpException" Then
            log.Error(err.Message, err)
            Dim rd As New RouteData()
            rd.Values.Add("controller", "error")
            rd.Values.Add("action", "PageNotFound")
            rd.Values.Add("msg", err.Message)
            Dim controller As IController = New Controllers.errorController()
            Dim rc = New RequestContext(New HttpContextWrapper(Context), rd)
            controller.Execute(rc)
            Exit Sub
        Else
            log.Error("GenericException: " & err.Message, err)
        End If
    End Sub
End Class
