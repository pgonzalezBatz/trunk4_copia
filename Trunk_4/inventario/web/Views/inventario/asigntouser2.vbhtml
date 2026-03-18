@imports web
@Code
    ViewBag.title = "Inventario"
End Code
    <div id="notifications">
        @h.Traducir("Etiqueta para usuario") @Model.nombre@Html.Encode(" ")<a href="@Url.Action("historicoUsuario", h.ToRouteValues(Request.QueryString,Nothing))">@h.traducir("ver historico")</a>
    </div>
    @Html.ValidationSummary()
    <form action="" method="post">
            
            <label>
            @h.Traducir("Etiqueta")<br />
            @Html.TextBox("idetiqueta")
        </label><br />
        <br />
        <input type="submit" value="@h.Traducir("Continuar con la asignación")"  />
    </form>
<br />

@If ViewData("activosasignados").count = 0 Then
    @<strong>@h.traducir("El usuario no tiene activos asignados en estos momentos")</strong>
Else
    @<strong>@h.traducir("Listado de activos asignados a")@Html.Encode(" ")@Model.nombre</strong>
    @<table class="table3">
    <thead>
        <tr>
            <th>@h.traducir("Etiqueta")</th>
            <th>@h.traducir("Fecha asignación")</th>
            <th>@h.traducir("Modelo")</th>
            <th>@h.traducir("Marca")</th>
            <th>@h.traducir("Tipo")</th>
            <th>@h.traducir("Precio")</th>
            <th>@h.traducir("Microsoft OM")</th>
            <th>@h.traducir("Imputacion")</th>
            <th>@h.traducir("Comentarios")</th>
        </tr>
    </thead>
    <tbody>
        @For Each a In ViewData("activosasignados")
            @<tr>
                <td>@a.idEtiqueta</td>
                <td>@a.fechaAlta.toshortdatestring</td>
                <td>@a.nombreModelo</td>
                <td>@a.nombreMarca</td>
                <td>@a.nombreTipo</td>
                <td>@a.precioModelo</td>
                <td>@a.numeroSerie</td>
                <td>
                    @If a.EsDepartamento Then
                            @h.traducir("Departamento")
                        Else
                            @h.traducir("Usuario")
                        End If
                </td>
                 <td>@a.descripcion</td>
             </tr>
                    Next
    </tbody>
</table>
End If

<script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
<script type="text/javascript">
    document.getElementById("idetiqueta").focus();
    $(".table3 tbody tr").click(function () {
        $("#idetiqueta").attr('value',$(this).children()[0].innerHTML)
            
    });
</script>
