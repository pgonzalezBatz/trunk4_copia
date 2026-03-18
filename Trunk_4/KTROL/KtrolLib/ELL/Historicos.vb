Namespace ELL

    Public Class Historicos

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _codOperacion As Integer = Integer.MinValue
        Private _idUsuario As Integer = Integer.MinValue
        Private _fecha As DateTime = DateTime.MinValue
        Private _fechaCorta As String = String.Empty
        Private _hora As String = String.Empty
        Private _usuario As String = String.Empty
        Private _idPlanta As Integer = Integer.MinValue
        Private _planta As String = String.Empty
        Private _turno As String = String.Empty
        Private _infoPieza As String = String.Empty
        Private _validacion As Boolean = False
        Private _validacionUsuario As String = String.Empty
        Private _comentario As String = String.Empty
        Private _reparacion As Boolean = False
        Private _cambioReferencia As Boolean = False
        Private _idRegistro As String = String.Empty
        Private _posicion As String = String.Empty
        Private _okNok As String = String.Empty
        Private _valor As String = String.Empty

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
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
        ''' Codigo de la operación
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CodOperacion() As Integer
            Get
                Return _codOperacion
            End Get
            Set(ByVal value As Integer)
                _codOperacion = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador del usuario
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
        ''' Codigo de trabajador del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property CodUsuario() As Integer
            Get
                Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
                Return usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = Me._idUsuario}).CodPersona
            End Get
        End Property

        ''' <summary>
        ''' Fecha del control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Fecha() As DateTime
            Get
                Return _fecha
            End Get
            Set(ByVal value As DateTime)
                _fecha = value
            End Set
        End Property

        ''' <summary>
        ''' Fecha corta del control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaCorta() As String
            Get
                Return _fechaCorta
            End Get
            Set(ByVal value As String)
                _fechaCorta = value
            End Set
        End Property

        ''' <summary>
        ''' Hora del control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Hora() As String
            Get
                Return _hora
            End Get
            Set(ByVal value As String)
                _hora = value
            End Set
        End Property

        ''' <summary>
        ''' Usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property Usuario() As String
            Get
                Dim usuariosBLL As New Sablib.BLL.UsuariosComponent
                Return usuariosBLL.GetUsuario(New Sablib.ELL.Usuario With {.Id = Me._idUsuario}).NombreCompleto
            End Get
        End Property

        ''' <summary>
        ''' Identificador de la planta del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlanta() As Integer
            Get
                Return _idPlanta
            End Get
            Set(ByVal value As Integer)
                _idPlanta = value
            End Set
        End Property

        ''' <summary>
        ''' Nombre de la planta del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property Planta() As String
            Get
                Dim oPlanta As New Sablib.BLL.PlantasComponent
                Return oPlanta.GetPlanta(_idPlanta).Nombre
            End Get
        End Property

        ''' <summary>
        ''' Turno de trabajo del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Turno() As String
            Get
                Return _turno
            End Get
            Set(ByVal value As String)
                _turno = value
            End Set
        End Property

        ''' <summary>
        ''' Información de la pieza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property InfoPieza() As String
            Get
                Return _infoPieza
            End Get
            Set(ByVal value As String)
                _infoPieza = value
            End Set
        End Property

        ''' <summary>
        ''' El usuario es de calidad
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Validacion() As Boolean
            Get
                Return _validacion
            End Get
            Set(ByVal value As Boolean)
                _validacion = value
            End Set
        End Property

        ''' <summary>
        ''' El usuario es de calidad
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Reparacion() As Boolean
            Get
                Return _reparacion
            End Get
            Set(ByVal value As Boolean)
                _reparacion = value
            End Set
        End Property

        ''' <summary>
        ''' El usuario es de calidad
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CambioReferencia() As Boolean
            Get
                Return _cambioReferencia
            End Get
            Set(ByVal value As Boolean)
                _cambioReferencia = value
            End Set
        End Property

        ''' <summary>
        ''' Información de la pieza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ValidacionUsuario() As String
            Get
                Return _validacionUsuario
            End Get
            Set(ByVal value As String)
                _validacionUsuario = value
            End Set
        End Property

        ''' <summary>
        ''' Información de la pieza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Comentario() As String
            Get
                Return _comentario
            End Get
            Set(ByVal value As String)
                _comentario = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador de la caracteristica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdRegistro() As String
            Get
                Return _idRegistro
            End Get
            Set(ByVal value As String)
                _idRegistro = value
            End Set
        End Property

        ''' <summary>
        ''' Posicion de la caracteristica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Posicion() As String
            Get
                Return _posicion
            End Get
            Set(ByVal value As String)
                _posicion = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si la característica es OK o NOK
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property OkNok() As String
            Get
                Return _okNok
            End Get
            Set(ByVal value As String)
                _okNok = value
            End Set
        End Property

        ''' <summary>
        ''' Valor de la característica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Valor() As String
            Get
                Return _valor
            End Get
            Set(ByVal value As String)
                _valor = value
            End Set
        End Property

#End Region

    End Class

End Namespace

