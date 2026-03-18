Imports System.Configuration
Imports System.IO

Namespace ELL

    Public Class FacturaProv

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Id As Integer

        ''' <summary>
        ''' Ruta relativa de la factura
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RutaFactura As String

        ''' <summary>
        ''' Numero de factura del proveedor
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumFactura As String

        ''' <summary>
        ''' Proveedor
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Proveedor As Integer

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
        ''' Fecha alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FechaAlta As DateTime

        ''' <summary>
        ''' Ruta completa de la factura
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property RutaFacturaCompleta As String
            Get
                Return Path.Combine(ConfigurationManager.AppSettings("rootFacturas"), _RutaFactura)
            End Get
        End Property

#End Region

    End Class

End Namespace
