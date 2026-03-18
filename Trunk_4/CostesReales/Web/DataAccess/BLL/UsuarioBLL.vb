Public Class UsuarioBLL

    Public Shared Function Obtener(ByVal usuario As String) As DataTable
        Dim db As UsuarioDAL = New UsuarioDAL()
        Return db.Obtener(usuario)

    End Function

End Class
