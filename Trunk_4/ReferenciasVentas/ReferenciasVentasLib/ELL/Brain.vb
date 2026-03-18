Namespace ELL

#Region "BRAIN BASE"

    Public Class BrainBase

#Region "Variables miembro"

        Private _elto As String = String.Empty
        Private _denoS As String = String.Empty

#End Region

#Region "Properties"

        ''' <summary>
        ''' Elto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ELTO() As String
            Get
                Return _elto
            End Get
            Set(ByVal value As String)
                _elto = value
            End Set
        End Property

        ''' <summary>
        ''' Deno_S
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property DENO_S() As String
            Get
                Return _denoS
            End Get
            Set(ByVal value As String)
                _denoS = value
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property CodigoProducto As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>ç
        Public Property Producto As String

#End Region

    End Class

#End Region
    
#Region "DATOS BRAIN"

    Public Class DatosBrain
        Private _planta As String = String.Empty
        Private _refPieza As String = String.Empty
        Private _idSolicitud As String = String.Empty
        Private _estado As String = String.Empty
        Private _descripcion1 As String = String.Empty
        Private _descripcion2 As String = String.Empty
        Private _matchCode As String = String.Empty
        Private _numDin As String = String.Empty
        Private _pasarDespieceWeb As String = String.Empty
        Private _pseudoSubconjunto As String = String.Empty
        Private _grupoMaterial As String = String.Empty
        Private _grupoProducto As String = String.Empty
        Private _refClientePlanoBatz As String = String.Empty
        Private _nivelIngenieria As String = String.Empty
        Private _planoWeb As String = String.Empty
        Private _material As String = String.Empty
        Private _dimensiones As String = String.Empty
        Private _pesoNeto As String = String.Empty
        Private _piezaCompraDirigida As String = String.Empty
        Private _proyecto As String = String.Empty
        Private _tipoProducto As String = String.Empty
        Private _observaciones As String = String.Empty
        Private _articuloRepuesto As String = String.Empty
        Private _numPiezasGolpe As String = String.Empty
        Private _medioFabricacion As String = String.Empty
        Private _tipoPieza As String = String.Empty
        Private _comentario As String = String.Empty
        Private _infoPlanta As List(Of InformacionPlanta)

        Structure InformacionPlanta
            Public Empresa As String
            'Public TipoPieza As String
            Public UnidadMedidaCantidad As String
            Public UnidadMedidaPrecio As String
            Public CategoriaProducto As String
            Public Disponente As String
            Public NumAlmacen As String
            Public Subproyecto As String
            Public ControlCalidad As String
            Public FlagCompleto As String
            Public FechaIntegracion As String
            Public FlagCorrecto As String
            Public CausaError As String
        End Structure

        ' ''' <summary>
        ' ''' Empresa
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>        
        'Public Property Empresa() As String
        '    Get
        '        Return _empresa
        '    End Get
        '    Set(ByVal value As String)
        '        _empresa = value
        '    End Set
        'End Property

        ''' <summary>
        ''' Planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As String
            Get
                Return _planta
            End Get
            Set(ByVal value As String)
                _planta = value
            End Set
        End Property

        ''' <summary>
        ''' RefPieza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property RefPieza() As String
            Get
                Return _refPieza
            End Get
            Set(ByVal value As String)
                _refPieza = value
            End Set
        End Property

        ''' <summary>
        ''' IdSolicitud
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdSolicitud() As String
            Get
                Return _idSolicitud
            End Get
            Set(ByVal value As String)
                _idSolicitud = value
            End Set
        End Property

        ''' <summary>
        ''' Estado
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Estado() As String
            Get
                Return _estado
            End Get
            Set(ByVal value As String)
                _estado = value
            End Set
        End Property

        ''' <summary>
        ''' Descripcion1
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Descripcion1() As String
            Get
                Return _descripcion1
            End Get
            Set(ByVal value As String)
                _descripcion1 = value
            End Set
        End Property

        ''' <summary>
        ''' Descripcion2
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Descripcion2() As String
            Get
                Return _descripcion2
            End Get
            Set(ByVal value As String)
                _descripcion2 = value
            End Set
        End Property

        ''' <summary>
        ''' MatchCode
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property MatchCode() As String
            Get
                Return _matchCode
            End Get
            Set(ByVal value As String)
                _matchCode = value
            End Set
        End Property

        ''' <summary>
        ''' NumDin
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NumDin() As String
            Get
                Return _numDin
            End Get
            Set(ByVal value As String)
                _numDin = value
            End Set
        End Property

        ''' <summary>
        ''' PasarDespieceWeb
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PasarDespieceWeb() As String
            Get
                Return _pasarDespieceWeb
            End Get
            Set(ByVal value As String)
                _pasarDespieceWeb = value
            End Set
        End Property

        ''' <summary>
        ''' PseudoSubconjunto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PseudoSubconjunto() As String
            Get
                Return _pseudoSubconjunto
            End Get
            Set(ByVal value As String)
                _pseudoSubconjunto = value
            End Set
        End Property

        ''' <summary>
        ''' GrupoMaterial
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property GrupoMaterial() As String
            Get
                Return _grupoMaterial
            End Get
            Set(ByVal value As String)
                _grupoMaterial = value
            End Set
        End Property

        ''' <summary>
        ''' GrupoProducto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property GrupoProducto() As String
            Get
                Return _grupoProducto
            End Get
            Set(ByVal value As String)
                _grupoProducto = value
            End Set
        End Property

        ''' <summary>
        ''' _refClientePlanoBatz
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property RefClientePlanoBatz() As String
            Get
                Return _refClientePlanoBatz
            End Get
            Set(ByVal value As String)
                _refClientePlanoBatz = value
            End Set
        End Property

        ''' <summary>
        ''' NivelIngenieria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NivelIngenieria() As String
            Get
                Return _nivelIngenieria
            End Get
            Set(ByVal value As String)
                _nivelIngenieria = value
            End Set
        End Property

        ''' <summary>
        ''' PlanoWeb
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PlanoWeb() As String
            Get
                Return _planoWeb
            End Get
            Set(ByVal value As String)
                _planoWeb = value
            End Set
        End Property

        ''' <summary>
        ''' Material
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Material() As String
            Get
                Return _material
            End Get
            Set(ByVal value As String)
                _material = value
            End Set
        End Property

        ''' <summary>
        ''' Dimensiones
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Dimensiones() As String
            Get
                Return _dimensiones
            End Get
            Set(ByVal value As String)
                _dimensiones = value
            End Set
        End Property

        ''' <summary>
        ''' PesoNeto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PesoNeto() As String
            Get
                Return _pesoNeto
            End Get
            Set(ByVal value As String)
                _pesoNeto = value
            End Set
        End Property

        ''' <summary>
        ''' PiezaCompraDirigida
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PiezaCompraDirigida() As String
            Get
                Return _piezaCompraDirigida
            End Get
            Set(ByVal value As String)
                _piezaCompraDirigida = value
            End Set
        End Property

        ''' <summary>
        ''' Proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Proyecto() As String
            Get
                Return _proyecto
            End Get
            Set(ByVal value As String)
                _proyecto = value
            End Set
        End Property

        ''' <summary>
        ''' TipoProducto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TipoProducto() As String
            Get
                Return _tipoProducto
            End Get
            Set(ByVal value As String)
                _tipoProducto = value
            End Set
        End Property

        ''' <summary>
        ''' TipoProducto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Observaciones() As String
            Get
                Return _observaciones
            End Get
            Set(ByVal value As String)
                _observaciones = value
            End Set
        End Property

        ''' <summary>
        ''' ArticuloRepuesto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ArticuloRepuesto() As String
            Get
                Return _articuloRepuesto
            End Get
            Set(ByVal value As String)
                _articuloRepuesto = value
            End Set
        End Property

        ''' <summary>
        ''' NumPiezasGolpe
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NumPiezasGolpe() As String
            Get
                Return _numPiezasGolpe
            End Get
            Set(ByVal value As String)
                _numPiezasGolpe = value
            End Set
        End Property

        ''' <summary>
        ''' MedioFabricacion
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property MedioFabricacion() As String
            Get
                Return _medioFabricacion
            End Get
            Set(ByVal value As String)
                _medioFabricacion = value
            End Set
        End Property

        ''' <summary>
        ''' TipoPieza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TipoPieza() As String
            Get
                Return _tipoPieza
            End Get
            Set(ByVal value As String)
                _tipoPieza = value
            End Set
        End Property

        ''' <summary>
        ''' Comentario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Comentario() As String
            Get
                Return _comentario
            End Get
            Set(ByVal value As String)
                _comentario = value
            End Set
        End Property

        ''' <summary>
        ''' Información por cada planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InfoPlanta() As List(Of InformacionPlanta)
            Get
                Return _infoPlanta
            End Get
            Set(ByVal value As List(Of InformacionPlanta))
                _infoPlanta = value
            End Set
        End Property

    End Class

