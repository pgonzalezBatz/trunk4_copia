@Code
    ViewBag.Title = "Confirmación de restablecimiento de contraseña"
End Code

<hgroup class="title">
    <h2>@ViewBag.Title.</h2>
</hgroup>
<div>
    <p>
        Se restableció la contraseña. @Html.ActionLink("Haga clic aquí para iniciar sesión", "Login", "Account", routeValues:=Nothing, htmlAttributes:=New With {Key .id = "loginLink"})
    </p>
</div>
