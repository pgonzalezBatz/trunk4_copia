@Imports Microsoft.AspNet.Identity

@If Request.IsAuthenticated Then
    @<ul class="nav navbar-nav navbar-right">
    <li>
        <a href='@Url.Action("Index", "Home")' title="Cambio de planta" data-toggle="tooltip" data-placement="bottom" id="cambioPlanta"><span class="glyphicon glyphicon-globe" style="margin-right:5px"></span>@NombrePlanta </a>
    </li>
    <li>
        <a style="padding-right:20px;"><span class="glyphicon glyphicon-user" style="margin-right:5px"></span>@NombreUsuario</a>
    </li>
    </ul>
End If


@helper NombreUsuario()
    Dim cookie As HttpCookie = Request.Cookies(FormsAuthentication.FormsCookieName)
    If cookie IsNot Nothing Then
        Dim ticket = FormsAuthentication.Decrypt(cookie.Value)
        Dim Nombre As String = If(String.IsNullOrWhiteSpace(ticket.UserData), User.Identity.GetUserName(), ticket.UserData)
        @Nombre
    End If
End Helper

@helper NombrePlanta()
    Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
    If aCookie IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(aCookie.Values("Planta_Nombre")) Then
        @aCookie.Values("Planta_Nombre")
    Else
        Dim planta As String = "Sin planta"
        @planta
    End If
End Helper


