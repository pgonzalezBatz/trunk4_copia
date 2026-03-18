@Imports CostCarriersLib

@Code
    Dim cabeceras As List(Of ELL.CabeceraCostCarrier) = CType(ViewData("CabecerasProyecto"), List(Of ELL.CabeceraCostCarrier))
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
    Dim rolesUsuario As List(Of ELL.UsuarioRol) = CType(Session("RolesUsuario"), List(Of ELL.UsuarioRol))
    Dim usuariosRol As List(Of ELL.UsuarioRol) = rolesUsuario.Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Financiero).ToList()
    Dim paginaActual As Integer = If(ViewData("PaginaActual") Is Nothing, 1, CInt(ViewData("PaginaActual")))
    Dim elementosPorPagina As Integer = CInt(ViewData("ElementosPorPagina"))
End Code

<script type="text/javascript">
    $(function () {
        $(".cambiarCodigoProyecto").click(function () {            
            var idCabecera = $(this).data("idcabecera");
            var codigo = $(this).data("codigo");
            $("#hIdCabecera").val(idCabecera);
            $("#txtCodigo").val(codigo);
            
            $('#modalWindowCambiarCodigo').modal('show');
        })
    })
</script> 

<h3><label>@Utils.Traducir("Gestionar apertura pasos")</label></h3>
<hr />

@code
    Dim clase As String = String.Empty
    Dim texto As String = String.Empty
    If (cabeceras.Count > 0) Then
        @<div class="row">
            <div class="col-sm-7">
                <table id="tabla" class="table table-condensed table-striped table-hover">
                    <thead>
                        <tr>
                            <th>@Utils.Traducir("Proyecto")</th>
                            <th>@Utils.Traducir("Código proyecto")</th>
                            <th></th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        @*Este primer for each es para sacar los proyectos que tiene pasos por abrir ya que los quieren los primero de la lista*@
                        @*@For Each cabecera In cabeceras
                            For Each usuarioRol In usuariosRol
                                ' Sacamos el id planta de la plantilla. Sólo sacamos los steps approved y que no tengan codigo de portador
                                ' 30/04/2019 Pide Silvia que se vean todos ya que si no no saben las acciones que han realiado
                                cabecera.Steps.AddRange(BLL.StepsBLL.CargarListadoPorPlantaConEstadoValidacion(usuarioRol.IdPlanta).Where(Function(f) f.IdCabecera = cabecera.Id AndAlso Not f.EsInfoGeneral AndAlso (f.IdEstadoValidacion = ELL.Validacion.Estado.Approved OrElse f.IdEstadoValidacion = ELL.Validacion.Estado.Opened OrElse f.IdEstadoValidacion = ELL.Validacion.Estado.Closed)).ToList())
                            Next

                            If (cabecera.Steps.Count > 0) Then
                                If (cabecera.Steps.Exists(Function(f) f.IdEstadoValidacion = ELL.Validacion.Estado.Approved AndAlso String.IsNullOrEmpty(f.CostCarrier))) Then
                                    cabecera.ContienePasosAbrir = True
                                End If
                            End If
                        Next*@

                        @For Each cabecera In cabeceras.OrderByDescending(Function(f) f.ContienePasosAbrir).ThenBy(Function(f) f.NombreProyecto).ToList().Skip((paginaActual - 1) * elementosPorPagina).Take(elementosPorPagina)
                            @<tr>
                                <td>@cabecera.NombreProyecto</td>
                                <td>@cabecera.CodigoProyecto</td>
                                <td Class="text-center">
                                    @code
                                        ' Este icono sólo lo va a tener el financiero de corporativo
                                        If (Not BLL.CabecerasCostCarrierBLL.ContienePasosAbiertos(cabecera.Id) AndAlso usuariosRol.Exists(Function(f) f.IdRol = ELL.Rol.TipoRol.Financiero AndAlso f.IdPlanta = 0)) Then
                                            @<a class="cambiarCodigoProyecto" style="cursor:pointer;" data-idcabecera='@cabecera.Id' data-codigo='@cabecera.CodigoProyecto'>
                                                <span Class="glyphicon glyphicon-pencil" aria-hidden="true" title="@Utils.Traducir("Editar código proyecto")"></span>
                                            </a>
                                        End If

                                        If (cabecera.ContienePasosAbrir) Then
                                            clase = "text-danger"
                                            texto = "<strong>" & Utils.Traducir("Pasos para abrir") & "</strong>"
                                        Else
                                            clase = "glyphicon glyphicon-th-list"
                                            texto = String.Empty
                                        End If
                                    End code
                                </td>
                                <td Class="text-center">
                                    <a href='@Url.Action("DetallePasos", "Financiero", New With {.idCabecera = cabecera.Id})'>
                                        <span class="@clase" aria-hidden="true" title="@Utils.Traducir("Ir a detalle de pasos")">@Html.Raw(texto)</span>
                                    </a>
                                </td>
                            </tr>

                                        Next
                                </tbody>
                            </table>
                        </div>
                    </div>

                                        If (cabeceras.Count > elementosPorPagina) Then
                    @<nav>
                         <ul Class="pagination">
                             @code
                                 Dim contador
                                 For contador = 1 To cabeceras.Count / elementosPorPagina
                                     If (contador = paginaActual) Then
                                         @<li Class="page-item active">@Html.ActionLink(contador, "Index", "Financiero", New With {.paginaActual = contador}, New With {.Class = "page-link"})</li>
                                     Else
                                     @<li Class="page-item">@Html.ActionLink(contador, "Index", "Financiero", New With {.paginaActual = contador}, New With {.Class = "page-link"})</li>
                                     End If

                                 Next

                                 If (cabeceras.Count Mod elementosPorPagina > 0) Then
                                     If (contador = paginaActual) Then
                                         @<li Class="page-item active">@Html.ActionLink(contador, "Index", "Financiero", New With {.paginaActual = contador}, New With {.Class = "page-link"})</li>
                                     Else
                                         @<li Class="page-item">@Html.ActionLink(contador, "Index", "Financiero", New With {.paginaActual = contador}, New With {.Class = "page-link"})</li>
                                     End If
                                 End If
                             End code

                         </ul>
        </nav>
        End if

                                 Else
                                    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
                                        End If
                                End code

            <div class="modal fade" id="modalWindowCambiarCodigo" tabindex="-1" role="dialog" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">@Utils.Traducir("Editar código proyecto")</h4>
                                                </div>
                                                <div class="modal-body">
                        @Using Html.BeginForm("CambiarCodigo", "Financiero", FormMethod.Post, New With {.class = "form-horizontal"})
                        @Html.Hidden("hIdCabecera")
                        @<div Class="form-group">
                            <label class="col-sm-4 control-label">@Utils.Traducir("Código proyecto")</label>
                            <div class="col-sm-8">
                                @Html.TextBox("txtCodigo", String.Empty, New With {.class = "form-control", .required = "required", .maxlength = "5", .style = "text-transform:uppercase;"})
                            </div>
                        </div>
                        @<div Class="form-group">
                            <div class="col-sm-offset-4 col-sm-8">
                                <input type="submit" id="btnConfirmCambiarCodigo" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" />
                            </div>
                        </div>
                        End Using
                                                </div>
                                                <div class="modal-footer">
                                                    <button type = "button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
