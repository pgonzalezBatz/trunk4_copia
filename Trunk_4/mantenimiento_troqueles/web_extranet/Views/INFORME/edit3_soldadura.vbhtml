@ModelType web_extranet.INFORMES

<h3>@h.traducir("Creación de informe")</h3>
<hr />

@Html.ValidationSummary(False, "", New With {.class = "alert alert-danger"})

    <div class="row">
        <div class="col-sm-6">
            <h4>@h.traducir("Datos seleccionados")</h4>
            </div>
        </div>
    <div class="row">
        <div class="col-sm-4">
            <label>@h.traducir("Tipo informe seleccionado")</label>
            <p>@Model.TIPOINFORME</p>
        </div>
        <div class="col-sm-4">
            <label>@h.traducir("Cliente")</label>
            <p>@Model.CLIENTE</p>
        </div>
        <div class="col-sm-4">
            <label>@h.traducir("Proyecto")</label>
            <p>@Model.proyecto</p>
        </div>
        </div>

    <div class="row">
        <div class="col-sm-4">
            <label>@h.traducir("OF - OP seleccionada")</label>
            <p>@Model.valorof - @Model.VALOROP</p>
        </div>
        <div Class="col-sm-4">
            <label>@h.traducir("Nº pieza")</label>
            <p>@Model.NPIEZA</p>
        </div>   
        <div Class="col-sm-4">
            <label>@h.traducir("Descripción pieza")</label>
            <p>@Model.DESCPIEZA</p>
        </div>
       </div>
    <div class="row">
        <div class="col-sm-4">
            <label>@h.traducir("Marcas seleccionada")</label>
            <p>
                @Html.Encode(" | ")
                @For Each m In ViewData("marca")
                    @m.marca @Html.Encode(" | ")
                Next
            </p>
        </div>
        <div Class="col-sm-4">
            <label>@h.traducir("Nº de troquel")</label>
            <p>@Model.NTROQUEL</p>
        </div>
        <div Class="col-sm-4">
            <label>@h.traducir("Material")</label>
            <p>@ViewData("comunesMarca").material</p>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4">
          
        </div>
        <div Class="col-sm-4">
            <label>@h.traducir("Tratamiento Requerido")</label>
            <p>@ViewData("comunesMarca").tratamiento</p>
        </div>
        <div Class="col-sm-4">
            <label>@h.traducir("Dureza Material") <span class="glyphicon glyphicon-info-sign text-info" title="@h.traducir("Es la dureza del material suministrado")"></span></label>
            <p>@ViewData("comunesMarca").dureza</p>
            </div>
    </div>
    <div class="row">
        <div class="col-sm-4">

        </div>
        <div Class="col-sm-4">
            <label>@h.traducir("Tratamiento Secundario")</label>
            <p>@ViewData("comunesMarca").tratamientosecundario</p>
        </div>
        <div Class="col-sm-4">
        </div>
    </div>

