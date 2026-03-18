Public Class CCGastoMODDAL
    ''''TODO: QUITAR TODOS LOS DATATABLES
    Public Function Obtener() As DataTable
        Dim query As String = "SELECT * FROM T_CtaContable_GastosMOD ORDER BY CC ASC"
        Return Utilidades.ObtenerQuerySQLSERVER("SELECT * FROM T_CtaContable_GastosMOD ORDER BY CC ASC")

    End Function

    Public Function ObtenerComboGastosMOD() As DataTable
        Dim query As String = "SELECT * FROM M_Gastos_MOD"
        Return Utilidades.ObtenerQuerySQLSERVER("SELECT * FROM M_Gastos_MOD")

    End Function

    Public Shared Sub Nuevo(CC As Integer, GastoMOD As String)
        Dim query As String = "INSERT INTO T_CtaContable_GastosMOD (CC, Gasto_MOD) VALUES ('" + CC.ToString() + "','" + GastoMOD.ToString() + "')"
        'Return Utilidades.ObtenerQuerySQLSERVER(query)
        Utilidades.EjecutarQuery(query)

    End Sub

    'Public Shared Function Actualizar(CC As Integer, GastoMOD As String, GastoMODID As Integer) As DataTable
    Public Shared Sub Actualizar(CC As Integer, GastoMOD As String)
        Dim query As String = "UPDATE T_CtaContable_GastosMOD SET Gasto_MOD = '" + GastoMOD.ToString() + "' WHERE CC = " + CC.ToString()
        'Return Utilidades.ObtenerQuerySQLSERVER(query)
        Utilidades.EjecutarQuery(query)
    End Sub

    Public Shared Sub Eliminar(Editando As String)
        Dim query As String = "DELETE FROM T_CtaContable_GastosMOD WHERE CC = " + Editando
        'Return Utilidades.ObtenerQuerySQLSERVER(query)
        Utilidades.EjecutarQuery(query)
    End Sub

End Class
