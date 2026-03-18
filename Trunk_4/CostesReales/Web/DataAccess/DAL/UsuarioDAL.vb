Public Class UsuarioDAL

    Public Function Obtener(ByVal usuario As String) As DataTable
        Return Utilidades.ObtenerQuery("SELECT * FROM USUARIOS WHERE nombreusuario LIKE LOWER ('%" & usuario & "%')", False)

    End Function

End Class
