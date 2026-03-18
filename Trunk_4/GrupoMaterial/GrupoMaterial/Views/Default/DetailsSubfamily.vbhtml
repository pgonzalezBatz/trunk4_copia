@ModelType GrupoMaterial.SubFamily

@Code

End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>DetailsSubfamily</title>
</head>
<body>
    <div>
        <h4>SubFamily</h4>
        <hr />
        <dl class="dl-horizontal">
            @*<dt>
                @Html.DisplayNameFor(Function(model) model.Id)
            </dt>    
            <dd>
                @Html.DisplayFor(Function(model) model.Id)
            </dd>*@ 
            <dt>
                @Html.DisplayNameFor(Function(model) model.Name)
            </dt> 
            <dd>
                @Html.DisplayFor(Function(model) model.Name)
            </dd>   
            @*<dt>
                @Html.DisplayNameFor(Function(model) model.ParentId)
            </dt>
            <dd>
                @Html.DisplayFor(Function(model) model.ParentId)
            </dd>*@
            <dt>
                @Html.DisplayNameFor(Function(model) model.Parent)
            </dt>   
            <dd>
                @Html.DisplayFor(Function(model) model.Parent)
            </dd>   
            @*<dt>
                @Html.DisplayNameFor(Function(model) model.GrandparentId)
            </dt>
            <dd>
                @Html.DisplayFor(Function(model) model.GrandparentId)
            </dd>*@
            <dt>
                @Html.DisplayNameFor(Function(model) model.Grandparent)
            </dt>  
            <dd>
                @Html.DisplayFor(Function(model) model.Grandparent)
            </dd>
        </dl>
    </div>
    @*<p>
        @Html.ActionLink("Edit", "EditSubfamily", New With {.id = Model.Id}) |
        @Html.ActionLink("Back to List", "GetSubfamilies")
    </p>*@
</body>
</html>
