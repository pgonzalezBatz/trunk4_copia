Public Class DiferenciaInventarioDAL

    Public Function Obtener() As DataTable
        Dim query As String = "SELECT * from T_Diferencia_Inventario"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function
    Public Function ObtenerFiltrados(paramFecha As Integer) As DataTable
        Dim query As String = "SELECT * from T_Diferencia_Inventario where fecha_id = @FECHA"
        Dim p1 = New SqlClient.SqlParameter("@FECHA", SqlDbType.Int, ParameterDirection.Input) With {.Value = paramFecha}
        Dim datos As New DataTable()
        Dim ms As New MPCR
        Using connection As New SqlClient.SqlConnection(ms.Cx)
            Dim cmd As New SqlClient.SqlCommand(query, connection)
            cmd.Connection.Open()
            If p1 IsNot Nothing Then cmd.Parameters.Add(p1)
            Dim da As New SqlClient.SqlDataAdapter(cmd)
            da.Fill(datos)
            connection.Close()
            da.Dispose()
        End Using

        Return datos

    End Function

    Public Shared Function Actualizar(Fecha_ID As Integer, Categoria As String, Referencia As String, Precio_Inventario As Decimal) As DataTable
        Return Utilidades.ObtenerQuerySQLSERVER("UPDATE T_Diferencia_Inventario SET FECHA_ID = '" + Fecha_ID.ToString() + "', Categoria = '" + Categoria.ToString() + "', Referencia = '" + Referencia.ToString() + "' WHERE Referencia = '" + Referencia.ToString() + "'")

    End Function

    Public Shared Function Eliminar(Fecha_ID As Integer, Categoria As String, Referencia As String, Precio_Inventario As Decimal) As DataTable
        Return Utilidades.ObtenerQuerySQLSERVER("DELETE FROM T_Diferencia_Inventario WHERE REFERENCIA = '" + Referencia.ToString() + "' AND Categoria = '" + Categoria.ToString() + "'")

    End Function

    Public Sub Nuevo(Fecha_ID As Integer, Categoria As Integer, Referencia As String, Precio_Inventario As Decimal)
        Dim query As String = "INSERT INTO T_Diferencia_Inventario (Fecha_ID, Categoria, Referencia, Precio_Inventario) VALUES ('" + Fecha_ID.ToString() + "','" + Categoria.ToString() + "', '" + Referencia.ToString() + "', '" + Precio_Inventario.ToString() + "')"
        Utilidades.ObtenerQuerySQLSERVER(query)
    End Sub


End Class
