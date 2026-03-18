@ModelType GrupoMaterial.FullElement

@Code
    Dim DAL = New DataAccessLayer
End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>EditFullElement</title>
</head>
<body>
    @Using (Html.BeginForm("SubmitFullElement", "Default"))
        @Html.AntiForgeryToken()

        @<div class="form-horizontal">
            <h4>FullElement</h4>
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
             @Html.HiddenFor(Function(model) model.ParentId)
            <div class="form-group">
                 @Html.LabelFor(Function(model) model.Parent, htmlAttributes:=New With {.class = "control-label col-md-2"})
                 <div class="col-md-10">
                     @Html.DropDownListFor(Function(model) model.Parent, New SelectList(DAL.GetSubFamilyAll(), "Id", "Name", Model.Parent), htmlAttributes:=New With {.class = "form-control", .onchange = "SubfamiliaChange(this)"})
                     @Html.ValidationMessageFor(Function(model) model.Parent, "", New With {.class = "text-danger"})
                 </div>
            </div>
             @Html.HiddenFor(Function(model) model.GrandparentId)
            <div class="form-group">
                @Html.LabelFor(Function(model) model.Grandparent, htmlAttributes:=New With {.class = "control-label col-md-2"})
                <div class="col-md-10">
                    @Html.DropDownListFor(Function(model) model.Grandparent, New SelectList(DAL.GetFamilyAll(), "Id", "Name", Model.Grandparent), htmlAttributes:=New With {.class = "form-control", .onchange = "FamiliaChange(this)"})
                    @Html.ValidationMessageFor(Function(model) model.Grandparent, "", New With {.class = "text-danger"})
                </div>
            </div>
             @Html.HiddenFor(Function(model) model.GrandgrandparentId)    
            <div class="form-group">
                @Html.LabelFor(Function(model) model.Grandgrandparent, htmlAttributes:=New With {.class = "control-label col-md-2"})
                <div class="col-md-10">
                    @Html.DropDownListFor(Function(model) model.Grandgrandparent, New SelectList(DAL.GetComodityAll(), "Id", "Name", Model.Grandgrandparent), htmlAttributes:=New With {.class = "form-control", .onchange = "ComodityChange(this)"})
                    @Html.ValidationMessageFor(Function(model) model.Grandgrandparent, "", New With {.class = "text-danger"})
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
        @Html.ActionLink("Back to List", "GetElements", "Default")
    </div>*@

    <script type="text/javascript">

        function SubfamiliaChange(sender) {
            var dd1 = document.getElementById('Parent');
            var dd2 = document.getElementById('Grandparent');
            var dd3 = document.getElementById('Grandgrandparent');
            var newcode = dd1.options[dd1.selectedIndex].value;
            // IMPORTANTES estas comillas para crear un string y que no haga la conversión octal-decimal
            //var ide = '@ViewData("IdElemento")'
            $.ajax({
                url: '@Url.Action("GetDropdownDataFromSubfamilyCode")',
                type: "POST",
                data: { newcode: newcode },
                success: function (response) {
                    for (var i = 0; i < dd2.options.length; i++) {
                        if (dd2.options[i].text == response.Grandparent) {
                            dd2.options[i].selected = true;
                            break;
                        }
                    }
                    for (var i = 0; i < dd3.options.length; i++) {
                        if (dd3.options[i].text == response.Grandgrandparent) {
                            dd3.options[i].selected = true;
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
            var dd0 = document.getElementById('Grandgrandparent');
            var dd1 = document.getElementById('Grandparent');
            var dd2 = document.getElementById('Parent');
            var desc = dd1.options[dd1.selectedIndex].text;
            $.ajax({
                url: '@Url.Action("GetSubfamiliesFromFamilyName")',
                type: "POST",
                data: { familyName: desc },
                success: function (response) {
                    $("#Parent").html("");
                    $("#Parent").append($('<option></option>').val(0).html("SELECT ONE:"))
                    for (var i = 0; i < response.length; i++) {
                        $("#Parent").append($('<option></option>').val(response[i].Id).html(response[i].Name))
                    }



                    /***/

                    $.ajax({
                        url: '@Url.Action("GetComodityFromFamilyName")',
                        type: "POST",
                        data: { desc: desc },
                        success: function (response) {
                            for (var i = 0; i < dd0.options.length; i++) {
                                if (dd0.options[i].value == response.Name) {
                                    dd0.options[i].selected = true;
                                    return;
                                }
                            }
                        },
                        error: function (response) {
                            alert("error");
                            console.log(response.status + "::" + response.statusText + "::" + response.responseText)
                        }
                    });

                    /***/





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
