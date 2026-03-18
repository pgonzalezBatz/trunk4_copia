Public Class QRData
    Private _codAlbaran As String
    Public Property CodAlbaran() As String
        Get
            Return _codAlbaran
        End Get
        Set(ByVal value As String)
            _codAlbaran = value
        End Set
    End Property

    Private _numPedido As String
    Public Property NumPedido() As String
        Get
            Return _numPedido
        End Get
        Set(ByVal value As String)
            _numPedido = value
        End Set
    End Property

    Private _linPedido As String
    Public Property LinPedido() As String
        Get
            Return _linPedido
        End Get
        Set(ByVal value As String)
            _linPedido = value
        End Set
    End Property
    Private _codReferencia As String
    Public Property CodReferencia() As String
        Get
            Return _codReferencia
        End Get
        Set(ByVal value As String)
            _codReferencia = value
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
    Private _of As String
    Public Property OfData() As String
        Get
            Return _of
        End Get
        Set(ByVal value As String)
            _of = value
        End Set
    End Property
    Private _op As String
    Public Property OpData() As String
        Get
            Return _op
        End Get
        Set(ByVal value As String)
            _op = value
        End Set
    End Property
    Private _marca As String
    Public Property Marca() As String
        Get
            Return _marca
        End Get
        Set(ByVal value As String)
            _marca = value
        End Set
    End Property
    Private _cantidad As String
    Public Property Cantidad() As String
        Get
            Return _cantidad
        End Get
        Set(ByVal value As String)
            _cantidad = value
        End Set
    End Property
    Private _precioUnitario As String
    Public Property PrecioUnitario() As String
        Get
            Return _precioUnitario
        End Get
        Set(ByVal value As String)
            _precioUnitario = value
        End Set
    End Property
    Private _importe As String
    Public Property Importe() As String
        Get
            Return _importe
        End Get
        Set(ByVal value As String)
            _importe = value
        End Set
    End Property

    Private _status As String
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    Private _texto As String
    Public Property Texto() As String
        Get
            Return _texto
        End Get
        Set(ByVal value As String)
            _texto = value
        End Set
    End Property

    Private _descripcion As String
    Public Property Descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property
End Class
