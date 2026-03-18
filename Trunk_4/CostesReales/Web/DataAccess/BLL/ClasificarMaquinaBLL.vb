Public Class ClasificarMaquinaBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New ClasificarMaquinaDAL()
        Return db.Obtener()

    End Function

    Public Shared Function ObtenerComboProcesos() As DataTable
        Dim db As New ClasificarMaquinaDAL()
        Return db.ObtenerComboProcesos()

    End Function

    Public Shared Function ObtenerComboPlantas() As DataTable
        Dim db As New ClasificarMaquinaDAL()
        Return db.ObtenerComboPlantas()

    End Function

    Public Shared Sub Nuevo(maquina As String, descripcion As String, proceso As Integer, planta As Integer, kwh As Integer)
        Dim db As New ClasificarMaquinaDAL()
        db.Nuevo(maquina, descripcion, proceso, planta, kwh)

    End Sub

End Class
