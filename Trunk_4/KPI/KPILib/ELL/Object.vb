Namespace ELL

    Public Class Negocio
        Property Id As Integer
        Property Nombre As String
    End Class

    Public Class Area
        Property Id As Integer
        Property Nombre As String
        Property IdNegocio As Integer
        Property Valores As List(Of Valor)
        Property Indicadores As List(Of Indicador)
    End Class

    Public Class Valor
        Property Id As Integer
        Property Nombre As String
        Property Descripcion As String        
        Property IdArea As Integer
        Property IdUnidad As Integer
        Property Historico As HistoricoValor
        Property NumOrden As Integer
        Property MetodoAcumulado As MetodoAcum = MetodoAcum.Total
        Property Obsoleto As Boolean

        ''' <summary>
        ''' Formas de calcular el acumulado
        ''' </summary>        
        Public Enum MetodoAcum As Integer
            Media = 0
            Total = 1
            Media_6_meses = 2
            Media_12_meses = 3
            Total_6_meses = 4
            Total_12_meses = 5
            Mensual = 6
            Manual = 7
        End Enum

    End Class

    Public Class Indicador
        Property Id As Integer
        Property Nombre As String
        Property Descripcion As String
        Property IdArea As Integer
        Property IdAreaResponsable As Integer
        Property IdUnidad As Integer
        Property Calculo As String        
        Property Historico As HistoricoIndicador
        Property TendenciaObjetivo As Tendencia = Tendencia.Ascendente
        Property NumOrden As Integer
        Property Obsoleto As Boolean
        Property Plantas As List(Of Integer)
        ''' <summary>
        ''' Tipos de tendencia de objetivos
        ''' </summary>        
        Public Enum Tendencia As Integer
            Descendente = 0
            Ascendente = 1
        End Enum

    End Class

    Public Class Unidad
        Property Id As Integer
        Property Nombre As String
        Property EsMoneda As Boolean
        Property TextoMostrar As String
    End Class

    Public Class HistoricoValor
        Property IdPlanta As Integer
        Property IdValor As Integer
        Property IdUsuario As Integer
        Property Anno As Integer
        Property Mes As Integer
        Property IdArea As Integer
        Property ValorPG As Decimal = Decimal.MinValue
        Property ValorReal As Decimal = Decimal.MinValue
        Property ValorRealSistema As Decimal = Decimal.MinValue
        Property AcumuladoReal As Decimal = Decimal.MinValue
        Property AcumuladoPG As Decimal = Decimal.MinValue
        Property FechaModificacion As DateTime = DateTime.MinValue

        ReadOnly Property Clone As HistoricoValor
            Get
                Dim oHist As New ELL.HistoricoValor With {.IdPlanta = IdPlanta, .IdValor = IdValor, .IdUsuario = IdUsuario, .Anno = Anno, .Mes = Mes, .IdArea = IdArea, .ValorPG = ValorPG, .ValorReal = ValorReal, .ValorRealSistema = ValorRealSistema, .AcumuladoPG = AcumuladoPG, .AcumuladoReal = AcumuladoReal, .FechaModificacion = FechaModificacion}
                Return oHist
            End Get
        End Property
    End Class

    Public Class HistoricoIndicador
        Property IdPlanta As Integer
        Property IdIndicador As Integer
        Property IdUsuario As Integer
        Property Anno As Integer
        Property Mes As Integer
        Property IdArea As Integer
        Property ValorPG As Decimal = Decimal.MinValue
        Property ValorReal As Decimal = Decimal.MinValue
        Property ValorRealSistema As Decimal = Decimal.MinValue
        Property AcumuladoReal As Decimal = Decimal.MinValue
        Property AcumuladoPG As Decimal = Decimal.MinValue
        Property FechaModificacion As DateTime = DateTime.MinValue

        ReadOnly Property Clone As HistoricoIndicador
            Get
                Dim oHist As New ELL.HistoricoIndicador With {.IdPlanta = IdPlanta, .IdIndicador = IdIndicador, .IdUsuario = IdUsuario, .Anno = Anno, .Mes = Mes, .IdArea = IdArea, .ValorPG = ValorPG, .ValorReal = ValorReal, .ValorRealSistema = ValorRealSistema, .AcumuladoPG = AcumuladoPG, .AcumuladoReal = AcumuladoReal, .FechaModificacion = FechaModificacion}
                Return oHist
            End Get
        End Property
    End Class

    Public Class Rol
        Property Id As Integer
        Property Nombre As String        
    End Class

    Public Class Planta
        Property Id As Integer
        Property Nombre As String
        Property IdMoneda As Integer
        Property NombreMoneda As String
        Property IdPlantaSAB As Integer
        Property Avisar As Boolean
    End Class

    Public Class PerfilArea
        Property IdPlanta As Integer
        Property IdNegocio As Integer
        Property IdArea As Integer
        Property IdUsuario As Integer
        Property NombrePlanta As String
        Property NombreNegocio As String
        Property NombreArea As String
        Property NombreUsuario As String
    End Class

    Public Class CierreIndicador
        Property IdPlanta As Integer
        Property IdUsuario As Integer
        Property Anno As Integer
        Property Mes As Integer
        Property Fecha As DateTime
    End Class


    Public Class Comite
        Property Id As Integer
        Property Nombre As String
        Property Obsoleto As Boolean
        Property Indicadores As List(Of ELL.Indicador)
    End Class

End Namespace
