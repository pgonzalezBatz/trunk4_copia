@imports web
<h4>@h.traducir("Cambiar contraseña")</h4>
<form action="" method="post">
    @Html.ValidationSummary()
    <div class="form-group">
        <label>
            <strong>@h.traducir("Nueva contraseña")</strong><br />

        </label>
        @Html.Password("pwd1", Nothing, New With {.class = "form-control"})
    </div>
    <div class="form-group">
        <label>
            @h.traducir("Repita la contraseña")
        </label>
        @Html.Password("pwd2", Nothing, New With {.class = "form-control"})
        </div>
        <input type="submit" value="@h.traducir("Cambiar contraseña")" class="btn btn-primary"/>
</form>