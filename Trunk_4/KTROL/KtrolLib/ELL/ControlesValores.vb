Namespace ELL

    Public Class ControlesValores

#Region "Variables miembro"

        Private _idRegistro As Integer = Integer.MinValue
        Private _valor As String = String.Empty
        'Private _operacion As Integer = Integer.MinValue
        'Private _maquina As String = String.Empty
        Private _posicion As String = String.Empty
        Private _ordenCarac As Single = Single.MinValue
        Private _caracParam As String = String.Empty
        Private _especificacion As String = String.Empty
		'Private _frecuenciaControl As String = String.Empty
		'Private _frecuenciaRegistro As String = String.Empty
		'Private _metodoEvaluacion As String = String.Empty
		'Private _responsable As String = String.Empty
		'Private _medioDenominacion As String = String.Empty
		'Private _medioRFA As String = String.Empty
		Private _clase As String = String.Empty
		'Private _observaciones As String = String.Empty
		'Private _accionRecomendada As String = String.Empty
		'Private _responsableRegistro As String = String.Empty
		'Private _contCausa As Integer = Integer.MinValue
		'Private _idCaracteristica As Integer = Integer.MinValue
		'Private _procedeDe As String = String.Empty
		'Private _procesoProducto As String = String.Empty
		'Private _tamaño As String = String.Empty
		'Private _metodoControl As String = String.Empty
		'Private _hojaRegistros As Boolean = False
		'Private _verRegRec As Boolean = False
		'Private _verRegPro As Boolean = False
		'Private _verRegDim As Boolean = False
		'Private _verRegMat As Boolean = False
		'Private _verRegFun As Boolean = False
		'Private _maxim As Single = Single.MinValue
		'Private _minim As Single = Single.MinValue
		'Private _frecuenciaControlCal As String = String.Empty
		'Private _tamañoCal As String = String.Empty
		'Cambios para la hoja de registros
		Private _tipo As String = String.Empty
        Private _okNok As Integer = Integer.MinValue

#End Region

#Region "Properties"

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

        '''' <summary>
        '''' Codigo de la operación
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>        
        'Public Property Operacion() As Integer
        '    Get
        '        Return _operacion
        '    End Get
        '    Set(ByVal value As Integer)
        '        _operacion = value
        '    End Set
        'End Property

        '''' <summary>
        '''' 
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>        
        'Public Property Maquina() As String
        '    Get
        '        Return _maquina
        '    End Get
        '    Set(ByVal value As String)
        '        _maquina = value
        '    End Set
        'End Property

        ''' <summary>
        ''' 
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
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property OrdenCarac() As Single
            Get
                Return _ordenCarac
            End Get
            Set(ByVal value As Single)
                _ordenCarac = value
            End Set
        End Property

        ''' <summary>
        ''' 
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
        ''' 
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

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property FrecuenciaControl() As String
		'    Get
		'        Return _frecuenciaControl
		'    End Get
		'    Set(ByVal value As String)
		'        _frecuenciaControl = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property FrecuenciaRegistro() As String
		'    Get
		'        Return _frecuenciaRegistro
		'    End Get
		'    Set(ByVal value As String)
		'        _frecuenciaRegistro = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property MetodoEvaluacion() As String
		'    Get
		'        Return _metodoEvaluacion
		'    End Get
		'    Set(ByVal value As String)
		'        _metodoEvaluacion = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property Responsable() As String
		'    Get
		'        Return _responsable
		'    End Get
		'    Set(ByVal value As String)
		'        _responsable = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property MedioDenominacion() As String
		'    Get
		'        Return _medioDenominacion
		'    End Get
		'    Set(ByVal value As String)
		'        _medioDenominacion = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property MedioRFA() As String
		'    Get
		'        Return _medioRFA
		'    End Get
		'    Set(ByVal value As String)
		'        _medioRFA = value
		'    End Set
		'End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>        
		Public Property Clase() As String
			Get
				Return _clase
			End Get
			Set(ByVal value As String)
				_clase = value
			End Set
		End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property Observaciones() As String
		'    Get
		'        Return _observaciones
		'    End Get
		'    Set(ByVal value As String)
		'        _observaciones = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property AccionRecomendada() As String
		'    Get
		'        Return _accionRecomendada
		'    End Get
		'    Set(ByVal value As String)
		'        _accionRecomendada = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property ResponsableRegistro() As String
		'    Get
		'        Return _responsableRegistro
		'    End Get
		'    Set(ByVal value As String)
		'        _responsableRegistro = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>
		'''' <remarks></remarks>
		'Public Property ContCausa() As Integer
		'    Get
		'        Return _contCausa
		'    End Get
		'    Set(ByVal value As Integer)
		'        _contCausa = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>
		'''' <remarks></remarks>
		'Public Property IdCaracteristica() As Integer
		'    Get
		'        Return _idCaracteristica
		'    End Get
		'    Set(ByVal value As Integer)
		'        _idCaracteristica = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property ProcedeDe() As String
		'    Get
		'        Return _procedeDe
		'    End Get
		'    Set(ByVal value As String)
		'        _procedeDe = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property ProcesoProducto() As String
		'    Get
		'        Return _procesoProducto
		'    End Get
		'    Set(ByVal value As String)
		'        _procesoProducto = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property Tamaño() As String
		'    Get
		'        Return _tamaño
		'    End Get
		'    Set(ByVal value As String)
		'        _tamaño = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property MetodoControl() As String
		'    Get
		'        Return _metodoControl
		'    End Get
		'    Set(ByVal value As String)
		'        _metodoControl = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property HojaRegistros() As Boolean
		'    Get
		'        Return _hojaRegistros
		'    End Get
		'    Set(ByVal value As Boolean)
		'        _hojaRegistros = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property VerRegRec() As Boolean
		'    Get
		'        Return _verRegRec
		'    End Get
		'    Set(ByVal value As Boolean)
		'        _verRegRec = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property VerRegPro() As Boolean
		'    Get
		'        Return _verRegPro
		'    End Get
		'    Set(ByVal value As Boolean)
		'        _verRegPro = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property VerRegDim() As Boolean
		'    Get
		'        Return _verRegDim
		'    End Get
		'    Set(ByVal value As Boolean)
		'        _verRegDim = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property VerRegMat() As Boolean
		'    Get
		'        Return _verRegMat
		'    End Get
		'    Set(ByVal value As Boolean)
		'        _verRegMat = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property VerRegFun() As Boolean
		'    Get
		'        Return _verRegFun
		'    End Get
		'    Set(ByVal value As Boolean)
		'        _verRegFun = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property Maxim() As Single
		'    Get
		'        Return _maxim
		'    End Get
		'    Set(ByVal value As Single)
		'        _maxim = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property Minim() As Single
		'    Get
		'        Return _minim
		'    End Get
		'    Set(ByVal value As Single)
		'        _minim = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property FrecuenciaControlCal() As String
		'    Get
		'        Return _frecuenciaControlCal
		'    End Get
		'    Set(ByVal value As String)
		'        _frecuenciaControlCal = value
		'    End Set
		'End Property

		'''' <summary>
		'''' 
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>        
		'Public Property TamañoCal() As String
		'    Get
		'        Return _tamañoCal
		'    End Get
		'    Set(ByVal value As String)
		'        _tamañoCal = value
		'    End Set
		'End Property

		'Cambios para la hoja de registros

		''' <summary>
		''' 
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
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>        
		Public Property OkNok() As Integer
            Get
                Return _okNok
            End Get
            Set(ByVal value As Integer)
                _okNok = value
            End Set
        End Property
#End Region

    End Class

End Namespace

