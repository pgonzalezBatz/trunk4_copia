Imports System.ComponentModel.DataAnnotations

Public Enum roles
    normal = 1
End Enum
Public Class LoginUser
    <Required()>
    Public Property usuario As String
    <Required()>
    Public Property contraseña As String

    Public Property msg As String
End Class