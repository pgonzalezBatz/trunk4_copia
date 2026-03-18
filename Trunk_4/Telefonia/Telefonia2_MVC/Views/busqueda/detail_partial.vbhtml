@Modeltype persona
@If Model Is Nothing Then
    @<div class="alert alert-warning">
         @h.traducir("No se han encontrado datos para el usuari@ seleccionad@")
    </div>

Else
    @<h3 Class="hidden-xs hidden-sm">@Model.nombre @Model.apellido1 @Model.apellido2</h3>
@<h4 Class="hidden-md hidden-lg">@Model.nombre @Model.apellido1 @Model.apellido2</h4>

@<h4 Class="hidden-xs hidden-sm">
     <a href="@Url.Action("index", New With {.idPlanta = Model.idPlanta, .idDepartamento = Model.idDepartamento, .nombreDepartamento = Model.departamento})">
         @Model.departamento
     </a>
    </h4>
@<h5 Class="hidden-md hidden-lg">
     <a href="@Url.Action("index", New With {.idPlanta = Model.idPlanta, .idDepartamento = Model.idDepartamento, .nombreDepartamento = Model.departamento})">
         @Model.departamento
     </a>
    </h5>
@<div Class="row">
    <div Class="col-sm-3">
        <img src = "@Url.Action("photo", New With {.id = Model.id})" height="142" alt="@h.traducir("Fotografia de la persona")" />

    </div>
    <div Class="col-sm-3 ">
                @If Model.nikEuskaraz Then
                @<img src="@Url.Content("~/Content/NikEuskaraz.png")" />
                End If
    </div>
     <div Class="col-sm-3 hidden-xs">
         <address>
             <strong>@ViewData("planta").nombre</strong><br />
             @ViewData("planta").direccion<br />
             @ViewData("planta").cp @ViewData("planta").ciudad  @ViewData("planta").provincia<br />
             @ViewData("planta").pais
         </address>
     </div>
     </div>
                @If Model.lstExtension.Count > 0 Then
                    Dim hasDirectoInterno = CType(Model.lstExtension, IEnumerable(Of Object)).Any(Function(e) Not String.IsNullOrEmpty(e.internoDirecto))
                @<table Class="table table-hover">
                    <thead>
                        <tr>
                            <th>@h.traducir("Ext. Fija")</th>
                            <th>@h.traducir("Ext. Movil")</th>
                            <th>@h.traducir("Nº Movil")</th>
                            @if hasDirectoInterno Then
                                @<th>@h.traducir("Nº directo")</th>
                            End If
                                                </tr>
                    </thead>
                    <tbody>
                        @For Each ext In Model.lstExtension
                        @<tr>
                            <td>
                                <a href="tel: @ext.extensionInterna" class="hidden-md hidden-lg">@ext.extensionInterna</a>
                                <span class="hidden-xs hidden-sm">@ext.extensionInterna</span>
                            </td>
                            <td>
                                <a href="tel: @ext.extensionExterna" class="hidden-md hidden-lg">@ext.extensionExterna</a>
                                <span class="hidden-xs hidden-sm">@ext.extensionExterna</span>
                            </td>
                            <td>
                                <a href="tel: @ext.numero" class="hidden-md hidden-lg">
                                @If ext.numero IsNot Nothing AndAlso ext.numero.Length > 4 Then
                                    @ext.numero
                                End If
                                </a>
                              

                                <span Class="hidden-xs hidden-sm">
                                @If ext.numero IsNot Nothing AndAlso ext.numero.Length > 4 Then
                                    @ext.numero
                                End If</span>
                            </td>
                             @If hasDirectoInterno Then
                                 @<td>
                                      <a href="tel: @ext.internoDirecto" class="hidden-md hidden-lg">
                                      @If ext.internoDirecto IsNot Nothing AndAlso ext.internoDirecto.Length > 4 Then
                                        @ext.internoDirecto
                                      End If
                                      </a>
                                      <span class="hidden-xs hidden-sm">
@If ext.internoDirecto IsNot Nothing AndAlso ext.internoDirecto.Length > 4 Then
                        @ext.internoDirecto
End If
                                      </span>

                       
                        </td>
                             End If
                        </tr>
                        Next
                    </tbody>
                </table>                        End If
End If
