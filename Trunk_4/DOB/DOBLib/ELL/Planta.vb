Namespace ELL

    Public Class Planta

#Region "Constantes"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Const PLANTA_BATZ_GROUP As Integer = 67

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' Planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As String

        ''' <summary>
        ''' Fecha baja
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaBaja() As DateTime

        ''' <summary>
        ''' Id usuario baja
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdUsuarioBaja() As Integer

        ''' <summary>
        ''' Id planta padre
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlantaPadre() As Integer

        ''' <summary>
        ''' Hereda los retos del padre
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property HeredaRetos() As Boolean

        ''' <summary>
        ''' Planta padre
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PlantaPadre() As String

#End Region

    End Class

End Namespace
