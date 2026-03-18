@imports web

@section header
    <title>@h.traducir("Recursos")</title>
<style type="text/css">
    #contenido1 {text-align: left;}
</style>
End section

@section  beforebody
    <strong><a href="@Url.Action("Elegiraccion", New With {.idSab = Request("idSab")})">@h.traducir("Volver")</a></strong>
    <br style="clear:both;" />
End Section

@If Model.count = 0 Then
@<strong>@h.Traducir("No se han encontrado elementos")</strong>
@<br />
 Else
    @For Each r In Model
    @<form class="touch" action="" method="post">
        @Html.Hidden("idSab",Request("idSab")) 
        @Html.Hidden("NombreGrupo", r.NombreGrupo)
        @If r.GetType.Name = "Registro" Then
        @Html.Hidden("coger", r.Coger)
        End If
        <input type="submit" name="id" value="@r.id" />
    </form>
    Next
    End If