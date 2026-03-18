@imports web
@Code
    ViewBag.title = "Añadir nuevo modelo"
End code
@Html.ValidationSummary()
<form action="" method="post">
    @h.Traducir("Nombre del modelo")<br />
    @Html.TextBox("nombre")<br />
    @h.Traducir("Precio")<br />
    @Html.TextBox("precio")<br />
    <input type="submit" value="@h.Traducir("Guardar")" />
</form>