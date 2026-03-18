@imports web
@section title
    - @h.traducir("Recursos")
End section
@section menu1
    @Html.Partial("menu")
End Section

<h3>@h.traducir("Volver a notificar al proveedor el acceso a la Extranet")</h3>
<hr />

<div class="row">
    <div class="col-xs-2">
        <a class="btn btn-primary" href="@Url.Action("listrecursos", h.ToRouteValues(Request.QueryString, Nothing))">@h.Traducir("Volver")</a>
    </div>
    <div class="col-xs-1">
        <form action="@Url.Action("RenotificarPost", h.ToRouteValues(Request.QueryString, Nothing))" method="post">
            <input class="btn btn-danger" type="submit" value="@h.Traducir("Cambiar contraeña y Notificar")" />
        </form>
    </div>
</div>