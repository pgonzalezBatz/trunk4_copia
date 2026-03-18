Namespace ELL
    Public Class EstadosIncidencia
        Private _IdEstadoIncidencia As Integer
        Private _Nombre As String

        Public Enum EstadoIncidencia As Integer
            Logged = 1
            Filtered = 2
            Rejected = 3
            Active = 4
            Waiting = 5
            Resolved = 6
            Closed = 7
            ValidatePendingResponsible = 8
            ValidatePendingIKS = 9
            RejectedClosed = 10
            Notes = 11
            ReActivate = 12
        End Enum

        Public Property IdEstadoIncidencia() As Integer
            Get
                Return _IdEstadoIncidencia
            End Get
            Set(ByVal value As Integer)
                _IdEstadoIncidencia = value
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
    End Class
End Namespace