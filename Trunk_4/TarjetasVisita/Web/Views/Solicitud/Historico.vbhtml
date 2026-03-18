@Imports TarjetasVisitaLib

@code
    Dim solicitudes As List(Of ELL.Solicitud) = CType(ViewData("Solicitudes"), List(Of ELL.Solicitud))
End Code

<script type="text/javascript">
    $(function () {
        $(".boton-reenviar").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea reenviar el mail a compras?"))");
        });
    })
</script>

<h3>@Utils.Traducir("Histórico solicitudes de tarjetas")</h3>
<hr />

@code

    If (solicitudes.Count = 0) Then
            @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
    Else
            @<table Class="table table-striped table-hover table-condensed" style="margin-top:10px;">
                <thead>
                    <tr>
                        <th>@Utils.Traducir("Nombre")</th>
                        <th>@Utils.Traducir("Puesto de trabajo")</th>
                        <th>@Utils.Traducir("Movil")</th>
                        <th>@Utils.Traducir("Dirección")</th>
                        <th>@Utils.Traducir("Telefono")</th>
                        <th>@Utils.Traducir("Email")</th>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @code
                        For Each solicitud In solicitudes
                            @<tr>
                                <td>@solicitud.Nombre</td>
                                <td>@solicitud.Puesto</td>
                                <td>@solicitud.Movil</td>
                                <td>@solicitud.Direccion</td>
                                <td>@solicitud.Fijo</td>
                                <td>@solicitud.Email</td>
                                <td>
                                    <a href='@Url.Action("Ver", "Solicitud", New With {.id = solicitud.Id})' class="">
                                        <span class="glyphicon glyphicon-search" aria-hidden="True" title="@Utils.Traducir("Ver solicitud")"></span>
                                    </a>
                                </td>
                                <td>
                                    <a href='@Url.Action("ReenviarMailCompras", "Solicitud", New With {.id = solicitud.Id})' class="">
                                        <span class="glyphicon glyphicon-envelope boton-reenviar" aria-hidden="True" title="@Utils.Traducir("Reenviar mail a compras")"></span>
                                    </a>
                                </td>
                            </tr>
                        Next
                    End Code
                </tbody>
            </table>
                        End If
End Code