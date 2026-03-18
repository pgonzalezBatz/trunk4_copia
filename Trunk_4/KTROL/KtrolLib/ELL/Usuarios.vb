Namespace ELL

    Public Class Usuarios

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        'Private _nombreUsuario As String = String.Empty
        Private _codPersona As Integer = Integer.MinValue
        Private _idRol As Integer = Integer.MinValue

#End Region

#Region "Enumeraciónes"

        Public Enum RolesUsuario As Integer
            Administrador = 1
            Operario = 2
            Calidad = 3
            Gestor = 4
        End Enum

        Public Enum RolesUsuarioControl As Integer
            Operario = 2
            Calidad = 3
            Gestor = 4
        End Enum
#End Region

#Region "Properties"

        ''' <summary>
        ''' Id del usuarios
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        ''' <summary>
        ''' Nombre corto del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property NombreUsuario() As String
            Get
                Dim usuariosComponent As New SabLib.BLL.UsuariosComponent
                Return usuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = _codPersona}, False).NombreCompleto
            End Get
        End Property

        ''' <summary>
        ''' Codigo de la persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CodPersona() As Integer
            Get
                Return _codPersona
            End Get
            Set(ByVal value As Integer)
                _codPersona = value
            End Set
        End Property

        ''' <summary>
        ''' Rol de la persona
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
#End Region

    End Class

End Namespace

