@ModelType GrupoMaterial.SubFamily

@Code
    Dim DAL = New DataAccessLayer
End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>EditSubFamily</title>
</head>
<body>
    @Using (Html.BeginForm("SubmitSubfamily", "Default"))
        @Html.AntiForgeryToken()

        @<div class="form-horizontal">
            <h4>SubFamily</h4>
            <hr />
            @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
            @Html.HiddenFor(Function(model) model.Id)
    
             @*<div class="form-group">
                 @Html.LabelFor(Function(model) model.Id, htmlAttributes:=New With {.class = "control-label col-md-2"})
                 <div class="col-md-10">
                     @Html.EditorFor(Function(model) model.Id, New With {.htmlAttributes = New With {.class = "form-control", .disabled = "disabled"}})
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
                 @Html.LabelFor(Function(model) model.Parent, htmlAttributes:=New With {.class = "control-label col-md-2"})
                 <div class="col-md-10">
                     @Html.DropDownListFor(Function(model) model.ParentId, New SelectList(DAL.GetFamilyAll(), "Id", "Name", Model.ParentId), htmlAttributes:=New With {.class = "form-control", .onchange = "FamiliaChange(this)"})
                     @Html.ValidationMessageFor(Function(model) model.ParentId, "", New With {.class = "text-danger"})
                 </div>
             </div>  
             <div class="form-group">
                 @Html.LabelFor(Function(model) model.Grandparent, htmlAttributes:=New With {.class = "control-label col-md-2"})
                 <div class="col-md-10">
                     @Html.DropDownListFor(Function(model) model.GrandparentId, New SelectList(DAL.GetComodityAll(), "Id", "Name", Model.GrandparentId), htmlAttributes:=New With {.class = "form-control", .onchange = "ComodityChange(this)"})
                     @*@Html.DropDownListFor(Function(model) model.GrandparentId, New SelectList(DAL.GetComodityAll(), "Id", "Name", Model.GrandparentId), htmlAttributes:=New With {.class = "form-control myDisabledClass", .onchange = "ComodityChange(this)", .disabled = "disabled"})*@
                     @Html.ValidationMessageFor(Function(model) model.GrandparentId, "", New With {.class = "text-danger"})
                 </div>
             </div>    
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <input type="submit" value="Save" class="btn btn-default" />
                </div>
            </div>
        </div>
    End Using

    <script type="text/javascript">
        function FamiliaChange(sender) {
            var dd0 = document.getElementById('ParentId');
            var dd1 = document.getElementById('GrandparentId');
            var newcode = dd0.options[dd0.selectedIndex].value;
            $.ajax({
                url: '@Url.Action("GetComodityFromFamilyCode")',
                type: "POST",
                data: { newCode: newcode },
                success: function (response) {
                    for (var i = 0; i < dd1.options.length; i++) {
                            if (dd1.options[i].text == response.Name) {
                            dd1.options[i].selected = true;
                            break;
                        }
                    }
                },
                error: function (response) {
                    alert("error");
                    console.log(response.status + "::" + response.statusText + "::" + response.responseText)
                }
            });
        }

        function ComodityChange(sender) {
            var dd1 = document.getElementById('GrandparentId');
            var newcode = dd1.options[dd1.selectedIndex].value;
            $.ajax({
                url: '@Url.Action("GetFamiliesFromComodityCode")',
                type: "POST",
                data: { newcode: newcode },
                success: function (response) {
                    var families = response
                    $("#Parent").html("");
                    $("#Parent").append($('<option></option>').val(0).html("SELECT ONE:"))
                    for (var i = 0; i < families.length; i++) {
                        $("#Parent").append($('<option></option>').val(families[i].Id).html(families[i].Name))
                    }
                },
                error: function (response) {
                    alert("error");
                    console.log(response.status + "::" + response.statusText + "::" + response.responseText)
                }
            });
        }
    </script>
</body>
</html>
