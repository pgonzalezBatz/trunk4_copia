Namespace ELL
    Public Class PerfilUsuario
#Region "Variables miembro"

        Private _idUsuario As Integer = Integer.MinValue
        Private _idDepartamento As String = String.Empty
        Private _idRol As Integer = Integer.MinValue
        Private _rolesUsuario As List(Of ELL.UsuarioRol) = New List(Of ELL.UsuarioRol)

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
        ''' Departamento del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdDepartamento() As String
            Get
                Return _idDepartamento
            End Get
            Set(ByVal value As String)
                _idDepartamento = value
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
        Public ReadOnly Property RolUsuario() As List(Of ELL.UsuarioRol)        
            Get
                Dim rolesUsuario As List(Of ELL.UsuarioRol)
                Dim usuarioRolBLL As New BLL.UsuariosRolBLL()

                rolesUsuario = usuarioRolBLL.CargarUsuarioRol(Me._idUsuario)
                If (rolesUsuario.Count = 0) Then
                    rolesUsuario.Add(New ELL.UsuarioRol With {.IdUsuario = IdUsuario, .IdRol = ELL.Roles.RolUsuarioTodos.Usuario})
                End If

                Return rolesUsuario
            End Get
        End Property

#End Region

    End Class

End Namespace

