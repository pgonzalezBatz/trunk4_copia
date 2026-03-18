@ModelType GrupoMaterial.FullElement

@Code

End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>DetailsElement</title>
</head>
<body>
    <div>
        <h4>FullElement</h4>
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
            @*<dt>
                @Html.DisplayNameFor(Function(model) model.GrandgrandparentId)
            </dt>
            <dd>
                @Html.DisplayFor(Function(model) model.GrandgrandparentId)
            </dd>*@
            <dt>
                @Html.DisplayNameFor(Function(model) model.Grandgrandparent)
            </dt>
            <dd>
                @Html.DisplayFor(Function(model) model.Grandgrandparent)
            </dd>
        </dl>
    </div>
    @*<p>
        @Html.ActionLink("EditCompleto", "EditFullElement", New With {.id = Model.Id}) |
        @Html.ActionLink("Back to List", "GetElements")
    </p>*@
</body>
</html>
