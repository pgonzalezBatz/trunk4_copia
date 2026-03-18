@imports web
@modeltype VMRecursosUsuarioEmpresaYSAB
@section title
    - @h.Traducir("Recursos")
End section
@section menu1
    @Html.Partial("menu")
End Section

<h3>@h.Traducir("Recursos para la extranet")</h3>
<hr />

<div>
    <a class="btn btn-danger" href="@Url.Action("Renotificar", h.ToRouteValues(Request.QueryString, Nothing))">@h.Traducir("Reestablecer acceso a la extranet")</a>
</div>

@Html.ValidationSummary()
@Html.EditorFor(Function(m) Model)