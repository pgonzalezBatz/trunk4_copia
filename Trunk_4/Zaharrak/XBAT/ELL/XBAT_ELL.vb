
Public MustInherit Class GCCilindros_ELL
	Private _CODART As String
	Private _CODPRO As String
	Private _ID As Nullable(Of Integer)
	Private _NUMPED As Nullable(Of Integer)
	Private _NUMLIN As Nullable(Of Integer)
	Private _NUMALBAR As Nullable(Of Integer)
	Private _SERIE As String

	''' <summary>
	''' Identificador unico del registro.
	''' </summary>
	Protected Property ID As Nullable(Of Integer)
		Get
			Return _ID
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_ID = value
		End Set
	End Property
	Public Property CODART As String
		Get
			Return _CODART
		End Get
		Set(ByVal value As String)
			_CODART = value
		End Set
	End Property
	Public Property CODPRO As String
		Get
			Return _CODPRO
		End Get
		Set(ByVal value As String)
			_CODPRO = value
		End Set
	End Property
	''' <summary>
	''' Numero de pedido.
	''' Campo de busqueda.
	''' </summary>
	Public Property NUMPED As Nullable(Of Integer)
		Get
			Return _NUMPED
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_NUMPED = value
		End Set
	End Property
	''' <summary>
	''' Numero de Linea.
	''' Campo de busqueda.
	''' </summary>
	Public Property NUMLIN As Nullable(Of Integer)
		Get
			Return _NUMLIN
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_NUMLIN = value
		End Set
	End Property
	''' <summary>
	''' Numero de Albaran.
	''' Campo de busqueda.
	''' </summary>
	Public Property NUMALBAR As Nullable(Of Integer)
		Get
			Return _NUMALBAR
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_NUMALBAR = value
		End Set
	End Property

	Public Property SERIE As String
		Get
			Return _SERIE
		End Get
		Set(ByVal value As String)
			_SERIE = value
		End Set
	End Property
End Class


Public MustInherit Class GCBulones_ELL
    Private _CODART As String
    Private _CODPRO As String
    Private _ID As Nullable(Of Integer)
    Private _NUMPED As Nullable(Of Integer)
    Private _NUMLIN As Nullable(Of Integer)
    Private _NUMALBAR As Nullable(Of Integer)
    Private _SERIE As String

    ''' <summary>
    ''' Identificador unico del registro.
    ''' </summary>
    Protected Property ID As Nullable(Of Integer)
        Get
            Return _ID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _ID = value
        End Set
    End Property
    Public Property CODART As String
        Get
            Return _CODART
        End Get
        Set(ByVal value As String)
            _CODART = value
        End Set
    End Property
    Public Property CODPRO As String
        Get
            Return _CODPRO
        End Get
        Set(ByVal value As String)
            _CODPRO = value
        End Set
    End Property
    ''' <summary>
    ''' Numero de pedido.
    ''' Campo de busqueda.
    ''' </summary>
    Public Property NUMPED As Nullable(Of Integer)
        Get
            Return _NUMPED
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _NUMPED = value
        End Set
    End Property
    ''' <summary>
    ''' Numero de Linea.
    ''' Campo de busqueda.
    ''' </summary>
    Public Property NUMLIN As Nullable(Of Integer)
        Get
            Return _NUMLIN
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _NUMLIN = value
        End Set
    End Property
    ''' <summary>
    ''' Numero de Albaran.
    ''' Campo de busqueda.
    ''' </summary>
    Public Property NUMALBAR As Nullable(Of Integer)
        Get
            Return _NUMALBAR
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _NUMALBAR = value
        End Set
    End Property

    Public Property SERIE As String
        Get
            Return _SERIE
        End Get
        Set(ByVal value As String)
            _SERIE = value
        End Set
    End Property
End Class


Public MustInherit Class FaPersonal_ELL
	Private _CODPER As Nullable(Of Integer)
	Private _USUARIO As String

	''' <summary>
	''' Identificador unico del registro.
	''' </summary>
	Protected Property CODPER As Nullable(Of Integer)
		Get
			Return _CODPER
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_CODPER = value
		End Set
	End Property
	Public Property USUARIO As String
		Get
			Return _USUARIO
		End Get
		Set(ByVal value As String)
			_USUARIO = value
		End Set
	End Property
End Class