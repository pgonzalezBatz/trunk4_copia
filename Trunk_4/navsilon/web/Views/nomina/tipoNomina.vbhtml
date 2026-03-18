@imports web
@Code
    ViewBag.title = h.traducir("Definir y ejecutar asientos de nomina")
End code


<h3>@h.traducir("Seleccionar tipo de nomina")</h3>
<form action="mesasiento" method="get">
    @Html.Hidden("year")
    @Html.Hidden("month")
    @Html.Hidden("paga")
@For Each m In Model
    @<input type="checkbox" name="idNomina" value="@m.idNomina" checked="checked" />
    @m.descripcion
    @<br/>
Next
<input type="submit" value="@h.traducir("Continuar")" />
</form>
