@Imports CostCarriersLib

@Code
    Dim pedido As ELL.Pedido = CType(ViewData("Pedido"), ELL.Pedido)
    Dim lineasPedido As List(Of ELL.LineaPedido) = CType(ViewData("LineasPedido"), List(Of ELL.LineaPedido))
    Dim listaIdSteps As List(Of Integer) = CType(Session("ListaIdStepsValidar"), List(Of Integer))
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
    Dim listaPaises As New List(Of SelectListItem)
    Dim paisesBd As New List(Of String())
    paisesBd = BLL.ValoresStepBLL.CargarPaisesImportados()
    For Each pais In paisesBd
        pais(1) = Utils.Traducir(pais(1)).ToUpper()
    Next
    paisesBd = paisesBd.OrderBy(Function(o) o(1)).ToList()
    listaPaises.Add(New SelectListItem With {.Value = 0, .Text = "NOT APPLICABLE"})

    listaPaises.AddRange(paisesBd.Select(Function(o) New SelectListItem With {.Text = o(1), .Value = o(0)}).ToList())
End code

<script src="~/Scripts/jquery.alphanum.js"></script>

<h3><label>@Utils.Traducir("Añadir lineas de pedido") - #@pedido.NumPedido</label></h3>
<hr />

<script type="text/javascript">
    $(function () {
        $.fn.alphanum.setNumericSeparators({
            thousandsSeparator: ".",
            decimalSeparator: ","
        });

        $("[id^='txtPorcentaje-']").numeric({
            allowThouSep: false,
            allowDecSep: false
        });

        $("[id^='txtImporteTotal-']").numeric({
            allowThouSep: false,
            allowDecSep: true,
            maxDecimalPlaces: 2
        });
    })
</script>

@code
    If (lineasPedido IsNot Nothing AndAlso lineasPedido.Count > 0) Then
        @<h4><label class="text-warning">@Utils.Traducir("Líneas de pedido")</label></h4>
        @<div class="row" style="margin-bottom: 30px;">
            <div>
                <table id="tabla" class="table table-condensed table-striped table-hover">
                    <thead>
                        <tr>
                            <th>@Utils.Traducir("Portador")</th>
                            <th>@Utils.Traducir("Localización definitiva del medio")</th>
                            <th>@Utils.Traducir("Items")</th>
                            <th class="text-right">%</th>
                            <th class="text-right">@Utils.Traducir("Importe")</th>
                            <th class="text-center">@Utils.Traducir("Moneda")</th>
                            <th class="text-center">@Utils.Traducir("Estado")</th>
                        </tr>
                    </thead>
                    <tbody>
                        @For Each linea In lineasPedido
                            If linea.CodigoPais.Equals(0) Then
                                linea.Pais = "NOT APPLICABLE"
                            End If
                            @<tr>
                                <td>@linea.CostCarrier - @linea.NombreStep</td>
                                <td>@Utils.Traducir(linea.Pais)</td>
                                <td>@linea.Posiciones</td>
                                <td class="text-right">@linea.Porcentaje</td>
                                <td class="text-right">@linea.Importe.ToString("N2", culturaEsES)</td>
                                <td class="text-center">@linea.MonedaFacturacion</td>
                                <td class="text-center">@Utils.Traducir(linea.EstadoFacturacion)</td>
                            </tr>
                        Next
                    </tbody>
                </table>
            </div>
        </div>
    End If

    If (listaIdSteps IsNot Nothing AndAlso listaIdSteps.Count > 0) Then
        @Using Html.BeginForm("AsociarStepsPedido", "CostCarriers", New With {.idCabecera = pedido.IdCabecera, .idPedido = pedido.Id}, FormMethod.Post, New With {.class = "form-horizontal"})
            @<h4><label class="text-success">@Utils.Traducir("Nuevo")</label></h4>
            @<input type="submit" id="submit" value="@Utils.Traducir("Enviar a facturar")" Class="btn btn-primary" />
            @<br />
            @<br />
            @<div class="row">
                <div>
                    <table id="tabla" class="table table-condensed table-striped table-hover">
                        <thead>
                            <tr>
                                <th>@Utils.Traducir("Portador")</th>
                                <th>@Utils.Traducir("Localización definitiva del medio") <span class="glyphicon glyphicon-info-sign" title="@Utils.Traducir("Necesario saber IVA para aplicar, seleccionar NOT APPLICABLE en caso de no ser utillaje")" style="color: #337ab7"></span></th>
                                <th>@Utils.Traducir("Items")</th>
                                <th class="text-right">%</th>
                                <th class="text-right">@Utils.Traducir("Importe")</th>
                                <th class="text-center">@Utils.Traducir("Moneda")</th>
                                <th class="text-center">@Utils.Traducir("Estado")</th>
                            </tr>
                        </thead>
                        <tbody>
                            @For Each idStep In listaIdSteps
                                @code
                                    Dim paso As ELL.Step = BLL.StepsBLL.Obtener(idStep)
                                End code

                                @<tr>
                                    <td>
                                        @paso.CostCarrier - @paso.Descripcion
                                    </td>
                                    <td>@Html.DropDownList("ddlPais-" & idStep, listaPaises, "", New With {.class = "form-control"})</td>
                                    <td>@Html.TextBox("txtPosiciones-" & idStep, String.Empty, New With {.required = "required", .Class = "form-control", .maxlength = 20})</td>
                                    <td>@Html.TextBox("txtPorcentaje-" & idStep, 100, New With {.required = "required", .Class = "form-control", .style = "text-align: right;"})</td>
                                    <td>@Html.TextBox("txtImporteTotal-" & idStep, String.Empty, New With {.required = "required", .Class = "form-control", .style = "text-align: right;"})</td>
                                    <td class="text-center">@pedido.Moneda</td>
                                    <td class="text-center">@Utils.Traducir("Sin estado")</td>
                                </tr>
                                    Next
                        </tbody>
                    </table>
                </div>
            </div>

            @<div Class="row">
                <label class="col-sm-1 col-form-label">@Utils.Traducir("Instrucciones adicionales")</label>
                <div class="col-sm-11">
                    @Html.TextArea("comentarios", pedido.Comentarios, New With {.maxlength = "1000", .rows = "5", .Class = "form-control"})
                </div>
            </div>
                                    End Using
    End if

End code