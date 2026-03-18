@imports web
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width" />
    <meta name="WT.sv" content="@Server.MachineName" />
    <title>Portal del Manager</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href="@Url.Content("~/Content/bootstrap.min.css")" rel="stylesheet" />
    <link href="@url.Content("~/Content/Site.css")" rel="stylesheet" type="text/css" />
    @RenderSection("header", False)
</head>
<body>
    <nav class="navbar navbar-expand-lg navbar-light bg-light" role="navigation">
        <div class="container-fluid">
            <nav class="navbar navbar-light bg-light">
                <a class="navbar-brand" href="\HomeIntranet">
                    <img src="//intranet2.batz.es/baliabideorokorrak/logo_batz_menu.png" alt="">
                </a>
            </nav>
            <!-- Collect the nav links, forms, and other content for toggling -->
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <ul class="navbar-nav mr-auto">
                    @If SimpleRoleProvider.IsUserAuthorised(Role.rrhh, Role.responsable) Then
                        @<li Class="nav-item @(If(Regex.IsMatch(Request.Url.AbsolutePath, "listado"), "active", "")) ">
                            <a href="@Url.Action("index", "listado") " Class="nav-link">
                                @H.Traducir("Listados")
                            </a>
                        </li>
                        @<li Class="nav-item @(If(Regex.IsMatch(Request.Url.AbsolutePath, "solicitud"), "active", "")) ">
                            <a href="@Url.Action("index", "solicitud") " Class="nav-link">
                                @H.Traducir("Solicitud contratación")
                            </a>
                        </li>
                        @<li Class="nav-item @(If(Regex.IsMatch(Request.Url.AbsolutePath, "evaluacion"), "active", "")) ">
                            <a href="@Url.Action("index", "evaluacion") " Class="nav-link">
                                @H.Traducir("Evaluación personal")
                            </a>
                        </li>
                        @<li Class="nav-item @(If(Regex.IsMatch(Request.Url.AbsolutePath, "guia"), "active", "")) ">
                            <a href="@Url.Action("index", "guia") " Class="nav-link">
                                @H.Traducir("Guías del manager")
                            </a>
                        </li>
                    End If
                    @If SimpleRoleProvider.IsUserAuthorised(Role.rrhh, Role.eki) Then
                        @<li Class="nav-item @(If(Regex.IsMatch(Request.Url.AbsolutePath, "personal"), "active", "")) ">
                            <a href="@Url.Action("BusquedaCompletaAltas", "Personal") " Class="nav-link">
                                @H.Traducir("EKI")
                            </a>
                        </li>
                    End If

                </ul>
               
                    <ul class="navbar-nav">
                        @If SimpleRoleProvider.IsUserAuthorised(Role.rrhh) Then
                            Dim user = db.GetUsuarioSab(SimpleRoleProvider.GetId(), ConfigurationManager.ConnectionStrings("oracle").ConnectionString)
                            @<li>
                                <span Class="navbar-text">
                                    @H.Traducir("Conectado como:")
                                    @user.nombre
                                    @user.apellido1
                                </span>
                            </li>
                            @<li class="nav-item ml-1">
                                 <a class="btn btn-info" href="@Url.Action("suplantarUsuario", "access", New With {.ac = ViewContext.RouteData.Values("action"), .co = ViewContext.RouteData.Values("controller")})">@H.Traducir("Suplantar usuario")</a>
</li>
                        End If
                        @If Not SimpleRoleProvider.IsUserAuthorised(Role.rrhh) Then
                            @<li class="ml-2">
                                <form action="@Url.Action("closesession", "access")" method="post">
                                    <input type="submit" value="@H.Traducir("Cerrar sessión")" class="btn btn-outline-dark" />
                                </form>
                            </li>
                        End If
                    </ul>
            </div>

            
                        </div>
                    </nav>

                    
                

                        <div id="main_container" class = "container-fluid" >
                @RenderBody()
                        </div>

</body>
</html>
