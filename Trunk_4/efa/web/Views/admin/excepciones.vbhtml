@Modeltype web.recurso
@imports web

@section header
    <title>@h.traducir("Excepciones para recursos concretos")</title>
End section


<ul style="font-style:italic;">
    <li>
        Al pulsar el botón "cambiar", se activa la excepción si no esta activada y se desactiva si esta activada
    </li>
    <li>
        Si en el proceso de activación, se introdujera fecha, está entraría como limite de la excepción. En el caso contrario, no hay fecha límite
    </li>
</ul>
<table class="table1">
    <thead>
        <tr>
            <th>@h.Traducir("Grupo")  </th>
            <th>@h.Traducir("Id recurso")  </th>
            <th>@h.Traducir("Excepción")  </th>
            <th></th>
        </tr>
    </thead>
    @For Each r As Recurso In ViewData("listofrecursos")
    @<tr>
        <td>@r.NombreGrupo</td>
        <td>@r.Id</td>
        <td>
            @If r.Excepcion Then
            @If r.ExcepcionFecha.HasValue Then
            @r.ExcepcionFecha.Value.ToShortDatestring
            Else
            @h.Traducir("Si")
            End If
            Else
            @h.Traducir("No")
            End If
        </td>
        <td>
            <form action="@Url.Action("cambiarexcepcion", New With {.nombre = r.NombreGrupo, .idrecurso = r.id})" method="post">
                @Html.TextBox("fecha", Nothing, New With {.class = "calendar"})
                <input type="submit" value="@h.Traducir(" cambiar")" />
            </form>
        </td>
    </tr>
    Next
</table>
<script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src='//intranet2.batz.es/baliabideorokorrak/ui.datepicker.es-ES.js' type="text/javascript"></script>
<link href="//intranet2.batz.es/baliabideorokorrak/ui.datepicker.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript">
    $(document).ready(function () {
        $('.calendar').datepicker();
    });
</script>