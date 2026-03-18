@imports web
<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href="//intranet2.batz.es/baliabideorokorrak/estilointranet.css" rel="stylesheet" type="text/css" />
    <link href="@Url.Content("~/Content/Site.css")" rel="Stylesheet" type="text/css"  />
    @RenderSection("header",False)
</head>
<body>
    <a href="\HomeIntranet" title="@h.traducir("Ir a la página principal de la intranet")" class="cabApp"></a>  
    <div id="menu1">
        <h1>@h.traducir("Baliabide Fisikoen Kudeaketa") </h1>
    </div>
    <div id="menu2"></div>
    <div id="contenido1">
        @RenderSection("beforebody", False)
        <div id="contenido2">
            @RenderBody()
        </div>        
    </div>  
    <div id="pie">
        <a href="://intranet2.batz.es/Helpdesk">
            @h.traducir("Helpdesk")
        </a>
    </div>
</body>
</html>
