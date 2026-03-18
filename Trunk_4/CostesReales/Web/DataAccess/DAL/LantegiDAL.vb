Public Class LantegiDAL

    Public Function Obtener() As DataTable
        Dim query As String = "SELECT * FROM D_Business"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerComboLantegis() As DataTable
        Dim query As String = "SELECT LANTEGI_ID, LANTEGI FROM D_Business"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Sub Nuevo(Lantegi_ID As Integer, Lantegi As String, Grupo_Producto As String)
        Utilidades.ObtenerQuerySQLSERVER("INSERT INTO D_Business (Lantegi_ID, Lantegi, Grupo_Producto) VALUES ('" + Lantegi_ID.ToString() + "','" + Lantegi + "', '" + Grupo_Producto.ToString() + "')")
    End Sub

    Public Shared Sub Actualizar(Lantegi_ID As Integer, Lantegi As String, Grupo_Producto As Integer)
        Utilidades.ObtenerQuerySQLSERVER("UPDATE D_Business SET Lantegi = '" + Lantegi.ToString() + "', Grupo_Producto = '" + Grupo_Producto.ToString() + "' WHERE Lantegi_ID = '" + Lantegi_ID.ToString() + "'")

    End Sub

    Public Shared Sub Eliminar(Lantegi_ID As Integer)
        Utilidades.ObtenerQuerySQLSERVER("DELETE FROM D_Business WHERE Lantegi_ID = '" + Lantegi_ID.ToString() + "'")
    End Sub

    Public Function Buscar(Lantegi_ID As Integer, Lantegi As String) As DataTable
        Dim query As String = "SELECT * FROM D_Business WHERE Lantegi_ID = " + Lantegi_ID.ToString() + " OR Lantegi = '" + Lantegi.ToString() + "'"
        Return Utilidades.ObtenerQuerySQLSERVER(query)
    End Function

End Class
