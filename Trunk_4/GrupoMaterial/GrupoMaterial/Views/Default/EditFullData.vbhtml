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
    @Using (Html.BeginForm("SubmitFullData", "Default"))
        @Html.AntiForgeryToken()

        @<div class="form-horizontal">
    @*<h4>Assign to:</h4>
    <label class="radio-inline"><input type="radio" name="optradio" value="new">New Element</label>
    <label class="radio-inline"><input type="radio" name="optradio" value="existing">Existing Element</label>*@
    <hr />

    @Html.Hidden("newElementName", "", New With {.id = "newElementName"})

    @Html.HiddenFor(Function(model) model.GMCode)
    @Html.HiddenFor(Function(model) model.Name)
    @Html.HiddenFor(Function(model) model.SubfamilyId, New With {.id = "newElementSubfamilyId"})
    @Html.HiddenFor(Function(model) model.FamilyId, New With {.id = "newElementFamilyId"})
    @Html.HiddenFor(Function(model) model.ComodityId, New With {.id = "newElementComodityId"})

    @*<div id="creation">
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

        @Html.Hidden("status", "0", New With {.id = "myHiddenField"})
        <div class="form-group">
            @Html.LabelFor(Function(model) model.Name, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10" style="padding:6px 12px">
                @Html.TextBoxFor(Function(model) model.Name, htmlAttributes:=New With {.class = "form-control", .id = "myNewName"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Subfamily, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.DropDownListFor(Function(model) model.SubfamilyId, New SelectList(DAL.GetSubFamilyAll(), "Id", "Name", Model.SubfamilyId), "SELECT ONE:", htmlAttributes:=New With {.Class = "form-control", .onchange = "SubfamiliaChange(this)"})
                @Html.ValidationMessageFor(Function(model) model.Subfamily, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Family, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.DropDownListFor(Function(model) model.FamilyId, New SelectList(DAL.GetFamilyAll(), "Id", "Name", Model.FamilyId), "SELECT ONE:", htmlAttributes:=New With {.class = "form-control", .onchange = "FamiliaChange(this)"})
                @Html.ValidationMessageFor(Function(model) model.Family, "", New With {.class = "text-danger"})
            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(model) model.Comodity, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.DropDownListFor(Function(model) model.ComodityId, New SelectList(DAL.GetComodityAll(), "Id", "Name", Model.ComodityId), "SELECT ONE:", htmlAttributes:=New With {.class = "form-control", .onchange = "ComodityChange(this)"})
                @Html.ValidationMessageFor(Function(model) model.Comodity, "", New With {.class = "text-danger"})
            </div>
        </div>
    </div>*@

    <div id="assignation">
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
        @Html.HiddenFor(Function(model) model.Name)
        <div class="form-group">
            @Html.LabelFor(Function(model) model.Name, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10" style="padding:6px 12px">
                @Html.DropDownListFor(Function(model) model.Name1, New SelectList(DAL.getUnassignedElements(), "Name", "Name", "SELECT ONE:"), "SELECT ONE:", htmlAttributes:=New With {.class = "form-control", .onchange = "ElementChange(this)", .id = "myNewName2"})
            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(model) model.Subfamily, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.DropDownListFor(Function(model) model.SubfamilyId1, New SelectList(DAL.GetSubFamilyAll(), "Id", "Name", ""), "", htmlAttributes:=New With {.Class = "form-control myDisabledClass", .id = "SubfamilyId1", .onmousedown = "(function(e) {e.preventDefault();})(event, this)"})
                @*@Html.DropDownListFor(Function(model) model.SubfamilyId1, New SelectList(DAL.GetSubFamilyAll(), "Id", "Name", ""), "", htmlAttributes:=New With {.Class = "form-control myDisabledClass", .id = "SubfamilyId1", .disabled = "disabled"})*@
                @Html.ValidationMessageFor(Function(model) model.Subfamily1, "", New With {.class = "text-danger"})
            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(model) model.Family, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.DropDownListFor(Function(model) model.FamilyId1, New SelectList(DAL.GetFamilyAll(), "Id", "Name", ""), "", htmlAttributes:=New With {.class = "form-control myDisabledClass", .id = "FamilyId1", .onmousedown = "(function(e) {e.preventDefault();})(event, this)"})
                @*@Html.DropDownListFor(Function(model) model.FamilyId1, New SelectList(DAL.GetFamilyAll(), "Id", "Name", ""), "", htmlAttributes:=New With {.class = "form-control myDisabledClass", .id = "FamilyId1", .disabled = "disabled"})*@
                @Html.ValidationMessageFor(Function(model) model.Family1, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Comodity, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.DropDownListFor(Function(model) model.ComodityId1, New SelectList(DAL.GetComodityAll(), "Id", "Name", ""), "", htmlAttributes:=New With {.class = "form-control myDisabledClass", .id = "ComodityId1", .onmousedown = "(function(e) {e.preventDefault();})(event, this)"})
                @*@Html.DropDownListFor(Function(model) model.ComodityId1, New SelectList(DAL.GetComodityAll(), "Id", "Name", ""), "", htmlAttributes:=New With {.class = "form-control myDisabledClass", .id = "ComodityId1", .disabled = "disabled"})*@
                @Html.ValidationMessageFor(Function(model) model.Comodity1, "", New With {.class = "text-danger"})
            </div>
        </div>
    </div>
    <br />
    <div class="form-group">
        <div class="col-md-offset-2 col-md-10">
            @*<input type="submit" value="Save" class="btn btn-default" onclick="saveName()" />*@
            <input type="submit" value="Save" class="btn btn-default" />
        </div>
    </div>
</div>
    End Using




         <script type="text/javascript">

        @*function SubfamiliaChange(sender) {
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
        }*@



      function ElementChange(sender) {
		  var dd0 = document.getElementById('myNewName2');
		  var dd1 = document.getElementById('SubfamilyId1');
		  var dd2 = document.getElementById('FamilyId1');
		  var dd3 = document.getElementById('ComodityId1');
          var newname = dd0.options[dd0.selectedIndex].text;

          var hf = document.getElementById('newElementName');
          var hf1 = document.getElementById('newElementSubfamilyId');
          var hf2 = document.getElementById('newElementFamilyId');
          var hf3 = document.getElementById('newElementComodityId');

          hf.value = newname;
			$.ajax({
				//url: '../GetDropdownDataFromSubfamilyCode',
				//url: '@Url.Action("GetDropdownDataFromElementCode")',
				url: '@Url.Action("GetDropdownDataFromElementName")',
                type: "POST",
                data: { newname: newname },
				success: function (response) {
                    for (var i = 0; i < dd1.options.length; i++) {
                        if (dd1.options[i].text == response.Subfamily) {
                            dd1.options[i].selected = true;
                            hf1.value = dd1.options[i].value;
                            break;
                        }
                    }
                    for (var i = 0; i < dd2.options.length; i++) {
                        if (dd2.options[i].text == response.Family) {
                            dd2.options[i].selected = true;
                            hf2.value = dd2.options[i].value;
                            break;
                        }
                    }
                    for (var i = 0; i < dd3.options.length; i++) {
                        if (dd3.options[i].text == response.Comodity) {
                            dd3.options[i].selected = true;
                            hf3.value = dd3.options[i].value;
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




			 //$(function () {
				// $('input:radio[name="optradio"]').change(function () {
				//	 var displayPanel1 = $(this).val() == "new";
				//	 var displayPanel2 = $(this).val() == "existing";
				//	 if (displayPanel1) {
				//		 document.getElementById("creation").style.display = "run-in";
				//		 document.getElementById("assignation").style.display = "none";
				//		 document.getElementById("myHiddenField").value = "1";
				//	 } else if (displayPanel2) {
				//		 document.getElementById("creation").style.display = "none";
				//		 document.getElementById("assignation").style.display = "run-in";
				//		 document.getElementById("myHiddenField").value = "2";
				//	 } else {
				//		 document.getElementById("creation").style.display = "none";
				//		 document.getElementById("assignation").style.display = "none";
				//	 }
				// });
			 //});

			 //function saveName() {
				// var myName = document.getElementById("newElementName");
				// var creationName = document.getElementById("myNewName")
				// var assignName = document.getElementById("myNewName2")
				// if (creationName.value == "") {
				//	 for (i = 0; i < assignName.length; i++) {
				//		 if (assignName.options(i).selected === true) {
				//			 myName.value = assignName.options(i).text
				//		 }
				//	 }
				// }
				// else {
				//	 myName.value = creationName.value;
				// }
			 //}

			 //$(document).ready(function () {
				// document.getElementById("creation").style.display = "none"
				// document.getElementById("assignation").style.display = "none"
			 //});
    </script>
</body>
</html>
