Namespace Controllers

    Public Class DireccionController
        Inherits BaseController

#Region "Propiedades"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides ReadOnly Property RolesAcceso As List(Of ELL.Rol.RolUsuario)
            Get
                Dim roles As New List(Of ELL.Rol.RolUsuario)
                roles.Add(ELL.Rol.RolUsuario.Responsable)
                roles.Add(ELL.Rol.RolUsuario.Lider_de_objetivos)
                roles.Add(ELL.Rol.RolUsuario.Consultor)
                Return roles
            End Get
        End Property

#End Region

    End Class
End Namespace