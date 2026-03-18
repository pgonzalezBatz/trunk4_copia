Public Class InventarioAjusteManualDAL

    Public Function Obtener() As DataTable
        Return Utilidades.ObtenerQuerySQLSERVER("SELECT * from T_Diferencia_Inventario_Ajuste_Manual")

    End Function

    Public Shared Function Actualizar(Fecha_id As Integer, Referencia As String, Unidades_Ajuste As Integer) As DataTable
        Return Utilidades.ObtenerQuerySQLSERVER("UPDATE T_Diferencia_Inventario_Ajuste_Manual SET Fecha_id = '" + Fecha_id.ToString() + "', Referencia = '" + Referencia.ToString() + "', Unidades_Ajuste = '" + Unidades_Ajuste.ToString() + "' WHERE Fecha_id = '" + Fecha_id.ToString + "'")

    End Function

    Public Shared Function Eliminar(Fecha_id As Integer) As DataTable
        Return Utilidades.ObtenerQuerySQLSERVER("DELETE FROM T_Diferencia_Inventario_Ajuste_Manual WHERE Fecha_id = '" + Fecha_id.ToString() + "'")

    End Function

    Public Shared Function Nuevo(Fecha_id As Integer, Referencia As String, Unidades_Ajuste As Integer) As DataTable
        Dim query As String = "INSERT INTO T_Diferencia_Inventario_Ajuste_Manual (Fecha_id, Referencia, Unidades_Ajuste) VALUES ('" + Fecha_id.ToString() + "', '" + Referencia.ToString() + "', '" + Unidades_Ajuste.ToString() + "')"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

End Class
