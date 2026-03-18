Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CodigosPresupuestoBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene codigos de presupuesto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Obtener(ByVal codigo As String) As ELL.CodigoPrespuesto
            Return DAL.CodigosPresupuestoDAL.getObject(codigo)
        End Function

        ''' <summary>
        ''' Obtiene codigos de presupuesto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal cadenaBusqueda As String) As List(Of ELL.CodigoPrespuesto)
            Return DAL.CodigosPresupuestoDAL.loadList(cadenaBusqueda)
        End Function

#End Region

    End Class

End Namespace