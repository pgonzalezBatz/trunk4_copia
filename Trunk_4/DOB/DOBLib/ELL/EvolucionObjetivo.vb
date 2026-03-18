Namespace ELL

    Public Class EvolucionObjetivo

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
        ''' Valor actual
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property ValorActual() As String = String.Empty

        ''' <summary>
        ''' Id periodicidad
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPeriodicidad() As Integer

        ''' <summary>
        ''' Id usuario alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdUsuarioAlta() As Integer

        ''' <summary>
        ''' Valor inicial
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ValorInicial() As Decimal

        ''' <summary>
        ''' Valor objetivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ValorObjetivo() As Decimal

        ''' <summary>
        ''' Tipo indicador
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TipoIndicador() As String

        ''' <summary>
        ''' Planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As String

#End Region

    End Class

End Namespace
