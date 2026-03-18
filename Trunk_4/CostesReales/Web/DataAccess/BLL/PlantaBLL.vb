Public Class PlantaBLL

    Public Shared Function Obtener() As DataTable

        Dim db As PlantaDAL = New PlantaDAL()
        Return db.Obtener()

    End Function

End Class
