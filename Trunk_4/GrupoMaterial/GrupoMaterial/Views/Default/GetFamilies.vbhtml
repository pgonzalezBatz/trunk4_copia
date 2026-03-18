@ModelType IEnumerable(Of GrupoMaterial.Family)

<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>GetFamilies</title>
</head>
<body>
    <p>
        @*@Html.ActionLink("Back to Home", "Index", "Home")*@
        @Html.ActionLink("Create New", "CreateFamily")
    </p>
    <table id="sortableTable" class="tablesorter">
        <thead>
            <tr>
                <th>
                    @Html.DisplayNameFor(Function(model) model.Name)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.Parent)
                </th>
                <th class="actionColumn">Acciones</th>
            </tr>
        </thead>
        <tbody>
            @For Each item In Model
                @<tr>
                    <td>
                        @Html.DisplayFor(Function(modelItem) item.Name)
                    </td>
                    <td>
                        @Html.DisplayFor(Function(modelItem) item.Parent)
                    </td>
                    <td>
                        @Html.ActionLink("Edit", "EditFamily", New With {.id = item.Id}) |
                        @Html.ActionLink("Details", "DetailsFamily", New With {.id = item.Id}) |
                        @Html.ActionLink("Delete", "DeleteFamily", New With {.id = item.Id}, htmlAttributes:=New With {.onclick = "javascript:return deleteItem()"})
                    </td>
                </tr>
            Next
        </tbody>
    </table>
</body>
</html>
