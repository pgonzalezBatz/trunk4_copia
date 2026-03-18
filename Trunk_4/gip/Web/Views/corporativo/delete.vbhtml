@imports web
@section title
    - @h.traducir("Busqueda")
End section
@section menu1
    @Html.Partial("menu")
End Section

<div class="row">
    <div class="col-sm-12">
        <h3>@h.traducir("Datos generales proveedor global")</h3>
        <form action="" method="post">
            <label>@h.traducir("CIF")</label>
            @Model.cif
            <br />
            <label>@h.traducir("Nombre")</label>
            @Model.nombre
            <br />
            <label>@h.traducir("Localidad")</label>
            @Model.localidad
            <br />
            <label>@h.traducir("Provincia")</label>
            @Model.provincia
            <br />
            <input type="submit" class="btn btn-primary" value="@h.traducir("Eliminar proveedor global")" />
        </form>
    </div>
</div>