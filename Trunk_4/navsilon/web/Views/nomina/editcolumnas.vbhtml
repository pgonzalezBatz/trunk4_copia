@imports web
@Code
    ViewBag.title = h.traducir("Definir Asiento")
End code

@section top
<a href="@Url.Action("index")">@h.traducir("Volver")</a>
End Section

<strong>@h.traducir("Columnas no incluidas")</strong>
<table class="table3">
    <thead>
        <tr>
            <th>@h.traducir("Nombre")</th>
            <th>@h.traducir("Operación")</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        @For Each n In ViewData("columnaspendientes")
            @<tr>
                <td>@n.nombre</td>
                 <td>@n.operacion</td>
                <td>
                    <form action="" method="post">
                        @Html.Hidden("idColumna", n.idColumna)
                        <input type="submit" value="@h.traducir("Añadir")" name="add" />
                    </form>
                </td>
            </tr>
        Next
    </tbody>
</table>


<strong>@h.traducir("Columnas incluidas")</strong>
<table class="table3">
    <thead>
        <tr>
            <th>@h.traducir("Nombre")</th>
            <th>@h.traducir("Operación")</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        @For Each n In ViewData("columnas")
            @<tr>
                <td>@n.nombre</td>
                <td>@n.operacion</td>
                <td>
                    <form action="" method="post">
                        @Html.Hidden("idColumna", n.idColumna)
                        <input type="submit" value="@h.traducir("Quitar")" name="remove" />
                    </form>
                </td>
            </tr>
        Next
    </tbody>
</table>