<!DOCTYPE html>
<html lang="es">
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <title>@Utils.Traducir("Despliegue de objetivos")</title>
    <link href="~/Content/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="~/Content/bootstrap.css" rel="stylesheet" />
    <link href="~/Content/style.css" rel="stylesheet" type="text/css" />
    <link href="~/Content/bootstrap-datetimepicker.css" rel="stylesheet" />
    <script src="~/Scripts/jquery-1.12.4.min.js"></script>
    <script src="~/Scripts/bootstrap.min.js"></script>
    <script src="~/Scripts/moment-with-locales.js"></script>
    <script src="~/Scripts/bootstrap-datetimepicker.js"></script>
    <script src="~/Scripts/ckeditor/ckeditor.js"></script>
    <script src="~/Scripts/Chart.bundle.min.js"></script>
    <script type="text/javascript">

        // Registra un plugin que añade texto cuando el grafico se ha pintado
        // Deshabilitar los tooltips
        Chart.pluginService.register({
            afterDraw: function (chartInstance) {
                var ctx = chartInstance.chart.ctx;
                // render the value of the chart above the bar
                ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontFamily, 'normal', Chart.defaults.global.defaultFontFamily);
                ctx.textAlign = 'center';
                ctx.textBaseline = 'bottom';
                ctx.fillStyle = Chart.defaults.global.defaultFontColor;

                chartInstance.data.datasets.forEach(function (dataset) {
                    for (var i = 0; i < dataset.data.length; i++) {
                        var model = dataset._meta[Object.keys(dataset._meta)[0]].data[i]._model;
                        ctx.fillText(dataset.data[i] + " " + $(ctx.canvas).data("indic"), model.x, model.y - 2);
                    }
                });
            }
        });

        Chart.defaults.global.elements.rectangle.backgroundColor = 'rgba(54, 162, 235, 0.2)';
        Chart.defaults.global.elements.rectangle.borderColor = 'rgba(54, 162, 235, 1)';
        Chart.defaults.global.elements.rectangle.borderWidth = 1;

        Chart.defaults.global.elements.line.backgroundColor = 'rgba(54, 162, 235, 0.2)';
        Chart.defaults.global.elements.line.borderColor = 'rgba(54, 162, 235, 1)';
        Chart.defaults.global.elements.line.borderWidth = 1;

        Chart.defaults.global.elements.point.backgroundColor = 'rgba(54, 162, 235, 0.2)';
        Chart.defaults.global.elements.point.borderColor = 'rgba(54, 162, 235, 1)';
        Chart.defaults.global.elements.point.borderWidth = 1;

        function MostrarMensaje(modo) {
            $("#mensaje").toggleClass(modo);
            $("#mensaje").slideDown('fast').delay(5000).slideUp(function () {
                $(this).toggleClass(modo)
            });
        }

        @*var culture = "@CStr(Session("Ticket").Culture)";
        if(culture != "en-GB"){
            culture = culture.Substring(0, culture.IndexOf("-")).ToLower();
        }
        else{
            culture = "en-GB";
        }

        var options = $.extend(
            {},
            $.datepicker.regional[culture],
            {
                changeMonth: true,
                changeYear: true,
                minDate: null,
                showWeek: true,
                firstDay: 1
            }
        );

        $.datepicker.setDefaults(options);*@
    </script>
</head>
<body>
    <header>
        @code
            @If ((TempData("menu") Is Nothing)) Then
                @Html.Partial("~/Views/Shared/_NavMenu.vbhtml", Session("Ticket"))
            End If
        End Code
    </header>
    <div id="mensaje">
        @code
            @If (Not (TempData("topbar") Is Nothing)) Then
                Dim topbar As Topbar = CType(TempData("topbar"), Topbar)

                @Html.Label(String.Empty, topbar.Mensaje, New With {.class = "negrita"})
                @<script type="text/javascript">
                    MostrarMensaje('@topbar.Estilo');
                </script>
            End If
        End code
    </div>
    @code
        Dim containerClass = "container"
        If (ViewData.ContainsKey("ContainerFluid")) Then
            containerClass = "container-fluid"
        End If
    End Code

    <div Class="@containerClass">
        @RenderBody()
    </div>
    <footer style="margin-top:15px;">
        <div class="panel panel-default text-center">
            <div class="panel-heading">@Utils.Traducir("encasodeteneralgunproblema contactarcon") <a href="/helpdesk" target="_blank">Helpdesk</a></div>
        </div>
    </footer>
</body>
</html>

@code
    If (Not (TempData("seleccionarRol") Is Nothing)) Then
        Dim seleccionarRol As Boolean = CType(TempData("seleccionarRol"), Boolean)

        If (seleccionarRol) Then
            @<div Class="modal fade" id="modalWindowRoles" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                <div Class="modal-dialog" role="document">
                    <div Class="modal-content">
                        <div Class="modal-header">
                            <h4 Class="modal-title">@Utils.Traducir("Seleccione una planta")</h4>
                        </div>
                        <div id="modalBodyObjetivos" Class="modal-body">
                            @Using Html.BeginForm("SeleccionarRol", "Login", FormMethod.Post)
                                @<div Class="form-group">
                                    <label class="form-control-label">@Utils.Traducir("Planta")</label>
                                    @Html.DropDownList("Roles", Nothing, New With {.required = "required", .class = "form-control"})
                                </div>
                                @<div Class="form-group">
                                    <div class="text-right">
                                        <input type="submit" id="submit" value="@Utils.Traducir("Aceptar")" class="btn btn-primary input-block-level form-control" />
                                    </div>
                                </div>
                            End Using
                        </div>
                    </div>
                </div>
            </div>
            @<script type="text/javascript">
                 $('#modalWindowRoles').modal('show');
            </script>
        End If
    End If
End Code
