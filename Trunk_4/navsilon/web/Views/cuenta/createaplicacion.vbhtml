@imports web
@Code
    ViewBag.title = h.traducir("Crear Aplicación")
End code

<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Datos de la aplicación")</legend>
        <label>
            @h.traducir("Nombre de la aplicación")<br />
            @Html.TextBox("nombre")
        </label>
        <br /><br />
        <input type="submit" value="@h.traducir("Guardar")" />
        <a href="@Url.Action("listaplicacion", New With {.idnegocio = Request("idnegocio"), .idmanoobra = Request("idmanoobra")})">@h.traducir("Volver")</a>
    </fieldset>
</form>