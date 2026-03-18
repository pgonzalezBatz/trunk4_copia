@imports web

<form action="" method="post">
    <fieldset>
        <legend>@h.traducir("Añadir respuesta posible")</legend>
        <label>
            @h.traducir("Puntuación")                            
            <br />
            @Html.TextBox("puntuacion")
        </label>
        <br />
        <label>
            @h.traducir("Descripción")
            <br />
            @Html.TextArea("descripcion")
        </label>
        <br />
    </fieldset>
    <input type="submit" value="@h.traducir("Guardar")" />
</form>