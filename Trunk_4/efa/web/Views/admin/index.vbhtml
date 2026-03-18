@imports web

@section header
    <title>@h.traducir("Recursos")</title>
End section


<h3>
    <a href="@Url.Action("grupo")">
        @h.Traducir("Administrar grupos")
    </a>
    <br />
    <a href="@Url.Action("alarmas")">
        @h.Traducir("Administrar Alarmas")
    </a>
    <br />
    <a href="@Url.Action("listtonerimpresora")">
        @h.traducir("Administrar Toners")
    </a>
    <br />
    <a href="@Url.Action("listcomponente")">
        @h.traducir("Administrar Componentes")
    </a>
    <br />
    <a href="@Url.Action("seleccionartrabajador")">
        @h.Traducir("Asignación manual de recursos")
    </a>
    <br />
    <a href="@Url.Action("listadorecursosnormales")">
        @h.Traducir("Listado de recursos Fisikos que estan cogidos")
    </a>
    <br />
    <a href="@Url.Action("listadotelefonos")">
        @h.Traducir("Listado de telefonos que estan cogidos")
    </a>
    <br />
    <a href="@Url.Action("ultimaspersonasencogerrecurso0")">
        @h.Traducir("Listado últimos usuarios que han cogido el recurso")
    </a>
</h3>