#End Region
    
#Region "RESUMEN MAESTRO PIEZAS BRAIN"

    Public Class MaestroPiezasBrainResumen
        Private _planta As String = String.Empty
        Private _refPieza As String = String.Empty
        Private _refClientePlanoBatz As String = String.Empty
        Private _nivelIngenieria As String = String.Empty
        Private _planoWeb As String = String.Empty
        Private _idCustomerProject As String = String.Empty

        ''' <summary>
        ''' Planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As String
            Get
                Return _planta
            End Get
            Set(ByVal value As String)
                _planta = value
            End Set
        End Property

        ''' <summary>
        ''' Referencia de Pieza de Batz
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property RefPieza() As String
            Get
                Return _refPieza
            End Get
            Set(ByVal value As String)
                _refPieza = value
            End Set
        End Property

        ''' <summary>
        ''' Referencia de Client de Plano
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property RefClientePlanoBatz() As String
            Get
                Return _refClientePlanoBatz
            End Get
            Set(ByVal value As String)
                _refClientePlanoBatz = value
            End Set
        End Property

        ''' <summary>
        ''' NivelIngenieria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NivelIngenieria() As String
            Get
                Return _nivelIngenieria
            End Get
            Set(ByVal value As String)
                _nivelIngenieria = value
            End Set
        End Property

        ''' <summary>
        ''' PlanoWeb
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PlanoWeb() As String
            Get
                Return _planoWeb
            End Get
            Set(ByVal value As String)
                _planoWeb = value
            End Set
        End Property

        ''' <summary>
        ''' IdCustomerProject
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdCustomerProject() As String
            Get
                Return _idCustomerProject
            End Get
            Set(ByVal value As String)
                _idCustomerProject = value
            End Set
        End Property

    End Class

