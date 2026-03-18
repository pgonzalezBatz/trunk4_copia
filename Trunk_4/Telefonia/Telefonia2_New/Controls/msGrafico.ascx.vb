Imports System.Web.UI.DataVisualization.Charting

Partial Public Class msGrafico
    Inherits System.Web.UI.UserControl

#Region "Enumeraciones"

    Enum EtiquetasLocation As Integer
        Disabled = 0
        Inside = 1
        Outside = 2
    End Enum

    Enum ShadowsOffset As Integer
        Cero = 0
        Uno = 1
        Dos = 2
        tres = 3
        Cuatro = 4
    End Enum

    Enum eGraficStyle2D As Integer
        _Default = 0
        _SoftEdge = 1
        _Concave = 2
    End Enum

    Enum SerieStyle As Integer
        _Default = 0
        _Emboss = 1
        _Cylinder = 2
        _Wedge = 3
        _LightToDark = 4
    End Enum

    Enum RadarDrawingStyle As Integer
        Area = 0
        Line = 1
        Marker = 2
    End Enum
    Enum AreaDrawingStyle As Integer
        Circle = 0
        Polygon = 1
    End Enum
    Enum CircularLabelsStyle As Integer
        Auto = 0
        Horizontal = 1
        Circular = 2
        Radial = 3
    End Enum

#End Region

#Region "Variables"

    'Grafico
    Private _listaSeries As List(Of cSerie) = Nothing
    Private _labels() As String
    Private _values As List(Of Double()) = Nothing
    Private _tipoGrafico As SeriesChartType = SeriesChartType.Pie
    Private _PaletaColoresPredefinida As Boolean = True
    Private _3D As Boolean = True
    Private _etiqLoc As EtiquetasLocation = EtiquetasLocation.Outside
    Private _backColor As Drawing.Color = Drawing.Color.WhiteSmoke
    Private _secondaryBackColor As Drawing.Color = Drawing.Color.WhiteSmoke
    Private _gradientStyle As GradientStyle = GradientStyle.None
    Private _borderColor As Drawing.Color = Drawing.Color.Gainsboro
    Private _borderWidth As Integer = 0
    Private _borderStyle As BorderStyle = WebControls.BorderStyle.Solid
    Private _anchura As Integer = 600
    Private _altura As Integer = 350
    Private _graficStyle2D As eGraficStyle2D = eGraficStyle2D._Default
    Private _showTitle As Boolean = True
    Private _showLegend As Boolean = False
    Private _contenidoStyle As BorderSkinStyle = BorderSkinStyle.Emboss

    'Titulo
    Private _titleText As String = String.Empty
    Private _titleTextOrientation As TextOrientation = TextOrientation.Auto
    Private _titleAlignment As Drawing.ContentAlignment = Drawing.ContentAlignment.TopCenter
    Private _titleBackColor As Drawing.Color = Drawing.Color.Transparent
    Private _titleBackImage As String = String.Empty
    Private _titleBorderWidth As Integer = 0
    Private _titleBorderColor As Drawing.Color = Drawing.Color.Gainsboro
    Private _titleBorderStyle As ChartDashStyle = ChartDashStyle.Solid
    Private _titleFontSize As Integer = 15
    Private _titleFontFamily As String = "Arial"
    Private _titleFontStyle As Drawing.FontStyle = Drawing.FontStyle.Bold
    Private _titleColor As Drawing.Color = Drawing.Color.Black
    Private _titleUrl As String = String.Empty
    Private _titlePostBackValue As String = String.Empty

    'Legenda    
    Private _legendAlignment As Drawing.StringAlignment = Drawing.StringAlignment.Center
    Private _legendPosition As Docking = Docking.Right
    Private _legendBackColor As Drawing.Color = Drawing.Color.Gainsboro
    Private _legendSecondaryBackColor As Drawing.Color = Drawing.Color.WhiteSmoke
    Private _legendGradientStyle As GradientStyle = GradientStyle.TopBottom
    Private _legendBorderColor As Drawing.Color = Drawing.Color.Gainsboro
    Private _legendBorderWidth As Integer = 1
    Private _legendBorderStyle As ChartDashStyle = ChartDashStyle.Solid
    Private _legendForeColor As Drawing.Color = Drawing.Color.Black
    Private _legendFontSize As Integer = 9
    Private _legendFontFamily As String = "Arial"
    Private _legendFontStyle As Drawing.FontStyle = Drawing.FontStyle.Regular
    Private _legendTitle As String = String.Empty
    Private _legendTitleAlignment As Drawing.StringAlignment = Drawing.StringAlignment.Center
    Private _legendTitleColor As Drawing.Color = Drawing.Color.Black
    Private _legendTitleSeparator As LegendSeparatorStyle = LegendSeparatorStyle.None
    Private _legendTitleSeparatorColor As Drawing.Color = Drawing.Color.Gainsboro
    Private _textAutoFit As Boolean = True
    Private _legendStyle As LegendStyle = DataVisualization.Charting.LegendStyle.Table
    Private _legendTableStyle As LegendTableStyle = DataVisualization.Charting.LegendTableStyle.Auto
    Private _legendItemSeparator As LegendSeparatorStyle = LegendSeparatorStyle.None
    Private _legendItemSeparatorColor As Drawing.Color = Drawing.Color.Gainsboro
    Private _legendTextCharacterWrap As Integer = 15
    Private _legendPintarFilas As Boolean = False
    Private _legendPintarFilasColor As Drawing.Color = Drawing.Color.WhiteSmoke

    'Chart
    Private _chartBackColor As Drawing.Color = Drawing.Color.White  'Drawing.Color.FromArgb(64, 165, 191, 228)
    Private _chartSecondaryBackColor As Drawing.Color = Drawing.Color.Transparent
    Private _chartGradientStyle As GradientStyle = DataVisualization.Charting.GradientStyle.None
    Private _chartBorderColor As Drawing.Color = Drawing.Color.Gainsboro
    Private _chartBorderWidth As Integer = 2
    Private _chartBorderStyle As ChartDashStyle = ChartDashStyle.Solid
    Private _chartLigthStyle As LightStyle = LightStyle.Realistic
    Private _chartInclination As Integer = 30
    Private _chartRotation As Integer = 15

    'Ejes del chart
    Private _chartEjeXMarginVisible As Boolean = False
    Private _chartEjeYMarginVisible As Boolean = True
    Private _chartEjeXMaximumValue As Double = 0
    Private _chartEjeXMinimumValue As Double = 0
    Private _chartEjeYMaximumValue As Double = 0
    Private _chartEjeYMinimumValue As Double = 0
    Private _chartEjeXTitle As String = String.Empty
    Private _chartEjeYTitle As String = String.Empty
    Private _chartEjeXInterval As Integer = 1
    Private _chartEjeYInterval As Integer = 0
    Private _chartEjeXTitleAlignment As Drawing.StringAlignment = Drawing.StringAlignment.Center
    Private _chartEjeYTitleAlignment As Drawing.StringAlignment = Drawing.StringAlignment.Center
    Private _chartEjeXTitleColor As Drawing.Color = Drawing.Color.Black
    Private _chartEjeYTitleColor As Drawing.Color = Drawing.Color.Black
    Private _chartEjeXTitleSize As Integer = 9
    Private _chartEjeXTitleFamily As String = "Arial"
    Private _chartEjeXTitleStyle As Drawing.FontStyle = Drawing.FontStyle.Bold
    Private _chartEjeYTitleSize As Integer = 9
    Private _chartEjeYTitleFamily As String = "Arial"
    Private _chartEjeYTitleStyle As Drawing.FontStyle = Drawing.FontStyle.Bold
    Private _chartEjeXTitleOrientation As TextOrientation = TextOrientation.Horizontal
    Private _chartEjeYTitleOrientation As TextOrientation = TextOrientation.Rotated270




