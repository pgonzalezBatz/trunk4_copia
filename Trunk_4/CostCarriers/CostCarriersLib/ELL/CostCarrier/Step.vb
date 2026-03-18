Imports System.Web.Script.Serialization
Imports System.Text.RegularExpressions

Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class [Step]

#Region "Enumerados"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum OrigenStep
            Plantilla = 1
            Oferta = 2
            DeSolicitud = 3
        End Enum

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum OrigenDR
            Externo = 0
            Interno = 1
            Interno_Externo = 2
        End Enum

#End Region

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
        Private _valoresStep As List(Of ValorStep)

        ''' <summary>
        ''' 
        ''' </summary>
        Private _valoresStepValidacion As List(Of ValorStep)

        ''' <summary>
        ''' 
        ''' </summary>
        Private _datoRealInterno As Integer = Integer.MinValue

        ''' <summary>
        ''' 
        ''' </summary>
        Private _datoRealExterno As Integer = Integer.MinValue

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

        ''' <summary>
        ''' Id cost group
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdCostGroup() As Integer

        ''' <summary>
        ''' Offer budget cost origen datos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property OBCOrigenDatos() As Integer

        ''' <summary>
        ''' Target cost formula
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TCFormula() As String

        ''' <summary>
        ''' Target cost formula customer
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TCFormulaCustomer() As String

        ''' <summary>
        ''' Target cost
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TC() As Integer = 0

        ''' <summary>
        ''' Target cost
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property TCValidacion() As Integer = 0

        ''' <summary>
        ''' Gastos año origen datos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property GastosAñoOrigenDatos() As Integer

        ''' <summary>
        ''' Ingresos año origen datos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IngresosAñoOrigenDatos() As Integer

        ''' <summary>
        ''' Orden
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Orden() As Integer

        ''' <summary>
        ''' Id bonos del cost group
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdBonos() As Integer

        ''' <summary>
        ''' Estado al que pertenece el cost group padre
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Estado() As String

        ''' <summary>
        ''' Proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Proyecto() As String

        ''' <summary>
        ''' Fecha validación
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property FechaValidacion() As DateTime

        ''' <summary>
        ''' Id validador
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdValidador() As Integer

        ''' <summary>
        ''' Descripción BRAIN
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property DescripcionBRAIN() As String

        ''' <summary>
        ''' Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Code() As Integer

        ''' <summary>
        ''' Paid by customer
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PBC() As Integer

        ''' <summary>
        ''' Paid by customer
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property PBCValidacion() As Integer

        ''' <summary>
        ''' Id planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlanta() As Integer

        ''' <summary>
        ''' Id planta SAB
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlantaSAB() As Integer

        ''' <summary>
        ''' Planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As String

        ''' <summary>
        ''' Indica si puede cambiar la planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CambiarPlanta() As Boolean

        ''' <summary>
        ''' Id del cost group de oferta técnica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdCostGroupOT() As Integer

        ''' <summary>
        ''' Id del step de oferta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdOferta() As String

        ''' <summary>
        ''' Indica el origen del step
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Origen() As Integer

        ''' <summary>
        ''' Porcentaje
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Porcentaje() As Integer = Integer.MinValue

        ''' <summary>
        ''' Agrupación del costgroup ot
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdAgrupacion() As Integer = Integer.MinValue

        ''' <summary>
        ''' Cambio porcentaje
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CambioPorcentaje() As Boolean = False

        ''' <summary>
        ''' Id cabecera
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdCabecera() As Integer

        ''' <summary>
        ''' Id estado
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdEstado() As Integer

        ''' <summary>
        ''' Id estado
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdEstadoProyecto() As Integer

        ''' <summary>
        ''' Budget approved cost
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property BACGastos() As Integer

        ''' <summary>
        ''' Budget approved cost
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property BACGastosValidacion() As Integer

        ''' <summary>
        ''' Visible
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Visible() As Boolean = True

        ''' <summary>
        ''' Origen de dato real
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property OrigenDatoReal() As Integer

        ''' <summary>
        ''' Codigo cost carrier en BRAIN
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CostCarrier() As String

        ''' <summary>
        ''' Abreviatura
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Abreviatura() As String

        ''' <summary>
        ''' Indica si es un paso de información general
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property EsInfoGeneral() As Boolean = False

        ''' <summary>
        ''' Margen
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property Margin() As String
            Get
                Dim valor As String = String.Empty
                If (PBC = 0) Then
                    valor = "NA"
                Else
                    valor = (((PBC - BACGastos) / PBC) * 100).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"))
                End If

                Return valor
            End Get
        End Property

        ''' <summary>
        ''' Margen
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property MarginReal() As String
            Get
                Dim valor As String = String.Empty
                If (PBC = 0) Then
                    valor = "NA"
                Else
                    valor = (((PBC - DatoRealInterno - DatoRealExterno) / PBC) * 100).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"))
                End If

                Return valor
            End Get
        End Property

        ''' <summary>
        ''' Margen
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property MarginValidacion() As String
            Get
                Dim valor As String = String.Empty
                If (PBCValidacion = 0) Then
                    valor = "NA"
                Else
                    valor = ((PBCValidacion - BACGastosValidacion) / PBCValidacion).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"))
                End If

                Return valor
            End Get
        End Property

        ''' <summary>
        ''' Id estado validación
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdEstadoValidacion() As Integer

        ''' <summary>
        ''' Estado validación
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property EstadoValidacion() As String

        ''' <summary>
        ''' Id usuario validador
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdUsuarioValidador() As Integer

        ''' <summary>
        ''' Id validacion línea
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdValidacionLinea() As Integer

        ''' <summary>
        ''' Nos dice si un step se puede mandar a validar y si se puede cambiar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property Cambiable() As Boolean
            Get
                Return IdEstadoValidacion = Integer.MinValue OrElse
                                      IdEstadoValidacion = ELL.Validacion.Estado.Opened OrElse
                                      IdEstadoValidacion = ELL.Validacion.Estado.Rejected OrElse
                                      (IdEstadoValidacion = ELL.Validacion.Estado.Approved AndAlso Not String.IsNullOrEmpty(CostCarrier))
            End Get
        End Property

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
        ''' Comentarios del histórico
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Comentarios() As String

        '************* VALORES *************'

        ''' <summary>
        ''' Valores Step
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property ValoresStep() As List(Of ValorStep)
            Get
                Return CargarValoresStep()
            End Get
        End Property

        ''' <summary>
        ''' Valores Step
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property ValoresStepValidacion() As List(Of ValorStep)
            Get
                Return CargarValoresStepValidacion()
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DatoRealInterno As Integer
            Get
                Dim dato As Integer = 0
                Dim jss As New JavaScriptSerializer()

                If (_datoRealInterno = Integer.MinValue AndAlso IdBonos <> Integer.MinValue AndAlso (OrigenDatoReal = OrigenDR.Interno OrElse OrigenDatoReal = OrigenDR.Interno_Externo)) Then
                    ' BONOS
                    Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                        dato += jss.Deserialize(Of DatoReal)(cliente.GetDatoReal_Bonos(IdPlantaSAB, Proyecto, Estado, IdBonos, IdMoneda)).Cost

                        ' En un mail de Maite del 30/09/2019 me dice que no está aplicandose el porcentaje a este campo
                        If (Porcentaje <> Integer.MinValue AndAlso OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_bonos) Then
                            dato = dato * Porcentaje / 100
                        End If

                        _datoRealInterno = dato
                    End Using
                ElseIf (_datoRealInterno <> Integer.MinValue) Then
                    dato = _datoRealInterno
                End If

                Return dato
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DatoRealExterno As Integer
            Get
                Dim dato As Integer = 0
                Dim jss As New JavaScriptSerializer()

                If (_datoRealExterno = Integer.MinValue AndAlso Not String.IsNullOrEmpty(CostCarrier) AndAlso (OrigenDatoReal = OrigenDR.Externo OrElse OrigenDatoReal = OrigenDR.Interno_Externo)) Then
                    ' XPERT
                    If (Not String.IsNullOrEmpty(CostCarrier)) Then
                        Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                            dato += jss.Deserialize(Of DatoReal)(cliente.GetDatoReal_XPERT(IdPlantaSAB, CostCarrier, Moneda)).Cost
                            _datoRealExterno = dato
                        End Using
                    End If
                ElseIf (_datoRealExterno <> Integer.MinValue) Then
                    dato = _datoRealExterno
                End If

                Return dato
            End Get
        End Property

        ''' <summary>
        ''' Correlativo para la formación del código de cost carrier
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CorrelativoCC() As Integer

