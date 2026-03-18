@ModelType IEnumerable(Of Object)
   
<h3>@h.traducir("Elección de proveedor")</h3>
<hr />
<div class="list-group">
    @For Each p In Model
        @<a class="list-group-item" href="@Url.Action("identityChange", h.ToRouteValues(Request.QueryString, New With {.idProveedor = p.idSab}))">
        @If p.informesPendientes > 0 Then
            @<span title="@h.traducir("Todos los informes validados")" Class="glyphicon glyphicon-remove text-danger"></span>
        Else
            @<span title="@h.traducir("Todos los informes validados")" Class="glyphicon glyphicon-ok text-success"></span>
        End If
    @p.nombre
</a>
    Next
</div>