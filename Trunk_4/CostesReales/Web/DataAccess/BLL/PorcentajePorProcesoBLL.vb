Public Class PorcentajePorProcesoBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New PorcentajePorProcesoDAL()
        Return db.Obtener()

    End Function

    Public Shared Sub Nuevo(NUM_ACTIVO As String, Criterio_Reparto_ID As Integer, Planta_ID As Integer, Proceso_ID As Integer)
        'Dim db As New PorcentajePorProcesoDAL()
        'db.Nuevo(NUM_ACTIVO, Criterio_Reparto_ID, Planta_ID, Proceso_ID)
        PorcentajePorProcesoDAL.Nuevo(NUM_ACTIVO, Criterio_Reparto_ID, Planta_ID, Proceso_ID)
    End Sub

End Class
