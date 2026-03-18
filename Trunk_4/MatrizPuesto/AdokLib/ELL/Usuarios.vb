Namespace ELL

    Public Class Usuarios

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _apellido1 As String = String.Empty
        Private _apellido2 As String = String.Empty
        Private _nombreUsuario As String = String.Empty
        Private _codPersona As Integer = Integer.MinValue
        Private _idRol As Integer = Integer.MinValue
        Private _nombreRol As String = String.Empty
        Private _fechaAlta As Date = Date.MinValue
        Private _fechaModificacion As Date = Date.MinValue
        Private _obsoleto As Boolean = False
        Private _directorIKS As Boolean = False

#End Region

#Region "Enum"

        Public Enum TipoTrabajador As Integer
            Batz = 1
            Subcontratado = 2
            Becario = 3
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
        ''' Nombre del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Nombre() As String
            Get
                Return If(String.IsNullOrEmpty(_nombre), String.Empty, _nombre.Trim())
            End Get
            Set(ByVal value As String)
                _nombre = If(String.IsNullOrEmpty(value), String.Empty, value.Trim())
            End Set
        End Property

        ''' <summary>
        ''' Primer apellido del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Apellido1() As String
            Get
                Return If(String.IsNullOrEmpty(_apellido1), String.Empty, _apellido1.Trim())
            End Get
            Set(ByVal value As String)
                _apellido1 = If(String.IsNullOrEmpty(value), String.Empty, value.Trim())
            End Set
        End Property

        Public Property Apellido2() As String
            Get
                Return If(String.IsNullOrEmpty(_apellido1), String.Empty, _apellido2.Trim())
            End Get
            Set(ByVal value As String)
                _apellido2 = If(String.IsNullOrEmpty(value), String.Empty, value.Trim())
            End Set
        End Property

        ''' <summary>
        ''' Nombre corto del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NombreUsuario() As String
            Get
                Return If(String.IsNullOrEmpty(_nombreUsuario), String.Empty, _nombreUsuario.Trim())
            End Get
            Set(ByVal value As String)
                _nombreUsuario = If(String.IsNullOrEmpty(value), String.Empty, value.Trim())
            End Set
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
        ''' Identificador de rol de la persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NombreRol() As String
            Get
                Return _nombreRol
            End Get
            Set(ByVal value As String)
                _nombreRol = value
            End Set
        End Property

        ''' <summary>
        ''' Fecha de alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaAlta() As Date
            Get
                Return _fechaAlta
            End Get
            Set(ByVal value As Date)
                _fechaAlta = value
            End Set
        End Property

        ''' <summary>
        ''' Fecha de alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaModificacion() As Date
            Get
                Return _fechaModificacion
            End Get
            Set(ByVal value As Date)
                _fechaModificacion = value
            End Set
        End Property

        ''' <summary>
        ''' Obsoleto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Obsoleto() As Boolean
            Get
                Return _obsoleto
            End Get
            Set(ByVal value As Boolean)
                _obsoleto = value
            End Set
        End Property

        ''' <summary>
        ''' Director IKS
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property DirectorIKS() As Boolean
            Get
                Return _directorIKS
            End Get
            Set(ByVal value As Boolean)
                _directorIKS = value
            End Set
        End Property

        ''' <summary>
        ''' Nombre completo del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property NombreCompleto() As String
            Get
                Return String.Format("{0} {1} {2}", Nombre.Trim(), Apellido1.Trim(), Me.Apellido2.Trim()).ToUpper()
            End Get
        End Property

#End Region

    End Class

End Namespace

