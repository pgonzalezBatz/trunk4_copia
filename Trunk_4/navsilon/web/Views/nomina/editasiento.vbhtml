@imports web
@Code
    ViewBag.title = h.traducir("Definir Asiento")
End code

@section top
<a href="@Url.Action("index")">@h.traducir("Volver")</a>
End Section

@Html.ValidationSummary()
<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Datos del asiento")</legend>
        <label>
            @h.traducir("Nombre del asiento")<br />
            @Html.TextBox("nombre")
        </label>
        <br /><br />
        <input type="submit" value="@h.traducir("Guardar")" />
        <a href="@Url.Action("listtipocuenta", New With {.idaplicacion = Request("idaplicacion")})">@h.traducir("Volver")</a>
    </fieldset>
</form>
