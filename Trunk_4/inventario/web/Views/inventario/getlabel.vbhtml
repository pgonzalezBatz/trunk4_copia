@imports web
@Code
    ViewBag.title = "Etiqueta"
End code

<div id="notifications">
    @h.traducir("La etiqeta Nº")@Html.Encode(" ")@Request("idetiqueta")@Html.Encode(" ")@h.traducir("esta inventariada")
    <br />
    @If IsDate(Model.fechaAlta) Then
        @h.traducir("y asignada a")@Html.Encode(" ")
        If Model.EsDepartamento Then
            @Model.nombreDepartamento
            @h.traducir("mediante")@Html.Encode(" ")
        End If
        @Model.nombreusuario@Html.Encode(" ")@Model.apellido1usuario@Html.Encode(" ")@Model.apellido2usuario
        @Html.Encode(" ")@h.traducir("el")@Html.Encode(" ")@Model.fechaAlta.toshortdatestring() 
    Else
        @h.traducir("pero no esta asignada a ningun usuario")
    End If
    <a href="@Url.Action("historicoetiqueta", New With {.idetiqueta = Request("idEtiqueta")})">@h.traducir("Ver historico")</a>
</div>
<strong>@h.traducir("Modelo")</strong><br />
@Model.nombremodelo<br />
<strong>@h.traducir("Marca")</strong><br />
@Model.nombremarca<br />
<strong>@h.traducir("Tipo")</strong><br />
@Model.nombretipo<br />
<strong>@h.traducir("Microsoft OM")</strong><br />
@Model.numeroSerie<br />
<strong>@h.traducir("Descripción")</strong><br />
@Model.descripcion<br />
@If IsDate(Model.fechaBajaEtiqueta) Then
    @<strong style="color:red;">@h.traducir("La etiqueta se dio de baja el")@Html.Encode(" ")@Model.fechaBajaEtiqueta</strong>@<br />
End If
<br />
@If IsDate(Model.fechaAlta) Then
    @<form action="@Url.Action("DesasignarEtiqueta")" method="post" style="display:inline;">
        @Html.Hidden("idEtiqueta", Request("idEtiqueta"))
        <input class="normalinput" type="submit" value="@h.traducir("Desasignar etiqueta")" />
     </form>
    @Html.Encode("|")
End If
@If Not IsDate(Model.fechaBajaEtiqueta) Then
    @<form action="@Url.Action("bajaEtiqueta")" method="post" style="display:inline;" onsubmit="return confirm('¿Realmente quieres dar de baja la etiqueta?');">
        @Html.Hidden("idEtiqueta", Request("idEtiqueta"))
        <input class="normalinput" type="submit" value="@h.traducir("Dar de baja la etiqueta")" />
     </form>
    @Html.Encode("|")
    
    @<a href="@Url.Action("AsignToUser", h.ToRouteValues(Request.QueryString, New With {.EsDepartamento = Model.EsDepartamento}))">@h.traducir("Reasignar a otro usuario")</a> @Html.Encode("|")
End If
<a href="@Url.Action("editlabel", h.ToRouteValues(Request.QueryString, Nothing))">@h.traducir("Editar Microsoft OM y descripcion")</a> @Html.Encode("|")
@Html.Encode("|")
    <a href="@Url.Action("addlabel")" >@h.traducir("Volver")</a>