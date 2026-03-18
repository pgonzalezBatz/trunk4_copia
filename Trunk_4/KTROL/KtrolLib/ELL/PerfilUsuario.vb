Namespace ELL
    Public Class PerfilUsuario

#Region "Variables miembro"

        Private _idUsuario As Integer = Integer.MinValue
        Private _idDepartamento As String = String.Empty
        Private _idTipoTrabajador As ELL.Usuarios.RolesUsuario
        'Private _calidad As Boolean = False
        'Private _operario As Boolean = False
        'Private _administrador As Boolean = False
        'Private _gestor As Boolean = False

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
        ''' Departamento del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdTipoTrabajador() As ELL.Usuarios.RolesUsuario
            Get
                Return _idTipoTrabajador
            End Get
            Set(ByVal value As ELL.Usuarios.RolesUsuario)
                _idTipoTrabajador = value
            End Set
        End Property

        '''' <summary>
        '''' El usuario es de calidad
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>        
        'Public Property Calidad() As Boolean
        '    Get
        '        Return _calidad
        '    End Get
        '    Set(ByVal value As Boolean)
        '        _calidad = value
        '    End Set
        'End Property

        '''' <summary>
        '''' El usuario es operario
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>        
        'Public Property Operario() As Boolean
        '    Get
        '        Return _operario
        '    End Get
        '    Set(ByVal value As Boolean)
        '        _operario = value
        '    End Set
        'End Property

        '''' <summary>
        '''' El usuario es administrador
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>        
        'Public Property Administrador() As Boolean
        '    Get
        '        Return _administrador
        '    End Get
        '    Set(ByVal value As Boolean)
        '        _administrador = value
        '    End Set
        'End Property

        '''' <summary>
        '''' El usuario es administrador
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>        
        'Public Property Gestor() As Boolean
        '    Get
        '        Return _gestor
        '    End Get
        '    Set(ByVal value As Boolean)
        '        _gestor = value
        '    End Set
        'End Property

#End Region

    End Class

End Namespace

