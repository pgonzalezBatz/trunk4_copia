Imports System.Web.Mvc

Namespace Controllers
    Public Class PruebaController
        Inherits BaseController

        ' GET: Prueba
        Function Index() As ActionResult
            ViewData("Evoluciones") = BLL.EvolucionAccionesBLL.CargarListado(23)
            Return View()
        End Function


        ' GET: Prueba
        Function Index1() As ActionResult
            ViewData("IdAccion") = 23
            Return View()
        End Function

    End Class
End Namespace