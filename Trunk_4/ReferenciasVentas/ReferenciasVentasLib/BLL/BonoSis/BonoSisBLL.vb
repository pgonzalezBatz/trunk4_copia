
Namespace BLL

    Public Class BonoSisBLL

        Private bonoSisDAL As New DAL.BonoSisDAL

#Region "PROYECTOS - PTKSIS"

        ''' <summary>
        ''' Obtiene el listado
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarLista(ByVal texto As String) As List(Of ELL.Proyectos)
            Return bonoSisDAL.loadList(texto)
        End Function

        ''' <summary>
        ''' Obtiene un proyecto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarProyectoPorId(ByVal id As String) As ELL.Proyectos
            Return bonoSisDAL.CargarProyectoPorId(id)
        End Function

        ''' <summary>
        ''' Obtiene un proyecto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarProyecto(ByVal texto As String) As ELL.Proyectos
            Return bonoSisDAL.CargarProyecto(texto)
        End Function

        ''' <summary>
        ''' Obtiene el listado
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarProyectosPTKSis(ByVal texto As String) As List(Of String())
            Return bonoSisDAL.CargarProyectosPTKSis(texto)
        End Function

#End Region

#Region "ROLES-USUARIO"

        ''' <summary>
        ''' Obtiene el rol de un usuario
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarRolUsuario(ByVal idUsuario As Integer) As ELL.UsuarioRol
            Return bonoSisDAL.CargarRolUsuario(idUsuario)
        End Function

        ''' <summary>
        ''' Obtiene los usuarios de un rol concreto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarUsuariosRol(ByVal idRol As Integer) As List(Of ELL.UsuarioRol)
            Return bonoSisDAL.CargarUsuariosRol(idRol)
        End Function

        '''' <summary>
        '''' Obtiene los usuarios con rol 'Project Leader'
        '''' </summary>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function CargarUsuariosProjectLeader() As List(Of ELL.Objecto)
        '    Return bonoSisDAL.CargarUsuariosProjectLeader()
        'End Function

        ''' <summary>
        ''' Obtiene los usuarios pásandole una lista de roles
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarUsuariosRol(ByVal listaRoles As List(Of ELL.Roles.RolUsuario)) As List(Of ELL.Usuarios)
            Return bonoSisDAL.CargarUsuariosRol(listaRoles)
        End Function
#End Region

#Region "ROLES"

        ''' <summary>
        ''' Obtiene el rol
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarRol(ByVal idRol As Integer) As ELL.Roles
            Return bonoSisDAL.CargarRol(idRol)
        End Function

#End Region

    End Class

End Namespace
