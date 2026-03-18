Namespace ELL

    Public Class AyudaVisual

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _archivo As Byte()
        Private _contentType As String = String.Empty

#End Region

#Region "Properties"

        Public Property ID() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property

        Public Property NOMBRE() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property ARCHIVO() As Byte()
            Get
                Return _archivo
            End Get
            Set(ByVal value As Byte())
                _archivo = value
            End Set
        End Property

        Public Property ContentType() As String
            Get
                Return _contentType
            End Get
            Set(ByVal value As String)
                _contentType = value
            End Set
        End Property

#End Region

    End Class

End Namespace

