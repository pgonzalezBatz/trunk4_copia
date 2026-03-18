Public Class Distribucion
    Private IdTrabajadorValue As Integer
    Private idTipoValue As Integer
    Private Lectura1Value As String
    Private Lectura2Value As String

    <Required()> _
    Public Property IdTrabajador As Integer
        Get
            Return IdTrabajadorValue
        End Get
        Set(ByVal value As Integer)
            IdTrabajadorValue = value
        End Set
    End Property
    <Required()> _
    Public Property IdTipo As Integer
        Get
            Return idTipoValue
        End Get
        Set(ByVal value As Integer)
            idTipoValue = value
        End Set
    End Property
    <Required()> _
    <RegularExpression("^[\d]{24}$", ErrorMessage:="La longitud de la lectura 1 es incorrecta")> _
    Public Property Lectura1 As String
        Get
            Return Lectura1Value
        End Get
        Set(ByVal value As String)
            Lectura1Value = value
        End Set
    End Property
    <Required()> _
    <RegularExpression("^[\d]{24}$", ErrorMessage:="La longitud de la lectura 2 es incorrecta")> _
    Public Property Lectura2 As String
        Get
            Return Lectura2Value
        End Get
        Set(ByVal value As String)
            Lectura2Value = value
        End Set
    End Property
End Class
Public Class DistribucionDesconpuesta
    Private DesdeValue As Integer
    Private HastaValue As Integer
    Private TipoValue As Integer
    Private PrecioValue As Decimal
    Private numeroChequesValue As Integer
    Private nombreValue As String
    Private apellido1Value As String
    Private apellido2Value As String
    Private dniValue As String
    Private idTrabajadorValue As Integer
    Private emailValue As String

    <Required()> _
    Public Property Desde As Integer
        Get
            Return DesdeValue
        End Get
        Set(ByVal value As Integer)
            DesdeValue = value
        End Set
    End Property
    <Required()> _
    Public Property Hasta As Integer
        Get
            Return HastaValue
        End Get
        Set(ByVal value As Integer)
            HastaValue = value
        End Set
    End Property
    <Required()> _
    Public Property Tipo As Integer
        Get
            Return TipoValue
        End Get
        Set(ByVal value As Integer)
            TipoValue = value
        End Set
    End Property
    Public Property Precio As Decimal
        Get
            Return PrecioValue
        End Get
        Set(ByVal value As Decimal)
            PrecioValue = value
        End Set
    End Property
    Public Property NumeroCheques As Decimal
        Get
            Return numeroChequesValue
        End Get
        Set(ByVal value As Decimal)
            numeroChequesValue = value
        End Set
    End Property
    Public Property Nombre As String
        Get
            Return nombreValue
        End Get
        Set(ByVal value As String)
            nombreValue = value
        End Set
    End Property
    Public Property Apellido1 As String
        Get
            Return apellido1Value
        End Get
        Set(ByVal value As String)
            apellido1Value = value
        End Set
    End Property
    Public Property Apellido2 As String
        Get
            Return apellido2Value
        End Get
        Set(ByVal value As String)
            apellido2Value = value
        End Set
    End Property
    Public Property DNI As String
        Get
            Return dniValue
        End Get
        Set(ByVal value As String)
            dniValue = value
        End Set
    End Property
    <Required()> _
    Public Property IdTrabajador As Integer
        Get
            Return IdTrabajadorValue
        End Get
        Set(ByVal value As Integer)
            IdTrabajadorValue = value
        End Set
    End Property
    Public Property Email As String
        Get
            Return emailValue
        End Get
        Set(ByVal value As String)
            emailValue = value
        End Set
    End Property
End Class

Public Class TipoDistribucion
    Private IdValue As Integer
    Private IdEmpresaValue As Integer
    Private precioValue As Decimal
    Private nombreValue As String
    Private numChequesValue As Decimal
    Private grupoValue As String
    Private obsoletoValue As Boolean

    <Required()> _
    Public Property Id As Integer
        Get
            Return IdValue
        End Get
        Set(ByVal value As Integer)
            IdValue = value
        End Set
    End Property
    <Required()> _
    Public Property IdEmpresa As Integer
        Get
            Return IdEmpresaValue
        End Get
        Set(ByVal value As Integer)
            IdEmpresaValue = value
        End Set
    End Property
    <Required()> _
    Public Property Precio As Decimal
        Get
            Return precioValue
        End Get
        Set(ByVal value As Decimal)
            precioValue = value
        End Set
    End Property
    Public Property Nombre As String
        Get
            Return nombreValue
        End Get
        Set(ByVal value As String)
            nombreValue = value
        End Set
    End Property
    <Required()> _
    Public Property NumCheques As Decimal
        Get
            Return numChequesValue
        End Get
        Set(ByVal value As Decimal)
            numChequesValue = value
        End Set
    End Property
    <Required()> _
    Public Property Grupo As String
        Get
            Return grupoValue
        End Get
        Set(ByVal value As String)
            grupoValue = value
        End Set
    End Property
    Public Property Obsoleto As Boolean
        Get
            Return obsoletoValue
        End Get
        Set(ByVal value As Boolean)
            obsoletoValue = value
        End Set
    End Property
End Class