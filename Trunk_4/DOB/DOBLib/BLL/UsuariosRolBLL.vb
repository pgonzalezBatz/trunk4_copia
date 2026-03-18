Imports DOBLib.DAL

Namespace BLL

    Public Class UsuariosRolBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un usuario
        ''' </summary>
        ''' <param name="idUsuarioRol"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal idUsuarioRol As Integer) As ELL.UsuarioRol
            Return UsuariosRolDAL.getUsuarioRol(idUsuarioRol)
        End Function

        ''' <summary>
        ''' Obtiene un usuario
        ''' </summary>
        ''' <param name="idRol"></param>
        ''' <param name="idSab"></param>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal idRol As Integer, ByVal idSab As Integer, ByVal idPlanta As Integer) As ELL.UsuarioRol
            Return UsuariosRolDAL.getUsuarioRol(idRol, idSab, idPlanta)
        End Function

        ''' <summary>
        ''' Obtiene un listado de usuario roles
        ''' </summary>
        ''' <param name="idSab"></param> 
        ''' <param name="idPlanta"></param>
        ''' <param name="listaIdRoles"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(Optional ByVal idSab As Nullable(Of Integer) = Nothing, Optional ByVal idPlanta As Nullable(Of Integer) = Nothing, Optional ByVal listaIdRoles As List(Of Integer) = Nothing) As List(Of ELL.UsuarioRol)
            Dim usuariosRoles As List(Of ELL.UsuarioRol) = UsuariosRolDAL.loadList(idSab, idPlanta, listaIdRoles).Where(Function(f) f.FechaBajaPlanta = DateTime.MinValue).ToList()

#Region "Usuarios del grupo SAB Consejo Rector para la planta Batz Group"
            If (idSab IsNot Nothing) Then
                Dim idGrupo As Integer = System.Configuration.ConfigurationManager.AppSettings.Get("IdGrupoConsejoRector")
                Dim gruposBLL As New SabLib.BLL.GruposComponent()
                Dim listaUsuarios As List(Of SabLib.ELL.Usuario) = gruposBLL.GetUsuariosGrupo(idGrupo).Where(Function(f) f.FechaBaja = DateTime.MinValue OrElse f.FechaBaja >= DateTime.Today).ToList()

                ' Si el usuario no tiene ya un rol para la planta Batz Group se le asigna dinamicamente el rol de consultor
                If (Not usuariosRoles.Exists(Function(f) f.IdPlanta = ELL.Planta.PLANTA_BATZ_GROUP) AndAlso
                    listaUsuarios.Exists(Function(f) f.Id = idSab)) Then
                    ' Cogemos cualquiera de los roles
                    Dim usuarioRol As ELL.UsuarioRol

                    If (usuariosRoles Is Nothing AndAlso usuariosRoles.Count > 0) Then
                        usuarioRol = usuariosRoles.First().ShallowCopy()
                    Else
                        usuarioRol = New ELL.UsuarioRol()
                        Dim usuariosBLL As New SabLib.BLL.UsuariosComponent()
                        Dim usuario As SabLib.ELL.Usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idSab})
                        usuarioRol.IdSab = idSab
                        usuarioRol.Nombre = usuario.Nombre
                        usuarioRol.Apellido1 = usuario.Apellido1
                        usuarioRol.Apellido2 = usuario.Apellido2
                        usuarioRol.FechaBaja = usuario.FechaBaja
                    End If

                    ' Le cambiamos el rol a consultor y le ponemos el id planta
                    Dim rol As ELL.Rol = BLL.RolesBLL.ObtenerRol(ELL.Rol.RolUsuario.Consultor)
                    Dim planta As ELL.Planta = BLL.PlantasBLL.ObtenerPlanta(ELL.Planta.PLANTA_BATZ_GROUP)
                    usuarioRol.Id = Integer.MaxValue
                    usuarioRol.IdRol = rol.Id
                    usuarioRol.DescripcionRol = rol.Descripcion
                    usuarioRol.IdPlanta = planta.Id
                    usuarioRol.Planta = planta.Planta

                    usuariosRoles.Add(usuarioRol)
                End If
            End If
#End Region

            Return usuariosRoles
        End Function

        ''' <summary>
        ''' Comprueba si existe un usuario rol
        ''' </summary>
        ''' <param name="idRol">Id del rol</param>
        ''' <param name="idSab">Id del usuario</param>
        ''' <param name="idPlanta">Id planta</param> 
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Existe(ByVal idRol As Integer, ByVal idSab As String, ByVal idPlanta As Integer) As Boolean
            Return UsuariosRolDAL.existsUsuarioRol(idRol, idSab, idPlanta)
        End Function

        ''' <summary>
        ''' Comprueba si existe una planta
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ExistePlanta(ByVal idPlanta As Integer) As Boolean
            Return UsuariosRolDAL.existsPlanta(idPlanta)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un usuario rol
        ''' </summary>
        ''' <param name="usuarioRol">Usuario rol</param>
        ''' <param name="idRecurso"></param>
        ''' <param name="idGrupo"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal usuarioRol As ELL.UsuarioRol, ByVal idRecurso As Integer, ByVal idGrupo As Integer)
            Dim recursosBLL As New SabLib.BLL.RecursosComponent()
            recursosBLL.AddUsuario(usuarioRol.IdSab, idRecurso, idGrupo)

            UsuariosRolDAL.SaveUsuarioRol(usuarioRol)
        End Sub

        ''' <summary>
        ''' Marca el usuario rol como obsoleto
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub MarcarComoObsoleto(ByVal id As Integer)
            UsuariosRolDAL.MarkAsObsoleteUsuarioRol(id)
        End Sub

        ''' <summary>
        ''' Cambia el rol
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub CambiarRol(ByVal id As Integer, ByVal nuevoRol As Integer)
            UsuariosRolDAL.ChangeRole(id, nuevoRol)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un usuario rol
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="idRecurso"></param>
        ''' <param name="idGrupo"></param>
        ''' <remarks></remarks>
        Public Shared Sub Eliminar(ByVal id As Integer, ByVal idRecurso As Integer, ByVal idGrupo As Integer)
            ' Obtenemos el usuario/rol
            Dim usuarioRol As ELL.UsuarioRol = BLL.UsuariosRolBLL.Obtener(id)

            Dim listaUsuarioRol As List(Of ELL.UsuarioRol) = UsuariosRolDAL.loadList(usuarioRol.IdSab, Nothing).Where(Function(f) f.FechaBajaPlanta = DateTime.MinValue).ToList()

            ' Si sólo tiene un rol quiere decir que es el que vamos a eliminar con lo cual eliminamos su vinculación al recurso
            If (listaUsuarioRol.Count = 1) Then
                Dim recursosBLL As New SabLib.BLL.RecursosComponent()
                recursosBLL.DeleteUsuario(usuarioRol.IdSab, idRecurso, idGrupo)
            End If

            UsuariosRolDAL.DeleteUsuarioRol(id)
        End Sub

#End Region

    End Class

End Namespace