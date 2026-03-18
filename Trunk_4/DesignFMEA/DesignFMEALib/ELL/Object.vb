Namespace ELL

    Public Class Amfe

        Private _Id As Integer
        Private _IdProyecto As String
        Private _Proyecto As String
        Private _IdUser As Integer
        Private _FechaCreacion As DateTime
        Private _Referencias As List(Of Referencia)
        Private _Producto As String
        Private _Tipo As Integer
        Private _TipoString As String

        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(ByVal value As Integer)
                _Id = value
            End Set
        End Property

        Public Property IdProyecto() As String
            Get
                Return _IdProyecto
            End Get
            Set(ByVal value As String)
                _IdProyecto = value
            End Set
        End Property

        Public Property IdUser() As Integer
            Get
                Return _IdUser
            End Get
            Set(ByVal value As Integer)
                _IdUser = value
            End Set
        End Property

        Public Property FechaCreacion() As DateTime
            Get
                Return _FechaCreacion
            End Get
            Set(ByVal value As DateTime)
                _FechaCreacion = value
            End Set
        End Property

        Public Property Referencias As List(Of Referencia)
            Get
                Return _Referencias
            End Get
            Set(value As List(Of Referencia))
                _Referencias = value
            End Set
        End Property

        Public Property Proyecto As String
            Get
                Return _Proyecto
            End Get
            Set(value As String)
                _Proyecto = value
            End Set
        End Property

        Public Property Producto As String
            Get
                Return _Producto
            End Get
            Set(value As String)
                _Producto = value
            End Set
        End Property

        Public Property Tipo As Integer
            Get
                Return _Tipo
            End Get
            Set(value As Integer)
                _Tipo = value
            End Set
        End Property

        Public Property TipoString As String
            Get
                Return _TipoString
            End Get
            Set(value As String)
                _TipoString = value
            End Set
        End Property

    End Class

    Public Class Referencia

        Private _Ref As String = String.Empty
        Private _IdEmpresa As String = 0
        Private _Lecciones As List(Of Leccion)

        Public Property Ref() As String
            Get
                Return _Ref
            End Get
            Set(ByVal value As String)
                _Ref = value
            End Set
        End Property

        Public Property IdEmpresa() As String
            Get
                Return _IdEmpresa
            End Get
            Set(ByVal value As String)
                _IdEmpresa = value
            End Set
        End Property

        Public Property Lecciones As List(Of Leccion)
            Get
                Return _Lecciones
            End Get
            Set(value As List(Of Leccion))
                _Lecciones = value
            End Set
        End Property

        Public Class Leccion

            Private _Lesson As LeccionesAprendidasLib.ELL.Leccion = Nothing
            Private _incluida As Boolean = False
            Private _comentario As String = String.Empty

            Public Property Lesson() As LeccionesAprendidasLib.ELL.Leccion
                Get
                    Return _Lesson
                End Get
                Set(ByVal value As LeccionesAprendidasLib.ELL.Leccion)
                    _Lesson = value
                End Set
            End Property

            Public Property Incluida() As Boolean
                Get
                    Return _incluida
                End Get
                Set(ByVal value As Boolean)
                    _incluida = value
                End Set
            End Property

            Public Property Comentario() As String
                Get
                    Return _comentario
                End Get
                Set(ByVal value As String)
                    _comentario = value
                End Set
            End Property

        End Class

    End Class

End Namespace