#End Region

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Function CargarValoresStep()
            If (_valoresStep Is Nothing) Then

                _valoresStep = BLL.ValoresStepBLL.CargarListado(Id)

                ' Aquí hay que discernir paso de oferta de los que no porque sino esta sumando de más
                If (Origen = OrigenStep.Oferta) Then
                    If (_valoresStep.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Budget_approved_cost)) Then
                        BACGastos = _valoresStep.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Budget_approved_cost).Valor
                    End If
                Else
                    For Each valor As ValorStep In _valoresStep.Where(Function(f) f.IdColumna = ELL.Columna.Tipo.Year_expenses)
                        BACGastos += valor.Valor
                    Next
                End If

                If (_datosPresupuesto IsNot Nothing) Then
                    If (OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_bonos) Then
                        If (_valoresStep.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost)) Then
                            _valoresStep.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor = _datosPresupuesto.Coste
                        Else
                            _valoresStep.Add(New ELL.ValorStep With {.IdStep = Id, .IdColumna = ELL.Columna.Tipo.Offer_budget_cost, .Valor = _datosPresupuesto.Coste})
                        End If
                    End If
                End If

                If (_datosPresupViajes IsNot Nothing) Then
                    If (OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_viajes) Then
                        If (_valoresStep.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost)) Then
                            _valoresStep.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor = _datosPresupViajes.Coste
                        Else
                            _valoresStep.Add(New ELL.ValorStep With {.IdStep = Id, .IdColumna = ELL.Columna.Tipo.Offer_budget_cost, .Valor = _datosPresupViajes.Coste})
                        End If
                    End If
                End If

                If (_datosPresupAnyos IsNot Nothing) Then
                    For Each datosPresup In _datosPresupAnyos
                        If (GastosAñoOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Planificacion) Then
                            If (_valoresStep.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Year_expenses AndAlso f.Año = datosPresup.Anyo AndAlso f.Trimestre = datosPresup.Trimestre)) Then
                                _valoresStep.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Year_expenses AndAlso f.Año = datosPresup.Anyo AndAlso f.Trimestre = datosPresup.Trimestre).Valor = datosPresup.Coste
                            Else
                                _valoresStep.Add(New ELL.ValorStep With {.IdStep = Id, .IdColumna = ELL.Columna.Tipo.Year_expenses, .Valor = datosPresup.Coste, .Año = datosPresup.Anyo, .Trimestre = datosPresup.Trimestre})
                            End If

                            BACGastos += datosPresup.Coste
                        End If
                    Next
                End If

                ' Sumamos todos los valores que sean de ingresos para calcular el PBC
                _valoresStep.Where(Function(f) f.IdColumna = ELL.Columna.Tipo.Year_incomes).ToList().ForEach(Sub(s) PBC += s.Valor)

                ' Esto quiere decir que es un valor de step que viene de oferta por eso el año y el trimestre están a 0
                If (_valoresStep.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Paid_by_customer AndAlso f.Año = 0 AndAlso f.Trimestre = 0)) Then
                    PBC = _valoresStep.FirstOrDefault(Function(f) f.IdColumna = ELL.Columna.Tipo.Paid_by_customer AndAlso f.Año = 0 AndAlso f.Trimestre = 0).Valor
                End If

