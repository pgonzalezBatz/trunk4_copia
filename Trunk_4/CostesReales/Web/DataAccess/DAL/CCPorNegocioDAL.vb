Public Class CCPorNegocioDAL

    Public Function Obtener() As DataTable
        Dim query As String = "SELECT * from T_CtaContable_Negocio"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerLantegi() As DataTable
        Dim query As String = "SELECT DISTINCT t.CC, t.Lantegi_id, t.Aplica_Ventas, d.LANTEGI FROM D_Business d 
                               INNER JOIN T_CtaContable_Negocio t ON d.LANTEGI_ID = t.Lantegi_id ORDER BY CC ASC"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerComboLantegis() As DataTable
        Dim query As String = "SELECT LANTEGI, LANTEGI_ID from D_Business"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Actualizar(CC As Integer, Lantegi_id As String, Aplica_Ventas As Boolean, lantegiEditando As String) As DataTable
        Dim query As String = "UPDATE T_CtaContable_Negocio SET Lantegi_ID = '" + Lantegi_id.ToString() + "', Aplica_Ventas = '" + Aplica_Ventas.ToString() + "' WHERE CC = '" + CC.ToString() + "'"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Eliminar(Editando As String) As DataTable
        Dim query As String = "DELETE FROM T_CtaContable_Negocio WHERE CC = '" + Editando + "'"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Sub Nuevo(CC As Integer, Lantegi_id As Integer, Aplica_Ventas As Boolean)
        Dim query As String = "INSERT INTO T_CtaContable_Negocio (CC, Lantegi_id, Aplica_Ventas) VALUES ('" + CC.ToString() + "','" + Lantegi_id.ToString() + "', '" + Aplica_Ventas.ToString() + "')"
        Utilidades.ObtenerQuerySQLSERVER(query)
    End Sub

End Class