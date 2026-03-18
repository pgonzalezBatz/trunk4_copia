Public Class CCGastoPorVentaBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New CCGastoPorVentaDAL()
        Return db.Obtener()

    End Function

    Public Shared Function ObtenerComboGastosVenta() As DataTable
        Dim db As New CCGastoPorVentaDAL()
        Return db.ObtenerComboGastosVenta()

    End Function

    Public Shared Function ObtenerComboTipoReparto() As DataTable
        Dim db As New CCGastoPorVentaDAL()
        Return db.ObtenerComboTipoReparto()

    End Function

    Public Shared Sub Nuevo(CC As Integer, partidaGasto As String, excepcionCarga As Boolean, tipoReparto As Integer)
        Dim db As New CCGastoPorVentaDAL()
        db.Nuevo(CC, partidaGasto, excepcionCarga, tipoReparto)
    End Sub

End Class