#Region "Propiedades"
    Dim colorsetPie As Drawing.Color() = {Drawing.Color.FromArgb(255, 72, 61, 139), Drawing.Color.FromArgb(255, 205, 133, 63), Drawing.Color.FromArgb(255, 139, 69, 19), _
                                              Drawing.Color.FromArgb(255, 100, 149, 237), Drawing.Color.FromArgb(255, 176, 196, 222), Drawing.Color.FromArgb(255, 107, 142, 35), _
                                              Drawing.Color.FromArgb(255, 165, 42, 42), Drawing.Color.FromArgb(255, 240, 128, 128), Drawing.Color.FromArgb(255, 112, 128, 144), _
                                              Drawing.Color.FromArgb(255, 32, 178, 170), Drawing.Color.FromArgb(255, 216, 191, 216), Drawing.Color.FromArgb(255, 211, 211, 211), _
                                              Drawing.Color.FromArgb(255, 46, 139, 87), Drawing.Color.FromArgb(255, 47, 79, 79), Drawing.Color.FromArgb(255, 128, 128, 128), _
                                              Drawing.Color.FromArgb(255, 240, 230, 140)}
    Dim colorsetRadar As Drawing.Color() = {Drawing.ColorTranslator.FromHtml("&H806666CC"), Drawing.ColorTranslator.FromHtml("&H80CC6666"), Drawing.ColorTranslator.FromHtml("&H6666CC"), Drawing.ColorTranslator.FromHtml("&HCC6666")}

    Dim colorsetBarras As Drawing.Color() = {Drawing.Color.FromArgb(255, 100, 149, 237), Drawing.Color.FromArgb(255, 176, 196, 222), Drawing.Color.FromArgb(255, 107, 142, 35), _
                                              Drawing.Color.FromArgb(255, 165, 42, 42)}
    'Dim colorsetBarras As Drawing.Color() = {Drawing.Color.FromArgb(255, 205, 133, 63), Drawing.Color.FromArgb(255, 139, 69, 19), Drawing.Color.FromArgb(255, 100, 149, 237), Drawing.Color.FromArgb(255, 176, 196, 222)}
    'Dim colorsetBarras As Drawing.Color() = {Drawing.Color.FromArgb(255, 25, 25, 112), Drawing.Color.FromArgb(255, 100, 149, 237), Drawing.Color.FromArgb(255, 176, 196, 222), Drawing.Color.FromArgb(255, 240, 128, 128)}

    'Dim colorsetPie As Drawing.Color() = {Drawing.Color.FromArgb(255, 72, 61, 139), Drawing.Color.FromArgb(255, 205, 133, 63), Drawing.Color.FromArgb(255, 139, 69, 19), _
    '                                          Drawing.Color.FromArgb(255, 100, 149, 237), Drawing.Color.FromArgb(255, 176, 196, 222), Drawing.Color.FromArgb(255, 107, 142, 35), _
    '                                          Drawing.Color.FromArgb(255, 165, 42, 42), Drawing.Color.FromArgb(255, 240, 128, 128), Drawing.Color.FromArgb(255, 112, 128, 144), _
    '                                          Drawing.Color.FromArgb(255, 32, 178, 170), Drawing.Color.FromArgb(255, 216, 191, 216), Drawing.Color.FromArgb(255, 211, 211, 211), _
    '                                          Drawing.Color.FromArgb(255, 46, 139, 87), Drawing.Color.FromArgb(255, 47, 79, 79), Drawing.Color.FromArgb(255, 128, 128, 128), _
    '                                          Drawing.Color.FromArgb(255, 240, 230, 140)}
#End Region

