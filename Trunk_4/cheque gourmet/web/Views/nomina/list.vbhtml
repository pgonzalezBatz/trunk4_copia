@imports web

@section header
    <title>@h.traducir("Exportar datos para la nomina")</title>
End section

<a target="_blank" style="margin:25px;font-size:1.2em;font-weight:bold;" href="@(Url.Action("list2"))?@Request.querystring.tostring()">@h.traducir("Generar archivo para Epsilon")</a>
<strong>@(Model.count.ToString()) trabajadores totalizando @ViewData("precioTotal")€</strong> <br />
<ul>
    @For Each p In Model
    @<li>
        @p.codtra - @p.nombre @p.apellido1 @p.apellido2  <strong>Precio:</strong>@CDec(p.precio).ToString("C")
    </li>
    Next
</ul>