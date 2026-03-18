@ModelType BezerreSis.CLIENTES
@Code
    ViewData("Title") = "Details"
End Code

<br />
<h2>Detalles cliente</h2>
<br />
<div>
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.NOMBRE)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.NOMBRE)
        </dd>
        <dt>Productos </dt>
        <dd></dd>
        @For Each item In Model.CLIPRO.Where(Function(o) o.PRODUCTOS IsNot Nothing).Select(Function(o) o.PRODUCTOS)
            @<dt>&nbsp;</dt>
                @<dd>
                @Html.DisplayFor(Function(modelItem) item.NOMBRE)
            </dd>
        Next
    </dl>
</div>
<div style="text-align:center">
    <div class="btn-group">
        <button class="btn btn-default" onclick="window.location.href = '@Url.Action("Edit", New With {.id = Model.ID})'" title="Editar" data-toggle="tooltip">
            <span class="glyphicon glyphicon-pencil" style="color:royalblue" aria-hidden="true"></span>
        </button>
        <button class="btn btn-default" onclick="window.location.href = '@Url.Action("Cli_Pro", New With {.id = Model.ID})'" title="Agregar Producto" data-toggle="tooltip">
            <span class="glyphicon glyphicon-import" style="color:green" aria-hidden="true"></span>
        </button>
        <button class="btn btn-default" onclick="if(confirm('Are you sure you want to delete this client?')){location.href = '@Url.Action("Delete", New With {.id = Model.ID})'}" title="Borrar" data-toggle="tooltip">
            <span class="glyphicon glyphicon-remove" style="color:red" aria-hidden="true"></span>
        </button>
        <button class="btn btn-default" onclick="window.location.href = '@Url.Action(TempData.Peek("ReturnUrl"))'" title="Volver al Listado" data-toggle="tooltip">
            <span class="glyphicon glyphicon-backward" aria-hidden="true"></span>
        </button>
    </div>
</div>