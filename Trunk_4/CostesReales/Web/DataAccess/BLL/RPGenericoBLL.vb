Public Class RPGenericoBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New RPGenericoDAL()
        Return db.Obtener()

    End Function

    Public Shared Function ObtenerLantegi() As DataTable
        Dim db As New RPGenericoDAL()
        Return db.ObtenerLantegi()

    End Function

    Public Shared Sub Nuevo(RP As String, Lantegi_id As Integer)
        'Dim db As New RPGenericoDAL()
        'db.Nuevo(RP, Lantegi_id)
        RPGenericoDAL.Nuevo(RP, Lantegi_id)
    End Sub

End Class
