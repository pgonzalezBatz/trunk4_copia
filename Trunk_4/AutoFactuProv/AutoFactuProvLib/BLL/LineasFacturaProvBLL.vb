Namespace BLL

    Public Class LineasFacturaProvBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de líneas de una factura
        ''' </summary>
        ''' <param name="idFacturaProv"></param> 
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal idFacturaProv As Integer) As List(Of ELL.LineasFacturaProv)
            Return DAL.LineasFacturaProvDAL.loadList(idFacturaProv)
        End Function

        ''' <summary>
        ''' Obtiene un listado de líneas de un factura de proveedor
        ''' </summary>
        ''' <param name="proveedor"></param> 
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal proveedor As Integer, ByVal empresa As String, ByVal planta As String) As List(Of ELL.LineasFacturaProv)
            Return DAL.LineasFacturaProvDAL.loadList(proveedor, empresa, planta)
        End Function

#End Region

    End Class

End Namespace