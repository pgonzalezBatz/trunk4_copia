@Imports DOBLib

@code
    Dim listaobjetivos As List(Of ELL.Objetivo) = CType(ViewData("Objetivos"), List(Of ELL.Objetivo))
    Dim rolActual As ELL.UsuarioRol = CType(Session("RolActual"), ELL.UsuarioRol)
    Dim idObjetivo As Integer? = ViewData("IdObjetivo")
End Code

<h3>@Utils.Traducir("Mis objetivos")</h3>
<hr />

<script type="text/javascript">
    $(function () {
        $("span.desplegar").click(function () {
            var span = $(this);
            var idObjetivo = span.data('padre');
            DesplegarAcciones(idObjetivo, span);
        });

        $("[id^='desplegar']").click(function () {
            var idObjetivo = $(this).data("idobjetivo");
            var idPlanta = $(this).data("idplanta");
            $("#hIdObjetivo").val(idObjetivo);
            $("#modalWindowDesplegarObjetivo").modal('show');
            $('#modalBodyPlantas').load('@Url.Action("ListarHijas", "PlantasDespliegue")' + '?idPlantaPadre=' + idPlanta.toString() + '&idObjetivo=' + idObjetivo + '&timeStamp=' + @DateTime.Now.Ticks);
            return false;
        });
    });

    function Desplegar(idObjetivo){
        var span = $('[data-padre="' + idObjetivo + '"]');
        DesplegarAcciones(idObjetivo, span);
    }

    function DesplegarAcciones(idObjetivo, span){
        var cargado = span.data('cargado');
        var collapse = $("#collapseme_" + idObjetivo.toString());
        if (span.hasClass("glyphicon-chevron-down")) {
            span.removeClass("glyphicon-chevron-down");
            span.addClass("glyphicon-chevron-up");
            collapse.collapse('show');
            if (cargado == 0) {
                collapse.find("td:nth-child(2)").html('@Utils.Traducir("Cargando")...');
                collapse.find("td:nth-child(2)").load('@Url.Action("MisAccionesPorObjetivo", "Acciones")' + '?idObjetivo=' + idObjetivo.toString() + '&timeStamp=' + @DateTime.Now.Ticks, function (responseTxt, statusTxt, xhr) {
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
    }
</script>

@Using Html.BeginForm("MisObjetivos", "Objetivos", FormMethod.Post)
    @<div Class="panel panel-default">
        <div Class="panel-heading">
            <h3 Class="panel-title">@Utils.Traducir("Filtros de búsqueda")</h3>
        </div>
        <div Class="panel-body">
            <div class="form-inline">
                <div Class="form-group">
                    <Label>@Utils.Traducir("Reto")</Label>
                    @Html.DropDownList("Retos", Nothing, New With {.class = "form-control"})
                </div>
                <div Class="form-group">
                    <Label>@Utils.Traducir("Proceso")</Label>
                    @Html.DropDownList("Procesos", Nothing, New With {.class = "form-control"})
                </div>
                <div Class="form-group">
                    <label>@Utils.Traducir("Ejercicio")</label>
                    @Html.DropDownList("Ejercicios", Nothing, New With {.class = "form-control"})
                </div>
                <input type="submit" id="submit" value="@Utils.Traducir("Buscar")" Class="btn btn-primary form-control" />
            </div>
        </div>
    </div>
End Using

@If (listaobjetivos.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<table class="table table-striped table-hover table-condensed">
        <thead>
            <tr>
                <th></th>
                <th>@Utils.Traducir("Descripción")</th>
                <th class="hidden-xs">@Utils.Traducir("Reto")</th>
                <th class="hidden-xs">@Utils.Traducir("Proceso")</th>
                <th class="text-center">@Utils.Traducir("Fecha objetivo")</th>
                <th class="hidden-xs">@Utils.Traducir("Indicador")</th>
                <th class="text-right">@Utils.Traducir("Valor inicial")</th>
                <th class="text-right">@Utils.Traducir("Valor objetivo")</th>
                <th class="text-right">@Utils.Traducir("Valor actual")</th>
                <th></th>
                <th></th>
                <th></th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @code
                For Each objetivo In listaobjetivos
                    Dim estiloCeldaValorActual As String = String.Empty
                    Dim estiloTendencia As String = String.Empty

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
                    @<tr>
                        <td class="text-center">
                            <span class="desplegar glyphicon glyphicon-chevron-down" style="cursor:pointer;" aria-hidden="true" data-padre="@objetivo.Id" data-cargado="0"></span>
                        </td>
                        <td>
                            @code
                                If (objetivo.IdObjetivoPadre <> Integer.MinValue) Then
                                    Dim objPadre As ELL.Objetivo = BLL.ObjetivosBLL.ObtenerObjetivo(objetivo.IdObjetivoPadre)
                                    If (objPadre IsNot Nothing) Then
                                    @<span Class="glyphicon glyphicon-import text-success" aria-hidden="true" title="@Utils.Traducir("Objetivo desplegado") - @objPadre.Planta"></span>
                                    @Html.Raw("&nbsp;")
                                    End If
                                End If
                            End code

                            @objetivo.Descripcion
                        </td>
                        <td class="hidden-xs">@objetivo.TituloReto</td>
                        <td class="hidden-xs">@objetivo.CodigoProceso</td>
                        <td class="text-center">@objetivo.FechaObjetivo.ToShortDateString()</td>
                        <td class="hidden-xs">@objetivo.NombreIndicador</td>
                        <td class="text-right">@String.Format("{0:n} {1}", objetivo.ValorInicial, objetivo.TipoIndicador)</td>
                        <td class="text-right">@String.Format("{0:n} {1}", objetivo.ValorObjetivo, objetivo.TipoIndicador)</td>
                        <td class="text-right @estiloCeldaValorActual">@String.Format("{0:n} {1} ", objetivo.ValorActual, objetivo.TipoIndicador)<span class="@estiloTendencia" aria-hidden="true"></span></td>
                        <td class="text-center">
                            @Code
                                If (rolActual.IdRol = ELL.Rol.RolUsuario.Lider_de_objetivos) Then
                                    @<a href='@Url.Action("Editar", "Objetivos", New With {.idObjetivo = objetivo.Id})'>
                                        <span class="glyphicon glyphicon-pencil" aria-hidden="true" title="@Utils.Traducir("Editar detalle")"></span>
                                    </a>

                                Else
                                    @<a href='@Url.Action("Ver", "Objetivos", New With {.idObjetivo = objetivo.Id})'>
                                        <span class="glyphicon glyphicon-pencil" aria-hidden="true" title="@Utils.Traducir("Ver detalle")"></span>
                                    </a>
                                End If
                            end code
                        </td>
                        <td Class="text-center">
                            <a href='@Url.Action("Agregar", "Acciones", New With {.idObjetivo = objetivo.Id})'>
                                <span class="glyphicon glyphicon-plus" aria-hidden="true" title="@Utils.Traducir("Agregar acción")"></span>
                            </a>
                        </td>
                        <td class="text-center">
                            <a href='@Url.Action("Editar", "EvolucionObjetivos", New With {.idObjetivo = objetivo.Id})'>
                                <span class="glyphicon glyphicon-signal" aria-hidden="true" title="@Utils.Traducir("Editar evolución objetivo")"></span>
                            </a>
                        </td>
                        <td class="text-center">
                            @Code
                                Dim plantaObjetivoTieneHijas As Boolean = BLL.PlantasDespliegueBLL.CargarListadoPorPlantaPadre(objetivo.IdPlanta).Count > 0

                                If ((rolActual.IdRol = ELL.Rol.RolUsuario.Lider_de_objetivos OrElse rolActual.IdRol = ELL.Rol.RolUsuario.Responsable) AndAlso plantaObjetivoTieneHijas) Then
                                    @<a>
    <span id="desplegar-@objetivo.Id" Class="glyphicon glyphicon-share" data-idobjetivo="@objetivo.Id" data-idplanta="@objetivo.IdPlanta" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Desplegar objetivos")"></span>
</a>
                                End If
                            end code
                        </td>
                    </tr>
                    @<tr class="collapse info" id="collapseme_@objetivo.Id" data-objetivo="@objetivo.Id">
                        <td></td>
                        <td colspan="11"></td>
                    </tr>
                Next
            End Code
        </tbody>
    </table>

End If

@code
    If (idObjetivo IsNot Nothing) Then
        @<script type="text/javascript">
            Desplegar(@idObjetivo);
        </script>
    End If
End Code

<div class="modal fade" id="modalWindowDesplegarObjetivo" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">@Utils.Traducir("Seleccionar plantas destino")</h4>
            </div>
            <div class="modal-body">
                @Using Html.BeginForm("Desplegar", "Objetivos", FormMethod.Post, New With {.class = "form-horizontal"})
                    @Html.Hidden("hIdObjetivo")
                    @<div id="modalBodyPlantas" Class="form-group">
                    </div>
                    @<div Class="form-group">
                        <div Class="col-sm-12">
                            <input type="submit" id="btnContinuar" value="@Utils.Traducir("Continuar")" Class="btn btn-primary input-block-level form-control" />
                        </div>
                    </div>
                End Using
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
            </div>
        </div>
    </div>
</div>