Namespace ELL

    Public Class ExtensionUsuDep

        Private _idExtension As Integer = Integer.MinValue
        Private _fDesde As Date = Date.MinValue
        Private _fHasta As Date = Date.MinValue
        Private _idUsuario As Integer = Integer.MinValue
        Private _idTlfno As Integer = Integer.MinValue
        Private _nombreUsuario As String = String.Empty
        Private _idDepartamento As String = String.Empty
        Private _nombreDepartamento As String = String.Empty
        Private _idDepartamentoFac As String = String.Empty
        Private _idOtros As Integer = Integer.MinValue
        Private _nombreOtros As String = String.Empty


        ''' <summary>
        ''' Identificador de la extension
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdExtension() As Integer
            Get
                Return _idExtension
            End Get
            Set(ByVal value As Integer)
                _idExtension = value
            End Set
        End Property


        ''' <summary>
        ''' Fecha en la que se asigna un telefono o extension a un usuario/departamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FechaDesde() As Date
            Get
                Return _fDesde
            End Get
            Set(ByVal value As Date)
                _fDesde = value
            End Set
        End Property


        ''' <summary>
        ''' Fecha en la que se desasigna un telefono o extension a un usuario/departamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FechaHasta() As Date
            Get
                Return _fHasta
            End Get
            Set(ByVal value As Date)
                _fHasta = value
            End Set
        End Property


        ''' <summary>
        ''' Identificador del usuario al que se le ha asignado el telefono o la extension
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property


        ''' <summary>
        ''' Identificador del nombre de usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NombreUsuario() As String
            Get
                Return _nombreUsuario
            End Get
            Set(ByVal value As String)
                _nombreUsuario = value
            End Set
        End Property



        ''' <summary>
        ''' Identificador del departamento al que se le ha asignado el telefono o la extension
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdDepartamento() As String
            Get
                Return _idDepartamento
            End Get
            Set(ByVal value As String)
                _idDepartamento = value
            End Set
        End Property


        ''' <summary>
        ''' Nombre del departamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NombreDepartamento() As String
            Get
                Return _nombreDepartamento
            End Get
            Set(ByVal value As String)
                _nombreDepartamento = value
            End Set
        End Property


        ''' <summary>
        ''' Identificador del departamento al que se le va a facturar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdDepartamentoFac() As String
            Get
                Return _idDepartamentoFac
            End Get
            Set(ByVal value As String)
                _idDepartamentoFac = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador de otros al que se le ha asignado el telefono o la extension
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdOtros() As Integer
            Get
                Return _idOtros
            End Get
            Set(ByVal value As Integer)
                _idOtros = value
            End Set
        End Property


        ''' <summary>
        ''' Nombre del elemento de otros
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NombreOtros() As String
            Get
                Return _nombreOtros
            End Get
            Set(ByVal value As String)
                _nombreOtros = value
            End Set
        End Property


        ''' <summary>
        ''' Identificador del telefono con el que esta asociado la extension
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdTelefono() As Integer
            Get
                Return _idTlfno
            End Get
            Set(ByVal value As Integer)
                _idTlfno = value
            End Set
        End Property

    End Class

End Namespace