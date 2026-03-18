@imports web
@section title
    - @h.traducir("Busqueda")
End section
@section menu1
    @Html.Partial("menu")
End Section
@section scripts
    <script type="text/javascript">
        $(function () {
            var selectUsuarioAdministrador = $("#idUsuarioAdministrador");

            var postUsuarioAdministradorChange = function (idUsuarioAdministrador) {
                $.post("@Url.Action("usuarioAdministradorSet", "corporativo", h.ToRouteValues(Request.QueryString, Nothing))&idUsuario=" + idUsuarioAdministrador);
            };

            selectUsuarioAdministrador.change(function() {
                if(selectUsuarioAdministrador.val().length >0){postUsuarioAdministradorChange(selectUsuarioAdministrador.val())};
            });
        });
    </script>

End Section

<div class="row">
    <div class="col-sm-12">
        <h3>@h.traducir("Datos generales proveedor global")</h3>
        <form action="" method="post">
            <label>@h.traducir("CIF")</label>
            @Html.TextBox("cif", Nothing, New With {.class = "form-control"})

            <label>@h.traducir("Nombre")</label>
            @Html.TextBox("nombre", Nothing, New With {.class = "form-control"})

            <label>@h.traducir("Localidad")</label>
            @Html.TextBox("localidad", Nothing, New With {.class = "form-control"})

            <label>@h.traducir("Provincia")</label>
            @Html.TextBox("provincia", Nothing, New With {.class = "form-control"})

            
<br />
            <input type="submit" class="btn btn-primary" value="@h.traducir("Guardar datos global")" />
        </form>
    </div>
</div>

@If Model IsNot Nothing Then
@<div Class="row">
    <div Class="col-sm-12">
        <h3>@h.traducir("Empresas en plantas")</h3>
    </div>
</div>
@<div class="row">
<div class="col-sm-12">
            <h4>@h.traducir("Usuario Administrador")</h4>
            @Html.DropDownList("idUsuarioAdministrador", CType(ViewData("lstUsuarios"), IEnumerable(Of SelectListItem)), h.traducir("Seleccionar Administrador"), New With {.class = "form-control"})
        </div>
    </div>

@<div>
<div Class="col-sm-5">
    <h4>@h.traducir("Empresas asignadas")</h4>
    <table class="table">
        <thead>
            <tr>
                <th>@h.traducir("Planta")</th>
                <th>@h.traducir("Nombre")</th>
                <th>@h.traducir("CIF")</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @For Each e In Model.lstEmpresas
                @<tr>
                    <td>@e.nombrePlantaEmpresa</td>
                    <td>@e.nombreEmpresa</td>
                    <td>@e.cifEmpresa</td>
                    <td>
                        <form action="@Url.Action("removeEmpresa", New With {.id = Model.id, .idEmpresa = e.idEmpresa})" method="post">

                            <input type="submit" value="@h.traducir("Quitar")" class="btn btn-primary" />
                        </form>
                    </td>

                </tr>
            Next
        </tbody>
    </table>
</div>

    <div Class="col-sm-7">
        <h4>@h.traducir("Busqueda empresas no asignadas")</h4>
        <form action="" method="get">
            <div class="row">

                <div class="col-sm-12">
                    <div class="input-group">
                        @Html.Hidden("id", Model.id)
                        @Html.TextBox("q", Nothing, New With {.class = "typeahead tt-input form-control", .autocomplete = "off", .spellcheck = "off", .placeholder = h.traducir("Buscar Nº Empresas sin relacionar")})
                        <div class="input-group-btn">
                            <button class="btn btn-primary" type="submit"><i class="glyphicon glyphicon-search"></i></button>
                        </div>
                    </div>
                </div>
            </div>
        </form>
        @If ViewData("lstEmpresasSinCorporativo") Is Nothing Then
            @<br />
            @<div class="alert alert-info">
            <strong>
                <span class="glyphicon glyphicon-info-sign"></span>
                @h.traducir("Utilizar la busqueda para filtrar empresas que no esten asignadas")
            </strong>
        </div>
        Else
            Dim j = 1
            @<div class="panel-group" id="accordion">
    @For Each p In ViewData("lstEmpresasSinCorporativo")
    @<div Class="panel panel-default">
        <div class="panel-heading">
            <a data-toggle="collapse" href="#collapse@(j)" data-parent="#accordion">@p.key.nombreplanta</a>
        </div>
        <div id="collapse@(j)" class="panel-collapse collapse @(If(j = 1, "in", ""))">
            <div Class="panel-body">
                <table class="table">
                    <thead>
                        <tr>
                            <th>@h.traducir("Empresa")</th>
                            <th>@h.traducir("CIF")</th>
                            <th></th>
                        </tr>
                    </thead>
                    @For Each e In p
                                @<tr>
                                    <td>@e.nombre</td>
                                    <td>@e.cif</td>
                                    <td>
                                        <form action="@Url.Action("addEmpresa", h.ToRouteValues(Request.QueryString, Nothing))" method="post">
                                            @Html.Hidden("idEmpresa", e.id)
                                            <input type="submit" value="@h.traducir("Añadir")" class="btn btn-primary" />
                                        </form>
                                    </td>
                                </tr>
                        Next
                </table>
            </div>
        </div>
    </div>
    j = j + 1
    Next
        </div>
    End If

</div>
    
</div>
End If
