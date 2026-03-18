@Imports DOBLib

@code
    Dim planta As ELL.Planta = CType(ViewData("Planta"), ELL.Planta)
End Code

<h3>@String.Format("{0} - {1}", Utils.Traducir("Procesos"), Utils.Traducir("Nuevo"))</h3>
<hr />

@Using Html.BeginForm("Agregar", "Procesos", FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-1 control-label">@Utils.Traducir("Planta")</label>
        <label class="col-sm-3 control-label" style="text-align:left;">@planta.Planta</label>
    </div>
    @<div Class="form-group">
        <label class="col-sm-1 control-label">@Utils.Traducir("Código")</label>
        <div class="col-sm-3">
            @Html.TextBox("codigo", String.Empty, New With {.maxlength = "5", .style = "text-transform:uppercase;", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-1 control-label">@Utils.Traducir("Nombre")</label>
        <div class="col-sm-3">
            @Html.TextArea("nombre", String.Empty, New With {.maxlength = "200", .rows = "2", .required = "required", .class = "form-control"})            
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-1 col-sm-3">
            <input type="submit" id="submit" value="@Utils.Traducir("Añadir")" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>
End Using
@*<br />
@Html.ActionLink(Utils.Traducir("Volver al listado"), "Index")*@