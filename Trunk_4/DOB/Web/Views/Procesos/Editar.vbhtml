@Imports DOBLib

@code
    Dim proceso As ELL.Proceso = CType(ViewData("Proceso"), ELL.Proceso)
End Code

<h3>@String.Format("{0} - {1}", Utils.Traducir("Procesos"), Utils.Traducir("Editar"))</h3>
<hr />

@Using Html.BeginForm("Editar", "Procesos", New With {.idProceso = proceso.Id}, FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-1 control-label">@Utils.Traducir("Planta")</label>
        <label class="col-sm-3 control-label" style="text-align:left;">@proceso.Planta</label>
    </div>
    @<div Class="form-group">
        <label class="col-sm-1 control-label">@Utils.Traducir("Código")</label>
        <div class="col-sm-3">
            @Html.TextBox("codigo", proceso.Codigo, New With {.maxlength = "5", .style = "text-transform:uppercase;", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-1 control-label">@Utils.Traducir("Nombre")</label>
        <div class="col-sm-3">
            @Html.TextArea("nombre", proceso.Nombre, New With {.maxlength = "200", .rows = "2", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-1 col-sm-3">
            <input type="submit" id="submit" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>
End Using
@*<br />
@Html.ActionLink(Utils.Traducir("Volver al listado"), "Index")*@