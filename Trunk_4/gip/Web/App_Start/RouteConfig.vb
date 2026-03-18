Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Routing

Public Class RouteConfig
    Public Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
        routes.MapRoute(name:="proveedor", url:="proveedor/{action}", defaults:=New With {.controller = "proveedor", .action = "Search"})
        routes.MapRoute(name:="Default", url:="{controller}/{action}", defaults:=New With {.controller = "access", .action = "Index"})
    End Sub
End Class