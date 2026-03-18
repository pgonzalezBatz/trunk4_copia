Public Class PorcentajeDepartamentoBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New PorcentajeDepartamentoDAL()
        Return db.Obtener()

    End Function

    Public Shared Sub Nuevo(id As Integer, departamento As Integer, porcentaje As Integer)
        'Dim db As New PorcentajeDepartamentoDAL()
        'db.Nuevo(id, departamento, porcentaje)
        PorcentajeDepartamentoDAL.Nuevo(id, departamento, porcentaje)
    End Sub

End Class
