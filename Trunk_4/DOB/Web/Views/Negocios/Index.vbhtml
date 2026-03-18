@Imports DOBLib

<script type="text/javascript">
    $(function () {
        $(".boton-eliminar").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea eliminar el elemento seleccionado?"))");
        });
    });

</script>

<h2>@Utils.Traducir("Administración - Negocios")</h2>

@code
    Dim listaNegocios As List(Of ELL.Negocio) = CType(ViewData("Negocios"), List(Of ELL.Negocio))
End Code

@If (listaNegocios.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<table class="table table-striped table-responsive table-hover table-condensed">
        <thead>
            <tr>
                <th>@Utils.Traducir("Negocio")</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            @code
                For Each negocio In listaNegocios
                    @<tr>
                        <td>@negocio.Negocio</td>
                        <td class="text-center">
                            <a href='@Url.Action("Eliminar", "Negocios", New With {.id = negocio.Id})'>
                                <span class="glyphicon glyphicon-remove boton-eliminar" aria-hidden="true" title="@Utils.Traducir("Eliminar")"></span>
                            </a>
                        </td>
                    </tr>
                Next
            End Code
        </tbody>
    </table>
                End if

@Using Html.BeginForm("Añadir", "Negocios", FormMethod.Post, New With {.Class = "form-horizontal"})
    @<h3>@Utils.Traducir("Añadir nuevo negocio")</h3>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Negocio")</label>
        <div class="col-sm-6">
            @Html.DropDownList("Divisiones", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-7 col-sm-1 text-right">
            <input type="submit" id="submit" value="@Utils.Traducir("Añadir")" class="btn btn-primary" />
        </div>
    </div>
End Using
