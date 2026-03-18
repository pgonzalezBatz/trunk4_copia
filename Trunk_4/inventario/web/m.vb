Public Class Registro
    Public Property id As String
    Public Property idSab As Integer
End Class
Public Class Etiqueta
    Public Property id As Integer
    Public Property idTipo As Integer
    Public Property nombreTipo As String
    Public Property nombrePersonaAsignada As String
    Public Property apellido1PersonaAsignada As String
    Public Property apellido2PersonaAsignada As String
    Public Property listOfModelo As List(Of Modelo)
End Class
Public Class Modelo
    Public Property idTipo As Integer
    Public Property idMarca As Integer
    Public Property idModelo As Integer
    Public Property nombreTipo As String
    Public Property nombreMarca As String
    Public Property nombreModelo As String
    Public Property precio As Decimal
End Class

