Namespace ELL

    Public Class Usuarios

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _idSab As Integer = Integer.MinValue
        Private _idRol As Integer = Integer.MinValue

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
        ''' Nombre del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        ' ''' <summary>
        ' ''' Primer apellido del usuario
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>        
        'Public Property Apellido1() As String
        '    Get
        '        Return If(String.IsNullOrEmpty(_apellido1), String.Empty, _apellido1.Trim())
        '    End Get
        '    Set(ByVal value As String)
        '        _apellido1 = If(String.IsNullOrEmpty(value), String.Empty, value.Trim())
        '    End Set
        'End Property

        'Public Property Apellido2() As String
        '    Get
        '        Return If(String.IsNullOrEmpty(_apellido1), String.Empty, _apellido2.Trim())
        '    End Get
        '    Set(ByVal value As String)
        '        _apellido2 = If(String.IsNullOrEmpty(value), String.Empty, value.Trim())
        '    End Set
        'End Property

        ''' <summary>
        ''' Id sab de la persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdSab() As Integer
            Get
                Return _idSab
            End Get
            Set(ByVal value As Integer)
                _idSab = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador de rol de la persona
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
        ''' Nombre del rol
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NombreRol() As String

        ' ''' <summary>
        ' ''' Fecha de alta
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>        
        'Public Property FechaAlta() As Date
        '    Get
        '        Return _fechaAlta
        '    End Get
        '    Set(ByVal value As Date)
        '        _fechaAlta = value
        '    End Set
        'End Property

        ' ''' <summary>
        ' ''' Fecha de alta
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>        
        'Public Property FechaModificacion() As Date
        '    Get
        '        Return _fechaModificacion
        '    End Get
        '    Set(ByVal value As Date)
        '        _fechaModificacion = value
        '    End Set
        'End Property

        ' ''' <summary>
        ' ''' Identificador de grupo de la persona
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>        
        'Public Property IdGrupo() As Integer
        '    Get
        '        Return _idGrupo
        '    End Get
        '    Set(ByVal value As Integer)
        '        _idGrupo = value
        '    End Set
        'End Property

        ' ''' <summary>
        ' ''' Nombre completo del usuario
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>        
        'Public ReadOnly Property NombreCompleto() As String
        '    Get
        '        Return String.Format("{0} {1} {2}", Nombre.Trim(), Apellido1.Trim(), Me.Apellido2.Trim()).ToUpper()
        '    End Get
        'End Property

#End Region

    End Class

End Namespace

