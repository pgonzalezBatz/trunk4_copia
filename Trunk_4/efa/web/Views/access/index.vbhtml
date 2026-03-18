@imports web

@section header
    <title>@h.traducir("Baliabide Fisikoen Kudeaketa")</title>
End section



<form action="" method="post" class="touch login">
    @Html.ValidationSummary()
    <label for="numerotrabajador" class="touch">@h.traducir("Numero de trabajador") </label>
    @Html.TextBox("numerotrabajador")<br /><br />
    <label for="pwd" class="touch">@h.traducir("Contraseña") </label>
    @Html.Password("pwd")<br /><br />
    <input type="submit" value="Continuar" />
</form>