Public Class PlantaSeleccionadaFilterAttribute
    Inherits ActionFilterAttribute

    Public Overrides Sub OnActionExecuting(filterContext As ActionExecutingContext)
        Dim cookies = filterContext.HttpContext.Request.Cookies("BezerreSis_Filtro")
        filterContext.HttpContext.Session("Home") = Not MvcApplication.PlantaSeleccionada(cookies)
    End Sub

End Class
