@imports web
@Html.ValidationSummary()

<h4>@h.traducir("Cambiar contraseña")</h4>
<form action="" method="post" class="form-group">
        <div class="form-group">
            <label>
                @h.traducir("Nueva contraseña")
            </label>
            @Html.Password("password", Nothing, New With {.class = "form-control"})
</div>
        <div class="form-group">
            <label>
                @h.traducir("Repetir nueva contraseña")
              </label>
            @Html.Password("password2", Nothing, New With {.class = "form-control"})
</div>
            <input type="submit" value="@h.traducir("Cambiar contraseña")" class="btn btn-primary" />
</form>