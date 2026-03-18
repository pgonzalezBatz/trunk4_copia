Namespace ELL

    Public Class Actividad

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _exentaIRPF As Boolean = False
        Private _reqTexto As Boolean = False
        Private _idPlanta As Integer = Integer.MinValue
        Private _obsoleta As Boolean = False
        Private _dptos As List(Of Sablib.ELL.Departamento) = Nothing
        Private _puestaAPunto As Boolean = False

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id de la planta
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
        ''' Nombre de la actividad
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
        ''' Indica si esta exenta o no de IRPF
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ExentaIRPF() As Boolean
            Get
                Return _exentaIRPF
            End Get
            Set(ByVal value As Boolean)
                _exentaIRPF = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si al elegir la actividad, requerira texto no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property RequiereTexto() As Boolean
            Get
                Return _reqTexto
            End Get
            Set(ByVal value As Boolean)
                _reqTexto = value
            End Set
        End Property

        ''' <summary>
        ''' Id de la planta a la que pertenece
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlanta() As String
            Get
                Return _idPlanta
            End Get
            Set(ByVal value As String)
                _idPlanta = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si esta obsoleta o no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Obsoleta() As Boolean
            Get
                Return _obsoleta
            End Get
            Set(ByVal value As Boolean)
                _obsoleta = value
            End Set
        End Property

        ''' <summary>
        ''' Departamentos con los que esta relacionado.Si es nothing, estara relacionado con todos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property DepartamentosAfectados() As List(Of Sablib.ELL.Departamento)
            Get
                Return _dptos
            End Get
            Set(ByVal value As List(Of Sablib.ELL.Departamento))
                _dptos = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si es una puesta a punto o no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PuestaAPunto() As Boolean
            Get
                Return _puestaAPunto
            End Get
            Set(ByVal value As Boolean)
                _puestaAPunto = value
            End Set
        End Property

#End Region

    End Class

End Namespace