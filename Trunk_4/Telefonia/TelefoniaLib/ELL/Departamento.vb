Namespace ELL

    Public Class Departamento

        Private _id As String = String.Empty
        Private _nombre As String = String.Empty
        Private _idPlanta As Integer = Integer.MinValue

        Public Property ID() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property IdPlanta() As Integer
            Get
                Return _idPlanta
            End Get
            Set(ByVal value As Integer)
                _idPlanta = value
            End Set
        End Property
    End Class

End Namespace