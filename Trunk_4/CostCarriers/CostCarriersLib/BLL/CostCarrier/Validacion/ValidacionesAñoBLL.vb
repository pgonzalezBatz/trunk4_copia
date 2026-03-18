Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ValidacionesAñoBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene lista de validaciones año
        ''' </summary>
        ''' <param name="idValidacionLinea"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorValidacionLinea(ByVal idValidacionLinea As Integer) As List(Of ELL.ValidacionAño)
            Return DAL.ValidacionesAñoDAL.loadListByValidacionLinea(idValidacionLinea)
        End Function

        ''' <summary>
        ''' Obtiene lista de validaciones año
        ''' </summary>
        ''' <param name="idValidacionLinea"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoUltimosValidadosPorValidacionLinea(ByVal idValidacionLinea As Integer) As List(Of ELL.ValidacionAño)
            Return DAL.ValidacionesAñoDAL.loadListUltimosValidadosByValidacionLinea(idValidacionLinea)
        End Function

#End Region

    End Class

End Namespace