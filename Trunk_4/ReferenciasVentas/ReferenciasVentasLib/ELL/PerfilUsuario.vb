Namespace ELL
    Public Class PerfilUsuario

#Region "Variables miembro"

        Private _idUsuario As Integer = Integer.MinValue
        Private _idRol As Integer = Integer.MinValue
        'Private _calidad As Boolean = False
        'Private _operario As Boolean = False
        'Private _administrador As Boolean = False
        'Private _rolesUsuario As List(Of ELL.UsuarioRol) = New List(Of ELL.UsuarioRol)

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property

        ''' <summary>
        ''' Id del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdRol() As Integer
            Get
                Return _idRol
            End Get
            Set(ByVal value As Integer)
                _idRol = value
            End Set
        End Property

        ''' <summary>
        ''' Roles del usuario dentro de la aplicación de gestor de colas
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property RolUsuario() As ELL.UsuarioRol
            Get
                'Dim rol As New ELL.Usuarios
                'Dim usuariosBLL As New BLL.UsuariosBLL()

                'rol = usuariosBLL.ObtenerUsuario(Me._idUsuario)
                'If (rol Is Nothing) Then
                '    rol.IdRol = ELL.Roles.RolUsuario.IngenieroProducto
                'End If

                'Return rol

                Dim usuariosRolBLL As New BLL.BonoSisBLL()
                Return usuariosRolBLL.CargarRolUsuario(Me._idUsuario)                
            End Get
        End Property

#End Region

    End Class

End Namespace

