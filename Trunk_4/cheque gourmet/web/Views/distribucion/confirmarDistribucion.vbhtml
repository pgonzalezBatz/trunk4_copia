@ModelType  DistribucionDesconpuesta
@imports web

@section header
    <title>@h.traducir("Confirmar distribución de cheques gourmet")</title>
End section

<strong>@h.traducir("Confirmar distribución de talonario")</strong>
<br />
@h.traducir("del Nº")
@Html.DisplayFor(Function(m) m.Desde)
<br />
@h.traducir("al Nº")
@Html.DisplayFor(Function(m) m.Hasta)
<br />
@h.traducir("Tipo")
@Html.DisplayFor(Function(m) m.Tipo)
<br />
@h.traducir("Precio")
@Html.DisplayFor(Function(m) m.Precio)
<br />
@h.traducir("Total")
@(Model.Precio * Model.NumeroCheques)
<br />
<hr />
@h.traducir("Nº Trabajador")
@Html.DisplayFor(Function(m) m.IdTrabajador)
<br />
@h.traducir("DNI")
@Html.DisplayFor(Function(m) m.DNI)
<br />
@h.traducir("Nombre")
@Html.DisplayFor(Function(m) m.Nombre)
<br />
@h.traducir("Apellido 1")
@Html.DisplayFor(Function(m) m.Apellido1)
<br />
@h.traducir("Apellido 2")
@Html.DisplayFor(Function(m) m.Apellido2)
<br />
@h.traducir("Email")
@Html.DisplayFor(Function(m) m.Email)
<br />
<form action="@Url.Action("guardardistribucion") " method="post">
    @Html.HiddenFor(Function(m) m.Desde)
    @Html.HiddenFor(Function(m) m.Hasta)
    @Html.HiddenFor(Function(m) m.Tipo)
    @Html.HiddenFor(Function(m) m.Precio)
    @Html.HiddenFor(Function(m) m.NumeroCheques)
    @Html.HiddenFor(Function(m) m.IdTrabajador)
    @Html.HiddenFor(Function(m) m.DNI)
    @Html.HiddenFor(Function(m) m.Nombre)
    @Html.HiddenFor(Function(m) m.Apellido1)
    @Html.HiddenFor(Function(m) m.Apellido2)
    @Html.HiddenFor(Function(m) m.Email)
    <input type="submit" value="@h.traducir("guardar")" />
</form>