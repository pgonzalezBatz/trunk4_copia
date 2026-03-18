Namespace ELL

    Public Class Objeto

        Private _Id As String
        Private _Nombre As String
        Private _IdPtksis As String

        Public Property Id() As String
            Get
                Return _Id
            End Get
            Set(ByVal value As String)
                _Id = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _Nombre
            End Get
            Set(ByVal value As String)
                _Nombre = value
            End Set
        End Property

        Public Property IdPtksis() As String
            Get
                Return _IdPtksis
            End Get
            Set(ByVal value As String)
                _IdPtksis = value
            End Set
        End Property

    End Class
End Namespace