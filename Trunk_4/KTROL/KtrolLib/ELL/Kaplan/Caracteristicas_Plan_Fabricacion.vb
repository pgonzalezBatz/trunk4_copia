Namespace ELL

    Public Class Caracteristicas_Plan_Fabricacion

#Region "Variables miembro"

        Private _idRegistro As Integer = Integer.MinValue
        Private _codigo As String = String.Empty
        Private _posicion As String = String.Empty
        Private _caracParam As String = String.Empty
        Private _clase As String = String.Empty
        Private _metodoControl As String = String.Empty
        Private _maxim As String = String.Empty
        Private _minim As String = String.Empty
        Private _operacion As Integer = Integer.MinValue
        Private _especificacion As String = String.Empty
        Private _verRegPro As Boolean = False
        Private _verRegRec As Boolean = False
        Private _verRegMat As Boolean = False
        Private _verRegFun As Boolean = False
        Private _verRegDim As Boolean = False
        Private _tamanyoCal As String = String.Empty
        Private _tamanyo As String = String.Empty
        Private _responsableRegOpe As String = String.Empty
        Private _responsableRegCal As String = String.Empty
        Private _responsableRegGestor As String = String.Empty
        Private _responsableRegistro As String = String.Empty
        Private _responsable As String = String.Empty
        Private _procesoProducto As String = String.Empty
        Private _procedeDe As String = String.Empty
        Private _ordenCarac As String = String.Empty
        Private _observaciones As String = String.Empty
        Private _metodoEvaluacion As String = String.Empty
        Private _medioRFA As String = String.Empty
        Private _medioDenominacion As String = String.Empty
        Private _maquina As String = String.Empty
        Private _idCaracteristica As String = String.Empty
        Private _hojaRegistros As Boolean = False
        Private _frecuenciaRegistro As String = String.Empty
        Private _frecuenciaControlCal As String = String.Empty
        Private _frecuenciaControl As String = String.Empty
        Private _contCausa As Integer = Integer.MinValue
        Private _accionRecomendada As String = String.Empty

#End Region

