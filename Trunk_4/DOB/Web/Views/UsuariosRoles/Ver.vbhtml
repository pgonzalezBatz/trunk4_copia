@Imports DOBLib

<h3>@String.Format("{0} - {1}", Utils.Traducir("Administración"), Utils.Traducir("Ver usuarios roles"))</h3>
<hr />

@code
    Dim listaUsuarioRoles As List(Of ELL.UsuarioRol) = CType(ViewData("UsuariosRoles"), List(Of ELL.UsuarioRol))
End Code

@If (listaUsuarioRoles.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<div class="row">
        <div class="col-sm-5">
            <table class="table table-striped table-hover table-condensed">
                <thead>
                    <tr>
                        <th>@Utils.Traducir("Usuario")</th>
                        <th>@Utils.Traducir("Rol")</th>                        
                    </tr>
                </thead>
                <tbody>
                    @code
                        For Each usuarioRol In listaUsuarioRoles.Where(Function(f) (f.FechaBaja = DateTime.MinValue OrElse f.FechaBaja > DateTime.Today) AndAlso f.FechaBajaDOB = DateTime.MinValue)
                            @<tr>
                                <td>@usuarioRol.NombreUsuario</td>
                                <td>@usuarioRol.DescripcionRol</td>
                            </tr>
                        Next
                        For Each usuarioRol In listaUsuarioRoles.Where(Function(f) (f.FechaBaja <> DateTime.MinValue AndAlso f.FechaBaja <= DateTime.Today) OrElse f.FechaBajaDOB <> DateTime.MinValue)

                    @<tr style="text-decoration: line-through;">
                        <td>@usuarioRol.NombreUsuario</td>
                        <td>@usuarioRol.DescripcionRol</td>
                    </tr>
                        Next
                    End Code
                </tbody>
            </table>
            </div>
    </div>
                        End If
