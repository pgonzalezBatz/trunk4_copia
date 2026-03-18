Namespace XBAT.BLL

    Public Class FacturasBLL

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ObtenerProximoDOCBATZ() As String
            Return DAL.Facturas.getNextDOCBATZ.PadLeft(6, "0")
        End Function

#End Region

    End Class

End Namespace