@ModelType IEnumerable(Of GrupoMaterial.DataFull)

@Code
    'Dim msg = ViewBag.Msg
    'Dim successOrDanger = ViewBag.Status
    'Dim msg = "test OK"
    'Dim successOrDanger = "success"
    Dim myType = ViewBag.Type
    Dim myName = ViewBag.Name
    Dim myVerb = ViewBag.Verb
    Dim myStatus = ViewBag.Status
    Dim i As Integer = 0

    Dim arrPlantas As String() = Nothing
    Dim Plantasconfig As String = ConfigurationManager.AppSettings("Plantas")
    Dim arrPlantasdesc As String() = Nothing
    Dim Plantasconfigdesc As String = ConfigurationManager.AppSettings("Plantasdesc")
    arrPlantas = Split(Plantasconfig, ";")

    arrPlantasdesc = Split(Plantasconfigdesc, ";")


    Dim vigenteList1 As New List(Of SelectListItem)
    vigenteList1.Add(New SelectListItem With {.Value = 0, .Text = "Selecciona el valor", .Selected = True})
    vigenteList1.Add(New SelectListItem With {.Value = 12, .Text = "Texto 1", .Selected = False})
    vigenteList1.Add(New SelectListItem With {.Value = 233, .Text = "Texto 2", .Selected = False})

End Code

@Html.Hidden("myModalShow", myType)

<div class="jumbotron" style="padding:10px;margin:10px;">
    <center><h1>Grupos Material</h1></center>
</div>

<br />

@*@If msg IsNot Nothing AndAlso Not msg.Equals("") Then
        @<h3>@msg
        </h3>
    End If*@

<br />
<br />
<div Class="row">

    <Table Class="table table-bordered">
        <tr id="myRow">
            @*<th style="background:#ddd">
                    @Html.Label("Code")
                </th>*@

            <th style="background:#7da7ca;">
                @Html.Label("ELEMENT")
            </th>
            <th style="background:#7da7ca;">
                @Html.Label("SUBFAMILY")
            </th>
            <th style="background:#7da7ca;">
                @Html.Label("FAMILY")
            </th>
            <th style="background:#7da7ca;min-width:200px;">
                @Html.Label("COMODITY")
            </th>
            <th style="background-image: linear-gradient(to right,#f95,#7da7ca);min-width:200px;">
                @Html.Label("CRITICIDAD")
            </th>

            @For i = 0 To arrPlantasdesc.Length - 1
                @<th class="actionColumn" style="min-width:10px; background-image: linear-gradient(to right,#f95,#7da7ca);">
                    @Html.Label(arrPlantasdesc(i))
                </th>
            Next

            <th class="actionColumn" style="background-image: linear-gradient(to right,#f95,#7da7ca);">
                @Html.Label("ASIGNACIÓN")
            </th>
        </tr>





        @For Each item In Model
            @<tr>


                <td>

                    @Html.DisplayFor(Function(modelItem) item.Name)

                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.Subfamily)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.Family)
                </td>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.Comodity)
                </td>





                @If arrPlantas.Length > 0 Then

                    @<td>
                        @*@Html.DropDownList(arrPlantas(0), Nothing, New With {.maxlength = 10, .Class = "form-control"})*@


                        @Html.DropDownList("VIGENTE1", vigenteList1, htmlAttributes:=New With {.class = "form-control"})

                    </td>



                    @<td>
                        @Html.TextBox(arrPlantas(0), Nothing, New With {.maxlength = 10, .Class = "form-control"})

                    </td>

                End If

                @If arrPlantas.Length > 1 Then

                    @<td>
                        @Html.TextBox("planta1", arrPlantas(1), New With {.maxlength = 10, .Class = "form-control"})

                    </td>

                End If

                @If arrPlantas.Length > 2 Then

                    @<td>
                        @Html.TextBox("planta2", arrPlantas(2), New With {.maxlength = 10, .Class = "form-control"})

                    </td>

                End If






                <td style="text-align:center">


                    @If item.GMCode.Trim.Equals("") OrElse item.Code.Trim.Equals("") Then
                        @Html.ActionLink(" ", "EditFullData2", "Default", New With {.id = item.GMCode, .name = item.Name}, New With {.class = "glyphicon glyphicon-edit", .title = "Homologar"})
                        @*End If
                            @If Not item.SubfamilyId.Trim.Equals("") OrElse Not item.FamilyId.Trim.Equals("") OrElse Not item.ComodityId.Trim.Equals("") Then
                                @Html.ActionLink(" ", "UnAssign", "Default", New With {.id = item.GMCode}, New With {.class = "glyphicon glyphicon-edit strikeout", .title = "Unassign", .onclick = "return confirm('Are you sure you want to unassign this element to the hierarchy?');", .style = "margin-left:10px"})*@
                    Else

                        @Html.ActionLink("Homologar", "Homologar", "Default", New With {.id = "x"}, New With {.class = "glyphicon glyphicon-edit", .title = "Asignar", .onclick = "return confirm('Are you sure you want to Assign this element?');"})

                    End If


                </td>





            </tr>
        Next
    </Table>




    @If myType IsNot Nothing AndAlso Not myType.Equals("") Then

        @<!-- Modal -->
        @<div id="myModal" Class="modal fade" role="dialog">
            <div Class="modal-dialog">
                <!-- Modal content-->
                <div Class="modal-content">
                    <div Class="modal-body">
                        <div Class="alert alert-@myStatus">
                            @myType <a href="#" class="alert-link">@myName</a> @myVerb
                        </div>
                        @*</div>
                            <div Class="modal-footer">*@
                        <div style="text-align:right">
                            <input type="button" Class="btn btn-default" data-dismiss="modal" value="Ok" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

    End If

</div>

@*<input type="button" id="button1" value="Button" />*@

<script type="text/javascript">
    window.onload = function () {
        var myModalShow = document.getElementById("myModalShow");
        if (myModalShow.value != "") {
            var myModal = $(document.getElementById('myModal'));
            myModal.modal('show');
        }
    };
</script>



<script type="text/javascript">


    $(function () {
        $("#submit").click(function () {

            //debugger;
            if (($("input#campo1").val() == "")) {
                //alert("falta");
                $("#campo1").val("Rellena campo criticidad");
                return false;
            };
            if (($("input#campo2").val() == "")) {
                //alert("falta");
                $("#campo2").val("Rellena campo descripción criticidad");
                return false;
            };
            var campo1 = $("input#campo1").val();
            var campo2 = $("input#campo2").val();
            var dataString = 'campo1=' + campo1 + '&campo2=' + campo2;




            $.ajax({
                data: dataString,
                url: "/Home/AddCriticidad",
                type: "POST",
                dataType: 'JSON',
                success: function (response) {

                }
            })
            location.reload();
            return true;

        });
    });



</script>


