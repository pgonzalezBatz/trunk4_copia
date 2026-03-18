Public Class PorcentajeDepartamentoDAL

    Public Function Obtener() As DataTable
        Dim query As String = "SELECT * FROM T_Dpto"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Nuevo(Id_Proceso As Integer, Id_Dpto As Integer, Porcentaje As Decimal) As DataTable
        Dim query As String = "INSERT INTO T_Dpto (Id, Dpto ) VALUES ('" + Id_Proceso.ToString() + "','" + Id_Dpto.ToString() + "')"
        If Not String.IsNullOrEmpty(Porcentaje) Then query += ", '" + Porcentaje + "'"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function


    Public Shared Function Eliminar(Id As String) As DataTable
        Dim query As String = ("DELETE FROM T_Proceso_Departamento WHERE ID_PROCESO = '" + Id + "'")
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerMaxId(Id As Integer) As DataTable
        Dim query As String = "SELECT MAX(Id) from T_Dpto"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

End Class
