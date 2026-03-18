@imports web
@Code
    ViewBag.title = h.traducir("Traspaso OK")
End code

<h3>@h.traducir("Se ha importado el asiento a Navision")</h3>
<h3>@h.traducir("Año") @Request("year") @h.traducir("Mes") @Request("month")</h3>
<strong><a href="@Url.Action("index")">@h.traducir("Volver")</a></strong>