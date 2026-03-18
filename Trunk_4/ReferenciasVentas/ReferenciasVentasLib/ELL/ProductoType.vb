Namespace ELL

    Public Class ProductoType

#Region "Variables miembro"

        Private _idProducto As Integer = Integer.MinValue
        Private _idType As Integer = Integer.MinValue

#End Region

#Region "Properties"

        ''' <summary>
        ''' IdProducto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
            End Set
        End Property

        ''' <summary>
        ''' IdType
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdType() As String
            Get
                Return _idType
            End Get
            Set(ByVal value As String)
                _idType = value
            End Set
        End Property

#End Region

    End Class

End Namespace

