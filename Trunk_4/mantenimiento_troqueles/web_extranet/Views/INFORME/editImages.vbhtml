@ModelType IEnumerable(Of FOTOS)


@section scripts
    <script type="text/javascript">
        $(document).on('change', ':file', function () {
            var input = $(this),
                numFiles = input.get(0).files ? input.get(0).files.length : 1,
                label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
            $("#file_feedback").html(label);
        });
    </script>
End Section

<h3>@h.traducir("Adjuntar imagenes")</h3>
<hr />
@Html.ValidationSummary(False, "", New With {.class = "alert alert-danger"})
<form action="" method="post" enctype="multipart/form-data">
    <div class="input-group">
        <div id="file_feedback" class="input-group-addon"></div>
        <label for="file" class="btn btn-default btn-file">
            @h.traducir("Elegir archivo")
            <input type="file" name="file" id="file" style="display: none;" />
        </label>
    </div>
    <div class="input-group">
        <input type="submit" class="btn btn-primary" value="@h.traducir("Subir")" />
    </div>
</form>

<h4>@h.traducir("Listado de imagenes adjuntadas a informe")</h4>
<div Class="row">

    @For Each f In Model

    @<div Class="col-xs-3">
    <div  Class="thumbnail">
      <img src = "@Url.Action("photo", h.ToRouteValues(Request.QueryString, New With {.idphoto = f.IDFOTO})))" alt="@h.traducir("Imagen adjuntada por usuario")">
        <div class="caption">
            <p><a href="@Url.Action("photo", h.ToRouteValues(Request.QueryString, New With {.idphoto = f.IDFOTO}))" class="btn btn-primary" role="button">@h.traducir("Ver")</a>
            <a href="@Url.Action("deleteimage", h.ToRouteValues(Request.QueryString, New With {.idphoto = f.IDFOTO}))" class="btn btn-default" role="button">@h.traducir("Eliminar")</a></p>
        </div>
        </div>
  </div>

    Next
</div>
<a href="@Url.Action("index")">@h.traducir("Volver al listado de informes")</a>