Public Class DepartamentoBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New DepartamentoDAL()
        Return db.Obtener()

    End Function

    Public Shared Sub Nuevo(departamento As Integer, Id As Integer)
        Dim db As New DepartamentoDAL()
        db.Nuevo(departamento, Id)

    End Sub

End Class
