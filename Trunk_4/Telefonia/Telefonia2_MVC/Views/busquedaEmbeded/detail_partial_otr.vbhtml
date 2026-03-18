@If Model Is Nothing Then
    @<div class="alert alert-warning">
         @h.Traducir("No se han encontrado datos")
    </div>

Else
@<div Class="row">
     <div Class="col-sm-12">
         <h3 Class="center"><b>NOMBRE: </b>@Model(0).nombre</h3>
         <br />


             <table Class="table table-hover">
                 <thead>
                     <tr>
                         <th>@h.Traducir("Extensión")</th>
                         <th>@h.Traducir("Número")</th>
                         <th>@h.Traducir("Planta")</th>
                     </tr>
                 </thead>
                 <tbody>
                     @For Each otr In Model
                         @<tr>
                             <td>
                                 <a href="tel: @otr.extensionOtro" class="hidden-md hidden-lg">@otr.extensionOtro</a>
                                 <span class="hidden-xs hidden-sm">@otr.extensionOtro</span>
                             </td>
                             <td>
                                 <a href="tel: @otr.numero" class="hidden-md hidden-lg">@otr.numero</a>
                                 <span class="hidden-xs hidden-sm">@otr.numero</span>
                             </td>
                             <td>
                                 @otr.planta
                             </td>                           
                         </tr>
                     Next
                 </tbody>
             </table>


     </div>
 </div> 
End If
