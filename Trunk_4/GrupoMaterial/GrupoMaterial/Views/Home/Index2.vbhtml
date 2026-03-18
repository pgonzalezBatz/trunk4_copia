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
    Dim j As Integer = 0
    Dim k As Integer = 0
    Dim arrPlantas As String() = Nothing
    Dim Plantasconfig As String = ConfigurationManager.AppSettings("Plantas")
    Dim tmp2 As Integer = 0

    Dim DAL As New OracleDataAccess


    'Dim identificador = ViewData("identificador")

    Dim arrPlantasdesc As String() = Nothing
    Dim arrPlantasValores As String() = Nothing
    Dim tmp As List(Of Criticidad)
    'Dim Plantasconfigdesc As String = ConfigurationManager.AppSettings("Plantasdesc")
    arrPlantas = Split(Plantasconfig, ",")
    arrPlantasdesc = Split(Plantasconfig, ",")
    arrPlantasValores = Split(Plantasconfig, ",")
    For j = 0 To arrPlantas.Length - 1
        arrPlantasdesc(j) = DAL.Valoresplantatxt(arrPlantas(j))
    Next
    'For j = 0 To arrPlantas.Length - 1
    '    tmp = DAL.Valoresplantaint(CInt(identificador), CInt(arrPlantas(j)))

    '    If tmp.Count > 0 Then
    '        arrPlantasValores(j) = tmp(0).Code
    '    Else
    '        arrPlantasValores(j) = 0
    '    End If

    'Next





    'pantas en BBDD comparado con arrPlantas
    Dim cont As Integer = 0

    'borrar las que no estén en webconfig de bbdd
    DAL.deletePlantas(Plantasconfig)

    tmp = DAL.PlantasBBDD()

    If tmp.Count > arrPlantas.Length Then
        cont = arrPlantas.Length   'se ha quitado en config, no puede ser, he borrado antes
    Else
        'se ha añadido en config
        cont = tmp.Count ' todos desconfigurados, lo da en la select
    End If



    'For j = 0 To cont - 1

    '    If arrPlantas(j) = tmp(j).Code Then
    '        tmp2 = 1 'bien
    '    Else
    '        If arrPlantas(j) > tmp(j).Code Then
    '            tmp2 = 2 'borrar de la bbdd
    '            'volver a pasar el bucle
    '        Else
    '            ' todos como pendientes, lo debe detectar
    '            tmp2 = 3
    '        End If

    '    End If
    '    ' todos como pendientes, lo debe detectar
    'Next




    'poner si está completo
    Dim completo As List(Of String())
    Dim completo2 As List(Of String())


    @For Each item In Model
        item.NewProperty = "No"
        If IsNumeric(Item.code) And Item.Name <> "" Then
            completo = DAL.Completo(Item.code)

            If completo.Count = arrPlantas.Length Then
                'hay que mirar si tiene tambien la criticidad en criticidad_elemento
                completo2 = DAL.Completo2(Item.code)
                If completo2.Count > 0 Then
                    item.NewProperty = "Si"
                End If


            End If
        Else
            item.NewProperty = "Si"
        End If
    Next


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

<div Class="col-md-12">
    <Table style="margin: 0 auto;" id="leyenda">
        <tbody>
            <tr>
                <td style="width: 20px; background-color: #f95;">&nbsp;</td>
                <td style="width:5px;"></td>
                <td> From Xpert</td>
                <td style="width: 20px;"></td>
                <td style="width: 20px; background-color: #7da7ca;">&nbsp;</td>
                <td style="width:5px;"></td>
                <td> Editable</td>
                <td style="width: 20px;"></td>
                <td style="width: 20px;">
                    <i class="glyphicon glyphicon-list"></i>
                </td>
                <td style="width:5px;"></td>
                <td> Show list</td>
                <td style="width: 20px;"></td>
                <td style="width: 20px;">
                    <i class="glyphicon glyphicon-plus"></i>
                </td>
                <td style="width:5px;"></td>
                <td> Create New</td>
            </tr>
        </tbody>
    </Table>
