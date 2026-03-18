Imports System.Web.Mvc

Namespace Controllers
    Public Class FacturasController
        Inherits BaseController

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index() As ActionResult
            ViewData("Facturas") = BLL.FacturasProvBLL.CargarListado(TicketExt.Proveedor, TicketExt.Empresa, TicketExt.Planta)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function IndexBatz() As ActionResult
            ViewData("Facturas") = AutoFactuProvLib.BRAIN.BLL.FacturasBLL.CargarListado(TicketExt.Proveedor, TicketExt.Empresa, TicketExt.Planta)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="fichero"></param>
        ''' <returns></returns>
        Function MostrarFactura(ByVal fichero As String) As ActionResult
            Dim rutaCompleta As String = System.IO.Path.Combine(ConfigurationManager.AppSettings("rootFacturasBatz"), fichero)

            If (IO.File.Exists(rutaCompleta)) Then
                Dim buffer As Byte() = IO.File.ReadAllBytes(rutaCompleta)
                Return File(buffer, "text/plain", fichero)
            Else
                MensajeAlerta(Utils.Traducir("Documento no encontrado"))
                Return Redirect(Request.UrlReferrer.OriginalString)
            End If

        End Function

#End Region

    End Class

End Namespace