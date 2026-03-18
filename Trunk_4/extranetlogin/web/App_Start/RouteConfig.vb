Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Routing

Public Class RouteConfig
    Public Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
        routes.MapRoute("OldLogin", "Login.aspx", New With {.controller = "access", .action = "index"})
        routes.MapRoute("OldDefault", "default.aspx", New With {.controller = "access", .action = "index"})
        routes.MapRoute("Default", "{controller}/{action}", New With {.controller = "access", .action = "index"})
        'routes.MapRoute("Error", "{*url}", New With {.controller = "error", .action = "PageNotFound"})
        routes.MapRoute("PageNotFound", "{*url}", New With {.controller = "error", .action = "PageNotFound"})

        ' to enable attribute routing:    [Route("{bla:int}/{blabla}")]
        ' routes.MapMvcAttributeRoutes()
    End Sub
End Class
