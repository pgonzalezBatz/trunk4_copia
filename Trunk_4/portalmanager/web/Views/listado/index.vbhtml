@imports web
@Code
    ViewBag.title = "Listados de información para el manager"
End Code
@section  header
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
End Section

@code
    'Dim fixedUrl = "http://usotegieta2.batz.es:80/ibmcognos/cgi-bin/cognosisapi.dll?b_action=xts.run&m=portal/report-viewer.xts&method=execute&m_obj=/content/folder[@name='BATZ']/folder[@name='Zerbitzu Orokorrak - Servicios Generales']/folder[@name='04 - Portal Manager']/"
    Dim fixedUrl = "https://cognos.batz.es/ibmcognos/bi/?Download=false&prompt=false&pathRef=.public_folders%2FINTRANET+APP%2FREPORTS+APP%2FPORTAL+MANAGER%2F"
    Dim urlVacaciones = fixedUrl + "VACACIONES"
    Dim urlOpcionales = fixedUrl + "OPCIONALES"
    Dim urlIndices = fixedUrl + "EVOLUCION+INDICES"
    Dim urlExtras = fixedUrl + "HORAS+EXTRAS"
    Dim urlAbsentismo = fixedUrl + "ABSENTISMO"
    Dim urlCalendarioMovil = fixedUrl + "CALENDARIO MOVIL"
    Dim urlResumenAnualTrabajador = fixedUrl + "RESUMEN+ANUAL+TRABAJADOR"
    Dim urlContratos = fixedUrl + "CONTRATOS"
    Dim urlPlanificacion = fixedUrl + "report[@name='PLANIFICACION']&run.outputFormat="

    Dim urlVisionGeneralEvaluacionesPropuestas = "https://cognos.batz.es/ibmcognos/bi/?pathRef=.public_folders%2FREPORTS%2FRRHH%2FEVALUACIONES%2FEVALUACIONES+Y+PROPUESTAS+DE+CONTINUIDAD&format=HTML&Download=false&prompt=true"

    Dim urlColaboradores = fixedUrl + "DATOS+TRABAJADORES"
    Dim urlEvaluaciones = fixedUrl + "EVALUACIONES"
    Dim urlEvaluacionesbajas = fixedUrl + "EVALUACIONES+BAJAS"

End Code

<div class="row mt-2 alert alert-info">
    <div class="col-6">
        <strong>@H.Traducir("Ejercicio seleccionado"): @ViewData("Ejercicioactual")</strong>
    </div>
    <div class="col-6">
        @If ViewData("Ejercicioactual") = Now.Year Then
            @<a href="@Url.Action("index", H.ToRouteValues(Request.QueryString, New With {.ejercicio = ViewData("Ejercicioactual") - 1}))">@H.Traducir("Seleccionar ejercicio anterior")</a>
        Else
            @<a href="@Url.Action("index", H.ToRouteValues(Request.QueryString, New With {.ejercicio = ViewData("Ejercicioactual") + 1}))">@H.Traducir("Seleccionar ejercicio siguiente")</a>
        End If
    </div>
