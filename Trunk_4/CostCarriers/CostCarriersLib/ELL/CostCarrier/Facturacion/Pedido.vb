Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class Pedido

#Region "Enumerados"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum EstadoFacturacion
            Sent_to_invoice = 10
            Invoiced = 20
        End Enum

#End Region

#Region "Properties"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NumPedido() As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ImporteTotal() As Decimal

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdCabecera() As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ImporteFacturado() As Decimal

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Comentarios() As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdMoneda() As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Moneda() As String

#End Region

    End Class

End Namespace

