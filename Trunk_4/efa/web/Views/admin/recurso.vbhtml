@Modeltype web.Recurso
@imports web

@section header
    <title>@h.traducir("Administrar grupos")</title>
End section

@section  beforebody
    <strong><a href="@Url.Action("grupo")">@h.traducir("Volver")</a></strong>
    <br style="clear:both;" />
End Section

@Html.ValidationSummary()
<form action="" method="post">
    <fieldset>
        @Html.HiddenFor(Function(m) m.NombreGrupo)
        <legend>Añadir nuevo recurso</legend>
        <label for="id">@h.Traducir("Identificador del recurso")  </label><br />
        @Html.TextBoxFor(Function(m) m.Id)<br />
        <label for="planta">@h.Traducir("Planta")  </label><br />
        @Html.DropDownListFor(Function(m) m.Planta, ViewData("listofplanta"))<br />
        <label for="descripcion">@h.Traducir("Descripción")  </label><br />
        @Html.TextArea("descripcion")<br />
        <input type="submit" value="@h.Traducir(" guardar")" />
    </fieldset>
</form>

<table class="table1">
    <thead>
        <tr>
            <th>@h.Traducir("Grupo")  </th>
            <th>@h.Traducir("Id Recurso")  </th>
            <th>@h.Traducir("Planta")  </th>
            <th>@h.Traducir("Descripción")  </th>
            <th>@h.Traducir("Acciones")  </th>
        </tr>
    </thead>
    @For Each r As Recurso In ViewData("listOfRecurso")
    @<tr>
        <td>@r.NombreGrupo</td>
        <td>@r.Id</td>
        <td>@r.Planta</td>
        <td>@r.Descripcion.Replace(Chr(13), "<br />")</td>
        <td>
            <a href="@Url.Action("editarrecurso",new with{.grupo=r.NombreGrupo,.idrecurso =r.id})">
                @h.Traducir("Editar")
            </a>
            |
            <a href="@Url.Action("eliminarrecurso",new with{.grupo=r.NombreGrupo,.idrecurso =r.id})">
                @h.Traducir("Eliminar")
            </a>
        </td>
    </tr>
    Next
</table>