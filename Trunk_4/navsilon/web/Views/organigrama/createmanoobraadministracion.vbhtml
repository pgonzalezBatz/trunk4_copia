@imports web
@Code
    ViewBag.title = h.traducir("Crear Mano de obra")
End code

<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Datos de la mano de obra")</legend>
        <label>
            @h.traducir("Nombre de la mano de obra")<br />
            @Html.TextBox("nombre")
        </label>
        <br /><br />
        <input type="submit" value="@h.traducir("Guardar")" />
        <a href="@Url.Action("listnegociosadministracion")">@h.traducir("Volver")</a>
    </fieldset>
</form>