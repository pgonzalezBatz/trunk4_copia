Public Class RepartoMOIBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New RepartoMOIDAL()
        Return db.Obtener()

    End Function

    Public Shared Sub Nuevo(departamento As Integer, Id As Integer)
        'Dim db As New RepartoMOIDAL()
        'db.Nuevo(departamento, Id)
        RepartoMOIDAL.Nuevo(departamento, Id)
    End Sub

End Class
