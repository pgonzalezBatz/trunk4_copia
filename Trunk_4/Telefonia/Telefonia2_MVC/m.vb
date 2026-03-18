Public Class persona
    Public Property id As Integer
    Public Property nombre As String
    Public Property apellido1 As String
    Public Property apellido2 As String
    Public Property nikEuskaraz As Boolean
    Public Property departamento As String
    Public Property dni As String
    Public Property idPlanta As Integer
    Public Property lstExtension As IEnumerable(Of Extension)
    Public Property idDepartamento As String
End Class

Public Class Extension
    Public Property extensionInterna As String
    Public Property extensionExterna As String
    Public Property numero As String
    Public Property tipoLinea As String
    Public Property internoDirecto As String
End Class

Public Enum role
    normal = 1
    redirecion = 2
End Enum