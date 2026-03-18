@Imports CostCarriersLib

@code
    Dim costCarrier = ViewData("CostCarrier")
End Code

<script type="text/javascript">

    $(function () {
        $("#Monedas").change(function () {
            $('#hfMonedas').val($('#Monedas option:selected').text());
        })
    })

</script>

<h3><label>@Utils.Traducir("Agregar metadatos anuales del portador de coste")</label></h3>
<hr />

@Using Html.BeginForm("Agregar", "MetadataYear", FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Código portador")</label>
        <div class="col-sm-4">
            @Html.DropDownList("CostCarriersMetadata", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Año")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Anyos", Nothing, New With {.class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Moneda")</label>
        <div class="col-sm-4">
            @Html.Hidden("hfMonedas")
            @Html.DropDownList("Monedas", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Presup. bonos persona")</label>
        <div class="col-sm-4">
            @Html.TextBox("presupBonosPersona", String.Empty, New With {.required = "required", .type = "number", .step = "any", .class = "form-control text-right"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Presup. facturas")</label>
        <div class="col-sm-4">
            @Html.TextBox("presupFacturas", String.Empty, New With {.required = "required", .type = "number", .step = "any", .class = "form-control text-right"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Presup. viajes")</label>
        <div class="col-sm-4">
            @Html.TextBox("presupViajes", String.Empty, New With {.required = "required", .type = "number", .step = "any", .class = "form-control text-right"})
        </div>
    </div>
    @code
        Dim empresa As String = costCarrier.split("-")(0)
        Dim planta As String = costCarrier.split("-")(1)
        Dim codigoPortador As String = costCarrier.split("-")(2)
        Dim metadata As ELL.BRAIN.CCMetadata = BLL.BRAIN.CCMetadataBLL.Obtener(empresa, planta, codigoPortador)

        ' Si es de propiedad de BATZ no se muestra el campo de Importe de venta al cliente
        If (metadata.Propiedad = "E") Then
            @<div Class="form-group">
                <label class="col-sm-3 control-label">@Utils.Traducir("Importe de venta al cliente")</label>
                <div class="col-sm-4">
                    @Html.TextBox("importeVentaCliente", String.Empty, New With {.type = "number", .step = "any", .class = "form-control text-right"})
                </div>
            </div>
        End If
    End Code
    @<div Class="form-group">
        <div class="col-sm-offset-3 col-sm-4">
            <input type="submit" id="submit" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>
        End Using
