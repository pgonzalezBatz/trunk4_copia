@ModelType List(Of Criticidad)

@Code
    ViewData("Title") = "Criticidades"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code


<h2>Criticidades</h2>
<table class="table table-bordered table-responsive table-hover">
    <tr style="background:#ddd">
        <th>Código</th>
        <th>Descripción</th>
        <th style="text-align:center">Acción</th>
    </tr>
    @*necesito, codigo, nombre y descripcion, seran, id, nombre y apellido1*@ 
    @For Each d As Criticidad In Model
        @<tr>
    <td>@d.Name</td>
    <td>@d.desc</td>
    <td align="center">@Html.ActionLink("Eliminar", "DeleteCriticidad", New With {.data = d.Code}, New With {.class = "btn btn-default btn-danger"})</td>
</tr>
    Next
</table>
<div class="col-sm-12">
    @*@Dim  xx as string = "44"*@


    @Html.Label("Añadir criticidad: ", New With {.class = "control-label"})

    @Html.Editor("USUARIO", New With {.htmlAttributes = New With {.class = "form-control", .style = "display:inline-block"}})
    @*<asp:TextBox class="form-control" type="text" ID="txtEmpresa" runat="server"  />*@

    @Html.ActionLink("Añadir", "AddCriticidad", Nothing, New With {.name = "USUARIO", .desc = "USUARIO"}, New With {.class = "btn btn-default btn-success"})

</div>
<br />
<br />

        @Section Scripts

            @Scripts.Render("~/bundles/jqueryval")

            <script type="text/javascript">


        $(function () {
            $('#USUARIO').autocomplete({
                source: '@Url.Action("Suggest")',
                minLength: 3,
                select: function (evt, ui) {
                //
                    @*$.ajax({
                        type: 'POST',
                        url: '@Url.Action("SetUserAsAdminForResource")',
                        data: { input: ui.item.id },
                        success: function (result) {

                        },
                        error: function (ex) {
                            alert('Failed to retrieve states.' + ex);
                        }
                    });*@                //
                    $.ajax({
                        type: 'POST',
                        url: '@Url.Action("SetUserAsAdminForResource")',
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
