@imports web
@Code
    ViewBag.title = "Departamento Cuentas"
End code


<h3>@h.traducir("Cuentas asignadas al departamento") @Model.nombre</h3>
<ul>
@For Each c In ViewData("lstcuentas")
    @<form action="@Url.Action("removeCuenta",New With{.idAplicacion=Request("idAplicacion"),.idDepartamento=Request("idDepartamento"),.idCuenta=c.idCuenta})" method="post">
        @Html.Hidden("idcuenta",c.idCuenta)
        <input type="submit" value="@c.nombreCuenta" />
    </form>
Next
    </ul>
    <br />
<h3>@h.traducir("Cuentas sin asignar al departamento") @Model.nombre</h3>
<ul>
 @For Each tp In ViewData("lstnocuentas")
     @<li>
         @tp.nombreTipoCuenta
         <ul>
             @For Each c In tp.listOfCuenta
                @<li>
                    <form action="@Url.Action("addCuenta",New With{.idAplicacion=Request("idAplicacion"),.idDepartamento=Request("idDepartamento"),.idCuenta=c.id})" method="post">
                        @Html.Hidden("idcuenta",c.id)
                        <input type="submit" value="@c.nombre" />
                    </form>
                 </li>
             Next
         </ul>
      </li>
             Next
    </ul>