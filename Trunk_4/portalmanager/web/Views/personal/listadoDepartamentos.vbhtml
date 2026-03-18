@imports web
@ModelType IEnumerable(Of object)

@Html.Partial("menu2")
<form action="" class="mb-4" method="get">
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
                <th>@H.Traducir("D.C.")</th>
                <th>@H.Traducir("Nombre Completo")</th>
                <th>@H.Traducir("N2")</th>
                <th>@H.Traducir("N3")</th>
                <th>@H.Traducir("N4")</th>
                <th>@H.Traducir("N5")</th>
                <th>@H.Traducir("N6")</th>
                <th>@H.Traducir("N7")</th>
                <th>@H.Traducir("Convenio")</th>
                <th>@H.Traducir("Categoria")</th>
                <th>@H.Traducir("Centro")</th>
            </tr>
        </thead>
        @For Each p In Model
            @<tr>
                <td>@p.idTrabajador</td>
                <td>@p.matricula</td>
                <td>@p.nombre @Html.Encode(" ") @p.apellido1 @Html.Encode(" ") @p.apellido2</td>
                <td>@p.n2</td>
                <td>@p.n3</td>
                <td>@p.n4</td>
                <td>@p.n5</td>
                <td>@p.n6</td>
                <td>@p.n7</td>
                <td>@p.convenio</td>
                <td>@p.categoria</td>
                <td>@p.centroFormador</td>
            </tr>   Next
    </table>
End If