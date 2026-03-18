@Imports DOBLib

<h3>@Utils.Traducir("Objetivos")</h3>
<hr />

@code
    Dim listaobjetivos As List(Of ELL.Objetivo) = CType(ViewData("Objetivos"), List(Of ELL.Objetivo))
    Dim rolActual As ELL.UsuarioRol = CType(Session("RolActual"), ELL.UsuarioRol)
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
    Dim rolesUsuario As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(ticket.IdUser)
    Dim listaResp As List(Of ELL.UsuarioRol) = CType(ViewData("ListaResponsables"), List(Of ELL.UsuarioRol))
    Dim idResponsable As Integer = CInt(ViewData("IdResponsable"))
End Code

<script type="text/javascript">
    $(function () {
        $(".boton-eliminar").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea eliminar el elemento seleccionado?"))");
        });

        $("[id^='desplegar']").click(function () {
            var idObjetivo = $(this).data("idobjetivo");
            var idPlanta = $(this).data("idplanta");
            $("#hIdObjetivo").val(idObjetivo);            
            $("#modalWindowDesplegarObjetivo").modal('show');
            $('#modalBodyPlantas').load('@Url.Action("ListarHijas", "PlantasDespliegue")' + '?idPlantaPadre=' + idPlanta.toString() + '&idObjetivo=' + idObjetivo + '&timeStamp=' + @DateTime.Now.Ticks);               
            return false;
        });

        $("#btnContinuar").click(function () {
            var marcado = 0;
            $.each($("[id^='chkBox-']:checked"), function () {
                marcado = 1;
                return true;
            });

            if (marcado == 1) {
                return true;
            } else {
                alert("@Html.Raw(Utils.Traducir("Debe seleccionar alguna planta hija para continuar"))");
                return false;
            }
        });

        $("#Ejercicios").change(function () {
            var anyo = $(this).val();

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

@code
    If (rolActual.IdRol = ELL.Rol.RolUsuario.Lider_de_objetivos OrElse rolActual.IdRol = ELL.Rol.RolUsuario.Responsable) Then
            @<a href="@Url.Action("Agregar")" Class="btn btn-info">
                <span Class="glyphicon glyphicon-plus"></span> @Utils.Traducir("Nuevo")
            </a>
            @<br />@<br />
    End If
End Code

@Using Html.BeginForm("Index", "Objetivos", FormMethod.Post)
    @<div Class="panel panel-default">
        <div Class="panel-heading">
            <h3 Class="panel-title">@Utils.Traducir("Filtros de búsqueda")</h3>
        </div>
        <div Class="panel-body">
            <div class="form-horizontal">
                <div Class="form-group">
                    <Label class="col-sm-1 control-label">@Utils.Traducir("Producto")</Label>
                    <div class="col-sm-6">
                        @Html.DropDownList("Retos", Nothing, New With {.class = "form-control"})
                    </div>
                    <Label class="col-sm-1 control-label">@Utils.Traducir("Proceso")</Label>
                    <div class="col-sm-4">
                        @Html.DropDownList("Procesos", Nothing, New With {.class = "form-control col-sm-4"})
                    </div>
                </div>
                <div Class="form-group">
                    <Label class="col-sm-1 control-label">@Utils.Traducir("Responsable")</Label>
                    <div class="col-sm-4">
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
                    <label class="col-sm-1 control-label">@Utils.Traducir("Ejercicio")</label>
                    <div class="col-sm-2">
                        @Html.DropDownList("Ejercicios", Nothing, New With {.class = "form-control"})
                    </div>
                    <div class="col-sm-4">
                        <input type="submit" id="submit" value="@Utils.Traducir("Buscar")" Class="btn btn-primary form-control" />
                    </div>
                </div>
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
                <th>@Utils.Traducir("Descripción")</th>
                <th class="hidden-xs">@Utils.Traducir("Reto")</th>
                <th class="hidden-xs">@Utils.Traducir("Proceso")</th>
                <th>@Utils.Traducir("Responsable")</th>
                <th class="text-center">@Utils.Traducir("Fecha objetivo")</th>
                <th class="hidden-xs">@Utils.Traducir("Indicador")</th>
                <th class="text-right">@Utils.Traducir("Valor inicial")</th>
                <th class="text-right">@Utils.Traducir("Valor objetivo")</th>
                <th class="text-right">@Utils.Traducir("Valor actual")</th>
                @code
                    If (rolActual.IdRol <> ELL.Rol.RolUsuario.Consultor) Then
                        @<th></th>
                        @<th></th>
                        @<th></th>
                    End If
                End code
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
                        <td>@objetivo.Responsable</td>
                        <td class="text-center">@objetivo.FechaObjetivo.ToShortDateString()</td>
                        <td class="hidden-xs">@objetivo.NombreIndicador</td>
                        <td class="text-right">@String.Format("{0:n} {1}", objetivo.ValorInicial, objetivo.TipoIndicador)</td>
                        <td class="text-right">@String.Format("{0:n} {1}", objetivo.ValorObjetivo, objetivo.TipoIndicador)</td>
                        <td class="text-right @estiloCeldaValorActual">@String.Format("{0:n} {1} ", objetivo.ValorActual, objetivo.TipoIndicador)<span class="@estiloTendencia" aria-hidden="true"></span></td>
                        @code
                            If ((rolActual.IdRol <> ELL.Rol.RolUsuario.Consultor AndAlso objetivo.IdUsuarioAlta = ticket.IdUser) OrElse
                                rolActual.IdRol = ELL.Rol.RolUsuario.Lider_de_objetivos OrElse rolesUsuario.Exists(Function(f) f.IdRol = ELL.Rol.RolUsuario.Administrador)) Then
                                @<td Class="text-center">
                                    <a href='@Url.Action("Editar", "Objetivos", New With {.idObjetivo = objetivo.Id})'>
                                        <span class="glyphicon glyphicon-pencil" aria-hidden="true" title="@Utils.Traducir("Editar")"></span>
                                    </a>
                                </td>
                                @<td Class="text-center">
                                    @code
                                        If (objetivo.IdObjetivoPadre = Integer.MinValue OrElse (objetivo.IdObjetivoPadre <> Integer.MinValue AndAlso rolesUsuario.Exists(Function(f) f.IdRol = ELL.Rol.RolUsuario.Administrador))) Then
                                        @<a href ='@Url.Action("Eliminar", "Objetivos", New With {.idObjetivo = objetivo.Id})'>
                                        <span class="glyphicon glyphicon-remove boton-eliminar" aria-hidden="true" title="@Utils.Traducir("Eliminar")"></span>
                                                                                                            </a>
                                        End If
                                    End code
                                </td>
                                        End If

                                        Dim plantaObjetivoTieneHijas As Boolean = BLL.PlantasDespliegueBLL.CargarListadoPorPlantaPadre(objetivo.IdPlanta).Count > 0

                                        If ((rolActual.IdRol = ELL.Rol.RolUsuario.Lider_de_objetivos OrElse rolActual.IdRol = ELL.Rol.RolUsuario.Responsable) AndAlso plantaObjetivoTieneHijas) Then
                                @<td Class="text-center">
                                     <a>
                                         <span id="desplegar-@objetivo.Id" class="glyphicon glyphicon-share" data-idobjetivo="@objetivo.Id" data-idplanta="@objetivo.IdPlanta" style="cursor:pointer;" title="@Utils.Traducir("Desplegar objetivo")"></span>
                                     </a>
</td>
                            End If
                        End code
                    </tr>
                            Next
            End Code
        </tbody>
    </table>
                            End If

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