#Region "Evaluación de formulas"

                ' Puede ser que el step tenga una formula o que el cost group padre venga de OT
                If (Not String.IsNullOrEmpty(TCFormula) OrElse IdCostGroupOT <> Integer.MinValue) Then
                    ' A través de expresiones regulares vamos a sacar los valores entre los corchetes \[\d+\]
                    Dim formula As String = String.Empty
                    Dim TCFormulaAux As String = String.Empty

                    If (IdCostGroupOT <> Integer.MinValue AndAlso Origen = ELL.Step.OrigenStep.Oferta) Then
                        ' Es un step que viene de OT. Su cost group tiene asociado una
                        ' Aquí dependecmos del valor del PBC
                        ' - Si el PBC es mayor que cero cogermos la FORMULA_CUSTOMER de la tabla COST_GROUP_OT
                        ' - Si el PBC es cero cogermos la FORMULA_BATZ de la tabla COST_GROUP_OT
                        Dim costGroupOT As ELL.CostGroupOT = BLL.CostsGroupOTBLL.Obtener(IdCostGroupOT)
                        If (PBC > 0) Then
                            formula = costGroupOT.FormulaCustomer
                            TCFormulaAux = costGroupOT.FormulaCustomer
                        Else
                            formula = costGroupOT.FormulaBATZ
                            TCFormulaAux = costGroupOT.FormulaBATZ
                        End If
                    Else
                        If (IdCostGroupOT <> Integer.MinValue AndAlso Origen = ELL.Step.OrigenStep.DeSolicitud) Then
                            Dim costGroupOT As ELL.CostGroupOT = BLL.CostsGroupOTBLL.Obtener(IdCostGroupOT)
                            If (PBC > 0) Then
                                formula = costGroupOT.FormulaCustomer
                                TCFormulaAux = costGroupOT.FormulaCustomer
                            Else
                                formula = costGroupOT.FormulaBATZ
                                TCFormulaAux = costGroupOT.FormulaBATZ
                            End If
                        ElseIf (IdCostGroupOT = Integer.MinValue OrElse Origen = ELL.Step.OrigenStep.Plantilla) Then
                            If (PBC > 0) Then
                                formula = TCFormulaCustomer
                                TCFormulaAux = TCFormulaCustomer
                            Else
                                formula = TCFormula
                                TCFormulaAux = TCFormula
                            End If
                        End If
                    End If

                    Dim variable As ELL.VariableFormula.Tipo
                    Dim valor As Integer = 0

                    For Each match As Match In Regex.Matches(formula, "\[\d+\]")
                        'Sacamos el valor interno de cada corchete que sabemos que es un número
                        variable = Regex.Match(match.Value, "\d+").Value

                        Select Case variable
                            Case ELL.VariableFormula.Tipo.Offer_budget
                                If (_valoresStep.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost)) Then
                                    valor = _valoresStep.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor
                                End If
                            Case ELL.VariableFormula.Tipo.Paid_by_customer
                                valor = PBC
                        End Select

                        TCFormulaAux = TCFormulaAux.Replace(match.Value, valor)
                    Next

                    Dim dt As New DataTable()
                    Try
                        ' Podría darse el caso de una formula mal construida por eso el try catch
                        ' Hacemos el Math.Round para quitar los decimales y redondear
                        If (Not String.IsNullOrEmpty(TCFormulaAux)) Then
                            TC = Math.Round(dt.Compute(TCFormulaAux, Nothing), MidpointRounding.AwayFromZero)
                        End If
                    Catch
                    End Try
                End If

                ' Podemos tenemos un step que su target en interface sea una caja de texto
                If (_valoresStep.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Target_cost)) Then
                    TC = _valoresStep.FirstOrDefault(Function(f) f.IdColumna = ELL.Columna.Tipo.Target_cost).Valor
                End If
