Namespace Entidades

	Public MustInherit Class Asset_ELL
		Private _asset As String
		Private _AssetName As String
		Private _Company As Nullable(Of Planta)
		Private _CompanyLevel As Nullable(Of Integer)
		Private _ParentAsset As String

		''' <summary>
		''' Empresas de BATZ para Prisma.
		''' </summary>
		Public Enum Planta
			''' <summary>
			''' Identificador para la empresa de Igorre.
			''' </summary>
			IGORRE_S

		End Enum

		''' <summary>
		''' Identificador de la empresa.
		''' Campo de busqueda.
		''' </summary>
		Public Property Company As Nullable(Of Planta)
			Get
				Return _Company
			End Get
			Set(ByVal value As Nullable(Of Planta))
				_Company = value
			End Set
		End Property

		''' <summary>
		''' Identificador del registro.
		''' Campo de Busqueda.
		''' </summary>
		Public Property Asset As String
			Get
				Return _asset
			End Get
			Set(ByVal value As String)
				_asset = value
			End Set
		End Property

		''' <summary>
		''' Identificardor del registro "Padre".
		''' Campo de busqueda.
		''' </summary>
		Public Property ParentAsset As String
			Get
				Return _ParentAsset
			End Get
			Set(ByVal value As String)
				_ParentAsset = value
			End Set
		End Property

		''' <summary>
		''' Descripcion.
		''' </summary>
		Public Property AssetName As String
			Get
				Return _AssetName
			End Get
			Set(ByVal value As String)
				_AssetName = value
			End Set
		End Property

		''' <summary>
		''' Niveles de la instalacion (Instalacion=2 --> Familia=3 --> Linea=4).
		''' Campo de busqueda.
		''' </summary>
		''' <remarks>
		''' La Instalacion consta de 3 niveles:
		''' 1.	Instalacion	(CompanyLevel = 2)
		''' 2.	Familia		(CompanyLevel = 3)
		''' 3.	Linea		(CompanyLevel = 4)
		''' </remarks>
		Public Property CompanyLevel As Nullable(Of Integer)
			Get
				Return _CompanyLevel
			End Get
			Set(ByVal value As Nullable(Of Integer))
				_CompanyLevel = value
			End Set
		End Property
	End Class
End Namespace