@ModelType IEnumerable(Of GrupoMaterial.DataFull)

@Code
    'Dim DAL = New DataAccessLayer()
    Dim DAL As New OracleDataAccess
    'Dim msg = ViewBag.Msg
    'Dim successOrDanger = ViewBag.Status
    'Dim msg = "test OK"
    'Dim successOrDanger = "success"
    Dim myType = ViewBag.Type
    Dim myName = ViewBag.Name
    Dim myVerb = ViewBag.Verb
    Dim myStatus = ViewBag.Status
    Dim i As Integer = 0
    Dim j As Integer = 0

    Dim tmp As List(Of Criticidad)

    Dim numero As Integer
    Dim identificador = ViewData("identificador")
    Dim Plantasconfig As String = ConfigurationManager.AppSettings("Plantas")
    Dim arrPlantas As String() = Nothing
    Dim arrPlantasdesc As String() = Nothing
    Dim arrPlantasValores As String() = Nothing

    'Dim Plantasconfigdesc As String = ConfigurationManager.AppSettings("Plantasdesc")
    arrPlantas = Split(Plantasconfig, ",")
    numero = arrPlantas.Length
    arrPlantasdesc = Split(Plantasconfig, ",")
    arrPlantasValores = Split(Plantasconfig, ",")
    For j = 0 To arrPlantas.Length - 1
        arrPlantasdesc(j) = DAL.Valoresplantatxt(arrPlantas(j))
    Next


    'arrPlantasdesc = Split(Plantasconfigdesc, ",")





    Dim vigenteList1 As New List(Of SelectListItem)
    vigenteList1.Add(New SelectListItem With {.Value = 0, .Text = "Select value", .Selected = True})

    'rellenar las lineas
    tmp = DAL.getCriticidades()
    For j = 0 To tmp.Count - 1
        vigenteList1.Add(New SelectListItem With {.Value = tmp(j).Code, .Text = tmp(j).desc, .Selected = False})
    Next

    'vigenteList1.Add(New SelectListItem With {.Value = 12, .Text = "Texto 1", .Selected = False})
    'vigenteList1.Add(New SelectListItem With {.Value = 233, .Text = "Texto 2", .Selected = False})



End Code



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


@Html.TextBox("textoaviso", "", New With {.style = "border: transparent;float:right; text-align:center; font-size:22px; background-color: white; color:red;"})
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
                @Html.Label("CRITICITY")
            </th>

            @For i = 0 To arrPlantasdesc.Length - 1
                @<th class="actionColumn" style="min-width:10px; background-image: linear-gradient(to right,#f95,#7da7ca);">
                    @Html.Label(arrPlantasdesc(i))
                </th>
            Next

            <th class="actionColumn" style="background-image: linear-gradient(to right,#f95,#7da7ca);">
                @Html.Label("ALLOCATION")
            </th>
        </tr>




        @For Each item In Model

            For j = 0 To arrPlantas.Length - 1
                If IsNumeric(CInt(item.code)) Then

                    tmp = DAL.Valoresplantaint(item.Code, CInt(arrPlantas(j)))

                    If tmp.Count > 0 Then
                        arrPlantasValores(j) = tmp(0).Code
                    Else
                        arrPlantasValores(j) = ""
                    End If
                Else
                    arrPlantasValores(j) = ""
                End If
            Next
            'buscar el seleccionado
            For j = 1 To vigenteList1.Count - 1

                tmp = DAL.Seleccionado(item.Code, vigenteList1(j).Value)

                If tmp.Count > 0 Then
                    vigenteList1(j).Selected = True

                End If
            Next

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



                <td>
                    @*@Html.DropDownList(arrPlantas(0), Nothing, New With {.maxlength = 10, .Class = "form-control"})*@


                    @Html.DropDownList("VIGENTE1", vigenteList1, htmlAttributes:=New With {.class = "form-control"})

                </td>




                <td>
                    @Html.TextBox("planta0", arrPlantasValores(0), Nothing, New With {.maxlength = 10, .Class = "form-control"})

                </td>



                <td>
                    @Html.TextBox("planta1", arrPlantasValores(1), New With {.maxlength = 10, .Class = "form-control"})

                </td>

                <td>
                    @Html.TextBox("planta2", arrPlantasValores(2), New With {.maxlength = 10, .Class = "form-control"})

                </td>

                <td>
                    @Html.TextBox("planta3", arrPlantasValores(3), New With {.maxlength = 10, .Class = "form-control"})

                </td>

                @If arrPlantas.Length > 4 Then

                    @<td>
                        @Html.TextBox("planta4", arrPlantasValores(4), New With {.maxlength = 10, .Class = "form-control"})

                    </td>

                End If

                @If arrPlantas.Length > 5 Then

                    @<td>
                        @Html.TextBox("planta5", arrPlantasValores(5), New With {.maxlength = 10, .Class = "form-control"})

                    </td>

                End If




                <input type="hidden" class="form-control" id="iden" name="iden" value=@identificador>
                <input type="hidden" class="form-control" id="numero" name="numero" value=@numero>

                <td style="text-align:center">



                    @Html.ActionLink(" ", "Valorplanta", "Home", New With {.id = item.Code, .name = item.Code}, New With {.class = "glyphicon glyphicon-edit", .title = "Assign"})



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


    $(function () {
        $("#submit").click(function () {
            $('#textoaviso').val("Valores Asignados");
            debugger;


            if (($("#VIGENTE1").val() == "0")) {
                $('#textoaviso').val("Rellena campo criticidad");
                return false;
            };

            var valnumero = $("input#numero").val();

            if (($("input#planta0").val() == "")) {
                $("#textoaviso").val("Rellena todos los valores");
                return false;
            };
            if (($("input#planta1").val() == "")) {
                $("#textoaviso").val("Rellena todos los valores");
                return false;
            };
            if (valnumero > 2) {
                if (($("input#planta2").val() == "")) {
                    $("#textoaviso").val("Rellena todos los valores");
                    return false;
                };
            }
            if (valnumero > 3) {
                if (($("input#planta3").val() == "")) {
                    $("#textoaviso").val("Rellena todos los valores");
                    return false;
                };
            }
            if (valnumero > 4) {
                if (($("input#planta4").val() == "")) {
                    $("#textoaviso").val("Rellena todos los valores");
                    return false;
                };
            }




            var val1 = $("input#iden").val();
            var val2 = $('#VIGENTE1').val();
            var campo0 = $("input#planta0").val();
            var campo1 = $("input#planta1").val();
            var campo2 = $("input#planta2").val();
            var campo3 = $("input#planta3").val();
            var campo4 = $("input#planta4").val();
            var campo5 = $("input#planta5").val();
            var campo6 = $("input#planta6").val();
            var campo7 = $("input#planta7").val();
            var dataString = 'val1=' + val1 + '&val2=' + val2 + '&campo0=' + campo0 + '&campo1=' + campo1 + '&campo2=' + campo2 + '&campo3=' + campo3 + '&campo4=' + campo4 + '&campo5=' + campo5 + '&campo6=' + campo6 + '&campo7=' + campo7;




            $.ajax({
                data: dataString,
                url: '@Url.Action("AddValores")',
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


