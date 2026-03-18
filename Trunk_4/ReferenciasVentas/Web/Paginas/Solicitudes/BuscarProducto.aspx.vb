Imports System.Runtime.Serialization.Json
Imports System.IO

Public Class BuscarProducto
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' Busca si existe el previous batz part number introducido por el usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim idProducto As String = Request.QueryString("idProducto")

        Dim producto As ELL.Producto = BLL.BusquedaJsonBLL.CargarProducto(idProducto)
        Dim resultado As String = JsonSerializer(producto)
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