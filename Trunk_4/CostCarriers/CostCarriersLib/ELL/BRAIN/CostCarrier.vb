Namespace ELL.BRAIN

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CostCarrier

#Region "Miembros"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Property Empresa As String

        ''' <summary>
        ''' 
        ''' </summary>
        Public Property Planta As String

        ''' <summary>
        ''' 
        ''' </summary>
        Public Property Valor As String

        ''' <summary>
        ''' 
        ''' </summary>
        Public Property Denominacion As String

        ''' <summary>
        ''' 
        ''' </summary>
        Public Property Lantegi As String

        ''' <summary>
        ''' 
        ''' </summary>
        Public Property FechaApertura As DateTime

        ''' <summary>
        ''' 
        ''' </summary>
        Public Property Datos As String

        ''' <summary>
        ''' Fecha en formato YYYYMMDD
        ''' </summary>
        Public ReadOnly Property FechaAperturaCadena As String
            Get
                Return String.Format("{0}{1}{2}", FechaApertura.Year, FechaApertura.Month.ToString().PadLeft(2, "0"), FechaApertura.Day.ToString().PadLeft(2, "0"))
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DescripcionCompleta As String
            Get
                Return String.Format("{0} - {1}", Valor, Denominacion)
            End Get
        End Property

#End Region

    End Class

End Namespace

