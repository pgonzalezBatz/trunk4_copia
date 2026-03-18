Public Class DiferenciaInventarioBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New DiferenciaInventarioDAL()
        Return db.Obtener()

    End Function
    Public Shared Function ObtenerFiltrados(paramFecha As Integer) As DataTable
        Dim db As New DiferenciaInventarioDAL()
        Return db.ObtenerFiltrados(paramFecha)

    End Function

    Public Sub Nuevo(fecha_id As Integer, categoria As String, referencia As String, precio_inventario As Decimal)
        Dim db As New DiferenciaInventarioDAL()
        db.Nuevo(fecha_id, categoria, referencia, precio_inventario)

    End Sub


End Class
