Namespace ELL

    Public Class Email

#Region "Variables miembro"

        Private _Traducir As Boolean = False
        Private _idiomaUsuario As String = String.Empty
        Private _IdSolicitud As Integer = Integer.MinValue
        Private _Asunto As String = String.Empty
        Private _Descripcion As String = String.Empty
        Private _Subject As String = String.Empty
        Private _AltaUsuario As String = String.Empty
        Private _UsuariosEmail As New List(Of Integer)
        Private _Campos As New List(Of Campos)
        Private _Enlace As String = String.Empty

#End Region

#Region "Campos"

        Structure Campos
            Public Cabecera As String
            Public Descripcion As String
        End Structure

#End Region        

#Region "Properties"

        Public Property Traducir() As Boolean
            Get
                Return _Traducir
            End Get
            Set(ByVal value As Boolean)
                _Traducir = value
            End Set
        End Property

        Public Property IdiomaUsuario() As String
            Get
                Return _idiomaUsuario
            End Get
            Set(ByVal value As String)
                _idiomaUsuario = value
            End Set
        End Property

        Public Property IdSolicitud() As Integer
            Get
                Return _IdSolicitud
            End Get
            Set(ByVal value As Integer)
                _IdSolicitud = value
            End Set
        End Property

        Public Property Subject() As String
            Get
                Return _Subject
            End Get
            Set(ByVal value As String)
                _Subject = value
            End Set
        End Property

        Public Property Asunto() As String
            Get
                Return _Asunto
            End Get
            Set(ByVal value As String)
                _Asunto = value
            End Set
        End Property

        Public Property Descripcion() As String
            Get
                Return _Descripcion
            End Get
            Set(ByVal value As String)
                _Descripcion = value
            End Set
        End Property

        Public Property AltaUsuario() As String
            Get
                Return _AltaUsuario
            End Get
            Set(ByVal value As String)
                _AltaUsuario = value
            End Set
        End Property

        Public Property UsuariosEmail() As List(Of Integer)
            Get
                Return _UsuariosEmail
            End Get
            Set(ByVal value As List(Of Integer))
                _UsuariosEmail = value
            End Set
        End Property

        Public Property Enlace() As String
            Get
                Return _Enlace
            End Get
            Set(ByVal value As String)
                _Enlace = value
            End Set
        End Property

        Public Property IncidenciaCampos() As List(Of Campos)
            Get
                Return _Campos
            End Get
            Set(ByVal value As List(Of Campos))
                _Campos = value
            End Set
        End Property

#End Region

    End Class

End Namespace