#End Region
                ' Si un step es susceptible de que le cambien el porcentaje y su valor es integer.minvalue debemos repartir dicho porcentaje
                If (CambioPorcentaje AndAlso Porcentaje = Integer.MinValue AndAlso IdAgrupacion <> Integer.MinValue AndAlso Origen = ELL.Step.OrigenStep.Plantilla AndAlso OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_bonos) Then
                    ' Vamos a ver con cuántos steps comparte dicho porcentaje
                    Dim numSteps As Integer = BLL.StepsBLL.ObtenerNumStepsAgrupados(IdCabecera, IdPlanta, IdEstado, IdAgrupacion)
                    Porcentaje = 100

                    If (numSteps <> 0) Then
                        Porcentaje = 100 / numSteps
                    End If
                End If

                If (Porcentaje <> Integer.MinValue AndAlso OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_bonos) Then
                    TC = TC * If(Porcentaje <> Integer.MinValue, Porcentaje / 100, 1)
                    BACGastos = BACGastos * If(Porcentaje <> Integer.MinValue, Porcentaje / 100, 1)

                    If (_valoresStep.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost)) Then
                        _valoresStep.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor = _valoresStep.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor * If(Porcentaje <> Integer.MinValue, Porcentaje / 100, 1)
                    End If
                End If
            End If
            Return _valoresStep

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Function CargarValoresStepValidacion()
            If (_valoresStepValidacion Is Nothing) Then
                _valoresStepValidacion = New List(Of ValorStep)

