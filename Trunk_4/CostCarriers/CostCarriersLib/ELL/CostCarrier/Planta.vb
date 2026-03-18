Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class Planta

#Region "Constantes"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Const PLANTA_SAB_ZAMUDIO As Integer = 47

        ''' <summary>
        ''' Código ficticio de XPERT para la planta de Zamudio
        ''' </summary>
        Public Const PLANTA_XPERT_ZAMUDIO As String = "Z"

#End Region

#Region "Miembros"

        ''' <summary>
        ''' 
        ''' </summary>
        Public _datosPresupuesto As List(Of DatosDistribucion)

        ''' <summary>
        ''' 
        ''' </summary>
        Public _datosPresupViajes As List(Of DatosDistribucion)

        ''' <summary>
        ''' 
        ''' </summary>
        Public _datosPresupAnyos As List(Of DatosDistribucionAnyos)

        ''' <summary>
        ''' 
        ''' </summary>
        Private _estados As New Hashtable

        ''' <summary>
        ''' 
        ''' </summary>
        Private _gastosAñoSumatorio As List(Of CostGroup.ValorAño)

        ''' <summary>
        ''' 
        ''' </summary>
        Private _ingresosAñoSumatorio As List(Of CostGroup.ValorAño)

        ''' <summary>
        ''' 
        ''' </summary>
        Private _BACGastosSumatorio As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        Private _BACGastosSumatorioStepsWithPBC As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        Private _OBCSumatorio As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        Private _PBCSumatorio As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        Private _TCSumatorio As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        Private _datoRealInternoSumatorio As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        Private _datoRealInternoSumatorioStepsWithPBC As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        Private _datoRealExternoSumatorio As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        Private _datoRealExternoSumatorioStepsWithPBC As Integer = Integer.MinValue

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' Id planta
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
        ''' Id cabecera
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdCabecera() As Integer

        ''' <summary>
        ''' Id moneda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdMoneda() As Integer

        ''' <summary>
        ''' Moneda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Moneda() As String

        ''' <summary>
        ''' Id planta de BRAIN
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdBrain() As String

        ''' <summary>
        ''' SOP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property SOP() As DateTime

        ''' <summary>
        ''' Años serie
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property AñosSerie() As Integer

        ''' <summary>
        ''' Lista de estados
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Estados As List(Of Estado)
            Get
                If (Not _estados.ContainsKey(Id)) Then
                    _estados.Add(Id, BLL.EstadosBLL.CargarListado(Id, _datosPresupuesto, _datosPresupViajes, _datosPresupAnyos).OrderBy(Function(f) f.Estado).ToList())
                End If

                Return _estados(Id)
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property GastosAñoSumatorio As List(Of CostGroup.ValorAño)
            Get
                If (_gastosAñoSumatorio Is Nothing) Then
                    _gastosAñoSumatorio = New List(Of CostGroup.ValorAño)

                    For Each estado In Estados
                        For Each valorEstado In estado.GastosAñoSumatorio
                            If (Not _gastosAñoSumatorio.Exists(Function(f) f.Año = valorEstado.Año AndAlso f.Trimestre = valorEstado.Trimestre)) Then
                                _gastosAñoSumatorio.Add(New CostGroup.ValorAño With {.Año = valorEstado.Año, .Trimestre = valorEstado.Trimestre, .Valor = valorEstado.Valor})
                            Else
                                _gastosAñoSumatorio.First(Function(f) f.Año = valorEstado.Año AndAlso f.Trimestre = valorEstado.Trimestre).Valor += valorEstado.Valor
                            End If
                        Next
                    Next
                End If

                Return _gastosAñoSumatorio
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IngresosAñoSumatorio As List(Of CostGroup.ValorAño)
            Get
                If (_ingresosAñoSumatorio Is Nothing) Then
                    _ingresosAñoSumatorio = New List(Of CostGroup.ValorAño)

                    For Each estado In Estados
                        For Each valorEstado In estado.IngresosAñoSumatorio
                            If (Not _ingresosAñoSumatorio.Exists(Function(f) f.Año = valorEstado.Año AndAlso f.Trimestre = valorEstado.Trimestre)) Then
                                _ingresosAñoSumatorio.Add(New CostGroup.ValorAño With {.Año = valorEstado.Año, .Trimestre = valorEstado.Trimestre, .Valor = valorEstado.Valor})
                            Else
                                _ingresosAñoSumatorio.First(Function(f) f.Año = valorEstado.Año AndAlso f.Trimestre = valorEstado.Trimestre).Valor += valorEstado.Valor
                            End If
                        Next
                    Next
                End If

                Return _ingresosAñoSumatorio
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property BACGastosSumatorio As Integer
            Get
                If (_BACGastosSumatorio = Integer.MinValue) Then
                    _BACGastosSumatorio = 0

                    Estados.ForEach(Sub(s) _BACGastosSumatorio += s.BACGastosSumatorio)
                End If

                Return _BACGastosSumatorio
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property BACGastosSumatorioStepsWithPBC As Integer
            Get
                If (_BACGastosSumatorioStepsWithPBC = Integer.MinValue) Then
                    _BACGastosSumatorioStepsWithPBC = 0

                    Estados.ForEach(Sub(s) _BACGastosSumatorioStepsWithPBC += s.BACGastosSumatorioStepsWithPBC)
                End If

                Return _BACGastosSumatorioStepsWithPBC
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property OBCSumatorio As Integer
            Get
                If (_OBCSumatorio = Integer.MinValue) Then
                    _OBCSumatorio = 0

                    Estados.ForEach(Sub(s) _OBCSumatorio += s.OBCSumatorio)
                End If

                Return _OBCSumatorio
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property PBCSumatorio As Integer
            Get
                If (_PBCSumatorio = Integer.MinValue) Then
                    _PBCSumatorio = 0

                    Estados.ForEach(Sub(s) _PBCSumatorio += s.PBCSumatorio)

                End If
                Return _PBCSumatorio
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property TCSumatorio As Integer
            Get
                If (_TCSumatorio = Integer.MinValue) Then
                    _TCSumatorio = 0

                    Estados.ForEach(Sub(s) _TCSumatorio += s.TCSumatorio)
                End If

                Return _TCSumatorio
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property MarginSumatorio As String
            Get
                Dim valor As String = String.Empty
                If (PBCSumatorio = 0) Then
                    valor = "NA"
                Else
                    valor = (((PBCSumatorio - BACGastosSumatorioStepsWithPBC) / PBCSumatorio) * 100).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"))
                End If

                Return valor
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property MarginRealSumatorio As String
            Get
                Dim valor As String = String.Empty

                If (PBCSumatorio = 0) Then
                    valor = "NA"
                Else
                    valor = (((PBCSumatorio - DatoRealInternoSumatorioStepsWithPBC - DatoRealExternoSumatorioStepsWithPBC) / PBCSumatorio) * 100).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"))
                End If

                Return valor
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DatoRealInternoSumatorio As Integer
            Get
                If (_datoRealInternoSumatorio = Integer.MinValue) Then
                    _datoRealInternoSumatorio = 0

                    Estados.ForEach(Sub(s) _datoRealInternoSumatorio += s.DatoRealInternoSumatorio)
                End If

                Return _datoRealInternoSumatorio
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DatoRealExternoSumatorio As Integer
            Get
                If (_datoRealExternoSumatorio = Integer.MinValue) Then
                    _datoRealExternoSumatorio = 0

                    Estados.ForEach(Sub(s) _datoRealExternoSumatorio += s.DatoRealExternoSumatorio)
                End If

                Return _datoRealExternoSumatorio
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DatoRealInternoSumatorioStepsWithPBC As Integer
            Get
                If (_datoRealInternoSumatorioStepsWithPBC = Integer.MinValue) Then
                    _datoRealInternoSumatorioStepsWithPBC = 0

                    Estados.ForEach(Sub(s) _datoRealInternoSumatorioStepsWithPBC += s.DatoRealInternoSumatorioStepsWithPBC)
                End If

                Return _datoRealInternoSumatorioStepsWithPBC
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DatoRealExternoSumatorioStepsWithPBC As Integer
            Get
                If (_datoRealExternoSumatorioStepsWithPBC = Integer.MinValue) Then
                    _datoRealExternoSumatorioStepsWithPBC = 0

                    Estados.ForEach(Sub(s) _datoRealExternoSumatorioStepsWithPBC += s.DatoRealExternoSumatorioStepsWithPBC)
                End If

                Return _datoRealExternoSumatorioStepsWithPBC
            End Get
        End Property

#End Region

    End Class

End Namespace

