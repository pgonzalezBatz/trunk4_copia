Public Class ListadoMaquinasBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New ListadoMaquinasDAL()
        Return db.Obtener()

    End Function

End Class
