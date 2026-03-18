<!DOCTYPE html>
<html lang="es">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <title>Batz S. Koop.</title>
    <link href="~/Content/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="@Url.Content(Request.Url.Scheme + "://intranet.batz.es/BaliabideOrokorrak/estiloIntranet.css")" rel="stylesheet" type="text/css" media="screen" />
    <link href="~/Content/menu.css" rel="stylesheet" type="text/css" />
    <link href="~/Scripts/jqueryUI/jquery-ui.min.css" rel="stylesheet" />
    <link href="~/Content/style.css" rel="stylesheet" type="text/css" />
    <script src="~/Scripts/jquery-3.1.0.min.js"></script>
    <script src="~/Scripts/ckeditor/ckeditor.js"></script>
    <script src="~/Scripts/jqueryUI/jquery-ui.min.js"></script>
    <script src="~/Scripts/jqueryUI/datepicker-cs.js"></script>
    <script src="~/Scripts/jqueryUI/datepicker-es.js"></script>
    <script src="~/Scripts/jqueryUI/datepicker-eu.js"></script>
    <script src="~/Scripts/jqueryUI/datepicker-pt.js"></script>
    <script src="~/Scripts/jqueryUI/datepicker-zh.js"></script>
    <script src="~/Scripts/Chart.bundle.min.js"></script>
    <script src="~/Scripts/jqmeter.min.js"></script>

    <script type="text/javascript">
        Chart.defaults.global.elements.rectangle.backgroundColor = 'rgba(54, 162, 235, 0.2)';
        Chart.defaults.global.elements.rectangle.borderColor = 'rgba(54, 162, 235, 1)';
        Chart.defaults.global.elements.rectangle.borderWidth = 1;

        function MostrarMensaje(modo) {
            $("#mensaje").toggleClass(modo);
            $("#mensaje").slideDown('fast').delay(3000).slideUp(function () {
                $(this).toggleClass(modo)
            });
        }

        var options = $.extend(
            {},
            $.datepicker.regional["@Session("Ticket").Culture.Substring(0, Session("Ticket").Culture.IndexOf("-")).ToLower()"],
            {
                minDate: 0,
                showOn: "both",
                buttonImage: "@Url.Content("~/Content/Images/calendar.gif")",
                buttonImageOnly: true,
                buttonText: "@Utils.Traducir("Seleccione una fecha")"
            }
        );

        $.datepicker.setDefaults(options);
    </script>
</head>
<body>
    <header>
        <a href="\HomeIntranet" class="cabApp" title='@WebRaiz.Utils.Traducir("Ir a la página principal de la intranet")'></a>
        <div class="header">
            <div class="header-left">@String.Format("{0}:", Utils.Traducir("usuarioConectado"))&nbsp;</div>
            <div class="header-left bold">@Session("Ticket").NombreCompleto</div>
            <div class="header-right">@DateTime.Now.ToLongDateString()</div>
            <div class="clear-float"></div>
        </div>
        <div style="margin-top: 5px;">
            <div style="float: left;">
                <a href='@Url.Action("Resumen", "MisIdiomas")'>
                    <img src='@Url.Content("~/Content/favicon.ico")' width="32" style="border:none;" title='@System.Environment.MachineName' />
                </a>
            </div>
            <div style="float: left; margin-left: 10px;">
                <div id="header">
                    @Html.Partial("~/Views/Shared/_NavMenu.vbhtml", Session("Ticket"))
                </div>
            </div>
            <div class="clear-float"></div>
        </div>
    </header>
    <div id="mensaje" class="topbar">
        @code
            @If (Not (TempData("topbar") Is Nothing)) Then
                Dim topbar As Topbar = CType(TempData("topbar"), Topbar)
                Dim modo As String = [Enum].GetName(GetType(Topbar.ModoMensaje), topbar.Modo)

                @Html.Label(String.Empty, topbar.Mensaje, New With {.class = "negrita"})
                @<script type="text/javascript">
                    MostrarMensaje('@modo');
                </script>
            End If
        End Code
    </div>
    <div id="body">
        <section>
            @RenderBody()
        </section>
    </div>
    <footer>
        <div class="footer">
            @Utils.Traducir("encasodeteneralgunproblema contactarcon") <a href="/helpdesk" target="_blank">Helpdesk</a>
        </div>
    </footer>
</body>
</html>
