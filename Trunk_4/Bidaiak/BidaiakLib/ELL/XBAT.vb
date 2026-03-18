Namespace ELL

    Public Class DocumentoProyecto

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _idProyecto As Integer = Integer.MinValue
        Private _descripcion As String = String.Empty
        Private _adjunto As Byte() = Nothing
        Private _contentType As String = String.Empty
        Private _idUsuario As Integer = Integer.MinValue
        Private _fecha As Date = Date.MinValue
        Private _nombreProy As String = String.Empty

#End Region

#Region "Properties"

        ''' <summary>
        ''' Identificador del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        ''' <summary>
        ''' Id del proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property IdProyecto() As Integer
            Get
                Return _idProyecto
            End Get
            Set(ByVal value As Integer)
                _idProyecto = value
            End Set
        End Property

        ''' <summary>
        ''' Descripcion del contenido del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        ''' <summary>
        ''' Content type del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property ContentType() As String
            Get
                Return _contentType
            End Get
            Set(ByVal value As String)
                _contentType = value
            End Set
        End Property

        ''' <summary>
        ''' Documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Adjunto() As Byte()
            Get
                Return _adjunto
            End Get
            Set(ByVal value As Byte())
                _adjunto = value
            End Set
        End Property

        ''' <summary>
        ''' Fecha de subida del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Fecha() As Date
            Get
                Return _fecha
            End Get
            Set(ByVal value As Date)
                _fecha = value
            End Set
        End Property

        ''' <summary>
        ''' Id del usuario que a subido el documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property

        ''' <summary>
        ''' Nombre del proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property NombreProyecto() As String
            Get
                Return _nombreProy
            End Get
            Set(ByVal value As String)
                _nombreProy = value
            End Set
        End Property

#End Region

    End Class

    Public Class Pais

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty

#End Region

#Region "Properties"

        ''' <summary>
        ''' Identificador del país
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        ''' <summary>
        ''' Nombre del país
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

#End Region

    End Class

End Namespace