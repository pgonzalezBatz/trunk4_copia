Public Class RepartoActivosAmortizacionBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New RepartoActivosAmortizacionDAL()
        Return db.Obtener()

    End Function

    Public Shared Sub Nuevo(numActivo As String, criterioRepartoId As Integer, plantaId As Integer, procesoId As Integer, maquinaId As String)
        'Dim db As New RepartoActivosAmortizacionDAL()
        RepartoActivosAmortizacionDAL.Nuevo(numActivo, criterioRepartoId, plantaId, procesoId, maquinaId)

    End Sub

    Public Shared Function ObtenerComboCriterioReparto() As DataTable
        Dim db As New RepartoActivosAmortizacionDAL()
        Return db.ObtenerComboCriterioReparto()

    End Function

    Public Shared Function ObtenerComboPlanta() As DataTable
        Dim db As New RepartoActivosAmortizacionDAL()
        Return db.ObtenerComboPlanta()

    End Function

    Public Shared Function ObtenerComboProceso() As DataTable
        Dim db As New RepartoActivosAmortizacionDAL()
        Return db.ObtenerComboProceso()
    End Function
    Public Shared Function ObtenerComboNegocio() As DataTable
        Dim db As New RepartoActivosAmortizacionDAL()
        Return db.ObtenerComboNegocio()
    End Function

    Public Shared Function ObtenerComboMaquina() As DataTable
        Dim db As New RepartoActivosAmortizacionDAL()
        Return db.ObtenerComboMaquina()
    End Function

End Class