#Region "Chart"

    ''' <summary>
    ''' Listado de todas las series del grafico
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListaSeries() As List(Of cSerie)
        Get
            Return _listaSeries
        End Get
        Set(ByVal value As List(Of cSerie))
            _listaSeries = value
        End Set
    End Property

    ''' <summary>
    ''' Lista de todos los posibles valores de la grafica
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Values() As List(Of Double())
        Get
            Return _values
        End Get
        Set(ByVal value As List(Of Double()))
            _values = value
        End Set
    End Property

    ''' <summary>
    ''' Etiquetas de los valores
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Labels() As String()
        Get
            Return _labels
        End Get
        Set(ByVal value As String())
            _labels = value
        End Set
    End Property


    ''' <summary>
    ''' Tipo de grafico que se va a pintar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TipoGrafico() As SeriesChartType
        Get
            Return _tipoGrafico
        End Get
        Set(ByVal value As SeriesChartType)
            _tipoGrafico = value
        End Set
    End Property
    ''' <summary>
    ''' Indica si se utiliza la paleta de colores predefinida
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PaletaColoresPredefinida() As Boolean
        Get
            Return _PaletaColoresPredefinida
        End Get
        Set(ByVal value As Boolean)
            _PaletaColoresPredefinida = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si el grafico estará en 3 dimensiones o no
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TresDimensiones() As Boolean
        Get
            Return _3D
        End Get
        Set(ByVal value As Boolean)
            _3D = value
        End Set
    End Property

    ''' <summary>
    ''' Se indica si las etiquetas estaran dentro del grafico, fuera o no seran visibles
    ''' Si las etiquetas estan outside, con la propiedad SerieBorderColor se podra establecer el color de las lineas
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LocalizacionEtiquetas() As EtiquetasLocation
        Get
            Return _etiqLoc
        End Get
        Set(ByVal value As EtiquetasLocation)
            _etiqLoc = value
        End Set
    End Property


    ''' <summary>
    ''' Muestra el contenido del grafico y leyenda con un estilo u otro (caja, etc..)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ContenidoStyle() As BorderSkinStyle
        Get
            Return _contenidoStyle
        End Get
        Set(ByVal value As BorderSkinStyle)
            _contenidoStyle = value
        End Set
    End Property


    ''' <summary>
    ''' Color del borde del grafico
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BorderColor() As Drawing.Color
        Get
            Return _borderColor
        End Get
        Set(ByVal value As Drawing.Color)
            _borderColor = value
        End Set
    End Property

    ''' <summary>
    ''' Ancho del borde del grafico
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BorderWidth() As Integer
        Get
            Return _borderWidth
        End Get
        Set(ByVal value As Integer)
            _borderWidth = value
        End Set
    End Property

    ''' <summary>
    ''' Ancho del borde del grafico
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BorderStyle() As BorderStyle
        Get
            Return _borderStyle
        End Get
        Set(ByVal value As BorderStyle)
            _borderStyle = value
        End Set
    End Property

    ''' <summary>
    ''' Color de fondo del grafico
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BackColor() As Drawing.Color
        Get
            Return _backColor
        End Get
        Set(ByVal value As Drawing.Color)
            _backColor = value
        End Set
    End Property

    ''' <summary>
    ''' Color secundario de fondo del grafico por si se aplicase un gradiente
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SecondaryBackColor() As Drawing.Color
        Get
            Return _secondaryBackColor
        End Get
        Set(ByVal value As Drawing.Color)
            _secondaryBackColor = value
        End Set
    End Property

    ''' <summary>
    ''' Gradiente aplicado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GradientStyle() As GradientStyle
        Get
            Return _gradientStyle
        End Get
        Set(ByVal value As GradientStyle)
            _gradientStyle = value
        End Set
    End Property

    ''' <summary>
    ''' Anchura del grafico
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Anchura() As Integer
        Get
            Return _anchura
        End Get
        Set(ByVal value As Integer)
            _anchura = value
        End Set
    End Property

    ''' <summary>
    ''' Altura del grafico
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Altura() As Integer
        Get
            Return _altura
        End Get
        Set(ByVal value As Integer)
            _altura = value
        End Set
    End Property

    ''' <summary>
    ''' Estilo del grafico en 2D. No tendra que estar activado el 3D
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GraficStyle2D() As eGraficStyle2D
        Get
            Return _graficStyle2D
        End Get
        Set(ByVal value As eGraficStyle2D)
            _graficStyle2D = value
        End Set
    End Property

#End Region

#Region "Titulo"

    ''' <summary>
    ''' Texto del titulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TitleText() As String
        Get
            Return _titleText
        End Get
        Set(ByVal value As String)
            _titleText = value
        End Set
    End Property


    ''' <summary>
    ''' Texto del titulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TitleTextOrientation() As TextOrientation
        Get
            Return _titleTextOrientation
        End Get
        Set(ByVal value As TextOrientation)
            _titleTextOrientation = value
        End Set
    End Property

    ''' <summary>
    ''' Texto del titulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TitleAlignment() As Drawing.ContentAlignment
        Get
            Return _titleAlignment
        End Get
        Set(ByVal value As Drawing.ContentAlignment)
            _titleAlignment = value
        End Set
    End Property


    ''' <summary>
    ''' Color del fondo del titulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TitleBackColor() As Drawing.Color
        Get
            Return _titleBackColor
        End Get
        Set(ByVal value As Drawing.Color)
            _titleBackColor = value
        End Set
    End Property


    ''' <summary>
    ''' Imagen de fondo del titulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TitleBackImage() As String
        Get
            Return _titleBackImage
        End Get
        Set(ByVal value As String)
            _titleBackImage = value
        End Set
    End Property


    ''' <summary>
    ''' Width del border
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TitleBorderWidth() As Integer
        Get
            Return _titleBorderWidth
        End Get
        Set(ByVal value As Integer)
            _titleBorderWidth = value
        End Set
    End Property

    ''' <summary>
    ''' Color del borde
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TitleBorderColor() As Drawing.Color
        Get
            Return _titleBorderColor
        End Get
        Set(ByVal value As Drawing.Color)
            _titleBorderColor = value
        End Set
    End Property

    ''' <summary>
    ''' Tipo del borde
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TitleBorderStyle() As ChartDashStyle
        Get
            Return _titleBorderStyle
        End Get
        Set(ByVal value As ChartDashStyle)
            _titleBorderStyle = value
        End Set
    End Property

    ''' <summary>
    ''' Fuente del titulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property TitleFont() As Drawing.Font
        Get
            Return New Drawing.Font(_titleFontFamily, _titleFontSize, _titleFontStyle, Drawing.GraphicsUnit.Point)
        End Get
    End Property

    ''' <summary>
    ''' Tamaño de la fuente del titulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TitleFontSize() As Integer
        Get
            Return _titleFontSize
        End Get
        Set(ByVal value As Integer)
            _titleFontSize = value
        End Set
    End Property


    ''' <summary>
    ''' Familia de la fuente del titulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TitleFontFamily() As String
        Get
            Return _titleFontFamily
        End Get
        Set(ByVal value As String)
            _titleFontFamily = value
        End Set
    End Property


    ''' <summary>
    ''' Estilo de la fuente del titulo (negrita,cursiva,normal)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TitleFontStyle() As Drawing.FontStyle
        Get
            Return _titleFontStyle
        End Get
        Set(ByVal value As Drawing.FontStyle)
            _titleFontStyle = value
        End Set
    End Property


    ''' <summary>
    ''' Color del titulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TitleColor() As Drawing.Color
        Get
            Return _titleColor
        End Get
        Set(ByVal value As Drawing.Color)
            _titleColor = value
        End Set
    End Property

    ''' <summary>
    ''' Url a la que se redirigira al hacer click
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TitleUrl() As String
        Get
            Return _titleUrl
        End Get
        Set(ByVal value As String)
            _titleUrl = value
        End Set
    End Property

    ''' <summary>
    ''' Value que se le pasa al evento onclick
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TitlePostBackValue() As String
        Get
            Return _titlePostBackValue
        End Get
        Set(ByVal value As String)
            _titlePostBackValue = value
        End Set
    End Property

#End Region

#Region "Leyenda"

    ''' <summary>
    ''' Alineacion del titulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendAlignment() As Drawing.StringAlignment
        Get
            Return _legendAlignment
        End Get
        Set(ByVal value As Drawing.StringAlignment)
            _legendAlignment = value
        End Set
    End Property

    ''' <summary>
    ''' Color del fondo de la leyenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendBackColor() As Drawing.Color
        Get
            Return _legendBackColor
        End Get
        Set(ByVal value As Drawing.Color)
            _legendBackColor = value
        End Set
    End Property

    ''' <summary>
    ''' Segundo color del fondo de la leyenda por si se utiliza gradiente
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendSecondaryBackColor() As Drawing.Color
        Get
            Return _legendSecondaryBackColor
        End Get
        Set(ByVal value As Drawing.Color)
            _legendSecondaryBackColor = value
        End Set
    End Property

    ''' <summary>
    ''' Gradiente de la leyenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendGradientStyle() As GradientStyle
        Get
            Return _legendGradientStyle
        End Get
        Set(ByVal value As GradientStyle)
            _legendGradientStyle = value
        End Set
    End Property

    ''' <summary>
    ''' Color del borde de la leyenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendBorderColor() As Drawing.Color
        Get
            Return _legendBorderColor
        End Get
        Set(ByVal value As Drawing.Color)
            _legendBorderColor = value
        End Set
    End Property

    ''' <summary>
    ''' Color del borde de la leyenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendBorderWidth() As Integer
        Get
            Return _legendBorderWidth
        End Get
        Set(ByVal value As Integer)
            _legendBorderWidth = value
        End Set
    End Property

    ''' <summary>
    ''' Estilo del borde de la leyenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendBorderStyle() As ChartDashStyle
        Get
            Return _legendBorderStyle
        End Get
        Set(ByVal value As ChartDashStyle)
            _legendBorderStyle = value
        End Set
    End Property

    ''' <summary>
    ''' Color del texto de la leyenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendForeColor() As Drawing.Color
        Get
            Return _legendForeColor
        End Get
        Set(ByVal value As Drawing.Color)
            _legendForeColor = value
        End Set
    End Property

    ''' <summary>
    ''' Fuente de la legenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property LegendFont() As Drawing.Font
        Get
            Return New Drawing.Font(_legendFontFamily, _legendFontSize, _legendFontStyle, Drawing.GraphicsUnit.Point)
        End Get
    End Property

    ''' <summary>
    ''' Tamaño de la fuente de la legenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendFontSize() As Integer
        Get
            Return _legendFontSize
        End Get
        Set(ByVal value As Integer)
            _legendFontSize = value
        End Set
    End Property


    ''' <summary>
    ''' Familia de la fuente de la leyenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendFontFamily() As String
        Get
            Return _legendFontFamily
        End Get
        Set(ByVal value As String)
            _legendFontFamily = value
        End Set
    End Property


    ''' <summary>
    ''' Estilo de la fuente de la leyenda (negrita,cursiva,normal)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendFontStyle() As Drawing.FontStyle
        Get
            Return _legendFontStyle
        End Get
        Set(ByVal value As Drawing.FontStyle)
            _legendFontStyle = value
        End Set
    End Property


    ''' <summary>
    ''' Texto del titulo de la leyenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendTitle() As String
        Get
            Return _legendTitle
        End Get
        Set(ByVal value As String)
            _legendTitle = value
        End Set
    End Property

    ''' <summary>
    ''' Alineacion del titulo de la leyenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendTitleAlignment() As Drawing.StringAlignment
        Get
            Return _legendTitleAlignment
        End Get
        Set(ByVal value As Drawing.StringAlignment)
            _legendTitleAlignment = value
        End Set
    End Property

    ''' <summary>
    ''' Color del titulo de la leyenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendTitleColor() As Drawing.Color
        Get
            Return _legendTitleColor
        End Get
        Set(ByVal value As Drawing.Color)
            _legendTitleColor = value
        End Set
    End Property

    ''' <summary>
    ''' Separador del titulo de la leyenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendTitleSeparator() As LegendSeparatorStyle
        Get
            Return _legendTitleSeparator
        End Get
        Set(ByVal value As LegendSeparatorStyle)
            _legendTitleSeparator = value
        End Set
    End Property


    ''' <summary>
    ''' Separador del titulo de la leyenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendTitleSeparatorColor() As Drawing.Color
        Get
            Return _legendTitleSeparatorColor
        End Get
        Set(ByVal value As Drawing.Color)
            _legendTitleSeparatorColor = value
        End Set
    End Property


    ''' <summary>
    ''' Posicion de la legenda con respecto al grafico
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendPosition() As Docking
        Get
            Return _legendPosition
        End Get
        Set(ByVal value As Docking)
            _legendPosition = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si el tamaño del texto, es automaticamente redimensionado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendTextAutoRedimensionado() As Boolean
        Get
            Return _textAutoFit
        End Get
        Set(ByVal value As Boolean)
            _textAutoFit = value
        End Set
    End Property

    ''' <summary>
    ''' Estilo de la legenda. Puede ser column(una debajo de otra), row(todos en fila) o table(en formato tabla)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendStyle() As LegendStyle
        Get
            Return _legendStyle
        End Get
        Set(ByVal value As LegendStyle)
            _legendStyle = value
        End Set
    End Property

    ''' <summary>
    ''' Para el estilo de tabla, puede ser auto, tall(a lo alto)o wide(a lo ancho)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendTableStyle() As LegendTableStyle
        Get
            Return _legendTableStyle
        End Get
        Set(ByVal value As LegendTableStyle)
            _legendTableStyle = value
        End Set
    End Property

    ''' <summary>
    ''' Separador de items de la leyenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendItemSeparator() As LegendSeparatorStyle
        Get
            Return _legendItemSeparator
        End Get
        Set(ByVal value As LegendSeparatorStyle)
            _legendItemSeparator = value
        End Set
    End Property

    ''' <summary>
    ''' Color del Separador de items de la leyenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendItemSeparatorColor() As Drawing.Color
        Get
            Return _legendItemSeparatorColor
        End Get
        Set(ByVal value As Drawing.Color)
            _legendItemSeparatorColor = value
        End Set
    End Property


    ''' <summary>
    ''' Numero de caracteres que se mostraran,antes de hacer un salto de linea
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendTextCharacterWrap() As Integer
        Get
            Return _legendTextCharacterWrap
        End Get
        Set(ByVal value As Integer)
            _legendTextCharacterWrap = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si cada fila la mostrara de un color
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendPintarFilas() As Boolean
        Get
            Return _legendPintarFilas
        End Get
        Set(ByVal value As Boolean)
            _legendPintarFilas = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si el color de las filas alternas
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LegendPintarFilasColor() As Drawing.Color
        Get
            Return _legendPintarFilasColor
        End Get
        Set(ByVal value As Drawing.Color)
            _legendPintarFilasColor = value
        End Set
    End Property

#End Region

#Region "Chart Area"

    ''' <summary>
    ''' Color del fondo del serie
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartBackColor() As Drawing.Color
        Get
            Return _chartBackColor
        End Get
        Set(ByVal value As Drawing.Color)
            _chartBackColor = value
        End Set
    End Property

    ''' <summary>
    ''' Segundo color del fondo del chart por si se utiliza gradiente
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartSecondaryBackColor() As Drawing.Color
        Get
            Return _chartSecondaryBackColor
        End Get
        Set(ByVal value As Drawing.Color)
            _chartSecondaryBackColor = value
        End Set
    End Property

    ''' <summary>
    ''' Gradiente del chart
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartGradientStyle() As GradientStyle
        Get
            Return _chartGradientStyle
        End Get
        Set(ByVal value As GradientStyle)
            _chartGradientStyle = value
        End Set
    End Property

    ''' <summary>
    ''' Color del borde del chart
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartBorderColor() As Drawing.Color
        Get
            Return _chartBorderColor
        End Get
        Set(ByVal value As Drawing.Color)
            _chartBorderColor = value
        End Set
    End Property

    ''' <summary>
    ''' Color del borde del chart
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartBorderWidth() As Integer
        Get
            Return _chartBorderWidth
        End Get
        Set(ByVal value As Integer)
            _chartBorderWidth = value
        End Set
    End Property

    ''' <summary>
    ''' Estilo del borde del chart
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartBorderStyle() As ChartDashStyle
        Get
            Return _chartBorderStyle
        End Get
        Set(ByVal value As ChartDashStyle)
            _chartBorderStyle = value
        End Set
    End Property


    ''' <summary>
    ''' Estilo de la luz del grafico
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartLigthStyle() As LightStyle
        Get
            Return _chartLigthStyle
        End Get
        Set(ByVal value As LightStyle)
            _chartLigthStyle = value
        End Set
    End Property

    ''' <summary>
    ''' Inclinacion del grafico. Puede estar entre -90 y 90
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartInclination() As Integer
        Get
            Return _chartInclination
        End Get
        Set(ByVal value As Integer)
            If (value < -90) Then
                _chartInclination = -90
            ElseIf (value > 90) Then
                _chartInclination = 90
            Else
                _chartInclination = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Rotacion del grafico. Puede estar entre -80 y 80
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartRotation() As Integer
        Get
            Return _chartRotation
        End Get
        Set(ByVal value As Integer)
            If (value < -80) Then
                _chartRotation = -80
            ElseIf (value > 80) Then
                _chartRotation = 80
            Else
                _chartRotation = value
            End If
        End Set
    End Property


    ''' <summary>
    ''' Indica si el margen del eje de las X es visible o no
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeXMarginVisible() As Boolean
        Get
            Return _chartEjeXMarginVisible
        End Get
        Set(ByVal value As Boolean)
            _chartEjeXMarginVisible = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si el margen del eje de las Y es visible o no
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeYMarginVisible() As Boolean
        Get
            Return _chartEjeYMarginVisible
        End Get
        Set(ByVal value As Boolean)
            _chartEjeYMarginVisible = value
        End Set
    End Property


    ''' <summary>
    ''' Indica el numero de labels a mostrar en el ejeX
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeXInterval() As Integer
        Get
            Return _chartEjeXInterval
        End Get
        Set(ByVal value As Integer)
            _chartEjeXInterval = value
        End Set
    End Property


    ''' <summary>
    ''' Indica el numero de labels a mostrar en el ejeY
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeYInterval() As Integer
        Get
            Return _chartEjeYInterval
        End Get
        Set(ByVal value As Integer)
            _chartEjeYInterval = value
        End Set
    End Property




    ''' <summary>
    ''' Valor maximo que tendra el eje de las X
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeXMaximumValue() As Double
        Get
            Return _chartEjeXMaximumValue
        End Get
        Set(ByVal value As Double)
            _chartEjeXMaximumValue = value
        End Set
    End Property

    ''' <summary>
    ''' Valor minimo que tendra el eje de las X
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeXMinimumValue() As Double
        Get
            Return _chartEjeXMinimumValue
        End Get
        Set(ByVal value As Double)
            _chartEjeXMinimumValue = value
        End Set
    End Property

    ''' <summary>
    ''' Valor maximo que tendra el eje de las Y
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeYMaximumValue() As Double
        Get
            Return _chartEjeYMaximumValue
        End Get
        Set(ByVal value As Double)
            _chartEjeYMaximumValue = value
        End Set
    End Property


    ''' <summary>
    ''' Valor minimo que tendra el eje de las Y
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeYMinimumValue() As Double
        Get
            Return _chartEjeYMinimumValue
        End Get
        Set(ByVal value As Double)
            _chartEjeYMinimumValue = value
        End Set
    End Property


    ''' <summary>
    ''' Titulo del eje de las X
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeXTitle() As String
        Get
            Return _chartEjeXTitle
        End Get
        Set(ByVal value As String)
            _chartEjeXTitle = value
        End Set
    End Property


    ''' <summary>
    ''' Titulo del eje de las Y
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeYTitle() As String
        Get
            Return _chartEjeYTitle
        End Get
        Set(ByVal value As String)
            _chartEjeYTitle = value
        End Set
    End Property

    ''' <summary>
    ''' Alineacion del titulo del eje de las X
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeXTitleAlignment() As Drawing.StringAlignment
        Get
            Return _chartEjeXTitleAlignment
        End Get
        Set(ByVal value As Drawing.StringAlignment)
            _chartEjeXTitleAlignment = value
        End Set
    End Property


    ''' <summary>
    ''' Alineacion del titulo del eje de las Y
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeYTitleAlignment() As Drawing.StringAlignment
        Get
            Return _chartEjeYTitleAlignment
        End Get
        Set(ByVal value As Drawing.StringAlignment)
            _chartEjeYTitleAlignment = value
        End Set
    End Property

    ''' <summary>
    ''' Color del titulo del eje de las X
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeXTitleColor() As Drawing.Color
        Get
            Return _chartEjeXTitleColor
        End Get
        Set(ByVal value As Drawing.Color)
            _chartEjeXTitleColor = value
        End Set
    End Property

    ''' <summary>
    ''' Color del titulo del eje de las Y
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeYTitleColor() As Drawing.Color
        Get
            Return _chartEjeYTitleColor
        End Get
        Set(ByVal value As Drawing.Color)
            _chartEjeYTitleColor = value
        End Set
    End Property

    ''' <summary>
    ''' Fuente del titulo del eje X
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property ChartEjeXTitleFont() As Drawing.Font
        Get
            Return New Drawing.Font(_chartEjeXTitleFamily, _chartEjeXTitleSize, _chartEjeXTitleStyle, Drawing.GraphicsUnit.Point)
        End Get
    End Property

    ''' <summary>
    ''' Fuente del titulo del eje Y
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property ChartEjeYTitleFont() As Drawing.Font
        Get
            Return New Drawing.Font(_chartEjeYTitleFamily, _chartEjeYTitleSize, _chartEjeYTitleStyle, Drawing.GraphicsUnit.Point)
        End Get
    End Property

    ''' <summary>
    ''' Tamaño del titulo del eje de las X
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeXTitleSize() As Integer
        Get
            Return _chartEjeXTitleSize
        End Get
        Set(ByVal value As Integer)
            _chartEjeXTitleSize = value
        End Set
    End Property

    ''' <summary>
    ''' Tamaño del titulo del eje de las Y
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeYTitleSize() As Integer
        Get
            Return _chartEjeYTitleSize
        End Get
        Set(ByVal value As Integer)
            _chartEjeYTitleSize = value
        End Set
    End Property

    ''' <summary>
    ''' Familia del titulo del eje de las X
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeXTitleFamily() As String
        Get
            Return _chartEjeXTitleFamily
        End Get
        Set(ByVal value As String)
            _chartEjeXTitleFamily = value
        End Set
    End Property

    ''' <summary>
    ''' Familia del titulo del eje de las Y
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeYTitleFamily() As String
        Get
            Return _chartEjeYTitleFamily
        End Get
        Set(ByVal value As String)
            _chartEjeYTitleFamily = value
        End Set
    End Property

    ''' <summary>
    ''' Estilo del titulo del eje de las X
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeXTitleStyle() As Drawing.FontStyle
        Get
            Return _chartEjeXTitleStyle
        End Get
        Set(ByVal value As Drawing.FontStyle)
            _chartEjeXTitleStyle = value
        End Set
    End Property

    ''' <summary>
    ''' Estilo del titulo del eje de las Y
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeYTitleStyle() As Drawing.FontStyle
        Get
            Return _chartEjeYTitleStyle
        End Get
        Set(ByVal value As Drawing.FontStyle)
            _chartEjeYTitleStyle = value
        End Set
    End Property


    ''' <summary>
    ''' Orientacion del texto del eje X
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeXTitleOrientation() As TextOrientation
        Get
            Return _chartEjeXTitleOrientation
        End Get
        Set(ByVal value As TextOrientation)
            _chartEjeXTitleOrientation = value
        End Set
    End Property

    ''' <summary>
    ''' Orientacion del texto del eje Y
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChartEjeYTitleOrientation() As TextOrientation
        Get
            Return _chartEjeYTitleOrientation
        End Get
        Set(ByVal value As TextOrientation)
            _chartEjeYTitleOrientation = value
        End Set
    End Property



#End Region

#Region "Mostrar"

    ''' <summary>
    ''' Muestra o no el titulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowTitle() As Boolean
        Get
            Return _showTitle
        End Get
        Set(ByVal value As Boolean)
            _showTitle = value
        End Set
    End Property

    ''' <summary>
    ''' Muestra o no la legenda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowLegenda() As Boolean
        Get
            Return _showLegend
        End Get
        Set(ByVal value As Boolean)
            _showLegend = value
        End Set
    End Property


#End Region

#Region "Agregar Serie"

    ''' <summary>
    ''' Agrega una serie a la lista y al grafico
    ''' </summary>
    ''' <param name="cSerie"></param>
    ''' <remarks></remarks>
    Public Sub AgregarSerie(ByVal cSerie As cSerie)
        If (ListaSeries Is Nothing) Then ListaSeries = New List(Of cSerie)
        ListaSeries.Add(cSerie)
        chGrafico.Series.Add(cSerie.Nombre)
        chGrafico.Series.Item(chGrafico.Series.Count - 1).ChartArea = "ChartArea1"  'Se les asigna el chartArea1 a todas las series
    End Sub

