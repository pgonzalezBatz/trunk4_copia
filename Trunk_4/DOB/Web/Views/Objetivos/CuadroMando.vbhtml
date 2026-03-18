@Imports DOBLib
@Imports System.Globalization

<h3>@Utils.Traducir("Cuadro de mando")</h3>
<hr />

@code
    Dim listaobjetivos As List(Of ELL.Objetivo) = CType(ViewData("Objetivos"), List(Of ELL.Objetivo))
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
    Dim tipoAgrupacionSeleccionada As Integer = CType(ViewData("TiposAgrupacion"), List(Of Mvc.SelectListItem)).FirstOrDefault(Function(f) f.Selected).Value
    Dim cultureInfo As CultureInfo = CultureInfo.CreateSpecificCulture(ticket.Culture)
    Dim rolActual As ELL.UsuarioRol = CType(Session("RolActual"), ELL.UsuarioRol)
    Dim listaResp As List(Of ELL.UsuarioRol) = CType(ViewData("ListaResponsables"), List(Of ELL.UsuarioRol))
    Dim idResponsable As Integer = CInt(ViewData("IdResponsable"))
End Code

<script type="text/javascript">
    $(function () {
        var urlCognos = "https://cognos.batz.es/ibmcognos/bi/?objRef=i8E2F6F5A34D6425DBD6EF0AC66E77C67&p_Ejercicio={1}&p_Planta={0}&run.outputFormat=HTML&run.prompt=false&ui_appbar=False&ui_navbar=False";

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

        $("#filtros").click(function () {
            $("#panel-filtros").toggleClass("hidden-xs");
        });
                
        if ($("#Ejercicios").val() == @Integer.MinValue){
            $("#linkImprimir").hide();
        }
        else {
            $("#linkImprimir").show();
            var url = urlCognos.replace("{0}", '@rolActual.IdPlanta');
            url = url.replace("{1}", $("#Ejercicios").val());
            $("#linkImprimir").attr("href", url);
        }

        $("#Ejercicios").change(function () {
            var anyo = $(this).val();

            if (anyo == @Integer.MinValue) {
                $("#linkImprimir").hide();
            }
            else {
                $("#linkImprimir").show();
                var url = urlCognos.replace("{0}", '@rolActual.IdPlanta');
                url = url.replace("{1}", $(this).val());
                $("#linkImprimir").attr("href", url);
            }

            //Vamos a cargar los responsables de los objetivos del año seleccionado
            $.ajax({
                url: '@Url.Action("CargarResponsablesAsJson", "Objetivos")',
                data: { anyo: anyo },
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (d) {
                    $('#Responsables').empty();
                    option = $('<option/>');
                    option.attr({ 'value': @Integer.MinValue }).text('<@Utils.Traducir("Todos")>');
                    $('#Responsables').append(option);

                    if (d.length > 0) {
                        $.each(d, function (i, resp) {
                            option = $('<option/>');
                            option.attr({ 'value': resp.IdSab }).text(resp.NombreUsuario);

                            if (resp.EsBaja == true) {
                                option.attr({ 'class': 'text-danger' });
                                option.attr({ 'style': 'text-decoration:line-through' });
                            }

                            $('#Responsables').append(option);
                        });
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        });
    });

</script>

@Using Html.BeginForm("CuadroMando", "Objetivos", FormMethod.Post)
    @*@<button id="filtros" type="button" Class="btn btn-default hidden-lg" title='@Utils.Traducir("Filtros")'>
        <span Class="glyphicon glyphicon-filter"></span>
    </button>*@

    @<div Class="panel panel-default">        
         <div Class="panel-heading">
             <h3 Class="panel-title">
                 <button id="filtros" type="button" Class="btn btn-default hidden-lg" title='@Utils.Traducir("Filtros")'>
                     <span Class="glyphicon glyphicon-filter"></span>
                 </button>&nbsp;@Utils.Traducir("Filtros de búsqueda")
             </h3>
         </div>
        <div id="panel-filtros" Class="panel-body hidden-xs">
            <div class="form-inline">
                <div Class="form-group">
                    <label>@Utils.Traducir("Ejercicio")</label>
                    @Html.DropDownList("Ejercicios", Nothing, New With {.class = "form-control"})
                </div>
                <div Class="form-group">
                    <Label>@Utils.Traducir("Agrupado por")</Label>
                    @Html.DropDownList("TiposAgrupacion", Nothing, New With {.class = "form-control"})
                </div>
                <div Class="form-group">
                    <Label>@Utils.Traducir("Responsable")</Label>
                    @*@Html.DropDownList("Responsables", Nothing, New With {.class = "form-control"})*@
                    <select name="Responsables" id="Responsables" class="form-control">
                        <option value="@Integer.MinValue">@String.Format("<{0}>", Utils.Traducir("Todos"))</option>
                        @For Each resp In listaResp.Where(Function(f) Not f.EsBaja)
                            @<option value="@resp.IdSab" selected="@(resp.IdSab = idResponsable)">@resp.NombreUsuario</option>
                        Next
                        @For Each resp In listaResp.Where(Function(f) f.EsBaja)
                            @<option value="@resp.IdSab" selected="@(resp.IdSab = idResponsable)" class="text-danger" style='text-decoration:line-through'>@resp.NombreUsuario</option>
                        Next
                    </select>
                </div>                
                <input type="submit" id="submit" value="@Utils.Traducir("Buscar")" Class="btn btn-primary form-control" />
                <div Class="form-group">
                    <a href="" id="linkImprimir" target="_blank">@Utils.Traducir("Imprimir")</a>
                </div>
            </div>
        </div>
    </div>

End Using

@If (listaobjetivos.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    Select Case tipoAgrupacionSeleccionada
        Case Integer.MinValue

            @<table class="table table-striped table-hover table-condensed">
                <thead>
                    <tr>
                        <th></th>
                        <th></th>
                        <th></th>
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
                            @<tr>
                                <td class="text-center">
                                    <span class="desplegar glyphicon glyphicon-chevron-down" style="cursor:pointer;visibility: @mostrarIconoAccion;" aria-hidden="true" data-padre="@objetivo.Id" data-cargado="0"></span>
                                </td>
                                <td class="text-center">
                                    @code
                                        Dim estiloEvolucion As String = "text-danger"
                                        Dim evolucion As ELL.EvolucionObjetivo = BLL.EvolucionObjetivosBLL.Obtener(objetivo.Id, DateTime.Today.AddMonths(-2).Month)
                                        If (evolucion Is Nothing OrElse evolucion.ValorActual = "0") Then
                                            estiloEvolucion = String.Empty
                                        End If
                                    End code

                                    @*<span class="estadisticas-objetivos glyphicon glyphicon-stats" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Mostrar estadísticas")" data-padre="@objetivo.Id" data-titulo="@Utils.Traducir("Evolución del indicador")"></span>*@
                                    <a href="@Url.Action("EvolucionObjetivo", "Objetivos", New With {.idObjetivo = objetivo.Id})" target="_blank"><span class="glyphicon glyphicon-stats @estiloEvolucion" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Mostrar estadísticas")"></span></a>                                    
                                </td>
                                <td class="text-center">
                                    @code
                                        Dim objetivosHijos As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListadoPorPadre(objetivo.Id)

                                        If (objetivosHijos IsNot Nothing AndAlso objetivosHijos.Count > 0) Then
                                            @<a href ="@Url.Action("CuadroMandoHijos", "Objetivos", New With {.idObjetivoPadre = objetivo.Id})" target="_blank"><span class="glyphicon glyphicon-share" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Mostrar objetivos hijos")"></span></a>
                                        End If
                                    End code
                                </td>
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
                            </tr>                                     Next
                    End Code
                </tbody>
            </table>
                        Case Else
                            ' Agrupamos los objetivos por el tipo que haya seleccionado el usuario
                            Dim objetivosAgrupados = Nothing
                            Select Case tipoAgrupacionSeleccionada
                                Case ELL.Objetivo.TipoAgrupacion.Reto
                                    objetivosAgrupados = From obj In listaobjetivos
                                                         Group By idAgrupacion = obj.IdReto, nombreGrupo = String.Format("{0}: {1}", Utils.Traducir("Reto"), obj.TituloReto)
                                                         Into objetivos = Group, Count()
                                Case ELL.Objetivo.TipoAgrupacion.Proceso
                                    objetivosAgrupados = From obj In listaobjetivos
                                                         Group By idAgrupacion = obj.IdProceso, nombreGrupo = String.Format("{0}: {1}", Utils.Traducir("Proceso"), obj.CodigoProceso)
                                                         Into objetivos = Group, Count()
                                Case ELL.Objetivo.TipoAgrupacion.Responsable
                                    objetivosAgrupados = From obj In listaobjetivos
                                                         Group By idAgrupacion = obj.IdResponsable, nombreGrupo = String.Format("{0}: {1}", Utils.Traducir("Responsable"), obj.Responsable)
                                                         Into objetivos = Group, Count()
                            End Select

                            For Each agrupacion In objetivosAgrupados
                                @<span Class="label label-default">@agrupacion.nombreGrupo</span>
                                @<table class="table table-striped table-hover table-condensed">
                                    <thead>
                                        <tr>
                                            @Code
                                                @<th></th>
                                                @<th></th>
                                                @<th></th>
                                                If (tipoAgrupacionSeleccionada <> ELL.Objetivo.TipoAgrupacion.Proceso) Then
                                                    @<th Class="hidden-xs">@Utils.Traducir("Proceso")</th>
                                                End If
                                                @<th width="20%">@Utils.Traducir("Objetivo")</th>
                                                If (tipoAgrupacionSeleccionada <> ELL.Objetivo.TipoAgrupacion.Reto) Then
                                                    @<th Class="hidden-xs">@Utils.Traducir("Reto")</th>
                                                End If
                                                @<th Class="text-center">@Utils.Traducir("Docs.")</th>
                                                If (tipoAgrupacionSeleccionada <> ELL.Objetivo.TipoAgrupacion.Responsable) Then
                                                    @<th>@Utils.Traducir("Responsable")</th>
                                                End If
                                                @<th class="hidden-xs">@Utils.Traducir("Fecha objetivo")</th>
                                                @<th class="hidden-xs">@Utils.Traducir("Indicador")</th>
                                                @<th Class="text-right hidden-xs">@Utils.Traducir("Valor inicial")</th>
                                                @<th Class="text-right hidden-xs">@Utils.Traducir("Valor objetivo")</th>
                                                @<th Class="text-right">@Utils.Traducir("Valor actual")</th>
                                                @<th Class="text-center hidden-xs">@Utils.Traducir("% Cumplimiento acciones")</th>
                                            End Code
                                        </tr>
                                    </thead>
                                    <tbody>
                                        @code
                                            For Each objetivo In agrupacion.objetivos
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
                                                @<tr>
                                                    <td class="text-center">
                                                        <span class="desplegar glyphicon glyphicon-chevron-down" style="cursor:pointer;visibility: @mostrarIconoAccion;" aria-hidden="true" data-padre="@objetivo.Id" data-cargado="0"></span>
                                                    </td>
                                                    <td class="text-center">
                                                        @code
                                                            Dim estiloEvolucion As String = "text-danger"
                                                            Dim evolucion As ELL.EvolucionObjetivo = BLL.EvolucionObjetivosBLL.Obtener(objetivo.Id, DateTime.Today.AddMonths(-2).Month)
                                                            If (evolucion IsNot Nothing AndAlso evolucion.ValorActual <> Decimal.MinValue) Then
                                                                estiloEvolucion = String.Empty
                                                            End If
                                                        End code

                                                        @*<span class="estadisticas-objetivos glyphicon glyphicon-stats" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Mostrar estadísticas")" data-padre="@objetivo.Id" data-titulo="@Utils.Traducir("Evolución del indicador")"></span>*@
                                                        <a href="@Url.Action("EvolucionObjetivo", "Objetivos", New With {.idObjetivo = objetivo.Id, .descripcion = objetivo.Descripcion})" target="_blank"><span class="glyphicon glyphicon-stats @estiloEvolucion" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Mostrar estadísticas")"></span></a>
                                                    </td>
                                                    <td class="text-center">
                                                        @code
                                                            Dim objetivosHijos As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListadoPorPadre(objetivo.Id)

                                                            If (objetivosHijos IsNot Nothing AndAlso objetivosHijos.Count > 0) Then
                                                                @<a href ="@Url.Action("CuadroMandoHijos", "Objetivos", New With {.idObjetivoPadre = objetivo.Id})" target="_blank"><span class="glyphicon glyphicon-share" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Mostrar objetivos desplegados")"></span></a>
                                                            End If
                                                        End code
                                                    </td>
                                                    @If (tipoAgrupacionSeleccionada <> ELL.Objetivo.TipoAgrupacion.Proceso) Then
                                                        @<td class="hidden-xs">@objetivo.CodigoProceso</td>
                                                    End If
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
                                                    @If (tipoAgrupacionSeleccionada <> ELL.Objetivo.TipoAgrupacion.Reto) Then
                                                        @<td Class="hidden-xs">@objetivo.TituloReto</td>
                                                    End If
                                                    <td class="text-center">
                                                        <a><span class="ventana-modal-objetivos glyphicon glyphicon-folder-open @estiloIconoDocumento" style="cursor:pointer;" aria-hidden="true" title="@Utils.Traducir("Mostrar documentos")" data-padre="@objetivo.Id" data-tipo="@CInt(ELL.TipoDocumento.Tipo.Objetivo)" data-titulo="@Utils.Traducir("Documentos asociados al objetivo")"></span></a>
                                                    </td>
                                                    @If (tipoAgrupacionSeleccionada <> ELL.Objetivo.TipoAgrupacion.Responsable) Then
                                                        @<td>@objetivo.Responsable</td>
                                                    End If
                                                    <td class="hidden-xs">@objetivo.FechaObjetivo.ToShortDateString()</td>
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
                                                </tr>
                                            Next
                                        End Code
                                    </tbody>
                                </table>
                                                    Next
                                                End Select
                                            End If

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