<h4>@h.traducir("Datos del informe")</h4>
<form action="@Url.Action("createEditSoldadura", h.ToRouteValues(Request.QueryString, Nothing))" method="post">
    @Html.HiddenFor(Function(m) m.IDINFORME)
    @Html.HiddenFor(Function(m) m.TIPOINFORME)
    @Html.HiddenFor(Function(m) m.VALOROF)
    @Html.HiddenFor(Function(m) m.VALOROP)
    @Html.Hidden("marca", String.Join("|", CType(ViewData("marca"), IEnumerable(Of marca)).Select(Function(m) m.marca)))
    @Html.HiddenFor(Function(m) m.CLIENTE)
    @Html.HiddenFor(Function(m) m.PROYECTO)
    @Html.HiddenFor(Function(m) m.NPIEZA)
    @Html.HiddenFor(Function(m) m.DESCPIEZA)
    @Html.HiddenFor(Function(m) m.NTROQUEL)
    @Html.HiddenFor(Function(m) m.CREADOPOR)
    @Html.Hidden("material", ViewData("comunesMarca").material)
    @Html.Hidden("tratamsec", ViewData("comunesMarca").tratamiento)
    @Html.Hidden("dureza", ViewData("comunesMarca").dureza)
    @Html.Hidden("numpedlin")
        <div Class="row">
            <div Class="col-sm-4">
                <div Class="form-group">
                    <Label>@h.traducir("Notas")</label>
                    @Html.TextAreaFor(Function(m) m.NOTAS, New With {.class = "form-control"})
                </div>
            </div>
            <div class="col-sm-4">
                
            </div>
        </div>
        <h4>@h.traducir("Duro")</h4>
        <div class="row">
            <div class="col-sm-3">
                <div class="form-group">
                    <label>@h.traducir("Material Aportación")</label>
                        <div>
                            @Html.DropDownListFor(Function(m) m.TIPOSOLDADURADURO, CType(ViewData("lsttiposoldaduraduro"), IEnumerable(Of SelectListItem)), "", New With {.class = "form-control"})
                        </div>
                        <div >
                            @Html.DropDownListFor(Function(m) m.MATERIALAPORTACIONSOLDDURO, CType(ViewData("lstmaterialaportacionsoldduro"), IEnumerable(Of SelectListItem)), "", New With {.class = "form-control"})
                        </div>
                    </div>
            </div>
                <div class="col-sm-2">
                    <div class="form-group">
                        <label>@h.traducir("Varilla")</label>
                        @Html.DropDownListFor(Function(m) m.VARILLASOLDADURADURO, CType(ViewData("lstvarillasoldaduraduro"), IEnumerable(Of SelectListItem)), "", New With {.class = "form-control"})
                    </div>
                </div>
               
                <div class="col-sm-1">
                    <div class="form-group">
                        <label>@h.traducir("cantidad")</label>
                        @Html.TextBox("CANTIDADDURO", Nothing, New With {.class = "form-control"})
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="form-group">
                        <label>@h.traducir("Kg. Consumidos")</label>
                        @Html.TextBoxFor(Function(m) m.KGCONSUMIDOSSOLDDURO, New With {.class = "form-control"})
                    </div>
                </div>
            <div class="col-sm-2">
                <div class="form-group">
                    <label>@h.traducir("Intensidad Máquina (A)")</label>
                    @Html.DropDownListFor(Function(m) m.INTENSIDADSOLDADURADURO, CType(ViewData("lstintensidadsoldaduraduro"), IEnumerable(Of SelectListItem)), New With {.class = "form-control"})
                </div>
            </div>
            </div>
    <h4>@h.traducir("Blando")</h4>
    <div class="row">
        <div class="col-sm-3">
            <div class="form-group">
                <label>@h.traducir("Material Aportación")</label>
                <div>
                        @Html.DropDownListFor(Function(m) m.TIPOSOLDADURABLANDO, CType(ViewData("lsttiposoldadurablando"), IEnumerable(Of SelectListItem)), "", New With {.class = "form-control"})
                    
                </div>
                <div>
                    @Html.DropDownListFor(Function(m) m.MATERIALAPORTACIONSOLDBLANDO, CType(ViewData("lstmaterialaportacionsoldblando"), IEnumerable(Of SelectListItem)), "", New With {.class = "form-control"})
                </div>
            </div>
        </div>
        <div class="col-sm-2">
            <div class="form-group">
                <label>@h.traducir("Varilla")</label>
                @Html.DropDownListFor(Function(m) m.VARILLASOLDADURABLANDO, CType(ViewData("lstvarillasoldadurablando"), IEnumerable(Of SelectListItem)), "", New With {.class = "form-control"})
            </div>
        </div>
      
        <div class="col-sm-1">
            <div class="form-group">
                <label>@h.traducir("cantidad")</label>
                
                @Html.TextBox("CANTIDADBLANDO", Nothing, New With {.class = "form-control"})
            </div>
        </div>
        <div class="col-sm-2">
            <div class="form-group">
                <label>@h.traducir("Kg. Consumidos")</label>
                @Html.TextBoxFor(Function(m) m.KGCONSUMIDOSSOLDBLANDO, New With {.class = "form-control"})
            </div>
        </div>
        <div class="col-sm-2">
            <div class="form-group">
                <label>@h.traducir("Intensidad Máquina (A)")</label>
                @Html.DropDownListFor(Function(m) m.INTENSIDADSOLDADURABLANDO, CType(ViewData("lstintensidadsoldadurablando"), IEnumerable(Of SelectListItem)), New With {.class = "form-control"})
            </div>
        </div>
    </div>
        <div class="row">
            <div class="col-sm-2">
                <div class="form-group">
                    @If Model.IDINFORME = 0 Then
                        @<input type="submit" value="@h.traducir("Crear informe")" Class="btn btn-primary" />
                    Else
                        @<input type="submit" value="@h.traducir("Guardar cambios")" Class="btn btn-primary" />
                    End If
                </div>
            </div>
        </div>
