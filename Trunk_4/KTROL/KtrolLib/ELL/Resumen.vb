Namespace ELL

    Public Class Resumen

#Region "Variables miembro"

        Private _codOperacion As String = String.Empty
        Private _idUsuario As Integer = Integer.MinValue
        Private _codUsuario As Integer = Integer.MinValue
        Private _usuario As String = String.Empty
        Private _idPlanta As Integer = Integer.MinValue
        Private _planta As String = String.Empty
        Private _turno As String = String.Empty
        Private _tipoTrabajador As String = String.Empty

#End Region

#Region "Properties"

        ''' <summary>
        ''' Codigo de la operación
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CodOperacion() As String
            Get
                Return _codOperacion
            End Get
            Set(ByVal value As String)
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
        ''' Tipo de trabajador
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TipoTrabajador() As String
            Get
                Return _tipoTrabajador
            End Get
            Set(ByVal value As String)
                _tipoTrabajador = value
            End Set
        End Property

#End Region

    End Class

End Namespace

