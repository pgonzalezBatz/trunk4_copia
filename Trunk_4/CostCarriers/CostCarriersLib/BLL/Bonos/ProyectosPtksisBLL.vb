Namespace BLL

    Public Class ProyectosPtksisBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un proyecto
        ''' </summary>
        ''' <param name="proyecto"></param>  
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Obtener(ByVal proyecto As String) As ELL.ProyectoPtksis
            Return DAL.ProyectosPtksisDAL.getObject(proyecto)
        End Function

        ''' <summary>
        ''' Obtiene un listado
        ''' </summary>
        ''' <param name="producto"></param>
        ''' <param name="estado"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal producto As String, ByVal estado As String) As List(Of ELL.ProyectoPtksis)
            Return DAL.ProyectosPtksisDAL.loadList(producto, estado)
        End Function

        ''' <summary>
        ''' Obtiene un listado
        ''' </summary>
        ''' <param name="producto"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal producto As String) As List(Of ELL.ProyectoPtksis)
            Return DAL.ProyectosPtksisDAL.loadList(producto)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="proyecto"></param>
        ''' <param name="anno"></param>
        ''' <returns></returns>
        Public Shared Function EstaEnPG(ByVal proyecto As String, ByVal anno As Integer) As Boolean
            Dim ret As Boolean = False
            Using cliente As New ServicioBonos.ServicioBonos
                ret = cliente.IsProjectInManagementPlant(proyecto, anno)
            End Using

            Return ret
        End Function

#End Region

    End Class

End Namespace