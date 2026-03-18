Public Class helpers

    Public Shared Function UpRoundedDouble(ByVal str As String) As Double
        str = str.Replace(".", ",")
        Return Math.Round(Double.Parse(str, System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")), 0, MidpointRounding.AwayFromZero)
    End Function
End Class
