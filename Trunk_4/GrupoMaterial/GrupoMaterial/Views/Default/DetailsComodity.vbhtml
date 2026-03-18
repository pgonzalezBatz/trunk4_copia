@ModelType GrupoMaterial.Comodity

@Code

End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Details</title>
</head>
<body>
    <div>
        <h4>Comodity</h4>
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
    
        </dl>
    </div>
    @*<p>
        @Html.ActionLink("Edit", "EditComodity", New With {.id = Model.Id}) |
        @Html.ActionLink("Back to List", "GetComodities")
    </p>*@
</body>
</html>
