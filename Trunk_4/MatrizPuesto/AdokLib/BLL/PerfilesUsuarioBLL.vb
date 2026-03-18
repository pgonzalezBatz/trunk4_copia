Imports Oracle.DataAccess.Client



Namespace BLL

    Public Class PerfilUsuariosBLL

#Region "Consultas"

        ''' <summary>
        ''' Carga el perfil del usuario
        ''' </summary>
        ''' <param name="idUsuario">Id del usuario</param> 
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 

        Public Function CargarPerfilUsuario(ByVal idUsuario As Integer, ByVal idDepartamento As String) As ELL.PerfilUsuario
            ' Llenamos los datos del perfil del usuario
            If (System.Configuration.ConfigurationManager.AppSettings("Administradores").ToString.Contains(idUsuario)) Then
                'Si el rol es 2 es administrador
                Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Administrador, .IdDepartamento = idDepartamento}
            Else 'If (System.Configuration.ConfigurationManager.AppSettings("Recepcion").ToString.Contains(idUsuario)) Then
                Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Supervisores}
                'ElseIf (System.Configuration.ConfigurationManager.AppSettings("ProductManager").ToString.Contains(idUsuario)) Then
                '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.ProductManager}
                'ElseIf (System.Configuration.ConfigurationManager.AppSettings("DocumentationTechnician").ToString.Contains(idUsuario)) Then
                '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.DocumentationTechnician}
                'ElseIf (System.Configuration.ConfigurationManager.AppSettings("ProjectLeaderManager").ToString.Contains(idUsuario)) Then
                '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.ProjectLeaderManager}
                'ElseIf (System.Configuration.ConfigurationManager.AppSettings("ProjectLeader").ToString.Contains(idUsuario)) Then
                '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.ProjectLeader}
                '    Else
                'Dim usuariosRolBLL As New BLL.BonoSisBLL()
                'Dim rol As ELL.UsuarioRol = usuariosRolBLL.CargarRolUsuario(idUsuario)
                'Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = If(rol IsNot Nothing, rol.IdRol, Integer.MinValue)}
            End If
        End Function


        Public Function CargarPerfilUsuario2(ByVal idUsuario As Integer, ByVal idDepartamento As String) As ELL.PerfilUsuario

            ' Llenamos los datos del perfil del usuario
            Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdDepartamento = idDepartamento}
        End Function

#End Region

    End Class

End Namespace

