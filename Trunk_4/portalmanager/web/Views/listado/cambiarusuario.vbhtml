@imports web
@Code
    ViewBag.title = "Listados de información para el manager"
End Code
@section  header
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
End Section
@code
    Dim fixedUrl = "http://usotegieta2.batz.es:80/ibmcognos/cgi-bin/cognosisapi.dll?b_action=cognosViewer&ui.action=run&ui.object=%2fcontent%2ffolder%5b%40name%3d%27BATZ%27%5d%2ffolder%5b%40name%3d%27Zerbitzu%20Orokorrak%20-%20Servicios%20Generales%27%5d%2ffolder%5b%40name%3d%2704%20-%20Portal%20Manager%27%5d%2freport%5b%40name%3d%27"
    Dim urlVacaciones = fixedUrl + "VACACIONES%27%5d&ui.name=VACACIONES&run.outputFormat="
    Dim urlOpcionales = fixedUrl + "OPCIONALES%27%5d&ui.name=OPCIONALES&run.outputFormat="
    Dim urlIndices = fixedUrl + "EVOLUCION%20INDICES%27%5d&ui.name=EVOLUCION%20INDICES&run.outputFormat="
    Dim urlExtras = fixedUrl + "HORAS%20EXTRAS%27%5d&ui.name=HORAS%20EXTRAS&run.outputFormat="
    Dim urlAbsentismo = fixedUrl + "ABSENTISMO%27%5d&ui.name=ABSENTISMO&run.outputFormat="
    Dim urlContratos = fixedUrl + "CONTRATOS%27%5d&ui.name=CONTRATOS%27%5d&ui.name=CONTRATOS&run.outputFormat="
    Dim urlPlanificacion = fixedUrl + "PLANIFICACION%27%5d&ui.name=PLANIFICACION&run.outputFormat="
    Dim urlEvolucionPlantillaPGReal = "http://usotegieta2.batz.es:80/ibmcognos/cgi-bin/cognosisapi.dll?b_action=cognosViewer&ui.action=run&ui.object=%2fcontent%2ffolder%5b%40name%3d%27BATZ%27%5d%2ffolder%5b%40name%3d%27Zerbitzu%20Orokorrak%20-%20Servicios%20Generales%27%5d%2ffolder%5b%40name%3d%2702%20-%20GGBB%20-%20RRHH%27%5d%2freport%5b%40name%3d%27Evolucion%20Plantilla%20PG-Real%27%5d&ui.name=Evolucion%20Plantilla%20PG-Real&run.outputFormat="
End Code

@If SimpleRoleProvider.IsUserAuthorised(Role.rrhh) Then
    @h.traducir("Usuario con el que se ejecutaran los listados") @ViewData("usuario").nombre @Html.Encode(" ") @ViewData("usuario").apellido1 
    @<a href="@Url.Action("cambiarusuario")">@h.traducir("Cambiar usuario")</a>
@<br />
End If

<strong>@h.traducir("Ejercicio seleccionado") @ViewData("Ejercicioactual")</strong>
@If ViewData("Ejercicioactual") = Now.Year Then
@<a href="@Url.Action("index", New With {.ejercicio = ViewData("Ejercicioactual") - 1})">@h.traducir("Seleccionar ejercicio anterior")</a>
Else
@<a href="@Url.Action("index", New With {.ejercicio = ViewData("Ejercicioactual") + 1})">@h.traducir("Seleccionar ejercicio siguiente")</a>
End If
<ul class="cognosLinks">
    <li>
        <span >@h.traducir("Consumos y saldos de vacaciones")</span>
        <a href="@(urlVacaciones)HTML&p_Ano=@ViewData("ejercicioactual")&p_Trabajador=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Report.png")"/>
        </a>
        <a href="@(urlVacaciones)singleXLS&p_Ano=@ViewData("ejercicioactual")&p_Trabajador=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Excel.png")"/>
        </a>
        <a href="@(urlVacaciones)PDF&p_Ano=@ViewData("ejercicioactual")&p_Trabajador=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Pdf.png")"/>
        </a>
    </li>
    <li>
        <span>@h.traducir("Consumos y saldos de horas opcionales")</span>
        <a href="@(urlOpcionales)HTML&p_Ano=@ViewData("ejercicioactual")&p_Trab=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Report.png")"/>
        </a>
        <a href="@(urlOpcionales)singleXLS&p_Ano=@ViewData("ejercicioactual")&p_Trab=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Excel.png")"/>
        </a>
        <a href="@(urlOpcionales)PDF&p_Ano=@ViewData("ejercicioactual")&p_Trab=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Pdf.png")"/>
        </a>
    </li>
    <li>
        <span>@h.traducir("Evolucion de indices")</span>
        <a href="@(urlIndices)HTML&p_Ano=@ViewData("ejercicioactual")&p_Jefe=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Grafico.png")"/>
        </a>    
    </li>
    <li>
        <span>@h.traducir("Consumos y saldos de horas extras")</span>
        <a href="@(urlExtras)HTML&p_Ano=@ViewData("ejercicioactual")&p_Trab=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Report.png")"/>
        </a>
        <a href="@(urlExtras)singleXLS&p_Ano=@ViewData("ejercicioactual")&p_Trab=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Excel.png")"/>
        </a>
    </li>
    <li>
        <span>@h.traducir("Resumen absentismo")</span>
        <a href="@(urlAbsentismo)HTML&p_Ano=@ViewData("ejercicioactual")&p_Trab=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Report.png")"/>
        </a>
        <a href="@(urlAbsentismo)singleXLS&p_Ano=@ViewData("ejercicioactual")&p_Trab=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Excel.png")"/>
        </a>
    </li>
    <li>
        <span>@h.traducir("Finalizacion de contratos")</span>
        <a href="@(urlContratos)HTML&p_Ano=@ViewData("ejercicioactual")&p_responsable=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Report.png")"/>
        </a>
        <a href="@(urlContratos)singleXLS&p_Ano=@ViewData("ejercicioactual")&p_responsable=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Excel.png")"/>
        </a>
    </li>
    <li>
        <span>@h.traducir("Planificacion de ausencias")</span>
        <a href="@(urlPlanificacion)HTML&p_Anno=@ViewData("ejercicioactual")&p_Responsable=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Report.png")"/>
        </a>
        <a href="@(urlPlanificacion)singleXLS&p_Anno=@ViewData("ejercicioactual")&p_Responsable=@ViewData("usuario").codpersona&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Excel.png")"/>
        </a>
    </li>
    <li>
        <span>@h.traducir("Evolución plantilla")</span>
        <a href="@(urlEvolucionPlantillaPGReal)HTML&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Report.png")"/>
        </a>
        <a href="@(urlEvolucionPlantillaPGReal)singleXLS&run.prompt=false" target="_blank">
            <img   src="@Url.Content("~/Content/Excel.png")"/>
        </a>
    </li>
</ul>