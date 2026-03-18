Namespace ELL

    Public Class PlantaNegocio

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' Id
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
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdNegocio() As Integer

        ''' <summary>
        ''' Negocio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Negocio() As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property PlantaNegocio As String
            Get
                Return String.Format("{0} ({1})", Planta, Negocio)
            End Get
        End Property

#End Region

    End Class

End Namespace
