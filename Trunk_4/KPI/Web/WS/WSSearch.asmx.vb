Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class WSSearch
    Inherits System.Web.Services.WebService

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function CargarItems(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String)
        Dim listaUsuarios As List(Of SabLib.ELL.Usuario) = Nothing
        Dim KeyValue As New List(Of String)()
        Dim usuariosComponentBLL As New SabLib.BLL.UsuariosComponent
        Dim info As String() = contextKey.Split("|")
        Dim soloActivos As Boolean = If(info.Length > 1, CType(info(1), Boolean), False)
        Dim opcion As String = info(0)
        If (opcion = "user") Then
            listaUsuarios = usuariosComponentBLL.GetUsuariosBusquedaSAB_Optimizado(prefixText, conDirectorioActivo:=True)
        ElseIf (opcion = "perf") Then
            listaUsuarios = usuariosComponentBLL.GetUsuariosBusquedaSAB_Optimizado(prefixText, CInt(ConfigurationManager.AppSettings("RecursoWeb")), True)
        End If
        If (listaUsuarios IsNot Nothing) Then
            listaUsuarios.Sort(Function(o1 As SabLib.ELL.Usuario, o2 As SabLib.ELL.Usuario) o1.NombreCompleto.ToLower < o2.NombreCompleto.ToLower)
            For Each usuario In listaUsuarios
                If Not (soloActivos AndAlso usuario.DadoBaja) Then
                    Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(usuario.NombreCompleto.ToUpper, usuario.Id)
                    KeyValue.Add(item)
                End If
            Next
        End If
        Return KeyValue.ToArray
    End Function


End Class