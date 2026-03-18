Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class EstadoBonos

#Region "Constantes"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Const R_D As String = "R-D"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Const Offer As String = "Offer"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Const Offer_RFI As String = "Offer RFI"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Const Development As String = "Development"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Const Industrialization As String = "Industrialization"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Const RFQ As String = "RFQ"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Const RFI As String = "RFI"

#End Region

#Region "Enumerados"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum Estado
            Offer = 1
            RFQ = 4
            Industrialization = 7
            Development = 12
            R_D = 21
            Offer_RFI = 201
            RFI = 221
        End Enum

#End Region

#Region "Properties"

        ''' <summary>
        ''' Proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As String

        ''' <summary>
        ''' Nombre proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Nombre() As String

#End Region

    End Class

End Namespace

