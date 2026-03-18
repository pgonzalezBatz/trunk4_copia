Namespace ELL
    Public Class Ticket

        Private _culture As String = String.Empty
        Private _idSession As String = String.Empty
        Private _idUser As Integer = Integer.MinValue
        Private _nombreUsuario As String = String.Empty
        Private _idTrabajador As Integer = Integer.MinValue
        Private _idEmpresa As Integer = Integer.MinValue
        Private _empresa As String = String.Empty
        Private _nombrePersona As String = String.Empty
        Private _apellido1 As String = String.Empty
        Private _apellido2 As String = String.Empty
        Private _plantas As List(Of ELL.Planta)
        Private _idDepartamento As String
        Private _email As String = String.Empty
        Private _dni As String = String.Empty


        Public Property Culture() As String
            Get
                Return _culture
            End Get
            Set(ByVal value As String)
                _culture = value
            End Set
        End Property


        Public Property IdSession() As String
            Get
                Return _idSession
            End Get
            Set(ByVal value As String)
                _idSession = value
            End Set
        End Property


        Public Property IdUser() As Integer
            Get
                Return _idUser
            End Get
            Set(ByVal value As Integer)
                _idUser = value
            End Set
        End Property


        Public Property NombreUsuario() As String
            Get
                Return _nombreUsuario
            End Get
            Set(ByVal value As String)
                _nombreUsuario = value
            End Set
        End Property


        Public Property IdTrabajador() As Integer
            Get
                Return _idTrabajador
            End Get
            Set(ByVal value As Integer)
                _idTrabajador = value
            End Set
        End Property


        Public Property IdEmpresa() As Integer
            Get
                Return _idEmpresa
            End Get
            Set(ByVal value As Integer)
                _idEmpresa = value
            End Set
        End Property

        Public Property Empresa() As String
            Get
                Return _empresa
            End Get
            Set(ByVal value As String)
                _empresa = value
            End Set
        End Property


        Public Property NombrePersona() As String
            Get
                Return _nombrePersona
            End Get
            Set(ByVal value As String)
                _nombrePersona = value
            End Set
        End Property


        Public Property Apellido1() As String
            Get
                Return _apellido1
            End Get
            Set(ByVal value As String)
                _apellido1 = value
            End Set
        End Property


        Public Property Apellido2() As String
            Get
                Return _apellido2
            End Get
            Set(ByVal value As String)
                _apellido2 = value
            End Set
        End Property


        Public Property Plantas() As List(Of ELL.Planta)
            Get
                Return _plantas
            End Get
            Set(ByVal value As List(Of ELL.Planta))
                _plantas = value
            End Set
        End Property


        Public Property IdDepartamento() As String
            Get
                Return _idDepartamento
            End Get
            Set(ByVal value As String)
                _idDepartamento = value
            End Set
        End Property


        Public ReadOnly Property NombreCompleto() As String
            Get
                Dim nombreComp As String = NombrePersona & " " & Apellido1 & " " & Apellido2
                Return nombreComp.Trim
            End Get
        End Property

        Public Property email() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
            End Set
        End Property

        Public Property Dni() As String
            Get
                Return _dni
            End Get
            Set(ByVal value As String)
                _dni = value
            End Set
        End Property
    End Class
End Namespace