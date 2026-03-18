@Code
    Layout = Nothing
End Code

@Imports DOBLib
@Imports System.Globalization

@code
    Dim listaAcciones As List(Of ELL.Accion) = CType(ViewData("Acciones"), List(Of ELL.Accion))
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
End Code

<script type="text/javascript">
    $(function () {
        $(".ventana-modal-acciones").click(function () {
            var idPadre = $(this).data('padre');
            var idTipoDocumento = $(this).data('tipo');
            var titulo = $(this).data('titulo');
            $("#modalWindowAcciones").find('.modal-title').text(titulo);
            $("#modalWindowAcciones").modal('show');
            $('#modalBodyAcciones').html('@Utils.Traducir("Cargando")...');
            $('#modalBodyAcciones').load('@Url.Action("Listar", "Documentos")' + '?idPadre=' + idPadre.toString() + '&idTipoDocumento=' + idTipoDocumento.toString() + '&timeStamp=' + @DateTime.Now.Ticks);
        });

        $(".estadisticas-acciones").click(function () {
            var idPadre = $(this).data('padre');
            var titulo = $(this).data('titulo');
            $("#modalWindowAcciones").find('.modal-title').text(titulo);
            $("#modalWindowAcciones").modal('show');
            $('#modalBodyAcciones').html('@Utils.Traducir("Cargando")...');
            $('#modalBodyAcciones').load('@Url.Action("EvolucionAccion", "Acciones")' + '?idAccion=' + idPadre.toString());
        });
    });

</script>

@If (listaAcciones.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<table class="table table-striped table-hover table-condensed">
        <thead>
            <tr>
                <th></th>
                <th>@Utils.Traducir("Plan acción")</th>
                <th class="text-center">@Utils.Traducir("Docs.")</th>
                <th class="text-center">@Utils.Traducir("Plazo")</th>
                <th class="text-right">@Utils.Traducir("Peso")</th>
                <th class="text-center">@Utils.Traducir("Avance")</th>
            </tr>
        </thead>
        <tbody>
            @code
                For Each accion In listaAcciones
                    Dim estiloIconoDocumento As String = "text-muted"

                    If (accion.TieneDocumentos) Then
                        estiloIconoDocumento = String.Empty
                    End If

                    @<tr>
                        <td class="text-center">
                            @*<span class="estadisticas-acciones glyphicon glyphicon-stats" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Mostrar estadísticas")" data-padre="@accion.Id" data-titulo="@Utils.Traducir("Evolución del cumplimiento acción")"></span>*@
                            <a href="@Url.Action("EvolucionAccion", "Acciones", New With {.idAccion = accion.Id, .descripcion = accion.Descripcion})" target="_blank"><span class="glyphicon glyphicon-stats" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Mostrar estadísticas")"></span></a>
                        </td>
                        <td>@accion.Descripcion</td>
                        <td class="text-center">
                            <a><span class="ventana-modal-acciones glyphicon  glyphicon-folder-open @estiloIconoDocumento" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Mostrar documentos")" data-padre="@accion.Id" data-tipo="@CInt(ELL.TipoDocumento.Tipo.Accion)" data-titulo="@Utils.Traducir("Documentos asociados a la acción")"></span></a>
                        </td>
                        <td class="text-center">@String.Format("{0}-{1}", accion.FechaObjetivo.ToString("MMM", CultureInfo.CreateSpecificCulture(ticket.Culture)), accion.FechaObjetivo.Year)</td>
                        <td class="text-right">@String.Format("{0:n} %", accion.GradoImportancia)</td>
                        <td>
                            <div class="progress">
                                <div class="progress-bar" role="progressbar" style="width:@CStr(accion.Porcentaje).Replace(",", ".")%;min-width: 2em;">
                                    @accion.Porcentaje%
                                </div>
                            </div>
                        </td>
                    </tr>
                Next
            End Code
        </tbody>
    </table>

                End If

<div class="modal fade bd-example-modal-lg" id="modalWindowAcciones" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Titulo</h4>
            </div>
            <div id="modalBodyAcciones" class="modal-body">
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
            </div>
        </div>
    </div>
</div>
