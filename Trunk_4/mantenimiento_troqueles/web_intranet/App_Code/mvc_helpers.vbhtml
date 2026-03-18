@Imports system.web.mvc.html
@Imports system.web.mvc
@Helper proveedorLinks(url As System.Web.Mvc.UrlHelper)
    @<li>
        <a href="@url.Action("Index", "informe")" Class="nav-link">@web_intranet.h.traducir("Activos")</a>
    </li>
    @<li>
        <a href="@url.Action("Index", "informe", New With {.displayEntregadas = "True"})" Class="nav-link">@web_intranet.h.traducir("Cerrados")</a>
    </li>
    @<li>
        <a href="@url.Action("marcasPendientes", "informe")" Class="nav-link">@web_intranet.h.traducir("Pedientes")</a>
    </li>
End Helper