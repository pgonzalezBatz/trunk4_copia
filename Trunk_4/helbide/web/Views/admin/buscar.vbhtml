@imports web

@section title
    @h.traducir("buscar")
End Section


<form method="get" action="">
    <div class="input-group">
        @Html.TextBox("q", Nothing, New With {.autofocus = "", .autocomplete = "off", .spellcheck = "off", .placeholder = h.traducir("Buscar direcciones por dirección, codigo postal o población   "), .class = "form-control"})
        <div class="input-group-btn">
            <button class="btn btn-primary" type="submit"><i class="glyphicon glyphicon-search" ></i></button>
        </div>
    </div>
</form>
<br />
    @If ViewData("listOfHelbide") IsNot Nothing Then
@<table class="table">
    <thead>
        <tr>
            <th>@h.traducir("Calle")</th>
            <th>@h.traducir("Codigo postal")</th>
            <th>@h.traducir("Poblacion")</th>
            <th>@h.traducir("Provincia")</th>
            <th>@h.traducir("Pais")</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
    @For Each e As web.Helbide In ViewData("listOfHelbide")
    @<tr>
        <td>@e.Calle</td>
        <td>@e.CodigoPostal</td>
        <td>@e.Poblacion</td>
        <td>@e.Provincia</td>
        <td>@e.Pais</td>
        <td><a href="@Url.Action("Editar", "admin", New With {.id = e.Id})">@h.traducir("Editar")</a></td>
    </tr>
    Next
    </tbody>
</table>
    End If