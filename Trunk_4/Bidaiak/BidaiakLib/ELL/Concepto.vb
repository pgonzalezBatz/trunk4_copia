Namespace ELL

    <Serializable()> _
    Public Class Concepto

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _bRequiereDet As Boolean = False
        Private _bMostrarHGRecibo As Boolean = False
        Private _bMostrarHGSinRecibo As Boolean = False
        Private _bObsoleto As Boolean = False
        Private _idPlanta As Integer = Integer.MinValue
        Private _desconocido As Boolean = False

#End Region

#Region "Properties"

        ''' <summary>
        ''' Identificador del concepto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        ''' <summary>
        ''' Nombre del concepto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Nombre As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si al seleccionar este elemento, obligara a que se meta un texto explicativo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property RequiereDetalle As Boolean
            Get
                Return _bRequiereDet
            End Get
            Set(ByVal value As Boolean)
                _bRequiereDet = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si se mostrara como linea en las hojas de gastos con recibo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property MostrarHojaGastosRecibo As Boolean
            Get
                Return _bMostrarHGRecibo
            End Get
            Set(ByVal value As Boolean)
                _bMostrarHGRecibo = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si se mostrara como linea en las hojas de gastos sin recibo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property MostrarHojaGastosSinRecibo As Boolean
            Get
                Return _bMostrarHGSinRecibo
            End Get
            Set(ByVal value As Boolean)
                _bMostrarHGSinRecibo = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si el concepto esta obsoleto o no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Obsoleto As Boolean
            Get
                Return _bObsoleto
            End Get
            Set(ByVal value As Boolean)
                _bObsoleto = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador de la planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlanta As Integer
            Get
                Return _idPlanta
            End Get
            Set(ByVal value As Integer)
                _idPlanta = value
            End Set
        End Property

#End Region

    End Class

End Namespace