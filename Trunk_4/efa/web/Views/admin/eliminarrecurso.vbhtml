@Modeltype web.recurso
@imports web

@section header
    <title>@h.traducir("Eliminar Grupo")</title>
End section
@section  beforebody
<strong><a href="@Url.Action("recurso", New With {.grupo = Model.nombreGrupo})">@h.Traducir("Volver")</a></strong>
<br style="clear:both;" />
End Section

@Html.ValidationSummary()
<h3>Estas seguro de que quieres eliminar esto?</h3>
    @Using Html.BeginForm()
@<fieldset>
    <legend>Datos del recurso</legend>

    <label>@h.Traducir("Nombre de grupo")  </label>
   @Html.HiddenFor(Function(m) m.NombreGrupo)
   @Model.NombreGrupo <br />
    <label>@h.Traducir("Id recurso")  </label>
   @Html.HiddenFor(Function(m) m.Id)
   @Model.Id<br />
    <label>@h.Traducir("Descripción")  </label>
   @Html.HiddenFor(Function(m) m.Descripcion)
   @Model.Descripcion<br />
    <label>@h.Traducir("Planta")  </label>
   @Html.HiddenFor(Function(m) m.Planta)
   @Model.Planta<br />
</fieldset>

@<span class="cancelanchor">@Html.ActionLink("Volver", "recurso", New With {.grupo = Model.NombreGrupo})</span>
@<input type="submit" value="@h.Traducir(" eliminar")  " />
    End Using 