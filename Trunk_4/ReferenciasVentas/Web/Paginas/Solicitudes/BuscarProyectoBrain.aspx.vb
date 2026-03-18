Imports System.Runtime.Serialization.Json
Imports System.IO

Public Class BuscarProyectoBrain
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' Busca los usuarios que coincidan con el texto escrito en su parte del nombre
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim proyecto As String = Request.QueryString("pro")

        Dim prevBatzPN As ELL.Proyectos = BLL.BusquedaJsonBLL.CargarProyectoBrain(proyecto)
        Dim resultado As String = JsonSerializer(prevBatzPN)
        enviarJSON(resultado)

        'Dim query As String = Request.QueryString("q")  'Texto a buscar
        'Dim oProyectos As New BLL.ProyectosPTKSisBLL
        'Dim proyectos As List(Of ELL.Proyectos) = oProyectos.CargarLista(query)
        'Dim resultado As String = "["
        'For Each proyecto As ELL.Proyectos In proyectos
        '    resultado &= "{""id"":""" & proyecto.Id & """,""nombre"":""" & proyecto.Nombre & """},"
        'Next
        'resultado &= "]"
        'resultado = resultado.Replace("},]", "}]")

        'enviarJSON(resultado)
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

    ''' <summary>
    ''' Serializ a JSON el objeto que se para como parámetro
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="o"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function JsonSerializer(Of T)(o As T) As String
        Dim ser As New DataContractJsonSerializer(GetType(T))
        Dim ms As New MemoryStream()
        ser.WriteObject(ms, o)
        Dim jsonString As String = Encoding.UTF8.GetString(ms.ToArray())
        ms.Close()
        Return jsonString
    End Function

End Class