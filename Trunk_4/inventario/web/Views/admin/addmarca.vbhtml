@imports web
@Code
    ViewBag.title = "Añadir/Editar marca"
End code
@Html.ValidationSummary()
<form action="" method="post">
    <fieldset>
            <legend>@h.Traducir("Introducir datos para crear o editar marca")</legend>
    @h.Traducir("Nombre de la marca")<br />
    @Html.TextBox("nombre")<br />
    <input type="submit" value="@h.Traducir("Guardar")" />
    </fieldset>
</form>