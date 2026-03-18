@ModelType List(Of SabLib.ELL.Usuario)

@Code
    ViewData("Title") = "Administrar"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code


<h2>Administrar</h2>
<table class="table table-bordered table-responsive table-hover">
    <tr style="background:#ddd">
        <th>Nombre</th>
        <th style="text-align:center">Acción</th>
    </tr>
    @For Each d As SabLib.ELL.Usuario In Model
        @<tr>
            <td>@d.NombreCompleto</td>
            <td align="center">@Html.ActionLink("Eliminar", "DeleteAdmin", New With {.data = d.Id}, New With {.class = "btn btn-default btn-danger"})</td>
        </tr>
    Next
</table>
<div class="col-sm-12">
    @Html.Label("Añadir usuario: ", New With {.class = "control-label"})
    @Html.Editor("USUARIO", New With {.htmlAttributes = New With {.class = "form-control", .style = "display:inline-block"}})
    @*@Html.ActionLink("Añadir", "AddAdmin", Nothing, New With {.class = "btn btn-default btn-success"})*@
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