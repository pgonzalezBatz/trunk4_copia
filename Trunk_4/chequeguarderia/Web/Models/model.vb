Public Class Guarderia
    Private idValue As Integer
    Private nombreValue As String
    Private cpValue As String
    Private tipoguarderiaValue As String
    Private idGourmetValue As String
    Private direccionValue As String
    Private poblacionValue As String
    Private provinciaValue As String
    Private telcentroValue As String
    Private mailcentroValue As String
    Private rescentroValue As String
    Public Property Id As Integer
        Get
            Return idValue
        End Get
        Set(ByVal value As Integer)
            idValue = value
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
    Public Property CP As String
        Get
            Return cpValue
        End Get
        Set(ByVal value As String)
            cpValue = value
        End Set
    End Property
    Public Property TipoGuarderia As String
        Get
            Return tipoguarderiaValue
        End Get
        Set(ByVal value As String)
            tipoguarderiaValue = value
        End Set
    End Property
    Public Property IdGourmet As String
        Get
            Return idGourmetValue
        End Get
        Set(ByVal value As String)
            idGourmetValue = value
        End Set
    End Property
    Public Property Direccion As String
        Get
            Return direccionValue
        End Get
        Set(ByVal value As String)
            direccionValue = value
        End Set
    End Property
    Public Property Poblacion As String
        Get
            Return poblacionValue
        End Get
        Set(ByVal value As String)
            poblacionValue = value
        End Set
    End Property
    Public Property Provincia As String
        Get
            Return provinciaValue
        End Get
        Set(ByVal value As String)
            provinciaValue = value
        End Set
    End Property
    Public Property TelCentro As String
        Get
            Return telcentroValue
        End Get
        Set(ByVal value As String)
            telcentroValue = value
        End Set
    End Property
    Public Property MailCentro As String
        Get
            Return mailcentroValue
        End Get
        Set(ByVal value As String)
            mailcentroValue = value
        End Set
    End Property
    Public Property ResCentro As String
        Get
            Return rescentroValue
        End Get
        Set(ByVal value As String)
            rescentroValue = value
        End Set
    End Property
End Class
Public Class SolicitudUI
    Private idSabValue As Integer
    Private nifHijaValue As String
    Private ejercicioValue As Integer
    Private mesValue As List(Of Integer)
    Private importeValue As Decimal
    Private idValue As Integer
    Private nombreValue As String
    Private cpValue As String
    Private tipoguarderiaValue As String
    Private idGourmetValue As String
    Private direccionValue As String
    Private poblacionValue As String
    Private provinciaValue As String
    Private telcentroValue As String
    Private mailcentroValue As String
    Private rescentroValue As String
    <Required()>
    Public Property IdSab As Integer
        Get
            Return idSabValue
        End Get
        Set(ByVal value As Integer)
            idSabValue = value
        End Set
    End Property
    <Required()> _
    Public Property NifHija As String
        Get
            Return nifHijaValue
        End Get
        Set(ByVal value As String)
            nifHijaValue = value
        End Set
    End Property
    <Required()> _
    <RegularExpression("^[\d]{4}$")> _
    Public Property Ejercicio As Integer
        Get
            Return ejercicioValue
        End Get
        Set(ByVal value As Integer)
            ejercicioValue = value
        End Set
    End Property
    <Required()> _
    Public Property Mes As List(Of Integer)
        Get
            Return mesValue
        End Get
        Set(ByVal value As List(Of Integer))
            mesValue = value
        End Set
    End Property
    <Required()> _
    Public Property Importe As Decimal
        Get
            Return importeValue
        End Get
        Set(ByVal value As Decimal)
            importeValue = value
        End Set
    End Property
    Public Property Id As Integer
        Get
            Return idValue
        End Get
        Set(ByVal value As Integer)
            idValue = value
        End Set
    End Property
    <Required()>
    Public Property Nombre As String
        Get
            Return nombreValue
        End Get
        Set(ByVal value As String)
            nombreValue = value
        End Set
    End Property
    Public Property CP As String
        Get
            Return cpValue
        End Get
        Set(ByVal value As String)
            cpValue = value
        End Set
    End Property
    <Required()> _
    Public Property TipoGuarderia As String
        Get
            Return tipoguarderiaValue
        End Get
        Set(ByVal value As String)
            tipoguarderiaValue = value
        End Set
    End Property
    Public Property IdGourmet As String
        Get
            Return idGourmetValue
        End Get
        Set(ByVal value As String)
            idGourmetValue = value
        End Set
    End Property
    <Required()>
    Public Property Direccion As String
        Get
            Return direccionValue
        End Get
        Set(ByVal value As String)
            direccionValue = value
        End Set
    End Property
    Public Property Poblacion As String
        Get
            Return poblacionValue
        End Get
        Set(ByVal value As String)
            poblacionValue = value
        End Set
    End Property
    Public Property Provincia As String
        Get
            Return provinciaValue
        End Get
        Set(ByVal value As String)
            provinciaValue = value
        End Set
    End Property
    Public Property TelCentro As String
        Get
            Return telcentroValue
        End Get
        Set(ByVal value As String)
            telcentroValue = value
        End Set
    End Property
    Public Property MailCentro As String
        Get
            Return mailcentroValue
        End Get
        Set(ByVal value As String)
            mailcentroValue = value
        End Set
    End Property
    Public Property ResCentro As String
        Get
            Return rescentroValue
        End Get
        Set(ByVal value As String)
            rescentroValue = value
        End Set
    End Property
End Class
Public Class SolicitudKey
    Private ejercicioValue As Integer
    Private rangoValue As Integer
    Private nombreGuarderiaValue As String
    Public Property Ejercicio As Integer
        Get
            Return ejercicioValue
        End Get
        Set(ByVal value As Integer)
            ejercicioValue = value
        End Set
    End Property
    Public Property Rango As Integer
        Get
            Return rangoValue
        End Get
        Set(ByVal value As Integer)
            rangoValue = value
        End Set
    End Property
    Public Property NombreGuarderia As String
        Get
            Return nombreGuarderiaValue
        End Get
        Set(ByVal value As String)
            nombreGuarderiaValue = value
        End Set
    End Property
End Class
Public Class Parametros
    Private limiteRangoValue As Decimal
    Private rangoActualValue As Integer
    Private EjercicioActualValue As Integer
    Private porcentajeTramiteValue As Decimal
    Private publicoMensualValue As Decimal
    Public Property LimiteRango As Decimal
        Get
            Return limiteRangoValue
        End Get
        Set(ByVal value As Decimal)
            limiteRangoValue = value
        End Set
    End Property
    Public Property RangoActual As Integer
        Get
            Return rangoActualValue
        End Get
        Set(ByVal value As Integer)
            rangoActualValue = value
        End Set
    End Property
    Public Property EjercicioActual As Integer
        Get
            Return EjercicioActualValue
        End Get
        Set(ByVal value As Integer)
            EjercicioActualValue = value
        End Set
    End Property
    Public Property PorcentajeTramite As Decimal
        Get
            Return porcentajeTramiteValue
        End Get
        Set(ByVal value As Decimal)
            porcentajeTramiteValue = value
        End Set
    End Property
    Public Property PublicoMensual As Decimal
        Get
            Return publicoMensualValue
        End Get
        Set(ByVal value As Decimal)
            publicoMensualValue = value
        End Set
    End Property
End Class

Public Enum Role
    normal = 1
    administracion = 2
End Enum