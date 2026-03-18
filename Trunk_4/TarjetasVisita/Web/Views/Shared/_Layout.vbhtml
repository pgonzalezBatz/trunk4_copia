@Imports System.Web.Optimization

<!DOCTYPE html>
<html lang="es">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <title>Tarjetas de visita</title>
    <link href="~/Content/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    @Styles.Render("~/bundles/css")
    @Scripts.Render("~/bundles/js")

    <script type="text/javascript">
        var inDelay = false;
        function MostrarMensaje(mensaje, modo) {
            if (inDelay) {
                // Para la animación actual
                $("#mensaje").finish();
            }

            $("#mensaje").hide();
            $("#mensaje").attr('class', '');

            $("#labelMensaje").text(mensaje);
            $("#mensaje").attr('class', modo);
            
            $("#mensaje").slideDown('fast', function () { inDelay = true; }).delay(5000).slideUp(function () { inDelay = false; });
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
                MostrarMensaje('@Html.Raw(topbar.Mensaje)','@topbar.Estilo');
            </script>
            TempData("topbar") = Nothing
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
</body>
</html>

