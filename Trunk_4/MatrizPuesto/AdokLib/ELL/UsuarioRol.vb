Namespace ELL

    Public Class UsuarioRol

#Region "Variables miembro"

        Private _idRol As Integer = Integer.MinValue
        Private _idUsuario As Integer = Integer.MinValue

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id del rol
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
        ''' Id de usuario
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
        ''' Rol en castellano
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property DescripcionRolC() As String
            Get
                'Dim rolesBLL As New BLL.RolesBLL
                'Return rolesBLL.CargarRol(Me._idRol).Nombre
                Return String.Empty
            End Get
        End Property

        ''' <summary>
        ''' Usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property NombreUsuario() As String
            Get
                'Dim usuariosBLL As New BLL.UsuariosBLL
                'Return usuariosBLL.ObtenerUsuarioSAB(Me._idUsuario).NombreCompleto
                Return String.Empty
            End Get
        End Property

#End Region

    End Class

End Namespace
