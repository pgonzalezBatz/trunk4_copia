@Imports DOBLib

<h3>@Utils.Traducir("Procesos")</h3>
<hr />

@code
    Dim listaProcesos As List(Of ELL.Proceso) = CType(ViewData("Procesos"), List(Of ELL.Proceso))
    Dim rolActual As ELL.UsuarioRol = CType(Session("RolActual"), ELL.UsuarioRol)
End Code

<script type="text/javascript">
    $(function () {
        $(".boton-baja").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea dar de baja el elemento seleccionado?"))");
        });

        $(".boton-alta").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea dar de alta el elemento seleccionado?"))");
        });
    });
</script>

@code
    If (rolActual.IdRol = ELL.Rol.RolUsuario.Lider_de_objetivos) Then
        @<a href="@Url.Action("Agregar")" Class="btn btn-info">
            <span Class="glyphicon glyphicon-plus"></span> @Utils.Traducir("Nuevo")
        </a>
        @<br />@<br />
    End If
End Code

@Using Html.BeginForm("Index", "Procesos", FormMethod.Post)
    @<div Class="panel panel-default">
        <div Class="panel-heading">
            <h3 Class="panel-title">@Utils.Traducir("Filtros de búsqueda")</h3>
        </div>
        <div Class="panel-body">
            <div class="form-inline">
                <div Class="form-group">
                    <label>@Utils.Traducir("Nombre")</label>
                    @Html.TextBox("nombre", String.Empty, New With {.class = "form-control"})
                </div>
                <input type="submit" id="submit" value="@Utils.Traducir("Buscar")" Class="btn btn-primary form-control" />
            </div>
        </div>
    </div>  End Using

@If (listaProcesos.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<div Class="row">
        <div Class="col-sm-6">
            <table class="table table-striped table-hover table-condensed">
                <thead>
                    <tr>
                        <th>@Utils.Traducir("Código")</th>
                        <th>@Utils.Traducir("Nombre")</th>
                        @code
                            If (rolActual.IdRol <> ELL.Rol.RolUsuario.Consultor) Then
                                @<th></th>
                                @<th></th>
                                @<th></th>
                                @<th></th>
                            End If
                        End code
                    </tr>
                </thead>
                <tbody>
                    @code
                        Dim posicion As Integer = Integer.MinValue
                        Dim posicionAux As Integer = Integer.MinValue
                        For Each proceso In listaProcesos
                            @<tr>
                                <td>
                                    @If (proceso.FechaBaja <> DateTime.MinValue) Then
                                        @<span style="text-decoration: line-through">@proceso.Codigo</span>
                                    Else
                                        @proceso.Codigo
                                    End If
                                </td>
                                <td>
                                    @If (proceso.FechaBaja <> DateTime.MinValue) Then
                                        @<span style="text-decoration: line-through">@proceso.Nombre</span>
                                    Else
                                        @proceso.Nombre
                                    End If
                                </td>
                                @code
                                    If (rolActual.IdRol <> ELL.Rol.RolUsuario.Consultor) Then
                                        @<td Class="text-center">
                                            <a href='@Url.Action("Editar", "Procesos", New With {.idProceso = proceso.Id})'>
                                                <span class="glyphicon glyphicon-pencil" aria-hidden="true" title="@Utils.Traducir("Editar")"></span>
                                            </a>
                                        </td>
                                        @<td Class="text-center">
                                             @If (proceso.FechaBaja = DateTime.MinValue) Then
                                                 @<a href='@Url.Action("DarDeBaja", "Procesos", New With {.idProceso = proceso.Id})'>
                                                     <span class="glyphicon glyphicon-circle-arrow-down boton-baja" aria-hidden="True" title="@Utils.Traducir("Dar de baja")"></span>
                                                 </a>
                                             Else
                                                 @<a href='@Url.Action("DarDeAlta", "Procesos", New With {.idProceso = proceso.Id})'>
                                                     <span class="glyphicon glyphicon-circle-arrow-up boton-alta" aria-hidden="True" title="@Utils.Traducir("Dar de alta")"></span>
                                                 </a>
                                             End If
                                        </td>
                                        @<td>
                                            @if (Not listaProcesos.First.Equals(proceso)) Then
                                                posicion = listaProcesos.IndexOf(proceso)
                                                @<a href='@Url.Action("CambiarOrdenProceso", "Procesos", New With {.idProceso = proceso.Id, .idProcesoCambio = listaProcesos(posicion - 1).Id})'>
                                                    <span Class="glyphicon glyphicon-chevron-up" style="cursor:pointer;" aria-hidden="true"></span>
                                                </a>
                                            End If
                                        </td>
                                        @<td>
                                            @if (Not listaProcesos.Last.Equals(proceso)) Then
                                                posicion = listaProcesos.IndexOf(proceso)
                                                @<a href='@Url.Action("CambiarOrdenProceso", "Procesos", New With {.idProceso = proceso.Id, .idProcesoCambio = listaProcesos(posicion + 1).Id})'>
                                                    <span Class="glyphicon glyphicon-chevron-down" style="cursor:pointer;" aria-hidden="true"></span>
                                                </a>
                                            End If
                                        </td>
                                    End If
                                End code
                            </tr>
                                    Next
                    End Code
                </tbody>
            </table>
        </div>
    </div>
                                    End If
