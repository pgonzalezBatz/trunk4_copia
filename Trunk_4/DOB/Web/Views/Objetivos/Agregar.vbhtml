@Imports DOBLib

<h3>@String.Format("{0} - {1}", Utils.Traducir("Objetivos"), Utils.Traducir("Nuevo"))</h3>
<hr />

@code    
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
                alert("@Html.Raw(Utils.Traducir("Fecha objetivo obligatoria"))");
                return false;
            }            
        });

        $('[data-toggle="popover"]').popover({
            title: '@Html.Raw(Utils.Traducir("Información"))',
            content: '@Html.Raw(Utils.Traducir("Ascendente: cuanto más valor mejor. Si nos acercamos o quedamos por encima del valor objetivo es bueno. Descendente: cuanto menos mejor, Si nos acercamos o quedamos por debajo del valor objetivo es bueno."))',
            trigger: 'focus',            
            placement: 'bottom'
        });        
    });

</script>

@Using Html.BeginForm("Agregar", "Objetivos", FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Descripción")</label>
        <div class="col-sm-6">
            @Html.TextArea("descripcion", String.Empty, New With {.maxlength = "200", .rows = "5", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Reto")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Retos", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Proceso")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Procesos", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Responsable")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Responsables", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Fecha objetivo")</label>
        <div class="col-sm-4">
            <div class="input-group date" id="fecha">
                @Html.TextBox("fechaObjetivo", String.Empty, New With {.class = "form-control"})
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Indicador")</label>
        <div class="col-sm-4">
            @Html.TextBox("indicador", String.Empty, New With {.maxlength = "100", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Sentido")</label>
        <div class="col-sm-4">
            @Html.RadioButton("sentido", 1, True)&nbsp;<label>@Utils.Traducir("Ascendente")</label>
            @Html.RadioButton("sentido", 0, False)&nbsp;<label>@Utils.Traducir("Descendente")</label>
            <a href="#" data-toggle="popover">
                <span class="glyphicon glyphicon glyphicon-info-sign" aria-hidden="true"></span>
            </a>
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Descripción indicador")</label>
        <div class="col-sm-4">
            @Html.TextArea("descripcionIndicador", String.Empty, New With {.maxlength = "200", .rows = "2", .class = "form-control"})
        </div>
    </div>
    @*@<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Ascendente")</label>
        <div class="col-sm-4">
            @Html.CheckBox("ascendente", True)             
        </div>
    </div>*@
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Tipo indicador")</label>
        <div class="col-sm-4">
            @Html.DropDownList("TiposIndicadores", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Valor inicial")</label>
        <div class="col-sm-4">
            @Html.TextBox("valorInicial", String.Empty, New With {.required = "required", .type = "number", .step = "any", .class = "form-control text-right"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Valor objetivo")</label>
        <div class="col-sm-4">
            @Html.TextBox("valorObjetivo", String.Empty, New With {.required = "required", .type = "number", .step = "any", .class = "form-control text-right"})
        </div>
    </div>
    @<div Class="form-group hide">
        <label class="col-sm-2 control-label">@Utils.Traducir("Periodicidad")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Periodicidad", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-2 col-sm-4">
            <label class="label label-info">@Utils.Traducir("NotaGuardarObjetivoIntroducirFicheros")</label>
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-2 col-sm-4">
            <input type="submit" id="submit" value="@Utils.Traducir("Añadir")" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>
End Using
@*<br />
@Html.ActionLink(Utils.Traducir("Volver al listado"), "Index")*@