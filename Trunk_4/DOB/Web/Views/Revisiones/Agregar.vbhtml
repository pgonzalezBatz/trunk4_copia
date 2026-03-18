@Imports DOBLib

<script type="text/javascript">
    $(function () {
        $("#RevisionesTipo").change(function () {
            var valor = $(this).val();

            $("#paAñoSiguiente").prop('checked', false);
            $("#paComentario").val('');

            if (valor == "@CInt(ELL.Revision.Tipo.Enero)") {
                $("#grupoPAAñoSiguiente").show();
                $("#grupoPAComentario").show();
            }
            else {
                $("#grupoPAAñoSiguiente").hide();
                $("#grupoPAComentario").hide();
            }
        })
    });

</script>

<h3>@String.Format("{0} - {1}", Utils.Traducir("Revisiones"), Utils.Traducir("Nuevo"))</h3>
<hr />

@Using Html.BeginForm("Agregar", "Revisiones", FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Objetivo")</label>
        <div class="col-sm-7">
            @Html.DropDownList("Objetivos", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Revisión")</label>
        <div class="col-sm-2">
            @Html.DropDownList("RevisionesTipo", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-3 control-label">@Utils.Traducir("Comentario")</label>
        <div class="col-sm-7">
            @Html.TextArea("comentario", New With {.maxlength = "500", .class = "form-control"})
        </div>
    </div>
    @<div id="grupoPAAñoSiguiente" Class="form-group" style="display:none;">
        <label class="col-sm-3 control-label">@Utils.Traducir("¿Plan acción año siguiente?")</label>
        <div class="col-sm-7">
            @Html.CheckBox("paAñoSiguiente", False)
        </div>
    </div>
    @<div id="grupoPAComentario" Class="form-group" style="display:none;">
        <label class="col-sm-3 control-label">@Utils.Traducir("Comentario plan acción")</label>
        <div class="col-sm-7">
            @Html.TextArea("paComentario", New With {.maxlength = "500", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-3 col-sm-7">
            <label class="label label-info">@Utils.Traducir("NotaGuardarRevisionIntroducirFicheros")</label>
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-3 col-sm-7">
            <input type="submit" id="submit" value="@Utils.Traducir("Añadir")" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>
End Using