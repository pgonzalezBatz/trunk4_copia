@Code
    ViewData("Title") = "index"
End Code
@section header
<link href="//intranet2.batz.es/baliabideorokorrak/bootstrap/css/typeahead.css" rel="stylesheet" />
End Section
@section scripts
<script src="//intranet2.batz.es/baliabideorokorrak/bootstrap/js/typeahead.bundle.min.js"></script>
<script type="text/javascript">
   
</script>  
End Section
<div class="hidden-xs">
    <h3>@h.traducir("Búsqueda de teléfonos y extensiones por departamentos")</h3>
    <hr />
</div>

<form>
    
    <div class="form-group">
        <label><strong>@h.traducir("Buscar planta")</strong>/strong></label>
        @Html.TextBox("q")
    </div>
    <div class="form-group">
            @<input type="submit" value="@h.traducir("Buscar")" Class="btn btn-primary" />
    </div>

</form>

@If Model IsNot Nothing Then
    @<table class="table">
        <thead>
            <tr>
                <th></th>
            </tr>
        </thead>
    <tbody>
        @for each p In Model
            @<tr>
                <td>
                    <a href="@Url.Action("departamento", h.ToRouteValues(Request.QueryString, New With {.idplanta = p.value, .nombrePlanta = p.text}))">@p.text</a>
                </td>
            </tr>
        Next
    </tbody>
            </table>
End If
