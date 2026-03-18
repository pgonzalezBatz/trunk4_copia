Namespace ELL
    <Serializable()> _
    Public Class gtkIrudiak
        ''' <summary>
        ''' Identificador de la Incidencia.
        ''' </summary>
        Private _idIncidencia As Integer = Integer.MinValue
        ''' <summary>
        ''' Identificador de la imagen.
        ''' </summary>
        Private _idImagen As Integer = Integer.MinValue
        ''' <summary>
        ''' Bytes de la imagen.
        ''' </summary>
        Private _imagen As Byte()
        ''' <summary>
        ''' Descripcion de la imagen.
        ''' </summary>
        Private _descripcion As String
        ''' <summary>
        ''' Accion a realizar en la base de datos.
        ''' </summary>
        Private _accion As Acciones = Acciones.Consultar
        ''' <summary>
        ''' Ruta de acceso de la imagen en el servidor.
        ''' </summary>
        Private _pathServer As String
        ''' <summary>
        ''' Extensión del archivo.
        ''' </summary>
        Private _extension As String

        ''' <summary>
        ''' Identificador de la imagen.
        ''' </summary>
        Public Property IdImagen() As Integer
            Get
                Return _idImagen
            End Get
            Set(ByVal value As Integer)
                _idImagen = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador de la Incidencia.
        ''' </summary>
        Public Property IdIncidencia() As Integer
            Get
                Return _idIncidencia
            End Get
            Set(ByVal value As Integer)
                _idIncidencia = value
            End Set
        End Property

        ''' <summary>
        ''' Bytes de la imagen
        ''' </summary>
        ''' <value>a</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Imagen() As Byte()
            Get
                Return _imagen
            End Get
            Set(ByVal value As Byte())
                _imagen = value
            End Set
        End Property

        ''' <summary>
        ''' Descripcion de la imagen.
        ''' </summary>
        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        ''' <summary>
        ''' Accion a realizar en la base de datos.
        ''' </summary>
        Public Property Accion() As Acciones
            Get
                Return _accion
            End Get
            Set(ByVal value As Acciones)
                _accion = value
            End Set
        End Property

        ''' <summary>
        ''' Ruta de acceso de la imagen en el servidor.
        ''' </summary>
        Public Property PathServer() As String
            Get
                Return _pathServer
            End Get
            Set(ByVal value As String)
                _pathServer = value
            End Set
        End Property

        ''' <summary>
        ''' Extensión del archivo.
        ''' </summary>
        Public Property Extension() As String
            Get
                Return _extension
            End Get
            Set(ByVal value As String)
                _extension = value
            End Set
        End Property
    End Class
    

End Namespace
