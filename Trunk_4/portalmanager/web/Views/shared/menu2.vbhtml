@imports web
    
<ul class="nav">
    <li class="nav-item">
        @If Regex.IsMatch(Request.RawUrl, "personal$|busquedacompletaaltas$", RegexOptions.IgnoreCase) Then
            @<a class="nav-link active" href="@Url.Action("busquedacompletaaltas", "personal")" >@h.traducir("Busqueda Detallada altas")</a>
        Else
            @<a  class="nav-link" href="@Url.Action("busquedacompletaaltas", "personal")">@h.traducir("Busqueda Detallada altas")</a>
        End If
    </li>
    <li class="nav-item">
        @If Regex.IsMatch(Request.RawUrl, "busquedacompleta", RegexOptions.IgnoreCase) Then
            @<a class="nav-link active" href="@Url.Action("busquedacompleta", "personal")">@h.traducir("Busqueda Detallada")</a>
        Else
            @<a class="nav-link" href="@Url.Action("busquedacompleta", "personal")">@h.traducir("Busqueda Detallada")</a>
        End If
    </li>
    <li class="nav-item">
        @If SimpleRoleProvider.IsUserAuthorised(Role.rrhh, Role.eki + Role.departamento) Then
            @If Regex.IsMatch(Request.RawUrl, "listadoDepartamentos", RegexOptions.IgnoreCase) Then
                @<a class="nav-link active" href="@Url.Action("listadoDepartamentos", "personal")" >@h.traducir("Tabla Departamental")</a>
            Else
                @<a class="nav-link" href="@Url.Action("listadoDepartamentos", "personal")">@h.traducir("Tabla Departamental")</a>
            End If
        End If
    </li>
    <li class="nav-item">
        @If SimpleRoleProvider.IsUserAuthorised(Role.rrhh, Role.eki + Role.excedencia) Then
            @If Regex.IsMatch(Request.RawUrl, "listadoexcedencias", RegexOptions.IgnoreCase) Then
                @<a class="nav-link active" href="@Url.Action("listadoexcedencias", "personal")" >@h.traducir("Excedencias")</a>
            Else
                @<a class="nav-link" href="@Url.Action("listadoexcedencias", "personal")">@h.traducir("Excedencias")</a>
            End If
        end if
    </li>
    <li class="nav-item">
        @If SimpleRoleProvider.IsUserAuthorised(Role.rrhh, Role.eki + Role.altasBajas) Then
            @If Regex.IsMatch(Request.RawUrl, "AltasBajasYCambios", RegexOptions.IgnoreCase) Then
                @<a class="nav-link active" href="@Url.Action("AltasBajasYCambios", "personal")" >@h.traducir("Altas Bajas y cambios")</a>
            Else
                @<a class="nav-link" href="@Url.Action("AltasBajasYCambios", "personal")">@h.traducir("Altas Bajas y cambios")</a>
            End If
        end if
    </li>
    @If SimpleRoleProvider.IsUserAuthorised(Role.rrhh) Then
        @<li  class="nav-item" >
    @If Regex.IsMatch(Request.RawUrl, "arbolDepartamentos", RegexOptions.IgnoreCase) Then
        @<a class="nav-link active"  href="@Url.Action("arbolDepartamentos", "personal")" >@h.traducir("Arbol Departamental")</a>
    Else
        @<a class="nav-link"  href="@Url.Action("arbolDepartamentos", "personal")">@h.traducir("Arbol Departamental")</a>
    End If
</li>
    End If

</ul>






