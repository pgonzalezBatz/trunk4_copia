@ModelType GrupoMaterial.DataFull

@Code
    Dim DAL = New DataAccessLayer
End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>EditFullData</title>
</head>
<body>
    @Using (Html.BeginForm("CreateElementFullData", "Default"))
        @Html.AntiForgeryToken()

        @<div class="form-horizontal">
            <h4>DataFull</h4>
            <hr />
            @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
            @Html.HiddenFor(Function(model) model.GMCode)
            @Html.HiddenFor(Function(model) model.GMName)
            <div class="form-group">
                @Html.LabelFor(Function(model) model.Name, htmlAttributes:=New With {.class = "control-label col-md-2"})
                <div class="col-md-10" style="padding:6px 12px">
                    @Html.TextBoxFor(Function(model) model.Name, htmlAttributes:=New With {.class = "form-control"})
                </div>
            </div>
            <div class="form-group">
                @Html.LabelFor(Function(model) model.Subfamily, htmlAttributes:=New With {.class = "control-label col-md-2"})
                <div class="col-md-10">
                    @Html.DropDownListFor(Function(model) model.SubfamilyId, New SelectList(DAL.GetSubFamilyAll(), "Id", "Name", "SELECT ONE:"), "SELECT ONE:", htmlAttributes:=New With {.Class = "form-control", .onchange = "SubfamiliaChange(this)"})
                    @Html.ValidationMessageFor(Function(model) model.SubfamilyId, "", New With {.class = "text-danger"})
                </div>
            </div>
			<div class="form-group">
				@Html.LabelFor(Function(model) model.Family, htmlAttributes:=New With {.class = "control-label col-md-2"})
				<div class="col-md-10">
					@Html.DropDownListFor(Function(model) model.FamilyId, New SelectList(DAL.GetFamilyAll(), "Id", "Name", "SELECT ONE:"), "SELECT ONE:", htmlAttributes:=New With {.class = "form-control", .onchange = "FamiliaChange(this)"})
					@Html.ValidationMessageFor(Function(model) model.FamilyId, "", New With {.class = "text-danger"})
				</div>
			</div>
            <div class="form-group">
                @Html.LabelFor(Function(model) model.Comodity, htmlAttributes:=New With {.class = "control-label col-md-2"})
                <div class="col-md-10">
                    @Html.DropDownListFor(Function(model) model.ComodityId, New SelectList(DAL.GetComodityAll(), "Id", "Name", "SELECT ONE:"), "SELECT ONE:", htmlAttributes:=New With {.class = "form-control", .onchange = "ComodityChange(this)"})
                    @Html.ValidationMessageFor(Function(model) model.Comodity, "", New With {.class = "text-danger"})
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

        function SubfamiliaChange(sender) {
			var dd1 = document.getElementById('SubfamilyId');
			var dd2 = document.getElementById('FamilyId');
			var dd3 = document.getElementById('ComodityId');
			var newcode = dd1.options[dd1.selectedIndex].value;
            // IMPORTANTES estas comillas para crear un string y que no haga la conversión octal-decimal
            //var ide = '@ViewData("IdElemento")'
			$.ajax({
				//url: '../GetDropdownDataFromSubfamilyCode',
				url: '@Url.Action("GetDropdownDataFromSubfamilyCode")',
                type: "POST",
                data: { newcode: newcode },
                success: function (response) {
					for (var i = 0; i < dd2.options.length; i++) {
                        if (dd2.options[i].text == response.Family) {
                            dd2.options[i].selected = true;
                            break;
                        }
                    }
                    for (var i = 0; i < dd3.options.length; i++) {
                        if (dd3.options[i].text == response.Comodity) {
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
            var dd0 = document.getElementById('ComodityId');
            var dd1 = document.getElementById('FamilyId');
            var dd2 = document.getElementById('SubfamilyId');
            var desc = dd1.options[dd1.selectedIndex].text;
            $.ajax({
                url: '@Url.Action("GetSubfamiliesFromFamilyName")',
                type: "POST",
                data: { familyName: desc },
                success: function (response) {
                    $("#SubfamilyId").html("");
                    $("#SubfamilyId").append($('<option></option>').val(0).html("SELECT ONE:"))
                    for (var i = 0; i < response.length; i++) {
                        $("#SubfamilyId").append($('<option></option>').val(response[i].Id).html(response[i].Name))
                    }

                    /***/

                    $.ajax({
                        url: '@Url.Action("GetComodityFromFamilyName")',
                        type: "POST",
                        data: { desc: desc },
						success: function (response) {
                            for (var i = 0; i < dd0.options.length; i++) {
                                if (dd0.options[i].text == response.Name) {
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
            var dd0 = document.getElementById('SubfamilyId');
            var dd1 = document.getElementById('FamilyId');
            var dd2 = document.getElementById('ComodityId')
			var newcode = dd2.options[dd2.selectedIndex].value;
            $.ajax({
                url: '@Url.Action("GetDataFromComodityCode")',
                type: "POST",
                data: { newcode: newcode },
                success: function (response) {
                    var families = response.FamilyList
                    var subfamilies = response.SubfamilyList
                    $("#SubfamilyId").html("");
                    $("#SubfamilyId").append($('<option></option>').val(0).html("SELECT ONE:"))
                    for (var i = 0; i < subfamilies.length; i++) {
                        $("#SubfamilyId").append($('<option></option>').val(subfamilies[i].Id).html(subfamilies[i].Name))
                    }
                    $("#FamilyId").html("");
                    $("#FamilyId").append($('<option></option>').val(0).html("SELECT ONE:"))
                    for (var i = 0; i < families.length; i++) {
                        $("#FamilyId").append($('<option></option>').val(families[i].Id).html(families[i].Name))
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
