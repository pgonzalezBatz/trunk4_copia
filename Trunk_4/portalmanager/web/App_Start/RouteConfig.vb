Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Routing

Public Class RouteConfig
    Public Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
        routes.MapRoute("eki", "personal/{action}", New With {.controller = "personal", .action = "busquedaCompletaAltas"})
        routes.MapRoute("Default", "{controller}/{action}", New With {.controller = "access", .action = "index"})

    End Sub
End Class