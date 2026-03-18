@Imports TarjetasVisitaLib

@ModelType SabLib.ELL.Ticket

@code
    Dim rolesUsuario As List(Of ELL.UsuarioRol) = CType(Session("RolesUsuario"), List(Of ELL.UsuarioRol))
End Code

<nav class="navbar navbar-default" role="navigation">
    <div class="container-fluid">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>

            <a href="@Url.Action("Cerrar", "Login")" class="navbar-brand" title='@System.Environment.MachineName'>
                <img src='@Url.Content("~/Content/Images/logo-batz.png")' style="border:none;" title='@Utils.Traducir("Ir a la página principal de la extranet")' />
            </a>
        </div>
        <div class="collapse navbar-collapse">
            <ul class="nav navbar-nav">
                @Html.Raw(Utils.GenerarMenu(rolesUsuario, Model.IdUser))
            </ul>
            <ul class="nav navbar-nav navbar-right">
                <li><a href="@Url.Action("Salir", "Login")"><span class='glyphicon glyphicon-off'></span> @Utils.Traducir("Salir")</a></li>
            </ul>
            <p class="navbar-text navbar-right"><span class='glyphicon glyphicon glyphicon-user'></span>&nbsp;@Html.Raw(String.Format("<b>{0}</b>", Model.NombreCompleto))</p>
        </div>
    </div>
</nav>