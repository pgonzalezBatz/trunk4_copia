Imports System.Web.Script.Serialization

Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CostGroup

#Region "Miembros"

        ''' <summary>
        ''' 
        ''' </summary>
        Public _datosPresupuesto As DatosDistribucion

        ''' <summary>
        ''' 
        ''' </summary>
        Public _datosPresupViajes As DatosDistribucion

        ''' <summary>
        ''' 
        ''' </summary>
        Public _datosPresupAnyos As List(Of DatosDistribucionAnyos)

        ''' <summary>
        ''' 
        ''' </summary>
        Private _desc As String = String.Empty

        ''' <summary>
        ''' 
        ''' </summary>
        Private _steps As New Hashtable

        ''' <summary>
        ''' 
        ''' </summary>
        Private _OBCSumatorio As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        Private _gastosAñoSumatorio As List(Of ValorAño)

        ''' <summary>
        ''' 
        ''' </summary>
        Private _ingresosAñoSumatorio As List(Of ValorAño)

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
        Private _TCSumatorio As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        Private _PBCSumatorio As Integer = Integer.MinValue

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
        ''' Descripcion
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Descripcion() As String
            Get
                Dim ret As String = String.Empty

                If (IdBonos <> Integer.MinValue) Then
                    If (IdCostGroupOT <> Integer.MinValue AndAlso IdCostGroupOT <> -1) Then
                        ret = CostGroupOT
                    ElseIf (IdBonos = -1) Then
                        ret = "No group"
                    Else
                        Dim jss As New JavaScriptSerializer()

                        Using cliente As New ServicioBonos.ServicioBonos
                            ret = jss.Deserialize(Of Object)(cliente.GetGrupoDistribucion(IdBonos))("Nombre")
                        End Using
                    End If
                ElseIf (String.IsNullOrEmpty(_desc) AndAlso IdCostGroupOT <> Integer.MinValue AndAlso IdCostGroupOT <> -1) Then
                    ret = CostGroupOT
                Else
                    ret = _desc
                End If

                Return ret
            End Get
            Set(value As String)
                _desc = value
            End Set
        End Property

        ''' <summary>
        ''' Id estado plantilla
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdEstado() As Integer

        ''' <summary>
        ''' Estado
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Estado() As String

        ''' <summary>
        ''' Id cost group de bonos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdBonos() As Integer

        ''' <summary>
        ''' Id cost group de bonos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdCostGroupOT() As Integer

        ''' <summary>
        ''' Cost group de bonos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CostGroupOT() As String

        ''' <summary>
        ''' Id agrupación del costgroup OT
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdAgrupacion() As Integer

        ''' <summary>
        ''' Id cabecera
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdCabecera() As Integer

        ''' <summary>
        ''' Id planta SAB
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlantaSAB() As Integer

        ''' <summary>
        ''' Id planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlanta() As Integer

        ''' <summary>
        ''' Id moneda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdMoneda() As Integer

        ''' <summary>
        ''' Lista de steps
        ''' </summary>
        ''' <returns></returns>
        Public Property Steps As List(Of [Step])
            Get
                If (Not _steps.ContainsKey(Id)) Then
                    _steps.Add(Id, BLL.StepsBLL.CargarListado(Id, _datosPresupuesto, _datosPresupViajes, _datosPresupAnyos).OrderBy(Function(f) f.Orden).ToList())
                End If

                Return _steps(Id)
            End Get
            Set
                _steps.Clear()
                _steps.Add(Id, Value)
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property OBCSumatorio As Integer
            Get
                If (_OBCSumatorio = Integer.MinValue) Then
                    _OBCSumatorio = 0

                    For Each paso In Steps
                        'If (paso.IdEstadoValidacion = Integer.MinValue OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Rejected OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Opened) Then
                        For Each valorStep In paso.ValoresStep.Where(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost)
                                If (valorStep.Valor <> Integer.MinValue) Then
                                    _OBCSumatorio += valorStep.Valor
                                End If
                            Next
                        'ElseIf (paso.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Approved OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Closed) Then
                        '    For Each valorStep In paso.ValoresStepValidacion.Where(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost)
                        '        If (valorStep.Valor <> Integer.MinValue) Then
                        '            _OBCSumatorio += valorStep.Valor
                        '        End If
                        '    Next
                        'End If
                    Next
                End If

                Return _OBCSumatorio
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property GastosAñoSumatorio As List(Of ValorAño)
            Get
                If (_gastosAñoSumatorio Is Nothing) Then
                    _gastosAñoSumatorio = New List(Of ValorAño)

                    For Each paso In Steps
                        'If (paso.IdEstadoValidacion = Integer.MinValue OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Rejected OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Opened) Then
                        For Each valorStep In paso.ValoresStep.Where(Function(f) f.Año <> 0 AndAlso f.Trimestre <> 0 AndAlso f.IdColumna = Columna.Tipo.Year_expenses)
                                If (Not _gastosAñoSumatorio.Exists(Function(f) f.Año = valorStep.Año AndAlso f.Trimestre = valorStep.Trimestre)) Then
                                    _gastosAñoSumatorio.Add(New ValorAño With {.Año = valorStep.Año, .Trimestre = valorStep.Trimestre, .Valor = valorStep.Valor})
                                Else
                                    _gastosAñoSumatorio.First(Function(f) f.Año = valorStep.Año AndAlso f.Trimestre = valorStep.Trimestre).Valor += valorStep.Valor
                                End If
                            Next
                        'ElseIf (paso.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Approved OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Closed) Then
                        '    For Each valorStep In paso.ValoresStepValidacion.Where(Function(f) f.Año <> 0 AndAlso f.Trimestre <> 0 AndAlso f.IdColumna = Columna.Tipo.Year_expenses)
                        '        If (Not _gastosAñoSumatorio.Exists(Function(f) f.Año = valorStep.Año AndAlso f.Trimestre = valorStep.Trimestre)) Then
                        '            _gastosAñoSumatorio.Add(New ValorAño With {.Año = valorStep.Año, .Trimestre = valorStep.Trimestre, .Valor = valorStep.Valor})
                        '        Else
                        '            _gastosAñoSumatorio.First(Function(f) f.Año = valorStep.Año AndAlso f.Trimestre = valorStep.Trimestre).Valor += valorStep.Valor
                        '        End If
                        '    Next
                        'End If
                    Next
                End If

                Return _gastosAñoSumatorio
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IngresosAñoSumatorio As List(Of ValorAño)
            Get
                If (_ingresosAñoSumatorio Is Nothing) Then
                    _ingresosAñoSumatorio = New List(Of ValorAño)

                    For Each paso In Steps
                        'If (paso.IdEstadoValidacion = Integer.MinValue OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Rejected OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Opened) Then
                        For Each valorStep In paso.ValoresStep.Where(Function(f) f.Año <> 0 AndAlso f.Trimestre <> 0 AndAlso f.IdColumna = Columna.Tipo.Year_incomes)
                                If (Not _ingresosAñoSumatorio.Exists(Function(f) f.Año = valorStep.Año AndAlso f.Trimestre = valorStep.Trimestre)) Then
                                    _ingresosAñoSumatorio.Add(New ValorAño With {.Año = valorStep.Año, .Trimestre = valorStep.Trimestre, .Valor = valorStep.Valor})
                                Else
                                    _ingresosAñoSumatorio.First(Function(f) f.Año = valorStep.Año AndAlso f.Trimestre = valorStep.Trimestre).Valor += valorStep.Valor
                                End If
                            Next
                        'ElseIf (paso.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Approved OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Closed) Then
                        '    For Each valorStep In paso.ValoresStepValidacion.Where(Function(f) f.Año <> 0 AndAlso f.Trimestre <> 0 AndAlso f.IdColumna = Columna.Tipo.Year_incomes)
                        '        If (Not _ingresosAñoSumatorio.Exists(Function(f) f.Año = valorStep.Año AndAlso f.Trimestre = valorStep.Trimestre)) Then
                        '            _ingresosAñoSumatorio.Add(New ValorAño With {.Año = valorStep.Año, .Trimestre = valorStep.Trimestre, .Valor = valorStep.Valor})
                        '        Else
                        '            _ingresosAñoSumatorio.First(Function(f) f.Año = valorStep.Año AndAlso f.Trimestre = valorStep.Trimestre).Valor += valorStep.Valor
                        '        End If
                        '    Next
                        'End If
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

                    For Each paso In Steps
                        'If (paso.IdEstadoValidacion = Integer.MinValue OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Rejected OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Opened) Then
                        _BACGastosSumatorio += paso.BACGastos
                        'ElseIf (paso.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Approved OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Closed) Then
                        '    _BACGastosSumatorio += paso.BACGastosValidacion
                        'End If
                    Next
                End If

                Return _BACGastosSumatorio
            End Get
        End Property

        ''' <summary>
        ''' Sumatorio de BAC de paso con PBC distinto de 0
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property BACGastosSumatorioStepsWithPBC As Integer
            Get
                If (_BACGastosSumatorioStepsWithPBC = Integer.MinValue) Then
                    _BACGastosSumatorioStepsWithPBC = 0

                    For Each paso In Steps
                        'If (paso.IdEstadoValidacion = Integer.MinValue OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Rejected OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Opened) Then
                        If (paso.PBC <> 0) Then
                                _BACGastosSumatorioStepsWithPBC += paso.BACGastos
                            End If
                        'ElseIf (paso.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Approved OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Closed) Then
                        '    If (paso.PBC <> 0) Then
                        '        _BACGastosSumatorioStepsWithPBC += paso.BACGastosValidacion
                        '    End If
                        'End If
                    Next
                End If

                Return _BACGastosSumatorioStepsWithPBC
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

                    For Each paso In Steps
                        'If (paso.IdEstadoValidacion = Integer.MinValue OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Rejected OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Opened) Then
                        If (paso.TC <> Integer.MinValue) Then
                                _TCSumatorio += paso.TC
                            End If
                        'ElseIf (paso.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Approved OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Closed) Then
                        '    If (paso.TCValidacion <> Integer.MinValue) Then
                        '        _TCSumatorio += paso.TCValidacion
                        '    End If
                        'End If
                    Next
                End If

                Return _TCSumatorio
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

                    For Each paso In Steps
                        'If (paso.IdEstadoValidacion = Integer.MinValue OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Rejected OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Opened) Then
                        _PBCSumatorio += paso.PBC
                        'ElseIf (paso.IdEstadoValidacion = ELL.Validacion.Estado.Waiting_for_approval OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Approved OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Closed) Then
                        '    _PBCSumatorio += paso.PBCValidacion
                        'End If
                    Next
                End If

                Return _PBCSumatorio
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
        Public ReadOnly Property Horas As Decimal
            Get
                Dim resultado As Decimal = Decimal.Zero

                If (IdBonos <> Integer.MinValue) Then
                    Dim jss As New JavaScriptSerializer()

                    Using cliente As New ServicioBonos.ServicioBonos
                        ' Coste:x;
                        'TODO:
                        Dim datosDistribucion As DatosDistribucion = jss.Deserialize(Of List(Of DatosDistribucion))(cliente.GetDatosDistribucion(Steps(0).Proyecto, ELL.OrigenDatosStep.TipoDistribucion.Planificacion, IdBonos, IdMoneda)).FirstOrDefault(Function(f) f.Estado = Estado)

                        If (datosDistribucion IsNot Nothing) Then
                            resultado = datosDistribucion.Horas
                        End If
                    End Using
                End If
                Return resultado
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DatoRealInternoSumatorio As Integer
            Get
                If (_DatoRealInternoSumatorio = Integer.MinValue) Then
                    _DatoRealInternoSumatorio = 0

                    Steps.ForEach(Sub(s) _DatoRealInternoSumatorio += s.DatoRealInterno)
                End If

                Return _DatoRealInternoSumatorio
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DatoRealExternoSumatorio As Integer
            Get
                If (_DatoRealExternoSumatorio = Integer.MinValue) Then
                    _DatoRealExternoSumatorio = 0

                    Steps.ForEach(Sub(s) _DatoRealExternoSumatorio += s.DatoRealExterno)

                End If
                Return _DatoRealExternoSumatorio
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

                    Steps.Where(Function(f) f.PBC <> 0).ToList().ForEach(Sub(s) _datoRealInternoSumatorioStepsWithPBC += s.DatoRealInterno)
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

                    Steps.Where(Function(f) f.PBC <> 0).ToList().ForEach(Sub(s) _datoRealExternoSumatorioStepsWithPBC += s.DatoRealExterno)

                End If
                Return _datoRealExternoSumatorioStepsWithPBC
            End Get
        End Property

