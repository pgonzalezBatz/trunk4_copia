@ModelType GrupoMaterial.Comodity

<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>EditComodity</title>
</head>
<body>
    @Using (Html.BeginForm("SubmitComodity", "Default"))
        @Html.AntiForgeryToken()

        @<div class="form-horizontal">
            <h4>Comodity</h4>
            <hr />
            @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
            @Html.HiddenFor(Function(model) model.Id)
             @*<div class="form-group">
                 @Html.LabelFor(Function(model) model.Id, htmlAttributes:=New With {.class = "control-label col-md-2"})
                 <div class="col-md-10">
                     @Html.EditorFor(Function(model) model.Id, New With {.htmlAttributes = New With {.class = "form-control myDisabledClass"}})
                     @Html.ValidationMessageFor(Function(model) model.Id, "", New With {.class = "text-danger"})
                 </div>
             </div>*@
            <div class="form-group">
                @Html.LabelFor(Function(model) model.Name, htmlAttributes:=New With {.class = "control-label col-md-2"})
                <div class="col-md-10">
                    @Html.EditorFor(Function(model) model.Name, New With {.htmlAttributes = New With {.class = "form-control"}})
                    @Html.ValidationMessageFor(Function(model) model.Name, "", New With {.class = "text-danger"})
                </div>
            </div>
    
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <input type="submit" value="Save" class="btn btn-default" />
                </div>
            </div>
        </div>
    End Using
    
    @*<div>
        @Html.ActionLink("Back to List", "GetComodities")
    </div>*@
</body>
</html>
