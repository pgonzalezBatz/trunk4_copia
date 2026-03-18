Public Class BuscarUsuarios
    Inherits Page

    ''' <summary>
    ''' Busca los usuarios que coincidan con el texto escrito
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim query As String = Request.QueryString("q")  'Texto a buscar
        Dim dadosBaja As String = Request.QueryString("baja")
        Dim soloIgorre As Boolean = If(Request.QueryString("igorre") IsNot Nothing, True, False)
        Dim opcion As String = Request.QueryString("o")
        Dim bContinue As Boolean
        Dim idPlanta As Integer = CInt(Session("IdPlanta"))
        Dim SabComponent As New SabLib.BLL.UsuariosComponent
        Dim itzultzaileWeb As New itzultzaile
        Dim usuarios As List(Of SabLib.ELL.Usuario) = Nothing
        usuarios = SabComponent.GetUsuariosBusquedaSAB_Optimizado(query, idPlanta:=idPlanta)
        '170419: Los subcontratados, no pueden aparecer
        Dim resultado As String = "["
        For Each usuario As SabLib.ELL.Usuario In usuarios
            bContinue = usuario.CodPersona < 900000 AndAlso (Not soloIgorre OrElse (soloIgorre AndAlso usuario.IdDepartamento <> String.Empty AndAlso CInt(usuario.IdDepartamento) < 200000))
            If (bContinue) Then
                If Not usuario.DadoBaja Then
                    resultado &= "{""id"":""" & usuario.Id & """,""user"":""" & usuario.NombreCompleto & """},"
                ElseIf (usuario.DadoBaja AndAlso usuario.FechaBaja.AddDays(75) > Now AndAlso dadosBaja <> String.Empty) Then  'Poder buscar personas cuya fecha de baja sea menor que dos meses y medio
                    resultado &= "{""id"":""" & usuario.Id & """,""user"":""" & usuario.NombreCompleto & "(" & itzultzaileWeb.Itzuli("Baja") & " - " & usuario.CodPersona & ")" & """},"
                End If
            End If
        Next
        resultado &= "]"
        resultado = resultado.Replace("},]", "}]")

        enviarJSON(resultado)
    End Sub

    ''' <summary>
    ''' Devuelve el resultado con JSON
    ''' </summary>
    ''' <param name="resultado">Resultado a enviar</param>    
    Public Sub enviarJSON(ByVal resultado As String)
        Response.ClearHeaders()
        Response.ClearContent()
        Response.Clear()
        'Do not cache response
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'Set the content type and encoding for JSON
        Response.ContentType = "application/json"
        Response.ContentEncoding = Encoding.UTF8    
        Response.Write(resultado)
        Response.End()
    End Sub

End Class