#Region "Cargamos los datos de los presupuesto"

                'If (_datosPresupuesto Is Nothing) Then
                '    If (IdBonos <> Integer.MinValue) Then
                '        Dim jss As New JavaScriptSerializer()

                '        Using cliente As New ServicioBonos.ServicioBonos
                '            ' Coste:x;
                '            'TODO:
                '            _datosPresupuesto = Nothing 'jss.Deserialize(Of List(Of DatosDistribucion))(cliente.GetDatosDistribucion(Proyecto, Estado, ELL.OrigenDatosStep.TipoDistribucion.Presupuesto, IdBonos, IdMoneda)).FirstOrDefault()
                '        End Using
                '    Else
                '        _datosPresupuesto = New DatosDistribucion()
                '    End If
                'End If

                'If (_datosPresupViajes Is Nothing) Then
                '    Dim jss As New JavaScriptSerializer()

                '    Using cliente As New ServicioBonos.ServicioBonos
                '        ' Coste:x;
                '        'TODO:
                '        _datosPresupViajes = Nothing 'jss.Deserialize(Of List(Of DatosDistribucion))(cliente.GetDatosDistribucion(Proyecto, Estado, ELL.OrigenDatosStep.TipoDistribucion.Viaje, Integer.MinValue, IdMoneda)).FirstOrDefault()
                '    End Using
                'End If

