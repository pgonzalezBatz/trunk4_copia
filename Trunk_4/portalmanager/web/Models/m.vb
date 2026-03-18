Imports System.ComponentModel.DataAnnotations
Imports ExpressiveAnnotations.Attributes
Public Class solicitud
    Public Property id As String
    <Required()> _
    Public Property negocio As String
    <Required()> _
    Public Property departamento As String
    <Range(1, 20)> _
    Public Property nPersonas As Integer
    <Required()> _
    Public Property responsable As Integer
    <Required()> _
    <StringLength(1000)> _
    Public Property descripcion As String
    <Required()> _
    <StringLength(100)> _
    Public Property especialidad As String
    <Required()> _
    <StringLength(200)> _
    Public Property conocimientos As String
    <StringLength(100)> _
    Public Property idiomas As String
    <StringLength(100)> _
    Public Property experiencia As String
    <Required()> _
    <StringLength(200)> _
    Public Property duracion As String
    <Required()> _
    Public Property fecha As Date
    <Required()>
    <StringLength(30)>
    Public Property Horario As String
    Public Property Creador As Integer
    Public Property NombreResponsable As String
    Public Property Apellido1Responsable As String
    Public Property Apellifdo2Responsable As String
    Public Property NombreDepartamento As String
    Public Property NombreNegocio As String
    Public Property FechaIncorporacion As Nullable(Of Date) 'Consideraremos esta fecha para cerrar el proceso
    Public Property FechaCreacion As Date
    Public Property ListOfValidacion As List(Of Validacion)
    Public Property UltimaValidacion As Nullable(Of DateTime)
    Public Property DatosIncorporacion As String
    Public Property ResponsableCierre As String
End Class
Public Class coberturaPuesto
    Inherits solicitud
    <Required(AllowEmptyStrings:=False, ErrorMessage:="Es necesario indicar si la solicitud entra en el plan de gestión")> _
    Public Property pgestion As Nullable(Of Boolean)
    <Required(AllowEmptyStrings:=False, ErrorMessage:="Es necesario indicar si es un puesto estructural")> _
    Public Property pestructural As Nullable(Of Boolean)
    <StringLength(300)> _
    Public Property puesto As String
    <StringLength(300)> _
    Public Property formacion As String
    <StringLength(300)> _
    Public Property formacion2 As String
    <StringLength(300)> _
    Public Property conocimientos2 As String
    <StringLength(100)> _
    Public Property idiomas2 As String
    <StringLength(300)> _
    Public Property experiencia2 As String
End Class
Public Class becaria
    Inherits solicitud
    <StringLength(100)> _
    Public Property universidad As String
    <StringLength(100)> _
    Public Property titulacion As String
End Class
Public Class Validacion
    <Required()> _
    Public Property idSab As Integer
    Public Property orden As Integer
    Public Property fechaValidacion As Nullable(Of DateTime)
    Public Property fechaRechazo As Nullable(Of DateTime)
    Public Property nombre As String
    Public Property apellido1 As String
    Public Property apellido2 As String
End Class
Public Enum Role
    responsable = 1
    rrhh = 4
    eki = 8
    excedencia = 16
    departamento = 32
    bajasSinEvolucion = 64
    altasBajas = 128
End Enum
Public Enum TipoPregunta
    puntuacion = 1
    libre = 2
End Enum
Public Class Pregunta
    Public Property id As Integer
    <Required()> _
    Public Property idFormulario As Integer
    <Required()> _
    Public Property titulo As String
    <Required()> _
    Public Property descripcion As String
    <Required()> _
    Public Property tipoPregunta As TipoPregunta
    Public Property respuestasPosibles As List(Of RespuestaPosible)
    Public Property respuesta As Respuesta
    Public Property peso As Nullable(Of Decimal)
End Class
Public Class RespuestaPosible
    Public Property id As Integer
    <Required()> _
    Public Property puntuacion As Decimal
    <Required()> _
    Public Property descripcion As String
End Class
Public Class Respuesta
    Public Property id As Integer
    <Required()> _
    Public Property idSab As Integer
    <Required()> _
    Public Property fecha As DateTime
    <Required()> _
    Public Property tituloPregunta As String
    <Required()> _
    Public Property descripcionPregunta As String
    <Required()> _
    Public Property tipopregunta As TipoPregunta
    Public Property pesoPregunta As Decimal
    Public Property puntuacion As Decimal
    Public Property puntuacionMax As Decimal
    Public Property texto As String
    <Required()> _
    Public Property fechaVencimiento As DateTime
    Public Property idFormulario As Integer
End Class
Public Class propuestaContinuidad
    Public Property id As Integer
    Public Property fechaVencimiento As DateTime
    Public Property idSab As Integer
    Public Property continua As Boolean
    Public Property duracion As String
    <RequiredIf("continua", ErrorMessage:="Necesario introducir indice")>
    <Range(0.01, 7.0)>
    Public Property indice As Nullable(Of Decimal)
    <StringLength(300)> _
    Public Property motivo As String
End Class
Public Class ConsolidacionCambioPuesto
    Public Property id As Integer
    Public Property fechaVencimiento As DateTime
    Public Property idSab As Integer
    Public Property continua As Boolean
    Public Property motivo As String
    Public Property indice As Nullable(Of Decimal)
End Class
Public Class FormularioVencimientos
    Public Property idFormulario As Integer
    Public Property lstVencimiento As List(Of String)
End Class

