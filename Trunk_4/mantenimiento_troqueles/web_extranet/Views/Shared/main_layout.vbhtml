<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="WT.sv" content="@Server.MachineName" />
    <title>@h.traducir("Certificados de Calidad")</title>
    <link href="//intranet2.batz.es/baliabideorokorrak/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/mvc.css" rel="stylesheet" />
    @RenderSection("header", False)
</head>
<body>
    <nav class="navbar navbar-default" role="navigation">
        <div class="container-fluid">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <img class="navbar-brand" src="//intranet2.batz.es/baliabideorokorrak/logo_batz_menu.png" />
            </div>
            <!-- Collect the nav links, forms, and other content for toggling -->
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <ul class="nav navbar-nav">
                    <li>
                        <a href="@Url.Action("Index", "informe")" class="nav-link">@h.traducir("Informes abiertos")</a>
                    </li>
                    <li>
                        <a href="@Url.Action("Index", "informe", New With {.displayEntregadas = "True"})" class="nav-link">@h.traducir("Informes cerrados")</a>
                    </li>
                    <li>
                        <a href="@Url.Action("marcasPendientes", "informe")" class="nav-link">@h.traducir("Pendientes de Informe")</a>
                    </li>
                </ul>
                <ul class="nav navbar-nav navbar-right">
                    <li>
                        <a href="@Url.Action("TransferToExtranet")" style="color:inherit;">
                            <span class="glyphicon glyphicon-off"></span>
                            @h.traducir("Salir")

                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
    <div class="container">
        @RenderBody()
     </div>
        <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.11.0.min.js" type="text/javascript"></script>
        <script src="//intranet2.batz.es/baliabideorokorrak/bootstrap/js/bootstrap.min.js"></script>
        @RenderSection("scripts", False)
</body>
</html>
