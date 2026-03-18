Namespace ELL

    Public Class LineaFactura

        Private _id As Integer = Integer.MinValue
        Private _idTlfno As Integer = Integer.MinValue
        Private _telefono As String = String.Empty
        Private _idExtension As String = Integer.MinValue
        Private _extension As String = String.Empty
        Private _trafico As String = String.Empty
        Private _tipoLlamada As String = String.Empty
        Private _tipoDestino As String = String.Empty
        Private _numeroLlamado As String = String.Empty
        Private _fecha As Date = Date.MinValue
        Private _hora As DateTime = DateTime.MinValue
        Private _tiempo As Decimal = 0
        Private _importe As Decimal = 0

#Region "Properties"

        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property IdTlfno() As Integer
            Get
                Return _idTlfno
            End Get
            Set(ByVal value As Integer)
                _idTlfno = value
            End Set
        End Property

        Public Property Telefono() As String
            Get
                Return _telefono
            End Get
            Set(ByVal value As String)
                _telefono = value
            End Set
        End Property

        Public Property IdExtension() As Integer
            Get
                Return _idExtension
            End Get
            Set(ByVal value As Integer)
                _idExtension = value
            End Set
        End Property

        Public Property Extension() As String
            Get
                Return _extension
            End Get
            Set(ByVal value As String)
                _extension = value
            End Set
        End Property

        Public Property Trafico() As String
            Get
                Return _trafico
            End Get
            Set(ByVal value As String)
                _trafico = value
            End Set
        End Property

        Public Property TipoLlamada() As String
            Get
                Return _tipoLlamada
            End Get
            Set(ByVal value As String)
                _tipoLlamada = value
            End Set
        End Property

        Public Property TipoDestino() As String
            Get
                Return _tipoDestino
            End Get
            Set(ByVal value As String)
                _tipoDestino = value
            End Set
        End Property

        Public Property NumeroLlamado() As String
            Get
                Return _numeroLlamado
            End Get
            Set(ByVal value As String)
                _numeroLlamado = value
            End Set
        End Property

        Public Property Fecha() As Date
            Get
                Return _fecha
            End Get
            Set(ByVal value As Date)
                _fecha = value
            End Set
        End Property

        Public Property Hora() As DateTime
            Get
                Return _hora
            End Get
            Set(ByVal value As DateTime)
                _hora = value
            End Set
        End Property

        Public Property Tiempo() As Decimal
            Get
                Return _tiempo
            End Get
            Set(ByVal value As Decimal)
                _tiempo = value
            End Set
        End Property

        Public Property Importe() As Decimal
            Get
                Return _importe
            End Get
            Set(ByVal value As Decimal)
                _importe = value
            End Set
        End Property

#End Region

    End Class

End Namespace