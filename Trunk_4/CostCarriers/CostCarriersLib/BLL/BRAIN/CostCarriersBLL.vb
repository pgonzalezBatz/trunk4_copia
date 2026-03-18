Namespace BLL.BRAIN

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CostCarriersBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <param name="empresa"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal codigo As String, ByVal empresa As String) As ELL.BRAIN.CostCarrier
            Return DAL.BRAIN.CostCarriersDAL.getObject(codigo, empresa)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <param name="empresa"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal codigo As String, ByVal empresa As String) As List(Of ELL.BRAIN.CostCarrier)
            Dim lista As New List(Of ELL.BRAIN.CostCarrier)
            If (empresa <> Integer.MinValue) Then
                lista = DAL.BRAIN.CostCarriersDAL.loadList(codigo, empresa)
            End If
            Return lista
        End Function

#End Region

    End Class

End Namespace