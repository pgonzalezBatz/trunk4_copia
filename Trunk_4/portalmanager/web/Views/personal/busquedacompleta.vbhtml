@imports web
@ModelType IEnumerable(Of object)


@Html.Partial("menu2")

<h3 class="my-3">Busqueda detallada</h3>

<form action="" class="mb-3" method="get">
    <div class="row">
        <div class="col-5">
            <div class="input-group">
                @Html.TextBox("s", Nothing, New With {.class = "form-control"})
                <span class="input-group-append">
                    <input class="btn btn-primary" type="submit" value="@H.Traducir("Buscar")" />
                </span>


            </div>
        </div>
    </div>
</form>
@If Model IsNot Nothing Then
    @<table class="table small">
        <thead class="thead-light">
            <tr>
                <th>@H.Traducir("Id Trabajador")</th>
                <th>@H.Traducir("Digito LA")</th>
                <th>@H.Traducir("Nombre")</th>
                <th>@H.Traducir("NIF")</th>
                <th>@H.Traducir("direccion")</th>
                <th>@H.Traducir("Nº SS")</th>
                <th>@H.Traducir("Relevo")</th>
                <th>@H.Traducir("Responsable")</th>
                <th>@H.Traducir("Telefono")</th>
                <th>@H.Traducir("Nacimiento")</th>
                <th>@H.Traducir("Antiguedad")</th>
                <th>@H.Traducir("Sexo")</th>
                <th>@H.Traducir("Convenio")</th>
                <th>@H.Traducir("Categoria")</th>
                <th>@H.Traducir("Contrato")</th>
                <th>@H.Traducir("Estudios")</th>
                <th>@H.Traducir("Departamento")</th>
                <th>@H.Traducir("Fecha baja")</th>
                <th>@H.Traducir("Acciones")</th>
            </tr>
        </thead>
        @For Each p In Model
            @<tr style="@(If(IsDate(p.fbaja), "text-decoration:line-through;", ""))">
                <td>
                    @p.idTrabajador
                </td>
                <td>@p.matricula</td>
                <td>@p.nombre @Html.Encode(" ") @p.apellido1 @Html.Encode(" ") @p.apellido2</td>
                <td>@p.nif</td>
                <td>
                    @p.domicilio @Html.Encode(" ") Nº @p.numero @Html.Encode(", ")
                    @Html.Raw(p.piso)@Html.Raw(p.puerta)
                    <br />
                    @p.codigopostal, @p.poblacion
                    <br />
                    @p.provincia
                </td>
                <td>@p.nass</td>

                <td>@p.turno</td>
                <td>@p.ntarjeta</td>
                <td>
                    @p.telefono1
                @If Not String.IsNullOrEmpty(p.telefono2) Then
                    @<br />
                    @p.telefono2                    End If
            </td>
            <td>@p.fNacimiento.toshortdatestring</td>
            <td>
                @If CType(p.antiguedad, Date?).HasValue Then
                    @p.antiguedad.toshortdatestring
                End If
            </td>
            <td>@p.sexo</td>
            <td>@p.convenio</td>
            <td>@p.categoria</td>
            <td>@p.contrato</td>
            <td>@p.estudios</td>
            <td>@p.n4</td>
            <td>
                @If IsDate(p.fbaja) Then
                    @p.fbaja.toshortdatestring
                End If
            </td>
            <td>
                @If SimpleRoleProvider.IsUserAuthorised(Role.rrhh) Then
                    @<a href="@Url.Action("evolucion", New With {.nif = p.nif})">@H.Traducir("Evolucion del trabajador")</a>
                End If
            </td>
        </tr>
        Next
    </table>
End If
