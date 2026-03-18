Imports Oracle.ManagedDataAccess.Client

Namespace BLL

    Public Class PerfilUsuariosBLL

#Region "Consultas"

        ''' <summary>
        ''' Carga el perfil del usuario
        ''' </summary>
        ''' <param name="idUsuario">Id del usuario</param> 
        ''' <returns>PerfilUsuario con su usuario y rol</returns>
        ''' <remarks></remarks>
        Public Function CargarPerfilUsuario(ByVal idUsuario As Integer) As ELL.PerfilUsuario
            ' Llenamos los datos del perfil del usuario
            If (System.Configuration.ConfigurationManager.AppSettings("Administradores").ToString.Contains(idUsuario)) Then
                Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Administrador}
            ElseIf (System.Configuration.ConfigurationManager.AppSettings("Product Engineer").ToString.Contains(idUsuario)) Then
                Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.ProductEngineer}
            ElseIf (System.Configuration.ConfigurationManager.AppSettings("Product Engineer Mgr.").ToString.Contains(idUsuario)) Then
                Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.ProductManager}
            ElseIf (System.Configuration.ConfigurationManager.AppSettings("Documentation Technician").ToString.Contains(idUsuario)) Then
                Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.DocumentationTechnician}
            ElseIf (System.Configuration.ConfigurationManager.AppSettings("Project Mgr.").ToString.Contains(idUsuario)) Then
                Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.ProjectLeaderManager}
            ElseIf (System.Configuration.ConfigurationManager.AppSettings("Project Leader").ToString.Contains(idUsuario)) Then
                Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.ProjectLeader}
            Else
                Dim usuariosRolBLL As New BLL.BonoSisBLL()
                Dim rol As ELL.UsuarioRol = usuariosRolBLL.CargarRolUsuario(idUsuario)
                Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = If(rol IsNot Nothing, rol.IdRol, Integer.MinValue)}
            End If
        End Function

#End Region

    End Class

End Namespace

