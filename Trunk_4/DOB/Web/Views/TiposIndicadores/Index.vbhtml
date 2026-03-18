@Imports DOBLib

<script type="text/javascript">
    $(function () {
        $(".boton-eliminar").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea eliminar el elemento seleccionado?"))");
        });
    });

</script>

<h3>@String.Format("{0} - {1}", Utils.Traducir("Administración"), Utils.Traducir("Tipos indicadores"))</h3>
<hr />

@code
    Dim listaTiposIndicadores As List(Of ELL.TipoIndicador) = CType(ViewData("TiposIndicadores"), List(Of ELL.TipoIndicador))
End Code

@If (listaTiposIndicadores.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<div class="row">
        <div class="col-sm-6">
            <table class="table table-striped table-hover table-condensed">
                <thead>
                    <tr>
                        <th>@Utils.Traducir("Nombre")</th>
                        <th>@Utils.Traducir("Descripción")</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @code
                        For Each tipoIndicador In listaTiposIndicadores
                            @<tr>
                                <td>@tipoIndicador.Nombre</td>
                                <td>@tipoIndicador.Descripcion</td>
                                <td class="text-center">
                                    <a href='@Url.Action("Eliminar", "TiposIndicadores", New With {.id = tipoIndicador.Id})'>
                                        <span class="glyphicon glyphicon-remove boton-eliminar" aria-hidden="true" title="@Utils.Traducir("Eliminar")"></span>
                                    </a>
                                </td>
                            </tr>
                        Next
                    End Code
                </tbody>
            </table>
        </div>
    </div>
                        End If

<div class="row">
    <div class="col-sm-6">
        <hr />
    </div>
</div>

<div class="row">
    <div class="col-sm-12">
        @Using Html.BeginForm("Añadir", "TiposIndicadores", FormMethod.Post, New With {.class = "form-horizontal"})
            @<h4>@Utils.Traducir("Añadir tipo indicador")</h4>
            @<div Class="form-group">
                <label class="col-sm-2 control-label">@Utils.Traducir("Nombre")</label>
                <div class="col-sm-3">
                    @Html.TextBox("nombre", String.Empty, New With {.maxlength = "100", .class = "form-control"})
                </div>
            </div>
            @<div Class="form-group">
                <label class="col-sm-2 control-label">@Utils.Traducir("Descripción")</label>
                <div class="col-sm-3">
                    @Html.TextArea("descripcion", String.Empty, New With {.maxlength = "200", .rows = "2", .required = "required", .class = "form-control"})
                </div>
            </div>
            @<div Class="form-group">
                <div class="col-sm-offset-2 col-sm-3">
                    <input type="submit" id="submit" value="@Utils.Traducir("Añadir")" class="btn btn-primary input-block-level form-control" />
                </div>
            </div>
        End Using
    </div>
</div>

