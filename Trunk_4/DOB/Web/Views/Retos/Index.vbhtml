@Imports DOBLib

<h3>@Utils.Traducir("Retos")</h3>
<hr />

@code
    Dim listaRetos As List(Of ELL.Reto) = CType(ViewData("Retos"), List(Of ELL.Reto))
    Dim rolActual As ELL.UsuarioRol = CType(Session("RolActual"), ELL.UsuarioRol)
    Dim planta As ELL.Planta = BLL.PlantasBLL.ObtenerPlanta(rolActual.IdPlanta)
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
    'Si la planta actual no hereda los retos se pueden crear nuevos
    If (rolActual.IdRol = ELL.Rol.RolUsuario.Lider_de_objetivos AndAlso Not planta.HeredaRetos) Then
        @<a href="@Url.Action("Agregar")" Class="btn btn-info">
            <span Class="glyphicon glyphicon-plus"></span> @Utils.Traducir("Nuevo")
        </a>    
        @<br/>@<br />
    End If
End Code

@Using Html.BeginForm("Index", "Retos", FormMethod.Post)
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
                <div Class="form-group">
                    <Label>@Utils.Traducir("Descripción")</Label>
                    @Html.TextBox("descripcion", String.Empty, New With {.Class = "form-control"})
                </div>
                <input type="submit" id="submit" value="@Utils.Traducir("Buscar")" Class="btn btn-primary form-control" />
            </div>
        </div>
    </div>
End Using

@If (listaRetos.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<table class="table table-striped table-hover table-condensed">
        <thead>
            <tr>
                <th>@Utils.Traducir("Código")</th>
                <th>@Utils.Traducir("Nombre")</th>
                <th>@Utils.Traducir("Descripción")</th>
                <th></th>
                @code
                    If (rolActual.IdRol <> ELL.Rol.RolUsuario.Consultor) Then
                        @<th></th>
                        @<th></th>
                    End If
                End code
            </tr>
        </thead>
        <tbody>
            @code
                For Each reto In listaRetos
                    @<tr>
                        <td>                             
                             @If (reto.FechaBaja <> DateTime.MinValue) Then
                                 @<span style = "text-decoration: line-through">@reto.Codigo</span>
                             Else
                                 @reto.Codigo
                             End If
                        </td>
                        <td>
                            @If (reto.FechaBaja <> DateTime.MinValue) Then
                                @<span style="text-decoration: line-through">@reto.Titulo</span>
                            Else
                                @reto.Titulo
                            End If
                        </td>
                        <td>@Html.Raw(reto.Descripcion)</td>
                        <td class="text-center">
                            @If (reto.IdDocumento <> Integer.MinValue) Then
                                @<a href='@Url.Action("Mostrar", "Documentos", New With {.idDocumento = reto.IdDocumento})'>
                                    <span class="glyphicon glyphicon-download-alt" aria-hidden="True" title="@Utils.Traducir("Descargar documento")"></span>
                                </a>
                            End If
                        </td>
                        @code
                            'Si la planta actual no hereda los retos se editar y eliminar
                            If (rolActual.IdRol <> ELL.Rol.RolUsuario.Consultor AndAlso Not planta.HeredaRetos) Then
                                @<td Class="text-center">
                                    <a href='@Url.Action("Editar", "Retos", New With {.idReto = reto.Id})'>
                                        <span class="glyphicon glyphicon-pencil" aria-hidden="True" title="@Utils.Traducir("Editar")"></span>
                                    </a>
                                </td>
                                @<td Class="text-center">
                                    @If (reto.FechaBaja = DateTime.MinValue) Then
                                        @<a href ='@Url.Action("DarDeBaja", "Retos", New With {.idReto = reto.Id})'>
                                            <span class="glyphicon glyphicon-circle-arrow-down boton-baja" aria-hidden="True" title="@Utils.Traducir("Dar de baja")"></span></a>
                                    Else
                                        @<a href ='@Url.Action("DarDeAlta", "Retos", New With {.idReto = reto.Id})'>
                                            <span class="glyphicon glyphicon-circle-arrow-up boton-alta" aria-hidden="True" title="@Utils.Traducir("Dar de alta")"></span></a>
                                    End If
                                </td>
                            End If
                        End code
                    </tr>
                            Next
            End Code
        </tbody>
    </table>
                            End if

