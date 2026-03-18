@If Model Is Nothing Then
    @<div class="alert alert-warning">
         @h.traducir("No se han encontrado datos para el departamento seleccionado")
    </div>

Else
@<div Class="row">
     <div Class="col-sm-9">
         <h3 Class="hidden-xs hidden-sm">@ViewData("nombreDepartamento")</h3>
         <strong Class="hidden-md hidden-lg">@ViewData("nombreDepartamento")</strong>
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
                @If Model.Count > 0 Then
                    Dim hasDirectoInterno = CType(Model, IEnumerable(Of Object)).Any(Function(e) Not String.IsNullOrEmpty(e.internoDirecto))
                @<table Class="table table-hover">
                    <thead>
                        <tr>
                            <th>@h.traducir("Nombre")</th>
                            <th>@h.traducir("Ext. Fija")</th>
                            <th>@h.traducir("Ext. Movil")</th>
                            <th>@h.traducir("Nº Movil")</th>
                            @if hasDirectoInterno Then
                                @<th>@h.traducir("Nº directo")</th>
                            End If
                            <th>Nik Euskaraz</th>
                                                </tr>
                    </thead>
                    <tbody>
                        @For Each ext In Model
                        @<tr>
                        <td>@ext.nombre @ext.apellido1 @ext.apellido2</td>
                            <td>    
                                <a href="tel: @ext.extensionInterna" class="hidden-md hidden-lg">@ext.extensionInterna</a>
                                <span class="hidden-xs hidden-sm">@ext.extensionInterna</span>
                            </td>
                            <td>
                                <a href="tel: @ext.extensionExterna" class="hidden-md hidden-lg">@ext.extensionExterna</a>
                                <span class="hidden-xs hidden-sm">@ext.extensionExterna</span>
                            </td>
                            <td>
                                <a href="tel: @ext.numero" class="hidden-md hidden-lg">@ext.numero</a>
                                <span class="hidden-xs hidden-sm">@ext.numero</span>
                            </td>
                             @if hasDirectoInterno Then
                                 @<td>
                                      <a href="tel: @ext.internoDirecto" class="hidden-md hidden-lg">@ext.internoDirecto</a>
                                      <span class="hidden-xs hidden-sm">@ext.internoDirecto</span>
                       
                        </td>
                             End If     
                        <td align="center">
                            @If ext.nikEuskaraz Then
                                @<img src="@Url.Content("~/Content/NikEuskaraz.png")" style="height:1.5em;"  />
                            End If  
                        </td>
                        </tr>
                        Next
                    </tbody>
                </table>                        End If
End If
