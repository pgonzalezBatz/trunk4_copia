@Code
    Layout = Nothing
End Code

@Imports DOBLib

@code
    Dim plantasHijas As List(Of ELL.PlantasDespliegue) = CType(ViewData("PlantasHijas"), List(Of ELL.PlantasDespliegue))
End Code

@If (plantasHijas.Count = 0) Then
    @<div class="col-sm-12">
    @Html.Label(String.Empty, Utils.Traducir("Objetivo ya desplegado en plantas"))
</div>
Else
    @<div>
        <ul style="list-style: none;">
            @for each planta In plantasHijas.OrderBy(Function(f) f.PlantaHija)
                @<li>
                    <div>
                        @Html.CheckBox("chkBox-" & planta.IdPlantaHija)
                        <label>@planta.PlantaHija</label>
                    </div>
                </li>
            Next
        </ul>
    </div>
End If