#End Region

#End Region

    ''' <summary>
    ''' Dibuja el grafico
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Paint()

        Dim titulo As Title = chGrafico.Titles("Title")
        Dim legenda As Legend = chGrafico.Legends("Legend")
        Dim cArea As ChartArea = chGrafico.ChartAreas("ChartArea1")

        Dim serie As Series
        Dim index As Integer

        chGrafico.BackColor = BackColor
        chGrafico.BackSecondaryColor = SecondaryBackColor
        chGrafico.BackGradientStyle = GradientStyle
        If PaletaColoresPredefinida Then
            chGrafico.Palette = ChartColorPalette.None
            chGrafico.PaletteCustomColors = obtenerPaleta(TipoGrafico)
        End If

        'Border Skin
        chGrafico.BorderSkin.SkinStyle = ContenidoStyle

        chGrafico.Height = New Unit(Altura, UnitType.Pixel)
        chGrafico.Width = Anchura
        chGrafico.BackColor = BackColor

        'Titulo
        titulo.Alignment = TitleAlignment
        titulo.BackColor = TitleBackColor
        titulo.BackImage = TitleBackImage
        titulo.BorderWidth = TitleBorderWidth
        titulo.BorderColor = TitleBorderColor
        titulo.BorderDashStyle = TitleBorderStyle
        titulo.Font = TitleFont
        titulo.PostBackValue = TitlePostBackValue
        titulo.Text = TitleText
        titulo.TextOrientation = TitleTextOrientation
        titulo.Url = TitleUrl
        titulo.Visible = ShowTitle

        'Leyenda
        legenda.Alignment = LegendAlignment
        legenda.Docking = LegendPosition
        legenda.BackColor = LegendBackColor
        legenda.BackSecondaryColor = LegendSecondaryBackColor
        legenda.BackGradientStyle = LegendGradientStyle
        legenda.BorderColor = LegendBorderColor
        legenda.BorderDashStyle = LegendBorderStyle
        legenda.BorderWidth = LegendBorderWidth
        legenda.Docking = LegendPosition
        legenda.ForeColor = LegendForeColor
        legenda.Font = LegendFont
        legenda.IsTextAutoFit = LegendTextAutoRedimensionado
        legenda.LegendStyle = LegendStyle
        legenda.Title = LegendTitle
        legenda.TitleAlignment = LegendTitleAlignment
        legenda.TitleForeColor = LegendTitleColor
        legenda.TitleSeparator = LegendTitleSeparator
        legenda.TitleSeparatorColor = LegendTitleSeparatorColor
        legenda.ItemColumnSeparator = LegendItemSeparator
        legenda.ItemColumnSeparatorColor = LegendItemSeparatorColor
        legenda.TableStyle = LegendTableStyle
        legenda.TextWrapThreshold = LegendTextCharacterWrap
        legenda.InterlacedRows = LegendPintarFilas
        legenda.InterlacedRowsColor = LegendPintarFilasColor

        'Serie
        index = 0

        For Each s As cSerie In ListaSeries
            serie = chGrafico.Series(s.Nombre)

            'Se asignan los valores y las etiquetas
            serie.Points.DataBindXY(Labels, Values.Item(index))
            'Tipo de grafico
            serie.ChartType = TipoGrafico

            If (Not PaletaColoresPredefinida) Then
                serie.Color = s.SerieBackColor
                serie.BackSecondaryColor = s.SerieSecondaryBackColor
                serie.BackGradientStyle = s.SerieGradientStyle
                serie.BorderColor = s.SerieBorderColor
                serie.BorderDashStyle = s.SerieBorderStyle
                serie.BorderWidth = s.SerieBorderWidth
                serie.ShadowOffset = s.SerieSombra
            End If
            If (TipoGrafico = SeriesChartType.Pie And Labels.Count > 10) Then
                LocalizacionEtiquetas = EtiquetasLocation.Outside
                serie("PieLabelStyle") = [Enum].GetName(GetType(EtiquetasLocation), LocalizacionEtiquetas)
                serie("PieLineColor") = Drawing.ColorTranslator.ToHtml(s.SerieBorderColor)  'Color de las lineas que enlazan cuando estan outside
            Else
                LocalizacionEtiquetas = EtiquetasLocation.Inside
            End If

            If ((TipoGrafico = SeriesChartType.Column And ListaSeries.Count > 1) Or (TipoGrafico = SeriesChartType.Pie)) Then  'En las columnas, cuando hay mas de una columna, se muestra la legenda o cuando es de tipo quesito
                serie.IsVisibleInLegend = True
            Else
                serie.IsVisibleInLegend = ShowLegenda
            End If

            If (TipoGrafico = SeriesChartType.Bar Or TipoGrafico = SeriesChartType.Column Or TipoGrafico = SeriesChartType.Pie) Then
                serie.IsValueShownAsLabel = s.SerieShowValueAsLabel
                serie.LabelBackColor = s.SerieValueBackColor
                serie.LabelBorderColor = s.SerieValueBorderColor
                serie.LabelBorderWidth = s.SerieValueBorderWidth
                serie.LabelBorderDashStyle = s.SerieValueBorderStyle
                serie.XAxisType = AxisType.Primary
                If (Not s.SerieAxisYAbajo) Then serie.XAxisType = AxisType.Secondary
                serie.YAxisType = AxisType.Primary
                If (Not s.SerieAxisXIzquierda) Then serie.YAxisType = AxisType.Secondary
                If (s.SerieShowValueAsPercent) Then
                    serie.Label = "#PERCENT{P1}" 'Para mostrar los tantos por ciento en vez de los values
                Else
                    serie.IsValueShownAsLabel = True
                End If

                If (s.SerieLegendShowWithLabel And TipoGrafico <> SeriesChartType.Column) Then  'En los de columnas, no se muestra bien el valor
                    serie.LegendText = "#VALX" 'Para mostrar en la leyenda, los labels y no los values
                End If

                serie("DrawingStyle") = [Enum].GetName(GetType(SerieStyle), s.SerieDrawingStyle).Substring(1)
            End If

            If TipoGrafico = SeriesChartType.Radar Then
                serie("RadarDrawingStyle") = [Enum].GetName(GetType(RadarDrawingStyle), RadarDrawingStyle.Area)
                serie("AreaDrawingStyle") = [Enum].GetName(GetType(AreaDrawingStyle), AreaDrawingStyle.Polygon)
                serie("CircularLabelsStyle") = [Enum].GetName(GetType(CircularLabelsStyle), CircularLabelsStyle.Horizontal)
                serie.BorderDashStyle = ChartDashStyle.Solid
                serie.BorderColor = colorsetRadar.ElementAt(index)
                serie.BorderWidth = 2
            End If

            'Agrupar trozos muy pequeños en uno generico
            serie("CollectedThreshold") = s.SerieAgruparPorcentajeMenor
            serie("CollectedLabel") = s.SerieAgruparLabel
            serie("CollectedLegendText") = s.SerieAgruparLabelLegend
            'serie("CollectedColor") = Drawing.ColorTranslator.ToHtml(s.SerieAgruparColor)

            If Not (TresDimensiones) Then
                'Se le quita el primer caracter porque en la enumeracion, no se podia definir el valor default por ser una palabra reservada
                serie("PieDrawingStyle") = [Enum].GetName(GetType(eGraficStyle2D), GraficStyle2D).Substring(1)
            End If

            index += 1
        Next

        'Chart Area
        cArea.AlignmentOrientation = AreaAlignmentOrientations.All
        cArea.AlignmentStyle = AreaAlignmentStyles.All
        cArea.BackColor = ChartBackColor
        cArea.BackSecondaryColor = ChartSecondaryBackColor
        If (TipoGrafico = SeriesChartType.Column And ChartGradientStyle = DataVisualization.Charting.GradientStyle.None) Then
            cArea.BackGradientStyle = DataVisualization.Charting.GradientStyle.TopBottom
        Else
            cArea.BackGradientStyle = ChartGradientStyle
        End If
        cArea.BorderColor = ChartBorderColor
        cArea.BorderWidth = ChartBorderWidth
        cArea.BorderDashStyle = ChartBorderStyle
        cArea.Area3DStyle.LightStyle = ChartLigthStyle
        If (TipoGrafico = SeriesChartType.Radar) Then
            cArea.Area3DStyle.Enable3D = False
        Else
            cArea.Area3DStyle.Enable3D = TresDimensiones
        End If
        cArea.Area3DStyle.Inclination = ChartInclination
        cArea.Area3DStyle.Rotation = ChartRotation
        cArea.AxisX.IsMarginVisible = ChartEjeXMarginVisible
        cArea.AxisY.IsMarginVisible = ChartEjeYMarginVisible
        cArea.AxisX.Interval = ChartEjeXInterval
        cArea.AxisY.Interval = ChartEjeYInterval

        'Coordenada X
        cArea.AxisX.LineColor = Drawing.Color.Black                     'Color de la línea del eje
        cArea.AxisX.MajorGrid.LineColor = Drawing.Color.LightGray       'Color de la línea dentro del grafico
        cArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Solid      'Estilo linea
        cArea.AxisX.MajorTickMark.LineDashStyle = ChartDashStyle.Solid  'Marca en el eje
        cArea.AxisX.MajorTickMark.LineColor = Drawing.Color.DarkGray    'Color de la marca en el eje 
        cArea.AxisX.MajorTickMark.LineWidth = 2                         'Grosor de la línea en el eje

        'Coordenada Y
        cArea.AxisY.LineColor = Drawing.Color.Black
        cArea.AxisY.MajorGrid.LineColor = Drawing.Color.LightGray
        cArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid
        cArea.AxisY.MajorTickMark.LineDashStyle = ChartDashStyle.Solid
        cArea.AxisY.MajorTickMark.LineColor = Drawing.Color.DarkGray
        cArea.AxisY.MajorTickMark.LineWidth = 2

        If TipoGrafico = SeriesChartType.Radar Then
            cArea.AxisY.MajorTickMark.LineDashStyle = ChartDashStyle.NotSet  'Marca en el eje Y , ninguna
            cArea.AxisY.LineDashStyle = ChartDashStyle.Solid                 'Estilo de la línea del eje Y
            cArea.AxisY.LineColor = Drawing.Color.LightGray                  'Color línea eje Y            
        Else
            'Si es un grafico de columnas, en 3 dimensiones y con mas de un valor, se mostrara en 2D ya que en 3D no se aprecia bien
            If (Values.Count > 1 And TresDimensiones And TipoGrafico = SeriesChartType.Column) Then
                cArea.Area3DStyle.Enable3D = False
            End If

            If (ChartEjeXMaximumValue <> 0) Then cArea.AxisX.Maximum = ChartEjeXMaximumValue
            If (ChartEjeXMinimumValue <> 0) Then cArea.AxisX.Minimum = ChartEjeXMinimumValue
            If (ChartEjeYMaximumValue <> 0) Then cArea.AxisY.Maximum = ChartEjeYMaximumValue
            If (ChartEjeYMinimumValue <> 0) Then cArea.AxisY.Minimum = ChartEjeYMinimumValue

            cArea.AxisX.Title = ChartEjeXTitle
            cArea.AxisX.TitleAlignment = ChartEjeXTitleAlignment
            cArea.AxisX.TitleForeColor = ChartEjeXTitleColor
            cArea.AxisX.TitleFont = ChartEjeXTitleFont

            cArea.AxisY.Title = ChartEjeYTitle
            cArea.AxisX.TitleAlignment = ChartEjeYTitleAlignment
            cArea.AxisY.TitleForeColor = ChartEjeYTitleColor
            cArea.AxisY.TitleFont = ChartEjeYTitleFont
            cArea.AxisX.TextOrientation = ChartEjeXTitleOrientation
            cArea.AxisY.TextOrientation = ChartEjeYTitleOrientation
        End If
        'Antialiasing
        chGrafico.AntiAliasing = AntiAliasingStyles.All
        chGrafico.TextAntiAliasingQuality = TextAntiAliasingQuality.High		
    End Sub


    Public Class cSerie

