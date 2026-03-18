Namespace ELL

    Public Class ControlesValoresResumen

#Region "Variables miembro"

        Private _idControl As Integer = Integer.MinValue
        Private _idRegistro As Integer = Integer.MinValue
        Private _valor As String = String.Empty
        Private _caracParam As String = String.Empty
        Private _especificacion As String = String.Empty
        Private _posicion As String = String.Empty
        'Hoja de registros
        Private _tipo As String = String.Empty
        Private _okNok As Integer = Integer.MinValue

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id del control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdControl() As Integer
            Get
                Return _idControl
            End Get
            Set(ByVal value As Integer)
                _idControl = value
            End Set
        End Property

        ''' <summary>
        ''' Id del registro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdRegistro() As Integer
            Get
                Return _idRegistro
            End Get
            Set(ByVal value As Integer)
                _idRegistro = value
            End Set
        End Property

        ''' <summary>
        ''' Valor de la característica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Valor() As String
            Get
                Return _valor
            End Get
            Set(ByVal value As String)
                _valor = value
            End Set
        End Property

        ''' <summary>
        ''' Parámetro de la característica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CaracParam() As String
            Get
                Return _caracParam
            End Get
            Set(ByVal value As String)
                _caracParam = value
            End Set
        End Property

        ''' <summary>
        ''' Especificación de la característica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Especificacion() As String
            Get
                Return _especificacion
            End Get
            Set(ByVal value As String)
                _especificacion = value
            End Set
        End Property

        ''' <summary>
        ''' Posición de la característica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Posicion() As String
            Get
                Return _posicion
            End Get
            Set(ByVal value As String)
                _posicion = value
            End Set
        End Property

        ''' <summary>
        ''' Tipo de control de la característica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Tipo() As String
            Get
                Return _tipo
            End Get
            Set(ByVal value As String)
                _tipo = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si la característica se ha realizado bien o mal (2 en caso de no haberse realizado)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property OkNok() As String
            Get
                Return _okNok
            End Get
            Set(ByVal value As String)
                _okNok = value
            End Set
        End Property

#End Region

    End Class

End Namespace

