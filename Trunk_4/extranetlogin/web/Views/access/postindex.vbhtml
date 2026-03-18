@imports web

<h4>@h.Traducir("Introducir el código recibido en el email")</h4>
<form action="" method="post" class="form-group">
    @Html.ValidationSummary("", New With {.class = "has-error alert alert-danger"})
    <div class="form-group">
        <label>
            <strong>@h.traducir("Usuario")</strong><br />
        </label>
        @Html.TextBox("usuario", Model.ToString(), New With {.class = "form-control"})
    </div>
    <div class="form-group">
        <label>
            <strong>@h.Traducir("Código")</strong><br />

        </label>
        @Html.Password("contraseña", Nothing, New With {.class = "form-control", .autofocus = ""})
    </div>
    <input type="submit" value="@h.traducir("Entrar")" class="btn btn-primary" />
    @*<a href="@Url.Action("resetpassword")" style="margin-left:2em;">@h.traducir("Se me ha olvidado la contraseña")</a>*@
</form>