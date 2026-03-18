@imports web
@Code
    ViewBag.title = h.traducir("Asientos mensuales")
End code

<a href="@Url.Action("composicionnomina")">
@h.traducir("Composición de asiento de nomina")
</a>


<strong>@h.traducir("Mes y año para llevar los datos de nomina de RRHH a Navision")</strong>
<br />
@For Each d In Model
    @<a href="@Url.Action("listaplicacion", New With {.year = d.year, .month = d.month})">@CDate(d).tostring("Y")</a>
    @<br/>
Next