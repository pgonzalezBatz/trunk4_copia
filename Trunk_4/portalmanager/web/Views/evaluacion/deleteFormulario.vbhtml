    @imports web

    <p>
        @h.traducir("Debido a que los resultados de los formularios son estaticos en el tiempo"),
        @h.traducir("es imposible modificar el formulario").
        @h.traducir("La unica opcion es eliminar el formulario y volverlo a rellenar")
    </p>
    <strong>@h.traducir("¿Estas seguro de que quieres eliminar el formlario?")</strong>
    <br />
    <a href="@Url.Action("listadoColaboradores")" class="big_link">@h.traducir("No, quiero volver sin eliminar")</a>
    <form action="" method="post" style="display:inline;">
        <input id="confirmation" type="submit" name="confirmation" onclick="return confirm('@h.traducir("¿Estad seguro de que quieres eliminar?")')" value="@h.traducir("Eliminar")" />
    </form>