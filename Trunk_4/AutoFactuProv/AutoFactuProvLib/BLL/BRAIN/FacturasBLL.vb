Namespace BRAIN.BLL

    Public Class FacturasBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una factura
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="proveedor"></param>
        ''' <param name="factura"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal empresa As String, ByVal proveedor As Integer, ByVal año As Integer, ByVal factura As String) As ELL.Factura
            Return DAL.FacturasDAL.getItem(empresa, proveedor, año, factura)
        End Function

        ''' <summary>
        ''' Obtiene un listado de facturas autofacturadas para un proveedor
        ''' </summary>
        ''' <param name="proveedor"></param> 
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal proveedor As Integer, ByVal empresa As String, ByVal planta As String) As List(Of ELL.Factura)
            Return DAL.FacturasDAL.loadList(proveedor, empresa, planta)
        End Function

#End Region

    End Class

End Namespace