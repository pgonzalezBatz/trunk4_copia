Public Class ProcesoDAL

    Public Function Obtener() As DataTable
        Dim query As String = "SELECT * FROM T_Procesos"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

End Class
