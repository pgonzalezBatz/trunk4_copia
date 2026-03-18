Public Class TestCookies
    Inherits Page

    Private Sub btnGet_Click(sender As Object, e As EventArgs) Handles btnGet.Click
        lblCookieLangile.Text = GetVisualizaciones()
    End Sub

    Public Function GetVisualizaciones() As Integer
        Dim numTrab As Integer = CType(Session("Ticket"), Sablib.ELL.Ticket).IdTrabajador
        Dim nombreCookie = "AholkuLangile_" & numTrab
        If (Request.Cookies(nombreCookie) IsNot Nothing) Then
            Dim cookie As HttpCookie = Request.Cookies(nombreCookie)
            Return cookie.Value
        Else
            Return 0
        End If
    End Function

    Private Sub btnUp_Click(sender As Object, e As EventArgs) Handles btnUp.Click
        Dim cookie As HttpCookie = Nothing
        Dim numTrab As Integer = CType(Session("Ticket"), Sablib.ELL.Ticket).IdTrabajador
        Dim nombreCookie = "AholkuLangile_" & numTrab
        GetVisualizaciones()
        Dim numVisualizaciones As Integer = GetVisualizaciones() + 1
        If (Request.Cookies(nombreCookie) IsNot Nothing) Then
            cookie = Request.Cookies(nombreCookie)
            cookie.Value = numVisualizaciones
            Response.Cookies.Set(cookie)
        Else
            Dim fExpiracion As DateTime = Now.AddDays(1)
            fExpiracion = New DateTime(fExpiracion.Year, fExpiracion.Month, fExpiracion.Day, 0, 0, 0)
            cookie = New HttpCookie(nombreCookie)
            cookie.Value = numVisualizaciones
            cookie.Expires = fExpiracion
            Response.Cookies.Add(cookie)
        End If
        lblCookieLangile.Text = numVisualizaciones
    End Sub

End Class