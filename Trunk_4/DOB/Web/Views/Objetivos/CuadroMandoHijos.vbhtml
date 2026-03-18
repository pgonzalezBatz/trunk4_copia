@Imports DOBLib
@Imports System.Globalization

<h3>@ViewData("Titulo")</h3>
<hr />

@code
    Dim listaobjetivos As List(Of ELL.Objetivo) = CType(ViewData("Objetivos"), List(Of ELL.Objetivo))
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
    Dim cultureInfo As CultureInfo = CultureInfo.CreateSpecificCulture(ticket.Culture)
End Code

<script type="text/javascript">
    $(function () {

        $("span.desplegar").click(function () {
            var span = $(this);
            var idObjetivo = span.data('padre');
            var cargado = span.data('cargado');
            var collapse = $("#collapseme_" + idObjetivo.toString());
            if (span.hasClass("glyphicon-chevron-down")) {
                span.removeClass("glyphicon-chevron-down");
                span.addClass("glyphicon-chevron-up");
                collapse.collapse('show');
                if (cargado == 0) {
                    collapse.find("td:nth-child(2)").html('@Utils.Traducir("Cargando")...');
                    collapse.find("td:nth-child(2)").load('@Url.Action("Listar", "Acciones")' + '?idObjetivo=' + idObjetivo.toString() + '&timeStamp=' + @DateTime.Now.Ticks, function (responseTxt, statusTxt, xhr) {
                        if (statusTxt == "success") {
                            span.data("cargado", 1);
                        }
                        else {
                            alert(xhr.status);
                            alert(responseTxt);
                        }
                    });
                }
            } else {
                span.removeClass("glyphicon-chevron-up");
                span.addClass("glyphicon-chevron-down");
                collapse.collapse("hide");
            }
        });

        $(".ventana-modal-objetivos").click(function () {
            var idPadre = $(this).data('padre');
            var idTipoDocumento = $(this).data('tipo');
            var titulo = $(this).data('titulo');
            $("#modalWindowObjetivos").find('.modal-title').text(titulo);
            $('#modalBodyObjetivos').html('@Utils.Traducir("Cargando")...');
            $("#modalWindowObjetivos").modal('show');
            $('#modalBodyObjetivos').load('@Url.Action("Listar", "Documentos")' + '?idPadre=' + idPadre.toString() + '&idTipoDocumento=' + idTipoDocumento.toString() + '&timeStamp=' + @DateTime.Now.Ticks);
        });

        $(".estadisticas-objetivos").click(function () {
            var idPadre = $(this).data('padre');
            var titulo = $(this).data('titulo');
            $("#modalWindowObjetivos").find('.modal-title').text(titulo);
            $('#modalBodyObjetivos').html('@Utils.Traducir("Cargando")...');
            $("#modalWindowObjetivos").modal('show');
            $('#modalBodyObjetivos').load('@Url.Action("EvolucionObjetivo", "Objetivos")' + '?idObjetivo=' + idPadre.toString());
        });
    });

</script>

