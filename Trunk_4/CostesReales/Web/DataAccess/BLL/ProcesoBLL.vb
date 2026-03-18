Public Class ProcesoBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New ProcesoDAL()
        Return db.Obtener()

    End Function

End Class
