@imports web
@Code
    ViewBag.title = h.traducir("Crear Cuenta")
End code

<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Datos de la cuenta")</legend>
        <label>
            @h.traducir("Nombre de la cuenta")<br />
            @Html.TextBox("nombre")
        </label>
        <br />
        <label>
            @h.traducir("Descripción de la cuenta")<br />
            @Html.TextBox("descripcion")
        </label>
        <br />
        <label>
            @h.traducir("Lantegi") (@h.traducir("En caso de que sea diferente de la estructura del organigrama"))<br />
            @Html.TextBox("lantegi")
        </label>
        <br /><br />
        <input type="submit" value="@h.traducir("Guardar")" />
        <a href="@Url.Action("listcuenta", h.ToRouteValuesDelete(Request.QueryString,""))">@h.traducir("Volver")</a>
    </fieldset>
</form>