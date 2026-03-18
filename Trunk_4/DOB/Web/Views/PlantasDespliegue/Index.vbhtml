@Imports DOBLib

<h3>@String.Format("{0} - {1}", Utils.Traducir("Administración"), Utils.Traducir("Plantas despliegue"))</h3>
<hr />

@code
    Dim listaPlantas As List(Of ELL.PlantasDespliegue) = CType(ViewData("PlantasDespliegue"), List(Of ELL.PlantasDespliegue))

    Dim listaIdsPlantasUnicos As IEnumerable(Of Object) = (From lstPlantas In listaPlantas
                                                           Group lstPlantas By lstPlantas.IdPlantaPadre, lstPlantas.PlantaPadre Into agrupacion = Group
                                                           Select New With {.IdPlantaPadre = IdPlantaPadre, .PlantaPadre = PlantaPadre}).ToList()
End Code

<a href="@Url.Action("Agregar")" Class="btn btn-info">
    <span Class="glyphicon glyphicon-plus"></span> @Utils.Traducir("Nueva")
</a>
<br /><br />

@If (listaPlantas.Count = 0) Then
    @Html.Label(String.Empty, Utils.Traducir("noExisteNingunRegistro"))
Else
    @<div class="row">
        <div class="col-sm-10">
            <table class="table table-striped table-hover table-condensed">
                <thead>
                    <tr>
                        <th>@Utils.Traducir("Planta padre")</th>
                        <th>@Utils.Traducir("Plantas hijas")</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @code
                        For Each planta In listaIdsPlantasUnicos.OrderBy(Function(f) f.PlantaPadre)
                            @<tr>
                                <td>@planta.PlantaPadre</td>
                                <td>
                                    <ul>
                                        @code
                                            For Each plantaHija In listaPlantas.Where(Function(f) f.IdPlantaPadre = planta.IdPlantaPadre).OrderBy(Function(f) f.PlantaHija)
                                                @<li>@plantaHija.PlantaHija</li>
                                            Next
                                        End code
                                    </ul>
                                </td>
                                <td Class="text-center">
                                    <a href='@Url.Action("Editar", "PlantasDespliegue", New With {.idPlantaPadre = planta.IdPlantaPadre})'>
                                        <span class="glyphicon glyphicon-pencil" aria-hidden="true" title="@Utils.Traducir("Editar")"></span>
                                    </a>
                                </td>
                            </tr>
                        Next
                    End Code
                </tbody>
            </table>
        </div>
    </div>
End If