#End Region

                'If (_datosPresupuesto IsNot Nothing) Then
                '    If (OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_bonos) Then
                '        If (_valoresStepValidacion.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost)) Then
                '            _valoresStepValidacion.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor = _datosPresupuesto.Coste
                '        Else
                '            _valoresStepValidacion.Add(New ELL.ValorStep With {.IdStep = Id, .IdColumna = ELL.Columna.Tipo.Offer_budget_cost, .Valor = _datosPresupuesto.Coste})
                '        End If
                '    End If
                'End If

                'If (_datosPresupViajes IsNot Nothing) Then
                '    If (OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_viajes) Then
                '        If (_valoresStepValidacion.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost)) Then
                '            _valoresStepValidacion.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor = _datosPresupViajes.Coste
                '        Else
                '            _valoresStepValidacion.Add(New ELL.ValorStep With {.IdStep = Id, .IdColumna = ELL.Columna.Tipo.Offer_budget_cost, .Valor = _datosPresupViajes.Coste})
                '        End If
                '    End If
                'End If

                ' Obtengo los valores de lo último validado (aprobado, abierto o cerrado)
                Dim validacionLinea As ELL.ValidacionLinea = BLL.ValidacionesLineaBLL.ObtenerUltimoAprobado(Id)

                If (validacionLinea IsNot Nothing) Then
                    'Cargamos los valores de año y los metemos en la valores validacion del step
                    validacionLinea.ValidacionesAño = BLL.ValidacionesAñoBLL.CargarListadoUltimosValidadosPorValidacionLinea(validacionLinea.Id)
                    For Each validacionAño In validacionLinea.ValidacionesAño
                        _valoresStepValidacion.Add(New ValorStep With {.Año = validacionAño.Año, .IdColumna = validacionAño.IdColumna, .IdStep = Id, .Trimestre = validacionAño.Trimestre, .Valor = validacionAño.Valor})
                    Next

                    PBCValidacion = validacionLinea.PaidByCustomer
                    BACGastosValidacion = validacionLinea.BudgetApproved

                    'If (Porcentaje <> Integer.MinValue) Then
                    '    BACGastosValidacion = BACGastosValidacion * If(Porcentaje <> Integer.MinValue, Porcentaje / 100, 1)
                    'End If

                    _valoresStepValidacion.Add(New ValorStep With {.Año = 0, .IdColumna = ELL.Columna.Tipo.Offer_budget_cost, .IdStep = Id, .Trimestre = 0, .Valor = validacionLinea.OfferBudget})
                Else
                    ' Si se entra por aquí es porque no hay nada validado (aprobado, abierto o cerrado). Cogemos los últimos valores enviar a validar. Podría se que ni hubiera datos porque
                    ' no haya nada validado con anterioridad

                    validacionLinea = BLL.ValidacionesLineaBLL.ObtenerPorStep(Id)

                    If (validacionLinea IsNot Nothing) Then
                        'Cargamos los valores de año y los metemos en la valores validacion del step
                        validacionLinea.ValidacionesAño = BLL.ValidacionesAñoBLL.CargarListadoPorValidacionLinea(validacionLinea.Id)
                        For Each validacionAño In validacionLinea.ValidacionesAño
                            _valoresStepValidacion.Add(New ValorStep With {.Año = validacionAño.Año, .IdColumna = validacionAño.IdColumna, .IdStep = Id, .Trimestre = validacionAño.Trimestre, .Valor = validacionAño.Valor})
                        Next

                        PBCValidacion = validacionLinea.PaidByCustomer
                        BACGastosValidacion = validacionLinea.BudgetApproved

                        'If (Porcentaje <> Integer.MinValue) Then
                        '    BACGastosValidacion = BACGastosValidacion * If(Porcentaje <> Integer.MinValue, Porcentaje / 100, 1)
                        'End If

                        _valoresStepValidacion.Add(New ValorStep With {.Año = 0, .IdColumna = ELL.Columna.Tipo.Offer_budget_cost, .IdStep = Id, .Trimestre = 0, .Valor = validacionLinea.OfferBudget})
                    End If
                End If

