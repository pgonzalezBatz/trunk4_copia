<h3>@h.traducir("Seleccion de Pedido")</h3>
<hr />
<div class="alert alert-info">
    <h4>
        <span class="glyphicon glyphicon-info-sign"></span>
        @h.traducir("En estos momentos tienes mas de un pedido en curso para la OF y OP seleccionada")
    </h4>
    </div>
    <h4>@h.traducir("Especifica el pedido")</h4>
    <ul>
        @For Each p In Model
    @<li>
        @Html.ActionLink(p.numpedlin.ToString, "create2", h.ToRouteValues(Request.QueryString, New With {.numpedlin = p.numpedlin}))
    </li>
        Next
    </ul>
