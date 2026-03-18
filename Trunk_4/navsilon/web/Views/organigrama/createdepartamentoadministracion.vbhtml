@imports web
@Code
    ViewBag.title = h.traducir("Crear Departamento")
End code

<h3>
@ViewData("negocio").nombre @ViewData("manoobra").nombre 
</h3> 
<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Datos del departamento")</legend>
        <label>
            @h.traducir("Nombre del departamento")<br />
            @Html.TextBox("nombre")
        </label>
        <br />
        <label>
            @h.traducir("Lantegi")<br />
            @Html.TextBox("lantegi")
        </label>
        <br />
        <label>
            @h.traducir("Obsoleto")<br />
            @Html.CheckBox("obsoleto")
        </label>
        <br /><br />
        <input type="submit" value="@h.traducir("Guardar")" />
        <a href="@Url.Action("listnegociosadministracion")">@h.traducir("Volver")</a>
        <a href="@Url.Action("deleteDepartamento", New With {.idDepartamento = Request("iddepartamento")})">@h.traducir("Eliminar")</a>
    </fieldset>
</form>