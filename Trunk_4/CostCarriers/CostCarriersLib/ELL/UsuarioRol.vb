Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class UsuarioRol

#Region "Properties"

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
        ''' Planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlanta() As Integer

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
        ''' Email
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Email() As String

        ''' <summary>
        ''' Rol
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property DescripcionRol() As String

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
        ''' Productos de los cuales es produc manager
        ''' </summary>
        ''' <returns></returns>
        Public Property ListaProductosProductManager As List(Of String)

#End Region

    End Class

End Namespace

