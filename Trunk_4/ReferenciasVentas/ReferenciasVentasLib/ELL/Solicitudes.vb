Namespace ELL

    <Serializable()>
    Public Class Solicitudes

#Region "Enumeraciones"

        Public Enum TiposSolicitudes As Integer
            ReferenciaFinalVenta = 1
            ReferenciaComponente = 2
            CentroCoste = 3
            PortadorCoste = 4
            PlanTrabajo = 5
        End Enum

        Public Enum ValidacionSolicitudes As Integer
            Pendiente = 1
            Aprobado = 2
            Rechazado = 3
            SinNecesidad = 4
        End Enum

#End Region

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _idSolicitante As Integer = Integer.MinValue
        Private _nombreSolicitante As String = String.Empty
        Private _fechaAlta As DateTime = DateTime.MinValue
        Private _idTipoSolicitud As String = String.Empty
        Private _fechaGestion As DateTime = DateTime.MinValue
        Private _usuarioTramitador As Integer = Integer.MinValue
        Private _aprobado As Boolean = False
        Private _comentarioDT As String = String.Empty
        Private _CustomerPartNumber As String = String.Empty
        Private _validado As Integer = Integer.MinValue
        Private _usuarioValidador As Integer = Integer.MinValue
        Private _usuarioValidadorFinal As Integer = Integer.MinValue

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
        ''' IdSolicitante
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdSolicitante() As Integer
            Get
                Return _idSolicitante
            End Get
            Set(ByVal value As Integer)
                _idSolicitante = value
            End Set
        End Property

        ''' <summary>
        ''' Nombre del solicitante
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NombreSolicitante() As String
            Get
                Try
                    Dim gtkUsuario As New SabLib.ELL.Usuario
                    Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent

                    gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = _idSolicitante}, False)
                    Return gtkUsuario.NombreCompleto
                Catch ex As Exception
                    Return "Error al cargar"
                End Try
            End Get
            Set(ByVal value As String)
                _nombreSolicitante = value
            End Set
        End Property

        ''' <summary>
        ''' Fecha de alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaAlta() As DateTime
            Get
                Return _fechaAlta
            End Get
            Set(ByVal value As DateTime)
                _fechaAlta = value
            End Set
        End Property

        ''' <summary>
        ''' IdTipoSolicitud
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdTipoSolicitud() As Integer
            Get
                Return _idTipoSolicitud
            End Get
            Set(ByVal value As Integer)
                _idTipoSolicitud = value
            End Set
        End Property

        ''' <summary>
        ''' Fecha de tramitación
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaGestion() As DateTime
            Get
                Return _fechaGestion
            End Get
            Set(ByVal value As DateTime)
                _fechaGestion = value
            End Set
        End Property

        ''' <summary>
        ''' UsuarioTramitador
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property UsuarioTramitador() As Integer
            Get
                Return _usuarioTramitador
            End Get
            Set(ByVal value As Integer)
                _usuarioTramitador = value
            End Set
        End Property

        ''' <summary>
        ''' UsuarioTramitador
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NombreUsuarioTramitador() As String
            Get
                Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
                If (Me._usuarioTramitador <> Integer.MinValue) Then
                    Return UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Me._usuarioTramitador}, False).NombreCompleto
                Else
                    Return String.Empty
                End If

            End Get
            Set(ByVal value As String)
                _usuarioTramitador = value
            End Set
        End Property

        ''' <summary>
        ''' Aprobado por Documentation Technician
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Aprobado() As Boolean
            Get
                Return _aprobado
            End Get
            Set(ByVal value As Boolean)
                _aprobado = value
            End Set
        End Property

        ''' <summary>
        ''' Comentario de DT
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ComentarioDT() As String
            Get
                Return _comentarioDT
            End Get
            Set(ByVal value As String)
                _comentarioDT = value
            End Set
        End Property

        ''' <summary>
        ''' CustomerPartNumber
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CustomerPartNumber() As String
            Get
                Return _CustomerPartNumber
            End Get
            Set(ByVal value As String)
                _CustomerPartNumber = value
            End Set
        End Property

        ''' <summary>
        ''' Estado de la validación de la solicitud
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Validado() As Boolean
            Get
                Return _aprobado
            End Get
            Set(ByVal value As Boolean)
                _aprobado = value
            End Set
        End Property

        ''' <summary>
        ''' Usuario validador de la solicitud
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property UsuarioValidador() As Integer
            Get
                Return _usuarioValidador
            End Get
            Set(ByVal value As Integer)
                _usuarioValidador = value
            End Set
        End Property

        ''' <summary>
        ''' UsuarioTramitador
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NombreUsuarioValidador() As String
            Get
                Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
                Return UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Me._usuarioValidador}, False).NombreCompleto
            End Get
            Set(ByVal value As String)
                _usuarioValidador = value
            End Set
        End Property

        ''' <summary>
        ''' Usuario validador final de la solicitud
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property UsuarioValidadorFinal() As Integer
            Get
                Return _usuarioValidadorFinal
            End Get
            Set(ByVal value As Integer)
                _usuarioValidadorFinal = value
            End Set
        End Property

        ''' <summary>
        ''' UsuarioTramitador
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NombreUsuarioValidadorFinal() As String
            Get
                Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
                If (Me._usuarioValidadorFinal <> Integer.MinValue) Then
                    Return UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Me._usuarioValidadorFinal}, False).NombreCompleto
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                _usuarioValidadorFinal = value
            End Set
        End Property

#End Region

    End Class

End Namespace

