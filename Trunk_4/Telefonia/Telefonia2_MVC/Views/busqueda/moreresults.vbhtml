@Code
    ViewData("Title") = "index"
End Code
@section header
<link href="//intranet2.batz.es/baliabideorokorrak/bootstrap/css/typeahead.css" rel="stylesheet" />
End Section

<table class="table">
    <thead>
        <tr>
            <th>@h.traducir("Nombre")</th>
            <th>@h.traducir("Apellido1")</th>
            <th>@h.traducir("Apellido2")</th>
        </tr>
    </thead>
    <tbody>
        @For Each e In Model
            @<tr>
        <td><a href="@Url.Action("index", h.ToRouteValues(Request.QueryString, New With {.id = e.id}))">@e.nombre</a></td>
                 <td><a href="@Url.Action("index", h.ToRouteValues(Request.QueryString, New With {.id = e.id}))">@e.apellido1</a></td>
                 <td><a href="@Url.Action("index", h.ToRouteValues(Request.QueryString, New With {.id = e.id}))">@e.apellido2</a></td>
        </tr>
        Next
    </tbody>
</table>
