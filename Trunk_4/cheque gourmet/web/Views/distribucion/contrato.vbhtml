@ModelType  Distribucion
@imports web

@section header
    <title>@h.traducir("Firma de contrato")</title>
End section


<strong>@h.traducir("Este trabajador no ha firmado el contrato")</strong>
<a href="@Url.Action("contratopdf", New With {.idtrabajador = Model.IdTrabajador})" target="_blank">@h.traducir("Imprimir contrato")</a><br />
<form action="" method="post">
    <fieldset>
        @Html.HiddenFor(Function(d) d.IdTrabajador)
        @Html.HiddenFor(Function(d) d.Lectura1)
        @Html.HiddenFor(Function(d) d.Lectura2)
        @Html.HiddenFor(Function(d) d.IdTipo)

        <input type="checkbox" name="firmado" />
        <strong>@h.traducir("Firmado")</strong>
        <br />
        <input type="submit" value="@h.traducir("guardar")" />
    </fieldset>
</form>