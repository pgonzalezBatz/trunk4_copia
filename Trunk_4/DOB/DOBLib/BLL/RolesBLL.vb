Imports DOBLib.DAL

Namespace BLL

    Public Class RolesBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un rol
        ''' </summary>
        ''' <param name="idRol">Id del rol</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ObtenerRol(ByVal idRol As Integer) As ELL.Rol
            Return RolesDAL.getRol(idRol)
        End Function

        ''' <summary>
        ''' Obtiene un listado de roles
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado() As List(Of ELL.Rol)
            Return RolesDAL.loadList()
        End Function

#End Region

    End Class

End Namespace