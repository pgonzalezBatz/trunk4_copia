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
    @RenderSection("header", False)
</head>
<body>
    <a href="\HomeIntranet" title=@h.traducir("Ir a la página principal de la intranet") class="cabApp"></a>  
    <div id="menu1">
        <ul>
            <li>
                @If Url.Action("home","access") = Request.RawUrl Then
                    @<a href="@url.Action("home","access")"  class="selected" >@h.Traducir("Inicio")</a>    
                Else
                    @<a href="@url.Action("home","access")">@h.Traducir("Inicio")</a>
                End If
             </li>
            <li>
                @If Url.Action("listnegociosadministracion","organigrama") = Request.RawUrl Then
                    @<a href="@url.Action("listnegociosadministracion","organigrama")"  class="selected" >@h.Traducir("Organigrama")</a>    
                Else
                    @<a href="@url.Action("listnegociosadministracion","organigrama")">@h.Traducir("Organigrama")</a>
                End If
             </li>
            <li>
                @If Url.Action("list","relaciones") = Request.RawUrl Then
                    @<a href="@url.Action("list","relaciones")"  class="selected" >@h.Traducir("Relaciones")</a>    
                Else
                    @<a href="@url.Action("list","relaciones")">@h.Traducir("Relaciones")</a>
                End If
             </li>
            <li>
                @If Url.Action("listaplicacion","cuenta") = Request.RawUrl Then
                    @<a href="@url.Action("listaplicacion","cuenta")"  class="selected" >@h.Traducir("Cuentas")</a>    
                Else
                    @<a href="@url.Action("listaplicacion","cuenta")">@h.Traducir("Cuentas")</a>
                End If
             </li>
            <li>
                @If Url.Action("index", "nomina") = Request.RawUrl Then
                    @<a href="@Url.Action("index", "nomina")" class="selected">@h.traducir("Nomina")</a>
                Else
                    @<a href="@Url.Action("index", "nomina")">@h.traducir("Nomina")</a>
                End If
            </li>
            <li>
               <a href="@url.Content("~/ayuda.html")" target="_blank">@h.traducir("Ayuda")</a>
            </li>
        </ul>
    </div>
    <div id="menu2">
    </div>
    <div id="contenido1">
        @RenderSection("top", False)
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