#End Region

#Region "RESUMEN MAESTRO PIEZAS BRAIN"

    'Public Class MaestroPiezasBrain
    '    Private _planta As String = String.Empty
    '    Private _refPieza As String = String.Empty
    '    Private _refCliente As String = String.Empty
    '    Private _idTipoPieza As String = String.Empty
    '    Private _idEstado As String = String.Empty
    '    Private _idGrupoMaterial As String = String.Empty
    '    Private _idGrupoProducto As String = String.Empty
    '    Private _piezaCompraDirigida As String = String.Empty
    '    Private _idTipoProducto As String = String.Empty
    '    Private _nivelIngenieria As String = String.Empty
    '    Private _pasarDespieceWeb As String = String.Empty
    '    Private _matchCode As String = String.Empty
    '    Private _planoWeb As String = String.Empty

    '    ''' <summary>
    '    ''' Planta
    '    ''' </summary>
    '    ''' <value></value>
    '    ''' <returns></returns>        
    '    Public Property Planta() As String
    '        Get
    '            Return _planta
    '        End Get
    '        Set(ByVal value As String)
    '            _planta = value
    '        End Set
    '    End Property

    '    ''' <summary>
    '    ''' RefPieza
    '    ''' </summary>
    '    ''' <value></value>
    '    ''' <returns></returns>        
    '    Public Property RefPieza() As String
    '        Get
    '            Return _refPieza
    '        End Get
    '        Set(ByVal value As String)
    '            _refPieza = value
    '        End Set
    '    End Property

    '    ''' <summary>
    '    ''' _refClientePlanoBatz
    '    ''' </summary>
    '    ''' <value></value>
    '    ''' <returns></returns>        
    '    Public Property RefCliente() As String
    '        Get
    '            Return _refCliente
    '        End Get
    '        Set(ByVal value As String)
    '            _refCliente = value
    '        End Set
    '    End Property

    '    ''' <summary>
    '    ''' NivelIngenieria
    '    ''' </summary>
    '    ''' <value></value>
    '    ''' <returns></returns>        
    '    Public Property NivelIngenieria() As String
    '        Get
    '            Return _nivelIngenieria
    '        End Get
    '        Set(ByVal value As String)
    '            _nivelIngenieria = value
    '        End Set
    '    End Property

    '    ''' <summary>
    '    ''' PlanoWeb
    '    ''' </summary>
    '    ''' <value></value>
    '    ''' <returns></returns>        
    '    Public Property PlanoWeb() As String
    '        Get
    '            Return _planoWeb
    '        End Get
    '        Set(ByVal value As String)
    '            _planoWeb = value
    '        End Set
    '    End Property

    'End Class

#End Region

End Namespace

