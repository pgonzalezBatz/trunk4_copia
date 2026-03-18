@imports web
@ModelType IEnumerable(Of Web.proveedor)
@section title
    - @h.traducir("Busqueda")
End section
@section menu1
    @Html.Partial("menu")
End Section
<form action="" method="get">
    <div class="container">
        <div Class="row">
            <div Class="col-sm-12">
                <h4>@Html.ActionLink(h.traducir("Crear Nuevo proveedor"), "Create")</h4>
            </div>
        </div>
        <div class="row">
            <div Class="col-sm-12">
                <h4>@h.traducir("Buscar proveedor potencial")</h4>
                </div>
            </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="input-group">
                    @Html.TextBox("q", Nothing, New With {.autofocus = "", .class = "form-control"})
                    <div class="input-group-btn">
                        <button class="btn btn-primary" type="submit"><i class="glyphicon glyphicon-search"></i></button>
                    </div>
                </div>
                </div>

        </div>
    </div>
</form>
@If Model Is Nothing Then

Else
    @ViewBag.count @Html.Encode(" ")  @h.traducir("Resultados")
@<table class="table">
	<thead>
    <tr>
        <th>
            @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.traducir("Código"))
        </th>
        <th>
			 @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.Traducir("nombre"))
        </th>
        <th>
			 @System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(h.Traducir("cif"))
        </th>
        <th></th>
    </tr>
	</thead>
	<tbody>
@For Each item In Model
        Dim currentItem = item
            @<tr>
                 <td>
                     <strong>@currentItem.id</strong>
                 </td>
        <td>
            <strong>@currentItem.nombre</strong>
        </td>
        <td>
            @currentItem.cif
        </td>
        <td>
            @Html.ActionLink(h.traducir("Importar"), "create", h.ToRouteValuesDelete(h.ToRouteValues(Request.QueryString, currentItem), "codpro")) 
        </td>
    </tr>
Next
</tbody>
</table>
End If
@code
    Dim take = If(Request("take"), proveedorController.TakeLimit)
    Dim skip = If(Request("skip"), proveedorController.skipMin)
End Code
@If skip > proveedorController.skipMin Then
    @<a href="@Url.Action("search", h.ToRouteValues(Request.QueryString, New With {.skip = skip - take}))">@h.traducir("Menos")</a>
End If
@If ViewBag.count > take Then
    @<a href="@Url.Action("search", h.ToRouteValues(Request.QueryString, New With {.skip = skip + take}))">@h.traducir("Mas")</a>
End If