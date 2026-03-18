@Imports DOBLib

<h2>@Utils.Traducir("Procesos")</h2>

@code
    Dim listaEvoluciones As List(Of ELL.EvolucionAccion) = CType(ViewData("Evoluciones"), List(Of ELL.EvolucionAccion))
End Code

<script src="~/Scripts/Chart.bundle.min.js"></script>

<canvas id="myChart"></canvas>

<script type="text/javascript">
    @code
        Dim labels As String = String.Empty
        Dim data As String = String.Empty
        For Each evolucion In listaEvoluciones
            If (Not String.IsNullOrEmpty(labels)) Then
                labels &= ", "
            End If
            labels &= """" & evolucion.FechaAlta & """"

            If (Not String.IsNullOrEmpty(data)) Then
                data &= ", "
            End If
            data &= """" & evolucion.Porcentaje & """"
        Next
    End Code

    var ctx = $("#myChart");
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [@Html.Raw(labels)],
            datasets: [{
                label: "@Html.Raw(Utils.Traducir("Evolución de la Acción"))",
                data: [@Html.Raw(data)]
            },
            {
                label: "@Html.Raw(Utils.Traducir("Evolución de la Acción1"))",
                data: [@Html.Raw(data)]
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        max: 100,
                        min: 0,
                        stepSize: 10
                    }
                }]
            }
        }
    });

</script>
