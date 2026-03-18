@imports web
@ModelType IEnumerable(Of Web.TelefonoDirecto)
@section title
    - list
End section

@section menu1
@Html.Partial("menu")
End Section


<h2>@h.Traducir("Listado") Telefono Directo</h2>

<p>
    @Html.ActionLink(h.Traducir("Crear Nuevo Telefono Directo"), "Create")
</p>
@if Model is nothing Then

Else
@<table class="table">
	<thead>
    <tr>
        <th>
			 @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.Traducir("numero"))
        </th>
        <th>
			 @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.Traducir("nombre"))
        </th>
        <th>
			 @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.Traducir("numero Proveedor"))
        </th>
        <th></th>
    </tr>
	</thead>
	<tbody>
@For Each item In Model
    Dim currentItem = item
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.numero)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.empresa)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.numeroProveedor)
        </td>
        <td>
            @Html.ActionLink(h.traducir("Editar"), "Edit", New With {.id = currentItem.idEmpresa}) |
            @Html.ActionLink(h.traducir("Eliminar"), "Delete", New With {.id = currentItem.idEmpresa})
        </td>
    </tr>
Next
</tbody>
</table>
End If
