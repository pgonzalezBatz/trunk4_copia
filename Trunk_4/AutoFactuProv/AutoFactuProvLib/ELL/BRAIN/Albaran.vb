Namespace ELL

    Public Class Albaran

#Region "CONSTANTES"

        Public Const ALBARAN_FACTURABLE As String = "A"
        Public Const PEDIDO_SIN_RECEPCIONAR As String = "P"
        Public Const FACTURADO As String = "F"

#End Region

#Region "Enumerados"

        Public Enum OrigenAlbaran
            Proveedor = 1
            Batz = 2
        End Enum

#End Region

#Region "Properties"

        ''' <summary>
        ''' Empresa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>         
        Public Property Empresa As String

        ''' <summary>
        ''' Planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Planta As String

        ''' <summary>
        ''' Num proveedor
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Proveedor As Integer

        ''' <summary>
        ''' Nombre proveedor
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NombreProveedor As String

        ''' <summary>
        ''' Tipo 
        ''' 'A' = Albaranes pendientes de facturar
        ''' 'P' = Pedidos pendientes de recepcionar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Tipo As String

        ''' <summary>
        ''' Albarán 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Albaran As String

        ''' <summary>
        ''' Núm pedido
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Pedido As Integer

        ''' <summary>
        ''' Línea
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Linea As Integer

        ''' <summary>
        ''' Cant recibida (para los del tipo A)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CantRecibida As Decimal

        ''' <summary>
        ''' Precio unitario sin IVA
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PrecioUnitario As Decimal

        ''' <summary>
        ''' Moneda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Moneda As String

        ''' <summary>
        ''' Concepto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Concepto As String

        ''' <summary>
        ''' Cant pendiente (para los de tipo P)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CantPendiente As Decimal

        ''' <summary>
        ''' Solicitante
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Solicitante As String

        ''' <summary>
        ''' Solicitante
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RefArticulo As String

        ''' <summary>
        ''' Solicitante
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PrecioTotalRecibidos As Decimal
            Get
                Return CantRecibida * PrecioUnitario
            End Get
        End Property

#End Region

    End Class

End Namespace
