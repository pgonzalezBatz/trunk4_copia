@imports web
@Code
    ViewBag.title = h.traducir("Crear Tipo Cuenta")
End code

<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Datos de tipo cuenta")</legend>
        <label>
            @h.traducir("Nombre de tipo cuenta")<br />
            @Html.TextBox("nombre")
        </label>
        <br /><br />
        <input type="submit" value="@h.traducir("Guardar")" />
        <a href="@Url.Action("listtipocuenta", New With {.idaplicacion = Request("idaplicacion")})">@h.traducir("Volver")</a>
    </fieldset>
</form>