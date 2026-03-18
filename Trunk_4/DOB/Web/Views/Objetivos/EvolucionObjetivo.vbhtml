@Code
    'Layout = Nothing
    Dim objetivo As ELL.Objetivo = BLL.ObjetivosBLL.ObtenerObjetivo(CInt(ViewData("idObjetivo")))
    Dim identificadorPeriodicidad As String = String.Empty

    Select Case objetivo.Periodicidad
        Case ELL.Objetivo.TipoPeriodicidad.Mensual
            identificadorPeriodicidad = Utils.Traducir("Mes")
        Case ELL.Objetivo.TipoPeriodicidad.Trimestral
            identificadorPeriodicidad = Utils.Traducir("Trimestre")
        Case ELL.Objetivo.TipoPeriodicidad.Cuatrimentral
            identificadorPeriodicidad = Utils.Traducir("Cuatrimestre")
        Case ELL.Objetivo.TipoPeriodicidad.Semestral
            identificadorPeriodicidad = Utils.Traducir("Semestre")
    End Select
End Code

@Imports DOBLib

<h3>@String.Format("{0} - {1}", Utils.Traducir("Evolución del objetivo"), ViewData("Descripcion"))</h3>
<hr />

<canvas id="myChartObjetivos" data-indic=""></canvas>
<div id="divSinDatos" class="alert alert-info" role="alert">@Utils.Traducir("Sin datos")</div>

<script type="text/javascript">
    var GetChartDataObj = function () {
        $.ajax({
            url: '@Url.Action("ObtenerEvoluciones", "EvolucionObjetivos")',
            data: "idObjetivo=" + @CStr(ViewData("idObjetivo")),
            method: 'GET',
            dataType: 'json',
            success: function (d) {                
                if(d.length > 0){
                    $("myChartObjetivos").data("indic", d[0].TipoIndicador);                    

                    var labels = new Array();
                    var data = new Array();
                    var data1 = new Array();
                    var valorObjetivo = d[0].ValorObjetivo;
                    var tipoIndicador = d[0].TipoIndicador;
                    var idPeriodicidad = 0;
                    //var valorMayor = 0;

                    $.each(d, function(index, value) {                        
                        var minValue = @Decimal.MinValue;
                        var valorActual = value.ValorActual;
                        var valorObjetivo = value.ValorObjetivo;                        

                        idPeriodicidad = parseInt(value.IdPeriodicidad) + 1;
                        labels.push('@identificadorPeriodicidad ' + String(idPeriodicidad));                        

                        if(valorActual != minValue){
                            data.push(valorActual.toString().replace(",", "."));
                            data1.push(valorObjetivo.toString().replace(",", "."));
                        };
                    });

                    var ctx = $("#myChartObjetivos");
                    var myChart = new Chart(ctx, {
                        type: 'line',
                        data: {
                            labels: labels,
                            datasets:  [{
                                label: "@Html.Raw(Utils.Traducir("Evolución del Objetivo"))",
                                data: data,
                                backgroundColor:  "rgba(54, 162, 235, 0.2)",
                                borderColor: "rgba(54, 162, 235, 1)",
                                pointBorderColor: "rgba(54, 162, 235, 1)",
                                pointBackgroundColor: "rgba(54, 162, 235, 0.2)",
                            },
                            {
                                label: "@Html.Raw(Utils.Traducir("Valor objetivo"))",
                                fill: false,
                                backgroundColor: "rgba(255,0,0,0.2)",
                                borderColor: "rgba(255,0,0,1)",
                                pointBorderColor: "rgba(255,0,0,1)",
                                pointBackgroundColor: "rgba(255,0,0,0.2)",
                                data: data1
                            }]
                        },
                        options: {
                            scales: {
                                yAxes: [{
                                    scaleLabel: {
                                        display: true,
                                        labelString: "@Html.Raw(Utils.Traducir("Valor"))" + " (" +  tipoIndicador + ")"
                                    }
                                    //,ticks: {
                                    //    max: valorMayor + (valorMayor / 10),
                                    //    min:  0,
                                    //    stepSize: valorMayor / 10
                                    //}
                                }]
                            },
                            tooltips :{
                                enabled: false
                            }
                        }
                    });

                    $("#myChartObjetivos").show();
                    $("#divSinDatos").hide();
                }
                else{
                    $("#myChartObjetivos").hide();
                    $("#divSinDatos").show();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    };

    $(function () {
        $(window).resize(GetChartDataObj);
        GetChartDataObj();
    });

</script>
