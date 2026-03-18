@Imports TarjetasVisitaLib

@code
    Dim datosAlternativos As List(Of ELL.DatosAlternativos) = CType(ViewData("DatosAlternativos"), List(Of ELL.DatosAlternativos))
End Code

<script type="text/javascript">
    $(function () {
        $(".boton-eliminar").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea eliminar el elemento seleccionado?"))");
        });
    })
</script>

<h3>@Utils.Traducir("Datos alternativos de trabajadores")</h3>
<hr />

@*@Using Html.BeginForm("MisSolicitudes", "Solicitud", FormMethod.Post)
    @<div Class="panel panel-primary">
        <div Class="panel-heading">
            <h3 Class="panel-title">@Utils.Traducir("Filtros de búsqueda")</h3>
        </div>
        <div Class="panel-body">
            <div class="form-horizontal">
                <div Class="form-group">
                    <Label class="col-sm-1 control-label">@Utils.Traducir("Estado")</Label>
                    <div class="col-sm-2">
                        @Html.DropDownList("FiltroEstadoSolicitud", Nothing, New With {.class = "form-control"})
                    </div>
                    <Label class="col-sm-2 control-label">@Utils.Traducir("Compañía gestora")</Label>
                    <div class="col-sm-2">
                        @Html.DropDownList("FiltroCompaniaGestora", Nothing, New With {.class = "form-control"})
                    </div>
                    <Label class="col-sm-1 control-label">@Utils.Traducir("Tipo solicitud")</Label>
                    <div class="col-sm-2">
                        @Html.DropDownList("FiltroTipoSolicitud", Nothing, New With {.class = "form-control"})
                    </div>
                    <div class="col-sm-2">
                        <input type="submit" id="submit" value="@Utils.Traducir("Buscar")" Class="btn btn-primary form-control" />
                    </div>
                </div>
            </div>
        </div>
    </div>  End Using*@

<a href="@Url.Action("Agregar")" Class="btn btn-info">
    <span Class="glyphicon glyphicon-plus"></span> @Utils.Traducir("Nuevo")
</a>
<br /><br />

@If (datosAlternativos.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<table Class="table table-striped table-hover table-condensed" style="margin-top:10px;">
        <thead>
            <tr>
                <th>@Utils.Traducir("Descripción")</th>
                <th>@Utils.Traducir("Trabajador")</th>
                <th>@Utils.Traducir("Nombre")</th>
                <th>@Utils.Traducir("Puesto")</th>
                <th>@Utils.Traducir("Movil")</th>
                <th>@Utils.Traducir("Dirección")</th>
                <th>@Utils.Traducir("Fijo")</th>
                <th>@Utils.Traducir("Email")</th>
                <th></th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @code
                For Each dato In datosAlternativos
                    @<tr>
                        <td>@dato.Descripcion</td>
                        <td>@dato.NombreUsuario</td>
                        <td>@dato.Nombre</td>
                        <td>@dato.Puesto</td>
                        <td>@dato.Movil</td>
                        <td>@dato.Direccion</td>
                        <td>@dato.Fijo</td>
                        <td>@dato.Email</td>
                        <td>
                            <a href='@Url.Action("Editar", "DatosAlternativos", New With {.id = dato.Id})'>
                                <span class="glyphicon glyphicon-edit" aria-hidden="True" title="@Utils.Traducir("Editar datos")"></span>
                            </a>
                        </td>
                        <td>
                            <a href='@Url.Action("Eliminar", "DatosAlternativos", New With {.id = dato.Id})'>
                                <span class="glyphicon glyphicon-trash boton-eliminar" aria-hidden="True" title="@Utils.Traducir("Eliminar datos")"></span>
                            </a>
                        </td>
                    </tr>
                Next
            End Code
        </tbody>
    </table>
End If