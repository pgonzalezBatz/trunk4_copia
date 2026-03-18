Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class adok_ws
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

    <WebMethod()>
    Public Function CargarDepartamento(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        'Dim listaType As List(Of KaplanLib.ELLResponsables) 'aqui todos

        'listaType = oDocBLL.CargarListaResponsabletexto(PageBase.plantaAdmin, prefixText)

        Dim depBLL As New SabLib.BLL.DepartamentosComponent
        Dim tDept As List(Of SabLib.ELL.Departamento)
        tDept = depBLL.GetDepartamentos(SabLib.BLL.Interface.IDepartamentosComponent.EDepartamentos.Activos, PageBase.plantaAdmin)



        Dim KeyValue As New List(Of String)()

        For Each empresa In tDept ' listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Id)
            KeyValue.Add(item)
            '   PageBase.CodResponsable = empresa.Id
        Next
        '  PageBase.CodResponsable = empresa.Id
        Return KeyValue.ToArray
    End Function

    <WebMethod()>
    Public Function CargarProveedor(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        Dim listaType As List(Of KaplanLib.ELL.Empresas) 'aqui todos

        listaType = oDocBLL.loadProveedorExacto(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Id)
            KeyValue.Add(item)
            '   PageBase.CodResponsable = empresa.Id
        Next
        '  PageBase.CodResponsable = empresa.Id
        Return KeyValue.ToArray
    End Function

    <WebMethod()>
    Public Function CargarEmpresasProcess(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        Dim listaType As List(Of KaplanLib.ELL.Kaplan)

        listaType = oDocBLL.CargarListaEmpresastextoProcess(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

    <WebMethod()>
    Public Function CargarEmpresasWork(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        Dim listaType As List(Of KaplanLib.ELL.Kaplan)

        listaType = oDocBLL.CargarListaEmpresastextoWork(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

    <WebMethod()>
    Public Function CargarEmpresasComponents(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        Dim listaType As List(Of KaplanLib.ELL.Kaplan)

        listaType = oDocBLL.CargarListaEmpresastextoComponent(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function


    <WebMethod()>
    Public Function CargarEmpresas(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        Dim listaType As List(Of KaplanLib.ELL.Kaplan)

        listaType = oDocBLL.CargarListaEmpresastexto(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function


    <WebMethod()>
    Public Function CargarEmpresasresulting(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        Dim listaType As List(Of KaplanLib.ELL.Kaplan)

        listaType = oDocBLL.CargarListaEmpresastextoResulting(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function


    <WebMethod()>
    Public Function CargarEmpresasCharacteristics(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        Dim listaType As List(Of KaplanLib.ELL.Kaplan)

        listaType = oDocBLL.CargarListaEmpresastextoCharacteristics(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function


    <WebMethod()>
    Public Function CargarEmpresas2(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        Dim listaType As List(Of KaplanLib.ELL.Kaplan)

        listaType = oDocBLL.CargarListaEmpresastexto22(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

    <WebMethod()>
    Public Function CargarEmpresas2P(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        Dim listaType As List(Of KaplanLib.ELL.Kaplan)

        listaType = oDocBLL.CargarListaEmpresastexto22P(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function


    <WebMethod()>
    Public Function CargarEmpresas2C(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        Dim listaType As List(Of KaplanLib.ELL.Kaplan)

        listaType = oDocBLL.CargarListaEmpresastexto22C(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

    <WebMethod()>
    Public Function CargarEmpresas2R(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        Dim listaType As List(Of KaplanLib.ELL.Kaplan)

        listaType = oDocBLL.CargarListaEmpresastexto22R(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

    <WebMethod()>
    Public Function CargarDocumentos(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        Dim listaType As List(Of KaplanLib.ELL.Kaplan)

        listaType = oDocBLL.CargarListaDocumentostexto(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaDoc
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function



    <WebMethod()>
    Public Function CargarReferencia(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Piezas_Recibidas As String = ""

        Dim FechaDesde As Date = Now.AddMonths(-6).Date
        '        Dim strSql As String = "select distinct(t2tenr) from cubos.DESPIECES where t2tenr like UPPER('%" & prefixText & "%') Order by t2tenr "
        Dim strSql As String = "select distinct(t2tenr) from cubos.DESPIECES where t2tenr like '" & prefixText & "%' Order by t2tenr "

        Dim cn As New OleDb.OleDbConnection()
        Dim dr As OleDb.OleDbDataReader
        cn.ConnectionString = ConfigurationManager.ConnectionStrings("AS400").ConnectionString
        Dim cm As New OleDb.OleDbCommand(strSql, cn)
        cm.CommandTimeout = 30
        cn.Open()
        dr = cm.ExecuteReader()


        Dim KeyValue As New List(Of String)()

        While dr.Read

            Piezas_Recibidas = If(dr("t2tenr") Is Nothing OrElse dr("t2tenr") Is DBNull.Value, Piezas_Recibidas, dr("t2tenr"))

            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(Piezas_Recibidas, Piezas_Recibidas)
            KeyValue.Add(item)

        End While

        'fin prueba
        dr.Close()

        cn.Close()
        cn.Dispose()


        Return KeyValue.ToArray
    End Function

End Class