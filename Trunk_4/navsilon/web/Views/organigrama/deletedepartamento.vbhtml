@imports web
@Code
    ViewBag.title = h.traducir("Eliminar Departamento")
End code


@If ViewData("mapeos").count = 0 Then
    @<form action="" method="post"> 
    <input type="submit" value="@h.traducir("Eliminar")" name="confirmacion"/>
</form>
Else
    
    @h.traducir("El departamento")
    @Html.Encode(" ") @Model.nombre@Html.Encode(" ")
    @h.traducir("tiene")@Html.Encode(" ") @ViewData("mapeos").count @Html.Encode(" ")
    @h.traducir("relacion/es con departamentos de RRHH")@Html.Encode(".")
    @<br/>
    @h.traducir("Es necesario eliminar estas relaciones antes de eliminar el departamento")
    @<br/>
    @h.traducir("Las relaciones se modifican en la pestaña de relaciones")
End If
    