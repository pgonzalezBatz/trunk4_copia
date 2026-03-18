Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class CE_ws
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function HelloWorld() As String
        Return "Hola a todos"
    End Function

    <WebMethod()>
    Public Function CargarResponsable(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New CEticoLib.BLL.cEtico


        Dim listaType As List(Of CEticoLib.ELL.Responsables) 'aqui todos
        '    listaType = oDocBLL.CargarListaResponsabletexto(PageBase.plantaAdmin, prefixText)
        listaType = oDocBLL.CargarListaResponsabletexto(1, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Id)
            KeyValue.Add(item)
            '   PageBase.CodResponsable = empresa.Id
        Next
        '  PageBase.CodResponsable = empresa.Id
        Return KeyValue.ToArray
    End Function


End Class