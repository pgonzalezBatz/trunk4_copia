    @imports web
@Code
    ViewBag.title = h.traducir("Editar puntuación")
End Code

<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Introducir nueva puntuación")</legend>
        <strong>@h.traducir("Titulo pregunta")</strong><br />
        @ViewData("respuesta").tituloPregunta
        <br />
        <strong>@h.traducir("Colaborador") </strong><br />
        @ViewData("usuario").nombre @ViewData("usuario").apellido1 @ViewData("usuario").apellido2   
        <br />
        <strong>@h.traducir("Puntuación actual")</strong><br />
        @ViewData("respuesta").puntuacion
        <br />
        
        <label>
            @h.traducir("Nueva puntuación")<br />
            @Html.TextBox("puntuacion")
            <span>@h.traducir("puntuación maxima adminida") @ViewData("respuesta").puntuacionmax</span>
        </label>
        <br />
        <input type="submit" value="@h.traducir("Guardar")" />
    </fieldset>
</form>    