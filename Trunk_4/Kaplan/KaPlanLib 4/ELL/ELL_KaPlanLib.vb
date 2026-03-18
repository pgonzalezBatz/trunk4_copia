
Public Structure PropiedadesGridView
	''' <summary>
	''' Identificador del Registro Seleccionado.
	''' </summary>
	Private _IdSeleccionado As Nullable(Of Integer)
	''' <summary>
	''' Nombre de la PROPIEDAD por la que se quiere ordenar los objetos.
	''' </summary>
	Private _CampoOrdenacion As String
	''' <summary>
	''' Direccion de Ordenacion para el Campo de Ordenacion (Nombre de la Propiedad).
	''' </summary>
	Private _DireccionOrdenacion As Nullable(Of System.ComponentModel.ListSortDirection)

	''' <summary>
	''' Identificador del Registro Seleccionado.
	''' </summary>
	Public Property IdSeleccionado() As Nullable(Of Integer)
		Get
			Return _IdSeleccionado
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_IdSeleccionado = value
		End Set
	End Property
	''' <summary>
	''' Nombre de la PROPIEDAD por la que se quiere ordenar los objetos.
	''' </summary>
	Public Property CampoOrdenacion() As String
		Get
			Return _CampoOrdenacion
		End Get
		Set(ByVal value As String)
			_CampoOrdenacion = value
		End Set
	End Property
	''' <summary>
	''' Direccion de Ordenacion para el Campo de Ordenacion (Nombre de la Propiedad).
	''' </summary>
	Public Property DireccionOrdenacion() As Nullable(Of System.ComponentModel.ListSortDirection)
		Get
			Return _DireccionOrdenacion
		End Get
		Set(ByVal value As Nullable(Of System.ComponentModel.ListSortDirection))
			_DireccionOrdenacion = value
		End Set
	End Property
End Structure