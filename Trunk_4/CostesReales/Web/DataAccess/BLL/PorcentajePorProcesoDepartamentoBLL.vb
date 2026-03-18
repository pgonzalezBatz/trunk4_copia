Public Class PorcentajePorProcesoDepartamentoBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New PorcentajePorProcesoDepartamentoDAL()
        Return db.Obtener()

    End Function

    Public Shared Sub Eliminar(IdProceso As Integer)
        'Dim db As New PorcentajePorProcesoDepartamentoDAL()
        'db.Eliminar(IdProceso)
        PorcentajePorProcesoDepartamentoDAL.Eliminar(IdProceso)
    End Sub

    Public Shared Sub Nuevo(IdProceso As Integer, IdDpto As Integer, Porcentaje As Decimal)
        'Dim db As New PorcentajePorProcesoDepartamentoDAL()
        'db.Nuevo(IdProceso, IdDpto, Porcentaje)
        PorcentajePorProcesoDepartamentoDAL.Nuevo(IdProceso, IdDpto, Porcentaje)
    End Sub


End Class
