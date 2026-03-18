Namespace ELL

    Public Class TiposReferenciaVenta

#Region "Enumerados"

        ''' <summary>
        ''' Tipos de referencia de venta
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum Tipos
            New_ = 1
            Evolution = 2
            Spares = 3
        End Enum

#End Region

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _descripcion As String = String.Empty

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        ''' <summary>
        ''' Nombre
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Nombre() As String
            Get
                Return If(String.IsNullOrEmpty(_nombre), String.Empty, _nombre.Trim())
            End Get
            Set(ByVal value As String)
                _nombre = If(String.IsNullOrEmpty(value), String.Empty, value.Trim())
            End Set
        End Property

        ''' <summary>
        ''' Descripción
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

#End Region

    End Class

End Namespace

