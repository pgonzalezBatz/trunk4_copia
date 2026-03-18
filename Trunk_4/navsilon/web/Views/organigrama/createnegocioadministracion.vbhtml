@imports web
@Code
    ViewBag.title = h.traducir("Crear negocio")
End code

<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Datos del negocio")</legend>
        <label>
            @h.traducir("Nombre del negocio")<br />
            @Html.TextBox("nombre")
        </label>
        <br /><br />
        <input type="submit" value="@h.traducir("Guardar")" />
        <a href="@Url.Action("listnegociosadministracion")">@h.traducir("Volver")</a>
    </fieldset>
</form>