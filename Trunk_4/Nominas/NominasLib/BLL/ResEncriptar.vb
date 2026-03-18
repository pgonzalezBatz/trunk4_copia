
Public Class ResEncriptar
    Private _codpersona As String
    Private _nombre As String
    Private _mail As String
    Private _encriptadoOK As Boolean
    Private _envioOK As Boolean
    Private _estructuraOK As Boolean
    Private _err As String
    Private _month As Integer
    Private _year As Integer
    Private _culture As String
    Private _test As Boolean

    Public Property CodPersona() As String
        Get
            Return _codpersona
        End Get
        Set(value As String)
            _codpersona = value
        End Set
    End Property

    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property Mail() As String
        Get
            Return _mail
        End Get
        Set(value As String)
            _mail = value
        End Set
    End Property

    Public Property EncriptadoOK() As Boolean
        Get
            Return _encriptadoOK
        End Get
        Set(value As Boolean)
            _encriptadoOK = value
        End Set
    End Property

    Public Property EnvioOK() As Boolean
        Get
            Return _envioOK
        End Get
        Set(value As Boolean)
            _envioOK = value
        End Set
    End Property

    Public Property EstructuraOK() As Boolean
        Get
            Return _estructuraOK
        End Get
        Set(value As Boolean)
            _estructuraOK = value
        End Set
    End Property

    Public Property Err() As String
        Get
            Return _err
        End Get
        Set(value As String)
            _err = value
        End Set
    End Property

    Public Property Month() As Integer
        Get
            Return _month
        End Get
        Set(value As Integer)
            _month = value
        End Set
    End Property

    Public Property Year() As Integer
        Get
            Return _year
        End Get
        Set(value As Integer)
            _year = value
        End Set
    End Property

    Public Property Culture() As String
        Get
            Return _culture
        End Get
        Set(value As String)
            _culture = value
        End Set
    End Property


    Public Property Test() As Boolean
        Get
            Return _test
        End Get
        Set(value As Boolean)
            _test = value
        End Set
    End Property
End Class