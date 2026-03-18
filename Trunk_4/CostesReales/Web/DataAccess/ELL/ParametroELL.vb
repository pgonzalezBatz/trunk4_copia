Public Class ParametroELL

#Region "Campos"
    Private _anyo As Integer
    Private _mes As Integer
    Private _fecha_cierre As Integer
    Private _anyo_AA As Integer
    Private _mes_AA As Integer
    Private _fecha_cierre_inicio_mes As Integer
    Private _fecha_TM As Integer
    Private _tasa_chatarra As Decimal
    Private _activo As Boolean
    Private _pyg As String
    Private _id As Integer
#End Region

#Region "Propiedades"
    Public Property Anyo As Integer
        Get
            Return _anyo
        End Get
        Set(value As Integer)
            _anyo = value
        End Set
    End Property

    Public Property Mes As Integer
        Get
            Return _mes
        End Get
        Set(value As Integer)
            _mes = value
        End Set
    End Property

    Public Property Fecha_cierre As Integer
        Get
            Return _fecha_cierre
        End Get
        Set(value As Integer)
            _fecha_cierre = value
        End Set
    End Property

    Public Property Anyo_AA As Integer
        Get
            Return _anyo_AA
        End Get
        Set(value As Integer)
            _anyo_AA = value
        End Set
    End Property

    Public Property Mes_AA As Integer
        Get
            Return _mes_AA
        End Get
        Set(value As Integer)
            _mes_AA = value
        End Set
    End Property

    Public Property Fecha_cierre_inicio_mes As Integer
        Get
            Return _fecha_cierre_inicio_mes
        End Get
        Set(value As Integer)
            _fecha_cierre_inicio_mes = value
        End Set
    End Property

    Public Property Fecha_TM As Integer
        Get
            Return _fecha_TM
        End Get
        Set(value As Integer)
            _fecha_TM = value
        End Set
    End Property

    Public Property Tasa_chatarra As Decimal
        Get
            Return _tasa_chatarra
        End Get
        Set(value As Decimal)
            _tasa_chatarra = value
        End Set
    End Property

    Public Property Activo As Boolean
        Get
            Return _activo
        End Get
        Set(value As Boolean)
            _activo = value
        End Set
    End Property

    Public Property PYG As String
        Get
            Return _pyg
        End Get
        Set(value As String)
            _pyg = value
        End Set
    End Property

    Public Property Id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property
#End Region

End Class
