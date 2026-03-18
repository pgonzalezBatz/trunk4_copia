Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class Permiso

#Region "Properties"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdSab() As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaSolicitud() As DateTime

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdSabResponsable() As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaRespuesta() As DateTime

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Autorizado() As Boolean

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Nombre() As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Apellido1() As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Apellido2() As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Email() As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property NombreCompleto() As String
            Get
                Return String.Format("{0} {1} {2}", Nombre, Apellido1, Apellido2).ToUpper()
            End Get
        End Property

#End Region

    End Class

End Namespace

