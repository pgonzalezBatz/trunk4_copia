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
    Public Function CargarProfesion(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Trabajadores) 'aqui solo los activos


        listaType = oDocBLL.CargarListaProfesiontexto(PageBase.plantaAdmin, prefixText)




        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function
    <WebMethod()>
    Public Function CargarResponsables(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Trabajadores) 'aqui solo los activos


        listaType = oDocBLL.CargarListaResponsablestexto(PageBase.plantaAdmin, prefixText)




        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function
    <WebMethod()>
    Public Function CargarCursos(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Trabajadores) 'aqui solo los activos


        listaType = oDocBLL.CargarListaCursostexto(PageBase.plantaAdmin, prefixText)




        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function
    <WebMethod()>
    Public Function CargarTrabajadores(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Trabajadores) 'aqui solo los activos

        If PageBase.intRol = 3 Then
            listaType = oDocBLL.CargarListaTrabajadorestextoResponsables(PageBase.plantaAdmin, prefixText, PageBase.intDependientes)
        Else
            listaType = oDocBLL.CargarListaTrabajadorestexto(PageBase.plantaAdmin, prefixText)
        End If
        If PageBase.empresaBusqueda > 0 And PageBase.empresaBusqueda <> 999 Then
            listaType = oDocBLL.CargarListaTrabajadorestextoPuesto(PageBase.plantaAdmin, prefixText, PageBase.empresaBusqueda)
        Else
            listaType = oDocBLL.CargarListaTrabajadorestexto(PageBase.plantaAdmin, prefixText)
        End If

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function
    <WebMethod()>
    Public Function CargarTrabajadoresNoCad(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Trabajadores) 'aqui solo los activos

        listaType = oDocBLL.CargarListaTrabajadorestextoNoCad(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function
    <WebMethod()>
    Public Function CargarTra(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Trabajadores) 'aqui todos

        listaType = oDocBLL.CargarListaTratexto(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function
    <WebMethod()>
    Public Function CargarTraEmp(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Trabajadores) 'aqui todos
        If PageBase.intRol = 3 Then
            listaType = oDocBLL.CargarListaTrabajadorestextoResponsables2(PageBase.plantaAdmin, prefixText, PageBase.intDependientes, PageBase.intPuestos)
        Else
            listaType = oDocBLL.CargarListaTrabajadorestexto(PageBase.plantaAdmin, prefixText)
        End If
        If PageBase.empresaBusqueda > 0 And PageBase.empresaBusqueda <> 999 Then
            listaType = oDocBLL.CargarListaTrabajadorestextoPuesto(PageBase.plantaAdmin, prefixText, PageBase.empresaBusqueda)
        Else
            listaType = oDocBLL.CargarListaTrabajadorestexto(PageBase.plantaAdmin, prefixText)
        End If

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function
    <WebMethod()>
    Public Function CargarTraDNI(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Trabajadores) 'aqui todos

        listaType = oDocBLL.CargarListaTratextoDNI(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.tDNI, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

    <WebMethod()>
    Public Function CargarResponsable(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Responsables) 'aqui todos

        listaType = oDocBLL.CargarListaResponsabletexto(PageBase.plantaAdmin, prefixText)

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
    Public Function CargarProveedor(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Empresas) 'aqui todos

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
    Public Function CargarEmpresas(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Empresas)

        listaType = oDocBLL.CargarListaEmpresastexto(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function
    <WebMethod()>
    Public Function CargarDocumentos(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Documentos)

        listaType = oDocBLL.CargarListaDocumentostexto(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaDoc
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function
    <WebMethod()>
    Public Function CargarDocumentosCer(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Documentos)

        listaType = oDocBLL.CargarDocumentosCer(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaDoc
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function
    <WebMethod()>
    Public Function CargarEmpresasActivas(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        '  Inherits PageBase
        Dim ItzultzaileWeb As New LocalizationLib.Itzultzaile
        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Empresas)
        If PageBase.intRol = 3 Then
            listaType = oDocBLL.CargarListaEmpresastextoActivasResponsables(PageBase.plantaAdmin, prefixText, PageBase.intPuestos)
        Else
            listaType = oDocBLL.CargarListaEmpresastextoActivas(PageBase.plantaAdmin, prefixText)
        End If


        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            '         Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ItzultzaileWeb.Itzuli(empresa.Nombre), ItzultzaileWeb.Itzuli(empresa.Nombre))
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

    <WebMethod()>
    Public Function CargarEmpresasActivas2(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Empresas)

        listaType = oDocBLL.CargarListaEmpresastextoActivas(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Id)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function

    <WebMethod()>
    Public Function CargarEmpresasXBATCod(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Empresas)
        Dim KeyValue As New List(Of String)()
        If IsNumeric(prefixText) Then
            listaType = oDocBLL.CargarListaEmpresastextoXBATCod(PageBase.plantaAdmin, CInt(prefixText))


            For Each empresa In listaType 'listaEmpresas
                Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Id, empresa.Id)
                KeyValue.Add(item)
            Next

        End If

        Return KeyValue.ToArray
    End Function

    <WebMethod()>
    Public Function CargarEmpresasXBATNombre(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Empresas)

        listaType = oDocBLL.CargarListaEmpresastextoXBATNombre(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function
    <WebMethod()>
    Public Function CargarEmpresasXBATNombre2(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        'solo las que esten en ADOK (saca activas y no activas)
        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Empresas)

        listaType = oDocBLL.CargarListaEmpresastextoXBATNombre2(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nombre, empresa.Nombre)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function
    <WebMethod()>
    Public Function CargarEmpresasXBATCif(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim oDocBLL As New BLL.DocumentosBLL

        Dim listaType As List(Of ELL.Empresas)

        listaType = oDocBLL.CargarListaEmpresastextoXBATCIF(PageBase.plantaAdmin, prefixText)

        Dim KeyValue As New List(Of String)()

        For Each empresa In listaType 'listaEmpresas
            Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(empresa.Nif, empresa.Nif)
            KeyValue.Add(item)
        Next
        Return KeyValue.ToArray
    End Function
End Class