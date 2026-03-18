@Imports CostCarriersLib

<h3><label>@Utils.Traducir("Envío validación pasos")</label></h3>
<hr />

@Code
    Dim steps As List(Of ELL.Step) = CType(ViewData("Steps"), List(Of ELL.Step))

    If (steps.Count = 0) Then
        @Html.Label(String.Empty, Utils.Traducir("No se puede enviar steps a validar con presupuesto aprobado a 0"))
        Return
    End If

    Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(steps(0).IdCabecera, False)
    Dim ultimaValidacion As ELL.Validacion = Nothing

    If (Session("DatosValidacion") IsNot Nothing) Then
        Dim datosValidacion As DatosValidacion = CType(Session("DatosValidacion"), DatosValidacion)
        ultimaValidacion = New ELL.Validacion With {.Denominacion = datosValidacion.Denominacion, .Descripcion = datosValidacion.Descripcion, .PrevistoPG = datosValidacion.PrevistoPG}
    ElseIf (ViewData("UltimaValidacion") IsNot Nothing) Then
        ultimaValidacion = CType(ViewData("UltimaValidacion"), ELL.Validacion)
    End If
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
End code

@Using Html.BeginForm("EnviarValidar", "CostCarriers", FormMethod.Post, New With {.class = "form-horizontal"})
    @Html.Hidden("hfIdCabecera", cabecera.Id)
    @<input type="submit" id="volver" name="submitButtonBack" value="@Utils.Traducir("Volver a solicitud")" Class="btn btn-primary" />
    @<input type="submit" id="validar" name="submitButtonSendValidate" value="@Utils.Traducir("Enviar a validar")" Class="btn btn-primary" />
    @<br />
    @<br />
    @<div Class="form-group">
        <label class="col-sm-2 col-form-label">@Utils.Traducir("Proyecto")</label>
        <label class="col-sm-10">@cabecera.NombreProyecto</label>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 col-form-label">@Utils.Traducir("Denominación del presupuesto")</label>
        <div class="col-sm-10">
            @Html.TextArea("denominacion", If(ultimaValidacion IsNot Nothing, ultimaValidacion.Denominacion, String.Empty), New With {.maxlength = "500", .rows = "5", .required = "required", .Class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 col-form-label">@Utils.Traducir("Descripción del presupuesto")</label>
        <div class="col-sm-10">
            @Html.TextArea("descripcion", If(ultimaValidacion IsNot Nothing, ultimaValidacion.Descripcion, String.Empty), New With {.maxlength = "500", .rows = "5", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 col-form-label">@Utils.Traducir("Prevista en PG")</label>
        <div class="col-sm-10">
            @Html.RadioButton("previstoPg", 1, If(ultimaValidacion IsNot Nothing, ultimaValidacion.PrevistoPG, True))&nbsp;<label>@Utils.Traducir("Si")</label>
            @Html.RadioButton("previstoPg", 0, If(ultimaValidacion IsNot Nothing, Not ultimaValidacion.PrevistoPG, False))&nbsp;<label>@Utils.Traducir("No")</label>
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 col-form-label">@Utils.Traducir("Resumen del presupuesto")</label>

        @code
            ' Vamos a obtener las distintas plantas de los steps
            Dim listaIdsPlantasUnicos As List(Of Integer) = (From lstStep In steps
                                                             Group lstStep By lstStep.IdPlantaSAB Into agrupacion = Group
                                                             Select IdPlantaSAB).ToList()
            Dim listaCostGroups As List(Of ELL.CostGroup) = Nothing
            Dim HorasSuma As Integer = Integer.MinValue
            Dim BACGastosSuma As Integer = Integer.MinValue
            Dim PBCSuma As Integer = Integer.MinValue
            Dim marginSuma As String = String.Empty
            Dim HorasSumaValidados As Integer = Integer.MinValue
            Dim BACGastosSumaValidados As Integer = Integer.MinValue
            Dim PBCSumaValidados As Integer = Integer.MinValue
            Dim estiloFilaDestacada As String = String.Empty
            Dim validacionLineaValidada As ELL.ValidacionLinea = Nothing
            Dim stepAux As ELL.Step = Nothing

            If (listaIdsPlantasUnicos.Count > 0) Then

                @<div Class="col-sm-10">
                    <Table id="tabla" Class="table table-bordered table-condensed">

                        @For Each idPlanta In listaIdsPlantasUnicos
                            @<thead>
                                <tr>
                                    <th Class="success" style="text-align:center;text-transform:uppercase" colspan="5">@steps.FirstOrDefault(Function(f) f.IdPlantaSAB = idPlanta).Planta</th>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th Class="danger" style="text-align:center;">@Utils.Traducir("Horas")</th>
                                    <th Class="danger" style="text-align:center;">@Utils.Traducir("Coste")</th>
                                    <th Class="info" style="text-align:center;">@Utils.Traducir("Dinero pagado por cliente")</th>
                                    <th Class="info" style="text-align:center;">@Utils.Traducir("Margen") %</th>
                                </tr>
                            </thead>
                            @<tbody>
                                @code
                                    ' Vamos a obtener todos los costgroup de este proyecto para esta planta
                                    listaCostGroups = BLL.CostsGroupBLL.CargarListadoPorCabeceraPlanta(steps(0).IdCabecera, idPlanta)

                                    For Each costGroup In listaCostGroups.OrderBy(Function(f) f.Descripcion)
                                        HorasSuma = 0
                                        BACGastosSuma = 0
                                        PBCSuma = 0
                                        HorasSumaValidados = 0
                                        BACGastosSumaValidados = 0
                                        PBCSumaValidados = 0
                                        estiloFilaDestacada = String.Empty

                                        ' Cargamos las validaciones linea validados
                                        validacionLineaValidada = BLL.ValidacionesLineaBLL.ObtenerValidadoByCostGroup(costGroup.Id)

                                        If (validacionLineaValidada IsNot Nothing) Then
                                            HorasSumaValidados = validacionLineaValidada.Hours
                                            BACGastosSumaValidados = validacionLineaValidada.BudgetApproved
                                            PBCSumaValidados = validacionLineaValidada.PaidByCustomer
                                        End If

                                        For Each paso In costGroup.Steps
                                            If (paso.IdEstadoValidacion = ELL.Validacion.Estado.Approved OrElse paso.IdEstadoValidacion = ELL.Validacion.Estado.Opened) Then
                                                BACGastosSuma += paso.BACGastosValidacion
                                                PBCSuma += paso.PBCValidacion
                                            End If

                                            ' Si se los pasos del costgroup alguno es de los que se ha enviado a validar se trata
                                            If (steps.Exists(Function(f) f.Id = paso.Id)) Then
                                                stepAux = steps.FirstOrDefault(Function(f) f.Id = paso.Id)
                                                If (stepAux.OBCOrigenDatos = ELL.OrigenDatosStep.OrigenDatosStep.Presupuesto_bonos) Then
                                                    HorasSuma = costGroup.Horas
                                                End If

                                                If (stepAux IsNot Nothing) Then
                                                    BACGastosSuma += stepAux.BACGastos
                                                    PBCSuma += stepAux.PBC
                                                End If

                                                estiloFilaDestacada = "warning text-danger"

                                                ' Si hay algun paso con porcentaje...
                                                If (stepAux.Porcentaje <> Integer.MinValue) Then
                                                    HorasSuma = costGroup.Horas * stepAux.Porcentaje / 100
                                                End If
                                            End If
                                        Next

                                        If (PBCSuma = 0) Then
                                            marginSuma = "NA"
                                        Else
                                            marginSuma = (((PBCSuma - BACGastosSuma) / PBCSuma)).ToString("N2", culturaEsES)
                                        End If

                                        @<tr class='@estiloFilaDestacada'>
                                            <td class="success">
                                                @String.Format("{0} ({1})", costGroup.Descripcion, costGroup.Estado)
                                            </td>
                                            @code
                                                @<td align="right" title='@String.Format("{0}: {1}", Utils.Traducir("Validado"), HorasSumaValidados.ToString("N0", culturaEsES))'>@HorasSuma.ToString("N0", culturaEsES)</td>
                                                @<td align="right" title='@String.Format("{0}: {1}", Utils.Traducir("Validado"), BACGastosSumaValidados.ToString("N0", culturaEsES))'>@BACGastosSuma.ToString("N0", culturaEsES)</td>
                                                @<td align="right" title='@String.Format("{0}: {1}", Utils.Traducir("Validado"), PBCSumaValidados.ToString("N0", culturaEsES))'>@PBCSuma.ToString("N0", culturaEsES)</td>
                                            End code
                                            <td align="right">@marginSuma</td>
                                        </tr>
                                                Next
                                end code
                                <tr>
                                    <td colspan="5"></td>
                                </tr>
                            </tbody>

                                                Next
                    </Table>
                </div>
                                                End If

        End Code
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 col-form-label">@Utils.Traducir("Pasos a validar")</label>

        @code
            @<div Class="form-group">
                <div Class="col-sm-6">
                    <Table id="tabla" Class="table table-bordered table-condensed">
                        <thead>
                            <tr>
                                <th Class="info" style="text-align:center;">@String.Format("{0} ({1})", Utils.Traducir("Nombre"), cabecera.Abreviatura)</th>
                                <th Class="info" style="text-align:center;">@Utils.Traducir("Presupuesto aprobado")</th>
                                <th Class="info" style="text-align:center;">@Utils.Traducir("Dinero pagado por cliente")</th>
                                <th Class="info" style="text-align:center;">@Utils.Traducir("Margen") %</th>
                                <th Class="info" style="text-align:center;">@Utils.Traducir("Empresa")</th>
                            </tr>
                        </thead>
                        <tbody>
                            @For Each paso In steps.OrderBy(Function(f) f.IdPlanta)
                                Dim longitudMaxima As Integer = 40 - cabecera.Abreviatura.Length
                                Dim descripcion As String = If(paso.Descripcion.Length > longitudMaxima, paso.Descripcion.Substring(0, longitudMaxima), paso.Descripcion)
                                paso.CargarValoresStep()

                                @<tr>
                                    <td>@Html.TextBox(String.Format("txtNombre-{0}", paso.Id), descripcion, New With {.maxlength = longitudMaxima, .style = "width: 325px;", .required = "required", .class = "form-control"})</td>
                                    <td align="right"><label class="control-label">@paso.BACGastos.ToString("N0", culturaEsES)</label></td>
                                    <td align="right"><label class="control-label">@paso.PBC.ToString("N0", culturaEsES)</label></td>
                                    <td align="right"><label class="control-label">@paso.Margin</label></td>
                                    <td><label class="control-label">@paso.Planta</label></td>
                                </tr>
                            Next
                        </tbody>
                    </Table>
                </div>
            </div>
        End code
    </div>

            End Using