#Region "Variables miembro"

        Private _nombre As String = String.Empty
        Private _serieBackColor As Drawing.Color = Drawing.Color.Red
        Private _serieSecondaryBackColor As Drawing.Color = Drawing.Color.Transparent
        Private _serieGradientStyle As GradientStyle = GradientStyle.None
        Private _serieBorderColor As Drawing.Color = Drawing.Color.Gainsboro
        Private _serieBorderWidth As Integer = 1
        Private _serieBorderStyle As ChartDashStyle = ChartDashStyle.Solid
        Private _serieSombra As ShadowsOffset = ShadowsOffset.Cero
        Private _serieShowValueAsLabel As Boolean = True
        Private _serieShowValueAsPercent As Boolean = False
        Private _serieLegendShowWithLabel As Boolean = True
        Private _serieValueBackColor As Drawing.Color = Drawing.Color.Transparent
        Private _serieValueBorderColor As Drawing.Color = Drawing.Color.WhiteSmoke
        Private _serieValueBorderWidth As Integer = 0
        Private _serieValueBorderStyle As ChartDashStyle = ChartDashStyle.Solid
        Private _serieAxisXIzquierda As Boolean = True
        Private _serieAxisYAbajo As Boolean = True
        Private _serieAgruparPorcentajeMenor As Integer = 0
        Private _serieAgruparLabel As String = "Otros"
        Private _serieAgruparLabelLegend As String = "Otros"
        Private _serieAgruparColor As Drawing.Color = Drawing.Color.White
        Private _serieDrawingStyle As SerieStyle = SerieStyle._Default

