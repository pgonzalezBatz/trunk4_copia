Namespace ELL
	Public Class EjecucionProc

#Region "Variables miembro"

		Private _id As Integer = Integer.MinValue
		Private _idUsuario As Integer = Integer.MinValue
		Private _fecha As DateTime = DateTime.MinValue
		Private _flag As Integer = Integer.MinValue
		Private _descripcion As String = String.Empty

#End Region

#Region "Properties"

		Public Property Id() As Integer
			Get
				Return _id
			End Get
			Set(ByVal value As Integer)
				_id = value
			End Set
		End Property

		Public Property IdUsuario() As Integer
			Get
				Return _idUsuario
			End Get
			Set(ByVal value As Integer)
				_idUsuario = value
			End Set
		End Property

		Public Property Fecha() As DateTime
			Get
				Return _fecha
			End Get
			Set(ByVal value As DateTime)
				_fecha = value
			End Set
		End Property

		Public Property Flag() As Integer
			Get
				Return _flag
			End Get
			Set(ByVal value As Integer)
				_flag = value
			End Set
		End Property

		Public Property Descripcion() As String
			Get
				Return _descripcion
			End Get
			Set(ByVal value As String)
				_descripcion = value
			End Set
		End Property

#End Region

	End Class
End Namespace
