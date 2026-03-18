Namespace ELL

    Public Class Accion

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' Id objetivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdObjetivo() As Integer

        ''' <summary>
        ''' Descripción
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Descripcion() As String

        ''' <summary>
        ''' Fecha objetivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaObjetivo As DateTime

        ''' <summary>
        ''' Porcentaje
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>       
        Public Property Porcentaje() As Decimal

        ''' <summary>
        ''' Grado importancia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>    
        Public Property GradoImportancia() As Decimal

        ''' <summary>
        ''' Fecha alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaAlta() As DateTime

        ''' <summary>
        ''' Fecha baja
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaBaja() As DateTime

        ''' <summary>
        ''' Id usuario alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdUsuarioAlta() As Integer

        ''' <summary>
        ''' Id usuario baja
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdUsuarioBaja() As Integer

        ''' <summary>
        ''' Descripción objetivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property DescripcionObjetivo() As String

        ''' <summary>
        ''' Id responsable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdResponsable() As Integer

        ''' <summary>
        ''' Periodicidad
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Periodicidad() As Integer

        ''' <summary>
        ''' Tiene documentos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TieneDocumentos() As Boolean

#End Region

    End Class

End Namespace
