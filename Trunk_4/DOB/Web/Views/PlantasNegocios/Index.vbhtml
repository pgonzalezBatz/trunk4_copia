@Imports DOBLib

<script type="text/javascript">
    $(function () {
        $(".boton-eliminar").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea eliminar el elemento seleccionado?"))");
        });
    });

</script>

<h2>@Utils.Traducir("Administración - Plantas/negocios")</h2>

@code
    Dim listaPlantasNegocios As List(Of ELL.PlantaNegocio) = CType(ViewData("PlantasNegocios"), List(Of ELL.PlantaNegocio))
    Dim listaPlantas = From p In listaPlantasNegocios
                       Order By p.Planta
                       Group p By p.IdPlanta, p.Planta Into Group
                       Select New With {.IdPlanta = IdPlanta, .Planta = Planta}
    Dim listaNegocios = From p In listaPlantasNegocios
                        Order By p.Negocio
                        Group p By p.IdNegocio, p.Negocio Into Group
                        Select New With {.IdNegocio = IdNegocio, .Negocio = Negocio}
End Code

@If (listaPlantasNegocios.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<table class="table table-striped table-responsive table-hover table-condensed">
        <thead>
            <tr>
                <th></th>
                @for Each negocio In listaNegocios
                    @<th class="text-center">@negocio.Negocio</th>
                Next
            </tr>
        </thead>
        <tbody>
            @code
                For Each planta In listaPlantas
                    @<tr>
                        <td>
                            @planta.Planta
                        </td>
                        @For Each negocio In listaNegocios
                            @<td class="text-center">
                                @code
    Dim plantaNegocio As ELL.PlantaNegocio = listaPlantasNegocios.FirstOrDefault(Function(f) f.IdPlanta = planta.IdPlanta AndAlso f.IdNegocio = negocio.IdNegocio)
                                End Code
                                @if(plantaNegocio IsNot Nothing) Then


                                    @<a href='@Url.Action("Eliminar", "PlantasNegocios", New With {.id = plantaNegocio.Id})'>
                                        <span class="glyphicon glyphicon-remove boton-eliminar" aria-hidden="true" title="@Utils.Traducir("Eliminar")"></span>
                                    </a>
                                End If
                            </td>
                                    Next
                    </tr>
                                    Next
            End Code
        </tbody>

    </table>
                                    End If

@Using Html.BeginForm("Añadir", "PlantasNegocios", FormMethod.Post, New With {.class = "form-horizontal"})
    @<h3>@Utils.Traducir("Añadir nueva planta/negocio")</h3>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Planta")</label>
        <div class="col-sm-6">
            @Html.DropDownList("Plantas", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Negocios")</label>
        <div class="col-sm-6">
            @Html.DropDownList("Negocios", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
        @<div Class="form-group">
            <div class="col-sm-offset-7 col-sm-1 text-right">
                <input type="submit" id="submit" value="@Utils.Traducir("Añadir")" class="btn btn-primary" />
            </div>
        </div>
End Using
