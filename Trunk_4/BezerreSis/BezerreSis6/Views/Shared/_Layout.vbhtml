<!DOCTYPE html>
<html>
    <head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
        <title>@ViewBag.Title</title>
        @Styles.Render("~/Content/css")
        @Scripts.Render("~/bundles/modernizr")
        @Styles.Render("~/Content/bootstrap-datetimepicker")   
    </head>
    <body style="padding-right:10px;display:none">
        <div class="navbar navbar-default navbar-fixed-top">
            <div class="container-fluid">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a>
                        <img class="navbar-brand" id="imgLogo" alt="BezerreSis" src="https://intranet2.batz.es/BaliabideOrokorrak/logo_batz_menu.png">
                    </a>
                </div>
                <div class="navbar-collapse collapse">
                    <ul class="nav navbar-nav">
                        <li class="dropdown">
                            <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                                Reclamaciones
                                <span class="caret"></span>
                            </a>
                            <ul class="dropdown-menu" role="menu">
                                <li>@Html.ActionLink("Reclamaciones", "Index", "Reclamaciones")</li>
                                <li>@Html.ActionLink("Testing", "IndexTest", "Reclamaciones")</li>
                            </ul>
                        </li>
                        <li class="dropdown">
                            <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                                Indicadores
                                <span class="caret"></span>
                            </a>
                            <ul class="dropdown-menu" role="menu">
                                <li>@Html.ActionLink("Indicadores Cliente KM.0", "Calidad", "INDICADORES2")</li>
                                <li>@Html.ActionLink("Indicadores Cliente Garantías", "CalidadGarantias", "INDICADORES2")</li>
                                @*<li class="disabled" style="cursor:not-allowed">@Html.ActionLink("Indicadores Proceso", "Proceso", "INDICADORES2", New With {.style = "pointer-events:none"})</li>*@
                                <li>@Html.ActionLink("Indicadores Proceso", "Proceso", "INDICADORES2")</li>
                            </ul>
                        </li>
                        <li class="dropdown">
                            <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                                Maestros
                                <span class="caret"></span>
                            </a>
                            <ul class="dropdown-menu" role="menu">
                                <li>@Html.ActionLink("Clientes", "Index", "Clientes")</li>
                                <li>@Html.ActionLink("Productos", "Index", "Productos")</li>
                                <li class="dropdown-submenu">
                                    <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                                        Objetivos
                                        <span class="caret-right"></span>
                                    </a>
                                    <ul class="dropdown-menu" role="menu">
                                        <li>@Html.ActionLink("Objetivos Cliente KM.0", "Index", "OBJETIVOS")</li>
                                        <li><!--class="disabled" style="cursor:not-allowed"-->@Html.ActionLink("Objetivos Proceso KM.0", "Proceso", "OBJETIVOS")</li>
                                    </ul>
                                </li>
                            </ul>
                        </li>
                    </ul>
                    @Html.Partial("_LoginPartial")
                </div>
            </div>
        </div>
        <div class="container body-content" style="width:100%;">
            @RenderBody()
        </div>
        @Html.Partial("_Cargando")
        @Scripts.Render("~/bundles/jquery")
        @Scripts.Render("~/bundles/bootstrap")
        @Scripts.Render("~/bundles/bootstrap-datetimepicker")
        @RenderSection("Scripts", required:=False)
    </body>
</html>
