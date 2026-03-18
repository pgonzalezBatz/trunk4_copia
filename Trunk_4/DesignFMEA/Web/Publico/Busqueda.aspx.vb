Public Class Busqueda
    Inherits Page

    ''' <summary>
    ''' Busca los usuarios que coincidan con el texto escrito
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim query As String = Request.QueryString("q")  'Texto a buscar
        Dim opcion As String = Request.QueryString("o")
        Dim resultado As String = "["
        Dim userBLL As New Sablib.BLL.UsuariosComponent
        Dim usuarios As List(Of Sablib.ELL.Usuario) = Nothing
        If (opcion = "user") Then
            usuarios = userBLL.GetUsuariosBusquedaSAB_Optimizado(query)
            resultado &= "{""a"":""" & "Id" & """,""b"":""" & "Usuario" & """,""c"":""" & "Num trab" & """,""d"":""" & "Fecha Alta" & """},"
            For Each usuario As Sablib.ELL.Usuario In usuarios
                If (Not usuario.DadoBaja) Then resultado &= "{""a"":""" & usuario.Id & """,""b"":""" & usuario.NombreCompleto & """,""c"":""" & usuario.CodPersona & """,""d"":""" & usuario.FechaAlta.ToShortDateString & """},"
            Next
        ElseIf (opcion = "perf") Then
            usuarios = userBLL.GetUsuariosBusquedaSAB_Optimizado(query, CInt(ConfigurationManager.AppSettings("RecursoWeb")), True)
            resultado &= "{""a"":""" & "Id" & """,""b"":""" & "Usuario" & """,""c"":""" & "Num trab" & """},"
            For Each usuario As Sablib.ELL.Usuario In usuarios
                If (Not usuario.DadoBaja) Then resultado &= "{""a"":""" & usuario.Id & """,""b"":""" & usuario.NombreCompleto & """,""c"":""" & usuario.CodPersona & """},"
            Next
        ElseIf (opcion = "refbrain") Then
            Dim neg As New BLL.BrainComponent
            Dim lRef As List(Of String()) = neg.loadReferenciasVentas(query)
            resultado &= "{""a"":""" & "Ref" & """,""b"":""" & "Denominacion" & """,""c&inv"":""" & "Factory" & """},"
            For Each ref As String() In lRef
                resultado &= "{""a"":""" & ref(0) & """,""b"":""" & ref(1) & """,""c&inv"":""" & ref(2) & """},"
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