</form>
@section scripts
<script type="text/javascript">
    $(function () {
        function populateValoresSoldadura(opcion, tipo, $selected) {
            $.ajax({
                url: "@Url.Action("getValoresSoldadura")",
                data: { tipoSoldadura: this.value, opcion: opcion, tipo: tipo },
            success: function (valores) {
                $selected.html($.map(valores, function (v, i) {
                    return $('<option>', {
                        value: v.VALOR
                    }).html(v.VALOR);
                }));
            }
        });
        };
        function visibilityControl(duro_blando) {
            $('#VARILLASOLDADURA' + duro_blando).attr('disabled', this.value == "No");
            $('#INTENSIDADSOLDADURA' + duro_blando).attr('disabled', this.value == "No");
            $('#CANTIDAD' + duro_blando).attr('disabled', document.getElementById('VARILLASOLDADURA' + duro_blando).value == "");

        };
        $("#TIPOSOLDADURADURO").change(function (evt) {
            if (this.value != "-1") {
                populateValoresSoldadura.apply(this, ["Duro", "Material", $('#MATERIALAPORTACIONSOLDDURO')]);
                populateValoresSoldadura.apply(this, ["Duro", "Varilla", $('#VARILLASOLDADURADURO')]);
                populateValoresSoldadura.apply(this, ["Duro", "Intensidad", $('#INTENSIDADSOLDADURADURO')]);
            }
        });
        $("#TIPOSOLDADURABLANDO").change(function (evt) {
            if (this.value != "-1") {
                populateValoresSoldadura.apply(this, ["Blando", "Material", $('#MATERIALAPORTACIONSOLDBLANDO')]);
                populateValoresSoldadura.apply(this, ["Blando", "Varilla", $('#VARILLASOLDADURABLANDO')]);
                populateValoresSoldadura.apply(this, ["Blando", "Intensidad", $('#INTENSIDADSOLDADURABLANDO')]);
            }
        });
        $('#MATERIALAPORTACIONSOLDDURO').change(function (evt) {
            visibilityControl.apply(this, ['DURO']);
        });
        $('#MATERIALAPORTACIONSOLDBLANDO').change(function (evt) {
            visibilityControl.apply(this, ['BLANDO']);
        });
        $('#VARILLASOLDADURADURO').change(function (evt) {
            visibilityControl.apply(this, ['DURO']);
        });
        $('#VARILLASOLDADURABLANDO').change(function (evt) {
            visibilityControl.apply(this, ['BLANDO']);
        });
        $('#CANTIDADDURO').focusout(function (evt) {
            var kgconsumidos = $('#CANTIDADDURO').val() * 0.025;
            $('#KGCONSUMIDOSSOLDDURO').val(kgconsumidos.toLocaleString('@Globalization.CultureInfo.CurrentCulture.Name'  ))
        })
        $('#CANTIDADBLANDO').focusout(function (evt) {
            var kgconsumidos =$('#CANTIDADBLANDO').val() * 0.025
            $('#KGCONSUMIDOSSOLDBLANDO').val(kgconsumidos.toLocaleString('@Globalization.CultureInfo.CurrentCulture.Name'))
        })
    });
</script>
End Section

