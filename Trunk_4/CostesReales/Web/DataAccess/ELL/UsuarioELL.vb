Public Class UsuarioELL

#Region "Campos"
    Private _id As Integer = Integer.MinValue

    Private _idEmpresa As Integer = Integer.MinValue
    Private _idCultura As String = String.Empty
    Private _nombreUsuario As String = String.Empty
    Private _idDirectorioActivo As String = String.Empty
    Private _codPersona As Integer = Integer.MinValue
    Private _pwd As String = String.Empty
    Private _fechaAlta As DateTime = DateTime.MinValue
    Private _fechaBaja As DateTime = DateTime.MinValue
    Private _apellido1 As String = String.Empty
    Private _apellido2 As String = String.Empty
    Private _idMatrix As String = String.Empty
    Private _idFtp As String = String.Empty
    Private _email As String = String.Empty
    Private _idDepartamento As String = String.Empty
    Private _nombre As String = String.Empty
    Private _foto As Byte() = Nothing
    Private _dni As String = String.Empty
    Private _nikEuskaraz As Boolean = False
    Private _idPlanta As Integer = Integer.MinValue
    Private _usuarioEmpresa As Boolean = False
#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Identificador del usuario para los usuarios antiguos
    ''' Si se usa UsuariosIntranet, correspondera con el nuevo id de usuario que no cambia aunque un usuario cambie de numero
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
    Public Property NombreUsuario() As String
        Get
            Return _nombreUsuario
        End Get
        Set(ByVal value As String)
            _nombreUsuario = value
        End Set
    End Property

    ''' <summary>
    ''' Id de la empresa
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Public Property IdEmpresa() As Integer
        Get
            Return _idEmpresa
        End Get
        Set(ByVal value As Integer)
            _idEmpresa = value
        End Set
    End Property

    ''' <summary>
    ''' Cultura del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Public Property Cultura() As String
        Get
            Return _idCultura
        End Get
        Set(ByVal value As String)
            _idCultura = value
        End Set
    End Property

    ''' <summary>
    ''' Número de Trabajador de Batz.
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
    ''' Email
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Public Property Email() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
        End Set
    End Property

    ''' <summary>
    ''' Fecha en la que se le dio de alta
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property FechaAlta() As DateTime
        Get
            Return _fechaAlta
        End Get
        Set(ByVal value As DateTime)
            _fechaAlta = value
        End Set
    End Property

    ''' <summary>
    ''' Fecha en la que se le dio de baja
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Public Property FechaBaja() As DateTime
        Get
            Return _fechaBaja
        End Get
        Set(ByVal value As DateTime)
            _fechaBaja = value
        End Set
    End Property

    ''' <summary>
    ''' Dominio\nombre de usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Public Property IdDirectorioActivo() As String
        Get
            Return _idDirectorioActivo
        End Get
        Set(ByVal value As String)
            _idDirectorioActivo = value
        End Set
    End Property

    Public Property IdFTP() As String
        Get
            Return _idFtp
        End Get
        Set(ByVal value As String)
            _idFtp = value
        End Set
    End Property

    Public Property IdMatrix() As String
        Get
            Return _idMatrix
        End Get
        Set(ByVal value As String)
            _idMatrix = value
        End Set
    End Property

    ''' <summary>
    ''' Password del portal del empleado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Public Property PWD() As String
        Get
            Return _pwd
        End Get
        Set(ByVal value As String)
            _pwd = value
        End Set
    End Property

    ''' <summary>
    ''' Identificador del departamento
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
    ''' Nombre de la persona
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

    ''' <summary>
    ''' Primer apellido
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Public Property Apellido1() As String
        Get
            Return _apellido1
        End Get
        Set(ByVal value As String)
            _apellido1 = value
        End Set
    End Property

    ''' <summary>
    ''' Segundo apellido
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Public Property Apellido2() As String
        Get
            Return _apellido2
        End Get
        Set(ByVal value As String)
            _apellido2 = value
        End Set
    End Property

    ''' <summary>
    ''' Se forma el nombre con los apellidos
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property NombreCompleto() As String
        Get
            Dim nombreComp As String = Nombre & " " & Apellido1 & " " & Apellido2
            Return nombreComp.Trim
        End Get
    End Property

    ''' <summary>
    ''' Se forma el nombre con los apellidos. Si tiene código de persona se añade delante entre paréntesis
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property NombreCompletoYCodpersona() As String
        Get
            Dim nombreComp As String = String.Empty

            If (CodPersona <> Integer.MinValue) Then
                nombreComp = "(" & CodPersona & ") " & Nombre & " " & Apellido1 & " " & Apellido2
            Else
                nombreComp = Nombre & " " & Apellido1 & " " & Apellido2
            End If

            Return nombreComp.Trim
        End Get
    End Property

    ''' <summary>
    ''' Identificador de la planta activa del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property IdPlanta() As Integer
        Get
            Return _idPlanta
        End Get
        Set(ByVal value As Integer)
            _idPlanta = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si esta dado de baja o no
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Public ReadOnly Property DadoBaja() As Boolean
        Get
            If (_fechaBaja = DateTime.MinValue) Then
                Return False
            ElseIf (_fechaBaja <> DateTime.MinValue) Then
                If (_fechaBaja < DateTime.Now) Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Get
    End Property

    ''' <summary>
    ''' Foto del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Public Property Foto() As Byte()
        Get
            Return _foto
        End Get
        Set(ByVal value As Byte())
            _foto = value
        End Set
    End Property

    ''' <summary>
    ''' DNI
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Public Property Dni() As String
        Get
            Return _dni
        End Get
        Set(ByVal value As String)
            _dni = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si el usuario preferira comunicarse en euskera
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Public Property NikEuskaraz() As Boolean
        Get
            Return _nikEuskaraz
        End Get
        Set(ByVal value As Boolean)
            _nikEuskaraz = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si el usuario es el usuario principal (proveedor)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Public Property UsuarioEmpresa() As Boolean
        Get
            Return _usuarioEmpresa
        End Get
        Set(ByVal value As Boolean)
            _usuarioEmpresa = value
        End Set
    End Property

#End Region

End Class
