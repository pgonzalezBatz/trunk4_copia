@Imports DOBLib

<script src="~/Scripts/usuarios.js"></script>

<script type="text/javascript">
    initBusquedaUsuarios("txtUsuario", "hfIdUsuario", "helperUsuario", "@Url.Action("BuscarUsuarios", "UsuariosRoles")");

    $(function () {
        $(".boton-eliminar").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea eliminar el elemento seleccionado?"))");
        });

        $(".boton-darbaja").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea dar de baja el elemento seleccionado?"))");
        });

        $("form.guardar").submit(function () {
            if ($("#hfIdUsuario").val() == "") {
                alert("@Html.Raw(Utils.Traducir("Usuario campo obligatorio"))");
                return false;
            }
        });        

        $("[name^='cambiarRol']").click(function () {
            $("#hfIdUsuarioRol").val($(this).data("idusuariorol"));
            $('#modalWindowCambiarRol').modal('show');
        });
    });

</script>

<h3>@String.Format("{0} - {1}", Utils.Traducir("Administración"), Utils.Traducir("Usuarios/roles"))</h3>
<hr />

@code
    Dim listaUsuarioRoles As List(Of ELL.UsuarioRol) = CType(ViewData("UsuariosRoles"), List(Of ELL.UsuarioRol))
    Dim usuario As ELL.UsuarioRol = CType(ViewData("Usuario"), ELL.UsuarioRol)
    Dim nombreUsuario As String = String.Empty
    Dim idUsuario As String = String.Empty
    Dim estilo As String = "auto-no-seleccionado"

    If (usuario IsNot Nothing) Then
        nombreUsuario = usuario.NombreUsuario
        idUsuario = usuario.IdSab
        estilo = "auto-seleccionado"
    End If
End Code

@Using Html.BeginForm("Añadir", "UsuariosRoles", FormMethod.Post, New With {.class = "guardar form-horizontal"})
    @<h4>@Utils.Traducir("Añadir nuevo usuario/rol")</h4>
    @<div Class="form-group">
        <label class="col-sm-1 control-label">@Utils.Traducir("Usuario")</label>
        <div class="col-sm-4">
            <input type="text" id="txtUsuario" class="form-control @estilo" style="width:100%" value="@nombreUsuario" />
            <input type="hidden" id="hfIdUsuario" name="hfIdUsuario" value="@idUsuario" />
            <div id="helperUsuario" style="margin-top: -1px;">
            </div>
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-1 control-label">@Utils.Traducir("Rol")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Roles", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-1 control-label">@Utils.Traducir("Planta")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Plantas", Nothing, New With {.class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-1 col-sm-4">
            <input type="submit" id="submit" value="@Utils.Traducir("Añadir")" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>
End Using

<hr />

@Using Html.BeginForm("Index", "UsuariosRoles", FormMethod.Post)
    @<div Class="panel panel-default">
        <div Class="panel-heading">
            <h3 Class="panel-title">@Utils.Traducir("Filtros de búsqueda")</h3>
        </div>
        <div Class="panel-body">
            <div class="form-inline">
                <div Class="form-group">
                    <label>@Utils.Traducir("Rol")</label>
                    @Html.DropDownList("RolesFiltro", Nothing, New With {.class = "form-control"})
                </div>
                <div Class="form-group">
                    <label>@Utils.Traducir("Planta")</label>
                    @Html.DropDownList("PlantasFiltro", Nothing, New With {.class = "form-control"})
                </div>
                <input type="submit" id="submit" value="@Utils.Traducir("Buscar")" Class="btn btn-primary form-control" />
            </div>
        </div>
    </div>
End Using

@If (listaUsuarioRoles.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<table class="table table-striped table-hover table-condensed">
        <thead>
            <tr>
                <th>@Utils.Traducir("Usuario")</th>
                <th>@Utils.Traducir("Rol")</th>
                <th>@Utils.Traducir("Planta")</th>
                <th></th>
                <th></th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @code
                For Each usuarioRol In listaUsuarioRoles.Where(Function(f) (f.FechaBaja = DateTime.MinValue OrElse f.FechaBaja > DateTime.Today) AndAlso f.FechaBajaDOB = DateTime.MinValue)
                    @<tr>
                        <td>@String.Format("{0} ({1})", usuarioRol.NombreUsuario, usuarioRol.PlantaActiva)</td>
                        <td>@Utils.Traducir(usuarioRol.DescripcionRol)</td>
                        <td>@usuarioRol.Planta</td>
                        <td class="text-center">
                            <a href='@Url.Action("Eliminar", "UsuariosRoles", New With {.id = usuarioRol.Id})'>
                                <span class="glyphicon glyphicon-remove boton-eliminar" aria-hidden="true" title="@Utils.Traducir("Eliminar")"></span>
                            </a>
                        </td>
                        <td class="text-center">
                            <a href='@Url.Action("DarBaja", "UsuariosRoles", New With {.id = usuarioRol.Id})'>
                                <span class="glyphicon glyphicon-circle-arrow-down boton-darbaja" aria-hidden="true" title="@Utils.Traducir("Dar de baja")"></span>
                            </a>
                        </td>
                        <td class="text-center">
                            <a id="cambiarRol-@usuarioRol.Id" name="cambiarRol-@usuarioRol.Id" data-idusuariorol="@usuarioRol.Id" style="cursor:pointer;">
                                <span class="glyphicon glyphicon-retweet" aria-hidden="true" title="@Utils.Traducir("Cambiar rol")"></span>
                            </a>
                        </td>
                    </tr>
                Next
                For Each usuarioRol In listaUsuarioRoles.Where(Function(f) (f.FechaBaja <> DateTime.MinValue AndAlso f.FechaBaja <= DateTime.Today) OrElse f.FechaBajaDOB <> DateTime.MinValue)
                    @<tr style="text-decoration: line-through;">
                        <td>@String.Format("{0} ({1})", usuarioRol.NombreUsuario, usuarioRol.PlantaActiva)</td>
                        <td>@Utils.Traducir(usuarioRol.DescripcionRol)</td>
                        <td>@usuarioRol.Planta</td>
                        <td class="text-center">
                            <a href='@Url.Action("Eliminar", "UsuariosRoles", New With {.id = usuarioRol.Id})'>
                                <span class="glyphicon glyphicon-remove boton-eliminar" aria-hidden="true" title="@Utils.Traducir("Eliminar")"></span>
                            </a>
                        </td>
                        <td>
                        </td>
                    </tr>
                Next

            End Code
        </tbody>
    </table>
                End If

     <div class="modal fade" id="modalWindowCambiarRol" tabindex="-1" role="dialog" aria-hidden="true">
         <div class="modal-dialog" role="document">
             <div class="modal-content">
                 <div class="modal-header">
                     <h4 class="modal-title">@Utils.Traducir("Cambiar rol")</h4>
                 </div>
                 <div class="modal-body">
                     @Using Html.BeginForm("CambiarRol", "UsuariosRoles", FormMethod.Post, New With {.class = "form-horizontal", .id = "CambiarRolForm"})
                         @Html.Hidden("hfIdUsuarioRol")
                         @<div Class="form-group">
                             <label class="col-sm-2 control-label">@Utils.Traducir("Nuevo rol")</label>
                             <div class="col-sm-10">
                                 @Html.DropDownList("RolesCambio", Nothing, New With {.required = "required", .class = "form-control"})                                 
                             </div>
                         </div>
                         @<div Class="form-group">
                             <div class="col-sm-offset-2 col-sm-10">
                                 <input type="submit" id="btnConfirmCambioRol" name="btnConfirmCambioRol" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" />
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


