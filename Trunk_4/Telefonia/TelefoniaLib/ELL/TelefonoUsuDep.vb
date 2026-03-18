Namespace ELL

    Public Class TelefonoUsuDep

        Private _idTelefono As Integer = Integer.MinValue
        Private _fDesde As Date = Date.MinValue
        Private _fHasta As Date = Date.MinValue
        Private _idUsuario As Integer = Integer.MinValue
        Private _nombreUsuario As String = String.Empty
        Private _idDepartamento As String = String.Empty
        Private _nombreDepartamento As String = String.Empty
        Private _idDepartamentoFac As String = String.Empty        


        ''' <summary>
        ''' Identificador del telefono
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdTelefono() As Integer
            Get
                Return _idTelefono
            End Get
            Set(ByVal value As Integer)
                _idTelefono = value
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

    End Class

End Namespace