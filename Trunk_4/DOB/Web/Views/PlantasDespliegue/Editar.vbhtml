@Imports DOBLib

<h3>@String.Format("{0} - {1}", Utils.Traducir("Plantas despliegue"), Utils.Traducir("Editar"))</h3>
<hr />

@code
    Dim plantasSeleccionar As List(Of ELL.Planta) = CType(ViewData("PlantasSeleccionar"), List(Of ELL.Planta))
    Dim plantasHijas As List(Of ELL.PlantasDespliegue) = CType(ViewData("PlantasHijas"), List(Of ELL.PlantasDespliegue))
End Code

<script type="text/javascript">
    $(function () {
        OcultarPlanta($("#Plantas").val());

        function OcultarPlanta(idPlantaCombo) {
            $("[id^='divPlanta-']").show();
            $("#divPlanta-" + idPlantaCombo).hide();
        }
    })
</script>

@Using Html.BeginForm("EditarPlantas", "PlantasDespliegue", Nothing, FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Planta padre")</label>
        <div class="col-sm-4">
            @Html.Hidden("hIdPlantaPadre", ViewData("IdPlantaPadre"))
            @Html.DropDownList("Plantas", Nothing, New With {.disabled = "disabled", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
    <label class="col-sm-2 control-label">@Utils.Traducir("Plantas hijas")</label>
    <div class="col-sm-4">
        @code
            ' Cargamos los objetivos de la planta
            Dim listaObjetivosPlanta As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListado(CInt(ViewData("IdPlantaPadre")))
            Dim listaObjetivosHijos As List(Of ELL.Objetivo)

            For Each planta In plantasSeleccionar.OrderBy(Function(f) f.Planta)
                listaObjetivosHijos = New List(Of ELL.Objetivo)

                ' De cada objetivo miramos si tiene hijos para la planta
                listaObjetivosPlanta.ForEach(Sub(s) listaObjetivosHijos.AddRange(BLL.ObjetivosBLL.CargarListadoPorPadre(s.Id).Where(Function(f) f.IdPlanta = planta.Id)))

                @<div id="divPlanta-@planta.Id">
    @code
        If (listaObjetivosHijos.Count > 0) Then
            @Html.CheckBox("chkBox-" & planta.Id, plantasHijas.Exists(Function(f) f.IdPlantaHija = planta.Id), New With {.onclick = "return false;"})
            @<label class="text-success">@planta.Planta</label>
        Else
            @Html.CheckBox("chkBox-" & planta.Id, plantasHijas.Exists(Function(f) f.IdPlantaHija = planta.Id))
                        @<label>@planta.Planta</label>
        End If
    End code
</div>
        Next
                        End Code
    </div>
</div>
    @<div Class="form-group">
        <div class="col-sm-offset-2 col-sm-4">
            <input type="submit" id="submit" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>
            End Using