@If (listaobjetivos.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<table class="table table-striped table-hover table-condensed table">
        <thead>
            <tr>
                <th></th>
                <th>@Utils.Traducir("Planta")</th>
                @*<th>@Utils.Traducir("Nivel")</th>*@
                @*<th>@Utils.Traducir("Planta")</th>*@
                <th class="hidden-xs">@Utils.Traducir("Proceso")</th>
                <th>@Utils.Traducir("Objetivo")</th>
                <th class="hidden-xs">@Utils.Traducir("Reto")</th>
                <th class="text-center">@Utils.Traducir("Docs.")</th>
                <th>@Utils.Traducir("Responsable")</th>
                <th class="text-center hidden-xs">@Utils.Traducir("Fecha objetivo")</th>
                <th class="hidden-xs">@Utils.Traducir("Indicador")</th>
                <th class="text-right hidden-xs">@Utils.Traducir("Valor inicial")</th>
                <th class="text-right hidden-xs">@Utils.Traducir("Valor objetivo")</th>
                <th class="text-right">@Utils.Traducir("Valor actual")
                <th class="text-center hidden-xs">@Utils.Traducir("% Cumplimiento acciones")</th>
            </tr>
        </thead>
        <tbody>
            @code
                For Each objetivo In listaobjetivos
                    Dim estiloCeldaValorActual As String = String.Empty
                    Dim estiloTendencia As String = String.Empty
                    Dim estiloIconoDocumento As String = "text-muted"
                    Dim mostrarIconoAccion As String = "hidden"

                    If (objetivo.TieneDocumentos) Then
                        estiloIconoDocumento = String.Empty
                    End If

                    If (objetivo.TieneAcciones) Then
                        mostrarIconoAccion = "visible"
                    End If

                    Select Case objetivo.ColorActual
                        Case ELL.Objetivo.Semaforo.Rojo
                            estiloCeldaValorActual = "danger"
                        Case ELL.Objetivo.Semaforo.Amarillo
                            estiloCeldaValorActual = "warning"
                        Case ELL.Objetivo.Semaforo.Verde
                            estiloCeldaValorActual = "success"
                    End Select

                    Select Case objetivo.Tendencia
                        Case ELL.Objetivo.TipoTendencia.Ascendente
                            estiloTendencia = "glyphicon glyphicon-arrow-up text-success"
                        Case ELL.Objetivo.TipoTendencia.Plana
                            estiloTendencia = "glyphicon glyphicon-arrow-right"
                        Case ELL.Objetivo.TipoTendencia.Descendente
                            estiloTendencia = "glyphicon glyphicon-arrow-down text-danger"
                    End Select

                    @code
                        Dim nivel As String = String.Empty
                        For cont = 0 To objetivo.NivelArbol
                            nivel &= cont
                            If (cont <> objetivo.NivelArbol) Then
                                nivel &= ">"
                            End If
                        Next
                        Dim estilo = "padding-left:" & objetivo.NivelArbol * 35 & "px;"
                        Dim estiloFila = "background-color:rgba(153, 204, 255, " & (1 - objetivo.NivelArbol * 0.3).ToString.Replace(",", ".") & ");"
                    End code

                    @<tr style="@estiloFila">
                        <td class="text-center">
                            <span class="desplegar glyphicon glyphicon-chevron-down" style="cursor:pointer;visibility: @mostrarIconoAccion;" aria-hidden="true" data-padre="@objetivo.Id" data-cargado="0"></span>
                        </td>
                        <td class="text-left" style="@estilo">
                            @*<span class="estadisticas-objetivos glyphicon glyphicon-stats" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Mostrar estadísticas")" data-padre="@objetivo.Id" data-titulo="@Utils.Traducir("Evolución del indicador")"></span>*@
                            <table>
                                <tr>
                                    @code
                                        Dim estiloEvolucion As String = "text-danger"
                                        If (BLL.EvolucionObjetivosBLL.CargarListado(objetivo.Id).Count > 0) Then
                                            estiloEvolucion = String.Empty
                                        End If
                                    End code

                                    <td><a href="@Url.Action("EvolucionObjetivo", "Objetivos", New With {.idObjetivo = objetivo.Id})" target="_blank"><span class="glyphicon glyphicon-stats estiloEvolucion" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Mostrar estadísticas")"></span></a></td>
                                    <td style="padding-left:10px">@objetivo.Planta</td>
                                </tr>
                            </table>
                        </td>

                        @*<td class="text-center">
                                @code
                                    Dim objetivosHijos As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListadoPorPadre(objetivo.Id)

                                    If (objetivosHijos IsNot Nothing AndAlso objetivosHijos.Count > 0) Then
                                        @<a href ="@Url.Action("CuadroMandoHijos", "Objetivos", New With {.idObjetivoPadre = objetivo.Id})" target="_blank"><span class="glyphicon glyphicon-share" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Mostrar objetivos hijos")"></span></a>
                                    End If
                                End code
                            </td>*@
                        @*<td>

                                @nivel
                            </td>*@
                        @*<td>

                            </td>*@
                        <td class="col-md-1 hidden-xs">@objetivo.CodigoProceso</td>
                        <td>
                            @code
                                If (objetivo.IdObjetivoPadre <> Integer.MinValue) Then
                                    Dim objPadre As ELL.Objetivo = BLL.ObjetivosBLL.ObtenerObjetivo(objetivo.IdObjetivoPadre)
                                    @<span Class="glyphicon glyphicon-import text-success" aria-hidden="true" title="@Utils.Traducir("Objetivo desplegado") - @objPadre.Planta"></span>
                                    @Html.Raw("&nbsp;")
                                End If
                            End code

                            @objetivo.Descripcion
                        </td>
                        <td class="hidden-xs">@objetivo.TituloReto</td>
                        <td class="text-center">
                            <a><span class="ventana-modal-objetivos glyphicon glyphicon-folder-open @estiloIconoDocumento" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Mostrar documentos")" data-padre="@objetivo.Id" data-tipo="@CInt(ELL.TipoDocumento.Tipo.Objetivo)" data-titulo="@Utils.Traducir("Documentos asociados al objetivo")"></span></a>
                        </td>
                        <td>@objetivo.Responsable</td>
                        <td class="text-center hidden-xs">@objetivo.FechaObjetivo.ToShortDateString()</td>
                        <td class="hidden-xs">@objetivo.NombreIndicador</td>
                        <td class="text-right hidden-xs">@String.Format("{0:n} {1}", objetivo.ValorInicial, objetivo.TipoIndicador)</td>
                        <td class="text-right hidden-xs">@String.Format("{0:n} {1}", objetivo.ValorObjetivo, objetivo.TipoIndicador)</td>
                        <td class="text-right @estiloCeldaValorActual">@String.Format("{0:n} {1} ", objetivo.ValorActual, objetivo.TipoIndicador)<span class="@estiloTendencia" aria-hidden="true"></span></td>
                        <td class="hidden-xs">
                            <div class="progress">
                                <div class="progress-bar" role="progressbar" style="width:@CStr(objetivo.CumplimientoAcciones).Replace(",", ".")%;min-width: 2em;">
                                    @objetivo.CumplimientoAcciones%
                                </div>
                            </div>
                        </td>
                    </tr>
                    @<tr class="collapse info" id="collapseme_@objetivo.Id" data-objetivo="@objetivo.Id">
                        <td></td>
                        <td colspan="12"></td>
                    </tr> Next
            End Code
        </tbody>
    </table>
End if

<div class="modal fade bd-example-modal-lg" id="modalWindowObjetivos" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Titulo</h4>
            </div>
            <div id="modalBodyObjetivos" class="modal-body">
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
            </div>
        </div>
    </div>
</div>