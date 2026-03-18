@ModelType IEnumerable(Of GrupoMaterial.Comodity)

<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>GetComodities</title>
</head>
<body>
    <p>
                @Html.ActionLink("Create New", "CreateComodity")
    </p>
        <table id="sortableTable" class="tablesorter">
            <thead>
                <tr>
                    <th>
                        @Html.DisplayNameFor(Function(model) model.Name)
                    </th>
                    <th class="actionColumn"></th>
                </tr>
            </thead>
            <tbody>
                @For Each item In Model
                    @<tr>
                        <td>
                            @Html.DisplayFor(Function(modelItem) item.Name)
                        </td>
                        <td>
                            @Html.ActionLink("Edit", "EditComodity", New With {.id = item.Id}) |
                            @Html.ActionLink("Details", "DetailsComodity", New With {.id = item.Id}) |
                            @Html.ActionLink("Delete", "DeleteComodity", New With {.id = item.Id}, htmlAttributes:=New With {.onclick = "javascript:return deleteItem()"})
                        </td>
                    </tr>
                Next
            </tbody>
        </table>
</body>
</html>
