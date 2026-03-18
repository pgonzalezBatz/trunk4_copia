<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="WT.sv" content="@Server.MachineName" />
    <title>@web.h.traducir("SAS")</title>
    <link href="//intranet2.batz.es/baliabideorokorrak/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <title>SAS</title>
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
                    <li><a href="@Url.Action("landingpage", "access")" class="nav-link">@web.h.traducir("Inicio")</a></li>
                   <li class="dropdown">
                        <a href="#" Class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">@h.traducir("Estados") <span class="caret"></span></a>
                        <ul class="dropdown-menu">
                            <li><a href="@Url.Action("ListMovimientosSinAsignar", "estado")">@h.traducir("Movimientos no asignados a bulto")</a></li>
                            @If SimpleRoleProvider.IsUserAuthorised(Role.creacion) Then
                                @<li role="separator" Class="divider"></li>
                                @<li><a href="@Url.Action("ListBultosSinAsignar", "estado")" Class="nav-link">@h.traducir("Bultos no asignados a albaran")</a></li>

                            End If
                        </ul>
                    </li>
                </ul>
                <ul class="nav navbar-nav navbar-right">
                   
                    <li>
                        <a href="@Url.Content("/homeintranet")" style="color:inherit;">
                            <span class="glyphicon glyphicon-off"></span>
                            @web.h.traducir("Salir")

                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
    <div class="container-fluid">
        @RenderBody()
    </div>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="//intranet2.batz.es/baliabideorokorrak/bootstrap/js/bootstrap.min.js"></script>
    @RenderSection("scripts", False)
</body>
</html>
