Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Routing

Public Module RouteConfig
    Public Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
        Dim MapRoute_defaults As Object = New With {.controller = "Home", .action = "Index", .id = UrlParameter.Optional}
        routes.MapRoute(
            name:="Default",
            url:="{controller}/{action}/{id}",
            defaults:=MapRoute_defaults)

    End Sub
End Module

