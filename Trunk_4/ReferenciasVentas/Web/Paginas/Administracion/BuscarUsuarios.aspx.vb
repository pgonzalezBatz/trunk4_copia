Public Class BuscarUsuarios
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' Busca los usuarios que coincidan con el texto escrito en su parte del nombre
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim query As String = Request.QueryString("q")  'Texto a buscar
        Dim SabComponent As New Sablib.BLL.UsuariosComponent
        Dim usuarios As List(Of Sablib.ELL.Usuario) = SabComponent.GetUsuariosBusquedaSAB_Optimizado(query)
        Dim resultado As String = "["
        For Each usuario As Sablib.ELL.Usuario In usuarios
            If Not (usuario.DadoBaja) Then
                resultado &= "{""id"":""" & usuario.Id & """,""nombre"":""" & usuario.NombreCompleto & """},"
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