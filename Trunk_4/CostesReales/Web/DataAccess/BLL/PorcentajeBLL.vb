Public Class PorcentajeBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New PorcentajeDAL()
        Return db.Obtener()

    End Function

    Public Shared Sub Nuevo(Dpto As Integer, Reubicados As Decimal, ETT As Decimal, Total As Decimal)
        Dim db As New PorcentajeDAL()
        db.Nuevo(Dpto, Reubicados, ETT, Total)

    End Sub

End Class
