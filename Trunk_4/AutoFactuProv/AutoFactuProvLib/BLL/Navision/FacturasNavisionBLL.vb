Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class FacturasNavisionBLL

        ''' <summary>
        ''' Comprueba si una factura para un proveedor existe
        ''' </summary>
        ''' <param name="idProveedor"></param>
        ''' <param name="numFactura"></param>
        ''' <returns></returns>
        Public Shared Function ExisteFacturaProveedor(ByVal idProveedor As String, ByVal numFactura As String) As Boolean
            Return DAL.Navision.FacturasNavisionDAL.ExisteFacturaProveedor(idProveedor, numFactura)
        End Function

    End Class

End Namespace
