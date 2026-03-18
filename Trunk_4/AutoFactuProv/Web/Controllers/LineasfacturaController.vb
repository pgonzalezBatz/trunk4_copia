Imports System.Web.Mvc

Namespace Controllers
    Public Class LineasFacturaController
        Inherits BaseController

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idFacturaProv"></param>
        ''' <returns></returns>
        Function Index(ByVal idFacturaProv As Integer) As ActionResult
            Dim lineasFactura As List(Of ELL.LineasFacturaProv) = BLL.LineasFacturaProvBLL.CargarListado(idFacturaProv)

            lineasFactura.ForEach(Sub(l) l.AlbaranBRAIN = BLL.AlbaranesBLL.ObtenerItem(l.Albaran, l.Pedido, l.Linea))

            ViewData("LineasFactura") = lineasFactura

            Return View()
        End Function

#End Region
    End Class
End Namespace