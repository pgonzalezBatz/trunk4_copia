Imports DOBLib.DAL

Namespace BLL

    Public Class DivisionesBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de divisiones
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(Optional ByVal obsoleto As Boolean = False) As List(Of ELL.Division)
            Return DivisionesDAL.loadList(obsoleto)
        End Function

#End Region

    End Class

End Namespace