Namespace ELL

    Public Class Alveolo

#Region "Enumeracion del estado"

        Public Enum EstadoAlv As Integer
            ok = 0
            noOk = 1
        End Enum

#End Region

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _ruta As String = String.Empty
        Private _estado As Boolean = False
        Private _idTipoAlv As Integer = Integer.MinValue
        Private _idPlanta As Integer = Integer.MinValue
        Private _obsoleto As Boolean = False
        Private _posicionFila As Integer = Integer.MinValue
        Private _posicionColumna As Integer = Integer.MinValue

#End Region

#Region "Columnas"

        ''' <summary>
        ''' Identificador unico
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property


        ''' <summary>
        ''' Ruta de un alveolo
        ''' <example>4-4-11</example>
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Ruta() As String
            Get
                Return _ruta
            End Get
            Set(ByVal value As String)
                _ruta = value
            End Set
        End Property


        ''' <summary>
        ''' El estado del alveolo (OK, no OK)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Estado() As Boolean
            Get
                Return _estado
            End Get
            Set(ByVal value As Boolean)
                _estado = value
            End Set
        End Property


        ''' <summary>
        ''' Identificador del tipo de alveolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdTipo() As Integer
            Get
                Return _idTipoAlv
            End Get
            Set(ByVal value As Integer)
                _idTipoAlv = value
            End Set
        End Property


        ''' <summary>
        ''' Identificador de la planta a la que pertenece el alveolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdPlanta() As Integer
            Get
                Return _idPlanta
            End Get
            Set(ByVal value As Integer)
                _idPlanta = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si el tipo esta obsoleto o no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Obsoleto() As Boolean
            Get
                Return _obsoleto
            End Get
            Set(ByVal value As Boolean)
                _obsoleto = value
            End Set
        End Property

        ''' <summary>
        ''' Fila donde se pincha en alveolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PosicionFila() As Integer
            Get
                Return _posicionFila
            End Get
            Set(ByVal value As Integer)
                _posicionFila = value
            End Set
        End Property

        ''' <summary>
        ''' Columna donde se pincha en alveolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PosicionColumna() As Integer
            Get
                Return _posicionColumna
            End Get
            Set(ByVal value As Integer)
                _posicionColumna = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()

        End Sub

        Public Sub New(ByVal pId As Integer)
            _id = pId
        End Sub

        Public Sub New(ByVal pRuta As String)
            _ruta = pRuta
        End Sub

#End Region

#Region "Property Names"

        ''' <summary>
        ''' Clase para definir los nombres de las propiedades de la clase
        ''' </summary>
        ''' <remarks></remarks>
        Public Class PropertyNames
            Private Const _ID As String = "ID"
            Private Const _RUTA As String = "RUTA"

            Public Shared ReadOnly Property ID() As String
                Get
                    Return _ID
                End Get
            End Property

            Public Shared ReadOnly Property RUTA() As String
                Get
                    Return _RUTA
                End Get
            End Property

        End Class

#End Region

    End Class

End Namespace
