@imports web
<div class="container">
    <h4>@h.traducir("Introducir usuario para el cual quieres regenerar la contraseña")</h4>
    <form action="" method="post">
        @Html.ValidationSummary()
        <div class="form-group">
            <label>
                @h.traducir("Usuario")
            </label>
            @Html.TextBox("usuario", Nothing, New With {.class = "form-control"})
        </div>
        <div class="form-group">
            <input type="submit" value="@h.traducir("Regenerar contraseña")" class="btn btn-primary" />
            </div>
</form>
</div>