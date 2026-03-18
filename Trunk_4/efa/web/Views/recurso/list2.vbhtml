@imports web

@section header
    <title>@h.traducir("Tipos de recursos")</title>
    <link href="@Url.Content("~/Content/Site.css")" rel="Stylesheet" type="text/css"  />
End section

@section  beforebody
   @If ViewData("recursos") IsNot Nothing Then
    @<a href="@Url.Action("listgrupo")" style="float:left;"><img src="@Url.Content("~/Content/back.png") " alt="@h.traducir("Volver")  " /></a>
   Else
    @<a href="@Url.Action("accion")" style="float:left;"><img src="@Url.Content("~/Content/back.png") " alt="@h.traducir("Volver")  " /></a>
   End If
    <a href="@Url.Action("logout")" style="float:right;" >
        <img src="@Url.Content("~/Content/exit.png") " alt="@h.Traducir("Cerrar sessión")  " />
    </a>
    <br style="clear:both;" />
End Section

@If ViewData("recursos") IsNot Nothing Then
    If ViewData("quienlostiene") IsNot Nothing Then
        @<h3 class="touch">
            @h.traducir("Todos los recursos del tipo " + Request("nombre") + " estan ocupados")
        </h3>
        @<h3 class="touch">
            @h.traducir("Acontinuación mostramos el listado de quien tiene los recursos ocupados:")
        </h3>
        @<table class="table1">
            <thead>
                <tr>
                    <th>@h.traducir("Identificador")  </th>
                    <th>@h.traducir("Nombre completo")  </th>
                    <th>@h.traducir("Cogido el")  </th>
      </tr>
            </thead>
    <tbody> 
        @For each r In ViewData("quienlostiene")
            @<tr>
                 <td>@r.Id</td>
                 <td>
                     @r.Nombre
                     @r.Apellido1
                     @r.Apellido2
                 </td>
                 <td>@r.Coger.ToShortDateString</td>
            </tr>
        Next
    </tbody>
        </table>

    Else
@<h3 class="touch">
    @h.traducir("Seleccione el " + Request("nombre") + " que ha cogido"):
</h3>
@For Each r As Recurso In ViewData("recursos")
    @<a class="touch" href="@Url.Action("seleccion", New With {.nombregrupo = Request("nombre"), .id = r.id, .idsab = SimpleRoleProvider.GetId()})">
        @r.id
    </a>
Next
    End If

Else
@<h3 class="touch">
    @h.Traducir("Seleccione el recurso físico que vaya a dejar")
</h3>
        @For Each r As Registro In ViewData("registros")
@<form action="" method="post" class="touch">
    @Html.Hidden("idSab", r.IdSab.ToString)
    @Html.Hidden("nombregrupo", r.NombreGrupo)
    @Html.Hidden("id", r.Id)
    @Html.Hidden("coger", r.Coger)
    <input type="submit" value="@r.NombreGrupo @r.id" />
</form>
        Next
    End If
