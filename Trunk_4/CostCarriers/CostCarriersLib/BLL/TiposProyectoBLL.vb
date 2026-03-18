Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class TiposProyectoBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un tipo de proyecto
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.TipoProyecto
            Return DAL.TiposProyectoDAL.getTipoProyecto(id)
        End Function

        ''' <summary>
        ''' Obtiene todas las plantillas
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado() As List(Of ELL.TipoProyecto)
            Return DAL.TiposProyectoDAL.loadList()
        End Function

#End Region

    End Class

End Namespace