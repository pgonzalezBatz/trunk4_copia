@ModelType web_extranet.CERTIFICADO

    <h3>@h.traducir("Estas segur@ que quieres eliminar El certificado seleccionado")?</h3>
        
    <form action="" method="post">
        <div class="row">
            <div class="col-sm-2">
                <input type="submit" class="btn btn-primary" value="@h.traducir("Eliminar")" />
            </div>
            <div class="col-sm-2">
                <a  href="@Url.Action("edit3", h.ToRouteValuesDelete(Request.QueryString, "idcertificado"))" class="btn btn-link">@h.traducir("Volver")</a>
            </div>
        </div>
    </form>
        