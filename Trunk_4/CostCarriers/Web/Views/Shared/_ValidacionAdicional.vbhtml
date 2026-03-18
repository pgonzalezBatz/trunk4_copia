@code
    Dim soloLectura As Boolean = True

    If (ViewData("SoloLectura") IsNot Nothing) Then
        soloLectura = CBool(ViewData("SoloLectura"))
    End If
End Code

<script src="~/Scripts/jquery.alphanum.js"></script>

<script type="text/javascript">
    $(function () {  
        if ("@soloLectura.ToString().ToLower()" == "true") {
            $("#txtMargenNetoAO").attr("readonly", "readonly");
            $("#txtMargenNeto").attr("readonly", "readonly");
            $("#txtVentasEfectivasAO").attr("readonly", "readonly");
            $("#txtVentasEfectivas").attr("readonly", "readonly");
            $("#txtBeneficioHerramentalAO").attr("readonly", "readonly");
            $("#txtBeneficioHerramental").attr("readonly", "readonly");
            $("#txtPlantasClienteAO").attr("readonly", "readonly");
            $("#txtPlantasCliente").attr("readonly", "readonly");
            $("#txtSOPAO").attr("readonly", "readonly");
            $("#txtSOP").attr("readonly", "readonly");            
            $("#txtVolumenMedioAO").attr("readonly", "readonly");
            $("#txtVolumenMedio").attr("readonly", "readonly");
            $("#txtAnyosSerieAO").attr("readonly", "readonly");
            $("#txtAnyosSerie").attr("readonly", "readonly");
            $("#btnConfirmDatosAdicionales").hide();
        }
        else {
            $("#txtMargenNetoAO").removeAttr("readonly");
            $("#txtMargenNeto").removeAttr("readonly");
            $("#txtVentasEfectivasAO").removeAttr("readonly");
            $("#txtVentasEfectivas").removeAttr("readonly");
            $("#txtBeneficioHerramentalAO").removeAttr("readonly");
            $("#txtBeneficioHerramental").removeAttr("readonly");
            $("#txtPlantasClienteAO").removeAttr("readonly");
            $("#txtPlantasCliente").removeAttr("readonly");
            $("#txtSOPv").removeAttr("readonly");
            $("#txtSOP").removeAttr("readonly");            
            $("#txtVolumenMedioAO").removeAttr("readonly");
            $("#txtVolumenMedio").removeAttr("readonly");
            $("#txtAnyosSerieAO").removeAttr("readonly");
            $("#txtAnyosSerie").removeAttr("readonly");
            $("#btnConfirmDatosAdicionales").show();
        }

        $.fn.alphanum.setNumericSeparators({   
            thousandsSeparator: ".",
            decimalSeparator: ","
        });

        $("#txtVentasEfectivasAO, #txtVolumenMedioAO, #txtAnyosSerieAO, #txtVentasEfectivas, #txtVolumenMedio, #txtAnyosSerie").numeric({            
            allowThouSep: false,
            allowDecSep: false
        });

        $("#txtMargenNetoAO, #txtMargenNeto, #txtBeneficioHerramentalAO, #txtBeneficioHerramental").numeric({            
            allowMinus: true,
            allowThouSep: false,            
            maxDecimalPlaces: 3
        });

        $("#SOP, #SOPAO").datetimepicker({
            showClear: true,
            locale: '@Threading.Thread.CurrentThread.CurrentCulture.Name',
            format: '@Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper'
        });
    })
</script>

