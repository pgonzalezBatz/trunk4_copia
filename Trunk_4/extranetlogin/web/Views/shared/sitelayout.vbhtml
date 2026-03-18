@imports web
<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="WT.sv" content="@Server.MachineName" />
    <title>Batz S. Coop. - Extranet</title>
    <link href="//intranet2.batz.es/baliabideorokorrak/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/extranetBatz.css" rel="stylesheet"/>
    @RenderSection("header", False)
</head>
<body>
        <nav class="navbar navbar-default">
            <div class="container-fluid">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href='@Url.Action("Index", "access")'><img src="~/Content/logo_20px.png" /></a>
                </div>
                <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                    @If User.Identity.IsAuthenticated Then
                        @<ul class="nav navbar-nav">
                            <li>
                                <a href="@Url.Action("changeLanguage", "access")">@h.traducir("Cambiar idioma")</a>
                            </li>
                            <li>
                                <a href="@Url.Action("changepassword", "access")">@h.traducir("Cambiar contraseña")</a>
                            </li>
                            @*@if ViewData("hasMultiplePlantas") IsNot Nothing AndAlso ViewData("hasMultiplePlantas") Then
                                @<li>
                                    <a href="@Url.Action("SeleccionarPlanta")">@h.traducir("Cambiar Planta")</a>
                                </li>
                            End If*@
                        </ul>
                        @<ul class="nav navbar-nav navbar-right">
                            <li>
                                <a href="@Url.Action("logoff", "access")" style="color:inherit;">
                                    <span class="glyphicon glyphicon-off"></span>
                                    @h.traducir("Cerrar sesión")

                                </a>
                            </li>
                        </ul>
                    End If
                </div>
                </div>
</nav>
    <div class="container">
        @RenderBody()
    </div>
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="~/Scripts/bootstrap.js"></script>
</body>
</html>
