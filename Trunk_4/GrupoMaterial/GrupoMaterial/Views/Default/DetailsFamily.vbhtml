@ModelType GrupoMaterial.Family

@Code

End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>DetailsFamily</title>
</head>
<body>
    <div>
        <h4>Familia</h4>
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
        </dl>
    </div>
    @*<p>
        @Html.ActionLink("Edit", "EditFamily", New With {.id = Model.Id}) |
        @Html.ActionLink("Back to List", "GetFamilies")
    </p>*@
</body>
</html>
