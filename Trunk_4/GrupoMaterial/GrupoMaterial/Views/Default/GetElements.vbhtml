@ModelType IEnumerable(Of GrupoMaterial.DataFull)

<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>GetElements</title>
</head>
<body>
    <p>
        @Html.ActionLink("Create New", "CreateElementFullData")
    </p>
    <table id="sortableTable" class="tablesorter">
        <thead>
            <tr>
                <th>
                    @Html.DisplayNameFor(Function(model) model.Name)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.Subfamily)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.Family)
                </th>
                <th>
                    @Html.DisplayNameFor(Function(model) model.Comodity)
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
                        @Html.DisplayFor(Function(modelItem) item.Subfamily)
                    </td>
                    <td>
                        @Html.DisplayFor(Function(modelItem) item.Family)
                    </td>
                    <td>
                        @Html.DisplayFor(Function(modelItem) item.Comodity)
                    </td>
                    <td>
                        @*@Html.ActionLink("EditCompleto", "EditFullElement", New With {.id = item.Code}) |*@
                        @Html.ActionLink("Edit", "EditElement", New With {.id = item.Code, .name = item.Name}) |
                        @Html.ActionLink("Details", "DetailsElement", New With {.id = item.Code}) |
                        @Html.ActionLink("Delete", "DeleteElement", New With {.id = item.Code, .name = item.Name}, htmlAttributes:=New With {.onclick = "javascript:return deleteItem()"})
                    </td>
                </tr>
            Next
        </tbody>
    </table>
</body>
</html>