#End Region

        ''' <summary>
        ''' 
        ''' </summary>
        Public Class ValorAño

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <returns></returns>
            Public Property Año As Integer

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <returns></returns>
            Public Property Trimestre As Integer

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <returns></returns>
            Public Property Valor As Integer

        End Class

    End Class

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CostGroupCambioPlantaStep

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
        ''' <returns></returns>
        Public Property Planta As String

        ''' <summary>
        ''' Estado
        ''' </summary>
        ''' <returns></returns>
        Public Property Estado As String

        ''' <summary>
        ''' 
        ''' </summary>
        Public ReadOnly Property PlantaEstado As String
            Get
                Return String.Format("{0} ({1})", Planta, Estado)
            End Get
        End Property

#End Region

    End Class

    ''' <summary>
    ''' Sirve para agrupar los datos de los costgroups. Por ejemplo quieren los serial tooling agrupados en dos serial tooling BATZ y CUSTOMER
    ''' </summary>
    Public Class CostGroupParaValidacion

#Region "Properties"

        ''' <summary>
        ''' Nombre
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Nombre() As String

        ''' <summary>
        ''' Horas suma
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property HorasSuma() As Integer = 0

        ''' <summary>
        ''' BAC gastos suma
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property BACGastosSuma() As Integer = 0

        ''' <summary>
        ''' PBC suma
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PBCSuma() As Integer = 0

        ''' <summary>
        ''' BAC gastos suma
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property BACGastosSumaUltimoAprobado() As Integer = 0

        ''' <summary>
        ''' PBC suma
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PBCSumaUltimoAprobado() As Integer = 0

        ''' <summary>
        ''' Real suma
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property RealSuma() As Integer = 0

        ''' <summary>
        ''' Horas suma validados
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property HorasSumaValidados() As Integer = 0

        ''' <summary>
        ''' BAC gasto suma validados
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property BACGastosSumaValidados() As Integer = 0

        ''' <summary>
        ''' PBC suma validados
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PBCSumaValidados() As Integer = 0

        ''' <summary>
        ''' Real suma validados
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property RealSumaValidados() As Integer = 0

        ''' <summary>
        ''' Estilo fila destacada
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property EstiloFilaDestacada() As String = String.Empty

        ''' <summary>
        ''' Lista de ids de costgroup. Se usa cuando agrupamos los cost group. PE: Serial tooling
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdsCostGroup() As New List(Of Integer)

        ''' <summary>
        ''' En caso de ser agrupado de costgroup me indica si es de batz o customer
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Tipo As String

#End Region

    End Class

End Namespace

