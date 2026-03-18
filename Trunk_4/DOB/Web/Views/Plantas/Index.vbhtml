@Imports DOBLib

<script type="text/javascript">
    $(function () {
        $(".boton-eliminar").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea eliminar el elemento seleccionado?"))");
        });

        $(".boton-baja").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea dar de baja el elemento seleccionado?"))");
        });

        $(".boton-alta").click(function () {
            return confirm("@Html.Raw(Utils.Traducir("¿Desea dar de alta el elemento seleccionado?"))");
        });
    });

</script>

<h3>@String.Format("{0} - {1}", Utils.Traducir("Administración"), Utils.Traducir("Plantas"))</h3>
<hr />

@code
    Dim listaPlantas As List(Of ELL.Planta) = CType(ViewData("Plantas"), List(Of ELL.Planta))
End Code

@If (listaPlantas.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<div class="row">
        <div class="col-sm-10">
            <table class="table table-striped table-hover table-condensed">
                <thead>
                    <tr>
                        <th>@Utils.Traducir("Planta")</th>
                        <th>@Utils.Traducir("Planta padre")</th>
                        <th>@Utils.Traducir("¿Hereda retos del padre?")</th>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @code
                        For Each planta In listaPlantas
                            @<tr>
                                @If (planta.FechaBaja <> DateTime.MinValue) Then
                                    @<td><span style="text-decoration: line-through">@planta.Planta</span></td>
                                Else
                                    @<td>@planta.Planta</td>
                                End If
                                <td>@planta.PlantaPadre</td>                                
                                @If (planta.HeredaRetos) Then
                                    @<td align="center">@Utils.Traducir("Si")</td>
                                Else
                                    @<td align="center">@Utils.Traducir("No")</td>
                                End If
                                <td Class="text-center">
                                    <a href ='@Url.Action("Eliminar", "Plantas", New With {.id = planta.Id})'>
                                        <span class="glyphicon glyphicon-remove boton-eliminar" aria-hidden="true" title="@Utils.Traducir(" Eliminar")"></span>
                                                                                            </a>
                                </td>
                                <td class="text-center">
                                    @If (planta.FechaBaja = DateTime.MinValue) Then
                                        @<a href='@Url.Action("DarBajaAlta", "Plantas", New With {.id = planta.Id, .baja = True})'>
                                            <span class="glyphicon glyphicon-circle-arrow-down boton-baja" aria-hidden="true" title="@Utils.Traducir("Dar de baja")"></span>
                                        </a>
                                    Else
                                        @<a href='@Url.Action("DarBajaAlta", "Plantas", New With {.id = planta.Id, .baja = False})'>
                                            <span class="glyphicon glyphicon-circle-arrow-up boton-alta" aria-hidden="true" title="@Utils.Traducir("Dar de alta")"></span>
                                        </a>
                                    End If
                                </td>
                            </tr>
                        Next
                    End Code
                </tbody>
            </table>
        </div>
    </div>
                        End If

<div class="row">
    <div class="col-sm-10">
        <hr />
    </div>
</div>

<div class="row">
    <div class="col-sm-12">
        @Using Html.BeginForm("Añadir", "Plantas", FormMethod.Post, New With {.class = "form-horizontal"})
            @<h4>@Utils.Traducir("Añadir nueva planta")</h4>
            @<div Class="form-group">
                <label class="col-sm-3 control-label">@Utils.Traducir("Nombre")</label>
                <div class="col-sm-3">
                    @Html.TextBox("nombre", Nothing, New With {.maxlength = "50", .class = "form-control", .required = "required", .style = "text-transform:uppercase"})
                </div>
            </div>
            @<div Class="form-group">
                <label class="col-sm-3 control-label">@Utils.Traducir("Planta padre")</label>
                <div class="col-sm-3">
                    @Html.DropDownList("PlantasPadre", Nothing, New With {.class = "form-control"})
                </div>
            </div>
            @<div Class="form-group">
                <label class="col-sm-3 control-label">@Utils.Traducir("¿Hereda retos del padre?")</label>
                <div class="col-sm-3">
                    @Html.RadioButton("rbHeredaRetos", 1)
                    <label Class="control-label">@Utils.Traducir("Si")</label>
                    @Html.RadioButton("rbHeredaRetos", 0, True) 
                    <label Class="control-label">@Utils.Traducir("No")</label>
                </div>
            </div>
            @<div Class="form-group">
                <div class="col-sm-offset-3 col-sm-3">
                    <input type="submit" id="submit" value="@Utils.Traducir("Añadir")" class="btn btn-primary input-block-level form-control" />
                </div>
            </div>
        End Using
    </div>
</div>
