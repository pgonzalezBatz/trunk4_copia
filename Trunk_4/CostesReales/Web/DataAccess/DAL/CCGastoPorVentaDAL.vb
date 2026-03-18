Public Class CCGastoPorVentaDAL

    Public Function Obtener() As DataTable
        Dim query As String = "SELECT CG.CC, CG.Gastos_Venta, CG.Excepcion_Carga, TR.Tipo_Reparto, MPG.id as Gastos_Venta_id, MTR.ID as Tipo_Reparto_id FROM M_Tipos_Reparto TR
                               INNER JOIN T_CtaContable_GastosVenta CG ON TR.ID = CG.Tipo_Reparto 
							   INNER JOIN M_Partidas_Gasto MPG ON  MPG.Partida_Gasto = CG.Gastos_Venta
							   INNER JOIN M_Tipos_Reparto MTR ON TR.Tipo_Reparto = MTR.Tipo_Reparto
							   ORDER BY CC ASC"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerPartidaGasto(partidaGastoId As Integer) As DataTable
        Dim query As String = "SELECT PARTIDA_GASTO FROM M_Partidas_Gasto WHERE ID = '" + partidaGastoId + "'"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerComboGastosVenta() As DataTable
        Dim query As String = "SELECT * from M_Partidas_Gasto"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerComboTipoReparto() As DataTable
        Dim query As String = "SELECT * from M_Tipos_Reparto"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Sub Eliminar(Editando As String)
        Dim query As String = "DELETE FROM T_CtaContable_GastosVenta WHERE CC = '" + Editando + "'"
        Utilidades.ObtenerQuerySQLSERVER(query)

    End Sub

    Public Sub Nuevo(CC As Integer, GastosVenta As String, excepcionCarga As Boolean, TipoReparto As Integer)
        Dim query As String = "INSERT INTO T_CtaContable_GastosVenta (CC, Gastos_Venta, Excepcion_Carga, Tipo_reparto) VALUES ('" + CC.ToString() + "','" + GastosVenta.ToString() + "', '" + excepcionCarga.ToString() + "', '" + TipoReparto.ToString() + "')"
        Utilidades.ObtenerQuerySQLSERVER(query)
    End Sub

    Public Shared Sub Actualizar(CC As Integer, GastosVenta As String, ExcepcionCarga As Boolean, TipoReparto As String, ccEditando As Integer)
        Dim query As String = "UPDATE T_CtaContable_GastosVenta SET Gastos_Venta = '" + GastosVenta.ToString() + "', Excepcion_Carga = '" + ExcepcionCarga.ToString() + "', Tipo_Reparto = '" + TipoReparto.ToString() + "'  WHERE CC = " + CC.ToString()
        Utilidades.ObtenerQuerySQLSERVER(query)
    End Sub

End Class
