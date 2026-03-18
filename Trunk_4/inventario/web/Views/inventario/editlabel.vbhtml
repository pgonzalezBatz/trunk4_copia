@imports web
@Code
    ViewBag.title = "Editar etiqueta"
End code

<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Modificar numero de serie y descripción")</legend>
        <strong>@h.traducir("Modelo")</strong><br />
        @Model.nombremodelo<br />
        <strong>@h.traducir("Marca")</strong><br />
        @Model.nombremarca<br />
        <strong>@h.traducir("Tipo")</strong><br />
        @Model.nombretipo<br />
        <strong>@h.traducir("Microsoft OM")</strong><br />
        @Html.TextBox("numeroserie", Model.numeroSerie)
        <br />
        <strong>@h.traducir("Descripción")</strong><br />
        @Html.TextArea("descripcion", Model.descripcion)
        <br />
        <input type="submit" value="@h.traducir("Guardar")" />
    </fieldset>
</form>