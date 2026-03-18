Namespace ELL

    Public Class BusquedaJson
        Private _pieza As String = String.Empty

        Public Property Pieza() As String
            Get
                Return _pieza
            End Get
            Set(ByVal value As String)
                _pieza = value
            End Set
        End Property
    End Class

End Namespace