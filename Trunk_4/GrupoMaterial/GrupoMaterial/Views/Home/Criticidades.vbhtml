@ModelType List(Of Criticidad)

@Code
    ViewData("Title") = "Criticity"
    Layout = "~/Views/Shared/_Layout.vbhtml"
    'Dim urlajax As String = "https://intranet-test.batz.es/GrupoMaterial/Home/AddCriticidad"
    '     url: "/GrupoMaterial/Home/AddCriticidad",  en produccion, pero  en local    url: "/Home/AddCriticidad",
    Dim mensaje = ViewBag.mensaje
    ViewBag.mensaje = "pp"
End Code

<br /><br />
@Html.TextBox("textoaviso", mensaje, New With {.style = "border: transparent;float:right; text-align:center; font-size:22px; background-color: white; color:red;"})

<h2>Criticity</h2>
<table class="table table-bordered table-responsive table-hover">
    <tr style="background:#ddd">
        <th>Code</th>
        <th>Description</th>
        <th style="text-align:center">Action</th>
    </tr>
    @*necesito, codigo, nombre y descripcion, seran, id, nombre y apellido1*@
    @For Each d As Criticidad In Model
        @<tr>
            <td>@d.Name</td>
            <td>@d.desc</td>
            <td align="center">@Html.ActionLink("Delete", "DeleteCriticidad", New With {.data = d.Code}, New With {.class = "btn btn-default btn-danger"})</td>
        </tr>
    Next
</table>



<div class="chatFooter">
    <form name="contact2" action="">

        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

        @Html.Label("Add criticity: ", New With {.class = "control-label"})
        @Html.Editor("campo1", New With {.htmlAttributes = New With {.class = "form-control", .style = "display:inline-block"}})


        @Html.Editor("campo2", New With {.htmlAttributes = New With {.class = "form-control", .style = "display:inline-block"}})


        <button name="submit" id="submit" type="button" class="btn btn-default btn-success">
            Add
        </button>

    </form>
</div>





@Section Scripts



    @Scripts.Render("~/bundles/jqueryval")


    <script type="text/javascript">


        $(function () {
            $("#submit").click(function () {
                debugger;
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
                    url: '@Url.Action("AddCriticidad")',
                    type: "POST",
                    dataType: 'JSON',
                    success: function (response) {
                        window.location.reload();
                    }
                })
               
                return false;

            });
        });



    </script>





End Section
