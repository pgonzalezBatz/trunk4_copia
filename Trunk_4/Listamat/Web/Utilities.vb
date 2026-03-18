Public Class Utilities
    Public Shared Function GetIDBInstance() As ListamatLib.IDBAccess
        Return New ListamatLib.DBAccess()
    End Function
    Public Shared Function GetSemanaAnual(ByVal d As DateTime) As String
        Dim c As Globalization.Calendar = Globalization.CultureInfo.CurrentCulture.Calendar
        Dim weekOfYear As String = c.GetWeekOfYear(d, Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.CalendarWeekRule, Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
        Return weekOfYear + "/" + c.GetYear(d).ToString
    End Function

    Public Shared Function GetTipoLista(ByVal tipolista As Integer) As String
        Select Case tipolista
            Case 1
                Return "Lista de Fundición"
            Case 2
                Return "Lista de Fabricación"
            Case 3
                Return "Lista de Adelanto"
        End Select
        Return ""
    End Function
End Class