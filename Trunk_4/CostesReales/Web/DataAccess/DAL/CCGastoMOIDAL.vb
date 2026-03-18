Public Class CCGastoMOIDAL
    ''''TODO: QUITAR TODOS LOS DATATABLES
    Public Function Obtener() As DataTable
        Dim query As String = "SELECT * FROM T_CtaContable_GastosMOI ORDER BY CC ASC"
        Return Utilidades.ObtenerQuerySQLSERVER("SELECT * FROM T_CtaContable_GastosMOI ORDER BY CC ASC")

    End Function

    Public Function ObtenerComboGastosMOI() As DataTable
        Dim query As String = "SELECT * FROM M_Gastos_MOI"
        Return Utilidades.ObtenerQuerySQLSERVER("SELECT * FROM M_Gastos_MOI")

    End Function

    Public Shared Function Nuevo(CC As Integer, GastoMOI As String) As DataTable
        Dim query As String = "INSERT INTO T_CtaContable_GastosMOI (CC, Gasto_MOI) VALUES ('" + CC.ToString() + "','" + GastoMOI.ToString() + "')"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    'Public Shared Function Actualizar(CC As Integer, GastoMOI As String, GastoMOIID As Integer) As DataTable
    Public Shared Function Actualizar(CC As Integer, GastoMOI As String) As DataTable
        Dim query As String = "UPDATE T_CtaContable_GastosMOI SET Gasto_MOI = '" + GastoMOI.ToString() + "' WHERE CC = " + CC.ToString()
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Eliminar(Editando As String) As DataTable
        Dim query As String = "DELETE FROM T_CtaContable_GastosMOI WHERE CC = " + Editando
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

End Class
