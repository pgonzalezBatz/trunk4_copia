Namespace ELL

    Public Class Controles

#Region "Variables miembro"
        Dim usuariosBLL As New SabLib.BLL.UsuariosComponent

        Private _id As Integer = Integer.MinValue
        Private _codOperacion As String = String.Empty
        Private _idUsuario As Integer = Integer.MinValue
        Private _idPlanta As String = String.Empty
        Private _fecha As Date = Date.MinValue
        Private _infoPieza As String = String.Empty
        Private _turno As String = String.Empty
        Private _nivelPlan As String = String.Empty
        Private _idTipo As ELL.Usuarios.RolesUsuario
        Private _UsrSab As SabLib.ELL.Usuario = Nothing

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id del control
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
                If _idUsuario <> Integer.MinValue Then
                    _UsrSab = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = Me._idUsuario}, SoloVigentes:=False)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property Usuario() As String
            Get
                Try
                    'Return usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = Me._idUsuario}, SoloVigentes:=False).NombreCompleto
                    Return If(_UsrSab Is Nothing, String.Empty, _UsrSab.NombreCompleto)
                Catch ex As Exception
                    Return String.Empty
                End Try
            End Get
        End Property

        ''' <summary>
        ''' Codigo de trabajador del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property CodUsuario() As Integer
            Get
                'Return usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = Me._idUsuario}).CodPersona
                Return If(_UsrSab Is Nothing, Nothing, _UsrSab.CodPersona)
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
        ''' Turno del trabajador
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdTipo() As ELL.Usuarios.RolesUsuario
            Get
                Select Case _idTipo
                    Case ELL.Usuarios.RolesUsuario.Calidad
                        Return ELL.Usuarios.RolesUsuario.Calidad
                    Case ELL.Usuarios.RolesUsuario.Operario
                        Return ELL.Usuarios.RolesUsuario.Operario
                    Case ELL.Usuarios.RolesUsuario.Gestor
                        Return ELL.Usuarios.RolesUsuario.Gestor
                    Case Else
                        Return ELL.Usuarios.RolesUsuario.Administrador
                End Select
            End Get
            Set(ByVal value As ELL.Usuarios.RolesUsuario)
                _idTipo = value
            End Set
        End Property

        ''' <summary>
        ''' Fecha del control
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
        ''' Turno en el que se ha hecho el control
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
        ''' Turno del trabajador
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property TurnoTrabajador() As String
            Get
                Select Case Me._turno
                    Case "M" : Return "Mañana"
                    Case "T" : Return "Tarde"
                    Case "N" : Return "Noche"
                    Case Else : Return "-"
                End Select                
            End Get
        End Property

        ''' <summary>
        ''' Nivel del plan de la operación
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NivelPlan() As String
            Get
                Return _nivelPlan
            End Get
            Set(ByVal value As String)
                _nivelPlan = value
            End Set
        End Property
#End Region

    End Class

End Namespace

