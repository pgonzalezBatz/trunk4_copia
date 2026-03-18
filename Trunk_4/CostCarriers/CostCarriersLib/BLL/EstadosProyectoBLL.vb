Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class EstadosProyectoBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un estado de proyecto
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.EstadoProyecto
            Return DAL.EstadosProyectoDAL.getEstadoProyecto(id)
        End Function

        ''' <summary>
        ''' Obtiene estados de proyecto por tipo de proyecto
        ''' </summary>
        ''' <param name="idTipoProyecto"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(Optional ByVal idTipoProyecto As Integer? = Nothing) As List(Of ELL.EstadoProyecto)
            Return DAL.EstadosProyectoDAL.loadList(idTipoProyecto)
        End Function

#End Region

    End Class

End Namespace