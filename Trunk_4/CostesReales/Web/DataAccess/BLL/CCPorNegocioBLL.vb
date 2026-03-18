Public Class CCPorNegocioBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New CCPorNegocioDAL()
        Return db.Obtener()

    End Function

    Public Shared Function ObtenerLantegi() As DataTable
        Dim db As New CCPorNegocioDAL()
        Return db.ObtenerLantegi()

    End Function

    Public Shared Function ObtenerComboLantegis() As DataTable
        Dim db As New CCPorNegocioDAL()
        Return db.ObtenerComboLantegis()

    End Function

    Public Sub Nuevo(CC As Integer, Lantegi_id As Integer, Aplica_Ventas As Boolean)
        Dim db As New CCPorNegocioDAL()
        db.Nuevo(CC, Lantegi_id, Aplica_Ventas)
    End Sub

End Class