<div Class="modal fade" id="modalWindowInfoAdicional" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">@Utils.Traducir("Información general")</h4>
            </div>
            <div class="modal-body">
                @Using Html.BeginForm("GuardarValidacionInfoAdicional", "CostCarriers", FormMethod.Post, New With {.class = "form-horizontal"})
                    @Html.Hidden("hIdCabecera", ViewData("IdCabecera"))
                    @Html.Hidden("hIdPlanta")
                    @<table id="tabla" Class="table table-bordered table-condensed">
                        <thead>
                            <tr>
                                <th></th>
                                <th Class="info" style="text-align:center;"><label class="control-label">@Utils.Traducir("Oferta de adjudicación")</label></th>
                                <th Class="info" style="text-align:center;"><label class="control-label">@Utils.Traducir("Real")</label></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="success"><label class="control-label">@Utils.Traducir("Margen neto")</label></td>
                                <td>@Html.TextBox("txtMargenNetoAO", String.Empty, New With {.class = "form-control numeric input-text", .type = "number", .required = "required", .readonly = ""})</td>
                                <td>@Html.TextBox("txtMargenNeto", String.Empty, New With {.class = "form-control numeric input-text", .type = "number", .required = "required", .readonly = ""})</td>
                            </tr>
                            <tr>
                                <td class="success"><label class="control-label">@Utils.Traducir("Ventas efectivas")</label></td>
                                <td>@Html.TextBox("txtVentasEfectivasAO", String.Empty, New With {.class = "form-control numeric input-text", .type = "number", .required = "required"})</td>
                                <td>@Html.TextBox("txtVentasEfectivas", String.Empty, New With {.class = "form-control numeric input-text", .type = "number", .required = "required"})</td>
                            </tr>
                            <tr>
                                <td class="success"><label class="control-label">@Utils.Traducir("Beneficio herramental propiedad cliente")</label></td>
                                <td>@Html.TextBox("txtBeneficioHerramentalAO", String.Empty, New With {.class = "form-control numeric input-text", .type = "number", .readonly = "readonly"})</td>
                                <td>@Html.TextBox("txtBeneficioHerramental", String.Empty, New With {.class = "form-control numeric input-text", .type = "number", .readonly = "readonly"})</td>
                            </tr>
                            <tr>
                                <td class="success"><label class="control-label">@Utils.Traducir("Plantas del cliente")</label></td>
                                <td>@Html.TextBox("txtPlantasClienteAO", String.Empty, New With {.class = "form-control", .maxLength = 100, .required = "required"})</td>
                                <td>@Html.TextBox("txtPlantasCliente", String.Empty, New With {.class = "form-control", .maxLength = 100, .required = "required"})</td>
                            </tr>
                            <tr>
                                <td class="success"><label class="control-label">@Utils.Traducir("SOP")</label></td>
                                <td>
                                    <div class="input-group date" id="SOP">
                                        @Html.TextBox("txtSOPAO", String.Empty, New With {.class = "form-control", .required = "required"})
                                        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                    </div>
                                </td>
                                <td>
                                    <div class="input-group date" id="SOPAO">
                                        @Html.TextBox("txtSOP", String.Empty, New With {.class = "form-control", .required = "required"})
                                        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
                                    </div>
                                </td>
                            </tr>                            
                            <tr>
                                <td class="success"><label class="control-label">@Utils.Traducir("Volumen medio anual")</label></td>
                                <td>@Html.TextBox("txtVolumenMedioAO", String.Empty, New With {.class = "form-control numeric input-text", .type = "number", .required = "required"})</td>
                                <td>@Html.TextBox("txtVolumenMedio", String.Empty, New With {.class = "form-control numeric input-text", .type = "number", .required = "required"})</td>
                            </tr>
                            <tr>
                                <td class="success"><label class="control-label">@Utils.Traducir("Años serie")</label></td>
                                <td>@Html.TextBox("txtAnyosSerieAO", String.Empty, New With {.class = "form-control numeric input-text", .type = "number", .required = "required"})</td>
                                <td>@Html.TextBox("txtAnyosSerie", String.Empty, New With {.class = "form-control numeric input-text", .type = "number", .required = "required"})</td>
                            </tr>
                        </tbody>
                    </table>

                    @<div>
                        <input type="submit" id="btnConfirmDatosAdicionales" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" />
                    </div>

                End Using
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
            </div>
        </div>
    </div>
</div>