#Region "Properties"

        Public Property ID_REGISTRO() As Integer
            Get
                Return _idRegistro
            End Get
            Set(ByVal value As Integer)
                _idRegistro = value
            End Set
        End Property

        Public Property CODIGO() As String
            Get
                Return _codigo
            End Get
            Set(ByVal value As String)
                _codigo = value
            End Set
        End Property

        Public Property POSICION() As String
            Get
                Return _posicion
            End Get
            Set(ByVal value As String)
                _posicion = value
            End Set
        End Property

        Public Property CARAC_PARAM() As String
            Get
                Return _caracParam
            End Get
            Set(ByVal value As String)
                _caracParam = value
            End Set
        End Property

        Public Property CLASE() As String
            Get
                Return _clase
            End Get
            Set(ByVal value As String)
                _clase = value
            End Set
        End Property

        Public Property METODO_CONTROL() As String
            Get
                Return _metodoControl
            End Get
            Set(ByVal value As String)
                _metodoControl = value
            End Set
        End Property

        Public Property MAXIM() As String
            Get
                Return _maxim
            End Get
            Set(ByVal value As String)
                _maxim = value
            End Set
        End Property

        Public Property MINIM() As String
            Get
                Return _minim
            End Get
            Set(ByVal value As String)
                _minim = value
            End Set
        End Property

        Public Property ESPECIFICACION() As String
            Get
                Return _especificacion
            End Get
            Set(ByVal value As String)
                _especificacion = value
            End Set
        End Property

        Public Property OPERACION() As Integer
            Get
                Return _operacion
            End Get
            Set(ByVal value As Integer)
                _operacion = value
            End Set
        End Property

        Public Property VER_REG_PRO() As Boolean
            Get
                Return _verRegPro
            End Get
            Set(ByVal value As Boolean)
                _verRegPro = value
            End Set
        End Property

        Public Property VER_REG_REC() As Boolean
            Get
                Return _verRegRec
            End Get
            Set(ByVal value As Boolean)
                _verRegRec = value
            End Set
        End Property

        Public Property VER_REG_MAT() As Boolean
            Get
                Return _verRegMat
            End Get
            Set(ByVal value As Boolean)
                _verRegMat = value
            End Set
        End Property

        Public Property VER_REG_FUN() As Boolean
            Get
                Return _verRegFun
            End Get
            Set(ByVal value As Boolean)
                _verRegFun = value
            End Set
        End Property

        Public Property VER_REG_DIM() As Boolean
            Get
                Return _verRegDim
            End Get
            Set(ByVal value As Boolean)
                _verRegDim = value
            End Set
        End Property

        Public Property TAMANYO_CAL() As String
            Get
                Return _tamanyoCal
            End Get
            Set(ByVal value As String)
                _tamanyoCal = value
            End Set
        End Property

        Public Property TAMANYO() As String
            Get
                Return _tamanyo
            End Get
            Set(ByVal value As String)
                _tamanyo = value
            End Set
        End Property

        Public Property Responsable_Reg_Ope() As String
            Get
                Return _responsableRegOpe
            End Get
            Set(ByVal value As String)
                _responsableRegOpe = value
            End Set
        End Property

        Public Property Responsable_Reg_Cal() As String
            Get
                Return _responsableRegCal
            End Get
            Set(ByVal value As String)
                _responsableRegCal = value
            End Set
        End Property

        Public Property Responsable_Reg_Gestor() As String
            Get
                Return _responsableRegGestor
            End Get
            Set(ByVal value As String)
                _responsableRegGestor = value
            End Set
        End Property

        Public Property Responsable_Registro() As String
            Get
                Return _responsableRegistro
            End Get
            Set(ByVal value As String)
                _responsableRegistro = value
            End Set
        End Property

        Public Property Responsable() As String
            Get
                Return _responsable
            End Get
            Set(ByVal value As String)
                _responsable = value
            End Set
        End Property

        Public Property PROCESO_PRODUCTO() As String
            Get
                Return _procesoProducto
            End Get
            Set(ByVal value As String)
                _procesoProducto = value
            End Set
        End Property

        Public Property PROCEDE_DE() As String
            Get
                Return _procedeDe
            End Get
            Set(ByVal value As String)
                _procedeDe = value
            End Set
        End Property

        Public Property ORDEN_CARAC() As String
            Get
                Return _ordenCarac
            End Get
            Set(ByVal value As String)
                _ordenCarac = value
            End Set
        End Property

        Public Property OBSERVACIONES() As String
            Get
                Return _observaciones
            End Get
            Set(ByVal value As String)
                _observaciones = value
            End Set
        End Property

        Public Property METODO_EVALUACION() As String
            Get
                Return _metodoEvaluacion
            End Get
            Set(ByVal value As String)
                _metodoEvaluacion = value
            End Set
        End Property

        Public Property MEDIO_RFA() As String
            Get
                Return _medioRFA
            End Get
            Set(ByVal value As String)
                _medioRFA = value
            End Set
        End Property

        Public Property MEDIO_DENOMINACION() As String
            Get
                Return _medioDenominacion
            End Get
            Set(ByVal value As String)
                _medioDenominacion = value
            End Set
        End Property

        Public Property MAQUINA() As String
            Get
                Return _maquina
            End Get
            Set(ByVal value As String)
                _maquina = value
            End Set
        End Property

        Public Property ID_CARACTERISTICA() As Integer
            Get
                Return _idCaracteristica
            End Get
            Set(ByVal value As Integer)
                _idCaracteristica = value
            End Set
        End Property

        Public Property HOJA_REGISTROS() As Boolean
            Get
                Return _hojaRegistros
            End Get
            Set(ByVal value As Boolean)
                _hojaRegistros = value
            End Set
        End Property

        Public Property FRECUENCIA_REGISTRO() As String
            Get
                Return _frecuenciaRegistro
            End Get
            Set(ByVal value As String)
                _frecuenciaRegistro = value
            End Set
        End Property

        Public Property FRECUENCIA_CONTROL_CAL() As String
            Get
                Return _frecuenciaControlCal
            End Get
            Set(ByVal value As String)
                _frecuenciaControlCal = value
            End Set
        End Property

        Public Property FRECUENCIA_CONTROL() As String
            Get
                Return _frecuenciaControl
            End Get
            Set(ByVal value As String)
                _frecuenciaControl = value
            End Set
        End Property

        Public Property CONT_CAUSA() As Integer
            Get
                Return _contCausa
            End Get
            Set(ByVal value As Integer)
                _contCausa = value
            End Set
        End Property

        Public Property ACCION_RECOMENDADA() As String
            Get
                Return _accionRecomendada
            End Get
            Set(ByVal value As String)
                _accionRecomendada = value
            End Set
        End Property

#End Region

    End Class

End Namespace

