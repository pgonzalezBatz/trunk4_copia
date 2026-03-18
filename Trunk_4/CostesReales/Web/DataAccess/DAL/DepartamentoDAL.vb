Public Class DepartamentoDAL

    Public Function Obtener() As DataTable
        Dim query As String = "SELECT * FROM T_Dpto"
        Return Utilidades.ObtenerQuerySQLSERVER("SELECT * FROM T_Dpto")

    End Function

    Public Shared Function Actualizar(DepartamentoId As Integer, Departamento As Integer) As DataTable
        Dim query As String = "UPDATE T_Dpto SET DPTO = '" + Departamento.ToString() + "' WHERE ID = " + DepartamentoId.ToString() + ""
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Eliminar(Id As Integer) As DataTable
        Return Utilidades.ObtenerQuerySQLSERVER("DELETE FROM T_Dpto WHERE id = " + Id.ToString())

    End Function

    Public Shared Function Nuevo(Departamento As Integer, Id As Integer) As DataTable
        Dim query As String = "INSERT INTO T_Dpto (Id, Dpto ) VALUES ('" + Id.ToString() + "','" + Departamento.ToString() + "')"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerMaxId(Id As Integer) As DataTable
        Dim query As String = "SELECT MAX(Id) from T_Dpto"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

End Class
