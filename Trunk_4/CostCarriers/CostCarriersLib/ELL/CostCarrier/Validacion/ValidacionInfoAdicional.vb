Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ValidacionInfoAdicional

#Region "Enumerados"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum TipoDato
            Awarding_offer = 1
            Current_values = 2
        End Enum

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' Id validación
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdValidacion() As Integer

        ''' <summary>
        ''' Id planta SAB
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlanta() As Integer

        ''' <summary>
        ''' Id cabecera
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdCabecera() As Integer = Integer.MinValue

        ''' <summary>
        ''' Net margin
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property NetMargin() As Decimal = Decimal.MinValue

        ''' <summary>
        ''' Effective sales
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property EffectiveSales() As Integer = Integer.MinValue

        ''' <summary>
        ''' Customer property tooling benefit
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CustomerProperty() As Decimal = Decimal.MinValue

        ''' <summary>
        ''' Customer plants
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CustomerPlants() As String = String.Empty

        ''' <summary>
        ''' SOP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property SOP() As DateTime = DateTime.MinValue

        ''' <summary>
        ''' Average volumen
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property AverageVolumen() As Integer = Integer.MinValue

        ''' <summary>
        ''' Tipo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Tipo() As Integer = Integer.MinValue

        ''' <summary>
        ''' Fecha alta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaAlta() As DateTime

        ''' <summary>
        ''' Años serie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property SeriesYears() As Integer = Integer.MinValue

#End Region

    End Class

End Namespace

