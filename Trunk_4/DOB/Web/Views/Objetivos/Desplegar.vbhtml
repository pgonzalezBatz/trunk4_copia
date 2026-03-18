@Imports DOBLib

@code
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
    Dim objetivo As ELL.Objetivo = CType(ViewData("Objetivo"), ELL.Objetivo)
    Dim idPlantaDespliegue As Integer = CInt(ViewData("IdPlantaDespliegue"))
    Dim plantaDespliegue As ELL.Planta = BLL.PlantasBLL.ObtenerPlanta(idPlantaDespliegue)
    Dim plantasDespliegue As List(Of Integer) = CType(ViewData("PlantasDespliegue"), List(Of Integer))
End Code

<script type="text/javascript">

    $(function () {
        $('[data-toggle="popover"]').popover({
            title: '@Html.Raw(Utils.Traducir("Información"))',
            content: '@Html.Raw(Utils.Traducir("Ascendente: cuanto más valor mejor. Si nos acercamos o quedamos por encima del valor objetivo es bueno. Descendente: cuanto menos mejor, Si nos acercamos o quedamos por debajo del valor objetivo es bueno."))',
            trigger: 'focus',
            placement: 'bottom'
        });

        // Este if es porque si la planta a la que se quiere desplegar el objetivo hereda retos no tiene que poder cambiar el reto. Coge el del padre
        if ('@plantaDespliegue.HeredaRetos' == 'True') {
            $("#Retos").prop("disabled", true);
        } else {
            $("#Retos").prop("disabled", false);
        }
    });

</script>

<h3>@String.Format("{0} - {1}. {2} > {3}", Utils.Traducir("Objetivos"), Utils.Traducir("Desplegar"), Utils.Traducir("Planta destino"), BLL.PlantasBLL.ObtenerPlanta(idPlantaDespliegue).Planta)</h3>
<hr />

@Using Html.BeginForm("DesplegarAPlanta", "Objetivos", New With {.idObjetivo = objetivo.Id}, FormMethod.Post, New With {.class = "form-horizontal"})
    @Html.Hidden("hIdObjetivo", objetivo.Id)
    @Html.Hidden("hIdPlantaDestino", idPlantaDespliegue)

    For Each planta In plantasDespliegue
        @Html.CheckBox("chkBox-" & planta, True, New With {.style = "display:none"})
    Next

    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Descripción")</label>
        <div class="col-sm-6">
            @Html.TextArea("descripcion", objetivo.Descripcion, New With {.maxlength = "200", .rows = "5", .required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Reto")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Retos", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Proceso")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Procesos", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Responsable")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Responsables", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Fecha objetivo")</label>
        <div class="col-sm-4">
            <div class="input-group date" id="fecha">
                @Html.TextBox("fechaObjetivo", objetivo.FechaObjetivo.ToShortDateString(), New With {.class = "form-control", .disabled = "disabled"})
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Indicador")</label>
        <div class="col-sm-4">
            @Html.TextBox("indicador", objetivo.NombreIndicador, New With {.maxlength = "100", .class = "form-control", .disabled = "disabled"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Sentido")</label>
        <div class="col-sm-4">
            @Html.RadioButton("sentido", 1, objetivo.Sentido, New With {.disabled = "disabled"})&nbsp;<label>@Utils.Traducir("Ascendente")</label>
            @Html.RadioButton("sentido", 0, Not objetivo.Sentido, New With {.disabled = "disabled"})&nbsp;<label>@Utils.Traducir("Descendente")</label>
            <a href="#" data-toggle="popover">
                <span class="glyphicon glyphicon glyphicon-info-sign" aria-hidden="true"></span>
            </a>
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Descripción indicador")</label>
        <div class="col-sm-4">
            @Html.TextArea("descripcionIndicador", objetivo.DescripcionIndicador, New With {.maxlength = "200", .rows = "2", .class = "form-control", .disabled = "disabled"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Tipo indicador")</label>
        <div class="col-sm-4">
            @Html.DropDownList("TiposIndicadores", Nothing, New With {.required = "required", .class = "form-control", .disabled = "disabled"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Valor inicial")</label>
        <div class="col-sm-4">
            @Html.TextBox("valorInicial", objetivo.ValorInicial, New With {.required = "required", .type = "number", .step = "any", .class = "form-control text-right"})
        </div>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Valor objetivo")</label>
        <div class="col-sm-4">
            @Html.TextBox("valorObjetivo", objetivo.ValorObjetivo, New With {.required = "required", .type = "number", .step = "any", .class = "form-control text-right"})
        </div>
    </div>
    @<div Class="form-group hide">
        <label class="col-sm-2 control-label">@Utils.Traducir("Periodicidad")</label>
        <div class="col-sm-4">
            @Html.DropDownList("Periodicidad", Nothing, New With {.required = "required", .class = "form-control"})
        </div>
    </div>
    @<div Class="form-group">
        <div class="col-sm-offset-2 col-sm-4">
            @code
                Dim textoBotonDespliegue As String = String.Empty
                If (plantasDespliegue.Count > 0) Then
                    textoBotonDespliegue = Utils.Traducir("Desplegar") & " " & Utils.Traducir("y siguiente planta")
                Else
                    textoBotonDespliegue = Utils.Traducir("Desplegar") & " " & Utils.Traducir("y finalizar")
                End If
            End Code

            <input type="submit" id="submit" value="@textoBotonDespliegue" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>
                End Using