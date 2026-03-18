Public Class PorcentajeDAL

    Public Function Obtener() As DataTable
        Dim query As String = "SELECT * from T_PCT_MOI"
        Return Utilidades.ObtenerQuerySQLSERVER("SELECT * from T_PCT_MOI")

    End Function

    Public Shared Function Actualizar(NUM_ACTIVO As String, Criterio_Reparto_ID As Integer, Planta_ID As Integer, Proceso_ID As Integer) As DataTable
        Return Utilidades.ObtenerQuerySQLSERVER("UPDATE T_PCT_MOI SET NUM_ACTIVO = '" + NUM_ACTIVO.ToString() + "', Criterio_Reparto_ID = '" + Criterio_Reparto_ID.ToString() + "', Planta_ID = '" + Planta_ID.ToString() + "',  Proceso_ID = '" + Proceso_ID.ToString() + "' WHERE NUM_ACTIVO = '" + NUM_ACTIVO.ToString() + "'")

    End Function

    Public Shared Function Eliminar(NUM_ACTIVO As String) As DataTable
        Return Utilidades.ObtenerQuerySQLSERVER("DELETE FROM T_PCT_MOI WHERE NUM_ACTIVO = '" + NUM_ACTIVO.ToString() + "'")

    End Function

    Public Shared Function Nuevo(Dpto As Integer, Reubicados As Decimal, ETT As Decimal, Total As Decimal) As DataTable
        Dim query As String = "INSERT INTO T_PCT_MOI (DPTO, Reubicados (%), ETT (%), Total) VALUES ('" + Dpto.ToString() + "','" + Reubicados.ToString() + "', '" + ETT.ToString() + "', '" + Total.ToString() + "')"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

End Class
