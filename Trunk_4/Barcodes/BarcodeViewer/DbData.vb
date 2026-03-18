Public Class DbData
    Private _numordf As String
    Public Property Numordf() As String
        Get
            Return _numordf
        End Get
        Set(ByVal value As String)
            _numordf = value
        End Set
    End Property
    Private _numope As String
    Public Property Numope() As String
        Get
            Return _numope
        End Get
        Set(ByVal value As String)
            _numope = value
        End Set
    End Property
    Private _nummar As String
    Public Property Nummar() As String
        Get
            Return _nummar
        End Get
        Set(ByVal value As String)
            _nummar = value
        End Set
    End Property
    Private _material As String
    Public Property Material() As String
        Get
            Return _material
        End Get
        Set(ByVal value As String)
            _material = value
        End Set
    End Property

    Private _canped As String
    Public Property CanPed() As String
        Get
            Return _canped
        End Get
        Set(ByVal value As String)
            _canped = value
        End Set
    End Property

    Private _canrec As String
    Public Property CanRec() As String
        Get
            Return _canrec
        End Get
        Set(ByVal value As String)
            _canrec = value
        End Set
    End Property
End Class
