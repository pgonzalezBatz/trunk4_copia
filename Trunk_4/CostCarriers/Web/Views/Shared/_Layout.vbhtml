@Imports System.Web.Optimization

<!DOCTYPE html>
<html lang="es">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <title>Cost carriers</title>
    <link href="~/Content/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    @Styles.Render("~/bundles/css")
    @Scripts.Render("~/bundles/js")

    <script type="text/javascript">
        function MostrarMensaje(mensaje, modo) {
            $("#labelMensaje").text(mensaje);
            $("#mensaje").toggleClass(modo);
            $("#mensaje").slideDown('fast').delay(5000).slideUp(function () {
                $(this).toggleClass(modo);
                $("#labelMensaje").text('');
            });
        }
    </script>
</head>
<body>
    <header>
        @Html.Partial("~/Views/Shared/_NavMenu.vbhtml", Session("Ticket"))
    </header>
    <div id="mensaje" style="display:none;">
        <label class="negrita" id="labelMensaje"></label>
    </div>
    @code
        @If (Not (TempData("topbar") Is Nothing)) Then
            Dim topbar As Topbar = CType(TempData("topbar"), Topbar)
            @<script type="text/javascript">
                MostrarMensaje('@topbar.Mensaje','@topbar.Estilo');
            </script>
        End If

        Dim containerClass = "container"
        If (ViewData.ContainsKey("ContainerFluid")) Then
            containerClass = "container-fluid"
        End If
    End code
    <div Class="@containerClass">
        @RenderBody()
    </div>
    <footer style="margin-top:15px;">
        <div class="panel panel-default text-center">
            <div class="panel-heading">@Utils.Traducir("encasodeteneralgunproblema contactarcon") <a href="/helpdesk" target="_blank">Helpdesk</a></div>
        </div>
    </footer>
    @RenderSection("scripts", False)
</body>
</html>

