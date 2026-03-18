@Imports CostCarriersLib

@Code
    Dim cabeceras As List(Of ELL.CabeceraCostCarrier) = CType(ViewData("CabecerasProyecto"), List(Of ELL.CabeceraCostCarrier))
End Code

<h3><label>@Utils.Traducir("Estado actual e histórico")</label></h3>
<hr />

@Using Html.BeginForm("IndexHistorico", "Validaciones", FormMethod.Post)
    @<div Class="panel panel-default">
        <div Class="panel-heading">
            <h3 Class="panel-title">@Utils.Traducir("Filtros de búsqueda")</h3>
        </div>
        <div Class="panel-body">
            <div class="form-horizontal">
                <div Class="form-group">
                    <Label class="col-sm-1 control-label">@Utils.Traducir("Producto")</Label>
                    <div class="col-sm-3">
                        @Html.DropDownList("Productos", Nothing, New With {.class = "form-control"})
                    </div>
                    <Label class="col-sm-2 control-label">@Utils.Traducir("Estado cost carrier")</Label>
                    <div class="col-sm-3">
                        @Html.DropDownList("EstadoCostCarrier", Nothing, New With {.class = "form-control"})
                    </div>
                    <div class="col-sm-3">
                        <input type="submit" id="submit" value="@Utils.Traducir("Buscar")" Class="btn btn-primary form-control" />
                    </div>
                </div>
            </div>
        </div>
    </div>
End Using

@code
    If (cabeceras.Count > 0) Then
        @<div class="row">
            <div class="col-sm-12">
                <table id="tabla" class="table table-condensed table-striped table-hover">
                    <thead>
                        <tr>
                            <th>@Utils.Traducir("Producto")</th>
                            <th>@Utils.Traducir("Proyecto")</th>
                            <th>@Utils.Traducir("Código")</th>
                            <th></th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        @For Each cabecera In cabeceras
                            @<tr>
                                <td>@cabecera.Producto</td>
                                <td>@cabecera.NombreProyecto</td>
                                <td>@cabecera.CodigoProyecto</td>
                                <td Class="text-center">
                                    <a href='@Url.Action("DetalleProyectoHistorico", "Validaciones", New With {.idCabecera = cabecera.Id})'>
                                        <span class="glyphicon glyphicon-option-horizontal text-danger" aria-hidden="true" title="@Utils.Traducir("Estado actual e histórico")"></span>
                                    </a>
                                </td>
                                <td Class="text-center">
                                    <a href='@Url.Action("DetalleProyecto", "Totales", New With {.idCabecera = cabecera.Id})'>
                                        <span class="glyphicon glyphicon-ok-circle text-success" aria-hidden="true" title="@Utils.Traducir("Totales por proyecto")"></span>
                                    </a>
                                </td>
                            </tr>
                        Next
                    </tbody>
                </table>
            </div>
        </div>
    Else
        @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
    End If
End code
