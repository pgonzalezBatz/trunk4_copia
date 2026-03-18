@Imports DOBLib

@code
    Dim listaAcciones As List(Of ELL.Accion) = CType(ViewData("Acciones"), List(Of ELL.Accion))
    Dim sinLayout As Boolean = ViewData.ContainsKey("SinLayout")
    Dim soloAcciones As Boolean = ViewData.ContainsKey("SoloAcciones")
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)

    If (sinLayout) Then
        Layout = Nothing
    End If
End Code

@If (Not sinLayout) Then
    @<h3>@Utils.Traducir("Mis acciones")</h3>
    @<hr />
End If

<script type="text/javascript">
    $(function () {
        $(".boton-eliminar").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea eliminar el elemento seleccionado?"))");
        });

        $("#plazoDesde, #plazoHasta").datetimepicker({
            showClear: true,
            locale: '@ticket.Culture',
            format: '@Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper'
        });
    });

</script>

@If (Not sinLayout) Then
    @Using Html.BeginForm("MisAcciones", "Acciones", FormMethod.Post)
        @<div Class="panel panel-default">
            <div Class="panel-heading">
                <h3 Class="panel-title">@Utils.Traducir("Filtros de búsqueda")</h3>
            </div>
            <div Class="panel-body">
                <div class="form-inline">
                    <div Class="form-group">
                        <label>@Utils.Traducir("Objetivo")</label>
                        @Html.DropDownList("Objetivos", Nothing, New With {.class = "form-control"})
                    </div>
                    <div Class="form-group">
                        <Label>@Utils.Traducir("Plazo desde")</Label>
                        @Html.TextBox("plazoDesde", String.Empty, New With {.class = "form-control"})
                    </div>
                    <div Class="form-group">
                        <Label>@Utils.Traducir("hasta")</Label>
                        @Html.TextBox("plazoHasta", String.Empty, New With {.class = "form-control"})
                    </div>
                    <input type="submit" id="submit" value="@Utils.Traducir("Buscar")" Class="btn btn-primary form-control" />
                </div>
            </div>
        </div>  End Using
End If

@If (listaAcciones.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<table class="table table-striped table-hover table-condensed">
        <thead>
            <tr>
                @code
                    If (soloAcciones) Then
                        @<th Class="hidden-xs">@Utils.Traducir("Objetivo")</th>
                    End If
                End Code
                <th>@Utils.Traducir("Descripción")</th>
                <th class="text-center">@Utils.Traducir("Plazo")</th>
                <th class="text-right">@Utils.Traducir("Porcentaje realización")</th>
                <th class="text-right">@Utils.Traducir("Grado importancia")</th>
                <th></th>
                <th></th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @code
                For Each accion In listaAcciones
                    @<tr>
                        @code
                            If (soloAcciones) Then
                                @<td class="hidden-xs">@accion.DescripcionObjetivo</td>
                            End If
                        End Code
                        <td>@accion.Descripcion</td>
                        <td class="text-center">@accion.FechaObjetivo.ToShortDateString()</td>
                        <td class="text-right">@String.Format("{0}%", accion.Porcentaje)</td>
                        <td class="text-right">@String.Format("{0}%", accion.GradoImportancia)</td>
                        <td class="text-center">
                            <a href='@Url.Action("Editar", "Acciones", New With {.idAccion = accion.Id})'>
                                <span class="glyphicon glyphicon-pencil" aria-hidden="true" title="@Utils.Traducir("Editar")"></span>
                            </a>
                        </td>
                        <td class="text-center">
                            <a href='@Url.Action("Eliminar", "Acciones", New With {.idAccion = accion.Id})'>
                                <span class="glyphicon glyphicon-remove boton-eliminar" aria-hidden="true" title="@Utils.Traducir("Eliminar")"></span>
                            </a>
                        </td>
                        <td class="text-center">
                            <a href='@Url.Action("Editar", "EvolucionAcciones", New With {.idAccion = accion.Id})'>
                                <span class="glyphicon glyphicon-signal" aria-hidden="true" title="@Utils.Traducir("Editar evolución acción")"></span>
                            </a>
                        </td>
                    </tr>
                            Next
            End Code
        </tbody>
    </table>

                            End If
