Namespace ELL

    Public Class Factura

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
        ''' Numero de factura
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumFactura As Integer

        ''' <summary>
        ''' Fecha alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FechaAlta As DateTime

        ''' <summary>
        ''' Importe sin IVA
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImporteSinIVA As String

        ''' <summary>
        ''' Moneda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Moneda As String

        ''' <summary>
        ''' Total factura
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TotalFactura As String

        ''' <summary>
        ''' Importe IVA
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImporteIVA As String

        ''' <summary>
        ''' Estado
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Estado As String

        ''' <summary>
        ''' Nombre fichero factura
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NombreFicheroFactura As String
            Get
                Return String.Format("{0}_{1}_{2}.pdf", Empresa, Proveedor, NumFactura)
            End Get
        End Property

        ''' <summary>
        ''' Factura
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Factura As String

#End Region

    End Class

End Namespace
