@imports web
@For Each r In Model
    @<div class="panel-group" id="divGroup">
         <div class="panel panel-default">
             <div class="panel-heading panelcollapse" data-toggle="collapse" aria-expanded="false" data-target="#divCollapse_@r.Key.Name" data-parent="divGroup">
                 <h4 class="panel-title">
                     <a style="font-size:18px;"><strong>@r.Key.Name</strong></a>
                 </h4>
             </div>
             <div class="panel-collapse collapse in" id="divCollapse_@r.Key.Name">
                 <div class="panel-body">
                     @For Each c In r
                     @<div class="col-md-4" style="margin-top:5px;margin-bottom:5px;line-height:1.3;">
                         <div class="panel panel-default">
                             <div class="panel-heading">
                                     <a href="@Url.Action("transfer", h.ToRouteValues(Request.QueryString, New With {.url = c.url, .iduser = c.iduser, .idrec = c.id}))">
                                         <div class="row" style="display:flex;align-items:center">
                                             <div class="col-md-12" style="margin:auto">
                                                 <h4 style="text-align:center">@c.nombre</h4>
                                             </div>
                                         </div>
                                     </a>
                             </div>
                             <div class="panel-body">
                                     <a href="@Url.Action("transfer", h.ToRouteValues(Request.QueryString, New With {.url = c.url, .idrec = c.id}))">
                                         <div class="row" style="display:flex;align-items:center">
                                             <div class="col-md-3"><img src="@Url.Action("image", New With {.id = c.id})" alt="Imagen de recurso" /></div>
                                                 @code
                                                     Dim lineheightForBigdescription = ""
                                                     If (c.descripcion IsNot DBNull.Value AndAlso c.descripcion.length > 50) Then
                                                         lineheightForBigdescription = "style=""line-height: 1.1"""
                                                     End If
                                                 End Code
                                                 <div class="col-md-9" @Html.Raw(lineheightForBigdescription)>
                                                     <p style="margin:auto">@c.descripcion</p>
                                                 </div>
                                         </div>
                                     </a>                         
                              </div>
                         </div>
                     </div>
                                                     Next
                 </div>
             </div>

        </div>
    </div>
                                                     Next