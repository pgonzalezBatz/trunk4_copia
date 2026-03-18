Public Class InventarioAjusteManualBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New InventarioAjusteManualDAL()
        Return db.Obtener()

    End Function

    Public Shared Sub Nuevo(Fecha_Id As Integer, Referencia As String, UnidadAjuste As Integer)
        'Dim db As New InventarioAjusteManualDAL()
        'db.Nuevo(Fecha_Id, Referencia, UnidadAjuste)
        InventarioAjusteManualDAL.Nuevo(Fecha_Id, Referencia, UnidadAjuste)
    End Sub

End Class
