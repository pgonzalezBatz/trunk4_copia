Public Class LantegiBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New LantegiDAL()
        Return db.Obtener()

    End Function

    Public Shared Function ObtenerComboLantegis() As DataTable
        Dim db As New LantegiDAL()
        Return db.ObtenerComboLantegis()

    End Function

    Public Sub Nuevo(lantegi_ID As Integer, lantegi As String, grupo_producto As String)
        'Dim db As New LantegiDAL()
        'db.Nuevo(lantegi_ID, lantegi, grupo_producto)
        LantegiDAL.Nuevo(lantegi_ID, lantegi, grupo_producto)
    End Sub

    Public Shared Function Buscar(LantegiID As Integer, Lantegi As String) As DataTable
        Dim db As New LantegiDAL()
        Return db.Buscar(LantegiID, Lantegi)

    End Function

End Class
