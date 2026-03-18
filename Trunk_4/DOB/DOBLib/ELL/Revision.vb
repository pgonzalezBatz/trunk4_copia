Namespace ELL

    Public Class Revision

#Region "Enumerados"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum Tipo
            Marzo = 1
            Junio = 2
            Octubre = 3
            Enero = 4
        End Enum

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id objetivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdObjetivo() As Integer

        ''' <summary>
        ''' Revision
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Revision() As Integer

        ''' <summary>
        ''' Comentario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Comentario() As String

        ''' <summary>
        ''' Plan de acción para le año siguiente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PAAñoSiguiente() As Boolean

        ''' <summary>
        ''' Comentario para le plan del acción del año siguient
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PAComentario() As String

        ''' <summary>
        ''' Fecha de alta
        ''' </summary>
        ''' <returns></returns>
        Public Property FechaAlta As DateTime

        ''' <summary>
        ''' Descripción del objetivo
        ''' </summary>
        ''' <returns></returns>
        Public Property DescripcionObjetivo As String

        ''' <summary>
        ''' Id responsable
        ''' </summary>
        ''' <returns></returns>
        Public Property IdResponsable As String

        ''' <summary>
        ''' Año objetivo
        ''' </summary>
        ''' <returns></returns>
        Public Property AñoObjetivo As Integer

        ''' <summary>
        ''' Descripción revision
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property DescripcionRevison() As String
            Get
                Return System.Enum.GetName(GetType(Tipo), Revision)
            End Get
        End Property

#End Region

    End Class

End Namespace
