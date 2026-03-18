Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations


Public Class Grupo
    <Required()> _
    Public Property Nombre As String
    Public Property image As Byte()
    Public Property Dias As Nullable(Of Integer)
End Class
Public Class Recurso
    <Required()> _
    Public Property nombreGrupo As String
    <Required()> _
    Public Property id As String
    Public Property descripcion As String
    <Required()> _
    Public Property planta As Integer
    Public Property excepcion As Boolean
    Public Property excepcionFecha As Nullable(Of DateTime)
End Class
Public Class Registro
    Inherits Recurso
    Public Property idSab As Integer
    Public Property coger As DateTime
    Public Property dejar As Nullable(Of DateTime)
End Class
Public Class RegistroDisplay
    Inherits Registro
    Public Property Nombre As String
    Public Property Apellido1 As String
    Public Property Apellido2 As String
End Class
Public Enum EfaRole
    touch = 1
    usuarioTemporal = 2
    jefegrupo = 4
    admin = 8
End Enum