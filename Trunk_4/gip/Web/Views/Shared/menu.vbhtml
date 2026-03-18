@imports web


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
                    <a href="@Url.Action("search", "proveedor")">@h.traducir("Busqueda proveedores")</a>
                </li>
                @If SimpleRoleProvider.IsUserAuthorised(Web.roles.AdministrarPotencialesYCapacidades) Then
                    @<li>
                        <a href="@Url.Action("searchPotenciales", "proveedor")">@h.Traducir("Proveedores potenciales")</a>
                    </li>
                End If
                @If SimpleRoleProvider.IsUserAuthorised(Web.roles.telefonosdirectos) Then
                    @<li>
                        <a href="@Url.Action("list", "telefonodirecto")" class="nav-link">@h.Traducir("Extensiones directas")</a>
                    </li>
                End If
                @If SimpleRoleProvider.IsUserAuthorised(Web.roles.editar) Then
                    @<li>
                        <a href="@Url.Action("index", "corporativo")" class="nav-link">@h.Traducir("Proveedores globales")</a>
                    </li>
                End If
                @If SimpleRoleProvider.IsUserAuthorised(Web.roles.homologaciones) Then
                    @<li>
                        <a href="@Url.Action("searchHomologacion", "proveedor")" class="nav-link">@h.Traducir("Búsqueda homologación")</a>
                    </li>
                End If
            </ul>
            <ul class="nav navbar-nav navbar-right">
                <li><a href="\HomeIntranet"><span class="glyphicon glyphicon-off"></span> @h.traducir("Salir")</a></li>
            </ul>
        </div>
    </div>
</nav>