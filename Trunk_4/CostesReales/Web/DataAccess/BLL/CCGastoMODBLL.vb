Public Class CCGastoMODBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New CCGastoMODDAL()
        Return db.Obtener()

    End Function

    Public Shared Function ObtenerComboGastosMOD() As DataTable
        Dim db As New CCGastoMODDAL()
        Return db.ObtenerComboGastosMOD()

    End Function

    Public Shared Sub Nuevo(CC As Integer, gastoMOD As String)
        'Dim db As New CCGastoMODDAL()
        'db.Nuevo(CC, gastoMOD)
        CCGastoMODDAL.Nuevo(CC, gastoMOD)
    End Sub

End Class
