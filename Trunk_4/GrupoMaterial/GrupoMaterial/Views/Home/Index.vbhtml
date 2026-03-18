@Code
    ViewData("Title") = "Home Page"
    Dim DAL = New DataAccessLayer
    Dim list = DAL.GetDataAll()
End Code

<div class="jumbotron" style="padding:10px;margin:10px;">
    <center><h1>Grupos Material</h1></center>
</div>

<div class="row">
    <div class="col-md-2"></div>
    <div class="col-md-4">
        <h3>List</h3>
        <ul>
            <li>@Html.ActionLink("Comodities", "GetComodities", "Default")</li>
            <li>@Html.ActionLink("Families", "GetFamilies", "Default")</li>
            <li>@Html.ActionLink("Subfamilies", "GetSubFamilies", "Default")</li>
            <li>@Html.ActionLink("Elements", "GetElements", "Default")</li>
        </ul>
    </div>
    <div class="col-md-4">
    </div>
    <div class="col-md-2"></div>
</div>
<br />
<div class="row">
    <table id="sortableTable" class="tablesorter">
        <thead>
            <tr>
                <th>
                    @Html.Label("Id")
                </th>
                <th>
                    @Html.Label("Element")
                </th>
                <th>
                    @Html.Label("Subfamily")
                </th>
                <th>
                    @Html.Label("Family")
                </th>
                <th>
                    @Html.Label("Comodity")
                </th>
                <th class="actionColumn">
                    Acciones
                </th>
            </tr>
        </thead>
        <tbody>
            @For Each item In list
                @<tr>
                    <td>
                        @item.Id
                    </td>
                    <td>
                        @item.Name
                    </td>
                    <td>
                        @item.Parent
                    </td>
                    <td>
                        @item.Grandparent
                    </td>
                    <td>
                        @item.Grandgrandparent
                    </td>
                    <td>
                        @Html.ActionLink("Edit", "EditFullElement", "Default", New With {.id = item.Id}, Nothing) |
                        @Html.ActionLink("Details", "DetailsElement", "Default", New With {.id = item.Id}, Nothing) |
                        @Html.ActionLink("Delete", "DeleteElement", "Default", New With {.id = item.Id}, htmlAttributes:=New With {.onclick = "javascript:return deleteItem()"})
                    </td>
                </tr>
            Next
        </tbody>
    </table>
</div>
