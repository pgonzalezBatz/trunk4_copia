@Imports DOBLib

<h3>@Utils.Traducir("Mis revisiones")</h3>
<hr />

@code
    Dim listaRevisiones As List(Of ELL.Revision) = CType(ViewData("Revisiones"), List(Of ELL.Revision))
End Code

<script type="text/javascript">
    $(function () {
        $(".boton-eliminar").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea eliminarto seleccionado?"))");
        });
    });
</script>

<a href="@Url.Action("Agregar")" Class="btn btn-info">
    <span Class="glyphicon glyphicon-plus"></span> @Utils.Traducir("Nuevo")
</a>
<br /><br />

@Using Html.BeginForm("MisRevisiones", "Revisiones", FormMethod.Post)
    @<div Class="panel panel-default">
        <div Class="panel-heading">
            <h3 Class="panel-title">@Utils.Traducir("Filtros de búsqueda")</h3>
        </div>
        <div Class="panel-body">
            <div class="form-inline">                
                <div Class="form-group">
                    <label>@Utils.Traducir("Ejercicio")</label>
                    @Html.DropDownList("Ejercicios", Nothing, New With {.class = "form-control"})
                </div>
                <input type="submit" id="submit" value="@Utils.Traducir("Buscar")" Class="btn btn-primary form-control" />
            </div>
        </div>
    </div>
End Using

@If (listaRevisiones.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<table class="table table-striped table-hover table-condensed">
        <thead>
            <tr>
                <th>@Utils.Traducir("Objetivo")</th>
                <th>@Utils.Traducir("Revisión")</th>
                <th class="text-center">@Utils.Traducir("¿Año siguiente?")</th>
                <th class="text-center">@Utils.Traducir("Ejercicio")</th>
                <th></th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @code
                For Each revision In listaRevisiones
                    @<tr>
                        <td>
                            @revision.DescripcionObjetivo
                        </td>
                        <td>
                            @revision.DescripcionRevison
                        </td>
                        <td class="text-center">
                            @Html.CheckBox("ascendente", revision.PAAñoSiguiente, New With {.disabled = "disabled"})
                        </td>
                        <td class="text-center">
                            @revision.AñoObjetivo
                        </td>
                        <td Class="text-center">
                            <a href='@Url.Action("Editar", "Revisiones", New With {.idObjetivo = revision.IdObjetivo, .revision = revision.Revision})'>
                                <span class="glyphicon glyphicon-pencil" aria-hidden="True" title="@Utils.Traducir("Editar")"></span>
                            </a>
                        </td>
                        <td Class="text-center">
                            <a href='@Url.Action("Eliminar", "Revisiones", New With {.idObjetivo = revision.IdObjetivo, .revision = revision.Revision})'>
                                <span class="glyphicon glyphicon-remove boton-eliminar" aria-hidden="True" title="@Utils.Traducir("Dar de baja")"></span>
                            </a>
                        </td>
                    </tr>
                Next
            End Code
        </tbody>
    </table>
End if

