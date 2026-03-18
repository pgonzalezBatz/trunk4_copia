@Imports CostCarriersLib

@code
    Dim ccMetadataYear As ELL.BRAIN.CCMetadataYear = CType(ViewData("CCMetadataYear"), ELL.BRAIN.CCMetadataYear)
    Dim costCarrier = ViewData("CostCarrier")
    Dim culturaEsES As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
End Code

<script type="text/javascript">

    $(function () {
        $("#Monedas").change(function () {
            $('#hfMonedas').val($('#Monedas option:selected').text());
        })
    })

</script>

<h3><label><@Utils.Traducir("Editar metadatos anuales del portador de coste")</label></h3>
<hr />

@Using Html.BeginForm("Editar", "MetadataYear", FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Código portador")</label>
        <div class="col-sm-4">
            @Html.Label("lblCodigoPortador", String.Format("{0} - ({1})", ccMetadataYear.CodigoPortador, ccMetadataYear.NombreEmpresa))
            @Html.Hidden("hfCodigoPortador", String.Format("{0}-{1}-{2}", ccMetadataYear.Empresa, ccMetadataYear.Planta, ccMetadataYear.CodigoPortador))
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Año")</label>
        <div class="col-sm-4">
            @Html.Label("lblAnyo", CStr(ccMetadataYear.Anyo))
            @Html.Hidden("hfAnyo", ccMetadataYear.Anyo)
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Moneda")</label>
        <div class="col-sm-4">
            @Html.Hidden("hfMonedas", ccMetadataYear.Moneda)
            @Html.DropDownList("Monedas", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Presup. bonos persona")</label>
        <div class="col-sm-4">
            @Html.TextBox("presupBonosPersona", ccMetadataYear.PresupBonosPersona.ToString("N2", culturaEsES), New With {.required = "required", .type = "number", .step = "any", .class = "form-control text-right"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Presup. facturas")</label>
        <div class="col-sm-4">
            @Html.TextBox("presupFacturas", ccMetadataYear.PresupFacturas.ToString("N2", culturaEsES), New With {.required = "required", .type = "number", .step = "any", .class = "form-control text-right"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Presup. viajes")</label>
        <div class="col-sm-4">
            @Html.TextBox("presupViajes", ccMetadataYear.PresupViajes.ToString("N2", culturaEsES), New With {.required = "required", .type = "number", .step = "any", .class = "form-control text-right"})
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
                    @Html.TextBox("importeVentaCliente", ccMetadataYear.ImporteVentaCliente.ToString("N2", culturaEsES), New With {.type = "number", .step = "any", .class = "form-control text-right"})
                </div>
            </div>
        End If
    End Code
    @<div Class="form-group">
        <div class="col-sm-offset-3 col-sm-4">
            <input type="submit" id="submit" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" /><br /><br />
            <a href='@Url.Action("Editar", "Metadata", New With {.Empresa = ccMetadataYear.Empresa, .Planta = ccMetadataYear.Planta, .Codigo = ccMetadataYear.CodigoPortador})'>
                @Utils.Traducir("Ir a cabecera")
            </a>
        </div>
    </div>
        End Using