#Region "Evaluación de formulas"

                ' Puede ser que el step tenga una formula o que el cost group padre venga de OT
                If (Not String.IsNullOrEmpty(TCFormula) OrElse IdCostGroupOT <> Integer.MinValue) Then
                    ' A través de expresiones regulares vamos a sacar los valores entre los corchetes \[\d+\]
                    Dim formula As String = String.Empty
                    Dim TCFormulaAux As String = String.Empty

                    If (IdCostGroupOT <> Integer.MinValue AndAlso Origen = ELL.Step.OrigenStep.Oferta) Then
                        ' Es un step que viene de OT. Su cost group tiene asociado una
                        ' Aquí dependecmos del valor del PBC
                        ' - Si el PBC es mayor que cero cogermos la FORMULA_CUSTOMER de la tabla COST_GROUP_OT
                        ' - Si el PBC es cero cogermos la FORMULA_BATZ de la tabla COST_GROUP_OT
                        Dim costGroupOT As ELL.CostGroupOT = BLL.CostsGroupOTBLL.Obtener(IdCostGroupOT)
                        If (PBCValidacion > 0) Then
                            formula = costGroupOT.FormulaCustomer
                            TCFormulaAux = costGroupOT.FormulaCustomer
                        Else
                            formula = costGroupOT.FormulaBATZ
                            TCFormulaAux = costGroupOT.FormulaBATZ
                        End If
                    Else
                        If (IdCostGroupOT <> Integer.MinValue AndAlso Origen = ELL.Step.OrigenStep.DeSolicitud) Then
                            Dim costGroupOT As ELL.CostGroupOT = BLL.CostsGroupOTBLL.Obtener(IdCostGroupOT)
                            If (PBCValidacion > 0) Then
                                formula = costGroupOT.FormulaCustomer
                                TCFormulaAux = costGroupOT.FormulaCustomer
                            Else
                                formula = costGroupOT.FormulaBATZ
                                TCFormulaAux = costGroupOT.FormulaBATZ
                            End If
                        ElseIf (IdCostGroupOT = Integer.MinValue OrElse Origen = ELL.Step.OrigenStep.Plantilla) Then
                            If (PBC > 0) Then
                                formula = TCFormulaCustomer
                                TCFormulaAux = TCFormulaCustomer
                            Else
                                formula = TCFormula
                                TCFormulaAux = TCFormula
                            End If
                        End If
                    End If

                    Dim variable As ELL.VariableFormula.Tipo
                    Dim valor As Integer = 0

                    For Each match As Match In Regex.Matches(formula, "\[\d+\]")
                        'Sacamos el valor interno de cada corchete que sabemos que es un número
                        variable = Regex.Match(match.Value, "\d+").Value

                        Select Case variable
                            Case ELL.VariableFormula.Tipo.Offer_budget
                                If (_valoresStepValidacion.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost)) Then
                                    valor = If(_valoresStepValidacion.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor = Integer.MinValue, 0, _valoresStepValidacion.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor)
                                End If
                            Case ELL.VariableFormula.Tipo.Paid_by_customer
                                valor = PBCValidacion
                        End Select

                        TCFormulaAux = TCFormulaAux.Replace(match.Value, valor)
                    Next

                    Dim dt As New DataTable()
                    Try
                        ' Podría darse el caso de una formula mal construida por eso el try catch
                        ' Hacemos el Math.Round para quitar los decimales y redondear
                        If (Not String.IsNullOrEmpty(TCFormulaAux)) Then
                            TCValidacion = Math.Round(dt.Compute(TCFormulaAux, Nothing), MidpointRounding.AwayFromZero)
                        End If

                        'If (Porcentaje <> Integer.MinValue) Then
                        '    TCValidacion = TCValidacion * If(Porcentaje <> Integer.MinValue, Porcentaje / 100, 1)
                        '    If (_valoresStepValidacion.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost)) Then
                        '        _valoresStepValidacion.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor = _valoresStepValidacion.First(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost).Valor * If(Porcentaje <> Integer.MinValue, Porcentaje / 100, 1)
                        '    End If

                        '    For Each valorStep In _valoresStepValidacion.Where(Function(f) f.IdColumna = ELL.Columna.Tipo.Year_expenses OrElse f.IdColumna = ELL.Columna.Tipo.Year_incomes)
                        '        valorStep.Valor = valorStep.Valor * If(Porcentaje <> Integer.MinValue, Porcentaje / 100, 1)
                        '    Next
                        'End If
                    Catch
                    End Try
                End If
