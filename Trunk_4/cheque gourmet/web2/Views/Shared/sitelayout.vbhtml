@imports web2
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href="//intranet2.batz.es/baliabideorokorrak/estilointranet.css" rel="stylesheet" type="text/css" />
    <link href="@Url.Content("~/Content/Site.css")" rel="Stylesheet" type="text/css"  />
    @RenderSection("header",False)
</head>
<body>
    <div id="menu1">
        <img src="@Url.Content("~/Content/cheque_gourmet.png")" alt="Icono de la aplicación" style="float:left;" />

        <ul>
            <li>
                <a href="@Url.Action("Index", "historico")">@h.traducir("Historico de cheques")</a>
            </li>
            <li>
                <a href="@Url.Content("~/help.html")">@h.traducir("Ayuda")</a>
            </li>
        </ul>
    </div>
    <div id="menu2"></div>
    <div id="contenido1">
        <form action="@url.action("volverportal","access")"  method="post">
            <input type="submit" value="@h.traducir("Volver al portal")" />
        </form>
        <form action="@Url.Action("cerrarsession", "access")" method="post">
            <input type="submit" value="@h.traducir("Cerrar sesión")" />
        </form>
        <div id="contenido2">
            @RenderBody()
        </div>        
    </div>  
    <div id="pie">
        <a href="mailto:diglesias@batz.es">
            
        </a>
    </div>
</body>
</html>
