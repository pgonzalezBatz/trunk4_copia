Public Class PorcentajeETTReubicadosBLL

    Public Shared Function Obtener() As DataTable
        Dim db As New PorcentajeETTReubicadosDAL()
        Return db.Obtener()

    End Function
    'Public Shared Function ObtenerFiltrados(fecha As String, dpto As String) As DataTable
    Public Shared Function ObtenerFiltrados(paramFecha As Integer, paramDpto As Integer) As DataTable
        'Dim paramFecha As Integer = Integer.MinValue
        'Dim paramDpto As Integer = Integer.MinValue
        'If Not String.IsNullOrEmpty(fecha) Then
        '    Dim fechaDate As New Date(fecha.Substring(0, 4), fecha.Substring(5, 2), 1)
        '    fechaDate = fechaDate.AddMonths(1).AddDays(-1)
        '    paramFecha = fechaDate.Year * 10000 + fechaDate.Month * 100 + fechaDate.Day
        'End If
        'If Not String.IsNullOrEmpty(dpto) Then
        '    paramDpto = CInt(dpto)
        'End If
        Dim db As New PorcentajeETTReubicadosDAL()
        Return db.ObtenerFiltrados(paramFecha, paramDpto)
    End Function
    Public Shared Function Existe(Departamento As Integer, fecha As Integer) As DataTable
        Dim db As New PorcentajeETTReubicadosDAL()
        Return db.Existe(Departamento, fecha)

    End Function

    Public Shared Sub Nuevo(departamento As Integer, reubicados As Integer, ett As Integer, fecha As String)
        Dim db As New PorcentajeETTReubicadosDAL()
        db.Nuevo(departamento, reubicados, ett, fecha)

    End Sub
End Class
