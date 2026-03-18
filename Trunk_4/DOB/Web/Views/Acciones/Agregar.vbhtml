@Imports DOBLib

<h3>@String.Format("{0} - {1}", Utils.Traducir("Acciones"), Utils.Traducir("Agregar"))</h3>
<hr />

@code
    Dim objetivo As ELL.Objetivo = CType(ViewData("Objetivo"), ELL.Objetivo)
    Dim gradoImportanciaResto = CDec(ViewData("GradoImportancia"))
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
End Code

<script type="text/javascript">
    $(function () {
        $("#fecha").datetimepicker({
            showClear: true,
            locale: '@ticket.Culture',
            format: '@Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper'
        });

        $("form").submit(function () {
            if ($("#fechaObjetivo").val() == "") {
                alert("@Html.Raw(Utils.Traducir("Plazo obligatorio"))");
                return false;
            }
        });
    });
</script>

@Using Html.BeginForm("Agregar", "Acciones", New With {.idObjetivo = objetivo.Id}, FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Objetivo")</label>
        <label class="col-sm-3 control-label" style="text-align:left;">@objetivo.Descripcion</label>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Descripción")</label>
        <div class="col-sm-3">
            @Html.TextArea("descripcion", String.Empty, New With {.maxlength = "200", .rows = "2", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Plazo")</label>
        <div class="col-sm-3">
            <div class="input-group date" id="fecha">
                @Html.TextBox("fechaObjetivo", String.Empty, New With {.class = "form-control"})
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Grado importancia")</label>
        <div class="col-sm-3">
            @Html.TextBox("gradoImportancia", gradoImportanciaResto, New With {.required = "required", .step = "any", .type = "number", .min = "0", .max = "100", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-2 col-sm-3">
            <label class="label label-info">@Utils.Traducir("NotaGuardarObjetivoIntroducirFicheros")</label>
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-2 col-sm-3">
            <input type="submit" id="submit" value="@Utils.Traducir("Añadir")" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>
End Using