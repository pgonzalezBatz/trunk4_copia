Public Partial Class DBImages
    Inherits Page

    ''' <summary>
    ''' Se pinta la imagen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim recurso As New RRHHLib.BLL.RRHHComponent
        Dim idRecurso As Integer = CInt(Request("idRecurso"))
        Dim imagen As Byte() = Cache.Get("recPortal_" & idRecurso)
        If imagen Is Nothing Then
            imagen = recurso.dameImagen(idRecurso)
            Cache.Insert("recPortal_" & idRecurso, imagen)
        End If
        If imagen IsNot Nothing Then Response.BinaryWrite(imagen)
    End Sub

End Class