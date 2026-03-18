@imports web
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
                <a href="@Url.Action("Index", "distribucion")">@h.traducir("Historico de cheques")</a>
            </li>
            <li>
                <a href="@Url.Action("list", "distribucion")">@h.traducir("Listado de distribuciones")</a>
            </li>
            <li>
                <a href="@Url.Action("index", "admin")">@h.traducir("Administración")</a>
            </li>
            <li>
                <a href="@Url.Action("Index", "devolucion")">@h.traducir("Devolución de cheques")</a>
            </li>
            <li>
                <a href="@Url.Action("index", "nomina")">@h.traducir("Exportar para nomina")</a>
            </li>
            <li>
                <a href="@Url.Content("~/help.html")">@h.traducir("Ayuda")</a>
            </li>
        </ul>
    </div>
    <div id="menu2"></div>
    <div id="contenido1">
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
