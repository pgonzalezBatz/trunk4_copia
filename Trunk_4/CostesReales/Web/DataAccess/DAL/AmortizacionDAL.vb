Public Class AmortizacionDAL

    Public Function Obtener() As DataTable
        Dim query As String = "SELECT ACR.NUM_ACTIVO, ACR.Criterio_Reparto_ID, AC.Criterio_Reparto, ACR.Planta_ID, pl.PLANTA, ACR.Proceso_ID, PR.PROCESO
                               FROM T_Amortizaciones_Criterios AC 
                               RIGHT JOIN T_Amortizaciones_Criterios_Reparto_Activos ACR On AC.ID = ACR.Criterio_Reparto_ID
                               LEFT JOIN T_Plantas PL ON ACR.Planta_ID = PL.ID
                               LEFT JOIN T_Procesos PR ON ACR.Proceso_ID = PR.ID ORDER BY ACR.NUM_ACTIVO"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Actualizar(NUM_ACTIVO As String, Criterio_Reparto_ID As Integer, Planta_ID As Integer, Proceso_ID As Integer) As DataTable
        Dim query As String = "UPDATE T_Amortizaciones_Criterios_Reparto_Activos Set NUM_ACTIVO = '" + NUM_ACTIVO.ToString() + "', Criterio_Reparto_ID = '" + Criterio_Reparto_ID.ToString() + "', Planta_ID = '" + Planta_ID.ToString() + "',  Proceso_ID = '" + Proceso_ID.ToString() + "' WHERE NUM_ACTIVO = '" + NUM_ACTIVO.ToString() + "')"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Eliminar(NUM_ACTIVO As String) As DataTable
        Dim query As String = "DELETE FROM T_Amortizaciones_Criterios_Reparto_Activos WHERE NUM_ACTIVO = '" + NUM_ACTIVO.ToString() + "'"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Nuevo(NUM_ACTIVO As String, Criterio_Reparto_ID As Integer, Planta_ID As Integer, Proceso_ID As Integer) As DataTable
        Dim query As String = "INSERT INTO T_Amortizaciones_Criterios_Reparto_Activos (NUM_ACTIVO, Criterio_Reparto_ID, Planta_ID, Proceso_ID) VALUES ('" + NUM_ACTIVO.ToString() + "','" + Criterio_Reparto_ID.ToString() + "', '" + Planta_ID.ToString() + "', '" + Proceso_ID.ToString() + "')"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerComboCriterioReparto() As DataTable
        Dim query As String = "SELECT ID, CRITERIO_REPARTO FROM T_Amortizaciones_Criterios"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerComboPlanta() As DataTable
        Dim query As String = "SELECT ID, PLANTA FROM T_Plantas"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerComboProceso() As DataTable
        Dim query As String = "SELECT ID, PROCESO FROM T_Procesos"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

End Class
