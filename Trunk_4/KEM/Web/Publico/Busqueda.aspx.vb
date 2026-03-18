Public Class Busqueda
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' Busca los usuarios que coincidan con el texto escrito
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim query As String = Request.QueryString("q")  'Texto a buscar
        Dim opcion As String = Request.QueryString("o")
        Dim soloActivos As Boolean = Request.QueryString("sa")
        Dim idPlanta As Integer = Request.QueryString("idP")
        If (idPlanta < 0) Then idPlanta = 0
        Dim resultado As String = "["
        Dim userBLL As New Sablib.BLL.UsuariosComponent
        Dim usuarios As List(Of Sablib.ELL.Usuario) = Nothing
        If (opcion = "user") Then
            usuarios = userBLL.GetUsuariosBusquedaSAB_Optimizado(query)
            If (idPlanta > 0) Then usuarios = usuarios.FindAll(Function(o As Sablib.ELL.Usuario) o.IdPlanta = idPlanta)
            resultado &= "{""a"":""" & "Id" & """,""b"":""" & "Usuario" & """,""c"":""" & "Num trab" & """,""d"":""" & "Fecha Alta" & """},"
            For Each usuario As Sablib.ELL.Usuario In usuarios
                If (soloActivos And usuario.DadoBaja) Then
                    Continue For
                Else
                    resultado &= "{""a"":""" & usuario.Id & """,""b"":""" & usuario.NombreCompleto & """,""c"":""" & usuario.CodPersona & """,""d"":""" & usuario.FechaAlta.ToShortDateString & """,""e"":""" & If(usuario.DadoBaja, 1, 0) & """},"
                End If
            Next
        ElseIf (opcion = "perf") Then
            usuarios = userBLL.GetUsuariosBusquedaSAB_Optimizado(query, CInt(ConfigurationManager.AppSettings("RecursoWeb")), True)
            If (idPlanta > 0) Then usuarios = usuarios.FindAll(Function(o As Sablib.ELL.Usuario) o.IdPlanta = idPlanta)
            resultado &= "{""a"":""" & "Id" & """,""b"":""" & "Usuario" & """,""c"":""" & "Num trab" & """},"
            For Each usuario As Sablib.ELL.Usuario In usuarios
                If (soloActivos And usuario.DadoBaja) Then
                    Continue For
                Else
                    resultado &= "{""a"":""" & usuario.Id & """,""b"":""" & usuario.NombreCompleto & """,""c"":""" & usuario.CodPersona & """,""e"":""" & If(usuario.DadoBaja, 1, 0) & """},"
                End If
            Next
        End If
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
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/json"
        Response.ContentEncoding = Encoding.UTF8
        Response.Write(resultado)
        Response.End()
    End Sub

End Class