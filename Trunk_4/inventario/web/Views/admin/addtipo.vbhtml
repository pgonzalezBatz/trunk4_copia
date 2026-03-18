@imports web
@Code
    ViewBag.title = "Añadir nuevo tipo"
End code
@Html.ValidationSummary()
<form action="" method="post">
    @h.Traducir("Nombre del tipo")<br />
    @Html.TextBox("nombre")<br />
    <input type="submit" value="@h.Traducir("Guardar")" />
</form>