@*@ModelType IEnumerable(Of String())*@
@ModelType IEnumerable(Of OBJETIVOPROCESO)
@Code
    ViewData("Title") = "Objetivos Proceso"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<br />
<h2>Objetivos de Proceso</h2>
<br />

<p>
    <div style="text-align:center">
        <button type="button" class="btn btn-info" onclick="window.location.href = '@Url.Action("CreateObjetivoProceso")';" style="font-size:18px">
            <i class="glyphicon glyphicon-plus" style="margin-right: 10px;"></i>Crear nuevo objetivo
        </button>
    </div>
</p>

<table class="table resultTable" id="objetivosprocesoTable">
    <tr style="background-color:#337ab7;color:white">
        <th>Año</th>
        <th>Repetitivas (%)</th>
        <th>Días acciones inmediatas</th>
        <th>Días acciones correctivas</th>
        <th></th>
    </tr>
    @For Each item In Model
        @*@Code
            Dim anno = item(0)
            Dim rep = item(1)
            Dim dias14 = item(2)
            Dim dias56 = item(3)
        End code*@

        @<tr>
    @*<td>@anno</td>
    <td>@rep</td>
    <td>@dias14</td>
    <td>@dias56</td>*@

    <td>@item.ANNO</td>
    <td>@item.REPETITIVAS</td>
    <td>@item.DIAS14</td>
    <td>@item.DIAS56</td>

    <td style="width:196px;">
        <div class="btn-group" role="group" aria-label="..." style="width:180px;">
            <button class="btn btn-default" onclick="location.href = '@Url.Action("EditObjetivoProceso", New With {.id = item.ID})';event.stopPropagation();" title="Editar" data-toggle="tooltip" data-container="body">
                <span class="glyphicon glyphicon-pencil" style="color:royalblue" aria-hidden="true"></span>
            </button>
            <button class="btn btn-default" onclick="if(confirm('Estás seguro de que deseas borrar este objetivo?')){location.href = '@Url.Action("DeleteObjetivoProceso", New With {.id = item.ID})'};event.stopPropagation();" title="Borrar" data-toggle="tooltip">
                <span class="glyphicon glyphicon-remove" style="color:red" aria-hidden="true"></span>
            </button>
        </div>
    </td>

</tr>
    Next
</table>

