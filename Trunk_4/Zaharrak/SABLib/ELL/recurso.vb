Namespace ELL
    Public Class recurso

#Region "VARIABLES MIEMBRO"
        Private _id As Integer
        Private _idParent As Integer        
        Private _url As String
        Private _obsoleto As Integer
        Private _urlImagen As String
        Private _imagen As Byte()
        Private _nombre As String
        Private _tipo As Char
        Private _idCultura As String
        Private _tieneIcono As Boolean
		Private _descripcion As String
		Private _visible As Boolean
		Private _fichero As Byte()
		Private _nombreFichero As String
		Private _target As String
#End Region

#Region "CONSTRUCTOR"
        Public Sub New()
            _id = Integer.MinValue
            _idParent = Integer.MinValue
            _url = String.Empty
            _obsoleto = Integer.MinValue
            _urlImagen = String.Empty
            _tipo = String.Empty
            _idCultura = String.Empty
            _tieneIcono = False
			_descripcion = String.Empty
			_visible = True	   'Por defecto todo son true. Todos los que existen con valor null seran visibles
			_fichero = Nothing
			_nombreFichero = String.Empty
			_target = String.Empty
        End Sub
#End Region

#Region "PROPERTIES"
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property IdParent() As Integer
            Get
                Return _idParent
            End Get
            Set(ByVal value As Integer)
                _idParent = value
            End Set
        End Property

        Public Property Url() As String
            Get
                Return _url
            End Get
            Set(ByVal value As String)
                _url = value
            End Set
        End Property

        Public Property Obsoleto() As Integer
            Get
                Return _obsoleto
            End Get
            Set(ByVal value As Integer)
                _obsoleto = value
            End Set
        End Property

        Public Property UrlImagen() As String
            Get
                Return _urlImagen
            End Get
            Set(ByVal value As String)
                _urlImagen = value
            End Set
        End Property

        Public Property Imagen() As Byte()
            Get
                Return _imagen
            End Get
            Set(ByVal value As Byte())
                _imagen = value
            End Set
        End Property


        Public Property Tipo() As Char
            Get
                Return _tipo
            End Get
            Set(ByVal value As Char)
                _tipo = value
            End Set
        End Property

        Public Property IdCultura() As String
            Get
                Return _idCultura
            End Get
            Set(ByVal value As String)
                _idCultura = value
            End Set
        End Property

		Public ReadOnly Property TieneIcono() As Boolean
			Get
				Return (_imagen IsNot Nothing AndAlso _imagen.Length > 0)
			End Get
		End Property

		Public ReadOnly Property TieneFichero() As Boolean
			Get
				Return (_fichero IsNot Nothing AndAlso _fichero.Length > 0)
			End Get			
		End Property

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
		End Property

		Public Property Visible() As Boolean
			Get
				Return _visible
			End Get
			Set(ByVal value As Boolean)
				_visible = value
			End Set
		End Property

		Public Property Fichero() As Byte()
			Get
				Return _fichero
			End Get
			Set(ByVal value As Byte())
				_fichero = value
			End Set
		End Property

		Public Property NombreFichero() As String
			Get
				Return _nombreFichero
			End Get
			Set(ByVal value As String)
				_nombreFichero = value
			End Set
		End Property

		Public Property Target() As String
			Get
				Return _target
			End Get
			Set(ByVal value As String)
				_target = value
			End Set
		End Property


#End Region

#Region "Columns Names"

        Public Class ColumnNames
            Private Const _ID As String = "Id"
            Private Const _NOMBRE As String = "Nombre"
			Private Const _IDCULTURA As String = "IdCultura"
			Private Const _VISIBLE As String = "Visible"

            Public Shared ReadOnly Property ID() As String
                Get
                    Return _ID
                End Get
            End Property

            Public Shared ReadOnly Property NOMBRE() As String
                Get
                    Return _NOMBRE
                End Get
            End Property

            Public Shared ReadOnly Property IDCULTURA() As String
                Get
                    Return _IDCULTURA
                End Get
			End Property

			Public Shared ReadOnly Property VISIBLE() As String
				Get
					Return _VISIBLE
				End Get
			End Property

        End Class

#End Region

#Region "Orden"
        <Obsolete("ORDENAR EN LA INTERFAZ")> _
        Public Class SortClass
            Implements System.Collections.Generic.IComparer(Of recurso)

            ''' <summary>
            ''' Nombre del campo por el que hay que ordenar
            ''' </summary>
            Private _NombreCampo As String
            ''' <summary>
            ''' Direccion de ordenamiento
            ''' </summary>
            ''' <remarks></remarks>
            Private _direccion As Direction

            ''' <summary>
            ''' Constructor que inicializa lka clase
            ''' </summary>
            ''' <param name="nombre">Nombre del campo</param>
            ''' <param name="dir">Direccion del ordenamiento</param>
            Public Sub New(ByVal nombre As String, ByVal dir As Direction)
                _NombreCampo = nombre
                _direccion = dir
            End Sub

            ''' <summary>
            ''' Enumeracion para indicar el tipo de orden que se va a realizar (Ascendente o descendente)
            ''' </summary>
            Public Enum Direction
                ASC
                DESC
            End Enum

            ''' <summary>
            ''' Funcion de comparacion que se usara al ordenar
            ''' </summary>
            ''' <param name="rec1">Objeto 1 a comparar</param>
            ''' <param name="rec2">Objeto 2 a comparar</param>
            ''' <returns></returns>
            Public Function Compare1(ByVal rec1 As recurso, ByVal rec2 As recurso) As Integer Implements System.Collections.Generic.IComparer(Of recurso).Compare

                If (_direccion = Direction.ASC) Then    'ORDEN ASCENDENTE
                    Select Case _NombreCampo
                        Case "Id"
                            Return rec1.Id < rec2.Id
                        Case "Nombre"
                            Return rec1.Nombre < rec2.Nombre
                    End Select
                Else                                    'ORDEN DESCENDENTE
                    Select Case _NombreCampo
                        Case "Id"
                            Return rec1.Id > rec2.Id
                        Case "Nombre"
                            Return rec1.Nombre > rec2.Nombre
                    End Select
                End If
            End Function

        End Class
#End Region

    End Class
End Namespace
