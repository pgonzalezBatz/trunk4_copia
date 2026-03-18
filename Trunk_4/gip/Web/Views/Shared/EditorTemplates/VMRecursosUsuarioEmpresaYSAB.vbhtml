@imports web
@ModelType  VMRecursosUsuarioEmpresaYSAB


<div class="row">
    <div class="col-xs-6">
        <h4>
            <strong>@h.Traducir("Contacto Planta")</strong>
        </h4>
        <h4>
            <strong>@Model.UsuarioEmpresa</strong>
        </h4>
        <form action="@Url.Action("UpdaterecursosUsuarioEmpresa", h.ToRouteValues(Request.QueryString, Nothing))" method="post">
            @Html.EditorFor(Function(m) m.ListOfUsuarioEmpresa)
            <div class="form-group">
                <input type="submit" value="@h.Traducir("Guardar Contacto Planta")" class="btn btn-primary " />
            </div>

        </form>
    </div>
    <div class="col-xs-6">
        <h4>
            <strong>@h.Traducir("Usuario Administrador")</strong>
        </h4>
        <h4>
            <strong>@Model.usuarioSabProveedores</strong>
        </h4>
        <form action="@Url.Action("UpdaterecursosSabProveedor", h.ToRouteValues(Request.QueryString, Nothing))" method="post">
            @Html.EditorFor(Function(m) m.ListOfUsuarioSABProveedor)
            <input type="submit" value="@h.Traducir("Guardar Usuario Adminstrador")" class="btn btn-primary " />
        </form>
    </div>
</div>

<div class="row">
    <div class="col-sm-3">

    </div>
    <div class="col-sm-4">
        @Html.ActionLink(h.Traducir("Volver al listado"), "search", h.ToRouteValues(Request.QueryString, Nothing))
    </div>
</div>