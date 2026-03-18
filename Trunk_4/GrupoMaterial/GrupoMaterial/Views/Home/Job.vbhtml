@Code
    Dim sql As New SQLDataAccess()
    Dim data = sql.getData()

    Dim vigenteList1 As New List(Of SelectListItem)
    vigenteList1.Add(New SelectListItem With {.Value = True, .Text = "True", .Selected = CBool(data(0)(4))})
    vigenteList1.Add(New SelectListItem With {.Value = False, .Text = "False", .Selected = Not CBool(data(0)(4))})
    Dim vigenteList2 As New List(Of SelectListItem)
    vigenteList2.Add(New SelectListItem With {.Value = True, .Text = "True", .Selected = CBool(data(1)(4))})
    vigenteList2.Add(New SelectListItem With {.Value = False, .Text = "False", .Selected = Not CBool(data(1)(4))})

    Dim statusFull = TempData("status")
    Dim status = ""
    Dim jobId = ""
    Dim statusData As New List(Of String())

    If statusFull IsNot Nothing Then
        Dim statusSplitted = statusFull.Split(";")
        status = statusSplitted(0)
        If statusSplitted.Length > 1 Then
            jobId = statusSplitted(1)
            statusData = sql.getStatusData(jobId)
        End If
    End If

    Dim refs = sql.getReferenciasNoVisibles()

End Code
@Using (Html.BeginForm("ActualizarYLanzarJob", "Home"))

    @<div>

        <br />
        <table>
            <tr>
                <th Class="text-center">FECHA</th>
                <th Class="text-center">INFO</th>
                <th Class="text-center">AÑO</th>
                <th Class="text-center">MES</th>
                <th Class="text-center">VIGENTE</th>
            </tr>
            <tr>
                <td Class="padTd">@Html.Label("Last Year", htmlAttributes:=New With {.class = "control-label", .style = "font-weight:300;display:inline"})</td>
                <td class="padTd">@Html.TextBox("INFO1", data(0)(1).ToString, htmlAttributes:=New With {.class = "form-control", .style = "font-weight:300;display:inline", .id = "INFO1", .readonly = "readonly"})</td>
                <td class="padTd">@Html.TextBox("ANYO1", data(0)(2), htmlAttributes:=New With {.class = "form-control"})</td>
                <td class="padTd">@Html.TextBox("MES1", data(0)(3), htmlAttributes:=New With {.class = "form-control"})</td>
                <td class="padTd">@Html.DropDownList("VIGENTE1", vigenteList1, htmlAttributes:=New With {.class = "form-control"})</td>
            </tr>
            <tr>
                <td class="padTd">@Html.Label("Closed Month", htmlAttributes:=New With {.class = "control-label", .style = "font-weight:300;display:inline"})</td>
                <td class="padTd">@Html.TextBox("INFO2", data(1)(1).ToString, htmlAttributes:=New With {.class = "form-control", .style = "font-weight:300;display:inline", .id = "INFO2", .readonly = "readonly"})</td>
                <td class="padTd">@Html.TextBox("ANYO2", data(1)(2), htmlAttributes:=New With {.class = "form-control"})</td>
                <td class="padTd">@Html.TextBox("MES2", data(1)(3), htmlAttributes:=New With {.class = "form-control"})</td>
                <td Class="padTd">@Html.DropDownList("VIGENTE2", vigenteList2, htmlAttributes:=New With {.class = "form-control"})</td>
            </tr>
        </table>
        <br />
        <input type="submit" name="ejecucion" value="Actualizar y lanzar job" class="btn btn-default btn-success" style="margin-left:20px" onclick="return confirm('Estás seguro de querer actualizar los datos y ejecutar el proceso?');" />
        <h6 style="display:inline"><span style="color:green;font-style:italic">@TempData("msg")</span></h6>
        <br />
        <div style="margin-left:20px;margin-top:5px;">
            @Html.ActionLink("Check Status", "VerEstadoJob", Nothing, New With {.class = "btn btn-default btn-info"})
            <h6 style="display:inline"><span style="color:green;font-style:italic">@status</span></h6>
        </div>
        <br />
        @If statusData.Count > 0 Then

            @<table class="table table-bordered table-responsive table-hover" style="max-width:85%;margin:0 auto;">
                <tr style="background-color:#eee!important">
                    <th>Run date</th>
                    <th>Run time</th>
                    <th>Run duration</th>
                    <th>Message</th>
                </tr>
                @For Each d As String() In statusData
                    @<tr>
                        <td>@d(0)</td>
                        <td>@d(1)</td>
                        <td>@d(2)</td>
                        <td>@d(3).Split(".")(0)</td>
                    </tr>
                Next

            </table>

        End If

    </div>

    @<br />
    @<div style="max-width:80%;margin:0 auto;font-size:12px;">
        <h5 style="font-variant:small-caps">Referencias desactivadas:</h5>
        <table class="table table-bordered table-responsive table-hover">
            <tr style="background:#eee">
                <th>Referencia</th>
                <th>Descripción</th>
                <th></th>
            </tr>
            @For Each r As String() In refs
                @<tr>
                    <td>@r(1)</td>
                    <td>@r(2)</td>
                    <td align="center">@Html.ActionLink("Activar", "ActivarReferencia", New With {.ref = r(0)}, New With {.class = "btn btn-default btn-warning", .onclick = "return confirm('Estás seguro de que quieres activar esta referencia?')"})</td>
                </tr>
            Next
        </table>
        <div class="col-sm-12">
            @Html.Label("Desactivar referencia: ", New With {.class = "control-label"})
            @Html.Editor("REFERENCIA", New With {.htmlAttributes = New With {.class = "form-control", .style = "display:inline-block"}})
            @*@Html.ActionLink("Añadir", "AddAdmin", Nothing, New With {.class = "btn btn-default btn-success"})*@
        </div>
    </div>
    @<br />
    @<br />
End Using

@Section Scripts
    <script type="text/javascript">
        var shortMonths = ['N/A', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

        $(function () {
            $('#VIGENTE1').change(function () {
                var val1 = $('#VIGENTE1').val();
                if (val1.trim() == "True") {
                    $('#VIGENTE2').val("False");
                } else {
                    $('#VIGENTE2').val("True");
                }
            })
        });

        $(function () {
            $('#VIGENTE2').change(function () {
                var val2 = $('#VIGENTE2').val();
                if (val2.trim() == "True") {
                    $('#VIGENTE1').val("False");
                } else {
                    $('#VIGENTE1').val("True");
                }
            })
        });

        var setInfo = function (event) {
            var id = event.data.n.toString();
            $('#INFO' + id).val($('#ANYO' + id).val() + "/" + shortMonths[$('#MES' + id).val()]);
        };

        $('#ANYO1').on('input', { n: 1 }, setInfo);
        $('#MES1').on('input', { n: 1 }, setInfo);
        $('#ANYO2').on('input', { n: 2 }, setInfo);
        $('#MES2').on('input', { n: 2 }, setInfo);




        $(function () {
            $('#REFERENCIA').autocomplete({
                source: '@Url.Action("SuggestRef")',
                minLength: 4,
                select: function (evt, ui) {
                    $.ajax({
                        type: 'POST',
                        url: '@Url.Action("DesactivarReferencia")',
                        data: { input: ui.item.id },
                        success: function (result) {
                            window.location.reload(true);
                        },
                        error: function (ex) {
                            alert('Failed to retrieve states.' + ex);
                        }
                    });
                }
            });
        });

    </script>
End Section