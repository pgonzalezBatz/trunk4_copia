Public Class Data
    Public _comodityList As List(Of Comodity)
    Public Property ComodityList() As List(Of Comodity)
        Get
            Return _comodityList
        End Get
        Set(ByVal value As List(Of Comodity))
            _comodityList = value
        End Set
    End Property
    Private _familyList As List(Of Family)
    Public Property FamilyList() As List(Of Family)
        Get
            Return _familyList
        End Get
        Set(ByVal value As List(Of Family))
            _familyList = value
        End Set
    End Property
    Private _subfamilyList As List(Of SubFamily)
    Public Property SubfamilyList() As List(Of SubFamily)
        Get
            Return _subfamilyList
        End Get
        Set(ByVal value As List(Of SubFamily))
            _subfamilyList = value
        End Set
    End Property
End Class
