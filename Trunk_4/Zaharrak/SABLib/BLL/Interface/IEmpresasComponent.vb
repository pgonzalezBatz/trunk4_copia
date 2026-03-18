Imports System.Collections.Generic

Namespace BLL.Interface
    Public Interface IEmpresasComponent


        ''' <summary>
        ''' Carga los datos de la empresa referenciada
        ''' </summary>
        ''' <param name="id">Identificador de la empresa</param>
        ''' <remarks></remarks>
        Function GetEmpresa(ByVal id As Integer) As ELL.Empresa


        ''' <summary>
        ''' Carga las empresas activas u obsoletas
        ''' </summary>
        ''' <param name="bActivas">Indicara si solo quiere las activas o no</param>
        Function GetEmpresas(Optional ByVal bActivas As Boolean = True) As List(Of ELL.Empresa)


        ''' <summary>
        ''' Carga las empresas activas que no tienen IdTroqueleria ni IdSistemas
        ''' </summary>
        ''' <remarks></remarks>
        Function GetEmpresasActivasSinTroqueleriaSistemas() As List(Of ELL.Empresa)


        ''' <summary>
        ''' Guarda los datos de la empresa
        ''' </summary>
        ''' <param name="oEmpresa">Objeto empresa a guardar</param>        
        ''' <returns>Booleano que indica si se ha guardado correctamente</returns>
        Function Save(ByVal oEmpresa As ELL.Empresa) As Boolean


        ''' <summary>
        ''' Borra la empresa
        ''' </summary>
        ''' <param name="idEmpresa">Identificador de la empresa</param>
        ''' <returns>Booleano indicando si se ha borrado correctamente</returns>
        Function Delete(ByVal idEmpresa As Integer) As Boolean


        ''' <summary>
        ''' Realiza una busqueda de las empresa
        ''' </summary>
        ''' <param name="filtro">Filtro a aplicar</param>
        ''' <param name="bTroqueleria">Flag que implicara buscar empresas de troqueleria</param>
        ''' <param name="bSistemas">Flag que implicar buscar empresas de sistemas</param>
        Function BuscarEmpresas(ByVal filtro As String, ByVal bTroqueleria As Boolean, ByVal bSistemas As Boolean) As List(Of ELL.Empresa)

    End Interface
End Namespace