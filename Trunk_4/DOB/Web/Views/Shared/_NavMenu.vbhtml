@Imports DOBLib

@ModelType SabLib.ELL.Ticket

@code
    'Dim rolesUsuario As List(Of ELL.UsuarioRol) = CType(Session("RolesUsuario"), List(Of ELL.UsuarioRol))
    Dim rolActual As ELL.UsuarioRol = CType(Session("RolActual"), ELL.UsuarioRol)
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

            <a href="@Url.Action("Salir", "Login")" class="navbar-brand" title='@System.Environment.MachineName'>
                <img src="//intranet2.batz.es/BaliabideOrokorrak/logo_batz_menu.png" style="border:none;" />
            </a>
        </div>
        <div class="collapse navbar-collapse">
            <ul class="nav navbar-nav">
                @code
                    Dim regenerarMenu As Boolean = False
                    If (Not (TempData("RegenerarMenu") Is Nothing)) Then
                        regenerarMenu = CType(TempData("RegenerarMenu"), Boolean)
                    End If

                    Dim cargaInicial As Boolean = False
                    If (Not (TempData("cargaInicial") Is Nothing)) Then
                        cargaInicial = CType(TempData("cargaInicial"), Boolean)
                    End If

                    Dim rol As String = String.Empty
                    @If (rolActual IsNot Nothing) Then
                        rol = rolActual.DescripcionRol
                    End If
                End Code

                @Html.Raw(Utils.GenerarMenu(regenerarMenu, cargaInicial, Model.IdUser, rolActual))
            </ul>
            <ul class="nav navbar-nav navbar-right">
                <li><a href="@Url.Action("Salir", "Login")"><span class='glyphicon glyphicon-off'></span> @Utils.Traducir("Salir")</a></li>
            </ul>
            @*<p class="navbar-text navbar-right">@Html.Raw(String.Format("{0}: <b>{1}</b>{2}{3}", Utils.Traducir("usuarioConectado"), Model.NombreCompleto, If(rolActual IsNot Nothing AndAlso Not String.IsNullOrEmpty(rolActual.Planta), " (" & rolActual.Planta & ")", String.Empty), If(Not String.IsNullOrEmpty(rol), " (" & rol & ")", String.Empty)))</p>*@
            <p class="navbar-text navbar-right"><span class='glyphicon glyphicon glyphicon-user'></span>&nbsp;@Html.Raw(String.Format("<b>{0}</b>{1}{2}", Model.NombreCompleto, If(rolActual IsNot Nothing AndAlso Not String.IsNullOrEmpty(rolActual.Planta), " (" & rolActual.Planta & ")", String.Empty), If(Not String.IsNullOrEmpty(rol), " (" & rol & ")", String.Empty)))</p>
        </div>
    </div>
</nav>
