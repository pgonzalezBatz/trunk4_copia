@Code
    'Layout = Nothing
    Dim accion As ELL.Accion = BLL.AccionesBLL.ObtenerAccion(CInt(ViewData("idAccion")))
    Dim identificadorPeriodicidad As String = String.Empty

    Select Case accion.Periodicidad
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

<h3>@String.Format("{0} - {1}", Utils.Traducir("Evolución de la acción"), ViewData("Descripcion"))</h3>
<hr />

<canvas id="myChartAcciones" data-indic="%"></canvas>
<div id="divSinDatos" class="alert alert-info" role="alert">@Utils.Traducir("Sin datos")</div>

<script type="text/javascript">
    var GetChartDataAcc = function () {
        $.ajax({
            url: '@Url.Action("ObtenerEvoluciones", "EvolucionAcciones")',
            data: "idAccion=" + @CStr(ViewData("idAccion")),
            method: 'GET',
            dataType: 'json',
            success: function (d) {
                if(d.length > 0){
                    var labels = new Array();
                    var data = new Array();
                    var idPeriodicidad = 0;                    

                    $.each(d, function(index, value) {
                        idPeriodicidad = parseInt(value.IdPeriodicidad) + 1;
                        labels.push('@identificadorPeriodicidad ' + String(idPeriodicidad));

                        var minValue = @Decimal.MinValue;
                        var porcentaje = value.Porcentaje;
                        if(porcentaje != minValue){
                            data.push(porcentaje.toString().replace(",", "."));
                        };
                    });

                    var ctx = $("#myChartAcciones");
                    var myChart = new Chart(ctx, {
                        type: 'bar',
                        data: {
                            labels: labels,
                            datasets: [{
                                label: "@Html.Raw(Utils.Traducir("Evolución de la Acción"))",
                                data: data,
                                backgroundColor :'rgba(54, 162, 235, 0.2)',
                                borderColor : 'rgba(54, 162, 235, 1)',
                                borderWidth : 1
                            }]
                        },
                        options: {
                            scales: {
                                yAxes: [{
                                    scaleLabel: {
                                        display: true,
                                        labelString: "@Html.Raw(Utils.Traducir("Porcentaje realización"))"
                                    },
                                    ticks: {
                                        //max: valorMayor + (valorMayor / 10),
                                        //min: 0,
                                        //stepSize: valorMayor / 10,
                                        callback: function(value) {
                                            return value + "%";
                                        }
                                    }
                                }]
                            },
                            tooltips :{
                                enabled: false
                            }
                        }
                    });

                    $("#myChartAcciones").show();
                    $("#divSinDatos").hide();
                }
                else{
                    $("#myChartAcciones").hide();
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
        $(window).resize(GetChartDataAcc);
        GetChartDataAcc();
    });

</script>


