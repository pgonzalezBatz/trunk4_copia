@ModelType IEnumerable(Of INFORMES)
<div class="row">
    <div class="col-sm-12">
        <div class="alert alert-success" role="alert">
            <strong>@h.traducir("Informes guardados correctamente")</strong>
        </div>
    </div>
</div>

<div class="row">
    <div class="col-sm-12">
       <h3>Ruta: <a href="@Request("path")">@Request("path")</a></h3>
    </div>
</div>
<div class="row">
    <div class="col-sm-12">
        <strong><a href="@Url.Action("index", h.ToRouteValuesDelete(Request.QueryString, "path"))">@h.traducir("Volver a la busqueda")</a></strong>
        </div>
    </div>

