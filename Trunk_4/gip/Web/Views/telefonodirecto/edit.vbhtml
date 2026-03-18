@imports web
@ModelType Web.TelefonoDirecto

@section title
    - edit
End section
<div class="container-fluid">
    <h3>@h.traducir("Datos de telefonos directos")</h3>
    <form action="" method="post" >
        @Html.AntiForgeryToken()
        @Html.ValidationSummary(True)
        @Html.HiddenFor(Function(m) m.IdEmpresa)
        <div class="form-group">
            <label>@h.traducir("Proveedor")</label>
            @Html.TextBoxFor(Function(model) model.numeroProveedor, New With {.class = "form-control"})
            @Html.ValidationMessageFor(Function(model) model.numeroProveedor)
        </div>
        <div class="form-group">
            <label>@h.traducir("Numero de telefono directo")</label>
            @Html.TextBoxFor(Function(model) model.numero, New With {.class = "form-control"})
            @Html.ValidationMessageFor(Function(model) model.numero)
        </div>
        <div class="row">
            <div class="col-sm-3">
                <input type="submit" value="@h.traducir("Guardar")" class="btn btn-primary btn-block" />
            </div>
            <div class="col-sm-4">
                @Html.ActionLink(h.traducir("Volver al listado"), "list")
            </div>
        </div>
</form>
</div>
