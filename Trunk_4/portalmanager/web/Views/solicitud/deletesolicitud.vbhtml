@imports web
@Code
    ViewBag.title = "Crear - Editar solicitud becari@"
End Code
@section  header
End Section

<h3>@h.traducir("Eliminar solicitud")</h3>

<strong>@h.traducir("Nº solicitud")</strong>
<br />
@model.id
<br />
<strong>@h.traducir("Nº personas")</strong>
<br />
@Model.npersonas
<br />
<strong>@h.traducir("Responsable")</strong>
<br />
@Model.nombreResponsable @Model.apellido1Responsable
<br />
<strong>@h.traducir("Descripcion")</strong>
<br />
@Model.descripcion

<br />
<br />

<form action="" method="post">
    <strong><a href="@Url.Action("index")">@h.traducir("Volver sin eliminar")</a></strong>
    | <input type="submit" value="@h.traducir("Eliminar")" name="confirm" />
</form>