</div>
<div class="row">
    <div class="col-xl-6">
        <h5>DATOS CONTROL DE PRESENCIA</h5>
        <table class="table mt-4">
            <tr>
                <td>@H.Traducir("Consumos y saldos de vacaciones")</td>
                <td>
                    <a href="@(urlVacaciones)&format=HTML&p_Ano=@ViewData("ejercicioactual")&p_Trabajador=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Report.png")" />
                    </a>
                    <a href="@(urlVacaciones)&format=singleXLS&p_Ano=@ViewData("ejercicioactual")&p_Trabajador=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Excel.png")" />
                    </a>
                    <a href="@(urlVacaciones)&format=PDF&p_Ano=@ViewData("ejercicioactual")&p_Trabajador=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Pdf.png")" />
                    </a>
                </td>
            </tr>
            <tr>
                <td>@H.Traducir("Consumos y saldos de horas opcionales")</td>
                <td>
                    <a href="@(urlOpcionales)&format=HTML&p_Ano=@ViewData("ejercicioactual")&p_Trab=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Report.png")" />
                    </a>
                    <a href="@(urlOpcionales)&format=singleXLS&p_Ano=@ViewData("ejercicioactual")&p_Trab=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Excel.png")" />
                    </a>
                    <a href="@(urlOpcionales)&format=PDF&p_Ano=@ViewData("ejercicioactual")&p_Trab=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Pdf.png")" />
                    </a>
                </td>
            </tr>

            <tr>
                <td>@H.Traducir("Consumos y saldos de horas extras")</td>
                <td>
                    <a href="@(urlExtras)&format=HTML&p_Ano=@ViewData("ejercicioactual")&p_Trab=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Report.png")" />
                    </a>
                    <a href="@(urlExtras)&format=singleXLS&p_Ano=@ViewData("ejercicioactual")&p_Trab=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Excel.png")" />
                    </a>
                </td>
            </tr>
            <tr>
                <td>@H.Traducir("Calendario Movil")</td>
                <td>

                    <a href="@(urlCalendarioMovil)&format=HTML&p_Trabajador=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Report.png")" />
                    </a>
                    <a href="@(urlCalendarioMovil)&format=singleXLS&p_Trabajador=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Excel.png")" />
                    </a>
                </td>
            </tr>
            <tr>
                <td>@H.Traducir("Resumen Anual Vacaciones, Opcionales, Horas y Calendario Movil")</td>
                <td>
                    <a href="@(urlResumenAnualTrabajador)&format=HTML&p_Trab=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Report.png")" />
                    </a>
                    <a href="@(urlResumenAnualTrabajador)&format=singleXLS&p_Trabajador=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Excel.png")" />
                    </a>
                </td>
            </tr>
            <tr>
                <td>@H.Traducir("Resumen absentismo")</td>
                <td>
                    <a href="@(urlAbsentismo)&format=HTML&p_Ano=@ViewData("ejercicioactual")&p_Trab=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Report.png")" />
                    </a>
                    <a href="@(urlAbsentismo)&format=singleXLS&p_Ano=@ViewData("ejercicioactual")&p_Trab=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Excel.png")" />
                    </a>
                </td>
            </tr>
        </table>
    </div>
    <div class="col-xl-6">
        <h5>INDICES, CONTRATOS Y EVOLUCIONES</h5>
        <table class="table mt-4">
            <tr>
                <td>@H.Traducir("Evolucion de indices")</td>
                <td>
                    <a href="@(urlIndices)&format=HTML&p_Ano=@ViewData("ejercicioactual")&p_Jefe=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Report.png")" />
                    </a>
                </td>
            </tr>
            <tr>
                <td>@H.Traducir("Finalizacion de contratos")</td>
                <td>
                    <a href="@(urlContratos)&format=HTML&p_Ano=@ViewData("ejercicioactual")&p_responsable=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Report.png")" />
                    </a>
                    <a href="@(urlContratos)&format=singleXLS&p_Ano=@ViewData("ejercicioactual")&p_responsable=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Excel.png")" />
                    </a>
                </td>
            </tr>
            <tr>
                <td>@H.Traducir("Datos de colaboradores")</td>
                <td>
                    <a href="@(urlColaboradores)&format=HTML&p_Responsable=@(ViewData("usuario").codpersona)" target="_blank">
                        <img src="@Url.Content("~/Content/Report.png")" />
                    </a>
                </td>
            </tr>
            <tr>
                <td>@H.Traducir("Datos de evaluaciones")</td>
                <td>
                    <a href="@(urlEvaluaciones)&format=HTML&p_Responsable=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Report.png")" />
                    </a>
                </td>
            </tr>
            <tr>
                <td>@H.Traducir("Datos de evaluaciones trabajadores de baja")</td>
                <td>
                    <a href="@(urlEvaluacionesbajas)&format=HTML&p_Responsable=@ViewData("usuario").codpersona" target="_blank">
                        <img src="@Url.Content("~/Content/Report.png")" />
                    </a>
                </td>
            </tr>
        </table>
    </div>
</div>
<div class="row">
    <div class="col-xl-6">
        @If SimpleRoleProvider.IsUserAuthorised(Role.rrhh) Then
            @<hr />
            @<h5>EVALUACIONES Y PROPUESTAS DE CONTINUIDAD</h5>
            @<Table Class="table mt-4">
                <tr>
                    <td>@H.Traducir("Evaluaciones y propuestas de continuidad")</td>
                    <td>
                        <a href="@(urlVisionGeneralEvaluacionesPropuestas)" target="_blank">
                            <img src="@Url.Content("~/Content/Report.png")" />
                        </a>
                    </td>
                </tr>
            </Table>
        End If

    </div>
    <div class="col-xl-6">

    </div>
</div>




