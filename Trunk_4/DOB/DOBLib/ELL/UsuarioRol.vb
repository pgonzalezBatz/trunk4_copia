Namespace ELL

    Public Class UsuarioRol

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' Id del rol
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdRol() As Integer

        ''' <summary>
        ''' Id de usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdSab() As Integer

        ''' <summary>
        ''' Nombre
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Nombre() As String

        ''' <summary>
        ''' Apellido 1
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Apellido1() As String

        ''' <summary>
        ''' Apellido 2
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Apellido2() As String

        ''' <summary>
        ''' Rol
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property DescripcionRol() As String

        ''' <summary>
        ''' Id planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlanta() As Integer

        ''' <summary>
        ''' Planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As String

        ''' <summary>
        ''' Planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PlantaActiva() As String

        ''' <summary>
        ''' Usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property NombreUsuario() As String
            Get
                Return String.Format("{0} {1} {2}", Nombre, Apellido1, Apellido2).ToUpper()
            End Get
        End Property

        ''' <summary>
        ''' Descripción completa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property DescripcionCompleta() As String
            Get
                Return String.Format("{0} ({1})", Planta, DescripcionRol)
            End Get
        End Property

        ''' <summary>
        ''' Fecha baja
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaBaja() As DateTime

        ''' <summary>
        ''' Fecha baja
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaBajaDOB() As DateTime

        ''' <summary>
        ''' Fecha baja planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaBajaPlanta() As DateTime

        ''' <summary>
        ''' 
        ''' </summary>
        Public ReadOnly Property EsBaja As Boolean
            Get
                Return (FechaBaja <> DateTime.MinValue AndAlso FechaBaja < DateTime.Today) OrElse FechaBajaDOB <> DateTime.MinValue
            End Get
        End Property

#End Region

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Function ShallowCopy() As UsuarioRol
            Return CType(Me.MemberwiseClone(), UsuarioRol)
        End Function

#End Region

    End Class

End Namespace

