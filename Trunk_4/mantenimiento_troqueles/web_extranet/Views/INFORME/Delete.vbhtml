@ModelType web_extranet.INFORMES
@Code
    ViewData("Title") = "Delete"
End Code

<h3>@h.traducir("Eliminación de informe")</h3>
<hr />

<h4>@h.traducir("¿Estas seguro de que deseas eliminar este informe?")</h4>


<div class="row">
    <div class="col-sm-6">
        <h4>@h.traducir("Datos seleccionados")</h4>
    </div>
</div>
<div class="row">
    <div class="col-sm-4">
        <label>@h.traducir("Tipo informe seleccionado")</label>
        <p>@Model.TIPOINFORME</p>
    </div>
    <div class="col-sm-4">
        <label>@h.traducir("Cliente")</label>
        <p>@Model.CLIENTE</p>
    </div>
    <div class="col-sm-4">
        <label>@h.traducir("Proyecto")</label>
        <p>@Model.proyecto</p>
    </div>
</div>

<div class="row">
    <div class="col-sm-4">
        <label>@h.traducir("OF - OP seleccionada")</label>
        <p>@Model.valorof - @Model.VALOROP</p>
    </div>
    <div Class="col-sm-4">
        <label>@h.traducir("Nº pieza")</label>
        <p>@Model.NPIEZA</p>
    </div>
    <div Class="col-sm-4">
        <label>@h.traducir("Descripción pieza")</label>
        <p>@Model.DESCPIEZA</p>
    </div>
</div>
<div class="row">
    <div class="col-sm-4">
        <label>@h.traducir("Marcas")</label>
        <p>
            @Html.Encode(" | ")
            @For Each m In ViewData("marca")
                @m.marca @Html.Encode(" | ")
            Next
        </p>
    </div>
    <div Class="col-sm-4">
        <label>@h.traducir("Nº de troquel")</label>
        <p>@Model.NTROQUEL</p>
    </div>
    <div Class="col-sm-4">
        <label>@h.traducir("Material")</label>
        <p>@ViewData("comunesMarca").material</p>
    </div>
</div>
<div class="row">
    <div class="col-sm-4">

    </div>
    <div Class="col-sm-4">
        <label>@h.traducir("Tratamiento Requerido")</label>
        <p>@ViewData("comunesMarca").tratamiento</p>
    </div>
    <div Class="col-sm-4">
        <label>@h.traducir("Dureza Material") <span class="glyphicon glyphicon-info-sign text-info" title="@h.traducir("Es la dureza del material suministrado")"></span></label>
        <p>@ViewData("comunesMarca").dureza</p>
    </div>
</div>
<div class="row">
    <div class="col-sm-4">

    </div>
    <div Class="col-sm-4">
        <label>@h.traducir("Tratamiento Secundario")</label>
        <p>@ViewData("comunesMarca").tratamientosecundario</p>
    </div>
    <div Class="col-sm-4">
    </div>
</div>












    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="@h.traducir("Eliminar Informe")" class="btn btn-primary" /> |
            @Html.ActionLink(h.traducir("Volver sin guardar"), "Index")
        </div>
    End Using
