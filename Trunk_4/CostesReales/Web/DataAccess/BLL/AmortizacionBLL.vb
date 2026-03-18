Public Class AmortizacionBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New AmortizacionDAL()
        Return db.Obtener()

    End Function

    Public Shared Sub Nuevo(numActivo As String, criterioRepartoId As Integer, plantaId As Integer, procesoId As Integer)
        Dim db As New AmortizacionDAL()
        db.Nuevo(numActivo, criterioRepartoId, plantaId, procesoId)

    End Sub

    Public Shared Function ObtenerComboCriterioReparto() As DataTable
        Dim db As New AmortizacionDAL()
        Return db.ObtenerComboCriterioReparto()

    End Function

    Public Shared Function ObtenerComboPlanta() As DataTable
        Dim db As New AmortizacionDAL()
        Return db.ObtenerComboPlanta()

    End Function

    Public Shared Function ObtenerComboProceso() As DataTable
        Dim db As New AmortizacionDAL()
        Return db.ObtenerComboProceso()

    End Function

End Class
