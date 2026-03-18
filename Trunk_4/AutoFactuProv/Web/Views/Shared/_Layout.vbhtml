<!DOCTYPE html>
<html lang="es">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <title>@Utils.Traducir("Autofactura de proveedores")</title>
    <link href="~/Content/favicon.ico" rel="shortcut icon" type="image/x-icon" />    
    <link href="~/Content/style.css" rel="stylesheet" type="text/css" />    
    <link href="~/Content/bootstrap.css" rel="stylesheet" />
    <script src="~/Scripts/jquery-1.12.0.min.js" type="text/javascript"></script>    
    <script src="~/Scripts/bootstrap.min.js"></script>

    <script type="text/javascript">
        function MostrarMensaje(modo) {
            $("#mensaje").toggleClass(modo);
            $("#mensaje").slideDown('fast').delay(5000).slideUp(function () {
                $(this).toggleClass(modo)
            });
        }
    </script>
</head>
<body>
    <header>
        @Html.Partial("~/Views/Shared/_NavMenu.vbhtml", Session("Ticket").Ticket)
    </header>
    <div id="mensaje">
        @code
            @If (Not (TempData("topbar") Is Nothing)) Then
                Dim topbar As Topbar = CType(TempData("topbar"), Topbar)

                @Html.Label(String.Empty, topbar.Mensaje, New With {.class = "negrita"})
                @<script type="text/javascript">
                    MostrarMensaje('@topbar.Estilo');
                </script>
            End If
        End code
    </div>
    <div Class="container">
        @RenderBody()
    </div>
    <footer style="margin-top:15px;">
        <div class="panel panel-default text-center">
            <div class="panel-heading">@Utils.Traducir("encasodeteneralgunproblema contactarcon") <a href="/helpdesk" target="_blank">Helpdesk</a></div>
        </div>
    </footer>
</body>
</html>

