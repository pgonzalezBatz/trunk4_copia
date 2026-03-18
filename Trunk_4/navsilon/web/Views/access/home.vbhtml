@imports web
@Code
    ViewBag.title = h.traducir("Relaciones de departamentos RRHH Administracion")
End code

@If ViewData("departamentossinmappear").count = 0 Then
    @<h3> @h.traducir("Todos los departamentos de RRHH estan asignados!")</h3>
Else
    @<h1 style="color:red;">@h.traducir("Faltan") @Html.Encode(" ") @ViewData("departamentossinmappear").count @h.traducir("departamentos de RRHH por relacionar. Los datos de nomina no son fiables")!!</h1>
    @<br />
       @<h1>
    <a href="@Url.Action("list", "relaciones")">
        <strong>@h.traducir("Proceder con la asignación de departamentos")</strong>
    </a>
</h1>
End If
