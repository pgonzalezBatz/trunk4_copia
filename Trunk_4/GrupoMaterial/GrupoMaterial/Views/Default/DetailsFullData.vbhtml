@ModelType GrupoMaterial.DataFull

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>DetailsFullData</title>
</head>
<body>
    <div>
        <h4>DataFull</h4>
        <hr />
        <dl class="dl-horizontal">
            <dt>
                @Html.DisplayNameFor(Function(model) model.Name)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.Name)
            </dd>
    
            @*<dt>
                @Html.DisplayNameFor(Function(model) model.Code)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.Code)
            </dd>*@
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.Subfamily)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.Subfamily)
            </dd>
    
            @*<dt>
                @Html.DisplayNameFor(Function(model) model.SubfamilyId)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.SubfamilyId)
            </dd>*@
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.Family)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.Family)
            </dd>
    
            @*<dt>
                @Html.DisplayNameFor(Function(model) model.FamilyId)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.FamilyId)
            </dd>*@
    
            <dt>
                @Html.DisplayNameFor(Function(model) model.Comodity)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.Comodity)
            </dd>
    
            @*<dt>
                @Html.DisplayNameFor(Function(model) model.ComodityId)
            </dt>
    
            <dd>
                @Html.DisplayFor(Function(model) model.ComodityId)
            </dd>*@
    
        </dl>
    </div>
    @*<p>
        @Html.ActionLink("Edit", "Edit", New With {.id = Model.PrimaryKey}) |
        @Html.ActionLink("Back to List", "Index")
    </p>*@
</body>
</html>