#End Region

#Region "Constructor"

        ''' <summary>
        ''' Constructor con el nombre de la serie
        ''' </summary>
        ''' <param name="NombreSerie">Nombre que se le va a asignar a la serie</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal NombreSerie As String)
            _nombre = NombreSerie
        End Sub

#End Region

#Region "Properties"

        ''' <summary>
        ''' Nombre asignado a la serie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        ''' <summary>
        ''' Color del fondo de la serie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieBackColor() As Drawing.Color
            Get
                Return _serieBackColor
            End Get
            Set(ByVal value As Drawing.Color)
                _serieBackColor = value
            End Set
        End Property

        ''' <summary>
        ''' Segundo color del fondo de la serie por si se utiliza gradiente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieSecondaryBackColor() As Drawing.Color
            Get
                Return _serieSecondaryBackColor
            End Get
            Set(ByVal value As Drawing.Color)
                _serieSecondaryBackColor = value
            End Set
        End Property

        ''' <summary>
        ''' Gradiente de la serie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieGradientStyle() As GradientStyle
            Get
                Return _serieGradientStyle
            End Get
            Set(ByVal value As GradientStyle)
                _serieGradientStyle = value
            End Set
        End Property

        ''' <summary>
        ''' Color del borde de la serie. Si las etiquetas estan outside, con esta propiedad se podra establecer el color de las lineas
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieBorderColor() As Drawing.Color
            Get
                Return _serieBorderColor
            End Get
            Set(ByVal value As Drawing.Color)
                _serieBorderColor = value
            End Set
        End Property

        ''' <summary>
        ''' Color del borde de la serie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieBorderWidth() As Integer
            Get
                Return _serieBorderWidth
            End Get
            Set(ByVal value As Integer)
                _serieBorderWidth = value
            End Set
        End Property

        ''' <summary>
        ''' Estilo del borde de la serie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieBorderStyle() As ChartDashStyle
            Get
                Return _serieBorderStyle
            End Get
            Set(ByVal value As ChartDashStyle)
                _serieBorderStyle = value
            End Set
        End Property


        ''' <summary>
        ''' Sombra de la serie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieSombra() As ShadowsOffset
            Get
                Return _serieSombra
            End Get
            Set(ByVal value As ShadowsOffset)
                _serieSombra = value
            End Set
        End Property


        ''' <summary>
        ''' Muestra el valor como label
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieShowValueAsLabel() As Boolean
            Get
                Return _serieShowValueAsLabel
            End Get
            Set(ByVal value As Boolean)
                _serieShowValueAsLabel = value
            End Set
        End Property


        ''' <summary>
        ''' Color del label que muestra el valor
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieValueBackColor() As Drawing.Color
            Get
                Return _serieValueBackColor
            End Get
            Set(ByVal value As Drawing.Color)
                _serieValueBackColor = value
            End Set
        End Property

        ''' <summary>
        ''' Color del borde del label que muestra el valor
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieValueBorderColor() As Drawing.Color
            Get
                Return _serieValueBorderColor
            End Get
            Set(ByVal value As Drawing.Color)
                _serieValueBorderColor = value
            End Set
        End Property

        ''' <summary>
        ''' Ancho del borde del label que muestra el valor
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieValueBorderWidth() As Integer
            Get
                Return _serieValueBorderWidth
            End Get
            Set(ByVal value As Integer)
                _serieValueBorderWidth = value
            End Set
        End Property


        ''' <summary>
        ''' Ancho del borde del label que muestra el valor
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieValueBorderStyle() As BorderStyle
            Get
                Return _serieValueBorderStyle
            End Get
            Set(ByVal value As BorderStyle)
                _serieValueBorderStyle = value
            End Set
        End Property


        ''' <summary>
        ''' Indica si el eje de las X ira a la izquierda o derecha
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieAxisXIzquierda() As Boolean
            Get
                Return _serieAxisXIzquierda
            End Get
            Set(ByVal value As Boolean)
                _serieAxisXIzquierda = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si el eje de las X ira a abajo o arriba
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieAxisYAbajo() As Boolean
            Get
                Return _serieAxisYAbajo
            End Get
            Set(ByVal value As Boolean)
                _serieAxisYAbajo = value
            End Set
        End Property


        ''' <summary>
        ''' Si se quiere agrupar trozos pequeños en uno solo,se debera indicar el porcentaje, del cual todos los menores, se agruparan en uno solo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieAgruparPorcentajeMenor() As Integer
            Get
                Return _serieAgruparPorcentajeMenor
            End Get
            Set(ByVal value As Integer)
                _serieAgruparPorcentajeMenor = value
            End Set
        End Property


        ''' <summary>
        ''' En la agrupacion de trozos, el label de la serie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieAgruparLabel() As String
            Get
                Return _serieAgruparLabel
            End Get
            Set(ByVal value As String)
                _serieAgruparLabel = value
            End Set
        End Property


        ''' <summary>
        ''' En la agrupacion de trozos, el label de la legenda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieAgruparLabelLegend() As String
            Get
                Return _serieAgruparLabelLegend
            End Get
            Set(ByVal value As String)
                _serieAgruparLabelLegend = value
            End Set
        End Property

        ''' <summary>
        ''' En la agrupacion de trozos, el color de la serie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieAgruparColor() As Drawing.Color
            Get
                Return _serieAgruparColor
            End Get
            Set(ByVal value As Drawing.Color)
                _serieAgruparColor = value
            End Set
        End Property


        ''' <summary>
        ''' Estilo de la serie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieDrawingStyle() As SerieStyle
            Get
                Return _serieDrawingStyle
            End Get
            Set(ByVal value As SerieStyle)
                _serieDrawingStyle = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si debe mostrar los valores como porcentajes.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieShowValueAsPercent() As Boolean
            Get
                Return _serieShowValueAsPercent
            End Get
            Set(ByVal value As Boolean)
                _serieShowValueAsPercent = value
            End Set
        End Property


        ''' <summary>
        ''' Indica si debe mostrar en la leyenda el valor de los labels
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SerieLegendShowWithLabel() As Boolean
            Get
                Return _serieLegendShowWithLabel
            End Get
            Set(ByVal value As Boolean)
                _serieLegendShowWithLabel = value
            End Set
        End Property

#End Region

    End Class

    Private Function obtenerPaleta(ByVal _tipoGrafico As SeriesChartType) As Drawing.Color()
        Try
            Select Case _tipoGrafico
                Case SeriesChartType.Pie        'Grafico Sectores
                    Return colorsetPie
                Case SeriesChartType.Radar      'Grafico Radar
                    Return colorsetRadar
                Case SeriesChartType.Column   'Grafico Barras
                    Return colorsetBarras
                Case Else
                    Return colorsetPie
            End Select

        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class

