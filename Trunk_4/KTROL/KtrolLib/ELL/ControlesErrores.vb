Namespace ELL

    Public Class ControlesErrores

#Region "Variables miembro"

        Private _idControl As Integer = Integer.MinValue
        Private _validado As Boolean = False
        Private _reparado As Boolean = False
        Private _cambioReferencia As Boolean = False
        Private _validacionUsuario As Integer = Integer.MinValue
        Private _nombreValidacionUsuario As String = String.Empty
        Private _comentario As String = String.Empty
        Private _idControlValidacion As Integer = Integer.MinValue

#End Region

#Region "Enumeracion"

        Public Enum TiposErrores
            Validacion = 1
            Reparacion = 2
            Mantenimiento = 3
        End Enum

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id del control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdControl() As Integer
            Get
                Return _idControl
            End Get
            Set(ByVal value As Integer)
                _idControl = value
            End Set
        End Property

        ''' <summary>
        ''' El control ha sido validado
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Validado() As Boolean
            Get
                Return _validado
            End Get
            Set(ByVal value As Boolean)
                _validado = value
            End Set
        End Property

        ''' <summary>
        ''' El control ha sido validado
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Reparado() As Boolean
            Get
                Return _reparado
            End Get
            Set(ByVal value As Boolean)
                _reparado = value
            End Set
        End Property

        ''' <summary>
        ''' El control ha sido cambiado de referencia
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
        ''' El usuario ha validado el control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ValidacionUsuario() As Integer
            Get
                Return _validacionUsuario
            End Get
            Set(ByVal value As Integer)
                _validacionUsuario = value
            End Set
        End Property

        ''' <summary>
        ''' El nombre del usuario que ha validado el control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NombreValidacionUsuario() As String
            Get
                Try
                    Dim oUsuarios As New SabLib.BLL.UsuariosComponent
                    If (Me._validacionUsuario <> Integer.MinValue) Then
                        Return oUsuarios.GetUsuario(New SabLib.ELL.Usuario With {.CodPersona = Me._validacionUsuario}).NombreCompleto
                    Else
                        Return String.Empty
                    End If

                Catch ex As Exception
                    Return String.Empty
                End Try

                'Return _nombreValidacionUsuario
            End Get
            Set(ByVal value As String)
                _nombreValidacionUsuario = value
            End Set
        End Property

        ''' <summary>
        ''' Comentario del usuario
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
        ''' Id del control que valida el control con parte a mantenimiento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdControlValidacion() As Integer
            Get
                Return _idControlValidacion
            End Get
            Set(ByVal value As Integer)
                _idControlValidacion = value
            End Set
        End Property

#End Region

    End Class

End Namespace

