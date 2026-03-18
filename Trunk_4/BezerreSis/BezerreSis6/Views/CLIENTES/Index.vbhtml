@ModelType IEnumerable(Of BezerreSis.CLIENTES)
@Code
    ViewData("Title") = "Listado clientes"
End Code

<br />
<h2>Listado clientes</h2>
<p>
    <div style="text-align:center">
        <button type="button" class="btn btn-info" onclick="window.location.href = '@Url.Action("Create")'" style="font-size:18px">
            <i class="glyphicon glyphicon-plus" style="margin-right: 10px;"></i>Nuevo cliente
        </button>
    </div>
</p>

@Html.ValidationSummary(True, "", New With {.class = "text-danger"})
<table class="table">
    <tr style="background-color:#337ab7;color:white">
        <th>
            @Html.DisplayNameFor(Function(model) model.NOMBRE)
        </th>
        <th></th>
    </tr>
    @For Each item In Model
        @<tr onclick="location.href = '@Url.Action("Details", New With {.id = item.ID})'" title="Detalle" data-toggle="tooltip" data-placement="left" style="height:50px">
            <td>
                @Html.DisplayFor(Function(modelItem) item.NOMBRE)
            </td>
            <td style="width:136px;">
                @If Not item.NOMBRE.Contains("default") Then
                    @<div Class="btn-group" role="group" aria-label="..." style="width:120px;">
                        <button class="btn btn-default" onclick="location.href = '@Url.Action("Edit", New With {.id = item.ID})';event.stopPropagation();" title="Editar" data-toggle="tooltip">
                            <span class="glyphicon glyphicon-pencil" style="color:royalblue" aria-hidden="true"></span>
                        </button>
                        <button class="btn btn-default" onclick="window.location.href = '@Url.Action("Cli_Pro", New With {.id = item.ID})';event.stopPropagation();" title="Agregar Producto" data-toggle="tooltip">
                            <span class="glyphicon glyphicon-import" style="color:green" aria-hidden="true"></span>
                        </button>
                        <button class="btn btn-default" onclick="if(confirm('Are you sure you want to delete this client?')){location.href = '@Url.Action("Delete", New With {.id = item.ID})'};event.stopPropagation();" title="Borrar" data-toggle="tooltip">
                            <span class="glyphicon glyphicon-remove" style="color:red" aria-hidden="true"></span>
                        </button>
                    </div>
                End If
            </td>
        </tr>
    Next
</table>

@Section Scripts
    <script>
        $(document).on('show.bs.tooltip', function (e) {
            $('.tooltip').not(e.target).hide();
        });
    </script>
End Section