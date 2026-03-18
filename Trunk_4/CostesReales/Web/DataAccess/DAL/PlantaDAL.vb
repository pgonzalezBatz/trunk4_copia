Public Class PlantaDAL

    Public Function Obtener() As DataTable
        Return Utilidades.ObtenerQuery("SELECT NOMBRE FROM PLANTAS", False)

    End Function

End Class
