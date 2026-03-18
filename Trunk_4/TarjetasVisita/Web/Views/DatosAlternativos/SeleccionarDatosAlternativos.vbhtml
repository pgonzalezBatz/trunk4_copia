@Imports TarjetasVisitaLib

@code
    Dim datosAlternativos As List(Of ELL.DatosAlternativos) = CType(ViewData("DatosAlternativos"), List(Of ELL.DatosAlternativos))
End Code

<script type="text/javascript">
    $(function () {
        $('#modalWindowSelecDatos').modal('show');
    })
</script>

<div class="modal fade" id="modalWindowSelecDatos" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">@Utils.Traducir("Origen de datos")</h4>
            </div>
            <div class="modal-body">
             @Using Html.BeginForm("Agregar", "Solicitud", FormMethod.Get, New With {.class = "form-horizontal"})
                        @<table Class="table table-striped table-hover table-condensed" style="margin-top:10px;">
                            <thead>
                                    <tr>
                                    <th></th>
                                    <th>@Utils.Traducir("Descripción")</th>
                                    <th>@Utils.Traducir("Nombre")</th>
                                    <th>@Utils.Traducir("Puesto")</th>
                                    <th>@Utils.Traducir("Movil")</th>
                                    <th>@Utils.Traducir("Dirección")</th>
                                    <th>@Utils.Traducir("Fijo")</th>
                                    <th>@Utils.Traducir("Email")</th>
                                </tr>
                            </thead>
                            <tbody>
                                @code
                                    For Each dato In datosAlternativos
                                        @<tr>
                                            <td>@Html.RadioButton("radioDatos", dato.Id, False, New With {.class = "form-check-input"})</td>
                                            <td>@dato.Descripcion</td>
                                            <td>@dato.Nombre</td>
                                            <td>@dato.Puesto</td>
                                            <td>@dato.Movil</td>
                                            <td>@dato.Direccion</td>
                                            <td>@dato.Fijo</td>
                                            <td>@dato.Email</td>
                                        </tr>
                                    Next
                                End Code
                            </tbody>
                        </table>
                        @<input type="submit" id="btnAceptarSelecTipoSol" name="btnAceptarSelecTipoSol" value="@Utils.Traducir("Aceptar")" Class="btn btn-primary input-block-level form-control" />
                                    End Using
                </div>
            </div>
    </div>
</div>
