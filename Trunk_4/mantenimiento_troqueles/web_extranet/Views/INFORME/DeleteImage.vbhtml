@ModelType web_extranet.FOTOS

    <h3>@h.traducir("Estas segur@ que quieres eliminar la foto seleccionada")?</h3>
        
    <div Class="thumbnail">
    <img src="@Url.Action("photo", New With {.id = Model.IDFOTO})" alt="@h.traducir("Imagen adjuntada por usuario")">
        </div>  
    <form action="" method="post">
        <div class="row">
            <div class="col-sm-2">
                <input type="submit" class="btn btn-primary" value="@h.traducir("Eliminar")" />
            </div>
            <div class="col-sm-2">
                <a  href="@Url.Action("editimages", h.ToRouteValuesDelete(Request.QueryString, "idphoto"))" class="btn btn-link">@h.traducir("Volver")</a>
            </div>
        </div>
    </form>
        