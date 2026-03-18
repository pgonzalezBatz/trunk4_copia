Public Class RPGenericoDAL

    Public Function Obtener() As DataTable
        Return Utilidades.ObtenerQuerySQLSERVER("SELECT * from T_RP_Genericos ")

    End Function

    Public Function ObtenerLantegi() As DataTable
        Return Utilidades.ObtenerQuerySQLSERVER("SELECT r.RP, r.Lantegi_id, d.LANTEGI FROM D_Business d INNER JOIN T_RP_Genericos r ON d.LANTEGI_ID = r.Lantegi_id")

    End Function

    Public Shared Function Actualizar(Editando As Integer, RP As String, Lantegi_ID As Integer, Lantegi As String) As DataTable
        Return Utilidades.ObtenerQuerySQLSERVER("UPDATE T_RP_Genericos SET RP = '" + RP + "', LANTEGI_ID = " + Lantegi.ToString() + " WHERE RP = " + RP.ToString())

    End Function

    Public Shared Function Actualizar(RP As String, Lantegi_ID As Integer, Lantegi As String) As DataTable
        Dim query = "UPDATE T_RP_Genericos SET LANTEGI_ID = " + Lantegi_ID.ToString() + " WHERE RP = '" + RP.ToString() + "'"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Eliminar(rpId As String, lantegiId As Integer) As DataTable
        Return Utilidades.ObtenerQuerySQLSERVER("DELETE FROM T_RP_Genericos WHERE RP = '" + rpId.ToString() + "' AND Lantegi_ID = '" + lantegiId.ToString() + "'")

    End Function

    Public Shared Function Nuevo(RP As String, Lantegi_id As Integer) As DataTable
        Dim query As String = "INSERT INTO T_RP_Genericos (RP, Lantegi_id) VALUES ('" + RP.ToString() + "','" + Lantegi_id.ToString() + "')"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

End Class
