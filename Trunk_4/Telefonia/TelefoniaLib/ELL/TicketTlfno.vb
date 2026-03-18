Namespace ELL

    Public Class TicketTlfno
        Inherits SabLib.ELL.Ticket

        Private _idPlantaActual As Integer = Integer.MinValue
        Private _planta As String = String.Empty
        Private _administrador As Boolean = False
        Private _administradorPlanta As Boolean = False
        Private _gestor As Boolean = False
        Private _portalEmpleado As Boolean = False


        ''' <summary>
        ''' Identificador de la planta en la que se esta realizando la gestion
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdPlantaActual() As Integer
            Get
                Return _idPlantaActual
            End Get
            Set(ByVal value As Integer)
                _idPlantaActual = value
            End Set
        End Property


        ''' <summary>
        ''' Nombre de la planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Planta() As String
            Get
                Return _planta
            End Get
            Set(ByVal value As String)
                _planta = value
            End Set
        End Property


        ''' <summary>
        ''' Indica si es administrador o no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EsAdministrador() As Boolean
            Get
                Return _administrador
            End Get
            Set(ByVal value As Boolean)
                _administrador = value
            End Set
        End Property


        ''' <summary>
        ''' Indica si es administrador de planta o no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EsAdministradorPlanta() As Boolean
            Get
                Return _administradorPlanta
            End Get
            Set(ByVal value As Boolean)
                _administradorPlanta = value
            End Set
        End Property



        ''' <summary>
        ''' Indica si es gestor de telefonos de la planta o no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EsGestor() As Boolean
            Get
                Return _gestor
            End Get
            Set(ByVal value As Boolean)
                _gestor = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si proviene del portal del empleado
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProvienePortalEmpleado() As Boolean
            Get
                Return _portalEmpleado
            End Get
            Set(ByVal value As Boolean)
                _portalEmpleado = value
            End Set
        End Property


    End Class

End Namespace