
Namespace BLL

    Public Class ProyectosPTKSisBLL

        Private proyectosDAL As New DAL.ProyectosPTKSisDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene el listado
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarLista(ByVal texto As String) As List(Of ELL.Proyectos)
            Return proyectosDAL.loadList(texto)
        End Function

        ''' <summary>
        ''' Obtiene un proyecto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarProyectoPorId(ByVal id As String) As ELL.Proyectos
            Return proyectosDAL.CargarProyectoPorId(id)
        End Function

        ''' <summary>
        ''' Obtiene un proyecto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarProyectoPorPrograma(ByVal programa As String) As ELL.Proyectos
            Return proyectosDAL.CargarProyectoPorPrograma(programa)
        End Function

        ''' <summary>
        ''' Obtiene un proyecto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarProyecto(ByVal texto As String) As ELL.Proyectos
            Return proyectosDAL.CargarProyecto(texto)
        End Function

        ''' <summary>
        ''' Obtiene el listado de proyectos de PTKSIS filtrado por texto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarProyectosPTKSisPorTexto(ByVal texto As String) As List(Of ELL.Objeto)
            Return proyectosDAL.CargarProyectosPTKSis(texto)
        End Function

        ''' <summary>
        ''' Obtiene el listado de proyectos de PTKSIS
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarProyectosPTKSis() As List(Of ELL.Objeto)
            Return proyectosDAL.CargarProyectosPTKSis()
        End Function

#End Region

    End Class

End Namespace
