@imports web
<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>@ViewBag.Title</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <link href="@url.Content("~/Content/reset.css")" rel="stylesheet" type="text/css" />
    <link href="//intranet2.batz.es/baliabideorokorrak/estilointranet.css" rel="stylesheet" type="text/css" />
    <link href="@url.Content("~/Content/Site.css")" rel="stylesheet" type="text/css" />
    @RenderSection("header",False)
</head>
<body>
    <a href="\HomeIntranet" title="@h.traducir("Ir a la página principal de la intranet")" class="cabApp"></a>  
    <div id="menu1">
        @If Url.Action("index") = Request.RawUrl Then
            @<a href="@url.Action("index","inventario")"  class="selected" >@h.Traducir("Inicio")</a>    
        Else
            @<a href="@url.Action("index","inventario")">@h.Traducir("Inicio")</a>
        End If
        
        <a href="@url.Content("~/Content/schema.png")" target="_blank">@h.Traducir("Ayuda")</a>
    </div>
    <div id="menu2"></div>
    <div id="contenido1">
        <div id="contenido2">
            @RenderBody()
        </div>        
    </div>  
    <div id="pie">
        <a href="mailto:aazkuenaga@batz.es">
            
        </a>
    </div>
</body>
</html>