#End Region

                ' Podemos tenemos un step que su target en interface sea una caja de texto
                If (_valoresStepValidacion.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Target_cost)) Then
                    TCValidacion = _valoresStepValidacion.FirstOrDefault(Function(f) f.IdColumna = ELL.Columna.Tipo.Target_cost).Valor
                End If

            End If
            Return _valoresStepValidacion

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="offerBudget"></param>
        ''' <param name="paidByCustomer"></param>
        ''' <returns></returns>
        Function EvaluarFormulaTarget(ByVal offerBudget As Integer, ByVal paidByCustomer As Integer) As Integer
            Dim target As Integer = 0

            ' Puede ser que el step tenga una formula o que el cost group padre venga de OT
            If (Not String.IsNullOrEmpty(TCFormula) OrElse IdCostGroupOT <> Integer.MinValue) Then
                ' A través de expresiones regulares vamos a sacar los valores entre los corchetes \[\d+\]
                Dim formula As String = String.Empty
                Dim TCFormulaAux As String = String.Empty

                If (IdCostGroupOT <> Integer.MinValue AndAlso Origen = ELL.Step.OrigenStep.Oferta) Then
                    ' Es un step que viene de OT. Su cost group tiene asociado una
                    ' Aquí dependecmos del valor del PBC
                    ' - Si el PBC es mayor que cero cogermos la FORMULA_CUSTOMER de la tabla COST_GROUP_OT
                    ' - Si el PBC es cero cogermos la FORMULA_BATZ de la tabla COST_GROUP_OT
                    Dim costGroupOT As ELL.CostGroupOT = BLL.CostsGroupOTBLL.Obtener(IdCostGroupOT)
                    If (PBC > 0) Then
                        formula = costGroupOT.FormulaCustomer
                        TCFormulaAux = costGroupOT.FormulaCustomer
                    Else
                        formula = costGroupOT.FormulaBATZ
                        TCFormulaAux = costGroupOT.FormulaBATZ
                    End If
                Else
                    If (IdCostGroupOT <> Integer.MinValue AndAlso Origen = ELL.Step.OrigenStep.DeSolicitud) Then
                        Dim costGroupOT As ELL.CostGroupOT = BLL.CostsGroupOTBLL.Obtener(IdCostGroupOT)
                        If (PBC > 0) Then
                            formula = costGroupOT.FormulaCustomer
                            TCFormulaAux = costGroupOT.FormulaCustomer
                        Else
                            formula = costGroupOT.FormulaBATZ
                            TCFormulaAux = costGroupOT.FormulaBATZ
                        End If
                    ElseIf (IdCostGroupOT = Integer.MinValue OrElse Origen = ELL.Step.OrigenStep.Plantilla) Then
                        If (PBC > 0) Then
                            formula = TCFormulaCustomer
                            TCFormulaAux = TCFormulaCustomer
                        Else
                            formula = TCFormula
                            TCFormulaAux = TCFormula
                        End If
                    End If
                End If

                Dim variable As ELL.VariableFormula.Tipo
                Dim valor As Integer = 0

                For Each match As Match In Regex.Matches(formula, "\[\d+\]")
                    'Sacamos el valor interno de cada corchete que sabemos que es un número
                    variable = Regex.Match(match.Value, "\d+").Value

                    Select Case variable
                        Case ELL.VariableFormula.Tipo.Offer_budget
                            If (_valoresStep.Exists(Function(f) f.IdColumna = ELL.Columna.Tipo.Offer_budget_cost)) Then
                                valor = offerBudget
                            End If
                        Case ELL.VariableFormula.Tipo.Paid_by_customer
                            valor = paidByCustomer
                    End Select

                    TCFormulaAux = TCFormulaAux.Replace(match.Value, valor)
                Next

                Dim dt As New DataTable()
                Try
                    ' Podría darse el caso de una formula mal construida por eso el try catch
                    ' Hacemos el Math.Round para quitar los decimales y redondear
                    If (Not String.IsNullOrEmpty(TCFormulaAux)) Then
                        target = Math.Round(dt.Compute(TCFormulaAux, Nothing), MidpointRounding.AwayFromZero)
                    End If
                Catch
                End Try
            End If

            Return target
        End Function

#End Region

    End Class

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class DatosDistribucion

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Coste As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Horas As Decimal

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property IdGrupoDistrib As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Estado As String

    End Class

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class DatosDistribucionAnyos

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Anyo As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Trimestre As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Coste As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Estado As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property IdGrupoDistrib As Integer

    End Class

    ''' <summary>
    ''' Clase para recoger los datos real interno y externo
    ''' </summary>
    Public Class DatoReal

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Cost As Decimal

    End Class

End Namespace

