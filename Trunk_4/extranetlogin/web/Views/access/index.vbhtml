@imports web
<h4>@h.traducir("Introducir usuario y contraseña para acceder a la extranet de Batz")</h4>
<form action="" method="post" class="form-group">
    @Html.ValidationSummary("", New With {.class = "has-error alert alert-danger"})
        <div class="form-group">
            <label>
                <strong>@h.traducir("Usuario")</strong><br />
            </label>
            @Html.TextBox("usuario", Nothing, New With {.class = "form-control", .autofocus = ""})
</div>
        <div class="form-group">
            <label>
                <strong>@h.traducir("Contraseña")</strong><br />

            </label>
            @Html.Password("contraseña", Nothing, New With {.class = "form-control"})
</div>
        <input type="submit" value="@h.traducir("Entrar")" class="btn btn-primary" />
        <a href="@Url.Action("resetpassword")" style="margin-left:2em;">@h.traducir("Se me ha olvidado la contraseña")</a>
</form>