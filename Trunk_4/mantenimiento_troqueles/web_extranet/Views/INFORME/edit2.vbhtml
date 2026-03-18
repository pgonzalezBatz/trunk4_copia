@section scripts
    <script type="text/javascript">
        $(function () {
            $('#select_all').change(function () {
                if ($(this).prop('checked')) {
                    $('tbody tr td input[type="checkbox"]').each(function () {
                        $(this).prop('checked', true);
                    });
                } else {
                    $('tbody tr td input[type="checkbox"]').each(function () {
                        $(this).prop('checked', false);
                    });
                }
            });
        });
    </script>
End Section
<h3>@h.traducir("Creación de informe")</h3>
<hr />
<div class="row">
    <div class="col-sm-6">
        <h4>@h.traducir("Datos seleccionados")</h4>
    </div>
</div>
<div class="row">
    <div class="col-sm-4">
        <label>@h.traducir("Tipo informe seleccionado")</label>
        <p>@Request("tipoInforme")</p>
    </div>
    <div class="col-sm-4">
        <label>@h.traducir("Nº OF")</label>
        <p>@Request("numord")</p>
    </div>
    <div class="col-sm-4">
        <label>@h.traducir("Operación")</label>
        <p>@Request("numope")</p>
    </div>
</div>

@If Model.count = 0 Then
    @<div class="alert alert-info">
         <h3>
             <span class="glyphicon glyphicon-info-sign"></span>
              @h.traducir("No se han encontrado marcas disponibles para esta OF y OPERACIÓN")
         </h3>
    </div>
Else
    @<h4>@h.traducir("Seleccionar marcas")</h4>
    @<div class="alert alert-info"><span class="glyphicon glyphicon-info-sign"></span><strong> @h.traducir("Obligatorio hacer un informe por cada tipología de pieza: Cuchillas, tacos, macho, matriz y pisador")</strong></div>
    @Html.ValidationSummary(False, "", New With {.class = "alert alert-danger"})
    @<form action="" method="post">
        @Html.AntiForgeryToken()
        @Html.Hidden("tipoInforme")
        @Html.Hidden("numord")
        @Html.Hidden("numope")
         @Html.Hidden("numpedlin")
        <table class="table table-hover">
            <thead>
                <tr>
                    <th><input type="checkbox" id="select_all" /></th>
                    <th>@h.traducir("Marca")</th>
                    <th>@h.traducir("Descripcion")</th>
                    <th>@h.traducir("Cantidad")</th>
                    <th>@h.traducir("Material")</th>
                    <th>@h.traducir("Tratamiento")</th>
                    <th>@h.traducir("T. Secundario")</th>
                    <th>@h.traducir("Dureza")</th>
                </tr>
            </thead>
            <tbody>
                @code
                    Dim i = 0
                End Code
                @For Each m In Model
                @<tr>
                    <td>
                        @Html.CheckBox("marca[" + i.ToString + "].isSelected", CType(Request("preseleccion") = m.hashMaterialTratamiento, Boolean))
                        @Html.Hidden("marca[" + i.ToString + "].id", m.marca)
                    </td>
                    <td>@m.marca</td>
                    <td>@m.descripcion</td>
                    <td>@m.cantidad</td>
                    <td>@m.material</td>
                    <td>@m.tratamiento</td>
                    <td>@m.tratamientoSecundario</td>
                    <td></td>
                </tr>
                    i = i + 1
                Next
            </tbody>
        </table>
        <input type="submit" value="@h.traducir("Crear informe")" Class="btn btn-primary" autofocus />
    </form>
                    End If