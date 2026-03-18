@Imports DOBLib

<h3>@String.Format("{0} - {1}", Utils.Traducir("Plantas despliegue"), Utils.Traducir("Agregar"))</h3>
<hr />

@code
    Dim plantasSeleccionar As List(Of ELL.Planta) = CType(ViewData("PlantasSeleccionar"), List(Of ELL.Planta))
End Code

<script type="text/javascript">
    $(function () {
        OcultarPlanta($("#Plantas").val());

        function OcultarPlanta(idPlantaCombo) {
            $("[id^='divPlanta-']").show();
            $("#divPlanta-" + idPlantaCombo).hide();
        }

        $("#Plantas").change(function () {            
            OcultarPlanta($("#Plantas").val())
        })
    })
</script>

@Using Html.BeginForm("Agregar", "PlantasDespliegue", Nothing, FormMethod.Post, New With {.class = "form-horizontal"})
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Planta padre")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Plantas", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
    <label class="col-sm-2 control-label">@Utils.Traducir("Plantas hijas")</label>
    <div class="col-sm-4">
        @code
            For each planta In plantasSeleccionar.OrderBy(Function(f) f.Planta)
                @<div id="divPlanta-@planta.Id">
                    @Html.CheckBox("chkBox-" & planta.Id, False)
                    <label>@planta.Planta</label>
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