</div>
<br />
<br />
<div Class="row">

    <Table Class="table table-bordered">
        <tr id="myRow">
            @*<th style="background:#ddd">
                    @Html.Label("Code")
                </th>*@
            <th style="background:#f95;min-width:90px">
                @Html.Label("GM CODE")
            </th>
            <th style="background:#f95;">
                @Html.Label("GM NAME")
            </th>
            <th class="actionColumn" style="background-image: linear-gradient(to right,#f95,#7da7ca);">
                ASSIGNATION
            </th>
            <th class="actionColumn" style="background:#7da7ca;">
                @Html.Label("ELEMENT")
                <span style="float:right">@Html.ActionLink(" ", "GetElements", "Default", "", New With {.class = "glyphicon glyphicon-list", .style = "color:black;font-size:25px;margin-top:-5px;transform:translateY(5px)", .title = "Show list"})</span>
                <span style="float:right;margin-right:5px">@Html.ActionLink(" ", "CreateElementFullData", "Default", "", New With {.class = "glyphicon glyphicon-plus", .style = "color:black;font-size:25px;margin-top:-5px;transform:translateY(5px)", .title = "Create new"})</span>
            </th>
            <th style="background:#7da7ca;">
                @Html.Label("SUBFAMILY")
                <span style="float:right">@Html.ActionLink(" ", "GetSubFamilies", "Default", "", New With {.class = "glyphicon glyphicon-list", .style = "color:black;font-size:25px;margin-top:-5px;transform:translateY(5px)", .title = "Show list"})</span>
                <span style="float:right;margin-right:5px">@Html.ActionLink(" ", "CreateSubFamily", "Default", "", New With {.class = "glyphicon glyphicon-plus", .style = "color:black;font-size:25px;margin-top:-5px;transform:translateY(5px)", .title = "Create new"})</span>
            </th>
            <th style="background:#7da7ca;">
                @Html.Label("FAMILY")
                <span style="float:right">@Html.ActionLink(" ", "GetFamilies", "Default", "", New With {.class = "glyphicon glyphicon-list", .style = "color:black;font-size:25px;margin-top:-5px;transform:translateY(5px)", .title = "Show list"})</span>
                <span style="float:right;margin-right:5px">@Html.ActionLink(" ", "CreateFamily", "Default", "", New With {.class = "glyphicon glyphicon-plus", .style = "color:black;font-size:25px;margin-top:-5px;transform:translateY(5px)", .title = "Create new"})</span>
            </th>
            <th style="background:#7da7ca;min-width:200px;">
                @Html.Label("COMODITY")
                <span style="float:right">@Html.ActionLink(" ", "GetComodities", "Default", "", New With {.class = "glyphicon glyphicon-list", .style = "color:black;font-size:25px;margin-top:-5px;transform:translateY(5px)", .title = "Show list"})</span>
                <span style="float:right;margin-right:5px">@Html.ActionLink(" ", "CreateComodity", "Default", "", New With {.class = "glyphicon glyphicon-plus", .style = "color:black;font-size:25px;margin-top:-5px;transform:translateY(5px)", .title = "Create new"})</span>
            </th>

            <th style="background:#f95;">
                @Html.Label("INCOMPLETE")
            </th>


        </tr>
        @For Each item In Model
            @<tr>
                <td>
                    @Html.DisplayFor(Function(modelItem) item.GMCode)
                </td>
                <td>

                    @Html.DisplayFor(Function(modelItem) item.GMName)
                </td>
                <td style="text-align:center">
                    @If item.GMCode.Trim.Equals("") OrElse item.Code.Trim.Equals("") Then
                        @Html.ActionLink(" ", "EditFullData", "Default", New With {.id = item.GMCode, .name = item.Name}, New With {.class = "glyphicon glyphicon-edit", .title = "Assign"})
                        @*End If
                            @If Not item.SubfamilyId.Trim.Equals("") OrElse Not item.FamilyId.Trim.Equals("") OrElse Not item.ComodityId.Trim.Equals("") Then
                                @Html.ActionLink(" ", "UnAssign", "Default", New With {.id = item.GMCode}, New With {.class = "glyphicon glyphicon-edit strikeout", .title = "Unassign", .onclick = "return confirm('Are you sure you want to unassign this element to the hierarchy?');", .style = "margin-left:10px"})*@
                    Else
                        @Html.ActionLink(" ", "UnAssign", "Default", New With {.id = item.GMCode}, New With {.class = "glyphicon glyphicon-edit strikeout", .title = "Unassign", .onclick = "return confirm('Are you sure you want to unassign this element to the hierarchy?');"})
                    End If
                </td>
                <td>
                    @If item.Name.Trim.Equals("") Then
                        'item.Name = ""
                    Else
                        @Html.ActionLink(item.Name, "Valorplanta", "Home", New With {.id = item.Code, .name = item.Code}, New With {.title = "Valorplanta"})

                    End If




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
                    @If item.NewProperty.Trim.Equals("No") Then
                        @Html.ActionLink("Incomplete", "Valorplanta", "Home", New With {.id = item.Code, .name = item.Code}, New With {.title = "Completar", .style = "text-align:center; font-size:22px; background-color: red; color:white;padding:0 5px 0 5px;"})


                    Else

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
