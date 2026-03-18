Public Class CCGastoMOIBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New CCGastoMOIDAL()
        Return db.Obtener()

    End Function

    Public Shared Function ObtenerComboGastosMOI() As DataTable
        Dim db As New CCGastoMOIDAL()
        Return db.ObtenerComboGastosMOI()

    End Function

    Public Shared Sub Nuevo(CC As Integer, gastoMOI As String)
        'Dim db As New CCGastoMOIDAL()
        'db.Nuevo(CC, gastoMOI)
        CCGastoMOIDAL.Nuevo(CC, gastoMOI)
    End Sub

End Class
