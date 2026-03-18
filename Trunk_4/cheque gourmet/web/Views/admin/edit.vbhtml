@ModelType TipoDistribucion
@imports web

@section header
    <title>@h.traducir("Editar")</title>
End section

<form method="post" action="">
    <fieldset>
        @Html.HiddenFor(Function(m) m.IdEmpresa)
        <legend>@h.traducir("Editar tipos de distribución")</legend>
        <strong>@h.traducir("Nombre")</strong><br />
        @Html.TextBoxFor(Function(m) m.Nombre, New With {.readonly = "readonly"})<br />
        <strong>@h.traducir("Precio")</strong><br />
        @Html.TextBoxFor(Function(m) m.Precio, New With {.readonly = "readonly"})<br />
        <strong>@h.traducir("Nº de cheques")</strong><br />
        @Html.TextBoxFor(Function(m) m.NumCheques)<br />
        <strong>@h.traducir("Tipo")</strong><br />
        @*@Html.DropDownListFor(Function(m) m.Grupo, ViewData("tipos"))<br />*@
        @Html.TextBoxFor(Function(m) m.Grupo, New With {.readonly = "readonly"})<br />
        <input type="submit" value="@h.traducir("guardar")" />
    </fieldset>
</form>