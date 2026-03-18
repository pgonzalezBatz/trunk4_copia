@imports web
@Code
    ViewBag.title = "Crear - Editar solicitud"
End Code

@section  header
    <link href="//intranet2.batz.es/baliabideorokorrak/ui.datepicker.css" rel="Stylesheet" type="text/css" />
    <script src="//intranet2.batz.es/baliabideorokorrak/jquery-1.5.1.min.js" type="text/javascript"></script>
End Section
    

<ol>
    <li>
        <h3>
            @h.traducir("Definición del puesto de trabajo")
        </h3>
            <strong>@h.traducir("Negocio")</strong>
            <br />
            @Model.nombrenegocio  
        <br />
            <strong>@h.traducir("Departamento")</strong>
            <br />
            @Model.nombredepartamento
        <br />
            <strong>@h.traducir("Nº de personas")</strong>
            <br />
            @Model.npersonas
        <br />
            <strong>@h.traducir("Descripción de las prácticas o proyecto")</strong>
            <br />
            @Model.descripcion
        <br />    
    </li>
    <li>
        <h3>
            @h.traducir("Requisitos del becario")
        </h3>
        <strong>@h.traducir("Universidad")</strong>
            <br />
            @Model.universidad
        <br />
        <strong>@h.traducir("Titulación")</strong>
        <br />
        @Model.titulacion
        <br />
            <strong>@h.traducir("Especialidad")</strong>
            <br />
            @Model.especialidad
        <br />
            <strong>@h.traducir("Conocimientos específicos")</strong>
            <br />
            @Model.conocimientos
        <br />
            <strong>@h.traducir("Idiomas")</strong>
            <br />
            @Model.idiomas
        <br />
            <strong>@h.traducir("Experiencia")</strong>
            <br />
            @Model.experiencia
    </li>
    <li>
        <h3>
            @h.traducir("Condiciones del convenio")
        </h3>
            <strong>@h.traducir("Fecha prevista incorporación")</strong>
            <br />
        @Model.fecha.toshortdatestring
        <br />
            <strong>@h.traducir("Duración del convenio")</strong>
            <br />
        @Model.duracion        
        <br />
        <strong>@h.traducir("Horario")</strong>
            <br />
        @Model.horario
    </li>
</ol>