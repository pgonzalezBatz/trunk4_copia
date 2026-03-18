@Imports CostCarriersLib

@Code
    Dim pedidos As List(Of ELL.Pedido) = CType(ViewData("Pedidos"), List(Of ELL.Pedido))
    Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(CInt(ViewData("IdCabecera")), False)

    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")

    Dim stepsFacturar As New List(Of ELL.Step)
    If (Session("ListaIdStepsValidar") IsNot Nothing) Then
        For Each idStep In CType(Session("ListaIdStepsValidar"), List(Of Integer))
            stepsFacturar.Add(BLL.StepsBLL.Obtener(idStep))
        Next
    End If

    Dim moneda As Integer = 90
    If (stepsFacturar.Count > 0) Then
        moneda = stepsFacturar.First.IdMoneda
    End If

End code

<script src="~/Scripts/jquery.alphanum.js"></script>

<h3><label>@Utils.Traducir("Envío a facturar") - @cabecera.NombreProyecto</label></h3>
<hr />

<script type="text/javascript">
    $(function () {
        $("#btnCrearPedido").click(function () {
            $("#hIdCabecera").val('@cabecera.Id');
            $("#txtNumPedido").val('');
            $("#txtImporteTotal").val('');

            $.ajax({
                url: '@Url.Action("CargarMonedas", "MetadataYear")',
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (d) {
                    if (d.length > 0) {
                        $('#selectMoneda').empty();
                        $.each(d, function (i, moneda) {
                            $('#selectMoneda').append($('<option>', {
                                value: moneda.Codmon,
                                text: moneda.CodmonBRAIN
                            }));
                        });
                        $('#selectMoneda').val(@moneda);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                    return false;
                }
            });        

            $('#modalWindowCrearPedido').modal('show');
        });

        $.fn.alphanum.setNumericSeparators({
            thousandsSeparator: " ",
            decimalSeparator: ","
        });

        $("#txtImporteTotal").numeric({
            allowThouSep: false,
            allowDecSep: true,
            maxDecimalPlaces: 2
        });
    })
</script>

@code
    If (stepsFacturar IsNot Nothing AndAlso stepsFacturar.Count > 0) Then
        @<h4><label class="text-success">@Utils.Traducir("Pasos seleccionados para facturar")</label></h4>
        @<div class="row">
            <div class="col-sm-4">
        <table id="tabla" class="table table-condensed table-striped table-hover">
            <thead>
                <tr>
                    <th>@Utils.Traducir("Portador")</th>
                </tr>
            </thead>
            <tbody>
                @For Each paso In stepsFacturar
                    @<tr>
                        <td>@paso.CostCarrier - @paso.Descripcion</td>
                    </tr>
                Next
            </tbody>
        </table>
    </div>
</div>
        @<br />@<br />
    End If
End Code

<button type="button" id="btnCrearPedido" class="btn btn-info">
    <span class="glyphicon glyphicon-plus"></span>&nbsp;@Utils.Traducir("Crear nuevo pedido")
</button>

<br /><br />

@code
    If (pedidos.Count > 0) Then
        @<div class="row">
            <div class="col-sm-7">
                <table id="tabla" class="table table-condensed table-striped table-hover">
                    <thead>
                        <tr>
                            <th>@Utils.Traducir("Pedido")</th>
                            <th class="text-right">@Utils.Traducir("Importe total")</th>
                            <th class="text-right">@Utils.Traducir("Importe facturado")</th>
                            <th class="text-center">@Utils.Traducir("Moneda")</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        @For Each pedido In pedidos
                            @<tr>
                                <td>@pedido.NumPedido</td>
                                <td class="text-right">@pedido.ImporteTotal.ToString("N2", culturaEsES)</td>
                                <td class="text-right">@pedido.ImporteFacturado.ToString("N2", culturaEsES)</td>
                                <td class="text-center">@pedido.Moneda</td>
                                <td class="text-center">
                                    <a href='@Url.Action("AsociarStepsPedido", "CostCarriers", New With {.idPedido = pedido.Id})'>
                                        <span class="glyphicon glyphicon-plus-sign" aria-hidden="true" title="@Utils.Traducir("Añadir selección como nueva linea de pedido")"></span>
                                    </a>
                                </td>
                            </tr>
                        Next
                    </tbody>
                </table>
            </div>
        </div>
    Else
        @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
    End If
End code

<div class="modal fade" id="modalWindowCrearPedido" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">

            <div class="modal-header">
                <h4 class="modal-title">@Utils.Traducir("Crear nuevo pedido")</h4>
            </div>
            <div class="modal-body">
                @Using Html.BeginForm("CrearPedido", "CostCarriers", Nothing, New With {.class = "form-horizontal"})
                    @<div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Pedido")</label>
                        <div class="col-sm-8">
                            @Html.Hidden("hIdCabecera")
                            @Html.TextBox("txtNumPedido", String.Empty, New With {.maxlength = "50", .required = "required", .Class = "form-control"})
                        </div>
                    </div>
                    @<div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Importe total")</label>
                        <div class="col-sm-8">
                            @Html.TextBox("txtImporteTotal", String.Empty, New With {.required = "required", .Class = "form-control"})
                        </div>
                    </div>
                    @<div Class="form-group">
                        <label class="col-sm-4 control-label">@Utils.Traducir("Moneda")</label>
                        <div class="col-sm-8">
                            <select id="selectMoneda" name="selectMoneda" class="form-control"></select>
                        </div>
                    </div>
                    @<div Class="form-group">
                        <div class="col-sm-offset-4 col-sm-8">
                            <input type="submit" id="btnConfirmCrearPedido" value="@Utils.Traducir("Crear")" class="btn btn-primary input-block-level form-control" />
                        </div>
                    </div>
                End Using
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
            </div>
        </div>
    </div>
</div>