@ModelType GrupoMaterial.FullElement

@Code
    Dim DAL = New DataAccessLayer
End Code

<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>CreateFullElement</title>
</head>
<body>
    @Using (Html.BeginForm("CreateFullElement", "Default"))
        @Html.AntiForgeryToken()

        @<div class="form-horizontal">
            <h4>FullElement</h4>
            <hr />
            @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
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
                    @Html.DropDownListFor(Function(model) model.Parent, New SelectList(DAL.GetSubFamilyAll(), "Id", "Name", "SELECT ONE:"), "SELECT ONE:", htmlAttributes:=New With {.class = "form-control", .onchange = "SubfamiliaChange(this)"})
                    @Html.ValidationMessageFor(Function(model) model.Parent, "", New With {.class = "text-danger"})
                </div>
            </div>
            <div class="form-group">
                @Html.LabelFor(Function(model) model.Grandparent, htmlAttributes:=New With {.class = "control-label col-md-2"})
                <div class="col-md-10">
                    @Html.DropDownListFor(Function(model) model.Grandparent, New SelectList(DAL.GetFamilyAll(), "Id", "Name", "SELECT ONE:"), "SELECT ONE:", htmlAttributes:=New With {.class = "form-control", .onchange = "FamiliaChange(this)"})
                    @Html.ValidationMessageFor(Function(model) model.Grandparent, "", New With {.class = "text-danger"})
                </div>
            </div>
            <div class="form-group">
                @Html.LabelFor(Function(model) model.Grandgrandparent, htmlAttributes:=New With {.class = "control-label col-md-2"})
                <div class="col-md-10">
                    @Html.DropDownListFor(Function(model) model.Grandgrandparent, New SelectList(DAL.GetComodityAll(), "Id", "Name", "SELECT ONE:"), "SELECT ONE:", htmlAttributes:=New With {.class = "form-control", .onchange = "ComodityChange(this)"})
                    @Html.ValidationMessageFor(Function(model) model.Grandgrandparent, "", New With {.class = "text-danger"})
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <input type="submit" value="Create" class="btn btn-default" />
                </div>
            </div>
        </div>
    End Using
    
    @*<div>
        @Html.ActionLink("Back to List", "Index")
    </div>*@

    <script type="text/javascript">

        function SubfamiliaChange(sender) {
            var dd0 = document.getElementById('Parent');
            var dd1 = document.getElementById('Grandparent');
            var dd2 = document.getElementById('Grandgrandparent');
            var newcode = dd0.options[dd0.selectedIndex].value;
            $.ajax({
                url: '@Url.Action("GetDropdownDataFromSubfamilyCode")',
                type: "POST",
                data: { newcode: newcode },
                success: function (response) {
                    for (var i = 0; i < dd1.options.length; i++) {
                        if (dd1.options[i].text == response.Grandparent) {
                            dd1.options[i].selected = true;
                            break;
                        }
                    }
                    for (var i = 0; i < dd2.options.length; i++) {
                        if (dd2.options[i].text == response.Grandgrandparent) {
                            dd2.options[i].selected = true;
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

        function FamiliaChange(sender) {
            var dd0 = document.getElementById('Parent');
            var dd1 = document.getElementById('Grandparent');
            var dd2 = document.getElementById('Grandgrandparent')
            var newcode = dd1.options[dd1.selectedIndex].value;
            $.ajax({
                url: '@Url.Action("GetDataFromFamilyCode")',
                type: "POST",
                data: { newCode: newcode },
                success: function (response) {
                    for (var i = 0; i < dd2.options.length; i++) {
                        if (dd2.options[i].text == response.FamilyList[0].Name) {
                            dd2.options[i].selected = true;
                            break;
                        }
                    }
                    var subfamilies = response.SubfamilyList
                    $("#Parent").html("");
                    $("#Parent").append($('<option></option>').val(0).html("SELECT ONE:"))
                    for (var i = 0; i < subfamilies.length; i++) {
                        $("#Parent").append($('<option></option>').val(subfamilies[i].Id).html(subfamilies[i].Name))
                    }
                },
                error: function (response) {
                    alert("error");
                    console.log(response.status + "::" + response.statusText + "::" + response.responseText)
                }
            });
        }

        function ComodityChange(sender) {
            var dd0 = document.getElementById('Parent');
            var dd1 = document.getElementById('Grandparent');
            var dd2 = document.getElementById('Grandgrandparent')
            var newcode = dd2.options[dd2.selectedIndex].value;
            $.ajax({
                url: '@Url.Action("GetDataFromComodityCode")',
                type: "POST",
                data: { newcode: newcode },
                success: function (response) {
                    var families = response.FamilyList
                    var subfamilies = response.SubfamilyList
                    $("#Parent").html("");
                    $("#Parent").append($('<option></option>').val(0).html("SELECT ONE:"))
                    for (var i = 0; i < subfamilies.length; i++) {
                        $("#Parent").append($('<option></option>').val(subfamilies[i].Id).html(subfamilies[i].Name))
                    }
                    $("#Grandparent").html("");
                    $("#Grandparent").append($('<option></option>').val(0).html("SELECT ONE:"))
                    for (var i = 0; i < families.length; i++) {
                        $("#Grandparent").append($('<option></option>').val(families[i].Id).html(families[i].Name))
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
