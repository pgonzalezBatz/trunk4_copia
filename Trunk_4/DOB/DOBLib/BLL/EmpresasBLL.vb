Imports DOBLib.DAL

Namespace BLL

    Public Class EmpresasBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de empresas
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(Optional ByVal obsoleto As Boolean = False) As List(Of ELL.Empresa)
            Return EmpresasDAL.loadList(obsoleto)
        End Function

#End Region

    End Class

End Namespace