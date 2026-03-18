Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class EstadosBonosBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene estados
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado() As List(Of ELL.EstadoBonos)
            Return DAL.EstadosBonos.loadList()
        End Function

        ''' <summary>
        ''' Obtiene estados
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListadoCCMetadata() As List(Of ELL.EstadoBonos)
            Return CargarListado.Where(Function(f) f.Id = ELL.EstadoBonos.Estado.Development OrElse f.Id = ELL.EstadoBonos.Estado.Industrialization OrElse f.Id = ELL.EstadoBonos.Estado.Offer OrElse
                                                     f.Id = ELL.EstadoBonos.Estado.Offer_RFI OrElse f.Id = ELL.EstadoBonos.Estado.R_D OrElse f.Id = ELL.EstadoBonos.Estado.RFQ OrElse
                                                     f.Id = ELL.EstadoBonos.Estado.RFI).ToList()
        End Function

#End Region

    End Class

End Namespace