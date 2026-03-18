@imports web
@ModelType Web.TelefonoDirecto
@section title
    - delete
End section

<h2>@h.Traducir("Eliminar") Telefono Directo</h2>

<h3>¿@h.Traducir("Estas segur@ que quieres borrar esto")?</h3>
<fieldset>
    <legend>TelefonoDirecto</legend>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.idEmpresa)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.idEmpresa)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.numero)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.numero)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.empresa)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.empresa)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.numeroProveedor)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.numeroProveedor)
    </div>
</fieldset>
@Using Html.BeginForm()
    @Html.AntiForgeryToken()
			@Html.HiddenFor(Function(model) model.idEmpresa)
			@Html.HiddenFor(Function(model) model.numero)
			@Html.HiddenFor(Function(model) model.empresa)
			@Html.HiddenFor(Function(model) model.numeroProveedor)
	    @<p>
        <input type="submit" value="@h.Traducir("Eliminar")" /> |
        @Html.ActionLink(h.Traducir("Volver al listado"), "List")
    </p>
End Using
