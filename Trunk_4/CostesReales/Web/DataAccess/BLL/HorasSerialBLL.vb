Public Class HorasSerialBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New HorasSerialDAL()
        Return db.Obtener()
    End Function

    Public Shared Function ObtenerComboCriteriosReparto() As DataTable
        Dim db As New HorasSerialDAL()
        Return db.ObtenerComboCriteriosReparto()
    End Function

    Public Shared Function ObtenerPortador(Portador As String) As DataTable
        Dim db As New HorasSerialDAL()
        Return db.ObtenerPortador(Portador)
    End Function

    Public Shared Function ObtenerComboBusiness() As DataTable
        Dim db As New HorasSerialDAL()
        Return db.ObtenerComboBusiness()
    End Function

    Public Shared Function ObtenerComboMaquinas() As DataTable
        Dim db As New HorasSerialDAL()
        Return db.ObtenerComboMaquinas()
    End Function

    Public Shared Sub NuevoPortadorNegocio(Portador As String, Negocio As String)
        Dim db As New HorasSerialDAL()
        db.NuevoPortadorNegocio(Portador, Negocio)
    End Sub

    Public Shared Sub NuevoPortadorMaquina(Portador As String, Maquina As String)
        Dim db As New HorasSerialDAL()
        db.NuevoPortadorMaquina(Portador, Maquina)
    End Sub

    'Public Shared Sub ActualizarNegocio(Portador As String, CriterioReparto As Integer, Negocio As Integer, idPlanta As Integer)
    '    HorasSerialDAL.ActualizarNegocio(Portador, CriterioReparto, Negocio, idPlanta)

    'End Sub

    'Public Shared Sub ActualizarMaquina(Portador As String, CriterioReparto As Integer, Negocio As Integer, idPlanta As Integer)
    '    HorasSerialDAL.ActualizarNegocio(Portador, CriterioReparto, Negocio, idPlanta)

    'End Sub

    Public Shared Sub EliminarNegocio(Portador As String)
        HorasSerialDAL.EliminarNegocio(Portador)
    End Sub

    Public Shared Sub EliminarMaquina(Portador As String, Maquina As String)
        HorasSerialDAL.EliminarMaquina(Portador, Maquina)
    End Sub

End Class
