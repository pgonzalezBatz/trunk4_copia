@ModelType BezerreSis.OBJETIVOS
@Code
    ViewData("Title") = "Details"
End Code

<br />
<h2>Detalles objetivo</h2>
<br />
<div>
    @*<h4>OBJETIVOS</h4>*@
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.FECHA)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.FECHA)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.PPM_ANUAL)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.PPM_ANUAL)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.PPM_MENSUAL)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.PPM_MENSUAL)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.IPB_SEMESTRAL)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.IPB_SEMESTRAL)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.CLIENTES.NOMBRE)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.CLIENTES.NOMBRE)
        </dd>

    </dl>
</div>
<div style="text-align:center">
    <div class="btn-group">
        <button class="btn btn-default" onclick="window.location.href = '@Url.Action("Edit", New With {.id = Model.ID})'" title="Editar" data-toggle="tooltip">
            <span class="glyphicon glyphicon-pencil" style="color:royalblue" aria-hidden="true"></span>
        </button>
        <button class="btn btn-default" onclick="if(confirm('Are you sure you want to delete this objective?')){location.href = '@Url.Action("DeleteObjetivo", New With {.id = Model.ID, .lClientes = ViewBag.lClientes_Selected_obj})'}" title="Borrar" data-toggle="tooltip">
            <span class="glyphicon glyphicon-remove" style="color:red" aria-hidden="true"></span>
        </button>
        <button class="btn btn-default" onclick="window.location.href = '@Url.Action(TempData.Peek("ReturnUrl"))'" title="Volver al Listado" data-toggle="tooltip">
            <span class="glyphicon glyphicon-backward" aria-hidden="true"></span>
        </button>
    </div>
</div>