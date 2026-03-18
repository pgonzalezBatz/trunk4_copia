@imports web
@Code
    ViewBag.title = "Tipos cuenta"
End code

@If ViewData("cuentaslibre").count = 0 Then
    @<h3> @h.traducir("No se ha encontrado ningun elemento")</h3>
Else
    @<table class="table3">
        <thead>
            <tr>
                <th>@h.traducir("Id")</th>
                <th>@h.traducir("Nombre")</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @For Each e In ViewData("cuentaslibre")
                @<tr>
                    <td>@e.id</td>
                    <td>@e.nombre</td>
                    <td>
                        
                        <form action="" method="post">
                            @Html.Hidden("idtipocuenta", e.id)
                            <input type="submit" value="+" name="add"/>
                            <input type="submit" value="-" name="subtract"/>
                        </form>
                    </td>
                </tr>
            Next
        </tbody>
    </table>
End If

    <h3> @h.traducir("Formula")</h3>
    <div class="formula">

        0 = 

@For Each e In ViewData("cuentasnolibres")
        @<form action="" method="post">
             <span>
                 @If e.SumaResta = "resta" Then
                     @Html.Encode("-")
                 Else
                     @Html.Encode("+")
                 End If
             </span>
             @e.nombre
            @Html.Hidden("idtipocuenta", e.id)
                <input type="submit" value="quitar" name="delete" />
        </form>
Next
    </div>