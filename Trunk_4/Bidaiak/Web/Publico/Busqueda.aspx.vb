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
        Dim soloActivos As Boolean = Request.QueryString("sa")
        Dim resultado As String = "["
        Dim userBLL As New SabLib.BLL.UsuariosComponent
        Dim idPlanta As Integer = CInt(Session("IdPlanta"))
        Dim usuarios As List(Of SabLib.ELL.Usuario) = Nothing
        '170419: Los subcontratados, no pueden aparecer
        If (opcion = "user") Then
            usuarios = userBLL.GetUsuariosBusquedaSAB_Optimizado(query, idPlanta:=idPlanta)
            usuarios = usuarios.OrderBy(Of String)(Function(o) o.NombreCompleto.ToLower).ToList
            For Each usuario As SabLib.ELL.Usuario In usuarios
                If (usuario.CodPersona < 900000) Then
                    If (soloActivos And usuario.DadoBaja) Then
                        Continue For
                    Else
                        resultado &= "{""id"":""" & usuario.Id & """,""no"":""" & usuario.NombreCompleto & """,""nt"":""" & usuario.CodPersona & """,""fa"":""" & usuario.FechaAlta.ToShortDateString & """,""fb"":""" & If(usuario.DadoBaja, 1, 0) & """},"
                    End If
                End If
            Next
        ElseIf (opcion = "userVisas") Then
            usuarios = userBLL.GetUsuariosBusquedaSAB_Optimizado(query, idPlanta:=idPlanta)
            If (usuarios.Count = 0 AndAlso opcion = "userVisas") Then
                usuarios = userBLL.GetUsuariosBusquedaSAB_Optimizado(query)
                Dim visasBLL As New BLL.VisasBLL
                Dim lVisas As List(Of ELL.Visa) = visasBLL.loadList(New ELL.Visa With {.NumTarjeta = query, .IdPlanta = idPlanta}, idPlanta)
                If (lVisas.Count > 0) Then
                    For Each oVisa As ELL.Visa In lVisas
                        usuarios.Add(oVisa.Propietario)
                    Next
                End If
            End If
            usuarios = usuarios.OrderBy(Of String)(Function(o) o.NombreCompleto.ToLower).ToList
            For Each usuario As SabLib.ELL.Usuario In usuarios
                'If (soloActivos And usuario.DadoBaja) Then
                '    Continue For
                'Else
                If (usuario.CodPersona < 900000) Then
                    resultado &= "{""id"":""" & usuario.Id & """,""no"":""" & usuario.NombreCompleto & """,""nt"":""" & usuario.CodPersona & """,""fa"":""" & usuario.FechaAlta.ToShortDateString & """,""fb"":""" & If(usuario.DadoBaja, 1, 0) & """},"
                End If
                'End If
            Next
        ElseIf (opcion = "perf") Then
            usuarios = userBLL.GetUsuariosBusquedaSAB_Optimizado(query, CInt(ConfigurationManager.AppSettings("RecursoWeb")), True, idPlanta:=idPlanta)
            usuarios = usuarios.OrderBy(Of String)(Function(o) o.NombreCompleto.ToLower).ToList
            For Each usuario As SabLib.ELL.Usuario In usuarios
                If (usuario.CodPersona < 900000) Then
                    If (soloActivos And usuario.DadoBaja) Then
                        Continue For
                    Else
                        resultado &= "{""id"":""" & usuario.Id & """,""no"":""" & usuario.NombreCompleto & """,""nt"":""" & usuario.CodPersona & """,""ba"":""" & If(usuario.DadoBaja, 1, 0) & """},"
                    End If
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